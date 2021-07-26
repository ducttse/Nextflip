let requestEditOBj = {
    UserID: "",
    Fullname: "",
    DateOfBirth: "",
    PictureURL: ""
}

function loadProfileData() {
    let data = getProfile();
    console.log(data);
    requestEditOBj.UserID = data.userID;
    requestEditOBj.PictureURL = data.pictureURL;
    document.getElementById("fullname_edit").value = data.fullname;
    document.getElementById("dob_edit").value = data.dateOfBirth.split("-").reverse().join("-");
}

function setTriggerLoadProfile() {
    document.getElementById("imgAndName_holder").addEventListener("click", () => {
        loadProfileData();
    })
}

async function requestUploadPicture() {
    if (file == null) {
        return;
    }
    let url = `https://storage.googleapis.com/upload/storage/v1/b/next-flip/o?uploadType=media&name=User Profile Image/${getProfile().userID}`;
    await fetch(url, {
        body: file,
        headers: {
            Authorization: `Bearer ${token}`,
        },
        method: "POST"
    })
        .then(res => res.json())
        .then(json => {
            requestEditOBj.PictureURL = "https://storage.googleapis.com/" + json.bucket + "/" + json.name
        })
        .catch(err => console.log(err));
}

function hideModal() {
    var modal = new bootstrap.Modal(document.getElementById("edit_profile_modal"), {
        keyboard: false
    })
    modal.hide();
}

function hideMessage(message) {
    let alert = document.querySelector("alert")
    alert.classList.add("d-none");
    alert.textContent = message;
}

function showErrMessage() {
    let alert = document.querySelector("alert");
    alert.classList.remove("d-none");
}
function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}

const onInputName = debounce(() => validateName());

function validateName() {
    let name = document.getElementById("fullname_edit");
    let parent = name.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    if (name.value.length >= 1) {
        name.setCustomValidity("");
        feedback.textContent = "";
        parent.classList.remove("was-validated");
    }
    else {
        name.setCustomValidity("invalid");
        feedback.textContent = "Fullname must have at least one charecter";
        parent.classList.add("was-validated");
    }
}

function validateDOB(message) {
    let name = document.getElementById("fullname_edit");
    let parent = name.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    if (message == null) {
        name.setCustomValidity("");
        feedback.textContent = "";
        parent.classList.remove("was-validated");
    }
    else {
        name.setCustomValidity("invalid");
        feedback.textContent = message;
        parent.classList.add("was-validated");
    }
}

async function updateProfile() {
    requestEditOBj.Fullname = document.getElementById("fullname_edit").value;
    requestEditOBj.DateOfBirth = document.getElementById("dob_edit").value;
    if (file != null) {
        await requestUploadPicture();
    }
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestEditOBj)
    };
    fetch("/api/ProfileManagement/ChangeProfile", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == true) {
                clearEditInputForm();
                hideModal();
                location.reload();
            }
            else {
                document.getElementById("edit_profile_modal").querySelector("alert").classList.remove("d-none");
            }
        })
}

function clearEditInputForm() {
    let name = document.getElementById("fullname_edit");
    let dob = document.getElementById("dob_edit");
    name.value = "";
    dob.value = "";
    document.getElementById("picture_edit").value = "";
    name.parentNode.classList.remove("was-validated");
    dob.parentNode.classList.remove("was-validated");
}
document.getElementById("edit_profile_modal").addEventListener("hidden.bs.modal", () => {
    clearEditInputForm();
    document.getElementById("edit_profile_modal").querySelector("alert").classList.add("d-none");
})