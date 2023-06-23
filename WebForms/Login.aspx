<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Site1.Login" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Login</title>
    <link rel="stylesheet" href="/WebForms/Styles/Login.css">  
     <link rel="stylesheet" href="/WebForms/Styles/reset.min.css" />
    <link type="image/ico" sizes="16x16" rel="icon" href="/WebForms/image/business_buildings_city_icon_235174.ico">
</head>
<body>
    <body>
    <div class="container">
      <header class="header">     
          </header>
      <main class="main">
        <section class="main-section">
          <div class="form-wrapper">
            <form  class="form" runat="server">
              <fieldset class="form-wrapper__sign-in">
                <a class="form-button">Log In</a>
                <a href="/WebForms/Register.aspx" class="form-button form-button-second">Registration</a>
              </fieldset>      
              <fieldset class="form-wrapper__input">
                <div class="form-wrapper__email">
                  <input type="text"  id="Username" class="form-input form-email" placeholder="Username" runat="server">
                </div>
                <div class="form-wrapper__password">
                  <input type="password"  id="Password" class="form-input form-password" placeholder="Password" runat="server">
                </div>
        <div class="button-container">
            <button id="LoginButton" runat="server" OnServerClick="LoginButton_Click">Login</button>
        </div>
        <div>
            <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>