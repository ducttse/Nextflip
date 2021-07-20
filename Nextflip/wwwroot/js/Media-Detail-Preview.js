let MediaObj;
async function getRequestObjID(id) {
    let result = "";
    await fetch(`/api/MediaManagerManagement/GetDetailedMedia/${id}`)
        .then(res => res.json())
        .then(json => {
            MediaObj = json;
            document.getElementById("content_holder").insertAdjacentHTML("afterbegin", renderMedia(json));
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

function renderMedia(media) {
    let category = media.categoryIDs.map(cate => cate.name).join(", ");
    let info = media.mediaInfo;
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
        <p>Category: ${category}</p>
        <p>Language: ${info.language}</p>
        <p>Desscription: ${info.description}</p>
    </div>`
}

function setSubmit(id, type) {
    document.getElementById("submit_btn").setAttribute("id", id);
    document.getElementById("submit_btn").setAttribute("type", type);
}


function renderCollapseTrigger(season) {
    let seasonInfo = season.seasonInfo;
    let action = `
            <button class="btn btn-primary me-1 btn-sm" onclick="approve('${seasonInfo.seasonID}','season')">
                Approve
            </button>
            <button class="btn btn-danger btn-sm" data-bs-toggle="modal" data-bs-target="#noteModal" onclick="setSubmit("${seasonInfo.seasonID}", 'season')">
                Disapprove
            </button>`;
    let status = seasonInfo.status == "Pending" ? ("bg-warning") : (seasonInfo.status == "Approved" ? ("bg-primary") : (seasonInfo.status == "Disapproved" ? "bg-danger" : "bg-light"));
    return `
        <p class="d-flex">
            <button class="btn btn-light position-relative me-auto" type="button" data-bs-toggle="collapse" data-bs-target="#season_${seasonInfo.number}" aria-expanded="false" aria-controls="collapseExample">
                Season ${seasonInfo.number}: ${seasonInfo.title}
                <span class="position-absolute top-0 start-100 translate-middle p-2 ${status} border border-light rounded-circle">
                    <span class="visually-hidden">New alerts</span>
                </span>
            </button>
            <button class="btn btn-secondary me-1 btn-sm" onclick="viewDetail('${seasonInfo.seasonID}')">
                View detail
            </button>
            ${seasonInfo.status == "Pending" ? action : ""}
        </p>`;
}

function renderCollapse(season) {
    let seasonEL = renderCollapseTrigger(season);
    let episodes = season.episodes.map(episode => {
        let status = episode.status == "Pending" ? ("bg-warning") : (episode.status == "Approved" ? ("bg-primary") : (episode.status == "Disapproved" ? "bg-danger" : "bg-light"));
        let action = `
            <button class="btn btn-primary me-1 btn-sm" onclick="approve('${episode.episodeID}','episode')">
                Approve
            </button>
            <button class="btn btn-danger btn-sm" data-bs-toggle="modal" data-bs-target="#noteModal" onclick="setSubmit("${episode.episodeID}", 'episode')">
                Disapprove
            </button>`;
        return `
        <div class="d-flex">
            <p class="me-auto position-relative btn mb-0">Episode ${episode.number}: ${episode.title}
                <span class="position-absolute top-0 start-100 translate-middle p-2 ${status} border border-light rounded-circle">
                    <span class="visually-hidden">New alerts</span>
                </span>
            </p>
            <a class="btn btn-secondary me-1 btn-sm text-center py-2" href="/MediaManagerManagement/PreviewEpisode/${episode.episodeID}">
                View detail
            </a>
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
            RequestID: id.trim(),
            Type: type.trim()
        })
    };
    fetch(`/api/MediaManagerManagement/ApproveRequest`, initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Success") {
                changeContent("Success", true);
            }
            else {
                if (type == "season") {
                    changeContent("You must change all pending episode in this season", false);
                }
                else {
                    changeContent("Something went wrong", false);
                }
            }
            showModalFlash();
        })
}

function disapprove(obj) {
    let id = obj.getAttribute("id");
    let type = obj.getAttribute("type");
    let note = document.getElementById("note_input").value;
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            RequestID: id.trim(),
            Type: type.trim(),
            Content: note
        })
    };
    fetch(`/api/MediaManagerManagement/ApproveRequest`, initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Success") {
                changeContent("Success", true);
                showModalFlash();
            }
            else {
                if (type == "season") {
                    changeContent("You must change all pending episode in this season", false);
                }
                else {
                    changeContent("Something went wrong", false);
                }
            }
        })
}

function publish() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            RequestID: MediaObj.mediaInfo.mediaID,
            Type: "media"
        })
    };
    fetch(`/api/MediaManagerManagement/DisapproveRequest/`, initObject)
    fetch(`/api/MediaManagerManagement/ApproveRequest`, initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Success") {
                changeContent("Publish success", true);
                showModalFlash();
            }
        })
}

function changeContent(text, bool) {
    let content = !bool ?
        ` <i class="far fa-times-circle fa-5x text-danger text-center"></i>
            <p class="fs-5 text-center text-dark">${text}</p>
            <button type="button" class="col-4 mx-auto btn btn-danger text-white" data-bs-dismiss="modal">
                Close
            </button>` :
        ` <i class="far fa-check-circle fa-5x text-center" style="color: #4bca81"></i>
            <p class="fs-5 text-center text-dark">${text}</p>
            <button type="button" class="col-4 mx-auto btn btn-success text-white"  style=" background-color: #4bca81 !important; border: #4bca81 !important;" data-bs-dismiss="modal">
                Continue
            </button>`;
    document.querySelector("#modal_flash .modal-body").innerHTML = content;
}

let modalFlash;
function showModalFlash() {
    if (modalFlash == null) {
        modalFlash = new bootstrap.Modal(document.getElementById('modal_flash'), {
            keyboard: false
        })
    }
    modalFlash.show();
}