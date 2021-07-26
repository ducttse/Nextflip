function rendeMedia(media) {
    console.log(media);
    return `
    <div class="col-4 result mb-2 p-1" onclick="return AppendDetails('${media.mediaID}');">
        <div id="img_container">
            <img src="${media.bannerURL}" alt="${media.mediaID}" style="width: 100%; height: 100%; object-fit: cover"/>
            <div id="title">
                <p class="link-light p-1 fs-6 ">${media.title}</p>
            </div>
        </div>
    </div>`;
}

function rendeMediaEL(arr) {
    console.log(arr);
    let rendered = arr.map(media => {
        console.log(media);
        return rendeMedia(media);
    }).join("");
    return `
        <div class="row">
            ${rendered}
        </div>
    `
}

function appendMedia(arr) {
    let el = rendeMediaEL(arr);
    document.getElementById("media_holder").insertAdjacentHTML("afterbegin", el);
}

function requestMediaByID(id, num) {
    fetch(`/api/ViewSubscribedUserDashboard/GetMediasByCategoryID/${id}/${num}`)
        .then(res => res.json())
        .then(json => {
            appendMedia(json);
        })
}

function requestFavourite() {
    let ID = localStorage.getItem("ID");
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(
            {
                UserID: ID
            }
        )
    };
    fetch("/api/ViewSubscribedUserDashboard/GetFavoriteList/", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.length == 0) {
                document.getElementById("empty").classList.remove("d-none");
            }
            else {
                document.getElementById("empty").classList.add("d-none");
                appendMedia(json);
            }
        })
}