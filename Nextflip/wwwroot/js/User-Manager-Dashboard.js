let Data = {
  data: []
};
let requestParam = {
  NumberOfPage: 3,
  RowsOnPage: 10,
  RequestPage: 1
};
function getData() {
  return Data.data;
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
            <i class="fas fa-edit"></i>Edit
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

function appendUserToWrapper(start, end) {
  if (maxPage * requestParam.RowsOnPage - end <= 0) {
    return;
  }
  let userArray = Data.data.slice(start, end).map((user, index) => {
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
}

function CountCurrentLoadedPage() {
  return parseInt(Data.data.length / requestParam.RowsOnPage)
}

function Load(rowsPerPage) {
  appendUserToWrapper(0, rowsPerPage);
  appendPagination(Data.data.length, rowsPerPage);
  setCurrentColor(1);
  setClickToIndex(appendUserToWrapper);
}

function PostRequest(page) {
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  if (page !== 1) {
    requestParam.RequestPage = page;
  }
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  //
  return fetch(
    "/api/UserManagerManagement/GetAccountsListAccordingRequest",
    initObject
  )
}

function RequestMoreData(RequestPage) {
  requestParam.RequestPage = RequestPage;
  console.log(Data.data.length);

  PostRequest(RequestPage).then(res => res.json()).then(json => {
    json.forEach(user => {
      Data.data.push(user);
    })
    let num = RequestPage - 1;
    appendUserToWrapper(num * rowsPerPage, (num + 1) * rowsPerPage);
    setCurrentColor(RequestPage);
  })
}

function Run(rowsPerPage) {
  PostRequest(1).then((response) => response.json())
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
      Load(rowsPerPage);
    });
}

function preLoad() {
  return new Promise((resolve, reject) => {
    setMaxPage(resolve);
  });
}

function setMaxPage(resolve) {
  fetch("/api/UserManagerManagement/NumberOfAccounts")
    .then((res) => res.json())
    .then((json) => {
      rowsPerPage = requestParam.RowsOnPage;
      maxPage = parseInt(parseInt(json) / rowsPerPage);
      resolve("resolved");
    });
}

function ReRun() {
  document.getElementById("dataWapper").innerHTML = "";
  document.getElementById("pagination").innerHTML = "";
}
