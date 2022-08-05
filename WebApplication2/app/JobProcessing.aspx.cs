using CommonUtilBase;
using DpJobStatus.DataAccess;
using DpJobStatus.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Management;
using System.Messaging;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using WMS.Gui.code;
using WMS.Gui.DataAccess;
using WMS.Gui.Model;
using static DpJobStatus.Model.TrakingTask;

namespace WMS.Gui.app
{
    public partial class JobProcessingPage : System.Web.UI.Page
    {
        static readonly char[] SPLITTER = new char[] { ',' };
        
        private string backFromVirtualInserter
        {
            get { return Convert.ToString(Session["backFromVirtualInserter_Session"]); }
            set { Session["backFromVirtualInserter_Session"] = value; }
        }

        private JobProcessingSearchBy DpSearchByDataSession
        {
            get { return (JobProcessingSearchBy)Session["dprint_searchby"]; }
            set { Session["dprint_searchby"] = value; }
        }

        private List<int> sellectedIndedSession
        {
            get { return (List<int>)Session["sellected_Inded_Session"]; }
            set { Session["sellected_Inded_Session"] = value; }
        }

        private string sellectedStatusSession
        {
            get { return (string)Session["sellected_Status_Session"]; }
            set { Session["sellected_Status_Session"] = value; }
        }

        private RowColor rowColorSession
        {
            get { return (RowColor)Session["row_Color_Session"]; }
            set { Session["row_Color_Session"] = value; }
        }

        private string equipmentNameSession
        {
            get { return Convert.ToString(Session["equipment_Session"]); }
            set { Session["equipment_Session"] = value; }
        }
        private string SortExp
        {
            get { return (string)ViewState["sortExp"]; }
            set { ViewState["sortExp"] = value; }
        }

        private string SiteIdSession
        {
            //get { return Convert.ToInt32(Session["Location"]); }
            get { return Convert.ToString(Session["Location"]); }
        }

        private byte[] PdfData
        {
            get { return (byte[])(Session["BinaryData"]); }
            set { Session["BinaryData"] = value; }
        }

        private string equipmentVertual
        {
            get { return Convert.ToString(Session["vertual_equipment_Session"]); }
            set { Session["vertual_equipment_Session"] = value; }
        }

        private bool virtualInserter
        {
            get { return (bool)(Session["vertual_Inserter_Session"]); }
            set { Session["vertual_Inserter_Session"] = value; }
        }

        private JobAttributes jobAttributesSession
        {
            get { return (JobAttributes)Session["jobAttributes_Session"]; }
            set { Session["jobAttributes_Session"] = value; }
        }

        private string duplicateFoundFromSubMenu
        {
            get { return Convert.ToString(Session["duplicateFoundFromSubMenu_Session"]); }
            set { Session["duplicateFoundFromSubMenu_Session"] = value; }
        }
        private string duplicateFoundFromNextClick
        {
            get { return Convert.ToString(Session["duplicateFoundFromNextClick_Session"]); }
            set { Session["duplicateFoundFromNextClick_Session"] = value; }
        }
        private List<JobProcessingModel> DpJobsSession
        {
            get { return (List<JobProcessingModel>)Session["dprint_items"]; }
            set { Session["dprint_items"] = value; }
        }

        private List<JobProcessingModel> DpSelectedJobSession
        {
            get { return (List<JobProcessingModel>)Session["dprint_selected_items"]; }
            set { Session["dprint_selected_items"] = value; }
        }

        //private int selectedIndex
        //{
        //    get { return (int)Session["selected_index"]; }
        //    set { Session["selected_index"] = value; }
        //}

        private int selectedDprintJobsId
        {
            get { return (int)Session["selected_DprintJobsId"]; }
            set { Session["selected_DprintJobsId"] = value; }
        }

        private bool? callFromRightarrow
        {
            get { return (bool)(Session["callFromRightarrow_Session"]); }
            set { Session["callFromRightarrow_Session"] = value; }
        }

        private string ddEquipmentSelected { get; set; }
        private JobInstructionMasterProvider _provider = null;

        private string StatusSession
        {
            get { return (string)Session["StatusSession"]; }
            set { Session["StatusSession"] = value; }
        }

        private string JobSizeThreshold
        {
            get { return Convert.ToString(Session["JobSizeThresholdSession"]); }
            set { Session["JobSizeThresholdSession"] = value; }
        }
        private bool LargeJobs
        {
            get { return (bool)(Session["LargeJobsSession"]); }
            set { Session["LargeJobsSession"] = value; }
        }
        private bool SmallJobs
        {
            get { return (bool)(Session["SmallJobsSession"]); }
            set { Session["SmallJobsSession"] = value; }
        }
        private bool ProductionJobs
        {
            get { return (bool)(Session["ProductionJobsSession"]); }
            set { Session["ProductionJobsSession"] = value; }
        }
        private bool TestJobs
        {
            get { return (bool)(Session["TestJobsSession"]); }
            set { Session["TestJobsSession"] = value; }
        }
        private bool CompletedJobsOnly
        {
            get { return (bool)(Session["CompletedJobsOnlySession"]); }
            set { Session["CompletedJobsOnlySession"] = value; }
        }
        private string ScheduledBeginningDate
        {
            get { return Convert.ToString(Session["ScheduledBeginningDateSession"]); }
            set { Session["ScheduledBeginningDateSession"] = value; }
        }
        private string ScheduledEndingDate
        {
            get { return Convert.ToString(Session["JScheduledEndingDateSession"]); }
            set { Session["JScheduledEndingDateSession"] = value; }
        }

        private bool DontFilterbyJobAttributeJobs
        {
            get { return (bool)(Session["DontFilterbyJobAttributeJobsSession"]); }
            set { Session["DontFilterbyJobAttributeJobsSession"] = value; }
        }



        protected void Page_Load(object sender, EventArgs e)
        {

            spnlErrorMessage.Text = "";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "<script>ClearErrorMsg();</script>", false);
            lblUserMessage.Text = "";
            ViewState["sortOrder"] = "desc";
            btnMoveAll.Enabled = false;
            btnUnSelectAll.Enabled = false;
            btnSelectAll.Enabled = false;
            ddProcessingLocation.Enabled = false;
            btnMoveToBack.Enabled = false;
            ddStatus.Enabled = false;
            // btnResetLocalSetting.Enabled = false;
            btnReprintJobTicket.Enabled = false;
            btnApplyStatus.Enabled = false;
            ddEquipmentId.Enabled = false;
            ddEquipmentIdProcessJob.Enabled = false;
            //Client Name
            ddlClient.Enabled = false;
            hfOparator.Value = "0";
            btnChangePriority.Enabled = false;
            btnUpdateCompletedOnDate.Enabled = false;
            btnApplyYes.Enabled = false;
            btnExpContinue.Enabled = false;

            btnConfirmJobRouting.Enabled = false;
            btnJobRouting.Enabled = false;
            ddPaperJob.Enabled = false;
            ddTonerJob.Enabled = false;
            ddEngineJob.Enabled = false;
            btnYesJobAttributes.Enabled = false;
            btnReleaseHold.Enabled = false;
            btnResendSpoolFile.Enabled = false;
            btnResendInserterControlFile.Enabled = false;
            btnMoveToBack.Enabled = false;
            ddPopStatus.Enabled = false;
            btnSaveLocalSetting.Enabled = false;
            btnRetrieveLocalSetting.Enabled = false;
            //if (!string.IsNullOrEmpty(SiteIdSession))
            //{
            //    var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            //    ddLocation.SelectedIndex = Convert.ToInt32(SiteIdSession);
            //}

            if (WebSession.User.HasPermissionExplicit("JobProcessing.Manager"))
            {
                ddProcessingLocation.Enabled = true;
                btnMoveToBack.Enabled = true;
                ddStatus.Enabled = true;
                //btnResetLocalSetting.Enabled = true;
                btnReprintJobTicket.Enabled = true;
                btnApplyStatus.Enabled = true;
                ddEquipmentId.Enabled = true;
                //Client Name
                ddlClient.Enabled = true;
                ddEquipmentIdProcessJob.Enabled = true;
                btnChangePriority.Enabled = true;
                btnUpdateCompletedOnDate.Enabled = true;
                btnApplyYes.Enabled = true;
                btnExpContinue.Enabled = true;

                btnConfirmJobRouting.Enabled = true;
                btnJobRouting.Enabled = true;
                ddPaperJob.Enabled = true;
                ddTonerJob.Enabled = true;
                ddEngineJob.Enabled = true;
                btnYesJobAttributes.Enabled = true;
                btnReleaseHold.Enabled = true;
                btnResendSpoolFile.Enabled = true;
                btnResendInserterControlFile.Enabled = true;
                btnMoveToBack.Enabled = true;
                ddPopStatus.Enabled = true;
                btnMoveAll.Enabled = true;
                btnUnSelectAll.Enabled = true;
                btnSelectAll.Enabled = true;
                btnSaveLocalSetting.Enabled = true;
                btnRetrieveLocalSetting.Enabled = true;
            }
            if (WebSession.User.HasPermissionExplicit("JobProcessing.Supervisor"))
            {
                ddProcessingLocation.Enabled = true;
                btnMoveToBack.Enabled = true;
                ddStatus.Enabled = true;
                // btnResetLocalSetting.Enabled = true;
                btnReprintJobTicket.Enabled = true;
                btnApplyStatus.Enabled = true;
                ddEquipmentId.Enabled = true;
                //Client Name
                ddlClient.Enabled = true;
                ddEquipmentIdProcessJob.Enabled = true;
                btnChangePriority.Enabled = true;
                btnUpdateCompletedOnDate.Enabled = true;
                btnApplyYes.Enabled = true;
                btnExpContinue.Enabled = true;

                btnConfirmJobRouting.Enabled = true;
                btnJobRouting.Enabled = true;
                ddPaperJob.Enabled = true;
                ddTonerJob.Enabled = true;
                ddEngineJob.Enabled = true;
                btnYesJobAttributes.Enabled = true;
                btnReleaseHold.Enabled = true;
                btnResendSpoolFile.Enabled = true;
                btnResendInserterControlFile.Enabled = true;
                btnMoveToBack.Enabled = true;
                ddPopStatus.Enabled = true;
                btnMoveAll.Enabled = true;
                btnUnSelectAll.Enabled = true;
                btnSelectAll.Enabled = true;
                btnSaveLocalSetting.Enabled = true;
                btnRetrieveLocalSetting.Enabled = true;
            }
            if (WebSession.User.HasPermissionExplicit("JobProcessing.LeadOperator"))
            {
                ddProcessingLocation.Enabled = true;
                ddStatus.Enabled = true;
                // btnResetLocalSetting.Enabled = true;
                ddEquipmentId.Enabled = true;
                //Client Name
                ddlClient.Enabled = true;
                ddEquipmentIdProcessJob.Enabled = true;
                btnMoveToBack.Enabled = true;
                btnExpContinue.Enabled = true;

                btnChangePriority.Enabled = true;
                ddPaperJob.Enabled = true;
                ddTonerJob.Enabled = true;
                ddEngineJob.Enabled = true;
                btnYesJobAttributes.Enabled = true;
                btnReleaseHold.Enabled = true;
                btnResendSpoolFile.Enabled = true;
                btnResendInserterControlFile.Enabled = true;
                btnMoveToBack.Enabled = true;
                ddPopStatus.Enabled = true;
                btnApplyStatus.Enabled = true;
                btnMoveAll.Enabled = true;
                btnUnSelectAll.Enabled = true;
                btnSelectAll.Enabled = true;
                btnSaveLocalSetting.Enabled = true;
                btnRetrieveLocalSetting.Enabled = true;
            }

            if (WebSession.User.HasPermissionExplicit("JobProcessing.Operator"))
            {
                hfOparator.Value = "1";
                btnReleaseHold.Enabled = true;
                btnMoveAll.Enabled = true;
                btnUnSelectAll.Enabled = true;
                btnSelectAll.Enabled = true;
                btnSaveLocalSetting.Enabled = false;
                btnRetrieveLocalSetting.Enabled = true;
                ddlClient.Enabled = false;
                ddEquipmentId.Enabled = false;
                ddStatus.Enabled = false;
                ddProcessingLocation.Enabled = false;
                btnChangeJobDates.Enabled = false;
            }

            CustomerNameText.Text = "";



            upDpJobStatus.Attributes["oncontextmenu"] = "return false;";

            if (!Page.IsPostBack)
            {

                hdnReturnToViewFlag.Value = "0";
                ViewState["Duplicate"] = 0;
                this.LoadLocation();
                this.LoadStatus();
                this.LoadPopStatus();
                //Client Number and Name DropDownList Binding
                this.LoadClient();
                if (ddEquipmentId.Items.Count == 0)
                    ddEquipmentId.Items.Add(new ListItem("N/A", "N/A"));

                gvJobInstructions.Columns[1].Visible = false;
                gvJobInstructions.Columns[2].Visible = false;


                var jd = new JobProcessingDataAdapter();
                if (!jd.FetchAdministrator(WebSession.User.LoginId))
                {
                    var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
                    ddLocation.Enabled = false;
                }
                ddProcessingLocation.SelectedValue = Session["Location"].ToString();
                if (Session["virtualInserter"] != null)
                {
                    LodEquipmentIdDetails();
                    Session["virtualInserter"] = null;
                }
                else
                {
                    if (WebSession.User.HasPermissionExplicit("JobProcessing.LeadOperator") || WebSession.User.HasPermissionExplicit("JobProcessing.Operator"))
                    {
                        btnRetrieveLocalSetting_Clicked(null, null);
                    }
                }

                if (WebSession.User.HasPermissionExplicit("JobProcessing.Operator"))
                    ResetSearch.Enabled = false;
                if (WebSession.User.HasPermissionExplicit("JobProcessing.Operator"))
                {
                    cbFilterByAttributes.Enabled = true;
                    cbFilterByAttributes.Checked = true;
                }
                else
                {
                    cbFilterByAttributes.Enabled = false;
                }
                //if (Request.QueryString["fromReprocessing"] != null)
                //    BtnProcessJobSplitOnebyOne();
            }

            //labTotalSheets.Text = TotalSheetsLabString + " " + ddStatus.SelectedItem.Text;
            //labTotalSplitQty.Text = TotalSplitQtyLabString + " " + ddStatus.SelectedItem.Text;
            //labNumberOfSplits.Text = NumOfSplitLabString + " " + ddStatus.SelectedItem.Text;              

            labStatus1.Text = ddStatus.SelectedItem.Text;
            labStatus2.Text = ddStatus.SelectedItem.Text;
            labStatus3.Text = ddStatus.SelectedItem.Text;

            if (backFromVirtualInserter == "Y")
            {
                ctlTimer_Tick(null, null);
                SetHeaderControlsFromSession();
                backFromVirtualInserter = string.Empty;
            }

            SaveLocalSettingEnableDesable();
            btnMoveAll.Enabled = false;
            //if (Session["virtualInserter"] != null)
            //{
            //    LodEquipmentIdDetails();
            //    Session["virtualInserter"] = null;
            //}
            ddProcessingLocation.Enabled = false;


        }
        private void LodEquipmentIdDetails()
        {
            var jd = new JobProcessingDataAdapter();
            UserDeviceToEquipment userDeviceToEquipment = new UserDeviceToEquipment();
            userDeviceToEquipment = (UserDeviceToEquipment)Session["virtualInserter"];

            ddStatus.SelectedValue = userDeviceToEquipment.DPrintTaskMasterID.ToString();
            var selectedStatusId = userDeviceToEquipment.DPrintTaskMasterID.ToString();
            ddEquipmentId.Items.Clear();
            Dictionary<string, string> equipmentIdDict = new Dictionary<string, string>();
            if (userDeviceToEquipment.DPrintTaskMasterID == 15 || userDeviceToEquipment.DPrintTaskMasterID == 17)
            {
                EquipmentIdDataAdapter equipmentIdDataAdapter = new EquipmentIdDataAdapter();
                Collection<EquipmentId> equipmentIdCollection = equipmentIdDataAdapter.GetEquipmentIdByTask(Int32.Parse(selectedStatusId), Int32.Parse(userDeviceToEquipment.SiteID.ToString()));

                if (equipmentIdCollection.Count > 0)
                {
                    foreach (var equipmentId in equipmentIdCollection)
                    {
                        ddEquipmentId.Items.Add(new ListItem(equipmentId.EquipmentName, equipmentId.Id));
                    }
                    setEquipmentAtr(selectedStatusId);
                }
                else
                {
                    ddEquipmentId.Items.Add(new ListItem("N/A", "N/A"));
                }
            }
            else
            {
                ddEquipmentId.Items.Add(new ListItem("N/A", "N/A"));
            }

            if (userDeviceToEquipment.Equipment != null)
            {
                ddEquipmentId.ClearSelection();
                ddEquipmentId.Items.FindByText(userDeviceToEquipment.Equipment.ToString()).Selected = true;
            }


            ddProcessingLocation.SelectedValue = userDeviceToEquipment.SiteID.ToString();

            labStatus1.Text = ddStatus.SelectedItem.Text;
            labStatus2.Text = ddStatus.SelectedItem.Text;
            labStatus3.Text = ddStatus.SelectedItem.Text;
            setEquipmentAtr(userDeviceToEquipment.DPrintTaskMasterID.ToString());
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string RetrieveLocalStorage(string location, string status, string eqId, bool showCmp)
        {

            EquipmentIdDataAdapter equipmentIdDataAdapter = new EquipmentIdDataAdapter();
            Collection<EquipmentId> equipmentIdCollection = equipmentIdDataAdapter.GetEquipmentIdByTask(Int32.Parse(status), Int32.Parse(location));

            if (equipmentIdCollection == null)
            {
                equipmentIdCollection.Add(new EquipmentId() { EquipmentName = "N/A" });
            }
            return new JavaScriptSerializer().Serialize(equipmentIdCollection);

        }

        private void LoadLocation()
        {
            SiteDataAdapter sd = new SiteDataAdapter();
            var sitesCollection = sd.GetAllActiveSites();
            Dictionary<string, string> sites = new Dictionary<string, string>();
            sites.Add("All", "0");
            foreach (var dpSite in sitesCollection)
                sites.Add(dpSite.Name, Convert.ToString(dpSite.Id));

            ddProcessingLocation.DataSource = sites;
            ddProcessingLocation.DataTextField = "Key";
            ddProcessingLocation.DataValueField = "Value";
            ddProcessingLocation.DataBind();
        }

        private void LoadStatus()
        {
            StatusDataAdapter statusDataAdapter = new StatusDataAdapter();
            Collection<Status> statusCollection = statusDataAdapter.GetAll();

            Dictionary<string, string> statusDict = new Dictionary<string, string>();

            statusDict.Add("All", "0");
            foreach (var status in statusCollection)
            {
                statusDict.Add(status.Task, Convert.ToString(status.ID));
            }

            ddStatus.DataSource = statusDict;
            ddStatus.DataTextField = "Key";
            ddStatus.DataValueField = "Value";
            ddStatus.DataBind();
        }

        //Client Number and Name DropDownList Binding
        private void LoadClient()
        {
            _provider = new JobInstructionMasterProvider(WebSession.User);
            DataTable _lstClientNumber = _provider.FetchDynamicDropdownValues(ddlClient.SelectedValue.ToInt());

            Dictionary<string, string> baseList = _lstClientNumber.AsEnumerable().ToDictionary(x => string.Format("{0} ({1})", x.Field<string>("ClientName"), x.Field<string>("ClientNumber")), x => x.Field<string>("ClientNumber"));
            string AllKeyMatch = "ALL (-1)";
            KeyValuePair<string, string> kv = new KeyValuePair<string, string>(
           baseList.FirstOrDefault(x => x.Key == AllKeyMatch).Key,
             baseList.FirstOrDefault(x => x.Key == AllKeyMatch).Value);

            baseList.Remove(baseList.FirstOrDefault(x => x.Key == AllKeyMatch).Key);
            Dictionary<string, string> finalList = new Dictionary<string, string>();
            finalList.Add(kv.Key.Replace(" (-1)", ""), kv.Value);
            foreach (var item in baseList.OrderBy(key => key.Key))
            {
                finalList.Add(item.Key, item.Value);
            }

            ddlClient.DataSource = finalList;
            ddlClient.DataTextField = "Key";
            ddlClient.DataValueField = "Value";
            ddlClient.DataBind();
        }

        private void LoadAltActiveSites(int excludeSiteId)
        {
            SiteDataAdapter sd = new SiteDataAdapter();
            var sitesCollection = sd.GetAllAltActiveSites();
            Dictionary<string, string> sites = new Dictionary<string, string>();
            foreach (var dpSite in sitesCollection)
            {
                if (dpSite.Id != excludeSiteId)
                    sites.Add(dpSite.Name, Convert.ToString(dpSite.Id));
            }

            ddDpRoutingSites.DataSource = sites;
            ddDpRoutingSites.DataTextField = "Key";
            ddDpRoutingSites.DataValueField = "Value";
            ddDpRoutingSites.DataBind();
        }
        private void LoadPopStatus()
        {
            StatusDataAdapter statusDataAdapter = new StatusDataAdapter();
            Collection<Status> statusCollection = statusDataAdapter.GetAllActiveStatus();

            Dictionary<string, string> statusDict = new Dictionary<string, string>();

            foreach (var status in statusCollection)
            {
                statusDict.Add(status.Task, Convert.ToString(status.ID));
            }

            ddPopStatus.DataSource = statusDict;
            ddPopStatus.DataTextField = "Key";
            ddPopStatus.DataValueField = "Value";
            ddPopStatus.DataBind();
        }

        private void LoadCancellePopStatus()
        {
            StatusDataAdapter statusDataAdapter = new StatusDataAdapter();
            Collection<Status> statusCollection = statusDataAdapter.CancelleStatusGetAll(); ;

            Dictionary<string, string> statusDict = new Dictionary<string, string>();

            foreach (var status in statusCollection)
            {
                statusDict.Add(status.Task, Convert.ToString(status.ID));
            }

            ddPopStatus.DataSource = statusDict;
            ddPopStatus.DataTextField = "Key";
            ddPopStatus.DataValueField = "Value";
            ddPopStatus.DataBind();
        }

        protected void setDdEquipmentId(object sender, EventArgs e)
        {
            txtInputPaperModulesJob.Text = string.Empty;
            txtTonerTypeJob.Text = string.Empty;
            txtPrintEngineNgOrSheetCodeJob.Text = string.Empty;

            txtEqInputPaperModules.Text = string.Empty;
            txtEqTonerTypeOrSheetCode.Text = string.Empty;
            txtEqPrintEngineOrCategory.Text = string.Empty;

            labInputPaperModulesJob.Text = string.Empty;
            labTonerTypeJob.Text = string.Empty;
            labPrintEngineNgOrSheetCodeJob.Text = string.Empty;

            labEqInputPaperModules.Text = string.Empty;
            labEqTonerTypeOrSheetCode.Text = string.Empty;
            labEqPrintEngineOrCategory.Text = string.Empty;

            var selectedStatusId = ddStatus.SelectedValue;
            ddEquipmentId.Items.Clear();
            Dictionary<string, string> equipmentIdDict = new Dictionary<string, string>();
            if (selectedStatusId.Equals("15") || selectedStatusId.Equals("17"))
            {
                EquipmentIdDataAdapter equipmentIdDataAdapter = new EquipmentIdDataAdapter();
                Collection<EquipmentId> equipmentIdCollection = equipmentIdDataAdapter.GetEquipmentIdByTask(Int32.Parse(selectedStatusId), Int32.Parse(ddProcessingLocation.SelectedValue));

                if (equipmentIdCollection.Count > 0)
                {
                    foreach (var equipmentId in equipmentIdCollection)
                    {
                        ddEquipmentId.Items.Add(new ListItem(equipmentId.EquipmentName, equipmentId.Id));
                    }
                    //if (selectedStatusId.Equals("15"))
                    //    LoadJobAtbWaitingToPrint();
                    //else if (selectedStatusId.Equals("17"))
                    //    LoadJobAtbWaitingToInsert();
                    //else
                    //    UnloadJobAtbDropwond();
                    setEquipmentAtr(selectedStatusId);
                }
                else
                {
                    ddEquipmentId.Items.Add(new ListItem("N/A", "N/A"));
                }
            }
            else
            {
                ddEquipmentId.Items.Add(new ListItem("N/A", "N/A"));
                cbFilterByAttributes.Enabled = false;
                cbFilterByAttributes.Checked = false;
            }
            ddEquipmentId.DataTextField = "Text";
            ddEquipmentId.DataValueField = "Value";
            ddEquipmentId.DataBind();

            //if (selectedStatusId.Equals("15"))
            //    LoadJobAtbWaitingToPrint();
            //else if (selectedStatusId.Equals("17"))
            //    LoadJobAtbWaitingToInsert();
            //else
            //    UnloadJobAtbDropwond();
            //
            btnSaveLocalSetting.Enabled = true;


            SaveLocalSettingEnableDesable();

            gvDpJobs.DataSource = null;
            gvDpJobs.DataBind();

            gvJobInstructions.DataSource = null;
            gvJobInstructions.DataBind();

        }

        private void LoadClientInfo()
        {
            using (var clientInfroProvider = new DpPrintStatusDataAdapter())
            {
                ddlClient.DataSource = clientInfroProvider.GetClientList();
                ddlClient.DataTextField = "ClientName";
                ddlClient.DataValueField = "ClientNumber";
                ddlClient.DataBind();
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ExportGridToExcel();
        }

        protected void btnResetLocalSetting_Clicked(object sender, EventArgs e)
        {
            DpJobsSession = null;
            gvDpJobs.DataSource = null;
            gvDpJobs.DataBind();

            gvPopUpWindows.DataSource = null;
            DpSelectedJobSession = null;
            gvPopUpWindows.DataBind();

            ddProcessingLocation.SelectedValue = Location.All.ToString();
            ddStatus.SelectedIndex = 0;
            txtJobNumber.Text = string.Empty;
            txtBeginningDate.Text = string.Empty;
            txtEndingDate.Text = string.Empty;
            ddEquipmentId.Items.Clear();
            ddEquipmentId.Items.Add(new ListItem("N/A", "N/A"));
            ddEquipmentId.SelectedIndex = 0;
            //ddlClient.Items.Clear();
            //ddlClient.Items.Add(new ListItem("N/A", "N/A"));
            ddlClient.SelectedIndex = 0;

            txtInputPaperModulesJob.Text = string.Empty;
            txtTonerTypeJob.Text = string.Empty;
            txtPrintEngineNgOrSheetCodeJob.Text = string.Empty;

            txtEqInputPaperModules.Text = string.Empty;
            txtEqTonerTypeOrSheetCode.Text = string.Empty;
            txtEqPrintEngineOrCategory.Text = string.Empty;

            labInputPaperModulesJob.Text = string.Empty;
            labTonerTypeJob.Text = string.Empty;
            labPrintEngineNgOrSheetCodeJob.Text = string.Empty;

            labEqInputPaperModules.Text = string.Empty;
            labEqTonerTypeOrSheetCode.Text = string.Empty;
            labEqPrintEngineOrCategory.Text = string.Empty;
        }

        protected void ResetSearch_Click(object sender, ImageClickEventArgs e)
        {
            gvDpJobs.DataSource = null;
            gvDpJobs.DataBind();
            //ddProcessingLocation.SelectedValue = Location.All.ToString();
            ddStatus.SelectedIndex = 0;
            txtJobNumber.Text = string.Empty;
            txtBeginningDate.Text = string.Empty;
            txtEndingDate.Text = string.Empty;
            ddEquipmentId.Items.Clear();
            ddEquipmentId.Items.Add(new ListItem("N/A", "N/A"));
            ddEquipmentId.SelectedIndex = 0;
            //ddlClient.Items.Clear();
            //ddlClient.Items.Add(new ListItem("N/A", "N/A"));
            ddlClient.SelectedIndex = 0;

            txtInputPaperModulesJob.Text = string.Empty;
            txtTonerTypeJob.Text = string.Empty;
            txtPrintEngineNgOrSheetCodeJob.Text = string.Empty;

            txtEqInputPaperModules.Text = string.Empty;
            txtEqTonerTypeOrSheetCode.Text = string.Empty;
            txtEqPrintEngineOrCategory.Text = string.Empty;

            labInputPaperModulesJob.Text = string.Empty;
            labTonerTypeJob.Text = string.Empty;
            labPrintEngineNgOrSheetCodeJob.Text = string.Empty;

            labEqInputPaperModules.Text = string.Empty;
            labEqTonerTypeOrSheetCode.Text = string.Empty;
            labEqPrintEngineOrCategory.Text = string.Empty;

            ddPaperJob.SelectedValue = "0";
            ddTonerJob.SelectedValue = "0";
            ddEngineJob.SelectedValue = "0";

            labStatus1.Text = ddStatus.SelectedItem.Text;
            labStatus2.Text = ddStatus.SelectedItem.Text;
            labStatus3.Text = ddStatus.SelectedItem.Text;
            SaveLocalSettingEnableDesable();
        }

        protected void ExecuteSearch_Click(object sender, ImageClickEventArgs e)
        {
            Thread.Sleep(500);
            // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", "<script>$('#spnlErrorMessage').text('');</script>", false);
            txtInputPaperModulesJob.Text = string.Empty;
            txtTonerTypeJob.Text = string.Empty;
            txtPrintEngineNgOrSheetCodeJob.Text = string.Empty;

            labInputPaperModulesJob.Text = string.Empty;
            labTonerTypeJob.Text = string.Empty;
            labPrintEngineNgOrSheetCodeJob.Text = string.Empty;

            var searchData = LoadSearchCriteria();
            DpSearchByDataSession = searchData;

            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            var jobList = jobProcessingDataAdapter.GetJobProcessing(searchData, siteID);
            DpJobsSession = jobList;

            txtTotalSheets.Text = string.Format("{0:n0}", jobList.Sum(x => x.TotalSheetCount));
            txtTotalSplitQty.Text = string.Format("{0:n0}", jobList.Sum(x => x.Quantity));
            txtNumberOfSplits.Text = string.Format("{0:n0}", jobList.Count);

            // gvDpJobs.Columns[13].Visible = false;
            var configurationDetails = GetDPSysConfigBySiteId("EnableOneClickNext", siteID);
            if (Convert.ToInt32(configurationDetails.DPVariableValue) == 1)
                gvDpJobs.Columns[13].Visible = true;
            else
                gvDpJobs.Columns[13].Visible = false;

            //check dpjobssession length
            if (jobList.Count == 0)
            {
                Type cstype = this.GetType();

                // Get a ClientScriptManager reference from the Page class.
                ClientScriptManager cs = Page.ClientScript;

                // Check to see if the startup script is already registered.
                if (!cs.IsStartupScriptRegistered(cstype, "PopupScript"))
                {
                    String cstext = "alert('No Record Found');";
                    cs.RegisterStartupScript(cstype, "PopupScript", cstext, true);
                }
            }

            List<JobInstructions> specificInstuctionsList = new List<JobInstructions>();
            gvJobInstructions.DataSource = specificInstuctionsList;
            gvJobInstructions.DataBind();

            gvDpJobs.DataSource = DpJobsSession;
            gvDpJobs.DataBind();
            //if(rowColorSession != null)
            //{
            //    if(rowColorSession.rowPosition > -1) // && gvDpJobs.Rows[rowColorSession.rowPosition] != null)
            //        gvDpJobs.Rows[rowColorSession.rowPosition].Cells[11].BackColor = rowColorSession.colourType;
            //}    
            SetSessionForHeaderControls();
        }

        public void BtnProcessJobSplitOnebyOne()
        {
            Dictionary<string, string> fileLocations = GetActiveSites();
            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);
            btnUpdateToNext.Text = "Update Job Status to [ " + dprintJob.NextTaskInProcess + " ]";

            if (dprintJob.StatusId == 2)
                btnReleaseHold.Enabled = true;
            else
                btnReleaseHold.Enabled = false;

            LodEquipmentId();
            ddEquipmentIdProcessJob.ClearSelection();
            try
            {
                ListItem item = ddEquipmentIdProcessJob.Items.FindByText(ddEquipmentId.SelectedItem.Text.ToString().Trim());
                if (item != null)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
                jd.Notifications('E', "#BtnProcessJobSplitOnebyOne- Equipment not found" + ex.Message);
            }
            int jobId = dprintJob.Job;
            CustomerNameText.Text = dprintJob.ClientName;
            //foreach (var jobProcessingT in DpJobsSession)
            //{
            //    if (jobProcessing.Job == jobId && jobProcessing.Status.Equals("Waiting to Print") &&
            //        Convert.ToInt32(ddStatus.SelectedValue) == 15 && fileLocations[jobProcessing.FromLocation].Equals(ddProcessingLocation.SelectedValue))
            //    {
            //        jobProcessing.EquipmentId = ddEquipmentId.SelectedValue;
            //    }
            //}
            if (Convert.ToInt32(ddStatus.SelectedValue) == 3)
            {
                litCancelled.Text = " You are Viewing a Cancelled Split ";
                LoadCancellePopStatus();
            }
            else
            {
                litCancelled.Text = string.Empty;
                LoadPopStatus();
            }

            gvDpJobs.DataSource = DpJobsSession;
            gvDpJobs.DataBind();


            int id = dprintJob.Id;


            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID;
            //if (Request.QueryString["fromReprocessing"] != null)
            //{
            //    siteID = Convert.ToInt32(Request.QueryString["siteId"]);

            //}
            //else
            //{
            siteID = Convert.ToInt32(ddLocation.SelectedValue);
            //}
            var details = jobProcessingDataAdapter.getAllSplitsJob(dprintJob.Job, siteID, dprintJob.PrintSiteId, dprintJob.StatusId, dprintJob.IsComplete).ToList();

            int i = 0;
            int selectedIndexInPopUp = i;
            foreach (var jobProcessing in details)
            {
                if (jobProcessing.Id == id)
                {
                    selectedIndexInPopUp = i;
                    break;
                }
                i++;
            }
            gvPopUpWindows.DataSource = details;
            DpSelectedJobSession = details;
            gvPopUpWindows.DataBind();
            gvPopUpWindows.Rows[selectedIndexInPopUp].ForeColor = Color.Green;
            //  gvPopUpWindows.Rows[selectedIndexInPopUp].BackColor= Color.Yellow;
            ((CheckBox)gvPopUpWindows.Rows[selectedIndexInPopUp].FindControl("cbDataSelected")).Checked = true;
            // ddPopStatus.SelectedValue = DpSelectedJobSession[i].Status.ToString();
            ddPopStatus.SelectedIndex = ddPopStatus.Items.IndexOf(ddPopStatus.Items.FindByText(DpSelectedJobSession[i].Status.ToString()));
            if (!string.IsNullOrEmpty(sellectedStatusSession))
                ddPopStatus.SelectedValue = sellectedStatusSession;
            else
                btnUpdateToNext.Text = "Update Job Status to [ " + DpSelectedJobSession[selectedIndexInPopUp].NextTaskInProcess + " ]";

            //Check status first
            if (DpSelectedJobSession[i].Status.ToString().ToLower().Trim() == "inserting")
            {
                btnUpdateToNext.Enabled = false;
                //Then check roles to enable update button
                if (WebSession.User.HasPermissionExplicit("JobProcessing.Manager")) btnUpdateToNext.Enabled = true;
            }
            else
            {
                btnUpdateToNext.Enabled = true;
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal2();", true);
            ModalJobStatusSplit.Show();
        }

        protected void BtnProcessJobSplit_Clicked(object sender, EventArgs e)
        {
            if (selectedDprintJobsId != 0)
            { BtnProcessJobSplitOnebyOne(); }
            else
            {

                string str = null;
                List<int> lst = new List<int>();
                List<JobProcessingModel> details = new List<JobProcessingModel>();
                Dictionary<int, int> jobCount = new Dictionary<int, int>();
                var jobProcessingDataAdapter = new JobProcessingDataAdapter();
                foreach (GridViewRow gvrow in gvDpJobs.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("chkbox");

                    if (chk != null & chk.Checked)
                    {
                        if (jobCount.ContainsKey(Convert.ToInt32(gvrow.Cells[4].Text)))
                        {
                            int value = jobCount[Convert.ToInt32(gvrow.Cells[4].Text)];
                            jobCount[Convert.ToInt32(gvrow.Cells[4].Text)] = value + 1;

                            selectedDprintJobsId = Convert.ToInt32(gvDpJobs.DataKeys[gvrow.RowIndex].Values[0]);
                            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);
                            int id = dprintJob.Id;
                            lst.Add(id);
                        }
                        else
                        {
                            jobCount.Add(Convert.ToInt32(gvrow.Cells[4].Text), 1);
                        }
                        if (jobCount[Convert.ToInt32(gvrow.Cells[4].Text)] <= 1)
                        {

                            str = gvrow.Cells[4].Text;
                            Dictionary<string, string> fileLocations = GetActiveSites();

                            selectedDprintJobsId = Convert.ToInt32(gvDpJobs.DataKeys[gvrow.RowIndex].Values[0]);
                            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);

                            if (dprintJob.StatusId == 2)
                                btnReleaseHold.Enabled = true;
                            else
                                btnReleaseHold.Enabled = false;

                            LodEquipmentId();
                            ddEquipmentIdProcessJob.ClearSelection();
                            ListItem item = ddEquipmentIdProcessJob.Items.FindByText(ddEquipmentId.SelectedItem.Text.ToString().Trim());
                            if (item != null)
                            {
                                item.Selected = true;
                            }

                            int jobId = dprintJob.Job;
                            CustomerNameText.Text = dprintJob.ClientName;

                            if (Convert.ToInt32(ddStatus.SelectedValue) == 3)
                            {
                                litCancelled.Text = " You are Viewing a Cancelled Split ";
                                LoadCancellePopStatus();
                            }
                            else
                            {
                                litCancelled.Text = string.Empty;
                                LoadPopStatus();
                            }

                            gvDpJobs.DataSource = DpJobsSession;
                            gvDpJobs.DataBind();


                            int id = dprintJob.Id;
                            lst.Add(id);
                            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
                            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

                            var processingModel = jobProcessingDataAdapter.getAllSplitsJob(dprintJob.Job, siteID, dprintJob.PrintSiteId, dprintJob.StatusId, dprintJob.IsComplete).ToList();

                            details.AddRange(processingModel);

                        }
                    }
                }


                gvPopUpWindows.DataSource = details;
                DpSelectedJobSession = details;
                gvPopUpWindows.DataBind();

                int i = 0;
                foreach (var jobProcessing in details)
                {
                    if (lst.Contains(jobProcessing.Id))
                    {
                        // lst1.Add(i);
                        gvPopUpWindows.Rows[i].ForeColor = Color.Green;
                        ((CheckBox)gvPopUpWindows.Rows[i].FindControl("cbDataSelected")).Checked = true;

                    }
                    i++;
                }

                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal2();", true);
                ModalJobStatusSplit.Show();

            }

        }


        protected void BtnProcessJobSplit2_Clicked(object sender, EventArgs e)
        {
            var index = 0;
            string addIndex = "";
            List<int> selectedIndex = new List<int>();
            Dictionary<string, string> fileLocations = GetActiveSites();
            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            foreach (GridViewRow row in gvDpJobs.Rows)
            {
                if (((CheckBox)row.FindControl("chkbox")).Checked)
                {
                    selectedIndex.Add(Convert.ToInt32(gvDpJobs.DataKeys[index].Values[0]));
                }
                index++;
            }
            if (selectedIndex.Count == 0) { selectedIndex.Add(selectedDprintJobsId); }
            string joined = string.Join(",", selectedIndex);
            //foreach (var value in selectedIndex)
            //{
            //    addIndex = addIndex + "," + value;
            //}
            List<JobProcessingModel> lstdprintJob = null;// jobProcessingDataAdapter.GetDPrintJobByList(joined);
            //JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);
            foreach (JobProcessingModel dprintJob in lstdprintJob)
            {
                if (dprintJob.StatusId == 2)
                    btnReleaseHold.Enabled = true;
                else
                    btnReleaseHold.Enabled = false;

                LodEquipmentId();
                ddEquipmentIdProcessJob.ClearSelection();
                ListItem item = ddEquipmentIdProcessJob.Items.FindByText(ddEquipmentId.SelectedItem.Text.ToString().Trim());
                if (item != null)
                {
                    item.Selected = true;
                }

                int jobId = dprintJob.Job;
                CustomerNameText.Text = dprintJob.ClientName;
                //foreach (var jobProcessingT in DpJobsSession)
                //{
                //    if (jobProcessing.Job == jobId && jobProcessing.Status.Equals("Waiting to Print") &&
                //        Convert.ToInt32(ddStatus.SelectedValue) == 15 && fileLocations[jobProcessing.FromLocation].Equals(ddProcessingLocation.SelectedValue))
                //    {
                //        jobProcessing.EquipmentId = ddEquipmentId.SelectedValue;
                //    }
                //}
                if (Convert.ToInt32(ddStatus.SelectedValue) == 3)
                {
                    litCancelled.Text = " You are Viewing a Cancelled Split ";
                    LoadCancellePopStatus();
                }
                else
                {
                    litCancelled.Text = string.Empty;
                    LoadPopStatus();
                }

                gvDpJobs.DataSource = DpJobsSession;
                gvDpJobs.DataBind();


                int id = dprintJob.Id;


                var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
                int siteID = Convert.ToInt32(ddLocation.SelectedValue);

                var details = jobProcessingDataAdapter.getAllSplitsJob(dprintJob.Job, siteID, dprintJob.PrintSiteId, dprintJob.StatusId, dprintJob.IsComplete).ToList();

                int i = 0;
                int selectedIndexInPopUp = i;
                foreach (var jobProcessing in details)
                {
                    if (jobProcessing.Id == id)
                    {
                        selectedIndexInPopUp = i;
                        break;
                    }
                    i++;
                }

                gvPopUpWindows.DataSource = details;
                DpSelectedJobSession = details;
                gvPopUpWindows.DataBind();
                gvPopUpWindows.Rows[selectedIndexInPopUp].ForeColor = Color.Green;
                //  gvPopUpWindows.Rows[selectedIndexInPopUp].BackColor= Color.Yellow;
                ((CheckBox)gvPopUpWindows.Rows[selectedIndexInPopUp].FindControl("cbDataSelected")).Checked = true;
                // ddPopStatus.SelectedValue = DpSelectedJobSession[i].Status.ToString();
                ddPopStatus.SelectedIndex = ddPopStatus.Items.IndexOf(ddPopStatus.Items.FindByText(DpSelectedJobSession[i].Status.ToString()));
                if (!string.IsNullOrEmpty(sellectedStatusSession))
                    ddPopStatus.SelectedIndex = Convert.ToInt32(sellectedStatusSession);
                else
                    btnUpdateToNext.Text = "Update Job Status to [ " + DpSelectedJobSession[selectedIndexInPopUp].NextTaskInProcess + " ]";

                //Check status first
                if (DpSelectedJobSession[i].Status.ToString().ToLower().Trim() == "inserting")
                {
                    btnUpdateToNext.Enabled = false;
                    //Then check roles to enable update button
                    if (WebSession.User.HasPermissionExplicit("JobProcessing.Manager")) btnUpdateToNext.Enabled = true;
                }

            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal2();", true);
            ModalJobStatusSplit.Show();
        }

        private void LodEquipmentId()
        {
            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);

            ddEquipmentIdProcessJob.Items.Clear();
            if (dprintJob.StatusId.Equals(15) || dprintJob.StatusId.Equals(16) || dprintJob.StatusId.Equals(17) || dprintJob.StatusId.Equals(18))
            {
                EquipmentIdDataAdapter equipmentIdDataAdapter = new EquipmentIdDataAdapter();
                Collection<EquipmentId> equipmentIdCollection = equipmentIdDataAdapter.GetEquipmentIdByTask(dprintJob.StatusId, Int32.Parse(ddProcessingLocation.SelectedValue));
                if (equipmentIdCollection.Count > 0)
                {
                    foreach (var equipmentId in equipmentIdCollection)
                    {

                        ddEquipmentIdProcessJob.Items.Add(new ListItem(equipmentId.EquipmentName, equipmentId.Id));
                    }
                    ddEquipmentIdProcessJob.Items.Add(new ListItem("N/A", "N/A"));
                }
                else
                {
                    ddEquipmentIdProcessJob.Items.Add(new ListItem("N/A", "N/A"));
                }
            }
            else
            {
                ddEquipmentIdProcessJob.Items.Add(new ListItem("N/A", "N/A"));
            }
            ddEquipmentIdProcessJob.DataTextField = "Text";
            ddEquipmentIdProcessJob.DataValueField = "Value";
            ddEquipmentIdProcessJob.DataBind();

            if (!string.IsNullOrEmpty(ddEquipmentSelected))
            {
                ddEquipmentIdProcessJob.SelectedItem.Value = ddEquipmentSelected;
            }
        }


        //protected void BtnUpdateJobStatus_Clicked(Object sender, EventArgs e)
        //{

        //}

        protected void BtnViewJobHistory_Clicked(Object sender, EventArgs e)
        {
            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);

            var dpPrintDataAdapter = new DpPrintStatusDataAdapter();
            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

            var details = dpPrintDataAdapter.GetDpJobHistories(dprintJob.Id, siteID).ToList();

            gvDetails.DataSource = details;
            gvDetails.DataBind();
            //for (int i = 0; i <= gvDetails.Rows.Count - 1; i++)
            //{
            //    if(gvDetails.Rows[i].Cells[7].Text.ToString() == "True")
            //        gvDetails.Rows[i].Cells[10].BackColor = Color.Green;
            //    else
            //        gvDetails.Rows[i].Cells[10].BackColor = Color.Green;
            //}          
            for (int i = 0; i <= gvDetails.Rows.Count - 1; i++)
            {
                if (gvDetails.Rows[i].Cells[11].Text.Trim().ToString() == "Green")
                {
                    gvDetails.Rows[i].Cells[11].BackColor = Color.Green;
                    gvDetails.Rows[i].Cells[11].ForeColor = Color.Green;
                }
                else if (gvDetails.Rows[i].Cells[11].Text.Trim().ToString() == "Red")
                {
                    gvDetails.Rows[i].Cells[11].BackColor = Color.Red;
                    gvDetails.Rows[i].Cells[11].ForeColor = Color.Red;
                }
                else if (gvDetails.Rows[i].Cells[11].Text.Trim().ToString() == "Yellow")
                {
                    gvDetails.Rows[i].Cells[11].BackColor = Color.Yellow;
                    gvDetails.Rows[i].Cells[11].ForeColor = Color.Yellow;
                }
                else if (gvDetails.Rows[i].Cells[11].Text.Trim().ToString() == "Orange")
                {
                    gvDetails.Rows[i].Cells[11].BackColor = Color.Orange;
                    gvDetails.Rows[i].Cells[11].ForeColor = Color.Orange;
                }
            }
            gvDetails.Columns[4].Visible = false;
            gvDetails.Columns[10].Visible = false;
            labPrintSite.Text = siteName(dprintJob.PrintSiteId);
            if (dprintJob.ClientName.ToString().Length >= 19)
                labCustomer.Text = dprintJob.ClientName.Split(' ')[0].ToString();
            else
                labCustomer.Text = dprintJob.ClientName.ToString();
            labJobComposerId.Text = dprintJob.ComposerId.ToString();
            labSplitQty.Text = dprintJob.Quantity.ToString();
            labJob.Text = dprintJob.Job.ToString();
            labProduct.Text = dprintJob.Product.ToString();
            labSequenceRange.Text = dprintJob.Sequences.ToString();
            labSplit.Text = dprintJob.Split.ToString();
            LinkReprintsSpolige.Text = Convert.ToString(dprintJob.QtyOfReprints);
            labSheetCount.Text = Convert.ToString(dprintJob.TotalSheetCount);
            labImageCount.Text = Convert.ToString(dprintJob.TotalLogicalPageCount);
            labException.Text = Convert.ToString(dprintJob.ExceptionStatus);
            if (dprintJob.IsComplete == 1)
                labIsComplete.Text = "Yes";
            else
                labIsComplete.Text = "No";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            modalDetails.Show();
        }

        private string siteName(int siteId)
        {
            SiteDataAdapter sd = new SiteDataAdapter();
            var siteInfo = sd.Get(siteId);
            return siteInfo.Name;
        }
        protected void gvDpJobs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDpJobs.PageIndex = e.NewPageIndex;
            JobProcessingSearchBy searchData;

            if (DpSearchByDataSession != null)
                searchData = DpSearchByDataSession;
            else
                searchData = LoadSearchCriteria();

            var jobList = bindGridView(SortExp, sortOrder);
            gvDpJobs.DataSource = jobList;
            gvDpJobs.DataBind();
            //upDpJobStatus.Update();
        }
        protected void BtnUpdateToNext_Clicked(Object sender, EventArgs e)
        {
            laJobprocessing.Text = string.Empty;
            var index = 0;
            var nextStatus = "";
            List<int> selectedIndex = new List<int>();
            bool duplicateFound = false;
            bool normalProcess = true;

            foreach (GridViewRow row in gvPopUpWindows.Rows)
            {
                if (((CheckBox)row.FindControl("cbDataSelected")).Checked)
                {
                    nextStatus = ((Label)row.FindControl("Label12")).Text;
                    selectedIndex.Add(index);
                }
                index++;
            }
            int k = selectedIndex.First();
            if (nextStatus.ToLower() == "inserting" || nextStatus.ToLower() == "printing")
            {
                if ((ddPopStatus.SelectedValue == "17" || ddPopStatus.SelectedValue == "15") && ddEquipmentIdProcessJob.SelectedValue == "N/A")
                {

                    ModalpopupexPanelEQMessage.Show();
                    return;
                }
            }
            WriteTraceLog("BtnUpdateToNext_Clicked");
            sellectedIndedSession = selectedIndex;

            List<DpJobHistory> duplicateTaskList = new List<DpJobHistory>();
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();

            string equipmentName = ddEquipmentIdProcessJob.SelectedValue.ToString();
            var inserterInfo = GetInserter(equipmentName, Int32.Parse(ddProcessingLocation.SelectedValue));
            bool IsvirtualInserter = jd.CheckForVertualInserterCategoryByInserterID(Convert.ToString(inserterInfo.InserterMasterID));

            foreach (int j in selectedIndex)
            {
                if (DpSelectedJobSession[j].StatusId == 17 && IsvirtualInserter == true)
                {
                    if (checkAccountPulls(DpSelectedJobSession[j].Id, DpSelectedJobSession[j].Job, DpSelectedJobSession[j].Product, DpSelectedJobSession[j].Split, DpSelectedJobSession[j].FromSiteID, DpSelectedJobSession[j].ClientNumber) == true)
                        normalProcess = true;
                    else
                        normalProcess = false;
                }
                else
                    normalProcess = true;
            }

            if (normalProcess)
            {
                foreach (int i in selectedIndex)
                {
                    int nextStatusId = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                    duplicateTaskList = jd.GetDuplicateTaskCheck(DpSelectedJobSession[i].Id, getCurrentSiteId(), nextStatusId);

                    if (duplicateTaskList.Count > 0)
                    {
                        duplicateFound = true;

                        labEnvironment.Text = WebApplication.EnvironmentText;
                        labStatusInMessage.Text = duplicateTaskList.FirstOrDefault().DPrintTask;
                        labDateInMessage.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().StartedOn);
                        hidStartedOnUtcTime.Value = duplicateTaskList.FirstOrDefault().StartedOnUtcTime;
                        labCompletedOn.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().CompletedOn);
                        labOperatroName.Text = duplicateTaskList.FirstOrDefault().UpdatedBy;
                        labSiteNameInMessage.Text = DpSelectedJobSession[i].PrintLocation;
                        labEquipmentInMessage.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().EquipmentUsed);
                        hidDupClientName.Value = DpSelectedJobSession[i].ClientName;
                        hidDupProduct.Value = DpSelectedJobSession[i].Product;
                        hidDupJob.Value = Convert.ToString(DpSelectedJobSession[i].Job);
                        hidDupSplit.Value = Convert.ToString(DpSelectedJobSession[i].Split);
                        //ctlTimer.Interval = 1000 * 60 * 5;
                        duplicateFoundFromSubMenu = "Y";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                        modalDuplicateWarning.Show();
                    }
                }
                if (duplicateFound == false)
                {
                    duplicateFoundFromSubMenu = "N";
                    duplicateCheckWarningFromSubMenu(null, null);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                ModalAccountPullWarning.Show();
            }
        }
        protected void duplicateCheckWarningFromSubMenu(Object sender, EventArgs e)
        {

            var index = 0;
            List<DpJobHistory> duplicateTaskList = new List<DpJobHistory>();
            List<int> selectedIndex = new List<int>();
            StringBuilder msg = new StringBuilder();
            StringBuilder log = new StringBuilder();

            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            string equipmentName = ddEquipmentIdProcessJob.SelectedItem.Text.Trim();
            var inserterInfo = GetInserter(equipmentName, Int32.Parse(ddProcessingLocation.SelectedValue));
            bool IsvirtualInserter = jd.CheckForVertualInserterCategoryByInserterID(Convert.ToString(inserterInfo.InserterMasterID));

            equipmentNameSession = inserterInfo.InserterName;

            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

            var printerStagingFolder = GetDPSysConfigBySiteId("SpoolFileStaging", siteID);
            if (printerStagingFolder.SiteID != siteID)
            {
                printerStagingFolder = GetDPSysConfig("SpoolFileStaging");
            }
            var PrintingInProcessFolder = GetDPSysConfigBySiteId("PrintingInProcess", siteID);

            if (PrintingInProcessFolder.SiteID != siteID)
            {
                PrintingInProcessFolder = GetDPSysConfig("PrintingInProcess");
            }
            var PrintingCompleteFolder = GetDPSysConfigBySiteId("PrintingComplete", siteID);

            if (PrintingCompleteFolder.SiteID != siteID)
            {
                PrintingCompleteFolder = GetDPSysConfig("PrintingComplete");
            }

            //printerStagingFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrinterStaging\";
            //PrintingInProcessFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrintingInProcess\";
            //PrintingCompleteFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrintingComplete\";

            var mrdfStagingFolder = GetDPSysConfigBySiteId("MRDFStaging", siteID);

            if (mrdfStagingFolder.SiteID != siteID)
            {
                mrdfStagingFolder = GetDPSysConfig("MRDFStaging");
            }
            var mrdfInProcessFolder = GetDPSysConfigBySiteId("MRDFInProcess", siteID);

            if (mrdfInProcessFolder.SiteID != siteID)
            {
                mrdfInProcessFolder = GetDPSysConfig("MRDFInProcess");
            }
            var mrdfCompleteFolder = GetDPSysConfigBySiteId("MRDFComplete", siteID);

            if (mrdfCompleteFolder.SiteID != siteID)
            {
                mrdfCompleteFolder = GetDPSysConfig("MRDFComplete");
            }
            ////mrdfStagingFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFStaging\";
            ////mrdfInProcessFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFInProcess\";
            ////mrdfInProcessFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFComplete\";

            jd.Notifications('I', "#1 gui - eq " + inserterInfo.InserterName + "statging path-" + mrdfStagingFolder.DPVariableValue.ToString() + "InProcess path - " + mrdfInProcessFolder.DPVariableValue.ToString());
            jd.Notifications('I', "#2 gui - eq - " + equipmentName + " - " + printerStagingFolder.DPVariableValue + "-" + PrintingInProcessFolder.DPVariableValue + "-" + PrintingCompleteFolder.DPVariableValue);



            foreach (GridViewRow row in gvPopUpWindows.Rows)
            {
                if (((CheckBox)row.FindControl("cbDataSelected")).Checked)
                {
                    selectedIndex.Add(index);
                }
                index++;
            }
            sellectedIndedSession = selectedIndex;

            foreach (int i in selectedIndex)
            {
                int nextStatusIdCheck = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                duplicateTaskList = jd.GetDuplicateTaskCheck(DpSelectedJobSession[i].Id, getCurrentSiteId(), nextStatusIdCheck);
                if (duplicateTaskList.Count > 0)
                {
                    sendEmailForDuplicateSplit(DpSelectedJobSession[i]);
                    jd.Notifications('D', "Duplicate Job Message presented to Operator - Operator DID Continue. Print Site " + labSiteNameInMessage.Text + "," + DpSelectedJobSession[i].ClientName + "," + Convert.ToString(DpSelectedJobSession[i].Job) + "," + DpSelectedJobSession[i].Product + " and Split " + Convert.ToString(DpSelectedJobSession[i].Split) + ",Operator " + WebSession.User.DisplayName + ", Step " + labStatusInMessage.Text);
                }

                if (DpSelectedJobSession[i].IsComplete == 0)
                {
                    if ((int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")) == 17)
                    {
                        if (IsvirtualInserter == true)
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(Session["VirtualInserterFromSubMenu"]))) { Session["VirtualInserterFromSubMenu"] = equipmentName; }
                            jd.Notifications('I', "# gui - Virtual Inserter - " + equipmentName + " found for " + DpSelectedJobSession[i].ComposerId.ToString() + " and status is " + DpSelectedJobSession[i].Status);
                            checkForQtyPullsAfterPrint(Convert.ToString(inserterInfo.InserterMasterID), Convert.ToString(DpSelectedJobSession[i].Id));
                            return;
                        }
                        jd.Notifications('I', "# gui - moving MRDF file from direct next image button for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + " and Status is " + DpSelectedJobSession[i].Status);
                        string sourceFileName = MrdfFileName(DpSelectedJobSession[i], mrdfStagingFolder.DPVariableValue);//string.Empty;

                        if (sourceFileName.Length == 0)
                        {
                            sourceFileName = MrdfFileName(DpSelectedJobSession[i], mrdfInProcessFolder.DPVariableValue);
                        }
                        if (sourceFileName.Length == 0)
                        {
                            sourceFileName = MrdfFileName(DpSelectedJobSession[i], mrdfCompleteFolder.DPVariableValue);
                        }
                        jd.Notifications('I', "#3 gui - get source file -" + sourceFileName + " for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString());

                        if (equipmentName == "N/A" || string.IsNullOrEmpty(equipmentName))
                            msg.Append("Inserter is not found for selected equipment " + equipmentName + ". User defaults for this computer have been lost. Please reset the  [Job Processing Dashboard] defaults for this step as defined in the User's Guide. ");
                        else if (string.IsNullOrEmpty(sourceFileName))
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(Session["MRDFfileNotFoundInFolderPath"])))
                            {
                                msg.AppendLine("Inserter Control File (MRDF) " + Convert.ToString(Session["MRDFfileNotFoundInFolderPath"]) + " could not be located in the " + mrdfStagingFolder.DPVariableValue + " folder or the " + mrdfInProcessFolder.DPVariableValue + " or the " + mrdfCompleteFolder.DPVariableValue + ". Job will need to be restarted.");

                                jd.Notifications('I', "# gui - MRDF file " + Convert.ToString(Session["MRDFfileNotFoundInFolderPath"]) + " could not be located in the " + mrdfStagingFolder.DPVariableValue + " folder or the " + mrdfInProcessFolder.DPVariableValue + " or the " + mrdfCompleteFolder.DPVariableValue + ".");

                                sendEmail("Inserter Control File (MRDF) " + Convert.ToString(Session["MRDFfileNotFoundInFolderPath"]) + " for this Split could not be found", "The file could not be located in the " + mrdfStagingFolder.DPVariableValue + " folder or the " + mrdfInProcessFolder.DPVariableValue + " or the " + mrdfCompleteFolder.DPVariableValue + ". Job will need to be restarted.Operator " + WebSession.User.DisplayName + " received this error for: Job=" + Convert.ToString(DpSelectedJobSession[i].Job) + " , Product = " + DpSelectedJobSession[i].Product + " , Split = " + Convert.ToString(DpSelectedJobSession[i].Split));

                                Session["MRDFfileNotFoundInFolderPath"] = string.Empty;
                            }
                            else if (!string.IsNullOrEmpty(Convert.ToString(Session["MRDFfileNotFoundInContainerContents"])))
                            {
                                jd.Notifications('I', "# gui - " + Convert.ToString(Session["MRDFfileNotFoundInContainerContents"]));
                                msg.AppendLine(Convert.ToString(Session["MRDFfileNotFoundInContainerContents"]));
                                Session["MRDFfileNotFoundInContainerContents"] = string.Empty;
                            }
                            else if (!string.IsNullOrEmpty(Convert.ToString(Session["MRDFfileError"])))
                            {
                                jd.Notifications('I', "# gui - " + Convert.ToString(Session["MRDFfileError"]));
                                msg.AppendLine(Convert.ToString(Session["MRDFfileError"]));
                                Session["MRDFfileError"] = string.Empty;
                            }
                            try
                            {
                                string filesnames = string.Format("{0}_{1}_{2}_{3}_{4}.NCP", DpSelectedJobSession[i].Product.ToString().PadLeft(5, '0'),
                                    DpSelectedJobSession[i].Job.ToString().PadLeft(7, '0'), DpSelectedJobSession[i].Split.ToString().PadLeft(3, '0'),
                                    DpSelectedJobSession[i].Sequences.Split('-')[1].Trim().ToString().PadLeft(7, '0'), DpSelectedJobSession[i].ComposerId.ToString().PadLeft(7, '0'));
                                commandQueue(filesnames, "", "email", "", "", string.Empty, false, DpSelectedJobSession[i], equipmentName);
                            }
                            catch { }
                        }
                        else
                        {
                            try
                            {
                                if (inserterInfo.TransferMethod.ToLower().Contains("ftp"))
                                {
                                    //FTP
                                    if (commandQueue(sourceFileName, inserterInfo.ImportFolder, "COPY", "FTP", inserterInfo.IPAddress, string.Empty, false, DpSelectedJobSession[i], equipmentName))
                                    {
                                        msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - MRDF is send to ftp queue.");
                                        jd.Notifications('I', "# gui - job composer id -" + DpSelectedJobSession[i].ComposerId.ToString() + " - MRDF is send to ftp queue.FTP source - " + sourceFileName + " and destination - " + inserterInfo.ImportFolder);
                                        //Completeing a Task
                                        int tid = jd.GetDprintTrackingTasksID(DpSelectedJobSession[i].Id, (int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")));
                                        jd.Notifications('I', "#gui before updating DPrintTrackingTasks for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + "," + "Equipment name " + equipmentName);

                                        int errorCode = jd.UpdateDprintTrackingTasks(DpSelectedJobSession[i].Id, tid, true, inserterInfo.InserterName, WebSession.User.DisplayName);

                                        jd.Notifications('I', "#gui after updating DPrintTrackingTasks for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + "," + "Equipment name " + equipmentName + " and Task Id" + tid.ToString());

                                        //Adding A Task
                                        int nextTaskId = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                                        jd.Notifications('I', "#gui before adding new task the next task id is" + nextTaskId.ToString());

                                        var tempId = jd.InsertDprintTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, nextTaskId, inserterInfo.InserterName);
                                        jd.Notifications('I', "#gui after Adding new task for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + "and result is" + tempId.ToString());
                                        msg.AppendLine("Task id " + Convert.ToString(tempId.ToString()) + " is added with next - traking task " + Convert.ToString(nextTaskId));


                                        if (commandQueue(sourceFileName, mrdfInProcessFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, DpSelectedJobSession[i], equipmentName, "FTP and "))
                                        {
                                            msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - move command is sent to command queue for moving mrdfInProcessFolder.");
                                            jd.Notifications('I', "#gui -job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - move command is sent for file-" + sourceFileName + " to command queue for moving " + mrdfInProcessFolder.DPVariableValue + " folder.");
                                        }
                                        else
                                        {
                                            msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - Error - move command is not added to queue for mrdfInProcessFolder. Please check the MSMQ configuration");
                                            jd.Notifications('I', "#gui -job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " Error - move command is not added  for file-" + sourceFileName + " to " + mrdfInProcessFolder.DPVariableValue + " folder.Please check the MSMQ configuration");


                                        }
                                        checkForQtyPullsAfterPrint(Convert.ToString(inserterInfo.InserterMasterID), Convert.ToString(DpSelectedJobSession[i].Id));
                                    }
                                    else
                                    {
                                        msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - Error - ftp command is not added to queue. Please check the MSMQ configuration.");
                                        msg.AppendLine("ftp source - " + sourceFileName);
                                        msg.AppendLine("destination - " + inserterInfo.ImportFolder);
                                        SendNotificationReport(inserterInfo.InserterName, msg.ToString());
                                        jd.Notifications('E', "# gui - MRDF file - ftp command is not added to queue for job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + "-" + "ftp source - " + sourceFileName + " - " + "destination - " + inserterInfo.ImportFolder);
                                    }

                                }
                                else if (inserterInfo.TransferMethod.ToLower().Contains("xcopy"))
                                {
                                    //XCOPY
                                    string importFolder = @"\\" + inserterInfo.ServerControllerName + "\\" + inserterInfo.ImportFolder;
                                    jd.Notifications('I', "#3 gui - get Import folder -" + importFolder);

                                    bool xQueue = commandQueue(sourceFileName, importFolder, "COPY", "Xcopy", string.Empty, string.Empty, false, DpSelectedJobSession[i], equipmentName);
                                    if (xQueue)
                                    {
                                        msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - xcopy command sent to queue to copy MRDF to import folder.");
                                        jd.Notifications('I', "# gui - job composer id -" + DpSelectedJobSession[i].ComposerId.ToString() + " - xcopy command sent to queue to copy MRDF file - " + sourceFileName + " to import folder - " + importFolder + ".");
                                    }
                                    else
                                    {
                                        msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - Error - xcopy command is not added to queue. Please check the MSMQ configuration.");
                                        jd.Notifications('I', "# gui - job composer id -" + DpSelectedJobSession[i].ComposerId.ToString() + " - Error - xcopy command is not added to queue to copy MRDF file - " + sourceFileName + " to import folder - " + importFolder + ".Please check the MSMQ configuration.");
                                    }

                                    if (xQueue == false)
                                    {
                                        SendNotificationReport(inserterInfo.InserterName, msg.ToString());
                                        jd.Notifications('E', "# gui - MRDF file is not copied.");
                                    }
                                    else
                                    {
                                        jd.Notifications('I', "#4 gui - xcopy command sent to queue to copy MRDF file - " + sourceFileName + " to import folder - " + importFolder + ".");
                                        //Completeing a Task
                                        int tid = jd.GetDprintTrackingTasksID(DpSelectedJobSession[i].Id, (int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")));
                                        jd.Notifications('I', "#gui before updating DPrintTrackingTasks for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + "," + "Equipment name " + equipmentName);
                                        int errorCode = jd.UpdateDprintTrackingTasks(DpSelectedJobSession[i].Id, tid, true, inserterInfo.InserterName, WebSession.User.DisplayName);
                                        jd.Notifications('I', "#gui after updating DPrintTrackingTasks for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + "," + "Equipment name " + equipmentName + " and Task Id" + tid.ToString());
                                        //Adding A Task
                                        int nextTaskId = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                                        jd.Notifications('I', "#gui before adding new task the next task id is" + nextTaskId.ToString());
                                        var tempId = jd.InsertDprintTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, nextTaskId, inserterInfo.InserterName);
                                        jd.Notifications('I', "#gui after Adding new task for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + "and result is" + tempId.ToString());

                                        msg.AppendLine("Task id " + Convert.ToString(tempId.ToString()) + " is added with next - traking task " + Convert.ToString(nextTaskId));

                                        if (commandQueue(sourceFileName, mrdfInProcessFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, DpSelectedJobSession[i], equipmentName))
                                        {
                                            msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - move command is sent to command queue for moving mrdfInProcessFolder.");

                                            jd.Notifications('I', "#gui -job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - move command is sent for file-" + sourceFileName + " to command queue for moving " + mrdfInProcessFolder.DPVariableValue + " folder.");
                                        }
                                        else
                                        {
                                            msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - Error - move command is not added to queue for mrdfInProcessFolder. Please check the MSMQ configuration");

                                            jd.Notifications('I', "#gui -job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " Error - move command is not added  for file-" + sourceFileName + " to " + mrdfInProcessFolder.DPVariableValue + " folder.Please check the MSMQ configuration");
                                        }


                                        checkForQtyPullsAfterPrint(Convert.ToString(inserterInfo.InserterMasterID), Convert.ToString(DpSelectedJobSession[i].Id));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                msg.AppendLine("FTP exception occurred, contact your administrator.");
                            }
                        }

                    }
                    else if ((int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")) == 21 || (int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")) == 20)
                    {
                        //Completeing a Task
                        int tid = jd.GetDprintTrackingTasksID(DpSelectedJobSession[i].Id, (int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")));
                        int errorCode = jd.UpdateDprintTrackingTasks(DpSelectedJobSession[i].Id, tid, true, equipmentName, WebSession.User.DisplayName);

                        //Adding new task
                        int nextTaskId = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                        var tempId = jd.InsertDprintTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, nextTaskId, equipmentName);

                        int nextTask = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                        jd.setJobProcessingStatus(DpSelectedJobSession[i].Id, nextTask, Convert.ToString(DpSelectedJobSession[i].ScheduledMailDate));

                        //US21435- Move Container file when Job is Completed
                        var jobStatusCode = (int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", ""));
                        ContainerFileMovement(msg, jd, equipmentName, siteID, DpSelectedJobSession[i], jobStatusCode);

                        //Calculate time , Days LAte adn FTP hand off date

                        var dpPrintDataAdapter = new DpPrintStatusDataAdapter();

                        var details = dpPrintDataAdapter.GetDpJobHistories(DpSelectedJobSession[i].Id, siteID).ToList();
                        jd.DaysTimeUpdate(DpSelectedJobSession[i].Id, details, Convert.ToString(DpSelectedJobSession[i].ScheduledMailDate));

                        //Print the Final Summary ticket
                        // msg.AppendLine(jd.PrintFSSTicket(details, DpSelectedJobSession[i].Id, DpSelectedJobSession[i].Job, DpSelectedJobSession[i].Product, DpSelectedJobSession[i].Split, DpSelectedJobSession[i].startSequence, DpSelectedJobSession[i].Sequences, DpSelectedJobSession[i].ComposerId, siteID));
                        if (FsstPrintRequest(DpSelectedJobSession[i].Id, DpSelectedJobSession[i].Job, DpSelectedJobSession[i].Product, DpSelectedJobSession[i].Split))
                            msg.Append("Final summary ticket print request is sent to the printer");
                        jd.Notifications('I', "Final summary ticket print request is sent to the printer.");
                    }
                    else if ((int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")) == 16)
                    {
                        jd.Notifications('I', "# gui - moving spool file from action menu process/split section.");
                        var spoolFile = GetSoolFileInfo(PrintingInProcessFolder.DPVariableValue, DpSelectedJobSession[i]);
                        jd.Notifications('I', "# gui spool file count " + spoolFile.Count() + " in " + PrintingInProcessFolder.DPVariableValue + "for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + " and Status is" + DpSelectedJobSession[i].Status);

                        foreach (FileInfo sf in spoolFile)
                        {
                            if (commandQueue(sf.FullName, PrintingCompleteFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, DpSelectedJobSession[i], equipmentName))
                                msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - move command is set to command queue for moving spool file (" + sf.Name + ") to PrintingCompleteFolder.");
                            else
                                msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - Error - move command is not added to queue for moving spool file (" + sf.Name + ") to PrintingCompleteFolder. Please check the MSMQ configuration.");
                        }
                        //Completeing a Task
                        int tid = jd.GetDprintTrackingTasksID(DpSelectedJobSession[i].Id, (int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")));
                        jd.Notifications('I', "#gui before updating DPrintTrackingTasks for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + "," + "Equipment name " + equipmentName);
                        int errorCode = jd.UpdateDprintTrackingTasks(DpSelectedJobSession[i].Id, tid, true, equipmentName, WebSession.User.DisplayName);
                        jd.Notifications('I', "#gui after updating DPrintTrackingTasks for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + "," + "Equipment name " + equipmentName + " and Task Id" + tid.ToString());
                        //Adding new task
                        int nextTaskId = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                        jd.Notifications('I', "#gui before adding new task the next task id is" + nextTaskId.ToString());
                        var tempId = jd.InsertDprintTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, nextTaskId, equipmentName);
                        jd.Notifications('I', "#gui after Adding new task for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + "and result is" + tempId.ToString());

                        int nextTask = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                        jd.Notifications('I', "#gui before setting  job processing status the next task is " + nextTask.ToString());
                        jd.setJobProcessingStatus(DpSelectedJobSession[i].Id, nextTask);
                        jd.Notifications('I', "#gui Set job processing status for print job Id " + DpSelectedJobSession[i].Id.ToString() + " and Composer Id " + DpSelectedJobSession[i].ComposerId.ToString());

                    }
                    else if ((int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")) == 15)
                    {

                        if (string.IsNullOrEmpty(equipmentName))
                        {
                            msg.Clear();
                            msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - Equipment is not found.");
                            laJobprocessing.ForeColor = Color.Red;
                            break;
                        }
                        jd.Notifications('I', "# 3 gui - moving spool file from action menu process/split section.");
                        int printersNgID = jd.GetPrinterNgIdByEquipment(equipmentName, Int32.Parse(ddProcessingLocation.SelectedValue));
                        var spoolFile = GetSoolFileInfo(printerStagingFolder.DPVariableValue, DpSelectedJobSession[i]);//"JET1D1";
                        var spoolFileForMail = GetSoolFileInfoForMail(DpSelectedJobSession[i]);

                        jd.Notifications('I', "# gui spool file count " + spoolFile.Count() + " in " + printerStagingFolder.DPVariableValue + "for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + " and Status is" + DpSelectedJobSession[i].Status);
                        if (spoolFile.Count() == 0)
                        {
                            spoolFile = GetSoolFileInfo(PrintingInProcessFolder.DPVariableValue, DpSelectedJobSession[i]);
                            jd.Notifications('I', "# gui spool file count " + spoolFile.Count() + " in " + PrintingInProcessFolder.DPVariableValue + "for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + " and Status is" + DpSelectedJobSession[i].Status);
                        }
                        if (spoolFile.Count() == 0)
                        {
                            spoolFile = GetSoolFileInfo(PrintingCompleteFolder.DPVariableValue, DpSelectedJobSession[i]);
                            jd.Notifications('I', "# gui spool file count " + spoolFile.Count() + " in " + PrintingCompleteFolder.DPVariableValue + "for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + " and Status is" + DpSelectedJobSession[i].Status);
                        }

                        jd.Notifications('I', "#4 gui - PrintersNg Id found - " + printersNgID);

                        jd.Notifications('I', "# 5 gui - spoolFilePath count - " + spoolFile.Count.ToString());

                        //US22212-ReloadContainer files
                        if (spoolFile.Count() == 0)
                        {
                            spoolFile = ReprocessContainerContent(jd, equipmentName, siteID, printerStagingFolder, DpSelectedJobSession[i], spoolFile, spoolFileForMail);
                        }


                        if (spoolFile.Count() > 0)
                        {
                            int fileMatchingCount = 0;
                            if (printersNgID != 0)
                            {
                                foreach (FileInfo spoolFileForMatch in spoolFile)
                                {
                                    string spoolFileName = spoolFileForMatch.Name;
                                    var printerTargets = jd.GetPrinterTargetsNg(printersNgID, spoolFileName);
                                    jd.Notifications('I', "# 7 gui - printer target count - " + printerTargets.Count.ToString());

                                    if (printerTargets.Count > 1)
                                    {
                                        string emailTitel = "[W11.3] Printer Master / Printer Targets Configuration Error in " + DpSelectedJobSession[i].PrintLocation;
                                        string emailBody = "Multiple records returned from PrinterTargetsNg table during single record lookup.Check MatchKeyType and MatchKeyValue configurations.";
                                        emailBody += Environment.NewLine + "Please resolve. Job = " + DpSelectedJobSession[i].Job.ToString() + " Product = " + DpSelectedJobSession[i].Product + " Split = " + DpSelectedJobSession[i].Split.ToString() + " PrinterNgID = " + printersNgID.ToString() + " and MatchKeyValue = " + spoolFileName;
                                        sendEmail(emailTitel, emailBody);
                                        jd.Notifications('I', "# 8 gui - Multiple records returned from PrinterTargetsNg table during single record lookup.Check MatchKeyType and MatchKeyValue configurations for spool file " + spoolFileForMatch.Name);
                                        msg.AppendLine("Multiple records returned from PrinterTargetsNg table during single record lookup.Check MatchKeyType and MatchKeyValue configurations for spool file " + spoolFileForMatch.Name);
                                        break;
                                    }
                                    else if (printerTargets.Count == 1)
                                    {
                                        string pattern = @"\[\d+\]";
                                        string MatchKeyType = Regex.IsMatch(printerTargets[0].MatchKeyType.ToLower(), pattern) == true ? printerTargets[0].MatchKeyType.ToLower() : string.Empty;

                                        int NextPrinterTargetsNgID = -1;
                                        string CommandType = "copy";
                                        string AutomationString = string.Empty;
                                        var printerTargetExeCmd = jd.GetPrinterTargetsNgExecuteCommand(printerTargets[0].NextPrinterTargetsNgID);
                                        if (printerTargetExeCmd.Count == 1 && printerTargetExeCmd[0].TargetPurpose.ToLower() == "execute command")
                                        {
                                            CommandType = "execute command";
                                            AutomationString = printerTargetExeCmd[0].AutomationString;
                                            NextPrinterTargetsNgID = printerTargets[0].NextPrinterTargetsNgID;
                                        }
                                        fileMatchingCount++;
                                        // Move spool file
                                        if (printerTargets[0].TransferMethod.Trim().ToLower().Contains("xcopy") || printerTargets[0].TransferMethod.Trim().ToLower().Contains("ftp"))
                                        {

                                            if (commandQueue(spoolFileForMatch.FullName, printerTargets[0].DropOffLocation, CommandType, printerTargets[0].TransferMethod, printerTargets[0].NetworkAddress, printerTargets[0].MatchKeyValue, false, DpSelectedJobSession[i], equipmentName, string.Empty, MatchKeyType, AutomationString, printerTargets[0].PrinterTargetsNgID, NextPrinterTargetsNgID))

                                            {
                                                msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - xcopy command is sent to queue to copy Spool file at Drop of location.");

                                                jd.Notifications('I', "#gui - Spool file is send to xcopy command for to Drop of location queue");
                                                laJobprocessing.ForeColor = Color.Black;

                                                /* xCopyOutPut xPinPOutput = xCopy(spoolFilePath, PrintingInProcessFolder.DPVariableValue); --------- */
                                                bool isMessage = false;

                                                foreach (FileInfo sf in spoolFile)
                                                {

                                                    if (commandQueue(sf.FullName, PrintingInProcessFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, DpSelectedJobSession[i], equipmentName))
                                                    {
                                                        if (isMessage == false)
                                                        {
                                                            msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - move command is sent to command queue to move spool file to ProcessFolder.");
                                                            jd.Notifications('I', "#gui - command is sent to queue to move spool file to" + PrintingInProcessFolder.DPVariableValue + " - folder for job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + ".Equipment Name-" + equipmentName + "spool file -" + sf.FullName);
                                                            isMessage = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (isMessage == false)
                                                        {
                                                            msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - Error - move command is not added to queue for to PrintingInProcessFolder folder. Please check the MSMQ configuration.");
                                                            jd.Notifications('E', "#gui - move command is not added to queue for to PrintingInProcessFolder folder for " + DpSelectedJobSession[i].ComposerId.ToString() + ".Please check the MSMQ configuration");
                                                            isMessage = true;
                                                        }
                                                    }
                                                }

                                                //Completeing a Task
                                                int tid = jd.GetDprintTrackingTasksID(DpSelectedJobSession[i].Id, (int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")));
                                                jd.Notifications('I', "#gui before updating DPrintTrackingTasks for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + "," + "Equipment name " + equipmentName);
                                                int errorCode = jd.UpdateDprintTrackingTasks(DpSelectedJobSession[i].Id, tid, true, equipmentName, WebSession.User.DisplayName);
                                                jd.Notifications('I', "#gui after updating DPrintTrackingTasks for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + "," + "Equipment name " + equipmentName + " and Task Id" + tid.ToString());
                                                //Adding new task
                                                int nextTaskId = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                                                jd.Notifications('I', "#gui before adding new task the next task id is" + nextTaskId.ToString());
                                                var tempId = jd.InsertDprintTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, nextTaskId, equipmentName);
                                                jd.Notifications('I', "#gui after Adding new task for Composer Id " + DpSelectedJobSession[i].ComposerId.ToString() + "and result is" + tempId.ToString());

                                                int nextTask = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                                                jd.Notifications('I', "#gui before setting  job processing status the next task is " + nextTask.ToString());
                                                jd.setJobProcessingStatus(DpSelectedJobSession[i].Id, nextTask);
                                                jd.Notifications('I', "#gui Set job processing status for print job Id " + DpSelectedJobSession[i].Id.ToString() + " and Composer Id " + DpSelectedJobSession[i].ComposerId.ToString());
                                                break;
                                            }
                                            else
                                            {
                                                msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - Error - xcopy command is not added to queue for to Drop of location folder. Please check the MSMQ configuration.");
                                                string emailTitel = "Spool File could not be sent to Printer";
                                                string emailBody = "Operator " + WebSession.User.DisplayName + " attempted to move Spool File <Spool File Name> to Printer<composer.dbo.printer_name> and operation failed. Please resolve issue.";

                                                jd.Notifications('E', "#gui - xcopy command is not added to queue for to Drop of location folder. Please check the MSMQ configuration");
                                                laJobprocessing.ForeColor = Color.Red;
                                                sendEmail(emailTitel, emailBody);
                                            }
                                        }
                                        else if (printerTargets[0].TransferMethod.Trim().ToLower().Contains("lpr"))
                                        {
                                            if (commandQueue(spoolFileForMatch.FullName, printerTargets[0].DropOffLocation, CommandType, printerTargets[0].TransferMethod, printerTargets[0].NetworkAddress, printerTargets[0].MatchKeyValue, printerTargets[0].TransferBinary, DpSelectedJobSession[i], equipmentName, string.Empty, MatchKeyType, AutomationString, printerTargets[0].PrinterTargetsNgID, NextPrinterTargetsNgID))
                                            {
                                                msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - Spool file is sent to LPR queue.");

                                                foreach (FileInfo sf in spoolFile)
                                                {
                                                    if (commandQueue(sf.FullName, PrintingInProcessFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, DpSelectedJobSession[i], equipmentName))
                                                    {
                                                        msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - move command is sent to command queue to move spool file to ProcessFolder.");
                                                        jd.Notifications('I', "#gui - command is sent to queue to move Spool file - ");
                                                    }
                                                    else
                                                    {
                                                        msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - Error - move command is not added to queue for to PrintingInProcessFolder folder. Please check the MSMQ configuration.");
                                                        jd.Notifications('E', "#gui - move command is not added to queue for to PrintingInProcessFolder folder. Please check the MSMQ configuration");
                                                    }
                                                }

                                                //Completeing a Task
                                                int tid = jd.GetDprintTrackingTasksID(DpSelectedJobSession[i].Id, (int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")));
                                                int errorCode = jd.UpdateDprintTrackingTasks(DpSelectedJobSession[i].Id, tid, true, equipmentName, WebSession.User.DisplayName);

                                                //Adding new task
                                                int nextTaskId = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                                                var tempId = jd.InsertDprintTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, nextTaskId, equipmentName);

                                                int nextTask = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                                                jd.setJobProcessingStatus(DpSelectedJobSession[i].Id, nextTask);
                                            }
                                            else
                                                msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - Error - LPR command is not added to MSMQ. Please check the MSMQ configuration");
                                        }

                                    }
                                }
                            }
                            else
                            {
                                msg.Append("printersNgID is not found for selected equipment " + equipmentName + ". User defaults for this computer have been lost. Please reset the  [Job Processing Dashboard] defaults for this step as defined in the User's Guide. ");
                                jd.Notifications('I', "#gui printersNgID is not found for selected equipment " + equipmentName + ". User defaults for this computer have been lost. Please reset the  [Job Processing Dashboard] defaults for this step as defined in the User's Guide.");
                            }


                            if (fileMatchingCount == 0)
                            {
                                msg.AppendLine("A spool file is available for this Split in the Printer Staging folder however it does not match the format needed for the Printer showing in the Equipment list.");
                                jd.Notifications('I', "#gui A spool file is available for this Split in the Printer Staging folder however it does not match the format needed for the Printer showing in the Equipment list.");

                                bool isMailFlag = false;
                                string spoolFileName = string.Empty;
                                if (spoolFile.Count() > 0)
                                {
                                    foreach (FileInfo spoolFileForMatch in spoolFile)
                                    {
                                        spoolFileName = spoolFileForMatch.Name;
                                        var printerTargets = jd.GetPrinterTargetsNg(printersNgID, spoolFileName);
                                        if (printerTargets.Count > 0)
                                        {
                                            isMailFlag = true;
                                            AddCommandQueueForEmail(DpSelectedJobSession[i], equipmentName, "Event= Update Button click Message = email for file not found for Waiting to print ", spoolFileName);
                                            msg.AppendLine("Spool file " + spoolFileName + " could not be located in the " + printerStagingFolder.DPVariableValue + " folder or the "
                                            + PrintingInProcessFolder.DPVariableValue + " or the " + PrintingCompleteFolder.DPVariableValue + ". Job will need to be restarted.");
                                        }
                                    }
                                }

                                if (!isMailFlag)
                                {
                                    AddCommandQueueForEmail(DpSelectedJobSession[i], equipmentName, "Event= Update Button click Message = email for file not found for Waiting to print ", spoolFileName);

                                }

                                break;
                            }
                            //jd.Notifications('I', "# 6 gui - spoolFilePath - " + spoolFilePath);
                        }
                        else
                        {
                            bool isMailFlag = false;
                            string spoolFileName = string.Empty;
                            if (spoolFileForMail.Count() > 0)
                            {
                                foreach (var spoolFileForMatch in spoolFileForMail)
                                {
                                    spoolFileName = spoolFileForMatch;
                                    var printerTargets = jd.GetPrinterTargetsNg(printersNgID, spoolFileName);
                                    if (printerTargets.Count > 0)
                                    {
                                        isMailFlag = true;
                                        // AddCommandQueueForEmail(DpSelectedJobSession[i], equipmentName, "Event= Update Button click Message = email for file not found for Waiting to print ", spoolFileName);
                                        msg.AppendLine("Spool file " + spoolFileName + " could not be located in the " + printerStagingFolder.DPVariableValue + " folder or the "
                                        + PrintingInProcessFolder.DPVariableValue + " or the " + PrintingCompleteFolder.DPVariableValue + ". Job will need to be restarted.");
                                    }
                                }
                            }

                            if (!isMailFlag)
                            {
                                //  AddCommandQueueForEmail(DpSelectedJobSession[i], equipmentName, "Event= Update Button click Message = email for file not found for Waiting to print ", spoolFileName);
                                msg.AppendLine("Spool file " + spoolFileName + " could not be located in the " + printerStagingFolder.DPVariableValue + " folder or the "
                                         + PrintingInProcessFolder.DPVariableValue + " or the " + PrintingCompleteFolder.DPVariableValue + ". Job will need to be restarted.");
                            }


                            // sendEmail("Spool file for this Split could not be found in Printer Staging Folder in " + DpSelectedJobSession[i].PrintLocation, "Operator " + WebSession.User.DisplayName + " received this error for: Job=" + Convert.ToString(DpSelectedJobSession[i].Job) + " , Product = " + DpSelectedJobSession[i].Product + " , Split = " + Convert.ToString(DpSelectedJobSession[i].Split) + ", Equipment Spool file Prefix = " + createEqupmentPrefix());

                        }
                    }
                    else
                    {
                        callFromRightarrow = false;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                        modalWarningMsg1.Show();
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                    modalWarningMsg.Show();
                }

            }

            if (!string.IsNullOrEmpty(Convert.ToString(Session["SpoolingError"])))
            {
                laJobprocessing.Text = laJobprocessing.Text + msg.ToString();  //  + ". " + Convert.ToString(Session["SpoolingError"]);
                Session["SpoolingError"] = string.Empty;
            }
            else
            {
                laJobprocessing.Text = laJobprocessing.Text + msg.ToString();
            }
            msg.Clear();

            DpJobsSession = jd.GetJobProcessing(DpSearchByDataSession, Convert.ToInt32(((DropDownList)this.Master.FindControl("ddlLocation")).SelectedIndex));


            BtnProcessJobSplit_Clicked(null, null);
        }



        private Inserter GetInserter(string inserterName, int siteId)
        {
            EquipmentIdDataAdapter equipmentIdDataAdapter = new EquipmentIdDataAdapter();
            Inserter inserter = equipmentIdDataAdapter.GetInserterByName(inserterName, siteId);
            return inserter;
        }

        public List<FileInfo> GetSoolFileInfo(string printerStagingPath, JobProcessingModel jobInfo)
        {
            List<FileInfo> fileInfo = new List<FileInfo>();
            List<string> spoolFiles = new List<string>();
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            List<ContainerContents> itemsInContainer = jd.GetContainerContentsByDpJobId(jobInfo.Id);

            try
            {
                string continousSpoolFile = Convert.ToString(itemsInContainer.Where(x => x.FileType == "ContinuousSpoolFile").Select(t => t.Filename).FirstOrDefault());
                string cutSheetSpoolFile = Convert.ToString(itemsInContainer.Where(x => x.FileType == "CutsheetSpoolFile").Select(t => t.Filename).FirstOrDefault());

                if (string.IsNullOrEmpty(continousSpoolFile) && string.IsNullOrEmpty(cutSheetSpoolFile))
                    jd.Notifications('E', "#gui - No spool files are found in  DPJobs.ContainerContents table. ");
                else
                {
                    if (!string.IsNullOrEmpty(continousSpoolFile))
                    {
                        jd.Notifications('I', "#gui - continous SpoolFile is found in  DPJobs.ContainerContents table - " + continousSpoolFile);
                        spoolFiles.Add(continousSpoolFile);
                    }

                    if (!string.IsNullOrEmpty(cutSheetSpoolFile))
                    {
                        jd.Notifications('I', "#gui - cutSheet SpoolFile is found in  DPJobs.ContainerContents table - " + cutSheetSpoolFile);
                        spoolFiles.Add(cutSheetSpoolFile);
                    }
                }
            }
            catch (Exception ex)
            {
                jd.Notifications('E', "#gui -Method : GetSoolFileInfo Records not found in DPJobs.ContainerContents table for DPrintJobsID " + jobInfo.Id.ToString() + " - " + ex.Message);
                laJobprocessing.Text = "Error occurred while retrieving Spool File Info. No record found in ContainerContents table.";
                return fileInfo;
            }
            //// string[] spoolFiles = jobInfo.SpoolFile.Trim().Split('|');           
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["SpoolingError"]))) { Session["SpoolingError"] = string.Empty; }

                var dir = new DirectoryInfo(printerStagingPath);

                FileInfo[] files = dir.GetFiles("*.pdf", SearchOption.AllDirectories);
                jd.Notifications('I', "#gui - printer staging directory found - " + printerStagingPath);
                foreach (string spoolFile in spoolFiles)
                {

                    if (files.Count() > 0 && !string.IsNullOrEmpty(spoolFile))
                    {
                        if (files.Where(x => x.Name.Equals(spoolFile)).FirstOrDefault() != null)
                        {
                            fileInfo.Add(files.Where(x => x.Name.Equals(spoolFile)).FirstOrDefault());
                            jd.Notifications('I', "#gui - Spool file is added to the matching list - " + spoolFile);
                        }
                        //fileInfo = files.Where(x => x.Name.Contains(jobInfo.Product.ToString()) && x.Name.Contains(jobInfo.startSequence.ToString()) && x.Name.Contains(jobInfo.ComposerId.ToString()) && x.Name.Contains(jobInfo.Job.ToString())).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                jd.Notifications('E', "#gui - directory not found - " + ex.Message);
                Session["SpoolingError"] = ex.Message;
            }

            return fileInfo;
        }

        public List<string> GetSoolFileInfoForMail(JobProcessingModel jobInfo)
        {

            List<string> spoolFiles = new List<string>();
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            List<ContainerContents> itemsInContainer = jd.GetContainerContentsByDpJobId(jobInfo.Id);

            try
            {
                string continousSpoolFile = Convert.ToString(itemsInContainer.Where(x => x.FileType == "ContinuousSpoolFile").Select(t => t.Filename).FirstOrDefault());
                string cutSheetSpoolFile = Convert.ToString(itemsInContainer.Where(x => x.FileType == "CutsheetSpoolFile").Select(t => t.Filename).FirstOrDefault());

                if (string.IsNullOrEmpty(continousSpoolFile) && string.IsNullOrEmpty(cutSheetSpoolFile))
                    jd.Notifications('E', "#gui - No spool files are found in  DPJobs.ContainerContents table. ");
                else
                {
                    if (!string.IsNullOrEmpty(continousSpoolFile))
                    {
                        jd.Notifications('I', "#gui - continous SpoolFile is found in  DPJobs.ContainerContents table - " + continousSpoolFile);
                        spoolFiles.Add(continousSpoolFile);
                    }

                    if (!string.IsNullOrEmpty(cutSheetSpoolFile))
                    {
                        jd.Notifications('I', "#gui - cutSheet SpoolFile is found in  DPJobs.ContainerContents table - " + cutSheetSpoolFile);
                        spoolFiles.Add(cutSheetSpoolFile);
                    }
                }
            }
            catch (Exception ex)
            {
                jd.Notifications('E', "#gui -Method : GetSoolFileInfo Records not found in DPJobs.ContainerContents table for DPrintJobsID " + jobInfo.Id.ToString() + " - " + ex.Message);
                laJobprocessing.Text = "Error occurred while retrieving Spool File Info. No record found in ContainerContents table.";
                return spoolFiles;
            }

            return spoolFiles;
        }

        private DPSysConfig GetDPSysConfig(string variableName)
        {
            JobProcessingDataAdapter jp = new JobProcessingDataAdapter();
            DPSysConfig dpSysConfig = jp.GetSysConfig(-1, variableName);
            return dpSysConfig;
        }

        private DPSysConfig GetDPSysConfigBySiteId(string variableName, int siteId)
        {
            JobProcessingDataAdapter jp = new JobProcessingDataAdapter();
            DPSysConfig dpSysConfig = jp.GetSysConfig(siteId, variableName);
            return dpSysConfig;
        }

        private string MrdfFileName(JobProcessingModel item, string mrdfFolderPath)
        {
            string fileName = string.Empty;
            string mrdfFile = string.Empty;
            var jd = new JobProcessingDataAdapter();

            try
            {
                //string temp_job = Convert.ToString(item.Job);
                //string job = temp_job.Length > 7 ? temp_job.Substring(temp_job.Length - 7, temp_job.Length) : temp_job.PadLeft(7, '0');
                //string split = item.Split.ToString("D3");
                //string EndingSeq = item.Sequences.Split('-')[1].PadLeft(7, '0');
                //string temp_composerId = Convert.ToString(item.ComposerId);
                //string composerId = temp_composerId.Length > 7 ? temp_composerId.Substring(temp_composerId.Length - 7, temp_composerId.Length) : temp_composerId.PadLeft(7, '0');

                //fileName = string.Concat(mrdfFolderPath, @"\", item.Product, "_", job, "_", split, "_", EndingSeq, "_", composerId, ".NCP");

                List<ContainerContents> itemsInContainer = jd.GetContainerContentsByDpJobId(item.Id);
                //string mrdfFile = Convert.ToString(itemsInContainer.Where(x => x.FileType == "MRDF").FirstOrDefault().Filename);
                string mrdfFileInContainerContents = Convert.ToString(itemsInContainer.Where(x => x.FileType == "MRDF").Select(t => t.Filename).FirstOrDefault());
                if (string.IsNullOrEmpty(mrdfFileInContainerContents))
                {

                    jd.Notifications('E', "#gui - No MRDF file found in  DPJobs.ContainerContents table for Composer Id " + item.ComposerId.ToString());
                    //laJobprocessing.Text = "No MRDF file found in  DPJobs.ContainerContents table for Composer Id " + item.ComposerId.ToString();
                    if (string.IsNullOrEmpty(Convert.ToString(Session["MRDFfileNotFoundInContainerContents"]))) { Session["MRDFfileNotFoundInContainerContents"] = "No MRDF file found in  DPJobs.ContainerContents table for Composer Id " + item.ComposerId.ToString(); }
                    return string.Empty;
                }
                else
                {
                    fileName = mrdfFileInContainerContents;
                    var dir = new DirectoryInfo(mrdfFolderPath);
                    FileInfo[] files = dir.GetFiles("*.NCP", SearchOption.AllDirectories);
                    if (files.Where(x => x.Name.Equals(mrdfFileInContainerContents)).FirstOrDefault() != null && files.Count() > 0)
                    {
                        mrdfFile = Convert.ToString(files.Where(x => x.Name.Equals(mrdfFileInContainerContents)).Select(t => t.Name).FirstOrDefault());
                    }
                    else
                    {
                        mrdfFile = string.Empty;
                    }
                }
                if (!string.IsNullOrEmpty(mrdfFile))
                {
                    jd.Notifications('I', "#gui - MRDF file name " + mrdfFile + " found in " + mrdfFolderPath + " for Job Composer Id " + item.ComposerId.ToString());
                    return string.Concat(mrdfFolderPath, @"\", mrdfFile); ;
                }
                else
                {
                    if (string.IsNullOrEmpty(Convert.ToString(Session["MRDFfileNotFoundInFolderPath"]))) { Session["MRDFfileNotFoundInFolderPath"] = fileName; }
                    jd.Notifications('I', "#gui - MRDF file name " + fileName + " not found in " + mrdfFolderPath + " for Job Composer Id " + item.ComposerId.ToString());
                    return string.Empty;
                }
                //fileName = string.Concat(mrdfFolderPath, @"\", mrdfFile);

                //return fileName;
            }
            catch (Exception ex)
            {
                jd.Notifications('E', "#gui - Error while fetching the MRDF file from DPJobs.ContainerContents table for Composer Id " + item.ComposerId.ToString() + " - " + ex.Message);
                if (string.IsNullOrEmpty(Convert.ToString(Session["MRDFfileError"])))
                {
                    Session["MRDFfileError"] = "Error while fetching the MRDF file for Composer Id " + item.ComposerId.ToString() + ".Error - " + ex.Message;
                }
                return string.Empty;
            }

        }

        protected void BtnUpdateToPrevious_Clicked(Object sender, EventArgs e)
        {
            var index = 0;
            bool isMRDFFileinStaging = false;
            laJobprocessing.Text = string.Empty;

            StringBuilder msg = new StringBuilder();
            List<int> selectedIndex = new List<int>();
            var jd = new JobProcessingDataAdapter();

            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);
            string equipmentName = ddEquipmentIdProcessJob.SelectedValue.ToString();



            var printerStagingFolder = GetDPSysConfigBySiteId("SpoolFileStaging", siteID);
            if (printerStagingFolder.SiteID != siteID)
            {
                printerStagingFolder = GetDPSysConfig("SpoolFileStaging");
            }
            var PrintingInProcessFolder = GetDPSysConfigBySiteId("PrintingInProcess", siteID);

            if (PrintingInProcessFolder.SiteID != siteID)
            {
                PrintingInProcessFolder = GetDPSysConfig("PrintingInProcess");
            }
            var PrintingCompleteFolder = GetDPSysConfigBySiteId("PrintingComplete", siteID);

            if (PrintingCompleteFolder.SiteID != siteID)
            {
                PrintingCompleteFolder = GetDPSysConfig("PrintingComplete");
            }

            ////printerStagingFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrinterStaging";
            ////PrintingInProcessFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrintingInProcess";
            ////PrintingCompleteFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrintingComplete";

            var mrdfStagingFolder = GetDPSysConfigBySiteId("MRDFStaging", siteID);

            if (mrdfStagingFolder.SiteID != siteID)
            {
                mrdfStagingFolder = GetDPSysConfig("MRDFStaging");
            }
            var mrdfInProcessFolder = GetDPSysConfigBySiteId("MRDFInProcess", siteID);

            if (mrdfInProcessFolder.SiteID != siteID)
            {
                mrdfInProcessFolder = GetDPSysConfig("MRDFInProcess");
            }

            var mrdfCompleteFolder = GetDPSysConfigBySiteId("MRDFComplete", siteID);
            if (mrdfCompleteFolder.SiteID != siteID)
            {
                mrdfCompleteFolder = GetDPSysConfig("MRDFComplete");
            }

            ////mrdfStagingFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFStaging";
            ////mrdfInProcessFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFInProcess";
            ////mrdfCompleteFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFComplete";

            foreach (GridViewRow row in gvPopUpWindows.Rows)
            {
                if (((CheckBox)row.FindControl("cbDataSelected")).Checked)
                {
                    selectedIndex.Add(index);
                }
                index++;
            }
            foreach (int i in selectedIndex)
            {

                if (DpSelectedJobSession.All(x => x.StatusId > 12) && DpSelectedJobSession.All(x => x.StatusId < 21))
                {
                    int previousTaskId = jd.GetDPrintTaskIdForHoldJob(DpSelectedJobSession[i].StatusId);

                    if (DpSelectedJobSession[i].StatusId == 16)
                    {

                        List<FileInfo> spoolFile = new List<FileInfo>();

                        spoolFile = GetSoolFileInfo(PrintingInProcessFolder.DPVariableValue, DpSelectedJobSession[i]);

                        if (spoolFile.Count() == 0)
                        {
                            spoolFile = GetSoolFileInfo(PrintingCompleteFolder.DPVariableValue, DpSelectedJobSession[i]);
                        }

                        if (spoolFile.Count() == 0)
                        {
                            spoolFile = GetSoolFileInfo(printerStagingFolder.DPVariableValue, DpSelectedJobSession[i]);
                        }

                        if (spoolFile.Count > 0)
                        {
                            foreach (FileInfo sf in spoolFile)
                            {
                                if (!((sf.DirectoryName?.Equals(printerStagingFolder.DPVariableValue, StringComparison.CurrentCultureIgnoreCase)) == true))
                                {
                                    if (commandQueue(sf.FullName, printerStagingFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, DpSelectedJobSession[i], equipmentName))
                                    {
                                        msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - move command is sent to command queue to move spool file to ProcessFolder.");
                                        jd.Notifications('I', "#gui - command is sent to queue to move Spool file - ");
                                    }
                                    else
                                    {
                                        msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - Error - move command is not added to queue for to PrintingInProcessFolder folder. Please check the MSMQ configuration.");
                                        jd.Notifications('E', "#gui - move command is not added to queue for to PrintingInProcessFolder folder. Please check the MSMQ configuration");
                                    }
                                }
                                else
                                {
                                    jd.Notifications('I', "#gui - Spool file " + sf.FullName + " already exist in the " + printerStagingFolder.DPVariableValue + " folder.");
                                }
                            }
                        }
                        else
                        {
                            /*msg.Append("Spool file could not be located in Staging Folders(Inprocess/Complete).Failing Job");*/
                            /*msg.Append("Spool file could not be located in Inprocess/Complete/Staging Folders.");//Job not Failed.*/
                            /*jd.InsertAttributesChangedTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, 1, string.Empty, "Red");*/
                            /*jd.setJobStatus(DpSelectedJobSession[i].Id, 1, string.Empty, "Spool file cannot be located");*/
                            /*prepareEmail(DpSelectedJobSession[i], "Spool", "printer");*/

                            jd.InsertAttributesChangedTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, previousTaskId, string.Empty, "Orange");
                            DPrintTaskEnum taskEnumPrintToWTP = (DPrintTaskEnum)previousTaskId;
                            jd.setJobStatus(DpSelectedJobSession[i].Id, previousTaskId, string.Empty, "Job moved back to " + Convert.ToString(taskEnumPrintToWTP));
                            msg.Append("Job moved back to " + Convert.ToString(taskEnumPrintToWTP));
                            jd.Notifications('W', Convert.ToString(DpSelectedJobSession[i].Job) + "," + DpSelectedJobSession[i].Product + "," + Convert.ToString(DpSelectedJobSession[i].Split) + " ,Job moved back to " + Convert.ToString(taskEnumPrintToWTP) + " by " + WebSession.User.DisplayName);

                            var details = jd.getAllSplitsJob(DpSelectedJobSession[i].Job, getCurrentLocationId(), DpSelectedJobSession[i].PrintSiteId, 1, DpSelectedJobSession[i].IsComplete).ToList();
                            gvPopUpWindows.DataSource = details;
                            gvPopUpWindows.DataBind();

                            gvPopUpWindows.Rows[i].ForeColor = Color.Green;
                            ((CheckBox)gvPopUpWindows.Rows[i].FindControl("cbDataSelected")).Checked = true;

                            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal2();", true);
                            ModalJobStatusSplit.Show();

                            break;
                        }
                    }
                    else if (DpSelectedJobSession[i].StatusId == 18)
                    {
                        string MRDFfile = MrdfFileName(DpSelectedJobSession[i], mrdfInProcessFolder.DPVariableValue);

                        if (MRDFfile.Length == 0)
                        {
                            MRDFfile = MrdfFileName(DpSelectedJobSession[i], mrdfCompleteFolder.DPVariableValue);
                        }

                        if (MRDFfile.Length == 0)
                        {
                            MRDFfile = MrdfFileName(DpSelectedJobSession[i], mrdfStagingFolder.DPVariableValue);
                            if (MRDFfile.Length > 0)
                                isMRDFFileinStaging = true;
                        }

                        if (File.Exists(MRDFfile))
                        {
                            if (!isMRDFFileinStaging)
                            {
                                if (commandQueue(MRDFfile, mrdfStagingFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, DpSelectedJobSession[i], equipmentName))
                                {
                                    msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - move command is sent to command queue for moving mrdfInProcessFolder.");
                                }
                                else
                                    msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - Error - move command is not added to queue for mrdfInProcessFolder. Please check the MSMQ configuration");
                            }
                            else
                            {
                                jd.Notifications('I', "#gui - MRDF file " + MRDFfile + " already exist in the " + mrdfStagingFolder.DPVariableValue + " folder");
                                isMRDFFileinStaging = false;
                            }
                        }
                        else
                        {
                            /*msg.Append("MRDF file could not be located in Staging folder.Failing Job");*/
                            /*msg.Append("Spool file could not be located in Inprocess/Complete/Staging Folders.");//Job not Failed. */
                            /*jd.InsertAttributesChangedTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, 1, string.Empty, "Red");*/
                            /*jd.setJobStatus(DpSelectedJobSession[i].Id, 1, string.Empty, "MRDF file cannot be located");*/
                            /*prepareEmail(DpSelectedJobSession[i], "MRDF", "Inserter");*/

                            jd.InsertAttributesChangedTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, previousTaskId, string.Empty, "Orange");
                            DPrintTaskEnum taskEnumInsertToWTInsert = (DPrintTaskEnum)previousTaskId;
                            jd.setJobStatus(DpSelectedJobSession[i].Id, previousTaskId, string.Empty, "Job moved back to " + Convert.ToString(taskEnumInsertToWTInsert));
                            msg.Append("Job moved back to " + Convert.ToString(taskEnumInsertToWTInsert));
                            jd.Notifications('W', Convert.ToString(DpSelectedJobSession[i].Job) + "," + DpSelectedJobSession[i].Product + "," + Convert.ToString(DpSelectedJobSession[i].Split) + " ,Job moved back to " + Convert.ToString(taskEnumInsertToWTInsert) + " by " + WebSession.User.DisplayName);

                            var details = jd.getAllSplitsJob(DpSelectedJobSession[i].Job, getCurrentLocationId(), DpSelectedJobSession[i].PrintSiteId, 1, DpSelectedJobSession[i].IsComplete).ToList();
                            gvPopUpWindows.DataSource = details;
                            gvPopUpWindows.DataBind();

                            gvPopUpWindows.Rows[i].ForeColor = Color.Green;
                            ((CheckBox)gvPopUpWindows.Rows[i].FindControl("cbDataSelected")).Checked = true;

                            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal2();", true);
                            ModalJobStatusSplit.Show();

                            break;
                        }
                    }

                    jd.InsertAttributesChangedTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, previousTaskId, string.Empty, "Orange");
                    DPrintTaskEnum taskEnum = (DPrintTaskEnum)previousTaskId;
                    jd.setJobStatus(DpSelectedJobSession[i].Id, previousTaskId, string.Empty, "Job moved back to " + Convert.ToString(taskEnum));
                    msg.Append("Job moved back to " + Convert.ToString(taskEnum));
                    jd.Notifications('W', Convert.ToString(DpSelectedJobSession[i].Job) + "," + DpSelectedJobSession[i].Product + "," + Convert.ToString(DpSelectedJobSession[i].Split) + " ,Job moved back to " + Convert.ToString(taskEnum) + " by " + WebSession.User.DisplayName);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                    ModalPreviousWarning.Show();
                }
            }

            laJobprocessing.Text = msg.ToString();
            msg.Clear();
            ExecuteSearch_Click(null, null);
            //BtnProcessJobSplit_Clicked(null, null);
        }


        protected void BtnApplyStatus_Clicked(Object sender, EventArgs e)
        {

            textComment.Text = "";
            var nextStatus = "";
            var index = 0; 
            int jobComposerId = 0;
            int selectedStatusId = Convert.ToInt32(ddPopStatus.SelectedValue);
            sellectedStatusSession = ddPopStatus.SelectedValue;
            string equipmentName = Convert.ToString(ddEquipmentIdProcessJob.SelectedValue);
            JobProcessingDataAdapter jobProcessingDataAdapter = new JobProcessingDataAdapter();
            List<int> selectedIndex = new List<int>();
            foreach (GridViewRow row in gvPopUpWindows.Rows)
            {
                if (((CheckBox)row.FindControl("cbDataSelected")).Checked)
                {
                    nextStatus = ((Label)row.FindControl("Label12")).Text;
                    jobComposerId = ((Label)row.FindControl("Label1")).Text.ToInt();
                    selectedIndex.Add(index);
                }
                index++;
            }
            if (selectedStatusId == 25)
            {
                ManualjobCompletion(jobComposerId, selectedIndex);
            }
            else
            {
                int j = selectedIndex.First();
                if (nextStatus.ToLower() == "inserting" || nextStatus.ToLower() == "printing")
                {
                    if ((ddPopStatus.SelectedValue == "17" || ddPopStatus.SelectedValue == "15") && ddEquipmentIdProcessJob.SelectedValue == "N/A")
                    {
                        ModalpopupexPanelEQMessage.Show();
                        return;
                    }
                }
                if (ddPopStatus.SelectedValue != "2" && ddPopStatus.SelectedValue != "3") // 2=hold , 3=canceled
                {
                    WriteTraceLog("BtnApplyStatus_Clicked");
                }
                /*******************          Exception code *******************************/
                if (selectedIndex.Count > 0)
                {
                    sellectedIndedSession = selectedIndex;
                    int i = selectedIndex.First();
                    int currentStatusId = jobProcessingDataAdapter.GetTaskIdByName(DpSelectedJobSession[i].Status, 1);
                    int nextStatusId = jobProcessingDataAdapter.GetNextTaskIdByTaskId(currentStatusId, 1);

                    if (DpSelectedJobSession[i].IsComplete == 0)
                    {
                        if (selectedStatusId != 1 && selectedStatusId != 2 && selectedStatusId != 3)
                        {
                            if (selectedStatusId == 5 || selectedStatusId == 12)
                            {
                                checkAndApplyStatus();
                            }
                            else if (DpSelectedJobSession[i].Status == "Failed" && (selectedStatusId == 15 || selectedStatusId == 17 || selectedStatusId == 20 || selectedStatusId == 25 || selectedStatusId == 36 || selectedStatusId == 37))
                            {
                                checkAndApplyStatus();
                            }
                            else if (nextStatusId == selectedStatusId || (selectedStatusId >= currentStatusId && selectedStatusId <= 21)) //(selectedStatusId >= currentStatusId && selectedStatusId <= 21))
                            {
                                //show warning - Cancel
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                                modalWarningApplyStatusCancel.Show();
                            }
                        }
                        else
                        {
                            //Add - update status
                            if (selectedStatusId == 1)
                                checkAndApplyStatus();
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                                modalWarningApplyStatusExp.Show();
                            }
                        }
                    }
                    else if (DpSelectedJobSession[i].IsComplete == 3)
                    {
                        if (selectedStatusId == 5 || selectedStatusId == 12)
                            checkAndApplyStatus();
                    }
                    else
                    {
                        //show warning - Yes or No,Cancel
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                        modalWarningApplyStatusYes.Show();
                    }
                }
                /*******************          Exception code *******************************/



                //foreach (int i in selectedIndex)
                //{
                //    var tempId = jobProcessingDataAdapter.InsertDprintTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, selectedStatusId, equipmentName);
                //    jobProcessingDataAdapter.setJobProcessingStatus(DpSelectedJobSession[i].Id, selectedStatusId, Convert.ToString(DpSelectedJobSession[i].ScheduledMailDate));
                //}

                DpJobsSession = jobProcessingDataAdapter.GetJobProcessing(DpSearchByDataSession, Convert.ToInt32(((DropDownList)this.Master.FindControl("ddlLocation")).SelectedIndex));
                BtnProcessJobSplit_Clicked(null, null);
            }
        }

        private void ManualjobCompletion(int jobComposerId, List<int> selectedIndex)
        {
            if (selectedIndex.Count > 1)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "showDateValidationMessage('Please select one record only');", true);
                ModalJobStatusSplit.Show();
            }
            else
            {

                txtSelectCompletionDate.Text = "";
                btnJobCompletionsApplyStatus.Enabled = false;
                chkCompletedOn.Checked = true;
                lblInsertionCompletedOn.ForeColor = Color.Black;
                txtTime.Text = DateTime.Now.ToString("hh\\:mm");
                JobProcessingDataAdapter jp = new JobProcessingDataAdapter();
                Session["JobComposerId"] = jobComposerId;
                DataSet ds = jp.FetchJobCompletionsApplyStatus(jobComposerId);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    lblJobNumber.Text = ds.Tables[0].Rows[0]["JobNumber"].ToString();
                    lblProductNumber.Text = ds.Tables[0].Rows[0]["ProductNumber"].ToString();
                    lblSplitNumber.Text = ds.Tables[0].Rows[0]["SplitNumber"].ToString();
                    lblSplitQty.Text = ds.Tables[0].Rows[0]["SplitQuantity"].ToString();
                    lblDataEnvironment.Text = ds.Tables[0].Rows[0]["DataEnvironment"].ToString();
                    lblScheduledMailDate.Text = ds.Tables[0].Rows[0]["ScheduledMailDate"].ToString();
                    lblUpdatedOn.Text = ds.Tables[0].Rows[0]["UpdatedOn"].ToString();
                    lblInsertionCompletedOn.Text = ds.Tables[0].Rows[0]["InsertionCompletedOn"].ToString();
                    lblCompletedOn.Text = ds.Tables[0].Rows[0]["CompletedOn"].ToString();
                    lblIsCompleted.Text = ds.Tables[0].Rows[0]["IsComplete"].ToString();
                    lblExceptionStatus.Text = ds.Tables[0].Rows[0]["ExceptionStatus"].ToString();
                }
                ModalPopUpManualJobCompletions.Show();
            }
        }

        protected void btnEquipmentok_Click(Object sender, EventArgs e)
        {
            ModalJobStatusSplit.Show();
            ModalpopupexPanelEQMessage.Hide();

        }

        private void checkAndApplyStatus()
        {
            int selectedStatus = Convert.ToInt32(sellectedStatusSession);
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            foreach (int i in sellectedIndedSession)
            {
                jd.ApplyDprintTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, selectedStatus, string.Empty);
                jd.setJobStatus(DpSelectedJobSession[i].Id, selectedStatus, Convert.ToString(DpSelectedJobSession[i].ScheduledMailDate), textComment.Text);
                jd.Notifications('W', "#gui - Apply Status button used for job Id " + Convert.ToString(DpSelectedJobSession[i].Id) + " Job Number" + Convert.ToString(DpSelectedJobSession[i].Job) + " Product " + Convert.ToString(DpSelectedJobSession[i].Product) + " Split " + Convert.ToString(DpSelectedJobSession[i].Split));
                //btnUpdateToNext.Text = "Update Job Status to [ " + DpSelectedJobSession[selectedIndexInPopUp].NextTaskInProcess + " ]";
            }
            if (selectedStatus == 3)
            {
                laJobprocessing.Text = "Cancel oparation will move spool file and MRDF to Canceled job folder";
                cancelJob();
            }


            sellectedIndedSession.Clear();
            DpJobsSession = jd.GetJobProcessing(DpSearchByDataSession, Convert.ToInt32(((DropDownList)this.Master.FindControl("ddlLocation")).SelectedIndex));

            BtnProcessJobSplit_Clicked(null, null);
        }

        private void cancelJob()
        {
            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);
            var printerStagingFolder = GetDPSysConfigBySiteId("SpoolFileStaging", siteID);
            if (printerStagingFolder.SiteID != siteID)
            {
                printerStagingFolder = GetDPSysConfig("SpoolFileStaging");
            }
            var PrintingInProcessFolder = GetDPSysConfigBySiteId("PrintingInProcess", siteID);

            if (PrintingInProcessFolder.SiteID != siteID)
            {
                PrintingInProcessFolder = GetDPSysConfig("PrintingInProcess");
            }
            var PrintingCompleteFolder = GetDPSysConfigBySiteId("PrintingComplete", siteID);

            if (PrintingCompleteFolder.SiteID != siteID)
            {
                PrintingCompleteFolder = GetDPSysConfig("PrintingComplete");
            }

            ////printerStagingFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrinterStaging\";
            ////PrintingInProcessFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrintingInProcess\";
            ////PrintingCompleteFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\\PrintingComplete\";

            var mrdfStagingFolder = GetDPSysConfigBySiteId("MRDFStaging", siteID);

            if (mrdfStagingFolder.SiteID != siteID)
            {
                mrdfStagingFolder = GetDPSysConfig("MRDFStaging");
            }
            var mrdfInProcessFolder = GetDPSysConfigBySiteId("MRDFInProcess", siteID);

            if (mrdfInProcessFolder.SiteID != siteID)
            {
                mrdfInProcessFolder = GetDPSysConfig("MRDFInProcess");
            }
            var mrdfCompleteFolder = GetDPSysConfigBySiteId("MRDFComplete", siteID);

            if (mrdfCompleteFolder.SiteID != siteID)
            {
                mrdfCompleteFolder = GetDPSysConfig("MRDFComplete");
            }

            ////mrdfStagingFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFStaging\";
            ////mrdfInProcessFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFInProcess\";
            ////mrdfCompleteFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFComplete\";

            string[] spoolFilePath = { printerStagingFolder.DPVariableValue, PrintingInProcessFolder.DPVariableValue, PrintingCompleteFolder.DPVariableValue };
            string[] mrdfFolderPath = { mrdfStagingFolder.DPVariableValue, mrdfInProcessFolder.DPVariableValue, mrdfCompleteFolder.DPVariableValue };

            var canceledFolder = GetDPSysConfigBySiteId("CanceledJobsFolder", siteID);

            if (canceledFolder.SiteID != siteID)
            {
                canceledFolder = GetDPSysConfig("CanceledJobsFolder");
            }
            string sourceFileName = string.Empty;
            string spoolfile = string.Empty;

            foreach (int i in sellectedIndedSession)
            {
                sourceFileName = searchMrdf(DpSelectedJobSession[i], mrdfFolderPath);
                if (sourceFileName != string.Empty)
                    commandQueue(sourceFileName, canceledFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, DpSelectedJobSession[i], string.Empty);

            }

            foreach (int i in sellectedIndedSession)
            {
                var spoolFileInfo = searchSpoolFile(spoolFilePath, DpSelectedJobSession[i]);
                if (spoolFileInfo.Count > 0)
                {
                    spoolfile = spoolFileInfo.OrderBy(x => x.LastWriteTime).Select(x => x.FullName).First().ToString();
                    commandQueue(spoolfile, canceledFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, DpSelectedJobSession[i], string.Empty);
                }
            }
            //Find and get container zip file
            //send request for moving container file to cancel folder

        }

        private List<FileInfo> searchSpoolFile(string[] spoolFilePath, JobProcessingModel jobInfo)
        {
            List<FileInfo> fileInfo = new List<FileInfo>();
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            try
            {
                foreach (string printerStagingPath in spoolFilePath)
                {
                    if (Directory.Exists(printerStagingPath))
                    {
                        var dir = new DirectoryInfo(printerStagingPath);
                        FileInfo[] files = dir.GetFiles("*.pdf", SearchOption.AllDirectories);
                        if (files.Count() > 0)
                        {
                            fileInfo = files.Where(x => x.Name.Contains(jobInfo.Product.ToString()) && x.Name.Contains(jobInfo.startSequence.ToString()) && x.Name.Contains(jobInfo.ComposerId.ToString()) && x.Name.Contains(jobInfo.Job.ToString())).ToList();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                jd.Notifications('E', "#gui - Spoolfile serch error for jobId " + Convert.ToString(jobInfo.Id) + " while cancel operation. Exp - " + ex.Message);
                laJobprocessing.Text = " - " + ex.Message;
            }

            if (fileInfo.Count == 0)
                jd.Notifications('W', "#gui - Spoolfile not found jobId " + Convert.ToString(jobInfo.Id) + " while cancel operation. Exp - ");

            return fileInfo;
        }
        private string searchMrdf(JobProcessingModel item, string[] mrdfFolderPath)
        {
            string fileName = string.Empty;
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();

            try
            {
                string product = item.Product.Substring(item.Product.Length - 5, item.Product.Length);
                string temp_job = Convert.ToString(item.Job);
                string job = temp_job.Length > 7 ? temp_job.Substring(temp_job.Length - 7, temp_job.Length) : temp_job.PadLeft(7, '0');
                string split = item.Split.ToString("D3");
                string EndingSeq = item.Sequences.Split('-')[1].PadLeft(7, '0');
                string temp_composerId = Convert.ToString(item.ComposerId);
                string composerId = temp_composerId.Length > 7 ? temp_composerId.Substring(temp_composerId.Length - 7, temp_composerId.Length) : temp_composerId.PadLeft(7, '0');

                foreach (string filePath in mrdfFolderPath)
                {
                    fileName = string.Concat(filePath, @"\", product, "_", job, "_", split, "_", EndingSeq, "_", composerId, ".NCP");
                    if (File.Exists(fileName))
                        break;
                    else
                    {
                        fileName = string.Empty;
                        jd.Notifications('W', "#gui - MRDF file not found for jobId " + Convert.ToString(item.Id) + " while cancel operation." + "File name " + fileName);
                    }
                }

            }
            catch (Exception ex)
            {
                jd.Notifications('E', "#gui - MRDF serch error for jobId " + Convert.ToString(item.Id) + " while cancel operation. Exp - " + ex.Message);
            }

            return fileName;
        }
        protected void BtnDismiss_Clicked(Object sender, EventArgs e)
        {
            //ctlTimer.Enabled = true;
            ctlTimer.Interval = 30000;
            if (rowColorSession != null)
            {
                if (rowColorSession.rowPosition > -1)
                {
                    // gvDpJobs.Rows[rowColorSession.rowPosition].Cells[11].BackColor = Color.White;
                    // rowColorSession.rowPosition = -1;
                    rowColorSession.colourType = Color.White;
                }
            }
            ExecuteSearch_Click(null, null);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            //ctlTimer.Enabled = true;
            ctlTimer.Interval = 30000;
        }

        protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["oncontextmenu"] = Page.ClientScript.GetPostBackClientHyperlink(gvDpJobs, "Select$" + e.Row.RowIndex);

                e.Row.Attributes["style"] = "cursor:pointer";

                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(gvDpJobs, "Select$" + e.Row.RowIndex);
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell cell = e.Row.Cells[0];
                TableCell cell_rightClick = e.Row.Cells[12];
                int composerID = int.Parse(cell.Text);
                if (rowColorSession != null)
                {
                    if (composerID == rowColorSession.rowPosition)// 4221595)
                    {
                        cell_rightClick.BackColor = rowColorSession.colourType;
                    }
                }
                ImageButton btnImageNext = (ImageButton)e.Row.FindControl("btnNext");
                if (ddProcessingLocation.SelectedItem.Text.Trim() == "All")
                {
                    btnImageNext.Enabled = false;
                    btnImageNext.ToolTip = "Change to next status is disabled";
                    btnImageNext.ImageUrl = "~/Images/NextDisabled.png";
                    actionMenu.Visible = false;
                }
                else
                {
                    btnImageNext.ToolTip = "Change to next status";
                    btnImageNext.ImageUrl = "~/Images/nextStatus.png";
                    btnImageNext.Enabled = true;
                    actionMenu.Visible = true;
                }
            }
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs s)
        {

            if (hfLeftClick.Value == "Y")
            {
                selectedDprintJobsId = Convert.ToInt32(gvDpJobs.DataKeys[gvDpJobs.SelectedRow.RowIndex].Values[0]);
                StatusSession = gvDpJobs.Rows[gvDpJobs.SelectedRow.RowIndex].Cells[12].Text;
                // -- need to delete
                // selectedIndex = gvDpJobs.SelectedRow.RowIndex;
                // --
                LoadJobAttributes();
            }
            else if (hfLeftClick.Value == "N")
            {
                ctlTimer.Interval = 1000 * 60 * 5;
                sellectedStatusSession = string.Empty;
                labMsgJobRouting.Text = string.Empty;
                Session.Remove("sellectedStatusSession");
                // -- need to delete
                //  selectedIndex = gvDpJobs.SelectedRow.RowIndex;
                // --
                selectedDprintJobsId = Convert.ToInt32(gvDpJobs.DataKeys[gvDpJobs.SelectedRow.RowIndex].Values[0]);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal3();", true);
                laJobprocessing.Text = string.Empty;
                actionMenuControl.Show();
            }
            hfLeftClick.Value = string.Empty;
        }

        protected void btnMoveAll_Click(object sender, EventArgs s)
        {

            ctlTimer.Interval = 1000 * 60 * 5;
            sellectedStatusSession = string.Empty;
            labMsgJobRouting.Text = string.Empty;
            Session.Remove("sellectedStatusSession");
            selectedDprintJobsId = 0;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal3();", true);
            laJobprocessing.Text = string.Empty;
            actionMenuControl.Show();

            hfLeftClick.Value = string.Empty;

        }

        private JobProcessingSearchBy LoadSearchCriteria()
        {
            var searchItem = new JobProcessingSearchBy();
            DateTime dt;
            int jn;
            searchItem.ProcessingLoation = Convert.ToInt32(ddProcessingLocation.SelectedItem.Value);
            searchItem.Status = Convert.ToInt32(ddStatus.SelectedValue);
            searchItem.ClientName = ddlClient.SelectedItem.ToString();
            searchItem.ClientNumber = ddlClient.SelectedValue;


            bool res = int.TryParse(txtJobNumber.Text, out jn);
            if (res)
            {
                searchItem.JobNumber = jn;
            }
            else
            {
                searchItem.JobNumber = null;
            }
            searchItem.BeginningDate = DateTime.TryParse(txtBeginningDate.Text, out dt) ? dt : (DateTime?)null;
            searchItem.EndingDate = DateTime.TryParse(txtEndingDate.Text, out dt) ? dt.AddHours(23).AddMinutes(25).AddSeconds(59) : (DateTime?)null;
            if (cbIncludeCompletedJobs.Checked)
                searchItem.IsComplete = 1;
            else
                searchItem.IsComplete = 0;

            if (cbFilterByAttributes.Checked)
            {
                searchItem.AtrFilter = true;
                if (searchItem.Status == 15)
                {
                    searchItem.InputPaperModules = txtEqInputPaperModules.Text;
                    searchItem.TonerType = txtEqTonerTypeOrSheetCode.Text;
                    searchItem.PrintEngineNg = txtEqPrintEngineOrCategory.Text;
                    searchItem.SheetCode = null;
                }
                else
                {
                    searchItem.AtrFilter = false;
                    searchItem.InputPaperModules = null;
                    searchItem.TonerType = null;
                    searchItem.PrintEngineNg = null;
                    searchItem.SheetCode = null;
                }

            }
            else
            {
                searchItem.AtrFilter = false;
                searchItem.InputPaperModules = null;
                searchItem.TonerType = null;
                searchItem.PrintEngineNg = null;
                searchItem.SheetCode = null;
            }

            searchItem.IsProductionJob = cbProductionJobs.Checked;
            searchItem.IsTestJob = cbTestJobs.Checked;
            searchItem.SmallJobs = chkSmalljobs.Checked;
            searchItem.LargeJobs = chkLargeJobs.Checked;
            searchItem.JobSizeThreshold = txtJobSizeThreshold.Text;

            return searchItem;
        }

        private void ExportGridToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            var excel = new GridView();

            excel.DataSource = DpJobsSession;
            excel.DataBind();

            using (var sw = new System.IO.StringWriter())
            using (var htw = new HtmlTextWriter(sw))
            {
                excel.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }

        }

        public string sortOrder
        {
            get
            {
                if (ViewState["sortOrder"].ToString() == "desc")
                {
                    ViewState["sortOrder"] = "asc";
                }
                else
                {
                    ViewState["sortOrder"] = "desc";
                }

                return ViewState["sortOrder"].ToString();
            }
            set
            {
                ViewState["sortOrder"] = value;
            }
        }

        public List<JobProcessingModel> bindGridView(string sortExp, string sortDir)
        {
            var jobList = new List<JobProcessingModel>();
            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

            if (DpSearchByDataSession != null)
            {
                JobProcessingSearchBy searchData = DpSearchByDataSession;

                var jobProcessingDataAdapter = new JobProcessingDataAdapter();
                jobList = jobProcessingDataAdapter.GetJobProcessing(searchData, siteID);//.OrderBy(x => x.JobComposerId).ToList();

                switch (sortExp)
                {
                    case "JobComposerID":
                        if (sortDir == "asc")
                            jobList = jobList.OrderBy(x => x.ComposerId).ToList();
                        else
                            jobList = jobList.OrderByDescending(x => x.ComposerId).ToList();
                        break;
                    case "FromSite":
                        if (sortDir == "asc")
                            jobList = jobList.OrderBy(x => x.FromLocation).ToList();
                        else
                            jobList = jobList.OrderByDescending(x => x.FromLocation).ToList();
                        break;
                    case "PrintSite":
                        if (sortDir == "asc")
                            jobList = jobList.OrderBy(x => x.EquipmentId).ToList();
                        else
                            jobList = jobList.OrderByDescending(x => x.EquipmentId).ToList();
                        break;
                    case "ClientName":
                        if (sortDir == "asc")
                            jobList = jobList.OrderBy(x => x.ClientName).ToList();
                        else
                            jobList = jobList.OrderByDescending(x => x.ClientName).ToList();
                        break;
                    case "JobNumber":
                        if (sortDir == "asc")
                            jobList = jobList.OrderBy(x => x.Product).ToList();
                        else
                            jobList = jobList.OrderByDescending(x => x.Product).ToList();
                        break;
                    case "ProductNumber":
                        if (sortDir == "asc")
                            jobList = jobList.OrderBy(x => x.Quantity).ToList();
                        else
                            jobList = jobList.OrderByDescending(x => x.Quantity).ToList();
                        break;
                    case "ScheduleMailDate":
                        if (sortDir == "asc")
                            jobList = jobList.OrderBy(x => x.ScheduledMailDate).ToList();
                        else
                            jobList = jobList.OrderByDescending(x => x.ScheduledMailDate).ToList();
                        break;
                    case "DpTask":
                        if (sortDir == "asc")
                            jobList = jobList.OrderBy(x => x.Status).ToList();
                        else
                            jobList = jobList.OrderByDescending(x => x.Status).ToList();
                        break;
                    case "CompletedOn":
                        if (sortDir == "asc")
                            jobList = jobList.OrderBy(x => x.NextTaskInProcess).ToList();
                        else
                            jobList = jobList.OrderByDescending(x => x.NextTaskInProcess).ToList();
                        break;
                    case "DPriority":
                        if (sortDir == "asc")
                            jobList = jobList.OrderBy(x => x.Priority).ToList();
                        else
                            jobList = jobList.OrderByDescending(x => x.Priority).ToList();
                        break;

                }
            }
            return jobList;
        }



        protected void btnCancelActionMenu_Click(object sender, EventArgs e)
        {
            //ctlTimer.Enabled = true;
            ctlTimer.Interval = 30000;
        }

        protected void SetddEquipmentProcessJob(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfEquipmentId.Value) && !string.IsNullOrWhiteSpace(hfEquipmentId.Value))
            {
                ddEquipmentSelected = hfEquipmentId.Value;
            }
        }

        private void SendNotificationReport(string inserterName, string mrdfFileName)
        {
            StringBuilder sbToEmail = new StringBuilder();
            var server = ConfigurationManager.AppSettings["mail.server"];
            var targets = GetStringArray("mail.error.targets");
            StringBuilder mailBody = new StringBuilder();

            mailBody.Append(Environment.NewLine);
            mailBody.Append("Operator " + WebSession.User.DisplayName + " attempted to move MRDF " + mrdfFileName + " to Inserter " + inserterName + " and the transfer failed. Please resolve issue. ");
            mailBody.Append(Environment.NewLine);
            mailBody.Append(Environment.NewLine);

            using (var message = new MailMessage())
            {
                message.From = new MailAddress("composer@ncpsolutions.com");
                message.Subject = "MRDF could not be sent to Inserter " + inserterName;

                foreach (string target in targets)
                {
                    sbToEmail.Append(target + ",");
                    message.To.Add(new MailAddress(target));
                }
                message.IsBodyHtml = true;
                message.Body = mailBody.ToString();

                using (var client = new SmtpClient(server))
                {
                    JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
                    jd.Notifications('I', string.Format("#gui-Before sending mail:Subject: {0},ToEmail: {1}", message.Subject, sbToEmail.ToString()));
                    client.Send(message);
                    jd.Notifications('I', ("#gui-Email sent successfully."));
                }
            }

            return;
        }

        public void sendEmail(string subject, string emailBody, bool sendToManagement = false)
        {

            try
            {
                StringBuilder sbToEmail = new StringBuilder();
                var server = ConfigurationManager.AppSettings["mail.server"];
                var targets = GetStringArray("mail.error.targets");

                if (sendToManagement == true)
                    targets = GetStringArray("mail.management.targets");

                StringBuilder mailBody = new StringBuilder();

                mailBody.Append(emailBody);


                using (var message = new MailMessage())
                {
                    message.From = new MailAddress("composer@ncpsolutions.com");
                    message.Subject = subject;

                    foreach (string target in targets)
                    {
                        sbToEmail.Append(target + ",");
                        message.To.Add(new MailAddress(target));
                    }
                    message.IsBodyHtml = true;
                    message.Body = mailBody.ToString();

                    using (var client = new SmtpClient(server))
                    {
                        JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
                        jd.Notifications('I', string.Format("#gui-Before sending mail:Subject: {0},ToEmail: {1}", subject, sbToEmail.ToString()));
                        client.Send(message);
                        jd.Notifications('I', ("#gui-Email sent successfully."));
                    }
                }
            }
            catch (Exception ex)
            {
                JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
                jd.Notifications('E', "#gui Not able to send an Email " + ex.Message);
            }

        }

        public static string[] GetStringArray(string name)
        {
            string email = ConfigurationManager.AppSettings[name];
            return email.Split(SPLITTER, StringSplitOptions.RemoveEmptyEntries);
        }

        private static bool PrintViaLPR(PrinterTargetsNg target, string file, out string message)
        {
            string msg = string.Empty;
            bool result = false;
            StringBuilder args = new StringBuilder();

            args.Append("-S " + target.NetworkAddress);
            args.Append(" -P " + target.MatchKeyValue);

            if (target.TransferBinary)
            {
                args.Append(" -o l");
            }

            args.Append(" " + file);

            string output;

            Process p = new Process();

            p.StartInfo.FileName = "lpr.exe";
            p.StartInfo.Arguments = args.ToString();

            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.UseShellExecute = false;

            p.Start();

            //p.WaitForExit(180000);

            output = "OUTPUT: " + p.StandardOutput.ReadToEnd();
            output += Environment.NewLine;
            output += "ERROR: " + p.StandardError.ReadToEnd();

            //if (!p.HasExited)
            //{
            //    p.Kill();
            //    throw new ApplicationException(string.Format(
            //        "LPR attempt terminated after 3 minutes. ARGS={0}; OUTPUT={1}",
            //        args.ToString(),
            //        output));
            //}

            if (p.ExitCode == 0) { result = true; }

            message = output;
            return result;
        }

        protected void Refresh_icon_Click(object sender, ImageClickEventArgs e)
        {
            ExecuteSearch_Click(null, null);
        }

        private bool commandQueue(string source, string destination, string commandType, string transferMethod, string networkAddress, string matchKeyValue, bool transferBinary, JobProcessingModel jobInfo, string equipment, string msg = "", string matchKeyType = "", string AutomationString = "", int printerTargetsNgID = -1, int nextPrinterTargetsNgID = -1)
        {
            bool flag = false;
            try
            {
                CommandData item = new CommandData();
                item.Source = source;
                item.Destination = destination;
                item.CommandType = commandType;
                item.NetworkAddress = networkAddress;
                item.MatchKeyValue = matchKeyValue;
                item.TransferBinary = transferBinary;
                item.TransferMethod = transferMethod;
                item.ComposerId = jobInfo.ComposerId;
                item.JobNumber = jobInfo.Job;
                item.ProductNumber = jobInfo.Product;
                item.siteId = jobInfo.PrintSiteId;
                item.SplitNumber = jobInfo.Split;
                item.TaskId = jobInfo.StatusId;
                if (jobInfo.StatusId == 16 && jobInfo.NextTaskInProcess.ToLower() == "waiting to insert")
                    item.Equipment = "N/A";
                else
                    item.Equipment = equipment;
                item.OperatorName = WebSession.User.DisplayName;
                item.MatchKeyType = matchKeyType;
                item.AutomationString = AutomationString;
                item.PrinterTargetsNgID = printerTargetsNgID;
                item.NextPrinterTargetsNgID = nextPrinterTargetsNgID;

                string queueName = ConfigurationManager.AppSettings["MessageQueuePath"];
                MessageQueue queue = new MessageQueue(queueName);
                queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(CommandData) });

                System.Messaging.Message m = new System.Messaging.Message();
                m.Recoverable = true;

                var serializer = new XmlSerializer(typeof(CommandData));

                using (var stream = new StringWriter())
                {
                    serializer.Serialize(stream, item);
                    m.Body = stream.ToString();
                }
                queue.Send(m);

                JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
                string exceptionStatus = msg + " " + transferMethod + " File Transfer Request added to NCP FTP Processer service queue";

                jd.ChangeExceptionStatus(exceptionStatus, jobInfo.ComposerId, jobInfo.Job, jobInfo.Product, jobInfo.Split);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }

            return flag;
        }

        private bool FsstPrintRequest(int dpJobId, int jobNumber, string product, int splitNumber)
        {
            bool flag = false;
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            try
            {
                FsstRequest item = new FsstRequest();
                item.jobId = dpJobId;
                item.jobNumber = jobNumber;
                item.product = product;
                item.splitNumber = splitNumber;

                string queueName = ConfigurationManager.AppSettings["FsstQueuePath"];
                MessageQueue queue = new MessageQueue(queueName);
                queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(FsstRequest) });

                System.Messaging.Message m = new System.Messaging.Message();
                m.Recoverable = true;

                var serializer = new XmlSerializer(typeof(FsstRequest));

                using (var stream = new StringWriter())
                {
                    serializer.Serialize(stream, item);
                    m.Body = stream.ToString();
                }
                queue.Send(m);
                flag = true;
                jd.Notifications('I', "#gui - Fsst Print Request is sent to the Queue -" + queueName);
            }
            catch (Exception ex)
            {
                flag = false;
                jd.Notifications('E', ex.Message);
            }

            return flag;
        }

        protected void gvDetails_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i <= gvDetails.Rows.Count - 1; i++)
            {
                // Label lblparent = (Label)gvDetails.Rows[i].FindControl("Label2");
                //gvDetails.Rows[i].Cells[10].BackColor = Color.Green;
                //if (lblparent.Text == "1")
                //{
                //    gvDetails.Rows[i].Cells[10].BackColor = Color.Yellow;
                //    lblparent.ForeColor = Color.Black;
                //}
                //else
                //{
                //    gvDetails.Rows[i].Cells[10].BackColor = Color.Green;
                //    lblparent.ForeColor = Color.Yellow;
                //}

            }

        }

        protected void LinkReprintsSpolige_Click(object sender, EventArgs e)
        {
            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);

            //int id = DpJobsSession[selectedIndex].Id;
            var dpPrintDataAdapter = new DpPrintStatusDataAdapter();

            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

            var details = dpPrintDataAdapter.GetRemoteReprintsByJobProduct(dprintJob.Job, dprintJob.Product, dprintJob.Split, siteID);
            gvReprintDetails.DataSource = details;
            gvReprintDetails.DataBind();

            labRePrintSite.Text = siteName(dprintJob.PrintSiteId);
            if (dprintJob.ClientName.ToString().Length >= 19)
                labReCustomer.Text = dprintJob.ClientName.Split(' ')[0].ToString();
            else
                labReCustomer.Text = dprintJob.ClientName.ToString();
            labReJobComposerId.Text = dprintJob.ComposerId.ToString();
            labReSplitQty.Text = dprintJob.Quantity.ToString();
            labReJob.Text = dprintJob.Job.ToString();
            labReProduct.Text = dprintJob.Product.ToString();
            labReSequenceRange.Text = dprintJob.Sequences.ToString();
            labReSplit.Text = dprintJob.Split.ToString();
            labReSheetCount.Text = Convert.ToString(dprintJob.TotalSheetCount);
            labReImageCount.Text = Convert.ToString(dprintJob.TotalLogicalPageCount);
            labReReprintsSpolige.Text = Convert.ToString(dprintJob.QtyOfReprints);

            if (dprintJob.IsComplete == 1)
                labReIsComplete.Text = "Yes";
            else
                labReIsComplete.Text = "No";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            modalRePrintDetails.Show();
        }

        protected void btnReClose_Click(object sender, EventArgs e)
        {
            //ctlTimer.Enabled = true;
            ctlTimer.Interval = 30000;
        }

        protected void cbShowRawReprintData_CheckedChanged(object sender, EventArgs e)
        {
            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);

            //int id = DpJobsSession[selectedIndex].Id;
            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

            var dpPrintDataAdapter = new DpPrintStatusDataAdapter();
            if (cbShowRawReprintData.Checked == true)
                gvReprintDetails.DataSource = dpPrintDataAdapter.GetRawRemoteReprintsByjobProduct(dprintJob.Job, dprintJob.Product, dprintJob.Split, siteID);
            else
                gvReprintDetails.DataSource = dpPrintDataAdapter.GetRemoteReprintsByJobProduct(dprintJob.Job, dprintJob.Product, dprintJob.Split, siteID);

            gvReprintDetails.DataBind();

            //for (int i = 0; i <= gvReprintDetails.Rows.Count - 1; i++)
            //    gvReprintDetails.Rows[i].Cells[10].BackColor = Color.LightGray;

            labRePrintSite.Text = siteName(dprintJob.PrintSiteId);
            if (dprintJob.ClientName.ToString().Length >= 19)
                labReCustomer.Text = dprintJob.ClientName.Split(' ')[0].ToString();
            else
                labReCustomer.Text = dprintJob.ClientName.ToString();
            labReJobComposerId.Text = dprintJob.ComposerId.ToString();
            labReSplitQty.Text = dprintJob.Quantity.ToString();
            labReJob.Text = dprintJob.Job.ToString();
            labReProduct.Text = dprintJob.Product.ToString();
            labReSequenceRange.Text = dprintJob.Sequences.ToString();
            labReSplit.Text = dprintJob.Split.ToString();
            labReSheetCount.Text = Convert.ToString(dprintJob.TotalSheetCount);
            labReImageCount.Text = Convert.ToString(dprintJob.TotalLogicalPageCount);
            labReReprintsSpolige.Text = Convert.ToString(dprintJob.QtyOfReprints);

            if (dprintJob.IsComplete == 1)
                labReIsComplete.Text = "Yes";
            else
                labReIsComplete.Text = "No";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            modalRePrintDetails.Show();
        }

        protected void btnCloseWarning_Click(object sender, EventArgs e)
        {

        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            WriteTraceLog("btnYes_Click");
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();

            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);
            var mrdfInProcessFolder = GetDPSysConfigBySiteId("MRDFInProcess", siteID);

            if (mrdfInProcessFolder.SiteID != siteID)
            {
                mrdfInProcessFolder = GetDPSysConfig("MRDFInProcess");
            }
            var mrdfCompleteFolder = GetDPSysConfigBySiteId("MRDFComplete", siteID);

            if (mrdfCompleteFolder.SiteID != siteID)
            {
                mrdfCompleteFolder = GetDPSysConfig("MRDFComplete");
            }
            string equipmentName = Convert.ToString(ddEquipmentIdProcessJob.SelectedValue.ToString()); //string.Empty;
            RowColor newColorItem = new RowColor();
            if (callFromRightarrow != null && callFromRightarrow == true)
            {
                JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

                if ((int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")) == 18)
                {
                    //Move MRDF from "MRDFinProcess" to "MRDFComplete"

                    string sourceFileName = MrdfFileName(dprintJob, mrdfInProcessFolder.DPVariableValue);
                    /*xCopyOutPut xOutPut = xCopy(sourceFileName, mrdfCompleteFolder.DPVariableValue);  ------------------*/

                    if (commandQueue(sourceFileName, mrdfCompleteFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, dprintJob, equipmentName))
                    {
                        //msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - move command is sent to queue for moving mrdf file to CompleteFolder.");

                        jd.Notifications('I', "#gui -move command is sent By user to the queue for moving mrdf file to CompleteFolder.");

                        //Completeing a Task
                        int tid = jd.GetDprintTrackingTasksID(dprintJob.Id, (int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")));
                        int errorCode = jd.UpdateDprintTrackingTasks(dprintJob.Id, tid, true, equipmentNameSession, WebSession.User.DisplayName);

                        //Adding new task
                        int nextTaskId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                        var tempId = jd.InsertDprintTrackingTasks(dprintJob, WebSession.User.DisplayName, nextTaskId, equipmentNameSession);

                        int nextTask = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                        jd.setJobProcessingStatus(dprintJob.Id, nextTask);
                        newColorItem.rowPosition = dprintJob.ComposerId;
                        newColorItem.colourType = Color.Green;
                    }
                    else
                    {
                        jd.Notifications('E', "-job composer id - " + dprintJob.ComposerId.ToString() + "Error - move command send by the User is not added to queue for mrdfCompleteFolder. Please check the MSMQ configuration.");
                        newColorItem.rowPosition = dprintJob.ComposerId;
                        newColorItem.colourType = Color.Red;
                    }

                }
                else
                {
                    //Completeing a Task
                    int tid = jd.GetDprintTrackingTasksID(dprintJob.Id, (int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")));
                    int errorCode = jd.UpdateDprintTrackingTasks(dprintJob.Id, tid, true, equipmentName, WebSession.User.DisplayName);

                    //Adding new task
                    int nextTaskId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                    var tempId = jd.InsertDprintTrackingTasks(dprintJob, WebSession.User.DisplayName, nextTaskId, equipmentName);

                    int nextTask = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                    jd.setJobProcessingStatus(dprintJob.Id, nextTask, Convert.ToString(dprintJob.ScheduledMailDate));
                    newColorItem.rowPosition = dprintJob.ComposerId;
                    newColorItem.colourType = Color.Green;
                }
            }
            else
            {
                foreach (int i in sellectedIndedSession)
                {
                    if ((int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")) == 18)
                    {
                        //Move MRDF from "MRDFinProcess" to "MRDFComplete"

                        string sourceFileName = MrdfFileName(DpSelectedJobSession[i], mrdfInProcessFolder.DPVariableValue);
                        /*xCopyOutPut xOutPut = xCopy(sourceFileName, mrdfCompleteFolder.DPVariableValue);  ------------------*/

                        if (commandQueue(sourceFileName, mrdfCompleteFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, DpSelectedJobSession[i], equipmentName))
                        {
                            //msg.AppendLine("-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + " - move command is sent to queue for moving mrdf file to CompleteFolder.");

                            jd.Notifications('I', "#gui -move command is sent By user to the queue for moving mrdf file to CompleteFolder.");

                            //Completeing a Task
                            int tid = jd.GetDprintTrackingTasksID(DpSelectedJobSession[i].Id, (int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")));
                            int errorCode = jd.UpdateDprintTrackingTasks(DpSelectedJobSession[i].Id, tid, true, equipmentNameSession, WebSession.User.DisplayName);

                            //Adding new task
                            int nextTaskId = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                            var tempId = jd.InsertDprintTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, nextTaskId, equipmentNameSession);

                            int nextTask = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                            jd.setJobProcessingStatus(DpSelectedJobSession[i].Id, nextTask);
                            newColorItem.rowPosition = DpSelectedJobSession[i].ComposerId;
                            newColorItem.colourType = Color.Green;
                        }
                        else
                        {
                            jd.Notifications('E', "-job composer id - " + DpSelectedJobSession[i].ComposerId.ToString() + "Error - move command send by the User is not added to queue for mrdfCompleteFolder. Please check the MSMQ configuration.");
                            newColorItem.rowPosition = DpSelectedJobSession[i].ComposerId;
                            newColorItem.colourType = Color.Red;
                        }

                    }
                    else
                    {
                        //Completeing a Task
                        int tid = jd.GetDprintTrackingTasksID(DpSelectedJobSession[i].Id, (int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")));
                        int errorCode = jd.UpdateDprintTrackingTasks(DpSelectedJobSession[i].Id, tid, true, equipmentName, WebSession.User.DisplayName);

                        //Adding new task
                        int nextTaskId = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                        var tempId = jd.InsertDprintTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, nextTaskId, equipmentName);

                        int nextTask = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                        jd.setJobProcessingStatus(DpSelectedJobSession[i].Id, nextTask, Convert.ToString(DpSelectedJobSession[i].ScheduledMailDate));
                        newColorItem.rowPosition = DpSelectedJobSession[i].ComposerId;
                        newColorItem.colourType = Color.Green;
                    }

                }
            }
            rowColorSession = newColorItem;
            if (sellectedIndedSession != null)
                sellectedIndedSession.Clear();
            equipmentNameSession = string.Empty;
            //ctlTimer.Enabled = true;
            ctlTimer.Interval = 30000;
            ExecuteSearch_Click(null, null);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            RowColor newColorItem = new RowColor();
            if (callFromRightarrow != null && callFromRightarrow == true)
            {
                JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);
                newColorItem.rowPosition = dprintJob.ComposerId;
                // newColorItem.colourType = Color.Green;
            }
            else
            {
                foreach (int i in sellectedIndedSession)
                {
                    newColorItem.rowPosition = DpSelectedJobSession[i].ComposerId;
                    // newColorItem.colourType = Color.Green;
                }
            }

            rowColorSession = newColorItem;
            if (sellectedIndedSession != null)
                sellectedIndedSession.Clear();
            equipmentNameSession = string.Empty;
            //ctlTimer.Enabled = true;
            ctlTimer.Interval = 30000;
            ExecuteSearch_Click(null, null);
        }


        protected void btnNext_Click(object sender, ImageClickEventArgs e)
        {

            SetSessionForHeaderControls();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "<script>ClearErrorMsg();</script>", false);

            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            int dpLocalSiteID = Convert.ToInt32(ConfigurationManager.AppSettings["PrintSiteId"]);
            ImageButton btnRightArrow = sender as ImageButton;

            if (btnRightArrow != null)
            {
                selectedDprintJobsId = int.Parse(btnRightArrow.CommandArgument);
            }

            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

            if (WebSession.User.HasPermissionExplicit("JobProcessing.Manager") || WebSession.User.HasPermissionExplicit("JobProcessing.Supervisor") || WebSession.User.HasPermissionExplicit("JobProcessing.LeadOperator")
                || WebSession.User.HasPermissionExplicit("JobProcessing.Operator"))
            {
                ddEquipmentList.Items.Clear();
                Dictionary<string, string> equipmentIdDict = new Dictionary<string, string>();

                List<DpJobHistory> duplicateTaskList = new List<DpJobHistory>();

                int nextStatusId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                duplicateTaskList = jd.GetDuplicateTaskCheck(dprintJob.Id, getCurrentSiteId(), nextStatusId);

                if (dprintJob.PrintSiteId == dpLocalSiteID)
                {
                    if (dprintJob.StatusId == 15 || dprintJob.StatusId == 17)
                    {
                        if (dprintJob.StatusId == 17)
                        {
                            if (checkAccountPulls(dprintJob.Id, dprintJob.Job, dprintJob.Product, dprintJob.Split, dprintJob.FromSiteID, dprintJob.ClientNumber))
                                HiddenFieldAccountCheck.Value = "yes";
                            else
                                HiddenFieldAccountCheck.Value = "no";
                        }
                        else
                            HiddenFieldAccountCheck.Value = "na";

                        if (duplicateTaskList.Count > 0)
                        {
                            labEnvironment.Text = WebApplication.EnvironmentText;
                            labStatusInMessage.Text = duplicateTaskList.FirstOrDefault().DPrintTask;
                            labDateInMessage.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().StartedOn);
                            hidStartedOnUtcTime.Value = duplicateTaskList.FirstOrDefault().StartedOnUtcTime;
                            labCompletedOn.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().CompletedOn);
                            labOperatroName.Text = duplicateTaskList.FirstOrDefault().UpdatedBy;
                            labSiteNameInMessage.Text = dprintJob.PrintLocation;
                            labEquipmentInMessage.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().EquipmentUsed);
                            hidDupClientName.Value = dprintJob.ClientName;
                            hidDupProduct.Value = dprintJob.Product;
                            hidDupJob.Value = Convert.ToString(dprintJob.Job);
                            hidDupSplit.Value = Convert.ToString(dprintJob.Split);

                            labCautionMessage.Visible = true;
                            labCautionMessage.Text = "Duplicate Warning : Duplicate split is being processed";
                            ViewState["Duplicate"] = "1";
                        }
                        else
                        {
                            labCautionMessage.Text = string.Empty;
                            labCautionMessage.Visible = false;
                        }
                        EquipmentIdDataAdapter equipmentIdDataAdapter = new EquipmentIdDataAdapter();
                        Collection<EquipmentId> equipmentIdCollection = equipmentIdDataAdapter.GetEquipmentIdByTask(dprintJob.StatusId, dpLocalSiteID);
                        foreach (var equipmentId in equipmentIdCollection)
                            ddEquipmentList.Items.Add(new ListItem(equipmentId.EquipmentName, equipmentId.Id));
                        ddEquipmentList.DataTextField = "Text";
                        ddEquipmentList.DataValueField = "Value";
                        ddEquipmentList.DataBind();
                        ctlTimer.Interval = 1000 * 60 * 5;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                        if (ddEquipmentId.Text == "N/A")
                        {
                            ModalSelectEquipment.Show();

                        }
                        else
                        {
                            ModalSelectEquipment.Hide();
                            if (duplicateTaskList.Count > 0)
                            {
                                duplicateFoundFromNextClick = "Y";
                                ctlTimer.Interval = 1000 * 60 * 5;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                                modalDuplicateWarning.Show();
                            }
                            else
                            {

                                btnSelectEquipmentOK_Click(null, null);
                            }
                        }
                    }
                    else
                    {
                        if (duplicateTaskList.Count > 0)
                        {
                            labEnvironment.Text = WebApplication.EnvironmentText;
                            labStatusInMessage.Text = duplicateTaskList.FirstOrDefault().DPrintTask;
                            labDateInMessage.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().StartedOn);
                            hidStartedOnUtcTime.Value = duplicateTaskList.FirstOrDefault().StartedOnUtcTime;
                            labCompletedOn.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().CompletedOn);
                            labOperatroName.Text = duplicateTaskList.FirstOrDefault().UpdatedBy;
                            labSiteNameInMessage.Text = dprintJob.PrintLocation;
                            labEquipmentInMessage.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().EquipmentUsed);
                            hidDupClientName.Value = dprintJob.ClientName;
                            hidDupProduct.Value = dprintJob.Product;
                            hidDupJob.Value = Convert.ToString(dprintJob.Job);
                            hidDupSplit.Value = Convert.ToString(dprintJob.Split);
                            ctlTimer.Interval = 1000 * 60 * 5;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                            modalDuplicateWarning.Show();
                        }
                        else
                        {
                            duplicateCheckWarning(sender, e);
                        }
                    }
                }
                else
                {
                    jd.Notifications('E', "Local dp site is not matching with job print dp site.User is trying to process " + dprintJob.PrintLocation + "-job which belongs to different DP site.");
                }
            }
            else // if user is Operator , Lead , supervisor
            {
                List<DpJobHistory> duplicateTaskList = new List<DpJobHistory>();
                string equipmentName = ddEquipmentId.SelectedValue.ToString();
                var inserterInfo = GetInserter(equipmentName, Int32.Parse(ddProcessingLocation.SelectedValue));
                bool IsvirtualInserter = jd.CheckForVertualInserterCategoryByInserterID(Convert.ToString(inserterInfo.InserterMasterID));

                if (dprintJob.PrintSiteId == dpLocalSiteID)
                {
                    if (dprintJob.StatusId == 17 && IsvirtualInserter == true)
                    {
                        if (checkAccountPulls(dprintJob.Id, dprintJob.Job, dprintJob.Product, dprintJob.Split, dprintJob.FromSiteID, dprintJob.ClientNumber))
                        {
                            int nextStatusId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                            duplicateTaskList = jd.GetDuplicateTaskCheck(dprintJob.Id, getCurrentSiteId(), nextStatusId);

                            if (duplicateTaskList.Count > 0)
                            {
                                labEnvironment.Text = WebApplication.EnvironmentText;
                                labStatusInMessage.Text = duplicateTaskList.FirstOrDefault().DPrintTask;
                                labDateInMessage.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().StartedOn);
                                hidStartedOnUtcTime.Value = duplicateTaskList.FirstOrDefault().StartedOnUtcTime;
                                labCompletedOn.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().CompletedOn);
                                labOperatroName.Text = duplicateTaskList.FirstOrDefault().UpdatedBy;
                                labSiteNameInMessage.Text = dprintJob.PrintLocation;
                                labEquipmentInMessage.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().EquipmentUsed);
                                hidDupClientName.Value = dprintJob.ClientName;
                                hidDupProduct.Value = dprintJob.Product;
                                hidDupJob.Value = Convert.ToString(dprintJob.Job);
                                hidDupSplit.Value = Convert.ToString(dprintJob.Split);
                                ctlTimer.Interval = 1000 * 60 * 5;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                                modalDuplicateWarning.Show();
                            }
                            else
                            {
                                duplicateCheckWarning(sender, e);
                            }
                        }
                        else
                        {
                            // Popup message that  If there are no Account Details available, then Display a Message Box saying "No Account Details are Available"  
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                            ModalAccountPullWarning.Show();
                        }
                    }
                    else
                    {
                        int nextStatusId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                        duplicateTaskList = jd.GetDuplicateTaskCheck(dprintJob.Id, getCurrentSiteId(), nextStatusId);

                        if (duplicateTaskList.Count > 0)
                        {
                            labEnvironment.Text = WebApplication.EnvironmentText;
                            labStatusInMessage.Text = duplicateTaskList.FirstOrDefault().DPrintTask;
                            labDateInMessage.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().StartedOn);
                            hidStartedOnUtcTime.Value = duplicateTaskList.FirstOrDefault().StartedOnUtcTime;
                            labCompletedOn.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().CompletedOn);
                            labOperatroName.Text = duplicateTaskList.FirstOrDefault().UpdatedBy;
                            labSiteNameInMessage.Text = dprintJob.PrintLocation;
                            labEquipmentInMessage.Text = Convert.ToString(duplicateTaskList.FirstOrDefault().EquipmentUsed);
                            hidDupClientName.Value = dprintJob.ClientName;
                            hidDupProduct.Value = dprintJob.Product;
                            hidDupJob.Value = Convert.ToString(dprintJob.Job);
                            hidDupSplit.Value = Convert.ToString(dprintJob.Split);
                            ctlTimer.Interval = 1000 * 60 * 5;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                            modalDuplicateWarning.Show();
                        }
                        else
                        {
                            duplicateCheckWarning(sender, e);
                        }
                    }
                }
                else
                {
                    jd.Notifications('E', "Local dp site is not matching with job print dp site.User is trying to process " + dprintJob.PrintLocation + "-job which belongs to different DP site.");
                }
            }
        }

        private bool checkAccountPulls(int Id, int job, string product, int split, int fromSiteID, string clientNumber)
        {
            bool flag = false;
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            List<AccountPulls> accountPullsList = new List<AccountPulls>();
            accountPullsList = jd.GetAccountPulls(Id, job, product, split, Convert.ToString(fromSiteID), clientNumber);
            if (accountPullsList.Count > 0)
                flag = true;

            return flag;
        }
        private void sendEmailForDuplicateSplit(JobProcessingModel item)
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            if (checkTimeDifference(hidStartedOnUtcTime.Value, item.PrintSiteId))
            {
                string subject = "[W11.1] Duplicate Job/Split in Process at " + labSiteNameInMessage.Text;
                StringBuilder emailBody = new StringBuilder();
                emailBody.Append("A duplicate job is at the " + labStatusInMessage.Text + " step, Equipment " + labEquipmentInMessage.Text + ". It is being processed by Operator " + WebSession.User.DisplayName + ".");
                emailBody.Append("<br>");
                emailBody.Append("<br>");
                string htmlTable = "<table border=\"1\"><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Client</td><td style=\"text-align:left;font-size:medium;width: 200px;\"> " + item.ClientName + "</td></tr><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Job </td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + Convert.ToString(item.Job) + "</td></tr> <tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Product</td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + item.Product + "</td></tr><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Split</td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + Convert.ToString(item.Split) + "</td></tr><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">PrintSite</td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + labSiteNameInMessage.Text + "</td></tr><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Total Volume</td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + Convert.ToString(item.Quantity) + "</td></tr><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Total Sheets</td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + Convert.ToString(item.TotalSheetCount) + "</td></tr><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Environment</td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + labEnvironment.Text + "</td></tr></table>";
                emailBody.Append(htmlTable);
                emailBody.Append("<br>");

                if (string.IsNullOrEmpty(labCompletedOn.Text))
                    emailBody.AppendLine("This Split was previously moved to " + labStatusInMessage.Text + " on " + labDateInMessage.Text + " by Operator " + labOperatroName.Text + " in " + labSiteNameInMessage.Text + ". The Split is not completed this process. Current Date/Time is: " + DateTime.Now.ToString());
                else
                    emailBody.AppendLine("This Split was previously moved to " + labStatusInMessage.Text + " on " + labDateInMessage.Text + " by Operator " + labOperatroName.Text + " in " + labSiteNameInMessage.Text + ". The Split completed this process on " + labCompletedOn.Text + ". Current Date/Time is: " + DateTime.Now.ToString());

                if (labEquipmentInMessage.Text != "N/A")
                    emailBody.AppendLine(" Split was moved to Equipment " + labEquipmentInMessage.Text + ".");

                sendEmail(subject, emailBody.ToString());
            }
            else
            {
            }

        }

        private bool checkTimeDifference(string startTime, int siteID)
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            string min = jd.SystemConfiguration(siteID, "DuplicateSplitEmailNotificationWaitTime");

            DateTime start = Convert.ToDateTime(startTime);

            DateTime currentTime = DateTime.UtcNow;

            TimeSpan difference = currentTime - start;

            if (difference.TotalMinutes > Convert.ToInt32(min))
                return true;
            else
                return false;

        }
        private void duplicateCheckWarning(object sender, ImageClickEventArgs e, string adminEquipment = "")
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            var index = 0;
            RowColor newColorItem = new RowColor();

            List<int> items = new List<int>();
            StringBuilder msg = new StringBuilder();
            StringBuilder log = new StringBuilder();

            LodEquipmentId();
            ddEquipmentIdProcessJob.ClearSelection();
            ListItem item = ddEquipmentIdProcessJob.Items.FindByText(ddEquipmentId.SelectedItem.Text.ToString().Trim());
            if (item != null)
            {
                item.Selected = true;
            }

            string equipmentName = string.Empty;
            if (string.IsNullOrEmpty(adminEquipment))
            {
                equipmentName = ddEquipmentIdProcessJob.SelectedValue.ToString();
            }
            else
            {
                equipmentName = adminEquipment;
            }

            var inserterInfo = GetInserter(equipmentName, Int32.Parse(ddProcessingLocation.SelectedValue));

            equipmentNameSession = inserterInfo.InserterName;

            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

            var printerStagingFolder = GetDPSysConfigBySiteId("SpoolFileStaging", siteID);
            if (printerStagingFolder.SiteID != siteID)
            {
                printerStagingFolder = GetDPSysConfig("SpoolFileStaging");
            }
            var PrintingInProcessFolder = GetDPSysConfigBySiteId("PrintingInProcess", siteID);

            if (PrintingInProcessFolder.SiteID != siteID)
            {
                PrintingInProcessFolder = GetDPSysConfig("PrintingInProcess");
            }
            var PrintingCompleteFolder = GetDPSysConfigBySiteId("PrintingComplete", siteID);

            if (PrintingCompleteFolder.SiteID != siteID)
            {
                PrintingCompleteFolder = GetDPSysConfig("PrintingComplete");
            }

            //// printerStagingFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrinterStaging\";
            //// PrintingInProcessFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrintingInProcess\";
            //// PrintingCompleteFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrintingComplete\";          
            ////
            var mrdfStagingFolder = GetDPSysConfigBySiteId("MRDFStaging", siteID);

            if (mrdfStagingFolder.SiteID != siteID)
            {
                mrdfStagingFolder = GetDPSysConfig("MRDFStaging");
            }
            var mrdfInProcessFolder = GetDPSysConfigBySiteId("MRDFInProcess", siteID);

            if (mrdfInProcessFolder.SiteID != siteID)
            {
                mrdfInProcessFolder = GetDPSysConfig("MRDFInProcess");
            }
            var mrdfCompleteFolder = GetDPSysConfigBySiteId("MRDFComplete", siteID);

            if (mrdfCompleteFolder.SiteID != siteID)
            {
                mrdfCompleteFolder = GetDPSysConfig("MRDFComplete");
            }

            //// mrdfStagingFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFStaging\";
            //// mrdfInProcessFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFInProcess\";
            //// mrdfCompleteFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFComplete\";

            jd.Notifications('I', "#1 gui - eq " + inserterInfo.InserterName + "statging path-" + mrdfStagingFolder.DPVariableValue.ToString() + "InProcess path - " + mrdfInProcessFolder.DPVariableValue.ToString());
            jd.Notifications('I', "#2 gui - eq - " + equipmentName + " - " + printerStagingFolder.DPVariableValue + "-" + PrintingInProcessFolder.DPVariableValue + "-" + PrintingCompleteFolder.DPVariableValue);

            // selectedIndex = gvDpJobs.SelectedRow.RowIndex;  
            /*
            if (sender != null)
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                selectedIndex = gvRow.RowIndex;

                items.Add(selectedIndex);
                sellectedIndedSession = items;
            }
            else
                items = sellectedIndedSession;


            DpSelectedJobSession = DpJobsSession;
            */
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

            //  foreach (int i in items)
            //  {
            if (sender == null && string.IsNullOrEmpty(adminEquipment))
            {

                sendEmailForDuplicateSplit(dprintJob);
                jd.Notifications('D', "Duplicate Job Message presented to Operator - Operator DID Continue. Print Site " + labSiteNameInMessage.Text + "," + dprintJob.ClientName + "," + Convert.ToString(dprintJob.Job) + "," + dprintJob.Product + " and Split " + Convert.ToString(dprintJob.Split) + ",Operator " + WebSession.User.DisplayName + ", Step " + labStatusInMessage.Text);
            }
            else if (!string.IsNullOrEmpty(adminEquipment) && !string.IsNullOrEmpty(labCautionMessage.Text))
            {
                sendEmailForDuplicateSplit(dprintJob);
                jd.Notifications('D', "Duplicate Job Message presented to Operator - Operator DID Continue. Print Site " + labSiteNameInMessage.Text + "," + dprintJob.ClientName + "," + Convert.ToString(dprintJob.Job) + "," + dprintJob.Product + " and Split " + Convert.ToString(dprintJob.Split) + ",Operator " + WebSession.User.DisplayName + ", Step " + labStatusInMessage.Text);
            }

            if (dprintJob.IsComplete == 0)
            {
                if ((int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")) == 17)
                {
                    //string sourceFileName = MrdfFileName(dprintJob, mrdfStagingFolder.DPVariableValue);
                    //jd.Notifications('I', "#3 gui - get source file -" + sourceFileName);
                    jd.Notifications('I', "# gui - moving MRDF file from direct next image button for Composer Id " + dprintJob.ComposerId.ToString() + " and Status is " + dprintJob.Status);
                    string sourceFileName = MrdfFileName(dprintJob, mrdfStagingFolder.DPVariableValue);

                    if (sourceFileName.Length == 0)
                    {
                        sourceFileName = MrdfFileName(dprintJob, mrdfInProcessFolder.DPVariableValue);
                    }
                    if (sourceFileName.Length == 0)
                    {
                        sourceFileName = MrdfFileName(dprintJob, mrdfCompleteFolder.DPVariableValue);
                    }
                    jd.Notifications('I', "#3 gui - get source file -" + sourceFileName + " for Composer Id " + dprintJob.ComposerId.ToString());

                    if (string.IsNullOrEmpty(sourceFileName))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["MRDFfileNotFoundInFolderPath"])))
                        {
                            msg.AppendLine("Inserter Control File (MRDF) " + Convert.ToString(Session["MRDFfileNotFoundInFolderPath"]) + " could not be located in the " + mrdfStagingFolder.DPVariableValue + " folder or the " + mrdfInProcessFolder.DPVariableValue + " or the " + mrdfCompleteFolder.DPVariableValue + ". Job will need to be restarted.");

                            jd.Notifications('I', "# gui - MRDF file " + Convert.ToString(Session["MRDFfileNotFoundInFolderPath"]) + " could not be located in the " + mrdfStagingFolder.DPVariableValue + " folder or the " + mrdfInProcessFolder.DPVariableValue + " or the " + mrdfCompleteFolder.DPVariableValue + ".");

                            sendEmail("Inserter Control File (MRDF) " + Convert.ToString(Session["MRDFfileNotFoundInFolderPath"]) + " for this Split could not be found", "The file could not be located in the " + mrdfStagingFolder.DPVariableValue + " folder or the " + mrdfInProcessFolder.DPVariableValue + " or the " + mrdfCompleteFolder.DPVariableValue + ". Job will need to be restarted.Operator " + WebSession.User.DisplayName + " received this error for: Job=" + Convert.ToString(dprintJob.Job) + " , Product = " + dprintJob.Product + " , Split = " + Convert.ToString(dprintJob.Split));
                            Session["MRDFfileNotFoundInFolderPath"] = string.Empty;
                            newColorItem.rowPosition = dprintJob.ComposerId;
                            newColorItem.colourType = Color.Red;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "<script>DisplayErrorMsg('" + HttpUtility.JavaScriptStringEncode(msg.ToString()) + "');</script>", false);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(Session["MRDFfileNotFoundInContainerContents"])))
                        {
                            jd.Notifications('I', "# gui - " + Convert.ToString(Session["MRDFfileNotFoundInContainerContents"]));

                            msg.AppendLine(Convert.ToString(Session["MRDFfileNotFoundInContainerContents"]));

                            Session["MRDFfileNotFoundInContainerContents"] = string.Empty;
                            newColorItem.rowPosition = dprintJob.ComposerId;
                            newColorItem.colourType = Color.Red;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "<script>DisplayErrorMsg('" + HttpUtility.JavaScriptStringEncode(msg.ToString()) + "');</script>", false);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(Session["MRDFfileError"])))
                        {
                            jd.Notifications('I', "# gui - " + Convert.ToString(Session["MRDFfileError"]));
                            msg.AppendLine(Convert.ToString(Session["MRDFfileError"]));
                            string errMessage = Convert.ToString(Session["MRDFfileError"]);
                            Session["MRDFfileError"] = string.Empty;
                            newColorItem.rowPosition = dprintJob.ComposerId;
                            newColorItem.colourType = Color.Red;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "<script>DisplayErrorMsg('" + HttpUtility.JavaScriptStringEncode(msg.ToString()) + "');</script>", false);
                        }
                        try
                        {
                            string filesnames = string.Format("{0}_{1}_{2}_{3}_{4}.NCP", dprintJob.Product.ToString().PadLeft(5, '0'),
                                dprintJob.Job.ToString().PadLeft(7, '0'), dprintJob.Split.ToString().PadLeft(3, '0'),
                                dprintJob.Sequences.Split('-')[1].Trim().ToString().PadLeft(7, '0'), dprintJob.ComposerId.ToString().PadLeft(7, '0'));
                            commandQueue(filesnames, "", "email", "", "", string.Empty, false, dprintJob, equipmentName);
                        }
                        catch { }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(inserterInfo.TransferMethod))
                        {
                            newColorItem.rowPosition = dprintJob.ComposerId;
                            newColorItem.colourType = Color.Red;
                            //break;
                        }
                        else
                        {
                            if (inserterInfo.TransferMethod.ToLower().Contains("ftp"))
                            {
                                //FTP
                                if (commandQueue(sourceFileName, inserterInfo.ImportFolder, "COPY", "FTP", inserterInfo.IPAddress, string.Empty, false, dprintJob, equipmentName))
                                {
                                    msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - MRDF is send to ftp queue.");
                                    jd.Notifications('I', "# gui - job composer id -" + dprintJob.ComposerId.ToString() + " - MRDF is send to ftp queue.FTP source - " + sourceFileName + " and destination - " + inserterInfo.ImportFolder);
                                    //Completeing a Task
                                    int tid = jd.GetDprintTrackingTasksID(dprintJob.Id, (int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")));
                                    jd.Notifications('I', "#gui before updating DPrintTrackingTasks for Composer Id " + dprintJob.ComposerId.ToString() + "," + "Equipment name " + equipmentName);
                                    int errorCode = jd.UpdateDprintTrackingTasks(dprintJob.Id, tid, true, inserterInfo.InserterName, WebSession.User.DisplayName);
                                    jd.Notifications('I', "#gui after updating DPrintTrackingTasks for Composer Id " + dprintJob.ComposerId.ToString() + "," + "Equipment name " + equipmentName + " and Task Id" + tid.ToString());
                                    //Adding A Task
                                    int nextTaskId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                                    jd.Notifications('I', "#gui before adding new task the next task id is" + nextTaskId.ToString());
                                    var tempId = jd.InsertDprintTrackingTasks(dprintJob, WebSession.User.DisplayName, nextTaskId, inserterInfo.InserterName);
                                    jd.Notifications('I', "#gui after Adding new task for Composer Id " + dprintJob.ComposerId.ToString() + "and result is" + tempId.ToString());
                                    msg.AppendLine("Task id " + Convert.ToString(tempId.ToString()) + " is added with next - traking task " + Convert.ToString(nextTaskId));



                                    if (commandQueue(sourceFileName, mrdfInProcessFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, dprintJob, equipmentName, "FTP and "))
                                    {
                                        msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - move command is sent to command queue for moving mrdfInProcessFolder.");
                                        jd.Notifications('I', "#gui -job composer id - " + dprintJob.ComposerId.ToString() + " - move command is sent for file-" + sourceFileName + " to command queue for moving " + mrdfInProcessFolder.DPVariableValue + " folder.");
                                        newColorItem.rowPosition = dprintJob.ComposerId;
                                        newColorItem.colourType = Color.Green;
                                        checkForQtyPullsAfterPrint(Convert.ToString(inserterInfo.InserterMasterID), Convert.ToString(dprintJob.Id));
                                    }
                                    else
                                    {
                                        msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - move command is not added to queue for mrdfInProcessFolder. Please check the MSMQ configuration");
                                        jd.Notifications('I', "#gui -job composer id - " + dprintJob.ComposerId.ToString() + " Error - move command is not added  for file-" + sourceFileName + " to " + mrdfInProcessFolder.DPVariableValue + " folder.Please check the MSMQ configuration");
                                        newColorItem.rowPosition = dprintJob.ComposerId;
                                        newColorItem.colourType = Color.Red;
                                    }
                                }
                                else
                                {
                                    newColorItem.rowPosition = dprintJob.ComposerId;
                                    newColorItem.colourType = Color.Red;
                                    msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - ftp command is not added to queue. Please check the MSMQ configuration.");
                                    msg.AppendLine("ftp source - " + sourceFileName);
                                    msg.AppendLine("destination - " + inserterInfo.ImportFolder);
                                    SendNotificationReport(inserterInfo.InserterName, msg.ToString());

                                    jd.Notifications('E', "# gui - MRDF file - ftp command is not added to queue for job composer id - " + dprintJob.ComposerId.ToString() + "-" + "ftp source - " + sourceFileName + " - " + "destination - " + inserterInfo.ImportFolder);
                                }

                            }
                            else if (inserterInfo.TransferMethod.ToLower().Contains("xcopy"))
                            {
                                //XCOPY
                                string importFolder = @"\\" + inserterInfo.ServerControllerName + "\\" + inserterInfo.ImportFolder;
                                jd.Notifications('I', "#3 gui - get Import folder -" + importFolder);

                                bool xQueue = commandQueue(sourceFileName, importFolder, "COPY", "Xcopy", string.Empty, string.Empty, false, dprintJob, equipmentName);
                                if (xQueue)
                                {
                                    msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - xcopy command sent to queue to copy MRDF to import folder.");
                                    jd.Notifications('I', "# gui - job composer id -" + dprintJob.ComposerId.ToString() + " - xcopy command sent to queue to copy MRDF file - " + sourceFileName + " to import folder - " + importFolder + ".");
                                }
                                else
                                {
                                    msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - xcopy command is not added to queue. Please check the MSMQ configuration.");
                                    jd.Notifications('I', "# gui - job composer id -" + dprintJob.ComposerId.ToString() + " - Error - xcopy command is not added to queue to copy MRDF file - " + sourceFileName + " to import folder - " + importFolder + ".Please check the MSMQ configuration.");
                                }

                                if (xQueue == false)
                                {
                                    newColorItem.rowPosition = dprintJob.ComposerId;
                                    newColorItem.colourType = Color.Red;
                                    SendNotificationReport(inserterInfo.InserterName, msg.ToString());
                                    jd.Notifications('E', "# gui - MRDF file is not copied.");
                                }
                                else
                                {
                                    jd.Notifications('I', "#4 gui - xcopy command sent to queue to copy MRDF file - " + sourceFileName + " to import folder - " + importFolder + ".");
                                    //Completeing a Task
                                    int tid = jd.GetDprintTrackingTasksID(dprintJob.Id, (int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")));
                                    jd.Notifications('I', "#gui before updating DPrintTrackingTasks for Composer Id " + dprintJob.ComposerId.ToString() + "," + "Equipment name " + equipmentName);
                                    int errorCode = jd.UpdateDprintTrackingTasks(dprintJob.Id, tid, true, inserterInfo.InserterName, WebSession.User.DisplayName);
                                    jd.Notifications('I', "#gui after updating DPrintTrackingTasks for Composer Id " + dprintJob.ComposerId.ToString() + "," + "Equipment name " + equipmentName + " and Task Id" + tid.ToString());
                                    //Adding A Task
                                    int nextTaskId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                                    jd.Notifications('I', "#gui before adding new task the next task id is" + nextTaskId.ToString());
                                    var tempId = jd.InsertDprintTrackingTasks(dprintJob, WebSession.User.DisplayName, nextTaskId, inserterInfo.InserterName);
                                    jd.Notifications('I', "#gui after Adding new task for Composer Id " + dprintJob.ComposerId.ToString() + "and result is" + tempId.ToString());
                                    msg.AppendLine("Task id " + Convert.ToString(tempId.ToString()) + " is added with next - traking task " + Convert.ToString(nextTaskId));

                                    if (commandQueue(sourceFileName, mrdfInProcessFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, dprintJob, equipmentName))
                                    {
                                        msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - move command is sent to command queue for moving mrdfInProcessFolder.");
                                        jd.Notifications('I', "#gui -job composer id - " + dprintJob.ComposerId.ToString() + " - move command is sent for file-" + sourceFileName + " to command queue for moving " + mrdfInProcessFolder.DPVariableValue + " folder.");
                                    }
                                    else
                                    {
                                        msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - move command is not added to queue for mrdfInProcessFolder. Please check the MSMQ configuration");
                                        jd.Notifications('I', "#gui -job composer id - " + dprintJob.ComposerId.ToString() + " Error - move command is not added  for file-" + sourceFileName + " to " + mrdfInProcessFolder.DPVariableValue + " folder.Please check the MSMQ configuration");
                                    }
                                    newColorItem.rowPosition = dprintJob.ComposerId;
                                    newColorItem.colourType = Color.Green;
                                    checkForQtyPullsAfterPrint(Convert.ToString(inserterInfo.InserterMasterID), Convert.ToString(dprintJob.Id));
                                }
                            }
                        }
                    }

                }
                else if ((int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")) == 21 || (int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")) == 20)
                {
                    //Completeing a Task
                    int tid = jd.GetDprintTrackingTasksID(dprintJob.Id, (int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")));
                    int errorCode = jd.UpdateDprintTrackingTasks(dprintJob.Id, tid, true, equipmentName, WebSession.User.DisplayName);

                    //Adding new task
                    int nextTaskId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                    var tempId = jd.InsertDprintTrackingTasks(dprintJob, WebSession.User.DisplayName, nextTaskId, equipmentName);

                    int nextTask = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                    jd.setJobProcessingStatus(dprintJob.Id, nextTask, Convert.ToString(dprintJob.ScheduledMailDate));

                    newColorItem.rowPosition = dprintJob.ComposerId;
                    //newColorItem.colourType = Color.Green;
                    //Calculate time , Days LAte adn FTP hand off date

                    //US21435- Move Container file when Job is Completed
                    var jobStatusCode = (int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", ""));
                    ContainerFileMovement(msg, jd, equipmentName, siteID, dprintJob, jobStatusCode);

                    var dpPrintDataAdapter = new DpPrintStatusDataAdapter();

                    var details = dpPrintDataAdapter.GetDpJobHistories(dprintJob.Id, siteID).ToList();
                    jd.DaysTimeUpdate(dprintJob.Id, details, Convert.ToString(dprintJob.ScheduledMailDate));

                    //Print the Final Summary ticket
                    int flag = -1;
                    try
                    {
                        flag = Convert.ToInt32(jd.SystemConfiguration(dprintJob.PrintSiteId, "FinalSplitTicketSummaryControlFlag"));
                    }
                    catch (Exception ex)
                    {
                        jd.Notifications('E', "# gui - Final SplitTicketSummary Control Flag is not configured correctly.");
                    }

                    if (flag == 1)
                    {
                        if (FsstPrintRequest(dprintJob.Id, dprintJob.Job, dprintJob.Product, dprintJob.Split))
                            msg.Append("Final summary ticket print request is sent to the printer");
                        jd.Notifications('I', "Final summary ticket print request is sent to the printer with flag : " + flag.ToString());
                    }
                    else if (flag == 2)
                    {
                        //2 = create PDF and display PDF only
                        displayFinalSplitTicketSumary(dprintJob);
                    }
                    else if (flag == 3)
                    {
                        //3 =  create PDF and display PDF and Print
                        if (FsstPrintRequest(dprintJob.Id, dprintJob.Job, dprintJob.Product, dprintJob.Split))
                            msg.Append("Final summary ticket print request is sent to the printer");
                        jd.Notifications('I', "Final summary ticket print request is sent to the printer with flag : " + flag.ToString());
                        displayFinalSplitTicketSumary(dprintJob);
                    }
                }

                else if ((int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")) == 16)
                {
                    jd.Notifications('I', "# 3 gui - moving spool file from direct next image button.");
                    var spoolFile = GetSoolFileInfo(PrintingInProcessFolder.DPVariableValue, dprintJob);
                    bool isMessage = false;
                    foreach (FileInfo sf in spoolFile)
                    {
                        if (commandQueue(sf.FullName, PrintingCompleteFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, dprintJob, equipmentName))
                        {
                            if (isMessage == false)
                            {
                                msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - move command is set to command queue for moving spool file (" + sf.Name + ") to PrintingCompleteFolder.");
                                newColorItem.rowPosition = dprintJob.ComposerId;
                                newColorItem.colourType = Color.Green;
                                isMessage = true;
                            }
                        }
                        else
                        {
                            if (isMessage == false)
                            {
                                newColorItem.rowPosition = dprintJob.ComposerId;
                                newColorItem.colourType = Color.Red;
                                msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - move command is not added to queue for moving spool file (" + sf.Name + ") toPrintingCompleteFolder. Please check the MSMQ configuration.");
                                isMessage = true;
                            }
                        }
                    }

                    //Completeing a Task
                    int tid = jd.GetDprintTrackingTasksID(dprintJob.Id, (int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")));
                    jd.Notifications('I', "#gui before updating DPrintTrackingTasks for Composer Id " + dprintJob.ComposerId.ToString() + "," + "Equipment name " + equipmentName);
                    int errorCode = jd.UpdateDprintTrackingTasks(dprintJob.Id, tid, true, equipmentName, WebSession.User.DisplayName);
                    jd.Notifications('I', "#gui after updating DPrintTrackingTasks for Composer Id " + dprintJob.ComposerId.ToString() + "," + "Equipment name " + equipmentName + " and Task Id" + tid.ToString());
                    //Adding new task
                    int nextTaskId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                    jd.Notifications('I', "#gui before adding new task the next task id is" + nextTaskId.ToString());
                    var tempId = jd.InsertDprintTrackingTasks(dprintJob, WebSession.User.DisplayName, nextTaskId, equipmentName);
                    jd.Notifications('I', "#gui after Adding new task for Composer Id " + dprintJob.ComposerId.ToString() + "and result is" + tempId.ToString());
                    int nextTask = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                    jd.Notifications('I', "#gui before setting  job processing status the next task is " + nextTask.ToString());
                    jd.setJobProcessingStatus(dprintJob.Id, nextTask);
                    jd.Notifications('I', "#gui Set job processing status for print job Id " + dprintJob.Id.ToString() + " and Composer Id " + dprintJob.ComposerId.ToString());

                }

                else if ((int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")) == 15)
                {

                    if (string.IsNullOrEmpty(equipmentName))
                    {
                        msg.Clear();
                        msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Equipment is not found.");
                        laJobprocessing.ForeColor = Color.Red;
                        newColorItem.rowPosition = dprintJob.ComposerId;
                        newColorItem.colourType = Color.Red;
                    }
                    else
                    {
                        jd.Notifications('I', "# 3 gui - moving spool file from direct next image button.");
                        int printersNgID = jd.GetPrinterNgIdByEquipment(equipmentName, Int32.Parse(ddProcessingLocation.SelectedValue));
                        var spoolFile = GetSoolFileInfo(printerStagingFolder.DPVariableValue, dprintJob);//"JET1D1";
                        var spoolFileForMail = GetSoolFileInfoForMail(dprintJob);
                        jd.Notifications('I', "# gui spool file count " + spoolFile.Count() + " in " + printerStagingFolder.DPVariableValue + "for Composer Id " + dprintJob.ComposerId.ToString() + " and Status is" + dprintJob.Status);
                        if (spoolFile.Count() == 0)
                        {
                            spoolFile = GetSoolFileInfo(PrintingInProcessFolder.DPVariableValue, dprintJob);

                            jd.Notifications('I', "# gui spool file count " + spoolFile.Count() + " in " + PrintingInProcessFolder.DPVariableValue + "for Composer Id " + dprintJob.ComposerId.ToString() + " and Status is" + dprintJob.Status);
                        }
                        if (spoolFile.Count() == 0)
                        {
                            spoolFile = GetSoolFileInfo(PrintingCompleteFolder.DPVariableValue, dprintJob);

                            jd.Notifications('I', "# gui spool file count " + spoolFile.Count() + " in " + PrintingCompleteFolder.DPVariableValue + "for Composer Id " + dprintJob.ComposerId.ToString() + " and Status is" + dprintJob.Status);
                        }
                        jd.Notifications('I', "#4 gui - PrintersNg Id found - " + printersNgID);

                        jd.Notifications('I', "# 5 gui - spoolFilePath count - " + spoolFile.Count.ToString());

                        //US22212-ReloadContainer files
                        if (spoolFile.Count() == 0)
                        {
                            spoolFile = ReprocessContainerContent(jd, equipmentName, siteID, printerStagingFolder, dprintJob, spoolFile, spoolFileForMail);
                        }

                        if (spoolFile.Count() > 0)
                        {
                            int fileMatchingCount = 0;
                            string allSpoolFileName = string.Empty;
                            foreach (FileInfo spoolFileForMatch in spoolFile)
                            {
                                jd.Notifications('I', "# 6 gui - spoolFilePath - " + spoolFileForMatch.FullName);
                                allSpoolFileName = allSpoolFileName + " " + spoolFileForMatch.Name;
                                if (printersNgID != 0)
                                {
                                    string spoolFileName = spoolFileForMatch.Name;
                                    var printerTargets = jd.GetPrinterTargetsNg(printersNgID, spoolFileName);

                                    jd.Notifications('I', "# 7 gui - printer target count - " + printerTargets.Count.ToString());

                                    if (printerTargets.Count > 1)
                                    {
                                        newColorItem.rowPosition = dprintJob.ComposerId;
                                        newColorItem.colourType = Color.Red;
                                        string emailTitel = "[W11.3] Printer Master / Printer Targets Configuration Error in " + dprintJob.PrintLocation;
                                        string emailBody = "Multiple records returned from PrinterTargetsNg table during single record lookup.Check MatchKeyType and MatchKeyValue configurations.";
                                        emailBody += Environment.NewLine + "Please resolve. Job = " + dprintJob.Job.ToString() + " Product = " + dprintJob.Product + " Split = " + dprintJob.Split.ToString() + " PrinterNgID = " + printersNgID.ToString() + " and MatchKeyValue = " + spoolFileName;
                                        sendEmail(emailTitel, emailBody);
                                        jd.Notifications('I', "# 8 gui - Multiple records returned from PrinterTargetsNg table during single record lookup.Check MatchKeyType and MatchKeyValue configurations.");
                                        msg.AppendLine("Multiple records returned from PrinterTargetsNg table during single record lookup.Check MatchKeyType and MatchKeyValue configurations.");
                                    }
                                    else if (printerTargets.Count == 1)
                                    {
                                        string pattern = @"\[\d+\]";
                                        string MatchKeyType = Regex.IsMatch(printerTargets[0].MatchKeyType.ToLower(), pattern) == true ? printerTargets[0].MatchKeyType.ToLower() : string.Empty;
                                        int NextPrinterTargetsNgID = -1;
                                        string CommandType = "copy";
                                        string AutomationString = string.Empty;
                                        var printerTargetExeCmd = jd.GetPrinterTargetsNgExecuteCommand(printerTargets[0].NextPrinterTargetsNgID);
                                        if (printerTargetExeCmd.Count == 1 && printerTargetExeCmd[0].TargetPurpose.ToLower() == "execute command")
                                        {
                                            CommandType = "execute command";
                                            AutomationString = printerTargetExeCmd[0].AutomationString;
                                            NextPrinterTargetsNgID = printerTargets[0].NextPrinterTargetsNgID;
                                        }
                                        fileMatchingCount++;
                                        if (printerTargets[0].TransferMethod.Trim().ToLower().Contains("xcopy") || printerTargets[0].TransferMethod.Trim().ToLower().Contains("ftp"))
                                        {
                                            if (commandQueue(spoolFileForMatch.FullName, printerTargets[0].DropOffLocation, CommandType, printerTargets[0].TransferMethod, printerTargets[0].NetworkAddress, printerTargets[0].MatchKeyValue, false, dprintJob, equipmentName, string.Empty, MatchKeyType, AutomationString, printerTargets[0].PrinterTargetsNgID, NextPrinterTargetsNgID))
                                            {
                                                msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - xcopy command is sent to queue to copy Spool file at Drop of location.");

                                                jd.Notifications('I', "#gui - Spool file is send to xcopy command for to Drop of location queue");
                                                laJobprocessing.ForeColor = Color.Black;

                                                /* xCopyOutPut xPinPOutput = xCopy(spoolFilePath, PrintingInProcessFolder.DPVariableValue); --------- */
                                                bool isMessage = false;
                                                foreach (FileInfo sf in spoolFile)
                                                {
                                                    if (commandQueue(sf.FullName, PrintingInProcessFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, dprintJob, equipmentName))
                                                    {
                                                        if (isMessage == false)
                                                        {
                                                            msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - move command is sent to command queue to move spool file to ProcessFolder.");
                                                            jd.Notifications('I', "#gui - command is sent to queue to move spool file to" + PrintingInProcessFolder.DPVariableValue + " - folder for job composer id - " + dprintJob.ComposerId.ToString() + ".Equipment Name-" + equipmentName + "spool file -" + sf.FullName);
                                                            isMessage = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (isMessage == false)
                                                        {
                                                            msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - move command is not added to queue for to PrintingInProcessFolder folder. Please check the MSMQ configuration.");
                                                            jd.Notifications('E', "#gui - move command is not added to queue for to PrintingInProcessFolder folder. Please check the MSMQ configuration");
                                                            jd.Notifications('E', "#gui - move command is not added to queue for to PrintingInProcessFolder folder for " + dprintJob.ComposerId.ToString() + ".Please check the MSMQ configuration");
                                                            isMessage = true;
                                                        }
                                                    }
                                                }

                                                //Completeing a Task
                                                int tid = jd.GetDprintTrackingTasksID(dprintJob.Id, (int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")));
                                                jd.Notifications('I', "#gui before updating DPrintTrackingTasks for Composer Id " + dprintJob.ComposerId.ToString() + "," + "Equipment name " + equipmentName);
                                                int errorCode = jd.UpdateDprintTrackingTasks(dprintJob.Id, tid, true, equipmentName, WebSession.User.DisplayName);
                                                jd.Notifications('I', "#gui after updating DPrintTrackingTasks for Composer Id " + dprintJob.ComposerId.ToString() + "," + "Equipment name " + equipmentName + " and Task Id" + tid.ToString());

                                                //Adding new task
                                                int nextTaskId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                                                jd.Notifications('I', "#gui before adding new task the next task id is" + nextTaskId.ToString());
                                                var tempId = jd.InsertDprintTrackingTasks(dprintJob, WebSession.User.DisplayName, nextTaskId, equipmentName);
                                                jd.Notifications('I', "#gui after Adding new task for Composer Id " + dprintJob.ComposerId.ToString() + "and result is" + tempId.ToString());

                                                int nextTask = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                                                jd.Notifications('I', "#gui before setting  job processing status the next task is " + nextTask.ToString());
                                                jd.setJobProcessingStatus(dprintJob.Id, nextTask);
                                                jd.Notifications('I', "#gui Set job processing status for print job Id " + dprintJob.Id.ToString() + " and Composer Id " + dprintJob.ComposerId.ToString());
                                                newColorItem.rowPosition = dprintJob.ComposerId;
                                                newColorItem.colourType = Color.Green;
                                            }
                                            else
                                            {
                                                newColorItem.rowPosition = dprintJob.ComposerId;
                                                newColorItem.colourType = Color.Red;
                                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "<script>DisplayErrorMsg('" + "-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - xcopy command is not added to queue for to Drop of location folder. Please check the MSMQ configuration." + "');</script>", false);
                                                msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - xcopy command is not added to queue for to Drop of location folder. Please check the MSMQ configuration.");
                                                string emailTitel = "Spool File could not be sent to Printer";
                                                string emailBody = "Operator " + WebSession.User.DisplayName + " attempted to move Spool File <Spool File Name> to Printer<composer.dbo.printer_name> and operation failed. Please resolve issue.";

                                                jd.Notifications('E', "#gui - xcopy command is not added to queue for to Drop of location folder. Please check the MSMQ configuration");
                                                laJobprocessing.ForeColor = Color.Red;
                                                sendEmail(emailTitel, emailBody);
                                            }
                                        }

                                        else if (printerTargets[0].TransferMethod.Trim().ToLower().Contains("lpr"))
                                        {
                                            if (commandQueue(spoolFileForMatch.FullName, printerTargets[0].DropOffLocation, CommandType, printerTargets[0].TransferMethod, printerTargets[0].NetworkAddress, printerTargets[0].MatchKeyValue, printerTargets[0].TransferBinary, dprintJob, equipmentName, string.Empty, MatchKeyType, AutomationString, printerTargets[0].PrinterTargetsNgID, NextPrinterTargetsNgID))
                                            {
                                                msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Spool file is sent to LPR queue.");
                                                newColorItem.rowPosition = dprintJob.ComposerId;
                                                newColorItem.colourType = Color.Green;

                                                foreach (FileInfo sf in spoolFile)
                                                {

                                                    if (commandQueue(sf.FullName, PrintingInProcessFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, dprintJob, equipmentName))
                                                    {
                                                        msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - move command is sent to command queue to move spool file to ProcessFolder.");
                                                        jd.Notifications('I', "#gui - command is sent to queue to move Spool file - ");
                                                    }
                                                    else
                                                    {
                                                        msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - move command is not added to queue for to PrintingInProcessFolder folder. Please check the MSMQ configuration.");
                                                        jd.Notifications('E', "#gui - move command is not added to queue for to PrintingInProcessFolder folder. Please check the MSMQ configuration");
                                                    }
                                                }

                                                //Completeing a Task
                                                int tid = jd.GetDprintTrackingTasksID(dprintJob.Id, (int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")));
                                                int errorCode = jd.UpdateDprintTrackingTasks(dprintJob.Id, tid, true, equipmentName, WebSession.User.DisplayName);

                                                //Adding new task
                                                int nextTaskId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                                                var tempId = jd.InsertDprintTrackingTasks(dprintJob, WebSession.User.DisplayName, nextTaskId, equipmentName);

                                                int nextTask = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                                                jd.setJobProcessingStatus(dprintJob.Id, nextTask);
                                            }
                                            else
                                            {
                                                newColorItem.rowPosition = dprintJob.ComposerId;
                                                newColorItem.colourType = Color.Red;
                                                msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - LPR command is not added to MSMQ. Please check the MSMQ configuration");
                                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "<script>DisplayErrorMsg('" + "-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - LPR command is not added to MSMQ. Please check the MSMQ configuration" + "');</script>", false);
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    msg.Append("printersNgID is not found for selected equipment " + equipmentName);
                                }
                            }

                            if (fileMatchingCount == 0)
                            {

                                newColorItem.rowPosition = dprintJob.ComposerId;
                                newColorItem.colourType = Color.Red;
                                string emailTitel = "[W11.2] PrinterTargetsNg record could not be found in " + dprintJob.PrintLocation;
                                string emailBody = "While attempting to move a Job to a Printer, a required record in the PrinterTargetsNg table could not belocated for";
                                emailBody += Environment.NewLine + " Job = " + dprintJob.Job.ToString() + " Product = " + dprintJob.Product + " Split = " + dprintJob.Split.ToString() + " PrinterNgID = " + printersNgID.ToString() + " and MatchKeyValue = " + allSpoolFileName;
                                sendEmail(emailTitel, emailBody);
                                // msg.AppendLine(" - PrinterTargetsNg record could not be found.");
                                jd.Notifications('I', "# 9 gui - PrinterTargetsNg record could not be found in " + dprintJob.PrintLocation);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "<script>DisplayErrorMsg('" + HttpUtility.JavaScriptStringEncode("A spool file is available for this Split in the Printer Staging folder however it does not match the format needed for the Printer showing in the Equipment list.") + "');</script>", false);
                                //28042021//msg.AppendLine("A spool file is available for this Split in the Printer Staging folder however it does not match the format needed for the Printer showing in the Equipment list.");
                                //28042021//jd.Notifications('I', "#gui A spool file is available for this Split in the Printer Staging folder however it does not match the format needed for the Printer showing in the Equipment list.");                                
                                //break;
                                bool isMailFlag = false;
                                string spoolFileName = string.Empty;
                                if (spoolFile.Count() > 0)
                                {
                                    foreach (FileInfo spoolFileForMatch in spoolFile)
                                    {
                                        spoolFileName = spoolFileForMatch.Name;
                                        var printerTargets = jd.GetPrinterTargetsNg(printersNgID, spoolFileName);
                                        if (printerTargets.Count > 0)
                                        {
                                            isMailFlag = true;
                                            AddCommandQueueForEmail(dprintJob, equipmentName, "Next button click->printer target not matched ", spoolFileName);
                                        }
                                    }
                                }

                                if (!isMailFlag)
                                {
                                    AddCommandQueueForEmail(dprintJob, equipmentName, "Next button click->printer target not matched ", spoolFileName);
                                }


                            }

                        }
                        else
                        {
                            string errMessage = string.Empty;
                            bool isMailFlag = false;
                            string spoolFileName = string.Empty;
                            if (spoolFileForMail.Count() > 0)
                            {
                                foreach (var spoolFileForMatch in spoolFileForMail)
                                {
                                    spoolFileName = spoolFileForMatch;
                                    var printerTargets = jd.GetPrinterTargetsNg(printersNgID, spoolFileName);
                                    if (printerTargets.Count > 0)
                                    {
                                        isMailFlag = true;
                                        // AddCommandQueueForEmail(dprintJob, equipmentName, "Event= Update Button click Message = email for file not found for wating to print ", spoolFileName);
                                        errMessage = "Spool file " + spoolFileName + " could not be located in the " + printerStagingFolder.DPVariableValue +
                               " folder or the " + PrintingInProcessFolder.DPVariableValue + " or the " + PrintingCompleteFolder.DPVariableValue + ". Job will need to be restarted";
                                    }
                                }
                            }

                            if (!isMailFlag)
                            {
                                // AddCommandQueueForEmail(dprintJob, equipmentName, "Event= Update Button click Message = email for file not found for wating to print ", spoolFileName);
                                errMessage = "Spool file " + spoolFileName + " could not be located in the " + printerStagingFolder.DPVariableValue +
                               " folder or the " + PrintingInProcessFolder.DPVariableValue + " or the " + PrintingCompleteFolder.DPVariableValue + ". Job will need to be restarted";
                            }



                            jd.Notifications('I', errMessage);
                            newColorItem.rowPosition = dprintJob.ComposerId;
                            newColorItem.colourType = Color.Red;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "<script>DisplayErrorMsg('" + HttpUtility.JavaScriptStringEncode(errMessage) + "');</script>", false);

                        }
                    }

                }
                else
                {
                    //ctlTimer.Enabled = false;

                    ctlTimer.Interval = 1000 * 60 * 5;
                    callFromRightarrow = true;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                    modalWarningMsg1.Show();
                    // Move following code to Yes button

                    ////Completeing a Task
                    //int tid = jd.GetDprintTrackingTasksID(DpSelectedJobSession[i].Id, (int)Enum.Parse(typeof(DPrintTaskEnum), DpSelectedJobSession[i].Status.Replace(" ", "")));
                    //int errorCode = jd.UpdateDprintTrackingTasks(DpSelectedJobSession[i].Id, tid, true, equipmentName, WebSession.User.DisplayName);

                    ////Adding new task
                    //int nextTaskId = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                    //var tempId = jd.InsertDprintTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, nextTaskId, equipmentName);

                    //int nextTask = jd.GetNextTaskId(DpSelectedJobSession[i].Status, DpSelectedJobSession[i].WorkflowProcessID);
                    //jd.setJobProcessingStatus(DpSelectedJobSession[i].Id, nextTask, Convert.ToString(DpSelectedJobSession[i].ScheduledMailDate));
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                modalWarningMsg.Show();
            }

            //  }
            //sellectedIndedSession.Clear();
            //equipmentNameSession = string.Empty;

            rowColorSession = newColorItem;
            ExecuteSearch_Click(null, null);
            msg.Clear();
        }

        protected void ctlTimer_Tick(object sender, EventArgs e)
        {
            txtInputPaperModulesJob.Text = string.Empty;
            txtTonerTypeJob.Text = string.Empty;
            txtPrintEngineNgOrSheetCodeJob.Text = string.Empty;

            labInputPaperModulesJob.Text = string.Empty;
            labTonerTypeJob.Text = string.Empty;
            labPrintEngineNgOrSheetCodeJob.Text = string.Empty;

            JobProcessingSearchBy searchData = new JobProcessingSearchBy();
            if (backFromVirtualInserter == "Y")
            {
                searchData = DpSearchByDataSession;
                backFromVirtualInserter = string.Empty;
            }
            else
            {
                searchData = LoadSearchCriteria();
                DpSearchByDataSession = searchData;
            }

            int siteID = 0;
            try
            {
                //var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
                //siteID = Convert.ToInt32(ddLocation.SelectedValue);
                siteID = int.Parse(Session["Location"].ToString());
            }
            catch (Exception ex)
            {
                siteID = Convert.ToInt32(ConfigurationManager.AppSettings["PrintSiteId"]);
            }

            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            var jobList = jobProcessingDataAdapter.GetJobProcessing(searchData, siteID);
            DpJobsSession = jobList;

            txtTotalSheets.Text = string.Format("{0:n0}", jobList.Sum(x => x.TotalSheetCount));
            txtTotalSplitQty.Text = string.Format("{0:n0}", jobList.Sum(x => x.Quantity));
            txtNumberOfSplits.Text = string.Format("{0:n0}", jobList.Count);

            // gvDpJobs.Columns[13].Visible = false;
            var configurationDetails = GetDPSysConfigBySiteId("EnableOneClickNext", siteID);
            if (Convert.ToInt32(configurationDetails.DPVariableValue) == 1)
                gvDpJobs.Columns[13].Visible = true;
            else
                gvDpJobs.Columns[13].Visible = false;

            //check dpjobssession length
            if (jobList.Count == 0)
            {
                Type cstype = this.GetType();

                // Get a ClientScriptManager reference from the Page class.
                ClientScriptManager cs = Page.ClientScript;

                // Check to see if the startup script is already registered.
                if (!cs.IsStartupScriptRegistered(cstype, "PopupScript"))
                {
                    String cstext = "alert('No Record Found');";
                    cs.RegisterStartupScript(cstype, "PopupScript", cstext, true);
                }
            }

            if (rowColorSession != null)
            {
                if (rowColorSession.rowPosition > -1)
                {
                    rowColorSession.colourType = Color.White;
                }
            }

            gvDpJobs.DataSource = DpJobsSession;

            gvDpJobs.DataBind();

            if (rowColorSession != null)
            {
                if (rowColorSession.rowPosition > -1)
                {
                    rowColorSession.rowPosition = -1;
                }
            }
        }

        protected void btnChangePriority_Click(object sender, EventArgs e)
        {
            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);

            //int jobId = DpJobsSession[selectedIndex].Job;

            CustomerNameText1.Text = dprintJob.ClientName;

            int id = dprintJob.Id;


            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

            var details = jobProcessingDataAdapter.getAllSplitsJob(dprintJob.Job, siteID, dprintJob.PrintSiteId, dprintJob.StatusId, dprintJob.IsComplete).ToList();

            int i = 0;
            int selectedIndexInPopUp = i;
            foreach (var jobProcessing in details)
            {
                if (jobProcessing.Id == id)
                {
                    selectedIndexInPopUp = i;
                    break;
                }
                i++;
            }
            gvChangePriority.DataSource = details;
            DpSelectedJobSession = details;
            gvChangePriority.DataBind();
            gvChangePriority.Rows[selectedIndexInPopUp].ForeColor = Color.Green;
            ((CheckBox)gvChangePriority.Rows[selectedIndexInPopUp].FindControl("cbDataSelected")).Checked = true;
            loadPriority();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            modalChangePriority.Show();
        }

        protected void btnDismissPriority_Click(object sender, EventArgs e)
        {
            //ctlTimer.Enabled = true;
            ctlTimer.Interval = 30000;
            laPriority.Text = string.Empty;
            //if (rowColorSession != null)
            //{
            //    if (rowColorSession.rowPosition > -1)
            //    {
            //        // gvDpJobs.Rows[rowColorSession.rowPosition].Cells[11].BackColor = Color.White;
            //        // rowColorSession.rowPosition = -1;
            //        rowColorSession.colourType = Color.White;
            //    }
            //}
            ExecuteSearch_Click(null, null);
        }

        private void loadPriority()
        {
            Dictionary<string, string> priorityKey = new Dictionary<string, string>();
            priorityKey.Add("Very Low - 5", "5");
            priorityKey.Add("Low - 10", "10");
            priorityKey.Add("Normal - 20", "20");
            priorityKey.Add("High - 30", "30");
            priorityKey.Add("Very High - 35", "35");
            priorityKey.Add("Critical - 40", "40");
            ddPriority.DataSource = priorityKey;
            ddPriority.DataTextField = "Key";
            ddPriority.DataValueField = "Value";
            ddPriority.DataBind();
        }

        protected void btnUpdatePriority_Click(object sender, EventArgs e)
        {
            int index = 0;
            string msg = string.Empty;

            List<int> selectedIndex = new List<int>();
            int priority = Convert.ToInt32(ddPriority.SelectedValue.ToString());
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();

            foreach (GridViewRow row in gvChangePriority.Rows)
            {
                if (((CheckBox)row.FindControl("cbDataSelected")).Checked)
                {
                    selectedIndex.Add(index);
                }
                index++;
            }

            foreach (int i in selectedIndex)
            {
                if (jd.changePriority(DpSelectedJobSession[i].Id, priority))
                {
                    var tempId = jd.InsertChangePriorityTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, 26, string.Empty);
                    msg = "Operation Completed Successfully";
                    jd.Notifications('I', "# gui - User " + WebSession.User.DisplayName + " changed priority from " + Convert.ToString(DpSelectedJobSession[i].Priority) + " to " + Convert.ToString(priority) + " for " + Convert.ToString(DpSelectedJobSession[i].Job) + " Product " + Convert.ToString(DpSelectedJobSession[i].Product) + " and Split " + Convert.ToString(DpSelectedJobSession[i].Split));
                }
                else
                {
                    jd.Notifications('E', "# gui - User " + WebSession.User.DisplayName + " changed priority from " + Convert.ToString(DpSelectedJobSession[i].Priority) + " to " + Convert.ToString(priority) + " for " + Convert.ToString(DpSelectedJobSession[i].Job) + " Product " + Convert.ToString(DpSelectedJobSession[i].Product) + " and Split " + Convert.ToString(DpSelectedJobSession[i].Split));
                    msg = "Operation Failed";
                }
            }
            laPriority.Text = msg;
            btnChangePriority_Click(null, null);
        }

        protected void btnChangeJobDates_Click(object sender, EventArgs e)
        {
            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);

            // int jobId = DpJobsSession[selectedIndex].Job;
            CustomerNameText2.Text = dprintJob.ClientName;

            int id = dprintJob.Id;

            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

            var details = jobProcessingDataAdapter.getAllSplitsJob(dprintJob.Job, siteID, dprintJob.PrintSiteId, dprintJob.StatusId, dprintJob.IsComplete).ToList();

            int i = 0;
            int selectedIndexInPopUp = i;
            foreach (var jobProcessing in details)
            {
                if (jobProcessing.Id == id)
                {
                    selectedIndexInPopUp = i;
                    break;
                }
                i++;
            }
            gvChangeJobMailDate.DataSource = details;
            DpSelectedJobSession = details;
            gvChangeJobMailDate.DataBind();
            gvChangeJobMailDate.Rows[selectedIndexInPopUp].ForeColor = Color.Green;
            ((CheckBox)gvChangeJobMailDate.Rows[selectedIndexInPopUp].FindControl("cbDataSelected")).Checked = true;

            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            modalChangeJobMailDate.Show();
            // laCompletedDateOnChange.Text = string.Empty;
        }

        protected void btnDismissCompletedOnDate_Click(object sender, EventArgs e)
        {
            laCompletedDateOnChange.Text = string.Empty;
            //ctlTimer.Enabled = true;
            ctlTimer.Interval = 30000;
            ExecuteSearch_Click(null, null);
        }

        protected void btnUpdateCompletedOnDate_Click(object sender, EventArgs e)
        {
            int index = 0;
            bool isCompleteFlag = false;
            List<int> selectedIndex = new List<int>();

            // JobProcessingDataAdapter jd = new JobProcessingDataAdapter();

            foreach (GridViewRow row in gvChangeJobMailDate.Rows)
            {
                if (((CheckBox)row.FindControl("cbDataSelected")).Checked)
                {
                    selectedIndex.Add(index);
                }
                index++;
            }

            foreach (int i in selectedIndex)
            {
                if (DpSelectedJobSession[i].IsComplete != 1)
                {
                    isCompleteFlag = false;
                    break;
                }
                else
                    isCompleteFlag = true;
            }

            if (isCompleteFlag == true)
            {
                sellectedIndedSession = selectedIndex;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                modalWarningCompletedDateYes.Show();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                modalWarningCompletedDateCancel.Show();
            }

        }

        protected void btnWarningCompletedDateCancel_Click(object sender, EventArgs e)
        {
            btnChangeJobDates_Click(null, null);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            List<int> selectedIndex = new List<int>();
            StringBuilder msg = new StringBuilder();
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();

            if (sellectedIndedSession != null)
                selectedIndex = sellectedIndedSession;
            foreach (int i in selectedIndex)
            {
                if (jd.changeCompletedOnDateChange(DpSelectedJobSession[i].Id, Convert.ToDateTime(DpSelectedJobSession[i].ScheduledMailDate), Convert.ToDateTime(txtCompletedOnDate.Text)))
                {
                    var tempId = jd.InsertChangePriorityTrackingTasks(DpSelectedJobSession[i], WebSession.User.DisplayName, 27, string.Empty);
                    msg.AppendLine("Updating Completed On Date to " + Convert.ToString(txtCompletedOnDate.Text));
                    msg.Append("<br />");
                    msg.AppendLine("Sending Email to Management");
                    msg.Append("<br />");
                    string emailTitel = "[W11.4] Completed On Mail Date Changed in " + DpSelectedJobSession[i].PrintLocation;
                    string emailBody = Environment.NewLine + "User " + WebSession.User.DisplayName + " changed the Completed On Mail Date from " + Convert.ToString(DpSelectedJobSession[i].CompletedOn) + " to " + Convert.ToString(txtCompletedOnDate.Text) + " for Client " + Convert.ToString(DpSelectedJobSession[i].ClientName) + " for job " + DpSelectedJobSession[i].Job.ToString() + " Product " + DpSelectedJobSession[i].Product + " Split " + DpSelectedJobSession[i].Split.ToString();
                    sendEmail(emailTitel, emailBody);
                    msg.Append("Operation Completed Successfully");
                    jd.Notifications('I', "# gui - User " + WebSession.User.DisplayName + " changed completed on date from " + Convert.ToString(DpSelectedJobSession[i].CompletedOn) + " to " + Convert.ToString(txtCompletedOnDate.Text) + " for " + Convert.ToString(DpSelectedJobSession[i].Job) + " Product " + Convert.ToString(DpSelectedJobSession[i].Product) + " and Split " + Convert.ToString(DpSelectedJobSession[i].Split));
                }
                else
                {
                    jd.Notifications('E', "# gui - User " + WebSession.User.DisplayName + " changed completed on date from " + Convert.ToString(DpSelectedJobSession[i].CompletedOn) + " to " + Convert.ToString(txtCompletedOnDate.Text) + " for " + Convert.ToString(DpSelectedJobSession[i].Job) + " Product " + Convert.ToString(DpSelectedJobSession[i].Product) + " and Split " + Convert.ToString(DpSelectedJobSession[i].Split));
                    msg.AppendLine("Operation Failed");
                }
            }
            laCompletedDateOnChange.Text = msg.ToString();
            msg.Clear();
            if (sellectedIndedSession != null)
                sellectedIndedSession.Clear();
            btnChangeJobDates_Click(null, null);
        }

        protected void btnWarningCompletedDateYesCancel_Click(object sender, EventArgs e)
        {

        }

        protected void txtCompletedOnDate_TextChanged(object sender, EventArgs e)
        {
            //laCompletedDateOnChange.Text = "Updating Completed On Date to " + Convert.ToString(txtCompletedOnDate.Text);
            //btnChangeJobDates_Click(null, null);
        }

        protected void btnApplyYes_Click(object sender, EventArgs e)
        {
            checkAndApplyStatus();
        }

        protected void btnApplyNo_Click(object sender, EventArgs e)
        {

        }

        protected void btnExpContinue_Click(object sender, EventArgs e)
        {
            WriteTraceLog("btnExpContinue_Click");
            checkAndApplyStatus();
        }

        protected void PrintFT_icon_Click(object sender, ImageClickEventArgs e)
        {

            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

            var dpPrintDataAdapter = new DpPrintStatusDataAdapter();
            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);
            var details = dpPrintDataAdapter.GetDpJobHistories(dprintJob.Id, siteID).ToList();
            byte[] output = jd.PrintFSSTicket(details, dprintJob.Id, dprintJob.Job, dprintJob.Product, dprintJob.Split, dprintJob.startSequence, dprintJob.Sequences, dprintJob.ComposerId, siteID, "");
            PdfData = output;
            string url = "FTpdf.aspx";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "newpage", "customOpen('" + url + "');", true);

            BtnViewJobHistory_Clicked(null, null);
        }

        protected void displayFinalSplitTicketSumary(JobProcessingModel item, bool print = false, string source = "")
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();

            try
            {
                var dpPrintDataAdapter = new DpPrintStatusDataAdapter();
                var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
                int siteID = Convert.ToInt32(ddLocation.SelectedValue);
                var details = dpPrintDataAdapter.GetDpJobHistories(item.Id, siteID).ToList();
                byte[] output = jd.PrintFSSTicket(details, item.Id, item.Job, item.Product, item.Split, item.startSequence, item.Sequences, item.ComposerId, siteID, txtReprintTicketWatermark.Text);
                PdfData = output;
                string url = "FTpdf.aspx";
                if (print == true)
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "FinalSummaryTicket.pdf";

                    File.WriteAllBytes(Path.Combine(source, fileName), PdfData);
                    PDFUtil.SendPDFToPrinter(Path.Combine(source, fileName), ddReprintTicketPrinters.Text, false, true);
                    if (File.Exists(Path.Combine(source, fileName)))
                        File.Delete(Path.Combine(source, fileName));
                    printerMsg.Text = ddReprintTicketType.Text + " is sent to the printer";
                    btnReprintTicket_Click(null, null);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "newpage", "customOpen('" + url + "');", true);

            }
            catch (Exception exc)
            {
                jd.Notifications('E', "#gui - Reprint Ticket Feature - error while final split ticket for " + exc.InnerException + "|" + exc.Message + "|" + exc.Source + "|" + exc.ToString());
            }
        }

        protected void btnJobRouting_Click(object sender, EventArgs e)
        {
            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);

            int jobId = dprintJob.Job;
            litCustomerName.Text = dprintJob.ClientName;

            int id = dprintJob.Id;


            LoadAltActiveSites(dprintJob.PrintSiteId);
            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

            var details = jobProcessingDataAdapter.getAllSplitsJob(dprintJob.Job, siteID, dprintJob.PrintSiteId, dprintJob.StatusId, dprintJob.IsComplete).ToList();

            int i = 0;
            int selectedIndexInPopUp = i;
            foreach (var jobProcessing in details)
            {
                if (jobProcessing.Id == id)
                {
                    selectedIndexInPopUp = i;
                    break;
                }
                i++;
            }
            gvJobRouting.DataSource = details;
            DpSelectedJobSession = details;
            gvJobRouting.DataBind();
            gvJobRouting.Rows[selectedIndexInPopUp].ForeColor = Color.Green;
            ((CheckBox)gvJobRouting.Rows[selectedIndexInPopUp].FindControl("cbDataSelected")).Checked = true;

            if (Convert.ToString(jobProcessingDataAdapter.SystemConfiguration(Convert.ToInt32(ddDpRoutingSites.SelectedValue), "MegaMRDF")) == "1")
                cbMegaMrdf.Checked = true;
            else
                cbMegaMrdf.Checked = false;

            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            modalJobRouting.Show();
        }

        protected void btnUpdateJobs_Click(object sender, EventArgs e)
        {
            var index = 0;
            List<int> selectedIndex = new List<int>();
            List<JobProcessingModel> jobList = new List<JobProcessingModel>();

            foreach (GridViewRow row in gvJobRouting.Rows)
            {
                if (((CheckBox)row.FindControl("cbDataSelected")).Checked)
                {
                    selectedIndex.Add(index);
                    jobList.Add(DpSelectedJobSession[index]);
                }
                index++;
            }
            DpSelectedJobSession.Clear();
            DpSelectedJobSession = jobList;

            if (jobList.Any(x => x.StatusId == 3) || jobList.Any(x => x.IsComplete > 0) || jobList.Any(x => x.StatusId > 4) && jobList.Any(x => x.StatusId < 15))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                modalWarningCancelJobRouting.Show();
            }
            else
            {
                labTotalSplitsPrinted.Text = string.Format("{0:n0}", jobList.Where(x => x.StatusId > 15).Sum(x => x.Quantity));
                labTotalSplitsMoving.Text = string.Format("{0:n0}", jobList.Count);
                labTotalVolume.Text = string.Format("{0:n0}", jobList.Sum(x => x.Quantity));
                labTotalSheetsCount.Text = string.Format("{0:n0}", jobList.Sum(x => x.TotalSheetCount));
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                modalWarningJobRouting.Show();
            }

        }

        protected void btnDismissJobRouting_Click(object sender, EventArgs e)
        {
            ctlTimer.Interval = 30000;
            ExecuteSearch_Click(null, null);
        }

        protected void btnJobRoutingCancel_Click(object sender, EventArgs e)
        {
            ctlTimer.Interval = 30000;
        }

        protected void btnConfirmJobRouting_Click(object sender, EventArgs e)
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

            StringBuilder splitString = new StringBuilder();
            string emailSubject = string.Empty;
            string emailBody = string.Empty;
            if (DpSelectedJobSession != null)
            {
                foreach (var rec in DpSelectedJobSession)
                {
                    splitString.Append(Convert.ToString(rec.Split));
                    splitString.Append(",");
                }

                emailBody = Convert.ToString(DpSelectedJobSession.FirstOrDefault().Job) + ", Product " + Convert.ToString(DpSelectedJobSession.FirstOrDefault().Product) + " and Split(s) " + Convert.ToString(splitString).TrimEnd(',') + " from " + Convert.ToString(DpSelectedJobSession.FirstOrDefault().PrintLocation) + " to " + ddDpRoutingSites.SelectedItem.Text;
                labMsgJobRouting.Text = "Preparing to move job " + emailBody + "<br>";
                labMsgJobRouting.Text += "Sending Email to Management" + "<br>";

                emailSubject = "Job is moving from " + Convert.ToString(DpSelectedJobSession.FirstOrDefault().PrintLocation) + " to " + ddDpRoutingSites.SelectedItem.Text;
                emailBody = "User " + WebSession.User.DisplayName + " has selected the following Job / Splits to be moved. Client " + DpSelectedJobSession.FirstOrDefault().ClientName + ", Job " + emailBody + "." + "<br>";
                jd.Notifications('I', "# gui -" + emailBody);
                splitString.Clear();
                splitString.Append(emailBody + "<br>");

                //splitString.Append("<table border=\"1\" style=\"border - collapse:collapse; width: 200px; font - family:Tahoma; font - size:10pt; \"><tr><td>Total Splits moving</td><td>"  + labTotalSplitsMoving.Text + "</td></tr><tr><td>Total Volume</td><td>"  + labTotalVolume.Text + "</td></tr><tr><td>Total Sheets</td><td>"  + labTotalSheetsCount.Text + "</td></tr></table>");
                splitString.Append("<table border=\"1\" style=\"border - collapse:collapse; width: 984px; font - family:Tahoma; font - size:10pt; \"><tr><th>Total Splits moving</th><th>Total Volume</th><th>Total Sheets</th></tr><tr><td align=\"center\"><span style=\"color:red;\" >" + labTotalSplitsMoving.Text + "</span></td><td align =\"center\"><span style = \"color:red;\">" + labTotalVolume.Text + "</span></td><td align =\"center\"><span style =\"color:red;\">" + labTotalSheetsCount.Text + "</span></td></tr></table>");
                sendEmail(emailSubject, Convert.ToString(splitString), true);

                if (cbMegaMrdf.Checked)
                {
                    try
                    {
                        if (Convert.ToString(jd.SystemConfiguration(Convert.ToInt32(ddDpRoutingSites.SelectedValue), "MegaMRDF")) == "1")
                        {
                            foreach (var rec in DpSelectedJobSession)
                            {
                                jd.SetInFlightJob(rec, Convert.ToInt32(ddDpRoutingSites.SelectedValue), WebSession.User.DisplayName, 38);
                                jd.InsertInFlightDprintTrackingTasks(rec, WebSession.User.DisplayName, 38, ddDpRoutingSites.SelectedItem.Text);

                                dprintJob.StatusId = 2;
                                dprintJob.PrintSiteId = Convert.ToInt32(ddDpRoutingSites.SelectedValue);
                                labMsgJobRouting.Text += "Adding Split " + Convert.ToString(rec.Split) + " to the Container Transfer Queue including mega MRDF" + "<br>";
                            }
                        }
                        else
                        {
                            jd.Notifications('E', "MegaMRDF Configuration flag entry is not found in in System.DPSysConfig for selected site " + ddDpRoutingSites.SelectedItem.Text);
                            labMsgJobRouting.Text = "Error - 'Include Summary Inserter Control File?' is checked but MegaMRDF Configuration flag entry is not found in System.DPSysConfig for selected site " + ddDpRoutingSites.SelectedItem.Text + "<br>";
                        }
                    }
                    catch (Exception ex)
                    {
                        jd.Notifications('E', "Error while reading MegraMRDF Configuration.Please check the MegaMRDF flag in System.DPSysConfig");
                        labMsgJobRouting.Text = "Error while reading MegraMRDF Configuration.Please check the MegaMRDF flag in System.DPSysConfig for destination site Id" + "<br>";
                    }

                }
                else
                {
                    foreach (var rec in DpSelectedJobSession)
                    {
                        if (rec.StatusId >= 14)
                        {
                            jd.SetInFlightJob(rec, Convert.ToInt32(ddDpRoutingSites.SelectedValue), WebSession.User.DisplayName, 28);
                            jd.InsertInFlightDprintTrackingTasks(rec, WebSession.User.DisplayName, 28, ddDpRoutingSites.SelectedItem.Text);
                        }
                        else
                        {
                            jd.SetInFlightJob(rec, Convert.ToInt32(ddDpRoutingSites.SelectedValue), WebSession.User.DisplayName, 29);
                            jd.InsertInFlightDprintTrackingTasks(rec, WebSession.User.DisplayName, 29, ddDpRoutingSites.SelectedItem.Text);
                        }
                        dprintJob.StatusId = 2;
                        dprintJob.PrintSiteId = Convert.ToInt32(ddDpRoutingSites.SelectedValue);
                        labMsgJobRouting.Text += "Adding Split " + Convert.ToString(rec.Split) + " to the Container Transfer Queue" + "<br>";
                    }
                }

            }
            btnJobRouting_Click(null, null);
        }

        protected void btnWarningJobRouting_Click(object sender, EventArgs e)
        {
            ctlTimer.Interval = 30000;
        }

        protected void InFlightQueue_icon_Click(object sender, ImageClickEventArgs e)
        {
            equipmentVertual = ddEquipmentId.SelectedValue.ToString();
            Response.Redirect("~/app/TransferQueue.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnOpenVirtualInserter_Click(object sender, EventArgs e)
        {
            if (ddEquipmentId.SelectedValue.ToString() == "N/A")
            {
                SessionforVertualInserter();
                if (ddEquipmentList.SelectedItem != null && ddEquipmentList.SelectedItem.Text != "")
                {
                    equipmentVertual = ddEquipmentList.SelectedItem.Text;
                }
                else
                {
                    equipmentVertual = ddEquipmentId.SelectedItem.Text;
                }

            }
            else
            {
                SessionforVertualInserter();
                equipmentVertual = ddEquipmentId.SelectedItem.Text;
            }
            SetSessionForHeaderControls();

            Response.Redirect("~/app/VirtualInserter.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void SessionforVertualInserter()
        {
            UserDeviceToEquipment userDeviceToEquipment = new UserDeviceToEquipment();
            userDeviceToEquipment.SiteID = int.Parse(ddProcessingLocation.SelectedValue);
            userDeviceToEquipment.DPrintTaskMasterID = int.Parse(ddStatus.SelectedValue);
            userDeviceToEquipment.Equipment = ddEquipmentId.SelectedItem.Text.ToString();
            Session["virtualInserter"] = userDeviceToEquipment;
        }
        private void SetSessionForHeaderControls()
        {
            JobSizeThreshold = txtJobSizeThreshold.Text;
            LargeJobs = chkLargeJobs.Checked;
            SmallJobs = chkSmalljobs.Checked;
            ProductionJobs = cbProductionJobs.Checked;
            TestJobs = cbTestJobs.Checked;
            CompletedJobsOnly = cbIncludeCompletedJobs.Checked;
            DontFilterbyJobAttributeJobs = cbFilterByAttributes.Checked;
            ScheduledBeginningDate = bdate.Text;
            ScheduledEndingDate = edate.Text;
        }
        private void SetHeaderControlsFromSession()
        {
            txtJobSizeThreshold.Text = JobSizeThreshold;
            chkLargeJobs.Checked = LargeJobs;
            chkSmalljobs.Checked = SmallJobs;
            cbProductionJobs.Checked = ProductionJobs;
            cbTestJobs.Checked = TestJobs;
            cbIncludeCompletedJobs.Checked = CompletedJobsOnly;
            cbFilterByAttributes.Checked = DontFilterbyJobAttributeJobs;
            bdate.Text = ScheduledBeginningDate;
            edate.Text = ScheduledEndingDate;
        }

        protected void cbIncludeCompletedJobs_CheckedChanged(object sender, EventArgs e)
        {

            if (cbIncludeCompletedJobs.Checked == true)
            {
                bdate.Text = "Completed On Beginning Date:";
                edate.Text = "Completed On Ending Date:";
            }
            else
            {
                bdate.Text = "Scheduled Beginning Date:";
                edate.Text = "Scheduled Ending Date:";
            }
        }

        private void checkForQtyPullsAfterPrint(string inserterId, string dpJobId, int flag = 0, bool isRedirect = true)
        {
            var jd = new JobProcessingDataAdapter();
            int qty = 0;
            virtualInserter = jd.CheckForVertualInserterCategoryByInserterID(inserterId);
            try
            {
                if (!string.IsNullOrEmpty(jd.GetQtyPullsAfterPrintByDpJobId(dpJobId)))
                    qty = Convert.ToInt32(jd.GetQtyPullsAfterPrintByDpJobId(dpJobId));
            }
            catch (Exception ex)
            {
                jd.Notifications('E', "# gui Unable to determin Qty pulls after print for dp job id -" + dpJobId + "-" + ex.Message);
            }

            if (qty > 0)
            {
                var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
                int siteID = Convert.ToInt32(ddLocation.SelectedValue);
                labPrinter.Text = jd.GetSysConfig(siteID, "JobInsertionPrinter").DPVariableValue;
                ctlTimer.Interval = 1000 * 60 * 5;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                modalOkAAR.Show();

            }
            else
            {
                if (virtualInserter)
                {

                    SessionforVertualInserter();
                    equipmentVertual = ddEquipmentId.SelectedItem.Text;
                    if (flag == 1)
                        btnOpenVirtualInserter_Click(null, null);
                    else
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["VirtualInserterFromSubMenu"])))
                        {
                            equipmentVertual = (Convert.ToString(Session["VirtualInserterFromSubMenu"]));
                            Session["VirtualInserterFromSubMenu"] = string.Empty;
                        }
                        if (isRedirect)
                        {
                            Response.Redirect("~/app/VirtualInserter.aspx", true);
                            //Server.Transfer("~/app/VirtualInserter.aspx", true);
                            Context.ApplicationInstance.CompleteRequest();
                        }

                    }
                }
            }
        }

        protected void btnOkAAR_Click(object sender, EventArgs e)
        {
            if (virtualInserter)
            {
                ctlTimer.Interval = 30000;

                SessionforVertualInserter();
                //equipmentVertual = ddEquipmentId.SelectedItem.Text;
                //Response.Redirect("~/app/VirtualInserter.aspx", true);
                //Response.Redirect("~/app/VirtualInserter.aspx", false);
                btnOpenVirtualInserter_Click(null, null);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                ctlTimer.Interval = 30000;
                if (rowColorSession != null)
                {
                    if (rowColorSession.rowPosition > -1)
                    {
                        // gvDpJobs.Rows[rowColorSession.rowPosition].Cells[11].BackColor = Color.White;
                        // rowColorSession.rowPosition = -1;
                        rowColorSession.colourType = Color.White;
                    }
                }
                ExecuteSearch_Click(null, null);
            }
        }

        private void LoadJobAtbWaitingToPrint()
        {

            labTonerTypeJob.Visible = true;
            labPrintEngineNgOrSheetCodeJob.Visible = true;

            //txtInputPaperModulesJob.Enabled = false;
            //txtTonerTypeJob.Enabled = false;
            //txtPrintEngineNgOrSheetCodeJob.Enabled = false;

            labTonerTypeJob.Text = "MICR Flag";
            labPrintEngineNgOrSheetCodeJob.Text = "Color";

            //labPrintEngineNgOrSheetCodeJob.Visible = true;
            //ddEngineJob.Visible = true;

            //ddPaperJob.Enabled = true;
            //ddTonerJob.Enabled = true;
            //ddEngineJob.Enabled = true;

            //Dictionary<string, string> paper = new Dictionary<string, string>();
            //paper.Add(string.Empty, "0");
            //paper.Add("Cut Sheet", "1");
            //paper.Add("Continuous", "2");
            //paper.Add("Fan Fold", "3");
            //paper.Add("Tabber", "4");

            //ddPaperJob.DataSource = paper;
            //ddPaperJob.DataTextField = "Key";
            //ddPaperJob.DataValueField = "Value";
            //ddPaperJob.DataBind();

            //labTonerTypeJob.Text = "Toner/Ink";
            //Dictionary<string, string> toner = new Dictionary<string, string>();
            //toner.Add(string.Empty, "0");
            //toner.Add("MICR", "1");
            //toner.Add("NON-MICR", "2");
            //toner.Add("BOTH", "3");

            //ddTonerJob.DataSource = toner;
            //ddTonerJob.DataTextField = "Key";
            //ddTonerJob.DataValueField = "Value";
            //ddTonerJob.DataBind();
            //ddTonerJob.ToolTip = "BOTH means Job requires both MICR and NON-MICR toner/ink";

            //labPrintEngineNgOrSheetCodeJob.Text = "Engine";
            //Dictionary<string, string> engine = new Dictionary<string, string>();
            //engine.Add(string.Empty, "0");
            //engine.Add("BnW Toner", "1");
            //engine.Add("Color Toner", "2");
            //engine.Add("Color Inkjet", "3");
            //engine.Add("BnW Inkjet", "4");

            //ddEngineJob.DataSource = engine;
            //ddEngineJob.DataTextField = "Key";
            //ddEngineJob.DataValueField = "Value";
            //ddEngineJob.DataBind();
        }

        private void LoadJobAtbWaitingToInsert()
        {
            ddPaperJob.Enabled = true;
            ddTonerJob.Enabled = true;
            ddEngineJob.Enabled = true;

            Dictionary<string, string> paper = new Dictionary<string, string>();
            paper.Add(string.Empty, "0");
            paper.Add("Cut Sheet", "1");
            paper.Add("Continuous", "2");
            paper.Add("Fan Fold", "3");
            paper.Add("Tabber", "4");

            ddPaperJob.DataSource = paper;
            ddPaperJob.DataTextField = "Key";
            ddPaperJob.DataValueField = "Value";
            ddPaperJob.DataBind();

            labTonerTypeJob.Text = "Sheet Code";
            Dictionary<string, string> SheetCode = new Dictionary<string, string>();
            SheetCode.Add(string.Empty, "0");
            SheetCode.Add("Single", "1");
            SheetCode.Add("Fixed", "2");
            SheetCode.Add("Varies", "3");

            ddTonerJob.DataSource = SheetCode;
            ddTonerJob.DataTextField = "Key";
            ddTonerJob.DataValueField = "Value";
            ddTonerJob.DataBind();
            ddTonerJob.ToolTip = string.Empty;

            labPrintEngineNgOrSheetCodeJob.Text = string.Empty;
            labPrintEngineNgOrSheetCodeJob.Visible = false;
            ddEngineJob.Items.Clear();
            ddEngineJob.Visible = false;

            //labPrintEngineNgOrSheetCodeJob.Text = "Category";
            //Dictionary<string, string> category = new Dictionary<string, string>();
            //category.Add(string.Empty, "0");
            //category.Add("High Speed", "1");
            //category.Add("Low Speed", "2");
            //category.Add("Virtual", "3");

            //ddEngineJob.DataSource = category;
            //ddEngineJob.DataTextField = "Key";
            //ddEngineJob.DataValueField = "Value";
            //ddEngineJob.DataBind();
        }

        private void UnloadJobAtbDropwond()
        {
            Dictionary<string, string> emptyList = new Dictionary<string, string>();
            ddPaperJob.DataSource = emptyList;
            ddPaperJob.DataTextField = "Key";
            ddPaperJob.DataValueField = "Value";
            ddPaperJob.DataBind();
            // ddPaperJob.Enabled = false;

            ddTonerJob.DataSource = emptyList;
            ddTonerJob.DataTextField = "Key";
            ddTonerJob.DataValueField = "Value";
            ddTonerJob.DataBind();
            // ddTonerJob.Enabled = false;

            ddEngineJob.DataSource = emptyList;
            ddEngineJob.DataTextField = "Key";
            ddEngineJob.DataValueField = "Value";
            ddEngineJob.DataBind();
            // ddEngineJob.Enabled = false;

            labTonerTypeJob.Text = string.Empty;
            labPrintEngineNgOrSheetCodeJob.Text = string.Empty;
            labEqTonerTypeOrSheetCode.Text = string.Empty;
            labEqPrintEngineOrCategory.Text = string.Empty;
            txtEqInputPaperModules.Text = string.Empty;
            txtEqTonerTypeOrSheetCode.Text = string.Empty;
            txtEqPrintEngineOrCategory.Text = string.Empty;
        }

        private void setEquipmentAtr(string status)
        {
            txtEqInputPaperModules.Text = string.Empty;
            txtEqTonerTypeOrSheetCode.Text = string.Empty;
            txtEqPrintEngineOrCategory.Text = string.Empty;

            labEqInputPaperModules.Text = string.Empty;
            labEqTonerTypeOrSheetCode.Text = string.Empty;
            labEqPrintEngineOrCategory.Text = string.Empty;

            if (status == "15")
            {
                labEqInputPaperModules.Text = "Paper";
                labEqTonerTypeOrSheetCode.Text = "MICR Flag";
                labEqPrintEngineOrCategory.Text = "Engine";
                var printerAtr = getPrinterInformaiton();
                txtEqInputPaperModules.Text = printerAtr.InoutPaperModule;
                txtEqTonerTypeOrSheetCode.Text = printerAtr.TonerType;
                txtEqTonerTypeOrSheetCode.ToolTip = "BOTH means Printer supports both MICR and NON-MICR";
                txtEqPrintEngineOrCategory.Text = printerAtr.PrintEngine;
            }
            else if (status == "17")
            {
                labEqInputPaperModules.Text = "Paper";
                labEqTonerTypeOrSheetCode.Text = "Sheet Code";
                labEqPrintEngineOrCategory.Text = "Category";
                var InserterAtr = getInserterInformation();
                txtEqInputPaperModules.Text = InserterAtr.InputPaperModule;
                txtEqTonerTypeOrSheetCode.Text = InserterAtr.SheetCode;
                txtEqTonerTypeOrSheetCode.ToolTip = "Fixed means all mail pieces have the same number of Sheets. Varies mean the number of sheets varies from mail piece to mail piece.";
                txtEqPrintEngineOrCategory.Text = InserterAtr.InserterCategory;
            }
            if (status == "15" && ddEquipmentId.SelectedValue.ToString() != "N/A")
            {
                cbFilterByAttributes.Enabled = true;
                if (WebSession.User.HasPermissionExplicit("JobProcessing.Operator"))
                {
                    cbFilterByAttributes.Checked = true;
                }
            }
            else
            {
                cbFilterByAttributes.Enabled = false;
                cbFilterByAttributes.Checked = false;
            }
        }

        private PrinterData getPrinterInformaiton()
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            string equipmentName = ddEquipmentId.SelectedValue.ToString();
            PrinterData item = jd.GetPrinterInfo(equipmentName);
            return item;
        }

        private InserterData getInserterInformation()
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddProcessingLocation.SelectedValue);
            string equipmentName = ddEquipmentId.SelectedValue.ToString();
            InserterData item = jd.GetInserterInfo(equipmentName, siteID);
            return item;
        }


        protected void btnLoadAtr_Click(object sender, EventArgs e)
        {
            var selectedStatusId = ddStatus.SelectedValue;
            if (selectedStatusId.Equals("15"))
                LoadJobAtbWaitingToPrint();
            else if (selectedStatusId.Equals("17"))
                LoadJobAtbWaitingToInsert();
            else
                UnloadJobAtbDropwond();
            setEquipmentAtr(selectedStatusId);
        }

        private void LoadJobAttributes()
        {
            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);

            string paper = dprintJob.InputPaperModules;
            string toner = dprintJob.TonerType;
            string printEngineNg = dprintJob.PrintEngineNg;
            string sheetCode = dprintJob.SheetCode;

            if (ddStatus.SelectedValue == "15")
            {
                labInputPaperModulesJob.Text = "Paper";
                labTonerTypeJob.Text = "MICR Flag";
                labPrintEngineNgOrSheetCodeJob.Text = "Color";
                txtInputPaperModulesJob.Text = paper;
                txtTonerTypeJob.Text = toner;
                txtTonerTypeJob.ToolTip = "BOTH means Printer supports both MICR and NON-MICR";
                txtPrintEngineNgOrSheetCodeJob.Text = printEngineNg;
            }
            else if (ddStatus.SelectedValue == "17")
            {
                labInputPaperModulesJob.Text = "Paper";
                labTonerTypeJob.Text = "MICR Flag";
                labPrintEngineNgOrSheetCodeJob.Text = "Sheet Code";
                txtInputPaperModulesJob.Text = paper;
                txtTonerTypeJob.Text = toner;
                txtTonerTypeJob.ToolTip = "Fixed means all mail pieces have the same number of Sheets. Varies mean the number of sheets varies from mail piece to mail piece.";
                txtPrintEngineNgOrSheetCodeJob.Text = sheetCode;
            }
            else
            {
                ddPaperJob.SelectedValue = "0";
                ddTonerJob.SelectedValue = "0";
                ddEngineJob.SelectedValue = "0";
            }

            gvJobInstructions.Columns[1].Visible = true;
            gvJobInstructions.Columns[2].Visible = true;

            List<JobInstructions> items = jobProcessingDataAdapter.GetInstructions(dprintJob.FromSiteID, dprintJob.PrintSiteId, dprintJob.Job, dprintJob.ClientNumber, dprintJob.Product, Convert.ToInt32(ddStatus.SelectedValue), ddEquipmentId.SelectedValue.ToString());
            if (items.Count() == 0)
            {
                JobInstructions item = new JobInstructions();
                items.Add(item);
            }
            gvJobInstructions.DataSource = items;
            gvJobInstructions.DataBind();



            for (int i = 0; i <= gvJobInstructions.Rows.Count - 1; i++)
            {
                if (gvJobInstructions.Rows[i].Cells[1].Text.ToString() == Convert.ToString(dprintJob.FromSiteID) && gvJobInstructions.Rows[i].Cells[2].Text.ToString() == Convert.ToString(dprintJob.Job))
                    gvJobInstructions.Rows[i].Cells[0].ForeColor = Color.Red;
            }
            gvJobInstructions.Columns[1].Visible = false;
            gvJobInstructions.Columns[2].Visible = false;
        }

        protected void btnSaveJobAttributes_Click(object sender, EventArgs e)
        {
            ctlTimer.Interval = 1000 * 60 * 5;
            saveJobAttributes(selectedDprintJobsId);
        }

        protected void saveJobAttributes(int Id)
        {
            JobAttributes item = new JobAttributes();
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();

            item = jd.GetJobAttributes(Id);
            //jobAttributesSession = item;

            if (ddStatus.SelectedValue.Equals("15"))
            {
                if (item.InputPaperModules == ddPaperJob.SelectedItem.Text && item.TonerType == ddTonerJob.SelectedItem.Text && item.PrintEngineNg == ddEngineJob.SelectedItem.Text)
                {
                    // Do nothing
                }
                else
                {
                    if (item.TonerType == "MICR" && ddTonerJob.SelectedItem.Text == "NON-MICR")
                    {
                        // Warning !!!
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                        modalWarningJobPrinterAttributes.Show();
                    }
                    else if (item.TonerType == "MICR" && ddTonerJob.SelectedItem.Text == "BOTH")
                    {
                        // Warning !!!
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                        modalWarningJobPrinterAttributes.Show();
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                        modalCautionJobPrinterAttributes.Show();
                    }
                }

            }
            else if (ddStatus.SelectedValue.Equals("17"))
            {
                if (item.InputPaperModules == ddPaperJob.SelectedItem.Text && item.SheetCode == ddTonerJob.SelectedItem.Text)
                {
                    // Do nothing
                }
                else
                {
                    // Process with Caution
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                    modalCautionJobPrinterAttributes.Show();
                }
            }
            ctlTimer.Interval = 30000;
        }

        protected void btnYesJobAttributes_Click(object sender, EventArgs e)
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

            jd.Notifications('I', "User " + WebSession.User.DisplayName + "has manually changed a Print Job Attribute for Client " + dprintJob.ClientName + "," + Convert.ToString(dprintJob.Job) + ",Product " + dprintJob.Product + " and Split " + Convert.ToString(dprintJob.Split) + " at " + dprintJob.FromLocation + " to " + dprintJob.PrintLocation + ".");

            JobAttributes item = new JobAttributes();
            item = jd.GetJobAttributes(selectedDprintJobsId);
            string equipmentName = string.Empty;

            if (ddStatus.SelectedValue.Equals("15"))
            {
                int taskID = 33;

                if (item.InputPaperModules != ddPaperJob.SelectedItem.Text)
                {
                    equipmentName = item.InputPaperModules + " changed to " + ddPaperJob.SelectedItem.Text;
                    jd.InsertAttributesChangedTrackingTasks(dprintJob, WebSession.User.DisplayName, taskID, equipmentName, "Yellow");
                }
                if (item.TonerType != ddTonerJob.SelectedItem.Text)
                {
                    equipmentName = item.TonerType + " changed to " + ddTonerJob.SelectedItem.Text;
                    jd.InsertAttributesChangedTrackingTasks(dprintJob, WebSession.User.DisplayName, taskID, equipmentName, "Yellow");
                }
                if (item.PrintEngineNg != ddEngineJob.SelectedItem.Text)
                {
                    equipmentName = item.PrintEngineNg + " changed to " + ddEngineJob.SelectedItem.Text;
                    jd.InsertAttributesChangedTrackingTasks(dprintJob, WebSession.User.DisplayName, taskID, equipmentName, "Yellow");
                }

                jd.ChangeJobPrinterAttributes(dprintJob.Id, ddPaperJob.SelectedItem.Text, ddTonerJob.SelectedItem.Text, ddEngineJob.SelectedItem.Text);
            }

            else if (ddStatus.SelectedValue.Equals("17"))
            {
                int taskID = 34;

                if (item.InputPaperModules != ddPaperJob.SelectedItem.Text)
                {
                    equipmentName = item.InputPaperModules + " changed to " + ddPaperJob.SelectedItem.Text;
                    jd.InsertAttributesChangedTrackingTasks(dprintJob, WebSession.User.DisplayName, taskID, equipmentName, "Yellow");
                }
                if (item.SheetCode != ddTonerJob.SelectedItem.Text)
                {
                    equipmentName = item.SheetCode + " changed to " + ddTonerJob.SelectedItem.Text;
                    jd.InsertAttributesChangedTrackingTasks(dprintJob, WebSession.User.DisplayName, taskID, equipmentName, "Yellow");
                }

                jd.ChangeJobInserterAttributes(dprintJob.Id, ddPaperJob.SelectedItem.Text, ddTonerJob.SelectedItem.Text);
            }
            ctlTimer.Interval = 30000;
            ExecuteSearch_Click(null, null);
        }

        protected void btnNoCancelJobAttributes_Click(object sender, EventArgs e)
        {
            ctlTimer.Interval = 30000;
        }

        protected void btnJobPrinterAttributesCancel_Click(object sender, EventArgs e)
        {
            ctlTimer.Interval = 30000;
        }

        private void trackAttributesChange(string befforeAttribute, string afterAttribute)
        {

        }

        private string createEqupmentPrefix()
        {
            PrinterData printerAtr = new PrinterData();
            printerAtr = getPrinterInformaiton();
            List<string> prefixString = new List<string>();
            string prefix = string.Empty;

            switch (printerAtr.InoutPaperModule)
            {
                case "Continuous":
                    prefixString.Add("J");
                    break;
                case "Cut Sheet":
                    prefixString.Add("C");
                    break;
                case "Fan Fold":
                    prefixString.Add("F");
                    break;
                case "Tabber":
                    prefixString.Add("T");
                    break;
            }
            switch (printerAtr.PrintEngine)
            {
                case "Color Inkjet":
                    prefixString.Add("E");
                    break;
                case "Color Toner":
                    prefixString.Add("T");
                    break;
                case "BnW InkJet":
                    prefixString.Add("W");
                    break;
                case "BnW Toner":
                    prefixString.Add("B");
                    break;
            }
            switch (printerAtr.TonerType)
            {
                case "BOTH":
                    prefixString.Add("T");
                    break;
                case "MICR":
                    prefixString.Add("M");
                    break;
                case "NON-MICR":
                    prefixString.Add("X");
                    break;
            }

            prefix = string.Join("", prefixString);
            return prefix;
        }

        private int getCurrentLocationId()
        {
            var dpPrintDataAdapter = new DpPrintStatusDataAdapter();
            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);
            return siteID;
        }

        private int getCurrentSiteId()
        {
            int siteID = -1;
            try
            {
                siteID = Convert.ToInt32(ConfigurationManager.AppSettings["PrintSiteId"]);
            }
            catch (Exception ex)
            {
                siteID = getCurrentLocationId();
                JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
                jd.Notifications('W', "Site id is not configured in machine config file.");
            }
            return siteID;
        }
        protected void btnContinueDuplicate_Click(object sender, EventArgs e)
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();

            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);
            //
            ////Completeing a Task
            int tid = jd.GetDprintTrackingTasksID(dprintJob.Id, (int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")));
            int errorCode = jd.UpdateDprintTrackingTasksForDuplicateEntry(dprintJob.Id, tid, true, WebSession.User.DisplayName, "Continue", dprintJob.Job, dprintJob.ComposerId, dprintJob.Split, dprintJob.PrintSiteId, dprintJob.Product);
            //
            ////Adding new task
            //int nextTaskId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
            //var tempId = jd.InsertDprintTrackingTasksForDuplicateEntry(dprintJob, WebSession.User.DisplayName, nextTaskId, equipmentNameSession, "Insert");
            //
            string[] array = hdnDuplicateWarnMessage.Value.Trim().Split(' ');
            string message = String.Join(" ", array.Where(s => !String.IsNullOrEmpty(s)));
            //
            jd.NotificationsForDuplicateEntry('I', "[Continued]-" + message, "User Responded to Msg", dprintJob.Job.ToString());
            //
            if (duplicateFoundFromSubMenu == "Y")
            {
                duplicateFoundFromSubMenu = string.Empty;
                duplicateCheckWarningFromSubMenu(null, null);
            }
            else if (duplicateFoundFromNextClick == "Y")
            {

                duplicateFoundFromNextClick = string.Empty;
                btnSelectEquipmentOK_Click(null, null);
            }
            else
            {
                ctlTimer.Interval = 30000;
                duplicateCheckWarning(null, null);
            }
        }

        protected void btnCancelNoDuplicate_Click(object sender, EventArgs e)
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();

            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

            ////Completeing a Task
            int tid = jd.GetDprintTrackingTasksID(dprintJob.Id, (int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")));
            int errorCode = jd.UpdateDprintTrackingTasksForDuplicateEntry(dprintJob.Id, tid, true, WebSession.User.DisplayName, "Cancel", dprintJob.Job, dprintJob.ComposerId, dprintJob.Split, dprintJob.PrintSiteId, dprintJob.Product);

            ////Adding new task
            //int nextTaskId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
            //var tempId = jd.InsertDprintTrackingTasksForDuplicateEntry(dprintJob, WebSession.User.DisplayName, nextTaskId, equipmentNameSession,"Insert");
            ctlTimer.Interval = 30000;
            string[] array = hdnDuplicateWarnMessage.Value.Trim().Split(' ');
            string message = String.Join(" ", array.Where(s => !String.IsNullOrEmpty(s)));

            jd.NotificationsForDuplicateEntry('I', "[Cancelled]-" + message, "User Responded to Msg", dprintJob.Job.ToString());
            jd.Notifications('D', "Duplicate Job Message presented to Operator - Operator did not Continue. Print Site " + labSiteNameInMessage.Text + "," + hidDupClientName.Value + "," + hidDupJob.Value + "," + hidDupProduct.Value + " and Split " + hidDupSplit.Value + ",Operator " + WebSession.User.DisplayName + ", Step " + labStatusInMessage.Text);
        }

        protected void btnReleaseHold_Click(object sender, EventArgs e)
        {
            var jd = new JobProcessingDataAdapter();
            int nextTaskId = jd.GetNextStatusOfHoldJob(selectedDprintJobsId);
            int previousTaskId = jd.GetDPrintTaskIdForHoldJob(nextTaskId);

            if (previousTaskId > 0)
            {
                DPrintTaskEnum taskEnum = (DPrintTaskEnum)previousTaskId;
                labReleaseHoldTask.Text = Convert.ToString(taskEnum);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                ModalReleaseHoldCaution.Show();
            }
        }

        protected void btnResendSpoolFile_Click(object sender, EventArgs e)
        {
            List<DPSysConfig> allStagingFolder = new List<DPSysConfig>();
            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);

            bool flag = false;
            laJobprocessing.Text = string.Empty;

            if (dprintJob.StatusId == 16 || dprintJob.StatusId == 15 || dprintJob.StatusId == 18 && ddEquipmentIdProcessJob.SelectedValue == "N/A")
            {
                if ((dprintJob.StatusId == 16 || dprintJob.StatusId == 15) && ddEquipmentIdProcessJob.SelectedValue == "N/A")
                {

                    ModalpopupexPanelEQMessage.Show();
                    return;
                }
                if (dprintJob.StatusId == 18 && ddEquipmentIdProcessJob.SelectedValue == "N/A")
                {
                    labResendWarning.Text = "Printing";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                    ModalResendWarning.Show();
                    return;
                }
                var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
                int siteID = Convert.ToInt32(ddLocation.SelectedValue);
                var printerStagingFolder = GetDPSysConfigBySiteId("SpoolFileStaging", siteID);
                if (printerStagingFolder.SiteID != siteID)
                {
                    printerStagingFolder = GetDPSysConfig("SpoolFileStaging");
                }
                var PrintingInProcessFolder = GetDPSysConfigBySiteId("PrintingInProcess", siteID);

                if (PrintingInProcessFolder.SiteID != siteID)
                {
                    PrintingInProcessFolder = GetDPSysConfig("PrintingInProcess");
                }
                var PrintingCompleteFolder = GetDPSysConfigBySiteId("PrintingComplete", siteID);

                if (PrintingCompleteFolder.SiteID != siteID)
                {
                    PrintingCompleteFolder = GetDPSysConfig("PrintingComplete");
                }

                ////printerStagingFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrinterStaging\";
                ////PrintingInProcessFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrintingInProcess\";
                ////PrintingCompleteFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrintingComplete\";

                List<FileInfo> spoolFile = new List<FileInfo>();
                allStagingFolder.Add(printerStagingFolder);
                allStagingFolder.Add(PrintingInProcessFolder);
                allStagingFolder.Add(PrintingCompleteFolder);
                JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
                List<string> spoolFileForMail = new List<string>();
                spoolFileForMail = GetSoolFileInfoForMail(dprintJob);
                foreach (DPSysConfig folder in allStagingFolder)
                {
                    spoolFile = GetSoolFileInfo(folder.DPVariableValue, dprintJob);

                    if (spoolFile.Count() > 0)
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag == true)
                {
                    Dictionary<string, string> ListOfFile = new Dictionary<string, string>();
                    foreach (FileInfo fi in spoolFile)
                        ListOfFile.Add(fi.Name, fi.FullName);

                    ddSpoolFileList.DataSource = ListOfFile;
                    ddSpoolFileList.DataTextField = "Key";
                    ddSpoolFileList.DataValueField = "Value";
                    ddSpoolFileList.DataBind();

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal2();", true);
                    ModalSelectSpoolFile.Show();
                }
                else
                {
                    int printersNgID = jd.GetPrinterNgIdByEquipment(ddEquipmentIdProcessJob.SelectedItem.Text, Int32.Parse(ddProcessingLocation.SelectedValue));
                    //bool isMailFlag = false;
                    string spoolFileName = string.Empty;
                    if (spoolFileForMail.Count() > 0)
                    {
                        foreach (var spoolFileForMatch in spoolFileForMail)
                        {
                            spoolFileName = spoolFileForMatch;
                            var printerTargets = jd.GetPrinterTargetsNg(printersNgID, spoolFileName);
                            //if (printerTargets.Count > 0)
                            //{
                            //    //isMailFlag = true;
                            //    AddCommandQueueForEmail(dprintJob, ddEquipmentIdProcessJob.SelectedItem.Text, "Event = ResendSkrfipoolFile Button click Message = /email /for file not found for inserting/Waiting to print ", spoolFileName);
                            //}
                        }
                    }

                    //if (!isMailFlag)
                    //{
                    //    AddCommandQueueForEmail(dprintJob, ddEquipmentIdProcessJob.SelectedItem.Text, "Event = ResendSpoolFile Button click Message = email for file not found for inserting/Waiting to print ", spoolFileName);
                    //}

                    laJobprocessing.Text = "Spool file could not be located in Staging, In‐process, or Completed folders.";
                    jd.InsertAttributesChangedTrackingTasks(dprintJob, WebSession.User.DisplayName, 1, string.Empty, "Red");
                    //jd.setJobStatus(dprintJob.Id, 1, string.Empty, "Spool file cannot be located");
                    prepareEmail(dprintJob, "Spool", "printer");

                    int i = 0;
                    int selectedIndexInPopUp = i;
                    var details = jd.getAllSplitsJob(dprintJob.Job, getCurrentLocationId(), dprintJob.PrintSiteId, 1, dprintJob.IsComplete).ToList();
                    foreach (var jobProcessing in details)
                    {
                        if (jobProcessing.Id == dprintJob.Id)
                        {
                            selectedIndexInPopUp = i;
                            break;
                        }
                        i++;
                    }
                    gvPopUpWindows.DataSource = details;// jd.getAllSplitsJob(DpJobsSession[selectedIndex].Job, getCurrentLocationId(), DpJobsSession[selectedIndex].PrintSiteId, 1, DpJobsSession[selectedIndex].IsComplete).ToList();
                    gvPopUpWindows.DataBind();

                    gvPopUpWindows.Rows[selectedIndexInPopUp].ForeColor = Color.Green;
                    ((CheckBox)gvPopUpWindows.Rows[selectedIndexInPopUp].FindControl("cbDataSelected")).Checked = true;

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal2();", true);
                    ModalJobStatusSplit.Show();
                }

            }
            else if (dprintJob.StatusId == 17)
            {
                labResendWarning.Text = "Printing";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                ModalResendWarning.Show();
            }
            else
            {
                labResendWarning.Text = "Printing";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                ModalResendWarning.Show();
            }
        }

        private void reSendSpoolFile(List<FileInfo> spoolFile)
        {
            StringBuilder msg = new StringBuilder();
            StringBuilder log = new StringBuilder();

            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

            string equipmentName = ddEquipmentIdProcessJob.SelectedValue.ToString();

            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);
            var printerStagingFolder = GetDPSysConfigBySiteId("SpoolFileStaging", siteID);
            if (printerStagingFolder.SiteID != siteID)
            {
                printerStagingFolder = GetDPSysConfig("SpoolFileStaging");
            }
            var PrintingInProcessFolder = GetDPSysConfigBySiteId("PrintingInProcess", siteID);

            if (PrintingInProcessFolder.SiteID != siteID)
            {
                PrintingInProcessFolder = GetDPSysConfig("PrintingInProcess");
            }
            var PrintingCompleteFolder = GetDPSysConfigBySiteId("PrintingComplete", siteID);

            if (PrintingCompleteFolder.SiteID != siteID)
            {
                PrintingCompleteFolder = GetDPSysConfig("PrintingComplete");
            }

            /////printerStagingFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrinterStaging\";
            /////PrintingInProcessFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrintingInProcess\";
            /////PrintingCompleteFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\PrintingComplete\";
            /////
            if (dprintJob.IsComplete == 0)
            {

                if (string.IsNullOrEmpty(equipmentName) || equipmentName == "N /A")
                {
                    msg.Clear();
                    msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Equipment is not found.");
                    laJobprocessing.ForeColor = Color.Red;
                }
                else
                {
                    jd.Notifications('I', "# 3 gui - moving spool file -");
                    int printersNgID = jd.GetPrinterNgIdByEquipment(equipmentName, Int32.Parse(ddProcessingLocation.SelectedValue));
                    jd.Notifications('I', "#4 gui - PrintersNg Id found - " + printersNgID);

                    jd.Notifications('I', "# 5 gui - spoolFilePath count - " + spoolFile.Count.ToString());
                    if (spoolFile.Count() > 0)
                    {
                        string spoolFilePath = spoolFile.FirstOrDefault().FullName;//  selectCorrectSpoolFile(spoolFile, DpJobsSession[selectedIndex]);// spoolFile.OrderBy(x => x.LastWriteTime).Select(x => x.FullName).First().ToString();
                        string spoolFileName = Path.GetFileName(spoolFilePath);// spoolFile.OrderBy(x => x.LastWriteTime).Select(x => x.Name).First().ToString();

                        if (string.IsNullOrEmpty(spoolFilePath))
                        {
                            // msg.AppendLine("A spool file is available for this Split in the Printer Staging folder however it does not match the format needed for the Printer showing in the Equipment list.");

                        }
                        else
                        {
                            jd.Notifications('I', "# 6 gui - spoolFilePath - " + spoolFilePath);
                            if (printersNgID != 0)
                            {
                                var printerTargets = jd.GetPrinterTargetsNg(printersNgID, spoolFileName);

                                jd.Notifications('I', "# 7 gui - printer target count - " + printerTargets.Count.ToString());

                                if (printerTargets.Count > 1)
                                {
                                    string emailTitel = "[W11.2] Printer Master / Printer Targets Configuration Error in " + dprintJob.PrintLocation;
                                    string emailBody = "Multiple records returned from PrinterTargetsNg table during single record lookup.Check MatchKeyType and MatchKeyValue configurations.";
                                    emailBody += Environment.NewLine + "Please resolve. Job = " + dprintJob.Job.ToString() + " Product = " + dprintJob.Product + " Split = " + dprintJob.Split.ToString() + " PrinterNgID = " + printersNgID.ToString() + " and MatchKeyValue = " + spoolFileName;
                                    sendEmail(emailTitel, emailBody);
                                    jd.Notifications('I', "# 8 gui - Multiple records returned from PrinterTargetsNg table during single record lookup.Check MatchKeyType and MatchKeyValue configurations.");
                                    msg.AppendLine("Multiple records returned from PrinterTargetsNg table during single record lookup.Check MatchKeyType and MatchKeyValue configurations.");
                                }
                                else if (printerTargets.Count == 0)
                                {
                                    msg.AppendLine("Selected spool file is available for this Split in the Printer Staging folder however it does not match the format needed for the Printer showing in the Equipment list.");
                                    string emailTitel = "[W11.2] PrinterTargetsNg record could not be found in " + dprintJob.PrintLocation;
                                    string emailBody = "While attempting to move a Job to a Printer, a required record in the PrinterTargetsNg table could not be located for";
                                    emailBody += Environment.NewLine + " Job = " + dprintJob.Job.ToString() + " Product = " + dprintJob.Product + " Split = " + dprintJob.Split.ToString() + " PrinterNgID = " + printersNgID.ToString() + " and MatchKeyValue = " + spoolFileName;
                                    sendEmail(emailTitel, emailBody);
                                    // msg.AppendLine(" - PrinterTargetsNg record could not be found.");
                                    jd.Notifications('I', "# 9 gui - PrinterTargetsNg record could not be found in " + dprintJob.PrintLocation);

                                }
                                else if (printerTargets.Count == 1)
                                {
                                    string pattern = @"\[\d+\]";
                                    string MatchKeyType = Regex.IsMatch(printerTargets[0].MatchKeyType.ToLower(), pattern) == true ? printerTargets[0].MatchKeyType.ToLower() : string.Empty;

                                    int NextPrinterTargetsNgID = -1;
                                    string CommandType = "copy";
                                    string AutomationString = string.Empty;
                                    var printerTargetExeCmd = jd.GetPrinterTargetsNgExecuteCommand(printerTargets[0].NextPrinterTargetsNgID);
                                    if (printerTargetExeCmd.Count == 1 && printerTargetExeCmd[0].TargetPurpose.ToLower() == "execute command")
                                    {
                                        CommandType = "execute command";
                                        AutomationString = printerTargetExeCmd[0].AutomationString;
                                        NextPrinterTargetsNgID = printerTargets[0].NextPrinterTargetsNgID;
                                    }
                                    if (printerTargets[0].TransferMethod.Trim().ToLower().Contains("xcopy") || printerTargets[0].TransferMethod.Trim().ToLower().Contains("ftp"))
                                    {

                                        if (commandQueue(spoolFilePath, printerTargets[0].DropOffLocation, CommandType, printerTargets[0].TransferMethod, printerTargets[0].NetworkAddress, printerTargets[0].MatchKeyValue, false, dprintJob, equipmentName, string.Empty, MatchKeyType, AutomationString, printerTargets[0].PrinterTargetsNgID, NextPrinterTargetsNgID))


                                        {
                                            // msg.AppendLine("-job composer id - " + DpJobsSession[selectedIndex].ComposerId.ToString() + " - xcopy command is sent to queue to copy Spool file at Drop of location.");
                                            msg.Append("Spool file located in " + spoolFilePath + " Spool file resent to Printer " + equipmentName + " using " + printerTargets[0].TransferMethod + ".");

                                            jd.Notifications('I', "#gui - Spool file is send to xcopy command for to Drop of location queue");
                                            laJobprocessing.ForeColor = Color.Black;

                                            /* xCopyOutPut xPinPOutput = xCopy(spoolFilePath, PrintingInProcessFolder.DPVariableValue); --------- */
                                            bool isMessage = false;
                                            foreach (ListItem fileInddList in ddSpoolFileList.Items)
                                            {
                                                spoolFilePath = fileInddList.Value;

                                                if (commandQueue(spoolFilePath, PrintingInProcessFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, dprintJob, equipmentName))
                                                {
                                                    if (isMessage == false)
                                                    {
                                                        msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - move command is sent to command queue to move spool file to ProcessFolder.");
                                                        jd.Notifications('I', "#gui - command is sent to queue to move Spool file - ");
                                                        isMessage = true;
                                                        ;
                                                    }
                                                }
                                                else
                                                {
                                                    if (isMessage == false)
                                                    {
                                                        msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - move command is not added to queue for to PrintingInProcessFolder folder. Please check the MSMQ configuration.");
                                                        jd.Notifications('E', "#gui - move command is not added to queue for to PrintingInProcessFolder folder. Please check the MSMQ configuration");
                                                        isMessage = true;
                                                    }
                                                }
                                            }

                                            if ((int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")) == 15)
                                            {
                                                int tid = jd.GetDprintTrackingTasksID(dprintJob.Id, (int)Enum.Parse(typeof(DPrintTaskEnum), dprintJob.Status.Replace(" ", "")));
                                                int errorCode = jd.UpdateDprintTrackingTasks(dprintJob.Id, tid, true, equipmentName, WebSession.User.DisplayName);

                                                //Adding new task
                                                int nextTaskId = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                                                var tempId = jd.InsertDprintTrackingTasks(dprintJob, WebSession.User.DisplayName, nextTaskId, equipmentName);

                                                int nextTask = jd.GetNextTaskId(dprintJob.Status, dprintJob.WorkflowProcessID);
                                                jd.setJobProcessingStatus(dprintJob.Id, nextTask);
                                            }

                                        }
                                        else
                                        {
                                            msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - xcopy command is not added to queue for to Drop of location folder. Please check the MSMQ configuration.");
                                            string emailTitel = "Spool File could not be sent to Printer";
                                            string emailBody = "Operator " + WebSession.User.DisplayName + " attempted to move Spool File <Spool File Name> to Printer<composer.dbo.printer_name> and operation failed. Please resolve issue.";

                                            jd.Notifications('E', "#gui - xcopy command is not added to queue for to Drop of location folder. Please check the MSMQ configuration");
                                            laJobprocessing.ForeColor = Color.Red;
                                            sendEmail(emailTitel, emailBody);
                                        }
                                    }
                                    else if (printerTargets[0].TransferMethod.Trim().ToLower().Contains("lpr"))
                                    {
                                        if (commandQueue(spoolFilePath, printerTargets[0].DropOffLocation, CommandType, printerTargets[0].TransferMethod, printerTargets[0].NetworkAddress, printerTargets[0].MatchKeyValue, printerTargets[0].TransferBinary, dprintJob, equipmentName, string.Empty, MatchKeyType, AutomationString, printerTargets[0].PrinterTargetsNgID, NextPrinterTargetsNgID))
                                        {
                                            //msg.AppendLine("-job composer id - " + DpJobsSession[selectedIndex].ComposerId.ToString() + " - Spool file is sent to LPR queue.");
                                            msg.Append("Spool file located in " + spoolFilePath + "Spool file resent to Printer " + equipmentName + " using " + printerTargets[0].TransferMethod);

                                            foreach (ListItem fileInddList in ddSpoolFileList.Items)
                                            {
                                                spoolFilePath = fileInddList.Value;

                                                if (commandQueue(spoolFilePath, PrintingInProcessFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, dprintJob, equipmentName))
                                                {
                                                    msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - move command is sent to command queue to move spool file to ProcessFolder.");
                                                    jd.Notifications('I', "#gui - command is sent to queue to move Spool file - ");
                                                }
                                                else
                                                {
                                                    msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - move command is not added to queue for to PrintingInProcessFolder folder. Please check the MSMQ configuration.");
                                                    jd.Notifications('E', "#gui - move command is not added to queue for to PrintingInProcessFolder folder. Please check the MSMQ configuration");
                                                }
                                            }

                                        }
                                        else
                                            msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - LPR command is not added to MSMQ. Please check the MSMQ configuration");
                                    }

                                }
                            }
                            else
                            {
                                msg.Append("printersNgID is not found for selected equipment " + equipmentName + ". User defaults for this computer have been lost. Please reset the  [Job Processing Dashboard] defaults for this step as defined in the User's Guide. ");
                            }
                        }
                    }
                    else
                    {
                        msg.Append("Spool file for this Split could not be found in Printer Staging Folder in " + dprintJob.PrintLocation);
                        sendEmail("Spool file for this Split could not be found in Printer Staging Folder in " + dprintJob.PrintLocation, "Operator " + WebSession.User.DisplayName + " received this error for: Job=" + Convert.ToString(dprintJob.Job) + " , Product = " + dprintJob.Product + " , Split = " + Convert.ToString(dprintJob.Split) + ", Equipment Spool file Prefix = " + createEqupmentPrefix());
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                modalWarningMsg.Show();
            }

            laJobprocessing.Text = msg.ToString();
            msg.Clear();

            DpJobsSession = jd.GetJobProcessing(DpSearchByDataSession, Convert.ToInt32(((DropDownList)this.Master.FindControl("ddlLocation")).SelectedIndex));
            BtnProcessJobSplit_Clicked(null, null);
        }


        private void setTrackingTask(List<FileInfo> spoolFiles)
        {
            var dpPrintDataAdapter = new DpPrintStatusDataAdapter();
            var jd = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);


            List<DpJobHistory> taskList = dpPrintDataAdapter.GetDpJobHistories(dprintJob.Id, dprintJob.PrintSiteId);
            int dpTrackingTasksID = taskList.Where(x => x.TaskId == 16).Select(o => o.DPrintTrackingTasksID).LastOrDefault();

            int failedAttempts = taskList.Where(x => x.DPrintTrackingTasksID == dpTrackingTasksID).Select(o => o.FailedAttempts).LastOrDefault() + 1;
            if (failedAttempts > 3)
                jd.setTrackingTaskStatus(dpTrackingTasksID, failedAttempts, "Red");
            else
                jd.setTrackingTaskStatus(dpTrackingTasksID, failedAttempts, "Yellow");

            jd.Notifications('I', "Spool file located in " + string.Join(",", spoolFiles.Select(x => x.FullName).ToArray()) + " Spool file resent to Printer <PrinterName> using <TransferMethod>.");
        }

        private void setTrackingTask(string MRDFfile)
        {
            var dpPrintDataAdapter = new DpPrintStatusDataAdapter();
            var jd = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

            List<DpJobHistory> taskList = dpPrintDataAdapter.GetDpJobHistories(dprintJob.Id, dprintJob.PrintSiteId);
            int dpTrackingTasksID = taskList.Where(x => x.TaskId == 18).Select(o => o.DPrintTrackingTasksID).LastOrDefault();

            int failedAttempts = taskList.Where(x => x.DPrintTrackingTasksID == dpTrackingTasksID).Select(o => o.FailedAttempts).LastOrDefault() + 1;
            if (failedAttempts > 3)
                jd.setTrackingTaskStatus(dpTrackingTasksID, failedAttempts, "Red");
            else
                jd.setTrackingTaskStatus(dpTrackingTasksID, failedAttempts, "Yellow");

            jd.Notifications('I', "MRDF file located in " + MRDFfile + " MRDF file resent to Inserter <PrinterName> using <TransferMethod>.");
        }


        private void prepareEmail(JobProcessingModel item, string fileType, string equipmentType)
        {
            string subject = "[E10.1] " + fileType + " File Missing in " + item.PrintLocation;
            StringBuilder emailBody = new StringBuilder();
            emailBody.Append("User " + WebSession.User.DisplayName + " attempted to re‐send a " + fileType + " file to a " + equipmentType + " and the " + fileType + " file could not be located in the Staging, In‐Process or Completed Folders. for the following Job / Split");
            emailBody.Append("<br>");
            emailBody.Append("<br>");
            string htmlTable = "<table border=\"1\"><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Client</td><td style=\"text-align:left;font-size:medium;width: 200px;\"> " + item.ClientName + "</td></tr><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Job </td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + Convert.ToString(item.Job) + "</td></tr> <tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Product</td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + item.Product + "</td></tr><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Split</td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + Convert.ToString(item.Split) + "</td></tr><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">PrintSite</td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + item.PrintLocation + "</td></tr><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Total Volume</td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + Convert.ToString(item.Quantity) + "</td></tr><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Total Sheets</td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + Convert.ToString(item.TotalSheetCount) + "</td></tr><tr><td style=\"text-align:left;font-weight:bold;font-size:medium;width: 200px;\">Scheduled Mail Date</td><td style=\"text-align:left;font-size:medium;width: 200px;\">" + Convert.ToString(item.ScheduledMailDate) + "</td></tr></table>";
            emailBody.Append(htmlTable);
            emailBody.Append("<br>");
            emailBody.AppendLine("Recommended Corrective Action is to Reset the Job to “Send to FTP Engine”. The DP system will reprocess the job as if it was just received at the DP site.This will result in new job tickets being printed with Duplicate watermarks. This may also result in duplicate warning being displayed to the Operator.However this action should correct the issue");
            sendEmail(subject, emailBody.ToString());
            var jd = new JobProcessingDataAdapter();
            jd.Notifications('E', "User " + WebSession.User.DisplayName + " attempted to re‐send a " + fileType + " file to a " + equipmentType + " and the " + fileType + " file could not be located in the Staging, In‐Process or Completed Folders. for the following Job / Split" + item.ClientName + "," + Convert.ToString(item.Job) + "," + item.Product + "," + Convert.ToString(item.Split));

        }

        protected void btnYesReleaseHold_Click(object sender, EventArgs e)
        {
            var jd = new JobProcessingDataAdapter();
            int nextTaskId = jd.GetNextStatusOfHoldJob(selectedDprintJobsId);
            int previousTaskId = jd.GetDPrintTaskIdForHoldJob(nextTaskId);
            jd.ChangeDprintTaskId(selectedDprintJobsId, previousTaskId);
            ctlTimer.Interval = 30000;
            ExecuteSearch_Click(null, null);
        }

        protected void btnNoCencelReleaseHold_Click(object sender, EventArgs e)
        {
            if (hdnReturnToViewFlag.Value == "1")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                ModalReprintTickets.Show();
            }
            else
            {
                ctlTimer.Interval = 30000;
            }

        }
        protected void btnResendInserterControlFile_Click(object sender, EventArgs e)
        {
            List<DPSysConfig> allStagingFolder = new List<DPSysConfig>();
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

            if ((dprintJob.StatusId == 18) && ddEquipmentIdProcessJob.SelectedValue == "N/A")
            {

                ModalpopupexPanelEQMessage.Show();
                return;
            }

            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

            var mrdfStagingFolder = GetDPSysConfigBySiteId("MRDFStaging", siteID);

            if (mrdfStagingFolder.SiteID != siteID)
            {
                mrdfStagingFolder = GetDPSysConfig("MRDFStaging");
            }
            var mrdfInProcessFolder = GetDPSysConfigBySiteId("MRDFInProcess", siteID);

            if (mrdfInProcessFolder.SiteID != siteID)
            {
                mrdfInProcessFolder = GetDPSysConfig("MRDFInProcess");
            }
            var mrdfCompleteFolder = GetDPSysConfigBySiteId("MRDFComplete", siteID);

            if (mrdfCompleteFolder.SiteID != siteID)
            {
                mrdfCompleteFolder = GetDPSysConfig("MRDFComplete");
            }

            //// mrdfStagingFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFStaging\";
            //// mrdfInProcessFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFInProcess\";
            //// mrdfCompleteFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFComplete\";

            bool flag = false;
            laJobprocessing.Text = string.Empty;

            if (dprintJob.StatusId == 18)
            {
                string MRDFfile = string.Empty;
                allStagingFolder.Add(mrdfStagingFolder);
                allStagingFolder.Add(mrdfInProcessFolder);
                allStagingFolder.Add(mrdfCompleteFolder);


                foreach (DPSysConfig folder in allStagingFolder)
                {
                    MRDFfile = MrdfFileName(dprintJob, folder.DPVariableValue);
                    if (File.Exists(MRDFfile))
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag == true)
                {
                    reSendMRDFfile(MRDFfile, false);
                    setTrackingTask(MRDFfile);
                }
                else
                {
                    laJobprocessing.Text = "Inserter Control File (MRDF) could not be located in Staging, In‐process, or Completed folders.";
                    jd.InsertAttributesChangedTrackingTasks(dprintJob, WebSession.User.DisplayName, 1, string.Empty, "Red");
                    //jd.setJobStatus(dprintJob.Id, 1, string.Empty, "MRDF file cannot be located");
                    prepareEmail(dprintJob, "MRDF", "Inserter");

                    int i = 0;
                    int selectedIndexInPopUp = i;
                    var details = jd.getAllSplitsJob(dprintJob.Job, getCurrentLocationId(), dprintJob.PrintSiteId, 1, dprintJob.IsComplete).ToList();
                    foreach (var jobProcessing in details)
                    {
                        if (jobProcessing.Id == dprintJob.Id)
                        {
                            selectedIndexInPopUp = i;
                            break;
                        }
                        i++;
                    }
                    gvPopUpWindows.DataSource = details;
                    gvPopUpWindows.DataBind();

                    gvPopUpWindows.Rows[selectedIndexInPopUp].ForeColor = Color.Green;
                    ((CheckBox)gvPopUpWindows.Rows[selectedIndexInPopUp].FindControl("cbDataSelected")).Checked = true;

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal2();", true);
                    ModalJobStatusSplit.Show();
                }

            }
            //if (dprintJob.StatusId == 17)
            //{
            //    BtnUpdateToNext_Clicked(btnUpdateToNext, new EventArgs());
            //}
            else
            {
                labResendWarning.Text = "Inserting";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                ModalResendWarning.Show();
            }
        }

        private void reSendMRDFfile(string sourceFileName, bool flag = true)
        {
            StringBuilder msg = new StringBuilder();
            StringBuilder log = new StringBuilder();

            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

            string equipmentName = ddEquipmentIdProcessJob.SelectedItem.Text;
            var inserterInfo = GetInserter(equipmentName, Int32.Parse(ddProcessingLocation.SelectedValue));
            equipmentNameSession = inserterInfo.InserterName;
            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);


            var mrdfInProcessFolder = GetDPSysConfigBySiteId("MRDFInProcess", siteID);

            if (mrdfInProcessFolder.SiteID != siteID)
            {
                mrdfInProcessFolder = GetDPSysConfig("MRDFInProcess");
            }

            // mrdfStagingFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFStaging\";
            //mrdfInProcessFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFInProcess\";
            // mrdfCompleteFolder.DPVariableValue = @"\\Bhtdps02\NCP\Jobs\MRDFComplete\";

            if (dprintJob.IsComplete == 0)
            {
                if (equipmentName == "N/A" || string.IsNullOrEmpty(equipmentName))
                    msg.Append("Inserter is not found for selected equipment " + equipmentName + ". User defaults for this computer have been lost. Please reset the  [Job Processing Dashboard] defaults for this step as defined in the User's Guide. ");
                else
                {
                    if (inserterInfo.TransferMethod.ToLower().Contains("ftp"))
                    {
                        //FTP
                        if (commandQueue(sourceFileName, inserterInfo.ImportFolder, "COPY", "FTP", inserterInfo.IPAddress, string.Empty, false, dprintJob, equipmentName))
                        {
                            msg.AppendLine("MRDF file located in " + sourceFileName + " MRDF file resent to Inserter " + equipmentName + " using " + inserterInfo.TransferMethod.ToLower());


                            if (commandQueue(sourceFileName, mrdfInProcessFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, dprintJob, equipmentName, "FTP and "))
                            {
                                msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - move command is sent to command queue for moving mrdfInProcessFolder.");
                            }
                            else
                                msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - move command is not added to queue for mrdfInProcessFolder. Please check the MSMQ configuration");

                            checkForQtyPullsAfterPrint(Convert.ToString(inserterInfo.InserterMasterID), Convert.ToString(dprintJob.Id), 0, flag);
                        }
                        else
                        {
                            msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - ftp command is not added to queue. Please check the MSMQ configuration.");
                            msg.AppendLine("ftp source - " + sourceFileName);
                            msg.AppendLine("destination - " + inserterInfo.ImportFolder);
                            SendNotificationReport(inserterInfo.InserterName, msg.ToString());
                            jd.Notifications('E', "# gui - MRDF file - ftp command is not added to queue.");
                        }

                    }
                    else if (inserterInfo.TransferMethod.ToLower().Contains("xcopy"))
                    {
                        //XCOPY
                        string importFolder = @"\\" + inserterInfo.ServerControllerName + "\\" + inserterInfo.ImportFolder;
                        jd.Notifications('I', "#3 gui - get Import folder -" + importFolder);

                        bool xQueue = commandQueue(sourceFileName, importFolder, "COPY", "Xcopy", string.Empty, string.Empty, false, dprintJob, equipmentName);
                        if (xQueue)
                            msg.AppendLine("MRDF file located in " + sourceFileName + " MRDF file resent to Inserter " + equipmentName + " using " + inserterInfo.TransferMethod.ToLower());
                        else
                            msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - xcopy command is not added to queue. Please check the MSMQ configuration.");


                        if (xQueue == false)
                        {
                            SendNotificationReport(inserterInfo.InserterName, msg.ToString());
                            jd.Notifications('E', "# gui - MRDF file is not copied.");
                        }
                        else
                        {

                            if (commandQueue(sourceFileName, mrdfInProcessFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, dprintJob, equipmentName))
                            {
                                msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - move command is sent to command queue for moving mrdfInProcessFolder.");
                            }
                            else
                                msg.AppendLine("-job composer id - " + dprintJob.ComposerId.ToString() + " - Error - move command is not added to queue for mrdfInProcessFolder. Please check the MSMQ configuration");

                            checkForQtyPullsAfterPrint(Convert.ToString(inserterInfo.InserterMasterID), Convert.ToString(dprintJob.Id), 0, flag);
                        }
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                modalWarningMsg.Show();
            }

            laJobprocessing.Text = msg.ToString();
            msg.Clear();

            DpJobsSession = jd.GetJobProcessing(DpSearchByDataSession, Convert.ToInt32(((DropDownList)this.Master.FindControl("ddlLocation")).SelectedIndex));
            BtnProcessJobSplit_Clicked(null, null);
        }

        protected void btnReprintTicket_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> TicketType = new Dictionary<string, string>();
            TicketType.Add("Job Ticket", "JT");
            TicketType.Add("Split Ticket", "ST");
            TicketType.Add("Pick Ticket", "PT");
            TicketType.Add("Final Split Ticket", "FT");

            // For Fetching the dynamic file type from the Db
            var reJobProcessingDataAdapter = new JobProcessingDataAdapter();
            JobProcessingModel repDprintJob = reJobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);
            List<ContainerContents> itemsInContainer = new List<ContainerContents>();
            itemsInContainer = reJobProcessingDataAdapter.GetContainerContentsByDpJobId(repDprintJob.Id);
            var item = itemsInContainer.Where(c => c.FileType == "Certified Listing").FirstOrDefault();
            if (item != null)
            {
                TicketType.Add(item.FileType, "CL");
            }

            ddReprintTicketType.DataSource = TicketType;
            ddReprintTicketType.DataTextField = "Key";
            ddReprintTicketType.DataValueField = "Value";

            //Dont overwrite if ticket type is selected
            //if (ddReprintTicketType.SelectedIndex > 0)
            //    ddReprintTicketType.SelectedIndex = ddReprintTicketType.SelectedIndex;
            ddReprintTicketType.DataBind();

            if (sender != null)
            {
                printerMsg.Text = string.Empty;

                ManagementScope objScope = new ManagementScope(ManagementPath.DefaultPath); //For the local Access
                objScope.Connect();

                SelectQuery selectQuery = new SelectQuery();
                selectQuery.QueryString = "Select * from win32_Printer";
                ManagementObjectSearcher MOS = new ManagementObjectSearcher(objScope, selectQuery);
                ManagementObjectCollection MOC = MOS.Get();
                //Check if dropdown has already list of printers
                if (ddReprintTicketPrinters.Items.Count <= 0)
                    foreach (ManagementObject mo in MOC)
                        ddReprintTicketPrinters.Items.Add(mo["Name"].ToString());
            }


            //foreach (string printer in PrinterSettings.InstalledPrinters)
            //    ddReprintTicketPrinters.Items.Add(printer);
            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);

            try
            {
                PrinterSettings settings = new PrinterSettings();
                //Dont overwrite if printer is selected
                if (sender == null)
                {
                    ddReprintTicketPrinters.SelectedIndex = ddReprintTicketPrinters.SelectedIndex;
                    ddReprintTicketPrinters.Text = ddReprintTicketPrinters.SelectedValue;
                }
                else
                {
                    ddReprintTicketPrinters.Text = settings.PrinterName;
                }
            }
            catch (Exception ex)
            {
                var jd = new JobProcessingDataAdapter();
                jd.Notifications('E', "#gui - Could not find the default printer settings." + ex.Message);
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            ModalReprintTickets.Show();

            txtReprintTicketJob.Text = Convert.ToString(dprintJob.Job);
            txtReprintTicketProduct.Text = dprintJob.Product;
            txtReprintTicketSplitNumber.Text = Convert.ToString(dprintJob.Split);
        }

        protected string createFileName()
        {
            var jobProcessingDataAdapter = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jobProcessingDataAdapter.GetDPrintJobById(selectedDprintJobsId);
            List<ContainerContents> itemsInContainer = new List<ContainerContents>();
            itemsInContainer = jobProcessingDataAdapter.GetContainerContentsByDpJobId(dprintJob.Id);
            string fileName = string.Empty;

            //Check if the container table has all the required file details
            if (itemsInContainer.Count > 0)
            {
                string TicketType = ddReprintTicketType.SelectedValue;
                if (TicketType == "JT")
                {
                    fileName = Convert.ToString(itemsInContainer.Where(x => x.FileType == "JobTicket").FirstOrDefault().Filename);
                }
                else if (TicketType == "ST")
                {
                    fileName = Convert.ToString(itemsInContainer.Where(x => x.FileType == "SplitTicket").FirstOrDefault().Filename);
                }
                else if (TicketType == "PT")
                {
                    fileName = Convert.ToString(itemsInContainer.Where(x => x.FileType == "PickTicket").FirstOrDefault().Filename);
                }
                else if (TicketType == "FT")
                {
                    string[] startEndSeq = dprintJob.Sequences.Split('-');
                    fileName = TicketType + "_" + dprintJob.Product + "_" +
                                           dprintJob.Job.ToString("D8") + "_" +
                                           dprintJob.Split.ToString("D7") + "_" +
                                           startEndSeq[0].Trim().ToInt().ToString("D9") + "_" +
                                           startEndSeq[1].Trim().ToInt().ToString("D9") + "_" +
                                           dprintJob.ComposerId.ToString("D9") + ".pdf";
                }
                else if (TicketType == "CL")
                {
                    fileName = Convert.ToString(itemsInContainer.Where(x => x.FileType == "Certified Listing").FirstOrDefault().Filename);
                }
            }
            else
            {
                jobProcessingDataAdapter.Notifications('i', "No records found in table [DPJobs].[ContainerContents] for job number" + dprintJob.Job.ToString("D8"), "Reprints Service");

                fileName = GenerateFileName(dprintJob, ddReprintTicketType.SelectedValue);

                jobProcessingDataAdapter.Notifications('i', "Generated file name for job number" + dprintJob.Job.ToString("D8") + ": " + fileName, "Reprints Service");
            }
            return fileName;

        }

        protected void btnTicketPrint_Click(object sender, EventArgs e)
        {
            string TicketType = ddReprintTicketType.SelectedValue;

            //Dont overwrite if ticket type is selected
            if (ddReprintTicketType.SelectedIndex > 0)
                ddReprintTicketType.SelectedIndex = ddReprintTicketType.SelectedIndex;

            //Dont overwrite if printer is selected
            //if (ddReprintTicketPrinters.SelectedIndex > 0)
            //    ddReprintTicketPrinters.SelectedIndex = ddReprintTicketPrinters.SelectedIndex;

            string fileType = string.Empty;
            var jd = new JobProcessingDataAdapter();
            try
            {

                JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

                string sourceFolder = jd.SystemConfiguration(dprintJob.PrintSiteId, "JobInfoArchive");
                string[] startEndSeq = dprintJob.Sequences.Split('-');
                string ticketFile = string.Empty;
                string fileName = createFileName();

                if (TicketType == "FT")
                {
                    displayFinalSplitTicketSumary(dprintJob, true, sourceFolder);
                }
                else
                {
                    ticketFile = Path.Combine(sourceFolder, fileName);

                    if (File.Exists(ticketFile))
                    {
                        printTickets(fileName, sourceFolder);
                    }
                    else
                    {
                        jd.Notifications('E', "#gui - Reprint Ticket Feature could not find document in archive to re‐print." + "Dp job id " + Convert.ToString(dprintJob.Id));
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                        txtReprintWarning.InnerText = "Selected document " + fileName + " could not be found at " + sourceFolder + " location";
                        ModalReprintWarning.Show();
                        printerMsg.Text = string.Empty;

                    }
                }
            }
            catch (Exception ex)
            {
                jd.Notifications('E', "An exception has occured while Printing the ticket. " + "DP Job Id:  " + Convert.ToString(selectedDprintJobsId) +
                    " Exception message: " + ex.Message.ToString());
            }
        }

        protected void btnViewPrint_Click(object sender, EventArgs e)
        {
            string TicketType = ddReprintTicketType.SelectedValue;
            //Dont overwrite if ticket type is selected
            if (ddReprintTicketType.SelectedIndex > 0)
                ddReprintTicketType.SelectedIndex = ddReprintTicketType.SelectedIndex;

            ////Dont overwrite if printer is selected
            //if (ddReprintTicketPrinters.SelectedIndex > 0)
            //    ddReprintTicketPrinters.SelectedIndex = ddReprintTicketPrinters.SelectedIndex;

            string fileType = string.Empty;
            var jd = new JobProcessingDataAdapter();
            try
            {

                JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);
                string sourceFolder = jd.SystemConfiguration(dprintJob.PrintSiteId, "JobInfoArchive");
                string[] startEndSeq = dprintJob.Sequences.Split('-');
                string ticketFile = string.Empty;
                string fileName = createFileName();

                if (TicketType == "FT")
                {
                    displayFinalSplitTicketSumary(dprintJob);
                    btnReprintTicket_Click(null, null);
                }
                else
                {
                    ticketFile = Path.Combine(sourceFolder, fileName);

                    if (File.Exists(ticketFile))
                    {
                        showPrintTicket(ticketFile);
                    }
                    else
                    {
                        jd.Notifications('E', "#gui - Reprint Ticket Feature could not find document in archive to re‐print." + "Dp job id " + Convert.ToString(dprintJob.Id));
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "<script>SetFlagToReturnViewGui(1);</script>", false);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                        txtReprintWarning.InnerText = "Selected document " + fileName + " could not be found at " + sourceFolder + " location";
                        ModalReprintWarning.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                jd.Notifications('E', "An exception has occured while Viewing the ticket. " + "DP Job Id:  " + Convert.ToString(selectedDprintJobsId) +
                    " Exception message: " + ex.Message.ToString());
            }
        }

        protected void showPrintTicket(string filename)
        {
            var dpPrintDataAdapter = new DpPrintStatusDataAdapter();
            var jd = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

            int siteID = Convert.ToInt32(ConfigurationManager.AppSettings["PrintSiteId"]);
            List<DpJobHistory> jobHistories = dpPrintDataAdapter.GetDpJobHistories(dprintJob.Id, siteID).ToList();

            int index = -1;
            int previousTaskIndex = -1;
            bool isTransferFlag = false;
            string lastTask = string.Empty;

            try
            {
                index = jobHistories.FindLastIndex(o => o.TaskId == 28);
                if (index == -1)
                    index = jobHistories.FindLastIndex(o => o.TaskId == 29);

                if (index != -1)
                {
                    isTransferFlag = true;
                    previousTaskIndex = index - 1;
                    lastTask = jobHistories[previousTaskIndex].DPrintTask;
                }
                else
                    isTransferFlag = false;
            }
            catch (Exception ex)
            {
                isTransferFlag = false;
            }

            bool isSplitTicket = false;
            string firstThree = Path.GetFileName(filename).Substring(0, 3).ToUpper();
            if (firstThree == "ST_")
                isSplitTicket = true;

            try
            {
                string equipmentUsed = ddReprintTicketType.SelectedItem.Text.ToString();
                int duplicate = dpPrintDataAdapter.CheckTicketAlreadyPrinted(dprintJob.Id, dprintJob.PrintSiteId, equipmentUsed);
                if (duplicate > 0)
                {
                    if (firstThree == "PT_")
                        PdfData = jd.AddWaterMark(filename, duplicate, dprintJob.FromLocation, true, false, lastTask, isSplitTicket, txtReprintTicketWatermark.Text);
                    else
                        PdfData = jd.AddWaterMark(filename, duplicate, dprintJob.FromLocation, true, isTransferFlag, lastTask, isSplitTicket, txtReprintTicketWatermark.Text);
                }
                else
                {
                    PdfData = jd.AddWaterMark(filename, duplicate, dprintJob.FromLocation, false, isTransferFlag, lastTask, isSplitTicket, txtReprintTicketWatermark.Text);
                }
            }
            catch (Exception exc)
            {

            }
            //PdfData = output;
            printerMsg.Text = string.Empty;
            btnReprintTicket_Click(null, null);
            string url = "FTpdf.aspx";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "newpage", "customOpen('" + url + "');", true);
        }

        protected void printTickets(string filename, string source)
        {
            var dpPrintDataAdapter = new DpPrintStatusDataAdapter();
            var jd = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);

            int siteID = Convert.ToInt32(ConfigurationManager.AppSettings["PrintSiteId"]);
            List<DpJobHistory> jobHistories = dpPrintDataAdapter.GetDpJobHistories(dprintJob.Id, siteID).ToList();

            int index = -1;
            int previousTaskIndex = -1;
            bool isTransferFlag = false;
            string lastTask = string.Empty;

            try
            {
                index = jobHistories.FindLastIndex(o => o.TaskId == 28);
                if (index == -1)
                    index = jobHistories.FindLastIndex(o => o.TaskId == 29);

                if (index != -1)
                {
                    isTransferFlag = true;
                    previousTaskIndex = index - 1;
                    lastTask = jobHistories[previousTaskIndex].DPrintTask;
                }
                else
                    isTransferFlag = false;
            }
            catch (Exception ex)
            {
                isTransferFlag = false;
            }

            bool isSplitTicket = false;
            string firstThree = Path.GetFileName(filename).Substring(0, 3).ToUpper();
            if (firstThree == "ST_")
                isSplitTicket = true;

            printerMsg.Text = string.Empty;

            try
            {
                string equipmentUsed = ddReprintTicketType.SelectedItem.Text.ToString();
                int duplicate = dpPrintDataAdapter.CheckTicketAlreadyPrinted(dprintJob.Id, dprintJob.PrintSiteId, equipmentUsed);
                string timeStemp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                DateTime startTime = DateTime.UtcNow;

                if (duplicate > 0)
                {
                    if (firstThree == "PT_")
                        jd.AddWaterMarkToFile(Path.Combine(source, filename), Path.Combine(source, "_T_" + timeStemp + filename), duplicate, dprintJob.FromLocation, true, false, lastTask, isSplitTicket, txtReprintTicketWatermark.Text);
                    else
                        jd.AddWaterMarkToFile(Path.Combine(source, filename), Path.Combine(source, "_T_" + timeStemp + filename), duplicate, dprintJob.FromLocation, true, isTransferFlag, lastTask, isSplitTicket, txtReprintTicketWatermark.Text);
                }
                else
                {
                    jd.AddWaterMarkToFile(Path.Combine(source, filename), Path.Combine(source, "_T_" + timeStemp + filename), duplicate, dprintJob.FromLocation, false, isTransferFlag, lastTask, isSplitTicket, txtReprintTicketWatermark.Text);
                }

                PDFUtil.SendPDFToPrinter(Path.Combine(source, "_T_" + timeStemp + filename), ddReprintTicketPrinters.Text, false, true);
                jd.Notifications('I', "#gui - Reprint Ticket Feature printed a document - Ticket is sent to the printer  " + ddReprintTicketPrinters.Text + " Ticket file -" + Path.Combine(source, "_T_" + timeStemp + filename));
                DpTrackingTask newTrackingTask = jd.buildTaskObject(dprintJob,
                                                                    22,
                                                                    startTime,
                                                                    DateTime.UtcNow,
                                                                    1,
                                                                    (DateTime.UtcNow - startTime).ToString(),
                                                                    0, true
                                                                    , WebSession.User.DisplayName, 99.99, "Green", Convert.ToString(ddReprintTicketType.SelectedItem));
                int newTaskId = jd.AddDprintTrackingTasks(newTrackingTask);
                jd.Notifications('I', "#gui - Reprint Ticket Feature printed a document - Ticket is sent to the printer  " + Convert.ToString(ddReprintTicketType.SelectedItem) + " Ticket file -" + Path.Combine(source, "_T_" + timeStemp + filename));
                printerMsg.Text = Convert.ToString(ddReprintTicketType.SelectedItem) + " is sent to the printer";
                if (File.Exists(Path.Combine(source, "_T_" + timeStemp + filename)))
                    File.Delete(Path.Combine(source, "_T_" + timeStemp + filename));
            }
            catch (Exception exc)
            {
                printerMsg.Text = "Error occurred while printing the ticket";
                jd.Notifications('E', "#gui - Reprint Ticket Feature - error while printing ticket for " + exc.InnerException + "|" + exc.Message + "|" + exc.Source + "|" + exc.ToString() + " Dp job id " + Convert.ToString(dprintJob.Id));
            }

            btnReprintTicket_Click(null, null);
        }

        protected void btnSelectSpoolFileOK_Click(object sender, EventArgs e)
        {
            var dir = new DirectoryInfo(Path.GetDirectoryName(ddSpoolFileList.SelectedValue));
            List<FileInfo> spoolFileInfo = dir.GetFiles(Path.GetFileName(ddSpoolFileList.SelectedValue)).ToList();

            reSendSpoolFile(spoolFileInfo);
            setTrackingTask(spoolFileInfo);
        }

        protected Dictionary<string, string> GetActiveSites()
        {
            SiteDataAdapter sd = new SiteDataAdapter();
            var sitesCollection = sd.GetAllActiveSites();
            Dictionary<string, string> sites = new Dictionary<string, string>();
            sites.Add("All", "0");
            foreach (var dpSite in sitesCollection)
                sites.Add(dpSite.Name, Convert.ToString(dpSite.Id));
            return sites;
        }

        protected void btnSelectEquipmentOK_Click(object sender, EventArgs e)
        {

            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            int dpLocalSiteID = Convert.ToInt32(ConfigurationManager.AppSettings["PrintSiteId"]);

            string equipmentName;

            if (ddEquipmentId.SelectedValue.ToString() != "N/A")
            {
                equipmentName = ddEquipmentId.SelectedItem.Text.Trim();
            }
            else
            {
                equipmentName = ddEquipmentList.SelectedItem.Text.Trim();
            }

            var inserterInfo = GetInserter(equipmentName, Int32.Parse(ddProcessingLocation.SelectedValue));
            bool IsvirtualInserter = jd.CheckForVertualInserterCategoryByInserterID(Convert.ToString(inserterInfo.InserterMasterID));
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);
            if ((txtEqPrintEngineOrCategory.Text.ToLower() == "virtual" || IsvirtualInserter == true) && dprintJob.StatusId == 17)
            {
                if (ddEquipmentId.SelectedValue.ToString() == "N/A" && ddEquipmentList.SelectedItem != null && ddEquipmentList.SelectedItem.Text != "" && int.Parse(ViewState["Duplicate"].ToString()) == 0)
                {
                    btnOpenVirtualInserter_Click(null, null);
                }
                else
                {
                    equipmentName = ddEquipmentIdProcessJob.SelectedValue.ToString();
                    checkForQtyPullsAfterPrint(Convert.ToString(inserterInfo.InserterMasterID), Convert.ToString(dprintJob.Id), 1);
                }
                ViewState["Duplicate"] = 0;
                return;
            }

            if (HiddenFieldAccountCheck.Value == "na")
            {
                ctlTimer.Interval = 30000;
                duplicateCheckWarning(null, null, equipmentName);
            }
            else
            {
                if (IsvirtualInserter)
                {
                    if (HiddenFieldAccountCheck.Value == "yes")
                    {
                        ctlTimer.Interval = 30000;
                        duplicateCheckWarning(null, null, equipmentName);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                        ModalAccountPullWarning.Show();
                    }
                }
                ctlTimer.Interval = 30000;
                duplicateCheckWarning(null, null, equipmentName);
            }

            ddEquipmentList.SelectedItem.Text = string.Empty;
            ddEquipmentList.DataSource = null;
            ddEquipmentList.DataBind();
        }


        protected void btnSelectEquipmentCancel_Click(object sender, EventArgs e)
        {

            ctlTimer.Interval = 30000;
        }

        protected void ddDpRoutingSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            if (Convert.ToString(jd.SystemConfiguration(Convert.ToInt32(ddDpRoutingSites.SelectedValue), "MegaMRDF")) == "1")
                cbMegaMrdf.Checked = true;
            else
                cbMegaMrdf.Checked = false;

            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            modalJobRouting.Show();
        }

        protected void btnViewInvItems_Click(object sender, EventArgs e)
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);
            List<Inventory> items = new List<Inventory>();
            items = jd.GetJobInventory(dprintJob.FromSiteID, dprintJob.Job, dprintJob.Product, dprintJob.Split, dprintJob.PrintSiteId);// (1, 922948, "9AMG1", 1,8);// (fromSiteID, jobNumber, productNumber, splitNumber,printSiteId);
            gvInventory.DataSource = items;
            gvInventory.DataBind();

            cbCurrentSiteInstruction.Checked = true;
            List<JobInstructions> specificInstuctionsList = new List<JobInstructions>();
            specificInstuctionsList = jd.GetJobInstructionsForActionMenu(dprintJob.FromSiteID, dprintJob.PrintSiteId, dprintJob.Job, dprintJob.ClientNumber, dprintJob.Product, Convert.ToInt32(ddStatus.SelectedValue), cbCurrentSiteInstruction.Checked, ddEquipmentId.SelectedValue);
            gvSpecificInstructions.DataSource = specificInstuctionsList;
            gvSpecificInstructions.DataBind();

            litInvJobNumber.Text = Convert.ToString(dprintJob.Job);
            litInvProduct.Text = dprintJob.Product;
            litInvSplitNumber.Text = Convert.ToString(dprintJob.Split);
            litInvCustomer.Text = dprintJob.ClientName;


            ctlTimer.Interval = 1000 * 60 * 5;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            modalInventory.Show();
        }

        protected void cbCurrentSiteInstruction_CheckedChanged(object sender, EventArgs e)
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);
            List<Inventory> items = new List<Inventory>();
            items = jd.GetJobInventory(dprintJob.FromSiteID, dprintJob.Job, dprintJob.Product, dprintJob.Split, dprintJob.PrintSiteId);// (1, 922948, "9AMG1", 1,8);// (fromSiteID, jobNumber, productNumber, splitNumber,printSiteId);
            gvInventory.DataSource = items;
            gvInventory.DataBind();

            List<JobInstructions> specificInstuctionsList = new List<JobInstructions>();
            specificInstuctionsList = jd.GetJobInstructionsForActionMenu(dprintJob.FromSiteID, dprintJob.PrintSiteId, dprintJob.Job, dprintJob.ClientNumber, dprintJob.Product, Convert.ToInt32(ddStatus.SelectedValue), cbCurrentSiteInstruction.Checked, ddEquipmentId.SelectedValue);
            gvSpecificInstructions.DataSource = specificInstuctionsList;
            gvSpecificInstructions.DataBind();

            litInvJobNumber.Text = Convert.ToString(dprintJob.Job);
            litInvProduct.Text = dprintJob.Product;
            litInvSplitNumber.Text = Convert.ToString(dprintJob.Split);
            litInvCustomer.Text = dprintJob.ClientName;

            ctlTimer.Interval = 1000 * 60 * 5;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            modalInventory.Show();
        }
        private void WriteTraceLog(string eventName)
        {
            string currentStatus = GetCurrentStatus();
            var severity = ErrorLog.SeverityEnum.Info;
            string text = eventName + "  event called";
            string description = string.Empty;
            if (eventName == "BtnApplyStatus_Clicked" || eventName == "btnExpContinue_Click")
            {
                description = string.Format("by user/userid: {0}/{1} selected the status from {2} to {3} at {4}.",
               WebSession.User.DisplayName, WebSession.User.UserId, currentStatus, ddPopStatus.SelectedItem.Text, DateTime.Now);
            }
            else if (eventName == "btnYes_Click")
            {
                string nextStatus = GetNextStatus();
                description = string.Format("by user/userid: {0}/{1} updated the status from {2} to {3} at {4}.",
               WebSession.User.DisplayName, WebSession.User.UserId, currentStatus, nextStatus, DateTime.Now);
            }
            else
            {
                description = string.Format("by user/userid: {0}/{1} selected the current job status as {2} at {3}.",
               WebSession.User.DisplayName, WebSession.User.UserId, currentStatus, DateTime.Now);
            }
            ErrorLog.LogError(severity, description, text, "N/A");
        }

        private string GetNextStatus()
        {
            string nextStatus = string.Empty;
            foreach (GridViewRow row in gvPopUpWindows.Rows)
            {
                if (((CheckBox)row.FindControl("cbDataSelected")).Checked)
                {
                    nextStatus = ((Label)row.FindControl("Label12")).Text;
                    break;
                }
            }
            return nextStatus;
        }

        private string GetCurrentStatus()
        {
            string currentStatus = string.Empty;
            foreach (GridViewRow row in gvPopUpWindows.Rows)
            {
                if (((CheckBox)row.FindControl("cbDataSelected")).Checked)
                {
                    currentStatus = ((Label)row.FindControl("Label11")).Text;
                    break;
                }
            }
            return currentStatus;
        }

        protected void ddEquipmentId_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtInputPaperModulesJob.Text = string.Empty;
            txtTonerTypeJob.Text = string.Empty;
            txtPrintEngineNgOrSheetCodeJob.Text = string.Empty;

            labInputPaperModulesJob.Text = string.Empty;
            labTonerTypeJob.Text = string.Empty;
            labPrintEngineNgOrSheetCodeJob.Text = string.Empty;

            var selectedStatusId = ddStatus.SelectedValue;
            setEquipmentAtr(selectedStatusId);
        }
        protected void btnSaveLocalSetting_Clicked(object sender, EventArgs e)
        {
            txtUserName.Text = WebSession.User.LoginId.ToString();
            ModalPopUpUser.Show();

        }

        protected void btnUserContinue_Click(object sender, EventArgs e)
        {
            string userId = string.Empty;
            var jd = new JobProcessingDataAdapter();
            UserDeviceToEquipment userDeviceToEquipment = new UserDeviceToEquipment();
            userDeviceToEquipment.LoginId = txtUserName.Text;
            userDeviceToEquipment.SiteID = int.Parse(ddProcessingLocation.SelectedValue);
            userDeviceToEquipment.DPrintTaskMasterID = int.Parse(ddStatus.SelectedValue);
            userDeviceToEquipment.Equipment = ddEquipmentId.SelectedValue.ToString();
            userDeviceToEquipment.AddedBy = WebSession.User.DisplayName;
            userDeviceToEquipment.UpdatedBy = WebSession.User.DisplayName;
            userDeviceToEquipment.Case = 3;
            bool flag = jd.CheckUserLoginID(userDeviceToEquipment);
            if (flag)
            {
                userId = txtUserName.Text;
                SaveLocalSetting(userId);
                lblUserMessage.Text = "";

            }
            else
            {
                txtUserName.Text = "";
                lblUserMessage.Text = "Invalid User";
                ModalPopUpUser.Show();
            }
        }

        protected void btnUserCancel_Click(object sender, EventArgs e)
        {
            ModalPopUpUser.Hide();
        }
        private void SaveLocalSetting(string useId)
        {
            var jd = new JobProcessingDataAdapter();
            UserDeviceToEquipment userDeviceToEquipment = new UserDeviceToEquipment();
            userDeviceToEquipment.SiteID = int.Parse(ddProcessingLocation.SelectedValue);
            userDeviceToEquipment.LoginId = useId == "" ? WebSession.User.LoginId : useId;
            userDeviceToEquipment.DPrintTaskMasterID = int.Parse(ddStatus.SelectedValue);
            userDeviceToEquipment.Equipment = ddEquipmentId.SelectedItem.Text.ToString();
            userDeviceToEquipment.AddedBy = WebSession.User.DisplayName;
            userDeviceToEquipment.UpdatedBy = WebSession.User.DisplayName;
            userDeviceToEquipment.Case = 1;
            int i = jd.SaveUpdateuserDeviceToEquipment(userDeviceToEquipment);
            ModalPopUpUser.Hide();
            if (i > 0)
            {
                ModalpopuppnlMessage.Show();
            }
            else
            {
                ModalpopupexPanelErrorMessage.Show();
            }
        }
        protected void btnMessageCancel_Click(object sender, EventArgs e)
        {
            ModalpopuppnlMessage.Hide();
        }
        protected void btnErrorMessageCancel_Click(object sender, EventArgs e)
        {
            ModalpopupexPanelErrorMessage.Hide();
        }
        protected void btnRetrieveLocalSetting_Clicked(object sender, EventArgs e)
        {

            var jd = new JobProcessingDataAdapter();
            UserDeviceToEquipment userDeviceToEquipment = new UserDeviceToEquipment();
            userDeviceToEquipment.SiteID = int.Parse(ddProcessingLocation.SelectedValue); ;
            userDeviceToEquipment.LoginId = WebSession.User.LoginId;
            userDeviceToEquipment.DPrintTaskMasterID = int.Parse(ddStatus.SelectedValue);
            userDeviceToEquipment.Equipment = ddEquipmentId.SelectedItem.Text.ToString();
            userDeviceToEquipment.AddedBy = WebSession.User.DisplayName;
            userDeviceToEquipment.UpdatedBy = WebSession.User.DisplayName;
            userDeviceToEquipment.Case = 2;

            List<UserDeviceToEquipment> lstUserDeviceToEquipment = jd.FetchUserDeviceToEquipment(userDeviceToEquipment);

            ViewState["UserDeviceToEquipment"] = lstUserDeviceToEquipment;
            if (lstUserDeviceToEquipment.Count == 0)
            {
                if ((WebSession.User.HasPermissionExplicit("JobProcessing.LeadOperator") || WebSession.User.HasPermissionExplicit("JobProcessing.Operator")) && sender == null && e == null)
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowMessage('This Machine has not been Configured for an Operator. Please contact your Lead, Supervisor or Manager to do this one - time Setup.');", true);
                }

            }
            if (lstUserDeviceToEquipment.Count == 1)
            {
                btnProcessingStepContinue_Click(null, null);
            }
            else if (lstUserDeviceToEquipment.Count > 1)
            {

                ddlProcessingStep.DataSource = lstUserDeviceToEquipment;
                ddlProcessingStep.DataTextField = "DPrintTaskEquipmentName";
                ddlProcessingStep.DataValueField = "DPrintTaskEquipment";
                ddlProcessingStep.DataBind();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                ModalpopuProcessingStep.Show();
            }
        }
        protected void btnProcessingStepContinue_Click(object sender, EventArgs e)
        {


            var jd = new JobProcessingDataAdapter();

            List<UserDeviceToEquipment> lstUserDeviceToEquipment = (List<UserDeviceToEquipment>)ViewState["UserDeviceToEquipment"];

            if (lstUserDeviceToEquipment.Count > 1)
            {
                string[] ProcessingStep = ddlProcessingStep.SelectedValue.ToString().Split('~');
                lstUserDeviceToEquipment = lstUserDeviceToEquipment.Where(x => x.DPrintTaskMasterID == int.Parse(ProcessingStep[0]) && x.Equipment == ProcessingStep[1]).ToList();
            }
            if (lstUserDeviceToEquipment.Count > 0)
            {
                if (lstUserDeviceToEquipment.Count > 1 && ddlProcessingStep.SelectedValue != "")
                {
                    string[] ProcessingStep = ddlProcessingStep.SelectedValue.ToString().Split('~');
                    if (ddStatus.SelectedValue != ProcessingStep[0].ToString())
                        GridViewRefresh();
                }
                else if (lstUserDeviceToEquipment.Count == 1)
                {
                    if (ddStatus.SelectedValue != lstUserDeviceToEquipment[0].DPrintTaskMasterID.ToString())
                        GridViewRefresh();
                }
            }
            foreach (var UserDeviceToEquipment in lstUserDeviceToEquipment)
            {

                ddStatus.SelectedValue = UserDeviceToEquipment.DPrintTaskMasterID.ToString();
                var selectedStatusId = UserDeviceToEquipment.DPrintTaskMasterID.ToString();
                ddEquipmentId.Items.Clear();
                Dictionary<string, string> equipmentIdDict = new Dictionary<string, string>();
                if (UserDeviceToEquipment.DPrintTaskMasterID == 15 || UserDeviceToEquipment.DPrintTaskMasterID == 17)
                {
                    EquipmentIdDataAdapter equipmentIdDataAdapter = new EquipmentIdDataAdapter();
                    Collection<EquipmentId> equipmentIdCollection = equipmentIdDataAdapter.GetEquipmentIdByTask(Int32.Parse(selectedStatusId), Int32.Parse(UserDeviceToEquipment.SiteID.ToString()));

                    if (equipmentIdCollection.Count > 0)
                    {
                        foreach (var equipmentId in equipmentIdCollection)
                        {
                            ddEquipmentId.Items.Add(new ListItem(equipmentId.EquipmentName, equipmentId.Id));
                        }
                        setEquipmentAtr(selectedStatusId);
                    }
                    else
                    {
                        ddEquipmentId.Items.Add(new ListItem("N/A", "N/A"));
                    }
                }
                else
                {
                    ddEquipmentId.Items.Add(new ListItem("N/A", "N/A"));
                }

                if (UserDeviceToEquipment.Equipment != null)
                {
                    ddEquipmentId.ClearSelection();
                    ddEquipmentId.Items.FindByText(UserDeviceToEquipment.Equipment.ToString()).Selected = true;
                }

                ddProcessingLocation.SelectedValue = UserDeviceToEquipment.SiteID.ToString();

                if (!WebSession.User.HasPermissionExplicit("JobProcessing.Operator"))
                {
                    SaveLocalSettingEnableDesable();
                }
                if (WebSession.User.HasPermissionExplicit("JobProcessing.Operator"))
                {
                    btnSaveLocalSetting.Enabled = false;
                }

                labStatus1.Text = ddStatus.SelectedItem.Text;
                labStatus2.Text = ddStatus.SelectedItem.Text;
                labStatus3.Text = ddStatus.SelectedItem.Text;
                setEquipmentAtr(UserDeviceToEquipment.DPrintTaskMasterID.ToString());
            }

        }

        private void GridViewRefresh()
        {
            gvDpJobs.DataSource = null;
            gvDpJobs.DataBind();
        }

        public void SaveLocalSettingEnableDesable()
        {
            var selectedStatusId = ddStatus.SelectedValue;
            if (ddProcessingLocation.SelectedValue == "0" || ddStatus.SelectedValue == "0")
            {
                btnSaveLocalSetting.Enabled = false;
            }
            else if (WebSession.User.HasPermissionExplicit("JobProcessing.Operator"))
            {
                btnSaveLocalSetting.Enabled = false;
            }
            else if (ddEquipmentId.SelectedValue.ToString().ToUpper() == "N/A" && selectedStatusId.Equals("20"))
            {
                btnSaveLocalSetting.Enabled = true;
            }
            else { btnSaveLocalSetting.Enabled = true; }
        }

        protected void btnProcessingStepCancel_Click(object sender, EventArgs e)
        {
            ModalpopuProcessingStep.Hide();
        }


        private void AddCommandQueueForEmail(JobProcessingModel dprintJob, string equipmentName, string traceMessage, string filesnames)
        {
            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            jd.Notifications('I', "#" + traceMessage);
            commandQueue(filesnames, "", "email", "", "", string.Empty, false, dprintJob, equipmentName);
        }

        /// <summary>
        /// This method is used to fetch default or site specfic configration
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="variableName"></param>
        /// <returns></returns>
        private DPSysConfig FetchConfigrationForSite(int siteId, string variableName)
        {
            JobProcessingDataAdapter jp = new JobProcessingDataAdapter();
            DPSysConfig dpSysConfig = jp.FetchConfigrationForSite(siteId, variableName);
            return dpSysConfig;
        }

        /// <summary>
        /// US21435- Move Container file when Job is Completed
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="jd"></param>
        /// <param name="equipmentName"></param>
        /// <param name="siteID"></param>
        /// <param name="i"></param>
        private void ContainerFileMovement(StringBuilder msg, JobProcessingDataAdapter jd, string equipmentName, int siteID, JobProcessingModel item, int jobStatusCode)
        {
            if (jobStatusCode == 20)
            {
                var sourceContainerFileFolder = FetchConfigrationForSite(siteID, "ContainerLandingFolderForDataFromSite");
                var destinationContainerFileFolder = FetchConfigrationForSite(siteID, "ContainerCompleteFolder");

                //var item = DpSelectedJobSession[i];
                string containerFileName;
                string product = item.Product.Substring(item.Product.Length - 5, item.Product.Length);
                string dprintJobId = Convert.ToString(item.Id);
                string temp_job = Convert.ToString(item.Job);
                string job = temp_job.Length > 8 ? temp_job.Substring(temp_job.Length - 8, temp_job.Length) : temp_job.PadLeft(8, '0');
                string split = item.Split.ToString("D7");
                string startingSeq = item.Sequences.Split('-')[0].PadLeft(9, '0');
                string EndingSeq = item.Sequences.Split('-')[1].PadLeft(9, '0');
                string temp_composerId = Convert.ToString(item.ComposerId);
                string composerId = temp_composerId.Length > 9 ? temp_composerId.Substring(temp_composerId.Length - 9, temp_composerId.Length) : temp_composerId.PadLeft(9, '0');
                //DPA1000A_1990934_001_0000055_3149279.zip
                containerFileName = string.Concat(sourceContainerFileFolder.DPVariableValue, "DPA", dprintJobId, "_", product, "_", job, "_",
                    split, "_", startingSeq, "_", EndingSeq, "_", composerId);

                var checkZip = File.Exists(containerFileName + ".zip");
                var check7z = File.Exists(containerFileName + ".7z");
                jd.Notifications('I', "#gui -job composer id - " + item.ComposerId.ToString() + " CheckZip-" + checkZip + " Check7z-" + check7z + " - generated file name-" + containerFileName + " destination " + destinationContainerFileFolder.DPVariableValue + ".");

                try
                {
                    //If zip file is present, move it
                    if (checkZip)
                    {
                        if (commandQueue(containerFileName + ".zip", destinationContainerFileFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, item, equipmentName, "FTP and "))
                        {
                            jd.Notifications('I', "#gui -job composer id - " + item.ComposerId.ToString() + " - move command is sent for zipped file-" + containerFileName + " to command queue for moving " + destinationContainerFileFolder.DPVariableValue + " folder.");
                        }
                        else
                        {
                            jd.Notifications('I', "#gui -job composer id - " + item.ComposerId.ToString() + " Error - move command is not added  for zipped file-" + containerFileName + " to " + destinationContainerFileFolder.DPVariableValue + " folder.Please check the MSMQ configuration");

                        }
                    }
                    //If 7z file is present, move it
                    if (check7z)
                    {
                        if (commandQueue(containerFileName + ".7z", destinationContainerFileFolder.DPVariableValue, "MOVE", "Xcopy", string.Empty, string.Empty, false, item, equipmentName, "FTP and "))
                        {
                            jd.Notifications('I', "#gui -job composer id - " + item.ComposerId.ToString() + " - move command is sent for 7z file-" + containerFileName + " to command queue for moving " + destinationContainerFileFolder.DPVariableValue + " folder.");
                        }
                        else
                        {
                            jd.Notifications('I', "#gui -job composer id - " + item.ComposerId.ToString() + " Error - move command is not added  for 7z file-" + containerFileName + " to " + destinationContainerFileFolder.DPVariableValue + " folder.Please check the MSMQ configuration");

                        }
                    }
                }
                catch (Exception ex)
                {

                    jd.Notifications('I', "##RetryThis - " + item.ComposerId.ToString() + " Error - move command is not added  for file-" + containerFileName + " to " + destinationContainerFileFolder.DPVariableValue + " folder.");


                }
            }
        }

        /// <summary>
        /// Generate the missing file name
        /// </summary>
        /// <returns></returns>
        private string GenerateFileName(JobProcessingModel item, string ticketType)
        {
            string[] startEndSeq = item.Sequences.Split('-');
            return ticketType + "_" + item.Product + "_" +
                                   item.Job.ToString("D8") + "_" +
                                   item.Split.ToString("D7") + "_" +
                                   startEndSeq[0].Trim().ToInt().ToString("D9") + "_" +
                                   startEndSeq[1].Trim().ToInt().ToString("D9") + "_" +
                                   item.ComposerId.ToString("D9") + ".pdf";
        }

        /// <summary>
        /// This method is use for redirect to Reprocess container pages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReprocessContainer_Click(object sender, EventArgs e)
        {
            string equipmentName = ddEquipmentIdProcessJob.SelectedItem.Text.ToString();
            //var index = 0;

            //foreach (GridViewRow row in gvPopUpWindows.Rows)
            //{
            //    if (((CheckBox)row.FindControl("cbDataSelected")).Checked)
            //    {
            //        selectedIndex = selectedIndex + "|" + (index);
            //    }
            //    index++;
            //}
            var returnURL = "JobProcessing.aspx";

            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

            Response.Redirect("ReprocessContainer.aspx?jobid=" + selectedDprintJobsId + "&siteid=" + siteID + "&equipmentName=" + equipmentName
                + "&returnURL=" + returnURL, false);
        }

        /// <summary>
        /// This method will attempt to rectify a problem by placing files in appropriate folders
        /// Author: Faheem Mir
        /// </summary>
        /// <param name="jd"></param>
        /// <param name="equipmentName"></param>
        /// <param name="siteID"></param>
        /// <param name="printerStagingFolder"></param>
        /// <param name="i"></param>
        /// <param name="spoolFile"></param>
        /// <param name="spoolFileForMail"></param>
        /// <returns></returns>
        private List<FileInfo> ReprocessContainerContent(JobProcessingDataAdapter jd, string equipmentName, int siteID, DPSysConfig printerStagingFolder, JobProcessingModel model, List<FileInfo> spoolFile, List<string> spoolFileForMail)
        {
            jd.Notifications('I', "## Attempting reloading process for Job:  " + model.Job);

            var initialCheck = ReprocessContainer.AccountDetailCheckForSelectedJob(model.Id, 1, siteID);
            jd.Notifications('I', "## Attempting reloading process for Job:  " + model.Job + " Initial Check Passed: " + initialCheck.isSuccessful
                + " Message: " + initialCheck.Message);

            ResponseInfo result;
            if (initialCheck.isSuccessful)
            {
                result = ReprocessContainer.ContinueReprocessingFiles(model.Id, 1, siteID, equipmentName);

                if (result.isSuccessful)
                {
                    spoolFile = GetSoolFileInfo(printerStagingFolder.DPVariableValue, model);

                }
            }

            //else if (!initialCheck.isSuccessful && initialCheck.Message.Contains("This job cannot be processed"))
            //{
            //    var fileList = "";
            //    if (spoolFileForMail.Count > 0)
            //        fileList = spoolFileForMail[0] + "," + spoolFileForMail[1];

            //    var additionalMetaData = string.Format("ClientName: {0} | Job Number: {1} | Product Number: {2} | Split Number: {3} <br> " +
            //        "Split Quantity: {4} |  Total Sheet Count: {5}  | Status: {6}  | Scheduled Mail Date: {7}",
            //        model.ClientName, model.Job, model.Product, model.Split, model.Quantity, model.TotalSheetCount, model.Status, model.ScheduledMailDate);

            //    jd.Notifications('I', "### Sending no dp container found email for Job:  " + model.Job + " MetaData: " + additionalMetaData);

            //    commandQueue(fileList, additionalMetaData, "ReprocessContainer", "", "", string.Empty, false, model, equipmentName);
            //}

            return spoolFile;
        }

        public void AddCommandQueueForReprocessContainerEmail(string filenames, string metaData, string commandType, JobProcessingModel item, string equipmentName)
        {
            commandQueue(filenames, metaData, commandType, "", "", string.Empty, false, item, equipmentName);

        }

        /// <summary>
        /// This method will be used to navigate to Job Diagnostic page from action menu
        /// Author:Faheem Mir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnJobDiagnostic_Click(object sender, EventArgs e)
        {
            string equipmentName = ddEquipmentId.SelectedItem.Text.ToString();
            var returnURL = "JobProcessing.aspx";

            var ddLocation = (DropDownList)this.Master.FindControl("ddlLocation");
            int siteID = Convert.ToInt32(ddLocation.SelectedValue);

            Response.Redirect("ReprocessContainer.aspx?jobid=" + selectedDprintJobsId + "&siteid=" + siteID + "&equipmentName=" + equipmentName
                + "&returnURL=" + returnURL, false);

        }
        /// <summary>
        /// update following field of manual job 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnJobCompletionsApplyStatus_Click(object sender, EventArgs e)
        {
            ManualjobCompletions manualjobCompletions = new ManualjobCompletions();
            manualjobCompletions.CompletedOn = DateTime.Parse(txtSelectCompletionDate.Text + " " + txtTime.Text);
            manualjobCompletions.CompletedOnDate = lblCompletedOn.Text;
            manualjobCompletions.ExceptionStatus = txtNote.Text;
            manualjobCompletions.JobComposerID = (int)Session["JobComposerId"];
            manualjobCompletions.InsertedOn = chkInsertedOn.Checked == true ? 1 : 0;
            manualjobCompletions.InsertionCompletedOn = DateTime.Parse(txtSelectCompletionDate.Text + " " + txtTime.Text);
            manualjobCompletions.InsertionCompletedOnDate = lblInsertionCompletedOn.Text;

            JobProcessingDataAdapter jp = new JobProcessingDataAdapter();
            int i = jp.UpdateJobCompletionsApplyStatus(manualjobCompletions);

            JobProcessingDataAdapter jd = new JobProcessingDataAdapter();
            JobProcessingModel dprintJob = jd.GetDPrintJobById(selectedDprintJobsId);
            var tempId = jd.InsertDprintTrackingTasks(dprintJob, WebSession.User.DisplayName, 21, txtNote.Text);


            string DestinationContainerFileFolder = GetCompleteFolder("PrintingComplete");
            //DestinationContainerFileFolder = @"\\Bhtdps02\NCP\Jobs\PrintingComplete\";
            FileMoveInPrintingCompletedFolder(DestinationContainerFileFolder, dprintJob);

            DestinationContainerFileFolder = GetCompleteFolder("MRDFComplete");
            //DestinationContainerFileFolder = @"\\Bhtdps02\NCP\Jobs\MRDFComplete\";
            FileMoveInMRDFCompletedFolder(DestinationContainerFileFolder, dprintJob);

             DestinationContainerFileFolder = GetCompleteFolder("ContainerCompleteFolder");
            //DestinationContainerFileFolder = @"\\Bhtdps02\NCP\Jobs\ContainerComplete\";
            FileMoveInContainerCompletedFolder(DestinationContainerFileFolder, dprintJob);
            
        }

        private void FileMoveInPrintingCompletedFolder(string DestinationContainerFileFolder, JobProcessingModel item)
        {
            string SourceContainerFileFolder = GetCompleteFolder("SpoolFileStaging");
            //SourceContainerFileFolder = @"\\Bhtdps02\NCP\Jobs\PrinterStaging\";
            //SurceContainerFileFolder = @"\\Bhtdps02\NCP\Jobs\PrinterStaging\ContainerStaging\";
            //SurceContainerFileFolder = @"\\Bhtdps02\NCP\Jobs\PrintingInProcess\";
            //SurceContainerFileFolder = @"\\Bhtdps02\NCP\Jobs\PrintingComplete\";
            var spoolFile = GetSoolFileInfo(SourceContainerFileFolder, item);

            // we need to remove line item
            if (!spoolFile.Any())
            {
                SourceContainerFileFolder = SourceContainerFileFolder + "ContainerStaging\\";
                spoolFile = GetSoolFileInfo(SourceContainerFileFolder, item);
            }
            // end we need to remove line item
            if (!spoolFile.Any())
            {
                SourceContainerFileFolder = GetCompleteFolder("PrintingInProcess");
                spoolFile = GetSoolFileInfo(SourceContainerFileFolder, item);
            }
            if (spoolFile.Any())
            {
                foreach (FileInfo sf in spoolFile)
                {
                    string sourceFile = Path.Combine(SourceContainerFileFolder, sf.FullName);
                    string DestinationContainerFilePath = Path.Combine(DestinationContainerFileFolder, sf.Name);
                    if (Directory.Exists(DestinationContainerFileFolder))
                    {                        
                        File.Move(sourceFile, DestinationContainerFilePath);
                    }
                }

            }

        }

        private void FileMoveInMRDFCompletedFolder(string DestinationContainerFileFolder, JobProcessingModel item)
        {
            var fileNameC = GenerateFileNameAsPerNamingConventionC(item);

            FileInformation fileInformation = ScanAndGetFileDetailsForNamingConventionC(int.Parse(SiteIdSession), fileNameC, "MRDFStaging");

            if (string.IsNullOrEmpty(fileInformation.FileName))
            {
                fileInformation = ScanAndGetFileDetailsForNamingConventionC(int.Parse(SiteIdSession), fileNameC, "MRDFInProcess");
                //fileInformation.FileLocation = @"\\Bhtdps02\NCP\Jobs\MRDFInProcess\";
            }

            if (!string.IsNullOrEmpty(fileInformation.FileName))
            {
                //fileInformation.FileLocation = @"\\Bhtdps02\NCP\Jobs\MRDFStaging\";
               
                string sourceFile = Path.Combine(fileInformation.FileLocation, fileInformation.FileName);
                string DestinationContainerFilePath = Path.Combine(DestinationContainerFileFolder, fileInformation.FileName);
                if (Directory.Exists(DestinationContainerFileFolder))
                {
                    File.Move(sourceFile, DestinationContainerFilePath);
                }


            }
        }


        private void FileMoveInContainerCompletedFolder(string DestinationContainerFileFolder, JobProcessingModel item)
        {
            var fileNameA = GenerateFileNameAsPerNamingConventionA(item);
            var fileInformation = ScanAndGetFileDetailsForNamingConventionA(int.Parse(SiteIdSession), fileNameA, "ContainerLandingFolderForDataFromSite");
            if (!string.IsNullOrEmpty(fileInformation.FileName))
            {
                //fileInformation.FileLocation = @"\\Bhtdps02\NCP\Jobs\ContainerStaging\";
                string sourceFile = Path.Combine(fileInformation.FileLocation, fileInformation.FileName);
                string DestinationContainerFilePath = Path.Combine(DestinationContainerFileFolder, fileInformation.FileName);
                if (Directory.Exists(DestinationContainerFileFolder))
                {
                    
                    File.Move(sourceFile, DestinationContainerFilePath);
                }

            }
        }


        public string GenerateFileNameAsPerNamingConventionA(JobProcessingModel item)
        {

            string containerFileName;
            string product = item.Product.Substring(item.Product.Length - 5, item.Product.Length);
            string dprintJobId = Convert.ToString(item.Id);
            string temp_job = Convert.ToString(item.Job);
            string job = temp_job.Length > 8 ? temp_job.Substring(temp_job.Length - 8, temp_job.Length) : temp_job.PadLeft(8, '0');
            string split = item.Split.ToString("D7");
            string startingSeq = item.Sequences.Split('-')[0].PadLeft(9, '0');
            string EndingSeq = item.Sequences.Split('-')[1].PadLeft(9, '0');
            string temp_composerId = Convert.ToString(item.ComposerId);
            string composerId = temp_composerId.Length > 9 ? temp_composerId.Substring(temp_composerId.Length - 9, temp_composerId.Length) : temp_composerId.PadLeft(9, '0');           
            containerFileName = string.Concat("DPA", dprintJobId, "_", product, "_", job, "_",
                split, "_", startingSeq, "_", EndingSeq, "_", composerId);

            return containerFileName;

        }
        private FileInformation ScanAndGetFileDetailsForNamingConventionA(int siteId, string fileNameA, string scanLocation)
        {
            JobProcessingDataAdapter jobProcessingAdapter = new JobProcessingDataAdapter();
            var fileDetails = new FileInformation();

            var sourceContainerFileFolder = jobProcessingAdapter.FetchConfigrationForSite(siteId, scanLocation).DPVariableValue;
            
            //sourceContainerFileFolder = @"\\Bhtdps02\NCP\Jobs\ContainerStaging\";
            fileDetails.FileLocation = sourceContainerFileFolder;


            var checkZip = File.Exists(string.Concat(sourceContainerFileFolder, fileNameA) + ".zip");

            var check7z = File.Exists(string.Concat(sourceContainerFileFolder, fileNameA) + ".7z");

            string fileType = "";

            if (checkZip)
            {
                fileType = ".zip";
                fileDetails.isFilePresent = true;
                fileDetails.FileExtention = fileType;
            }
            else if (check7z)
            {
                fileType = ".7z";
                fileDetails.isFilePresent = true;
                fileDetails.FileExtention = fileType;
            }
            else
            {
                fileDetails.isFilePresent = false;
            }


            if (fileDetails.isFilePresent)
            {

                FileInfo fileInfo = new FileInfo(string.Concat(sourceContainerFileFolder, fileNameA) + fileType);
                fileDetails.FileName = fileNameA + fileDetails.FileExtention;
            }

            return fileDetails;
        }

        private FileInformation ScanAndGetFileDetailsForNamingConventionC(int siteId, string fileNameC, string scanLocation)
        {
            JobProcessingDataAdapter jobProcessingAdapter = new JobProcessingDataAdapter();
            var fileDetails = new FileInformation();

            var sourceContainerFileFolder = jobProcessingAdapter.FetchConfigrationForSite(siteId, scanLocation).DPVariableValue;
            fileDetails.FileLocation = sourceContainerFileFolder;

            //sourceContainerFileFolder= @"\\Bhtdps02\NCP\Jobs\MRDFStaging\";
            var checkNCP = File.Exists(string.Concat(string.Concat(sourceContainerFileFolder, "\\"), fileNameC) + ".ncp");
            var checkOut = File.Exists(string.Concat(string.Concat(sourceContainerFileFolder, "\\"), fileNameC) + ".out");
            var checkSel = File.Exists(string.Concat(string.Concat(sourceContainerFileFolder, "\\"), fileNameC) + ".sel");

            // string fileType = "";

            //TODO:Check all files, multiple can exist
            if (checkNCP)
            {
                fileDetails.FileExtention = ".ncp";
                fileDetails.isFilePresent = true;
            }
            else if (checkOut)
            {
                fileDetails.FileExtention = ".out";
                fileDetails.isFilePresent = true;
            }
            else if (checkSel)
            {
                fileDetails.FileExtention = ".sel";
                fileDetails.isFilePresent = true;
            }
            else
            {
                fileDetails.isFilePresent = false;
            }


            if (fileDetails.isFilePresent)
            {

                FileInfo fileInfo = new FileInfo(string.Concat(string.Concat(sourceContainerFileFolder, "\\"), fileNameC) + fileDetails.FileExtention);
                fileDetails.FileName = fileNameC + fileDetails.FileExtention;
            }

            return fileDetails;
        }
        public string GenerateFileNameAsPerNamingConventionC(JobProcessingModel item)
        {

            string containerFileName;
            string temp_job = Convert.ToString(item.Job);
            string job = temp_job.Length > 7 ? temp_job.Substring(temp_job.Length - 7, temp_job.Length) : temp_job.PadLeft(7, '0');
            string split = item.Split.ToString("D3");
            string EndingSeq = item.Sequences.Split('-')[1].PadLeft(7, '0');
            string temp_composerId = Convert.ToString(item.ComposerId);
            string composerId = temp_composerId.Length > 7 ? temp_composerId.Substring(temp_composerId.Length - 7, temp_composerId.Length) : temp_composerId.PadLeft(7, '0');

            containerFileName = string.Concat(item.Product, "_", job, "_", split, "_", EndingSeq, "_", composerId);

            return containerFileName;

        }

        /// <summary>
        /// fethc the folder path
        /// </summary>
        private String GetCompleteFolder(string variableName)
        {

            JobProcessingDataAdapter jp = new JobProcessingDataAdapter();
            string dpSysConfigPath = jp.GetCompleteFolder(SiteIdSession, variableName);
            return dpSysConfigPath;
        }

        protected void btnJobCompletionsCancel_Click(object sender, EventArgs e)
        {
            chkInsertedOn.Checked = false;
            txtNote.Text = "";
            txtSelectCompletionDate.Text = "";
            ModalPopUpManualJobCompletions.Hide();
            ModalJobStatusSplit.Show();
        }

        protected void txtSelectCompletionDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (lblInsertionCompletedOn.Text.Trim() != "")
                {
                    DateTime SelectCompletionDate = DateTime.Parse(txtSelectCompletionDate.Text);
                    DateTime InsertionCompletedOn = DateTime.Parse(lblInsertionCompletedOn.Text);
                    int result = DateTime.Compare(SelectCompletionDate, InsertionCompletedOn);
                    if (result < 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "showDateValidationMessage('Are you sure?.')", true);
                        lblInsertionCompletedOn.ForeColor = Color.Red;
                        //AlertBox('Are you sure?.');
                    }
                    else
                    {
                        lblInsertionCompletedOn.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "showDateValidationMessage('Please enter a valid Date and Time.');", true);
            }


            if (txtSelectCompletionDate.Text.Trim() == "" || txtNote.Text.Trim() == "")
            {
                btnJobCompletionsApplyStatus.Enabled = false;
            }
            else
            {
                btnJobCompletionsApplyStatus.Enabled = true;
            }
            //txtTime.Text= time;
            ModalPopUpManualJobCompletions.Show();
        }
        protected void txtNote_TextChanged(object sender, EventArgs e)
        {
            if (txtSelectCompletionDate.Text.Trim() == "" || txtNote.Text.Trim() == "")
            {
                btnJobCompletionsApplyStatus.Enabled = false;
            }
            else
            {
                btnJobCompletionsApplyStatus.Enabled = true;
            }
            ModalPopUpManualJobCompletions.Show();
        }
    }
}