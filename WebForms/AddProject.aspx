<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProject.aspx.cs" Inherits="Site1.WebForms.AddProject" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <link type="image/ico" sizes="16x16" rel="foto" href="/WebForms/image/business_buildings_city_icon_235174.ico"/>
    <link rel="stylesheet" href="/WebForms/Styles/AddProject1.css"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        #map {
            height: 400px;
            width: 100%;
        }
    </style>
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDA9V9I1IVHJwxv3vT5qmp6Ivk18CC42ps"></script>
    <script type="text/javascript">
        function initMap() {
            var map = new google.maps.Map(document.getElementById('map'), {
                center: { lat: 50.4521089, lng: 30.5182261 },
                zoom: 8
            });

            var marker = new google.maps.Marker({
                position: { lat: 50.4521089, lng: 30.5182261 },
                map: map,
                draggable: true
            });

            google.maps.event.addListener(map, 'click', function (event) {
                marker.setPosition(event.latLng);
                document.getElementById("latitude").value = event.latLng.lat();
                document.getElementById("longitude").value = event.latLng.lng();
            });
        }
    </script>
</head>
<body onload="initMap()">


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

    <form id="form1" runat="server">
        <label for="image">Upload an image:</label>
        <div class="field">
        <asp:FileUpload ID="ng_file" runat="server" />
        <div class="button-container">
            <asp:Button ID="btnUpload" runat="server" CssClass="btn" Text="Upload an image" OnClick="UploadButtonClick" />
       </div>
            </div>

        <img ID="preview" runat="server" style="display:block; margin-bottom:10px; margin-top:10px; max-width:200px; max-height:200px;" />

        <label for="nameOfProject">Enter name of municipal project</label>
        <input type="text" id="nameOfProject" runat="server" />

        <label for="projectDescription">Enter description of your project</label>
        <textarea type="text" id="projectDescription" name="projectDescription" rows="4" cols="50"></textarea>

        <label for="NecessaryFunds">Enter required amount of money</label>
        <input type="text" id="NecessaryFunds" runat="server">    
        
        <label for="SufficientVotes">Enter required amount of votes for this project</label>
        <input type="text" id="SufficientVotes" runat="server">    

        <div>
            <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" CssClass="error-label"></asp:Label>
        </div>
        <div id="map"></div>

        <label for="latitude">Latitude:</label>
        <input type="text" id="latitude" name="latitude" />

        <label for="longitude">Longitude:</label>
        <input type="text" id="longitude" name="longitude" />

        <div class="button-container">
            <asp:Button ID="btnSubmit" runat="server" CssClass="btn" Text="Submit" OnClick="SubmitButtonClick" />
        </div>
    </form>
</body>
</html>
