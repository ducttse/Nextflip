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

function appendMedia(data) {
  document
    .getElementById("wrapper")
    .insertAdjacentHTML("afterbegin", renderMedia(data));
  document.getElementById("media_title").innerHTML = data.mediaTitle + " - " + data.title;
  let categories = data.categories.map(category => {
    return category.name;
  }).join(", ");
  document.getElementById("category").insertAdjacentHTML("beforeend", " " + categories);
  document.getElementById("description").insertAdjacentHTML("beforeend", data.mediaDescription);
}
let currentEpisodeID;
let currentSeasonID;
async function Run(mediaID, episodeID) {
  currentEpisodeID = episodeID;
  let categories;
  await fetch(`/api/ViewMediaDetails/GetEpisode/${mediaID}/${episodeID}`)
    .then((response) => response.json())
    .then(async (json) => {
      currentSeasonID = json.seasonID;
      categories = json.categories;
      appendMedia(json);
      getMeidaList(mediaID, json.seasonID);
      getSeason(mediaID, json.seasonID);
    })
  requestSuggestion(categories[ 0 ].categoryID);
}

function renderEpisode(mediaID, episode) {
  return `
  <a class="text-decoration-none episode d-flex justify-content-start mb-2 overflow-hidden ${currentEpisodeID == episode.episodeID ? "current" : ""}" href="/WatchMedia/Watch/${mediaID}/${episode.episodeID}">
    <div class="img_container col-4">
      <img class="image w-100 h-100" src="${episode.thumbnailURL}" alt="${episode.number}"/>
    </div>
    <div class="col-8">
      <p class="fs-6 text-light ps-2 pt-1 text">Episode ${episode.number} ${episode.title == "" ? "" : (" - " + episode.title)}</p>
    </div>
  </a>`;
}

function getMeidaList(mediaID, seasonID) {
  fetch(`https://localhost:44341/api/ViewMediaDetails/GetEpisodesOfSeason/${seasonID}`)
    .then(res => res.json())
    .then(json => {
      var seasons = json.map(episode => {
        return renderEpisode(mediaID, episode);
      }).join("");
      let epsidesEL = document.getElementById("episode_list");
      if (epsidesEL.innerHTML != "") {
        epsidesEL.innerHTML = "";
      }
      epsidesEL.insertAdjacentHTML("afterbegin", seasons);
    })
    .catch(err => console.log(err))
}

function getSeason(mediaID) {
  fetch(`/api/ViewMediaDetails/GetSeasons/${mediaID}`)
    .then(res => res.json())
    .then(json => {
      let seasons = json.map(season => {
        if (currentSeasonID == season.seasonID) {
          setSeasonName(`Season ${season.number}: ${season.title}`);
        }
        return `<div class="p-2 d-flex season_btn ${currentSeasonID == season.seasonID ? "current_season" : ""}" seasonID="${season.seasonID}">Season ${season.number}: ${season.title}</div>`;
      }).join(`<div class="dropdown-divider m-0"></div>`);
      document.getElementById("seasons").insertAdjacentHTML("afterbegin", seasons);
      setReloadEpisodes(mediaID);
    })
}

function setCorlor() {
  document.getElementsByClassName("current_season")[ 0 ].classList.remove("current_season");
  let current = [ ...document.querySelectorAll(".season_btn") ].filter(btn => btn.getAttribute("seasonID") == currentSeasonID);
  current[ 0 ].classList.add("current_season");
}

function setSeasonName(name) {
  let nameHodler = document.getElementById("season_name");
  if (nameHodler.innerHTML != "") {
    nameHodler.innerHTML = "";
  }
  nameHodler.insertAdjacentHTML("beforeend", name + `<i class="pt-1 ps-1 fas fa-caret-down"></i>`)
  // <i class="fas fa-caret-down"></i>
}

function setReloadEpisodes(mediaID) {
  document.querySelectorAll(".season_btn").forEach(element => {
    let seasonID = element.getAttribute("seasonID");
    element.addEventListener("click", (evt) => {
      currentSeasonID = evt.target.getAttribute("seasonID");
      reloadEpisodesContainer(mediaID, seasonID)
      setCorlor();
      setSeasonName(evt.target.innerText);
    })
  });
}

function reloadEpisodesContainer(mediaID, seasonID) {
  console.log(mediaID);
  console.log(seasonID);
  getMeidaList(mediaID, seasonID);
}

function showMore(obj) {
  document.getElementById("description").classList.remove("short");
  obj.classList.add("d-none");
  document.getElementById("btn_hide").classList.remove("d-none");
}
function hide(obj) {
  document.getElementById("description").classList.add("short");
  obj.classList.add("d-none");
  document.getElementById("btn_show").classList.remove("d-none");
}

function rendeMedia(media) {
  console.log(media);
  return `
  <div class="col-4 result mb-2 p-1" onclick="return AppendDetails('${media.mediaID}');">
      <div class="img_container">
          <img src="${media.bannerURL}" alt="${media.mediaID}" style="width: 100%; height: 100%; object-fit: cover"/>
          <div id="title">
              <p class="link-light p-1 fs-6 ">${media.title}</p>
          </div>
      </div>
  </div>`;
}

function rendeMediaEL(arr) {
  console.log(arr);
  let rendered = arr.map(media => {
    console.log(media);
    return rendeMedia(media);
  }).join("");
  return `
      <div class="row">
          ${rendered}
      </div>
  `
}

function showSuggestion(arr) {
  let el = rendeMediaEL(arr);
  document.getElementById("media_holder").insertAdjacentHTML("afterbegin", el);
}

function requestSuggestion(id) {
  fetch(`/api/ViewSubscribedUserDashboard/GetMediasByCategoryID/${id}/9`)
    .then(res => res.json())
    .then(json => {
      showSuggestion(json)
    })
}


