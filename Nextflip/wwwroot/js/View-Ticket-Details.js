let ticketDetail;

function renderTicketDetails(ticket) {
  return `
  <div id="useremail_detail">
    <p><b>User's email:</b> ${ticket.userEmail}</p>
  </div>
  <div id="topic_detail">
    <p><b>Topic:</b> ${ticket.topicName}</p>
  </div>
  <div id="time_detail">
    <p><b>Time:</b> ${ticket.createdDate}</p>
  </div>
  <div id="content_detail">
    <b>Content:</b>
    <p>
      ${ticket.content}
    </p>
  </div>`;
}


function appendTicketDetails(data) {
  let content = document.getElementById("detail_wrapper");
  if (content.innerHTML != "") {
    content.innerHTML = "";
  }
  content.insertAdjacentHTML("afterbegin", renderTicketDetails(data));
}

function showDetail(id) {
  var detail = new bootstrap.Modal(document.getElementById('detail_modal'), {
    keyboard: false
  })
  requestDetails(id).then(
    () => {
      detail.show();
    }
  )
}

function sendTicketDetail(receiver) {
  ticketDetail.btnAction = receiver;
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(ticketDetail)
  };
  changeFlashContent("waiting");
  showFlashModal();
  fetch("/api/ViewSupporterDashboard/ForwardSupportTicket", initObject)
    .then(res => res.json())
    .then(json => {
      if (json.message == "Forward successful") {
        changeFlashContent("success");
      }
    })
}


async function requestDetails(id) {
  await fetch(`/api/ViewSupporterDashboard/GetSupportTicketDetails/${id}`)
    .then((response) => response.json())
    .then((json) => {
      ticketDetail = { btnAction: "", ...json }
      appendTicketDetails(json);
    });
  return new Promise(resolve => resolve("resolved"))
}
let flashMessage;

async function hideFlash() {
  if (flashMessage == null) {
    var flashMessage = new bootstrap.Modal(document.getElementById('modal_flash'), {
      keyboard: false
    })
  }
  await flashMessage.hide();
  return new Promise((resolve) => {
    resolve("resolved");
  })
}

function changeFlashContent(status) {
  let content;
  switch (status) {
    case "waiting":
      content = `<div class="loader"></div>`
      break;
    case "success":
      content = ` <i class="far fa-check-circle fa-5x" style="color: #4bca81"></i>
                  <p class="fs-5">Success</p>
                  <button type="button" class="col-4 mx-auto btn btn-success text-white" style=" background-color: #4bca81 !important; border: #4bca81 !important;" data-bs-dismiss="modal">
                    Continue
                  </button>`
      break;
    case "fail":
      content = ` <i class="far fa-times-circle fa-5x text-danger"></i>
                  <p class="fs-5">Opps! Something went wrong</p>
                  <button type="button" class="col-4 mx-auto btn btn-danger text-white" data-bs-dismiss="modal">
                   Try again
                  </button>`
      break;
  }
  let messageHolder = document.getElementById("flash_message");
  if (messageHolder.innerHTML != "") {
    messageHolder.innerHTML = "";
  }
  messageHolder.insertAdjacentHTML("afterbegin", content);
}

function showFlashModal() {
  if (flashMessage == null) {
    var flashMessage = new bootstrap.Modal(document.getElementById('modal_flash'), {
      keyboard: false
    })
  }
  flashMessage.show();
}