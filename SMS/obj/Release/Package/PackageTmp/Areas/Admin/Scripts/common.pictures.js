$(document).ready(function () {
    $("#btnShowPictures").click(function (e) {
	    GetPictures();
	});
});

function GetPictures() {
    $.ajax({
        type: "GET",
        url: "/Admin/Slider/GetSliderPicturesList",
        data: {},
        async: true,
        context: this,
        success: function (data) {
            $("#updateDiv").html(data);
            $("#pic-list").show();
        },
        error: function (xhr, errorType, exception) {
            responseText = $.parseJSON(xhr.responseText);
            $(".validation-summary-errors").empty();
            $(".validation-summary-errors").append("<li>" + responseText.Message + "</li>");
        }
    });
}

function UpdatePicture(pictureId, altText, dispOrder, act) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "../../Admin/Slider/UpdatePicture",
        data: { 'pictureId': pictureId, 'alternateText': altText, 'displayOrder': dispOrder, 'active': act },
        success: function (result) {
            if (result.Success == "True") {
                GetPictures();
            }
            else {
                $(".validation-summary-errors").empty();
                $(".validation-summary-errors").append("<li>" + result.Message  + "</li>");
            }
        },
        error: function (xhr, errorType, exception) {
            responseText = jQuery.parseJSON(xhr.responseText);
            $(".validation-summary-errors").empty();
            $(".validation-summary-errors").append("<li>" + responseText.Message + "</li>");
        }
    });
}

function DeletePicture(pictureId) {
    $.ajax({
        type: "POST",
        url: "../../Admin/Slider/DeletePicture",
        data: { 'pictureId': pictureId },
        success: function (result) {
            if (result.Success == "True") {
                GetPictures();
            }
            else {
                $(".validation-summary-errors").empty();
                $(".validation-summary-errors").append("<li>" + result.Message + "</li>");
            }
        },
        error: function (xhr, errorType, exception) {
            responseText = jQuery.parseJSON(xhr.responseText);
            $(".validation-summary-errors").empty();
            $(".validation-summary-errors").append("<li>" + responseText.Message + "</li>");
        }
    });
}