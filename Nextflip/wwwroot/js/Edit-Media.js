let modalEditForm;

function showEditForm() {
    if (modalEditForm == null) {
        modalEditForm = new bootstrap.Modal(document.getElementById('modal'), {
            modalEditForm: false
        })
    }
    modalEditForm.show();
}

