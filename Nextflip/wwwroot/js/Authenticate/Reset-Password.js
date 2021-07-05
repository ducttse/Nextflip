let requestResetData = {
    UserID: "00000000000000000000",
    Password: "",
    ConfirmPassword: ""
}
function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}
const onInputPassword = debounce(() => validatePassword());


function validatePassword() {
    let password = document.getElementById("newpassword");
    let container = password.parentNode;
    let feedback = container.querySelector(".invalid-feedback");
    container.classList.add("was-validated");
    if (password.value.length >= 8 && password.value.length <= 32) {
        password.setCustomValidity("");
        feedback.textContent = "";
        container.classList.remove("was-validated");
        requestResetData.password = password.value;
    }
    else {
        password.setCustomValidity("length issue");
        feedback.textContent = "Invalid Password ! Password length must range from 8 - 32 character !";
    }

}

const onInputConfirmPassword = debounce(() => checkRePassword());

function checkRePassword() {
    let password = document.getElementById("newpassword");
    let repassword = document.getElementById("confirmPassword");
    let container = repassword.parentNode;
    let feedback = container.querySelector(".invalid-feedback");
    if (password.value == repassword.value) {
        repassword.setCustomValidity("");
        feedback.textContent = "";
        container.classList.remove("was-validated");
        requestResetData.ConfirmPassword = repassword.value;
    }
    else if (repassword.value != password.value) {
        repassword.setCustomValidity("not equal");
        feedback.textContent = "Cofirm password must match"
        container.classList.add("was-validated");
    }
}

function requestResetPassword() {
    console.log(requestResetData);
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestResetData)
    }
    fetch("/api/ProfileManagement/ChangePassword", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == true) {
                window.location.replace("/Profie/Index")
            }
        })
}