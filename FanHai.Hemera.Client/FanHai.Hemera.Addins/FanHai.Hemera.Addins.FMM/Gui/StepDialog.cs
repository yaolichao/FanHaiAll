//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-01-29            修改。
// =================================================================================
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
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.UDA;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Addins.EDC;
using FanHai.Hemera.Share.Interface;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 设置工步信息的对话框。
    /// </summary>
    public partial class StepDialog : BaseDialog
    {
        private StepEntity _step = null;
        private UdaCommonControl _udaCommonControl = null;

        /// <summary>
        /// 构造函数。
        /// </summary>
        public StepDialog(StepEntity step)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.StepDialog.Title}"))
        {
            InitializeComponent();
            _step = step;
         
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        private void StepDialog_Load(object sender, EventArgs e)
        {
            InitUIValue();
            InitUIResource();
            BindDataType();
            BindDataFrom();
            BindDCType();
            BindValidateRule();
            BindValidateFailedRule();
            BindOperationParams();
            BindCalculateRule();
        }
        /// <summary>
        /// 初始化界面控件值。
        /// </summary>
        private void InitUIValue()
        {
            if (null != _step)
            {
                _udaCommonControl = new UdaCommonControl(EntityType.Step, _step.StepKey);
                MapStepToControls();
            }
            _udaCommonControl.Dock = DockStyle.Fill;
            panelUda.Controls.Add(_udaCommonControl);
            btnConfirm.Enabled = true;
        }
        /// <summary>
        /// 初始化UI本地化资源。
        /// </summary>
        private void InitUIResource()
        {
            this.btnConfirm.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");

            this.xptBaseInfo.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.OperationTabPage}");
            this.xtpAttribute.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.OperationAttrTabPage}");
            this.lblStepName.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lblOperationName}");
            this.lblVersion.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lblVersion}");
            this.lblDuration.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lblDuration}");
            this.lblDescription.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationCtrl.lblDescription}");
        }
        /// <summary>
        /// 绑定工序参数数据。
        /// </summary>
        private void BindOperationParams()
        {
            this.gcParams.MainView = this.gvParams;
            this._step.Params.DefaultView.Sort = POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_INDEX;
            this._step.Params.DefaultView.RowFilter = string.Format("{0}=0", POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_DELETED);
            this.gcParams.DataSource = this._step.Params.DefaultView;
            BestFitParamsGridControlColumns();
        }
        /// <summary>
        /// 调整工序参数列。
        /// </summary>
        private void BestFitParamsGridControlColumns()
        {
            this.gclParamName.MinWidth = this.gvParams.CalcColumnBestWidth(this.gclParamName);
            this.gvParams.BestFitColumns();
        }
        /// <summary>
        /// 绑定数据类型。
        /// </summary>
        private void BindDataType()
        {
            this.rilueDataType.Columns.Add(new LookUpColumnInfo(COMMON_FIELDS.FIELD_COMMON_DESCRIPTION, ""));
            DataTable dtReturn = CommonUtils.ConvertEnumTypeToDataTable(typeof(AttributeDataType));
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
        /// 映射工步数据到控件。
        /// </summary>
        private void MapStepToControls()
        {
            txtStepName.Text = _step.StepName;
            mmDescription.Text = _step.OsDescription;
            txtDuration.Text = _step.OsDuration;
            txtOperationVersion.Text = _step.StepVersion;

            this.beScrapReasonCode.Tag = _step.ScrapCodesKey;
            this.beScrapReasonCode.Text = _step.ScrapCodesName;
            this.beDefectReasonCode.Tag = _step.DefectCodesKey;
            this.beDefectReasonCode.Text = _step.DefectCodesName;
            this.teParamCountPerCount.Text = Convert.ToString(_step.ParamCountPerRow);
            this.rgOrderType.EditValue = (int)_step.ParamOrderType;
            // Set Uda
            _udaCommonControl.UserDefinedAttrs = _step.UserDefinedAttrs;
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
        /// 确定按钮。
        /// </summary>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.EnsureSaveCurrentData}", "${res:Global.SystemInfo}"))
            {
                MapControlsToStep();

                //check data result
                if (!_udaCommonControl.DataTypeCheckResult)
                {
                    return;
                }

                if (_step.StepSeqence != string.Empty)
                {
                    if (_step.Update())
                    {
                        for (int i = _step.UserDefinedAttrs.UserDefinedAttrList.Count - 1; i >= 0; i--)
                        {
                            if (_step.UserDefinedAttrs.UserDefinedAttrList[i].OperationAction == OperationAction.Delete)
                            {
                                _step.UserDefinedAttrs.UserDefinedAttrList.RemoveAt(i);
                            }
                            else
                            {
                                _step.UserDefinedAttrs.UserDefinedAttrList[i].OperationAction = OperationAction.Update;
                            }
                        }
                    }
                }

                this.Close();
            }
        }
        /// <summary>
        /// 验证并收集工步信息数据。
        /// </summary>
        private void MapControlsToStep()
        {
            if (null == _step)
            {
                throw (new Exception("Error Step Set"));
            }
            if (txtStepName.Text.Trim() == string.Empty)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:Global.NameNotNullMessage}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            // TODO: Data validation
            _step.StepName = txtStepName.Text;
            _step.OsDescription = mmDescription.Text;
            _step.OsDuration = txtDuration.Text;
            _step.ParamCountPerRow = Convert.ToInt32(this.teParamCountPerCount.Text);
            int nOrderType = Convert.ToInt32(this.rgOrderType.EditValue);
            _step.ParamOrderType = (OperationParamOrderType)nOrderType;
            _step.ScrapCodesKey = Convert.ToString(this.beScrapReasonCode.Tag);
            _step.ScrapCodesName = this.beScrapReasonCode.Text;
            _step.DefectCodesKey = Convert.ToString(this.beDefectReasonCode.Tag);
            _step.DefectCodesName = this.beDefectReasonCode.Text;
            string userName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            string timeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            _step.Creator = userName;
            _step.CreateTimeZone = timeZone;
            _step.Editor = userName;
            _step.EditTimeZone = timeZone;
            // Set Uda
            _step.UserDefinedAttrs = _udaCommonControl.UserDefinedAttrs;
        }
        /// <summary>
        /// 取消按钮。
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
                DataRow drNew = dtParams.NewRow();
                dtParams.Rows.Add(drNew);
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_ROUTE_STEP_PARAM_KEY] = CommonUtils.GenerateNewKey(0);
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_ROUTE_STEP_KEY] = this._step.StepKey;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_KEY] = dlg.ParamKey;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_NAME] = dlg.ParamName;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_DATA_FROM] = 0;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_DATA_TYPE] = dlg.ParamDataType;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_DC_TYPE] = 1;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_MUSTINPUT] = 0;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_FEEDING] = 0;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_READONLY] = 0;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_COMPLETE_PREVALUE] = 0;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_VALIDATE_RULE] = 1;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_MAT_RULE] = "";
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_VALIDATE_FAILED_RULE] = 0;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_CALCULATE_RULE] = 0;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_FIELD_LENGTH] = "";
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_CALCULATE_RULE] = 0;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_DELETED] = 0;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_EDIT_TIMEZONE] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                drNew[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                int i = 1;
                foreach (DataRow dr in dtParams.Rows)
                {
                    int isDeleted = Convert.ToInt32(dr[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_DELETED]);
                    if (isDeleted == 0)
                    {
                        dr[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_INDEX] = i;
                        i++;
                    }
                }
                BestFitParamsGridControlColumns();
            }
        }
        /// <summary>
        /// 移除参数
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
                drvDelete.Row[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_DELETED] = 1;
                drvDelete.Row[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_INDEX] = -1;
            }
            int i = 1;
            foreach (DataRow dr in dtParams.Rows)
            {
                int isDeleted = Convert.ToInt32(dr[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_DELETED]);
                if (isDeleted == 0)
                {
                    dr[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_INDEX] = i;
                    i++;
                }
            }
            BestFitParamsGridControlColumns();
        }
        /// <summary>
        /// 上移
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
            DataRowView drvUp = dvParams[index - 1];
            DataRowView drvMove = dvParams[index];
            drvUp[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_INDEX] = (index + 1);
            drvMove[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_INDEX] = (index + 1) - 1;
            this.gvParams.RefreshData();
            this.gvParams.Focus();
        }
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            DataView dvParams = this.gcParams.DataSource as DataView;
            int index = this.gvParams.FocusedRowHandle;
            if (index == dvParams.Count - 1) return;
            if (index < 0)
            {
                MessageService.ShowMessage("请选择要下移的参数信息。", "提示");
                return;
            }
            DataRowView drvDown = dvParams[index + 1];
            DataRowView drvMove = dvParams[index];
            drvDown[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_INDEX] = (index + 1);
            drvMove[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_INDEX] = (index + 1) + 1;
            this.gvParams.RefreshData();
            this.gvParams.Focus();
        }
    }
}
