/*
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
    public partial class GASD : System.Web.UI.Page
    {
        static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static int cost_;
        static int newProgress_;
        static string user_;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                rptProjects = (Repeater)Page.FindControl("rptProjects");
                BindProjects();
                RepeaterItem items = (RepeaterItem)Session["item"];
                if (cost_ == 0)
                {
                    return;
                }
                else
                {
                    // Fetch data from the database
                    List<CROWD> projects = CROWD.FetchProjectsFromDatabase();

                    // Bind data to the repeater control
                    rptProjects.DataSource = projects;
                    rptProjects.DataBind();

                    int userId = 0;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("SELECT ID FROM Table_CROWD WHERE Username = @Username", connection);
                        command.Parameters.AddWithValue("@Username", user_);
                        userId = (int)command.ExecuteScalar();
                    }

                    int i = 0;
                    foreach (RepeaterItem item in rptProjects.Items)
                    {

                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {

                            CROWD project = projects[i];
                            int projectId = project.ID;
                            // If the current item matches the user's project ID, update the progress bar
                            if (projectId == userId)
                            {
                                double progress = (double)newProgress_ / cost_ * 100;
                                UpdatePanel myUpdatePanel = item.FindControl("myUpdatePanel") as UpdatePanel;
                                Panel progressBar = (Panel)myUpdatePanel.FindControl("progress");
                                progressBar.Style["width"] = progress.ToString() + "%";
                                break;

                            }
                            i++;

                        }

                    }
                    i = 0;

                }

            }
            else
            {
                rptProjects = (Repeater)Session["rptProjects"];
            }
        }

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

        [WebMethod]
        public static void Make_Donat(string amount, string cardId, string username, int cost)
        {

            // Check for null or empty values
            if (string.IsNullOrEmpty(amount) || string.IsNullOrEmpty(cardId) || string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Invalid input parameters.");
            }
            user_ = username;

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
        public class ProgressInfo
        {
            public int MaxValue { get; set; }
            public int CurrentProgress { get; set; }
        }

        [WebMethod]
        public static ProgressInfo GetProgress(int projectId)
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
            CROWD project_2 = projects[projectId - 1];
            int max = Convert.ToInt32(project_2.Cost);
            ProgressInfo progressInfo = new ProgressInfo();
            progressInfo.CurrentProgress = progress;
            progressInfo.MaxValue = max;
            return progressInfo;
        }
    }
}

*/