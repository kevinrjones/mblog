jQuery.namespace = function () {
    var a = arguments, o = null, i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = a[i].split(".");
        o = window;
        for (j = 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
};


$(function () {
    $(".listmedia-details").hover(function (evt) {

        var node = $(this).children("div");
        //$(node).css({ visibilty: "visible" });
        $("node").removeClass('hidden-row-actions');
        $("node").removeClass('visble-row-actions');

        evt.preventDefault();
    });
    addListHighlighting();
});

function addListHighlighting() {
    $(".lists li:odd").addClass('list-odd');
    $(".lists li:even").addClass('list-even');
}

$.namespace('Dashboard.Media');

Dashboard.Media.showRow = function () {
    alert("hover");
};