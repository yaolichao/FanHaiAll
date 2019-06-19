using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.UDA;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using FanHai.Hemera.Addins.EDC;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 显示工序管理界面的用户自定义控件类。
    /// </summary>
    public partial class OperationCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 当前记录状态。
        /// </summary>
        private ControlState _ctrlState = ControlState.Empty;
        /// <summary>
        /// 工序实体类。
        /// </summary>
        private OperationEntity _operation = null;
        /// <summary>
        /// 自定义属性控件。
        /// </summary>
        private UdaCommonControl _udaCommonControl = null;
        /// <summary>
        /// 控件状态改变事件委托。
        /// </summary>
        /// <param name="controlState"></param>
        private new delegate void AfterStateChanged(ControlState controlState);
        /// <summary>
        /// 当控件状态改变时触发的事件。
        /// </summary>
        private new AfterStateChanged afterStateChanged = null;
        /// <summary>
        /// 控件状态。
        /// </summary>
        public ControlState CtrlState
        {
            get
            {
                return _ctrlState;
            }
            set
            {
                _ctrlState = value;
                if (ControlState.Read == value)
                {
                    _udaCommonControl.CtrlState = ControlState.Edit;
                }
                else
                {
                    _udaCommonControl.CtrlState = _ctrlState;
                }
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }
        /// <summary>
        /// 控件状态事件改变。
        /// </summary>
        /// <param name="state">控件状态。</param>
        private void OnAfterStateChanged(ControlState state)
        {
            switch (state)
            {
                case ControlState.Edit:
                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = true;
                    toolbarStatus.Enabled = true;
                    txtOperationName.Properties.ReadOnly = true;
                    break;
                case ControlState.New:
                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = false;
                    txtOperationName.Properties.ReadOnly = false;
                    _operation = new OperationEntity();
                    _udaCommonControl.LinkedToItemKey = _operation.OperationVerKey;
                    this.gvParams.RefreshData();
                    MapOperationToControls();
                    break;
                case ControlState.ReadOnly:
                    toolbarSave.Enabled = false;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = false;
                    txtOperationName.Properties.ReadOnly = true;
                    break;
                case ControlState.Read:
                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = true;
                    txtOperationName.Properties.ReadOnly = true;
                    break;
            }
            toolbarNew.Enabled = true;
            toolbarQuery.Enabled = true;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OperationCtrl(OperationEntity operation)
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            this._operation = operation;
        }
        /// <summary>
        ///窗体载入事件。
        /// </summary>
        private void OperationCtrl_Load(object sender, EventArgs e)
        {
            BindOpeartionParams();
            BindDataType();
            BindDataFrom();
            BindCalculateRule();
            BindDCType();
            BindValidateRule();
            BindValidateFailedRule();
            InitUIResources();
            InitUIValue();
        }
        /// <summary>
        /// 初始UI本地化资源。
        /// </summary>
        internal void InitUIResources()
        {
            lblMenu.Text = "基础数据 > 流程管理 > 工序属性";
            GridViewHelper.SetGridView(gvParams);

            this.toolbarNew.Text = StringParser.Parse("${res:Global.New}");
            this.toolbarSave.Text = StringParser.Parse("${res:Global.Save}");
            this.toolbarQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.toolbarDelete.Text = StringParser.Parse("${res:Global.Delete}");
            this.toolbarStatus.Text = StringParser.Parse("${res:Global.Status}");

            this.xtpBaseInfo.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.OperationTabPage}");
            this.xtpUdaInfo.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.OperationAttrTabPage}");
            this.lblOperationName.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lblOperationName}");
            this.lblVersion.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lblVersion}");
            this.lblDuration.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lblDuration}");
            this.lblDescription.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lblDescription}");

            lciScrapReasonCode.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lbl.0001}");//报废原因组
            lciSortSeq.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lbl.0002}");//排序序号
            lciDefectCode.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lbl.0003}");//不良原因组
            rgOrderType.Properties.Items[0].Description = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lbl.0004}");//先行后列顺序排列参数
            rgOrderType.Properties.Items[1].Description = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lbl.0005}");//先列后行顺序排列参数
            lciParamCountPerRow.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lbl.0006}");//每行参数数量
            btnAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lbl.0007}");//添加
            btnRemove.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lbl.0008}");//移除
            btnMoveUp.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lbl.0009}");//上移
            btnMoveDown.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lbl.0010}");//下移
            gclRowNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0001}");//序号
            gclParamName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0002}");//参数名称
            gclDataType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0003}");//数据类型
            gclIsMustInput.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0004}");//必填
            gclDataFrom.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0005}");//数据来源
            gclDCType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0006}");//采集类型
            gclReadOnly.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0007}");//只读
            gclIsCompletePreValue.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0008}");//自动填值
            gcFeeding.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0009}");//自动上料
            gclValidateRule.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0010}");//验证工单属性物料控制规则
            gclValidateFailedRule.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0011}");//验证失败规则
            gclValidateFailedMessage.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0012}");//验证失败提示信息
            gclValidateBomRule.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0013}");//Bom物料类型
            gcCalculateRule.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0014}");//计算规则
            gcLength.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OperationCtrl.GridControl.0015}");//长度

            xtpParameters.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lbl.0011}");//参数配置
        }
        /// <summary>
        /// 初始化界面值。
        /// </summary>
        private void InitUIValue() 
        {
            //工序为空或者主键为空，处于空状态。
            if (null == _operation || _operation.OperationVerKey.Length < 1)
            {
                CtrlState = ControlState.Empty;
            }
            //工序主键不为空但名称为空，处于新增状态。
            else if (_operation.OperationVerKey.Length > 1 && _operation.OperationName == string.Empty)
            {
                //添加工序的自定义属性控件。
                _udaCommonControl = new UdaCommonControl(EntityType.Operator, "");
                _udaCommonControl.UserDefinedAttrs = _operation.UserDefinedAttrs;
                CtrlState = ControlState.New;
            }
            else
            {
                //添加工序的自定义属性控件。
                _udaCommonControl = new UdaCommonControl(EntityType.Operator, _operation.OperationVerKey);
                //映射工序数据到控件中。
                MapOperationToControls();
                //如果记录未激活，则处于编辑状态。
                if (_operation.Status == EntityStatus.InActive)
                    CtrlState = ControlState.Edit;
                else//否则只能读取。
                    CtrlState = ControlState.Read;
            }
            _udaCommonControl.Dock = DockStyle.Fill;
            panelUda.Controls.Add(_udaCommonControl);
        }
        /// <summary>
        /// 设置工序数据到控件中。
        /// </summary>
        private void MapOperationToControls()
        {
            txtOperationName.Text = _operation.OperationName;
            txtOperationVersion.Text = _operation.OperationVersion;
            mmDescription.Text = _operation.OsDescription;
            txtDuration.Text = _operation.OsDuration;
            teSortSeq.Text = Convert.ToString(_operation.SortSequence);
            this.beScrapReasonCode.Tag = _operation.ScrapCodesKey;
            this.beDefectReasonCode.Tag = _operation.DefectCodesKey;
            this.beScrapReasonCode.Text = _operation.ScrapCodesName;
            this.beDefectReasonCode.Text = _operation.DefectCodesName;
            this.teParamCountPerCount.Text = Convert.ToString(_operation.ParamCountPerRow);
            this.rgOrderType.EditValue = (int)_operation.ParamOrderType;
            //设置自定义属性值。
            _udaCommonControl.UserDefinedAttrs = _operation.UserDefinedAttrs;
        }
        /// <summary>
        /// 绑定工序参数数据。
        /// </summary>
        private void BindOpeartionParams()
        {
            this.gcParams.MainView = this.gvParams;
            this._operation.Params.DefaultView.Sort = POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_PARAM_INDEX;
            this._operation.Params.DefaultView.RowFilter = string.Format("{0}=0",POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_DELETED);
            this.gcParams.DataSource = this._operation.Params.DefaultView;
        }
        /// <summary>
        /// 绑定数据类型。
        /// </summary>
        private void BindDataType()
        {
            this.rilueDataType.Columns.Add(new LookUpColumnInfo(COMMON_FIELDS.FIELD_COMMON_DESCRIPTION, ""));
            DataTable dtReturn=CommonUtils.ConvertEnumTypeToDataTable(typeof(AttributeDataType));
            this.rilueDataType.DataSource = dtReturn;
            this.rilueDataType.DisplayMember = COMMON_FIELDS.FIELD_COMMON_DESCRIPTION;
            this.rilueDataType.ValueMember = COMMON_FIELDS.FIELD_COMMON_VALUE;
        }
        /// <summary>
        /// 绑定数据来源。
        /// </summary>
        private void BindDataFrom()
        {
            DataTable dtReturn = CommonUtils.ConvertEnumTypeToDataTable(typeof(OperationParamDataFrom));
            this.rilueDataFrom.DataSource = dtReturn;
            this.rilueDataFrom.DisplayMember = COMMON_FIELDS.FIELD_COMMON_DESCRIPTION;
            this.rilueDataFrom.ValueMember = COMMON_FIELDS.FIELD_COMMON_VALUE;
            this.rilueDataFrom.Columns.Add(new LookUpColumnInfo(COMMON_FIELDS.FIELD_COMMON_DESCRIPTION, ""));
        }
        /// <summary>
        /// 绑定采集类型。
        /// </summary>
        private void BindDCType()
        {
            DataTable dtReturn = CommonUtils.ConvertEnumTypeToDataTable(typeof(OperationParamDCType));
            this.rilueDCType.DataSource = dtReturn;
            this.rilueDCType.DisplayMember = COMMON_FIELDS.FIELD_COMMON_DESCRIPTION;
            this.rilueDCType.ValueMember = COMMON_FIELDS.FIELD_COMMON_VALUE;
            this.rilueDCType.Columns.Add(new LookUpColumnInfo(COMMON_FIELDS.FIELD_COMMON_DESCRIPTION, ""));
        }
        /// <summary>
        /// 绑定验证规则。
        /// </summary>
        private void BindValidateRule()
        {
            this.rilueValidateRule.Columns.Add(new LookUpColumnInfo(COMMON_FIELDS.FIELD_COMMON_DESCRIPTION, ""));
            DataTable dtReturn = CommonUtils.ConvertEnumTypeToDataTable(typeof(OperationParamValidateRule));
            this.rilueValidateRule.DataSource = dtReturn;
            this.rilueValidateRule.DisplayMember = COMMON_FIELDS.FIELD_COMMON_DESCRIPTION;
            this.rilueValidateRule.ValueMember = COMMON_FIELDS.FIELD_COMMON_VALUE;
        }
        /// <summary>
        /// 绑定验证失败规则。
        /// </summary>
        private void BindValidateFailedRule()
        {
            DataTable dtReturn = CommonUtils.ConvertEnumTypeToDataTable(typeof(OperationParamValidateFailedRule));
            this.rilueValidateFailedRule.DataSource = dtReturn;
            this.rilueValidateFailedRule.DisplayMember = COMMON_FIELDS.FIELD_COMMON_DESCRIPTION;
            this.rilueValidateFailedRule.ValueMember = COMMON_FIELDS.FIELD_COMMON_VALUE;
            this.rilueValidateFailedRule.Columns.Add(new LookUpColumnInfo(COMMON_FIELDS.FIELD_COMMON_DESCRIPTION, ""));
        }
        /// <summary>
        /// 绑定计算规则。
        /// </summary>
        private void BindCalculateRule()
        {
            this.riRadioRules.Columns.Add(new LookUpColumnInfo(COMMON_FIELDS.FIELD_COMMON_DESCRIPTION, ""));
            DataTable dtReturn = CommonUtils.ConvertEnumTypeToDataTable(typeof(CalculateRule));
            this.riRadioRules.DataSource = dtReturn;
            this.riRadioRules.DisplayMember = COMMON_FIELDS.FIELD_COMMON_DESCRIPTION;
            this.riRadioRules.ValueMember = COMMON_FIELDS.FIELD_COMMON_VALUE;
        }
        /// <summary>
        /// 显示原因代码组查询对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beReasonCode_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ShowReasonCodeQueryHelpDialog(sender as ButtonEdit);
        }
        /// <summary>
        /// 显示原因代码组查询对话框。
        /// </summary>
        /// <param name="btClickButton"></param>
        /// <param name="clickTransactType"></param>
        private void ShowReasonCodeQueryHelpDialog(ButtonEdit sender)
        {
            ReasonCodeCategoryQueryType type = ReasonCodeCategoryQueryType.None;
            if (sender == this.beDefectReasonCode)
            {
                type = ReasonCodeCategoryQueryType.Defect;
            }
            else if (sender == this.beScrapReasonCode)
            {
                type = ReasonCodeCategoryQueryType.Scrap;
            }
            ReasonCodeCategoryQueryHelpModel model = new ReasonCodeCategoryQueryHelpModel()
            {
                QueryType = type
            };
            ReasonCodeCategoryQueryHelpDialog dlg = new ReasonCodeCategoryQueryHelpDialog(model);
            if (sender == this.beScrapReasonCode)
            {
                dlg.OnValueSelected += new ReasonCodeCategoryQueryValueSelectedEventHandler(Scrap_OnValueSelected);
            }
            else if (sender == this.beDefectReasonCode)
            {
                dlg.OnValueSelected += new ReasonCodeCategoryQueryValueSelectedEventHandler(Defect_OnValueSelected);
            }
            Point i = sender.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            dlg.Width = sender.Width;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + sender.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X, i.Y - dlg.Height);
                }
            }
            else
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X + sender.Width - dlg.Width, i.Y + sender.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + sender.Width - dlg.Width, i.Y - dlg.Height);
                }
            }
            dlg.Visible = true;
            dlg.Show();
        }
        /// <summary>
        /// 当报废代码组值改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Scrap_OnValueSelected(object sender, ReasonCodeCategoryQueryValueSelectedEventArgs e)
        {
            this.beScrapReasonCode.Tag = e.ReasonCodeCategoryKey;
            this.beScrapReasonCode.Text = e.ReasonCodeCategoryName;
        }
        /// <summary>
        /// 当不良代码组值改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Defect_OnValueSelected(object sender, ReasonCodeCategoryQueryValueSelectedEventArgs e)
        {
            this.beDefectReasonCode.Tag = e.ReasonCodeCategoryKey;
            this.beDefectReasonCode.Text = e.ReasonCodeCategoryName;
        }
        /// <summary>
        /// 新增按钮事件。
        /// </summary>
        private void toolbarNew_Click(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationViewContent.TitleName}"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    OperationCtrl ctrl = (OperationCtrl)viewContent.Control.Controls.Find("OperationCtrl", true)[0];
                    if (ctrl.txtOperationName.Text.Trim() != "")
                    {
                        if (!MessageService.AskQuestion("${res:Global.ClearNoteMessage}", "${res:Global.SystemInfo}"))
                        {
                            return;
                        }
                    }
                    ctrl.CtrlState = ControlState.New;
                    return;
                }
            }
            OperationViewContent operationContent = new OperationViewContent(new OperationEntity());
            WorkbenchSingleton.Workbench.ShowView(operationContent);
        }
        /// <summary>
        /// 查询按钮事件。
        /// </summary>
        private void toolbarQuery_Click(object sender, EventArgs e)
        {
            OperationSearchDialog dlg = new OperationSearchDialog();
            if (DialogResult.OK == dlg.ShowDialog())
            {
                if (string.Empty == dlg.OperationKey || dlg.OperationKey.Length < 1)
                    return;
                if (string.Empty == dlg.OperationName || dlg.OperationName.Length < 1)
                    return;
                if (string.Empty == dlg.OperationVersion || dlg.OperationVersion.Length < 1)
                    return;
                string title = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationViewContent.TitleName}") + "_" + dlg.OperationName + "." + dlg.OperationVersion;
                foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                {
                    if (viewContent.TitleName == title)
                    {
                        viewContent.WorkbenchWindow.SelectWindow();
                        return;
                    }
                }
                OperationViewContent vContent = new OperationViewContent(new OperationEntity(dlg.OperationKey));
                WorkbenchSingleton.Workbench.ShowView(vContent);
            }
        }
        /// <summary>
        /// 删除按钮事件。
        /// </summary>
        private void toolbarDelete_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.DeleteNoteMessage}", "${res:Global.SystemInfo}"))
            {
                if (_operation.Delete())
                {
                    foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                    {
                        if (viewContent.TitleName ==
                            StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationViewContent.TitleName}"))
                        {
                            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
                            viewContent.WorkbenchWindow.SelectWindow();
                            return;
                        }
                    }
                    WorkbenchSingleton.Workbench.ActiveViewContent.TitleName =
                        StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationViewContent.TitleName}");
                    CtrlState = ControlState.New;
                }
            }
        }
        /// <summary>
        /// 状态按钮事件。
        /// </summary>
        private void toolbarStatus_Click(object sender, EventArgs e)
        {
            EntityStatus oldStatus = _operation.Status;
            StatusDialog status = new StatusDialog(_operation);
            status.ShowDialog();

            if (_operation.Status != oldStatus)
            {
                if (_operation.Status == EntityStatus.Active)
                {
                    CtrlState = ControlState.Read;
                }
                if (_operation.Status == EntityStatus.Archive)
                {
                    CtrlState = ControlState.ReadOnly;
                }
            }
        }
        /// <summary>
        /// 保存按钮事件。
        /// </summary>
        private void toolbarSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.EnsureSaveCurrentData}", "${res:Global.SystemInfo}"))
            {
                if (this.gvParams.State == GridState.Editing
                      && this.gvParams.IsEditorFocused
                      && this.gvParams.EditingValueModified)
                {
                    this.gvParams.SetFocusedRowCellValue(this.gvParams.FocusedColumn, this.gvParams.EditingValue);
                }
                this.gvParams.UpdateCurrentRow();

                MapControlsToOperation();
                if (_operation.OperationName == string.Empty)
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:Global.NameNotNullMessage}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
                if (!_udaCommonControl.DataTypeCheckResult)
                {
                    return;
                }
                //新增或另存为。
                if (CtrlState == ControlState.New)
                {
                    if (_operation.Insert())
                    {
                        WorkbenchSingleton.Workbench.ActiveViewContent.TitleName
                            = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationViewContent.TitleName}") + "_"
                            + _operation.OperationName + "."
                            + _operation.OperationVersion;
                        txtOperationVersion.Text = _operation.OperationVersion;
                        CtrlState = ControlState.Edit;
                    }
                }
                else
                {
                    //更新。
                    if (_operation.Update())
                    {
                        if (CtrlState != ControlState.Read)
                        {
                            CtrlState = ControlState.Edit;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 验证并收集输入的工序数据。
        /// </summary>
        private void MapControlsToOperation()
        {
            _operation.OperationName = txtOperationName.Text;
            _operation.OperationVersion = txtOperationVersion.Text;
            _operation.OsDescription = mmDescription.Text;
            _operation.OsDuration = txtDuration.Text;
            _operation.SortSequence = Convert.ToDecimal(teSortSeq.Text);
            _operation.ParamCountPerRow = Convert.ToInt32(this.teParamCountPerCount.Text);
            int nOrderType=Convert.ToInt32(this.rgOrderType.EditValue);
            _operation.ParamOrderType = (OperationParamOrderType)nOrderType;
            _operation.ScrapCodesKey = Convert.ToString(this.beScrapReasonCode.Tag);
            _operation.DefectCodesKey = Convert.ToString(this.beDefectReasonCode.Tag);
            string userName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            string timeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            _operation.Creator = userName;
            _operation.CreateTimeZone = timeZone;
            _operation.Editor = userName;
            _operation.EditTimeZone = timeZone;
            //设置自定义属性数据。
            _operation.UserDefinedAttrs = _udaCommonControl.UserDefinedAttrs;
        }
        /// <summary>
        /// 添加参数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ParamSearchDialog dlg = new ParamSearchDialog("5");//5:表示参数类型为工序参数。
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DataView dvParams = this.gcParams.DataSource as DataView;
                DataTable dtParams = dvParams.Table;
                DataRow drNew= dtParams.NewRow();
                dtParams.Rows.Add(drNew);
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_ROUTE_OPERATION_PARAM_KEY] = CommonUtils.GenerateNewKey(0);
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_OPERATION_VER_KEY] = this._operation.OperationVerKey;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_PARAM_KEY] = dlg.ParamKey;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_PARAM_NAME] = dlg.ParamName;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_DATA_FROM] = 0;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_DATA_TYPE] = dlg.ParamDataType;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_DC_TYPE] = 1;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_MUSTINPUT] = 0;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_FEEDING] = 0;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_READONLY] = 0;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_COMPLETE_PREVALUE] = 0;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_VALIDATE_RULE] = 1;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_CALCULATE_RULE] = 0;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_FIELD_LENGTH] = "";
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_VALIDATE_FAILED_RULE] = 0;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_DELETED] = 0;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_EDIT_TIMEZONE] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                drNew[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                int i = 1;
                foreach (DataRow dr in dtParams.Rows)
                {
                    int isDeleted = Convert.ToInt32(dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_DELETED]);
                    if (isDeleted == 0)
                    {
                        dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_PARAM_INDEX] = i;
                        i++;
                    }
                }
            }
        }
        /// <summary>
        /// 移除参数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = this.gvParams.FocusedRowHandle;
            if (index < 0)
            {
                MessageService.ShowMessage("请选择要移除的参数信息。", "提示");
                return;
            }
            DataView dvParams = this.gcParams.DataSource as DataView;
            DataTable dtParams = dvParams.Table;
            DataRowView drvDelete = dvParams[index];
            if (drvDelete.Row.RowState == DataRowState.Added)
            {
                drvDelete.Row.Delete();
            }
            else
            {
                drvDelete.Row[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_DELETED] = 1;
                drvDelete.Row[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_PARAM_INDEX] = -1;
            }
            int i = 1;
            foreach (DataRow dr in dtParams.Rows)
            {
                int isDeleted = Convert.ToInt32(dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_DELETED]);
                if (isDeleted==0)
                {
                    dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_PARAM_INDEX] = i;
                    i++;
                }
            }
        }
        /// <summary>
        /// 上移参数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            int index = this.gvParams.FocusedRowHandle;
            if (index == 0) return;
            if (index < 0)
            {
                MessageService.ShowMessage("请选择要上移的参数信息。", "提示");
                return;
            }
            DataView dvParams = this.gcParams.DataSource as DataView;
            DataRowView drvUp = dvParams[index-1];
            DataRowView drvMove = dvParams[index];
            drvUp[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_PARAM_INDEX] = (index + 1);
            drvMove[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_PARAM_INDEX] = (index + 1) - 1;
            this.gvParams.RefreshData();
            this.gvParams.Focus();
        }
        /// <summary>
        /// 下移参数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            DataView dvParams = this.gcParams.DataSource as DataView;
            int index = this.gvParams.FocusedRowHandle;
            if (index == dvParams.Count-1) return;
            if (index < 0)
            {
                MessageService.ShowMessage("请选择要下移的参数信息。", "提示");
                return;
            }
            DataRowView drvDown = dvParams[index + 1];
            DataRowView drvMove = dvParams[index];
            drvDown[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_PARAM_INDEX] = (index + 1);
            drvMove[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_PARAM_INDEX] = (index + 1) + 1;
            this.gvParams.RefreshData();
            this.gvParams.Focus();
        }

        private void gvParams_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
