<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Site1.Register" %>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <link rel="stylesheet" href="/WebForms/Styles/Register__.css">   
    <link type="image/ico" sizes="16x16" rel="icon" href="/WebForms/image/business_buildings_city_icon_235174.ico">
</head>

<body>
    <div class="container">
        <header class="header">            
    </div>
    </header>
    <main class="main">
        <div class="form-description">    
<a href="/WebForms/Login.aspx" class="paragraph__description sign-description">Log In</a>
<p class="paragraph__description">Registration</p>
        </div>    
        <form class="form" id="registerForm" runat="server">
            <input type="text" placeholder="Name" id="Username" class="input" runat="server" />
            <input type="text" placeholder="Email" id="Email" class="input" runat="server" />
            <input type="password" placeholder="Password" id="Password" class="input input-icon password-input" data-password-input-id="Password" runat="server" />
            <input type="password" placeholder="Re-enter password" id="ConfirmPassword" class="input input-icon password-input" data-password-input-id="ConfirmPassword" runat="server" />
            <div class="select">
  <label class="label-user_type" for="user_type">User Type:</label>
  <div class="user-type-container">
    <select name="user_type" id="user_type" runat="server">
      <option value="common">Common User</option>
      <option value="admin">Government worker</option>
    </select>
    <div id="secret_key_container" style="display:none;">
      <label for="secret_key" id="secret_key_label">Secret Key:</label>
      <input type="password" id="secret_key_input" name="secret_key" runat="server">
    </div>
  </div>     
        </div><br>
        </form>
         <div>
            <button id="RegisterButton" runat="server" OnServerClick="RegisterButton_Click">Register</button>
        </div>
    <div>
        <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red"></asp:Label>
    </div>
    </main>

    <script src="/WebForms/scripts/Register.js"></script>
</body>

</html>

