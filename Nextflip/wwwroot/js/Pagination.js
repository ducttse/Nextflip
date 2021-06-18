let pageData = {
  totalPage: 0,
  currentArr: [],
  currentPage: 1
}

function setCurrentArr() {
  pageData.currentArr = [];
  for (let i = 1; i < pageData.totalPage; i++) {
    pageData.currentArr.push(i);
  }
}

function removeCurrentColor() {
  document.getElementById(`page_${pageData.currentPage}`).classList.remove("active");
}

function setCurrentColor() {
  console.log(pageData.currentPage);
  document.getElementById(`page_${pageData.currentPage}`).classList.add("active");
}


function renderCurrentArray() {
  setCurrentArr();
  let Pages = pageData.currentArr.map((page) => {
    return `<li class="page-item pageNumber" id="page_${page}" page="${page}" onclick="setCurrentPage(this)"><a class="page-link" href="#">${page}</a></li>`
  });
  Pages = Pages.join("");
  return `
    <nav  class="col-6 mx-auto">
      <ul class="pagination">
        ${Pages}
      </ul>
    </nav>`;
}

function appendCurrentArray() {
  let pagination = document.getElementById("pagination");
  if (pagination !== null) {
    pagination.innerHTML = "";
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
  setRequestPage(num)
    .then(res => res.json())
    .then(json => {
      Data = json;
      removeCurrentColor()
      pageData.currentPage = num;
      AppendToDataWrapper();
    });
}
