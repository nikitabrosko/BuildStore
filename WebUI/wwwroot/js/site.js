$.validator.addMethod(
    "regex",
    function (value, element, regexp) {
        var re = new RegExp(regexp);
        return this.optional(element) || re.test(value);
    },
    "Check your input!"
);

$(function () {
    $('[data-toggle="popover"]').popover();
});

$(function () {
    $('.popover-dismiss').popover({
        trigger: 'focus'
    });
});

$('.addToCart').click(function () {
    var butWrap = $(this).parents('#but-wrap');
    butWrap.append('<div class="animtocart"></div>');
});

jQueryAjaxAddProductAndRefreshProductsCount = (addProductUrl, enableAnimation, addProductId,
    productsCountUrl, productsCountId) => {
    jQueryAjaxAddProduct(addProductUrl, enableAnimation, addProductId);
    setTimeout(jQueryAjaxGet, 800, productsCountUrl, productsCountId);
    }

jQueryAjaxGet = (url, id) => {
    $.ajax({
        type: 'GET',
        url: url,

        success: function (res) {
            console.log(res);
            $(id).html(res);
        }
    });
}

jQueryAjaxAddProduct = (url, enableAnimation, id) => {
    $(id).attr("disabled");

    try {
        $.ajax({
            type: 'GET',
            url: url,

            success: function (e) {
                if (enableAnimation === true) {
                    $('.animtocart').css({
                        'position': 'absolute',
                        'background': '#007bff',
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

                $(id).removeAttr("disabled");
            },

            failure: function (fail) {
                $(id).removeAttr("disabled");
            },

            error: function (err) {
                $(id).removeAttr("disabled");
            }
        });
    } catch (e) {
        $(id).removeAttr("disabled");
    } 
    
}

jQueryAjaxPost = (url, elementId) => {
    $.ajax({
        type: 'POST',
        url: url,

        success: function (res) {
            console.log(res);
            $(elementId).html(res);
        },

        error: function (err) {
            console.log(err);
        },

        failure: function(fail) {
            console.log(fail);
        }
    });
}

jQueryAjaxPostForm = (url, form) => {
    try {
        console.log(form);
        $.ajax({
            type: 'POST',
            url: url,
            data: new FormData(form),
            processData: false,

            success: function (res) {
                console.log(res);
                alert(res);
                $("#categoryGet").html(res.html);
            },

            error: function (err) {
                console.log(err);
                alert(err);
            },

            failure: function (fail) {
                console.log(fail);
                alert(fail);
            }
        });
    } catch (e) {
        console.log(e);
        alert(e);
    } 
}

$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});