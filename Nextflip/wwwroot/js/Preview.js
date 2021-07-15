let requestMediaID;

async function getRequestObjID(id) {
    let result = "";
    await fetch(`/api/MediaManagerManagement/GetMediaEditRequestByID/${id}`)
        .then(res => res.json())
        .then(json => {
            document.getElementById("note").innerHTML = "<b>Note: </b>" + json.data.note;
            result = json.data.id;
        });
    return result;
}

async function getRequestDetail(type, mediaID, requestID) {
    let ObjID = await getRequestObjID(requestID);
    switch (type) {
        case "media":
            getMediaDetail(ObjID);
            break;
        case "season":
            getSeasonDetail(ObjID);
            break;
        case "episode":
            getEpisodeDetail(ObjID);
            break;
    }
}

function renderMedia(media, categories) {
    let category = categories.map(cate => cate.name).join(", ");
    return `
    <div class="col-5 me-2">
        <img class="img-fluid" src="${media.bannerURL}" width="500" height="500"/>
    </div>
    <div class="col-6">
        <p class="h4">${media.title}</p>
        <p>Category: ${category}</p>
        <p>Film type: ${media.filmType}</p>
        <p>Dicrector: ${media.director}</p>
        <p>Cast: ${media.cast}</p>
        <p>Public year: ${media.publishYear}</p>
        <p>Duration: ${media.duration}</p>
        <p>Language: ${media.language}</p>
        <p>Desscription: ${media.description}</p>
    </div>`
}

function renderSeason(season) {
    return `
    <div class="col-5 me-2">
        <p class="h2 text-center">New season thumbnail</p>
        <img class="img-fluid" src="${season.thumbnailURL}" width="500" height="500"/>
    </div>
    <div class="col-6 mt-5">
        <p class="fs-5">Season name: ${season.title}</p>
        <p class="fs-5">Season number: ${season.number}</p>
    </div>`;
}

function renderEpisode(episode) {
    return `
    <div class="col-5 me-2">
        <p class="h2 text-center">New episode thumbnail</p>
        <img class="img-fluid" src="${episode.thumbnailURL}" width="500" height="500"/>
    </div>
    <div class="col-6 mt-5">
        <p class="fs-5">Episode name: ${episode.title}</p>
        <p class="fs-5">Episode number: ${episode.number}</p>
        <a href="${episode.episodeURL}">Link</a>
    </div>`;
}

function getMediaDetail(mediaID) {
    fetch(`/api/ViewMediaDetails/GetMediaDetails/${mediaID}`)
        .then(res => res.json())
        .then(json => {
            document.getElementById("content_holder").insertAdjacentHTML("afterbegin", renderMedia(json.media, json.categories));
        })
}

function getSeasonDetail(seasonID) {
    fetch(`/api/ViewMediaDetails/GetSeasonByID/${seasonID}`)
        .then(res => res.json())
        .then(json => {
            console.log(json);
            document.getElementById("content_holder").insertAdjacentHTML("afterbegin", renderSeason(json));
        })
}

function getEpisodeDetail(episodeID) {
    fetch(`/api/ViewMediaDetails/GetEpisodeByID/${episodeID}`)
        .then(res => res.json())
        .then(json => {
            console.log(json);
            document.getElementById("content_holder").insertAdjacentHTML("afterbegin", renderSeason(json));
        })
}

function setRequestMediaID(id) {
    requestMediaID = id
}

function requestApprove() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            RequestID: requestMediaID
        })
    };
    fetch("/api/MediaManagerManagement/ApproveRequest/", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "success") {
                window.location.replace("/MediaManagerManagement/Index");
            }
            else {
                window.location.reload();
            }
        })
}

function requestDisapprove() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            RequestID: requestMediaID,
            Note: document.getElementById("note_input").value
        })
    };
    fetch("/api/MediaManagerManagement/DisapproveRequest/", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "success") {
                window.location.replace("/MediaManagerManagement/Index");
            } else {
                window.location.reload();
            }
        })
}