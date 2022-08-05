// Create namepace for wms configuration.
if (typeof wms === 'undefined')
    var wms = new Object;
if (typeof wms.workflow === 'undefined')
    wms.workflow = new Object();
wms.workflow.group = new Object;

wms.workflow.group.hideOverridesUI = function() {
    var addPanel = $('.addNewOverrideContainer');
    addPanel.show();
}

wms.workflow.group.initializeOverrideUI = function()
{
    var addButton = $('[name="addOverride"]');
    addButton.bind('click', function () { wms.workflow.group.hideOverridesUI(); });
}

wms.workflow.group.onReady = function() {
    wms.workflow.group.initializeOverrideUI();
}

// Page Load Event
//$(document).ready(wms.workflow.group.onReady);