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
    <div onclick="return AppendDetails('${media.mediaID}');">
      <img
        src="${CheckedURL}"
        class="w-100 h-90"
        alt="..."/>
    </div>
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
  let mediaArr = listMedia.data;
  let length = Math.floor(mediaArr.length / 4);
  let renderedCarousels = "";
  for (let i = 0; i < length; i++) {
    var subArr = mediaArr.slice(i * 4, (i + 1) * 4);
    renderedCarousels += renderCarousel(subArr, i);
  }
  let button = `
    <button class="carousel-control-prev" type="button" data-bs-target="#carousel_${listMedia.name}" data-bs-slide="prev">
      <span class="carousel-control-prev-icon" aria-hidden="true"></span>
      <span class="visually-hidden">Previous</span>
    </button>
    <button class="carousel-control-next" type="button" data-bs-target="#carousel_${listMedia.name}" data-bs-slide="next">
      <span class="carousel-control-next-icon" aria-hidden="true"></span>
      <span class="visually-hidden">Next</span>
    </button>
  `;
  let showMoreBtn = "";
  if (listMedia.name != "newest") {
    showMoreBtn = `<a class="text-muted fs-6 ms-2 pt-2 text-decoration-none" href="/SubcribedUserDashBoard/ViewByCategory/${listMedia.id}">See More</a>`;
  }
  return `
  <div class="mt-3 listHolder px-2">
    <div class="d-flex justify-content-start">
      <p class="h3 listTitle ps-2 text-capitalize">${listMedia.name}</p>
      ${showMoreBtn}
    </div>
    <div>
      <div id="carousel_${listMedia.name}" class="carousel slide h-25" data-bs-ride="carousel">
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

function Run() {
  fetch("/api/ViewSubscribedUserDashboard/GetMedias")
    .then((res) => res.json())
    .then((mediasArr) => {
      let el = mediasArr.map((media) => {
        return renderLisrHolder(media);
      }).join("")
      appendToWrapper(el);
    });
}

Run();
