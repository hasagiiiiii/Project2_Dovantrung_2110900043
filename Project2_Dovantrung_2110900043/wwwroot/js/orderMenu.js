const cartChecked = document.querySelector("#cart");
const dashboardOrder = document.querySelector(".dashboard-order");
console.log(dashboardOrder);
cartChecked.addEventListener("click", () => {
    dashboardOrder.classList.toggle("checked");
});
