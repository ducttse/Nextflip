function renderTicket(ticket) {
  return `
  <div id="useremail">
    <p><b>User's email:</b> ${ticket.userEmail}</p>
  </div>
  <div id="topic">
    <p><b>Topic:</b> ${ticket.topicName}</p>
  </div>
  <div id="content">
    <b>Content:</b>
    <p>
      ${ticket.content}
    </p>
  </div>`;
}

function appendTicket(data) {
  document
    .getElementById("contentWrapper")
    .insertAdjacentHTML("afterbegin", renderTicket(data));
}

function Run(id) {
  fetch(`/api/ViewSupporterDashboard/GetSupportTicketDetails/${id}`)
    .then((response) => response.json())
    .then((json) => {
      appendTicket(json);
    });
}

