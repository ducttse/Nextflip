let pageData = {
  totalPage: 0,
  currentArr: [],
  currentPage: 1
}

function setPageDataCurrentPage(num) {
  pageData.currentPage = num;
}

function setCurrentArr() {
  pageData.currentArr = [];
  let delta = 2;
  let current = parseInt(pageData.currentPage);
  let total = parseInt(pageData.totalPage);
  let left = current - delta;
  let right = current + delta + 1;
  let range = [];
  let rangeWithDots = [];
  let j;
  range.push(1);
  if (total > 1) {
    for (let i = current - delta; i <= current + delta; i++) {
      if (i >= left && i < right && i < total && i > 1) {
        range.push(i);
      }
    }
    range.push(total);
    for (let i of range) {
      if (j) {
        if (i - j === 2) {
          rangeWithDots.push(j + 1);
        }
        else if (i - j !== 1) {
          rangeWithDots.push("...");
        }
      }
      rangeWithDots.push(i);
      j = i;
    }
    pageData.currentArr = rangeWithDots;
  }
  else {
    pageData.currentArr = range;
  }
}


function removeCurrentColor() {
  document.getElementById(`page_${pageData.currentPage}`).classList.remove("active");
}

function setCurrentColor() {
  document.getElementById(`page_${pageData.currentPage}`).classList.add("active");
}


function renderCurrentArray() {
  setCurrentArr();
  let Pages = pageData.currentArr.map((page) => {
    if (page == "...") {
      return `<li class="page-item disabled" page="${page}"><a class="page-link" href="#">${page}</a></li>`
    }
    return `<li class="page-item pageNumber" id="page_${page}" page="${page}" onclick="setCurrentPage(this)"><a class="page-link" href="#">${page}</a></li>`
  });
  Pages = Pages.join("");
  return `
    <nav>
      <ul class="pagination my-auto">
        ${Pages}
      </ul>
    </nav>`;
}

function appendCurrentArray() {
  let pagination = document.getElementById("pagination");
  if (pagination === null) {
    return;
  }
  if (pagination.innerHTML !== "") {
    pagination.innerHTML = ""
  }
  pagination.insertAdjacentHTML("afterbegin", renderCurrentArray());
  setCurrentColor();
}

let AppendToDataWrapper;
function setAppendToDataWrapper(func) {
  AppendToDataWrapper = func;
}

function setCurrentPage(obj) {
  let num = obj.getAttribute("page");
  if (pageData.currentPage == num) {
    return;
  }
  setRequestPage(num)
    .then(res => res.json())
    .then(json => {
      Data = json;
      removeCurrentColor()
      pageData.currentPage = num;
      AppendToDataWrapper();
    });
}
