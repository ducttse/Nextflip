let imgURL = "";
function loadLocalStorage() {
    let profile = localStorage.getItem("profile");
    if (profile == null) return;
    profile = JSON.parse(profile);
    document.getElementById("email").value = profile.email;
    document.getElementById("fullname").value = profile.fullname;
    imgURL = profile.email;
    localStorage.removeItem("profile");
}
loadLocalStorage();
function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}

const processChange = debounce((obj) => {
    if (obj.value.trim().length == 0) {
        return;
    }
    checkMail(obj.value);
});


function checkMail(value) {
    console.log("check");
    let inputData = {
        Email: value
    }
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(inputData)
    }
    fetch("/api/Login/CheckEmail", initObject)
        .then(res => res.json())
        .then(json => {
            validateMail(json.message);
        });
}

function validateMail(message) {
    let container = document.getElementById("email_container");
    let feedback = container.querySelector(".invalid-feedback");
    let email = document.getElementById("email");
    container.classList.add("was-validated");
    if (message == "Email does not exist !") {
        email.setCustomValidity("");
        feedback.textContent = "";
        container.classList.remove("was-validated");
    }
    else if (message == "Invalid Email !") {
        email.setCustomValidity("invalid");
        feedback.textContent = "Wrong email format";
    }
    else if (message == "Valid") {
        email.setCustomValidity("existed");
        feedback.textContent = "Email is exsited";
    }

}

const onInputPassword = debounce(() => validatePassword());

function validatePassword() {
    let password = document.getElementById("password");
    let container = document.getElementById("password_container")
    let feedback = container.querySelector(".invalid-feedback");
    container.classList.add("was-validated");
    if (password.value.length >= 8 && password.value.length <= 32) {
        password.setCustomValidity("");
        feedback.textContent = "";
        container.classList.remove("was-validated");
    }
    else {
        password.setCustomValidity("length issue");
        feedback.textContent = "Invalid Password ! Password length must range from 8 - 32 character !";
    }
}

const onInputRePassword = debounce(() => checkRePassword());

function checkRePassword() {
    let password = document.getElementById("password");
    let repassword = document.getElementById("confirmPassword");
    let container = document.getElementById("repassword_container")
    let feedback = container.querySelector(".invalid-feedback");
    if (password.value == repassword.value) {
        repassword.setCustomValidity("");
        feedback.textContent = "";
        container.classList.remove("was-validated");
    }
    else if (repassword.value != password.value) {
        repassword.setCustomValidity("not equal");
        feedback.textContent = "Confirm password must match"
        container.classList.add("was-validated");
    }
}

const onInputNanme = debounce(() => checkName())

function checkName() {
    let container = document.getElementById("name_container");
    let name = document.getElementById("fullname");
    let feedback = container.querySelector(".invalid-feedback");
    container.classList.add("was-validated");
    if (name.value.length >= 1) {
        container.classList.remove("was-validated");
        name.setCustomValidity("");
        feedback.textContent = "";
    }
    else {
        name.setCustomValidity("");
        feedback.textContent = "Invalid name length";
    }
}

function checkDOB(message) {
    let container = document.getElementById("dob_container");
    let dob = document.getElementById("dob");
    let feedback = container.querySelector(".invalid-feedback");
    console.log(message);
    if (message == null) {
        container.classList.remove("was-validated");
        dob.setCustomValidity("");
        feedback.textContent = "";
    }
    else {
        container.classList.add("was-validated");
        dob.setCustomValidity("invalid");
        feedback.textContent = message;
    }
}

function requestSignUp() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let date = $("#dob").datepicker("option", "dateFormat", "yy-mm-dd").val();
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            UserEmail: document.getElementById("email").value,
            Password: document.getElementById("password").value,
            ConfirmPassword: document.getElementById("confirmPassword").value,
            Fullname: document.getElementById("fullname").value,
            DateOfBirth: date
        })
    }
    $("#dob").datepicker("option", "dateFormat", "dd-mm-yy");
    fetch("/api/RegisterAccount/Register", initObject).then(res => res.json()).then(json => {
        if (json.message != null && json.message == "Success") {
            fetch("/api/Login/LoginAccount", {
                method: "POST",
                headers: {
                    "Content-Type": "text/json",
                    "Accept": "application/json, text/plain, */*"
                },
                body: JSON.stringify({
                    Email: document.getElementById("email").value,
                    Password: document.getElementById("password").value
                })
            })
                .then(res => res.json())
                .then(json => {
                    if (json.message == true) {
                        window.location.replace(json.url);
                    }
                })
        }
        else {
            checkMail(document.getElementById("email").value);
            validatePassword();
            checkRePassword();
            checkName();
            checkDOB(json.dateOfBirthError);
        }
    })
}
