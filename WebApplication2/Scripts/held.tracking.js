if (typeof wms === 'undefined')
	wms = new Object();
if (typeof wms.heldTracking === 'undefined')
	wms.heldTracking = new Object();
if (typeof wms.webGrid === 'undefined')
	wms.webGrid = new Object();

wms.heldTracking.reasonIndex = 1;
wms.heldTracking.workflowStepIndex = 7;


// Create a custom context menu for held tracking gui.
/*wms.webGrid.configureContextMenu =
function () {
	$('.webGrid tr').bind("contextmenu", function (e) {
		var isStepHoldPointB;
		var elem, evt = e ? e : event;
		if (evt.srcElement) elem = evt.srcElement;
		else if (evt.target) elem = evt.target;
		var grid = $(elem).closest('table');
		var primaryKeyIndex = grid.data('pk');
		var primaryKey = '';
		if (typeof primaryKeyIndex !== 'undefined')
			primaryKey = $($(elem).parent().find("td")[primaryKeyIndex]).text();
		var jobIdIndex = grid.data('job-id-key');
		var jobId = '';
		if (typeof jobIdIndex !== 'undefined')
			jobId = $($(elem).parent().find("td")[jobIdIndex]).text();
		var trackingIdIndex = grid.data('tracking-id-key');
		var trackingId = '';
		if (typeof trackingIdIndex !== 'undefined')
			trackingId = $($(elem).parent().find("td")[trackingIdIndex]).text();
		var hpbIndicatorIndex = grid.data('hbp-indicator-key');
		var hpbIndicator = '';
		if (typeof hpbIndicatorIndex !== 'undefined')
		hpbIndicator = $($(elem).parent().find("td")[hpbIndicatorIndex]).text();
		var reason = '';
		reason = $($(elem).parent().find("td")[wms.heldTracking.reasonIndex]).text();
		var workflowStep = '';
		workflowStep = $($(elem).parent().find("td")[wms.heldTracking.workflowStepIndex]).text();


		if (typeof primaryKey !== 'undefined') {
			if (elem.tagName.toLowerCase() == "tr") {
				elem = $(elem).find("td")[primaryKey];
				id = $(elem).text();
			}
			if (elem.tagName.toLowerCase() == "td") {
				if (typeof hpbIndicator === 'undefined')
					isStepHoldPointB = false;
				else
					isStepHoldPointB = hpbIndicator.toLowerCase().indexOf('hpb') != -1;
				elem = $(elem).parent().find("td")[primaryKey];
				id = $(elem).text();
			}
			if (id == -1)
				return;
		}

		$(".contextMenu")
			.css({ "top": (e.pageY - 15) + "px", "left": Math.min(e.pageX - 5, e.pageX - ((document.body.offsetWidth / 2) - 580)) + "px" })
			.bind('mouseleave', function () {
				$('.contextMenu').hide();
			})
			.show();
		$(".contextMenu .releaseFile").unbind();
		$(".contextMenu .createPull").unbind();
		$(".contextMenu .reconvert").unbind();

		$(".contextMenu .releaseFile").bind('click', function () {
			if (!isStepHoldPointB)
			{
				wms.modalDialog.launch(trackingId, "Are you sure", "This will release job id " + trackingId + ". Are you sure?", wms.webGrid.confirmReleaseFile, wms.webGrid.cancel, wms.modalDialog.buttons.OkayCancel, true);
			}
			else
			{
				__doPostBack(grid.attr('id'), 'release$hpb$' + trackingId);
			}
			$('.contextMenu').hide();
		});
		$(".contextMenu .createPull").bind('click', function () {
			$('#pullDialog').show();
			$('.contextMenu').hide();
		});
		$(".contextMenu .reconvert").bind('click', function () {
			wms.modalDialog.launch(trackingId, "Are you sure", "This will reconvert tracking id " + trackingId + ". Are you sure?", wms.webGrid.confirmReconvertFile, wms.webGrid.cancel, wms.modalDialog.buttons.OkayCancel, true);
			$('.contextMenu').hide();
		});
		e.preventDefault();
	});
}*/