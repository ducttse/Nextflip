let Data;
let requestParam = {
    RowsOnPage: 10,
    RequestPage: 1
}

function renderCard(notification) {
    let date = notification.publishedDate.slice(0, 10).split("-").reverse().join("-");
    return `<div class="card mb-2 text-white bg-dark notification">
                <div class="card-body">
                    <div class="d-flex">
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
    document.getElementById("notification_wrapper").insertAdjacentHTML("afterbegin", renderedData);
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
            appendToWrapper();
        });
}