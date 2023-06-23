using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Site1
{
    /// <summary>
    /// Helper Method take data from DB
    /// </summary>
    public class CROWD
    {
        public string Username { get; set; }
        public string Text { get; set; }
        public decimal Cost { get; set; }
        public byte[] Image { get; set; }
        public int ID { get; set;}

        /// <summary>
        /// Take data about crowd from db
        /// </summary>
        /// <returns></returns>
        public static List<CROWD> FetchProjectsFromDatabase()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            // Fetch data from the database and return as a list of Project objects
            // You can replace this with your actual database retrieval logic
            List<CROWD> projects = new List<CROWD>();

            // Example data retrieval logic
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID, Username, Text, Cost, Image FROM Table_CROWD";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                       if (reader.IsDBNull(reader.GetOrdinal("Text")) || reader.IsDBNull(reader.GetOrdinal("Cost")) || reader.IsDBNull(reader.GetOrdinal("Image")))
                       { continue; }
                        CROWD project = new CROWD();
                        project.Username = reader["Username"].ToString();
                        project.Text = reader["Text"].ToString();
                        project.Cost = Convert.ToDecimal(reader["Cost"]);
                        project.Image = (byte[])reader["Image"];
                        project.ID = Convert.ToInt32(reader["ID"]);
                        projects.Add(project);
                    }
                }
            }

            return projects;
        }
    }


}