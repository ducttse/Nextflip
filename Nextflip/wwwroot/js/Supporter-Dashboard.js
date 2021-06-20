let Data = {
  data: []
};
let requestParam = {
  RowsOnPage: 10,
  RequestPage: 1,
  TopicName: "Account",
  SearchValue: "",
  Status: ""
};
let isSearched = false;
let isFiltered = false;
function setTopic(topic) {
  requestParam.TopicName = topic;
}

function setRequestPage(num) {
  requestParam.RequestPage = num;
  if (isFiltered && isSearched) {
    return searchWithFilterOnly();
  }
  else if (isSearched) {
    return searchOnly(requestParam.SearchValue);
  }
  else if (isFiltered) {
    return requestWithFilter();
  }
  return requestTopicData(requestParam.TopicName);
}

function ShowNotFound() {
  let error = `<p>There is no result for <b>${requestParam.SearchValue}</b></p>`;
  let notFound = document.getElementById("notFound");
  if (notFound.innerHTML != "") {
    notFound.innerHTML = "";
  }
  notFound.insertAdjacentHTML("afterbegin", error);
  notFound.classList.remove("hide");
  document.getElementById("table_holder").classList.add("hide");
  document.getElementById("filter").setAttribute("disabled", "disabled");
}

function HideNotFound() {
  let notFound = document.getElementById("notFound");
  if (!notFound.classList.contains("hide")) {
    notFound.classList.add("hide");
    document.getElementById("table_holder").classList.remove("hide");
    document.getElementById("filter").removeAttribute("disabled");
  }
}

function renderTicket(ticket, index) {
  return `
      <tr>
          <td>${index + 1}</td>
          <td>${ticket.userEmail}</td>
          <td>${ticket.topicName}</td>
          <td>${ticket.status}</td>
          <td>
              <a class="text-decoration-none" 
              href="/SupporterDashboard/Detail/${ticket.supportTicketID}">
                  Detail
              </a>
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

function requestTopicData(topic) {
  requestParam.TopicName = topic;
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

function search(searchValue) {
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
  requestParam.SearchValue = searchValue;
  if (searchValue == "") {
    isSearched = false;
    return;
  }
  isSearched = true;
  requestParam.SearchValue = searchValue;
  if (isFiltered && isSearched) {
    searchWithFilter()
  }
  else {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
      method: "POST",
      headers: reqHeader,
      body: JSON.stringify(requestParam)
    };
    fetch("/api/ViewSupporterDashboard/SearchSupportTicketByTopic", initObject)
      .then(res => res.json())
      .then(json => {
        if (json.totalPage == 0) {
          ShowNotFound();
        }
        else {
          HideNotFound();
          Data = json;
          pageData.currentPage = 1;
          appendTicketToWrapper();
        }
      })
  }
}

function searchOnly(searchValue) {
  requestParam.SearchValue = searchValue;
  if (searchValue == "") {
    isSearched = false;
    return;
  }
  isSearched = true;
  requestParam.SearchValue = searchValue;
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
  if (searchValue == "") {
    isSearched = false;
    return;
  }
  isSearched = true;
  requestParam.SearchValue = searchValue;
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  fetch("/api/ViewSupporterDashboard/SearchSupportTicketByTopicAndStatus", initObject)
    .then(res => res.json())
    .then(json => {
      if (json.totalPage == 0) {
        ShowNotFound();
      }
      else {
        HideNotFound();
        Data = json;
        pageData.currentPage = 1;
        appendTicketToWrapper();
      }
    });
}

function searchWithFilterOnly() {
  if (searchValue == "") {
    isSearched = false;
    return;
  }
  isSearched = true;
  requestParam.SearchValue = searchValue;
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

function getTopics() {
  fetch("/api/ViewSupporterDashboard/GetAllSupportTopics")
    .then(res => res.json())
    .then(json => {
      TopicArr = json;
      appendCollase("Topic", "topicName", requestTopicData, appendTicketToWrapper);
      requestTopicData("Account")
        .then(res => res.json())
        .then(json => {
          Data = json;
          appendTicketToWrapper();
          setChoosenColor(0);
        })
    })
}

function resetSearch() {
  document.getElementById("search").value = "";
  requestParam.SearchValue = "";
  isSearched = false;
}

function requestWithFilter() {
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
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

function doFilter() {
  let filter = document.getElementById("filter");
  filter.addEventListener("change", () => {
    let choosenValue = filter.options[ filter.selectedIndex ].value;
    if (choosenValue === "All") {
      requestTopicData(requestParam.TopicName)
        .then(res => res.json())
        .then(json => {
          Data = json;
          appendTicketToWrapper();
        })
      isFiltered = false;
      return;
    }
    isFiltered = true;
    requestParam.Status = choosenValue;
    if (isFiltered && isSearched) {
      searchWithFilter();
    }
    else {
      requestWithFilter()
        .then(res => res.json())
        .then(json => {
          Data = json;
          appendTicketToWrapper();
          setChoosenColor(TopicArr.findIndex((role) => {
            console.log(role);
            return role.topicName === requestParam.TopicName;
          }))
        })
    }
  })
}

function resetFilter() {
  document.getElementById('filter').getElementsByTagName('option')[ 0 ].selected = 'selected';
  isFiltered = false;
  requestParam.Status = "";
}
doFilter();
