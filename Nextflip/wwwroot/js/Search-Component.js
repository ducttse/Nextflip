function renderSearch(placeHolderText, searchValue) {
  return `
    <div class="row mb-2">
      <div class="col-10">
        <div class="input-group">
          <input
              id="search"
              type="text"
              class="form-control d-inline"
              placeholder="${placeHolderText}"
              value="${searchValue}"
            />
          <div id="searchBtn" class="btn btn-primary d-inline border-start-0 border border-light">Search</div>
        </div>
      </div>
    </div>
    `;
}

function appendSearch(placeHolderText, searchValue) {
  let value;
  if (searchValue === null) {
    value = "";
  } else {
    value = searchValue;
  }
  document
    .getElementById("search_wrapper")
    .insertAdjacentHTML("afterbegin", renderSearch(placeHolderText, value));
  setClick();
}

function setClick() {
  let searhObj = document.getElementById("search");
  document.getElementById("searchBtn").addEventListener("click", () => {
    searchAndResetPage(searhObj.value);
  });
}

function setEnterEvent() {
  document.getElementById("search").addEventListener("keyup", (evt) => {
    if (evt.keyCode === 13) {
      if (evt.target.value.trim().length != 0) {
        searchAndResetPage(evt.target.value);
      }
    }
  })
}
