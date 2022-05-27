﻿$(function () {
    $('[data-toggle="popover"]').popover();
});

$(function () {
    $('.popover-dismiss').popover({
        trigger: 'focus'
    });
});

$('.addToCart').click(function () {
    var butWrap = $(this).parents('.but-wrap');
    butWrap.append('<div class="animtocart"></div>');
});

jQueryAjaxAddProduct = (url) => {
    $.ajax({
        type: 'GET',
        url: url,

        success: function (e) {
            $('.animtocart').css({
                'position': 'absolute',
                'background': '#FF0000',
                'width': '25px',
                'height': '25px',
                'border-radius': '100px',
                'z-index': '9999999999',
                'left': e.pageX - 25,
                'top': e.pageY - 25
            });

            var cart = $('#shoppingCart').offset();
            $('.animtocart').animate({ top: cart.top + 'px', left: cart.left + 'px', width: 0, height: 0 }, 800, function () {
                $(this).remove();
            });
        }
    });
}