let videoData;
function renderMedia(media) {
    return `
    <video
      id="video"
      class="video-js vjs-default-skin vjs-big-play-centered w-100 h-100"
      controls preload="auto" controlsList="nodownload"
      poster="${media.thumbnailURL}"
      data-setup='{ "aspectRatio":"16:9", "playbackRates": [1, 1.5, 2] }'>
      <source src="${media.episodeURL}" type="video/mp4" />
    </video>
    `;
}

function requestEpisode(id) {
    fetch(`/api/ViewMediaDetails/GetEpisodeByID/${id}`)
        .then(res => res.json())
        .then(json => {
            videoData = json;
            document.getElementById("media_holder").innerHTML = renderMedia(json);
            document.getElementById("title").innerText = `Title: ${json.title}`;
            document.getElementById("number").innerText = `Number: ${json.number}`;
            if (json.status == "Pending") {
                document.getElementById("btn_group").classList.remove("d-none");
            }
        })
}

function isFinishReview() {
    return trackWatchTime();
}

function requestApprove(id) {
    if (!isFinishReview()) {
        document.getElementById("flash_trigger").click();
        return;
    }
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            RequestID: id,
            Type: "episode"
        })
    };
    fetch("/api/MediaManagerManagement/ApproveRequest", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Success") {
                location.replace("/MediaManagerManagement/Index");
            }

        })
}

function requestDisapprove(id) {
    if (!isFinishReview()) return;
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            RequestID: id,
            Type: "episode",
            Content: document.getElementById("note_input").value
        })
    };
    fetch("/api/MediaManagerManagement/DisapproveRequest", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Success") {
                location.replace("/MediaManagerManagement/Index");
            }
        })
}