let MediaObj;
async function getRequestObjID(id) {
    let result = "";
    await fetch(`/api/MediaManagerManagement/GetDetailedMedia/${id}`)
        .then(res => res.json())
        .then(json => {
            MediaObj = json;
            document.getElementById("content_holder").insertAdjacentHTML("afterbegin", renderMedia(json.mediaInfo));
            let seasons = json.seasons.map(season => {
                return renderCollapse(season);
            }).join("");
            document.getElementById("season_holder").insertAdjacentHTML("afterbegin", seasons);
        });
    return result;
}

async function getRequestDetail(mediaID) {
    getRequestObjID(mediaID);
}

function renderMedia(info) {
    // let category = categories.map(cate => cate.name).join(", ");
    // TODO  add category
    //<p>Category: ${category}</p>
    return `
    <div class="col-5 me-2">
        <img class="img-fluid" src="${info.bannerURL}" width="500" height="500"/>
    </div>
    <div class="col-6">
        <p class="h4">${info.title}</p>
        <p>Film type: ${info.filmType}</p>
        <p>Dicrector: ${info.director}</p>
        <p>Cast: ${info.cast}</p>
        <p>Public year: ${info.publishYear}</p>
        <p>Duration: ${info.duration}</p>
        <p>Language: ${info.language}</p>
        <p>Desscription: ${info.description}</p>
    </div>`
}

function renderCollapseTrigger(seasonInfo) {
    let action = `
            <button class="btn btn-secondary me-1" onclick="viewDetail('${seasonInfo.seasonID}')">
                View detail
            </button>
            <button class="btn btn-primary me-1">
                Approve
            </button>
            <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#noteModal">
                Disapprove
            </button>`;
    return `
        <p class="d-flex">
            <button class="btn btn-light me-auto" type="button" data-bs-toggle="collapse" data-bs-target="#season_${seasonInfo.number}" aria-expanded="false" aria-controls="collapseExample">
                Season ${seasonInfo.number}: ${seasonInfo.title}
            </button>
            ${seasonInfo.status == "Pending" ? action : ""}
        </p>`;
}

function renderCollapse(season) {
    let seasonEL = renderCollapseTrigger(season.seasonInfo);
    let episodes = season.episodes.map(episode => {
        let action = `
            <a class="btn btn-secondary me-1" href="/MediaManagerManagement/PreviewEpisode/${episode.episodeID}">
                View detail
            </a>
            <button class="btn btn-primary me-1" onclick="approve('${episode.episodeID}', "episode")">
                Approve
            </button>
            <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#noteModal">
                Disapprove
            </button>`;
        return `
        <div class="d-flex">
            <p class="me-auto">Episode ${episode.number}: ${episode.title}</p>
            ${episode.status == "Pending" ? action : ""}
        </div>`
    }).join("");
    return `
    <div>
        ${seasonEL}
        <div class="ps-5 collapse" id="season_${season.seasonInfo.number}">
            ${episodes}
        </div>
    </div>`;
}

let seasonDetailModal;

function showSeasonDetailModal() {
    if (seasonDetailModal == null) {
        seasonDetailModal = new bootstrap.Modal(document.getElementById('season_detail_modal'), {
            keyboard: false
        })
    }
    seasonDetailModal.show();
}

function renderSeasonDetail(season) {
    return `
    <div class="col-10 mx-auto">
        <img class="img-fluid" src="${season.thumbnailURL}" width="500" height="500"/>
    </div>
    <div class="col-6 ps-5 pt-2">
        <p>Title: ${season.title}</p>
        <p>Number: ${season.number}</p>
    </div>`
        ;
}
function viewDetail(id) {
    fetch(`/api/ViewMediaDetails/GetSeasonByID/${id}`)
        .then(res => res.json())
        .then(json => {
            console.log(json)
            document.getElementById("season_detail_modal").querySelector(".modal-body").innerHTML = renderSeasonDetail(json);
            showSeasonDetailModal();
        })
}

function approve(id, type) {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            RequestID: id,
            Type: type
        })
    };
    fetch(`/api/MediaManagerManagement/ApproveRequest`, initObject)
        .then(res => res.json())
        .then(json => {
            console.log(json);
        })
}

function disapprove(id, type) {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            RequestID: id,
            Type: type
        })
    };
    fetch(`/api/MediaManagerManagement/ApproveRequest`, initObject)
        .then(res => res.json())
        .then(json => {
            console.log(json);
        })
}