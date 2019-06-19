using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.UDA;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Controls.Common;
using System.Collections;


namespace FanHai.Hemera.Addins.EDC
{
    /// <summary>
    /// 显示参数组管理的用户控件类。
    /// </summary>
    public partial class EDCManageCtl : BaseUserCtrl
    {
        //Define delegate manager control state
        public new delegate void AfterStateChanged(ControlState controlState);
        //Define event change control state
        public new AfterStateChanged afterStateChanged = null;
        //Define and initialize control state
        private ControlState _ctrlState = ControlState.Empty;
        private EdcManage edcManage = null;

        //Control state property
        public ControlState CtrlState
        {
            get
            {
                return _ctrlState;
            }
            set
            {
                _ctrlState = value;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        /// <summary>
        /// Construct function
        /// </summary>
        public EDCManageCtl(EdcManage edc)
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);

            _edc = edc;

            if (null == _edc || _edc.EdcName.Length < 1)
            {
                CtrlState = ControlState.New;
                FormatParamData();
            }
            else
            {
                if (_edc.Status == EntityStatus.InActive)
                    CtrlState = ControlState.Edit;
                else
                    CtrlState = ControlState.ReadOnly;

                MapEdcAndParamToControls(_edc);
            }
        }

        /// <summary>
        /// Control state change method
        /// </summary>
        /// <param name="state">Control state</param>
        private void OnAfterStateChanged(ControlState state)
        {
            switch (state)
            {
                #region case state of editer
                case ControlState.Edit:
                    txtEdcName.Properties.ReadOnly = true;
                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = true;
                    toolbarStatus.Enabled = true;

                    mainParamTable.Clear();
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    txtEdcName.Properties.ReadOnly = false;
                    txtEdcVersion.Properties.ReadOnly = false;
                    mmEdcDescription.Properties.ReadOnly = false;

                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;

                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = false;

                    mainParamTable.Clear();
                    _edc = new EdcManage();
                    MapEdcAndParamToControls(_edc);

                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:
                    txtEdcName.Properties.ReadOnly = true;
                    txtEdcVersion.Properties.ReadOnly = true;
                    mmEdcDescription.Properties.ReadOnly = true;

                    btnAdd.Enabled = false;
                    btnDelete.Enabled = false;

                    toolbarSave.Enabled = false;
                    toolbarDelete.Enabled = false;
                    break;
                #endregion
            }
            toolbarNew.Enabled = true;
            toolbarQuery.Enabled = true;
        }

        #region Validation & Set Controls Data To Param
        /// <summary>
        /// Validation & Collection Data for Sales Order
        /// </summary>
        private void MapControlsToEdcAndParam()
        {
            if (null == _edc)
            {
                throw (new Exception("Error Param Set"));
            }

            // TODO: Data validation
            _edc.EdcName = txtEdcName.Text;
            _edc.EdcVersion = txtEdcVersion.Text;
            _edc.EdcDescription = mmEdcDescription.Text;
            _edc.ParamTable = mainParamTable;
        }
        #endregion

        #region Validation & Set Param Data To Controls
        /// <summary>
        /// Set Sales Order Item data to Controls
        /// </summary>
        private void MapEdcAndParamToControls(EdcManage edcManage)
        {
            txtEdcName.Text = edcManage.EdcName;
            txtEdcVersion.Text = edcManage.EdcVersion;
            mmEdcDescription.Text = edcManage.EdcDescription;

            gcMainParam.MainView = gvMainParam;
            gcMainParam.DataSource = edcManage.ParamTable;
        }
        #endregion

        /// <summary>
        /// Tool Bar New Click
        /// </summary>
        private void toolbarNew_Click(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCManageViewContent}"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    EDCManageCtl ctrl = (EDCManageCtl)viewContent.Control.Controls.Find("EDCManageCtl", true)[0];
                    if (ctrl.txtEdcName.Text.Trim() != "")
                    {
                        if (MessageBox.Show(StringParser.Parse("${res:Global.ClearNoteMessage}"),
                            StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        {
                            return;
                        }
                    }

                    ctrl.CtrlState = ControlState.New;
                    return;
                }
            }

            EDCManageViewContent edcManageContent = new EDCManageViewContent(new EdcManage());
            WorkbenchSingleton.Workbench.ShowView(edcManageContent);
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
        }

        /// <summary>
        /// Tool Bar Save Click
        /// </summary>
        private void toolbarSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.EnsureSaveCurrentData}", "${res:Global.SystemInfo}"))
            {
                MapControlsToEdcAndParam();

                if (_edc.EdcName == string.Empty)
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:Global.NameNotNullMessage}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }

                bool bNew = (CtrlState == ControlState.New);

                if (bNew && !_edc.EdcNameValidate())
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:Global.NameIsExistMessage}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }

                if (_edc.Save(bNew))
                {
                    WorkbenchSingleton.Workbench.ActiveViewContent.TitleName =
                        StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCManageViewContent}") + "_" + _edc.EdcName;
                    CtrlState = ControlState.Edit;
                }
            }
        }

        /// <summary>
        /// Tool Bar Delete Click
        /// </summary>
        private void toolbarDelete_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.DeleteNoteMessage}","${res:Global.SystemInfo}"))
            {
                if (_edc.Delete())
                {
                    foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                    {
                        if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCManageViewContent}"))
                        {
                            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
                            viewContent.WorkbenchWindow.SelectWindow();
                            return;
                        }
                    }

                    WorkbenchSingleton.Workbench.ActiveViewContent.TitleName =
                        StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCManageViewContent}");
                    CtrlState = ControlState.New;
                }
            }
        }

        /// <summary>
        /// Tool Bar Query Click
        /// </summary>
        private void toolbarQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable edcHashTable = new Hashtable();
                DataSet dataSet = new DataSet();

                string edcName = this.txtEDC.Text.Trim();

                if (edcName.Length > 0)
                {
                    edcHashTable.Add(EDC_MAIN_FIELDS.FIELD_EDC_NAME, edcName);
                }

                DataTable edcDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(edcHashTable);
                edcDataTable.TableName = EDC_MAIN_FIELDS.DATABASE_TABLE_NAME;
                dataSet.Tables.Add(edcDataTable);

                string msg = string.Empty;
                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    dsReturn = serverFactory.CreateIEDCEngine().SearchEdcMain(dataSet);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        grdCtrlEdc.MainView = gridViewEdc;
                        grdCtrlEdc.DataSource = dsReturn.Tables[EDC_MAIN_FIELDS.DATABASE_TABLE_NAME];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
            }

            
        }

        #region set active status
        /// <summary>
        /// set active status
        /// </summary>
        private void toolbarStatus_Click(object sender, EventArgs e)
        {
            EntityStatus oldStatus = _edc.Status;
            //show dialog
            StatusDialog status = new StatusDialog(_edc);
            status.ShowDialog();

            if (_edc.Status != oldStatus)
            {
                //set page control status according to status
                if (_edc.Status == EntityStatus.Active || _edc.Status == EntityStatus.Archive)
                {
                    CtrlState = ControlState.ReadOnly;
                }
            }
        }
        #endregion

        private void FormatParamData()
        {
            DataTable paramTable = new DataTable(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME);
            DataColumn paramColum1 = new DataColumn(BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY);
            DataColumn paramColum2 = new DataColumn(BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME);
            paramTable.Columns.Add(paramColum1);
            paramTable.Columns.Add(paramColum2);

            gcMainParam.MainView = gvMainParam;
            gcMainParam.DataSource = paramTable;
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            ParamSearchDialog param = new ParamSearchDialog();
            if (DialogResult.OK == param.ShowDialog())
            {
                if (null == gcMainParam.DataSource)
                {
                    FormatParamData();
                }

                DataTable paramTable = (DataTable)gcMainParam.DataSource;

                for (int i = 0; i < paramTable.Rows.Count; i++)
                {
                    if (param.ParamKey == paramTable.Rows[i][BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY].ToString())
                    {
                        return;
                    }
                }

                paramTable.Rows.Add(new object[] { param.ParamKey, param.ParamName });
                paramTable.AcceptChanges();

                for (int i = 0; i < mainParamTable.Rows.Count; i++)
                {
                    if (param.ParamKey == mainParamTable.Rows[i][BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY].ToString())
                    {
                        if (Convert.ToInt32(OperationAction.New) == Convert.ToInt32(mainParamTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]))
                        {
                            return;
                        }
                        else
                        {
                            mainParamTable.Rows.Remove(mainParamTable.Rows[i]); 
                            return;
                        }
                    }
                }

                Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION, 
                                                                    Convert.ToString((int)OperationAction.New)},
                                                                {EDC_MAIN_PARAM_FIELDS.FIELD_EDC_KEY, _edc.EdcKey},
                                                                {EDC_MAIN_PARAM_FIELDS.FIELD_PARAM_KEY,param.ParamKey}
                                                            };

                FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref mainParamTable, rowData);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvMainParam.RowCount == 0)
                return;

            DataRow row = this.gvMainParam.GetDataRow(this.gvMainParam.FocusedRowHandle);
            string paramKey = row[BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY].ToString();

            DataTable paramTable = (DataTable)gcMainParam.DataSource;
            paramTable.Rows.Remove(row);

            for (int i = 0; i < mainParamTable.Rows.Count; i++)
            {
                if (paramKey == mainParamTable.Rows[i][BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY].ToString())
                {
                    if (Convert.ToInt32(OperationAction.Delete) ==  Convert.ToInt32(mainParamTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]))
                    {
                        return;
                    }
                    else
                    {
                        mainParamTable.Rows.Remove(mainParamTable.Rows[i]);
                        return;
                    }
                }
            }

            Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION, 
                                                                    Convert.ToString((int)OperationAction.Delete)},
                                                                {EDC_MAIN_PARAM_FIELDS.FIELD_EDC_KEY, _edc.EdcKey},
                                                                {EDC_MAIN_PARAM_FIELDS.FIELD_PARAM_KEY,paramKey}
                                                            };

            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref mainParamTable, rowData);
        }

        public static DataTable CreateMainParamTable()
        {
            List<string> fields = new List<string>() {
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,
                                                        EDC_MAIN_PARAM_FIELDS.FIELD_EDC_KEY,
                                                        EDC_MAIN_PARAM_FIELDS.FIELD_PARAM_KEY
                                                     };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(EDC_MAIN_PARAM_FIELDS.DATABASE_TABLE_NAME, fields);
        }


        #region Load Resource file data change UI languages
        /// <summary>
        /// Load Resource file data change UI languages
        /// </summary>
        private void EDCManageCtl_Load(object sender, EventArgs e)
        {
            this.toolbarNew.Text = StringParser.Parse("${res:Global.New}");
            this.toolbarSave.Text = StringParser.Parse("${res:Global.Save}");
            this.toolbarQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.toolbarDelete.Text = StringParser.Parse("${res:Global.Delete}");
            this.toolbarStatus.Text = StringParser.Parse("${res:Global.Status}");

            this.EdcBaseTabPage.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCManageCtl.EdcBaseTabPage}");
            this.EdcParamTabPage.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCManageCtl.EdcParamTabPage}");
            this.lblEdcName.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCManageCtl.lblEdcName}");
            this.lblEdcVersion.Text = 
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCManageCtl.lblEdcVersion}");
            this.lblEdcDescription.Text = 
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCManageCtl.lblEdcDescription}");
            this.btnAdd.Text = 
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCManageCtl.btnAdd}");
            this.btnDelete.Text = 
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCManageCtl.btnDelete}");
            this.gridColumn_paramName.Caption = 
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCManageCtl.gridColumn_paramName}");
            this.lblMenu.Text = "基础数据 > 抽检管理 > 抽检分组";
            edcManage = new EdcManage();
            GridViewHelper.SetGridView(gvMainParam);
        }
        #endregion

        #region Private variable definition
        private EdcManage _edc = null;

        DataTable mainParamTable = CreateMainParamTable();
        #endregion

        private void gvMainParam_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdCtrlEdc_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                if (null == edcManage.EdcKey || edcManage.EdcKey.Length < 1)
                    return;
                if (null == edcManage.EdcName || edcManage.EdcName.Length < 1)
                    return;

                //string title = StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCManageViewContent}") + "_" + edcManage.EdcName;

                //foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                //{
                //    if (viewContent.TitleName == title)
                //    {
                //        viewContent.WorkbenchWindow.SelectWindow();
                //        return;
                //    }
                //}

                //EDCManageViewContent paramContent = new EDCManageViewContent(new EdcManage(edcManage.EdcKey));
                //WorkbenchSingleton.Workbench.ShowView(paramContent);

                MapEdcAndParamToControls(edcManage);
            }
        }

        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gridViewEdc.GetDataRowHandleByGroupRowHandle(gridViewEdc.FocusedRowHandle);
            if (rowHandle >= 0)
            {
                edcManage.EdcKey = gridViewEdc.GetRowCellValue(rowHandle, EDC_MAIN_FIELDS.FIELD_EDC_KEY).ToString();
                edcManage.EdcName = gridViewEdc.GetRowCellValue(rowHandle, EDC_MAIN_FIELDS.FIELD_EDC_NAME).ToString();
                edcManage.GetEdcByKey(edcManage.EdcKey);
                return true;
            }
            return false;
        }
    }
}
