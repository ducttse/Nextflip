let requestAddNewEpisodeObj = {
    seasonID: "",
    title: "",
    ThumbnailURL: "",
    episodeURL: "",
    Number: "",
    UserEmail: ""
};
let modalAddEpisodeForm;
function showAddEpisodeForm() {
    if (modalAddEpisodeForm == null) {
        modalAddEpisodeForm = new bootstrap.Modal(document.getElementById('modalAddEpisodeForm'), {
            modalAddEpisodeForm: false
        })
    }
    modalAddEpisodeForm.show();
}

function showNoSeasonModal() {
    if (modalAddEpisodeForm == null) {
        modalAddEpisodeForm = new bootstrap.Modal(document.getElementById('NoSeasonModal'), {
            modalAddEpisodeForm: false
        })
    }
    modalAddEpisodeForm.show();
}

async function setResetRequestAddNewEpisodeObj() {
    requestAddNewEpisodeObj.title = document.getElementById("titleEpisode").value;
    requestAddNewEpisodeObj.seasonID = document.getElementById("seasonEpisode").value;
    requestAddNewEpisodeObj.Number = document.getElementById("numberEpisode").value;
    requestAddNewEpisodeObj.UserEmail = getProfile().userEmail;
    if (requestAddNewEpisodeObj.title.length > 0 && requestAddNewEpisodeObj.Number.length > 0) {
        getFile(document.getElementById("bannerEpisode"));
        requestAddNewEpisodeObj.ThumbnailURL = await requestUploadEpisodeThumbnail(requestAddNewEpisodeObj.seasonID, requestAddNewEpisodeObj.Number);
        getFile(document.getElementById("videoEpisode"));
        requestAddNewEpisodeObj.episodeURL = await requestUploadVideo(requestAddNewEpisodeObj.seasonID, requestAddNewEpisodeObj.Number);
    }
    return new Promise(resolve => resolve("resolve"));
}

function resetRequestAddNewEpisodeObj() {
    requestAddNewEpisodeObj.seasonID = "";
    requestAddNewEpisodeObj.Number = "";
    requestAddNewEpisodeObj.ThumbnailURL = "";
    requestAddNewEpisodeObj.episodeURL = "";
    requestAddNewEpisodeObj.title = "";
    let episode = document.getElementById("seasonEpisode");
    let number = document.getElementById("numberEpisode");
    let banner = document.getElementById("bannerEpisode");
    let video = document.getElementById("videoEpisode");
    let title = document.getElementById("titleEpisode");
    video.value = "";
    episode.innerHTML = "";
    number.value = "";
    banner.value = "";
    title.value = "";
    video.parentNode.classList.remove("was-validated");
    episode.parentNode.classList.remove("was-validated");
    number.parentNode.classList.remove("was-validated");
    banner.parentNode.classList.remove("was-validated");
    title.parentNode.classList.remove("was-validated");
}

async function getSeasons(id) {
    fetch(`/api/ViewMediaDetails/GetSeasons/${id}`)
        .then(res => res.json())
        .then(json => {
            console.log(json);
            if (json.length > 0) {
                let options = json.map(season => {
                    return `<option value="${season.seasonID}">${season.title}</option>`
                }).join("");
                document.getElementById("seasonEpisode").insertAdjacentHTML("beforeend", options);
                showAddEpisodeForm();
            }
            else {
                showNoSeasonModal();
            }
        })
}

async function validateAddEpisodeFormInput() {
    checkEmpty(document.getElementById("titleEpisode"));
    checkEmpty(document.getElementById("numberEpisode"));
    getFile(document.getElementById("bannerEpisode"));
    getFile(document.getElementById("videoEpisode"));
}

async function requestAddNewEpisode() {
    await setResetRequestAddNewEpisodeObj();
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestAddNewEpisodeObj)
    };
    fetch("/api/ViewEditorDashboard/AddEpisode/", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Success") {
                hideModalWithName("modalAddEpisodeForm");
                changeContent("Add success", true);
                messageModal.show();
            }
            else {
                validateAddEpisodeFormInput();
            }
        })
}

document.getElementById("modalAddEpisodeForm").addEventListener("hidden.bs.modal", () => {
    resetRequestAddNewEpisodeObj();
})