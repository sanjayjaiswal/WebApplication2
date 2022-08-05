/// <reference path="global.js" />
if (typeof wms === 'undefined')
    wms = new Object();
if (typeof wms.convertedData === 'undefined')
    wms.convertedData = new Object();
if (typeof wms.composedData === 'undefined')
    wms.composedData = new Object();

wms.convertedData.disableInputControls 
= function(exemptControls)
{
    if ($.isArray(exemptControls)) {
        $.each(exemptControls, function () {
            $(this).prop("disabled", false);
        });
        if ($.inArray(wms.convertedData.trackingId, exemptControls) < 0)
            wms.convertedData.trackingId.prop("disabled", true);
        if ($.inArray(wms.convertedData.accountNumber, exemptControls) < 0)
            wms.convertedData.accountNumber.prop("disabled", true);
        if ($.inArray(wms.convertedData.workflowGroup, exemptControls) < 0)
            wms.convertedData.workflowGroup.prop("disabled", true);
        if ($.inArray(wms.convertedData.startDate, exemptControls) < 0)
            wms.convertedData.startDate.prop("disabled", true);
        if ($.inArray(wms.convertedData.startTime, exemptControls) < 0)
            wms.convertedData.startTime.prop("disabled", true);
        if ($.inArray(wms.convertedData.endDate, exemptControls) < 0)
            wms.convertedData.endDate.prop("disabled", true);
        if ($.inArray(wms.convertedData.endTime, exemptControls) < 0)
        wms.convertedData.endTime.prop("disabled", true);
    }
    else {
        $(exemptControls).prop("disabled", false);
        wms.convertedData.trackingId.not(exemptControls).prop("disabled", true);
        wms.convertedData.accountNumber.not(exemptControls).prop("disabled", true);
        wms.convertedData.workflowGroup.not(exemptControls).prop("disabled", true);
        wms.convertedData.startDate.not(exemptControls).prop("disabled", true);
        wms.convertedData.startTime.not(exemptControls).prop("disabled", true);
        wms.convertedData.endDate.not(exemptControls).prop("disabled", true);
        wms.convertedData.endTime.not(exemptControls).prop("disabled", true);
    }
}

wms.convertedData.initialize
= function()
{
    wms.convertedData.showDetailedButton = $('#convertedDetailsDetailedButton');
    wms.convertedData.showSummaryButton = $('#convertedDetailsSummaryButton');
    wms.convertedData.detailedDiv = $('#convertedDetailsDetailedDiv');
    wms.convertedData.summaryDiv = $('#convertedDetailsSummaryDiv');
    wms.convertedData.trackingRadio = $('#trackingIdRadio');
    wms.convertedData.trackingId = $('[jqname="TrackingIdTextbox"]');
    wms.convertedData.accountRadio = $('#accountSearchRadio');
    wms.convertedData.accountNumber = $('[jqname="AccountSearchTextbox"]');
    wms.convertedData.workflowGroupRadio = $('#workflowGroupRadio');
    wms.convertedData.workflowGroup = $('[jqname="WorkflowGroupDropDown"]');
    wms.convertedData.dateRangeRadio = $('#dateRangeRadio');
    wms.convertedData.searchButton = $('#searchButton');
    wms.convertedData.startDate = $('[jqname="StartDateTextbox"]');
    wms.convertedData.startTime = $('[jqname="StartTimeTextbox"]');
    wms.convertedData.endDate = $('[jqname="EndDateTextbox"]');
    wms.convertedData.endTime = $('[jqname="EndTimeTextbox"]');

    wms.convertedData.workflowGroupRadio.exemptControls
        = new Array(
            wms.convertedData.workflowGroup,
            wms.convertedData.startDate,
            wms.convertedData.startTime,
            wms.convertedData.endDate,
            wms.convertedData.endTime
        );

    wms.convertedData.dateRangeRadio.exemptControls
        = new Array(
            wms.convertedData.startDate,
            wms.convertedData.startTime,
            wms.convertedData.endDate,
            wms.convertedData.endTime
        );

    if (wms.convertedData.accountRadio.prop('checked'))
    {
        wms.convertedData.accountNumber.prop("disabled", false);
        wms.convertedData.disableInputControls(wms.convertedData.accountNumber);
    }
    if (wms.convertedData.trackingRadio.prop('checked')) {
        wms.convertedData.disableInputControls(wms.convertedData.trackingId);
    }
    if (wms.convertedData.workflowGroupRadio.prop('checked')) {
        wms.convertedData.disableInputControls(wms.convertedData.workflowGroupRadio.exemptControls);
    }
    if (wms.convertedData.dateRangeRadio.prop('checked')) {
        wms.convertedData.disableInputControls(wms.convertedData.dateRangeRadio.exemptControls);
    }

    wms.convertedData.trackingId.focus(function () {
        wms.convertedData.disableInputControls(wms.convertedData.trackingId);
        wms.convertedData.trackingId.val('');
        wms.convertedData.trackingRadio.prop('checked', true);
    }).keypress(function (e) {
        if (e.keyCode == 13) {
            wms.convertedData.searchButton.click();
            return false;
        }
    });
    wms.convertedData.trackingRadio.change(function () {
        wms.convertedData.disableInputControls(wms.convertedData.trackingId);
        wms.convertedData.trackingId.val('');
    });

    wms.convertedData.accountNumber.focus(function () {
        wms.convertedData.disableInputControls(wms.convertedData.accountNumber);
        wms.convertedData.accountNumber.val('');
        wms.convertedData.accountRadio.prop('checked', true);
    }).keypress(function (e) {
        if (e.keyCode == 13) {
            wms.convertedData.searchButton.click();
            return false;
        }
    });
    wms.convertedData.accountRadio.change(function () {
        wms.convertedData.disableInputControls(wms.convertedData.accountNumber);
        wms.convertedData.accountNumber.val('');
    });

    wms.convertedData.workflowGroup.focus(function () {
        wms.convertedData.disableInputControls(wms.convertedData.workflowGroupRadio.exemptControls);
        wms.convertedData.workflowGroup.val('');
        wms.convertedData.workflowGroupRadio.prop('checked', true);
    }).keypress(function (e) {
        if (e.keyCode == 13) {
            wms.convertedData.searchButton.click();
            return false;
        }
    });
    wms.convertedData.workflowGroupRadio.change(function () {
        wms.convertedData.disableInputControls(wms.convertedData.workflowGroupRadio.exemptControls);
        wms.convertedData.workflowGroup.val('');
    });
    
    wms.convertedData.dateRangeRadio.change(function () {
        wms.convertedData.disableInputControls(wms.convertedData.dateRangeRadio.exemptControls);
    });

    wms.convertedData.showDetailedButton.click(function () {
        wms.convertedData.detailedDiv.show();
        wms.convertedData.summaryDiv.hide();
    });
    wms.convertedData.showSummaryButton.click(function () {
        wms.convertedData.detailedDiv.hide();
        wms.convertedData.summaryDiv.show();
    });
}


wms.composedData.disableInputControls 
= function () {
    wms.composedData.jobNumber.prop("disabled", true);
    wms.composedData.accountNumber.prop("disabled", true);
    wms.composedData.workflowGroup.prop("disabled", true);
    wms.composedData.startDate.prop("disabled", true);
    wms.composedData.startTime.prop("disabled", true);
    wms.composedData.endDate.prop("disabled", true);
    wms.composedData.endTime.prop("disabled", true);
}

wms.composedData.addfilter 
= function(event)
{
    var answer = "";
    if(event !== 'undefined' &&
        event.data !== 'undefined' &&
        event.data.param2 !== 'undefined')
    {
        answer = event.data.param2;
    }
    __doPostBack('composedDetailsAddFilterButton', '{filterValue:' + answer + '}');
}

wms.composedData.cancelAddFilter
= function () { }

wms.composedData.clearFilter
= function (event) {
    var answer = "";
    if (event !== 'undefined' &&
        event.data !== 'undefined' &&
        event.data.param2 !== 'undefined') {
        answer = event.data.param2;
    }
    __doPostBack('composedDetailsClearFilterButton', 'clearFilter');
}

wms.composedData.cancelClearFilter
= function () { }

wms.composedData.initialize
= function () {
    wms.composedData.addFilterDialogButton = $('#composedDetailsAddFilterButton');
    wms.composedData.clearFilterDialogButton = $('#composedDetailsClearFilterButton');
    wms.composedData.showDetailedButton = $('#composedDetailsDetailedButton');
    wms.composedData.showSummaryButton = $('#composedDetailsSummaryButton');
    wms.composedData.detailedDiv = $('#composedDetailsDetailedDiv');
    wms.composedData.summaryDiv = $('#composedDetailsSummaryDiv');
    wms.composedData.jobNumberRadio = $('#composedJobNumberRadio');
    wms.composedData.jobNumber = $('[jqname="ComposedJobNumberTextbox"]');
    wms.composedData.accountRadio = $('#composedAccountSearchRadio');
    wms.composedData.accountNumber = $('[jqname="ComposedAccountSearchTextbox"]');
    wms.composedData.workflowGroupRadio = $('#composedWorkflowGroupSearchRadio');
    wms.composedData.workflowGroup = $('[jqname="ComposedWorkflowGroupDropDown"]');
    wms.composedData.dateRangeRadio = $('#composedDateRangeSearchRadio');
    wms.composedData.searchButton = $('#composedSearchButton');
    wms.composedData.startDate = $('[jqname="ComposedStartDateTextbox"]');
    wms.composedData.startTime = $('[jqname="ComposedStartTimeTextbox"]');
    wms.composedData.endDate = $('[jqname="ComposedEndDateTextbox"]');
    wms.composedData.endTime = $('[jqname="ComposedEndTimeTextbox"]');

    wms.composedData.disableInputControls();

    if (wms.composedData.accountRadio.prop('checked')) {
        wms.composedData.accountNumber.prop("disabled", false);
    }
    if (wms.composedData.jobNumberRadio.prop('checked')) {
        wms.composedData.jobNumber.prop("disabled", false);
    }
    if (wms.composedData.workflowGroupRadio.prop('checked')) {
        wms.composedData.workflowGroup.prop("disabled", false);
        wms.composedData.startDate.prop("disabled", false);
        wms.composedData.startTime.prop("disabled", false);
        wms.composedData.endDate.prop("disabled", false);
        wms.composedData.endTime.prop("disabled", false);
    }
    if (wms.composedData.dateRangeRadio.prop('checked')) {
        wms.composedData.startDate.prop("disabled", false);
        wms.composedData.startTime.prop("disabled", false);
        wms.composedData.endDate.prop("disabled", false);
        wms.composedData.endTime.prop("disabled", false);
    }

    wms.composedData.jobNumber.focus(function () {
        wms.composedData.disableInputControls();
        wms.composedData.jobNumber.prop("disabled", false);
        wms.composedData.jobNumber.val('');
        wms.composedData.jobNumberRadio.prop('checked', true);
    }).keypress(function (e) {
        if (e.keyCode == 13) {
            wms.composedData.searchButton.click();
            return false;
        }
    });
    wms.composedData.jobNumberRadio.change(function () {
        wms.composedData.disableInputControls();
        wms.composedData.jobNumber.prop("disabled", false);
        wms.composedData.jobNumber.val('');
    });

    wms.composedData.accountNumber.focus(function () {
        wms.composedData.disableInputControls();
        wms.composedData.accountNumber.prop("disabled", false);
        wms.composedData.accountNumber.val('');
        wms.composedData.accountRadio.prop('checked', true);
    }).keypress(function (e) {
        if (e.keyCode == 13) {
            wms.composedData.searchButton.click();
            return false;
        }
    });
    wms.composedData.accountRadio.change(function () {
        wms.composedData.disableInputControls();
        wms.composedData.accountNumber.prop("disabled", false);
        wms.composedData.accountNumber.val('');
    });

    wms.composedData.workflowGroup.focus(function () {
        wms.composedData.disableInputControls();
        wms.composedData.workflowGroup.prop("disabled", false);
        wms.composedData.startDate.prop("disabled", false);
        wms.composedData.startTime.prop("disabled", false);
        wms.composedData.endDate.prop("disabled", false);
        wms.composedData.endTime.prop("disabled", false);
        wms.composedData.workflowGroup.val('');
        wms.composedData.workflowGroupRadio.prop('checked', true);
    }).keypress(function (e) {
        if (e.keyCode == 13) {
            wms.composedData.searchButton.click();
            return false;
        }
    });
    wms.composedData.workflowGroupRadio.change(function () {
        wms.composedData.disableInputControls();
        wms.composedData.workflowGroup.prop("disabled", false);
        wms.composedData.startDate.prop("disabled", false);
        wms.composedData.startTime.prop("disabled", false);
        wms.composedData.endDate.prop("disabled", false);
        wms.composedData.endTime.prop("disabled", false);
        wms.composedData.workflowGroup.val('');
    });

    wms.composedData.dateRangeRadio.change(function () {
        wms.composedData.disableInputControls();
        wms.composedData.startDate.prop("disabled", false);
        wms.composedData.startTime.prop("disabled", false);
        wms.composedData.endDate.prop("disabled", false);
        wms.composedData.endTime.prop("disabled", false);
    });

    wms.composedData.showDetailedButton.click(function () {
        wms.composedData.detailedDiv.show();
        wms.composedData.summaryDiv.hide();
    });
    wms.composedData.showSummaryButton.click(function () {
        wms.composedData.detailedDiv.hide();
        wms.composedData.summaryDiv.show();
    });
    wms.composedData.addFilterDialogButton.click(function () {
        wms.launchModalDialog(
            wms.composedData.showFilterDialogButton,
            "Filter Composed Details",
            "Enter an account number or address.",
            wms.composedData.addfilter,
            wms.composedData.cancelAddFilter,
            wms.ModalButtons.OkCancel,
            false,
            true
            );
    });
    wms.composedData.clearFilterDialogButton.click(function () {
        wms.launchModalDialog(
            wms.composedData.showFilterDialogButton,
            "Clear Composed Details Filters",
            "This will remove all current filters. Do you wish to continue?",
            wms.composedData.clearFilter,
            wms.composedData.cancelClearFilter,
            wms.ModalButtons.OkCancel,
            true,
            true
            );
    });
}

wms.convertedData.onReady
= function ()
{
    wms.convertedData.initialize();
    wms.composedData.initialize();
}
