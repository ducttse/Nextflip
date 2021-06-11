function renderSearch(requetsURL, placeHolderText) {
  return `
    <div class="row col-4 offset-1 p-3 mb-2 bg-white rounded">
        <span id="searchBtn" class="col-1 btn btn-danger"><i class="fas fa-search"></i></span>
        <input
            id="search"
            type="text"
            style="width: 90% !important"
            class="form-control col-2 d-inline"
            placeholder="${placeHolderText}"
            requestURL="${requetsURL}"
            value=""
        />
    </div>
    `;
}

function appendSearch(requetsURL, placeHolderText) {
  document
    .getElementById("search_wrapper")
    .insertAdjacentHTML(
      "afterbegin",
      renderSearch(requetsURL, placeHolderText)
    );
}

function setClick(func) {
  let searhObj = document.getElementById("search");
  document.getElementById("searchBtn").addEventListener("click", () => {
    func(searhObj.value);
  });
}
