let Data;
let requestParam = {
    SearchValue: "",
    UserEmail: "",
    Status: "All",
    RowsOnPage: 12,
    RequestPage: 1
}
let isSearched = false;
let isFiltered = false;
function renderEditRequest(request, index) {
    return `
<tr>
    <td>${index + 1}</td>
    <td>${request.mediaTitle}</td>
    <td>${request.note}</td>
    <td>${request.status}</td>
</tr>`;
}

function setRequestPage(num) {
    requestParam.RequestPage = num;
    if (isSearched) {
        return searchOnly();
    }
    else return requestRequestDataOnly();
}

function ShowNotFound() {
    let error;
    setTotalPage();
    if (isFiltered && isSearched) {
        error = `<p class="fs-6">There is no <b>${requestParam.Status}</b> edit request contain  <b>${requestParam.SearchValue}</b></p>`
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

function setTotalPage() {
    pageData.totalPage = Data.totalPage
}

function countStart() {
    return (pageData.currentPage - 1) * requestParam.RowsOnPage
}

function appendToRequestWrapper() {
    setTotalPage();
    let start = countStart();
    let ticketArray = Data.data.slice(0, requestParam.RowsOnPage).map((ticket, index) => {
        return renderEditRequest(ticket, index + start);
    });
    ticketArray = ticketArray.join("");
    let dataWapper = document.getElementById("dataWapper");
    if (dataWapper.innerHTML !== "") {
        dataWapper.innerHTML = "";
    }
    dataWapper.insertAdjacentHTML("afterbegin", ticketArray);
    appendCurrentArray();
}

setAppendToDataWrapper(appendToRequestWrapper);

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
    else requestRequestDataAndResetPage();
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
    else requestRequestDataAndResetPage();
}

async function requestRequestDataOnly() {
    await loadAccount();
    requestParam.UserEmail = getProfile().userEmail;
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestParam)
    };
    return fetch("/api/ViewEditorDashboard/GetRequestMediaFilterStatus/", initObject);
}

function requestRequestData() {
    requestRequestDataOnly()
        .then(res => res.json())
        .then(json => {
            if (json.totalPage == 0) {
                ShowNotFound();
            }
            else {
                HideNotFound();
                Data = json;
                appendToRequestWrapper();
            }
        })
}

function requestRequestDataAndResetPage() {
    requestParam.RequestPage = 1;
    setPageDataCurrentPage(1);
    requestRequestData();
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
    return fetch("/api/ViewEditorDashboard/SearchingRequestMediaFilterStatus/", initObject);
}

function search(searchValue) {
    requestParam.SearchValue = searchValue;
    searchOnly()
        .then(res => res.json())
        .then(json => {
            isSearched = true;
            if (json.totalPage == 0) {
                ShowNotFound();
            }
            else {
                HideNotFound();
                Data = json;
                appendToRequestWrapper();
            }
        })
}

function searchAndResetPage(searchValue) {
    requestParam.RequestPage = 1;
    setPageDataCurrentPage(1);
    search(searchValue);
}