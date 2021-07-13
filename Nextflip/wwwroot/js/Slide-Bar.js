let TopicArr;
let currentIndex;

function appendButton(button) {
    document.getElementById("topic_List").querySelector(".text-muted").insertAdjacentHTML("afterend", button);
}

function setRoleName(role) {
    document.getElementById("role_name").innerHTML = role;
}

function setName(name) {
    document.getElementById("account_name").innerHTML = name;
}

function setImg(url) {
    document.getElementById("profile_img").setAttribute("src", url);
}