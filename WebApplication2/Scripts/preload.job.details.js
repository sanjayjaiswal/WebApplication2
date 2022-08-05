// Page Load Event
$(document).ready(function () {
    $("#pnlActions").bind('mouseover', function () {
        $('#pnlActionsPopup').bind('mouseout', function () {
            $('#pnlActionsPopup').hide();
        }).show();
    });
});