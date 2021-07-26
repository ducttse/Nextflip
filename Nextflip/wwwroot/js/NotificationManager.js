let Data;
let requestParam = {
    RowsOnPage: 8,
    RequestPage: 1,
    Status: "",
    SearchValue: ""
}
let isSearched = false;
let isFiltered = false;
function setRequestPage(num) {
    setPageDataCurrentPage(num);
    requestParam.RequestPage = num;
    if (isSearched) {
        return searchOnly();
    }
    return requestNotificationsOnly();
}

function renderNotification(notification, index) {
    let date = notification.publishedDate.slice(0, 10).split("-").reverse().join("-");
    let shortCotent = notification.content.length > 100 ? notification.content.slice(0, 100) + "..." : notification.content;
    return `
    <tr>
        <td class="col">${index + 1}</td>
        <td class="col">${notification.title}</td>
        <td class="col">${shortCotent}</td>
        <td class="col text-center">${date}</td>
        <td class="col text-center">${notification.status}</td>
        <td class="col link-primary text-center" style="cursor: pointer" notiID="${notification.notificationID}"  onclick="EditNotification(this)">Edit</td>
    </tr>`
}

function setTotalPage() {
    pageData.totalPage = Data.totalPage
}

function countStart() {
    return (pageData.currentPage - 1) * requestParam.RowsOnPage
}

function appendToDataWrapper() {
    setTotalPage();
    let start = countStart();
    let requestArray = Data.data.slice(0, requestParam.RowsOnPage).map((request, index) => {
        return renderNotification(request, start + index);
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
    requestParam.RequestPage = 1;
    setPageDataCurrentPage(1);
    setRequestPage(requestParam.RequestPage)
        .then(res => res.json())
        .then(json => {
            Data = json;
            appendToDataWrapper();
        })
}

function setStatus(obj) {
    let status = obj.value;
    if (status == "All") {
        requestParam.Status = "";
    }
    else {
        requestParam.Status = obj.value;
    }
    requestNotificationsAndResetPage();
}

function requestNotificationsOnly() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestParam)
    };
    return fetch("/api/NotificationBoard/ViewNotifications", initObject);

}

function requestNotifications() {
    requestNotificationsOnly()
        .then(res => res.json())
        .then(json => {
            Data = json;
            appendToDataWrapper();
        })
}

function requestNotificationsAndResetPage() {
    requestParam.RequestPage = 1;
    setPageDataCurrentPage(1);
    requestNotifications();
}

function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
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
    return fetch("/api/NotificationBoard/SearchNotifications", initObject)
}

function searchNotifications(searchValue) {
    requestParam.SearchValue = searchValue;
    isSearched = true;
    searchOnly()
        .then(res => res.json())
        .then(json => {
            Data = json;
            appendToDataWrapper();
        })
}
function searchAndResetPage(searchValue) {
    requestParam.RequestPage = 1;
    setPageDataCurrentPage(1);
    searchNotifications(searchValue);
}

const onInput = debounce((obj) => {
    let parent = obj.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    parent.classList.add("was-validated");
    if (obj.value.trim().length == 0) {
        feedback.textContent = "This field can not empty";
        obj.setCustomValidity("empty");
    }
    else {
        parent.classList.remove("was-validated");
        feedback.textContent = "";
        obj.setCustomValidity("");
    }
});

function checkEmpty(obj) {
    let parent = obj.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    parent.classList.add("was-validated");
    if (obj.value.trim().length == 0) {
        feedback.textContent = "This field can not empty";
        obj.setCustomValidity("empty");
        return false;
    }
    else {
        parent.classList.remove("was-validated");
        feedback.textContent = "";
        obj.setCustomValidity("");
        return true;
    }
}

function addNewNotification() {
    let titleEl = document.getElementById("title");
    let contentEl = document.getElementById("content");
    if (checkEmpty(titleEl) && checkEmpty(contentEl)) {
        let reqHeader = new Headers();
        reqHeader.append("Content-Type", "text/json");
        reqHeader.append("Accept", "application/json, text/plain, */*");
        let initObject = {
            method: "POST",
            headers: reqHeader,
            body: JSON.stringify({
                title: titleEl.value,
                content: contentEl.value
            })
        };
        fetch("/api/NotificationBoard/AddNotification", initObject)
            .then(res => res.json())
            .then(json => {
                if (json.message == "success") {
                    changeContent("Add success", true);
                }
                else {
                    changeContent("Some thing went wrong", false);
                }
                document.querySelector("#addNotificationModal .btn-close").click();
                showMessageModal();
            })
    }
}
let editID;
function EditNotification(obj) {
    let notiID = obj.getAttribute("notiID");
    editID = notiID;
    fetch(`/api/NotificationBoard/GetDetailOfNotification/${notiID}`)
        .then(res => res.json())
        .then(json => {
            document.getElementById("editTitle").value = json.title;
            document.getElementById("editContent").value = json.content;
            document.getElementById("status").checked = json.status == "Available" ? true : false;
        })
    document.getElementById("EditModal_trigger").click();
}

function requestEditNotification() {
    let available = document.getElementById("status").checked ? "Available" : "Unavailable"
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            NotificationID: editID,
            Title: document.getElementById("editTitle").value,
            Content: document.getElementById("editContent").value,
            Status: available
        })
    };
    fetch("/api/NotificationBoard/EditNotification", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "success") {
                changeContent("Add success", true);
            }
            else {
                changeContent("Some thing went wrong", false);
            }
            document.querySelector("#editNotificationModal .btn-close").click();
            showMessageModal();
        })
}

setAppendToDataWrapper(appendToDataWrapper);
