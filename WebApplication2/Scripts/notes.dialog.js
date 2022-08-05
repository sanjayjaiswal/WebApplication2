if (typeof wms === "undefined")
    wms = new Object();
if (typeof wms.notes === "undefined")
    wms.notes = new Object();

wms.notes.recordId = -1;

wms.notes.initialize
= function ()
{
    wms.notes.dialog = $('#notesDialog');
    wms.notes.text = $('#noteText');
    wms.notes.addButton = $('#addNoteButton');
    wms.notes.cancelButton = $('#cancelNoteButton');
    wms.notes.grid = $('#notesGrid');
    wms.notes.page = $('#pageTitleHidden').val();
    wms.notes.cancelButton.click(wms.notes.cancel);
    wms.notes.addButton.click(wms.notes.save);
    wms.notes.text.onChanged
    = function (e)
    {
        if (e.keyCode == 13) {
            wms.notes.save();
            return false;
        }
    }
    wms.notes.text.bind('keydown', wms.notes.text.onChanged);
}



wms.notes.show
= function(recordId)
{
    wms.notes.recordId = recordId;
    var url = "NoteService.asmx/GetAll";
    var inputData = "{'pageTitle':'" + wms.notes.page + "', 'recordId':'" + wms.notes.recordId + "'}";
    wms.handleCallback(url, inputData, wms.notes.show.done, null, null, true);
}

wms.notes.show.done
= function(data)
{
    wms.notes.grid.empty();
    if (data.d.length == 0)
        $('<span>No notes exist for this record.</span>').appendTo(wms.notes.grid);
    else
    {
        for (var i = 0; i < data.d.length; i++) {
            var note = data.d[i];
            $('<span>' + note.EntryText + '</span><br/>').appendTo(wms.notes.grid);
            $('<span>Entered By:' + note.EnteredBy + '</span><br/>').appendTo(wms.notes.grid);
            var date = new Date(parseInt(note.EnteredOn.substr(6)));
            $('<span>Entered On:' + $.datepicker.formatDate('M dd yy', date) + '</span><br/>').appendTo(wms.notes.grid);
            $('<hr/>').appendTo(wms.notes.grid);
        }
    }
    wms.notes.dialog.show();
}

wms.notes.hide
= function ()
{
    wms.notes.dialog.hide();
}

wms.notes.cancel
= function()
{
    wms.notes.recordId = -1;
    wms.notes.text.val('');
    wms.notes.hide();
}

wms.notes.save
= function ()
{
    var url = "NoteService.asmx/AddNote";
    var inputData = "{'pageTitle':'" + wms.notes.page + "', 'recordId':'" + wms.notes.recordId + "', 'noteText':'" + wms.notes.text.val() + "'}";
    wms.handleCallback(url, inputData, wms.notes.save.done, null, null, true);
    wms.notes.text.val('');
}

wms.notes.save.done
= function (data)
{
    var note = data.d;
    var date = new Date(parseInt(note.EnteredOn.substr(6)));
    $('<hr/>').prependTo(wms.notes.grid);
    $('<span>Entered On:' + $.datepicker.formatDate('M dd yy', date) + '</span><br/>').prependTo(wms.notes.grid);
    $('<span>Entered By:' + note.EnteredBy + '</span><br/>').prependTo(wms.notes.grid);
    $('<span>' + note.EntryText + '</span><br/>').prependTo(wms.notes.grid);
}

wms.notes.onReady
= function ()
{
    wms.notes.initialize();
}