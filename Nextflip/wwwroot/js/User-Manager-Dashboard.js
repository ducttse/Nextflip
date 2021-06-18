let Data;
let requestParam = {
  RequestPage: 1,
  RowsOnPage: 10,
  roleName: "Subscribed User"
};
let currentChooseRole;
function setRequestPage(num) {
  requestParam.RequestPage = num;
  return requestUserData(requestParam.roleName);
}

function renderUser(user, index) {
  return `
    <tr>
        <td>${index + 1}</td>
        <td>${user.userEmail}</td>
        <td>${user.roleName}</td>
        <td>${user.fullname}</td>
        <td class="checkBox">
            <input type="checkbox" 
            name="userRole" 

            initValue=${user.status}
            value="${user.status}" 
            ${user.status === "Active" ? "checked" : ""} /> ${user.status}
        </td>
        <td>
            <a class="text-decoration-none" 
            href="/Edit/${user.userID}">
            <i class="fas fa-edit"></i>
            </a>
        </td>
    </tr>`;
}
function reRenderCheckbox() {
  var checkBoxList = document.getElementsByClassName("checkBox");
  for (let index = 0; index < checkBoxList.length; index++) {
    let child = checkBoxList[ index ];
    let checkBoxEl = child.querySelector("input[type=checkbox]");
    checkBoxEl.addEventListener("click", () => {
      if (checkBoxEl.checked) {
        child.innerHTML = "";
        checkBoxEl.setAttribute("value", "Active");
        child.append(checkBoxEl);
        child.append(" Active");
      } else {
        child.innerHTML = "";
        checkBoxEl.setAttribute("value", "Inactive");
        child.append(checkBoxEl);
        child.append(" Inactive");
      }
      console.log(checkBoxEl);
      console.log(checkBoxEl.getAttribute("initValue"));
      if (
        checkBoxEl.getAttribute("initValue") !==
        checkBoxEl.getAttribute("value")
      ) {
        child.classList.add("text-warning");
        // document.getElementById("message").innerHTML =
        //   "Something changed. Click to save";
      }
      // remove if unchange
      else if (child.classList.contains("text-warning")) {
        child.classList.remove("text-warning");
      }
    });
  }
}

function setTotalPage() {
  pageData.totalPage = Data.totalPage
}

function countStart() {
  return (pageData.currentPage - 1) * requestParam.RowsOnPage
}

function appendUserToWrapper() {
  setTotalPage();
  let start = countStart();
  let userArray = Data.data.slice(0, 10).map((user, index) => {
    return renderUser(user, start + index);
  });
  userArray = userArray.join("");
  let dataWapper = document.getElementById("dataWapper");
  if (dataWapper.innerHTML !== "") {
    dataWapper.innerHTML = "";
  }
  dataWapper.insertAdjacentHTML("afterbegin", userArray);
  // add reRender function to each checkbox
  reRenderCheckbox();
  appendCurrentArray();
}

setAppendToDataWrapper(appendUserToWrapper);

function search(searchValue) {
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  fetch(`/api/UserManagerManagement/GetAccountListByEmail/${searchValue}`, initObject)
    .then(res => res.json())
    .then(json => {
      Data = json;
      console.log(json);
      pageData.currentPage = 1;
      appendUserToWrapper();
    })
}

function requestUserData(role) {
  requestParam.roleName = role;
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  return fetch("/api/UserManagerManagement/GetAccountsListByRoleAccordingRequest", initObject);
}

function getRoles() {
  fetch("/api/UserManagerManagement/GetRoleNameList")
    .then(res => res.json())
    .then(json => {
      TopicArr = json;
      appendCollase("Role", "roleName", requestUserData, appendUserToWrapper)
    })
}

