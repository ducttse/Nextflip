let MediaObj;
let isEdited = false;
async function getRequestObjID(id) {
    let result = "";
    await fetch(`/api/MediaManagerManagement/GetDetailedMedia/${id}`)
        .then(res => res.json())
        .then(json => {
            console.log(json);
            MediaObj = json;
            setValue();
        });
    return result;
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


function setValue() {
    document.getElementById("title").value = MediaObj.mediaInfo.title;
    document.getElementById("director").value = MediaObj.mediaInfo.director;
    document.getElementById("cast").value = MediaObj.mediaInfo.cast;
    document.getElementById("publicYear").value = MediaObj.mediaInfo.publishYear;
    document.getElementById("duration").value = MediaObj.mediaInfo.duration;
    document.getElementById("language").value = MediaObj.mediaInfo.language;
    document.getElementById("description").value = MediaObj.mediaInfo.description;
    setChosenCategory();
    setChosenMediaType();
    document.getElementById("season_container").innerHTML = MediaObj.seasons.map(
        (season, seasonIndex) => {
            let episodes = season.episodes.map((episode, index) => {
                return renderEpisode(episode, seasonIndex, index)
            }).join("");
            let seasonRendered = renderSeason(season, seasonIndex).concat(episodes).concat("</div>");
            return seasonRendered;
        }
    ).join("");
}

function getUpdateValue() {
    MediaObj.mediaInfo.title = document.getElementById("title").value;
    MediaObj.mediaInfo.cast = document.getElementById("cast").value;
    MediaObj.mediaInfo.publishYear = document.getElementById("publicYear").value;
    MediaObj.mediaInfo.duration = document.getElementById("duration").value;
    MediaObj.mediaInfo.language = document.getElementById("language").value;
    MediaObj.mediaInfo.description = document.getElementById("description").value;
}

function chooseCategory(obj) {
    let id = obj.getAttribute("categoryID");
    let category = obj.querySelector("p").textContent;
    document.getElementById("category_holder").insertAdjacentHTML("afterbegin", `<p onclick="removeCategory(this)" class="mx-1 badge bg-primary category p-2" id="${id}">${category}</p>`)
    obj.classList.add("d-none");
}

function removeCategory(obj) {
    let id = obj.id;
    document.querySelector(`#category_${id}`).classList.remove("d-none");
    obj.remove();
}

function setChosenCategory() {
    MediaObj.categoryIDs.map(cate => document.querySelector(`#category_${cate}`).click());
}

function renderCategory(category) {
    return `<li id="category_${category.categoryID}" onclick="chooseCategory(this)" categoryID="${category.categoryID}"><p class="dropdown-item mb-0" >${category.name}</p></li>`;
}

function chooseCategory(obj) {
    let id = obj.getAttribute("categoryID");
    let category = obj.querySelector("p").textContent;
    document.getElementById("category_holder").insertAdjacentHTML("afterbegin", `<p onclick="removeCategory(this)" class="mx-1 badge bg-primary category p-2" id="${id}">${category}</p>`)
    obj.classList.add("d-none");
}

function removeCategory(obj) {
    let id = obj.id; SeasonInfo
    document.querySelector(`#category_${id}`).classList.remove("d-none");
    obj.remove();
}

function setChosenMediaType() {
    document.getElementById("filmType").value = MediaObj.mediaInfo.filmType;
}

function requestCategories() {
    fetch(`/api/ViewSubscribedUserDashboard/GetCategories`).then(res => res.json()).then(json => {
        let checkboxs = json.map(category => {
            return renderCategory(category);
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

function renderSeason(item, num) {
    console.log(num);
    return `
    <div class="d-flex flex-column mb-2" id="season_${item.seasonInfo.number}">
        <div class="d-flex">
            <p class="me-auto mb-0 align-self-center" id="season_title_${item.seasonInfo.number}">Season ${item.seasonInfo.number}: ${item.seasonInfo.title}</p>
            <div class="btn btn-primary btn-sm me-2" onclick="setEpisodeNumber('${item.seasonInfo.number}'); setModalAddEpisode('${item.episodes.length + 1}');" data-bs-toggle="modal" data-bs-target="#modalAddEpisodeForm">Add new episode</div>
            <div class="btn btn-danger btn-sm me-2" onclick="showConfirm('season', '${item.seasonInfo.number}')" data-bs-toggle="modal" data-bs-target="#confirmModal">Delete</div>
            <div class="btn btn-secondary btn-sm"  data-bs-toggle="modal" data-bs-target="#modalEditSeasonForm" onclick="setSeasonEditFormValue('${item.seasonInfo.number}')">Edit</div>
        </div>
    `
}

function renderEpisode(episode, seasonIndex, episodeIndex) {
    return `
    <div class="ps-3 mt-2 d-flex" id="episode_${episode.number}">
        <p class="mb-1 me-auto" id="episode_title_${episode.number}">Episode ${episode.number}: ${episode.title}</p>
        <div class="btn btn-danger btn-sm me-2" onclick="showConfirm('episode', '${episode.number}', '${seasonIndex}')" data-bs-toggle="modal" data-bs-target="#confirmModal">Delete</div>
        <div class="btn btn-secondary btn-sm" data-bs-toggle="modal" data-bs-target="#modalEditEpisodeForm")" onclick="setEpisodeFormValue('${seasonIndex}','${episodeIndex}')">Edit</div>
    </div>
    `
}

requestCategories();
requestMediaType();
const addSeason = debounce(() => setSeason());
async function setSeason() {
    let Season = {
        seasonInfo: {
            title: "",
            thumbnailURL: "",
            number: "",
        },
        episodes: []
    }
    document.getElementById("spinner").classList.remove("d-none");
    document.getElementById("submit_btn").disabled = true;
    Season.seasonInfo.title = document.getElementById("titleSeason").value;
    Season.seasonInfo.number = MediaObj.seasons.length + 1;
    getFile(document.getElementById("bannerSeason"));
    Season.seasonInfo.thumbnailURL = await requestUploadBanner();
    MediaObj.seasons.push(Season);
    document.getElementById("season_container").insertAdjacentHTML("beforeend", renderSeason(Season, MediaObj.seasons.length + 1));
    document.getElementById("spinner").classList.add("d-none");
    document.querySelector("#modalAddSeasonForm .btn-close").click();
    document.getElementById("submit_btn").disabled = false;
}

const addEpisode = debounce((obj) => {
    console.log(obj);
    setEpisode(obj)
});
async function setEpisode(obj) {
    console.log(obj);
    console.log(obj.getAttribute("index"));
    let index = parseInt(obj.getAttribute("index"));
    console.log(index);
    let episode = {
        title: "",
        thumbnailURL: "",
        episodeURL: "",
        number: "",
    }
    document.getElementById("spinner").classList.remove("d-none");
    document.getElementById("submit_btn").disabled = true;
    episode.title = document.getElementById("titleEpisode").value;
    episode.number = MediaObj.seasons[ index ].episodes.length + 1;
    getFile(document.getElementById("bannerEpisode"));
    episode.thumbnailURL = await requestUploadEpisodeThumbnail(MediaObj.seasons[ index ].seasonInfo.number, episode.number);
    getFile(document.getElementById("videoEpisode"));
    episode.episodeURL = await requestUploadVideo(MediaObj.seasons[ index ].seasonInfo.number, episode.number);
    MediaObj.seasons[ index ].episodes.push(episode);
    document.getElementById(`season_${index + 1}`).insertAdjacentHTML("beforeend", renderEpisode(episode, index));
    document.getElementById("spinner").classList.add("d-none");
    document.querySelector("#modalAddEpisodeForm .btn-close").click();
    document.getElementById("submit_btn").disabled = false;
}


function setModalAddEpisode(number) {
    document.getElementById("episode_submit_btn").setAttribute("index", number);
}

function setSeasonNumber() {
    document.getElementById("numberSeason").textContent = MediaObj.seasons.length + 1;
}

function setEpisodeNumber(index) {
    let num = parseInt(index) - 1;
    document.getElementById("numberEpisode").textContent = MediaObj.seasons[ num ].episodes.length + 1;
}

async function changeSeason(obj) {
    console.log(obj);
    let seasonIndex = parseInt(obj.getAttribute("seasonIndex"));
    let title = document.getElementById("editTitleSeason").value;
    let banner;
    let season = MediaObj.seasons[ seasonIndex - 1 ];
    if (updateFile(document.getElementById("editBannerSeason"))) {
        banner = await requestUploadSeasonBanner();
        season.seasonInfo.thumbnailURL = banner;
    };
    season.seasonInfo.title = title;
    document.getElementById(`season_title_${seasonIndex}`).textContent = `Season ${seasonIndex}: ${title}`;
    document.querySelector("#modalEditSeasonForm .btn-close").click();
}

async function changeEpisode(obj) {
    let seasonIndex = parseInt(obj.getAttribute("seasonIndex"));
    let episodeIndex = parseInt(obj.getAttribute("episodeIndex"));
    let episode = MediaObj.seasons[ seasonIndex ].episodes[ episodeIndex ];
    let title = document.getElementById("editTitleEpisode").value;
    let banner;
    if (updateFile(document.getElementById("editBannerEpisode"))) {
        banner = await requestUploadEpisodeThumbnail();
        episode.thumbnailURL = banner;
    }
    let video;
    if (updateFile(document.getElementById("editVideoEpisode"))) {
        video = await requestUploadVideo();
        episode.episodeURL = video;
    }
    episode.title = title;
    document.querySelector(`#season_${seasonIndex + 1} #episode_title_${episodeIndex + 1}`).textContent = `Episode ${episodeIndex + 1}: ${title}`;
    console.log(document.querySelector("#modalEditEpisodeForm btn-close"));
    document.querySelector("#modalEditEpisodeForm .btn-close").click();
}

function requestEditMedia() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(MediaObj)
    };
    fetch("/api/ViewEditorDashboard/EditMedia", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Success") {
                isEdited = true;
                location.replace("/EditorDashboard/Index")
            }
        })
}

function setSeasonEditFormValue(seasonIndex) {
    let index = parseInt(seasonIndex - 1);
    let season = MediaObj.seasons[ index ];
    console.log(season);
    document.getElementById("editTitleSeason").value = season.seasonInfo.title;
    document.getElementById("editNumberSeason").textContent = season.seasonInfo.number;
    document.getElementById("editSeasonSubmit_btn").setAttribute("seasonIndex", seasonIndex);
}

function setEpisodeFormValue(seasonIndex, episodeIndex) {
    let episode = MediaObj.seasons[ seasonIndex ].episodes[ episodeIndex ];
    console.log(episode);
    document.getElementById("editTitleEpisode").value = episode.title;
    document.getElementById("editNumberEpisode").textContent = episode.number;
    document.getElementById("editEpisode_submit_btn").setAttribute("seasonIndex", seasonIndex);
    document.getElementById("editEpisode_submit_btn").setAttribute("episodeIndex", episodeIndex);
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
function deleteSeason(number) {
    MediaObj.seasons = MediaObj.seasons.filter(season => season.seasonInfo.number != number);
    document.getElementById(`season_${number}`).parentNode.remove();
}

function deleteEpisode(number, index) {
    MediaObj.seasons[ index ].episodes = MediaObj.seasons[ index ].episodes.filter(episode => episode.number != number);
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

function requestEdit() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(MediaObj)
    };
    fetch("/api/ViewEditorDashboard/EditMedia", initObject).then(res => res.json()).then(json => {
        console.log(json);
    })
}