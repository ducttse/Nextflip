let TopicArr;
function renderItem(item) {
    return `
        <li class="topicItem" topic="${item.topicName}">
            <a href="#" class="text-decoration-none link-light rounded">
            ${item.topicName}
            </a>
        </li>`;
}

function renderCollapse() {
    let Items = TopicArr.map((item) => {
        return renderItem(item);
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
            Topic
        </button>
        <div class="collapse show" id="topic-collapse">
            <ul class="btn-toggle-nav list-unstyled pb-1 small">
                ${Items}
            </ul>
        </div>
    </li>`;
}

function appendCollase(requestFunc, appendToWrapper) {
    document.getElementById("topic_List").insertAdjacentHTML("afterbegin", renderCollapse());
    setClickToItems(requestFunc, appendToWrapper);
}

function setClickToItems(requestFunc, appendToWrapper) {
    let collection = document.getElementsByClassName("topicItem");
    for (let item of collection) {
        let topic = item.getAttribute("topic");
        item.addEventListener("click", () => {
            requestFunc(topic)
                .then(res => res.json())
                .then(json => {
                    Data.data = json;
                    appendToWrapper(0, Data.data.length);
                });
        })
    }
}