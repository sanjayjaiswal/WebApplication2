if (typeof (wms) === "undefined")
    wms = new Object;
if (typeof (wms.colorModel) === "undefined")
    wms.colorModel = new Object();

wms.colorModel.displayButton = $('.colorModelConfigurationButton');
wms.colorModel.dialog = $('.colorModelConfigurationDialog');
wms.colorModel.dialog.add = wms.colorModel.dialog.find('.add');
wms.colorModel.dialog.save = wms.colorModel.dialog.find('.save');
wms.colorModel.dialog.cancel = wms.colorModel.dialog.find('.cancel');

wms.colorModel.onReady = function()
{
    wms.colorModel.dialog.hide();
    wms.colorModel.displayButton.click(function () {
        wms.colorModel.dialog.show();
    });
}