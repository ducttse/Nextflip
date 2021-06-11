let Data = {
  data: []
};
function renderRequest(request) {
  return `
      <tr>
          <td>${request.name}</td>
          <td>${request.userEmail}</td>
          <td>${request.note}</td>
          <td><a class="text-decoration-none" href="#${request.mediaID}">Preview</a></td>
          <td>
              <button class="btn btn-primary col-5">
                  <i class="fas fa-check text-white"></i>
              </button>
              <button class="btn btn-danger col-5">
                  <i class="fas fa-times"></i>
              </button>
          </td>
      </tr>`;
}
let rowsPerPage = 4;
let currentPage = 0;

function appendRequest(start, end) {
  let requestArray = Data.data.slice(start, end).map((request) => {
    return renderRequest(request);
  });
  requestArray = requestArray.join("");
  let requestWrapper = document.getElementById("requestWrapper");
  if (requestWrapper.innerHTML !== "") {
    requestWrapper.innerHTML = "";
  }
  requestWrapper.insertAdjacentHTML("afterbegin", requestArray);
}

function renderPagination(length, rowsPerPage) {
  let numberOfPage = Math.ceil(length / rowsPerPage);
  let Pages = "";
  console.log(numberOfPage);
  for (let i = 1; i <= numberOfPage; i++) {
    Pages += `<li class="page-item" page="${i}" onClick="setCurrentPage(${i})"><a class="page-link" href="#">${i}</a></li>`;
  }
  return `
    <nav>
      <ul class="pagination">
        <li class="page-item disabled">
          <a class="page-link" href="#">Previous</a>
        </li>
        ${Pages}
        <li class="page-item"><a class="page-link" href="#">Next</a></li>
      </ul>
      </nav>`;
}
// jump to another pagination
function setCurrentPage(number) {
  currentPage = number - 1;
  appendRequest(
    currentPage * rowsPerPage,
    rowsPerPage + Data.data.length - currentPage * rowsPerPage > rowsPerPage
      ? ++currentPage * rowsPerPage
      : currentPage * rowsPerPage +
          (Data.data.length - currentPage * rowsPerPage)
  );
  setCurrentColor(number);
}

function removeCurrentColor() {
  let pageArray = Array.from(document.getElementsByClassName("page-item"));
  let curPage = pageArray.filter((page) => {
    return page.classList.contains("active");
  });
  if (curPage.length > 0) {
    curPage[0].classList.remove("active");
  }
}

function appendPagination(length, rowsPerPage) {
  document
    .getElementById("pagination")
    .insertAdjacentHTML("afterbegin", renderPagination(length, rowsPerPage));
}

function setCurrentColor(number) {
  removeCurrentColor();
  let pageArray = Array.from(document.getElementsByClassName("page-item"));
  let curPage = pageArray.filter((page) => {
    return parseInt(page.getAttribute("page")) === number;
  });
  curPage[0].classList.add("active");
}

fetch("/api/MediaManagerManagement")
  .then((res) => res.json())
  .then((json) => {
    Data.data = json;
    appendPagination(Data.data.length, rowsPerPage);
    appendRequest(0, rowsPerPage);
    setCurrentColor(1);
  });
