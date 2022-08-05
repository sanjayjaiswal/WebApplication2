if (typeof wms === 'undefined')
    var wms = new Object;
if (typeof wms.trackingsets === 'undefined')
    wms.trackingsets = new Object();

wms.trackingsets.onTrackingSetSelect = function (event, ui) {
    var rawUrl = location.href.split('?');
    location.href = rawUrl[0] + "?id=" + ui.item.id;
};
wms.trackingsets.onReady = function () {
    $('.tracking-set').data('on-select', wms.trackingsets.onTrackingSetSelect);
};
$(document).ready(wms.trackingsets.onReady);