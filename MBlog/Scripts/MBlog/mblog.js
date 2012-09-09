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
    $('.listmedia-details .row-actions .delete a').bind('click', function () {
        if (!confirm('Are you sure you want to delete this item?'))
            return false;
    });

    $("a.deletepost").click(function() {
        var self = this;
        if (confirm('Are you sure you want to delete this item?')) {
            $.post(this.href)
                .done(function () {
                    $(self).parent().parent().remove();
                    return false;
                })
                .fail(function () {
                    alert("unable to delete post");
                    return false;
                });
        }
        return false;
    });
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