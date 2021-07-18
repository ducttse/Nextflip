let requestAddNewMediaObj = {
    UserEmail: "",
    Title: "",
    FilmType: "",
    Director: "",
    Cast: "",
    PublishYear: "",
    Duration: "",
    BannerURL: "",
    Language: "",
    Description: "",
    CategoryIDArray: 1
}

function renderCategoryCheckBox(category) {
    return `
        <div class="form-check form-check-inline">
            <input class="form-check-input category" type="checkbox" id="inlineCheckbox1" value="${category.categoryID}">
            <label class="form-check-label text-capitalize" for="inlineCheckbox1">${category.name}</label>
        </div>
    `;
}

function requestCategories() {
    fetch(`/api/ViewSubscribedUserDashboard/GetCategories`).then(res => res.json()).then(json => {
        let checkboxs = json.map(category => {
            return renderCategoryCheckBox(category);
        }).join("");
        document.getElementById("CB_holder").insertAdjacentHTML("afterbegin", checkboxs);
        let options = json.map(category => {
            return `<option value="${category.name}">${category.name}</option>`
        }).join("");
        document.getElementById("category_filter").insertAdjacentHTML("beforeend", options);
    })
}

function requestMediaType() {
    fetch(`/api/FilmTypeManagement/GetAllFilmTypes`).then(res => res.json()).then(json => {
        let selectEl = json.map(option => {
            return `<option value="${option.type}">${option.type}</option>`
        }).join("");
        document.getElementById("filmType").insertAdjacentHTML("afterbegin", selectEl);
    })
}

function validateCheckBoxValue() {
    if (document.querySelectorAll(`#modalAddForm .category[type="checkbox"]:checked`).length == 0) {
        document.getElementById("empty_checkbox").classList.remove("d-none");
    }
    else {
        document.getElementById("empty_checkbox").classList.add("d-none");
    }
}

function getChosenCategory() {
    return Array.from(document.querySelectorAll('input[type="checkbox"].category'))
        .filter(cate => cate.checked)
        .map(cate => cate.value);
}

function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}

const onInput = debounce((obj) => {
    let parent = obj.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    parent.classList.add("was-validated");
    if (obj.value.trim().length == 0) {
        feedback.textContent = "This field can not empty";
        obj.setCustomValidity("empty");
    }
    else {
        parent.classList.remove("was-validated");
        feedback.textContent = "";
        obj.setCustomValidity("");
    }
});

async function setInputValue() {
    requestAddNewMediaObj.UserEmail = getProfile().userEmail;
    requestAddNewMediaObj.FilmType = document.getElementById("filmType").value;
    requestAddNewMediaObj.CategoryIDArray = getChosenCategory();
    requestAddNewMediaObj.Title = document.getElementById("title").value;
    requestAddNewMediaObj.Cast = document.getElementById("cast").value;
    requestAddNewMediaObj.PublishYear = document.getElementById("publicYear").value;
    requestAddNewMediaObj.Duration = document.getElementById("duration").value;
    requestAddNewMediaObj.Language = document.getElementById("language").value;
    requestAddNewMediaObj.Description = document.getElementById("description").value;
    requestAddNewMediaObj.Director = document.getElementById("director").value;
    requestAddNewMediaObj.BannerURL = await requestUploadBanner();
}

function checkEmpty(obj) {
    let parent = obj.parentNode;
    let feedback = parent.querySelector(".invalid-feedback");
    parent.classList.add("was-validated");
    if (obj.value.trim().length == 0) {
        feedback.textContent = "This field can not empty";
        obj.setCustomValidity("empty");
    }
    else {
        parent.classList.remove("was-validated");
        feedback.textContent = "";
        obj.setCustomValidity("");
    }
}

function validateInputAddMedia() {
    checkEmpty(document.getElementById("title"));
    checkEmpty(document.getElementById("description"));
    getFile(document.getElementById("banner"));
}


async function requestAddMedia() {
    await setInputValue();
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify(requestAddNewMediaObj)
    };
    fetch("/api/ViewEditorDashboard/AddMedia", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "success") {
                document.querySelector("#modalAddForm .btn-close").click()
                changeContent("Add success", true);
                messageModal.show();
            }
            else {
                validateCheckBoxValue();
                validateInputAddMedia();
            }
        })
}

requestCategories();
requestMediaType();

function resetRequestAddNewMediaObj() {
    requestAddNewMediaObj.CategoryIDArray = "";
    requestAddNewMediaObj.Title = "";
    requestAddNewMediaObj.Cast = ""
    requestAddNewMediaObj.PublishYear = ""
    requestAddNewMediaObj.Duration = ""
    requestAddNewMediaObj.Language = ""
    requestAddNewMediaObj.Description = ""
    requestAddNewMediaObj.BannerURL = ""
    requestAddNewMediaObj.Director = ""
    let title = document.getElementById("title");
    let cast = document.getElementById("cast");
    let publicYear = document.getElementById("publicYear");
    let duration = document.getElementById("duration");
    let language = document.getElementById("language");
    let description = document.getElementById("description");
    let Banner = document.getElementById("banner");
    let director = document.getElementById("director");
    title.value = "";
    cast.value = "";
    publicYear.value = "";
    duration.value = "";
    language.value = "";
    description.value = "";
    Banner.value = "";
    director.value = "";
    document.getElementById("empty_checkbox").classList.remove("d-none");
    title.parentNode.classList.remove("was-validated");
    description.parentNode.classList.remove("was-validated");
    Banner.parentNode.classList.remove("was-validated");

}

document.getElementById("modalAddForm").addEventListener("hidden.bs.modal", () => {
    resetRequestAddNewMediaObj();
})