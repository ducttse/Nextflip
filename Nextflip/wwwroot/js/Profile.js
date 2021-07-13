let Profile;

function getProfile() {
    return Profile;
};

async function loadAccount() {
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
    await fetch("/api/ProfileManagement/GetProfile", initObject)
        .then(res => res.json())
        .then(json => {
            Profile = { ...json, userID: ID }
        })
    return new Promise(resolve => { resolve("resolved") })
}

function renderProfile() {
    let stamp = Date.now();
    return `<div>
                <img width="200" height="200" style="object-fit: cover;" class="rounded-circle" src="${Profile.pictureURL}?time=${stamp}" alt="profile picture"/>
            </div> 
            <div class="d-flex">
                <p class="text-light h2 pb-3 pt-1 ps-5 mt-5">${Profile.fullname}</p><i class="fas fa-pen fa-sm mt-5 pt-3 ps-2"></i>
            </div>`
}

function appendUserToWrapper() {
    document.getElementById("imgAndName_holder").insertAdjacentHTML("afterbegin", renderProfile());
    document.getElementById("email").innerHTML = Profile.userEmail;
    document.getElementById("dob").innerHTML = Profile.dateOfBirth;
    if (Profile.subscriptionEndDate == null) {
        document.getElementById("exp_date").innerHTML = "N/A"
    }
    else document.getElementById("exp_date").innerHTML = Profile.subscriptionEndDate;
}
loadAccount().then(() => {
    appendUserToWrapper();
    setTriggerLoadProfile();
})
