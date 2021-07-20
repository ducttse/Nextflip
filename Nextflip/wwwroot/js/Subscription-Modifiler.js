
function extendSubscription(obj) {
    let day = obj.getAttribute("duration");
    let planID = obj.getAttribute("id");
    let Id = getProfile().userID;
    let time = (new Date(Date.now())).toISOString().slice(0, 19).replace('T', ' ');
    let reqHeader = new Headers();
    reqHeader.append("Content-Type", "text/json");
    reqHeader.append("Accept", "application/json, text/plain, */*");
    let initObject = {
        method: "POST",
        headers: reqHeader,
        body: JSON.stringify({
            UserId: Id,
            ExtensionDays: day,
            PaymentPlanId: planID,
            IssueTime: time
        })
    };
    console.log(initObject.body);
    fetch("/api/SubscriptionManagement/ExtendSubscription", initObject)
        .then(res => res.json())
        .then(json => {
            if (json.message == "Purchase Successfully ! Your subscription has been extended !") {
                changeContent(json.message, true);
            }
            else {
                changeContent(json.message, false);
            }
            showModal();
        })
}
let modal;

function showModal() {
    if (modal == null) {
        modal = new bootstrap.Modal(document.getElementById('Modal'), {
            keyboard: false
        })
    }
    modal.show();
}

function changeContent(text, bool) {
    let content = !bool ?
        ` <i class="far fa-times-circle fa-5x text-danger text-center"></i>
            <p class="fs-5 text-center text-dark">${text}</p>
            <button type="button" class="col-4 mx-auto btn btn-danger text-white" data-bs-dismiss="modal">
                Close
            </button>` :
        ` <i class="far fa-check-circle fa-5x text-center" style="color: #4bca81"></i>
            <p class="fs-5 text-center text-dark">${text}</p>
            <button type="button" class="col-4 mx-auto btn btn-success text-white"  style=" background-color: #4bca81 !important; border: #4bca81 !important;" data-bs-dismiss="modal">
                Continue
            </button>`;
    document.querySelector(".modal .modal-body").innerHTML = content;
}

function renderCard(index, plan) {
    let month = Math.floor(plan.duration / 30);
    return `
    <div id="item${index}" class="col-4 card text-white bg-primary mb-3 me-3" style="max-width: 18rem;">
        <div class="card-body d-flex flex-column justify-content-between">
            <p class="card-title text-center h5">${month} ${month > 1 ? "months" : "month"}</p>
            <p class="card-text text-center fs-1">${plan.price}$</p>
        </div>
        <div class="card-footer d-flex flex-column">
            <div class="choose_btn btn btn-light align-self-center shadow"  duration="${plan.duration}" id="${plan.paymentPlanID}" onclick="extendSubscription(this)">
                Choose
            </div>
        </div>  
    </div>
    `;
}

function requestPlans() {
    fetch("/api/SubscriptionManagement/GetPaymentPlan")
        .then(res => res.json())
        .then(json => {
            let rendered = json.map((data, index) => {
                return renderCard(index + 1, data);
            }).join("");
            document.getElementById("cardHolder").innerHTML = rendered;
        })

}