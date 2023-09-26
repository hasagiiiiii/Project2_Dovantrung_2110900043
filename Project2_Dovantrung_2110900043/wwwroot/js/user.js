let btUpload = document.querySelector(".btnUpload");
let showForm = document.querySelector(".form_changeUser");
let overLay = document.querySelector(".overlay");
btUpload.addEventListener("click", () => {
    showForm.classList.toggle("show");
});

overLay.addEventListener("click", () => {
    showForm.classList.remove("show");
});
