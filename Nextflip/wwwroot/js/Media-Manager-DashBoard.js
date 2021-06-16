let Data = {
  data: []
};
let requestParam = {
  NumberOfPage: 3,
  RowsOnPage: 4,
  RequestPage: 1
};
function renderRequest(request) {
  return `
      <tr>
          <td>${request.requestID}</td>
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


function CountCurrentLoadedPage() {
  return parseInt(Data.data.length / requestParam.RowsOnPage)
}


function setMaxPage(resolve) {
  fetch("/api/MediaManagerManagement/NumberOfPendingMedias")
    .then((res) => res.json())
    .then((json) => {
      rowsPerPage = requestParam.RowsOnPage;
      maxPage = parseInt(parseInt(json) / rowsPerPage);
      resolve("resolved");
    });
}

function preLoad() {
  return new Promise((resolve, reject) => {
    setMaxPage(resolve);
  });
}

function PostRequest(page) {
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  if (page !== 1) {
    requestParam.RequestPage = page;
  }
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  //
  return fetch(
    "/api/MediaManagerManagement/GetPendingMediasListAccordingRequest",
    initObject
  )
}

function RequestMoreData(RequestPage) {
  requestParam.RequestPage = RequestPage;
  console.log(Data.data.length);

  PostRequest(RequestPage).then(res => res.json()).then(json => {
    json.forEach(user => {
      Data.data.push(user);
    })
    let num = RequestPage - 1;
    appendRequest(num * rowsPerPage, (num + 1) * rowsPerPage);
    setCurrentColor(RequestPage);
  })
}


function Run(rowsPerPage) {
  PostRequest(1)
    .then((res) => res.json())
    .then((json) => {
      Data.data = json;
      appendRequest(0, rowsPerPage);
      appendPagination(Data.data.length, rowsPerPage);
      setCurrentColor(1);
      setClickToIndex(appendRequest);
    });
}
