let MediaInfo = {
    Title: "",
    FilmType: "",
    Director: "",
    Cast: "",
    PublishYear: "",
    Duration: "",
    BannerURL: "",
    Language: "",
    Description: ""
}

let Media = {
    MediaInfo: MediaInfo,
    Seasons: [],
    CategoryIDs: []
}

function getChosenCategory() {
    return Array.from(document.querySelectorAll('input[type="checkbox"].category'))
        .filter(cate => cate.checked)
        .map(cate => cate.value);
}

function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}

const onInput = debounce((obj) => {
    let parent = obj.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    parent.classList.add("was-validated");
    if (obj.value.trim().length == 0) {
        feedback.textContent = "This field can not empty";
        obj.setCustomValidity("empty");
    }
    else {
        parent.classList.remove("was-validated");
        feedback.textContent = "";
        obj.setCustomValidity("");
    }
});

function checkEmpty(obj) {
    let parent = obj.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    parent.classList.add("was-validated");
    if (obj.value.trim().length == 0) {
        feedback.textContent = "This field can not empty";
        obj.setCustomValidity("empty");
        return false;
    }
    else {
        parent.classList.remove("was-validated");
        feedback.textContent = "";
        obj.setCustomValidity("");
        return true;
    }
}

async function setMediaInfo() {
    MediaInfo.Title = document.getElementById("title").value;
    MediaInfo.FilmType = document.getElementById("filmType").value;
    MediaInfo.Director = document.getElementById("director").value;
    MediaInfo.Cast = document.getElementById("cast").value;
    MediaInfo.PublishYear = document.getElementById("publicYear").value;
    MediaInfo.Duration = document.getElementById("duration").value;
    getFile(document.getElementById("banner"));
    MediaInfo.BannerURL = await requestUploadBanner();
    MediaInfo.Language = document.getElementById("language").value;
    MediaInfo.Description = document.getElementById("description").value;
    Media.CategoryIDs = getChosenCategory();
    return new Promise(resolve => resolve("resolved"));
}


const addSeason = debounce(() => setSeason());

function setSeasonNumber() {
    document.getElementById("numberSeason").textContent = Media.Seasons.length + 1;
}

function setEpisodeNumber(index) {
    let num = parseInt(index) - 1;
    document.getElementById("numberEpisode").textContent = Media.Seasons[ num ].Episodes.length + 1;
}

async function setSeason() {
    if (validateAddNewSeason() == 0) {
        return;
    }
    let Season = {
        SeasonInfo: {
            Title: "",
            ThumbnailURL: "",
            Number: "",
        },
        Episodes: []
    }
    document.getElementById("spinner").classList.remove("d-none");
    document.getElementById("submit_btn").disabled = true;
    Season.SeasonInfo.Title = document.getElementById("titleSeason").value;
    getFile(document.getElementById("bannerSeason"));
    Season.SeasonInfo.ThumbnailURL = await requestUploadBanner();
    Season.SeasonInfo.Number = Media.Seasons.length + 1;
    Media.Seasons.push(Season);
    document.getElementById("season_container").insertAdjacentHTML("beforeend", renderSeason(Season, Media.Seasons.length - 1));
    document.getElementById("spinner").classList.add("d-none");
    document.querySelector("#modalAddSeasonForm .btn-close").click();
    document.getElementById("submit_btn").disabled = false;
}

const addEpisode = debounce((obj) => setEpisode(obj));

async function setEpisode(obj) {
    if (validateAddNewEpisode() == 0) {
        return;
    }
    let index = parseInt(obj.getAttribute("index"));
    let Episode = {
        Title: "",
        ThumbnailURL: "",
        EpisodeURL: "",
        Number: "",
    }
    document.getElementById("spinner").classList.remove("d-none");
    document.getElementById("episode_submit_btn").disabled = true;
    Episode.Title = document.getElementById("titleEpisode").value;
    let EpisodeNumber = Media.Seasons[ index ].Episodes.length + 1;
    Episode.Number = EpisodeNumber;
    getFile(document.getElementById("bannerEpisode"));
    Episode.ThumbnailURL = await requestUploadEpisodeThumbnail(Media.Seasons[ index ].SeasonInfo.Number, Episode.Number);
    getFile(document.getElementById("videoEpisode"));
    Episode.EpisodeURL = await requestUploadVideo(Media.Seasons[ index ].SeasonInfo.Number, Episode.Number);
    Media.Seasons[ index ].Episodes.push(Episode);
    document.getElementById("season_container").insertAdjacentHTML("beforeend", renderEpisode(Episode, index));
    document.getElementById("spinner").classList.add("d-none");
    document.querySelector("#modalAddEpisodeForm .btn-close").click();
    document.getElementById("episode_submit_btn").disabled = false;
}

function deleteSeason(number) {
    Media.Seasons = Media.Seasons.filter(season => season.SeasonInfo.Number != number);
    document.getElementById(`season_${number}`).parentNode.remove();
}

function deleteEpisode(number, index) {
    Media.Seasons[ index ].Episodes = Media.Seasons[ index ].Episodes.filter(episode => episode.Number != number);
    document.getElementById(`episode_${number}`).remove();
}

function deleteItem(obj) {
    let type = obj.getAttribute("type");
    let number = obj.getAttribute("number");
    switch (type) {
        case "episode":
            deleteEpisode(number, parseInt(obj.getAttribute("seasonIndex")));
            break;
        case "season":
            deleteSeason(number);
            break;
    }
    if (confirmModal == null) {
        confirmModal = new bootstrap.Modal(document.getElementById('confirmModal'), {
            keyboard: false
        })
    }
    document.getElementById("hide_confirm").click();
}

let confirmModal;
function showConfirm(type, number, index) {
    if (confirmModal == null) {
        confirmModal = new bootstrap.Modal(document.getElementById('confirmModal'), {
            keyboard: false
        })
    }
    let confirmBtn = document.getElementById("confirm_btn");
    confirmBtn.setAttribute("number", number);
    switch (type) {
        case "episode":
            confirmBtn.setAttribute("type", "episode");
            confirmBtn.setAttribute("seasonIndex", index);
            break;
        case "season":
            confirmBtn.setAttribute("type", "season");
            break;
    }
}

function renderEpisode(episode, index) {
    return `
    <div class="ps-3 mt-2 d-flex" id="episode_${episode.Number}">
        <p class="mb-1 me-4">Episode ${episode.Number}: ${episode.Title}</p>
        <div class="btn btn-danger btn-sm" onclick="showConfirm('episode', '${episode.Number}', '${index}')" data-bs-toggle="modal" data-bs-target="#confirmModal">Delete</div>
    </div>
    `
}

function setModalAddEpisode(number) {
    document.getElementById("episode_submit_btn").setAttribute("index", number);
}

function renderSeason(item, num) {
    return `
        <div class="d-flex" id="season_${item.SeasonInfo.Number}">
            <p class="me-4 mb-0 align-self-center">Season ${item.SeasonInfo.Number}: ${item.SeasonInfo.Title}</p>
            <div class="btn btn-primary btn-sm me-2" onclick="setEpisodeNumber('${item.SeasonInfo.Number}'); setModalAddEpisode('${num}');" data-bs-toggle="modal" data-bs-target="#modalAddEpisodeForm">Add new episode</div>
            <div class="btn btn-danger btn-sm" onclick="showConfirm('season', '${item.SeasonInfo.Number}')" data-bs-toggle="modal" data-bs-target="#confirmModal">Delete</div>
        </div>
    `
}

function renderCategoryCheckBox(category) {
    return `
        <div class="form-check form-check-inline">
            <input class="form-check-input category" type="checkbox" id="inlineCheckbox1" value="${category.categoryID}">
            <label class="form-check-label text-capitalize" for="inlineCheckbox1">${category.name}</label>
        </div>
    `;
}

function requestCategories() {
    fetch(`/api/ViewSubscribedUserDashboard/GetCategories`).then(res => res.json()).then(json => {
        let checkboxs = json.map(category => {
            return renderCategoryCheckBox(category);
        }).join("");
        document.getElementById("CB_holder").insertAdjacentHTML("afterbegin", checkboxs);
    })
}

function requestMediaType() {
    fetch(`/api/FilmTypeManagement/GetAllFilmTypes`).then(res => res.json()).then(json => {
        let selectEl = json.map(option => {
            return `<option value="${option.type}">${option.type}</option>`
        }).join("");
        document.getElementById("filmType").insertAdjacentHTML("afterbegin", selectEl);
    })
}

const addNewMedia = debounce(() => requestAddNewMedia());
async function requestAddNewMedia() {
    await setMediaInfo();
    if (validateInputAddMedia() == 0) {
        return;
    }
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(Media)
    };
    fetch("/api/ViewEditorDashboard/AddNewMedia", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Success") {
                location.replace("/EditorDashboard/Index")
            }
        })
}

function validateCheckBoxValue() {
    if (document.querySelectorAll(`.category[type="checkbox"]:checked`).length == 0) {
        document.getElementById("empty_checkbox").classList.remove("d-none");
        return false;
    }
    else {
        document.getElementById("empty_checkbox").classList.add("d-none");
        return true;
    }
}

function validateInputAddMedia() {
    return checkEmpty(document.getElementById("title")) &
        checkEmpty(document.getElementById("language")) &
        checkEmpty(document.getElementById("description")) &
        getFile(document.getElementById("banner")) &
        validateCheckBoxValue();
}

function validateAddNewSeason() {
    return checkEmpty(document.getElementById("titleSeason")) &
        getFile(document.getElementById("bannerSeason"));
}

function validateAddNewEpisode() {
    return checkEmpty(document.getElementById("titleEpisode")) &
        getFile(document.getElementById("bannerEpisode")) &
        getFile(document.getElementById("videoEpisode"));
}

requestCategories();
requestMediaType();

document.getElementById("modalAddSeasonForm").addEventListener("hidden.bs.modal", () => {
    let title = document.getElementById("titleSeason");
    let banner = document.getElementById("bannerSeason");
    banner.value = "";
    title.value = "";
    banner.parentNode.classList.remove("was-validated");
    title.parentNode.classList.remove("was-validated");
})

document.getElementById("modalAddEpisodeForm").addEventListener("hidden.bs.modal", () => {
    let title = document.getElementById("titleEpisode");
    let banner = document.getElementById("bannerEpisode");
    let video = document.getElementById("videoEpisode");
    title.value = "";
    banner.value = "";
    video.value = "";
    title.parentNode.classList.remove("was-validated");
    banner.parentNode.classList.remove("was-validated");
    video.parentNode.classList.remove("was-validated");
})