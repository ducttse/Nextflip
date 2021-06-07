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

function appendMedia(start, end) {
  let mediaArray = Data.data.slice(start, end).map((media) => {
    return renderMedia(media);
  });
    mediaArray = mediaArray.join("");
    document.getElementById("wrapper1").insertAdjacentHTML("afterbegin", mediaArray);
    document.getElementById("wrapper2").insertAdjacentHTML("afterbegin", mediaArray);
}

fetch("/api/ViewSubscribedUserDashboard/GetMediasByCategoryID/1")
  .then((res) => res.json())
  .then((json) => {
    Data.data = json;
    appendMedia(0, 4);
  });
