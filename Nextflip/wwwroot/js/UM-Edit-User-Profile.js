let UserRequestData = {
  userID: "00000000000000000000",
  userEmail: "longpnhse150499@fpt.edu.vn",
  googleID: null,
  googleEmail: null,
  roleName: "customer supporter",
  hashedPassword: null,
  fullname: "Phan Ngọc Hoàng Long",
  dateOfBirth: "2001-12-31T00:00:00",
  status: null,
  pictureURL: null,
  note: null
}
let isEdit = false;
let isUpdateDate = false;
let EditStaffModal;

function selectForm(role) {
  let body;
  if (role == "subscribed user") {
    body = `
        <p class="fs-1 text-center">Edit Subscribed User</p>
        <div class="col-12">
          <div class="mb-3 row ms-3">
            <label for="email_edit_label" class="col-form-label col-4 pe-0">Email</label>
            <div class="col-9 w-50 ps-0">
              <p class="py-2 pe-0 mb-0" id="plain_email_text"> </p>
            </div>
          </div>
          <div class="mb-3 row ms-3">
            <label for="fullname_edit_label" class="col-form-label col-4 pe-0">Full name</label>
            <div class="col-9 w-50 ps-0">
              <p class="py-2 pe-0 mb-0" id="plain_name_text"> </p>
            </div>
          </div>
          <div class="mb-3 row ms-3">
            <label id="dob_edit_label" for="dob" class="col-form-label col-4 pe-0">Expiration date</label>
            <div class="col-5 ps-0 me-0">
              <div class="d-inline-flex ps-0 pe-0">
                <div>
                    <input type="date" class="form-control pe-0 mb-0" id="plain_date_text" required/>
                </div>
              </div>
            </div>
            <div class="btn btn-primary col-2" onclick="requestUpdateDate()">
              Extend
            </div>
          </div>
        </div>`
  }
  else {
    body = `
              <p class="fs-1 text-center">Edit staff</p>
              <div class="col-12">
                <div class="mb-3 row ms-5">
                  <label for="email_edit_label" class="col-form-label col-3 pe-0">Email</label>
                  <div class="col-8 w-45 d-inline-flex ps-0 pe-0">
                    <div>
                      <p class="py-2 pe-0 mb-0" id="email_edit_text"></p>
                    </div>
                  </div>
                </div>
                <div class="mb-3 row ms-5">
                  <label for="fullname_edit_label" class="col-form-label col-3 pe-0">Fullname</label>
                  <div class="col-9 w-50 ps-0">
                    <div class="w-100">
                      <input id="fullname_edit_input" type="text" class="form-control" required autocomplete="nope"/>
                      <div class="invalid-feedback">
                      </div>
                    </div>
                  </div>
                </div>
                <div class="mb-3 row ms-5">
                  <label id="role_edit_label"for="role" class="col-3 col-form-label pe-0">Role</label>
                  <div class="col-9 ps-0">
                    <select id="role_edit_input" class="form-select w-55 =" aria-label="Default select example" required>
                      <option value="user manager">User Manager</option>
                      <option value="media editor">Media Editor</option>
                      <option value="media manager">Media Manager</option>
                      <option value="customer supporter">Customer Supporter</option>
                    </select>
                  </div>
                </div>
                <div class="mb-3 row ms-5">
                  <label id="dob_edit_label" for="dob" class="col-form-label col-3 pe-0">Date of birth</label>
                  <div class="col-9 ps-0">
                    <div class="w-100">
                      <input id="dob_detail_edit" type="date" class="form-control w-55" min="1900-01-01" max="2021-12-31" required/>
                      <div class="invalid-feedback">
                      </div>
                    </div>
                  </div>
                </div>
                <div class="mb-3 row">
                  <p class="btn btn-danger col-4 offset-4" onclick="requestEdit()">Save</p>
                </div>
              </div>`
  }
  let EditBody = document.getElementById("edit_body");
  if (EditBody.innerHTML != "") {
    EditBody.innerHTML = "";
  }
  EditBody.insertAdjacentHTML("afterbegin", body);
  showEditStaffModal();
  return new Promise(resolve => {
    resolve("resolved");
  })
}


function appendInfoToEditWrapper() {
  let emailEL = document.getElementById("email_edit_text");
  if (emailEL.innerHTML != "") {
    emailEL.innerHTML = ""
  }
  emailEL.innerHTML = UserRequestData.googleEmail == null ? UserRequestData.userEmail : UserRequestData.googleEmail;
  let fullnameEl = document.getElementById("fullname_edit_input");
  let roleEL = document.getElementById("role_edit_input");
  let dobEL = document.getElementById("dob_detail_edit");
  fullnameEl.value = UserRequestData.fullname;
  fullnameEl.setAttribute("initvalue", UserRequestData.fullname);
  roleEL.value = UserRequestData.roleName;
  roleEL.setAttribute("initvalue", UserRequestData.roleName);
  dobEL.value = UserRequestData.dateOfBirth.slice(0, 10);
  dobEL.setAttribute("initvalue", UserRequestData.dateOfBirth.slice(0, 10));
}

function appendUserInfoToEditWrapper() {
  let plaintEmailEl = document.getElementById("plain_email_text");
  let plaintNameEL = document.getElementById("plain_name_text");
  let plaintDateEl = document.getElementById("plain_date_text");
  if (plaintEmailEl.innerHTML != "") {
    plaintEmailEl.innerHTML = "";
  }
  if (plaintNameEL.innerHTML != "") {
    plaintNameEL.innerHTML = "";
  }
  plaintEmailEl.innerHTML = UserRequestData.googleEmail == null ? UserRequestData.userEmail : UserRequestData.googleEmail;
  plaintNameEL.innerHTML = UserRequestData.fullname;
  plaintDateEl.value = UserRequestData.expiration.endDate.slice(0, 10);
  plaintDateEl.setAttribute("initvalue", UserRequestData.expiration.endDate.slice(0, 10));
}

function showEditStaffModal() {
  if (EditStaffModal == null) {
    EditStaffModal = new bootstrap.Modal(document.getElementById("edit_user_modal"), {
      keyboard: false
    })
  }
  EditStaffModal.show();
}

function hideEditStaffModalOnSuccess() {
  var myModalEl = document.getElementById('edit_user_modal')
  myModalEl.addEventListener("hide.bs.modal", () => {
    if (isEdit) {
      resetValidation();
      appendFlashMessageContent(true);
      showModal("modal_flash");
      isEdit = false;
    }
    if (isUpdateDate) {
      appendFlashMessageContent(true);
      showModal("modal_flash");
      isUpdateDate = false;
    }
  })
  EditStaffModal.hide();
}


function GetUserProfile(obj) {
  UserRequestData.userID = obj.getAttribute("userID");
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify({
      userID: UserRequestData.userID
    })
  };
  fetch("/api/UserManagerManagement/GetUserProfile", initObject)
    .then(res => res.json())
    .then(json => {
      UserRequestData = json;
      selectForm(json.roleName).then(() => {
        if (json.roleName == "subscribed user") {
          appendUserInfoToEditWrapper();
        }
        else appendInfoToEditWrapper();
      })
    })
}

function checkInput() {
  let fullnameEl = document.getElementById("fullname_edit_input");
  let roleEL = document.getElementById("role_edit_input");
  let dobEL = document.getElementById("dob_detail_edit");
  if (fullnameEl.value != fullnameEl.getAttribute("initvalue")) {
    UserRequestData.fullname = fullnameEl.value;
    isEdit = true;
  }
  if (roleEL.value != roleEL.getAttribute("initvalue")) {
    UserRequestData.roleName = roleEL.value;
    isEdit = true;
  }
  if (dobEL.value != dobEL.getAttribute("initvalue")) {
    UserRequestData.dobEL = dobEL.value;
    isEdit = true;
  }
}

function checkUpdate() {
  let plaintDateEl = document.getElementById("plain_date_text");
  if (plaintDateEl.value != plaintDateEl.getAttribute("initvalue")) {
    UserRequestData.expiration.endDate = plaintDateEl.value
    isUpdateDate = true;
  }
}

function resetValidation() {
  document.getElementById("fullname_edit_input").parentNode.classList.remove("was-validated");
  document.getElementById("dob_detail_edit").parentNode.classList.remove("was-validated");
}

function validateFullNameDetail(message) {
  let fullNamInputEl = document.getElementById("fullname_edit_input");
  let parent = fullNamInputEl.parentNode;
  let feedback = parent.querySelector(".invalid-feedback");
  if (message == "Full name must not be empty") {
    feedback.innerHTML = message;
    fullNamInputEl.setCustomValidity("empty");
  }
  else {
    feedback.innerHTML = "";
    fullNamInputEl.setCustomValidity("");
  }
  parent.classList.add("was-validated");
}

function validateDOBDetail(message) {
  let InputEl = document.getElementById("dob_detail_edit");
  let parent = InputEl.parentNode;
  let feedback = parent.querySelector(".invalid-feedback");
  if (message == "Date of birth is Invalid") {
    feedback.innerHTML = message;
    InputEl.setCustomValidity("empty");
  }
  else {
    feedback.innerHTML = "";
    InputEl.setCustomValidity("");
  }
  parent.classList.add("was-validated");
}

function requestEdit() {
  checkInput();
  if (!isEdit) {
    return;
  }
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify({
      userID: UserRequestData.userID,
      roleName: UserRequestData.roleName,
      fullname: UserRequestData.fullname,
      dateOfBirth: UserRequestData.dateOfBirth
    })
  };
  fetch("/api/UserManagerManagement/EditStaffInfo", initObject)
    .then(res => res.json())
    .then(json => {
      if (json.message == "Fail") {
        isEdit = false;
        if (json.dateTimeErr != null) {
          validateDOBDetail(json.dateTimeErr);
        }
        if (json.nameErr != null) {
          validateFullNameDetail(json.nameErr);
        }
      }
      else if (json.message == "Success") {
        isEdit = true;
        hideEditStaffModalOnSuccess();
      }
    })
}

function requestUpdateDate() {
  let el = document.getElementById("plain_date_text");
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify({
      userID: UserRequestData.userID,
      endDate: el.value
    })
  };
  fetch("/api/UserManagerManagement/UpdateExpiredDate", initObject)
    .then(res => res.json())
    .then(json => {
      isUpdateDate = false;
      if (json.message == " Fail") {
        if (json.dateTimeErr != null) {
          if (json.dateTimeErr == "Date is invalid") {
            console.log("//TODO")
          }
        }
      }
      else if (json.message == "Success") {
        isUpdateDate = true;
        hideEditStaffModalOnSuccess();
      }
    })
}