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
            document.getElementById("media_holder").innerHTML = renderMedia(json);
        })
}

function isFinishReview() {
    // TODO caculate total time and current time 
    // save to storage
}