<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CROUDFAUD.aspx.cs" Inherits="Site1.WebForms.CROUDFAUD" %>

<!DOCTYPE html>
<html>
<head>
   <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
  <link rel="stylesheet" href="/WebForms/Styles/ROLL22.css">
   <link type="image/ico" sizes="16x16" rel="icon" href="/WebForms/image/business_buildings_city_icon_235174.ico">
  <script src="/WebForms/scripts/CROWD-var5.js"></script>
</head>

<body>
 <%-- <div class="project-grid">
    <div class="scroll-left-btn">&lt;</div>
    <div class="viewport">
        <ul>
            <li><a href="/WebForms/Main.aspx">Profile</a></li>
            <li><a href="/WebForms/MunicipalProjects.aspx">Project platform</a></li>
            <li><a href="" class="active">Crowdfunding platform</a></li>
            <li><a href="/WebForms/Regional_budget.aspx">Regional budget</a></li>
          </ul>

--%>
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

      <div class="projects-container">
  <form  class="form" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  <asp:Repeater ID="rptProjects" runat="server" >
  <ItemTemplate>
   

    <div class="project" data-project-id='<%# Eval("ID") %>'>
           <% if (Session["type"] != null && Session["type"].ToString() == "admin") { %>
    <button CssClass="btn" class="delete-btn" data-project-id='<%# Eval("ID") %>'  OnClick="Delete(event)" runat="server">Delete</button>
     <% } %>

       <h2><asp:Label ID="Username" runat="server" Text='<%# Eval("Username") %>' CssClass="project-username" Visible="true"></asp:Label></h2>
      <asp:Label ID="Text" runat="server" Text='<%# Eval("Text") %>' CssClass="project-text" Visible="true"></asp:Label>
      <asp:Label ID="Cost" runat="server" Text='<%# Eval("Cost") %>' CssClass="project-cost" Visible="true"></asp:Label>        
      <img src='<%# "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("Image")) %>' alt="Project Image" />
     <button class="donate-btn" runat="server"  OnClick="ShowDonationDialog(event)" >Make Donation</button>  
    
  <div class="progress-bar" data-project-id='<%# Eval("ID") %>'>
 <asp:UpdatePanel ID="myUpdatePanel" runat="server"  CssClass="update-panel" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel runat="server" class="progress" ID="progress"></asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
  </div>     
    </div>      
  </ItemTemplate>
</asp:Repeater>           
</form>
</div>
  <div id="donation-modal" class="modal" style="display: none;">
  <div class="modal-content">
    <span class="close" onclick="CloseDonationDialog()">&times;</span>
    <h2>Make Donation</h2>
    <p>Max value:</p>
    <p>Enter the amount you wish to donate:</p>
    <input type="text" id="donation-amount" name="donation-amount">
    <p>Enter your card ID:</p>
    <input type="text" id="card-id" name="card-id">
      <br>
    <button class="donate-confirm-btn" onclick="MakeDonation()">Confirm</button>
  </div>
</div>
    </div>
    <%--<div class="scroll-right-btn">&gt;</div>--%>
  </div>
         <script src = "/WebForms/scripts/Data.js"></script>   
</body>

    </html>

