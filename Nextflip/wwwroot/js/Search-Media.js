function renderCategory(arr) {
  let renderedArr = arr.map((category) => {
    return `<p class="badge rounded-pill bg-secondary">${category.name}</p>`;
  });
  renderedArr = renderedArr.join("");
  return renderedArr;
}

function renderResult(result) {
  let categories = renderCategory(result.categories);
  let description = SliceDescription(result.media.description);
  return `
    <div class="result" onclick="redirectToDetails('${result.media.mediaID}')">
        <div class="mb-2 row">
            <div class="col-3">
                <img class="w-100 h-100" src="${result.media.bannerURL}" alt="${result.media.title}" />
            </div>
            <div class="col-9">
                <div class="col-10">
                    <p class="h3">
                        ${result.media.title}
                    </p>
                    ${categories}
                    <p>
                        ${description}
                    </p>
                </div>
            </div>
        </div>
    </div>`;
}

function redirectToDetails(id) {
  window.location.href = `/WatchMedia/MediaDetails/${id}`;
  return false;
}

function SliceDescription(description) {
  if (description.length > 500) {
    return description.slice(0, 501) + "...";
  } else return description;
}

function appendToWrapper(data) {
  let renderedResults = data.map((item) => {
    return renderResult(item);
  });
  renderedResults = renderedResults.join("");
  document
    .getElementById("result_holder")
    .insertAdjacentHTML("afterbegin", renderedResults);
}

function search(searchValue) {
  fetch(`/api/ViewSubscribedUserDashboard/GetMediasByTitle/${searchValue}`)
    .then((res) => res.json())
    .then((json) => {
      Refresh();
      appendToWrapper(json);
    });
}

function Refresh() {
  let holder = document.getElementById("result_holder");
  if (!holder.innerHTML !== "") {
    holder.innerHTML = "";
  }
}
