﻿let TopicArr;
let currentIndex;
function renderItem(item, itemName, index) {
    return `
        <li class="topicItem p-2 rounded-3 mt-1" id="item_${index}" index=${index} topic="${item[ itemName ]}" prop="${itemName}">
            <a href="#" class="text-decoration-none link-light rounded">
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
            class="btn btn-dark btn-lg text-light align-items-center rounded collapsed"
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

function setClickToItems(requestFunc, appendToWrapper) {
    let collection = document.getElementsByClassName("topicItem");
    for (let item of collection) {
        let topic = item.getAttribute("topic");
        let index = item.getAttribute("index");
        let prop = item.getAttribute("prop");
        item.addEventListener("click", () => {
            requestFunc(topic)
                .then(res => res.json())
                .then(json => {
                    Data = json;
                    if (pageData !== null) {
                        if (pageData.currentPage !== 1) {
                            setCurrentColor();
                            removeCurrentColor();
                            pageData.currentPage = 1;
                        }
                    }
                    setTopic(topic);
                    isSearched = false;
                    setChoosenColor(index);
                    appendToWrapper();
                });
        })
    }
}