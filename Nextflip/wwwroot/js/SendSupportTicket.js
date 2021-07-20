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
      if (json == "Success") {
        setToastContent(true);
        showToast();
        selectedTopic[ 0 ].selected = "selected";
        content.value = "";
        ticketContent.TopicName = "";
        ticketContent.Content = "";
        content.parentNode.classList.remove("was-validated");
      }
      else {
        setToastContent(false);
        showToast();
      }
    })
}

