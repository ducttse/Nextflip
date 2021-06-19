let Data;
let requestParam = {
  RequestPage: 1,
  RowsOnPage: 10,
  RoleName: "Customer Supporter",
  SearchValue: "",
  Status: ""
};
let isSearched = false;
let isFiltered = false;
function setTopic(topic) {
  requestParam.RoleName = topic;
}


function setRequestPage(num) {
  requestParam.RequestPage = num;
  console.log();
  if (isFiltered && isSearched) {
    console.log("1");
    return searchWithFilter();
  }
  else if (isFiltered) {
    console.log("2");

    return requestWithFilter();
  }
  else if (isSearched) {
    console.log("3");

    return searchOnly(requestParam.SearchValue);
  }
  return requestUserData(requestParam.RoleName);
}

function ShowNotFound() {
  let error = `<p>There is no result for <b>${requestParam.SearchValue}</b></p>`;
  let notFound = document.getElementById("notFound");
  if (notFound.innerHTML != "") {
    notFound.innerHTML = "";
  }
  notFound.insertAdjacentHTML("afterbegin", error);
  notFound.classList.remove("hide");
  document.getElementById("table_holder").classList.add("hide");
  document.getElementById("filter").setAttribute("disabled", "disabled");
}

function HideNotFound() {
  let notFound = document.getElementById("notFound");
  if (!notFound.classList.contains("hide")) {
    notFound.classList.add("hide");
    document.getElementById("table_holder").classList.remove("hide");
    document.getElementById("filter").removeAttribute("disabled");
  }
}

function renderUser(user, index) {
  return `
    <tr style="max-width: 45px">
        <td>${index + 1}</td>
        <td>${user.userEmail}</td>
        <td>${user.roleName}</td>
        <td>${user.fullname}</td>
        <td class="checkBox">
          <div class="col-2 mx-auto">
            <input type="checkbox" name="" id="" value="${user.status}" ${user.status === "Active" ? "checked" : ""}  />
          </div>
        </td>
        <td>
            <a class="text-decoration-none" 
            href="/Edit/${user.userID}">
            <i class="fas fa-edit"></i>
            </a>
        </td>
    </tr>`;
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
  appendCurrentArray();
}

setAppendToDataWrapper(appendUserToWrapper);

function search(searchValue) {
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
  requestParam.SearchValue = searchValue;
  if (searchValue == "") {
    isSearched = false;
    return;
  }
  isSearched = true;
  requestParam.SearchValue = searchValue;
  console.log(isFiltered + "  " + isSearched);
  if (isFiltered && isSearched) {
    searchWithFilter()
  }
  else {
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
      method: "POST",
      headers: reqHeader,
      body: JSON.stringify(requestParam)
    };
    fetch("/api/UserManagerManagement/GetAccountListByEmailFilterRole", initObject)
      .then(res => res.json())
      .then(json => {
        if (json.totalPage == 0) {
          ShowNotFound();
        }
        else {
          HideNotFound();
          Data = json;
          pageData.currentPage = 1;
          appendUserToWrapper();
        };
      })
  }
}

function searchOnly(searchValue) {
  requestParam.SearchValue = searchValue;
  isSearched = true;
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  return fetch("/api/UserManagerManagement/GetAccountListByEmailFilterRole", initObject)
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
  return fetch("/api/UserManagerManagement/GetAccountsListOnlyByRole", initObject);
}

function getRoles() {
  fetch("/api/UserManagerManagement/GetRoleNameList")
    .then(res => res.json())
    .then(json => {
      TopicArr = json;
      appendCollase("Role", "roleName", requestUserData, appendUserToWrapper)
      requestUserData("Customer Supporter")
        .then(res => res.json())
        .then(json => {
          Data = json;
          appendUserToWrapper();
          setChoosenColor(0);
        })
    });

}

function requestWithFilter() {
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
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

function searchWithFilter() {
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  fetch("/api/UserManagerManagement/GetAccountListByEmailFilterRoleStatus", initObject)
    .then(res => res.json())
    .then(json => {
      if (json.totalPage == 0) {
        ShowNotFound();
      }
      else {
        HideNotFound();
        Data = json;
        pageData.currentPage = 1;
        appendUserToWrapper();
      }
    });
}

function searchWithFilterOnly() {
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  return fetch("/api/UserManagerManagement/GetAccountListByEmailFilterRoleStatus", initObject)
}

function doFilter() {
  let filter = document.getElementById("filter");
  filter.addEventListener("change", () => {
    let choosenValue = filter.options[ filter.selectedIndex ].value;
    if (choosenValue === "All") {
      isFiltered = false;
      return;
    }
    isFiltered = true;
    requestParam.Status = choosenValue;
    console.log(isFiltered + "  " + isSearched);
    if (isFiltered && isSearched) {
      searchWithFilter();
    }
    else {
      requestWithFilter()
        .then(res => res.json())
        .then(json => {
          Data = json;
          appendUserToWrapper();
          setChoosenColor(TopicArr.findIndex((role) => {
            return role.roleName === requestParam.RoleName;
          }))
        })
    }
  })
}

function resetSearch() {
  document.getElementById("search").value = "";
  requestParam.SearchValue = "";
  isSearched = false;
}

function resetFilter() {
  document.getElementById('filter').getElementsByTagName('option')[ 0 ].selected = 'selected';
  isFiltered = false;
  requestParam.Status = "";
}

doFilter();