if (typeof wms === 'undefined')
    wms = new Object();
if (typeof wms.pullDialog === 'undefined')
    wms.pullDialog = new Object();

wms.pullDialog.trackingId = -1;

wms.pullDialog.pullTypeEnum = { Straight: 0, Reverse: 1, Restart: 2, Reprint: 3, Samples: 4, Both: 5, Unknown: 99 }
wms.pullDialog.pullType = wms.pullDialog.pullTypeEnum.Both;
wms.pullDialog.pullByEnum = { Unknown: -1, AccountNumber: 0, FlexField1: 1, FlexField2: 2, FlexField3: 3, ProductNumber: 4, OptionNumber: 5, None: -99 };
wms.pullDialog.sampleModeEnum = { Unknown: 0, First: 1, Middle: 2, Last: 3, Random: 4 };
wms.pullDialog.sampleMode = wms.pullDialog.sampleModeEnum.Unknown;

wms.pullDialog.pullBy = wms.pullDialog.pullByEnum.None;

wms.pullDialog.pullConfirm_clicked
= function (e)
{
    if (wms.pullDialog.pullBy == wms.pullDialog.pullTypeEnum.Unknown ||
        wms.pullDialog.pullBy == wms.pullDialog.pullTypeEnum.None ||
        wms.pullDialog.pullType == wms.pullDialog.pullTypeEnum.Unknown)
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
    if (wms.pullDialog.pullBy != wms.pullDialog.pullByEnum.AccountNumber) {
        $('#pullGrid input:checked').each(function () {
            checkList.push($(this).val());
        });
    } else {
        $.each($('#pullText').val().split(','), function (index, value) {
            checkList.push(value.trim());
        });
    }

    if (typeof checkList !== 'undefined' && checkList.length > 0) {
        var pullType = wms.pullDialog.pullType != wms.pullDialog.pullTypeEnum.Both ? wms.pullDialog.pullType : wms.pullDialog.pullTypeEnum.Straight;
        var pullReason = $('#pullReason :selected').text();
        var pullNotes = $('#pullNotes').val();

        var getSamples = $('#chooseSamplesCheckbox').is(':checked');
        switch (wms.pullDialog.pullBy) {
        case wms.pullDialog.pullByEnum.AccountNumber:
            url = "PullService.asmx/CreatePullRequestByAccountNumbers";
            inputData = "{'trackingId':'" + wms.pullDialog.trackingId + "', " +
                "'accountNumbers':" + JSON.stringify(checkList).replace(/\"/g, "'") + isValidationRequired;
            break;
        case wms.pullDialog.pullByEnum.ProductNumber:
            if (getSamples) {
                url = "PullService.asmx/CreateSamplesByProductList";
                inputData = "{'trackingId':'" + wms.pullDialog.trackingId +
                    "', 'productList':" + JSON.stringify(checkList).replace(/\"/g, "'") +
                    ", 'sampleMethod':'" + wms.pullDialog.sampleMode + "'" +
                    ", 'sampleSize':'" + $('#sampleSize').val() + "'" +
                    ", 'pullReason':'" + pullReason + "'" +
                    ", 'pullNotes':'" + pullNotes + "'}";
            } else {
                url = "PullService.asmx/CreatePullRequestByProductList";
                inputData = "{'trackingId':'" + wms.pullDialog.trackingId +
                    "', 'productList':" + JSON.stringify(checkList).replace(/\"/g, "'");
            }
            break;
        case wms.pullDialog.pullByEnum.OptionNumber:
            if (getSamples) {
                url = "PullService.asmx/CreateSamplesByOptionList";
                inputData = "{'trackingId':'" + wms.pullDialog.trackingId +
                    "', 'optionList':" + JSON.stringify(checkList).replace(/\"/g, "'") +
                    ", 'sampleMethod':'" + wms.pullDialog.sampleMode + "'" +
                    ", 'sampleSize':'" + $('#sampleSize').val() + "'" +
                    ", 'pullReason':'" + pullReason + "'" +
                    ", 'pullNotes':'" + pullNotes + "'}";
            } else {
                url = "PullService.asmx/CreatePullRequestByOptionList";
                inputData = "{'trackingId':'" + wms.pullDialog.trackingId +
                    "', 'optionList':" + JSON.stringify(checkList).replace(/\"/g, "'");
            }
            break;
        case wms.pullDialog.pullByEnum.FlexField1:
            if (getSamples) {
                url = "PullService.asmx/CreateSamplesByFlexFields";
                inputData = "{'flexFieldNumber':'1', 'trackingId':'" + wms.pullDialog.trackingId +
                    "', 'flexFieldList':" + JSON.stringify(checkList).replace(/\"/g, "'") +
                    ", 'sampleMethod':'" + wms.pullDialog.sampleMode + "'" +
                    ", 'sampleSize':'" + $('#sampleSize').val() + "'" +
                    ", 'pullReason':'" + pullReason + "'" +
                    ", 'pullNotes':'" + pullNotes + "'}";
            } else {
                url = "PullService.asmx/CreatePullRequestByFlexFields";
                inputData = "{'flexFieldNumber':'1', 'trackingId':'" + wms.pullDialog.trackingId +
                    "', 'flexFieldList':" + JSON.stringify(checkList).replace(/\"/g, "'");
            }
            break;
        case wms.pullDialog.pullByEnum.FlexField2:
            if (getSamples) {
                url = "PullService.asmx/CreateSamplesByFlexFields";
                inputData = "{'flexFieldNumber':'2', 'trackingId':'" + wms.pullDialog.trackingId +
                    "', 'flexFieldList':" + JSON.stringify(checkList).replace(/\"/g, "'") +
                    ", 'sampleMethod':'" + wms.pullDialog.sampleMode + "'" +
                    ", 'sampleSize':'" + $('#sampleSize').val() + "'" +
                    ", 'pullReason':'" + pullReason + "'" +
                    ", 'pullNotes':'" + pullNotes + "'}";
            } else {
                url = "PullService.asmx/CreatePullRequestByFlexFields";
                inputData = "{'flexFieldNumber':'2', 'trackingId':'" + wms.pullDialog.trackingId +
                    "', 'flexFieldList':" + JSON.stringify(checkList).replace(/\"/g, "'");
            }
            break;
        case wms.pullDialog.pullByEnum.FlexField3:
            if (getSamples) {
                url = "PullService.asmx/CreateSamplesByFlexFields";
                inputData = "{'flexFieldNumber':'3', 'trackingId':'" + wms.pullDialog.trackingId +
                    "', 'flexFieldList':" + JSON.stringify(checkList).replace(/\"/g, "'") +
                    ", 'sampleMethod':'" + wms.pullDialog.sampleMode + "'" +
                    ", 'sampleSize':'" + $('#sampleSize').val() + "'" +
                    ", 'pullReason':'" + pullReason + "'" +
                    ", 'pullNotes':'" + pullNotes + "'}";
            } else {
                url = "PullService.asmx/CreatePullRequestByFlexFields";
                inputData = "{'flexFieldNumber':'3', 'trackingId':'" + wms.pullDialog.trackingId +
                    "', 'flexFieldList':" + JSON.stringify(checkList).replace(/\"/g, "'");
            }
            break;
        }

        if (getSamples) {
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
            }).always(function() {
                wms.pullDialog.hide();
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
            }).always(function() {
                wms.pullDialog.hide();
            });
            if (wms.pullDialog.pullType == wms.pullDialog.pullTypeEnum.Both) {
                var jqxhr = $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    url: url,
                    data: inputData +
                        ", 'pullType':'" + wms.pullDialog.pullTypeEnum.Reverse +
                        ", 'pullReason':'" + pullReason + "'" +
                        ", 'pullNotes':'" + pullNotes + "'" + "'}"
                }).done(function(data, textStatus, jqXHR) {
                    $('#pullGrid').empty();
                    $('#pullText').empty();
                    $('#pullDialog').hide();
                }).always(function() {
                    wms.pullDialog.hide();
                });
            }
        }
    }
}

wms.pullDialog.pullType_changed
= function (e)
{
    var optionSelected = $('option:selected', this);
    var valueSelected = this.value;
    switch (valueSelected) {
        case "Both":
            wms.pullDialog.pullType = wms.pullDialog.pullTypeEnum.Both;
            break;
        case "Straight":
            wms.pullDialog.pullType = wms.pullDialog.pullTypeEnum.Straight;
            break;
        case "Reverse":
            wms.pullDialog.pullType = wms.pullDialog.pullTypeEnum.Reverse;
            break;
        default:
            wms.pullDialog.pullType = wms.pullDialog.pullTypeEnum.Unknown;
            wms.pullDialog.handlePullByDisplay(false, false);
            break;
    }
}

wms.pullDialog.pullBy_changed
= function (e)
{
    var optionSelected = $('option:selected', this);
    var valueSelected = this.value;
    switch (valueSelected) {
        case "Account #":
            wms.pullDialog.pullBy = wms.pullDialog.pullByEnum.AccountNumber;
            wms.pullDialog.handlePullByDisplay(false, true);
            break;
        case "Flex Field 1":
            wms.pullDialog.pullBy = wms.pullDialog.pullByEnum.FlexField1;
            wms.pullDialog.handlePullByDisplay(true, false);
            break;
        case "Flex Field 2":
            wms.pullDialog.pullBy = wms.pullDialog.pullByEnum.FlexField2;
            wms.pullDialog.handlePullByDisplay(true, false);
            break;
        case "Flex Field 3":
            wms.pullDialog.pullBy = wms.pullDialog.pullByEnum.FlexField3;
            wms.pullDialog.handlePullByDisplay(true, false);
            break;
        case "Product #":
            wms.pullDialog.pullBy = wms.pullDialog.pullByEnum.ProductNumber;
            wms.pullDialog.handlePullByDisplay(true, false);
            break;
        case "Option #":
            wms.pullDialog.pullBy = wms.pullDialog.pullByEnum.OptionNumber;
            wms.pullDialog.handlePullByDisplay(true, false);
            break;
        default:
            wms.pullDialog.pullBy = wms.pullDialog.pullByEnum.Unknown;
            wms.pullDialog.handlePullByDisplay(false, false);
            break;
    }
    console.info(wms.pullDialog.pullBy);
}

wms.pullDialog.chooseSamplesCheckbox_changed
= function (e)
{
    if ($('#chooseSamplesCheckbox').is(':checked'))
    {
        $('#sampleOptions').show();
    }
    else
    {
        $('#sampleOptions').hide();
    }

}

wms.pullDialog.loadGrid
= function ()
{
    var url = "";
    var inputData = "";
    switch (wms.pullDialog.pullBy) {
        case wms.pullDialog.pullByEnum.ProductNumber:
            url = "PullService.asmx/GetProductByTrackingGroupId";
            inputData = "{'trackingId':'" + wms.pullDialog.trackingId + "'}";
            break;
        case wms.pullDialog.pullByEnum.OptionNumber:
            url = "PullService.asmx/GetProductOptionByTrackingGroupId";
            inputData = "{'trackingId':'" + wms.pullDialog.trackingId + "'}";
            break;
        case wms.pullDialog.pullByEnum.FlexField1:
            url = "PullService.asmx/GetFlexFieldByTrackingGroupId";
            inputData = "{'flexFieldNumber':'1', 'trackingId':'" + wms.pullDialog.trackingId + "'}";
            break;
        case wms.pullDialog.pullByEnum.FlexField2:
            url = "PullService.asmx/GetFlexFieldByTrackingGroupId";
            inputData = "{'flexFieldNumber':'2', 'trackingId':'" + wms.pullDialog.trackingId + "'}";
            break;
        case wms.pullDialog.pullByEnum.FlexField3:
            url = "PullService.asmx/GetFlexFieldByTrackingGroupId";
            inputData = "{'flexFieldNumber':'3', 'trackingId':'" + wms.pullDialog.trackingId + "'}";
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
    }).error(function (data, textStatus, jqXHR) {
        var d = data;
        var text = textStatus;
        var response = jqXHR;
    });
}

wms.pullDialog.handlePullByDisplay
= function (showList, showText)
{
    var pullGrid = $('#pullGridDiv');
    var pullText = $('#pullText');
    var chooseSamplesCheckbox = $('#chooseSamplesCheckbox');
    var chooseSamples = $('#chooseSamples');
    var sampleOptions = $('#sampleOptions');

    if (wms.pullDialog.pullBy == wms.pullDialog.pullByEnum.FlexField1 ||
        wms.pullDialog.pullBy == wms.pullDialog.pullByEnum.FlexField2 ||
        wms.pullDialog.pullBy == wms.pullDialog.pullByEnum.FlexField3 ||
        wms.pullDialog.pullBy == wms.pullDialog.pullByEnum.OptionNumber ||
        wms.pullDialog.pullBy == wms.pullDialog.pullByEnum.ProductNumber)
    {
        wms.pullDialog.loadGrid();
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

wms.pullDialog.pullText_pasted
= function (e)
{
    var e = $(this);
    setTimeout(function () {
        e.val($.trim(e.val()).replace(/\n+/g, ', '));
    }, 0);
}

wms.pullDialog.sampleMode_changed
= function (e)
{
    var valueSelected = this.value;
    switch (valueSelected) {
        case "First":
            wms.pullDialog.sampleMode = wms.pullDialog.sampleModeEnum.First;
            break;
        case "Random":
            wms.pullDialog.sampleMode = wms.pullDialog.sampleModeEnum.Random;
            break;
        case "Last":
            wms.pullDialog.sampleMode = wms.pullDialog.sampleModeEnum.Last;
            break;
        case "Middle":
            wms.pullDialog.sampleMode = wms.pullDialog.sampleModeEnum.Middle;
            break;
        default:
            wms.pullDialog.sampleMode = wms.pullDialog.sampleModeEnum.Unknown;
            wms.pullDialog.handlePullByDisplay(false, false);
            break;
    }
}

wms.pullDialog.hide
= function()
{
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
    $('#pullDialog').hide();
}

wms.pullDialog.configurePullDialog = function () {
    var pullDialog = $('#pullDialog');
    var pullGrid = $('#pullGridDiv');
    var pullText = $('#pullText');
    var pullCancelButton = $('#pullCancel');
    var pullConfirmButton = $('#pullConfirm');
    var pullType = $('#pullType');
    var pullBy = $('#pullBy');
    var sampleMode = $('#sampleMode');
    var chooseSamplesCheckbox = $('#chooseSamplesCheckbox');

    chooseSamplesCheckbox.bind('change', wms.pullDialog.chooseSamplesCheckbox_changed);
    pullText.bind('paste', wms.pullDialog.pullText_pasted);
    pullCancelButton.bind('click', wms.pullDialog.hide);
    pullConfirmButton.bind('click', wms.pullDialog.pullConfirm_clicked);
    pullBy.on('change', wms.pullDialog.pullBy_changed);
    pullType.on('change', wms.pullDialog.pullType_changed);
    sampleMode.on('change', wms.pullDialog.sampleMode_changed);
    pullGrid.hide();
    pullText.hide();
    pullDialog.hide();
}

wms.pullDialog.onReady
= function()
{
    wms.pullDialog.configurePullDialog();
}