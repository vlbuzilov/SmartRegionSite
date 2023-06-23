//Eye open and close 
const passwordInput = document.getElementById("Password");
const confirmPasswordInput = document.getElementById("ConfirmPassword");
const showPasswordIcons = document.querySelectorAll(".input-icon");

showPasswordIcons.forEach(icon => {
    icon.addEventListener("click", () => {
        const inputId = icon.getAttribute("data-password-input-id");
        const inputEl = document.getElementById(inputId);
        if (inputEl.type === "password") {
            inputEl.type = "text";
            icon.classList.add("show-password");
        } else {
            inputEl.type = "password";
            icon.classList.remove("show-password");
        }
    });
});


//Secret key if type == admin
var userTypeDropdown = document.getElementById("user_type");
var secretKeyContainer = document.getElementById("secret_key_container");

userTypeDropdown.addEventListener("change", function () {
    if (userTypeDropdown.value === "admin") {
        secretKeyContainer.style.display = "block";
    } else {
        secretKeyContainer.style.display = "none";
    }
});