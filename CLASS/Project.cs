using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Web;
using System.Web.UI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Collections;

namespace Site1.CLASS
{
    public class Project 
    {
        /// <summary>
        /// field for take data from bd
        /// </summary>
        public string nameOfProject { get; set; }
        public string username { get; set; }
        public string description { get; set; }
        public decimal costOfProject { get; set; }
        public byte[] image { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

        public int projectId { get; set; }
        public int voteCount { get; set; }
        public int SufficientCountOfVotes { get; set; }

        /// <summary>
        /// Get data from db and add to list
        /// </summary>
        /// <returns></returns>
        public static List<Project> GetMunicipalProjectsFromDataBase()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            List<Project> projects = new List<Project>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM MunicipalProjects";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Project project = new Project();
                        project.username = reader["Username"].ToString();
                        project.nameOfProject = reader["NameOfProject"].ToString();
                        project.description = reader["Description"].ToString();

                        if (Convert.IsDBNull(reader["Cost"]))
                        {
                            project.costOfProject = 0;
                        }
                        else
                        {
                            project.costOfProject = Convert.ToDecimal(reader["Cost"]);
                        }

                        if (!Convert.IsDBNull(reader["Image"]) && ((byte[])reader["Image"]).Length > 0)
                        {
                            project.image = (byte[])reader["Image"];
                        }

                        if (!Convert.IsDBNull(reader["Latitude"]) && !Convert.IsDBNull(reader["Longitude"]))
                        {
                            project.latitude = reader["Latitude"].ToString();
                            project.longitude = reader["Longitude"].ToString();
                        }

                        project.projectId = Convert.ToInt32(reader["ProjectID"]);

                        project.SufficientCountOfVotes = Convert.ToInt32(reader["SufficientVoteCount"]);

                        projects.Add(project);
                    }
                }
            }

            return projects;
        }

        /// <summary>
        /// Get vote count current project use projectID
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static int GetVoteCountFromDatabase(int projectID)
        {
            int count = 0;
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string query = "SELECT VoteCount FROM MunicipalProjects WHERE ProjectID = @ProjectID";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@ProjectID", projectID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Add vote to db use projectID
        /// </summary>
        /// <param name="projectId"></param>
        public static void AddVoteToDatabase(int projectId)
        {
            int SufficientVoteCount = 0;

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string query = "Select SufficientVoteCount FROM MunicipalProjects WHERE  ProjectID = @ProjectID";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@ProjectID", projectId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            SufficientVoteCount = reader.GetInt32(0);
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string query = "UPDATE MunicipalProjects SET VoteCount = CASE WHEN VoteCount < @SufficientNumberOfVotes THEN VoteCount + 1 ELSE VoteCount END WHERE ProjectID = @ProjectID";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@ProjectID", projectId);
                    command.Parameters.AddWithValue("@SufficientNumberOfVotes", SufficientVoteCount);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Delete selected Project and add data to Excel Table
        /// </summary>
        /// <param name="projectId"></param>
        public static void DeletAndAddToExcel(int projectId)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            string nameOfProject = "";
            string description = "";
            double cost = 0;
            string longitude = "";
            string latitude = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "Select NameOfProject, Cost, Description, Longitude, Latitude FROM MunicipalProjects WHERE  ProjectID = @ProjectID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", projectId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nameOfProject = reader["NameOfProject"].ToString();
                            description = reader["Description"].ToString();
                            cost = Convert.ToDouble(reader["Cost"]);
                            latitude = reader["Latitude"].ToString();
                            longitude = reader["Longitude"].ToString();
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebForms", "Budget.xlsx");
            XSSFWorkbook workbook;

            if (File.Exists(filePath))
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    workbook = new XSSFWorkbook(fileStream);
                    var sheet = workbook.GetSheetAt(0);
                    var row = sheet.CreateRow(sheet.LastRowNum + 1);
                    row.CreateCell(0).SetCellValue(nameOfProject);
                    row.CreateCell(1).SetCellValue(description);
                    row.CreateCell(2).SetCellValue(longitude + ", " + latitude);
                    row.CreateCell(3).SetCellValue(cost);
                }
            }
            else
            {
                workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Sheet1");
                var row = sheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Name of Project");
                row.CreateCell(1).SetCellValue("Description");
                row.CreateCell(2).SetCellValue("Location");
                row.CreateCell(3).SetCellValue("Cost");

                row = sheet.CreateRow(1);
                row.CreateCell(0).SetCellValue(nameOfProject);
                row.CreateCell(1).SetCellValue(description);
                row.CreateCell(2).SetCellValue(longitude + "," + latitude);
                row.CreateCell(3).SetCellValue(cost);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM MunicipalProjects WHERE ProjectID = @ProjectID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", projectId);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Delete Selected Project 
        /// </summary>
        /// <param name="projectID"></param>
        public static void Delete(int projectID)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM MunicipalProjects WHERE ProjectID = @ProjectID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", projectID);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}