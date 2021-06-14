let Data = {
  data: []
};

function getData() {
  return Data.data;
}

function renderUser(user) {
  return `
    <tr>
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
            <i class="fas fa-edit"></i>Edit
            </a>
        </td>
    </tr>`;
}
function reRenderCheckbox() {
  var checkBoxList = document.getElementsByClassName("checkBox");
  for (let index = 0; index < checkBoxList.length; index++) {
    let child = checkBoxList[index];
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

function appendUserToWrapper(start, end) {
  let userArray = Data.data.slice(start, end).map((user) => {
    return renderUser(user);
  });
  userArray = userArray.join("");
  let dataWapper = document.getElementById("dataWapper");
  if (dataWapper.innerHTML !== "") {
    dataWapper.innerHTML = "";
  }
  dataWapper.insertAdjacentHTML("afterbegin", userArray);
  // add reRender function to each checkbox
  reRenderCheckbox();
}

function Load(rowsPerPage) {
  appendUserToWrapper(0, rowsPerPage);
  appendPagination(Data.data.length, rowsPerPage);
  setCurrentColor(1);
  setClickToIndex(appendUserToWrapper);
}

function Run(rowsPerPage) {
  ///
  // let searchValue = {
  //   searchValue: "dSRFgJ2L3CqrZJrmOkWD@gmail.com"
  // };
  // ////
  // let reqHeader = new Headers();
  // reqHeader.append("Content-Type", "text/json");
  // reqHeader.append("Accept", "application/json, text/plain, */*");

  // let initObject = {
  //   method: "POST",
  //   headers: reqHeader,
  //   body: JSON.stringify()
  // };
  ////
  fetch("/api/UserManagerManagement/GetAllAccounts")
    .then((response) => response.json())
    .then((json) => {
      Data.data = json;
      Load(rowsPerPage);
    });
}

function search(searchValue) {
  if (!searchValue) {
    return;
  }
  ReRun();
  fetch(`/api/UserManagerManagement/GetAccountListByEmail/${searchValue}`)
    .then((response) => response.json())
    .then((json) => {
      Data.data = json;
      Load();
    });
}

function ReRun() {
  document.getElementById("dataWapper").innerHTML = "";
  document.getElementById("pagination").innerHTML = "";
}
