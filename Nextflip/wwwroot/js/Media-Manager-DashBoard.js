let Data;
let requestParam = {
  SearchValue: "",
  Status: "All",
  Type: "All",
  RowsOnPage: 6,
  RequestPage: 1,
  SortBy: "desc"
};
let isFiltered = false;
let isSearched = false;
function setRequestPage(num) {
  requestParam.RequestPage = num;
  return requestEditRequestDataOnly();
}

function ShowNotFound() {
  let error;
  setTotalPage();
  if (isFiltered && isSearched) {
    error = `<p class="fs-6">There is no <b>${requestParam.Status}</b> request in this role contain  <b>${requestParam.SearchValue}</b></p>`
  }
  else if (isFiltered) {
    error = `<p class="fs-6">There is no <b>${requestParam.Status}</b> request for this type</p>`
  }
  else if (isSearched) { error = `<p class="fs-6">There is no result for <b>${requestParam.SearchValue}</b></p>` }
  else { error = `<p class="fs-6">There is no request in this type</p>` }
  let notFound = document.getElementById("notFound");
  if (notFound.innerHTML != "") {
    notFound.innerHTML = "";
  }
  notFound.insertAdjacentHTML("afterbegin", error);
  notFound.classList.remove("hide");
  document.getElementById("table_holder").classList.add("hide");
  document.getElementById("pagination").classList.add("hide");
  if (!isFiltered) {
    document.getElementById("status_filter").setAttribute("disabled", "disabled");
  }
}

function HideNotFound() {
  let notFound = document.getElementById("notFound");
  if (!notFound.classList.contains("hide")) {
    notFound.classList.add("hide");
    document.getElementById("table_holder").classList.remove("hide");
    document.getElementById("pagination").classList.remove("hide");
    if (!isFiltered) {
      document.getElementById("status_filter").removeAttribute("disabled");
    }
  }
}

function makeShortNote(text) {
  if (text.length < 130) {
    return text
  }
  return text.slice(0, 130) + "...";
}

function renderRequest(request, index) {
  let bgcolor;
  switch (request.status) {
    case "Disapproved":
      bgcolor = "bg-danger";
      break;
    case "Pending":
      bgcolor = "bg-warning";
      break;
    case "Approved":
      bgcolor = "bg-primary";
      break;
  }
  let shortText = makeShortNote(request.note);
  if (isFiltered) {
    return `
    <tr>
        <td>${index + 1}</td> 
        <td>${request.userEmail}</td>
        <td>${shortText}</td>
        <td class="text-center"><a class="text-decoration-none" href="/MediaManagerManagement/DetailPreview/${request.mediaID}/${request.requestID}">Preview</a></td>
    </tr>`;
  }
  return `
      <tr>
          <td>${index + 1}</td> 
          <td>${request.userEmail}</td>
          <td>${shortText}</td>
          <td class="text-center"><p class="ticket_status ${bgcolor} rounded text-center text-light text-center px-2 py-1">${request.status}</p></td>
          <td class="text-center"><a class="text-decoration-none" href="/MediaManagerManagement/DetailPreview/${request.type}/${request.mediaID}/${request.requestID}">Preview</a></td>
      </tr>`;
}

function setTotalPage() {
  pageData.totalPage = Data.totalPage
}

function countStart() {
  return (pageData.currentPage - 1) * requestParam.RowsOnPage
}

function appendRequestToWrapper() {
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

setAppendToDataWrapper(appendRequestToWrapper);

function setSeletedType(obj) {
  requestParam.Type = obj.value;
  if (isFiltered) {
    requestWithFilterAndResetPage();
  }
  else if (isSearched) {
    searchAndResetPage
  }
  else requestEditRequestDataAndResetPage();
}

function setRowsPerPage(obj) {
  requestParam.RowsOnPage = obj.value;
  setRequestPage(requestParam.RequestPage)
    .then(res => res.json())
    .then(json => {
      Data = json;
      appendRequestToWrapper();
    })
}

function removeCurrentStatus() {
  let current = document.querySelector(".me-4.chosen")
  if (current != null) {
    current.classList.remove("chosen");
  }
}

function setStatus(obj) {
  let value = obj.getAttribute("value");
  if (requestParam.Status == value) {
    return;
  }
  requestParam.Status = value;
  removeCurrentStatus();
  obj.classList.add("chosen");
  if (value == "All") {
    isFiltered = false;
    if (isSearched) {
      searchAndResetPage(requestParam.SearchValue);
    }
    else requestEditRequestDataAndResetPage();
    document.getElementById("status").classList.remove("d-none");
  }
  else {
    isFiltered = true;
    if (isSearched) {
      searchAndResetPage(requestParam.SearchValue);
    }
    else requestWithFilterAndResetPage();
    document.getElementById("status").classList.add("d-none");
  }
}

function requestEditRequestDataOnly() {
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  return fetch("/api/MediaManagerManagement/GetMediaRequest", initObject);
}

function requestEditRequestData() {
  requestEditRequestDataOnly()
    .then(res => res.json())
    .then(json => {
      if (json.totalPage == 0) {
        ShowNotFound();
      }
      else {
        HideNotFound();
        Data = json;
        appendRequestToWrapper();
      }
    })
}

function requestEditRequestDataAndResetPage() {
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
  requestEditRequestData();
}

function requestWithFilterOnly() {
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  return fetch("/api/MediaManagerManagement/GetMediaRequest", initObject);
}

function requestWithFilter() {
  requestWithFilterOnly()
    .then(res => res.json())
    .then(json => {
      isFiltered = true;
      if (json.totalPage == 0) {
        ShowNotFound();
      }
      else {
        Data = json;
        HideNotFound();
        appendRequestToWrapper();
      }
    })
}

function requestWithFilterAndResetPage() {
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
  requestWithFilter();
}

function searchOnly() {
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  return fetch("/api/MediaManagerManagement/SearchingMediaRequest", initObject);
}

function search(searchValue) {
  requestParam.SearchValue = searchValue;
  isSearched = true;
  searchOnly()
    .then(res => res.json())
    .then(json => {
      if (json.totalPage == 0) {
        ShowNotFound();
      }
      else {
        HideNotFound();
        Data = json;
        appendRequestToWrapper();
      }
    })
}

function searchAndResetPage(searchValue) {
  if (searchValue.trim().length == 0) {
    requestParam.searchValue = "";
    return;
  }
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
  search(searchValue);
}