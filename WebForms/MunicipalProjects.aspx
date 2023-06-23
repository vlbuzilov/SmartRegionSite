<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MunicipalProjects.aspx.cs" Inherits="Site1.WebForms.MunicipalProjects" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Projects</title>
    <link rel="stylesheet" href="/WebForms/Styles/MunicipalProjects1.css">
    <link type="image/ico" sizes="16x16" href="/WebForms/image/business_buildings_city_icon_235174.ico">
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDA9V9I1IVHJwxv3vT5qmp6Ivk18CC42ps"></script>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
   
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
    <form runat="server">
          <div class="project-grid">
                <div class="projects-container">
                    <asp:Repeater ID="rptProjects" runat="server">
                        <ItemTemplate>
                            <div class="project">
                                
                               <% if (Session["type"] != null && Session["type"].ToString() == "admin") { %>
                                <asp:Button ID="btnConsider" runat="server" CssClass="btn btn-primary" Text="Send for consideration" CommandArgument='<%# Eval("projectId") %>' OnClick="btnDeleteAndSend_Click" />
                                <% } %>

                                <% if (Session["type"] != null && Session["type"].ToString() == "admin") { %>
                                <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-primary" Text="Delete" CommandArgument='<%# Eval("projectId") %>' OnClick="btnDelete_Click" />
                                <% } %>


                                <h2><%# Eval("nameOfProject") %></h2>
                                <p><%# Eval("description") %></p>
                                <p><%# Eval("costOfProject") != DBNull.Value ? Eval("costOfProject") : "" %></p>
                                <p><%# Eval("username") %></p>
                                <img src='<%# "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("image")) %>' alt="Project Image" />
                                
                                <p style="margin-bottom:10px;">Votes: <span id="voteCount_<%# Eval("projectId") %>"><%# Eval("voteCount") %></span> /<%# Eval("SufficientCountOfVotes") %></p>
                               <button type="button" class="btn btn-primary" id="voteBtn_<%# Eval("projectId") %>" onclick="addVote(<%# Eval("projectId") %>, <%# Eval("SufficientCountOfVotes") %>)">Vote</button>
 
                                <button type="button"  onclick="initMap(<%# Eval("latitude") %>, <%# Eval("longitude") %>);">Show Map</button>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                </div>
                <div id="map"></div>
            </div>
          

        </div>
    </form>
    <script>
        function addVote(projectId, maxVotes) {
            var voteCountSpan = $("#voteCount_" + projectId);
            if (voteCountSpan.length > 0) {
                var currentVoteCount = parseInt(voteCountSpan.text());
                if (currentVoteCount >= maxVotes) {
                    alert("Sufficient number of votes was recieved, the project was sent for consideration.");
                    return;
                }
                $.ajax({
                    type: "POST",
                    url: "MunicipalProjects.aspx/AddVote",
                    data: JSON.stringify({ projectId: projectId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        voteCountSpan.text(response.d);
                        if (parseInt(response.d) >= maxVotes) {
                            $("<div class='alert alert-success'>Sufficient number of votes were received, the project was sent for consideration</div>").insertBefore(".vote-form");
                            voteCountSpan.parent().hide();
                        }
                    },
                    failure: function (response) {
                        alert("Failed to add vote.");
                    }
                });
            }
        }
    </script>


   
    <script>
        function initMap(latitude, longitude) {
            var map = new google.maps.Map(document.getElementById('map'), {
                center: { lat: latitude, lng: longitude },
                zoom: 12
            });

            var marker = new google.maps.Marker({
                position: { lat: latitude, lng: longitude },
                map: map,
                draggable: true
            });
        }
    </script>

</body>
</html>
