function requestLogin() {
    let inputData = {
        Email: "",
        Password: ""
    }
    inputData.Email = document.getElementById("email").value;
    inputData.Password = document.getElementById("password").value;
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(inputData)
    };
    fetch("/api/Login/LoginAccount", initObject)
        .then(res => res.json())
        .then(json => {
            console.log(json);
            if (json.message == true) {
                console.log("login success");
                //TODO redirect to page
            }
            else {
                document.getElementsByClassName("alert")[ 0 ].classList.remove("d-none");
            }
        })
}

function onSignIn(googleUser) {
    var profile = googleUser.getBasicProfile();
    console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
    console.log('Name: ' + profile.getName());
    console.log('Image URL: ' + profile.getImageUrl());
    console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
}