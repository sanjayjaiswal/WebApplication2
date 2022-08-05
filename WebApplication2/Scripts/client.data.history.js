if (typeof wms === 'undefined')
    wms = new Object();
if (typeof wms.clientdatahistory === 'undefined')
    wms.clientdatahistory = new Object();

var PullTypeEnum = { Straight: 0, Reverse: 1, Restart: 2, Reprint: 3, Samples: 4, Both: 5, Unknown: 99 }
var PullType = PullTypeEnum.Both;
var PullByEnum = { Unknown: -1, AccountNumber: 0, FlexField1: 1, FlexField2: 2, FlexField3: 3, ProductNumber: 4, OptionNumber: 5, None: -99 };
var SampleModeEnum = { Unknown: 0, First: 1, Middle: 2, Last: 3, Random: 4};
var SampleMode = SampleModeEnum.Unknown;
var PullBy = PullByEnum.None;
var AccountNumbers;
var id = -1;

function showTop_Checked(e) {
    var isChecked = $('.showAllCheckBox').is(':checked');
    $('.showTopText').prop('disabled', isChecked);
}

function handleRadioChecked(e) {
    var elem, evt = e ? e : event;
    if (evt.srcElement) elem = evt.srcElement;
    else if (evt.target) elem = evt.target;
    hideValidators();

    switch (elem.name) {
        case "trackingSearchMode":
            var trackingIdTextBox = $('.trackingIdTextBox');
            var trackingDataCenterNumber = $('.trackingDataCenterNumber');
            var trackingDataCenterName = $('.trackingDataCenterName');
            var trackingWorkflowGroup = $('.trackingWorkflowGroup');
            var startDateTextBox = $('.startTracking');
            var endDateTextBox = $('.endTracking');
            //wms.clientdatahistory.clearTextBoxes();
            switch (elem.value) {
                case "trackingId":
                    trackingIdTextBox.prop('disabled', false);
                    trackingDataCenterNumber.prop('disabled', true);
                    trackingDataCenterName.prop('disabled', true);
                    trackingWorkflowGroup.prop('disabled', true);
                    startDateTextBox.prop('disabled', true);
                    endDateTextBox.prop('disabled', true);
                    break;
                case "trackingDataCenterNumber":
                    trackingIdTextBox.prop('disabled', true);
                    trackingDataCenterNumber.prop('disabled', false);
                    trackingDataCenterName.prop('disabled', true);
                    trackingWorkflowGroup.prop('disabled', true);
                    startDateTextBox.prop('disabled', false);
                    endDateTextBox.prop('disabled', false);
                    break;
                case "trackingDataCenterName":
                    trackingIdTextBox.prop('disabled', true);
                    trackingDataCenterNumber.prop('disabled', true);
                    trackingDataCenterName.prop('disabled', false);
                    trackingWorkflowGroup.prop('disabled', true);
                    startDateTextBox.prop('disabled', false);
                    endDateTextBox.prop('disabled', false);
                    break;
                case "trackingWorkflowGroup":
                    trackingIdTextBox.prop('disabled', true);
                    trackingDataCenterNumber.prop('disabled', true);
                    trackingDataCenterName.prop('disabled', true);
                    trackingWorkflowGroup.prop('disabled', false);
                    startDateTextBox.prop('disabled', false);
                    endDateTextBox.prop('disabled', false);
                    break;
            }
            break;
        case "JobSearchMode":
            var jobNumberText = $('.JobNumberText');
            var startDateText = $('.startDateText');
            var endDateText = $('.endDateText');
            var readOnly = $('.JobNumberReadOnly');
            switch (elem.value) {
                case "JobNumber":
                    jobNumberText.prop('disabled', false);
                    readOnly.val('enabled');
                    startDateText.prop('disabled', true);
                    endDateText.prop('disabled', true);
                    break;
                case "dates":
                    jobNumberText.prop('disabled', true);
                    readOnly.val('disabled');
                    startDateText.prop('disabled', false);
                    endDateText.prop('disabled', false);
                    break;
            }
            break;
    }
}

function hideValidators() {
    $('.validator').hide();
}

function isTrackingIdValid(classname) {
    hideValidators();
    if (classname == null) {
        var radio = $('input[name=trackingSearchMode]:checked').val();
        switch (radio) {
            case 'trackingId':
                classname = '.trackingIdTextBox';
                break;
            case 'trackingDataCenterNumber':
                classname = '.trackingDataCenterNumber';
                break;
            case 'trackingDataCenterName':
                classname = '.trackingDataCenterName';
                break;
            case 'trackingWorkflowGroup':
                classname = '.trackingWorkflowGroup';
                break;
        }
    }
    var trackingIdTextBox = $(classname);
    var trackingIdTextBoxValidator = $(classname + 'Validator');
    if (trackingIdTextBox.val() == '') {
        trackingIdTextBoxValidator.show();
        return false;
    }
    trackingIdTextBoxValidator.hide();
    return true;
}

function isJobInfoSearchValid() {
    hideValidators();
    var jobNumberText = $('.JobNumberText');
    var jobNumberTextValidator = $('.jobNumberTextValidator');
    if (!jobNumberText.prop('disabled')) {
        if (jobNumberText.val() == '') {
            jobNumberTextValidator.show();
            return false;
        }
    }
    jobNumberTextValidator.hide();
    return true;
}

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
        $('#pullGrid input:checked').each(function() {
            checkList.push($(this).val());
        });
    } else {
        $.each($('#pullText').val().split(','), function(index, value) {
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
                    ", 'pullType':'" + pullType +
                    ", 'pullReason':'" + pullReason + "'" +
                    ", 'pullNotes':'" + pullNotes + "'" + "'}"
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
                        ", 'pullNotes':'" + pullNotes + "'" + "'}"
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

function sampleMode_changed(e){
    var valueSelected = this.value;
    switch (valueSelected) {
        case "First":
            SampleMode = SampleModeEnum.First;
            break;
        case "Random":
            SampleMode = SampleModeEnum.Random;
            break;
        case "Last":
            SampleMode = SampleModeEnum.Last;
            break;
        case "Middle":
            SampleMode = SampleModeEnum.Middle;
            break;
        default:
            SampleMode = SampleModeEnum.Unknown;
            handlePullByDisplay(false, false);
            break;
    }
}

wms.clientdatahistory.configurePullDialog = function() {
    var pullDialog = $('#pullDialog');
    var pullGrid = $('#pullGridDiv');
    var pullText = $('#pullText');
    var pullCancelButton = $('#pullCancel');
    var pullConfirmButton = $('#pullConfirm');
    var pullType = $('#pullType');
    var pullBy = $('#pullBy');
    var sampleMode = $('#sampleMode');
    var chooseSamplesCheckbox = $('#chooseSamplesCheckbox');

    PullType = PullTypeEnum.Unknown;
    SampleMode = SampleModeEnum.Unknown;
    PullBy = PullByEnum.None;
    $('#chooseSamplesCheckbox').prop('checked', false);
    $('#fileOriginalCheckbox').prop('checked', true);
    $('#requireAccountValidationCheckbox').prop('checked', true);
    
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

wms.clientdatahistory.configureContextMenu = function() {
    // Handle Tracking Grid Context Menu
    $('.trackingWebGrid tr').bind("contextmenu", function (e) {
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
        $("#trackingContextMenu").css({ "top": (e.pageY - 15) + "px", "left": Math.min(e.pageX - 5, e.pageX - ((document.body.offsetWidth / 2) - 580)) + "px" })
            .bind('mouseleave', function () {
                $('#trackingContextMenu').hide();
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
        $("#trackingContextMenu .pullRequest").unbind();

        $("#trackingContextMenu .pullRequest").bind('click', function () {
            $('#pullDialog').show();
            $('#contextMenu').hide();
        });
        e.preventDefault();
    });

    $('.jobWebGrid tr').bind("contextmenu", function (e) {
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
        e.preventDefault();
    });

    $('.clientDataHistoryWebGrid tr').bind("contextmenu", function (e) {
        $("#contextMenu").css({ "top": (e.pageY - 15) + "px", "left": Math.min(e.pageX - 5, e.pageX - ((document.body.offsetWidth / 2) - 580)) + "px" })
            .bind('mouseleave', function () {
                $('#contextMenu').hide();
            })
            .show();
        $("#contextMenu .preview").unbind();

        $("#contextMenu .preview").bind('click', function () {
            $('#contextMenu').hide();
        });
        e.preventDefault();
    });
}

function chooseSamplesCheckbox_changed(e) {
    if ($('#chooseSamplesCheckbox').is(':checked')) {
        $('#sampleOptions').show();
    }
    else {
        $('#sampleOptions').hide();
    }

}

wms.clientdatahistory.onReady = function() {
    wms.clientdatahistory.configurePullDialog();
    wms.clientdatahistory.configureContextMenu();
}

// Page Load Event
$(document).ready(wms.clientdatahistory.onReady);