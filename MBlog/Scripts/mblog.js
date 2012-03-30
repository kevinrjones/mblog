$(function() {
    $(":checkbox").click(function(evt) {
        $(this).closest('li').remove().appendTo("#div-complete ul");
        var itemid = $(this).siblings("input[name^='itemid']:hidden").first().val();
        $.post("Item/Complete", { "Id": itemid });
    });
    $("#_deleteLists").click(function(evt) {
        $('.list-item-selected')
            .each(function() {
                $(this);
            })
            .remove();

        resetListHighlighting();

        evt.preventDefault();
    });
    addListHighlighting();
});

function done(data) {
}

function appendTodoList(data) {
    $(".lists")
        .append($("<li>")
                .append($("<a>")
                .attr("href", "/List/show/" + data.id)
                .append(data.name)));
    addListHighlighting();

}

function resetListHighlighting() {
    removeListHighlighting();
    addListHighlighting();
}

function removeListHighlighting() {
    $(".lists li").removeClass('list-odd');
    $(".lists li").removeClass('list-even');
}

function addListHighlighting() {
    $(".lists li:odd").addClass('list-odd');
    $(".lists li:even").addClass('list-even');
}