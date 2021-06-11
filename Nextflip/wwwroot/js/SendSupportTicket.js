function renderOption(item, index) {
  return `
      <option value="${index}">${item.topicName}</option>
      `;
}

function appendToWrapper(items) {
  document.getElementById("option").insertAdjacentHTML("afterbegin", items);
}

function onLoad(data) {
  let optionArr = data.map((item, index) => {
    return renderOption(item, index);
  });
  optionArr = optionArr.join("");
  appendToWrapper(optionArr);
}

fetch("https://localhost:44341/api/SendSupportTicket/GetAllTopic")
  .then((res) => res.json())
  .then((json) => {
    onLoad(json);
  });
