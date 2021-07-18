let Data;
let requestParam = {
    RowsOnPage: 12,
    RequestPage: 1,
    CategoryID: "all",
    SearchValue: "",
    Status: ""
};
let isSearched = false;
let isFiltered = false;

function setRequestPage(num) {
    requestParam.RequestPage = num;
    if (isFiltered && isSearched) {
        return searchWithFilterOnly();
    }
    else if (isSearched) {
        return searchOnly();
    }
    else if (isFiltered) {
        return requestWithFilterOnly();
    }
    return requestMediaDataOnly();
}

function ShowNotFound() {
    let error;
    setTotalPage();
    if (isFiltered && isSearched) {
        error = `<p class="fs-6">There is no <b>${requestParam.Status}</b> media in this category contain  <b>${requestParam.SearchValue}</b></p>`
    }
    else if (isFiltered) {
        error = `<p class="fs-6">There is no <b>${requestParam.Status}</b> media for this category</p>`
    }
    else { error = `<p class="fs-6">There is no result for <b>${requestParam.SearchValue}</b></p>` }
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

function renderMedia(media, index) {
    return `
      <tr>
          <td>${index + 1}</td>
          <td>${media.title}</td>
          <td class="text-center">
            <div>
                <input class="status_btn" type="checkbox" mediaID="${media.mediaID}" value="${media.status}" ${media.status === "Enabled" ? "checked" : ""}  />
            </div>
          </td>
          <td  class="text-center">
            <div class="btn btn-secondary" onclick="showAddSeasonForm('${media.mediaID}');">Add new season</div>
            <div class="btn btn-secondary" onclick="getSeasons('${media.mediaID}');">Add new episode</div>
            <div class="btn btn-secondary" onclick="return showEditForm();">Edit</div>
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
    addSetStatusEvent();
}

setAppendToDataWrapper(appendMediaToWrapper);

function setRowsPerPage(obj) {
    requestParam.RowsOnPage = obj.value;
    setRequestPage(requestParam.RequestPage)
        .then(res => res.json())
        .then(json => {
            Data = json;
            appendMediaToWrapper();
        })
}

function setSelectedCategory(obj) {
    requestParam.CategoryName = obj.value;
    if (isSearched) {
        searchAndResetPage(requestParam.SearchValue);
    }
    else if (isFiltered) {
        requestWithFilterAndResetPage();
    }
    else requestMediaDataAndResetPage();
}

function setSelectedStatus(obj) {
    requestParam.Status = obj.value;
    if (requestParam.Status != "All") {
        isFiltered = true;
    }
    else isFiltered = false;
    if (isSearched) {
        searchAndResetPage(requestParam.SearchValue);
    }
    else requestWithFilterAndResetPage();
}

function requestMediaDataOnly() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestParam)
    };
    let url = (requestParam.CategoryName == "all")
        ? "/api/ViewEditorDashboard/ViewAllMedia"
        : "/api/ViewEditorDashboard/ViewMediasFilterCategory";
    return fetch(url, initObject);
}

function requestMediaData() {
    requestMediaDataOnly()
        .then(res => res.json())
        .then(json => {
            console.log(json);
            Data = json;
            appendMediaToWrapper();
        })
}

function requestMediaDataAndResetPage() {
    requestParam.RequestPage = 1;
    setPageDataCurrentPage(1);
    requestMediaData();
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
    let url = (requestParam.CategoryName == "all")
        ? (isFiltered ? "/api/ViewEditorDashboard/ViewAllMediaFilterStatus" : "/api/ViewEditorDashboard/ViewAllMedia")
        : (isFiltered ? "/api/ViewEditorDashboard/ViewMediasFilterCategory_Status" : "/api/ViewEditorDashboard/ViewMediasFilterCategory");
    return fetch(url, initObject);
}

function requestWithFilter() {
    requestWithFilterOnly()
        .then(res => res.json())
        .then(json => {
            if (json.totalPage == 0) {
                ShowNotFound();
            }
            else {
                HideNotFound();
                Data = json;
                appendMediaToWrapper();
            }
        })
}

function requestWithFilterAndResetPage() {
    requestParam.RequestPage = 1;
    setPageDataCurrentPage(1);
    requestWithFilter();
}

function searchOnly(searchValue) {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestParam)
    };
    let url = (requestParam.CategoryName == "all")
        ? (isFiltered ? "/api/ViewEditorDashboard/GetMediasByTitleFilterStatus" : "/api/ViewEditorDashboard/GetMediasByTitle")
        : (isFiltered ? "/api/ViewEditorDashboard/GetMediasByTitleFilterCategory_Status" : "/api/ViewEditorDashboard/GetMediasByTitleFilterCategory");
    return fetch(url, initObject);
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
    let url = (requestParam.CategoryName == "all")
        ? (isFiltered ? "/api/ViewEditorDashboard/GetMediasByTitleFilterStatus" : "/api/ViewEditorDashboard/GetMediasByTitle")
        : (isFiltered ? "/api/ViewEditorDashboard/GetMediasByTitleFilterCategory_Status" : "/api/ViewEditorDashboard/GetMediasByTitleFilterCategory");
    return fetch(url, initObject);
}

function search(searchValue) {
    requestParam.SearchValue = searchValue;
    let searchFunc = searchOnly;
    isSearched = true;
    if (isFiltered) {
        searchFunc = searchWithFilterOnly;
    }
    searchFunc(searchValue)
        .then(res => res.json())
        .then(json => {
            console.log(json);
            if (json.totalPage != null) {
                Data = json;
                if (parseInt(json.totalPage) == 0) {
                    ShowNotFound();
                }
                else {
                    HideNotFound();
                    pageData.currentPage = 1;
                    appendMediaToWrapper();
                };
            }
            else {
                ShowNotFound();
            }
        })
}

function searchAndResetPage(searchValue) {
    console.log(searchValue);
    if (searchValue.trim().length == 0) {
        requestParam.searchValue = "";
        return;
    }
    requestParam.RequestPage = 1;
    setPageDataCurrentPage(1);
    search(searchValue);
}

function resetSearch() {
    document.getElementById("search").value = "";
    requestParam.SearchValue = "";
    isSearched = false;
}

function resetFilter() {
    document.getElementById('filter').getElementsByTagName('option')[ 0 ].selected = 'selected';
    isFiltered = false;
    requestParam.Status = "";
}