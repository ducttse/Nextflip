let CurrentMediaID;
async function addToFavourite() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(
            {
                userID: getProfile().userID,
                mediaID: CurrentMediaID
            }
        )
    };
    await fetch("/api/ViewMediaDetails/AddMediaToFavorite", initObject).then(res => res.json()).then(json => {
        if (json.message == "Success") {
            document.getElementById("add_btn").classList.add("d-none");
            document.getElementById("remove_btn").classList.remove("d-none");
        }
    })

}

function removeFavourite() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(
            {
                userID: getProfile().userID,
                mediaID: CurrentMediaID
            }
        )
    };
    fetch("/api/ViewMediaDetails/RemoveMediaFromFavorite", initObject).then(res => res.json()).then(json => {
        if (json.message == "Success") {
            document.getElementById("remove_btn").classList.add("d-none");
            document.getElementById("add_btn").classList.remove("d-none");
            if (location.pathname == "/SubcribedUserDashBoard/ViewFavourite") {
                location.reload();
            }
        }
    })

}

function isFavouriteMedia(id) {
    CurrentMediaID = id;
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    console.log(getProfile().userID);
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(
            {
                userID: getProfile().userID,
                mediaID: id
            }
        )
    };
    fetch("/api/ViewMediaDetails/IsFavoriteMedia", initObject)
        .then(res => res.json())
        .then(json => {
            console.log(json);
            if (json.message == "True") {
                console.log("favourite");
                document.getElementById("add_btn").classList.add("d-none");
                document.getElementById("remove_btn").classList.remove("d-none");
            }
            else {
                console.log("not");
                document.getElementById("remove_btn").classList.add("d-none");
                document.getElementById("add_btn").classList.remove("d-none");
            }
        })
}