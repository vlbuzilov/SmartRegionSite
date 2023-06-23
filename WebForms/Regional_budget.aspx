<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Regional_budget.aspx.cs" Inherits="Site1.WebForms.Regional_budget" %>

<!DOCTYPE html>

<html>
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
     <link rel="stylesheet" href="/WebForms/Styles/Regional_budget.css">
    <link type="image/ico" sizes="16x16" rel="icon" href="/WebForms/image/business_buildings_city_icon_235174.ico">
  <script src="/WebForms/scripts/Budget.js"></script>
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
            <li><a href="/WebForms/Main.aspx" class="active">Profile</a></li>
            <li><a href="/WebForms/MunicipalProjects.aspx">Project platform</a></li>
            <li><a href="/WebForms/CROUDFAUD.aspx">Crowdfunding platform</a></li>
            <li><a href="/WebForms/Regional_budget.aspx">Regional budget</a></li>
          </ul>
        </nav>         
      </div>
    </header>
        <main>
            <div class="main_container">
                <h2>Table of budget</h2>
             <form id="form1" runat="server">
        <div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField DataField="Project Name" HeaderText="Project Name" />
         <asp:BoundField DataField="Describe Project " HeaderText="Describe Project" />
        <asp:BoundField DataField="Location" HeaderText="Location" />
        <asp:BoundField DataField="Cost" HeaderText="Cost" />
    </Columns>
</asp:GridView>
            <h2>Graphic of budget</h2>
            <img class="img_container" src="/WebForms/ViewPage1.cshtml" />
        </div>
    </form>
                </div>
        </main>

        </body>
</html>
