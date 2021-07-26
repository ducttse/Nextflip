let userID;
function sendCode() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            Email: document.getElementById("email").value
        })
    };
    fetch("/api/Login/ForgotPassword", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Success") {
                document.getElementById("alert").classList.add("d-none");
                userID = json.userID;
            }
            else {
                document.getElementById("alert").classList.remove("d-none");
            }
        })
}

function verirfyCode() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            Email: document.getElementById("email").value,
            Token: document.getElementById("code").value
        })
    };
    fetch("/api/Login/ForgotPassword", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Success") {
                document.getElementById("confirmEmail_wrapper").classList.add("d-none");
                document.getElementById("resetPassword_wrapper").classList.remove("d-none");
            }
            else {
                document.getElementById("alert").classList.remove("d-none");
            }
        })
}

function requestResetPassword() {
    let newPassword = document.getElementById("newPassword");
    let confirmPassword = document.getElementById("confirmPassword");
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            UserID: userID,
            Password: newPassword.value,
            ConfirmPassword: confirmPassword.value
        })
    }
    fetch("/api/ProfileManagement/ChangePassword", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == true) {
                location.replace("/Account/Login");
            }
            else {
                if (confirmPassword.value != newPassword.value) {

                }
            }
        })
}
