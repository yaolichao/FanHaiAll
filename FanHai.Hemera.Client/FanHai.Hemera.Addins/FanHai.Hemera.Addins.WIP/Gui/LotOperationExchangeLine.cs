using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Card;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 撤销批次的窗体类。
    /// </summary>
    public partial class LotOperationExchangeLine : BaseUserCtrl
    {
        LotOperationEntity _entity = new LotOperationEntity();
        LineSettingEntity _lineSettingEntity = new LineSettingEntity();


        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotOperationExchangeLine()
        {
            InitializeComponent();
            InitializeLanguage();
            GridViewHelper.SetGridView(gvExchangeLine);
        }



        private void InitializeLanguage()
        {
            this.gcolRowNum.Caption = StringParser.Parse("${res:Global.RowNumber}");//"序号";
            this.gcolLotNumber.Caption = StringParser.Parse("${res:Global.LotNumber}");//"批次号";
            this.btnRemove.Text = StringParser.Parse("${res:Global.Remove}");// "移除";
            this.btnAdd.Text = StringParser.Parse("${res:Global.New}");// "新增";
            //this.lcgResult.Text = StringParser.Parse("${res:Global.List}");//"列表";
            //this.lciRemark.Text = StringParser.Parse("${res:Global.Remark}");//"备注";
            //this.lciLotNumber.Text = StringParser.Parse("${res:Global.LotNumber}");//"批次号";
            //this.lciAdd.Text = StringParser.Parse("${res:Global.New}");// "新增";
            //this.lciRemove.Text = StringParser.Parse("${res:Global.Remove}");// "移除";
            this.gcolEditor.Caption = StringParser.Parse("${res:Global.Operator}");//"操作者";
            this.gcolEditTime.Caption = StringParser.Parse("${res:Global.Operation.Time}");//"操作时间";
            this.btnSave.Text = StringParser.Parse("${res:Global.Save}");//保存
            this.gcolWorkorderNo.Caption = StringParser.Parse("${res:Global.WorkNumber}");//工单号

            this.gcolLotLineCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.gcolLotLineCode}");//"主线名称";
            this.gcolProID.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.gcolProID}");//"产品ID";
            this.gcolEnterpriseName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.gcolEnterpriseName}");//"工艺流程组";
            this.gcolRouteName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.gcolRouteName}");//"工艺流程";
            this.gcolStepName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.gcolStepName}");//"当前工序";
            this.gcolQuantity.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.gcolQuantity}");//"电池片数量";
            this.lblMenu.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.lblTitle}");//"线别调整";
            this.lblMenu.Text = "质量管理>质量作业>组件转线";//"线别调整";
            //this.lciFactoryRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.lciFactoryRoom}");//"车间名称";
            //this.lciLotLine.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.lciLotLine}");//"批次线别";
        }



        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotOperationExchangeLine_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            BindSubLineList();
            BindExchangeLineInfo();
            ResetControlValue();
        }
        /// <summary>
        /// 重置控件值。
        /// </summary>
        private void ResetControlValue()
        {
            DataTable dtList = this.gcExchangeLine.DataSource as DataTable;
            if (dtList != null)
            {
                dtList.Rows.Clear();
                dtList.AcceptChanges();
            }
            this.teLotNumber.Text = string.Empty;
            this.teLotNumber.Select();
        }
        /// <summary>
        /// 绑定车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            if (dt != null)
            {
                this.lueFactoryRoom.Properties.DataSource = dt;
                this.lueFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
                if (dt.Rows.Count > 0)
                {
                    this.lueFactoryRoom.EditValue = dt.Rows[0]["LOCATION_KEY"].ToString();
                }
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
            //禁用领料车间
            if (dt == null || dt.Rows.Count <= 1)
            {
                this.lueFactoryRoom.Properties.ReadOnly = true;
            }
        }
        /// <summary>
        /// 线别下拉绑定
        /// </summary>
        private void BindSubLineList()
        {
            string userName = string.Empty;

            DataSet dsSubLine = null;

            try
            {
                userName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);

                dsSubLine = _lineSettingEntity.GetLineByUserNameAndLineName(userName, string.Empty);

                if (string.IsNullOrEmpty(_lineSettingEntity.ErrorMsg))
                {
                    //绑定子线下拉数据

                    DataTable dtSubLineBind = dsSubLine.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME];

                    lueLotLine.Properties.DisplayMember = FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME;
                    lueLotLine.Properties.ValueMember = FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY;
                    lueLotLine.Properties.DataSource = dtSubLineBind;
                }

            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }

        /// <summary>
        /// 初始化批次调整列表。
        /// </summary>
        private void BindExchangeLineInfo()
        {
            DataTable dtList = new DataTable();
            dtList.Columns.Add("LOT_NUMBER", typeof(string));
            dtList.Columns.Add("LOT_LINE_KEY", typeof(string));
            dtList.Columns.Add("LOT_LINE_CODE", typeof(string));
            dtList.Columns.Add("WORK_ORDER_NO", typeof(string));
            dtList.Columns.Add("PRO_ID", typeof(string));
            dtList.Columns.Add("ENTERPRISE_NAME", typeof(string));
            dtList.Columns.Add("ROUTE_NAME", typeof(string));
            dtList.Columns.Add("STEP_NAME", typeof(string));
            dtList.Columns.Add("QUANTITY", typeof(decimal));
            dtList.Columns.Add("ACTIVITY", typeof(string));
            dtList.Columns.Add("EDITOR", typeof(string));
            dtList.Columns.Add("EDIT_TIME", typeof(DateTime));

            this.gcExchangeLine.MainView = this.gvExchangeLine;
            this.gcExchangeLine.DataSource = dtList;
        }

        /// <summary>
        /// 自定义绘制单元格显示值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UndoGridView_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {

            if (e.Column == this.gcolRowNum)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }

        }

        /// <summary>
        /// 批次号回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teLotNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnAdd_Click(sender, e);
            }
        }

        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:Global.SystemInfo}"); //提示

        /// <summary>
        /// 新增批次操作记录。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtList = this.gcExchangeLine.DataSource as DataTable;
                string lotNumber = this.teLotNumber.Text.ToUpper().Trim();
                if (string.IsNullOrEmpty(lotNumber))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotUndoCtrl.Msg001}"), MESSAGEBOX_CAPTION);//请输入序列号
                    //MessageService.ShowMessage("请输入序列号。", "提示");
                    this.teLotNumber.Select();
                    return;
                }

                int count = dtList.AsEnumerable()
                                  .Count(dr => Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]) == lotNumber);
                if (count > 0)
                {
                    MessageService.ShowMessage(string.Format("【{0}】在列表中已存在，请确认。", lotNumber), "提示");
                    this.teLotNumber.SelectAll();
                    return;
                }
                LotQueryEntity queryEntity = new LotQueryEntity();
                DataSet dsReturn = queryEntity.GetLotInfo(lotNumber);
                if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
                {
                    MessageService.ShowError(queryEntity.ErrorMsg);
                    dsReturn = null;
                    this.teLotNumber.SelectAll();
                    return;
                }
                if (dsReturn.Tables[0].Rows.Count < 1)
                {
                    MessageService.ShowMessage(string.Format("获取【{0}】信息失败，请检查。", lotNumber));
                    dsReturn = null;
                    this.teLotNumber.SelectAll();
                    return;
                }
                //判断批次号在指定车间中是否存在。
                string currentRoomKey = Convert.ToString(dsReturn.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
                string roomKey = lueFactoryRoom.EditValue.ToString();
                if (roomKey != currentRoomKey)
                {
                    MessageService.ShowMessage(string.Format("【{0}】在当前车间中不存在，请确认。", lotNumber), "提示");
                    this.teLotNumber.SelectAll();
                    return;
                }
                //判断批次是否被锁定
                int holdFlag = Convert.ToInt32(dsReturn.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_HOLD_FLAG]);
                if (holdFlag == 1)
                {
                    MessageService.ShowMessage(string.Format("【{0}】已被暂停，请确认。", lotNumber), "提示");
                    this.teLotNumber.SelectAll();
                    return;
                }
                //判断批次是否被删除
                int deleteFlag = Convert.ToInt32(dsReturn.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]);
                if (deleteFlag == 1)
                {
                    MessageService.ShowMessage(string.Format("【{0}】已结束，请确认。", lotNumber), "提示");
                    this.teLotNumber.SelectAll();
                    return;
                }
                //判断批次是否已结束
                if (deleteFlag == 2)
                {
                    MessageService.ShowMessage(string.Format("【{0}】已删除，请确认。", lotNumber), "提示");
                    this.teLotNumber.SelectAll();
                    return;
                }
                //判断组件状态
                int curState = int.Parse(dsReturn.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_STATE_FLAG].ToString());
                if (curState > 10)
                {
                    MessageService.ShowMessage(string.Format("【{0}】已完工，请确认。", lotNumber), "提示");
                    return;
                }
                //判断用户是否有组件所在工序权限
                string curOperationName = Convert.ToString(dsReturn.Tables[0].Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);//获取有权限的所有工序

                if ((operations + ",").IndexOf(curOperationName + ",") == -1)
                {
                    MessageService.ShowMessage(string.Format("您没有权限调整工序[{0}]组件线别的操作。", curOperationName), "提示");
                    return;
                }

                if (dtList == null)
                {
                    this.gcExchangeLine.DataSource = dsReturn.Tables[0];
                }
                else
                {
                    if (dtList.Rows.Count >= 100)
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.Msg001}"), MESSAGEBOX_CAPTION);//一次调整录数不能超过100条
                        //MessageService.ShowMessage("一次调整录数不能超过100条。", "提示");
                        return;
                    }
                    dsReturn.Tables[0].TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
                    dtList.Merge(dsReturn.Tables[0]);
                    this.teLotNumber.SelectAll();
                }
            }
            finally
            {
                this.teLotNumber.Select();
                this.teLotNumber.SelectAll();
            }
        }
        /// <summary>
        /// 移除批次操作记录。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int rowHandler = this.gvExchangeLine.FocusedRowHandle;
            if (rowHandler < 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.Msg002}"), MESSAGEBOX_CAPTION);//请选择要删除的记录
                //MessageService.ShowMessage("请选择要删除的记录。", "提示");
                return;
            }
            DataRow dr = this.gvExchangeLine.GetDataRow(rowHandler);
            DataTable dtList = this.gcExchangeLine.DataSource as DataTable;
            if (dtList != null)
            {
                dtList.Rows.Remove(dr);
            }
        }
        /// <summary>
        /// 确定撤销操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

            DataTable dtExchangeLineInfo = this.gcExchangeLine.DataSource as DataTable;
            if (dtExchangeLineInfo.Rows.Count < 1)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.Msg003}"), MESSAGEBOX_CAPTION);//暂停信息列表至少要有一条记录
                //MessageService.ShowMessage("暂停信息列表至少要有一条记录。", "提示");
                return;
            }

            string lotMainLineCode = lueLotLine.Text.ToString();

            //判断是否有进行线别的选择
            if (string.IsNullOrEmpty(lotMainLineCode))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.Msg004}"), MESSAGEBOX_CAPTION);//请选择要调整到的线别
                //MessageService.ShowMessage("请选择要调整到的线别！", "提示");
                return;
            }

            string lotMainLineKey = Convert.ToString(lueLotLine.EditValue);

            string remark = this.teRemark.Text.Trim();
            if (string.IsNullOrEmpty(remark))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.Msg005}"), MESSAGEBOX_CAPTION);//备注必须输入
                //MessageService.ShowMessage("备注必须输入。", "提示");
                this.teRemark.Select();
                return;
            }

            //对视图信息进行更新
            if (this.gvExchangeLine.State == GridState.Editing
                && this.gvExchangeLine.IsEditorFocused
                && this.gvExchangeLine.EditingValueModified)
            {
                this.gvExchangeLine.SetFocusedRowCellValue(this.gvExchangeLine.FocusedColumn, this.gvExchangeLine.EditingValue);
            }
            this.gvExchangeLine.UpdateCurrentRow();

            DataTable dtLotInfo = this.gcExchangeLine.DataSource as DataTable;

            string shiftName = string.Empty;
            string shiftKey = string.Empty;
            string userName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            string oprComputer = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
            string timezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            DataSet dsParams = new DataSet();
            //存放待线别调整的批次的操作数据
            WIP_TRANSACTION_FIELDS transFields = new WIP_TRANSACTION_FIELDS();
            DataTable dtTransaction = CommonUtils.CreateDataTable(transFields);
            foreach (DataRow dr in dtLotInfo.Rows)
            {
                //组织待线别调整的批次的操作数据
                DataRow drTransaction = dtTransaction.NewRow();
                dtTransaction.Rows.Add(drTransaction);
                string transKey = CommonUtils.GenerateNewKey(0);
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY] = transKey;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY] = dr[POR_LOT_FIELDS.FIELD_LOT_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY] = POR_LOT_FIELDS.FIELD_LOT_LINE_EXCHANGE;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN] = dr[POR_LOT_FIELDS.FIELD_QUANTITY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT] = dr[POR_LOT_FIELDS.FIELD_QUANTITY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY] = dr[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME] = dr[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY] = dr[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME] = dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY] = dr[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME] = dr[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY] = dr[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY] = shiftKey;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME] = shiftName;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG] = dr[POR_LOT_FIELDS.FIELD_STATE_FLAG];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG] = dr[POR_LOT_FIELDS.FIELD_IS_REWORKED];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPERATOR] = userName;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER] = oprComputer;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY] = dr[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE] = dr[POR_LOT_FIELDS.FIELD_OPR_LINE];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE] = dr[POR_LOT_FIELDS.FIELD_OPR_LINE_PRE];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY] = dr[POR_LOT_FIELDS.FIELD_EDC_INS_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY] = dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT] = remark;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDITOR] = userName;
                //用于暂存序列号批次信息最后的编辑时间，以便判断序列号信息是否过期。
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME] = dr[POR_LOT_FIELDS.FIELD_EDIT_TIME];
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY] = timezone;
                drTransaction[WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP] = DBNull.Value;
            }

            foreach (DataRow dr in dtLotInfo.Rows)
            {
                dr[POR_LOT_FIELDS.FIELD_LOT_LINE_KEY] = lotMainLineKey;
                dr[POR_LOT_FIELDS.FIELD_LOT_LINE_CODE] = lotMainLineCode;
                dr[POR_LOT_FIELDS.FIELD_EDITOR] = userName;
                dr[POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE] = timezone;
            }

            dsParams.Tables.Add(dtTransaction);

            if (dtLotInfo.DataSet != null)
            {
                dtLotInfo.DataSet.Clear();
            }

            dtLotInfo.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
            dsParams.Tables.Add(dtLotInfo);

            //执行批次线别调整。
            this._entity.LotExchangeLine(dsParams);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
            }
            else
            {
                dsParams.Tables.Clear();
                dtTransaction = null;
                dtLotInfo = null;
                dsParams = null;

                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationExchangeLine.Msg006}"), MESSAGEBOX_CAPTION);//保存成功
                //MessageService.ShowMessage("保存成功！", "提示");

                ResetControlValue();
            }
        }

        /// <summary>
        /// 关闭按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
        }

        private void gvExchangeLine_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
