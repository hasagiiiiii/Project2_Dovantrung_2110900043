// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/*-----------------------------------*\
  #PRELOAD
\*-----------------------------------*/
const preloader = document.querySelector("[data-preload]");
window.addEventListener("load", function () {
    preloader.classList.add("loaded");
    document.body.classList.add("loaded");
});
/*-----------------------------------*\
  #BUTTON BAR
\*-----------------------------------*/
const bars = document.querySelector(".bar");
const menuTop = document.querySelector(".menu-top");
const iconBar = document.querySelector(".fa-bars");
bars.addEventListener("click", () => {
    menuTop.classList.toggle("open");
});

/*-----------------------------------*\
  Reveal website
\*-----------------------------------*/

window.addEventListener("scroll", () => {
    let reveals = document.querySelectorAll(".reveal");
    for (var i = 0; i < reveals.length; i++) {
        var windowHeight = window.innerHeight;
        var revealTop = reveals[i].getBoundingClientRect().top;
        var revealPoin = 150;
        if (revealTop < windowHeight - revealPoin) {
            reveals[i].classList.add("active");
        } else {
            reveals[i].classList.remove("active");
        }
    }
});

/*-----------------------------------*\
  Expand element 
\*-----------------------------------*/

window.addEventListener("scroll", () => {
    let expands = document.querySelectorAll(".expand");
    for (var i = 0; i < expands.length; i++) {
        var windowHeight = window.innerHeight;
        var expandTop = expands[i].getBoundingClientRect().top;
        var expandPoin = 150;
        if (expandTop < windowHeight - expandPoin) {
            expands[i].classList.add("show");
            window.removeEventListener("scroll", () => { });
        }
        // else {
        //   expands[i].classList.remove("show");
        // }
    }
});
/*-----------------------------------*\
  increment element 
\*-----------------------------------*/
window.addEventListener("scroll", () => {
    let increment = document.querySelectorAll(".increment");
    for (var i = 0; i < increment.length; i++) {
        let windowHeight = window.innerHeight;
        let increTop = increment[i].getBoundingClientRect().top;
        let increPoin = 100;

        if (increTop < windowHeight - increPoin) {
            increment[i].classList.add("zoom");
            window.removeEventListener("scroll", () => { });
        }
        //  else {
        //   increment[i].classList.remove("zoom");
        // }
    }
});
/*-----------------------------------*\
  Zoom IMG 
\*-----------------------------------*/

let zoom = document.querySelector(".imgProduct");
let ImgZoom = document.querySelector("#imgZoom");
zoom.addEventListener("mousemove", (e) => {
    ImgZoom.style.opacity = 1;
    let positionPixel = e.x - zoom.getBoundingClientRect().left;
    let positionX = (positionPixel / zoom.offsetWidth) * 100;
    ImgZoom.style.setProperty("--zoom-x", positionX + "%");

    let positionPixelTop = e.y - zoom.getBoundingClientRect().top;
    let positionY = (positionPixelTop / zoom.offsetHeight) * 100;
    ImgZoom.style.setProperty("--zoom-y", positionY + "%");

    let tranformX = -(positionX - 50) / 5;
    let tranformY = -(positionY - 50) / 5;

    ImgZoom.style.transform = `scale(1.3) translateX(${tranformX}%) translateY(${tranformY}%)`;
});
zoom.addEventListener("mouseout", (e) => {
    ImgZoom.style.opacity = 0;
});