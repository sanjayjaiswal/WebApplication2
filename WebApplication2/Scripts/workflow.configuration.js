// Create namepace for wms configuration.
if (typeof wms === 'undefined')
    var wms = new Object;
if (typeof wms.workflow === 'undefined')
    wms.workflow = new Object();
wms.workflow.configuration = new Object;

// Enumeration for supported workflow steps.  Other returned values (if any) will "fall through".
wms.workflow.configuration.WorkflowStep = { PreProcessing: 2, PreConversion: 8, PostConversion: 32, PreBatch: 128 };
wms.workflow.configuration.newTaskProcessorCount = 0;
wms.workflow.configuration.queries = {};
wms.workflow.configuration.workflowConfigId = -1;
wms.workflow.configuration.isUserControl = false;

wms.workflow.configuration.populateCurrentTasks = function (id) {
    var jqxhr = $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        url: "workflowconfigurationService.asmx/GetTasksByWorkflowGroupId",
        data: "{'id':'" + id + "'}"
    }).done(function (data, textStatus, jqXHR) {
        var response = JSON.parse(data.d);
        var workflowGroupTaskList = JSON.parse(response.task);
        var workflowGroup = JSON.parse(response.group);
        wms.workflow.configuration.setHoldPoints(workflowGroup);
        for (var index in workflowGroupTaskList) {
            if (workflowGroupTaskList[index].WorkflowStep == wms.workflow.configuration.WorkflowStep.PreProcessing) {
                wms.workflow.configuration.attachListItem(
                    workflowGroupTaskList[index],
                    false,
                    ".pre-processing-dropcontainer");
            } else if (workflowGroupTaskList[index].WorkflowStep == wms.workflow.configuration.WorkflowStep.PreConversion) {
                wms.workflow.configuration.attachListItem(
                    workflowGroupTaskList[index],
                    false,
                    ".pre-conversion-dropcontainer");
            } else if (workflowGroupTaskList[index].WorkflowStep == wms.workflow.configuration.WorkflowStep.PostConversion) {
                wms.workflow.configuration.attachListItem(
                    workflowGroupTaskList[index],
                    false,
                    ".post-conversion-dropcontainer");
            } else if (workflowGroupTaskList[index].WorkflowStep == wms.workflow.configuration.WorkflowStep.PreBatch) {
                wms.workflow.configuration.attachListItem(
                    workflowGroupTaskList[index],
                    false,
                    ".pre-batch-dropcontainer");
            }
        }
        wms.workflow.configuration.updateOlStartValues();
    });
}

wms.workflow.configuration.attachListItem = function (task, isNew, parentClassName) {
    var li = "<li class=\"configurable \"></li>";
    var warning = "";
    if (isNew) {
        warning = " <span style=\"color:red;cursor:pointer;\" title=\"Warning: Task will not be added to database until the workflow is saved!\" >*</span>";
    }
    $(li).html(task.ProcessorName + warning).attr('taskid', task.Id)
        .attr('task', JSON.stringify(task)).attr("isnew", isNew)
        .bind("contextmenu", function (e) {
            var location = { "top": (e.pageY - 15) + "px", "left": (e.pageX - 200) + "px" };
            if (wms.workflow.configuration.isUserControl) {
                location = { "top": (e.pageY - 240) + "px", "left": (e.pageX - 400) + "px" };
            }
            $("#contextMenu").css(location)
                .bind('mouseleave', function () {
                    $('#contextMenu').hide();
                })
                .show();
            $("#contextMenu .config").unbind();
            $("#contextMenu .remove").unbind();
            $("#contextMenu .cancel").unbind();
            $("#contextMenu .config").bind('click', function () {
                wms.workflow.configuration.launchConfigurationDialog(task, isNew);
                $('#contextMenu').hide();
            });
            $("#contextMenu .remove").bind('click', function () {
                wms.workflow.configuration.removeTask(task, isNew, parentClassName);
                $('#contextMenu').hide();
            });
            $("#contextMenu .cancel").bind('click', function () {
                $('#contextMenu').hide();
            });
            e.preventDefault();
        })
        .appendTo($(parentClassName).find(".workflowTasksDynamic"));
}

wms.workflow.configuration.removeTask = function (task, isNew, parentClassName, confirmed) {
    if (confirmed == true) {
        var element = $(parentClassName).find("ol").find("[taskid='" + task.Id + "']");
        if (!isNew) {
            var onDone = function (data, textStatus, jqXHR) {
                element.remove();
                wms.workflow.configuration.updateOlStartValues();
            };
            var onFail = function (jqXHR, textStatus) {
                console.info("Failed Status: " + textStatus + "\rText: " + jqXHR.responseText);
                alert("A problem occurred removing this task.  See console for more information.");
            };

            //DeleteTaskConfiguration(int workflowGroupId, int taskid)
            wms.handleCallback("workflowconfigurationService.asmx/DeleteTaskConfiguration",
                { "workflowGroupId": task.WorkflowGroupId, "taskid": task.Id },
                onDone,
                onFail,
                null,
                false);
        }
        else {
            element.remove();
            wms.workflow.configuration.updateOlStartValues();
        }
    }
    else {
        wms.launchModalDialog($(this), "Are you sure?",
            "This action will permanently remove this task.  Are you sure?",
            function () { wms.workflow.configuration.removeTask(task, isNew, parentClassName, true); },
            function () { },
            wms.ModalButtons.YesNo, true);
    }
}

wms.workflow.configuration.textValidate = function (event, control, validationString) {
    var charCode = (typeof event.which == "undefined") ? event.keyCode : event.which;
    var charStr = String.fromCharCode(charCode);
    if (validationString == "integer") {
        if (/\D/.test(charStr)) {
            event.preventDefault();
            return false;
        }
    }
    if (validationString == "float") {
        if (/\D/.test(charStr) && !/\x2E/.test(charStr)) {
            event.preventDefault();
            return false;
        }
        if (/\x2E/.test(charStr) && /\x2E/.test(control.value)) {
            event.preventDefault();
            return false;
        }
    }
}

wms.workflow.configuration.launchConfigurationDialog = function (task, isNew) {
    var onDone = function (data, textStatus, jqXHR) {
        $("#configDialog .dataGridBody").empty();
        console.info(JSON.stringify(data));
        var result = JSON.parse(data.d);
        console.info(JSON.stringify(result));
        for (var key in result) {
            console.info("Key: " + result[key].Key + ", Value:" + result[key].Value);
            if (result.hasOwnProperty(key)) {
                //alert("Key: " + key + "\nValue: " + result[key].Value + "\nType: " + result[key].SettingType);
                if (result[key].Value.SettingType == "S") {
                    $("#configDialog .dataGridBody").append("<tr><td class=\"setting\">" + result[key].Key + "</td><td><input type='text' class=\"value\" data-setting-type='S' name='" + result[key].Key + "Textbox' value='" + result[key].Value.Value + "'/></td></tr>");
                }
                if (result[key].Value.SettingType == "I") {
                    $("#configDialog .dataGridBody").append("<tr><td class=\"setting\">" + result[key].Key + "</td><td><input type='text' class=\"value\" data-setting-type='I' onkeypress=\"textValidate(event, this, 'integer')\" name='" + result[key].Key + "Textbox' value='" + result[key].Value.Value + "'/></td></tr>");
                }
                if (result[key].Value.SettingType == "F") {
                    $("#configDialog .dataGridBody").append("<tr><td class=\"setting\">" + result[key].Key + "</td><td><input type='text' class=\"value\" data-setting-type='F' onkeypress=\"textValidate(event, this, 'float')\" name='" + result[key].Key + "Textbox' value='" + result[key].Value.Value + "'/></td></tr>");
                }
                if (result[key].Value.SettingType == "B") {
                    if (result[key].Value.Value == "true") {
                        $("#configDialog .dataGridBody").append("<tr><td class=\"setting\">" + result[key].Key + "</td><td><input type='checkbox' class=\"value\" data-setting-type='B' id='" + result[key].Key + "CheckBox' name='" + result[key].Key + "CheckBox' checked='checked' value='" + result[key].Value.Value + "'/><label for='" + result[key].Key + "CheckBox'></label> </td></tr>");
                    }
                    else {
                        $("#configDialog .dataGridBody").append("<tr><td class=\"setting\">" + result[key].Key + "</td><td><input type='checkbox' class=\"value\" data-setting-type='B' id='" + result[key].Key + "CheckBox'name='" + result[key].Key + "CheckBox' value='" + result[key].Value.Value + "'/><label for='" + result[key].Key + "CheckBox'></label></td></tr>");
                    }
                }
                if (result[key].Value.SettingType == "D") {
                    $("#configDialog .dataGridBody").append("<tr><td class=\"setting\">" + result[key].Key + "</td><td><input type='text' class=\"value date\" data-setting-type='D' readonly=\"true\" name='" + result[key].Key + "Textbox' value='" + result[key].Value.Value + "'/></td></tr>");
                }
            }
        }

        $(".value.date").datepicker({

        });

        $("#configDialog").find("#programName").text(task.ProcessorName);
        $("#configDialog").find(".updateButton").unbind("click").bind("click", function () {
            wms.workflow.configuration.updateConfiguration(task, isNew);
            $("#configDialog").hide();
        });
        $("#configDialog").find(".cancelButton").unbind("click").bind("click", function () {
            $("#configDialog").hide();
        });
        $("#configDialog").show();
    };

    var onFail = function (jqXHR, textStatus) {
        console.info("Failed Status: " + textStatus + "\rText: " + jqXHR.responseText);
        alert("error");
    };
    if (isNew) {
        var settings = $('[taskid="' + task.Id + '"]').attr('settings');
        if (settings == null) {
            wms.handleCallback("workflowconfigurationService.asmx/GetTaskConfiguration",
                "{'taskid':'" + task.Id + "', 'processorid':'" + task.TaskProcessorId + "'}",
                onDone,
                onFail);
        }
        else {
            var result = new Object();
            var settingsObj = jQuery.parseJSON(settings);
            result.d = JSON.stringify(settingsObj.settings.Values);
            console.info("taskid=" + task.Id + ", data = " + result);
            onDone(result);
        }
    }
    else {
        wms.handleCallback("workflowconfigurationService.asmx/GetTaskConfiguration",
            "{'taskid':'" + task.Id + "', 'processorid':'" + task.TaskProcessorId + "'}",
            onDone,
            onFail);
    }
}

wms.workflow.configuration.updateConfiguration = function (task, isNew) {
    var rows = $("#configDialog .dataGridBody").find("tr");
    var inputData = new Object();
    inputData.settings = new Object();
    inputData.settings.Id = task.Id;
    inputData.settings.Values = new Array();
    rows.each(function (index) {
        var control = $(this).find(".value");
        var settingType = control.data('setting-type');
        console.info($(this).find(".value").val());
        var value = $(this).find(".value").val();
        if (settingType == 'B') {
            value = 'false';
            console.info(control.is(':checked'));
            if (control.is(':checked')) {
                value = 'true';
            }
        }
        var settingId = $(this).find(".setting").text();
        var taskSetting = { "Value": value, "SettingType": settingType };
        inputData.settings.Values.push({ "Key": settingId, "Value": taskSetting });
    });
    console.info(JSON.stringify(inputData));

    var onDone = function (data, textStatus, jqXHR) {
    };
    var onFail = function (jqXHR, textStatus) {
        console.info("Failed Status: " + textStatus + "\rText: " + jqXHR.responseText);
    };
    if (!isNew) {
        wms.handleCallback("workflowconfigurationService.asmx/UpdateWorkflowTaskSettings",
            inputData,
            onDone,
            onFail,
            null,
            false);
    }
    else {
        $('[taskid="' + task.Id + '"]').attr('settings', JSON.stringify(inputData));
    }
}

wms.workflow.configuration.getListContainerByStep = function (stepid) {
    switch (stepid) {
        case wms.workflow.configuration.WorkflowStep.PreProcessing:
            return $(".pre-processing-dropcontainer");
            break;
        case wms.workflow.configuration.WorkflowStep.PreConversion:
            return $(".pre-conversion-dropcontainer");
            break;
        case wms.workflow.configuration.WorkflowStep.PostConversion:
            return $(".post-conversion-dropcontainer");
            break;
        case wms.workflow.configuration.WorkflowStep.PreBatch:
            return $(".pre-batch-dropcontainer");
            break;
    }
    // Whoah, how'd that happen?
    return null;
}

wms.workflow.configuration.handleDropEvent = function (event, ui, stepid, parentClassName) {
    var text = ui.draggable.text();
    var id = ui.draggable.data("id");
    var task = new Object();
    wms.workflow.configuration.newTaskProcessorCount++;
    task.Id = -wms.workflow.configuration.newTaskProcessorCount;
    task.ProcessorName = $.trim(text);
    task.WorkflowGroupId = wms.workflow.configuration.workflowConfigId;
    task.WorkflowStep = stepid;
    task.CanMove = true;
    task.StepSequence = -1;
    task.TaskProcessorId = id;
    task.HoldAfterExecute = false;
    task.HoldReason = "";

    wms.workflow.configuration.attachListItem(task, true, parentClassName);
    wms.workflow.configuration.updateOlStartValues();
};

wms.workflow.configuration.configureDragAndDrop = function () {

    $('.any, .PreProcessing, .PreConversion, .PostConversion, .PreBatch').draggable({
        revert: true
    });

    $(".pre-processing-dropcontainer").droppable({
        accept: ".any, .PreProcessing",
        hoverClass: "ui-state-hover",
        activeClass: "ui-state-active",
        drop: function (event, ui) {
            wms.workflow.configuration.handleDropEvent(event, ui, wms.workflow.configuration.WorkflowStep.PreProcessing, ".pre-processing-dropcontainer");
        }
    })
        .find(".workflowTasksDynamic").sortable({
            items: "li",
            sort: function () {
            }
        });

    $(".pre-conversion-dropcontainer").droppable({
        accept: ".any, .PreConversion",
        hoverClass: "ui-state-hover",
        activeClass: "ui-state-active",
        drop: function (event, ui) {
            wms.workflow.configuration.handleDropEvent(event, ui, wms.workflow.configuration.WorkflowStep.PreConversion, ".pre-conversion-dropcontainer");
        }
    })
        .find(".workflowTasksDynamic").sortable({
            items: "li",
            sort: function () {
            }
        });

    $(".post-conversion-dropcontainer").droppable({
        accept: ".any, .PostConversion",
        hoverClass: "ui-state-hover",
        activeClass: "ui-state-active",
        drop: function (event, ui) {
            wms.workflow.configuration.handleDropEvent(event, ui, wms.workflow.configuration.WorkflowStep.PostConversion, ".post-conversion-dropcontainer");
        }
    })
        .find(".workflowTasksDynamic").sortable({
            items: "li",
            sort: function () {
            }
        });

    $(".pre-batch-dropcontainer").droppable({
        accept: ".any, .PreBatch",
        hoverClass: "ui-state-hover",
        activeClass: "ui-state-active",
        drop: function (event, ui) {
            wms.workflow.configuration.handleDropEvent(event, ui, wms.workflow.configuration.WorkflowStep.PreBatch, ".pre-batch-dropcontainer");
        }
    })
        .find(".workflowTasksDynamic").sortable({
            items: "li",
            sort: function () {
            }
        });

}

wms.workflow.configuration.cancelConfiguration = function () {
    if (wms.workflow.configuration.workflowConfigId === -1) {
        // No workflow config, just hide the junk.
        $('.workflowConfigurationContainer').hide();
        return;
    }
    $(".ui-sortable").empty();
    wms.workflow.configuration.populateCurrentTasks(wms.workflow.configuration.workflowConfigId);
    $('.workflowConfigurationContainer').hide();
}

wms.workflow.configuration.saveConfiguration = function (confirmed) {
    if (wms.workflow.configuration.workflowConfigId === -1) return;
    if (confirmed == true) {
        var stepSequence = 0;
        var preProcessingContainer = $(".pre-processing-dropcontainer");
        var preConversionContainer = $(".pre-conversion-dropcontainer");
        var postConversionContainer = $(".post-conversion-dropcontainer");
        var preBatchContainer = $(".pre-batch-dropcontainer");

        var tasks = new Array();

        stepSequence = wms.workflow.configuration.pushTasks(tasks, preProcessingContainer, stepSequence);
        stepSequence = wms.workflow.configuration.pushTasks(tasks, preConversionContainer, stepSequence);
        stepSequence = wms.workflow.configuration.pushTasks(tasks, postConversionContainer, stepSequence);
        stepSequence = wms.workflow.configuration.pushTasks(tasks, preBatchContainer, stepSequence);

        var holdPoints = wms.workflow.configuration.getHoldPoints();
        console.info(JSON.stringify(holdPoints));

        var onDone = function (data, textStatus, jqXHR) {
            $(".ui-sortable").empty();
            wms.workflow.configuration.populateCurrentTasks(wms.workflow.configuration.workflowConfigId);
        };
        var onFail = function (jqXHR, textStatus) {
            console.info("Failed Status: " + textStatus + "\rText: " + jqXHR.responseText);
        };

        wms.handleCallback("workflowconfigurationService.asmx/UpdateWorkflowTask",
            { "tasks": tasks, "holdPoints": holdPoints },
            onDone,
            onFail,
            null,
            false);
    }
    else {
        wms.launchModalDialog($(this), "Are you sure?",
            "This permanently will update the workflow group.  Are you sure?",
            function () { wms.workflow.configuration.saveConfiguration(true); },
            function () { },
            wms.ModalButtons.YesNo, true);
    }
}

wms.workflow.configuration.setHoldPoints = function (group) {
    var holdPoints = new Array();
    var holdPointAChk = $('.holdA>input[type=checkbox]');
    var holdPointBChk = $('.holdB>input[type=checkbox]');
    if (group.EnableHoldPointA) {
        holdPointAChk.prop('checked', 'checked');
        holdPointAChk.attr('reason', group.HoldPointAReason);
    }
    else {
        holdPointAChk.prop('checked', false);
    }
    if (group.EnableHoldPointB) {
        holdPointBChk.prop('checked', 'checked');
        holdPointBChk.attr('reason', group.HoldPointBReason);
    }
    else {
        holdPointBChk.prop('checked', false);
    }
    return holdPoints;
}

wms.workflow.configuration.getHoldPoints = function () {
    var holdPoints = new Array();
    var holdPointAChk = $('.holdA>input[type=checkbox]');
    var holdPointBChk = $('.holdB>input[type=checkbox]');
    var holdPointA = new Object();
    var holdPointB = new Object();
    holdPointA.Name = 'A';
    holdPointA.Enabled = holdPointAChk.is(':checked');
    holdPointA.Reason = holdPointAChk.attr('reason');
    holdPoints.push(holdPointA);
    holdPointB.Name = 'B';
    holdPointB.Enabled = holdPointBChk.is(':checked');
    holdPointB.Reason = holdPointBChk.attr('reason');
    holdPoints.push(holdPointB);
    return holdPoints;
}

wms.workflow.configuration.pushTasks = function (tasks, container, stepSequence) {
    var listItems = container.find("ol.workflowTasksDynamic > li");
    $(listItems).each(function (index) {
        var workflowGroupTaskConfig = new Object();
        var isNew = $(listItems[index]).attr('isnew');
        var task = $.parseJSON($(listItems[index]).attr("task"));
        workflowGroupTaskConfig.Id = task.Id;
        workflowGroupTaskConfig.WorkflowStep = task.WorkflowStep;
        workflowGroupTaskConfig.TaskProcessorId = task.TaskProcessorId;
        workflowGroupTaskConfig.ProcessorName = task.ProcessorName;
        workflowGroupTaskConfig.WorkflowGroupId = task.WorkflowGroupId;
        workflowGroupTaskConfig.CanMove = task.CanMove;
        workflowGroupTaskConfig.HoldAfterExecute = task.HoldAfterExecute;
        workflowGroupTaskConfig.HoldReason = task.HoldReason;
        stepSequence += 1;
        workflowGroupTaskConfig.StepSequence = stepSequence;
        if (isNew && $(listItems[index]).attr('settings') != null) {
            workflowGroupTaskConfig.Settings = new Array();
            var settings = jQuery.parseJSON($(listItems[index]).attr('settings')).settings.Values;
            $(settings).each(function (settingIndex) {
                workflowGroupTaskConfig.Settings.push(settings[settingIndex]);
            });
        }
        tasks.push(workflowGroupTaskConfig);
    });

    return stepSequence;
}

wms.workflow.configuration.configureSaveAndCancel = function () {
    $('#SaveCancelDiv>#saveButton').click(wms.workflow.configuration.saveConfiguration);
    $('#SaveCancelDiv>#cancelButton').click(wms.workflow.configuration.cancelConfiguration);
}

wms.workflow.configuration.enableHoldPoint = function (event) {
    var control = event.data.param1;
    var reason = $('#modal').find('#answer').val();
    console.info(control, reason);
    $(control).attr('reason', reason);
}

wms.workflow.configuration.cancelHoldPoint = function (event) {
    var control = event.data.param1;
    control.checked = false;
}

wms.workflow.configuration.configureModalDialog = function () {
    var dialog = $('#modal');
    if (wms.workflow.configuration.isUserControl) {
        dialog.css('left', "250px");
        dialog.css('top', "150px");
    }
    dialog.hide();
    $('.holdA>input[type=checkbox], .holdB>input[type=checkbox]').on('change', function () {
        if ($(this).is(':checked')) {
            wms.launchModalDialog(this, "Hold point Reason", "Enter a reason for enabling this hold point.", wms.workflow.configuration.enableHoldPoint, wms.workflow.configuration.cancelHoldPoint);
        }
    });
}

wms.workflow.configuration.configureContextMenuForStaticItems = function () {
    $('.workflowStepStatic>li').bind("contextmenu", function (e) {
        var task = JSON.parse($(this).attr('task'));
        var isNew = false;
        var location = { "top": (e.pageY - 15) + "px", "left": (e.pageX - 200) + "px" };
        if (wms.workflow.configuration.isUserControl) {
            location = { "top": (e.pageY - 240) + "px", "left": (e.pageX - 400) + "px" };
        }
        console.info(location);
        $("#contextMenu .remove").hide();
        $("#contextMenu").css(location)
            .bind('mouseleave', function () {
                $('#contextMenu').hide();
                $("#contextMenu .remove").show();
            })
            .show();
        $("#contextMenu .config").unbind();
        $("#contextMenu .remove").unbind();
        $("#contextMenu .cancel").unbind();
        $("#contextMenu .config").bind('click', function () {
            wms.workflow.configuration.launchConfigurationDialog(task, isNew);
            $('#contextMenu').hide();
            $("#contextMenu .remove").show();
        });
        $("#contextMenu .cancel").bind('click', function () {
            $('#contextMenu').hide();
            $("#contextMenu .remove").show();
        });
        e.preventDefault();
    });
}

wms.workflow.configuration.updateOlStartValues = function () {
    var lists = $.find('ol');
    for (var ol in lists) {
        if (ol == 0) {
            $(lists[ol]).attr('start', 1);
        }
        else {
            console.info($(lists[ol - 1]).children().length);
            $(lists[ol]).attr('start', parseInt($(lists[ol - 1]).attr('start')) + $(lists[ol - 1]).children().length);
        }
        wms.workflow.configuration.lastOl = $(lists[ol]);
    }
}

wms.workflow.configuration.showWorkflowConfiguration = function () {
    $('.workflowConfigurationContainer').show();
}

wms.workflow.configuration.configureRecordIdChanged = function () {
    var recordId = $('[jqname="tbRecordId"]');
    wms.workflow.configuration.workflowConfigId = recordId.val();
    recordId.bind('onchange', function () { wms.workflow.configuration.workflowConfigId = recordId.val(); });
}

wms.workflow.configuration.onReady = function () {
    // If there's no query string, this is a user control.
    wms.workflow.configuration.isUserControl = (document.location.search.indexOf('&') === -1);
    if (wms.workflow.configuration.isUserControl) {
        wms.workflow.configuration.configureRecordIdChanged();
        $('[name="editWorkflowConfiguration"]').bind('click', function () { wms.workflow.configuration.showWorkflowConfiguration(); });
        $('.progressContainer.workflowConfiguration').hide();
    }
    else {
        $.each(document.location.search.substr(1).split('&'), function (c, q) { var i = q.split('='); wms.workflow.configuration.queries[i[0].toString()] = i[1].toString(); });
        if (wms.workflow.configuration.queries['workflowgroupid'] != null) {
            wms.workflow.configuration.workflowConfigId = wms.workflow.configuration.queries['workflowgroupid'];
        }
    }
    wms.workflow.configuration.populateCurrentTasks(wms.workflow.configuration.workflowConfigId);
    wms.workflow.configuration.configureDragAndDrop();
    wms.workflow.configuration.configureSaveAndCancel();
    wms.workflow.configuration.configureModalDialog();
    wms.workflow.configuration.configureContextMenuForStaticItems();
    $('.progressContainer.workflowConfiguration').hide();
}

// Page Load Event
//$(document).ready(wms.workflow.configuration.onReady);