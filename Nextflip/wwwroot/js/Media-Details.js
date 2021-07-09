let mediaData;
function renderBanner() {
  return `<img class="img-fluid"
    src="${mediaData.media.bannerURL}"
    alt="${mediaData.media.mediaID}"/>
    `;
}

function renderDetail(title, content) {
  return `<div flex="row">
            <p class="feild text-white-50 col-3 d-inline fs-5">${title}:</p>
            <p class="col-9 d-inline fs-5" style="word-wrap: break-word">
            ${content == null ? "" : content}
            </p>
          </div>`
}

function renderArrayDetail(title, arrContent) {
  let content = "";
  if (arrContent != null) {
    for (let i = 0; i < arrContent.length; i++) {
      content += arrContent[ i ].name + ", ";
    }
    content = content.slice(0, content.length - 2)
  }
  return `<div flex="row">
            <p class="feild text-white-50 col-3 d-inline fs-5">${title}:</p>
            <p class="col-9 d-inline fs-5" style="word-wrap: break-word">
            ${arrContent == null ? "" : content}
            </p>
          </div>`
}

function ShowInfor() {
  let info = "";
  info += renderDetail("Director", mediaData.media.director);
  info += renderDetail("Cast", mediaData.media.cast);
  info += renderDetail("Language", mediaData.media.language);
  info += renderDetail("Type", mediaData.media.filmType);
  info += renderDetail("Publish year", mediaData.media.publishYear);
  info += renderDetail("Duration", mediaData.media.duration);
  info += renderArrayDetail("Category", mediaData.categories);
  return info;
}

function watch(url) {
  window.location.href = "/WatchMedia/Watch/" + url;
}

function renderEpisodeBySeason(id) {
  let episodes = mediaData.episodesMapSeason[ id ].map((episode) => {
    return `<div class="row ps-3" onclick="watch('${mediaData.media.mediaID}/${episode.episodeID}')">
                <p class="episode_detail ps-2 d-inline fs-5"><i class="far fa-play-circle d-inline"></i> ${episode.title}</p>
            </div>`
  })
  return episodes.join("");
}

function sessionButton(season) {
  return `<p>
            <button class="btn btn-lg btn-dark" type="button" data-bs-toggle="collapse" data-bs-target="#session_${season.seasonID}" aria-expanded="false" aria-controls="collapseExample">
              ${season.title}
            </button>
          </p>
          <div class="ms-3 ps-1 collapse" id="session_${season.seasonID}">
            ${renderEpisodeBySeason(season.seasonID)}
          </div>`
}

function getSeason() {
  let season = mediaData.seasons.map((item) => {
    return sessionButton(item);
  })
  return season.join("");
}

function chooseOptions(option, obj) {
  document.querySelector(".chosen").classList.remove("chosen");
  if (obj != null) {
    obj.classList.add("chosen");
  }
  else {
    document.getElementById("description").classList.add("chosen")
  }
  switch (option) {
    case "description":
      content = `<p class="ps-2">${mediaData.media.description}</p>`
      break;
    case "season":
      content = getSeason();
      break;
  }
  let detail = document.getElementById("option_detail");
  if (detail.innerHTML != "") {
    detail.innerHTML = "";
  }
  detail.insertAdjacentHTML("afterbegin", content);
}

function appendToDetailWrapper() {
  document.getElementById("img_holder").insertAdjacentHTML("afterbegin", renderBanner());
  document.getElementById("detail_title").insertAdjacentHTML("afterbegin", mediaData.media.title)
  document.getElementById("detail_table").insertAdjacentHTML("afterbegin", ShowInfor());
  chooseOptions('description');
}

function clearWrapper() {
  document.getElementById("img_holder").innerHTML = "";
  document.getElementById("detail_title").innerHTML = "";
  document.getElementById("detail_table").innerHTML = "";
}

let detailModal;
function showDetail() {
  if (detailModal == null) {
    var detailModal = new bootstrap.Modal(document.getElementById('modalDetail'), {
      keyboard: false
    })
  }
  detailModal.show();
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
      mediaData = json;
      appendToDetailWrapper();
      showDetail();
    });
});

function AppendDetails(id) {
  mediaID = id;
  clearWrapper();
  processChange();
}
