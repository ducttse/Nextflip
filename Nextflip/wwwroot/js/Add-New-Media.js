let modaAddlForm;

function showForm() {
    if (modaAddlForm == null) {
        modaAddlForm = new bootstrap.Modal(document.getElementById('modalAddForm'), {
            keyboard: false
        })
    }
    modaAddlForm.show();
}