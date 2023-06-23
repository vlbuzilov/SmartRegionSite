document.addEventListener("DOMContentLoaded", function () {
    const scrollLeftBtn = document.querySelector(".scroll-left-btn");
    const scrollRightBtn = document.querySelector(".scroll-right-btn");
    const viewport = document.querySelector(".viewport");

    scrollLeftBtn.addEventListener("click", function () {
        viewport.scrollLeft -= 300;
    });

    scrollRightBtn.addEventListener("click", function () {
        viewport.scrollLeft += 300;
    });
});


function ShowDonationDialog(event) {
    event.preventDefault();
  
    var project = $(event.target).closest('.project');
 
    var name = project.find('.project-username').text();
    var cost = project.find('.project-cost').text();
  
    $('#donation-modal').show();

    var progress = project.find('.progress-bar .progress').width() / project.find('.progress-bar').width() * 100;

    if (progress === 100) {
        alert('Sorry, this project has already reached its goal and cannot accept any more donations.');
    }

    $('#donation-modal h2').text('Make Donation for ' + name);
    $('#donation-modal p:nth-of-type(1)').text('Max value - ' + cost);
    $('#donation-modal p:nth-of-type(2)').text('Enter the amount you wish to donate for ' + name + ':');
    $('#donation-modal p:nth-of-type(3)').text('Enter your card ID');

 

   
}

function CloseDonationDialog() {
    var modal = document.getElementById("donation-modal");
    $('#donation-modal').hide();
    $('#donation-amount').val('');
    $('#card-id').val('');
    modal.style.display = "none";
}


function MakeDonation() {
    var amount = document.getElementById("donation-amount").value;
    var cardId = document.getElementById("card-id").value;
    var username = $('#donation-modal h2').text().replace('Make Donation for ', '');
    var cost = $('#donation-modal p:nth-of-type(1)').text().replace('Max value - ', '');

    var amount = document.getElementById("donation-amount").value;
    if (isNaN(amount) || amount == "") {
        alert('Amount only number!');
        return;
    }
  

    $.ajax({
        type: "POST",
        url: "/WebForms/CROUDFAUD.aspx/Make_Donat",
        data: JSON.stringify({ amount: amount, cardId: cardId, username: username, cost: cost }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log("Donation processed successfully.");
        },
        failure: function (response) {
            console.log("Failed to process donation.");
        }
    });
    CloseDonationDialog();
}

function Delete(event) {
    event.preventDefault();
    var project = $(event.target).closest('.project');
    var name = project.find('.project-username').text();
    $.ajax({
        type: "POST",
        url: "/WebForms/CROUDFAUD.aspx/Delete",
        data: JSON.stringify({ username: name }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log("Successfully.");
        },
        failure: function (response) {
            console.log("Failed");
        }
    });
    location.reload(true);

}
