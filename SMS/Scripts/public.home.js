$(document).ready(function () {
    $(".carousel-indicators li:first-child").addClass("active");
    $(".carousel-inner div:first-child").addClass("active");

    // Bootstrap collapse
    $('[data-toggle="collapse"]').on({
        'click': function () {
            var $this = $(this),
                target = $this.data('target') || null;

            if ($(target).size() > 0) {
                $this.toggleClass('target-open', !$(target).hasClass('in'));
            }
        },
    });

    // Prevent Special Char
    $('input.no-special-char').on('keypress', function (event) {
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });

    $(".hover-content").hover(function () {
        $($(this).find('.btn-view-all')[0]).show();
    }, function () {
        $($(this).find('.btn-view-all')[0]).hide();
    });

    setTimeout(function () {
        $(".notify").alert('close');
    }, 10000);

    // Tooltip & popovers
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover();

    // Background image via data tag
    $('[data-block-bg-img]').each(function () {
        // @todo - invoke backstretch plugin if multiple images
        var $this = $(this),
            bgImg = $this.data('block-bg-img');

        $this.css('backgroundImage', 'url(' + bgImg + ')').addClass('block-bg-img');
    });

    //Scroll Top link
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.scrolltop').fadeIn();
        } else {
            $('.scrolltop').fadeOut();
        }
    });

    $('.scrolltop').click(function () {
        $("html, body").animate({
            scrollTop: 0
        }, 600);
        return false;
    });

    $(document).on("change", ".reg-email", function () {
        $.ajax({
            cache: false,
            type: "GET",
            url: "/Account/CheckEmailExists",
            data: { email: $(this).val() },
            dataType: "JSON",
            success: function (data) {
                if (data) {
                    $('.reg-email').val('');
                    alert("Username not available, Please try another.");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError);
            },
            traditional: true
        });
    });
});

function checkNumber(e) {
    var key = window.event ? e.keyCode : e.which;
    if (key != 8) {
        var keychar = String.fromCharCode(key);
        reg = /\d/;
        var result = reg.test(keychar);
        if (!result) {
            return false;
        }
        else {
            return true;
        }
    }
}

function isValidEmailAddress(emailAddress) {
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(emailAddress)) {
        return false;
    } else {
        return true;
    }
}

(function (i, s, o, g, r, a, m) {
    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
        (i[r].q = i[r].q || []).push(arguments)
    }, i[r].l = 1 * new Date(); a = s.createElement(o),
        m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
})(window, document, 'script', 'https://www.google-analytics.com/analytics.js', 'ga');

ga('create', 'UA-96031423-1', 'auto', {
    userId: 0
});
ga('send', 'pageview');

