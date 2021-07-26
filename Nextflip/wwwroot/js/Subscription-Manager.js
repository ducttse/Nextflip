let Data;
let requestParam = {
    Rows: 10,
    Page: 1,
    Status: "All",
    UserEmail: ""
}
let isSearched = false;
let isFiltered = false;
function formatDate(date) {
    return date.slice(0, 10).split("-").reverse().join("-");
}

function setRequestPage(num) {
    setPageDataCurrentPage(num);
    requestParam.Page = num;
    if (isSearched) {
        return searchOnly();
    }
    return getUserSubscriptionOnly();
}

function requestRefund(obj) {
    let userID = this.getAttribute("userID");
    let subID = this.getAttribute("subID");
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            userID: userID,
            subscriptionID: subID
        })
    };
    fetch("/api/UserManagerManagement/RefundSubscription", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Success") {

            }
        })
}

function renderSubscription(subscription, index) {
    let refund = `<button class="btn btn-primary" userID="${subscription.userID}" subID="${subscription.subscriptionID}" onclick="requestRefund(this)">Refund</button>`;
    let fakeRefund = `<button class="btn btn-primary disabled">Refund</button>`;
    return `
    <tr>
        <td class="">${index + 1}</td>
        <td class="">${subscription.userEmail}</td>
        <td class="text-center" >${formatDate(subscription.startDate)}</td>
        <td class="text-center" >${formatDate(subscription.endDate)}</td>
        <td class="text-center" >${subscription.price}</td>
        <td class="text-center" >${subscription.status}</td>
        <td class="text-center">
            ${subscription.canRefund ? refund : fakeRefund}
        </td>
    </tr> `;
}

function setTotalPage() {
    pageData.totalPage = Data.totalPage
}

function countStart() {
    return (pageData.currentPage - 1) * requestParam.Rows
}

function appendToDataWrapper() {
    setTotalPage();
    let start = countStart();
    let requestArray = Data.data.slice(0, requestParam.Rows).map((request, index) => {
        return renderSubscription(request, start + index);
    });
    requestArray = requestArray.join("");
    let requestWrapper = document.getElementById("dataWapper");
    if (requestWrapper.innerHTML !== "") {
        requestWrapper.innerHTML = "";
    }
    requestWrapper.insertAdjacentHTML("afterbegin", requestArray);
    appendCurrentArray();
}

function setRowsPerPage(obj) {
    requestParam.RowsOnPage = obj.value;
    requestParam.Page = 1;
    setPageDataCurrentPage(1);
    setRequestPage(requestParam.Page)
        .then(res => res.json())
        .then(json => {
            Data = json;
            appendToDataWrapper();
        })
}

function setStatus(obj) {
    if (obj.value == "All") {
        requestParam.Status = "All";
        isFiltered = false;
    }
    else {
        isFiltered = true;
        requestParam.Status = obj.value;
    }
    if (isSearched) {
        searchAndResetPage();
    }
    else getUserSubscriptionAndResetPage();
}

function getUserSubscriptionOnly() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestParam)
    };
    return fetch("/api/UserManagerManagement/GetSubscriptions", initObject)
}

function getUserSubscription() {
    getUserSubscriptionOnly()
        .then(res => res.json())
        .then(json => {
            Data = json;
            appendToDataWrapper();
        })
}

function getUserSubscriptionAndResetPage() {
    requestParam.Page = 1;
    setPageDataCurrentPage(1);
    getUserSubscription();
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
    return fetch("/api/UserManagerManagement/GetSubscriptionsByUserEmail", initObject)
}

function search(searchValue) {
    requestParam.UserEmail = searchValue;
    isSearched = true;
    searchOnly()
        .then(res => res.json())
        .then(json => {
            Data = json;
            appendToDataWrapper();
        })
}

function searchAndResetPage(searchValue) {
    requestParam.Page = 1;
    setPageDataCurrentPage(1);
    search(searchValue);
}
setAppendToDataWrapper(appendToDataWrapper);
