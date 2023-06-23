<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WRITE_CROWD.aspx.cs" Inherits="Site1.WebForms.WRITE_CROWD" %>

<!DOCTYPE html>
<html>
<head>
    <title>CROWDFAUND</title>
     <link rel="stylesheet"  href="/WebForms/Styles/Write_CROWD22.css">
    <link type="image/ico" sizes="16x16" rel="icon" href="/WebForms/image/business_buildings_city_icon_235174.ico">

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
    <div class="body_div">
        <div class="container_foto">
        <img class="crowdfunding_foto" src="\WebForms\Images\crowdfunding.png"  runat="server" />
        </div>
    <h1 class="CROWDFAUND_h1" >CROWDFAUND</h1>
    <p style="font-size:20px">What is Crowdfunding?</p>
    <p style="font-size:20px">Crowdfunding is a collective collaboration of people (donors) who voluntarily pool their money or other resources together, 
        usually over the Internet, to support the efforts of other people or organizations (recipients). 
        Fundraising can serve a variety of purposes - disaster relief, fan support, political campaign support, 
        funding for startup companies and small businesses, creating free software, generating profits from co-investments, and much more.
</p>
       <hr  color ="#1f6c7d" />

    <form runat="server" enctype="multipart/form-data" method="post">
         <label class="up_text" for="image">Upload an image:</label>
        <div class="field">
         <asp:FileUpload ID="ng_file" runat="server"  CssClass="btn"/>
            <div class="button-container">
            <asp:Button ID="btnUpload" runat="server" CssClass="btn" Text="Upload Image" OnClick="btnUpload_Click" />
            </div>
        </div>
          <img ID="preview" runat="server" style="display:none;"/>

        <label style="font-weight: bold;" for="crowdfunding_text">Tell us about Crowdfunding:</label><br>
       <textarea type="text" id="crowdfunding_text" name="crowdfunding_text" rows="4" cols="50"></textarea>
        <script>
    // Retrieve the form field element
    var crowdfundingTextElement = document.getElementById("crowdfunding_text");
    
    // Update the value with the retrieved text from the database
    crowdfundingTextElement.value = '<%= dbText %>'; // Replace 'retrievedText' with the actual variable name holding the retrieved text from the database
        </script>
        <label for="costsneeded" style="font-weight: bold; margin-top: 10px;" >Costs Needed:</label>
        <input type="text" id="Cost_Needed" runat="server"><br>
       
        <div class="button-container">
            <asp:Button ID="Submit" runat="server" CssClass="btn" Text="Submit" OnClick="Submit_Click" />
        </div>
        <div class="button-container">
            <asp:Button ID="Button1" runat="server" CssClass="btn" Text="Change Old" OnClick="Change_Old" />
        </div>
        <div style="padding-top:10px;">
            <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" CssClass="error-label" ></asp:Label>
        </div>
    
    </form>
            <hr  color ="#1f6c7d" />
        </div>
</body>
</html>
