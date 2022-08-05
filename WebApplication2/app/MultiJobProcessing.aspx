<%@ page language="C#" masterpagefile="~/site.master" autoeventwireup="true" codebehind="MultiJobProcessing.aspx.cs" inherits="WMS.Gui.app.MultiJobProcessing" enableeventvalidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="server">

    <style type="text/css">
        #page {
            min-height: 945px;
        }

        .baseTable {
            margin: 0px 15px 15px 10px;
        }

        .btn_fileInfo {
            background-color: darkgray;
            /*margin-left: 210px;*/
            font-size: xx-small;
            border-style: solid;
            border-width: 1px;
            border-radius: 5px;
            height: 22px;
            width: 120px;
            border-color: gray;
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

        .alignLe {
            margin-left: 13px;
            font-size: 12px;
        }

        #notification {
            /* top: 0px;
            width: 65%;
            text-align: center;
            font-weight: normal;
            font-size: 14px;
            font-weight: bold;
            color: white;
            background-color: blue;
            padding: 5px;*/
            display: inline-block;
        }

        .contWidth {
            width: 20px;
        }

        .repoTable {
            border: 1px solid black;
            text-align: left;
            /*width:45px !important;*/
        }

            .repoTable td {
                border: 1px solid black;
            }
    </style>
    <div class="col-md-12">
        <%-- <div style="float: right; margin-top: -27px; margin-right: 19px;">--%>
        <h2 style="font-size: 16px; font-weight: bold;">Multi-Job Processing</h2>
        <center>
            <table style="width: 90%; border: solid 3px black;padding:20px;">
                <tr>
                    <td style="width: 40%;">
                        <table style="padding: 10px;margin:20px;width:90%;" border="1">
                            <tr>
                                <th>Status</th>
                                <th>Next Task</th>
                                <th>Equipment</th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:label id="lblStatus">A</asp:label></td>
                                <td>
                                    <asp:label id="lblNextTask">B</asp:label></td>
                                <td>
                                    <asp:label id="lblEquipment">C</asp:label></td>
                            </tr>
                            </table>
                        <center>
                            <div style="margin:20px;">
                                 
                                    <asp:RadioButtonList id="rdbBarcode" runat="server" BorderColor="Black" BorderStyle="Solid" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" selected="true">3of9 Barcode</asp:ListItem>
                                        <asp:ListItem Value="2">2d Barcode</asp:ListItem>
                                    </asp:RadioButtonList>
                                
                            </div>
                        </center>
                    </td>
                    <td>
                        <table style="padding: 10px;margin:10px;width:90%;">
                            <tr>
                                <td style="width:33%;">
                                    <asp:Button runat="server" id="btnStartSccaning" style="width:150px;height:27px;font-size:x-small;margin-top: 4px" Text="Start Sccaning" />
                                </td>
                                <td style="width:33%;">
                                    <asp:Label runat="server" id="lblToyalJobs" Text="Total Jobs"></asp:Label>&nbsp;&nbsp;

                                <asp:TextBox runat="server" id="txtTotalJobs" style="width: 50px;"></asp:TextBox>

                                </td>
                                <td>
                                    <asp:Label runat="server" id="lblRemainingJobs" Text="Remaining Jobs"></asp:Label>&nbsp;&nbsp;
 <asp:TextBox runat="server" id="txtRemainingJobs" style="width: 50px;"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table border="1" style="margin-top:10px;margin-bottom:10px;width:100%;">
                                        <tr>
                                            <th>Processing Client Name</th>
                                            <th>Job</th>
                                            <th>Product</th>
                                            <th>Split</th>
                                            <th>Qty</th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" id="lblProcessingClient">A</asp:Label></td>
                                            <td>
                                                <asp:Label runat="server" id="lblJob"></asp:Label>B</td>
                                            <td>
                                                <asp:Label runat="server" id="lblProduct">C</asp:Label></td>
                                            <td>
                                                <asp:Label runat="server" id="lblSplit">D</asp:Label></td>
                                            <td>
                                                <asp:Label runat="server" id="lblQty">E</asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="server" id="btnStartProcessing" style="width:150px;height:27px;font-size:x-small;margin-top: 4px" Text="Start Processing" />
                                </td>
                                <td>
                                    <asp:Button runat="server" id="btnStopProcessing" style="width:150px;height:27px;font-size:x-small;margin-top: 4px" Text="Stop Processing" />
                                </td>
                                <td>
                                    <asp:Button runat="server" id="btnCancel" style="width:150px;height:27px;font-size:x-small;margin-top: 4px" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table><br />
            <center>            
                <div style="text-align: center;">
                <asp:RadioButtonList id="rdbQueued" runat="server" BorderColor="Black" BorderStyle="Solid" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Selected="True">Queued</asp:ListItem>
                    <asp:ListItem Value="2">Processed</asp:ListItem>
                    <asp:ListItem Value="2">Error or requires individual processing</asp:ListItem>
                </asp:RadioButtonList>
            </div>
                </center>
            <asp:GridView ID="gvMultiJobProcessing"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="baseTable"
                AllowPaging="True"
                AllowSorting="True"
                PageSize="10"
                EmptyDataText="No records found."
                DataKeyNames="Id"
                ShowHeaderWhenEmpty="True">
                <columns>

                                         <asp:TemplateField HeaderText="DPrintJobId">
                                            <ItemTemplate>
                                                <asp:Label ID="Label19" runat="server" Text='<%# Bind("Id") %>' Width="60px" Style="text-align: left"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FromSite">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("FromSite") %>' Width="60px" Style="text-align: left"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Client Name" >
                                            <ItemTemplate>
                                                <asp:Label ID="Label20" runat="server" Text='<%# Bind("ClientName") %>' Width="110px" Style="text-align: left"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("Job") %>' Width="40px" Style="text-align: left"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("Product") %>' Width="40px" Style="text-align: left"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Split" >
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("Split") %>' Width="30px" Style="text-align: left"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Qty" >
                                            <ItemTemplate>
                                                <asp:Label ID="Label21" runat="server" Text='<%# Bind("Quantity") %>' Width="30px" Style="text-align: left"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sheets">
                                            <ItemTemplate>
                                                <asp:Label ID="Label8" runat="server" Text='<%# Bind("Sheets") %>' Width="50px" Style="text-align: left"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Last Updated">
                                            <ItemTemplate>
                                                <asp:Label ID="Label22" runat="server"  Text='<%# Bind("LastUpdated") %>' Width="50px" Style="text-align: left"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="Label11" runat="server" Text='<%# Bind("Status") %>' Width="90px" Style="text-align: left"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                     
                                    </columns>
            </asp:GridView>
            </center>
        <%-- </div>--%>
    </div>
</asp:Content>
