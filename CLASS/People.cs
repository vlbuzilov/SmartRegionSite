using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Site1
{
    /// <summary>
    /// Class helper which take current user data
    /// </summary>
    public class People
    {
        private string username = "";
        private string password = "";
        private string email = "";
        private string type = "";

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Helper method take date from db
        /// </summary>
        /// <param name="e"></param>
        /// <param name="people"></param>
        public static void Take(object e, People people)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Username = @username", conn);
            cmd.Parameters.AddWithValue("@username", e);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            people.Username = reader["Username"].ToString();
            people.Email = reader["Email"].ToString();
            people.Type = reader["Permission"].ToString();
            conn.Close();
        }

      

    }
}