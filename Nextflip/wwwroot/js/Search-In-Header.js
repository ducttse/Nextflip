function displaySearch() {
    let searchHolder = document.getElementById("search_holder");
    let searchBtn = document.getElementById("searchBtn");
    if (searchHolder.classList.contains("btn_hide")) {
        searchHolder.classList.remove("btn_hide");
        searchHolder.classList.add("btn_show");
        searchBtn.classList.add("animate");
    }
    else {
        searchHolder.classList.remove("btn_show");
        searchHolder.classList.add("btn_hide");
        searchBtn.classList.remove("animate");
    }
}

function showError(searchValue) {
    let resultHolder = document.getElementById("result_holder");
    let oldError = resultHolder.querySelector("#error");
    let oldResult = resultHolder.querySelector(".row");
    if (oldError !== null || oldResult !== null) {
        resultHolder.innerHTML = ""
    }
    let error = `<div id="error">  
                    <p class="text-light fs-4">Sorry we couldn't find any match <b>${searchValue}</b></p>
                </div>`
    resultHolder.insertAdjacentHTML("afterbegin", error);
}

function renderResult(result) {
    return `
    <div class="col-4 result mb-2 p-1" onclick="return AppendDetails('${result.media.mediaID}');">
        <div id="img_container">
            <img src="${result.media.bannerURL}" alt="${result.media.mediaID}" style="width: 100%; height: 100%; object-fit: cover"/>
            <div id="title">
                <p class="link-light p-1 fs-6 ">${result.media.title}</p>
            </div>
        </div>
    </div>`;
}

function renderResults(resultArray) {
    let rendered = resultArray.map(result => {
        return renderResult(result);
    })
    rendered = rendered.join("");
    return `
        <div class="row">
            ${rendered}
        </div>
    `
}

function appendResult(resultArray) {
    let resultHolder = document.getElementById("result_holder");
    let oldError = resultHolder.querySelector("#error");
    let oldResult = resultHolder.querySelector(".row");
    if (oldError !== null || oldResult !== null) {
        resultHolder.innerHTML = ""
    }
    document.getElementById("result_holder").insertAdjacentHTML("afterbegin", renderResults(resultArray));
}

function setEnterEvent(func) {
    let search = document.getElementById("search");
    search.addEventListener("keyup", (evt) => {
        if (evt.keyCode === 13) {
            if (func.name === "search") {
                let value = search.value;
                func(value);
            }
            else {
                func();
            }
        }
    })
}

function search(searchValue) {
    fetch(`/api/ViewSubscribedUserDashboard/GetMediasByTitle/${searchValue}`)
        .then(res => res.json())
        .then(json => {
            console.log(json);
            if (json.length == 0) {
                console.log("error");
                showError(searchValue);
            }
            else appendResult(json);
        });
}

function goToSearch() {
    let value = document.getElementById("search").value;
    window.location.href = `/SubcribedUserDashBoard/Search/${value}`;
}

function setSearchEvent(value) {
    document.getElementById("searchBtn").addEventListener("click", () => {
        displaySearch();
    })
    setEnterEvent(search);
    if (value != null) {
        document.getElementById("search").setAttribute("value", value);
    }
}

function setGoToSearchEvent() {
    document.getElementById("searchBtn").addEventListener("click", () => {
        displaySearch();
    })
    setEnterEvent(goToSearch);
}
