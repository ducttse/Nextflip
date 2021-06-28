let newUserInfo = {
    UserEmail: "",
    Fullname: "",
    dateOfBirth: "",
    RoleName: ""
}

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
    requestParam = {
        UserEmail: "",
        Fullname: "",
        dateOfBirth: "",
        RoleName: ""
    }
    document.getElementById("email").value = requestParam.UserEmail;
    document.getElementById("email").parentNode.classList.remove("was-validated");
    document.getElementById("fullname").value = requestParam.Fullname;
    document.getElementById("dob").value = requestParam.dateOfBirth;
    document.getElementById("role").value = requestParam.RoleName;

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

const checkEmail = debounce(() => {
    requestParam.UserEmail = document.getElementById("fullname_detail").value;
    requestParam.dateOfBirth = "";
    requestApi()
        .then(res => res.json())
        .then(json => {
            if (json.message != null) {
                let email = document.getElementById("fullname_detail");
                let emailInputContainer = email.parentNode;
                let feedback = emailInputContainer.querySelector(".invalid-feedback");
                feedback.innerHTML = json.emailErr;
                if (json.message == "fail") {
                    if (json.emailErr != null) {
                        if (json.emailErr == "Email is existed") {
                            email.setCustomValidity("existed");
                        }
                        else if (json.emailErr == "Email is invalid format") {
                            email.setCustomValidity("invalid");
                        }
                        emailInputContainer.classList.add("was-validated");
                    }
                    else {
                        email.setCustomValidity("");
                    }
                }
            }
        }
        );
});

function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}
