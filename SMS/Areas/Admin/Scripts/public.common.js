var breakCards = true;

var searchVisible = 0;
var transparent = true;

var transparentDemo = true;
var fixedTop = false;

var mobile_menu_visible = 0,
    mobile_menu_initialized = false,
    toggle_initialized = false,
    bootstrap_nav_initialized = false;

var seq = 0,
    delays = 80,
    durations = 500;
var seq2 = 0,
    delays2 = 80,
    durations2 = 500;
var navbar_menu_visible = 0;
var active_collapse = true;
var disabled_collapse_init = 0;
var sidebar_mini_active = false;
$(document).ready(function () {
    var $sidebar = $('.sidebar');
    var windowWidth = $(window).width();
    var imageSrc = $sidebar.data('image');
    var $sidebarImgContainer = $sidebar.find('.sidebar-background');
    var $fullPage = $('.full-page');
    var $sidebarResponsive = $('body > .navbar-collapse');
    var isWindows = navigator.platform.indexOf('Win') > -1 ? true : false;
    var $sidebarWrapper = $('.sidebar-wrapper');

    if (isWindows) {
        // if we are on windows OS we activate the perfectScrollbar function
        $('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar();
        $('html').addClass('perfect-scrollbar-on');
    } else {
        $('html').addClass('perfect-scrollbar-off');
    }
    if (imageSrc !== undefined) {
        var sidebarContainer = '<div class="sidebar-background" style="background-image: url(' + imageSrc + ') "/>';
        $sidebar.append(sidebarContainer);
    }

    $('#minimizeSidebar').click(function () {
        var $btn = $(this);
        if (sidebar_mini_active === true) {
            $('body').removeClass('sidebar-mini');
            sidebar_mini_active = false;
        } else {
            $('body').addClass('sidebar-mini');
            sidebar_mini_active = true;
        }

        // we simulate the window Resize so the charts will get updated in realtime.
        var simulateWindowResize = setInterval(function () {
            window.dispatchEvent(new Event('resize'));
        }, 180);

        // we stop the simulation of Window Resize after the animations are completed
        setTimeout(function () {
            clearInterval(simulateWindowResize);
        }, 1000);
    });
    $('#success_message').delay(5000).fadeOut(); $('#error_message').delay(5000).fadeOut();
    $('.mdb-select').material_select();

    $(document).on('click', '.navbar-toggler', function () {
        var $toggle = $(this);
        if (mobile_menu_visible === 1) {
            $('html').removeClass('nav-open');
            $('.close-layer').remove();
            setTimeout(function () {
                $toggle.removeClass('toggled');
            }, 400);

            mobile_menu_visible = 0;
        } else {
            setTimeout(function () {
                $toggle.addClass('toggled');
            }, 430);

            var $layer = $('<div class="close-layer"></div>');

            if ($('body').find('.main-panel').length !== 0) {
                $layer.appendTo(".main-panel");

            } else if (($('body').hasClass('off-canvas-sidebar'))) {
                $layer.appendTo(".wrapper-full-page");
            }

            setTimeout(function () {
                $layer.addClass('visible');
            }, 100);

            $layer.click(function () {
                $('html').removeClass('nav-open');
                mobile_menu_visible = 0;

                $layer.removeClass('visible');

                setTimeout(function () {
                    $layer.remove();
                    $toggle.removeClass('toggled');

                }, 400);
            });

            $('html').addClass('nav-open');
            mobile_menu_visible = 1;

        }

    });

    // activate collapse right menu when the windows is resized
    $(window).resize(function () {
        if ($(window).width() <= 991) {
            if ($sidebar.length !== 0) {
                debounce(function () {
                    if (!mobile_menu_initialized) {
                        var $navbar = $('nav').find('.navbar-collapse').children('.navbar-nav');
                        var mobileMenuContent = '';
                        var navContent = $navbar.html();
                        navContent = '<ul class="nav navbar-nav nav-mobile-menu">' + navContent + '</ul>';
                        var navbarForm = $('nav').find('.navbar-form').get(0).outerHTML;
                        var $sidebarNav = $sidebarWrapper.find(' > .nav');

                        // insert the navbar form before the sidebar list
                        var $navContent = $(navContent);
                        var $navbarForm = $(navbarForm);
                        $navContent.insertBefore($sidebarNav);
                        $navbarForm.insertBefore($navContent);

                        $(".sidebar-wrapper .dropdown .dropdown-menu > li > a").click(function (event) {
                            event.stopPropagation();
                        });

                        // simulate resize so all the charts/maps will be redrawn
                        window.dispatchEvent(new Event('resize'));

                        mobile_menu_initialized = true;
                    } else {
                        if ($(window).width() > 991) {
                            // reset all the additions that we made for the sidebar wrapper only if the screen is bigger than 991px
                            $sidebarWrapper.find('.navbar-form').remove();
                            $sidebarWrapper.find('.nav-mobile-menu').remove();

                            mobile_menu_initialized = false;
                        }
                    }
                }, 200);
            }
        }
        seq = seq2 = 0;
    });
});
function debounce(func, wait, immediate) {
    var timeout;
    return function () {
        var context = this,
            args = arguments;
        clearTimeout(timeout);
        timeout = setTimeout(function () {
            timeout = null;
            if (!immediate) func.apply(context, args);
        }, wait);
        if (immediate && !timeout) func.apply(context, args);
    };
};
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