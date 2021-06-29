let newUserInfo = {
    UserEmail: "",
    Fullname: "",
    dateOfBirth: "",
    RoleName: "User Manager"
}
let isReset = false;
function requestApi() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(newUserInfo)
    };
    return fetch("/api/UserManagerManagement/CreateStaff", initObject);
}

function resetRequestParam() {
    newUserInfo = {
        UserEmail: "",
        Fullname: "",
        dateOfBirth: "",
        RoleName: "User Manager"
    }
    document.getElementById("email_detail").value = "";
    document.getElementById("email_detail").parentNode.classList.remove("was-validated");
    document.getElementById("fullname_detail").value = "";
    document.getElementById("fullname_detail").parentNode.classList.remove("was-validated");
    document.getElementById("dob").value = "";
    document.getElementById("dob").parentNode.classList.remove("was-validated");
    document.getElementById("role").value = "";
    isReset = true;
}

let AddStaffModal;

function showAddStaffModal() {
    if (AddStaffModal == null) {
        AddStaffModal = new bootstrap.Modal(document.getElementById("add_staff_modal"), {
            keyboard: false
        })
    }
    AddStaffModal.show();
}

function hideAddStaffModalOnSuccess() {
    var myModalEl = document.getElementById('add_staff_modal')
    myModalEl.addEventListener("hidden.bs.modal", () => {
        if (isReset) {
            appendFlashMessageContent(true);
            showModal("modal_flash");
            isReset = false;
        }

    })
    AddStaffModal.hide();
}

function requestCheckEmail() {
    let emailInputEl = document.getElementById("email_detail");
    if (emailInputEl.value.trim() == "") {
        return;
    }
    let parent = emailInputEl.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    newUserInfo.UserEmail = emailInputEl.value + "@gmail.com";
    fetch(`/api/UserManagerManagement/IsValidEmail/${newUserInfo.UserEmail}`)
        .then(res => res.json())
        .then(json => {
            if (json.message != null) {
                if (json.message == "Email is existed") {
                    feedback.innerHTML = json.message;
                    emailInputEl.setCustomValidity("existed");
                }
                else if (json.message == "Email is invalid format") {
                    feedback.innerHTML = json.message;
                    emailInputEl.setCustomValidity("invalid");
                }
                else if (json.message == "") {
                    feedback.innerHTML = "";
                    emailInputEl.setCustomValidity("");
                }
            }
            parent.classList.add("was-validated");
        })
}

const checkEmail = debounce(() => {
    requestCheckEmail();
});

function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}

function setNewInfo() {
    newUserInfo.UserEmail = document.getElementById("email_detail").value;
    newUserInfo.Fullname = document.getElementById("fullname_detail").value;
    newUserInfo.dateOfBirth = document.getElementById("dob").value;
    newUserInfo.RoleName = document.getElementById("role").value;
}

function requestCreateApi() {
    setNewInfo();
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(newUserInfo)
    };
    fetch("/api/UserManagerManagement/CreateStaff", initObject)
        .then(res => res.json())
        .then(json => {
            isReset = false;
            if (json.message == "fail") {
                if (json.nameErr != null) {
                    validateFullName(json.nameErr);
                }
                if (json.dateTimeErr != null) {
                    validateDOB(json.dateTimeErr);
                }
                if (json.emailErr != null) {
                    validateEmail(json.emailErr);
                }
            }
            else if (json.message == "success") {
                resetRequestParam();
                hideAddStaffModalOnSuccess();
            }
        })
}

function validateFullName(message) {
    let fullNamInputEl = document.getElementById("fullname_detail");
    let parent = fullNamInputEl.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    if (message == "Full name must not be empty") {
        feedback.innerHTML = message;
        fullNamInputEl.setCustomValidity("empty");
    }
    else {
        feedback.innerHTML = "";
        fullNamInputEl.setCustomValidity("");
    }
    parent.classList.add("was-validated");
}

function validateDOB(message) {
    let InputEl = document.getElementById("dob");
    let parent = InputEl.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    if (message == "Date of birth must not be empty") {
        feedback.innerHTML = message;
        InputEl.setCustomValidity("empty");
    }
    else {
        feedback.innerHTML = "";
        InputEl.setCustomValidity("");
    }
    parent.classList.add("was-validated");
}

function validateEmail(message) {
    let emailInputEl = document.getElementById("email_detail");
    let parent = InputEl.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    if (message == "Email is existed") {
        feedback.innerHTML = json.message;
        emailInputEl.setCustomValidity("existed");
    }
    else if (message == "Email is invalid format") {
        feedback.innerHTML = json.message;
        emailInputEl.setCustomValidity("invalid");
    }
    else if (message == "") {
        feedback.innerHTML = "";
        emailInputEl.setCustomValidity("");
    }
    parent.classList.add("was-validated");
}

