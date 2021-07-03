let Data;
let requestParam = {
  RowsOnPage: 12,
  RequestPage: 1,
  RoleName: "User Manager",
  SearchValue: "",
  Status: ""
};
let isSearched = false;
let isFiltered = false;

function resetSearch() {
  document.getElementById("search").value = "";
  requestParam.SearchValue = "";
  isSearched = false;
}

function resetFilter() {
  document.getElementById('status_filter').getElementsByTagName('option')[ 0 ].selected = 'selected';
  isFiltered = false;
  requestParam.Status = "";
}

function setRequestPage(num) {
  setPageDataCurrentPage(num);
  requestParam.RequestPage = num;
  if (isFiltered && isSearched) {
    return searchWithFilterOnly();
  }
  else if (isFiltered) {
    return requestWithFilterOnly();
  }
  else if (isSearched) {
    return searchOnly();
  }
  return requestUserDataOnly();
}

function ShowNotFound() {
  let error;
  setTotalPage();
  if (isFiltered && isSearched) {
    error = `<p class="fs-6">There is no <b>${requestParam.Status}</b> user in this role contain  <b>${requestParam.SearchValue}</b></p>`
  }
  else if (isFiltered) {
    error = `<p class="fs-6">There is no <b>${requestParam.Status}</b> user for this role</p>`
  }
  else { error = `<p class="fs-6">There is no result for <b>${requestParam.SearchValue}</b></p>` }
  let notFound = document.getElementById("notFound");
  if (notFound.innerHTML != "") {
    notFound.innerHTML = "";
  }
  notFound.insertAdjacentHTML("afterbegin", error);
  notFound.classList.remove("hide");
  document.getElementById("table_holder").classList.add("hide");
  document.getElementById("pagination").classList.add("hide");
  if (!isFiltered) {
    document.getElementById("status_filter").setAttribute("disabled", "disabled");
  }
}

function HideNotFound() {
  let notFound = document.getElementById("notFound");
  if (!notFound.classList.contains("hide")) {
    notFound.classList.add("hide");
    document.getElementById("table_holder").classList.remove("hide");
    document.getElementById("pagination").classList.remove("hide");
    if (!isFiltered) {
      document.getElementById("status_filter").removeAttribute("disabled");
    }
  }
}

function renderUser(user, index) {
  return `
    <tr style="max-width: 45px">
        <td>${index + 1}</td>
        <td>${user.userEmail}</td>
        <td>${user.fullname}</td>
        <td class="checkBox">
          <div>
            <input class="status_btn" type="checkbox" userID="${user.userID}" value="${user.status}" ${user.status === "Active" ? "checked" : ""}  />
          </div>
        </td>
        <td>
            <a class="text-decoration-none" 
              userID = "${user.userID}"
              onclick="GetUserProfile(this)"
              href="#">
              Edit
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
  let userArray = Data.data.slice(0, requestParam.RowsOnPage).map((user, index) => {
    return renderUser(user, start + index);
  });
  userArray = userArray.join("");
  let dataWapper = document.getElementById("dataWapper");
  if (dataWapper.innerHTML !== "") {
    dataWapper.innerHTML = "";
  }
  dataWapper.insertAdjacentHTML("afterbegin", userArray);
  appendCurrentArray();
  addEvent();
}

function setRowsPerPage(obj) {
  requestParam.RowsOnPage = obj.value;
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
  setRequestPage(requestParam.RequestPage)
    .then(res => res.json())
    .then(json => {
      Data = json;
      appendUserToWrapper();
    })
}

function requestUserDataOnly() {
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

function requestUserData() {
  requestUserDataOnly()
    .then(res => res.json())
    .then(json => {
      Data = json
      appendUserToWrapper();
    })
}

function requestUserDataAndResetPage() {
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
  return requestUserData();
}

function setSelectedRole(obj) {
  requestParam.RoleName = obj.value;
  if (isFiltered) {
    resetFilter();
  }
  if (isSearched) {
    searchAndResetPage(requestParam.SearchValue);
  }
  else requestUserDataAndResetPage();
}

function requestWithFilterOnly() {
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

function requestWithFilter() {
  requestWithFilterOnly()
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
    })
}

function requestWithFilterAndResetPage() {
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
  requestWithFilter();
}

function setSelectedStatus(obj) {
  if (obj.value == "All") {
    isFiltered = false;
    requestParam.Status = "";
    if (isSearched) {
      searchAndResetPage(requestParam.SearchValue);
    }
    else requestUserDataAndResetPage();
    return;
  }
  isFiltered = true;
  requestParam.Status = obj.value;
  if (isSearched) {
    searchAndResetPage(requestParam.SearchValue);
  }
  else requestWithFilterAndResetPage();
}

function searchOnly() {
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

function searchWithFilterOnly() {
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

function search(searchValue) {
  requestParam.SearchValue = searchValue;
  let searchFunc = searchOnly;
  isSearched = true;
  if (isFiltered) {
    searchFunc = searchWithFilterOnly;
  }
  searchFunc(searchValue)
    .then(res => res.json())
    .then(json => {
      if (json.totalPage != null) {
        Data = json;
        if (parseInt(json.totalPage) == 0) {
          ShowNotFound();
        }
        else {
          HideNotFound();
          pageData.currentPage = 1;
          appendUserToWrapper();
        };
      }
      else {
        ShowNotFound();
      }
    })
}

function searchAndResetPage(searchValue) {
  if (searchValue.trim().length == 0) {
    requestParam.searchValue = "";
    return;
  }
  requestParam.RequestPage = 1;
  setPageDataCurrentPage(1);
  search(searchValue);
}

setAppendToDataWrapper(appendUserToWrapper);
