function displaySearch() {
    let searchHolder = document.getElementById("search_holder");
    let searchBtn = document.getElementById("searchBtn");
    if (searchHolder.classList.contains("hide")) {
        searchHolder.classList.remove("hide");
        searchHolder.classList.add("show");
        searchBtn.classList.remove("offset-10");
        searchBtn.classList.remove("ps-3");
        searchBtn.classList.add("ps-4");

    }
    else {
        searchHolder.classList.remove("show");
        searchBtn.classList.add("offset-10");
        searchHolder.classList.add("hide");
        searchBtn.classList.remove("ps-4");
        searchBtn.classList.add("ps-3");

    }
}

function showError(searchValue) {
    let resultHolder = document.getElementById("result_holder");
    let oldError = resultHolder.querySelector("#error");
    let oldResult = resultHolder.querySelector("#row");
    if (oldError !== null || oldResult !== null) {
        resultHolder.innerHTML = ""
    }
    let error = `<div id="error">  
                    <p class="text-light fs-4">Sorry we couldn't find any match <b>${searchValue}</b></p>
                </div>`
    console.log(error);
    resultHolder.insertAdjacentHTML("afterbegin", error);
}

function renderResult(result) {
    return `
    <div class="col-3" >
        <a href="/WatchMedia/MediaDetails/${result.media.mediaID}">
            <img
                src="${result.media.bannerURL}"
                alt="${result.media.mediaID}"
                class="w-100 h-100"
            />
        </a>
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
    let oldResult = resultHolder.querySelector("#row");
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
    console.log(searchValue);
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

function setSearchEvent() {
    document.getElementById("searchBtn").addEventListener("click", () => {
        displaySearch();
    })
    setEnterEvent(search);
}

function setGoToSearchEvent() {
    document.getElementById("searchBtn").addEventListener("click", () => {
        displaySearch();
    })
    setEnterEvent(goToSearch);
}
