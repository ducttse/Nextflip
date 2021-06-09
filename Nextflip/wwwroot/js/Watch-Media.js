function renderMedia(media) {
  return `
  <video
    id="video"
    class="video-js vjs-default-skin vjs-big-play-centered"
    data-setup="{}"
    controls
    preload="none"
    data-setup='{ "aspectRatio":"16:9", "playbackRates": [1, 1.5, 2] }'
  >
    <source src="${media.episodeURL}" type="video/mp4" />
  </video>
  `;
}

function renderName(media) {
  return `
  <div id="name" class="fixed-top mt-2 ml-5" style="color: white">
    <div class="row">
      <i id="icon" class="fas fa-arrow-left h5 pt-1"></i>
      <!-- put video name -->
      <p class="h4 ml-2">Video name</p>
    </div>
    <div class="ml-3">
      <!-- put episode name -->
      <p>Episode</p>
    </div>
  </div>`;
}

function appendMedia(media) {
  document
    .getElementById("wrapper")
    .insertAdjacentHTML("afterbegin", renderMedia(media));
}

function appendName(media) {
  document
    .getElementById("name")
    .insertAdjacentHTML("afterbegin", renderName());
}

function hideName() {
  var name = document.getElementById("name");
  if (name.classList.contains("show")) {
    name.classList.remove("show");
  }
  name.classList.add("hide");
}
function showName() {
  var name = document.getElementById("name");
  if (name.classList.contains("hide")) {
    name.classList.remove("hide");
  }
  name.classList.add("show");
}

// let searchValue = {
//   searchValue: "dSRFgJ2L3CqrZJrmOkWD@gmail.com"
// };
// ////
// let reqHeader = new Headers();
// reqHeader.append("Content-Type", "text/json");
// reqHeader.append("Accept", "application/json, text/plain, */*");

// let initObject = {
//   method: "POST",
//   headers: reqHeader,
//   body: JSON.stringify(searchValue)
// };
// ////

fetch("/api/ViewMediaDetails/GetEpisode/1DIt6YqpmgDTOPDzsaqz")
  .then((response) => response.json())
  .then((json) => {
    appendName();
    appendMedia(json);
    var video = document.querySelector("video");
    video.addEventListener("play", () => {
      setTimeout(hideName, 5000);
    });
    video.addEventListener("pause", () => {
      setTimeout(showName, 500);
    });
  });
