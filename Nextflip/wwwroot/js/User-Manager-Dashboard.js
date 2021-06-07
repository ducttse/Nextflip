let Data = {
  data: [
    {
      userID: "U001",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "Admin",
      status: "Active"
    },
    {
      userID: "U002",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "User",
      status: "Inactive"
    },
    {
      userID: "U001",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "Admin",
      status: "Active"
    },
    {
      userID: "U002",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "User",
      status: "Inactive"
    },
    {
      userID: "U001",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "Admin",
      status: "Active"
    },
    {
      userID: "U002",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "User",
      status: "Inactive"
    },
    {
      userID: "U001",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "Admin",
      status: "Active"
    },
    {
      userID: "U002",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "User",
      status: "Inactive"
    },
    {
      userID: "U001",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "Admin",
      status: "Active"
    },
    {
      userID: "U002",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "User",
      status: "Inactive"
    },
    {
      userID: "U001",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "Admin",
      status: "Active"
    },
    {
      userID: "U002",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "User",
      status: "Inactive"
    },
    {
      userID: "U001",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "Admin",
      status: "Active"
    },
    {
      userID: "U002",
      userEmail: "tranbaolong14@gmail.com",
      fullname: "Trần Bảo Long",
      role: "User",
      status: "Inactive"
    }
  ]
};

function renderUser(user) {
  return `
    <tr>
        <td>${user.userEmail}</td>
        <td>${user.role}</td>
        <td>${user.fullname}</td>
        <td class="checkBox cur">
            <input type="checkbox" 
            name="user1" 
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
function renderPagination(length, rowsPerPage) {
    let numberOfPage = Math.ceil(length / rowsPerPage);
    console.log(numberOfPage);
    let Pages = "";
    for (let i = 1; i <= numberOfPage; i++) {
        Pages += `<li class="page-item"><a class="page-link" href="#">${i}</a></li>`;
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
    });
  }
}

function appendUserToWrapper(start, end) {
  let userArray = Data.data.slice(start, end).map((user) => {
    return renderUser(user);
  });
  userArray = userArray.join("");
  document
    .getElementById("dataWapper")
    .insertAdjacentHTML("afterbegin", userArray);
}

function appendPagination(length, rowsPerPage) {
    document
        .getElementById("pagination")
        .insertAdjacentHTML("afterbegin", renderPagination(length, rowsPerPage));
}

let rowsPerPage = 10;
let currentPage = 1;
///
let searchValue = {
    searchValue:"dSRFgJ2L3CqrZJrmOkWD@gmail.com"
    };
////
let reqHeader = new Headers();
reqHeader.append('Content-Type', 'text/json');
reqHeader.append('Accept', 'application/json, text/plain, */*');

let initObject = {
    method: 'POST',
    headers: reqHeader,
    body: JSON.stringify(searchValue)
};
////

fetch("/api/UserManagerManagement", initObject)
    .then((response) => response.json())
        .then((json) => {
            console.log(json);
            Data.data = json;
            console.log(json);
            appendUserToWrapper(0, rowsPerPage);
            reRenderCheckbox();
            appendPagination(Data.data.length, rowsPerPage);
        });