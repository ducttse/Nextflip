let Data = {
  data: []
};
let requestParam;
let isSearched = false;
let isFiltered = false;

function loadStorageData() {
  if (sessionStorage.getItem("requestParam") === null) {
    console.log("true")
    requestParam = {
      RowsOnPage: 8,
      RequestPage: 1,
      TopicName: "Account",
      SearchValue: "",
      Status: ""
    };
  }
  else {
    requestParam = JSON.parse(sessionStorage.getItem("requestParam"));
  }
  if (sessionStorage.getItem("isSearched") === null) {
    isSearched = false;
  }
  else {
    isSearched = (sessionStorage.getItem("isSearched") === 'true');
  }
  if (sessionStorage.getItem("isFiltered") === null) {
    isFiltered = false;
  }
  else {
    isFiltered = (sessionStorage.getItem("isFiltered") === 'true');
  }
}
loadStorageData();

function setTopic(topic) {
  requestParam.TopicName = topic;
}

function setRequestPage(num) {
  requestParam.RequestPage = num;
  if (isFiltered && isSearched) {
    console.log("1");
    return searchWithFilterOnly();
  }
  else if (isSearched) {
    console.log("2");
    return searchOnly(requestParam.SearchValue);
  }
  else if (isFiltered) {
    console.log("3");
    return requestWithFilter();
  }
  console.log("4");
  return requestTopicData(requestParam.TopicName);
}

function ShowNotFound() {
  let error;
  if (isFiltered) {
    error = `<p>There is no <b>${requestParam.Status}</b> ticket for this topic</p>`
  }
  else { error = `<p>There is no result for <b>${requestParam.SearchValue}</b></p>` }
  let notFound = document.getElementById("notFound");
  if (notFound.innerHTML != "") {
    notFound.innerHTML = "";
  }
  notFound.insertAdjacentHTML("afterbegin", error);
  notFound.classList.remove("hide");
  document.getElementById("table_holder").classList.add("hide");
  if (!isFiltered) {
    document.getElementById("filter").setAttribute("disabled", "disabled");
  }
}

function HideNotFound() {
  let notFound = document.getElementById("notFound");
  if (!notFound.classList.contains("hide")) {
    notFound.classList.add("hide");
    document.getElementById("table_holder").classList.remove("hide");
    if (!isFiltered) {
      document.getElementById("filter").removeAttribute("disabled");
    }
  }
}

function makeShortContent(content) {
  if (content.length > 100) {
    return content.slice(0, 100) + "..."
  }
  return content
}

function renderTicket(ticket, index) {
  let shortContent = makeShortContent(ticket.content);
  return `
      <tr style="min-height: 70px;">
          <td>${index + 1}</td>
          <td>${ticket.userEmail}</td>
          <td>${ticket.status}</td>
          <td>${shortContent}</td>
          <td>
              <a class="text-decoration-none" 
              onclick="return storeToStorage();"
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

function requestTopicDataAndResetPahe(topic) {
  requestParam.RequestPage = 1;
  return requestTopicData(topic);
}

function search(searchValue) {
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
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

function searchWithFilter() {
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
  isSearched = true;
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
      appendCollase("Topic", "topicName", requestTopicDataAndResetPahe, appendTicketToWrapper);
      setRequestPage(requestParam.RequestPage)
        .then(res => res.json())
        .then(json => {
          Data = json;
          console.log(isSearched);
          console.log(isFiltered);
          if (isSearched) {
            console.log("run");
            document.getElementById("search").value = requestParam.SearchValue;
          }
          if (isFiltered) {
            let options = document.getElementById('filter').getElementsByTagName('option');
            for (let i = 0; i < options.length; i++) {
              if (options[ i ].value == requestParam.Status) {
                options[ i ].selected = 'selected';
              }
            }
          }
          appendTicketToWrapper();
          setChoosenColor(TopicArr.findIndex((item) => {
            return item.topicName === requestParam.TopicName;
          }))
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
      requestParam.Status = "";
      if (isSearched) {
        searchOnly(requestParam.SearchValue)
          .then(res => res.json())
          .then(json => {
            Data = json;
            appendTicketToWrapper();
          })
        return;
      }
      else {
        requestTopicData(requestParam.TopicName)
          .then(res => res.json())
          .then(json => {
            Data = json;
            appendTicketToWrapper();
          })
        isFiltered = false;
        return;
      }
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
          if (json.totalPage == 0) {
            ShowNotFound();
            return;
          }
          Data = json;
          HideNotFound();
          appendTicketToWrapper();
          setChoosenColor(TopicArr.findIndex((item) => {
            return item.topicName === requestParam.TopicName;
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


function storeToStorage() {
  sessionStorage.setItem("requestParam", JSON.stringify(requestParam));
  sessionStorage.setItem("isFiltered", isFiltered.toString());
  sessionStorage.setItem("isSearched", isSearched.toString());
}