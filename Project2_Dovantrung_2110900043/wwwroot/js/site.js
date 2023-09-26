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
