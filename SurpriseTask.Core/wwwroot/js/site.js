$(document).ready(function() {
    toastr.options.closeButton = true;
    var success = window.success;
    var error = window.error;
    if (success) {
        toastr.success(success, "success", { timeOut: 3000 });
    }
    if (error) {
        toastr.error(error, "error", { timeOut: 3000 });
    }
});


let openeye3 = document.querySelector(".openeye3");
let closeeye3 = document.querySelector(".closeeye3");
let exampleInputPassword4 = document.querySelector("#exampleInputPassword4");
let count3 = false;


function togglePassword(eyeOpen, eyeClose, input, countState, countName) {
    countState = !countState;    
    if (countState) {
        eyeOpen.classList.add("active");
        eyeOpen.classList.remove("inactive");
        eyeClose.classList.remove("active");
        eyeClose.classList.add("inactive");
        input.type = "text";
    } else {
        eyeOpen.classList.add("inactive");
        eyeOpen.classList.remove("active");
        eyeClose.classList.remove("inactive");
        eyeClose.classList.add("active");
        input.type = "password";
    }
    
    return countState;
}


function eyeActive3() {
    count3 = togglePassword(openeye3, closeeye3, exampleInputPassword4, count3, "count3");
}


