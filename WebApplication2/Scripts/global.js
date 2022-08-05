$.fn.isBound = function (type, fn) {
    var data = this.data('events')[type];

    if (data === undefined || data.length === 0) {
        return false;
    }

    return (-1 !== $.inArray(fn, data));
};
// Initialize wms if it does not already exist.
if (typeof wms === 'undefined')
    wms = new Object();
if (typeof wms.WebGrid === 'undefined')
    wms.WebGrid = new Object();
if (typeof wms.workflow === 'undefined')
    wms.workflow = new Object();
if (typeof wms.workflow.configuration === 'undefined')
    wms.workflow.configuration = new Object();

wms.ModalButtons = Object.freeze({
    YesNo: 1, OkCancel: 2, Ok: 3
});

wms.workflow.configuration.ModalButtons = Object.freeze({ 
    YesNo: 1, 
    OkCancel: 2, 
    Ok: 3 
});

wms.workflowSteps = Object.freeze({
    FileReceiver: 1,
    Preprocessing: 2,
    HoldPointA: 4,
    PreConversion: 8,
    Conversion: 16,
    PostConversion: 32,
    HoldPointB: 64,
    PreBatch: 128,
    Batch: 256,
    PostBatch: 512,
    Complete: 1024,
    All: 65535
});

wms.progressContainer = $('#content>.progressContainer');
wms.progressContainer.hide();

wms.appRoot = function () {
    //return document.getElementById('ApplicationRoot').href;
    return location.protocol + "//" + location.host + "/wms/";
}

wms.configureModalDialog = function () {
    var modalBackground = $('#modalBackground');
    var dialog = $('#modal');
    if (wms.workflow.configuration.isUserControl) {
        dialog.css('left', "250px");
        dialog.css('top', "150px");
    }
    modalBackground.hide();
    dialog.hide();
    $('.holdA>input[type=checkbox], .holdB>input[type=checkbox]').on('change', function () {
        if ($(this).is(':checked')) {
            wms.workflow.configuration.launchModalDialog(this, "Hold point Reason", "Enter a reason for enabling this hold point.", wms.workflow.configuration.enableHoldPoint, wms.workflow.configuration.cancelHoldPoint);
        }
    });
}

wms.launchModalDialog = function (sender, titleText, messageText, onConfirm, onCancel, buttons, hideAnswer, isModal) {
    var modalBackground = $('#modalBackground');
    var modal = $('#modal');
    var title = modal.find('#title');
    var message = modal.find('#prompt');
    var confirm = modal.find('#confirm');
    var cancel = modal.find('#cancel');
    var answer = $('#modal').find('#answer');
    if (typeof isModal === 'undefined')
        isModal = false;

    switch (buttons) {
        case wms.workflow.configuration.ModalButtons.OkCancel:
            confirm.val("Ok");
            cancel.val("Cancel");
            break;
        case wms.workflow.configuration.ModalButtons.YesNo:
            confirm.val("Yes");
            cancel.val("No");
            break;
        default:
            confirm.val("Ok");
            cancel.val("Cancel");
            break;

    }
    if (hideAnswer == true)
        answer.hide();
    else
        answer.show();
    if ($(sender).attr('reason') != null) {
        answer.val($(sender).attr('reason'));
    }
    else {
        answer.val("");
    }
    modalBackground.css('top', $(document).scrollTop());
    modalBackground.css('left', 0);
    modalBackground.css('right', 0);
    var top = ((window.innerHeight / 2) - (modal.height()/2)) + $(document).scrollTop();
    var left = ((window.innerWidth / 2) - (modal.width()/2)) + $(document).scrollLeft();
    var css = '{top:' + top + ', left:' + left + '}';
    var styles = modal.attr('style').split(';');
    var newStyles = "";
    if (modal.attr('style').indexOf('left') >= 0) {
        for (var i = 0; i < styles.length; i++) {
            if (styles[i] == '')
                continue;
            else if (styles[i].split(':')[0].trim() == 'left')
                newStyles += 'left:' + left + 'px;';
            else if (styles[i].split(':')[0].trim() == 'top')
                newStyles += 'top:' + top + 'px;';
            else
                newStyles += styles[i] + ';';
        }
    }
    else
    {
        newStyles = styles + ';left:' + left + 'px;' + 'top:' + top + 'px;';
    }
    modal.attr('style', newStyles);

    $('body').css({'overflow': 'hidden','height': '100%'});
    
    title.text(titleText);
    message.text(messageText);
    cancel.unbind('click').click({ param1: sender }, onCancel)
        .click(function () {
            modal.hide();
            modalBackground.hide();
            $('body').css({ 'overflow': 'auto', 'height': 'auto' });
        });
    confirm.unbind('click').click(function()
    {
        var e = new Event("click");
        e.data = new Object();
        e.data.param1 = sender;
        e.data.param2 = $('#modal').find('#answer').val();
        onConfirm(e);
    }).click(function () {
        modal.hide();
        modalBackground.hide();
        $('body').css({ 'overflow': 'auto', 'height': 'auto' });
    });
    if (isModal)
        modalBackground.show();
    modal.show();
}

wms.onMessageConfirm = function () {
    var lightbox = $('.messagesContainer');
    var messageHiddenField = $('#MessagesControl');
    var inputData = messageHiddenField.val(); //.replace(/\/Date/g, "\\\/Date").replace(/\)\//g, "\)\\\/");
    var webMethodUrl = wms.appRoot() + "app/MessageService.asmx/Audit";
    var onDone = function () {
        lightbox.hide();
    };
    var onFail = function (jqXHR, textStatus, errorThrown) {
        var errorResponse = jqXHR.responseText;
        console.warn(webMethodUrl + " failed with response: " + errorResponse);
    };

    wms.handleCallback(webMethodUrl,
        "{'messages':" + inputData + ",'url':'" + window.location.pathname + "'}",
        onDone,
        onFail,
        null,
        true);
}
wms.initializeMessages = function () {
    var lightbox = $('.messagesContainer');
    var messages = lightbox.find('.dataGridBody');
    var confirm = lightbox.find('#confirm');
    var scopeHiddenField = $('#MessageScopeControl');
    var messageHiddenField = $('#MessagesControl');
    var webMethodUrl = "./app/messageservice.asmx/Get";
    var currentMessages = new Array();
    if (typeof messageHiddenField.val() !== 'undefined' &&
        messageHiddenField.val() != '') {
        currentMessages = JSON.parse(messageHiddenField.val());
    } 

    if (currentMessages == null || currentMessages.length == 0) {
        lightbox.hide();
        return;
    }
    messages.empty();
    var showLightbox = false;
    for (index in currentMessages) {
        var thisMessage = currentMessages[index];
        console.info(thisMessage);
        messages.append("<tr><td class=\"message\">" + thisMessage.Text + "</td></tr>");
        if (thisMessage.DisplayMessage)
            showLightbox = true;
    }
    if (!showLightbox) {
        lightbox.hide();
    } else {
        confirm.unbind('click');
        confirm.click(wms.onMessageConfirm);
        lightbox.show();
    }
}


wms.updateMessages = function (currentMessages) {
    var lightbox = $('.lightbox');
    var messages = lightbox.find('.dataGridBody');
    var messageHiddenField = $('#MessagesControl');
    messageHiddenField.val(JSON.stringify(currentMessages));
    wms.initializeMessages();
}

wms.showMessages = function (result) {
    var lightbox = $('.messagesContainer');
    var showLightbox = false;
    for (var index in result) {
        var thisMessage = result[index];
        console.info(thisMessage);
        messages.append("<tr><td class=\"message\">" + thisMessage.Text + "</td></tr>");
        if (thisMessage.DisplayMessage)
            showLightbox = true;
    }
    if (!showLightbox)
        lightbox.hide();
    else
        lightbox.show();
}

wms.loadMessages = function () {
    var lightbox = $('.messagesContainer');
    var messages = lightbox.find('.dataGridBody');
    var confirm = lightbox.find('#confirm');
    var scopeHiddenField = $('#MessageScopeControl');
    var messageHiddenField = $('#MessagesControl');
    var webMethodUrl = "./app/messageservice.asmx/Get";

    var onDone = function (data, textStatus, jqXHR) {
        var result = JSON.parse(data.d);
        wms.showMessages(result);
    };

    var onFail = function (jqXHR, textStatus, errorThrown) {
        var errorResponse = jqXHR.responseText;
        console.warn(webMethodUrl + " failed with response: " + errorResponse);
    };

    wms.handleCallback(webMethodUrl,
        inputData,
        onDone,
        onFail);

}

wms.handleCallback = function (url, data, onDone, onFail, onAlways, isString) {
    wms.progressContainer.show();
    isString = typeof isString !== 'undefined' ? isString : true;
    if (isString) {
        var jqxhr = $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: url,
            data: data
        })
           .done(onDone)
           //.fail(wms.handleCallback.handleFail(xhr, error, onFail))
           .always(function () {
               if (onAlways != null) {
                   onAlways();
               }
               wms.progressContainer.hide();
           });
    }
    else {
        var jqxhr = $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: url,
            data: JSON.stringify(data)
        })
           .done(onDone)
           //.fail(wms.handleCallback.handleFail(xhr, error, onFail))
           .always(function () {
               if (onAlways != null) {
                   onAlways();
               }
               wms.progressContainer.hide();
           });
    }
}

wms.handleCallback.handleFail = function(jqxhr, exception, onFail)
{
    onFail();

}

wms.handleAutoComplete = function (control, url) {
    control.autocomplete({
        source: function (request, response) {
            console.info(request.term);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: url,
                dataType: "json",
                data: "{'filter':'" + request.term + "'}"
            })
            .done(function (data) {
                response($.map(data.d, function (item) {
                    var displayLabel = new Object();
                    if (item.Id != null)
                        displayLabel.Id = item.Id;
                    else
                        displayLabel.Id = item;
                    if (item.Name != null)
                        displayLabel.Name = item.Name;
                    else
                        displayLabel.Name = item;
                    return {
                        label: displayLabel.Name,
                        id: displayLabel.Id,
                        abbrev: displayLabel.Name
                    };
                }));
            })
           .fail(function (jqXHR, textStatus) {
               console.info("Failed Status: " + textStatus + "\rText: " + jqXHR.responseText);
           });
        },
        minLength: 2,
        change: function (event, ui) {
            if (typeof ui === 'undefined')
                ui = new Object();
            if (ui.item == null ||
                typeof ui.item === 'undefined') {
                ui.item = new Object();
                ui.item.id = control.val();
            }
            $('.ui-menu-item>a').each(function (index, value) {
                if ($(value).html() == ui.item.id) {
                    var onSelect = $(control).data('on-select');
                    if (onSelect != null)
                        onSelect(event, ui);
                }
            });
        },
        select: function (event, ui) {
            control.val(ui.item.label);
            var onSelect = $(control).data('on-select');
            if (onSelect != null)
                onSelect(event, ui);
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });
}

wms.initializeAutoComplete = function () {
    $(".autocomplete").each(function (index) {
        wms.handleAutoComplete($(this), $(this).data('url'), $(this).data('on-select'));
    });
}

wms.padString = function (n, width, z) {
    z = z || '0';
    n = n + '';
    return n.length >= width ? n : new Array(width - n.length + 1).join(z) + n;
}

wms.onReady = function () {
    wms.configureModalDialog();
    wms.initializeMessages();
    wms.initializeAutoComplete();
    wms.configureModalDialog();
    if (!$('.datepicker.initialized').hasClass('hasDatepicker') && $('.datepicker.initialized').val() == '') {
        $('.datepicker.initialized').datepicker({});
        $('.timepicker').timepicker({ 'timeFormat': 'g:i A' });
        $('.datepicker.initialized').datepicker("setDate", new Date());
    }
    $('.datepicker').datepicker({});
    $('.timepicker').timepicker({ 'timeFormat': 'g:i A' });
    $('.timepicker.initialized').timepicker("setTime", new Date());

    var startTime = new Date();
    startTime.setHours(0, 0, 0, 0);
    $('.timepicker.initialized.start').timepicker("setTime", startTime);
    var endTime = new Date();
    endTime.setHours(24, 0, 0, -1);
    $('.timepicker.initialized.end').timepicker("setTime", endTime);
    $('.numeric').keypress(function (event) {
        var charCode = (typeof event.which == "undefined") ? event.keyCode : event.which;
        var charStr = String.fromCharCode(charCode);
        if (/\D/.test(charStr)) {
            event.preventDefault();
            return false;
        }
        return true;
    });
    $('.regex').keypress(function (event) {
        var regexExpression = $(event.target).data('regex-expression');
        var charCode = (typeof event.which == "undefined") ? event.keyCode : event.which;
        var charStr = String.fromCharCode(charCode);
        var newInput = $(event.target).val() + charStr;
        var regex = new RegExp("^" + regexExpression + "$");
        var match = regex.test(newInput);
        if (match) {
            return true;
        }
        event.preventDefault();
        return false;
    });
    wms.progressContainer = $('.progressContainer');
    wms.progressContainer.hide();
}

// Page Load Event
$(document).ready(wms.onReady);