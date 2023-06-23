using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Site1
{

/// <summary>
/// Register page
/// </summary>
public partial class Register : System.Web.UI.Page
{
        /// <summary>
        /// Method which validate input data and add to BD (Users)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            ///Fields from html page
            string secret = "admin";
            string secret_input = secret_key_input.Value.ToString();
            string username = Username.Value;
            string email = Email.Value;            
            string password = Password.Value;
            string confirm_password = ConfirmPassword.Value;
            string type = user_type.Value;
            

            ///validate email
            string pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            if (!Regex.IsMatch(email, pattern))
            {
                // email is not valid, display error message
                ErrorLabel.Text = "Invalid email address";
                return;
            }

            ///validate password
            if (password != confirm_password)
            {
               ErrorLabel.Text = "Passwords do not match";
               return;
            }

            ///validate type secret key if admin
            if (type == "admin")
            {
                if (secret_input != secret)
                {
                    ErrorLabel.Text = "Error you are not admin try Again!!";
                    return;
                }
            }

            ///validate if empty fields
            if (username == "" || email == "" || password == "") { ErrorLabel.Text = "Not empty line!!"; return; }

            
            /// conection string to bd
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            ///Add fields to bd
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username=@username", connection);
                    command.Parameters.AddWithValue("@username", username);

                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        ErrorLabel.Text = "Username already taken.";
                        return;
                    }

                    command = new SqlCommand("INSERT INTO Users (Username, Email, Password, Permission) VALUES (@username, @email, @password, @permission)", connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@permission", type);

                int result = command.ExecuteNonQuery();

                    if (result == 1)
                    {
                       Response.Redirect("/WebForms/Login.aspx");
                    }
                    else
                    {
                    ErrorLabel.Text = "Error creating account.";
                    }
                }
            }
        }
  }