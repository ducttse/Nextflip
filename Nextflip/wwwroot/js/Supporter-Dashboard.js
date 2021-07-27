let Data = {
  data: []
};
let requestParam = {
  RowsOnPage: 8,
  RequestPage: 1,
  TopicName: "All",
  SearchValue: "",
  Status: "",
  SortBy: "createdDate",
  According: "ASC"
};
let isSearched = false;
let isFiltered = false;

function setTopic(topic) {
  requestParam.TopicName = topic;
}

function setRequestPage(num) {
  setPageDataCurrentPage(num);
  requestParam.RequestPage = num;
  if (isFiltered && isSearched) {
    return searchWithFilterOnly();
  }
  else if (isSearched) {
    return searchOnly(requestParam.SearchValue);
  }
  else if (isFiltered) {
    return requestWithFilterOnly();
  }
  return requestTopicDataOnly();
}

function ShowNotFound() {
  let error;
  if (isFiltered && isSearched) {
    error = `<p class="fs-6">There is no <b>${requestParam.Status}</b> ticket in this topic contain <b>${requestParam.SearchValue}</b></p>`
  }
  else if (isFiltered) {
    error = `<p class="fs-6">There is no <b>${requestParam.Status}</b> ticket for this topic</p>`
  }
  else { error = `<p class="fs-5">There is no result for <b>${requestParam.SearchValue}</b></p>` }
  let notFound = document.getElementById("notFound");
  if (notFound.innerHTML != "") {
    notFound.innerHTML = "";
  }
  notFound.insertAdjacentHTML("afterbegin", error);
  notFound.classList.remove("hide");
  document.getElementById("table_holder").classList.add("hide");
}

function HideNotFound() {
  let notFound = document.getElementById("notFound");
  if (!notFound.classList.contains("hide")) {
    notFound.classList.add("hide");
    document.getElementById("table_holder").classList.remove("hide");
  }
}

function makeShortContent(content) {
  if (content.length > 100) {
    return content.slice(0, 100) + "..."
  }
  return content
}

function renderTicket(ticket, index) {
  let bgcolor;
  switch (ticket.status) {
    case "Closed":
      bgcolor = "bg-danger";
      break;
    case "Pending":
      bgcolor = "bg-warning";
      break;
    case "Assigned":
      bgcolor = "bg-primary";
      break;
  }

  let shortContent = makeShortContent(ticket.content);
  let topic;
  if (document.getElementById("status_filter").value != "All") {
    topic = "";
    document.getElementById("topic").classList.add("d-none");
  }
  else {
    console.log(ticket);
    topic = `<td class="text-center">${ticket.topicName}</td>`;
    document.getElementById("topic").classList.remove("d-none");
  }
  if (isFiltered) {
    return `
    <tr>
        <td class="text-center">${index + 1}</td>
        <td>${ticket.userEmail}</td>
        <td class="text-center">${ticket.createdDate.slice(0, 9)}</td>
        ${topic}
        <td>${shortContent}</td>
        <td>
            <p class="detail_btn" onclick="showDetail('${ticket.supportTicketID}')">
            Detail
            </p>
        </td>
    </tr>`;
  }
  return `
      <tr>
          <td class="text-center">${index + 1}</td>
          <td>${ticket.userEmail}</td>
          <td class="text-center">${ticket.createdDate.slice(0, 10)}</td>
          ${topic}
          <td>${shortContent}</td>
          <td><p class="ticket_status ${bgcolor} rounded text-center text-light px-2 py-1">${ticket.status}<p></td>
          <td>
              <p class="detail_btn" onclick="showDetail('${ticket.supportTicketID}')">
              Detail
              </p>
          </td>
      </tr>`;
}

function setTotalPage() {
  pageData.totalPage = Data.totalPage
}

function countStart() {
  return (pageData.currentPage - 1) * requestParam.RowsOnPage
}

function appendTicketToWrapper() {
  setTotalPage();
  let start = countStart();
  let ticketArray = Data.data.slice(0, requestParam.RowsOnPage).map((ticket, index) => {
    return renderTicket(ticket, index + start);
  });
  ticketArray = ticketArray.join("");
  let dataWapper = document.getElementById("dataWapper");
  if (dataWapper.innerHTML !== "") {
    dataWapper.innerHTML = "";
  }
  dataWapper.insertAdjacentHTML("afterbegin", ticketArray);
  appendCurrentArray();
}

setAppendToDataWrapper(appendTicketToWrapper);

function setRowsPerPage(obj) {
  requestParam.RowsOnPage = obj.value;
  setRequestPage(requestParam.RequestPage)
    .then(res => res.json())
    .then(json => {
      Data = json;
      appendTicketToWrapper();
    })
}

function setSelectedTopic(obj) {
  requestParam.TopicName = obj.value;
  if (isFiltered) {
    requestWithFilterAndResetPage();
  }
  else if (isSearched) {
    searchAndResetPage(requestParam.SearchValue);
  }
  else requestTopicDataAndResetPage();
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
  removeCurrentStatus();
  obj.classList.add("chosen");
  requestParam.Status = value;
  if (value.length == 0) {
    isFiltered = false;
    if (isSearched) {
      searchAndResetPage(requestParam.SearchValue);
    }
    else requestTopicDataAndResetPage();
    //show
    document.getElementById("status").classList.remove("d-none");
  }
  else {
    isFiltered = true;
    if (isSearched) {
      searchAndResetPage(requestParam.SearchValue);
    }
    else requestWithFilterAndResetPage();
    // Hide column 
    document.getElementById("status").classList.add("d-none");
  }

}

function requestTopicDataOnly() {
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  return fetch("/api/ViewSupporterDashboard/ViewSupportTicketByTopic", initObject);
}

function requestTopicData() {
  requestTopicDataOnly()
    .then(res => res.json())
    .then((json) => {
      Data = json;
      appendTicketToWrapper();
    })
}

function requestTopicDataAndResetPage() {
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
  requestTopicData();
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
  return fetch("/api/ViewSupporterDashboard/ViewSupportTicketByTopicAndStatus", initObject);
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
        appendTicketToWrapper();
        HideNotFound();
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
  return fetch("/api/ViewSupporterDashboard/SearchSupportTicketByTopic", initObject)
}

function searchWithFilterOnly() {
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  return fetch("/api/ViewSupporterDashboard/SearchSupportTicketByTopicAndStatus", initObject)
}

function search(searchValue) {
  requestParam.SearchValue = searchValue;
  isSearched = true;
  let searchFunc = searchOnly;
  if (isFiltered) {
    searchFunc = searchWithFilterOnly;
  }
  searchFunc()
    .then(res => res.json())
    .then(json => {
      if (json.totalPage == 0) {
        ShowNotFound();
      }
      else {
        HideNotFound();
        Data = json;
        appendTicketToWrapper();
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

function sort(obj) {
  let list = obj.classList;
  if (list.contains("fa-sort-up")) {
    list.remove("fa-sort-up");
    list.add("fa-sort-down");
    requestParam.According = "DESC"
  }
  else {
    list.remove("fa-sort-down");
    list.add("fa-sort-up");
    requestParam.According = "ASC"
  }
  let func;
  if (isSearched) {
    func = searchAndResetPage;
  }
  else if (isFiltered) {
    func = requestWithFilterAndResetPage;
  }
  else {
    func = requestTopicDataAndResetPage;
  }
  func();
}

function resetSearch() {
  document.getElementById("search").value = "";
  requestParam.SearchValue = "";
  isSearched = false;
}
