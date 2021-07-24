function renderRow(category, index) {
    return `
    <tr>
        <td scope="col" class="col-3">${index}</td>
        <td scope="col" class="col-6">${category.name}</td>
        <td scope="col" class="col-3 text-start" onclick="setEditModalContent('Edit', 'Category', '${category.name}','${category.categoryID}')"><i class="ps-2 fas fa-edit"></i></td>
    </tr>`;
}

function appendToContainer(data) {
    let rows = data.map((category, index) => {
        return renderRow(category, index + 1)
    }).join("");
    document.getElementById("dataContainer").innerHTML = rows;
}

function requestCategories() {
    fetch("/api/ViewSubscribedUserDashboard/GetCategories")
        .then(res => res.json())
        .then(json => appendToContainer(json))
}

function renderMediaTypeRow(type, index) {
    console.log(type);
    return `
    <tr>
        <td scope="col" class="col-3">${index}</td>
        <td scope="col" class="col-6">${type.type}</td>
        <td scope="col" class="col-3 text-start" onclick="setEditModalContent('Edit', 'Media type', '${type.type}', '${type.typeID}')"><i class="ps-2 fas fa-edit"></i></td>
    </tr>`;
}

function appendToMediaTypeContainer(data) {
    let rows = data.map((type, index) => {
        return renderMediaTypeRow(type, index + 1)
    }).join("");
    document.getElementById("mediaTypeDataContainer").innerHTML = rows;
}

function requestMediaTypes() {
    fetch("/api/FilmTypeManagement/GetAllFilmTypes")
        .then(res => res.json())
        .then(json => appendToMediaTypeContainer(json))
}

function create(obj) {
    let type = obj.getAttribute("itemType");
    console.log(type);
    let value = document.getElementById("item_name").value;
    switch (type) {
        case "category":
            createCategory(value);
            break;
        case "media type":
            createMediaType(value);
            break;
    }
}

function update(obj) {
    let type = obj.getAttribute("itemType");
    console.log(type);
    let value = document.getElementById("item_edit_name").value;
    let id = obj.getAttribute("cateID");
    switch (type) {
        case "category":
            updateCategory(id, value);
            break;
        case "media type":
            updateMediaType(id, value);
            break;
    }
}


function createCategory(name) {
    fetch(`/api/CategoryManagement/CreateNewCategory/${name}`)
        .then(res => res.json())
        .then(json => {
            addModal.hide();
            if (json.message == "Success") {
                setFlashModalContent(true);
            }
            else {
                setFlashModalContent(false);
            }
        })
}

function updateCategory(id, newName) {
    fetch(`/api/CategoryManagement/UpdateCategory/${id}/${newName}`)
        .then(res => res.json())
        .then(json => {
            editModal.hide();
            if (json.message == "Success") {
                setFlashModalContent(true);
            }
            else {
                setFlashModalContent(false);
            }
        })
}

function createMediaType(type) {
    fetch(`/api/FilmTypeManagement/CreateNewFilmType/${type}`)
        .then(res => res.json())
        .then(json => {
            addModal.hide();
            if (json.message == "Success") {
                setFlashModalContent(true);
            }
            else {
                setFlashModalContent(false);
            }
        })
}

function updateMediaType(id, newName) {
    fetch(`/api/FilmTypeManagement/UpdateFilmType/${id}/${newName}`)
        .then(res => res.json())
        .then(json => {
            editModal.hide();
            if (json.message == "Success") {
                setFlashModalContent(true);
            }
            else {
                setFlashModalContent(false);
            }
        })
}


let addModal = new bootstrap.Modal(document.getElementById('add_Modal'), {
    keyboard: false
})
let editModal = new bootstrap.Modal(document.getElementById('edit_Modal'), {
    keyboard: false
})

let flashModel = new bootstrap.Modal(document.getElementById('modal_flash'), {
    keyboard: false
})

function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}

function checkEmpty(obj) {
    let parent = obj.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    parent.classList.add("was-validated");
    if (obj.value.trim().length == 0) {
        feedback.textContent = "This field can not empty";
        obj.setCustomValidity("empty");
        return false;
    }
    else {
        parent.classList.remove("was-validated");
        feedback.textContent = "";
        obj.setCustomValidity("");
        return true;
    }
}

function setAddModalContent(title, label) {
    document.getElementById("add_title").textContent = title;
    document.getElementById("add_label").textContent = label;
    if (title == "Create new category") {
        document.getElementById("trigger_add").setAttribute("itemType", "category");
    }
    else if (title == "Create new meida type") {
        document.getElementById("trigger_add").setAttribute("itemType", "media type");
    }
    addModal.show();
}

function setEditModalContent(title, label, value, id) {
    document.getElementById("edit_title").textContent = title;
    document.getElementById("edit_label").textContent = label;
    document.getElementById("item_edit_name").value = value;
    document.getElementById("trigger_update").setAttribute("cateID", id);
    if (label == "Category") {
        document.getElementById("trigger_update").setAttribute("itemType", "category");
    }
    else if (label == "Media type") {
        document.getElementById("trigger_update").setAttribute("itemType", "media type");
    }
    editModal.show();
}

function setFlashModalContent(bool) {
    let content = !bool ? ` <i class="far fa-times-circle fa-5x text-danger"></i>
                    <p class="fs-5">Opps! Something went wrong</p>
                    <button type="button" class="col-4 mx-auto btn btn-danger text-white" data-bs-dismiss="modal">
                     Try again
                    </button>`
        : ` <i class="far fa-check-circle fa-5x" style="color: #4bca81"></i>
                    <p class="fs-5">Success</p>
                    <button type="button" class="col-4 mx-auto btn btn-success text-white" 
                        style=" background-color: #4bca81 !important; border: #4bca81 !important;"
                        data-bs-dismiss="modal">
                        Continue
                    </button>`;
    document.getElementById("flash_message").innerHTML = content;
    flashModel.show();
}
document.getElementById("modal_flash").addEventListener("hidden.bs.modal", () => {
    location.reload();
})