<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeBehind="JobProcessing.aspx.cs" Inherits="WMS.Gui.app.JobProcessingPage" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="server">
    <link rel="stylesheet" href="Content/bootstrap.css" />

    <style>
        .PoupTable > tbody > tr > td {
            padding: 4px 6px;
        }

        .txtwatermark {
            color: gray;
        }

            .txtwatermark:focus {
                color: black;
            }

        #cphContent_txtSelectCompletionDate {
            width: 150px
        }

        #cphContent_txtTime {
            width: 150px
        }

        #cphContent_txtNote {
            width: 95.5%;
            height: 60px;
        }
    </style>
    <script type="text/javascript">

        function SetFlagToReturnViewGui(flag) {
            $("#<%= hdnReturnToViewFlag.ClientID %>").val(flag);
        }

        function GVcheckAll() {
            $("input[id*='chkbox']").prop('checked', true)
        }

        function GVUncheckAll() {
            $("input[id*='chkbox']").prop('checked', false)
        }

        function Show() {

            var arrStatus = $("input[id*='txtstatus']").val();
            var co = 0, status, co1 = 0, i = 0, flag = 0, StatusFlag = 0;
            $("[id*=gvDpJobs] input[type=checkbox]:checked").each(function () {
                var row = $(this).closest("tr")[0];
                // var n = arrStatus.includes(row.cells[12].innerHTML);
                var n = arrStatus.indexOf(row.cells[12].innerHTML);
                if (n > -1) {

                    //if (i != 0) {
                    if (co == 0) {
                        status = row.cells[12].innerHTML;
                        co = 1;
                    }
                    if (status != row.cells[12].innerHTML && co1 == 0) {

                        window.ShowMessage("Please select the same status for selecting multiple jobs.")
                        co1 = 1;
                        StatusFlag = 1;
                        return false;
                    }
                    // }
                    i = i + 1;
                    flag = 1;
                }
                else {
                    window.ShowMessage("Please select the right status (" + arrStatus + ") for selecting jobs.")

                    StatusFlag = 1;
                    flag = 1;
                    return false;
                }
            });
            if (flag == 0) { window.ShowMessage("Please select at least one job with same status."); return false; }
            if (StatusFlag == 1) { return false; }
        }
        function DisplayErrorMsg(message) {
            $('#<%=spnlErrorMessage.ClientID%>').text(decodeURIComponent(message));
        }
        function ClearErrorMsg() { $('#<%=spnlErrorMessage.ClientID%>').text(""); }
    </script>
    <style type="text/css">
        .checkboxsize {
            position: relative;
            padding-top: 5px;
            padding-left: 20px;
            cursor: pointer;
            font-size: 9pt;
            /* color: #7CACEE; */
            color: #313131;
        }

        .warningAdditionAction {
            background-color: #dddddd;
            color: white;
            border-style: ridge;
            border-width: 2px;
            border-color: #555555;
            width: 700px;
            height: 360px;
            border-radius: 10px;
            font-size: small;
            z-index: 999999;
        }

        #cphContent_gvDpJobs th {
            position: sticky !important;
            top: -1px;
        }

        .slavhis {
            font-size: 1.0em;
        }

        .wordwrap {
            white-space: pre-wrap;
            white-space: -moz-pre-wrap;
            white-space: -o-pre-wrap;
            word-wrap: break-word;
        }

        #cphContent_gvDetails th {
            position: sticky !important;
            top: -1px;
        }
    </style>
    <script src="https://code.jquery.com/jquery-3.1.1.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript">  
        function AlertBox(msgtitle, message, controlToFocus) {
            $("#msgDialogAlert").dialog({
                autoOpen: false,
                modal: true,
                title: msgtitle,
                closeOnEscape: true,
                buttons: [{
                    text: "Ok",
                    click: function () {
                        $(this).dialog("close");
                        if (controlToFocus != null)
                            controlToFocus.focus();
                    }
                }],
                close: function () {
                    $("#operationMsgAlert").html("");
                    if (controlToFocus != null)
                        controlToFocus.focus();
                },
                show: { effect: "clip", duration: 500 }
            });
            $("#operationMsgAlert").html(message);
            $("#msgDialogAlert").dialog("open");
        };
    </script>

    <script type="text/javascript">

        function Check_Click(objRef) {
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                row.style.backgroundColor = "#D6EAF8";
            }
            else {
                if (row.rowIndex % 2 == 0) {
                    row.style.backgroundColor = "white";
                }

                else {
                    row.style.backgroundColor = "white";
                }
            }
        }


        function Check_Click1(objRef) {

            var arrStatus = $("input[id*='txtstatus']").val();/* ["Waiting to Print", "Waiting to Insert"];*/

            var co = 0, status, co1 = 0;

            $("[id*=gvDpJobs] input[type=checkbox]:checked").each(function () {
                var row = $(this).closest("tr")[0];
                // var n = arrStatus.includes(row.cells[12].innerHTML);
                var n = arrStatus.indexOf(row.cells[12].innerHTML);
                if (n > -1) {
                    if (co == 0) {
                        status = row.cells[12].innerHTML;
                        co = 1;
                    }
                    if (status != row.cells[12].innerHTML && co1 == 0) {
                        window.ShowMessage("Please select the same status for selecting multiple jobs.")

                        var row = objRef.parentNode.parentNode;
                        objRef.checked = false;
                        co1 = 1;
                        row.style.backgroundColor = "white";
                        return false;
                    }
                    else { row.style.backgroundColor = "#D6EAF8"; }
                }
                else {
                    window.ShowMessage("Please select the right status (" + arrStatus + ") for selecting jobs.")

                    var row = objRef.parentNode.parentNode;
                    objRef.checked = false;
                    co1 = 1;
                    row.style.backgroundColor = "white";
                    return false;
                }
            });

            var row = objRef.parentNode.parentNode;


        }

    </script>
    <script>

        $(function () {

            var localStorageGet = {}
            localStorageGet.location = null;
            localStorageGet.status = null;
            localStorageGet.eqId = null;
            localStorageGet.showCmp = null;

            var obj = localStorage.getItem(localStorage.key('managerPreferences'));
            if (obj != null) {
                var parsedStorage = JSON.parse(obj);
                localStorageGet.location = parsedStorage['location'];
                localStorageGet.status = parsedStorage['status'];
                localStorageGet.eqId = parsedStorage['eqId'];
                localStorageGet.eqId = parsedStorage['eqId'];
                localStorageGet.showCmp = parsedStorage['showCmp'];

            }

            if (localStorageGet.location != null) {
                $("#<%=ddProcessingLocation.ClientID%>").val(localStorageGet.location);
                document.getElementById('<%=hfLocation.ClientID%>').value = localStorageGet.location.toString();
            }
            if (localStorageGet.status != null) {
                $("#<%=ddStatus.ClientID%>")[0].selectedIndex = localStorageGet.status;
                document.getElementById('<%=hfStatus.ClientID%>').value = localStorageGet.status.toString();
            }
            if (localStorage.getItem("showCmp") != null) {
                if (localStorage.getItem("showCmp") == "true")
                    $('#<%= cbIncludeCompletedJobs.ClientID %>').attr('checked', true);
                else
                    $('#<%= cbIncludeCompletedJobs.ClientID %>').attr('checked', false);
            }

            if (localStorageGet.eqId != null) {

                var path = "../app/JobProcessing.aspx";
                $.ajax({
                    type: "POST",
                    url: path + '/RetrieveLocalStorage',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(localStorageGet),
                    success: function (response) {
                        var mySelect = $('#<%=ddEquipmentId.ClientID%>');
                        $.each(JSON.parse(response.d), function (index, obj) {
                            $.each(obj, function (val, text) {
                                mySelect.append($('<option></option>').val(text).html(text));
                            });
                        });
                        var count = 0;
                        $($('#<%=ddEquipmentId.ClientID%>').children()).each(
                            function () {
                                if ($(this).attr('value') == localStorageGet.eqId) {
                                    $("#<%=ddEquipmentId.ClientID%>")[0].selectedIndex = count;
                                    document.getElementById('<%=hfEquipmentId.ClientID%>').value = localStorageGet.eqId;
                                }
                                else
                                    count++;
                            }
                        )
                    },
                    fail: function () {

                    }
                })
            }
        });


        function setHtmlData() {
            var localStorageData = {};
            localStorageData.location = $('[id$=ddProcessingLocation] option:selected').val();
            localStorageData.status = $('[id$=ddStatus] option:selected').val();
            localStorageData.eqId = $('[id$=ddEquipmentId] option:selected').text();
            localStorageData.showCmp = $('#<%= cbIncludeCompletedJobs.ClientID %>').is(':checked');
            console.log(localStorageData);
            localStorage.setItem("managerPreferences", JSON.stringify(localStorageData));
        }

        function clearData() {
            localStorage.clear();
            document.getElementById('<%=hfEquipmentId.ClientID%>').value = "";
            return false;
        }


        function checkAll() {
            $("input[id*='cbDataSelected']").prop('checked', true)
        }

        function unCheckAll() {
            $("input[id*='cbDataSelected']").prop('checked', false)
        }


        function checkDate() {

            var beginDateStr = $("input[name*='txtBeginningDate']").val().split("/");
            var endDateStr = $("input[name*='txtEndingDate']").val().split("/");
            var str = beginDateStr[2] + "-" + beginDateStr[0] + "-" + beginDateStr[1];
            var beginDate = new Date(str);
            str = endDateStr[2] + "-" + endDateStr[0] + "-" + endDateStr[1];
            var endDate = new Date(str);
            if (endDate < beginDate) {
                window.ShowMessage("Invalid Date");
                return false;
            }
            return true;
        }

        $(document).ready(function () {
            console.log('test message');
            $(document).oncontextmenu = function () {
                return false;
            }

        });

        var launch = false;
        var launch2 = false;
        var launch3 = false;

        function launchModal() {
            launch = true;
        }

        function launchModal2() {
            launch2 = true;
        }

        function launchModal3() {
            launch3 = true
        }

        function pageLoad() {
            if (launch) {
                var obj = $find("ModalJobStatusSplit");
                if (obj != null || obj != "undefined")
                    $find("ModalJobStatusSplit").show();
            }
            if (launch2) {
                var obj = $find("ModalJobStatusSplit");
                if (obj != null || obj != "undefined")
                    $find("ModalJobStatusSplit").show();
            }
            if (launch3) {
                var obj = $find("actionMenuControl");
                if (obj != null || obj != "undefined")
                    $find("actionMenuControl").show();
            }
        }

        function CompareDates() {


        }
        function changeDateText(checkbox) {
            if (checkbox.checked) {
                document.getElementById('<%=bdate.ClientID%>').innerText = 'Completed On Beginning Date:';
                document.getElementById('<%=edate.ClientID%>').innerText = 'Completed On Ending Date:';
            }
            else {
                document.getElementById('<%=bdate.ClientID%>').innerText = 'Scheduled Beginning Date:';
                document.getElementById('<%=edate.ClientID%>').innerText = 'Scheduled Ending Date:';
            }
        }

        function enableTimer() {
            var timer = $find('<%= ctlTimer.ClientID %>');
            timer._startTimer();
        }

        function disableTimer() {
            // document.getElementById('<%=ctlTimer.ClientID%>')._stopTimer();
            //$find('#ctlTimer')._stopTimer();
            var timer = $find('<%= ctlTimer.ClientID %>');
            timer._stopTimer();
        }
        function GetSelectedTextValue(ddPriority) {
            var selectedText = ddPriority.options[ddPriority.selectedIndex].innerHTML;
            var selectedValue = ddPriority.value;
            document.getElementById('<%=laPriority.ClientID%>').innerText = 'Updating Current Priority  to ' + selectedValue;

        }
        function Changed(txtCompletedOnDate) {
            var newDate = txtCompletedOnDate.value;
            document.getElementById('<%=laCompletedDateOnChange.ClientID%>').innerText = 'Updating Completed On Date to ' + newDate;
        }

        function customOpen(url) {
            var w = window.open(url, '', 'width=1000,height=600,toolbar=0,status=0,location=0,menubar=0,directories=0,resizable=1,scrollbars=1');
            w.focus();

        }

        function changeFilterOption(option) {
            if (option == 3 || option == 21) {
                var lastDate = new Date();
                var sevenDaysOld = new Date(lastDate.getTime() - (7 * 24 * 60 * 60 * 1000));
                document.getElementById('<%=txtBeginningDate.ClientID%>').value = [sevenDaysOld.getMonth() + 1, sevenDaysOld.getDate(), sevenDaysOld.getFullYear()].join('/');
                document.getElementById('<%=txtEndingDate.ClientID%>').value = [lastDate.getMonth() + 1, lastDate.getDate(), lastDate.getFullYear()].join('/');
            }
            else {
                document.getElementById('<%=txtBeginningDate.ClientID%>').value = "";
                document.getElementById('<%=txtEndingDate.ClientID%>').value = "";
            }
        }

        function GetWarningMessage() {
            $("#<%=hdnDuplicateWarnMessage.ClientID%>").val($("#contextMenuDuplicateWarning").text());
        }

        $(document).on('mousedown', '.baseTable tr', function (e) {

            if (e.which == 1) {
                document.getElementById('<%=hfLeftClick.ClientID%>').value = "Y";
                return true;
            }
            else {
                document.getElementById('<%=hfLeftClick.ClientID%>').value = "N";
                return true;
            }
        });

        function checkCompletionDate() {
            if ($("#cphContent_txtSelectCompletionDate").val().trim() != "" && $("#cphContent_txtNote").val().trim() != "") {
                $("#cphContent_btnJobCompletionsApplyStatus").prop('disabled', false);
            }
            else {
                $("#cphContent_btnJobCompletionsApplyStatus").prop('disabled', true);
            }
        }

        function checkCompletionNote() {

            if ($("#cphContent_txtSelectCompletionDate").val().trim() != "" && $("#cphContent_txtNote").val().trim() != "") {
                $("#cphContent_btnJobCompletionsApplyStatus").prop('disabled', false);
            }
            else {
                $("#cphContent_btnJobCompletionsApplyStatus").prop('disabled', true);
            }
        }

        function isNumber(evt) {
            return false;
        }

        function isNumberTime(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 47 || charCode > 58)) {
                return false;
            }
            return true;
        }

        function showDateValidationMessage(msg) {

            var message = msg;
            $("#dialog").dialog({
                modal: true,
                open: function () {
                    var markup = '<div style="visibility: visible;color: red;text-align:center; min-height: 40px;"><h5>' + message + '</h5></div>';
                    $(this).html(markup);
                },
                title: 'Message',
                zIndex: 10000,
                autoOpen: true,
                width: 'auto',
                resizable: false,
                close: function () {
                    if (bAccepts == false) {
                        wms.progressContainer.hide();
                        $(this).dialog("close");
                        if (destroyed == false)
                            $(this).dialog("destroy");
                        $("#dialog").hide();

                        return false;
                    }
                },
                buttons: {
                    "Ok": function cacnel() {
                        bAccepts = false;
                        destroyed = true;
                        wms.progressContainer.hide();
                        $(this).dialog("close");
                        $(this).dialog("destroy");
                        $("#dialog").hide();
                        return false;
                    }
                }
            });

            $("#dialog").dialog("open");


        }

    </script>

    <style>
        .searchWidth {
            width: 80px
        }

        @media screen and (-ms-high-contrast: active), (-ms-high-contrast: none) {
            .searchWidth {
                width: 0px
            }

            #cphContent_txtSelectCompletionDate {
                width: 150px
            }

            #cphContent_txtTime {
                width: 150px
            }

            #cphContent_txtNote {
                width: 94%;
                height: 60px;
            }
        }
    </style>
    <style type="text/css">
        .spacer {
            width: 20px;
        }

        .lblSearchFields {
            width: 140px;
        }


        #cphContent_TechnicalDashboardNotificationLogs_gvNotificationLogViewer tbody tr td:nth-child(4n+4) {
            text-overflow: ellipsis !important;
            white-space: nowrap;
            overflow: hidden;
            max-width: 120Px;
        }

        .ui-dialog {
            position: fixed;
            top: 50%;
            left: 50%;
            width: auto;
            height: auto;
            -webkit-transform: translate(-50%,-50%);
            -moz-transform: translate(-50%,-50%);
            -ms-transform: translate(-50%,-50%);
            -o-transform: translate(-50%,-50%);
            transform: translate(-50%,-50%);
            min-width: 700px;
        }
    </style>

    <div id="msgDialogAlert" style="display: none; text-align: left; vertical-align: central;">
        <p id="operationMsgAlert"></p>
    </div>
     <div id="dialog" title="Confirmation Required" style="visibility: hidden" class="confirmDia">
        </div>
    <%--  <asp:Button Text="Click!" runat="server" OnClientClick="return ShowMessage('hi');" />  --%>


    <h2>Job Processing Dashboard <strong></strong></h2>
    <asp:panel id="PanelEQMessage" runat="server" cssclass="warningMsgJobRouting" style="display: none; height: 152px; width: 340px">
        
        <div style="align-self: center; padding-bottom: 20px; color:black ; padding-top:35px" align="center">
          <b style="color:red">Error- No Equipment Selected</b><br />
            Please select the appropriate <br />
            Equipment to complete this process. <br />
        </div>
        <div align="center">

            <asp:Button ID="btnEquipmentok" runat="server" Text="OK" style="border-radius: 5px; width: 100px; height: 25px; border-style: solid; border-width: 1px;" OnClick="btnEquipmentok_Click"></asp:Button>
        </div>

        <ajax:modalpopupextender id="ModalpopupexPanelEQMessage" runat="server"
            popupcontrolid="PanelEQMessage"
            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
            targetcontrolid="HiddenField12">
                    </ajax:modalpopupextender>
        <asp:HiddenField ID="HiddenField12" runat="server" />
    </asp:panel>
    <div>
        <asp:textbox id="txtstatus" runat="server" text="<%$appSettings:StatusName %>" style="display: none" />
    </div>
    <asp:panel runat="server" defaultbutton="ExecuteSearch">
        <div>
            <asp:HiddenField ID="hfEquipmentId" runat="server" OnValueChanged ="setDdEquipmentId"/>
            <%--<asp:HiddenField ID="hfClientName" runat="server" OnValueChanged ="setClientNameId"/>--%>
            <asp:HiddenField ID="hfLocation" runat="server" />
            <asp:HiddenField ID="hfStatus" runat="server" />
            <asp:HiddenField ID="hfOparator" runat="server" />
            <asp:HiddenField ID="hfLeftClick" runat="server" />
               <fieldset id="searchControls" style="float: right;  width: 1140px; border: 1px solid black; margin-right: 8px;">
                    <table style="margin-top:5px;">
                       <%-- <tr>
                            <td>Search By:</td>
                        </tr>--%>
                         <tr>                      
                         <td style="width: 170px;"">Client: </td>
                        <td>
                            <asp:DropDownList ID="ddlClient" Style="width: 160px;" AutoPostBack="true" runat="server"><asp:ListItem Value="168"></asp:ListItem></asp:DropDownList>
                        </td>
                       <%-- <td class="spacer" />--%>
                        <td class="spacer" />
                        
                        <td><asp:CheckBox id="chkSmalljobs" runat="server"  Checked="true" text="Small Jobs" ></asp:CheckBox></td>
                        <td> <asp:CheckBox id ="chkLargeJobs" runat="server"  Checked="true" text="Large Jobs"></asp:CheckBox></td>
                       <td class="spacer" />
                      <td colspan="2">Job Size Threshold =</td>
                     <td>
                         <asp:TextBox id="txtJobSizeThreshold" runat="server" text="35" style="width:70px;text-align:right;background-color:white;border:1px solid"></asp:TextBox></td>
                        <td> 
                           <div style=" width:100px"> <div style="width:30px; float:left"><asp:ImageButton ID="InFlightQueue_icon" runat="server" ImageUrl="~/Images/InFlightQueue.jpg" Height="25px" Width="30px"  Style="float: right;" ToolTip="View Splits in Container Transfer Queue" OnClick="InFlightQueue_icon_Click"/></div>
                            <div style="width:30px; float:left"> <asp:ImageButton ID="ExportToExcel_icon" runat="server" ImageUrl="~/Images/excel.png" Height="25px" Width="25px" Style="float: right; " OnClick="btnExportToExcel_Click" /></div>
                             <div style="width:30px; float:left"><asp:ImageButton ID="Refresh_icon" runat="server" ImageUrl="~/Images/refresh_small.png" Style="float: right; margin-left: 5px" OnClick="Refresh_icon_Click" /></div>
                           </div>
                        </td>
                    </tr>
                    <%--<tr style="height:35px"><td class="spacer" /></tr>--%>
                    <tr>
                        <td>Processing Location: </td>
                        <td>
                            <asp:DropDownList ID="ddProcessingLocation" AutoPostBack="true" Style="width: 160px;" OnSelectedIndexChanged="setDdEquipmentId" runat="server" />
                        </td>
                        <td style="width:62px;" />
                        <td>Job Number: </td>
                        <td>
                            <asp:TextBox ID="txtJobNumber" runat="server" />
                        </td>
                        <td style="width:62px;">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                ControlToValidate="txtJobNumber"
                                ValidationExpression="\d+"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Please enter positive numbers"
                                ForeColor="Red"
                                Font-Size="8"
                                runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="labTotalSheets" runat="server" Text="Total Sheets"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labStatus1" runat="server" Text=""/>
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtTotalSheets" runat="server" style="width:70px;text-align:right;background-color:white;border:1px solid" ReadOnly="True"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ExecuteSearch" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>Status: </td>
                        <td style="padding-top: 10px">
                       
                            <asp:DropDownList ID="ddStatus" AutoPostBack="true" Style="width: 160px;" OnSelectedIndexChanged="setDdEquipmentId" onchange="changeFilterOption(this.value);"  runat="server" />
                        </td>
                        <td class="spacer"></td>
                        <td>
                            <asp:Label id="bdate" AutoPostBack="true" runat="server" Text="Scheduled Beginning Date: "></asp:Label>
                        </td>
                        <td style="padding-top: 10px;"><%-- <div class='input-group date' id='datetimepicker2'>--%>
                            <div>
                                <%--  <input type='text' class="form-control" />--%>
                                <asp:TextBox ID="txtBeginningDate" runat="server"></asp:TextBox>
                                <ajax:calendarextender id="calBeginDate" runat="server" targetcontrolid="txtBeginningDate" />
                                <%-- <span class="input-group-addon" style="width:14%">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>--%>
                            </div>
                        </td>
                        <td>
                            <asp:CompareValidator
                                ID="CompareValidator1" runat="server"
                                Type="Date"
                                Operator="DataTypeCheck"
                                ControlToValidate="txtBeginningDate"
                                ErrorMessage="Please enter a valid date."
                                ForeColor="Red"
                                Font-Size="8">
                            </asp:CompareValidator>
                        </td>
                        <td>
                            <asp:Label ID="labTotalSplitQty" runat="server" Text="Total Split Qty"></asp:Label>
                        </td>
                        <td style="min-width:61px">
                            <asp:Label ID="labStatus2" runat="server" Text=""/>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtTotalSplitQty" runat="server" style="width:70px;text-align:right;background-color:white;border:1px solid" ReadOnly="True"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ExecuteSearch" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 170px;">Equipment ID: </td>
                        <td style="padding-top: 10px;">
       
                            <asp:DropDownList ID="ddEquipmentId" AutoPostBack="true"  Style="width: 160px;" runat="server" OnSelectedIndexChanged="ddEquipmentId_SelectedIndexChanged" />
                              
                           
                        </td>
                        <td class="spacer"></td>
                        <td>
                            <asp:Label ID="edate" runat="server" Text="Scheduled Ending Date: "></asp:Label>
                        </td>
                        <td style="padding-top: 10px;"><%--<div class='input-group date' id='datetimepicker3'>--%>
                            <div>
                                <asp:TextBox ID="txtEndingDate" runat="server" />
                                <ajax:calendarextender runat="server" id="calEndDate" targetcontrolid="txtEndingDate" />
                                <%-- <span class="input-group-addon" style="width:14%">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>--%>
                            </div>
                        </td>
                        <td>
                            <asp:CompareValidator
                                ID="dateValidator" runat="server"
                                Type="Date"
                                Operator="DataTypeCheck"
                                ControlToValidate="txtEndingDate"
                                ErrorMessage="Please enter a valid date."
                                ForeColor="Red"
                                Font-Size="8">
                            </asp:CompareValidator>
                        </td>
                        <td style="width:80px">
                            <asp:Label ID="labNumberOfSplits" runat="server" Text="Number of Splits"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labStatus3" runat="server" Text=""/>
                        </td>
                        <td>
                           <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtNumberOfSplits" runat="server" style="width:70px;text-align:right;background-color:white;border:1px solid" ReadOnly="True"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ExecuteSearch" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td> Production Jobs <asp:CheckBox ID="cbProductionJobs" runat="server" Text="&#32;" Checked="true"/></td>
                        <td> Test Jobs <asp:CheckBox ID="cbTestJobs" runat="server" Text="&#32;" Checked="false"/></td>                        
                        <td class="searchWidth">
                            <asp:ImageButton ID="ResetSearch" runat="server" ImageUrl="~/Images/cancel.png"  OnClick="ResetSearch_Click" />
                            <span style="margin-left: 10px; display: inline-block;">
                                  <asp:UpdatePanel ID="UpdatePanelExecuteSearch" runat="server">
                                <ContentTemplate>
                                <asp:ImageButton ID="ExecuteSearch" runat="server" ImageUrl="~/Images/find.png" OnClick="ExecuteSearch_Click" Height="29px" /></ContentTemplate>
                                      <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ExecuteSearch" EventName="Click" />
                                </Triggers>
                                  </asp:UpdatePanel>
                            </span></td>
                        <td style="width:160px"><span style="margin-left: 5px; ">Completed Jobs Only ?  <asp:CheckBox ID="cbIncludeCompletedJobs" runat="server" Text="&#32;" Checked="false" onclick="changeDateText(this);" OnCheckedChanged="cbIncludeCompletedJobs_CheckedChanged"/></span></td>
                     
                        <td style="overflow:inherit!important; width:450px" colspan="2"><asp:Button ID="btnSaveLocalSetting" Style="font-size:x-small;" runat="server" Text="Save Local Setting" OnClick="btnSaveLocalSetting_Clicked"/>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnRetrieveLocalSetting" Style="font-size:x-small;" runat="server" Text="Retrieve Local Setting" OnClick="btnRetrieveLocalSetting_Clicked"/>
                           <%-- <asp:Button ID="btnProcessingStepContinue" runat="server" Text="Continue" style="float: left; margin-left: 10%;border-radius:5px;width:105px;height:25px;border-style:solid;border-width:1px;" OnClick="btnProcessingStepContinue_Click"  ></asp:Button>--%>
                            <%--<asp:Button ID="btnResetLocalSetting" Style="font-size:x-small;" runat="server" Text="Reset Local Setting" OnClientClick="return clearData();" OnClick="btnResetLocalSetting_Clicked" />--%>
                        </td>
                    
                        <td colspan="3">Filter by Job Attribute  <asp:CheckBox ID="cbFilterByAttributes" Text="&#32;" Checked="false" runat="server">
                            </asp:CheckBox></td>
                     
                    </tr>
                </table>
                 <asp:Panel ID="ProcessingStep" runat="server" CssClass="warningMsgJobRouting" Style="display:none;height:140px;width:340px">
                    <div id="contextProcessingStep">
                        <div style="align-self: center">
                            <p style="color: black;font-size:medium" align="center">
                                Please Select Processing Step and Equipment </p>                                                                                                                                                                                                    
                        </div>
                    </div>
                    <div style="align-self: center;padding-bottom:20px;">                        
                      <asp:DropDownList runat="server" id="ddlProcessingStep" style="margin-left: 11px; width:316px; height:31px;"></asp:DropDownList>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnProcessingStepContinue" runat="server" Text="Continue" style="float: left; margin-left: 10%;border-radius:5px;width:105px;height:25px;border-style:solid;border-width:1px;" OnClick="btnProcessingStepContinue_Click"  ></asp:Button>
                        <asp:Button ID="btnProcessingStepCancel" runat="server" Text="Cancel" style="float: right; margin-right: 10%;border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" OnClick="btnProcessingStepCancel_Click" ></asp:Button>
                    </div>
                   
                   <ajax:modalpopupextender id="ModalpopuProcessingStep" runat="server"
                            popupcontrolid="ProcessingStep"
                          dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="HiddenField8">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="HiddenField8" runat="server" />
                </asp:Panel>

                   <asp:Panel ID="pnlMessage" runat="server" CssClass="warningMsgJobRouting" Style="display:none;height:140px;width:340px">
                    <div id="contextMessage">
                        <div style="align-self: center">
                            <p style="color: black;font-size:medium" align="center">
                                Message</p>                                                                                                                                                                                                    
                        </div>
                    </div>
                    <div style="align-self: center;padding-bottom:20px;" align="center">                        
                     <asp:Label runat="server" id="lblMessage" Text="Record Saved successfully."  Style="color:black;"></asp:Label>

                                          
                    </div>
                     <div align="center">
                        
                        <asp:Button ID="btnMessageCancel" runat="server" Text="Ok" style="border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" OnClick="btnMessageCancel_Click" ></asp:Button>
                    </div>
                   
                   <ajax:modalpopupextender id="ModalpopuppnlMessage" runat="server"
                            popupcontrolid="pnlMessage"
                          dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="HiddenField10">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="HiddenField10" runat="server" />
                </asp:Panel>

                   <asp:Panel ID="PanelErrorMessage" runat="server" CssClass="warningMsgJobRouting" Style="display:none;height:140px;width:340px">
                    <div id="contexterrorMessage">
                        <div style="align-self: center">
                            <p style="color: black;font-size:medium" align="center">
                                Message</p>                                                                                                                                                                                                    
                        </div>
                    </div>
                    <div style="align-self: center;padding-bottom:20px;" align="center">                        
                      <asp:Label runat="server" id="lblErrorMessage" Text="Record failed to saved."  Style="color:black;"></asp:Label>                         
                    </div>
                     <div align="center">
                        
                        <asp:Button ID="btnErrorMessageCancel" runat="server" Text="Ok" style="border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" OnClick="btnErrorMessageCancel_Click" ></asp:Button>
                    </div>
                   
                   <ajax:modalpopupextender id="ModalpopupexPanelErrorMessage" runat="server"
                            popupcontrolid="PanelErrorMessage"
                          dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="HiddenField11">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="HiddenField11" runat="server" />
                </asp:Panel>

            </fieldset>
        </div>
        <asp:UpdatePanel ID="upGvControls" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvDpJobs" EventName="SelectedIndexChanged" />
            </Triggers>
            <ContentTemplate>
                <fieldset id="jobHeader" style="background-color:rgba(230,230,230,1);margin-top:2px;margin-bottom:2px;width:1140px;margin-right:8px;float:right; border: 1px solid black;">
                <table style="width:100%; border:thin; border-color:red">
                       <tr>
                           <td rowspan="2">Job<br />Attributes</td>
                           <td style="text-align:center">
                               <asp:Label ID="labInputPaperModulesJob" runat="server" Text=""></asp:Label>
                           </td>
                           <td style="text-align:center">
                               <asp:Label ID="labTonerTypeJob" runat="server" Text=""></asp:Label>

                           </td>
                           <td style="text-align:center"> 
                               <asp:Label ID="labPrintEngineNgOrSheetCodeJob" runat="server" Text=""></asp:Label>

                           </td>
                           <td rowspan="2"> 
                               <asp:Button runat="server" ID="btnSelectAll" Text="Select All" OnClientClick="GVcheckAll()"  Style="height:27px;width:100px;font-size:x-small; margin-top: 4px"/>
                           </td>
                           <td rowspan="2"> 
                               <asp:Button ID="btnUnSelectAll" runat="server" OnClientClick="GVUncheckAll()"  Style="height:27px;width:100px;font-size:x-small;margin-top: 4px"   Text="Un-Select All" />
                           </td>
                            <td rowspan="2" style="text-align:baseline"> 
                                <asp:Button ID="btnMoveAll" runat="server" OnClientClick="return Show()"  onClick="btnMoveAll_Click" Style="height:27px;width:100px;font-size:x-small;margin-top: 4px" Text="Move All" />
                            </td>
                           <td rowspan="2">Equipment<br />Attributes</td>
                           <td style="text-align:center">
                               <asp:Label ID="labEqInputPaperModules" runat="server"  Text=""></asp:Label>

                           </td>
                           <td style="text-align:center">
                               <asp:Label ID="labEqTonerTypeOrSheetCode" runat="server"  Text=""></asp:Label></td>
                           <td style="text-align:center">
                               <asp:Label ID="labEqPrintEngineOrCategory" runat="server"  Text=""></asp:Label></td>
                       </tr>
                       <tr>
                           <%--<td></td>--%>
                           <td>
                               <asp:DropDownList ID="ddPaperJob" runat="server" style="width:77px;font-size:x-small;display:none"></asp:DropDownList>
                               <asp:TextBox ID="txtInputPaperModulesJob" runat="server" ReadOnly="true" style="width:100px;text-align:right;background-color:lightgray;border:1px solid;font-size:x-small;text-align:center"></asp:TextBox>
                           </td>
                           <td>
                               <asp:DropDownList ID="ddTonerJob" runat="server" style="width:77px;font-size:x-small;display:none">
                               </asp:DropDownList>
                               <asp:TextBox ID="txtTonerTypeJob" runat="server" ReadOnly="true" style="width:100px;text-align:right;background-color:lightgray;border:1px solid;font-size:x-small;text-align:center"></asp:TextBox>
                           </td>
                           <td>
                               <asp:DropDownList ID="ddEngineJob" runat="server" style="width:80px;font-size:x-small;display:none">
                               </asp:DropDownList>
                               <asp:TextBox ID="txtPrintEngineNgOrSheetCodeJob" runat="server" ReadOnly="true" style="width:100px;text-align:right;background-color:lightgray;border:1px solid;font-size:x-small;text-align:center"></asp:TextBox>
                           </td>
                       
                           
                           <td>                                
                               <asp:TextBox ID="txtEqInputPaperModules" runat="server" ReadOnly="true" style="width:100px;text-align:right;background-color:lightgray;border:1px solid;font-size:x-small;text-align:center"></asp:TextBox>
                           </td>
                           <td>
                               <asp:TextBox ID="txtEqTonerTypeOrSheetCode" runat="server" ReadOnly="true" style="width:100px;text-align:right;background-color:lightgray;border:1px solid;font-size:x-small;text-align:center"></asp:TextBox>

                           </td>
                           <td>
                               <asp:TextBox ID="txtEqPrintEngineOrCategory" runat="server" ReadOnly="true" style="width:100px;text-align:right;background-color:lightgray;border:1px solid;font-size:x-small;text-align:center"></asp:TextBox>

                           </td>
                       </tr>
                        <tr style="display:none">
                            <td> <asp:Button ID="btnLoadAtr" runat="server" OnClick="btnLoadAtr_Click" Style="font-size:x-small;height:25px" Text="Get Attributes" /></td>
                        </tr>
                   </table>
                </fieldset>
            </ContentTemplate>
            <triggers>
                <asp:AsyncPostBackTrigger ControlID="btnLoadAtr" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="gvDpJobs" EventName="SelectedIndexChanged" />
            </triggers>
        </asp:UpdatePanel>        
        <asp:UpdatePanel ID="upGvJobInstruction" runat="server" UpdateMode="Conditional">            
            <ContentTemplate> 
                 
                         <asp:GridView ID="gvJobInstructions" runat="server"
                                     AutoGenerateColumns="false"                           
                                                CssClass="baseTable"                           
                                                AllowPaging="false"
                                                AllowSorting="false"                                                
                                                PageSize="50"
                                                PagerSettings-Mode="NumericFirstLast"
                                                ShowHeaderWhenEmpty="true" style="width:1159px;margin-left:8px;margin-bottom:2px">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Job Instruction"><ItemTemplate><asp:Label ID="Label22" runat="server" Text='<%# Bind("Instruction") %>' Width="1140px" style="text-align:Left;word-wrap:break-word;white-space:normal;font-size: 10pt;"></asp:Label></ItemTemplate></asp:TemplateField>                                                    
                                                    <asp:BoundField DataField="FromSiteID" HeaderText="FromSiteID" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="JobNumber" HeaderText="JobNumber" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                                </Columns>
                                </asp:GridView>                          
            </ContentTemplate>
            <triggers>               
                <asp:AsyncPostBackTrigger ControlID="gvDpJobs" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ExecuteSearch" EventName="Click" />
            </triggers>                
        </asp:UpdatePanel>  
        <div style="padding-left:9px;">
            <asp:Label runat="server"  id="spnlErrorMessage" style="color: red;" ></asp:Label>            
        </div>
        <fieldset style="float: right; height: 442px; width: 1140px; border: 1px solid black; margin-right: 8px;margin-top:2px;">
            <div style="overflow: scroll; height: 445px; width: 1145px;" id="Result"> 
               
                <div style="margin-bottom: 5px; margin-top: 5px">
                    Results :</div>
                <asp:Timer ID="ctlTimer" runat="server" Interval="45000" OnTick="ctlTimer_Tick">
                </asp:Timer>
                <asp:UpdatePanel ID="upDpJobStatus" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>                        
                        <asp:GridView ID="gvDpJobs"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="baseTable"
                            AllowPaging="True"
                            AllowSorting="True"
                            PageSize="100"
                            DataKeyNames="Id"
                            OnRowDataBound="OnRowDataBound"
                            OnSelectedIndexChanged="OnSelectedIndexChanged"
                            OnPageIndexChanging="gvDpJobs_PageIndexChanging"
                            ShowHeaderWhenEmpty="True">
                            <Columns>
                                <asp:BoundField DataField="ComposerId" HeaderText="Job Composer Id" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="FromLocation" HeaderText="From Site" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="EquipmentId" HeaderText="Equipment ID" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ClientName" HeaderText="Client Name" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Job" HeaderText="Job" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Product" HeaderText="Product" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Split" HeaderText="Split" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Select" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                   <ItemTemplate>
                                     <asp:CheckBox ID="chkbox" runat="server" onclick ="Check_Click(this),Check_Click1(this)"  />
                                   </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Sequences" HeaderText="Sequences" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" DataFormatString="{0:n0}" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TotalSheetCount" HeaderText="Sheets" DataFormatString="{0:n0}" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ScheduledMailDate" HeaderText="Scheduled Mail Date" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Next" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate >
                                        <asp:ImageButton ID="btnNext" runat="server" Onclick="btnNext_Click" ImageUrl="~/Images/nextStatus.png" width="20px" height="15px" vertical-align="middle" CommandArgument='<%# Bind("Id") %>' 
                                        ToolTip="Chang to next status"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="NextTaskInProcess" HeaderText="Next Task In Process" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Priority" HeaderText="Priority" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>
                        <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>                        
                        <asp:AsyncPostBackTrigger ControlID="ExecuteSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID ="btnDismiss" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ctlTimer" EventName="Tick"/>
                        <asp:AsyncPostBackTrigger ControlID="btnDismissPriority" EventName="Click"/>
                        <asp:AsyncPostBackTrigger ControlID="btnDismissJobRouting" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnDismissCompletedOnDate" EventName="Click"/>
                        <asp:AsyncPostBackTrigger ControlID="btnAdditionalActionRequired" EventName="Click"/>
                        <asp:AsyncPostBackTrigger ControlID="btnYesJobAttributes" EventName="Click"/>
                        <asp:AsyncPostBackTrigger ControlID="gvDpJobs" EventName="PageIndexChanging"/>
                        <asp:AsyncPostBackTrigger ControlID="btnContinueDuplicate" EventName="Click"/>
                        <asp:AsyncPostBackTrigger ControlID="btnYesReleaseHold" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnMoveToBack" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSelectEquipmentOK" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </fieldset>
        <asp:UpdatePanel ID="popupDetails" runat="server">
            <ContentTemplate>
                <asp:Panel ID="actionMenu" runat="server" CssClass="searchPanel" Style="border: 1px solid #000; width: 100px; display: none; background-color: rgba(204, 204, 204, 1)">
                    <%--<h6 align="center">Action Menu</h6>--%>
                    <div align="center" style="font-size:x-large;font-weight:bold; margin-top: 10px; margin-bottom: 10px">
                        Action Menu</div>
                    <div style="width: 190px; margin-left: 8px; height: auto; margin-left: 8px; border: thin; border-style: inset inset inset inset;">
                        <asp:Button ID="btnProcessJobSplit" runat="server" Style="background-color: #f1f1f1" Text="Process Job/Split" Width="100%" OnClientClick="$find('ModalJobStatusSplit').show()" OnClick="BtnProcessJobSplit_Clicked" />
                        <br />
                        <asp:Button ID="btnChangePriority" runat="server" Style="background-color: #f1f1f1" Text="Change Priority" OnClick="btnChangePriority_Click" Width="100%" />
                        <br />
                        <asp:Button ID="btnChangeJobDates" runat="server" Style="background-color: #f1f1f1" Text="Change Job Dates" OnClick="btnChangeJobDates_Click" Width="100%"/>
                        <br />
                        <asp:Button ID="btnJobRouting" runat="server" Style="background-color: #f1f1f1" Text="In-Flight Job Routing" OnClick="btnJobRouting_Click" Width="100%"/>
                        <br />
                        <asp:Button ID="btnViewJobHistory" runat="server" Style="background-color: #f1f1f1" Text="View Job History" Width="100%" OnClick="BtnViewJobHistory_Clicked" />
                        <br />
                        <asp:Button ID="btnOpenVirtualInserter" runat="server" Style="background-color: #f1f1f1" Text="Virtual Inserter" Width="100%" OnClick="btnOpenVirtualInserter_Click" />
                        <br />
                        <asp:Button ID="btnViewInvItems" runat="server" Style="background-color: #f1f1f1" Text="View Inv Items/Job Instructions" Width="100%" OnClick="btnViewInvItems_Click" Enabled="true"  />
                        <br />
                    </div>
                    <%--<div style="margin-bottom:10px"></div>--%>
                    <div style="border: thin; border-style: inset; width: 100%; margin-top: 5px; margin-bottom: 5px">
                    </div>
                    <div style="width: 190px; height: auto; margin-left: 8px; border: thin; border-style: inset inset inset inset;">
                        <asp:Button ID="btnJobDiagnostic" runat="server" Style="background-color: #f1f1f1;white-space:normal;" Text="Job Diagnostics/Repair/ Submit a Ticket" Width="100%" OnClick="btnJobDiagnostic_Click" ></asp:Button>
                        <br />
                        <asp:Button ID="btnReprintJobTicket" runat="server" Style="background-color: #f1f1f1" Text="Reprint a Ticket" Width="100%" OnClick="btnReprintTicket_Click" />
                        <br />
                        <asp:Button ID="btnReprintPreSortDocs" runat="server" Style="background-color: #f1f1f1" Text="Reprint PreSort Docs" Width="100%" Enabled="false"  />
                        <br />
                    </div>
                    <div style="border: thin; border-style: inset; width: 100%; margin-top: 5px; margin-bottom: 5px">
                    </div>
                    <div style="width: 190px; margin-left: 8px; height: auto; margin-left: 8px; border: thin; border-style: inset inset inset inset;">
                        <asp:Button ID="btnCancelActionMenu" runat="server" Style="background-color: #f1f1f1" Text="Cancel Action Menu" Width="100%" OnClick="btnCancelActionMenu_Click" />
                    </div>
                    <ajax:modalpopupextender id="actionMenuControl" runat="server"
                        popupcontrolid="actionMenu"
                        dropshadow="True" drag="true" backgroundcssclass="modalBackground"
                        targetcontrolid="actionMenuControlHid">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="actionMenuControlHid" runat="server" />
                    <div style="margin: 0 0 10px 10px">
                    </div>
                </asp:Panel>
                <asp:Panel ID="jobSplitStatusPanel" runat="server" CssClass="processJob" Style="display: none;">
                    <div>
                        <table>
                            <tr>
                                <td style="font-weight:bold;font-size:small;padding:10px 0px 5px 5px">Update Job Status Sub-Menu</td>
                                <td style="width: 150px;text-align:center;font-weight:bold;font-size:small;color:red">
                                    <asp:Literal ID="litCancelled" runat="server"></asp:Literal>
                                </td>
                                <td style="font-weight:bold;font-size:small;width:215px;text-align:right">
                                    <asp:Literal ID="CustomerNameText" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="overflow-y: auto; height: 200px; width: 585px; margin: 5px 5px 5px 5px;">
                        <%--<asp:Label ID="laJobprocessing" Style="font-size: x-small; word-wrap: normal; word-break: break-all;" runat="server" text=""></asp:Label>--%>
                        <asp:UpdatePanel ID="UpdateJobStatusSubMenu" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvPopUpWindows"
                                    runat="server"
                                    AutoGenerateColumns="False"
                                    CssClass="baseTable"
                                    Style="width: 10%; font-size: smaller"
                                    AllowPaging="True"
                                    AllowSorting="True"
                                    PageSize="100"
                                    DataKeyNames="Id"
                                    ShowHeaderWhenEmpty="True"
                                    ClientSideEvent=" ">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Composer Job Id" SortExpression="JobComposerID">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ComposerID") %>' Width="60px" Style="text-align: center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job" SortExpression="JobNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("Job") %>' Width="40px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product" SortExpression="ProductNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("Product") %>' Width="40px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Split" SortExpression="SplitNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("Split") %>' Width="30px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sequences" SortExpression="Sequences">
                                            <ItemTemplate>
                                                <asp:Label ID="Label8" runat="server" Text='<%# Bind("Sequences") %>' Width="50px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity" SortExpression="SplitQuantity">
                                            <ItemTemplate>
                                                <asp:Label ID="Label9" runat="server" Text='<%# Bind("Quantity") %>' Width="50px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Current Status" SortExpression="TaskName">
                                            <ItemTemplate>
                                                <asp:Label ID="Label11" runat="server" Text='<%# Bind("Status") %>' Width="90px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Next Task" SortExpression="NextTaskName">
                                            <ItemTemplate>
                                                <asp:Label ID="Label12" runat="server" Text='<%# Bind("NextTaskInProcess") %>' Width="90px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Selected" SortExpression="Selected">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbDataSelected" Font-Size="Smaller" Text="Select?" AutoPostBack="false" Checked="false" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>


                    <hr style="margin-right:5px"/>
                    <div>
                     
                        <table>
                            <tr>
                                <td>
                                    <button id="btnUnCheckALl" style="background-color:darkgray; width:85px;font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:22px;border-color:gray;margin-left:5px" onclick="unCheckAll(); return false;">
                                        Un-Check All
                                    </button>
                                </td>
                                <td style="width: 19%" />
                                <td>
                                    <button id="btnCheckAll" style="background-color:darkgray;width:120px;font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:22px;border-color:gray;" onclick="checkAll();return false;">
                                        Check All
                                    </button>
                                </td>
                                <td style="width: 9.4%" />
                                <td>
                                    <asp:Button ID="btnMoveToBack" Style="background-color:darkgray;margin-bottom:5px;width:204px;font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:22px;border-color:gray;" runat="server" Text='Move Selected Splits to "Previous" Task' OnClick="BtnUpdateToPrevious_Clicked" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 590px">
                        <%--<button ID="btnUnCheckALl" OnClick="unCheckAll(); return false;" >Un-Check All</button>
                <asp:Button ID="btnDismiss" runat="server" Text="Dismiss" OnClick="BtnDismiss_Clicked" />--%>
                        <table>
                            <tr>
                                <td style="font-size:xx-small; padding-left: 5px;">Equipment ID</td>
                                <td>
                                    <asp:DropDownList ID="ddEquipmentIdProcessJob" Style="background-color:darkgray;width: 92px; height: 22px; font-size: xx-small;border-radius:5px;border-color:gray;margin-top:5px;border-style:solid;border-width:1px;" runat="server" />
                                </td>
                                <td style="padding-left:225px" />
                                <td>
                                    <asp:Button ID="btnUpdateToNext" Style="background-color:darkgray;height: 22px; font-size: xx-small;border-radius:5px;border-color:gray;border-style:solid;border-width:1px;width:205px" runat="server" Text="Update Job with {Next Task}" OnClick="BtnUpdateToNext_Clicked" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="font-size:xx-small; padding-left: 5px;">Status</td>
                                <td>
                                    <asp:DropDownList ID="ddPopStatus" Style="background-color:darkgray;width: 92px; height: 22px; font-size: xx-small;border-radius:5px;border-color:gray;margin-top:5px;border-style:solid;border-width:1px;" AutoPostBack="false" runat="server" />
                                </td>
                                <td style="padding-left:51px">
                                    <asp:Button ID="btnApplyStatus" Style=" background-color:darkgray;width:120px; font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:22px;border-color:gray;" runat="server" Text="Apply Status" OnClick="BtnApplyStatus_Clicked" />
                                </td>
                                <td style="float:right">
                                    <asp:Button ID="btnDismiss" Style="background-color:darkgray;width:90px; font-size:xx-small;border-radius:5px;height:22px;border-color:gray;margin-top:5px;border-style:solid;border-width:1px" runat="server" Text="Dismiss"  OnClick="BtnDismiss_Clicked"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:35px;"></td>
                                <td><asp:Button runat="server" ID="btnReleaseHold" Style=" background-color:darkgray; font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:22px;border-color:gray;" Text="Release from Hold" OnClick="btnReleaseHold_Click"></asp:Button></td>
                                <td><asp:Button runat="server" ID="btnResendSpoolFile" Style=" background-color:darkgray; margin-left:50px; font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:22px;width:120px;border-color:gray;" Text="Resend Printer Spool File" OnClick="btnResendSpoolFile_Click"></asp:Button></td>
                                <td><asp:Button runat="server" ID="btnResendInserterControlFile" Style=" background-color:darkgray;font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:22px;border-color:gray;" Text="Resend Inserter Control File" OnClick="btnResendInserterControlFile_Click"></asp:Button></td>
                            </tr>
                        </table>
                        <div id="jobStatusSubPart" runat="server" style="display: none;"></div>                      
                    <asp:HiddenField ID="hidForJobStatusSplit" runat="server" />
                    <ajax:modalpopupextender id="ModalJobStatusSplit" runat="server"
                        popupcontrolid="jobSplitStatusPanel" okcontrolid=""
                        cancelcontrolid=""
                        dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                        targetcontrolid="hidForJobStatusSplit">
                    </ajax:modalpopupextender>
                     <hr />
                        <td><asp:Button runat="server" ID="Button3" Style=" background-color:darkgray; margin-left:198px; font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:30px;width:135px;border-color:gray;white-space:normal;" Text="Job Diagnostics Resolve/Report a Problem" OnClick="ReprocessContainer_Click"></asp:Button></td>
                            <br />
                        <asp:Label ID="laMessage" Style="margin: 5px 5px; font-size: x-small;font-weight:bold" runat="server" Text="Message Area:"></asp:Label>
                        <div style="width: 98%; margin: 10px 5px">
                            <asp:Label ID="laJobprocessing" Style="font-size: x-small; word-wrap: normal; word-break: break-all;" runat="server" text=""></asp:Label>
                        </div>
                         </div>
                    
                </asp:Panel>
                 
                <asp:Panel ID="pnlDetails" runat="server" CssClass="jobHistory" Style="display: none; width: 1130px;">
                    <h2 style="display:inline-block">Job History</h2>
                    <asp:ImageButton ID="PrintFT_icon" runat="server" ImageUrl="~/Images/printer.png" Height="35px" Width="35px" Style="display:inline-block; float: right; margin-right: 20px;margin-top:10px" OnClick="PrintFT_icon_Click" >
                    </asp:ImageButton>
                    <fieldset style="height: 70px; border: 1px solid black; margin:10px;background-color:rgba(230,230,230,1)">
                        <table style="width:100%;">
                            <tr>
                                <td style="width:12%;font-weight:bold;font-size:small">Print Site<label style="float:right">:</label> </td>
                                <td style="width:10%;">
                                    <asp:Label ID="labPrintSite" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:12%;font-weight:bold;font-size:small">Customer<label style="float:right">:</label></td>
                                <td style="width:16%">
                                    <asp:Label ID="labCustomer" style="font-size:larger;" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:16%;font-weight:bold;font-size:small">Job Composer ID<label style="float:right">:</label></td>
                                <td style="width:12%">
                                    <asp:Label ID="labJobComposerId" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:12%;font-weight:bold;font-size:small">Split Qty<label style="float:right">:</label></td>
                                <td style="width:15%">
                                    <asp:Label ID="labSplitQty" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:12%;font-weight:bold;font-size:small">Job<label style="float:right">:</label></td>
                                <td style="width:10%">
                                    <asp:Label ID="labJob" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:12%;font-weight:bold;font-size:small">Product<label style="float:right">:</label></td>
                                <td style="width:16%">
                                    <asp:Label ID="labProduct" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:16%;font-weight:bold;font-size:small">Sequence Range<label style="float:right">:</label></td>
                                <td style="width:12%">
                                    <asp:Label ID="labSequenceRange" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:12%;font-weight:bold;font-size:small">Split<label style="float:right">:</label></td>
                                <td style="width:15%">
                                    <asp:Label ID="labSplit" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:12%;font-weight:bold;font-size:small">Sheet Count<label style="float:right">:</label></td>
                                <td style="width:10%;">
                                    <asp:Label ID="labSheetCount" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:13%;font-weight:bold;font-size:small">Image Count<label style="float:right">:</label></td>
                                <td style="width:16%">
                                    <asp:Label ID="labImageCount" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:15%;font-weight:bold;font-size:small">Reprints/Spoilage<label style="float:right">:</label></td>
                                <td style="width:12%">
                                    <asp:Label ID="labReprintsSpolige" style="font-size:larger" runat="server" Text=""></asp:Label>
                                    <asp:LinkButton ID="LinkReprintsSpolige" style="font-size:larger" runat="server" Text="" OnClick="LinkReprintsSpolige_Click"></asp:LinkButton>
                                </td>
                                <td style="width:12%;font-weight:bold;font-size:small">Is Complete<label style="float:right">:</label></td>
                                <td style="width:15%">
                                    <asp:Label ID="labIsComplete" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td style="width:16%;font-weight:bold;font-size:small">Exception Status :</td>
                                <td colspan="7">
                                    <asp:Label runat="server" ID="labException" style="font-size:x-small" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <div style="margin-left: 10px;overflow-y: auto; height: 309px; width: 1117px;">
                        <asp:GridView ID="gvDetails" runat="server"
                            AutoGenerateColumns="false"                           
                            CssClass="baseTable slavhis"                           
                            AllowPaging="false"
                            AllowSorting="true"
                            EmptyDataText="No records found."
                            PageSize="50"
                            PagerSettings-Mode="NumericFirstLast">
                            <Columns>
                                <asp:BoundField DataField="TaskId" HeaderText="DP Task ID" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="DPrintTask" HeaderText="DPrint Task" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="StartedOn" HeaderText="Started On" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CompletedOn" HeaderText="Completed On" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="DstOffSet" HeaderText="Dst OffSet" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ExecutionTime" HeaderText="Execution Time hh:mm:ss" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="FailedAttempts" HeaderText="Failed Attempts" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IsComplete" HeaderText="Is Complete" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center" />
                                <%--<asp:BoundField DataField="EquipmentUsed" HeaderText="Equipment Used"  ItemStyle-Width="210px" ItemStyle-HorizontalAlign="Center" />--%>
                               <asp:TemplateField HeaderText="Notes/Equipment Used" ItemStyle-Width="210" ItemStyle-HorizontalAlign="Center" >
                                     <ItemTemplate>
                                         <asp:Label ID="lblEquipmentUsed" runat="server" Text='<%#Eval("EquipmentUsed") %>' Width="210" class="wordwrap" ></asp:Label>
                                      </ItemTemplate>
                                 </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stat" ControlStyle-Width="15px" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="RealTimeTaskStatus" HeaderText="Status" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>
                        <ajax:modalpopupextender id="modalDetails" runat="server"
                            popupcontrolid="pnlDetails"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidClient">
                        </ajax:modalpopupextender>
                    </div>
                    <asp:HiddenField ID="hidClient" runat="server" />
                    <div style="margin: 10px 0 10px 10px">
                        <asp:LinkButton ID="btnClose" runat="server" Text="Close" CssClass="linkButton" OnClientClick="enableTimer()" OnClick="btnClose_Click" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlReprintDetails" runat="server" CssClass="jobHistory" Style="display:none">
                    <span style="float: left;width:22%;font-weight:bold;font-size:large;text-indent:20px;margin-top:8px;margin-bottom:8px">Reprints</span><span style="float: right;width:22%;margin-top:8px;margin-bottom:8px">Show Raw Reprint Data 
                    <label style="margin-left:10px;">
                    </label>
                    <asp:CheckBox ID="cbShowRawReprintData" Text="&#32;" Checked="false" runat="server" AutoPostBack="True" OnCheckedChanged="cbShowRawReprintData_CheckedChanged" />
                    </span>
                    <fieldset style="height: 50px; width:815px;border: 1px solid black; margin:10px;background-color:rgba(230,230,230,1)">
                        <table>
                            <tr>
                                <td style="width:12%;font-weight:bold;font-size:small">Print Site<label style="float:right">:</label> </td>
                                <td style="width:10%;">
                                    <asp:Label ID="labRePrintSite" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:12%;font-weight:bold;font-size:small">Customer<label style="float:right">:</label></td>
                                <td style="width:16%">
                                    <asp:Label ID="labReCustomer" style="font-size:larger;" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:16%;font-weight:bold;font-size:small">Job Composer ID<label style="float:right">:</label></td>
                                <td style="width:12%">
                                    <asp:Label ID="labReJobComposerId" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:12%;font-weight:bold;font-size:small">Split Qty<label style="float:right">:</label></td>
                                <td style="width:15%">
                                    <asp:Label ID="labReSplitQty" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:12%;font-weight:bold;font-size:small">Job<label style="float:right">:</label></td>
                                <td style="width:10%">
                                    <asp:Label ID="labReJob" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:12%;font-weight:bold;font-size:small">Product<label style="float:right">:</label></td>
                                <td style="width:16%">
                                    <asp:Label ID="labReProduct" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:16%;font-weight:bold;font-size:small">Sequence Range<label style="float:right">:</label></td>
                                <td style="width:12%">
                                    <asp:Label ID="labReSequenceRange" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:12%;font-weight:bold;font-size:small">Split<label style="float:right">:</label></td>
                                <td style="width:15%">
                                    <asp:Label ID="labReSplit" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:12%;font-weight:bold;font-size:small">Sheet Count<label style="float:right">:</label></td>
                                <td style="width:10%;">
                                    <asp:Label ID="labReSheetCount" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:13%;font-weight:bold;font-size:small">Image Count<label style="float:right">:</label></td>
                                <td style="width:16%">
                                    <asp:Label ID="labReImageCount" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:15%;font-weight:bold;font-size:small">Reprints/Spoilage<label style="float:right">:</label></td>
                                <td style="width:12%">
                                    <asp:Label ID="labReReprintsSpolige" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width:12%;font-weight:bold;font-size:small">Is Complete<label style="float:right">:</label></td>
                                <td style="width:15%">
                                    <asp:Label ID="labReIsComplete" style="font-size:larger" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <div style="margin-left: 10px;overflow-y: auto;width:850px; height: 309px;">
                        <asp:GridView ID="gvReprintDetails" runat="server"
                            AutoGenerateColumns="false"                           
                            CssClass="baseTable sla"                           
                            AllowPaging="true"                            
                            AllowSorting="true"
                            EmptyDataText="No records found."
                            PageSize="500"
                            PagerSettings-Mode="NumericFirstLast">
                            <Columns>
                                <asp:BoundField DataField="DPRemoteReprintID" HeaderText="DP Reprint ID" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReprintSequence" HeaderText="Sequence Number" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="AccountNumber" HeaderText="Account Number" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AddedOn" HeaderText="Date Record Added" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="MailPieceStatus" HeaderText="Reject Status" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IMBMailerIDSeq" HeaderText="IMB MailerID & Sequence" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Disposition" HeaderText="Disposition" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="OriginalDocumentID" HeaderText="Document ID" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReprintJobNumber" HeaderText="Reprint Job Number" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReprintGroupID" HeaderText="Reprint Seq #" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReprintStatus" HeaderText="Reprint Status" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>
                        <ajax:modalpopupextender id="modalRePrintDetails" runat="server"
                            popupcontrolid="pnlReprintDetails"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidReprint">
                        </ajax:modalpopupextender>
                    </div>
                    <asp:HiddenField ID="hidReprint" runat="server" />
                    <div style="margin: 10px 0 10px 10px">
                        <asp:LinkButton ID="btnReClose" runat="server" Text="Close" CssClass="linkButton" OnClick="btnReClose_Click" />
                    </div>
                </asp:Panel>                
                <asp:Panel ID="pnlWarningAAR" runat="server" CssClass="warningAdditionAction" Style="display:none">
   <div id="contextAdditionalActionRequired">
   <div style="align-self: center">
   <div style="color: #FF0000;font-size:40px; font-weight:bold;padding-left:10px; padding-top:14px;">This Split Contains Account Pulls</div>
   <center> <div style="color: #FE1F1F; font-size:30px; text-align:center;padding-top:5px;">Additional Action Required</div></center>
   <div style="padding-top:2px;"><span style="color: black;font-size:24px; padding-left:12px;">This Split contains Account / Orders that need to be pulled. An</span>
   <center> <span style="color: black;font-size:26px;"> “Account Pulls Ticket” is being printed on Printer</span></center>
   <center><span style="text-align:center; color:black; font-weight:bold; font-size:26px;">
   <asp:Label ID="labPrinter" runat="server" Text="" style="text-align:center"></asp:Label>
   </span></center>                             
   <span style="color: black;font-size:24px; padding-left:21px;">listing each mail piece that needs to be removed. The Inserter</span>
   <span style="color: black;font-size:24px; padding-left:41px;">will automatically divert the mail piece(s) that needs pulled.</span>
   <span style="color: black;font-size:24px; padding-left:90px;">Follow instructions on "Account Pulls Ticket".</span>  
   <p></p><p></p>
   </div>
    </div>
   </div>
                    <div align="center" style="padding-top:5px;">
                        <asp:Button ID="btnAdditionalActionRequired" runat="server" Text="OK" style="border-radius:5px;width:100px; font-size:20px; font-weight:550; height:42px;border-style:solid;border-width:1px;" Onclick="btnOkAAR_Click"></asp:Button>
                    </div>
                    <ajax:modalpopupextender id="modalOkAAR" runat="server"
                            popupcontrolid="pnlWarningAAR"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="HiddenField1">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlWarning" runat="server" CssClass="warningMsg" Style="display:none">
                    <div id="contextMenuNoAction">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Warning</p>
                            <p align="center" style="color: black">
                                This Job is Complete. No further</p>
                            <p align="center" style="color: black">
                                action can be taken on this Job / Split.</p>
                        </div>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnCloseWarning" Text="Dismiss" runat="server" style="border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" OnClick="btnCloseWarning_Click" />
                    </div>
                    <ajax:modalpopupextender id="modalWarningMsg" runat="server"
                            popupcontrolid="pnlWarning"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidWarningMsg">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidWarningMsg" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlWarningCancel" runat="server" CssClass="warningMsgYesStatusChange" Style="display:none">
                    <div id="contextMenuCancel">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Warning</p>
                            <p align="center" style="color: black">
                                You are attempting to update the Job to a Status that is typically automatically set by the Distributed Print System.
                                <p align="center" style="color: black">
                                   If attempting to Change status from “Inserting” to the next step; all Mail pieces that were damaged for this job and need to be reprinted will be lost; resulting in <font color="red">work not being Mailed.</font>                                                           
                                    <p align="center" style="color: black">
                                         Are you <font color="red">really</font> sure you want to do this Manually?                                                                                                                                                                                                                
                        </div>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnYes" Text="Yes" runat="server" style="float: left; margin-left: 20%;border-radius:5px;width:50px;height:25px;border-style:solid;border-width:1px;" OnClick="btnYes_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="No, Cancel" style="float: right; margin-right: 15%;border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" OnClick="btnCancel_Click" />
                    </div>
                    <ajax:modalpopupextender id="modalWarningMsg1" runat="server"
                            popupcontrolid="pnlWarningCancel"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidWarningMsg1">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidWarningMsg1" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlChangePriority" runat="server" CssClass="processJob" Style="display: none;">
                    <div>
                        <table>
                            <tr>
                                <td style="font-weight:bold;font-size:small;padding:10px 0px 5px 5px">Change Priority</td>
                                <td style="width: 65%" />
                                <td>
                                    <asp:Literal ID="CustomerNameText1" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="overflow-y: auto; height: 200px; width: 585px; margin: 5px 5px 5px 5px;">
                        <asp:UpdatePanel ID="UpdateChangePriority" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvChangePriority"
                                    runat="server"
                                    AutoGenerateColumns="False"
                                    CssClass="baseTable"
                                    Style="width: 10%; font-size: smaller"
                                    AllowPaging="True"
                                    AllowSorting="True"
                                    PageSize="100"
                                    DataKeyNames="Id"
                                    ShowHeaderWhenEmpty="True"
                                    ClientSideEvent=" ">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Composer Job Id" SortExpression="JobComposerID">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ComposerID") %>' Width="60px" Style="text-align: center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job" SortExpression="JobNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("Job") %>' Width="40px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product" SortExpression="ProductNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("Product") %>' Width="40px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Split" SortExpression="SplitNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("Split") %>' Width="30px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sequences" SortExpression="Sequences">
                                            <ItemTemplate>
                                                <asp:Label ID="Label8" runat="server" Text='<%# Bind("Sequences") %>' Width="50px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity" SortExpression="SplitQuantity">
                                            <ItemTemplate>
                                                <asp:Label ID="Label9" runat="server" Text='<%# Bind("Quantity") %>' Width="50px" Style="text-align: right"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Current Status" SortExpression="TaskName">
                                            <ItemTemplate>
                                                <asp:Label ID="Label11" runat="server" Text='<%# Bind("Status") %>' Width="90px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Priority" SortExpression="Priority">
                                            <ItemTemplate>
                                                <asp:Label ID="Label12" runat="server" Text='<%# Bind("Priority") %>' Width="90px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Selected" SortExpression="Selected">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbDataSelected" Font-Size="Smaller" Text="Select?" AutoPostBack="false" Checked="false" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <hr style="margin-right:5px"/>
                    <div>
                        <table>
                            <tr>
                                <td style="width: 30%" />
                                <td>
                                    <button id="btnUnCheckAllPriority" style="background-color:darkgray; width:85px;font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:22px;border-color:gray;margin-left:5px" onclick="unCheckAll(); return false;">
                                        Un-Check All
                                    </button>
                                </td>
                                <td style="width: 30%" />
                                <td>
                                    <button id="btnCheckAllPriority" style="background-color:darkgray;width:90px;font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:22px;border-color:gray;" onclick="checkAll();return false;">
                                        Check All
                                    </button>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 590px">
                        <table>
                            <tr>
                                <td style="width: 90px" />
                                <td style="font-size:xx-small; padding-left: 6px;">Job Priority</td>
                                <td>
                                    <asp:DropDownList ID="ddPriority" Style="background-color:darkgray;width: 85px; height: 22px; font-size: xx-small;border-radius:5px;border-color:gray;margin-top:5px;border-style:solid;border-width:1px;" runat="server" onchange="GetSelectedTextValue(this)" />
                                </td>
                                <td style="width: 135px" />
                                <td>
                                    <asp:Button ID="btnUpdatePriority" Style="background-color:darkgray;height: 22px; font-size: xx-small;border-radius:5px;border-color:gray;border-style:solid;border-width:1px;width:205px" runat="server" Text="Update Selected Records to this Priority" Onclick="btnUpdatePriority_Click" />
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnDismissPriority" Style="margin-left:255px; background-color:darkgray;width:90px; font-size:xx-small;border-radius:5px;height:22px;border-color:gray;margin-top:5px;border-style:solid;border-width:1px" runat="server" Text="Dismiss" OnClick="btnDismissPriority_Click"  />
                                </td>
                            </tr>
                        </table>
                        <hr />
                        <asp:Label ID="laMessagePriority" Style="margin: 5px 5px; font-size: x-small;font-weight:bold" runat="server" Text="Message Area:"></asp:Label>
                        <div style="width: 98%; margin: 10px 5px">
                            <asp:Label ID="laPriority" Style="font-size: x-small; word-wrap: normal; word-break: break-all;" runat="server"></asp:Label>
                        </div>
                    </div>
                    <ajax:modalpopupextender id="modalChangePriority" runat="server"
                            popupcontrolid="pnlChangePriority"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidChangePriority">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidChangePriority" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlChangeJobMailDate" runat="server" CssClass="processJob" Style="display: none;">
                    <div>
                        <table>
                            <tr>
                                <td style="font-weight:bold;font-size:small;padding:10px 0px 5px 5px">Change Completed On Mail Date</td>
                                <td style="width: 45%" />
                                <td>
                                    <asp:Literal ID="CustomerNameText2" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="overflow-y: auto; height: 200px; width: 585px; margin: 5px 5px 5px 5px;">
                        <asp:UpdatePanel ID="UpdateChangeJobMailDate" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvChangeJobMailDate"
                                    runat="server"
                                    AutoGenerateColumns="False"
                                    CssClass="baseTable"
                                    Style="width: 10%; font-size: smaller"
                                    AllowPaging="True"
                                    AllowSorting="True"
                                    PageSize="100"
                                    DataKeyNames="Id"
                                    ShowHeaderWhenEmpty="True"
                                    ClientSideEvent=" ">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Composer Job Id" SortExpression="JobComposerID">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ComposerID") %>' Width="60px" Style="text-align: center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job" SortExpression="JobNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("Job") %>' Width="40px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product" SortExpression="ProductNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("Product") %>' Width="40px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Split" SortExpression="SplitNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("Split") %>' Width="30px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sequences" SortExpression="Sequences">
                                            <ItemTemplate>
                                                <asp:Label ID="Label8" runat="server" Text='<%# Bind("Sequences") %>' Width="50px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity" SortExpression="SplitQuantity">
                                            <ItemTemplate>
                                                <asp:Label ID="Label9" runat="server" Text='<%# Bind("Quantity") %>' Width="50px" Style="text-align: right"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Current Status" SortExpression="TaskName">
                                            <ItemTemplate>
                                                <asp:Label ID="Label11" runat="server" Text='<%# Bind("Status") %>' Width="90px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Completed On Date" SortExpression="CompletedOn">
                                            <ItemTemplate>
                                                <asp:Label ID="Label12" runat="server" Text='<%# Bind("CompletedOn") %>' Width="110px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Selected" SortExpression="Selected">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbDataSelected" Font-Size="Smaller" Text="Select?" AutoPostBack="false" Checked="false" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <hr style="margin-right:5px"/>
                    <div>
                        <table>
                            <tr>
                                <td style="width: 31%" />
                                <td>
                                    <button id="btnUnCheckAllCompletedOnDate" style="background-color:darkgray; width:85px;font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:22px;border-color:gray;margin-left:5px" onclick="unCheckAll(); return false;">
                                        Un-Check All
                                    </button>
                                </td>
                                <td style="width: 30%" />
                                <td>
                                    <button id="btnCheckAllCompletedOnDate" style="background-color:darkgray;width:90px;font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:22px;border-color:gray;" onclick="checkAll();return false;">
                                        Check All
                                    </button>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 590px">
                        <table>
                            <tr>
                                <td style="width: 0px" />
                                <td style="font-size:xx-small; padding-left: 6px; width:145px">Update Completed On Date / Time</td>
                                <td>
                                    <asp:TextBox ID="txtCompletedOnDate" runat="server" style="width:100px;font-size:smaller" onchange="javascript: Changed( this );" OnTextChanged="txtCompletedOnDate_TextChanged"></asp:TextBox>
                                    <ajax:calendarextender runat="server" id="calCompletedOnDate" targetcontrolid="txtCompletedOnDate" />
                                </td>
                                <td></td>
                                <td style="width: 135px" />
                                <td>
                                    <asp:Button ID="btnUpdateCompletedOnDate" Style="background-color:darkgray;height: 22px; font-size: xx-small;border-radius:5px;border-color:gray;border-style:solid;border-width:1px;width:205px" runat="server" Text="Update Selected Records to this Date" Onclick="btnUpdateCompletedOnDate_Click" />
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnDismissCompletedOnDate" Style="margin-left:270px; background-color:darkgray;width:90px; font-size:xx-small;border-radius:5px;height:22px;border-color:gray;margin-top:5px;border-style:solid;border-width:1px" runat="server" Text="Dismiss" Onclick="btnDismissCompletedOnDate_Click" />
                                </td>
                            </tr>
                        </table>
                        <hr />
                        <asp:Label ID="laMessageCompletedOnDate" Style="margin: 5px 5px; font-size: x-small;font-weight:bold" runat="server" Text="Message Area:"></asp:Label>
                        <div style="width: 98%; margin: 10px 5px">
                            <asp:Label ID="laCompletedDateOnChange" Style="font-size: x-small; word-wrap: normal; word-break: break-all;" runat="server"></asp:Label>
                        </div>
                    </div>
                    <ajax:modalpopupextender id="modalChangeJobMailDate" runat="server"
                            popupcontrolid="pnlChangeJobMailDate"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidChangeJobMailDate">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidChangeJobMailDate" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlWarningCompletedDateCancel" runat="server" CssClass="warningMsg" Style="display:none">
                    <div id="contextMenuNoAction">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Warning</p>
                            <p align="center" style="color: black">
                                One or more selected Splits is not Complete.<br />You cannot re-set the Completed On date on a Split that is still in process.</p>
                        </div>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnWarningCompletedDateCancel" Text="Dismiss" runat="server" style="border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" Onclick="btnWarningCompletedDateCancel_Click" />
                    </div>
                    <ajax:modalpopupextender id="modalWarningCompletedDateCancel" runat="server"
                            popupcontrolid="pnlWarningCompletedDateCancel"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidWarningCompletedDateCancelMsg">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidWarningCompletedDateCancelMsg" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlWarningCompletedDateYes" runat="server" CssClass="warningMsgYes" Style="display:none">
                    <div id="contextMenuCancel">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Warning</p>
                            <p align="center" style="color: black">
                                This update will result in changes to job Statistics.<br/>If you select "Are You Sure" below,
							    <br/>
                                and email will be sent to email group [ DistributedPrint@ncpsolutions.com ] to
								<br/>
                                inform Management of this change.</p>
                        </div>
                    </div>
                    <div align="center">
                        <asp:Button ID="Button1" Text="Are You Sure?" runat="server" style="float: left; margin-left: 10%;border-radius:5px;width:105px;height:25px;border-style:solid;border-width:1px;" Onclick="Button1_Click" />
                        <asp:Button ID="btnWarningCompletedDateYesCancel" runat="server" Text="No, Cancel" style="float: right; margin-right: 10%;border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" Onclick="btnWarningCompletedDateYesCancel_Click"/>
                    </div>
                    <ajax:modalpopupextender id="modalWarningCompletedDateYes" runat="server"
                            popupcontrolid="pnlWarningCompletedDateYes"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidmodalWarningCompletedDateYesMsg">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidmodalWarningCompletedDateYesMsg" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlWarningApplyStatusYes" runat="server" CssClass="warningMsg" Style="display:none">
                    <div id="contextMenuCancel">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Warning</p>
                            <p align="center" style="color: black">
                                This Job is flagged "Complete". Are you sure
							    <br/>
                                you want to take action on this Job/Split?</p>
                        </div>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnApplyYes" runat="server" Text="Yes" style="float: left; margin-left: 10%;border-radius:5px;width:105px;height:25px;border-style:solid;border-width:1px;" OnClick="btnApplyYes_Click"></asp:Button>
                        <asp:Button ID="btnApplyNo" runat="server" Text="No, Cancel" style="float: right; margin-right: 10%;border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" OnClick="btnApplyNo_Click" ></asp:Button>
                    </div>
                    <ajax:modalpopupextender id="modalWarningApplyStatusYes" runat="server"
                            popupcontrolid="pnlWarningApplyStatusYes"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidmodalWarningApplyStatusYes">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidmodalWarningApplyStatusYes" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlWarningApplyStatusCancel" runat="server" CssClass="warningCancel" Style="display:none">
                    <div id="contextMenuCancel">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Warning</p>
                            <p align="center" style="color: black">
                                Use the "Update Job Status to {Next Status}"
							    <br/>
                                button. This button should not be used for
							    <br/>
                                normally processing. This buttton cannot be
							    <br/>
                                used to "skip" a processing step.</p>
                        </div>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnApplyCancel" runat="server" Text="Cancel" style="border-radius:5px;width:105px;height:25px;border-style:solid;border-width:1px;" ></asp:Button>
                    </div>
                    <ajax:modalpopupextender id="modalWarningApplyStatusCancel" runat="server"
                            popupcontrolid="pnlWarningApplyStatusCancel"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidmodalWarningApplyStatusCancel">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidmodalWarningApplyStatusCancel" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlWarningApplyStatusExp" runat="server" CssClass="warningCancel" Style="display:none">
                    <div id="contextMenuCancel">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Please enter an Exception Comment</p>
                        </div>
                    </div>
                    <div align="center">
                        <asp:TextBox ID="textComment" runat="server" Height="60px" MaxLength="120" Rows="2" TextMode="MultiLine" Width="265px" style="margin-bottom:10px; border-style: ridge;border-width: 1px;border-color:#555555;"></asp:TextBox>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnExpContinue" runat="server" Text="Continue" style="float: left; margin-left: 10%;border-radius:5px;width:75px;height:25px;border-style:solid;border-width:1px;" OnClick="btnExpContinue_Click"></asp:Button>
                        <asp:Button ID="btnExpCancel" runat="server" Text="Go Back Do not Update" style="float: right; margin-right: 10%;border-radius:5px;width:155px;height:25px;border-style:solid;border-width:1px;"></asp:Button>
                    </div>
                    <ajax:modalpopupextender id="modalWarningApplyStatusExp" runat="server"
                            popupcontrolid="pnlWarningApplyStatusExp"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidmodalWarningApplyStatusExp">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidmodalWarningApplyStatusExp" runat="server" />
                </asp:Panel>
               
                <asp:Panel ID="pnlJobRouting" runat="server" CssClass="processJob" Style="display: none;">
                    <div>
                        <table>
                            <tr>
                                <td style="font-weight:bold;font-size:small;padding:10px 0px 5px 5px">In-flight Job Routing</td>
                                <td style="width: 150px;text-align:center;font-weight:bold;font-size:small;color:red">
                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                </td>
                                 <td style="font-weight:bold;font-size:small;width:215px;text-align:right">
                                    <asp:Literal ID="litCustomerName" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="overflow-y: auto; height: 200px; width: 585px; margin: 5px 5px 5px 5px;">
                        <asp:UpdatePanel ID="JobRouting" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvJobRouting"
                                    runat="server"
                                    AutoGenerateColumns="False"
                                    CssClass="baseTable"
                                    Style="width: 10%; font-size: smaller"
                                    AllowPaging="True"
                                    AllowSorting="True"
                                    PageSize="100"
                                    DataKeyNames="Id"
                                    ShowHeaderWhenEmpty="True"
                                    ClientSideEvent=" ">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Composer Job Id" SortExpression="JobComposerID">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ComposerID") %>' Width="60px" Style="text-align: center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job" SortExpression="JobNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("Job") %>' Width="40px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product" SortExpression="ProductNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("Product") %>' Width="40px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Split" SortExpression="SplitNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("Split") %>' Width="30px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sequences" SortExpression="Sequences">
                                            <ItemTemplate>
                                                <asp:Label ID="Label8" runat="server" Text='<%# Bind("Sequences") %>' Width="50px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity" SortExpression="SplitQuantity">
                                            <ItemTemplate>
                                                <asp:Label ID="Label9" runat="server" Text='<%# Bind("Quantity") %>' Width="50px" Style="text-align: right"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Current Status" SortExpression="TaskName">
                                            <ItemTemplate>
                                                <asp:Label ID="Label11" runat="server" Text='<%# Bind("Status") %>' Width="90px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Current Processing Site" SortExpression="CurrentProcessingSite">
                                            <ItemTemplate>
                                                <asp:Label ID="Label12" runat="server" Text='<%# Bind("PrintLocation") %>' Width="110px" Style="text-align: Center"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Selected" SortExpression="Selected">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbDataSelected" Font-Size="Smaller" Text="Select?" AutoPostBack="false" Checked="false" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <hr style="margin-right:5px"/>
                    <div>
                        <table>
                            <tr>
                                <td style="width: 31%" />
                                <td>
                                    <button id="btnUnCheckAllJobs" style="background-color:darkgray; width:85px;font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:22px;border-color:gray;margin-left:6px" onclick="unCheckAll(); return false;">
                                        Un-Check All
                                    </button>
                                </td>
                                <td style="width: 30%" />
                                <td>
                                    <button id="btnCheckAllJobs" style="background-color:darkgray;width:90px;font-size:xx-small;border-style:solid;border-width:1px;border-radius:5px;height:22px;border-color:gray;" onclick="checkAll();return false;">
                                        Check All
                                    </button>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 590px">
                        <table>
                            <tr>
                                <td style="width: 0px" />
                                <td style="font-size:xx-small; padding-left: 6px; width:145px;text-align:center;">Move Split(s) to this Site</td>
                                <td>
                                    <asp:DropDownList ID="ddDpRoutingSites" Style="background-color:darkgray;width: 85px; height: 22px; font-size: xx-small;border-radius:5px;border-color:gray;margin-top:5px;border-style:solid;border-width:1px;" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddDpRoutingSites_SelectedIndexChanged" />
                                </td>
                                <td></td>
                                <td style="width: 135px" />
                                <td>
                                    
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td style="width: 40px" />
                                <td style="font-size:xx-small; padding-left: 6px; width:170px;text-align:center;">Include Summary Inserter Control File?</td>                                
                                <td style="width:25px"><asp:CheckBox ID="cbMegaMrdf" Font-Size="Smaller" Text="&#32;" AutoPostBack="false" Checked="false" runat="server"></asp:CheckBox></td>
                                <td><asp:Button ID="btnDismissJobRouting" Style="margin-bottom:5px; margin-left:20px; background-color:darkgray;width:90px; font-size:xx-small;border-radius:5px;height:22px;border-color:gray;margin-top:5px;border-style:solid;border-width:1px" runat="server" Text="Dismiss" OnClick="btnDismissJobRouting_Click" /></td>
                                <td>
                                    <asp:Button ID="btnUpdateJobs" Style="margin-left:24px; background-color:darkgray;height: 22px; font-size: xx-small;border-radius:5px;border-color:gray;border-style:solid;border-width:1px;width:205px" runat="server" Text="Move Selected Jobs/Splits to this Site" Onclick="btnUpdateJobs_Click"/>
                                </td>
                            </tr>
                        </table>
                        <hr />
                        <asp:Label ID="labMessageJobRouting" Style="margin: 5px 5px; font-size: x-small;font-weight:bold" runat="server" Text="Message Area:"></asp:Label>
                        <div style="width: 98%; margin: 10px 5px">
                            <asp:Label ID="labMsgJobRouting" Style="font-size: x-small; word-wrap: normal; word-break: break-all;" runat="server"></asp:Label>
                        </div>
                    </div>
                    <ajax:modalpopupextender id="modalJobRouting" runat="server"
                            popupcontrolid="pnlJobRouting"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidJobRouting">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidJobRouting" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlWarningCancelJobRouting" runat="server" CssClass="warningCancel" Style="display:none">
                    <div id="contextMenuCancelJobRouting">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Warning</p>
                            <p align="center" style="color: black">
                                One or more selected Splits is
							    <br/>
                                showing as Complete or Cancelled or not yet ready for transfer
							    <br/>
                                You cannot send a completed Split to
							    <br/>
                                an alternate site for processing</p>
                        </div>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnJobRoutingCancel" runat="server" Text="Cancel" style="border-radius:5px;width:105px;height:25px;border-style:solid;border-width:1px;" Onclick="btnJobRoutingCancel_Click"></asp:Button>
                    </div>
                    <ajax:modalpopupextender id="modalWarningCancelJobRouting" runat="server"
                            popupcontrolid="pnlWarningCancelJobRouting"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidmodalWarningCancelJobRouting">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidmodalWarningCancelJobRouting" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlWarningJobRouting" runat="server" CssClass="warningMsgJobRouting" Style="display:none">
                    <div id="contextMenuJobRouting">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Warning</p>
                            <p align="center" style="color: black">
                                This update will send selected Splits to an alternate Site
							    <br/>
                                for Processing. Any "work-in-progress" at the current location must be located and disposed of properly.<br/>If you select "Are You Sure" below, an email will be sent to email group [ DistributedPrint@ncpsolutions.com ] to
								<br/>
                                inform Management of this change.</p>
                        </div>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" style="margin-left:10px;color:black" Text="Total Splits Printed"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labTotalSplitsPrinted" runat="server" style="margin-left:10px;color:black" Text=""/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" style="margin-left:10px;color:black" Text="Total Splits Moving"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labTotalSplitsMoving" runat="server" style="margin-left:10px;color:black" Text=""/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" style="margin-left:10px;color:black" Text="Total Volume"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labTotalVolume" runat="server" style="margin-left:10px;color:black" Text=""/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label13" runat="server" style="margin-left:10px;color:black" Text="Total Sheets"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labTotalSheetsCount" runat="server" style="margin-left:10px;color:black" Text=""/>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnConfirmJobRouting" Text="Are You Sure?" runat="server" style="float: left; margin-left: 10%;border-radius:5px;width:105px;height:25px;border-style:solid;border-width:1px;" OnClick="btnConfirmJobRouting_Click"  />
                        <asp:Button ID="btnWarningJobRouting" runat="server" Text="No, Cancel" style="float: right; margin-right: 10%;border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" Onclick="btnWarningJobRouting_Click"/>
                    </div>
                    <ajax:modalpopupextender id="modalWarningJobRouting" runat="server"
                            popupcontrolid="pnlWarningJobRouting"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidmodalWarningJobRouting">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidmodalWarningJobRouting" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlWarningJobPrinterAttributes" runat="server" CssClass="warningAtrCancel" Style="display:none">
                    <div id="contextMenuCancelJobRouting">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Warning</p>
                            <p align="center" style="color: black">
                                This change will allow a Job that requires MICR toner/Ink
							    <br/>
                                to be available to a NON‐MICR print device. This
							    <br/>
                                could result in serious Lock Box issues and may result in
							    <br/>
                                penalties to be paid by Harland Clarke.</p>
                            <p align="center" style="color: black">
                                This option is not allowed</p>
                        </div>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnJobPrinterAttributesCancel" runat="server" Text="Cancel" style="border-radius:5px;width:105px;height:25px;border-style:solid;border-width:1px;" OnClick="btnJobPrinterAttributesCancel_Click"></asp:Button>
                    </div>
                    <ajax:modalpopupextender id="modalWarningJobPrinterAttributes" runat="server"
                            popupcontrolid="pnlWarningJobPrinterAttributes"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidmodalWarningJobPrinterAttributes">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidmodalWarningJobPrinterAttributes" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlCautionJobPrinterAttributes" runat="server" CssClass="warningMsgJobRouting" Style="display:none;height:200px">
                    <div id="contextMenuCaution">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Caution</p>
                            <p align="center" style="color: black">
                                You are attempting to change a Job Attribute that
							    <br/>
                                could result in work being printed incorrectly.                                                                                                                                                                           
                        </div>
                    </div>
                    <div>
                        <p align="center" style="color: black">
                            Do you want to Continue?</p>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnYesJobAttributes" runat="server" Text="Yes" style="float: left; margin-left: 10%;border-radius:5px;width:105px;height:25px;border-style:solid;border-width:1px;" OnClick="btnYesJobAttributes_Click"></asp:Button>
                        <asp:Button ID="btnNoCancelJobAttributes" runat="server" Text="No, Cancel" style="float: right; margin-right: 10%;border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" OnClick="btnNoCancelJobAttributes_Click"></asp:Button>
                    </div>
                    <ajax:modalpopupextender id="modalCautionJobPrinterAttributes" runat="server"
                            popupcontrolid="pnlCautionJobPrinterAttributes"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidmodalCaution">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidmodalCaution" runat="server" />
                </asp:Panel>
                <asp:panel ID="pnlDuplicateWarning" runat="server" CssClass="warningDuplicate" Style="display:none;" >
                    <div id="contextMenuDuplicateWarning">
                        <div style="align-self: center">
                            <div style="color: red;font-size:70px" align="center">Duplicate Warning</div>  
                            <div align="center" style="color:black">Environment:<asp:Label ID="labEnvironment" runat="server" Text=""></asp:Label>
                                <br />This Split was previously moved to <asp:Label ID="labStatusInMessage" runat="server" Text=""/> on
                                <br /><asp:Label ID="labDateInMessage" runat="server" Text=""/> by Operator <asp:Label ID="labOperatroName" runat="server" Text=""/> in <asp:Label ID="labSiteNameInMessage" runat="server" Text=""/>. 
                                <br />Split was moved to Equipment <asp:Label ID="labEquipmentInMessage" runat="server" Text=""/>
                                <br />If you continue, an email message will be sent to Leads and Management informing them that a duplicate split is being processed.<asp:Label ID="labCompletedOn" runat="server" Visible="False"/>
                            </div>  
                              <asp:HiddenField ID="hidDupClientName" runat="server"></asp:HiddenField><asp:HiddenField ID="hidDupJob" runat="server"></asp:HiddenField><asp:HiddenField ID="hidDupProduct" runat="server"></asp:HiddenField><asp:HiddenField ID="hidDupSplit" runat="server"></asp:HiddenField><asp:HiddenField ID="hidStartedOnUtcTime" runat="server"></asp:HiddenField>                                                                                                                                                                                                                           
                        <asp:HiddenField ID="hdnDuplicateWarnMessage" runat="server" value=""></asp:HiddenField>
                        </div>
                    </div>
                    <div align="center" style="margin-top:10px">
                        <asp:Button ID="btnContinueDuplicate" Text="Are you Sure you want to Continue?" runat="server" style="font-size:22px; float: left; margin-left: 10%;border-radius:5px;width:400px;height:50px;border-style:solid;border-width:1px;border-color:red" OnClientClick="GetWarningMessage()" OnClick="btnContinueDuplicate_Click"></asp:Button>
                        <asp:Button ID="btnCancelNoDuplicate" Text="No,Cancel" runat="server" style="font-size:22px;float: right; margin-right: 10%;border-radius:5px;width:150px;height:50px;border-style:solid;border-width:1px;;border-color:red" OnClientClick="GetWarningMessage()" OnClick="btnCancelNoDuplicate_Click"></asp:Button>
                    </div>
                    <ajax:modalpopupextender id="modalDuplicateWarning" runat="server"
                            popupcontrolid="pnlDuplicateWarning"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidmodalDuplicateWarning">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidmodalDuplicateWarning" runat="server" />
                </asp:panel>
                <asp:Panel ID="pnlReleaseHoldCaution" runat="server" CssClass="warningMsgJobRouting" Style="display:none;height:200px">
                    <div id="contextMenuCaution">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Caution</p>
                            <p align="center" style="color: black">
                               If you answer Yes, then the current Status of this Split will be changed to <asp:Label Id="labReleaseHoldTask" runat="server" Text=""></asp:Label>  and it will no longer be on Hold.							                                                                                                                                                                              
                        </div>
                    </div>
                    <div>
                        <p align="center" style="color: black">
                            Do you want to Continue?</p>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnYesReleaseHold" runat="server" Text="Yes" style="float: left; margin-left: 10%;border-radius:5px;width:105px;height:25px;border-style:solid;border-width:1px;" OnClick="btnYesReleaseHold_Click" ></asp:Button>
                        <asp:Button ID="btnNoCencelReleaseHold" runat="server" Text="No, Cancel" style="float: right; margin-right: 10%;border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" OnClick="btnNoCencelReleaseHold_Click" ></asp:Button>
                    </div>
                    <ajax:modalpopupextender id="ModalReleaseHoldCaution" runat="server"
                            popupcontrolid="pnlReleaseHoldCaution"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidReleaseHoldCaution">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidReleaseHoldCaution" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlResendWarning" runat="server" CssClass="warningMsgJobRouting" Style="display:none;height:150px">
                    <div id="contextMenuCaution">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Warning</p>
                            <p align="center" style="color: black">
                               The Status of the Split must be in the <asp:Label Id="labResendWarning" style="color:red;" runat="server" Text=""></asp:Label>  step for this feature to work.							                                                                                                                                                                              
                        </div>
                    </div>                   
                    <div align="center">                        
                        <asp:Button ID="btnReturnToGui" runat="server" Text="Return to GUI" style="border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" OnClick="btnNoCencelReleaseHold_Click" ></asp:Button>                        
                         <asp:HiddenField ID="hdnReturnToViewFlag" runat="server" />
                    </div>
                    <ajax:modalpopupextender id="ModalResendWarning" runat="server"
                            popupcontrolid="pnlResendWarning"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidResendWarning">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidResendWarning" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlPreviousWarning" runat="server" CssClass="warningMsgJobRouting" Style="display:none;height:150px">
                    <div id="contextMenuCaution">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Warning</p>
                            <p align="center" style="color: black;font-size:medium;margin:5px 5px 10px 5px;">
                               The current Status of this Split does not allow it to be moved to a previous step.							                                                                                                                                                                              
                        </div>
                    </div>                   
                    <div align="center">                        
                        <asp:Button ID="btnPreviousWarning" runat="server" Text="Return to GUI" style="font-size:medium;border-radius:5px;width:120px;height:25px;border-style:solid;border-width:1px;" OnClick="btnNoCencelReleaseHold_Click" ></asp:Button>
                    </div>
                    <ajax:modalpopupextender id="ModalPreviousWarning" runat="server"
                            popupcontrolid="pnlPreviousWarning"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="hidPreviousWarning">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="hidPreviousWarning" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlAccountPullWarning" runat="server" CssClass="warningMsgJobRouting" Style="display:none;height:150px">
                    <div id="contextMenuCaution">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Warning</p>
                            <p align="center" style="color: black;font-size:medium;margin:5px 5px 10px 5px;">
                               No Account Details are Available.							                                                                                                                                                                              
                        </div>
                    </div>                   
                    <div align="center" style="padding-top:15px;">                        
                        <asp:Button ID="Button2" runat="server" Text="Return to GUI" style="font-size:medium;border-radius:5px;width:120px;height:25px;border-style:solid;border-width:1px;" OnClick="btnNoCencelReleaseHold_Click" ></asp:Button>
                    </div>
                    <ajax:modalpopupextender id="ModalAccountPullWarning" runat="server"
                            popupcontrolid="pnlAccountPullWarning"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="HiddenField2">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlReprintTickets" runat="server" CssClass="warningMsgJobRouting" Style="display:none;height:300px;width:550px">
                    <div id="contextReprintTickets">
                        <div style="align-self: center">
                            <p style="color: black;font-size:16px;font-weight:bold" align="center">
                                Job Control Documents Reprints Menu</p>                            						                                                                                                                                                                              
                        </div>
                         <div style="margin-left: 45px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="labTicketType" runat="server" style="color:black" Text="Ticket Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddReprintTicketType" Style="width: 140px; height: 22px; font-size:small;border-radius:5px;border-color:gray;margin-top:5px;border-style:solid;border-width:1px;" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labReprintTicketJob" runat="server" style="color:black" Text="Job"></asp:Label>
                                    </td>
                                    <td style="padding-top:10px">
                                        <asp:TextBox ID="txtReprintTicketJob" runat="server" style="width: 135px;border-style: ridge;border-width: 1px;border-color:#555555;" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labReprintTicketProduct" runat="server" style="color:black" Text="Product"></asp:Label>
                                    </td>
                                    <td style="padding-top:10px">
                                        <asp:TextBox ID="txtReprintTicketProduct" runat="server" style="width: 135px;border-style: ridge;border-width: 1px;border-color:#555555;" ReadOnly="True"></asp:TextBox>              
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labReprintTicketSplitNmber" runat="server" style="color:black" Text="Split"></asp:Label>
                                    </td>
                                    <td style="padding-top:10px">
                                        <asp:TextBox ID="txtReprintTicketSplitNumber" runat="server" style="width: 135px;border-style: ridge;border-width: 1px;border-color:#555555;" ReadOnly="True"></asp:TextBox>              
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:80px">
                                        <asp:Label ID="labReprintTicketWatermark" runat="server" style="color:black" Text="Custom Watermark"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReprintTicketWatermark" runat="server" MaxLength="35" style="width: 380px;border-style: ridge;border-width: 1px;border-color:#555555;"></asp:TextBox>              
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label ID="labReprintTicketPrinter" runat="server" style="color:black" Text="Printer"></asp:Label></td>
                                    <td><asp:DropDownList ID="ddReprintTicketPrinters" Style="width: 388px; height: 22px; font-size:small;border-radius:5px;border-color:gray;margin-top:5px;border-style:solid;border-width:1px;" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Label ID="printerMsg" runat="server" style="color:green;font-size:smaller" Text=""></asp:Label></td>
                                </tr>
                            </table>
                        </div>
                        <div align="center" style="margin-top:10px">
                            <table>
                                <tr>
                                    <td style="width:100px"><asp:Button ID="btnTicketPrint" Text="Print" runat="server" style="float: left;border-radius:5px;width:60px;height:25px;border-style:solid;border-width:1px;border-color:gray" OnClick="btnTicketPrint_Click" ></asp:Button></td>
                                    <td><asp:Button ID="btnViewPrint" Text="View" runat="server" style="border-radius:5px;width:60px;height:25px;border-style:solid;border-width:1px;border-color:gray" OnClick="btnViewPrint_Click" ></asp:Button></td>
                                    <td style="width:100px"><asp:Button ID="btnTicketPrintCancel" Text="Cancel" runat="server" style="float: right;border-radius:5px;width:60px;height:25px;border-style:solid;border-width:1px;border-color:gray" ></asp:Button></td>
                                </tr>
                            </table>                                                
                    </div>
                    </div>                                       
                    <ajax:modalpopupextender id="ModalReprintTickets" runat="server"
                            popupcontrolid="pnlReprintTickets"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="HiddenField3">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="HiddenField3" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlReprintWarning" runat="server" CssClass="warningMsgJobRouting" Style="display:none;height:150px;width:680px">
                    <div id="contextMenuCaution">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Warning</p>
                            <p align="center" style="color: black;font-size:medium;margin:5px 5px 10px 5px;" runat="server" id="txtReprintWarning"></p>
                               <%--Selected document could not be found.--%>							                                                                                                                                                                              
                        </div>
                    </div>                   
                    <div align="center" style="padding-top:15px;">                        
                        <asp:Button ID="btnReprintWarningCancel" runat="server" Text="Cancel" style="font-size:medium;border-radius:5px;width:120px;height:25px;border-style:solid;border-width:1px;" OnClick="btnNoCencelReleaseHold_Click" ></asp:Button>
                    </div>
                    <ajax:modalpopupextender id="ModalReprintWarning" runat="server"
                            popupcontrolid="pnlReprintWarning"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="HiddenField2">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="HiddenField4" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlSelectSpoolFile" runat="server" CssClass="warningMsgJobRouting" Style="display:none;height:130px;width:540px">
                    <div id="contextMenuCaution">
                        <div style="align-self: center">
                            <p style="color: red;font-size:medium" align="center">
                                Select Spool File</p>                                                                                                                                                                                                    
                        </div>
                    </div>
                    <div style="align-self: center;padding-bottom:20px;">                        
                       <asp:DropDownList ID="ddSpoolFileList" Style="width: 520px; height: 22px;margin-left:10px; font-size:small;border-radius:5px;border-color:gray;border-style:solid;border-width:1px;" runat="server"></asp:DropDownList>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnSelectSpoolFileOK" runat="server" Text="Yes" style="float: left; margin-left: 10%;border-radius:5px;width:105px;height:25px;border-style:solid;border-width:1px;" OnClick="btnSelectSpoolFileOK_Click" ></asp:Button>
                        <asp:Button ID="btnSelectSpoolFileCancel" runat="server" Text="No, Cancel" style="float: right; margin-right: 10%;border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" OnClick="btnNoCancelJobAttributes_Click"></asp:Button>
                    </div>
                    <ajax:modalpopupextender id="ModalSelectSpoolFile" runat="server"
                            popupcontrolid="pnlSelectSpoolFile"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="HiddenField5">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="HiddenField5" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlSelectEquipment" runat="server" CssClass="warningMsgJobRouting" Style="display:none;height:140px;width:340px">
                    <div id="contextMenuCaution">
                        <div style="align-self: center">
                            <asp:Label ID="labCautionMessage" align="center" style="margin-left:35px; font-style:italic;background-color:white;color:red;font-size:x-small" runat="server" Text="Duplicate Warning : Duplicate split is being processed"/>
                            <p style="color: black;font-size:medium" align="center">
                                Select Equipment</p>                                                                                                                                                                                                    
                        </div>
                    </div>
                    <div style="align-self: center;padding-bottom:20px;">                        
                       <asp:DropDownList ID="ddEquipmentList" Style="width: 220px; height: 22px;margin-left:60px; font-size:small;border-radius:5px;border-color:gray;border-style:solid;border-width:1px;" runat="server"></asp:DropDownList>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnSelectEquipmentOK" runat="server" Text="Yes" style="float: left; margin-left: 10%;border-radius:5px;width:105px;height:25px;border-style:solid;border-width:1px;" OnClick="btnSelectEquipmentOK_Click"  ></asp:Button>
                        <asp:Button ID="btnSelectEquipmentCancel" runat="server" Text="No, Cancel" style="float: right; margin-right: 10%;border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" OnClick="btnSelectEquipmentCancel_Click" ></asp:Button>
                    </div>
                    <asp:HiddenField ID="HiddenFieldAccountCheck" runat="server"></asp:HiddenField>
                    <ajax:modalpopupextender id="ModalSelectEquipment" runat="server"
                            popupcontrolid="pnlSelectEquipment"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="HiddenField6">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="HiddenField6" runat="server" />
                </asp:Panel>


                 <asp:Panel ID="Pnluser" runat="server" CssClass="warningMsgJobRouting" Style="display:none;height:140px;width:340px">
                    <div id="contextMenuCaution1">
                        <div style="align-self: center">
                            <p style="color: black;font-size:medium" align="center">
                                Please enter Username</p>                                                                                                                                                                                                    
                        </div>
                    </div>
                    <div style="align-self: center;padding-bottom:20px;">                        
                       <asp:TextBox runat="server" id="txtUserName" style=" margin-left:85px"></asp:TextBox>
                        <asp:Label runat="server" id="lblUserMessage" Text="" style=" color:red"></asp:Label>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnUserContinue" runat="server" Text="Continue" style="float: left; margin-left: 10%;border-radius:5px;width:105px;height:25px;border-style:solid;border-width:1px;" OnClick="btnUserContinue_Click"  ></asp:Button>
                        <asp:Button ID="btnUserCancel" runat="server" Text="Cancel" style="float: right; margin-right: 10%;border-radius:5px;width:100px;height:25px;border-style:solid;border-width:1px;" OnClick="btnUserCancel_Click" ></asp:Button>
                    </div>
                   
                    <ajax:modalpopupextender id="ModalPopUpUser" runat="server"
                            popupcontrolid="Pnluser"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="HiddenField9">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="HiddenField9" runat="server" />
                </asp:Panel>


               

                <asp:Panel ID="pnlInventory" runat="server" CssClass="jobInventory" Style="display: none;">     
                    <div align="center">
                        <table>
                            <tr>
                                <td><h2>Job</h2></td>
                                <td style="width: 250px;text-align:left;font-size:15px;padding-left:10px">
                                    <asp:Literal ID="litInvJobNumber" runat="server"></asp:Literal>
                                </td>
                                <td><h2>Product</h2></td>
                                <td style="font-size:15px;width:250px;text-align:left;padding-left:10px">
                                    <asp:Literal ID="litInvProduct" runat="server"></asp:Literal>
                                </td>
                                <td><h2>Split</h2></td>
                                <td style="font-size:15px;width:100px;text-align:left;padding-left:10px"><asp:Literal ID="litInvSplitNumber" runat="server"></asp:Literal></td>
                                <td style="width:250px;text-align:left;padding-left:10px"><h2><asp:Literal ID="litInvCustomer" runat="server"></asp:Literal></h2></td>
                            </tr>
                        </table>
                    </div>   
                    <div align="center">            
                        <fieldset style="border-radius:5px; height: 150px; width:1015px;border: 1px solid black; margin:10px;background-color:rgba(230,230,230,1)">
                            <div align="center" ><h2 style="display:inline-block">Inventory Items</h2></div>
                            <div style="margin-left: 10px;overflow-y: auto;width:1010px; height: 120px;">
                                <asp:GridView ID="gvInventory" runat="server"
                                     AutoGenerateColumns="false"                           
                                                CssClass="baseTable"                           
                                                AllowPaging="false"
                                                AllowSorting="false"
                                                EmptyDataText="No records found."
                                                PageSize="50"
                                                PagerSettings-Mode="NumericFirstLast"
                                                ShowHeaderWhenEmpty="true" Style="font-size:11px">
                                                <Columns>
                                                    <asp:BoundField DataField="SenderItemNumber" HeaderText="Sender Item#" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField DataField="ItemRevision" HeaderText="Rev#" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="ItemNumberQty" HeaderText="Qty Reqd" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="Feeder" HeaderText="Feeder" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="ReceiverItemNumber" HeaderText="Receiver Item#" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="Category" HeaderText="Category" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="SpecialInstruction" HeaderText="Special Instructions" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" /> 
                                               </Columns>
                                </asp:GridView>            
                            </div>
                        </fieldset>                        
                        <fieldset style="border-radius:5px; height: 150px; width:1015px;border: 1px solid black; margin:10px;background-color:rgba(230,230,230,1)">
                            <div align="center" ><h2 style="display:inline-block">Job Instructions</h2></div>
                            <div style="margin-left: 10px;overflow-y: auto;width:1010px; height: 100px;">
                                <asp:GridView ID="gvSpecificInstructions" runat="server"
                                     AutoGenerateColumns="false"                           
                                                CssClass="baseTable"                           
                                                AllowPaging="false"
                                                AllowSorting="false"                                                
                                                PageSize="50"
                                                PagerSettings-Mode="NumericFirstLast"
                                                ShowHeaderWhenEmpty="true" Style="font-size:11px">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Site" ><ItemTemplate><asp:Label ID="Label14" runat="server" Text='<%# Bind("SiteName") %>' Width="70px" style="text-align:center"></asp:Label></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Process (Task)" ><ItemTemplate><asp:Label ID="Label15" runat="server" Text='<%# Bind("ProcessCategory") %>' Width="75px" style="text-align:center"></asp:Label></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Equipment" ><ItemTemplate><asp:Label ID="Label16" runat="server" Text='<%# Bind("Equipment") %>' Width="15px" style="text-align:center"></asp:Label></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Instruction"><ItemTemplate><asp:Label ID="Label17" runat="server" Text='<%# Bind("Instruction") %>' Width="600px" style="text-align:Left;word-wrap:break-word;white-space:normal"></asp:Label></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Last Updated By" ><ItemTemplate><asp:Label ID="Label18" runat="server" Text='<%# Bind("LastUpdatedBy") %>' Width="82px" style="text-align:center"></asp:Label></ItemTemplate></asp:TemplateField>
                                                </Columns>
                                </asp:GridView>            
                            </div>
                        </fieldset>
                    </div>
                    <div>
                        <table>
                            <tr style="width:70%">
                                <td><asp:Button runat="server" Text="Cancel" style="margin-left:480px; border-radius:5px;width:110px;height:25px;border-style:solid;border-width:1px;margin-bottom:10px;" OnClientClick="enableTimer()" OnClick="btnClose_Click"></asp:Button></td>
                                <td style="float:right;margin-left:215px">Only Instructions that apply to this Site and Job<asp:CheckBox ID="cbCurrentSiteInstruction" Text="&#32;" runat="server" Checked="false" AutoPostBack="True" OnCheckedChanged="cbCurrentSiteInstruction_CheckedChanged"></asp:CheckBox></td>
                            </tr>
                        </table>                                                 
                    </div>
                    <ajax:modalpopupextender id="modalInventory" runat="server"
                            popupcontrolid="pnlInventory"
                            dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="HiddenField7">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="HiddenField7" runat="server" />
                </asp:Panel>

                 <asp:Panel ID="PnlManualJobCompletions" runat="server" CssClass="jobHistory" Style="display: none; width:700px">
                     <fieldset title="Manual job Completions">
                         <legend style="text-align:left"> <h2 style="margin-left: -20px;"> Manual job Completions </h2></legend>
                         <table style="width:100%" class="PoupTable">
                             <tr>
                                 <td style="width: 20%"><b>Job Number</b></td>
                                 <td style="width: 30%"><asp:Label runat="server" id="lblJobNumber"></asp:Label></td>
                                   <td style="width: 22%"><b>Product Number</b></td>
                                  <td style="width: 28%"><asp:Label runat="server" id="lblProductNumber"></asp:Label></td>
                             </tr>
                             
                              <tr>
                                 <td><b>Split Number</b></td>
                                  <td><asp:Label runat="server" id="lblSplitNumber"></asp:Label></td>
                                  <td><b>Split (Mail Pieces) Qty</b></td>
                                  <td><asp:Label runat="server" id="lblSplitQty"></asp:Label></td>
                             </tr>
                             
                             <tr>
                               
                                 <td colspan="4"><br /></td>
                             </tr>
                              <tr>
                                 <td><b>Data Environment</b> </td>
                                  <td><asp:Label runat="server" id="lblDataEnvironment"></asp:Label></td>
                                  <td><b>Scheduled Mail Date</b></td>
                                  <td><asp:Label runat="server" id="lblScheduledMailDate"></asp:Label></td>
                             </tr>
                             
                             <tr>
                                 <td><b>Updated On</b></td>
                                  <td><asp:Label runat="server" id="lblUpdatedOn"></asp:Label></td>
                                  <td><b>Insertion Completed On</b></td>
                                  <td><asp:Label runat="server" id="lblInsertionCompletedOn"></asp:Label></td>
                             </tr>
                             
                             <tr>
                                 <td><b>Completed On</b></td>
                                  <td><asp:Label runat="server" id="lblCompletedOn"></asp:Label></td>
                                  <td><b>IsComplete</b></td>
                                  <td><asp:Label runat="server" id="lblIsCompleted"></asp:Label></td>
                             </tr>
                             
                             <tr>
                                 <td><b>Exception Status</b></td>
                                  <td><asp:Label runat="server" id="lblExceptionStatus"></asp:Label></td>
                             </tr>
                             
                            
                         </table>
                         <br />
                         <div><h2 style="margin-left: -20px;">Update</h2></div>
                         <table style="width:100%" class="PoupTable">
                             <tr>
                                  <td style="width:50%"><b><asp:CheckBox runat="server" id="chkInsertedOn" text="Inserted On"></asp:CheckBox></b>
                                    </td>
                                  <td style="width:21%"><b>Select Completion Date</b></td>
                                  <td style="width:30%"><asp:TextBox runat="server" id="txtSelectCompletionDate" class="SelectCompletionDate" AutoPostBack="true" onKeyUp="checkCompletionDate()" OnTextChanged="txtSelectCompletionDate_TextChanged" onkeypress="return isNumber(event)"></asp:TextBox>
                                      <ajax:calendarextender runat="server" id="Calendarextender1" targetcontrolid="txtSelectCompletionDate" />
                                     
                                  </td>
                                  
                             </tr>
                             <tr>
                                  <td><b><asp:CheckBox runat="server" id="chkCompletedOn" text="Completed On"></asp:CheckBox></b></td>
                                  <td><b>Time</b></td>
                                  <td>
                                
                                    <asp:TextBox runat="server"  id="txtTime" class="time" onkeypress="return isNumberTime(event)"></asp:TextBox>
                               
                                      <%--<ajax:TimePicker ID="ftbe" runat="server" TargetControlID="txtTime" /> TextMode="Time" EnableViewState="true"  --%>
                                  </td>
                                  
                             </tr>
                             <tr>
                                 <td colspan="3"><b>Note</b></td>
                             </tr>
                             <tr>
                                 <td colspan="3"><asp:TextBox runat="server" ID="txtNote" MaxLength="120"  TextMode="MultiLine"  class="note" onKeyUp="checkCompletionNote()" />
                                     <ajax:TextBoxWatermarkExtender ID="wmr"  runat="server" TargetControlID="txtNote" WatermarkText="Must enter a note to Activate “Apply Status” button" WatermarkCssClass="txtwatermark"></ajax:TextBoxWatermarkExtender> 
                                  
                                     <%--AutoPostBack="true"  OnTextChanged="txtNote_TextChanged"--%>
                                 </td>
                             </tr>
                         </table>
                         <br />
                         <table style="width:100%" align="center" class="PoupTable">
                             <tr>
                                 <td colspan="2" align="center"><asp:Button runat="server" id="btnJobCompletionsApplyStatus" Text="Apply Status"  OnClick="btnJobCompletionsApplyStatus_Click"></asp:Button>
                                     <asp:Button runat="server" id="btnJobCompletionsCancel" Text="Cancel"  OnClick="btnJobCompletionsCancel_Click"></asp:Button>
                                 </td>
                               
                             </tr>
                         </table>
                     </fieldset>
                       
                     <br />
                     
                     <ajax:modalpopupextender id="ModalPopUpManualJobCompletions" runat="server"
                            popupcontrolid="PnlManualJobCompletions"
                          dropshadow="True" drag="False" backgroundcssclass="modalBackground"
                            targetcontrolid="HiddenField13">
                    </ajax:modalpopupextender>
                    <asp:HiddenField ID="HiddenField13" runat="server" />
                 </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:panel>
    <asp:updateprogress id="UpdateProgress1" runat="server" associatedupdatepanelid="UpdatePanelExecuteSearch">
    <ProgressTemplate>
        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: white; opacity: 0.8;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/loading_circle.gif" ToolTip="Loading ..." style="padding: 10px;position:fixed;top:45%;left:50%;width: 50px; height:50px;background-color: white;"/>
        </div>
    </ProgressTemplate>
</asp:updateprogress>
    <%-- <asp:TextBox runat="server"  id="TextBox1" class="time" TextMode="Time" EnableViewState="false"></asp:TextBox>--%>
</asp:Content>
