using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site1.WebForms
{
    public partial class WRITE_CROWD : System.Web.UI.Page
    {
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static string username = "";
        private static bool check_uploud = false;
        public string dbText { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            username = Session["username"].ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Table_CROWD WHERE Username = @Username", connection);
                checkCommand.Parameters.AddWithValue("@Username", username);
                connection.Open();
                int count = (int)checkCommand.ExecuteScalar();

                if (count == 0) // If username does not exist in the table
                {
                    SqlCommand insertCommand = new SqlCommand("INSERT INTO Table_CROWD (Username) VALUES (@Username)", connection);
                    insertCommand.Parameters.AddWithValue("@Username", username);
                    insertCommand.ExecuteNonQuery();
                }
            }
           
            DisplayImage();

        }

  

        protected void Submit_Click(object sender, EventArgs e)
        {
            // Check if "Costs Needed" input is an integer            
            int costsNeeded;
            if (int.TryParse(Cost_Needed.Value, out costsNeeded))
            {
                if (check_uploud)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand("UPDATE Table_CROWD SET Text = @Text, Cost = @Cost, Progress = @Progress WHERE Username = @Username", connection);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Text", Request.Form["crowdfunding_text"]);
                        command.Parameters.AddWithValue("@Cost", costsNeeded);
                        command.Parameters.AddWithValue("@Progress", 0);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }                  
                    Response.Redirect("/WebForms/Main.aspx");
                }
                else
                {
                    ErrorLabel.Text = "Please add image before continue";
                }
            }
            else
            {
                // Invalid input, display error message
                ErrorLabel.Text = "Please enter a valid integer for Costs Needed.";
            }
            
        }

        protected void Change_Old(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Retrieve values from database
                SqlCommand command = new SqlCommand("SELECT Username, Text, Cost, Image FROM Table_CROWD WHERE Username = @Username", connection);
                command.Parameters.AddWithValue("@Username", username);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Check if the retrieved values are not null
                        string retrievedUsername = reader["Username"] as string;
                        string retrievedText = reader["Text"] as string;
                        int? retrievedCost = reader["Cost"] as int?;
                        byte[] retrievedImage = reader["Image"] as byte[];

                        if (!string.IsNullOrEmpty(retrievedUsername) && !string.IsNullOrEmpty(retrievedText) && retrievedCost.HasValue && retrievedImage != null)
                        {
                            Cost_Needed.Value = retrievedCost.ToString();
                            dbText = retrievedText; // Assign retrievedText value to dbText property
                        }
                    }
                }
                reader.Close();
                
            }
           

        }

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
                    SqlCommand command = new SqlCommand("UPDATE Table_CROWD SET Image = @Image WHERE Username = @Username", connection);
                    command.Parameters.AddWithValue("@Image", imageData);
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                check_uploud = true;
            }
            DisplayImage();
        }


        protected void DisplayImage()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Image FROM Table_CROWD WHERE Username = @Username", connection);
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
                        check_uploud = true;
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