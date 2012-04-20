$(function () {
    $("#overlay").hide();
    $("#add-media").click(function() {
        $('#overlay').show();
        $('#upload-wrapper').show();
    });
    $("#mediaclose").click(function (evt) {
        $('.media-details').hide();
        $('#upload-error-text').css('visibility', 'hidden').html("");
        $('#overlay').hide();
        $('#upload-wrapper').hide();
    });
});
