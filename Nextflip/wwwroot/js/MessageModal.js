let messageModal;

function changeContent(text, bool) {
    let content = !bool ?
        ` <i class="far fa-times-circle fa-5x text-danger text-center"></i>
        <p class="fs-5 text-center">${text}</p>
        <button type="button" class="col-4 mx-auto btn btn-danger text-white" data-bs-dismiss="modal">
            close
        </button>` :
        ` <i class="far fa-check-circle fa-5x text-center" style="color: #4bca81"></i>
        <p class="fs-5 text-center">${text}</p>
        <button type="button" class="col-4 mx-auto btn btn-success text-white" onclick="location.reload()"  style=" background-color: #4bca81 !important; border: #4bca81 !important;" data-bs-dismiss="modal">
            Continue
        </button>`;
    document.querySelector("#messageModal .modal-body").innerHTML = content;
}

function showMessageModal() {
    if (messageModal == null) {
        messageModal = new bootstrap.Modal(document.getElementById('messageModal'), {
            keyboard: false
        })
    }
    messageModal.show();
}