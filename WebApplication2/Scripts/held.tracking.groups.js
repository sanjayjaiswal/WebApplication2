if (typeof wms === 'undefined')
    wms = new Object();
if (typeof wms.heldtracking === 'undefined')
    wms.heldtracking = new Object();
if (typeof wms.heldtracking.fileReleaseWizard === 'undefined')
    wms.heldtracking.fileReleaseWizard = new Object();

var turnTimeGrid = $('.TurnTimeWebGrid');
var heldTrackingGrid = $('.heldTrackingWebGrid');
var ModalButtons = { YesNo: 1, OkCancel: 2 };
var PullTypeEnum = { Straight: 0, Reverse: 1, Restart: 2, Reprint: 3, Samples: 4, Both: 5, Unknown: 99 };
var PullType = PullTypeEnum.Both;
var PullByEnum = { Unknown: -1, AccountNumber: 0, FlexField1: 1, FlexField2: 2, FlexField3: 3, ProductNumber: 4, OptionNumber: 5, None: -99 };
var PullBy = PullByEnum.None;
var SampleModeEnum = { FirstMiddleLast: 0, First: 1, Middle: 2, Last: 3,Random: 4 };
var SampleMode = SampleModeEnum.Unknown;
var AccountNumbers;
var id = -1;

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

function launchModalDialog(sender, titleText, messageText, onConfirm, onCancel, buttons, hideAnswer) {
    var modal = $('#modal');
    var title = modal.find('#title');
    var message = modal.find('#prompt');
    var confirm = modal.find('#confirm');
    var cancel = modal.find('#cancel');
    var answer = $('#modal').find('#answer');
    switch (buttons) {
        case ModalButtons.OkCancel:
            confirm.val("Ok");
            cancel.val("Cancel");
            break;
        case ModalButtons.YesNo:
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
    title.text(titleText);
    message.text(messageText);
    cancel.unbind('click').click({ param1: sender }, onCancel).click(function () { modal.hide(); });
    confirm.unbind('click').click({ param1: sender }, onConfirm).click(function () { modal.hide(); });
    modal.show();

}

function configureContextMenu() {
    $('.heldTrackingWebGrid tr').bind("contextmenu", function (e) {
        var isStepHoldPointB;
        var elem, evt = e ? e : event;
        if (evt.srcElement) elem = evt.srcElement;
        else if (evt.target) elem = evt.target;

        if (elem.tagName.toLowerCase() == "tr") {
            elem = $(elem).find("td")[0];
            id = $(elem).text();
        }
        if (elem.tagName.toLowerCase() == "td") {
            isStepHoldPointB = $(elem).parent().find("td")[1].innerHTML.toLowerCase().indexOf('hpb') != -1;
            elem = $(elem).parent().find("td")[0];
            id = $(elem).text();
        }
        if (id == -1)
            return;

        $("#contextMenu")
            .css({ "top": (e.pageY - 15) + "px", "left": Math.min(e.pageX - 5, e.pageX - ((document.body.offsetWidth / 2) - 580)) + "px" })
            .bind('mouseleave', function () {
                $('#contextMenu').hide();
            })
            .bind('mouseenter', function () {
                $('#sampleOptions').hide();
                $('#chooseSamples').hide();
                $("#pullType").prop('selectedIndex', 0);
                $("#pullBy").prop('selectedIndex', 0);
                $("#pullReason").prop('selectedIndex', 0);
                $("#sampleMode").prop('selectedIndex', 0);
                $('#chooseSamplesCheckbox').prop('checked', false);
                $('#fileOriginalCheckbox').prop('checked', true);
                $('#requireAccountValidationCheckbox').prop('checked', true);
                $('#pullNotes').val('');
                $('#pullGridDiv').hide();
            })
            .show();
        
        $("#contextMenu .releaseFile").unbind();
        $("#contextMenu .createPull").unbind();
        $("#contextMenu .reconvert").unbind();

        $("#contextMenu .releaseFile").bind('click', function () {
            if (!isStepHoldPointB) {
                launchModalDialog(elem, "Are you sure", "This will release job id " + id + ". Are you sure?", confirmReleaseFile, cancelReleaseFile, ModalButtons.OkCancel, true);
            } else {
                __doPostBack(heldTrackingGrid.attr('id').replace('_', '$'), 'release$' + id);
            }
            $('#contextMenu').hide();
        });
        $("#contextMenu .createPull").bind('click', function () {
            $('#pullDialog').show();
            $('#contextMenu').hide();
        });
        $("#contextMenu .reconvert").bind('click', function () {
            launchModalDialog(elem, "Are you sure", "This will reconvert tracking id " + id + ". Are you sure?", confirmReconvertFile, cancelReleaseFile, ModalButtons.OkCancel, true);
            $('#contextMenu').hide();
        });
        e.preventDefault();
    });
}

function confirmReleaseFile(event) {
    var control = event.data.param1;
    __doPostBack(heldTrackingGrid.attr('id').replace('_', '$'), 'release$' + $(control).text());
    id = -1;
}

function confirmReconvertFile(event) {
    var control = event.data.param1;
    __doPostBack(heldTrackingGrid.attr('id').replace('_', '$'), 'reconvert$' + $(control).text());
    id = -1;
}
function cancelReleaseFile() {
    id = -1;
}

function pullConfirm_clicked(e) {
    if (PullBy == PullByEnum.Unknown ||
        PullBy == PullByEnum.None ||
        PullType == PullTypeEnum.Unknown)
        return;
    
    var url = "";
    var inputData = "";
    var checkList = new Array();
    var isValidationRequired = "";
    if ($('#requireAccountValidationCheckbox').prop('checked')) {
        isValidationRequired = ", 'requireAccountValidation': true";
    } else {
        isValidationRequired = ", 'requireAccountValidation': false";
    }
    if (PullBy != PullByEnum.AccountNumber) {
        $('#pullGrid input:checked').each(function () {
            checkList.push($(this).val());
        });
    } else {
        $.each($('#pullText').val().split(','), function (index, value) {
            checkList.push(value.trim());
        });
    }

    if (typeof checkList !== 'undefined' && checkList.length > 0) {
        var pullType = PullType != PullTypeEnum.Both ? PullType : PullTypeEnum.Straight;
        var pullReason = $('#pullReason :selected').text();
        var pullNotes = $('#pullNotes').val();

        var getSamples = $('#chooseSamplesCheckbox').is(':checked');
        switch (PullBy) {
        case PullByEnum.AccountNumber:
            url = "PullService.asmx/CreatePullRequestByAccountNumbers";
            inputData = "{'trackingId':'" + id + "', 'accountNumbers':" + JSON.stringify(checkList).replace(/\"/g, "'") + isValidationRequired;
            break;
        case PullByEnum.ProductNumber:
            url = "PullService.asmx/CreatePullRequestByProductList";
            inputData = "{'trackingId':'" + id + "', 'productList':" + JSON.stringify(checkList).replace(/\"/g, "'");
            break;
        case PullByEnum.OptionNumber:
            url = "PullService.asmx/CreatePullRequestByOptionList";
            inputData = "{'trackingId':'" + id + "', 'optionList':" + JSON.stringify(checkList).replace(/\"/g, "'");
            break;
        case PullByEnum.FlexField1:
            if (getSamples) {
                url = "PullService.asmx/CreateSamplesByFlexFields";
                inputData = "{'flexFieldNumber':'1', 'trackingId':'" + id + "', 'flexFieldList':" + JSON.stringify(checkList).replace(/\"/g, "'") +
                    ", 'sampleMethod':'" + SampleMode + "'" +
                    ", 'sampleSize':'" + $('#sampleSize').val() + "'" +
                    ", 'pullReason':'" + pullReason + "'" +
                    ", 'pullNotes':'" + pullNotes + "'}";
            } else {
                url = "PullService.asmx/CreatePullRequestByFlexFields";
                inputData = "{'flexFieldNumber':'1', 'trackingId':'" + id + "', 'flexFieldList':" + JSON.stringify(checkList).replace(/\"/g, "'");
            }
            break;
        case PullByEnum.FlexField2:
            url = "PullService.asmx/CreatePullRequestByFlexFields";
            inputData = "{'flexFieldNumber':'2', 'trackingId':'" + id + "', 'flexFieldList':" + JSON.stringify(checkList).replace(/\"/g, "'");
            break;
        case PullByEnum.FlexField3:
            url = "PullService.asmx/CreatePullRequestByFlexFields";
            inputData = "{'flexFieldNumber':'3', 'trackingId':'" + id + "', 'flexFieldList':" + JSON.stringify(checkList).replace(/\"/g, "'");
            break;
        }

        if (PullBy == PullByEnum.FlexField1 && getSamples) {
            var jqxhr = $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: url,
                data: inputData// + ", 'pullType':'" + pullType + "'}"
            }).done(function(data, textStatus, jqXHR) {
                $('#pullGrid').empty();
                $('#pullText').empty();
                $('#pullDialog').hide();
            }).error(function(jqXHR, textStatus, errorThrown) {
                console.info(errorThrown);
                console.info(jqXHR.responseText);
            });
        } else {
            var jqxhr = $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: url,
                data: inputData +
                    ", 'pullType':'" + pullType + "'" +
                    ", 'pullReason':'" + pullReason + "'" +
                    ", 'pullNotes':'" + pullNotes + "'}"
            }).done(function(data, textStatus, jqXHR) {
                $('#pullGrid').empty();
                $('#pullText').empty();
                $('#pullDialog').hide();
            }).error(function(jqXHR, textStatus, errorThrown) {
                console.info(errorThrown);
                console.info(jqXHR.responseText);
            });
            if (PullType == PullTypeEnum.Both) {
                var jqxhr = $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    url: url,
                    data: inputData +
                        ", 'pullType':'" + PullTypeEnum.Reverse +
                        ", 'pullReason':'" + pullReason + "'" +
                        ", 'pullNotes':'" + pullNotes + "'" +
                        "'}"
                }).done(function(data, textStatus, jqXHR) {
                    $('#pullGrid').empty();
                    $('#pullText').empty();
                    $('#pullDialog').hide();
                });
            }
        }
    }
}

function pullType_changed(e) {
    var optionSelected = $('option:selected', this);
    var valueSelected = this.value;
    switch (valueSelected) {
        case "Both":
            PullType = PullTypeEnum.Both;
            break;
        case "Straight":
            PullType = PullTypeEnum.Straight;
            break;
        case "Reverse":
            PullType = PullTypeEnum.Reverse;
            break;
        default:
            PullType = PullTypeEnum.Unknown;
            handlePullByDisplay(false, false);
            break;
    }
}
function pullBy_changed(e) {
    var optionSelected = $('option:selected', this);
    var valueSelected = this.value;
    switch (valueSelected) {
        case "Account #":
            PullBy = PullByEnum.AccountNumber;
            handlePullByDisplay(false, true);
            break;
        case "Flex Field 1":
            PullBy = PullByEnum.FlexField1;
            handlePullByDisplay(true, false);
            break;
        case "Flex Field 2":
            PullBy = PullByEnum.FlexField2;
            handlePullByDisplay(true, false);
            break;
        case "Flex Field 3":
            PullBy = PullByEnum.FlexField3;
            handlePullByDisplay(true, false);
            break;
        case "Product #":
            PullBy = PullByEnum.ProductNumber;
            handlePullByDisplay(true, false);
            break;
        case "Option #":
            PullBy = PullByEnum.OptionNumber;
            handlePullByDisplay(true, false);
            break;
        default:
            PullBy = PullByEnum.Unknown;
            handlePullByDisplay(false, false);
            break;
    }
    console.info(PullBy);
}

function loadGrid() {
    var url = "";
    var inputData = "";
    switch (PullBy) {
        case PullByEnum.ProductNumber:
            url = "PullService.asmx/GetProductByTrackingGroupId";
            inputData = "{'trackingId':'" + id + "'}";
            break;
        case PullByEnum.OptionNumber:
            url = "PullService.asmx/GetProductOptionByTrackingGroupId";
            inputData = "{'trackingId':'" + id + "'}";
            break;
        case PullByEnum.FlexField1:
            url = "PullService.asmx/GetFlexFieldByTrackingGroupId";
            inputData = "{'flexFieldNumber':'1', 'trackingId':'" + id + "'}";
            break;
        case PullByEnum.FlexField2:
            url = "PullService.asmx/GetFlexFieldByTrackingGroupId";
            inputData = "{'flexFieldNumber':'2', 'trackingId':'" + id + "'}";
            break;
        case PullByEnum.FlexField3:
            url = "PullService.asmx/GetFlexFieldByTrackingGroupId";
            inputData = "{'flexFieldNumber':'3', 'trackingId':'" + id + "'}";
            break;
    }
    var jqxhr = $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        url: url,
        data: inputData
    }).done(function (data, textStatus, jqXHR) {
        $('#pullGrid').empty();
        for (key in data.d) {
            if (typeof (data.d[key]) == 'string') {
                var docId = data.d[key] == '' ? '[Null]' : data.d[key];
                $('#pullGrid').append('<li><input type="checkbox" id="' + docId + 'textbox" value="' + docId + '"/>' + docId +
                    '<label for="' + docId + 'textbox" </label> </li>');
            }
        }
    });
}

function handlePullByDisplay(showList, showText) {
    var pullGrid = $('#pullGridDiv');
    var pullText = $('#pullText');
    var chooseSamplesCheckbox = $('#chooseSamplesCheckbox');
    var chooseSamples = $('#chooseSamples');
    var sampleOptions = $('#sampleOptions');

    if (PullBy == PullByEnum.FlexField1 ||
        PullBy == PullByEnum.FlexField2 ||
        PullBy == PullByEnum.FlexField3 ||
        PullBy == PullByEnum.OptionNumber ||
        PullBy == PullByEnum.ProductNumber)
        loadGrid();

    if (PullBy == PullByEnum.FlexField1) {
        chooseSamples.show();
        if (chooseSamplesCheckbox.is(':checked'))
            sampleOptions.show();
        else
            sampleOptions.hide();
    }
    else {
        chooseSamples.hide();
        sampleOptions.hide();
    }

    if (showList)
        pullGrid.show();
    else
        pullGrid.hide();
    if (showText)
        pullText.show();
    else
        pullText.hide();
}

function pullText_pasted(e) {
    var e = $(this);
    setTimeout(function () {
        e.val($.trim(e.val()).replace(/\n+/g, ', '));
    }, 0);
}

function chooseSamplesCheckbox_changed(e) {
    if ($('#chooseSamplesCheckbox').is(':checked'))
    {
        $('#sampleOptions').show();
    }
    else
    {
        $('#sampleOptions').hide();
    }

}

function sampleMode_changed(e) {
    var valueSelected = this.value;
    switch (valueSelected) {
        case "First":
            SampleMode = SampleModeEnum.First;
            break;
        case "Random":
            SampleMode = SampleModeEnum.Random;
            break;
        case "Middle":
            SampleMode = SampleModeEnum.Middle;
            break;
        case "Last":
            SampleMode = SampleModeEnum.Last;
            break;
        default:
            SampleMode = SampleModeEnum.Unknown;
            handlePullByDisplay(false, false);
            break;
    }
}

function configurePullDialog() {
    var pullDialog = $('#pullDialog');
    var pullGrid = $('#pullGridDiv');
    var pullText = $('#pullText');
    var pullCancelButton = $('#pullCancel');
    var pullConfirmButton = $('#pullConfirm');
    var pullType = $('#pullType');
    var pullBy = $('#pullBy');
    var chooseSamplesCheckbox = $('#chooseSamplesCheckbox');
    var sampleMode = $('#sampleMode');
    
    chooseSamplesCheckbox.bind('change', chooseSamplesCheckbox_changed);
    pullText.bind('paste', pullText_pasted);
    pullCancelButton.bind('click', function () { pullDialog.hide(); });
    pullConfirmButton.bind('click', pullConfirm_clicked);
    pullBy.on('change', pullBy_changed);
    pullType.on('change', pullType_changed);
    sampleMode.on('change', sampleMode_changed);
    pullGrid.hide();
    pullText.hide();
    pullDialog.hide();
}

function configureReleaseWizard() {
    var releaseWizard = $('#ReleaseWizard');
    //var pullGrid = $('#pullGridDiv');
    //var pullText = $('#pullText');
    //var pullCancelButton = $('#pullCancel');
    //var pullConfirmButton = $('#pullConfirm');
    //var pullType = $('#pullType');
    //var pullBy = $('#pullBy');
    //var chooseSamplesCheckbox = $('#chooseSamplesCheckbox');
    //var sampleMode = $('#sampleMode');

    //chooseSamplesCheckbox.bind('change', chooseSamplesCheckbox_changed);
    //pullText.bind('paste', pullText_pasted);
    //pullCancelButton.bind('click', function () { releaseWizard.hide(); });
    //pullConfirmButton.bind('click', pullConfirm_clicked);
    //pullBy.on('change', pullBy_changed);
    //pullType.on('change', pullType_changed);
    //sampleMode.on('change', sampleMode_changed);
    //pullGrid.hide();
    //pullText.hide();
    //pullDialog.hide();
}

wms.heldtracking.fileReleaseWizard.PrintToPdf_Click = function (e) {
    wms.heldtracking.fileReleaseWizard.ReturnToRep.prop('disabled', wms.heldtracking.fileReleaseWizard.PrintToPdf.is(':checked'));
    wms.heldtracking.fileReleaseWizard.ReturnToRep.attr('checked', false);
    wms.heldtracking.fileReleaseWizard.ReturnToProgrammer.prop('disabled', wms.heldtracking.fileReleaseWizard.PrintToPdf.is(':checked'));
    wms.heldtracking.fileReleaseWizard.ReturnToProgrammer.attr('checked', false);
    wms.heldtracking.fileReleaseWizard.IsRush.prop('disabled', wms.heldtracking.fileReleaseWizard.PrintToPdf.is(':checked'));
    wms.heldtracking.fileReleaseWizard.IsRush.attr('checked', false);
    wms.heldtracking.fileReleaseWizard.UseSampleLogic.prop('disabled', wms.heldtracking.fileReleaseWizard.PrintToPdf.is(':checked'));
    wms.heldtracking.fileReleaseWizard.UseSampleLogic.attr('checked', false);
};

wms.heldtracking.fileReleaseWizard.deleteBilling_Click = function (e) {
    wms.heldtracking.fileReleaseWizard.DeleteReason.prop('disabled', !wms.heldtracking.fileReleaseWizard.DeleteBilling.is(':checked'));
};

wms.heldtracking.fileReleaseWizard.turnTimeAdjuster_Click = function (e) {
    wms.heldtracking.fileReleaseWizard.CalendarAdjuster.prop('disabled', !wms.heldtracking.fileReleaseWizard.TurnTimeAdjuster.is(':checked'));
    wms.heldtracking.fileReleaseWizard.TimeAdjuster.prop('disabled', !wms.heldtracking.fileReleaseWizard.TurnTimeAdjuster.is(':checked'));
};

// Make a button to handle updating the cells and handle posting back to the server the values.
// The button's postback event shoult be the code below:
wms.heldtracking.fileReleaseWizard.MailDateTimeChanged = function()
{
    var TrackingIdIndex = 0;
    var ScheduledMailDateIndex = 5;
    var selectedRow = $('.TurnTimeWebGrid tr').filter(function () {
        var selectedColor = 'rgb(255, 255, 255)';
        return ($(this).css('color') == selectedColor);
    });
    var TrackingId = selectedRow.find('td')[TrackingIdIndex];
    var ScheduledMailDate = selectedRow.find('td')[ScheduledMailDateIndex];

    ScheduledMailDate.innerText =
        wms.heldtracking.fileReleaseWizard.CalendarAdjuster.val() + ' ' +
        wms.heldtracking.fileReleaseWizard.TimeAdjuster.val();

    if (wms.heldtracking.fileReleaseWizard.TurnTimesJSON.length == 0)
    {
        wms.heldtracking.fileReleaseWizard.TurnTimesJSON.push({ 'SlaId': TrackingId.innerText, 'ScheduledTurnTime': ScheduledMailDate.innerText });
    }
    else
    {
        var turnTimePreviouslyModified = false;
        for(index in wms.heldtracking.fileReleaseWizard.TurnTimesJSON)
        {
            if (wms.heldtracking.fileReleaseWizard.TurnTimesJSON[index].SlaId == TrackingId.innerText)
            {
                wms.heldtracking.fileReleaseWizard.TurnTimesJSON[index].ScheduledTurnTime = ScheduledMailDate.innerText;
                turnTimePreviouslyModified = true;
            }
        };
        if(!turnTimePreviouslyModified)
        {
            wms.heldtracking.fileReleaseWizard.TurnTimesJSON.push({ 'SlaId': TrackingId.innerText, 'ScheduledTurnTime': ScheduledMailDate.innerText });
        }
    }
    // Also save information in hidden field as JSON construct, for use in server.
    wms.heldtracking.fileReleaseWizard.AdjustedTurnTimes.val(
            JSON.stringify(wms.heldtracking.fileReleaseWizard.TurnTimesJSON)
        );
    // __doPostBack(updateTurnTimeButton.attr('id').replace('_', '$'), wms.heldtracking.fileReleaseWizard.ScheduledMailDate.innerText);
}

wms.heldtracking.fileReleaseWizard.Initialize = function()
{
    wms.heldtracking.fileReleaseWizard.ReleaseForSamples = $('[JQName="ReleaseForSamples"]>input');
    wms.heldtracking.fileReleaseWizard.PrintToPdf = $('[JQName="PrintToPdf"]>input');
    wms.heldtracking.fileReleaseWizard.ReturnToRep = $('[JQName="ReturnToRep"]>input');
    wms.heldtracking.fileReleaseWizard.ReturnToProgrammer = $('[JQName="ReturnToProgrammer"]>input');
    wms.heldtracking.fileReleaseWizard.IsRush = $('[JQName="IsRush"]>input');
    wms.heldtracking.fileReleaseWizard.UseSampleLogic = $('[JQName="UseSampleLogic"]>input');
    wms.heldtracking.fileReleaseWizard.DeleteBilling = $('[JQName="DeleteBilling"]>input');
    wms.heldtracking.fileReleaseWizard.DeleteReason = $('[JQName="DeleteReason"]');
    wms.heldtracking.fileReleaseWizard.TurnTimeAdjuster = $('[JQName="TurnTimeAdjuster"]>input');
    wms.heldtracking.fileReleaseWizard.CalendarAdjuster = $('[JQName="CalendarAdjuster"]');
    wms.heldtracking.fileReleaseWizard.TimeAdjuster = $('[JQName="TimeAdjuster"]');
    wms.heldtracking.fileReleaseWizard.AdjustedTurnTimes = $('#AdjustedTurnTimes');
    wms.heldtracking.fileReleaseWizard.PrintToPdf.bind('click', wms.heldtracking.fileReleaseWizard.PrintToPdf_Click);
    wms.heldtracking.fileReleaseWizard.DeleteBilling.bind('click', wms.heldtracking.fileReleaseWizard.deleteBilling_Click);
    wms.heldtracking.fileReleaseWizard.TurnTimeAdjuster.bind('click', wms.heldtracking.fileReleaseWizard.turnTimeAdjuster_Click);
    wms.heldtracking.fileReleaseWizard.TurnTimesJSON = new Array();
    //  Instead of using the event to update turn times trigger from these controls:
    wms.heldtracking.fileReleaseWizard.CalendarAdjuster.bind('change', wms.heldtracking.fileReleaseWizard.MailDateTimeChanged);
    wms.heldtracking.fileReleaseWizard.TimeAdjuster.bind('change', wms.heldtracking.fileReleaseWizard.MailDateTimeChanged);

    // Trigger the turn time update from this button's click event:
    // updateTurnTimeButton.bind('click', wms.heldtracking.fileReleaseWizard.MailDateTimeChanged);

    wms.heldtracking.fileReleaseWizard.PrintToPdf_Click();
    wms.heldtracking.fileReleaseWizard.deleteBilling_Click();
    wms.heldtracking.fileReleaseWizard.turnTimeAdjuster_Click();
}

wms.heldtracking.onReady = function () {
    heldTrackingGrid = $('table.heldTrackingWebGrid');
    configurePullDialog();
    configureContextMenu();
    wms.heldtracking.fileReleaseWizard.Initialize();
}

// Page Load Event
$(document).ready(wms.heldtracking.onReady);

