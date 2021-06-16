let CurrentPage = 1;
function renderPagination(length, rowsPerPage) {
  let numberOfPage = maxPage;
  let curentLoadedPage = CountCurrentLoadedPage();
  if (curentLoadedPage === 0) {
    curentLoadedPage = 1;
  }
  let Pages = "";
  for (let i = 1; i <= numberOfPage; i++) {
    Pages += `<li class="page-item indexPage  ${i > curentLoadedPage ? "d-none" : ""}" id="page_${i}" page="${i}"><a class="page-link" href="#">${i}</a></li>`;
  }
  return `
    <nav  class="col-6 mx-auto">
      <ul class="pagination">
        ${Pages}
      </ul>
    </nav>`;
}

function showPage(number, appendToWrapper) {
  let showPage = parseInt(number);
  if (CurrentPage === showPage) {
    return;
  }
  let beforePrevPage = document.getElementById(`page_${showPage - 2}`);
  let prevPage = document.getElementById(`page_${showPage - 1}`);
  let nextPage = document.getElementById(`page_${showPage + 1}`);
  let afterNextPage = document.getElementById(`page_${showPage + 2}`);
  let minus = showPage - CurrentPage;
  if (minus === 1) {
    if (nextPage !== null) {
      //location 3
      if (beforePrevPage !== null && !beforePrevPage.classList.contains("d-none")) {
        beforePrevPage.classList.add("d-none");
        nextPage.classList.remove("d-none");
      }
    }
  }
  else if (minus === 2) {
    if (afterNextPage !== null) {
      if (afterNextPage.classList.contains("d-none")) {
        beforePrevPage.classList.add("d-none");
        nextPage.classList.remove("d-none");
      }
    }
  }
  else if (minus === -1) {
    if (prevPage !== null) {
      if (prevPage.classList.contains("d-none")) {
        afterNextPage.classList.add("d-none");
        prevPage.classList.remove("d-none")
      }
    }
  }
  else if (minus === -2) {
    if (prevPage !== null) {
      if (prevPage.classList.contains("d-none")) {
        afterNextPage.classList.add("d-none");
        prevPage.classList.remove("d-none")
      }
    }
  }
  if (CountCurrentLoadedPage() < showPage) {
    RequestMoreData(showPage);
  }
  else setCurrentPage(number, appendToWrapper)
}


// jump to another pagination
function setCurrentPage(number, appendToWrapper) {
  CurrentPage = parseInt(number);
  let currentPage = number - 1;
  appendToWrapper(currentPage * rowsPerPage, (currentPage + 1) * rowsPerPage);
  setCurrentColor(number);
  setDisable(number);
}

function setDisable(number) {
  //set prev and next button
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
  return curPage[ 0 ].getAttribute("page");
}

function goToNextPage(appendToWrapper) {
  let currentPage = parseInt(getCurrentPage());
  console.log(currentPage);
  if (currentPage == maxPage) {
    return;
  }
  showPage(currentPage + 1, appendToWrapper);
}

function goToPrevPage(appendToWrapper) {
  let currentPage = parseInt(getCurrentPage());
  if (currentPage == 1) {
    return;
  }
  showPage(currentPage - 1, appendToWrapper);
}

function appendNextButton() {
  let button = `<li class="page-item" id="next">
                  <a class="page-link" href="#">Next</a> 
                </li>` ;
  document.getElementsByClassName("pagination")[ 0 ].insertAdjacentHTML("beforeend", button);
}

function appendPreviousButton() {
  let button = `<li class="page-item disabled" id="previous">
                  <a class="page-link" href="#">Previous</a>
                </li>`;
  document.getElementsByClassName("pagination")[ 0 ].insertAdjacentHTML("afterbegin", button);
}

function removeCurrentColor() {
  let pageArray = Array.from(document.getElementsByClassName("page-item"));
  let curPage = pageArray.filter((page) => {
    return page.classList.contains("active");
  });
  if (curPage.length > 0) {
    curPage[ 0 ].classList.remove("active");
  }
}

function setCurrentColor(number) {
  removeCurrentColor();
  let pageArray = Array.from(document.getElementsByClassName("page-item"));
  let curPage = pageArray.filter((page) => {
    return page.getAttribute("page") == number;
  });
  curPage[ 0 ].classList.add("active");
}

function appendPagination(length, rowsPerPage) {
  document
    .getElementById("pagination")
    .insertAdjacentHTML("afterbegin", renderPagination(length, rowsPerPage));
  appendNextButton();
  appendPreviousButton();
  // setDisable();
}

function setClickToIndex(func) {
  let collection = document.getElementsByClassName("indexPage");
  for (let i = 0; i < collection.length; i++) {
    let el = collection[ i ];
    let num = el.getAttribute("page");
    el.addEventListener("click", () => {
      showPage(num, func);
    });
  }

  document
    .getElementById("next")
    .addEventListener("click", () => goToNextPage(func));
  document
    .getElementById("previous")
    .addEventListener("click", () => goToPrevPage(func));
}
