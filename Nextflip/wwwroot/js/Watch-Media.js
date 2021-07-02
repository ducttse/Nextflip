function renderMedia(media) {
  return `
  <video
    id="video"
    class="video-js vjs-default-skin vjs-big-play-centered w-100 h-100"
    controls preload="auto" controlsList="nodownload"
    data-setup='{ "aspectRatio":"16:9", "playbackRates": [1, 1.5, 2] }'>
    <source src="${media.episodeURL}" type="video/mp4" />
  </video>
  `;
}

function appendMedia(data) {
  document
    .getElementById("wrapper")
    .insertAdjacentHTML("afterbegin", renderMedia(data.episode));
  // document.getElementById("name").innerHTML = data.media.title;
}

function Run(mediaID, episodeID) {
  fetch(`/api/ViewMediaDetails/GetEpisode/${mediaID}/${episodeID}`)
    .then((response) => response.json())
    .then((json) => {
      console.log(json);
      appendMedia(json);
    })
}
