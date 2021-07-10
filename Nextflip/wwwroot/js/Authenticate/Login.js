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
                window.location.replace(json.url);
            }
            else {
                let alert = document.getElementsByClassName("alert")[ 0 ]
                alert.classList.remove("d-none");
            }
        })
}

async function onSignIn(googleUser) {
    var profile = await googleUser.getBasicProfile();
    gapi.auth2.getAuthInstance().disconnect().then(() => {
        return SignInToBackEnd(profile);
    })
}

async function SignInToBackEnd(profile) {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            Email: profile.getEmail()
        })
    };
    await fetch("/api/Login/LoginByGmail", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "New Account") {
                localStorage.setItem("profile", JSON.stringify({
                    email: profile.getEmail(),
                    fullname: profile.getName(),
                    imgURL: profile.getImageUrl()
                }));
                window.location.href = json.url;
            }
            else if (json.message == true) {
                localStorage.setItem("ID", json.userID)
                window.location.href = json.url;
            }
            else {
                let alert = document.getElementsByClassName("alert")[ 0 ]
                alert.classList.remove("d-none");
            }
        })
}

function signOut() {
    var auth2 = gapi.auth2.getAuthInstance();
    console.log(auth2);
    auth2.signOut().then();
}
// signOut();