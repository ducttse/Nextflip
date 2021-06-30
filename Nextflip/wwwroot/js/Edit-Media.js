let modalEditForm;

function showEditForm() {
    if (modalEditForm == null) {
        modalEditForm = new bootstrap.Modal(document.getElementById('modalEditForm'), {
            modalEditForm: false
        })
    }
    modalEditForm.show();
}