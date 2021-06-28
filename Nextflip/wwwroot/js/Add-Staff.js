let requestParam = {
    UserEmail: "",
    Fullname: "",
    dateOfBirth: "",
    RoleName: ""
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

function requestApi() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestParam)
    };
    return fetch("/api/UserManagerManagement/CreateStaff", initObject);
}

function setRequetsParam() {
    let email = document.getElementById("email").value;
    let fullname = document.getElementById("fullname").value;
    let dob = document.getElementById("dob").value;
    let role = document.getElementById("role").value;
    requestParam = {
        UserEmail: email,
        Fullname: fullname,
        dateOfBirth: dob,
        RoleName: role
    }
}

function showModal() {
    var myModal = new bootstrap.Modal(document.getElementById("modal_flash"), {
        keyboard: false
    })
    myModal.show();
}

function CreateStaff() {
    setRequetsParam();
    requestApi()
        .then(res => res.json())
        .then(json => {
            console.log(json);
            let success = validate(json);
            if (success == "success") {
                resetRequestParam();
                appendFlashMessageContent(true);
                showModal();
            }
        })
}

function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}




function validate(json) {
    if (json.message != null) {
        if (json.message == "success") {
            return "success";
        }
        //check DOB
        let dob = document.getElementById("dob");
        let dobContainer = dob.parentNode;
        let DOBfeedback = dobContainer.querySelector(".invalid-feedback");
        if (json.dateTimeErr != null) {
            DOBfeedback.innerHTML = json.dateTimeErr;
            if (json.emailErr == "Date of birth is Invalid") {
                dob.setCustomValidity("Invalid");
            }
        }
        else {
            dob.setCustomValidity("");
        }
        dobContainer.classList.add("was-validated");
        // check role
        let fullname = document.getElementById("fullname");
        let nameContainer = fullname.parentNode;
        let nameFeedback = nameContainer.querySelector(".invalid-feedback");
        if (json.nameErr != null) {
            nameFeedback.innerHTML = json.nameErr;
            if (json.nameErr == "Full name must not be empty") {
                dob.setCustomValidity("empty");
            }
        }
        else {
            dob.setCustomValidity("");
        }
        nameContainer.classList.add("was-validated");
        //email
        let email = document.getElementById("email");
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
            }
            else {
                email.setCustomValidity("");
            }
        }
        emailInputContainer.classList.add("was-validated");
    }
}

const checkEmail = debounce(() => {
    requestParam.UserEmail = document.getElementById("email").value;
    requestParam.dateOfBirth = "";
    requestApi()
        .then(res => res.json())
        .then(json => {
            if (json.message != null) {
                let email = document.getElementById("email");
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

function appendFlashMessageContent(isSuccess) {
    let fail = ` <i class="far fa-times-circle fa-5x text-danger"></i>
                    <p class="fs-5">Opps! Something went wrong</p>
                    <button type="button" class="col-4 mx-auto btn btn-danger text-white" data-bs-dismiss="modal">
                        Try again
                    </button>`
    let success = ` <i class="far fa-check-circle fa-5x" style="color: #4bca81"></i>
                    <p class="fs-5">Success</p>
                    <button type="button" class="col-4 mx-auto btn btn-success text-white" 
                        style=" background-color: #4bca81 !important; border: #4bca81 !important;"
                        data-bs-dismiss="modal">
                        Continue
                    </button>`;

    let flashModal = document.getElementById("flash_message");
    if (flashModal.innerHTML != "") {
        flashModal.innerHTML = "";
    }
    flashModal.insertAdjacentHTML("afterbegin", isSuccess ? success : fail);
}