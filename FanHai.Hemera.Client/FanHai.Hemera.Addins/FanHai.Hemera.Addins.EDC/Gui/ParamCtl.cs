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
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils.Controls.Common;
using System.Collections;

namespace FanHai.Hemera.Addins.EDC
{
    /// <summary>
    /// 显示参数管理界面的用户控件类。
    /// </summary>
    public partial class ParamCtl : BaseUserCtrl
    {
        private Param _param = null;
        //Define and initialize control state
        private ControlState _ctrlState = ControlState.Empty;
        DataTable derivaParamTable = CreateDerivaParamTable();
        //Define delegate manager control state
        public new delegate void AfterStateChanged(ControlState controlState);
        //Define event change control state
        public new AfterStateChanged afterStateChanged = null;
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
        public ParamCtl(Param param)
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);

            _param = param;

            if (null == _param || _param.ParamKey.Length < 1)
            {
                CtrlState = ControlState.Empty;
            }
            else if (_param.ParamKey.Length > 1 && _param.ParamName == string.Empty)
            {
                CtrlState = ControlState.New;
            }
            else
            {
                if (_param.Status == EntityStatus.InActive)
                    CtrlState = ControlState.Edit;
                else
                    CtrlState = ControlState.ReadOnly;
            }

            if (!chkDerivation.Checked)
            {
                ParamDerivTabPage.PageVisible = false;
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
                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = true;
                    toolbarStatus.Enabled = true;

                    txtParamName.Properties.ReadOnly = true;
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    txtTarget.Properties.ReadOnly = false;
                    txtParamUom.Properties.ReadOnly = false;
                    txtParamName.Properties.ReadOnly = false;
                    txtUpperSpec.Properties.ReadOnly = false;
                    txtLowerSpec.Properties.ReadOnly = false;
                    txtUpperBoundary.Properties.ReadOnly = false;
                    txtLowerBoundary.Properties.ReadOnly = false;
                    lueDevice.Properties.ReadOnly = false;


                    lpParamCategory.Properties.ReadOnly = false;
                    lpParamDataType.Properties.ReadOnly = false;
                    mmParamDescription.Properties.ReadOnly = false;

                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = false;

                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                    gcDerivaParam.DataSource = null;
                    derivaParamTable.Clear();
                    _param = new Param();
                    MapParamToControls();
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:
                    txtTarget.Properties.ReadOnly = true;
                    txtParamUom.Properties.ReadOnly = true;
                    txtParamName.Properties.ReadOnly = true;
                    txtUpperSpec.Properties.ReadOnly = true;
                    txtLowerSpec.Properties.ReadOnly = true;
                    txtUpperBoundary.Properties.ReadOnly = true;
                    txtLowerBoundary.Properties.ReadOnly = true;

                    lpParamCategory.Properties.ReadOnly = true;
                    lpParamDataType.Properties.ReadOnly = true;
                    lueDevice.Properties.ReadOnly = true;
                    mmParamDescription.Properties.ReadOnly = true;

                    btnAdd.Enabled = false;
                    btnDelete.Enabled = false;
                    toolbarSave.Enabled = false;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = false;
                    break;
                    #endregion
            }
            toolbarNew.Enabled = true;
            toolbarQuery.Enabled = true;
        }
        /// <summary>
        /// Validation & Collection Data for Param
        /// </summary>
        private void MapControlsToParam()
        {
            if (null == _param)
            {
                throw (new Exception("Error Param Set"));
            }

            // TODO: Data validation
            _param.ParamName = txtParamName.Text;
            _param.ParamDescription = mmParamDescription.Text;
            if (lpParamCategory.EditValue != null)
            {
                _param.ParamCategory = lpParamCategory.EditValue.ToString();
            }
            if (lpParamDataType.EditValue != null)
            {
                _param.ParamDataType = lpParamDataType.EditValue.ToString();
            }
            if (lueDevice.EditValue != null)
            {
                _param.DeviceType = lueDevice.EditValue.ToString();
            }
            _param.ParamDefaultUom = txtParamUom.Text;
            _param.ParamUpperBoundary = txtUpperBoundary.Text;
            _param.ParamUpperSpec = txtUpperSpec.Text;
            _param.ParamTarget = txtTarget.Text;
            _param.ParamLowerBoundary = txtLowerBoundary.Text;
            _param.ParamLowerSpec = txtLowerSpec.Text;

            if (chkMandatory.Checked)
            {
                _param.ParamMandatory = "1";
            }
            else
            {
                _param.ParamMandatory = "0";
            }

            if (chkDerivation.Checked)
            {
                if (_param.IsDerived == "0")
                {
                    _param.OperationAction = OperationAction.New;
                }
                else
                {
                    _param.OperationAction = OperationAction.Modified;
                }

                _param.IsDerived = "1";
                _param.CalculateType = lpCalcType.EditValue != null ? lpCalcType.EditValue.ToString() : "";
                _param.DerivaParamTable = derivaParamTable;
            }
        }
        /// <summary>
        /// Set Param Item data to Controls
        /// </summary>
        private void MapParamToControls()
        {
            txtParamName.Text = _param.ParamName;
            mmParamDescription.Text = _param.ParamDescription;
            txtParamUom.Text = _param.ParamDefaultUom;
            txtUpperBoundary.Text = _param.ParamUpperBoundary;
            txtUpperSpec.Text = _param.ParamUpperSpec;
            txtTarget.Text = _param.ParamTarget;
            txtLowerBoundary.Text = _param.ParamLowerBoundary;
            txtLowerSpec.Text = _param.ParamLowerSpec;

            if (_param.DeviceType != string.Empty)
            {
                lueDevice.EditValue = _param.DeviceType;
            }
            else
                lueDevice.ItemIndex = 0;
            if (_param.ParamCategory != string.Empty)
                lpParamCategory.EditValue = _param.ParamCategory;
            else
                lpParamCategory.ItemIndex = 0;
            if (_param.ParamDataType != string.Empty)
                lpParamDataType.EditValue = _param.ParamDataType;
            else
                lpParamDataType.ItemIndex = 0;

            if (_param.ParamMandatory == "1")
            {
                chkMandatory.Checked = true;
            }
            else
            {
                chkMandatory.Checked = false;
            }

            if (_param.IsDerived == "1")
            {
                chkDerivation.Checked = true;
                gcDerivaParam.MainView = gvDerivaParam;
                gcDerivaParam.DataSource = _param.DerivaParamTable;
            }
            else
            {
                chkDerivation.Checked = false;
                gcDerivaParam.MainView = gvDerivaParam;
                gcDerivaParam.DataSource = null;
            }

            if (_param.CalculateType != string.Empty)
                lpCalcType.EditValue = _param.CalculateType;
            else
                lpCalcType.ItemIndex = 0;
        }
        /// <summary>
        /// 新增参数
        /// </summary>
        private void toolbarNew_Click(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamViewContent}"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    if (viewContent == viewContent.WorkbenchWindow.ActiveViewContent)
                    {
                        ParamCtl ctrl = (ParamCtl)viewContent.Control.Controls.Find("ParamCtl", true)[0];
                        if (ctrl.txtParamName.Text.Trim() != "")
                        {
                            if (!MessageService.AskQuestion("${res:Global.ClearNoteMessage}", "${res:Global.SystemInfo}"))
                            {
                                return;
                            }
                        }
                        ctrl.CtrlState = ControlState.New;
                    }
                    return;
                }
            }

            ParamViewContent paramContent = new ParamViewContent(new Param());
            WorkbenchSingleton.Workbench.ShowView(paramContent);
        }
        /// <summary>
        /// Tool Bar Save Click
        /// </summary>
        private void toolbarSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.EnsureSaveCurrentData}", "${res:Global.SystemInfo}"))
            {
                MapControlsToParam();

                if (_param.ParamName == string.Empty)
                {
                    MessageService.ShowMessage("${res:Global.NameNotNullMessage}", "${res:Global.SystemInfo}");
                    return;
                }

                if (CtrlState == ControlState.New && !_param.ParamNameValidate())
                {
                    MessageService.ShowMessage("${res:Global.NameIsExistMessage}", "${res:Global.SystemInfo}");
                    return;
                }

                if (CtrlState == ControlState.New)
                {
                    if (_param.Insert())
                    {
                        WorkbenchSingleton.Workbench.ActiveViewContent.TitleName
                            = StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamViewContent}") + "_" + _param.ParamName;
                        CtrlState = ControlState.Edit;
                    }
                }
                else
                {
                    _param.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    if (_param.Update())
                    {
                        CtrlState = ControlState.Edit;
                    }
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
                Hashtable htParams = new Hashtable();
                DataSet dsParams = new DataSet();

                string paramName = this.tbParamName.Text.Trim();

                if (paramName.Length > 0)
                {
                    htParams.Add(BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME, paramName);
                }
                //if (!string.IsNullOrEmpty(this._category))
                //{
                //    htParams.Add(BASE_PARAMETER_FIELDS.FIELD_PARAM_CATEGORY, this._category);
                //    htParams.Add(BASE_PARAMETER_FIELDS.FIELD_STATUS, "1");
                //}
                DataTable dtParams = CommonUtils.ParseToDataTable(htParams);
                dtParams.TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
                dsParams.Tables.Add(dtParams);

                string msg = string.Empty;
                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (serverFactory != null)
                {
                    dsReturn = serverFactory.CreateIParamEngine().SearchParam(dsParams);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        gcParamList.MainView = gvParamList;
                        gcParamList.DataSource = dsReturn.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME];
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
            }
        }
        /// <summary>
        /// Tool Bar Delete Click
        /// </summary>
        private void toolbarDelete_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.DeleteNoteMessage}", "${res:Global.SystemInfo}"))
            {
                if (_param.Delete())
                {
                    foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                    {
                        if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamViewContent}"))
                        {
                            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
                            viewContent.WorkbenchWindow.SelectWindow();
                            return;
                        }
                    }

                    WorkbenchSingleton.Workbench.ActiveViewContent.TitleName =
                        StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamViewContent}");
                    CtrlState = ControlState.New;
                }
            }
        }
        /// <summary>
        /// 对下拉列表“参数类型”进行数据绑定
        /// </summary>
        public void BindParamCategory()
        {
            DataSet dataSet = new DataSet();
            dataSet = UdaCommonFunction.GetUdaDataSetOfSomeType("66");//Basic_ParamType
            if (dataSet != null)
            {
                this.lpParamCategory.Properties.DataSource = dataSet.Tables[0];
                this.lpParamCategory.Properties.DisplayMember = "NAME";
                this.lpParamCategory.Properties.ValueMember = "CODE";
                this.lpParamCategory.ItemIndex = 0;
            }
        }
        /// <summary>
        /// 对下拉列表“数据类型”进行数据绑定
        /// </summary>
        public void BindParamDataType()
        {
            DataSet dataSet = new DataSet();
            dataSet = UdaCommonFunction.GetUdaDataSetOfSomeType("67");//Basic_DataType
            if (dataSet != null)
            {
                this.lpParamDataType.Properties.DataSource = dataSet.Tables[0];
                this.lpParamDataType.Properties.DisplayMember = "NAME";
                this.lpParamDataType.Properties.ValueMember = "CODE";
                this.lpParamDataType.ItemIndex = 0;
            }
        }
        /// <summary>
        /// 对下拉列表“计算类型”进行数据绑定
        /// </summary>
        public void BindCalculateType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Calculate_Rule");

            this.lpCalcType.Properties.DataSource = BaseData.Get(columns, category);
            this.lpCalcType.Properties.DisplayMember = "NAME";
            this.lpCalcType.Properties.ValueMember = "CODE";
            this.lpCalcType.ItemIndex = 0;
        }
        /// <summary>
        /// 对下拉列表“关联设备”进行数据绑定
        /// </summary>
        public void BindDeviceType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Device_Type");
            lueDevice.Properties.DataSource = BaseData.Get(columns, category);
            lueDevice.Properties.DisplayMember = "NAME";
            lueDevice.Properties.ValueMember = "CODE";
            lueDevice.ItemIndex = 0;
        }
        /// <summary>
        /// Check box checked changed event 
        /// </summary>
        private void chkDerivation_Properties_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDerivation.Checked)
            {
                ParamDerivTabPage.PageVisible = true;
            }
            else
            {
                ParamDerivTabPage.PageVisible = false;
            }
        }
        /// <summary>
        /// Selected page changed event
        /// </summary>
        private void ParamTabCtrl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page.Name == ParamDerivTabPage.Name && _param.CalculateType != string.Empty)
                lpCalcType.EditValue = _param.CalculateType;
            else
                lpCalcType.ItemIndex = 0;
        }
        /// <summary>
        /// Page load event
        /// </summary>
        private void ParamCtl_Load(object sender, EventArgs e)
        {
            BindParamCategory();
            BindParamDataType();
            BindCalculateType();
            BindDeviceType();
            LoadResourceFileToUI();

            if (CtrlState != ControlState.New)
            {
                MapParamToControls();
            }
        }
        /// <summary>
        /// set active status
        /// </summary>
        private void toolbarStatus_Click(object sender, EventArgs e)
        {
            EntityStatus oldStatus = _param.Status;
            //show dialog
            StatusDialog status = new StatusDialog(_param);
            status.ShowDialog();

            if (_param.Status != oldStatus)
            {
                //set page control status according to status
                if (_param.Status == EntityStatus.Active || _param.Status == EntityStatus.Archive)
                {
                    CtrlState = ControlState.ReadOnly;
                }
            }
        }
        /// <summary>
        /// Add derivation param
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ParamSearchDialog param = new ParamSearchDialog();
            if (DialogResult.OK == param.ShowDialog())
            {
                if (null == gcDerivaParam.DataSource)
                {
                    FormatDerivaParamData();
                }

                DataTable paramTable = (DataTable)gcDerivaParam.DataSource;

                for (int i = 0; i < paramTable.Rows.Count; i++)
                {
                    if (param.ParamKey == paramTable.Rows[i][BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY].ToString())
                    {
                        return;
                    }
                }

                paramTable.Rows.Add();
                paramTable.Rows[paramTable.Rows.Count - 1][BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY] = param.ParamKey;
                paramTable.Rows[paramTable.Rows.Count - 1][BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME] = param.ParamName;
                paramTable.AcceptChanges();

                for (int i = 0; i < derivaParamTable.Rows.Count; i++)
                {
                    if (param.ParamKey == derivaParamTable.Rows[i][BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY].ToString())
                    {
                        if (Convert.ToInt32(OperationAction.New) ==
                            Convert.ToInt32(derivaParamTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]))
                        {
                            return;
                        }
                        else
                        {
                            derivaParamTable.Rows.Remove(derivaParamTable.Rows[i]);
                            return;
                        }
                    }
                }

                Dictionary<string, string> rowData = new Dictionary<string, string>()
                                                            {
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.New)},
                                                                {BASE_PARAMETER_DERIVTION_FIELDS.FIELD_ROW_KEY,CommonUtils.GenerateNewKey(0)},
                                                                {BASE_PARAMETER_DERIVTION_FIELDS.FIELD_DERIVATION_KEY,_param.ParamKey},
                                                                {BASE_PARAMETER_DERIVTION_FIELDS.FIELD_PARAM_KEY,param.ParamKey},
                                                                {BASE_PARAMETER_DERIVTION_FIELDS.FIELD_PARAM_NAME,param.ParamName},
                                                                {COMMON_FIELDS.FIELD_COMMON_EDITOR,PropertyService.Get(PROPERTY_FIELDS.USER_NAME)}
                                                            };

                FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref derivaParamTable, rowData);
            }
        }
        /// <summary>
        /// Delete derivation param
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvDerivaParam.RowCount == 0)
                return;

            DataRow row = this.gvDerivaParam.GetDataRow(this.gvDerivaParam.FocusedRowHandle);
            string paramKey = row[BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY].ToString();
            string paramName = row[BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME].ToString();

            DataTable paramTable = (DataTable)gcDerivaParam.DataSource;
            paramTable.Rows.Remove(row);

            for (int i = 0; i < derivaParamTable.Rows.Count; i++)
            {
                if (paramKey == derivaParamTable.Rows[i][BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY].ToString())
                {
                    if (Convert.ToInt32(OperationAction.Delete) ==
                        Convert.ToInt32(derivaParamTable.Rows[i][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]))
                    {
                        return;
                    }
                    else
                    {
                        derivaParamTable.Rows.Remove(derivaParamTable.Rows[i]);
                        break;
                    }
                }
            }

            Dictionary<string, string> rowData = new Dictionary<string, string>()
                                                            {
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION, Convert.ToString((int)OperationAction.Delete)},
                                                                {BASE_PARAMETER_DERIVTION_FIELDS.FIELD_ROW_KEY,""},
                                                                {BASE_PARAMETER_DERIVTION_FIELDS.FIELD_DERIVATION_KEY,_param.ParamKey},
                                                                {BASE_PARAMETER_DERIVTION_FIELDS.FIELD_PARAM_KEY,paramKey},
                                                                {BASE_PARAMETER_DERIVTION_FIELDS.FIELD_PARAM_NAME,paramName},
                                                                {COMMON_FIELDS.FIELD_COMMON_EDITOR,_param.Editor}
                                                            };

            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref derivaParamTable, rowData);
        }
        /// <summary>
        /// Formating data source
        /// </summary>
        private void FormatDerivaParamData()
        {
            DataTable derivaParamTable = new DataTable(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME);
            DataColumn paramColum1 = new DataColumn(BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY);
            DataColumn paramColum2 = new DataColumn(BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME);
            derivaParamTable.Columns.Add(paramColum1);
            derivaParamTable.Columns.Add(paramColum2);

            gcDerivaParam.MainView = gvDerivaParam;
            gcDerivaParam.DataSource = derivaParamTable;
        }
        /// <summary>
        /// Create derivation param table
        /// </summary>
        public static DataTable CreateDerivaParamTable()
        {
            List<string> fields = new List<string>() {
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,
                                                        BASE_PARAMETER_DERIVTION_FIELDS.FIELD_ROW_KEY,
                                                        BASE_PARAMETER_DERIVTION_FIELDS.FIELD_DERIVATION_KEY,
                                                        BASE_PARAMETER_DERIVTION_FIELDS.FIELD_PARAM_KEY,
                                                        BASE_PARAMETER_DERIVTION_FIELDS.FIELD_PARAM_NAME,
                                                        COMMON_FIELDS.FIELD_COMMON_EDITOR
                                                     };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(BASE_PARAMETER_DERIVTION_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        /// <summary>
        /// Load resource file to UI
        /// </summary>
        private void LoadResourceFileToUI()
        {
            this.toolbarNew.Text = StringParser.Parse("${res:Global.New}");
            this.toolbarSave.Text = StringParser.Parse("${res:Global.Save}");
            this.toolbarQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.toolbarDelete.Text = StringParser.Parse("${res:Global.Delete}");
            this.toolbarStatus.Text = StringParser.Parse("${res:Global.Status}");
            this.btnAdd.Text = StringParser.Parse("${res:Global.New}");
            this.btnDelete.Text = StringParser.Parse("${res:Global.Delete}");

            this.ParamBaseTabPage.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.ParamBaseTabPage}");
            this.ParamRangeTabPage.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.ParamRangeTabPage}");
            this.ParamDerivTabPage.Text =
               StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.ParamDerivTabPage}");
            this.lblParamName.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lblParamName}");
            this.lblParamDescription.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lblParamDescription}");
            this.lciParamCategory.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lblParamCategory}");
            this.lblParamDataType.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lblParamDataType}");
            this.lblParamUom.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lblParamUom}");
            this.grbValueSet.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.grbValueSet}");
            this.lblUpperBoundary.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lblUpperBoundary}");
            this.lblUpperSpec.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lblUpperSpec}");
            this.lblTarget.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lblTarget}");
            this.lblLowerSpec.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lblLowerSpec}");
            this.lblLowerBoundary.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lblLowerBoundary}");
            this.lciCalcType.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lblCalculate}");
            this.lciCalcType.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lblCalculate}");
            lciDerivation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lbl.0001}");//是否运算
            lblDevice.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lbl.0002}");//关联设备:
            lcCalcType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lbl.0003}");//选择计算类型
            lcgCalcParams.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamCtl.lbl.0004}");//添加运算参数
            this.lblMenu.Text = "基础数据 > 抽检管理 > 抽检参数";

            GridViewHelper.SetGridView(gvDerivaParam);
        }

        private void gvDerivaParam_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 映射选择项。
        /// </summary>
        /// <returns></returns>
        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gvParamList.GetDataRowHandleByGroupRowHandle(gvParamList.FocusedRowHandle);
            if (rowHandle >= 0)
            {
                DataRow drParamter = gvParamList.GetDataRow(rowHandle);
                _param.ParamKey = Convert.ToString(drParamter[BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY]);
                _param.ParamName = Convert.ToString(drParamter[BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME]);
                _param.GetParamByKey(_param.ParamKey);
                return true;
            }
            return false;
        }

        private void gvParamList_DoubleClick(object sender, EventArgs e)
        {

            if (MapSelectedItemToProperties())
            {
                if (null == _param.ParamKey || _param.ParamKey.Length < 1)
                    return;
                if (null == _param.ParamName || _param.ParamName.Length < 1)
                    return;

                MapParamToControls();
                //dsManage = edcManage.GetEdcByEDCKey(edcManage.EdcKey);
                ////判断返回的结果若成功则绑定在视图上，若失败则弹出对话框进行提示
                //if (dsManage != null)
                //{
                //    if (dsManage.Tables.Count > 0)
                //    {
                //        gcMainParam.DataSource = dsManage.Tables[1];
                //        gridViewEdc.BestFitColumns();
                //        BindData(edcManage);
                //    }
                //}
                //else
                //{
                //    MessageService.ShowError(_param.ErrorMsg);
                //}
            }
        }
    }
}
