let TopicArr;
let currentIndex;

function appendButton(button) {
    console.log("append")
    document.getElementById("topic_List").insertAdjacentHTML("afterbegin", button);
}

function setRoleName(role) {
    document.getElementById("role_name").innerHTML = role;
}

function setName(name) {
    document.getElementById("account_name").innerHTML = name;
}