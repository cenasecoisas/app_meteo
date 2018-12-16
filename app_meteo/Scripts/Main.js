$(document).ready(function () {
    $('#searchForm').submit(function (e) {
        e.preventDefault();
        returnCard($('#searchBox').val(), 1);
    });
    var temp = window.location.href.toString().split(window.location.host)[1];
    if (temp === '/Forecast') {
        $('.forecast').addClass('active');
    } else $('.weather').addClass('active');

});

function returnCard(city, add) {
    $.ajax({
        type: "POST",
        url: "Home/Card",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: city, add: add })
    }).done(function (responseData) {
        var cardContainer = $("#cardContainer");
        var voidDivs = $(cardContainer).children('.void').toArray();
        var cardDivs = $('.card');
        if (cardDivs.length === 0) {
            $(cardContainer).append('<div class="col-4 void">');
            $(cardContainer).append(responseData);
            $(cardContainer).append('<div class="col-4 void">');
        } else if (cardDivs.length === 1) {
            $(voidDivs[0]).removeClass('col-4');
            $(voidDivs[0]).addClass('col-2');
            $(voidDivs[1]).remove();
            $(cardContainer).append(responseData);
            $(cardContainer).append('<div class="col-2 void">');
        } else if (cardDivs.length === 2) {
            $(voidDivs).remove();
            $(cardContainer).append(responseData);
        }
        var button = $(cardContainer).last('.card').find('.remover');
        $(button).click(function () {
            removeCard(this, city);
        });
    }).fail(function (errorThrown) {
        console.log(errorThrown);
    });
}

function removeCard(button, city) {
    $.ajax({
        type: "POST",
        url: "Home/RemoveCity",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: city })
    }).done(function (responseData) {
        var cardContainer = $("#cardContainer");
        var voidDivs = $(cardContainer).children('.void').toArray();
        var cardDivs = $('.card');
        if (cardDivs.length === 3) {
            $(button).closest('.card').remove();
            $(cardContainer).prepend('<div class="col-2 void">');
            $(cardContainer).append('<div class="col-2 void">');
        } else if (cardDivs.length === 2) {
            $(button).closest('.card').remove();
            $(voidDivs[0]).removeClass('col-2');
            $(voidDivs[0]).addClass('col-4');
        }
    }).fail(function (errorThrown) {
        console.log(errorThrown);
    });
}