using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using static Site1.WebForms.CROUDFAUD;

namespace Site1
{

    /// <summary>
    /// Main page Profile
    /// </summary>
    public partial class WebForm1: System.Web.UI.Page
    {
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private static string username = "";

        /// <summary>
        /// On load page take data from BD use Session["username"] for html fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Display the name and email on the profile page
            string email = "";
            string type = "";
            People people = new People();
            People.Take(Session["username"], people);
            username = people.Username;
            email = people.Email;  
            type = people.Type;
            Session["type"] = type;
            lblEmail.InnerText = "Name: " + username;
            lblName.InnerText = "Email: " + email;  
            lblType.InnerText = "Type of user: " + type;
            DisplayImage();

            ///Check progress current user. Have possible to create new crowd or his crows already done
            ///And write message in profile page
            try
            {
                if (Session["type"].ToString() == "admin")
                {
                    return;
                }
                else
                {
                    int progress = 0;
                    int cost = 0;
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SELECT Progress, Cost FROM Table_CROWD WHERE Username=@user", conn);
                        cmd.Parameters.AddWithValue("@user", Session["username"]);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            progress = Convert.ToInt32(reader["Progress"]);
                            cost = Convert.ToInt32(reader["Cost"]);
                        }
                        reader.Close();
                    }
                    if (cost == progress)
                    {
                        Creat_New.Visible = true;
                        New.Visible = false;
                    }
                    else
                    {
                        Creat_New.Visible = false;
                    }
                }
            }
            catch 
            {
                New.Visible = true;
            };

        }

        /// <summary>
        /// Redirect to Write_City_Project Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ADD_CITY(object sender, EventArgs e)
        {
            Response.Redirect("/WebForms/AddProject.aspx");
        }

        /// <summary>
        /// Redirect to Write_Crowd Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ADD_CROWD(object sender, EventArgs e)
        {
            Response.Redirect("/WebForms/WRITE_CROWD.aspx");
        }

        /// <summary>
        /// Display image if user have in BD
        /// </summary>
        protected void DisplayImage()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Image FROM Users WHERE Username = @Username", connection);
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("Image")))
                    {
                        byte[] imageData = (byte[])reader["Image"];
                        string imageBase64 = Convert.ToBase64String(imageData);
                        preview.Src = "data:image/png;base64," + imageBase64;
                        preview.Style["display"] = "block";
                    }
                    else
                    {
                        preview.Style["display"] = "none";
                    }
                }
                else
                {
                    preview.Style["display"] = "none";
                }
            }
        }

        /// <summary>
        /// Upload Image which user want to add to profile and After display on page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            HttpPostedFile file = ng_file.PostedFile;
            byte[] imageData = null;
            if (file != null && file.ContentLength > 0)
            {
                using (BinaryReader reader = new BinaryReader(file.InputStream))
                {
                    imageData = reader.ReadBytes(file.ContentLength);
                }
            }
            if (imageData != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string username = Session["username"].ToString();
                    SqlCommand command = new SqlCommand("UPDATE Users SET Image = @Image WHERE Username = @Username", connection);
                    command.Parameters.AddWithValue("@Image", imageData);
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            DisplayImage();
        }
    }
}

