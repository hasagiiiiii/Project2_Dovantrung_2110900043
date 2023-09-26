let addUser = document.querySelector("#add");
let FormAdd = document.querySelector(".form");
let btnUpdate = document.querySelector(".update");
let FormUpdate = document.querySelector(".form-update");
let overlay = document.querySelector(".overlay");
addUser.addEventListener("click", () => {
    FormAdd.classList.toggle("show");
})
btnUpdate.addEventListener("click", () => {
    FormUpdate.classList.toggle("show");
})
overlay.addEventListener("click", () => {
    FormAdd.classList.remove("show");
    FormUpdate.classList.remove("show");
})