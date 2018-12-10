$(document).ready(function () {
	
});

//function GetPictures() {
//    $.ajax({
//        type: "GET",
//        url: "../../Admin/Slider/GetPicturesList",
//        data: {},
//        success: function (data) {
//            console.log(data);
//            $("#updateDiv").html(data);
//        },
//        error: function (xhr, errorType, exception) {
//            console.log(xhr);
//            responseText = $.parseJSON(xhr.responseText);
//            $(".validation-summary-errors").empty();
//            $(".validation-summary-errors").append("<li>" + responseText.Message + "</li>");
//        }
//    });
//}

//function UpdatePicture(pictureId, altText, dispOrder, act) {
//    $.ajax({
//        type: "POST",
//        dataType: "json",
//        url: "../../Admin/Slider/UpdatePicture",
//        data: { 'pictureId': pictureId, 'alternateText': altText, 'displayOrder': dispOrder, 'active': act },
//        success: function (result) {
//            if (result.Success == "True") {
//                GetPictures();
//            }
//            else {
//                $(".validation-summary-errors").empty();
//                $(".validation-summary-errors").append("<li>" + result.Message  + "</li>");
//            }
//        },
//        error: function (xhr, errorType, exception) {
//            responseText = jQuery.parseJSON(xhr.responseText);
//            $(".validation-summary-errors").empty();
//            $(".validation-summary-errors").append("<li>" + responseText.Message + "</li>");
//        }
//    });
//}

//function DeletePicture(pictureId) {
//    $.ajax({
//        type: "POST",
//        url: "../../Admin/Slider/DeletePicture",
//        data: { 'pictureId': pictureId },
//        success: function (result) {
//            if (result.Success == "True") {
//                GetPictures();
//            }
//            else {
//                $(".validation-summary-errors").empty();
//                $(".validation-summary-errors").append("<li>" + result.Message + "</li>");
//            }
//        },
//        error: function (xhr, errorType, exception) {
//            responseText = jQuery.parseJSON(xhr.responseText);
//            $(".validation-summary-errors").empty();
//            $(".validation-summary-errors").append("<li>" + responseText.Message + "</li>");
//        }
//    });
//}