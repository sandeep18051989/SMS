$(document).ready(function () {
    $('#success_message').delay(5000).fadeOut(); $('#error_message').delay(5000).fadeOut();
    $('.mdb-select').material_select();
});
function checkNumber(e) {
    var key = window.event ? e.keyCode : e.which;
    if (key != 8) {
        var keychar = String.fromCharCode(key);
        //var msg = $(this).next($('.msg'));
        reg = /\d/;
        var result = reg.test(keychar);
        if (!result) {
            //msg.innerHTML = "number only";
            return false;
        }
        else {
            //msg.innerHTML = "";
            return true;
        }
    }
}

// CSRF (XSRF) security
function addAntiForgeryToken(data) {
    //if the object is undefined, create a new one.
    if (!data) {
        data = {};
    }
    //add token
    var tokenInput = $('input[name=__RequestVerificationToken]');
    if (tokenInput.length) {
        data.__RequestVerificationToken = tokenInput.val();
    }
    return data;
};