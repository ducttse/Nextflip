let Data = {
  data: []
};

let RequestObject;

function renderTicket(ticket) {
  return `
      <tr>
          <td>${1}</td>
          <td>${ticket.userEmail}</td>
          <td>${ticket.topicName}</td>
          <td>${ticket.status}</td>
          <td>
              <a class="text-decoration-none" 
              href="/SupporterDashboard/Detail/${ticket.supportTicketID}">
                  Detail
              </a>
          </td>
          <td>
            <button name="btnAction" value="technical" class="btn btn-warning">Send to Technical</button>
            <button name="btnAction" value="customerRelation" class="btn btn-warning">Send to Customer Relation</button>
          </td>
      </tr>`;
}

function appendTicketToWrapper(start, end) {
  let ticketArray = Data.data.slice(start, end).map((ticket) => {
    return renderTicket(ticket);
  });
  ticketArray = ticketArray.join("");
  let dataWapper = document.getElementById("dataWapper");
  if (dataWapper.innerHTML !== "") {
    dataWapper.innerHTML = "";
  }
  dataWapper.insertAdjacentHTML("afterbegin", ticketArray);
}

function onLoad(rowsPerPage) {
  appendTicketToWrapper(0, rowsPerPage);
  appendPagination(Data.data.length, rowsPerPage);
  setCurrentColor(1);
  setClickToIndex(appendTicketToWrapper);
}
function Run(rowsPerPage) {
  //   ///
  //   let searchValue = {
  //     searchValue: "dSRFgJ2L3CqrZJrmOkWD@gmail.com"
  //   };
  //   ////
  //   let reqHeader = new Headers();
  //   reqHeader.append("Content-Type", "text/json");
  //   reqHeader.append("Accept", "application/json, text/plain, */*");

  //   let initObject = {
  //     method: "POST",
  //     headers: reqHeader,
  //     body: JSON.stringify(searchValue)
  //   };
  //   ////
  fetch("/api/ViewSupporterDashboard/GetPendingSupportTickets")
    .then((response) => response.json())
    .then((json) => {
      Data.data = json;
      onLoad(rowsPerPage);
    });
}
