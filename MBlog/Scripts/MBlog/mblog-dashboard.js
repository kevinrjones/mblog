$(function() {
    $(".listmedia-details").mouseover(function(evt) {
        var node = $(this).children("div.row-actions");
        $(node).css({ visibility: 'visible' });

        evt.preventDefault();
    });
    $(".listmedia-details").mouseout(function(evt) {
        var node = $(this).children("div.row-actions");
        $(node).css({ visibility: 'hidden' });

        evt.preventDefault();
    });
});

