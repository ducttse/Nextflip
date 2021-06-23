function ShowEpisode(el) {
  let iconRight = el;
  let iconDown = el.parentElement.getElementsByClassName("fa-caret-down")[ 0 ];
  let episode =
    el.parentElement.parentElement.getElementsByClassName("episode_holder")[ 0 ];
  iconRight.classList.add("hide");
  episode.classList.remove("hide");
  iconDown.classList.remove("hide");
}
function HideEpisode(el) {
  let iconRight = el.parentElement.getElementsByClassName("fa-caret-right")[ 0 ];
  let iconDown = el;
  let episode =
    el.parentElement.parentElement.getElementsByClassName("episode_holder")[ 0 ];
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

function renderCategory(arr) {
  let renderedArr = arr.map((category) => {
    return `<p class="badge rounded-pill bg-secondary">${category.name}</p>`;
  });
  renderedArr = renderedArr.join("");
  return renderedArr;
}

function renderUpSection(media, categories) {
  let renderedCategories = renderCategory(categories);
  let title = media.title;
  let displayName =
    title.charAt(0).toUpperCase() + title.slice(1, title.length + 1);
  return `
    <div class="col-12">
      <p id="title">${displayName}</p>
      ${renderedCategories}
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

function renderEpisodes(episodes, mediaID) {
  let renderedArray = episodes.map((episode) => {
    return `
      <p>
        <a class="badge bg-secondary rounded-pill text-decoration-none" href="/WatchMedia/Watch/${mediaID}/${episode.episodeID}">
          <i class="fas fa-play"></i>
        </a> Episode ${episode.number} : ${episode.title}
      </p>`;
  });
  return renderedArray.join("");
}

function renderSeasons(episodeArray, title, mediaID) {
  let episodes = renderEpisodes(episodeArray, mediaID);
  return `
    <div>
        <p>${title}
            <i class="fas fa-caret-down" onclick="HideEpisode(this)"></i>
            <i class="fas fa-caret-right hide" onclick="ShowEpisode(this)"></i>
        </p>
        <div class="episode_holder">
            ${episodes}
        </div>
    </div>`;
}

function renderAboveSection(seasons, episodeMap, mediaID) {
  let renderedArray = seasons.map((season) => {
    return renderSeasons(episodeMap[ season.seasonID ], season.title, mediaID);
  });
  renderedArray = renderedArray.join("");
  return `
    <div class="season_holder">
      ${renderedArray}
    </div>`;
}

function appendToDetailWrapper(data) {
  document
    .getElementById("imageHolder")
    .insertAdjacentHTML("afterbegin", renderImg(data.media));
  document
    .getElementById("infor_hodler")
    .insertAdjacentHTML(
      "afterbegin",
      renderUpSection(data.media, data.categories)
    );
  document
    .getElementById("details_holder")
    .insertAdjacentHTML(
      "afterbegin",
      renderAboveSection(
        data.seasons,
        data.episodesMapSeason,
        data.media.mediaID
      )
    );
}

function clearWrapper() {
  document.getElementById("imageHolder").innerHTML = "";
  document.getElementById("infor_hodler").innerHTML = "";
  document.getElementById("details_holder").innerHTML = "";
}

function onload(json) {
  appendToDetailWrapper(json);
  var myModal = new bootstrap.Modal(document.getElementById('modalDetail'), {
    keyboard: false
  })
  myModal.show();
}

function debounce(func, timeout = 300) {
  let timer;
  return (...args) => {
    clearTimeout(timer);
    timer = setTimeout(() => { func.apply(this, args); }, timeout);
  };
}
let mediaID;
const processChange = debounce(() => {
  fetch(`/api/ViewMediaDetails/GetMediaDetails/${mediaID}`)
    .then((response) => response.json())
    .then((json) => {
      onload(json);
    });
});

function AppendDetails(id) {
  mediaID = id;
  clearWrapper();
  processChange();
}
