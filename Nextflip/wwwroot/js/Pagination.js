function renderPagination(length, rowsPerPage) {
  let numberOfPage = Math.ceil(length / rowsPerPage);
  let Pages = "";
  for (let i = 1; i <= numberOfPage; i++) {
    Pages += `<li class="page-item indexPage" page="${i}"><a class="page-link" href="#">${i}</a></li>`;
  }
  return `
    <nav  class="col-6 mx-auto">
      <ul class="pagination">
        <li class="page-item disabled" id="previous">
          <a class="page-link" href="#">Previous</a>
        </li>
        ${Pages}
        <li class="page-item" id="next" id="next">
          <a class="page-link" href="#">Next</a>
        </li>
      </ul>
      </nav>`;
}

// jump to another pagination
function setCurrentPage(number, appendToWrapper) {
  curPage = number;
  let currentPage = number - 1;
  appendToWrapper(currentPage * rowsPerPage, (currentPage + 1) * rowsPerPage);
  setCurrentColor(number);
  let next = document.getElementById("next");
  let prev = document.getElementById("previous");
  if (number == 1 && !prev.classList.contains("disabled")) {
    prev.classList.add("disabled");
  } else if (prev.classList.contains("disabled")) {
    prev.classList.remove("disabled");
  }
  if (number == maxPage && !next.classList.contains("disabled")) {
    next.classList.add("disabled");
  } else if (next.classList.contains("disabled")) {
    next.classList.remove("disabled");
  }
}

function getCurrentPage() {
  let pageArray = Array.from(document.getElementsByClassName("page-item"));
  let curPage = pageArray.filter((page) => {
    return page.classList.contains("active");
  });
  return curPage[0].getAttribute("page");
}

function goToNextPage(appendToWrapper) {
  let currentPage = parseInt(getCurrentPage());
  if (currentPage == maxPage) {
    return;
  }
  setCurrentPage(currentPage + 1, appendToWrapper);
}

function goToPrevPage(appendToWrapper) {
  let currentPage = parseInt(getCurrentPage());
  if (currentPage == 1) {
    return;
  }
  setCurrentPage(currentPage - 1, appendToWrapper);
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

function setCurrentColor(number) {
  removeCurrentColor();
  let pageArray = Array.from(document.getElementsByClassName("page-item"));
  let curPage = pageArray.filter((page) => {
    return page.getAttribute("page") == number;
  });
  curPage[0].classList.add("active");
}

function appendPagination(length, rowsPerPage) {
  document
    .getElementById("pagination")
    .insertAdjacentHTML("afterbegin", renderPagination(length, rowsPerPage));
}

function setClickToIndex(func) {
  let collection = document.getElementsByClassName("indexPage");
  for (let i = 0; i < collection.length; i++) {
    let el = collection[i];
    let num = el.getAttribute("page");
    el.addEventListener("click", () => {
      setCurrentPage(num, func);
    });
  }
  document
    .getElementById("next")
    .addEventListener("click", () => goToNextPage(func));
  document
    .getElementById("previous")
    .addEventListener("click", () => goToPrevPage(func));
}
