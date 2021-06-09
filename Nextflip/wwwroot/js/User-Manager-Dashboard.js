let Data = {
  data: []
};

let RequestObject;

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
let rowsPerPage = 10;
let currentPage = 0;

function renderPagination(length, rowsPerPage) {
  let numberOfPage = Math.ceil(length / rowsPerPage);
  let Pages = "";
  for (let i = 1; i <= numberOfPage; i++) {
    Pages += `<li class="page-item" page="${i}" onClick="setCurrentPage(${i})"><a class="page-link" href="#">${i}</a></li>`;
  }
  return `
  <nav>
    <ul class="pagination">
      <li class="page-item disabled">
        <a class="page-link" href="#">Previous</a>
      </li>
      ${Pages}
      <li class="page-item"><a class="page-link" href="#">Next</a></li>
    </ul>
    </nav>`;
}
// jump to another pagination
function setCurrentPage(number) {
  currentPage = number - 1;
  appendUserToWrapper(
    currentPage * rowsPerPage,
    rowsPerPage + Data.data.length - currentPage * rowsPerPage > rowsPerPage
      ? ++currentPage * rowsPerPage
      : currentPage * rowsPerPage +
          (Data.data.length - currentPage * rowsPerPage)
  );
  setCurrentColor(number);
}

function removeCurrentColor() {
  let pageArray = Array.from(document.getElementsByClassName("page-item"));
  let curPage = pageArray.filter((page) => {
    return page.classList.contains("active");
  });
  if (curPage.length > 0) {
    curPage[0].classList.remove("active");
  }
}

function setCurrentColor(number) {
  removeCurrentColor();
  let pageArray = Array.from(document.getElementsByClassName("page-item"));
  let curPage = pageArray.filter((page) => {
    return parseInt(page.getAttribute("page")) === number;
  });
  curPage[0].classList.add("active");
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

function appendPagination(length, rowsPerPage) {
  document
    .getElementById("pagination")
    .insertAdjacentHTML("afterbegin", renderPagination(length, rowsPerPage));
}

function Load() {
  appendPagination(Data.data.length, rowsPerPage);
  appendUserToWrapper(0, rowsPerPage);
  setCurrentColor(1);
}

function Run() {
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
      Load();
    });
}
Run();
