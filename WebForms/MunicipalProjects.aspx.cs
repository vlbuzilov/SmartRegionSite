using Site1.CLASS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site1.WebForms
{
    /// <summary>
    /// MunicipalProjects Page
    /// </summary>
    public partial class MunicipalProjects : System.Web.UI.Page
    {

        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProjects();
            }
        }

        /// <summary>
        /// Take data from BD
        /// </summary>
        private void BindProjects()
        {
            // Fetch data from the database
            List<Project> projects = Project.GetMunicipalProjectsFromDataBase();

            // Bind data to the repeater control
            rptProjects.DataSource = projects;
            rptProjects.DataBind();
        }


        /// <summary>
        /// Add Vote to BD WEB_METHOD use JS 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [WebMethod]
        public static int AddVote(int projectId)
        {
            Project.AddVoteToDatabase(projectId);
            int voteCount = Project.GetVoteCountFromDatabase(projectId);
            return voteCount;
        }

        /// <summary>
        /// Delete selected Project and send data to Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteAndSend_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int projectId = Convert.ToInt32(btn.CommandArgument);

            // Add a vote to the project in the database
            Project.DeletAndAddToExcel(projectId);

            // Rebind the projects to update the vote count
            BindProjects();
        }

        /// <summary>
        /// Delete Selected Project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int projectId = Convert.ToInt32(btn.CommandArgument);

            // Add a vote to the project in the database
            Project.Delete(projectId);

            // Rebind the projects to update the vote count
            BindProjects();
        }


        /// <summary>
        /// Add a vote to the project in the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void VoteButtonClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int projectId = Convert.ToInt32(btn.CommandArgument);

            // Add a vote to the project in the database
            Project.AddVoteToDatabase(projectId);

            // Rebind the projects to update the vote count
            BindProjects();
        }



    }

}