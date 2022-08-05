if (typeof wms === 'undefined')
    var wms = new Object;
if (typeof wms.jobaccountdetails === 'undefined')
    wms.jobaccountdetails = new Object();

wms.jobaccountdetails.customerSelectionScript = function (event, ui) {
    var rawUrl = location.href.split('?');
    location.href = rawUrl[0] + "?jobid=" + ui.item.id;
};
wms.jobaccountdetails.onReady = function () {
    $('.job-numbers').data('on-select', wms.jobaccountdetails.customerSelectionScript);
};
$(document).ready(wms.jobaccountdetails.onReady);