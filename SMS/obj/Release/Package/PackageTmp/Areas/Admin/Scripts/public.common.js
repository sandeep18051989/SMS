$(document).ready(function () {
    $('#success_message').delay(5000).fadeOut(); $('#error_message').delay(5000).fadeOut();
    $('.mdb-select').material_select();

    // Prevent Special Char
    $('input.no-special-char').on('keypress', function (event) {
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });
});
function checkNumber(e) {
    var key = window.event ? e.keyCode : e.which;
    if (key !== 8) {
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
function warningValidation(validationUrl, warningElementName, passedParameters) {
    addAntiForgeryToken(passedParameters);
    $.ajax({
        cache: false,
        url: validationUrl,
        type: 'post',
        dataType: "json",
        data: passedParameters,
        success: function (data) {
            var element = $('[data-valmsg-for="' + warningElementName + '"]');
            if (data.Result) {
                element.addClass("warning");
                element.html(data.Result);
            }
            else {
                element.removeClass("warning");
                element.html('');
            }
        }
    });
};
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

function blockSpecialChar(e) {
    var k = e.keyCode == 0 ? e.charCode : e.keyCode;
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
}