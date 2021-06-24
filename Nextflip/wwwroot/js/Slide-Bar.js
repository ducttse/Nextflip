let TopicArr;
let currentIndex;
function renderItem(item, itemName, index) {
    return `
        <li class="topicItem p-2 rounded-3 mt-1" id="item_${index}" index=${index} topic="${item[ itemName ]}" prop="${itemName}">
            <a href="#" class="ps-3 text-decoration-none link-light rounded">
            ${item[ itemName ]}
            </a>
        </li>`;
}

function renderCollapse(Name, itemName) {
    let Items = TopicArr.map((item, index) => {
        return renderItem(item, itemName, index);
    })
    Items = Items.join("");
    return `
    <li class="mb-1">
        <button
            class="side_bar_btn btn btn-lg btn-dark text-light text-start rounded collapsed w-100"
            data-bs-toggle="collapse"
            data-bs-target="#topic-collapse"
            aria-expanded="true"
        >
            ${Name}
        </button>
        <div class="collapse show" id="topic-collapse">
            <ul class="btn-toggle-nav list-unstyled pb-1 small">
                ${Items}
            </ul>
        </div>
    </li>`;
}

function appendCollase(name, itemName, requestFunc, appendToWrapper) {
    document.getElementById("topic_List").insertAdjacentHTML("afterbegin", renderCollapse(name, itemName));
    setClickToItems(requestFunc, appendToWrapper);
}

function setChoosenColor(chooseIndex) {
    let current = document.getElementById(`item_${currentIndex}`);
    if (current !== null) {
        current.classList.remove("choose");
    }
    currentIndex = chooseIndex;
    let choosenItem = document.getElementById(`item_${chooseIndex}`);
    choosenItem.classList.add("choose");
}

function appendButton(button) {
    console.log("append")
    document.getElementById("topic_List").insertAdjacentHTML("afterbegin", button);
}

function setClickToItems(requestFunc, appendToWrapper) {
    let collection = document.getElementsByClassName("topicItem");
    for (let item of collection) {
        let topic = item.getAttribute("topic");
        let index = item.getAttribute("index");
        item.addEventListener("click", () => {
            requestFunc(topic)
                .then(res => res.json())
                .then(json => {
                    Data = json;
                    if (json.totalPage == 0) {
                        ShowNotFound();
                    }
                    else {
                        if (pageData !== null) {
                            if (pageData.currentPage !== 1) {
                                setCurrentColor();
                                removeCurrentColor();
                                pageData.currentPage = 1;
                            }
                        }
                        resetSearch();
                        resetFilter();
                        HideNotFound();
                        setTopic(topic);
                        isSearched = false;
                        setChoosenColor(index);
                        appendToWrapper();
                    }
                });
        })
    }
}