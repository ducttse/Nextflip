let Data = {
  data: [
    {
      mediaID: "M001",
      title: "Spirder-Man",
      status: "Enabled",
      language: "English",
      bannerURL: "https://storage.googleapis.com/next-flip/Image/Banner"
    },
    {
      mediaID: "M001",
      title: "Spirder-Man",
      status: "Enabled",
      language: "English",
      bannerURL: "https://storage.googleapis.com/next-flip/Image/Banner"
    },
    {
      mediaID: "M001",
      title: "Spirder-Man",
      status: "Enabled",
      language: "English",
      bannerURL: "https://storage.googleapis.com/next-flip/Image/Banner"
    },
    {
      mediaID: "M001",
      title: "Spirder-Man",
      status: "Enabled",
      language: "English",
      bannerURL: "https://storage.googleapis.com/next-flip/Image/Banner"
    }
  ]
};

function renderMedia(media) {
  return `
  <div class="col-3 w-25" id="mediaWapper">
    <img
      src="${media.bannerURL}"
      alt="img"
      class="w-100 h-100"
    />
    <p class="bg-dark text-white title">${media.title}</p>
  </div>`;
}

function appendMedia() {
  let mediaArray = Data.data.map((media) => {
    return renderMedia(media);
  });
  mediaArray = mediaArray.join("");
  console.log(mediaArray);
  document
    .getElementById("wapper")
    .insertAdjacentHTML("afterbegin", mediaArray);
}

xhr.onreadystatechange();

let xhr = new XMLHttpRequest();
//url
xhr.open("POST", "/ViewSubscribedUserDashboard/GetFavoriteMedias", true);
xhr.send("userID=05fRBPEgvmSBYSEhG0i7");
xhr.onreadystatechange = function () {
  if (this.readyState == 4 && this.status == 200) {
    appendMedia();
  }
};