let Data;
let requestParam = {
  RequestPage: 1,
  RowsOnPage: 4,
};

function setRequestPage(num) {
  console.log(num)
  requestParam.RequestPage = num;
  return requestPendingData();
}

function makeShortNote(text) {
  if (text.length < 200) {
    return text
  }
  return text.slice(0, 200) + "...";
}

function renderRequest(request, index) {
  let shortText = makeShortNote(request.note);
  return `
      <tr>
          <td>${index}</td> 
          <td>${request.userEmail}</td>
          <td>${shortText}</td>
          <td><a class="text-decoration-none" href="#${request.mediaID}">Preview</a></td>
      </tr>`;
}

function setTotalPage() {
  pageData.totalPage = Data.totalPage
}

function countStart() {
  return (pageData.currentPage - 1) * requestParam.RowsOnPage
}

function appendRequest() {
  setTotalPage();
  let start = countStart();
  let requestArray = Data.data.slice(0, requestParam.RowsOnPage).map((request, index) => {
    return renderRequest(request, start + index);
  });
  requestArray = requestArray.join("");
  let requestWrapper = document.getElementById("requestWrapper");
  if (requestWrapper.innerHTML !== "") {
    requestWrapper.innerHTML = "";
  }
  requestWrapper.insertAdjacentHTML("afterbegin", requestArray);
  appendCurrentArray();
}

setAppendToDataWrapper(appendRequest);

function requestPendingData() {
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  return fetch("/api/MediaManagerManagement/GetPendingMediasListAccordingRequest", initObject)
}

function Start() {
  requestPendingData()
    .then(res => res.json())
    .then(json => {
      Data = json;
      console.log(Data);
      appendRequest();
    })
}