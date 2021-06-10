let Data = {
  data: [
    {
      supportTicketID: "1fcumqdcKvBJhhDtkQqB",
      userEmail: "iSzNDhj4a4hl3V0akEfQ@gmail.com",
      topicName: "Others",
      status: "Pending",
      content:
        "egg portion sleep deck like loss guess different professor pie closer confusion symptom global officer progress happy pocket frequency test paint sacred exceed tea orange measure foreign read council baby miracle contribute Jewish determine operate movie dinner privacy adapt may capture use bottle base allow recommend movement growth home himself ready bet summit block connection fight instructor furthermore psychological identification efficient politics cooking difficult importance appearance rub given think second carefully cast reasonable peace respect stare cap positive responsible fat offense beach bright illustrate poor exhibit downtown financial model then thanks large doubt club slightly detect then acquire legal shortly"
    }
  ]
};

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

function appendTicket() {
  document
    .getElementById("contentWrapper")
    .insertAdjacentHTML("afterbegin", renderTicket(Data.data));
}
