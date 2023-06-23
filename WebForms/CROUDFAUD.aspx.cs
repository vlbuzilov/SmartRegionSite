using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Site1;

namespace Site1.WebForms
{
    public partial class CROUDFAUD : System.Web.UI.Page
    {
        static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static int newProgress_;
        private static int cost_;

        /// <summary>
        /// On load page update progress in bar and take new data from db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                rptProjects = (Repeater)Page.FindControl("rptProjects");
                BindProjects();

                ClientScriptManager cs = Page.ClientScript;
                string scriptPath = "/WebForms/scripts/Data.js";
                cs.RegisterClientScriptInclude(this.GetType(), "DataScript", scriptPath);
            }
        }

        /// <summary>
        /// Take data from db use helper class
        /// </summary>
        private void BindProjects()
        {

            // Fetch data from the database
            List<CROWD> projects = CROWD.FetchProjectsFromDatabase();

            // Bind data to the repeater control
            rptProjects.DataSource = projects;
            rptProjects.DataBind();

            // Store a reference to the repeater control in the session
            Session["rptProjects"] = rptProjects;

        }


        /// <summary>
        /// Web_Method which take data from html page form use JS
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="cardId"></param>
        /// <param name="username"></param>
        /// <param name="cost"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        [WebMethod]
        public static void Make_Donat(string amount, string cardId, string username, int cost)
        {

            // Check for null or empty values
            if (string.IsNullOrEmpty(amount) ||  string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Invalid input parameters.");
            }
           

            // Fetch the current progress for the user
            int currentProgress = GetCurrentProgress(username);

            // Calculate the new progress
            cost_ = cost;
            newProgress_ = currentProgress + Convert.ToInt32(amount);
            if (newProgress_ >= cost)
            {
                newProgress_ = cost;
            }

            // Update the progress in the database
            UpdateProgress(username, newProgress_);

            // Get the current item from the repeater control
            CROUDFAUD page = HttpContext.Current.Handler as CROUDFAUD;
            if (page == null)
            {
                throw new Exception("Could not get the current page.");
            }

            RepeaterItem item = page.GetRepeaterItemByProjectId(username);

           
        }

        /// <summary>
        /// Take current item id repeater
        /// </summary>
        /// <param name="projectUsername"></param>
        /// <returns></returns>
        private RepeaterItem GetRepeaterItemByProjectId(string projectUsername)
        {
            // Find the Repeater control on the page
            Repeater rptProjects = (Repeater)Session["rptProjects"];

            // Loop through the data items in the Repeater
            foreach (RepeaterItem item in rptProjects.Items)
            {
                // Find the project username label within the item
                Label projectUsernameLabel = (Label)item.FindControl("Username");

                // If the project username label matches the given username, return the item
                if (projectUsernameLabel != null && projectUsernameLabel.Text == projectUsername)
                {
                    Session["item"] = item;
                    return item;
                }
            }

            // If no matching item is found, return null
            return null;
        }

        /// <summary>
        /// Take current progress bar from db
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private static int GetCurrentProgress(string username)
        {
            int currentProgress = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Progress FROM Table_CROWD WHERE Username=@Username", connection);
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    currentProgress = Convert.ToInt32(result);
                }
            }
            return currentProgress;
        }

        /// <summary>
        /// Update progress bar
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newProgress"></param>
        private static void UpdateProgress(string username, int newProgress)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE Table_CROWD SET Progress=@NewProgress WHERE Username=@Username", connection);
                command.Parameters.AddWithValue("@NewProgress", newProgress);
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Method which help JS update css style bar in html
        /// </summary>
        public class ProgressInfo
        {
            public int MaxValue { get; set; }
            public int CurrentProgress { get; set; }
        }

        [WebMethod]
        public static ProgressInfo GetProgress(int projectId)
        {
            try
            {
                // Fetch data from the database
                List<CROWD> projects = CROWD.FetchProjectsFromDatabase();
                int progress = 0;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Progress FROM Table_CROWD WHERE ID=@projectId", conn);
                    cmd.Parameters.AddWithValue("@projectId", projectId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        progress = Convert.ToInt32(reader["Progress"]);
                    }
                    reader.Close();
                }
                CROWD project_2 = projects.FirstOrDefault(p => p.ID == projectId);
                int max = Convert.ToInt32(project_2.Cost);
                ProgressInfo progressInfo = new ProgressInfo();
                progressInfo.CurrentProgress = progress;
                progressInfo.MaxValue = max;
                return progressInfo;
            }
            catch(Exception ex) { return null; }
        }


        /// <summary>
        /// Delete selected CROWD
        /// </summary>
        /// <param name="username"></param>
        [WebMethod]
        public static void Delete(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "UPDATE Table_CROWD SET Text = NULL, Cost = NULL, Image = NULL, Progress = NULL WHERE Username = @Username";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception ex) { }
        }


    }
}

        