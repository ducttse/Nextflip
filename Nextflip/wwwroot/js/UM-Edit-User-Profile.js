let UserRequestData = {
    userID: "",
    userEmail: "",
    googleID: null,
    googleEmail: null,
    roleName: "",
    hashedPassword: null,
    fullname: "",
    dateOfBirth: "",
    status: null,
    pictureURL: null,
    note: null
}
let isEdit = false;

let EditStaffModal;

function appendInfoToEditWrapper() {
    let emailEL = document.getElementById("email_edit_text");
    if (emailEL.innerHTML != "") {
        emailEL.innerHTML = ""
    }
    emailEL.innerHTML = UserRequestData.googleEmail == null ? UserRequestData.userEmail : UserRequestData.googleEmail;
    let fullnameEl = document.getElementById("fullname_edit_input");
    let roleEL = document.getElementById("role_edit_input");
    let dobEL = document.getElementById("dob_detail_edit");
    fullnameEl.value = UserRequestData.fullname;
    fullnameEl.setAttribute("initvalue", UserRequestData.fullname);
    roleEL.value = UserRequestData.roleName;
    roleEL.setAttribute("initvalue", UserRequestData.roleName);
    dobEL.value = UserRequestData.dateOfBirth.slice(0, 10);
    dobEL.setAttribute("initvalue", UserRequestData.dateOfBirth.slice(0, 10));
}

function showEditStaffModal() {
    if (EditStaffModal == null) {
        EditStaffModal = new bootstrap.Modal(document.getElementById("edit_user_modal"), {
            keyboard: false
        })
    }
    EditStaffModal.show();
}

function hideEditStaffModalOnSuccess() {
    var myModalEl = document.getElementById('edit_user_modal')
    myModalEl.addEventListener("hide.bs.modal", () => {
        resetValidation();
        if (isEdit) {
            appendFlashMessageContent(true);
            showModal("modal_flash");
            isEdit = false;
        }
    })
    EditStaffModal.hide();
}


function GetUserProfile(obj) {
    UserRequestData.userID = obj.getAttribute("userID");
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            userID: UserRequestData.userID
        })
    };
    fetch("/api/UserManagerManagement/GetUserProfile", initObject)
        .then(res => res.json())
        .then(json => {
            UserRequestData = json;
            resetValidation();
            appendInfoToEditWrapper();
            showEditStaffModal();
        })
}

function checkInput() {
    let fullnameEl = document.getElementById("fullname_edit_input");
    let roleEL = document.getElementById("role_edit_input");
    let dobEL = document.getElementById("dob_detail_edit");
    if (fullnameEl.value != fullnameEl.getAttribute("initvalue")) {
        UserRequestData.fullname = fullnameEl.value;
        isEdit = true;
    }
    if (roleEL.value != roleEL.getAttribute("initvalue")) {
        UserRequestData.roleName = roleEL.value;
        isEdit = true;
    }
    if (dobEL.value != dobEL.getAttribute("initvalue")) {
        UserRequestData.dobEL = dobEL.value;
        isEdit = true;
    }
}

function resetValidation() {
    console.log("reste")
    document.getElementById("fullname_edit_input").parentNode.classList.remove("was-validated");
    document.getElementById("dob_detail_edit").parentNode.classList.remove("was-validated");
}

function validateFullNameDetail(message) {
    let fullNamInputEl = document.getElementById("fullname_edit_input");
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

function validateDOBDetail(message) {
    let InputEl = document.getElementById("dob_detail_edit");
    let parent = InputEl.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    if (message == "Date of birth is Invalid") {
        feedback.innerHTML = message;
        InputEl.setCustomValidity("empty");
    }
    else {
        feedback.innerHTML = "";
        InputEl.setCustomValidity("");
    }
    parent.classList.add("was-validated");
}

function requestEdit() {
    checkInput();
    if (!isEdit) {
        return;
    }
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            userID: UserRequestData.userID,
            roleName: UserRequestData.roleName,
            fullname: UserRequestData.fullname,
            dateOfBirth: UserRequestData.dateOfBirth
        })
    };
    fetch("/api/UserManagerManagement/EditStaffInfo", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Fail") {
                isEdit = false;
                if (json.dateTimeErr != null) {
                    validateDOBDetail(json.dateTimeErr);
                }
                if (json.nameErr != null) {
                    validateFullNameDetail(json.nameErr);
                }
            }
            else if (json.message == "Success") {
                isEdit = true;
                hideEditStaffModalOnSuccess();
            }
        })
}