let requestAddNewSeasonObj = {
    MediaID: "",
    title: "",
    ThumbnailURL: "",
    Number: "",
    UserEmail: ""
};
let modalAddSeasonForm;
function showAddSeasonForm(id) {
    requestAddNewSeasonObj.MediaID = id;
    if (modalAddSeasonForm == null) {
        modalAddSeasonForm = new bootstrap.Modal(document.getElementById('modalAddSeasonForm'), {
            modalAddSeasonForm: false
        })
    }
    modalAddSeasonForm.show();
}

async function setRequestAddNewSeasonObj() {
    requestAddNewSeasonObj.ThumbnailURL = await requestUploadSeasonBanner();
    requestAddNewSeasonObj.title = document.getElementById("titleSeason").value;
    requestAddNewSeasonObj.Number = document.getElementById("numberSeason").value;
    requestAddNewSeasonObj.UserEmail = getProfile().userEmail;
}

function resetRequestAddNewSeasonObj() {
    requestAddNewSeasonObj.ThumbnailURL = "";
    requestAddNewSeasonObj.Number = "";
    requestAddNewSeasonObj.title = "";
    let title = document.getElementById("titleSeason");
    let season = document.getElementById("numberSeason");
    let file = document.getElementById("bannerSeason");
    title.value = "";
    season.value = "";
    file.value = "";
    title.parentNode.classList.remove("was-validated");
    season.parentNode.classList.remove("was-validated");
    file.parentNode.classList.remove("was-validated");
}

function validateCheckBox() {
    document.getElementById("")
}

async function validateInput() {
    checkEmpty(document.getElementById("titleSeason"));
    getFile(document.getElementById("bannerSeason"));
    checkEmpty(document.getElementById("numberSeason"));
}

async function requestAddNewSeason() {
    await setRequestAddNewSeasonObj();
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestAddNewSeasonObj)
    };
    fetch("/api/ViewEditorDashboard/AddSeason", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Success") {
                hideModalWithName("modalAddSeasonForm");
                changeContent("Add success", true);
                messageModal.show();
            }
            else {
                validateInput();
            }
        })
}

document.getElementById("modalAddSeasonForm").addEventListener("hidden.bs.modal", () => {
    resetRequestAddNewSeasonObj();
})