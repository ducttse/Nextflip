let Data;
let requestParam = {
    RowsOnPage: 10,
    RequestPage: 1
}

function renderCard(notification) {
    let date = notification.publishedDate.slice(0, 10).split("-").reverse().join("-");
    return `<div class="card mb-2 text-white bg-dark notification">
                <div class="card-body">
                    <div class="d-flex" onclick="ViewDetail('${notification.notificationID}')">
                        <h5 class="card-title me-auto">${notification.title}</h5>
                        <h6 class="card-subtitle pt-1 mb-2 text-muted">${date}</h6>
                    </div>
                </div>
            </div>`;
}

function appendToWrapper() {
    let renderedData = Data.data.map(noti => {
        return renderCard(noti)
    }).join("");
    document.getElementById("notification_wrapper").innerHTML = renderedData;
}

function getNotification() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestParam)
    };
    fetch("/api/NotificationBoard/ViewAvailableNotifications", initObject)
        .then(res => res.json())
        .then(json => {
            Data = json;
            document.getElementById("notification_detail").classList.add("d-none");
            document.getElementById("notification_wrapper").classList.remove("d-none");
            document.getElementById("pagination").classList.remove("d-none");
            document.getElementById("totalPage").textContent = "of " + json.totalPage;
            appendToWrapper();
        });
}

function renderDetail(item) {
    let date = item.publishedDate.slice(0, 10).split("-").reverse().join("-");
    return `        
    <button class="d-flex btn btn-dark" onclick="HideDetail()">
        <i class="fas fa-arrow-left pt-1 me-2"></i> Back
    </button>
    <div class="bg-dark p-2 mt-2" style="min-height: 30vh">
        <p class="h2">${item.title}</p>
        <p class="text-muted">${date}</p>
        <p class="fs-5">${item.content}</p>
    </div>`;
}

function ViewDetail(id) {
    fetch(`/api/NotificationBoard/GetDetailOfNotification/${id}`)
        .then(res => res.json())
        .then(json => {
            document.getElementById("notification_wrapper").classList.add("d-none");
            document.getElementById("pagination").classList.add("d-none");
            document.getElementById("notification_detail").innerHTML = renderDetail(json);
            document.getElementById("notification_detail").classList.remove("d-none");
        })
}

function HideDetail() {
    document.getElementById("notification_detail").classList.add("d-none");
    document.getElementById("pagination").classList.remove("d-none");
    document.getElementById("notification_wrapper").classList.remove("d-none");
}

function nextPage() {
    if (requestParam.RequestPage == Data.totalPage) {
        return;
    }
    requestParam.RequestPage += 1;
    document.getElementById("currentPage").innerText = requestParam.RequestPage;
    getNotification();
}

function previousPage() {
    if (requestParam.RequestPage <= 1) {
        return;
    }
    requestParam.RequestPage -= 1;
    document.getElementById("currentPage").innerText = requestParam.RequestPage;
    getNotification();
}