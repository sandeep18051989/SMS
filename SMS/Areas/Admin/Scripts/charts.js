$(document).ready(function () {
    $("#SelectedMonth").change(function () {
        DrawUserRegistrationsChartAfterMonthChange();
    });

    $("#SelectedYear").change(function () {
        DrawUserRegistrationsChartAfterYearChange();
    });

    $("#Feedbacks_SelectedMonth").change(function () {
        DrawFeedbacksChartAfterMonthChange();
    });

    $("#Feedbacks_SelectedYear").change(function () {
        DrawFeedbacksChartAfterYearChange();
    });

    $("#Events_SelectedMonth").change(function () {
        DrawEventsChartAfterMonthChange();
    });

    $("#Events_SelectedYear").change(function () {
        DrawEventsChartAfterYearChange();
    });
});

function DrawUserRegistrationsChartAfterMonthChange() {
    if ($("#SelectedMonth").val() != 0) {
        $.ajax({
            cache: false,
            type: "GET",
            url: "/Admin/Dashboard/DrawRegisteredUsersChart",
            data: { month: $("#SelectedMonth").val(), year: $("#SelectedYear").val() },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                $("#user-registeration-chart").html('');
                $("#user-registeration-chart").html(data);
                $(document).bind('change', '#SelectedMonth', function () { DrawUserRegistrationsChartAfterMonthChange(); });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError);
            },
            traditional: true
        });
    }
}

function DrawUserRegistrationsChartAfterYearChange() {
    if ($("#SelectedYear").val() != 0) {
        $.ajax({
            cache: false,
            type: "GET",
            url: "Admin/Dashboard/DrawRegisteredUsersChart",
            data: { 'month': $("#SelectedMonth").val(), 'year': $("#SelectedYear").val() },
            success: function (data) {
                $("#user-registeration-chart").html('');
                $("#user-registeration-chart").html(data);
                $(document).bind('change', '#SelectedYear', function () { DrawUserRegistrationsChartAfterYearChange(); });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError);
            },
            traditional: true
        });
    }
}

function DrawFeedbacksChartAfterMonthChange() {
    if ($("#Feedbacks_SelectedMonth").val() != 0) {
        $.ajax({
            cache: false,
            type: "GET",
            url: "/Admin/Dashboard/DrawFeedbacksChart",
            data: { month: $("#Feedbacks_SelectedMonth").val(), year: $("#Feedbacks_SelectedYear").val() },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                $("#feedbacks-chart").html('');
                $("#feedbacks-chart").html(data);
                $(document).bind('change', '#Feedbacks_SelectedMonth', function () { DrawFeedbacksChartAfterMonthChange(); });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError);
            },
            traditional: true
        });
    }
}

function DrawFeedbacksChartAfterYearChange() {
    if ($("#Feedbacks_SelectedYear").val() != 0) {
        $.ajax({
            cache: false,
            type: "GET",
            url: "Admin/Dashboard/DrawFeedbacksChart",
            data: { 'month': $("#Feedbacks_SelectedMonth").val(), 'year': $("#Feedbacks_SelectedYear").val() },
            success: function (data) {
                $("#feedbacks-chart").html('');
                $("#feedbacks-chart").html(data);
                $(document).bind('change', '#Feedbacks_SelectedYear', function () { DrawFeedbacksChartAfterYearChange(); });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError);
            },
            traditional: true
        });
    }
}

function DrawEventsChartAfterMonthChange() {
    if ($("#Events_SelectedMonth").val() != 0) {
        $.ajax({
            cache: false,
            type: "GET",
            url: "/Admin/Dashboard/DrawEventsChart",
            data: { month: $("#Events_SelectedMonth").val(), year: $("#Events_SelectedYear").val() },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (data) {
                $("#events-chart").html('');
                $("#events-chart").html(data);
                $(document).bind('change', '#Events_SelectedMonth', function () { DrawEventsChartAfterMonthChange(); });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError);
            },
            traditional: true
        });
    }
}

function DrawEventsChartAfterYearChange() {
    if ($("#Events_SelectedYear").val() != 0) {
        $.ajax({
            cache: false,
            type: "GET",
            url: "Admin/Dashboard/DrawEventsChart",
            data: { 'month': $("#Events_SelectedMonth").val(), 'year': $("#Events_SelectedYear").val() },
            success: function (data) {
                $("#feedbacks-chart").html('');
                $("#feedbacks-chart").html(data);
                $(document).bind('change', '#Events_SelectedYear', function () { DrawEventsChartAfterYearChange(); });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError);
            },
            traditional: true
        });
    }
}