function renderMedia() {
  return `
  <video
    id="video"
    class="video-js vjs-default-skin vjs-big-play-centered"
    data-setup="{}"
    controls
    preload="none"
    data-setup='{ "aspectRatio":"16:9", "playbackRates": [1, 1.5, 2] }'
  >
    <source src="https://storage.googleapis.com/next-flip/Media/koe8imWOkoi1dAsd03ds/amg3LkxWuWdPQvgYYlsY/2qn8JEjbqoDNJAjtkEdp/2qn8JEjbqoDNJAjtkEdp" type="video/mp4" />
  </video>
  `;
}

function renderName() {
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

function appendMedia() {
  document
    .getElementById("wapper")
    .insertAdjacentHTML("afterbegin", appendMedia);
}
function appendName() {
  document.getElementById("name").insertAdjacentHTML("afterbegin", appendName);
}
appendName();
appendMedia();
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

// fetch("/api/ViewMediaDetails/GetEpisodes/7BEoR5EnOjIjPqHghUxI", initObject)
//   .then((response) => response.json())
//   .then((json) => {
//     Data.data = json;
//   });
