function renderSearch(placeHolderText, searchValue) {
  return `
    <div class="row col-4 offset-1 p-3 mb-2 bg-white rounded">
        <input
            id="search"
            type="text"
            style="width: 90% !important"
            class="form-control col-2 d-inline"
            placeholder="${placeHolderText}"
            value="${searchValue}"
        />
        <span id="searchBtn" class="col-1 btn btn-danger"><i class="fas fa-search"></i></span>
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
}

function setClick(func) {
  let searhObj = document.getElementById("search");
  document.getElementById("searchBtn").addEventListener("click", () => {
    func(searhObj.value);
  });
}
