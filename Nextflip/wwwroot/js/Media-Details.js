function ShowEpisode() {
  let iconRight = document.getElementsByClassName("fa-caret-right")[0];
  let iconDown = document.getElementsByClassName("fa-caret-down")[0];
  let episode = document.getElementsByClassName("episode_holder")[0];
  iconRight.classList.add("hide");
  episode.classList.remove("hide");
  iconDown.classList.remove("hide");
}
function HideEpisode() {
  let iconRight = document.getElementsByClassName("fa-caret-right")[0];
  let iconDown = document.getElementsByClassName("fa-caret-down")[0];
  let episode = document.getElementsByClassName("episode_holder")[0];
  episode.classList.add("hide");
  iconRight.classList.remove("hide");
  iconDown.classList.add("hide");
}

function renderImg(media) {
  return `
  <img
  src="${media.bannerURL}"
  alt="${media.title}"
  class="w-100 h-100"
  />
  `;
}

function renderUpSection(media) {
  let title = media.title;
  let displayName =
    title.charAt(0).toUpperCase() + title.slice(1, title.length + 1);
  return `
    <div class="col-12">
      <p id="title">${media.title}</p>
      <p id="description">${media.description}</p>
    </div>
    <form method="POST" class="row">
      <button class="btn btn-danger col-2 offset-2">
        <i class="fas fa-play"></i> <span>Play</span>
      </button>
      <button class="btn btn-secondary col-3 mx-1">
        <i class="fas fa-heart"></i> <span>Add to Favourite</span>
      </button>
    </form>`;
}

function renderEpisodes(episodes) {
  let renderedArray = episodes.map((episode) => {
    return `
      <p>
        <a class="badge bg-secondary rounded-pill text-decoration-none" href="/watch/${episode.episodeID}">
          <i class="fas fa-play"></i>
        </a> ${episode.number}:${episode.title}
      </p>`;
  });
  return renderedArray.join("");
}

function renderSeasons(episodeArray, title) {
  let episodes = renderEpisodes(episodeArray);
  return `
    <p>${title}
      <i class="fas fa-caret-down" onclick="HideEpisode()"></i>
      <i class="fas fa-caret-right hide" onclick="ShowEpisode()"></i>
    </p>
    <div class="episode_holder">
      ${episodes}
    </div>`;
}

function renderAboveSection(seasons, episodeMap) {
  // x.episodesMapSeason[x.seasons[0].seasonID]
  let renderedArray = seasons.map((season) => {
    return renderSeasons(episodeMap[season.seasonID], season.title);
  });
  renderedArray = renderedArray.join("");
  return `
    <div class="season_holder">
      ${renderedArray}
    </div>`;
}

function appendToWrapper(data) {
  document
    .getElementById("imageHolder")
    .insertAdjacentHTML("afterbegin", renderImg(data.media));
  document
    .getElementById("infor_hodler")
    .insertAdjacentHTML("afterbegin", renderUpSection(data.media));
  document
    .getElementById("details_holder")
    .insertAdjacentHTML(
      "afterbegin",
      renderAboveSection(data.seasons, data.episodesMapSeason)
    );
}

function onload(json) {
  appendToWrapper(json);
}
// x.episodesMapSeason[x.seasons[0].seasonID]
function Run() {
  fetch("/api/ViewMediaDetails/GetMediaDetails/knoZvTFPyBjmZpzekmOI")
    .then((response) => response.json())
    .then((json) => {
      onload(json);
    });
}
Run();
