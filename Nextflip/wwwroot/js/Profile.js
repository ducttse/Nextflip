let Data = {
    "userID": "9A0u0Kmy78OFFycpGkZQ",
    "userEmail": "IiFPqKJ0bnOjqmOT6p1g@gmail.com",
    "googleID": null,
    "googleEmail": null,
    "roleName": "user manager",
    "hashedPassword": null,
    "fullname": "Ngô Minh Dung",
    "dateOfBirth": "0001-01-01T00:00:00",
    "status": "Active",
    "pictureURL": "https://storage.googleapis.com/next-flip/User%20Profile%20Image/Default"
}

function renderProfile() {
    return `<div>
                <img width="200" height="200" class="rounded-circle" src="${Data.pictureURL}" alt="${Data.userID}"/>
            </div> 
            <div>
                <p class="text-light h2 pb-3 pt-4 ps-5 mt-5">${Data.fullname}</p>
            </div>`
}

function appendUserToWrapper() {
    document.getElementById("imgAndName_holder").insertAdjacentHTML("afterbegin", renderProfile());
    document.getElementById("email").innerHTML = Data.userEmail;
    document.getElementById("dob").innerHTML = Data.dateOfBirth;
    document.getElementById("exp_date").innerHTML = "";
}
appendUserToWrapper();