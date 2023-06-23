<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Site1.WebForm1" %>

<!DOCTYPE html>
<html>
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>SMARTREGION</title>
  <link rel="stylesheet"  href="/WebForms/Styles/Main-var1.css">
  <link type="image/ico" sizes="16x16" rel="foto" href="/WebForms/image/business_buildings_city_icon_235174.ico">
</head>
<body>  
    <header>
      <div class="header-container">
        <h1>SMARTREGION</h1>         
          <div class="logout-container">
              <img src="/WebForms/Images/logout-svgrepo-com.svg" alt="Log out">
              <a class="logouttext" href ="/WebForms/Login.aspx">Log out</a>
          </div>
        <nav>
          <ul>
            <li><a href="#" class="active">Profile</a></li>
            <li><a href="/WebForms/MunicipalProjects.aspx">Project platform</a></li>
            <li><a href="/WebForms/CROUDFAUD.aspx">Crowdfunding platform</a></li>
            <li><a href="/WebForms/Regional_budget.aspx">Regional budget</a></li>
          </ul>
        </nav>         
      </div>
    </header>
    <main>       
        <ContentTemplate>
          <div class="profile-container">
            <h2>Profile</h2>
            <div class="fields">
              <div class="field">
                  <form id="form1" runat="server" enctype="multipart/form-data" method="post">
                <asp:FileUpload ID="ng_file" runat="server" />
                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                        </form>
                <img ID="preview" runat="server" />
              </div>
              <div class="field">
                <label for="name" id="lblName" runat="server">Name:</label>
              </div>
              <div class="field">
                <label for="email" id="lblEmail" runat="server">Email:</label>
              </div>
              <div class="field">
                <label for="type" id="lblType" runat="server">Type of user:</label>
              </div>
                <div>
<button id="AddProject" OnServerClick="ADD_CITY" runat="server" >Add new city project</button>
<button id="Crowdfunding" OnServerClick="ADD_CROWD" runat="server" >Write a text about help (crowdfund)</button>
    <script>  
        sessionStorage.setItem('type', '<%= Session["type"] %>');
        if (sessionStorage.getItem('type') === "admin") {
            document.getElementById("AddProject").style.display = "block";
            document.getElementById("Crowdfunding").style.display = "none";
        }
        else {
            document.getElementById("AddProject").style.display = "none";
            document.getElementById("Crowdfunding").style.display = "block";
        }
    </script>
</div>
                <div> 
                    <label for="name" id="Creat_New" runat="server" visible ="false">Congratulate your raised money. If you wish your can create crowdfund! 
                        <img  class="Label_text" src="/WebForms/Images/celebrate.png" alt="Congratulate your raised money" /> 
                    </label> 
               </div>
                 <div> <label for="name" id="New" runat="server" visible ="false">Your can create crowdfund! </label> </div>
            </div>
          </div>
        </ContentTemplate>
    </main>
</body>
</html>

