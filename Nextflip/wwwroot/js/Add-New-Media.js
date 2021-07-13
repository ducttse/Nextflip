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
    })
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
    requestAddNewMediaObj.CategoryIDArray = getChosenCategory();
    requestAddNewMediaObj.Title = document.getElementById("title").value;
    requestAddNewMediaObj.FilmType = document.getElementById("filmType").value;
    requestAddNewMediaObj.Cast = document.getElementById("cast").value;
    requestAddNewMediaObj.PublishYear = document.getElementById("publicYear").value;
    requestAddNewMediaObj.Duration = document.getElementById("duration").value;
    requestAddNewMediaObj.Language = document.getElementById("language").value;
    requestAddNewMediaObj.Description = document.getElementById("description").value;
    requestAddNewMediaObj.BannerURL = await requestUploadBanner();
}

function requestAddMedia() {
    setInputValue();
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
                console.log("success");
            }
        })
}

requestCategories();
