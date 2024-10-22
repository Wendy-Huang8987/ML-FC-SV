function togglePasswordVisibility() {
    const passwordField = document.getElementById("password");
    const toggleIcon = document.getElementById("toggleIcon");

    if (passwordField.type === "password") {
        passwordField.type = "text";
        toggleIcon.classList.remove("bi-eye");
        toggleIcon.classList.add("bi-eye-slash");
    } else {
        passwordField.type = "password";
        toggleIcon.classList.remove("bi-eye-slash");
        toggleIcon.classList.add("bi-eye");
    }
}

function togglePasswordVisibility1() {
    const passwordField = document.getElementById("password1");
    const toggleIcon = document.getElementById("toggleIcon1");

    if (passwordField.type === "password") {
        passwordField.type = "text";
        toggleIcon.classList.remove("bi-eye");
        toggleIcon.classList.add("bi-eye-slash");
    } else {
        passwordField.type = "password";
        toggleIcon.classList.remove("bi-eye-slash");
        toggleIcon.classList.add("bi-eye");
    }
}
function togglePasswordVisibility2() {
    const passwordField = document.getElementById("password2");
    const toggleIcon = document.getElementById("toggleIcon2");

    if (passwordField.type === "password") {
        passwordField.type = "text";
        toggleIcon.classList.remove("bi-eye");
        toggleIcon.classList.add("bi-eye-slash");
    } else {
        passwordField.type = "password";
        toggleIcon.classList.remove("bi-eye-slash");
        toggleIcon.classList.add("bi-eye");
    }
}
function resetToggleIcon() {
    const toggleIcon = document.getElementById("toggleIcon");
    const passwordField = document.getElementById("password");

    // Reset to bi-eye icon and password type
    isPasswordVisible = false;
    toggleIcon.classList.remove("bi-eye-slash");
    toggleIcon.classList.add("bi-eye");
    passwordField.type = "password";
}
function clearInputs() {
    var inputs = document.querySelectorAll('form input')

    inputs.forEach(function (input) {
        input.value = '';
    });
}
function resetpwd() {
    var inputField = document.getElementById("password");

    var currentType = inputField.type;

    if (currentType === 'text') {
        inputField.type = 'password';
    }
}
function resetpwd1() {
    var inputField = document.getElementById("password1");

    var currentType = inputField.type;

    if (currentType === 'text') {
        inputField.type = 'password';
    }
}
function resetpwd2() {
    var inputField = document.getElementById("password2");

    var currentType = inputField.type;

    if (currentType === 'text') {
        inputField.type = 'password';
    }
}

const passwordInput = document.getElementById('password1');
const passwordRequirements = document.getElementById('passwordRequirements');

//passwordInput.addEventListener('input', function () {
//    const password = passwordInput.value;

//    // Password requirements
//    const minLength = 8;
//    const hasUpperCase = /[A-Z]/.test(password);
//    const hasLowerCase = /[a-z]/.test(password);
//    const hasDigit = /\d/.test(password);

//    // Check if password meets requirements
//    const isPasswordValid = password.length >= minLength && hasUpperCase && hasLowerCase && hasDigit;

//    // Display feedback
//    if (isPasswordValid) {
//        passwordRequirements.style.color = 'green';
//        passwordRequirements.textContent = 'Password meets requirements';
//    } else {
//        passwordRequirements.style.color = 'red';
//        passwordRequirements.textContent = 'Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one digit.';
//    }
//});

function changeLanguage(lang) {
    $.ajax({
        url: '../Home/ChangeLanguage',
        type: 'POST',
        data: { lang: lang },
        success: function () {
            // 刷新页面以显示切换后的语言
            location.reload();
        },
        error: function () {
            // 处理错误
            alert('Failed to change language.');
        }
    });
}

// toggle dropdown button 
var currentdropbtn = ""

function mydropFunction(mode) {
    document.getElementById(mode).classList.toggle("show");
}

window.onclick = function (event) {
    if (!event.target.matches('.topleft-sidebar-dropbtn')) {
        var dropdowns = document.getElementsByClassName(currentdropbtn);
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}

// To show guide info box on click
var guide = ""

function openguide(mode) {
    if (guide !== "" && guide !== mode) document.getElementById(guide).className = "guide-wrapper";
    if (document.getElementById(mode).className == "guide-wrapper-shown") {
        document.getElementById(mode).className = "guide-wrapper";
    }
    else {
        guide = mode;
        document.getElementById(mode).className = "guide-wrapper-shown";
    }
}

// Toggle slide menu sidebar
// function openSlideMenu() {
//   document.getElementById('menu').style.width = '250px';
//   document.getElementById('content').style.marginLeft = '250px';
// }

// function closeSlideMenu() {
//   document.getElementById('menu').style.width = '0px';
//   document.getElementById('content').style.marginLeft = '0px';
// }

function toggleSlideMenu() {
    var menu = document.getElementById('menu');
    var content = document.getElementById('content');
    var sidebar = document.getElementById('sidebar');
    var sidebar_nav = document.getElementById('sidebar-nav');

    // Check if the menu is currently open
    if (menu.style.width === '250px') {
        // If open, close the menu
        menu.style.width = '0px';
        content.style.marginLeft = '65px';
        sidebar.style.marginLeft = '0px';
        menu.style.opacity = '0';
        sidebar_nav.style.opacity = '1';
    } else {
        // If closed, open the menu
        menu.style.width = '250px';
        content.style.marginLeft = '250px';
        sidebar.style.marginLeft = '250px';
        menu.style.opacity = '1';
        sidebar_nav.style.opacity = '0';
        menu.style.transition = 'width 0.5s ease, opacity 0.5s ease';
        content.style.transition = 'margin-left 0.5s ease';
        sidebar.style.transition = 'margin-left 0.5s ease';
        sidebar_nav.style.transition = 'opacity 0.2s ease';
    }

}

// Get file input element
let fileInput = document.getElementById('formFile');
let span = document.getElementById('fileName');
// Fires on file upload
fileInput.addEventListener('change', function (event) {

    // Get file name
    let fileName = fileInput.files[0].name;

    // Update file name in span
    span.innerText = fileName;
});

function filterSearchFunction() {
    // Declare variables
    var input, filter, table, tr, td, i, j, txtValue;
    input = document.getElementById("search-input");
    filter = input.value.toUpperCase();
    table = document.getElementById("search-table");
    tr = table.getElementsByTagName("tr");

    // Loop through all table rows starting from index 1 (skip thead)
    for (i = 1; i < tr.length; i++) {
        // Loop through all cells in the current row
        for (j = 0; j < tr[i].cells.length; j++) {
            td = tr[i].cells[j];
            if (td) {
                txtValue = td.textContent || td.innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    // If the search query is found, show the row and break the inner loop
                    tr[i].style.display = "";
                    break;
                } else {
                    // If the search query is not found in any cell, hide the row
                    tr[i].style.display = "none";
                }
            }
        }
    }
}

function toggleTheme() {
    const toggleSwitch = document.querySelector('.theme-switch input[type="checkbox"]');

    if (toggleSwitch.checked) {
        document.documentElement.setAttribute('data-theme', 'dark');
        localStorage.setItem('theme', 'dark');
    } else {
        document.documentElement.setAttribute('data-theme', 'light');
        localStorage.setItem('theme', 'light');
    }
}






