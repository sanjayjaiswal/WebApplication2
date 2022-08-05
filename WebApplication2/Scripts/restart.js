if (typeof wms === 'undefined')
    wms = new Object();
if (typeof wms.restart === 'undefined')
    wms.restart = new Object();

wms.restart.onKeyPress = function(event)
{
    var control = wms.restart.getSearchInfoControl();
    
    if ((typeof control !== 'undefined') &&
        (event.keyCode == 13 || event.keyCode == 9))
    {
        switch (control.id) {
            case wms.restart.sequenceNumberTextBox.attr('id'):
                wms.restart.addSequenceButton.trigger('click');
                wms.restart.sequenceNumberTextBox.val('');
                wms.restart.sequenceNumberTextBox.focus();
                break;
            case wms.restart.beginSequenceTextBox.attr('id'):
                if (wms.restart.beginSequenceTextBox.val() == '' ||
                    wms.restart.endSequenceTextBox.val() == '')
                {
                    wms.restart.endSequenceTextBox.focus();
                }
                else
                {
                    wms.restart.addSequenceButton.trigger('click');
                    wms.restart.beginSequenceTextBox.val('');
                    wms.restart.endSequenceTextBox.val('');
                    wms.restart.beginSequenceTextBox.focus();
                }
                break;
            case wms.restart.endSequenceTextBox.attr('id'):
                if (wms.restart.beginSequenceTextBox.val() == '' ||
                    wms.restart.endSequenceTextBox.val() == '') {
                    wms.restart.beginSequenceTextBox.focus();
                }
                else {
                    wms.restart.addSequenceButton.trigger('click');
                    wms.restart.beginSequenceTextBox.val('');
                    wms.restart.endSequenceTextBox.val('');
                    wms.restart.beginSequenceTextBox.focus();
                }
                break;
            case wms.restart.accountNumberTextBox.attr('id'):
                wms.restart.addAccountButton.trigger('click');
                wms.restart.accountNumberTextBox.val('');
                wms.restart.accountNumberTextBox.focus();
                break;
            case wms.restart.nameOrAddressTextBox.attr('id'):
                wms.restart.addAccountButton.trigger('click');
                wms.restart.nameOrAddressTextBox.val('');
                wms.restart.nameOrAddressTextBox.focus();
                break;
            default:
                break;
        }
        return false;
    }
}

wms.restart.onKeyUp = function (event)
{
    var control = wms.restart.getSearchInfoControl();
    if (typeof control === 'undefined') {
        wms.restart.batchInfo_onKeyUp();
        return;
    }
    switch (control.id) {
        case wms.restart.sequenceNumberTextBox.attr('id'):
            wms.restart.sequenceNumberTextBox.prop('disabled', false);
            wms.restart.beginSequenceTextBox.prop('disabled', true);
            wms.restart.endSequenceTextBox.prop('disabled', true);
            wms.restart.accountNumberTextBox.prop('disabled', true);
            wms.restart.nameOrAddressTextBox.prop('disabled', true);
            break;
        case wms.restart.beginSequenceTextBox.attr('id'):
            wms.restart.sequenceNumberTextBox.prop('disabled', true);
            wms.restart.beginSequenceTextBox.prop('disabled', false);
            wms.restart.endSequenceTextBox.prop('disabled', false);
            wms.restart.accountNumberTextBox.prop('disabled', true);
            wms.restart.nameOrAddressTextBox.prop('disabled', true);
            break;
        case wms.restart.endSequenceTextBox.attr('id'):
            wms.restart.sequenceNumberTextBox.prop('disabled', true);
            wms.restart.beginSequenceTextBox.prop('disabled', false);
            wms.restart.endSequenceTextBox.prop('disabled', false);
            wms.restart.accountNumberTextBox.prop('disabled', true);
            wms.restart.nameOrAddressTextBox.prop('disabled', true);
            break;
        case wms.restart.accountNumberTextBox.attr('id'):
            wms.restart.sequenceNumberTextBox.prop('disabled', true);
            wms.restart.beginSequenceTextBox.prop('disabled', true);
            wms.restart.endSequenceTextBox.prop('disabled', true);
            wms.restart.accountNumberTextBox.prop('disabled', false);
            wms.restart.nameOrAddressTextBox.prop('disabled', true);
            break;
        case wms.restart.nameOrAddressTextBox.attr('id'):
            wms.restart.sequenceNumberTextBox.prop('disabled', true);
            wms.restart.beginSequenceTextBox.prop('disabled', true);
            wms.restart.endSequenceTextBox.prop('disabled', true);
            wms.restart.accountNumberTextBox.prop('disabled', true);
            wms.restart.nameOrAddressTextBox.prop('disabled', false);
            break;
        default:
            break;
    }
}

wms.restart.batchInfo_onKeyUp = function(event)
{
    if (wms.restart.batchNumberTextBox.val() == '' ||
        wms.restart.productNumberTextBox.val() == '' ||
        wms.restart.groupNameTextBox.val() == '')
    {
        wms.restart.sequenceNumberTextBox.prop('disabled', true);
        wms.restart.beginSequenceTextBox.prop('disabled', true);
        wms.restart.endSequenceTextBox.prop('disabled', true);
        wms.restart.accountNumberTextBox.prop('disabled', true);
        wms.restart.nameOrAddressTextBox.prop('disabled', true);
    }
    else
    {
        wms.restart.sequenceNumberTextBox.prop('disabled', false);
        wms.restart.beginSequenceTextBox.prop('disabled', false);
        wms.restart.endSequenceTextBox.prop('disabled', false);
        wms.restart.accountNumberTextBox.prop('disabled', false);
        wms.restart.nameOrAddressTextBox.prop('disabled', false);
    }
}

wms.restart.getSearchInfoControl = function()
{
    var search = $('.searchInfo').find('input:text').filter(function () {
        return $(this).val() != "";
    });
    if(search.length == 0)
        return;
    return search[0];
}

wms.restart.clearInputButton_onClick = function(event)
{
    wms.restart.productNumberTextBox.val('');
    wms.restart.batchNumberTextBox.val('');
    wms.restart.sequenceNumberTextBox.val('');
    wms.restart.beginSequenceTextBox.val('');
    wms.restart.endSequenceTextBox.val('');
    wms.restart.accountNumberTextBox.val('');
    wms.restart.nameOrAddressTextBox.val('');
    wms.restart.batchInfo_onKeyUp();
}

wms.restart.initializeControls = function()
{
    wms.restart.sequenceNumberTextBox.keydown(wms.restart.onKeyPress);
    wms.restart.beginSequenceTextBox.keydown(wms.restart.onKeyPress);
    wms.restart.endSequenceTextBox.keydown(wms.restart.onKeyPress);
    wms.restart.accountNumberTextBox.keydown(wms.restart.onKeyPress);
    wms.restart.nameOrAddressTextBox.keydown(wms.restart.onKeyPress);
    wms.restart.sequenceNumberTextBox.keyup(wms.restart.onKeyUp);
    wms.restart.beginSequenceTextBox.keyup(wms.restart.onKeyUp);
    wms.restart.endSequenceTextBox.keyup(wms.restart.onKeyUp);
    wms.restart.accountNumberTextBox.keyup(wms.restart.onKeyUp);
    wms.restart.nameOrAddressTextBox.keyup(wms.restart.onKeyUp);

    wms.restart.batchNumberTextBox.keyup(wms.restart.batchInfo_onKeyUp);
    wms.restart.productNumberTextBox.keyup(wms.restart.batchInfo_onKeyUp);
    

    wms.restart.clearInputButton.click(wms.restart.clearInputButton_onClick);

    wms.restart.batchInfo_onKeyUp();
    wms.restart.onKeyUp();
}

wms.restart.confirmSave = function()
{

}

wms.restart.configureContextMenu = function () {
    // Handle Tracking Grid Context Menu
    $('.groupGrid tr').bind("contextmenu", function (e) {
        var elem, evt = e ? e : event;
        if (evt.srcElement) elem = evt.srcElement;
        else if (evt.target) elem = evt.target;

        if (elem.tagName.toLowerCase() == "tr") {
            elem = $(elem).find("td")[0];
            id = $(elem).text();
        }
        if (elem.tagName.toLowerCase() == "td") {
            elem = $(elem).parent().find("td")[0];
            id = $(elem).text();
        }
        if (id == -1)
            return;
        $("#groupsContextMenu").css({ "top": (e.pageY - 15) + "px", "left": Math.min(e.pageX - 5, e.pageX - ((document.body.offsetWidth / 2) - 580)) + "px" })
            .bind('mouseleave', function () {
                $('#groupsContextMenu').hide();
            })
            .show();
        $("#groupsContextMenu .approve").unbind();
        $("#groupsContextMenu .reject").unbind();

        $("#groupsContextMenu .approve").bind('click', function () {
            $('#approveConfirmDialog').show();
            $('#groupsContextMenu').hide();
        });

        $("#groupsContextMenu .reject").bind('click', function () {
            $('#rejectConfirmDialog').show();
            $('#groupsContextMenu').hide();
        });
        e.preventDefault();
    });
}

wms.restart.onReady = function () {
    wms.restart.clearInputButton = $('.clearInputButton');
    wms.restart.groupNameTextBox = $('.groupNameTextBox');
    wms.restart.batchNumberTextBox = $('.batchNumberTextBox');
    wms.restart.productNumberTextBox = $('.productNumberTextBox');
    wms.restart.addSequenceButton = $('.addSequenceNumberButton');
    wms.restart.sequenceNumberTextBox = $('.sequenceNumberTextBox');
    wms.restart.beginSequenceTextBox = $('.beginSequenceTextBox');
    wms.restart.endSequenceTextBox = $('.endSequenceTextBox');
    wms.restart.addAccountButton = $('.addAccountButton');
    wms.restart.accountNumberTextBox = $('.accountNumberTextBox');
    wms.restart.nameOrAddressTextBox = $('.nameOrAddressTextbox');
    wms.restart.groupNameTextBox = $('.groupNameTextBox');
    wms.restart.modalConfirm = $('.modalConfirm');
    wms.restart.initializeControls();
    if(wms.restart.groupNameTextBox.val() == '')
    {
        wms.restart.groupNameTextBox.focus();
    }
    else if(wms.restart.batchNumberTextBox.val() == '')
    {
        wms.restart.batchNumberTextBox.focus();
    }
    else
    {
        wms.restart.sequenceNumberTextBox.focus();
    }
    //wms.restart.configureContextMenu();
}

// Page Load Event
$(document).ready(wms.restart.onReady);