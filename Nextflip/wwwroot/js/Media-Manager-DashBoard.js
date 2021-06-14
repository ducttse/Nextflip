let Data = {
  data: []
};

function renderRequest(request) {
  return `
      <tr>
          <td>${request.requestID}</td>
          <td>${request.userEmail}</td>
          <td>${request.note}</td>
          <td><a class="text-decoration-none" href="#${request.mediaID}">Preview</a></td>
          <td>
              <button class="btn btn-primary col-5">
                  <i class="fas fa-check text-white"></i>
              </button>
              <button class="btn btn-danger col-5">
                  <i class="fas fa-times"></i>
              </button>
          </td>
      </tr>`;
}

function appendRequest(start, end) {
  let requestArray = Data.data.slice(start, end).map((request) => {
    return renderRequest(request);
  });
  requestArray = requestArray.join("");
  let requestWrapper = document.getElementById("requestWrapper");
  if (requestWrapper.innerHTML !== "") {
    requestWrapper.innerHTML = "";
  }
  requestWrapper.insertAdjacentHTML("afterbegin", requestArray);
}

function Run(rowsPerPage) {
  fetch("/api/MediaManagerManagement/GetAllPendingMedias")
    .then((res) => res.json())
    .then((json) => {
      Data.data = json;
      appendRequest(0, rowsPerPage);
      appendPagination(Data.data.length, rowsPerPage);
      setCurrentColor(1);
      setClickToIndex(appendRequest);
    });
}
