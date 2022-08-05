if (typeof wms === 'undefined')
    wms = new Object();
if (typeof wms.fileReleaseWizard === 'undefined')
    wms.fileReleaseWizard = new Object();


wms.fileReleaseWizard.initializeControls
= function()
{
    wms.fileReleaseWizard.isTest = $('#IsTest').val().toLowerCase() == 'true' ? true : false;
    wms.fileReleaseWizard.printToPdf = $('[jqname="PrintToPdf"').find('input:checkbox');
    wms.fileReleaseWizard.returnToRep = $('[jqname="ReturnToRep"').find('input:checkbox');
    wms.fileReleaseWizard.releaseForSamples = $('[jqname="ReleaseForSamples"').find('input:checkbox');
    wms.fileReleaseWizard.deleteBilling = $('[jqname="DeleteBilling"]').find('input:checkbox');
    wms.fileReleaseWizard.deleteReason = $('[jqname="DeleteReason"]');
    wms.fileReleaseWizard.useSampleLogic = $('[jqname="UseSampleLogic"]').find('input:checkbox');
    wms.fileReleaseWizard.batchOnly = $('[jqname="BatchOnly"]').find('input:checkbox');


    if (wms.fileReleaseWizard.printToPdf.is(':checked'))
    {
        wms.fileReleaseWizard.returnToRep.prop('checked', false).prop('disabled', true);
        wms.fileReleaseWizard.releaseForSamples.prop('checked', false).prop('disabled', true);
        wms.fileReleaseWizard.batchOnly.prop('checked', false).prop('disabled', true);
        wms.fileReleaseWizard.useSampleLogic.prop('checked', false).prop('disabled', true);
    }
    if (wms.fileReleaseWizard.useSampleLogic.is(":checked") ||
        wms.fileReleaseWizard.batchOnly.is(":checked")) {
        wms.fileReleaseWizard.returnToRep.prop('checked', true).prop('disabled', true);
    }
    if (wms.fileReleaseWizard.isTest) {
        wms.fileReleaseWizard.releaseForSamples.prop('disabled', true);
        wms.fileReleaseWizard.returnToRep.prop('disabled', true);
    }

    wms.fileReleaseWizard.batchOnly.click(function () {
        if (wms.fileReleaseWizard.batchOnly.is(':checked')) {
            wms.fileReleaseWizard.returnToRep.prop('checked', true);
            wms.fileReleaseWizard.returnToRep.prop('disabled', true);
        }
    });

    wms.fileReleaseWizard.useSampleLogic.click(function () {
        if (wms.fileReleaseWizard.useSampleLogic.is(':checked')) {
            wms.fileReleaseWizard.returnToRep.prop('checked', true);
            wms.fileReleaseWizard.returnToRep.prop('disabled', true);
        }
    });

    wms.fileReleaseWizard.printToPdf.click(function () {
        if (wms.fileReleaseWizard.printToPdf.is(':checked'))
        {
            wms.fileReleaseWizard.returnToRep.prop('checked', false).prop('disabled', true);
            wms.fileReleaseWizard.releaseForSamples.prop('checked', false).prop('disabled', true);
            wms.fileReleaseWizard.batchOnly.prop('checked', false).prop('disabled', true);
            wms.fileReleaseWizard.useSampleLogic.prop('checked', false).prop('disabled', true);
        }
        else
        {
            wms.fileReleaseWizard.returnToRep.prop('disabled', false);
            wms.fileReleaseWizard.batchOnly.prop('disabled', false);
            wms.fileReleaseWizard.useSampleLogic.prop('disabled', false);
            if (wms.fileReleaseWizard.isTest)
            {
                wms.fileReleaseWizard.releaseForSamples.prop('checked', true).prop('disabled', true);
                wms.fileReleaseWizard.returnToRep.prop('checked', true).prop('disabled', true);
            }
        }
    });
    wms.fileReleaseWizard.releaseForSamples.click(function () {
        wms.fileReleaseWizard.returnToRep.prop(
            'disabled',
            wms.fileReleaseWizard.releaseForSamples.is(':checked'));
        if (wms.fileReleaseWizard.releaseForSamples.is(':checked'))
            wms.fileReleaseWizard.returnToRep.prop('checked', true);
    });

    wms.fileReleaseWizard.deleteBilling.click(function () {
        wms.fileReleaseWizard.deleteReason.prop(
            'disabled',
            !wms.fileReleaseWizard.deleteBilling.is(':checked'));
    });
    wms.fileReleaseWizard.deleteReason.prop(
        'disabled',
        !wms.fileReleaseWizard.deleteBilling.is(':checked'));
}

wms.fileReleaseWizard.onReady
= function()
{
    wms.fileReleaseWizard.initializeControls();
}
