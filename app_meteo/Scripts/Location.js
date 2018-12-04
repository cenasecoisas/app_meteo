$(document).ready(function () {
    
});

function returnCard(city) {  
    $.ajax({
        type: "POST",
        url: "Home/Card",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: city })
        }).done(function (responseData, textStatus, jqXHR) {
            console.log(responseData);
            $(responseData).appendTo($("cardContainer"));
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        });
}

function returnCardForm(){
    returnCard($('#searchBox').val());
}