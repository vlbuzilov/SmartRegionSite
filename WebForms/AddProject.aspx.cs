using Site1.CLASS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;

namespace Site1.WebForms
{
    /// <summary>
    /// Add project Page
    /// </summary>
    public partial class AddProject : System.Web.UI.Page
    {
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static string Username = "";
        private static int UserID = 0;
        private static int ProjectID = 0;
        private static bool check_upload = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            Username = Session["Username"].ToString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand insertCommand = new SqlCommand("INSERT INTO MunicipalProjects (Username) VALUES (@Username); SELECT SCOPE_IDENTITY()", sqlConnection);
                insertCommand.Parameters.AddWithValue("@username", Username);
                ProjectID = Convert.ToInt32(insertCommand.ExecuteScalar());
            }
        }

        /// <summary>
        /// Add data to BD and validate fields on page 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButtonClick(object sender, EventArgs e)
        {
            int cost, sufficientVotes;

            if (int.TryParse(Request.Form["NecessaryFunds"], out cost))
            {
                if (int.TryParse(Request.Form["SufficientVotes"], out sufficientVotes) && Request.Form["SufficientVotes"] != null)
                {
                    if (check_upload)
                    {
                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            sqlConnection.Open();
                            SqlCommand command = new SqlCommand("UPDATE TOP (1) MunicipalProjects SET NameOfProject = @NameOfProject, Cost = @Cost, Description = @Description, Latitude = @Latitude, Longitude = @Longitude, VoteCount = @VoteCount, SufficientVoteCount = @SufficientVotes WHERE Username = @Username AND NameOfProject IS NULL", sqlConnection);
                            command.Parameters.AddWithValue("@NameOfProject", Request.Form["nameOfProject"]);
                            command.Parameters.AddWithValue("@Cost", cost);
                            command.Parameters.AddWithValue("@Description", Request.Form["projectDescription"]);
                            command.Parameters.AddWithValue("@Username", Username);
                            command.Parameters.AddWithValue("@Latitude", Request.Form["latitude"]);
                            command.Parameters.AddWithValue("@Longitude", Request.Form["longitude"]);
                            command.Parameters.AddWithValue("@VoteCount", 0);
                            command.Parameters.AddWithValue("@SufficientVotes", sufficientVotes);
                            command.Connection = sqlConnection;
                            command.ExecuteNonQuery();

                            command.CommandText = "DELETE FROM MunicipalProjects WHERE NameOfProject IS NULL";
                            command.ExecuteNonQuery();
                        }

                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            sqlConnection.Open();

                            SqlCommand command = new SqlCommand("SELECT ProjectID FROM MunicipalProjects WHERE Username = @Username AND NameOfProject = @NameOfProject", sqlConnection);
                            command.Parameters.AddWithValue("@Username", Username);
                            command.Parameters.AddWithValue("@NameOfProject", Request.Form["nameOfProject"]);

                            ProjectID = Convert.ToInt32(command.ExecuteScalar());
                        }

                        Response.Redirect("/WebForms/Main.aspx");
                    }
                    else
                    {
                        ErrorLabel.Text = "Please select an image for your project.";
                    }
                }
                else
                {
                    ErrorLabel.Text = "Please enter a valid integer for sufficient number of votes.";
                }

            }
            else
            {
                ErrorLabel.Text = "Please enter a valid integer for Costs Needed.";
            }
        }

        /// <summary>
        /// Upload image to BD and display on page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UploadButtonClick(object sender, EventArgs e)
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
                if (ProjectID > 0)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand("UPDATE MunicipalProjects SET Image = @Image WHERE (Username = @Username AND Image IS NULL) OR (Username = @Username AND ProjectID = @ProjectID)", connection);
                        command.Parameters.AddWithValue("@Image", imageData);
                        command.Parameters.AddWithValue("@Username", Username);
                        command.Parameters.AddWithValue("@ProjectID", ProjectID);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    check_upload = true;
                    DisplayImage();
                }
            }

        }


        /// <summary>
        /// Display image from bd 
        /// </summary>
        protected void DisplayImage()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Image FROM MunicipalProjects WHERE Username = @Username AND ProjectID = @ProjectID", connection);
                command.Parameters.AddWithValue("@Username", Username);
                command.Parameters.AddWithValue("@ProjectID", ProjectID);
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
                        check_upload = true;
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
    }
}