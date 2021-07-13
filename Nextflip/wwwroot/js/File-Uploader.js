let token;

if (token == null) {
    refreshToken();
}
function refreshToken() {
    var details = {
        'client_id': '753240362122-dl13kbuakrta772hv66npll8sjqoo8p2.apps.googleusercontent.com',
        'client_secret': 'II1T9IoGZMl7IoXUF8E45NKC',
        'refresh_token': '1//0gE6Dmt6cqqdRCgYIARAAGBASNwF-L9Irps5D7CpCUlcNCyo9IVVazN9O5et1V1V4NHzy1m6y85t5b6mwCEpwzaSpKTJ_r2ErGso',
        'grant_type': 'refresh_token'
    };
    var formBody = [];
    for (var property in details) {
        var encodedKey = encodeURIComponent(property);
        var encodedValue = encodeURIComponent(details[ property ]);
        formBody.push(encodedKey + "=" + encodedValue);
    }
    formBody = formBody.join("&");
    fetch("https://oauth2.googleapis.com/token", {
        body: formBody,
        method: "POST",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        }
    })
        .then(res => res.json())
        .then(json => {
            token = json.access_token;
        })
        .catch(err => console.log(err));
}
let file;
function getFile(obj) {
    file = obj.files[ 0 ];
}
async function requestUploadBanner() {
    if (file == null) {
        return;
    }
    let stamp = Date.now();
    let responseURL;
    let url = `https://storage.googleapis.com/upload/storage/v1/b/next-flip/o?uploadType=media&name=Image/1280 x 720 banner/${stamp}`;
    await fetch(url, {
        body: file,
        headers: {
            Authorization: `Bearer ${token}`,
        },
        method: "POST"
    })
        .then(res => res.json())
        .then(json => {
            responseURL = "https://storage.googleapis.com/" + json.bucket + "/" + json.name
        })
        .catch(err => console.log(err));
    return responseURL;
}