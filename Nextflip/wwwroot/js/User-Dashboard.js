let noImage = "https://storage.googleapis.com/next-flip/Image/no%20image.png";

function validateUrl(value) {
  return /^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:[/?#]\S*)?$/i.test(
    value
  );
}

function renderImageHolder(media) {
  let CheckedURL = noImage;
  if (validateUrl(media.bannerURL)) {
    CheckedURL = media.bannerURL;
  }
  return `
  <div class="col-3 imageHolder px-1">
  <a href="/WatchMedia/MediaDetails/${media.mediaID}">
    <img
      src="${CheckedURL}"
      class="w-100 h-90"
      alt="..."/>
  </a>
  </div>`;
}

function renderCarousel(mediaArr, index) {
  let renderedImgHolder = mediaArr.map((media) => {
    return renderImageHolder(media);
  });
  renderedImgHolder = renderedImgHolder.join("");
  return `
  <div class="carousel-item ${index === 0 ? "active" : ""}"> 
    <div class="row">
      ${renderedImgHolder}
    </div>
  </div>
  `;
}

function renderLisrHolder(listMedia) {
  let mediaArr = listMedia.mediaArr;
  let length = Math.floor(mediaArr.length / 4);
  let renderedCarousels = "";
  for (let i = 0; i < length; i++) {
    var subArr = mediaArr.slice(i * 4, (i + 1) * 4);
    renderedCarousels += renderCarousel(subArr, i);
  }
  let button = `
    <button class="carousel-control-prev" type="button" data-bs-target="#carousel_${listMedia.categoryID}" data-bs-slide="prev">
      <span class="carousel-control-prev-icon" aria-hidden="true"></span>
      <span class="visually-hidden">Previous</span>
    </button>
    <button class="carousel-control-next" type="button" data-bs-target="#carousel_${listMedia.categoryID}" data-bs-slide="next">
      <span class="carousel-control-next-icon" aria-hidden="true"></span>
      <span class="visually-hidden">Next</span>
    </button>
  `;
  return `
  <div class="mt-5 listHolder">
    <p class="h3 listTitle">${listMedia.name}</p>
    <div>
      <div id="carousel_${listMedia.categoryID}" class="carousel slide h-25" data-bs-ride="carousel">
        <div class="carousel-inner">
          ${renderedCarousels}
        </div>
        ${length <= 1 ? "" : button}
      </div>
    </div>
  </div>`;
}

function appendToWrapper(renderedEL) {
  document
    .getElementById("wrapper")
    .insertAdjacentHTML("beforeend", renderedEL);
}

function fetchCategoryID(category) {
  fetch(
    `/api/ViewSubscribedUserDashboard/GetMediasByCategoryID/${category.categoryID}`
  )
    .then((res) => res.json())
    .then((json) => {
      console.log(json);
      if (json.length < 8) {
        return;
      }
      category.mediaArr = json;
      appendToWrapper(renderLisrHolder(category));
    });
}


function Run() {
  fetch("/api/ViewSubscribedUserDashboard/GetCategories")
    .then((res) => res.json())
    .then((categories) => {
      console.log(categories);
      categories.forEach((category) => {
        fetchCategoryID(category);
      });
    });
}

Run();
