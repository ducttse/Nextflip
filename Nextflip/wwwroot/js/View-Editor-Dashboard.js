let Data;
let requestParam;
let isSearched = false;
let isFiltered = false;
function loadStorageData() {
    if (sessionStorage.getItem("requestParam") === null) {
        console.log("true")
        requestParam = {
            RowsOnPage: 10,
            RequestPage: 1,
            CategoryName: "action",
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
    return requestMediaData(requestParam.CategoryName);
}

function setTopic(topic) {
    requestParam.TopicName = topic;
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

function renderMedia(media, index) {
    return `
      <tr>
          <td>${index + 1}</td>
          <td>${media.title}</td>
          <td>${media.language}</td>
          <td>${media.status}</td>
          <td>
              <a class="text-decoration-none" 
              onclick="return storeToStorage();"
              href="/Edit/${media.mediaID}">
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

function appendMediaToWrapper() {
    setTotalPage();
    let start = countStart();
    let ticketArray = Data.data.slice(0, requestParam.RowsOnPage).map((ticket, index) => {
        return renderMedia(ticket, index + start);
    });
    ticketArray = ticketArray.join("");
    let dataWapper = document.getElementById("dataWapper");
    if (dataWapper.innerHTML !== "") {
        dataWapper.innerHTML = "";
    }
    dataWapper.insertAdjacentHTML("afterbegin", ticketArray);
    appendCurrentArray();
}

setAppendToDataWrapper(appendMediaToWrapper);

function requestMediaData(name) {
    requestParam.CategoryName = name;
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestParam)
    };
    return fetch("/api/ViewEditorDashboard/ViewMediasFilterCategory", initObject);
}

function requestMediaDataAndResetPage(name) {
    requestParam.RequestPage = 1;
    setPageDataCurrentPage(1);
    return requestMediaData(name);
}

function getCategories() {
    fetch("/api/ViewEditorDashboard/GetCategories")
        .then(res => res.json())
        .then(json => {
            TopicArr = json;
            appendCollase("Category", "name", requestMediaDataAndResetPage, appendMediaToWrapper);
            setRequestPage(requestParam.RequestPage)
                .then(res => res.json())
                .then(json => {
                    Data = json;
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
                    appendMediaToWrapper();
                    setChoosenColor(TopicArr.findIndex((item) => {
                        return item.topicName === requestParam.TopicName;
                    }))
                })
        })
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
        fetch("/api/ViewEditorDashboard/GetMediasByTitleFilterCategory", initObject)
            .then(res => res.json())
            .then(json => {
                if (json.totalPage == 0) {
                    ShowNotFound();
                }
                else {
                    HideNotFound();
                    Data = json;
                    pageData.currentPage = 1;
                    appendMediaToWrapper();
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
    return fetch("/api/ViewEditorDashboard/GetMediasByTitleFilterCategory", initObject)
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
    fetch("/api/ViewEditorDashboard/GetMediasByTitleFilterCategory_Status", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.totalPage == 0) {
                ShowNotFound();
            }
            else {
                HideNotFound();
                Data = json;
                pageData.currentPage = 1;
                appendMediaToWrapper();
            }
        });
}

function requestWithFilter() {
    isSearched = true;
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestParam)
    };
    return fetch("/api/ViewEditorDashboard/ViewMediasFilterCategory_Status", initObject);
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
    return fetch("/api/ViewEditorDashboard/GetMediasByTitleFilterCategory_Status", initObject)
}

function requestWithFilterAndResetPage() {
    requestParam.RequestPage = 1;
    setPageDataCurrentPage(1);
    return requestWithFilter();
}

function doFilter() {
    let filter = document.getElementById("filter");
    filter.addEventListener("change", () => {
        let choosenValue = filter.options[ filter.selectedIndex ].value;
        if (choosenValue === "All") {
            requestParam.Status = "";
            isFiltered = false;
            if (isSearched) {
                searchOnly(requestParam.SearchValue)
                    .then(res => res.json())
                    .then(json => {
                        Data = json;
                        HideNotFound();
                        appendMediaToWrapper();
                    })
                return;
            }
            else {
                requestMediaDataAndResetPage(requestParam.CategoryName)
                    .then(res => res.json())
                    .then(json => {

                        Data = json;
                        HideNotFound();
                        appendMediaToWrapper();
                    })
                return;
            }
        }
        isFiltered = true;
        requestParam.Status = choosenValue;
        if (isFiltered && isSearched) {
            searchWithFilter();
        }
        else {
            requestWithFilterAndResetPage()
                .then(res => res.json())
                .then(json => {
                    if (json.totalPage == 0) {
                        ShowNotFound();
                        return;
                    }
                    Data = json;
                    HideNotFound();
                    appendMediaToWrapper();
                    setChoosenColor(TopicArr.findIndex((item) => {
                        return item.topicName === requestParam.TopicName;
                    }))
                })
        }
    })
}

function resetSearch() {
    document.getElementById("search").value = "";
    requestParam.SearchValue = "";
    isSearched = false;
}

doFilter();


function resetFilter() {
    document.getElementById('filter').getElementsByTagName('option')[ 0 ].selected = 'selected';
    isFiltered = false;
    requestParam.Status = "";
}