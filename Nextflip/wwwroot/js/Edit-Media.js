let MediaObj;
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
        (season) => {
            let episodes = season.episodes.map(episode => {
                return renderEpisode(episode, '1')
            }).join("");
            let seasonRendered = renderSeason(season).concat(episodes).concat("</div>");
            return seasonRendered;
        }
    ).join("");
}

function setChosenCategory() {
    return Array.from(document.querySelectorAll('input[type="checkbox"].category'))
        .filter(cate => MediaObj.categoryIDs.includes(parseInt(cate.value)))
        .map(cate => cate.setAttribute("checked", true));
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
    console.log(item);
    return `
    <div class="d-flex flex-column mb-2" id="season_${item.seasonInfo.number}">
        <div class="d-flex">
            <p class="me-auto mb-0 align-self-center">Season ${item.seasonInfo.number}: ${item.seasonInfo.title}</p>
            <div class="btn btn-primary btn-sm me-2" onclick="setEpisodeNumber('${item.seasonInfo.number}'); setModalAddEpisode('${num}');" data-bs-toggle="modal" data-bs-target="#modalAddEpisodeForm">Add new episode</div>
            <div class="btn btn-danger btn-sm" onclick="showConfirm('season', '${item.seasonInfo.number}')" data-bs-toggle="modal" data-bs-target="#confirmModal">Delete</div>
        </div>
    `
}

function renderEpisode(episode, index) {
    return `
    <div class="ps-3 mt-2 d-flex" id="episode_${episode.number}">
        <p class="mb-1 me-auto">Episode ${episode.number}: ${episode.title}</p>
        <div class="btn btn-danger btn-sm" onclick="showConfirm('episode', '${episode.number}', '${index}')" data-bs-toggle="modal" data-bs-target="#confirmModal">Delete</div>
    </div>
    `
}
requestCategories();
requestMediaType();