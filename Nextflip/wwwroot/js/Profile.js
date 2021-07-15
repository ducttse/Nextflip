let Profile;

function getProfile() {
    return Profile;
};

function loadToHeader() {
    document.getElementById("profile_img").setAttribute("src", Profile.pictureURL + `?time=${stamp}`);
}

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
let stamp = Date.now();

function renderProfile() {
    return `<div>
                <img width="200" height="200" style="object-fit: cover;" class="rounded-circle" src="${Profile.pictureURL}?time=${stamp}" alt="profile picture"/>
            </div> 
            <div class="d-flex">
                <p class="h2 pb-3 pt-1 ps-5 mt-5">${Profile.fullname}</p><i class="fas fa-pen fa-sm mt-5 pt-3 ps-2"></i>
            </div>`
}

function appendUserProfileToWrapper() {
    document.getElementById("imgAndName_holder").insertAdjacentHTML("afterbegin", renderProfile());
    document.getElementById("email").innerHTML = Profile.userEmail;
    document.getElementById("dob").innerHTML = Profile.dateOfBirth;
    if (localStorage.getItem("URL") == "SubcribedUserDashBoard") {
        if (Profile.subscriptionEndDate == null) {
            document.getElementById("exp_date").innerHTML = "N/A"
        }
        else document.getElementById("exp_date").innerHTML = Profile.subscriptionEndDate;
    }
}

const routeRegex = new RegExp("^\/SubcribedUserDashBoard\/[\w\d\s]*$");
const profileRegex = new RegExp("^\/Profile\/[\w\d\s]*$");

const ProfileRoute = [];


loadAccount().then(() => {
    if (document.getElementById("header") != null) {
        loadToHeader();
    }
    if (document.getElementById("imgAndName_holder") != null) {
        appendUserProfileToWrapper();
        setTriggerLoadProfile();
    }
    if (document.getElementById("sideBar") != null) {
        let role;
        console.log(localStorage.getItem("URL").split("/")[ 1 ]);
        if (document.getElementById("imgAndName_holder") != null) {
            switch (localStorage.getItem("URL").split("/")[ 1 ]) {
                case "UserManagerManagement":
                    role = "User Manager";
                    let button1 = `<a href="/UserManagerManagement/Index" class="side_bar_btn btn btn-dark text-decoration-none link-light text-start w-100">
                            Manage user account
                      </a>`;
                    let button2 = `<a href="#" class="side_bar_btn btn btn-dark text-decoration-none link-light text-start w-100" onclick="showAddStaffModal()">
                            Create new staff
                          </a>`;
                    appendButton(button1);
                    appendButton(button2);
                    break;
                case "MediaManagerManagement":
                    role = "Media Manager";
                    let button3 = `<a id="back_btn" href="/MediaManagerManagement/Index" class="side_bar_btn btn btn-dark text-decoration-none text-start link-light mx-auto w-100">
                        Media Manager
                      </a>`;
                    appendButton(button3);
                    break;
                case "EditorDashboard":
                    role = "Media Editor";
                    let button4 = `<a href="/EditorDashboard/Index" class="side_bar_btn btn btn-dark text-decoration-none link-light text-start w-100" onclick="showAddStaffModal()">
                            Media manager
                          </a>`;
                    let button5 = `<a href="/EditorDashboard/ViewEditRequest"  onclick="return showForm();" class="side_bar_btn btn btn-dark text-decoration-none link-light text-start w-100" onclick="showAddStaffModal()">
                            View edit request
                        </a>`;
                    appendButton(button4);
                    appendButton(button5);
                    break;
                case "SupporterDashboard":
                    role = "Ticket Supporter";
                    let button6 = `<a id="back_btn" href="/SupporterDashboard/Index" class="side_bar_btn btn btn-dark text-decoration-none text-start link-light mx-auto w-100">
                        Support Ticket
                      </a>`;
                    appendButton(button6);
                    break;
            }
        }
        setRoleName(role);
        setName(Profile.fullname);
        setImg(Profile.pictureURL + `?time=${Date.now()}`);
    }
})
