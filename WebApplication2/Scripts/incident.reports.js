if (typeof wms === 'undefined')
    wms = new Object();
if (typeof wms.incidentreports === 'undefined')
    wms.incidentreports = new Object();


wms.incidentreports.onReady = function () {
}

// Page Load Event
$(document).ready(wms.incidentreports.onReady);