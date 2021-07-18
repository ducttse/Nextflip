function renderRow(category, index) {
    return `
    <tr>
        <td scope="col" class="col-3">${index}</td>
        <td scope="col" class="col-6">${category.name}</td>
        <td scope="col" class="col-3 text-start" onclick="#${category.categoryID}"><i class="ps-2 fas fa-edit"></i></td>
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
    return `
    <tr>
        <td scope="col" class="col-3">${index}</td>
        <td scope="col" class="col-6">${type}</td>
        <td scope="col" class="col-3 text-start" onclick="#${type}"><i class="ps-2 fas fa-edit"></i></td>
    </tr>`;
}

function appendToMediaTypeContainer(data) {
    console.log(data);
    let rows = data.map((type, index) => {
        return renderRow(type, index + 1)
    }).join("");
    document.getElementById("mediaTypeDataContainer").innerHTML = rows;
}

function requestMediaTypes() {
    fetch("/api/FilmTypeManagement/GetAllFilmTypes")
        .then(res => res.json())
        .then(json => appendToMediaTypeContainer(json))
}

function createCategory(name) {
    fetch(`/api/CategoryManagement/CreateNewCategory/Action${name}`)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Success") {
                location.reload();
            }
        })
}