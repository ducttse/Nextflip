let requetsParam = {
    UserEmail: "bs3123131dfgc@gmail.com",
    Fullname: "bnih",
    dateOfBirth: "2012-03-05",
    RoleName: ""
}

function requestApi() {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestParam)
    };
    return fetch("/api/UserManagerManagement/CreateStaff", initObject);
}

function setRequetsParam() {
    let email = document.getElementById("email").value;
    let fullname = document.getElementById("fullname").value;
    let dob = document.getElementById("dob").value;
    let role = document.getElementById("role").value;
    if (role == "Choose role") {
        role = ""
    }
    requetsParam = {
        UserEmail: email,
        Fullname: fullname,
        dateOfBirth: dob,
        RoleName: role
    }
}

function CreateStaff() {
    setRequetsParam();
    // requestApi();
}