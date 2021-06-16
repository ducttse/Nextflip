let Data = {
  data: []
};
let requestParam = {
  NumberOfPage: 3,
  RowsOnPage: 10,
  RequestPage: 1
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

function onLoad(rowsPerPage) {
  appendTicketToWrapper(0, rowsPerPage);
  appendPagination(Data.data.length, rowsPerPage);
  setCurrentColor(1);
  setClickToIndex(appendTicketToWrapper);
}

function setMaxPage(resolve) {
  fetch("/api/ViewSupporterDashboard/GetNumOfSupportTicket")
    .then((res) => res.json())
    .then((json) => {
      rowsPerPage = requestParam.RowsOnPage;
      maxPage = parseInt(parseInt(json) / rowsPerPage);
      resolve("resolved");
    });
}

function CountCurrentLoadedPage() {
  return parseInt(Data.data.length / requestParam.RowsOnPage)
}

function RequestMoreData(RequestPage) {
  requestParam.RequestPage = RequestPage;
  console.log(Data.data.length);

  PostRequest(RequestPage).then(res => res.json()).then(json => {
    json.forEach(user => {
      Data.data.push(user);
    })
    let num = RequestPage - 1;
    appendTicketToWrapper(num * rowsPerPage, (num + 1) * rowsPerPage);
    setCurrentColor(RequestPage);
  })
}

function preLoad() {
  return new Promise((resolve, reject) => {
    setMaxPage(resolve);
  });
}

function PostRequest(page) {
  let reqHeader = new Headers();
  reqHeader.append("Content-Type", "text/json");
  reqHeader.append("Accept", "application/json, text/plain, */*");
  if (page !== 1) {
    requestParam.RequestPage = page;
  }
  let initObject = {
    method: "POST",
    headers: reqHeader,
    body: JSON.stringify(requestParam)
  };
  //
  return fetch(
    "/api/ViewSupporterDashboard/GetPendingSupportTickets",
    initObject
  )
}


function Run(rowsPerPage) {
  PostRequest(1)
    .then((response) => response.json())
    .then((json) => {
      Data.data = json;
      onLoad(rowsPerPage);
    });
}
