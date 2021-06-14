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

function renderName(data) {
  return `
  <div id="name" class="fixed-top mt-2 ml-5" style="color: white">
    <div class="row">
      <p class="h4 ml-2">
        <a class="text-decoration-none link-secondary" href="/WatchMedia/MediaDetails/${data.mediaID}">
          <i id="icon" class="fas fa-arrow-left h5 pt-1"></i>
        </a> ${data.episode.title}
      </p>
    </div>
    <div class="ml-3">
      <p>Episode ${data.episode.number}</p>
    </div>
  </div>`;
}

function appendMedia(data) {
  document
    .getElementById("wrapper")
    .insertAdjacentHTML("afterbegin", renderMedia(data.episode));
}

function appendName(data) {
  document
    .getElementById("name")
      .insertAdjacentHTML("afterbegin", renderName(data));
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

function Run(mediaID, episodeID) {
  fetch(`/api/ViewMediaDetails/GetEpisode/${mediaID}/${episodeID}`)
    .then((response) => response.json())
    .then((json) => {
      appendName(json);
      appendMedia(json);
      var video = document.querySelector("video");
      video.addEventListener("play", () => {
        setTimeout(hideName, 5000);
      });
      video.addEventListener("pause", () => {
        setTimeout(showName, 500);
      });
    })
    .catch((err) => console.log(err));
}
