function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}

const processChange = (obj) => {
    let value = obj.value
    debounce(() => { checkMail(value) });
}

function checkMail(value) {
    let inputData = {
        Email: value
    }
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(inputData)
    }
    fetch("/api/Login/CheckEmail", initObject)
        .then(res => res.json())
        .then(json => {
            console.log(json);
            if (json.message == "Email does not exist!") {
                //TODO success
            }
            else if (json.message == "Valid") {
                //TODO fail
            }
        });
}