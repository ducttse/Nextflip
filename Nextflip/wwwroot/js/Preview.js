let requestMediaID;

function setRequestMediaID(id) {
    requestMediaID = id
}

function requestApprove() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            RequestID: requestMediaID
        })
    };
    fetch("/api/MediaManagerManagement/ApproveRequest/", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "success") {
                window.location.replace("/MediaManagerManagement/Index");
            }
            else {
                window.location.reload();
            }
        })
}

function requestDisapprove() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            RequestID: requestMediaID,
            Note: document.getElementById("note_input").value
        })
    };
    fetch("/api/MediaManagerManagement/DisapproveRequest/", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "success") {
                window.location.replace("/MediaManagerManagement/Index");
            } else {
                window.location.reload();
            }
        })
}