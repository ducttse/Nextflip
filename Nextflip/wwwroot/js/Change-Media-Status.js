let ChosenCheckBox;
let isSuccess = false;
let isRequest = false;
let RequestOBj = {
    UserEmail: "vlxx.com@gmail.com",
    ID: "",
    Note: "",
    LinkPreview: "N/A",
    Status: "Disabled",
    Type: "media"
}
function setChosenCheckBox(item) {
    ChosenCheckBox = item;
}

function rollBack() {
    ChosenCheckBox.checked ? ChosenCheckBox.checked = false : ChosenCheckBox.checked = true;
}

let currentModal;

function hideModal() {
    currentModal.hide();
}

function hideModalWithName(name) {
    var myModal = new bootstrap.Modal(document.getElementById(name), {
        keyboard: false
    })
    myModal.hide();
}

function showModal(modalName) {
    var myModal = new bootstrap.Modal(document.getElementById(modalName), {
        keyboard: false
    })
    myModal.show();
    currentModal = myModal;
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

function convertToFormData(obj) {
    let formData = new FormData();
    for (let [ key, value ] of Object.entries(obj)) {
        formData.append(key, value);
    }
    return formData;
}

async function requestChangeStatus() {
    let FormData = convertToFormData(RequestOBj);
    let initObject = {
        method: "POST",
        body: FormData
    };
    await fetch("/api/ViewEditorDashboard/RequestChangeMediaStatus", initObject)
        .then(res => res.json())
        .then(json => {
            console.log(json);
            isRequest = true;
            if (json.message != null && json.message == "success") {
                appendFlashMessageContent(true);
                isSuccess = true;
            }
            else {
                appendFlashMessageContent(false)
            }
        })
    return new Promise(resolve => {
        resolve("resolved");
    })
}

function setRequestEventToTigger() {
    document.getElementById("submit_btn").addEventListener("click", () => {
        console.log("requets change disable");
        RequestOBj.Note = document.getElementById("note").value;
        requestChangeStatus().then(() => {
            hideModal();
            showModal("modal_flash");
        })
    })
    document.getElementById("confirm_btn").addEventListener("click", () => {
        console.log("requets change  able");
        requestChangeStatus().then(() => {
            hideModal();
            showModal("modal_flash");
        })
    })
    document.getElementById("modal_flash").addEventListener("hide.bs.modal", () => {
        isRequest = false;
        isSuccess = false;
        if (isSearched) {
            search(requestParam.SearchValue);
        }
        else if (isFiltered) {
            requestWithFilter();
        }
        else {
            requestUserData();
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
setRequestEventToTigger();
function addSetStatusEvent() {
    let collection = document.getElementsByClassName("status_btn");
    for (let i = 0; i < collection.length; i++) {
        let item = collection[ i ];
        item.addEventListener("change", (evt) => {
            setChosenCheckBox(evt.target);
            RequestOBj.Status = evt.target.value == "Enabled" ? "Disabled" : "Enabled";
            RequestOBj.ID = evt.target.getAttribute("mediaID");
            if (item.checked) {
                showModal("modalCheck");
            }
            else {
                showModal("modalForm");
            }
        })
    }
}