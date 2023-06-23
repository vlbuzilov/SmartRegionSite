using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site1
{
    /// <summary>
    /// Login page
    /// </summary>
    public partial class Login : System.Web.UI.Page
    {
        /// <summary>
        /// Method which check HasRow in BDTable user and password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            /// Html fields
            string username = Username.Value;
            string password = Password.Value;

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE Username=@username AND Password=@password", connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {                   
                    Session["username"] = username;
                    Response.Redirect("/WebForms/Main.aspx");
                }
                else
                {
                    ErrorLabel.Text = "Invalid username or password";
                }

                reader.Close();
            }
        }
    }
}