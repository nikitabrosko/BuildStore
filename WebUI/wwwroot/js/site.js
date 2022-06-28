jQueryAjaxGet = (url, id) => {
    $.ajax({
        type: 'GET',
        url: url,

        success: function (res) {
            $(id).html(res);
        },

        error: function (err) {
            console.log(err);
        },

        failure: function (fail) {
            console.log(fail);
        }
    });
}

jQueryAjaxGetAsyncOff = (url, id) => {
    $.ajax({
        type: 'GET',
        url: url,
        async: false,

        success: function (res) {
            $(id).html(res);
        },

        error: function (err) {
            console.log(err);
        },

        failure: function (fail) {
            console.log(fail);
        }
    });
}

async function jQueryAjaxGetForShoppingCart(url, id) {
    await $.ajax({
        type: 'GET',
        url: url,

        success: function (res) {
            if (res == undefined) {
                $(id).empty();
            }
            else {
                $(id).html(res);
            }
        },

        error: function (err) {
            console.log(err);
        },

        failure: function (fail) {
            console.log(fail);
        }
    });
}

jQueryAjaxPost = (url, elementId) => {
    $.ajax({
        type: 'POST',
        url: url,

        success: function (res) {
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

jQueryAjaxDelete = (url, elementId) => {
    $.ajax({
        type: 'POST',
        url: url,

        success: function (res) {
            if (res == undefined) {
                $('#' + elementId).empty();
            }
            else {
                $('#' + elementId).html(res);
            }
        },

        error: function (err) {
            console.log(err);
        },

        failure: function (fail) {
            console.log(fail);
        }
    });
}

jQueryAjaxPostForm = (form, elementId) => {
    $.ajax({
        type: 'POST',
        url: form.action,
        data: new FormData(form),
        processData: false,
        contentType: false,

        success: function (res) {
            $(elementId).html(res);
        },

        error: function (err) {
            console.log(err);
        },

        failure: function (fail) {
            console.log(fail);
        }
    })
}

jQueryAjaxPostFormAsyncOff = (form, elementId) => {
    $.ajax({
        type: 'POST',
        url: form.action,
        data: new FormData(form),
        processData: false,
        contentType: false,
        async: false,

        success: function (res) {
            $(elementId).html(res);
        },

        error: function (err) {
            console.log(err);
        },

        failure: function (fail) {
            console.log(fail);
        }
    })
}

async function jQueryAjaxForShoppingCart(urlFirst, elementIdFirst, urlSecond, elementIdSecond) {
    await jQueryAjaxGetForShoppingCart(urlFirst, elementIdFirst);
    await jQueryAjaxGetForShoppingCart(urlSecond, elementIdSecond);

    var sum = 0;
    var subsum = 0

    $('td.product-subtotal').each(function () {
        var str = $(this).html().trim();
        sum += parseInt(str.substr(4));
        subsum += parseInt(str.substr(str.length - 2));
    })

    var subsumstr = subsum.length == 2 ? subsum.length : '0' + subsum;

    $('#shopping-cart-total-sum').html('ИТОГО: BYN ' + sum + ',' + subsumstr);
}

$(function () {
    $('[data-toggle="popover"]').popover();
});

$(function () {
    $('.popover-dismiss').popover({
        trigger: 'focus'
    });
});