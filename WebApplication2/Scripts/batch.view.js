if (typeof wms === 'undefined')
    wms = new Object();
if(typeof wms.batchhistory === 'undefined')
    wms.batchhistory = new Object();

var ModalButtons = { YesNo: 1, OkCancel: 2 };

function handleContextMenu(event) {
    if (event.button == 2) {
        var point = new Object();
        point.x = event.pageX - 200;
        point.y = event.pageY;
        console.info(JSON.stringify(point));
        $("#contextMenu").css({ "top": point.y + "px", "left": point.x + "px" }).show();
    }
    event.preventDefault();
}


function configureContextMenu() {
    $('#batchWebGrid tr').bind("contextmenu", function (e) {
        $("#contextMenu").css({ "top": (e.pageY - 15) + "px", "left": Math.min(e.pageX - 5, e.pageX - ((document.body.offsetWidth / 2) - 580)) + "px" })
            .bind('mouseleave', function () {
                $('#contextMenu').hide();
            })
            .show();
        $("#contextMenu .viewJobTicket").unbind();
        $("#contextMenu .viewPickTicketSummary").unbind();
        $("#contextMenu .viewPickTicketDetail").unbind();
        $("#contextMenu .viewZipSortReport").unbind();

        $("#contextMenu .viewJobTicket").bind('click', function () {
            $('#contextMenu').hide();
        });
        $("#contextMenu .viewPickTicketSummary").bind('click', function () {
            $('#contextMenu').hide();
        });
        $("#contextMenu .viewPickTicketDetail").bind('click', function () {
            $('#contextMenu').hide();
        });
        $("#contextMenu .viewZipSortReport").bind('click', function () {
            $('#contextMenu').hide();
        });
        e.preventDefault();
    });
}

wms.batchhistory.setRadioButton = function () {
    if (!$('input[name="searchType"]:checked').val())
    {
        $('.batchSearchType').prop('checked', true);
        $('.batchSearchType').click();
    }
}

wms.batchhistory.onReady = function() {
    configureContextMenu();
    wms.batchhistory.setRadioButton();
}

// Page Load Event
$(document).ready(wms.batchhistory.onReady);