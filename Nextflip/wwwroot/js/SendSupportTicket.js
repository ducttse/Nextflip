let ticketContent = {
  UserEmail: "",
  TopicName: "",
  Content: ""
}

function renderOption(item) {
  return `
      <option value="${item.topicName}">${item.topicName}</option>
      `;
}

function appendToWrapper(items) {
  document.getElementById("option").insertAdjacentHTML("beforeend", items);
}

function loadTopic(data) {
  let optionArr = data.map((item) => {
    return renderOption(item);
  });
  optionArr = optionArr.join("");
  appendToWrapper(optionArr);
}

let modal;

function showModal() {
  if (modal == null) {
    modal = new bootstrap.Modal(document.getElementById('Modal'), {
      keyboard: false
    })
  }
  modal.show();
}

function changeContent(text, bool) {
  let content = !bool ?
    ` <i class="far fa-times-circle fa-5x text-danger text-center"></i>
            <p class="fs-5 text-center text-dark">${text}</p>
            <button type="button" class="col-6 mx-auto btn btn-danger text-white" data-bs-dismiss="modal">
                Close
            </button>` :
    ` <i class="far fa-check-circle fa-5x text-center" style="color: #4bca81"></i>
            <p class="fs-5 text-center text-dark">${text}</p>
            <button onclick = "BackToDashboard()" type="button" class="col-6 mx-auto btn btn-success text-white"  style=" background-color: #4bca81 !important; border: #4bca81 !important;" data-bs-dismiss="modal">
                Back to dashboard
            </button>`;
  document.querySelector(".modal .modal-body").innerHTML = content;
}

function BackToDashboard() {
  window.location.href = "/SubcribedUserDashBoard/Index";
}
function renderCard(index, plan) {
  let month = Math.floor(plan.duration / 30);
  return `
    <div id="item${index}" class="col-4 card text-white bg-primary mb-3 me-3" style="max-width: 18rem;">
        <div class="card-body d-flex flex-column justify-content-between">
            <p class="card-title text-center h5">${month} ${month > 1 ? "months" : "month"}</p>
            <p class="card-text text-center fs-1">${plan.price}$</p>
        </div>
        <div class="card-footer d-flex flex-column">
            <div class="choose_btn btn btn-light align-self-center shadow"  duration="${plan.duration}" id="${plan.paymentPlanID}" onclick="extendSubscription(this)">
                Choose
            </div>
        </div>  
    </div>
    `;
}


function requestTopic() {
  fetch("/api/SendSupportTicket/GetAllTopic")
    .then((res) => res.json())
    .then((json) => {
      loadTopic(json);
    });
}
let toast;
function showToast() {
  if (toast == null) {
    toast = new bootstrap.Toast(document.getElementById("message_toast"));
  }
  toast.show();
}

function hideToast() {
  if (toast == null) {
    toast = new bootstrap.Toast(document.getElementById("message_toast"));
  }
  toast.hide();
}

function validateForm() {
  let content = document.getElementById("content");
  let formCotainer = content.parentNode;
  let isContentValidateSuccess = false;
  formCotainer.classList.add("was-validated");
  if (content.value.trim().length = 0) {
    option.setCustomValidity("empty");
  }
  else {
    option.setCustomValidity("");
    isContentValidateSuccess = true;
    formCotainer.classList.remove("was-validated");
  }
  return isContentValidateSuccess;
}

function setToastContent(isSuccess) {
  let toastContent = document.getElementById("toast_content");
  let message = isSuccess ? `<i class="fas fa-check-circle" style="color: #4bca81"></i> Send sucess!`
    : `<i class="fas fa-times-circle text-danger"></i> Something went wrong!`;
  if (toastContent.innerHTML != "") {
    toastContent.innerHTML = "";
  }
  toastContent.insertAdjacentHTML("afterbegin", message);
}

function sendTicket() {
  if (!validateForm()) {
    return;
  }
  ticketContent.UserEmail = getProfile().userEmail;
  let selectedTopic = document.getElementById("option");
  let content = document.getElementById("content");
  ticketContent.TopicName = selectedTopic[ selectedTopic.selectedIndex ].value;
  ticketContent.Content = content.value;
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(ticketContent)
  };
  fetch("/api/SendSupportTicket/SendTicket", initObject)
    .then(res => res.json())
    .then(json => {
      console.log(json);
      if (json.message == "Success") {
        console.log("modal")
        changeContent(json.message, true);
        showModal();
      }
      else {
        console.log("toast")
        setToastContent(false);
        showToast();
      }
    })
}

