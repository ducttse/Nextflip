let ChosenCheckBox;
let isSuccess = false;
let isRequest = false;
function setChosenCheckBox(item) {
    ChosenCheckBox = item;
}

function rollBack() {
    ChosenCheckBox.checked ? ChosenCheckBox.checked = false : ChosenCheckBox.checked = true;
}

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

async function reuqestDeactive() {
    let formData = new FormData();
    let UserID = document.getElementById("Form_UserID").value;
    let Note = document.getElementById("note").value;
    formData.append('UserID', UserID);
    formData.append('Note', Note);
    if (Note.trim().length != 0) {
        isRequest = true;
    }
    else {
        isRequest = false;
    }
    await fetch("https://localhost:44341/api/UserManagerManagement/InactiveAccount", {
        body: formData,
        method: "post"
    })
        .then(res => res.json())
        .then(json => {
            console.log("in fetch DEactive");
            isSuccess = false;
            if (json.message !== null) {
                if (json.message == "success") {
                    appendFlashMessageContent(true);
                    isSuccess = true;
                }
                else {
                    appendFlashMessageContent(false);
                }
            }
            else {
                appendFlashMessageContent(false);
            }
            document.getElementById("note").value = "";
        });
    return new Promise((resolve) => { resolve("resolved") });
}

async function requestActive() {
    let formData = new FormData();
    let UserID = document.getElementById("Check_UserID").value;
    formData.append('UserID', UserID);
    isRequest = true;
    await fetch("https://localhost:44341/api/UserManagerManagement/ActiveAccount", {
        body: formData,
        method: "post"
    })
        .then(res => res.json())
        .then(json => {
            isSuccess = false;
            if (json.message !== null) {
                if (json.message == "success") {
                    appendFlashMessageContent(true);
                    isSuccess = true;
                }
                else {
                    appendFlashMessageContent(false);
                }
            }
            else {
                appendFlashMessageContent(false);
            }
        });
    return new Promise((resolve) => { resolve("resolved") });
}

function setNote(note) {
    let reason = document.getElementById("reason");
    if (reason.innerHTML != "") {
        reason.innerHTML = "";
    }
    reason.insertAdjacentHTML("beforeend", note);
}

function getNote(userid) {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({ UserID: userid })
    };
    fetch("/api/UserManagerManagement/ReasonInactived", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.reason !== null) {
                setNote(json.reason);
            }
        })
}

let currentModal;

function hideModal() {
    currentModal.hide();
}

function showModal(modalName) {
    var myModal = new bootstrap.Modal(document.getElementById(modalName), {
        keyboard: false
    })
    myModal.show();
    currentModal = myModal;
}

function SetEvent() {
    document.getElementById("confirm_btn").addEventListener("click", () => {
        requestActive().then(() => {
            hideModal();
            showModal("modal_flash");
        })
    });
    document.getElementById("submit_btn").addEventListener("click", () => {
        reuqestDeactive().then(() => {
            hideModal();
            showModal("modal_flash");
        })

    })
    document.getElementById("modal_flash").addEventListener("hide.bs.modal", () => {
        isRequest = false;
        isSuccess = false;
        if (isFiltered && isSearched) {
            searchWithFilter();
        }
        else if (isFiltered) {
            requestWithFilter();
        }
        else if (isSearched) {
            search(requestParam.SearchValue);
        }
    })
    document.getElementById("modalForm").addEventListener("hide.bs.modal", () => {
        if (isRequest) {
            if (!isSuccess) {
                rollBack();
            }
        }
        else {
            rollBack();
        }
    })
    document.getElementById("modalCheck").addEventListener("hide.bs.modal", () => {
        if (isRequest) {
            if (!isSuccess) {
                rollBack();
            }
        }
        else {
            rollBack();
        }
    })
}

function addEvent() {
    let collection = document.getElementsByClassName("status_btn");
    for (let i = 0; i < collection.length; i++) {
        let item = collection[ i ];
        let userid = item.getAttribute("userid");
        item.addEventListener("change", (evt) => {
            setChosenCheckBox(evt.target);
            if (item.checked) {
                getNote(userid);
                showModal("modalCheck");
                document.getElementById("Check_UserID").value = userid;
            }
            else {
                showModal("modalForm");
                document.getElementById("Form_UserID").value = userid;
            }
        })
    }
}