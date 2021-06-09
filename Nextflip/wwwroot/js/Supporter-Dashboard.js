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
              href="/Edit/${ticket.supportTicketID}">
                  Details
              </a>
          </td>
          <td>
            <button name="btnAction" value="technical" class="btn btn-warning">Send to Technical</button>
            <button name="btnAction" value="customerRelation" class="btn btn-warning">Send to Customer Relation</button>
          </td>
      </tr>`;
}
let rowsPerPage = 10;
let currentPage = 0;

function renderPagination(length, rowsPerPage) {
  let numberOfPage = Math.ceil(length / rowsPerPage);
  let Pages = "";
  for (let i = 1; i <= numberOfPage; i++) {
    Pages += `<li class="page-item" page="${i}" onClick="setCurrentPage(${i})"><a class="page-link" href="#">${i}</a></li>`;
  }
  return `
    <nav>
      <ul class="pagination">
        <li class="page-item disabled">
          <a class="page-link" href="#">Previous</a>
        </li>
        ${Pages}
        <li class="page-item"><a class="page-link" href="#">Next</a></li>
      </ul>
      </nav>`;
}
// jump to another pagination
function setCurrentPage(number) {
  currentPage = number - 1;
  appendTicketToWrapper(
    currentPage * rowsPerPage,
    rowsPerPage + Data.data.length - currentPage * rowsPerPage > rowsPerPage
      ? ++currentPage * rowsPerPage
      : currentPage * rowsPerPage +
          (Data.data.length - currentPage * rowsPerPage)
  );
  setCurrentColor(number);
}

function removeCurrentColor() {
  let pageArray = Array.from(document.getElementsByClassName("page-item"));
  let curPage = pageArray.filter((page) => {
    return page.classList.contains("active");
  });
  if (curPage.length > 0) {
    curPage[0].classList.remove("active");
  }
}

function setCurrentColor(number) {
  removeCurrentColor();
  let pageArray = Array.from(document.getElementsByClassName("page-item"));
  let curPage = pageArray.filter((page) => {
    return parseInt(page.getAttribute("page")) === number;
  });
  curPage[0].classList.add("active");
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

function appendPagination(length, rowsPerPage) {
  document
    .getElementById("pagination")
    .insertAdjacentHTML("afterbegin", renderPagination(length, rowsPerPage));
}

function onLoad() {
  appendPagination(Data.data.length, rowsPerPage);
  appendTicketToWrapper(0, rowsPerPage);
}

onLoad();

function Run() {
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
      console.log(json);
      Data.data = json;
      onLoad();
    });
}
Run();
