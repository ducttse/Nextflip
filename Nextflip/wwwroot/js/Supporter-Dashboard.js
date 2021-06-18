let Data = {
  data: []
};
let requestParam = {
  RowsOnPage: 10,
  RequestPage: 1,
  TopicName: "Account"
};

function renderTicket(ticket, index) {
  return `
      <tr>
          <td>${index + 1}</td>
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


function setTotalPage() {
  pageData.totalPage = Data.totalPage
}

function countStart() {
  return (pageData.currentPage - 1) * requestParam.RowsOnPage
}

function appendTicketToWrapper(start, end) {
  let ticketArray = Data.data.slice(start, end).map((ticket, index) => {
    return renderTicket(ticket, index + start);
  });
  ticketArray = ticketArray.join("");
  let dataWapper = document.getElementById("dataWapper");
  if (dataWapper.innerHTML !== "") {
    dataWapper.innerHTML = "";
  }
  dataWapper.insertAdjacentHTML("afterbegin", ticketArray);
}

function requestTopicData(topic) {
  requestParam.TopicName = topic;
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  return fetch("/api/ViewSupporterDashboard/GetPendingSupportTickets", initObject);
}

function getTopics() {
  fetch("/api/ViewSupporterDashboard/GetAllSupportTopics")
    .then(res => res.json())
    .then(json => {
      TopicArr = json;
      appendCollase("Topic", requestTopicData, appendTicketToWrapper);
    })
}

function Run() {
  requestData()
    .then((response) => response.json())
    .then((json) => {
      Data.data = json;
      appendTicketToWrapper(0, Data.data.length);
    });
}


