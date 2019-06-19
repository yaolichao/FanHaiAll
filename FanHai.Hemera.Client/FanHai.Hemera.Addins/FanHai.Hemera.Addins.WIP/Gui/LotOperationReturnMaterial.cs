using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.UDA;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using BarCodePrint;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using FanHai.Hemera.Share.Common;
using System.Linq;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示批次操作（批次退料）明细的控件类。
    /// </summary>
    public partial class LotOperationReturnMaterial : BaseUserCtrl
    {
        LotOperationDetailModel _model = null;
        IViewContent _view = null;
        LotOperationEntity _entity = new LotOperationEntity();
        /// <summary>
        /// 用于暂存当前批次的类型 N:生产批次 L：组件补片批次。
        /// </summary>
        string _lotType = string.Empty;
        /// <summary>
        /// 用于暂存当前批次信息。
        /// </summary>
        DataSet _dsLotInfo = null;
        string _activity = string.Empty;                            //操作动作名称

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        public LotOperationReturnMaterial(LotOperationDetailModel model, IViewContent view)
        {
            InitializeComponent();
            this._view = view;
            this._model = model;
        }
        /// <summary>
        /// 窗体载入事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotOperationReturnMaterial_Load(object sender, EventArgs e)
        {
            //this.lblTitle.Text = this._view.TitleName;
            BindLotInfo();
            InitList();
            ResetControlValue();
            lblMenu.Text = "生产管理>电池片管理>组件退料";
        }
        /// <summary>
        /// 绑定批次信息。
        /// </summary>
        private void BindLotInfo()
        {
            LotQueryEntity queryEntity = new LotQueryEntity();
            this._dsLotInfo = queryEntity.GetLotInfo(this._model.LotNumber);
            if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
            {
                MessageService.ShowError(queryEntity.ErrorMsg);
                this._dsLotInfo = null;
                return;
            }
            if (_dsLotInfo.Tables[0].Rows.Count < 1)
            {
                MessageService.ShowMessage(string.Format("获取【{0}】信息失败，请重试。",this._model.LotNumber));
                this._dsLotInfo = null;
                return;
            }
            this.teLotNumber.Text = this._model.LotNumber;
            this.teLotNumber.Tag = Convert.ToString(_dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY]);
            this.teWorkorderNo.Text = Convert.ToString(_dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
            this.teProId.Text = Convert.ToString(_dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_PRO_ID]);
            this.teEnterpriseName.Text = Convert.ToString(_dsLotInfo.Tables[0].Rows[0][POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            this.teRouteName.Text = Convert.ToString(_dsLotInfo.Tables[0].Rows[0][POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
            this.teStepName.Text = Convert.ToString(_dsLotInfo.Tables[0].Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            this.teEnterpriseName.Tag = Convert.ToString(_dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            string routeKey=Convert.ToString(_dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
            this.teRouteName.Tag = routeKey;
            this.teStepName.Tag = Convert.ToString(_dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            this.teQty.Text = Convert.ToString(_dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY]);
            this.teEfficiency.Text = Convert.ToString(_dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_EFFICIENCY]);
            this._lotType = Convert.ToString(_dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_TYPE]);
            BindTroubleStep();
        }
        /// <summary>
        /// 绑定退料工序。
        /// </summary>
        private void BindTroubleStep()
        {
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);  //拥有权限的工序
            string lotKey = Convert.ToString(this.teLotNumber.Tag);
            DataSet dsTroubleStep = this._entity.GetTroubleStepInfo(lotKey, operations);
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowError(_entity.ErrorMsg);
                return;
            }
            this.rilueStep.Columns.Add(new LookUpColumnInfo(POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME, "名称"));
            this.rilueStep.DataSource = dsTroubleStep.Tables[0];
            this.rilueStep.DisplayMember = POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME;
            this.rilueStep.ValueMember = POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY;
        }

        /// <summary>
        /// 绑定原因代码信息。
        /// </summary>
        private void BindReturnMaterialReasonCode(string stepKey)
        {
            this.rilueReasonCode.DataSource = null;
            DataSet dsReasonCode = null;
            //如果工步主键为空，并且当前批次是组件补片批次,则获取默认的退料原因代码。
            if (string.IsNullOrEmpty(stepKey) && this._lotType == "L")
            {
                dsReasonCode = this._entity.GetReturnMaterialReasonCode();
            }
            else
            {
                dsReasonCode = this._entity.GetReturnMaterialReasonCode(stepKey);
            }
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowError(_entity.ErrorMsg);
                return;
            }
            this.rilueReasonCode.Columns.Clear();
            this.rilueReasonCode.Columns.Add(new LookUpColumnInfo(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_NAME, "名称"));
            this.rilueReasonCode.DataSource = dsReasonCode.Tables[0];
            this.rilueReasonCode.DisplayMember = FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_NAME;
            this.rilueReasonCode.ValueMember = FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_KEY;
        }
        /// <summary>
        /// 初始化原因列表。
        /// </summary>
        private void InitList()
        {
            DataTable dtList = CommonUtils.CreateDataTable(new WIP_RETURN_MAT_FIELDS());
            this.gcList.MainView = this.gvList;
            this.gcList.DataSource = dtList;
        }
        /// <summary>
        /// 重置控件值。
        /// </summary>
        private void ResetControlValue()
        {
            DataTable dtList = this.gcList.DataSource as DataTable;
            dtList.Rows.Clear();
            //初始化原因列表。
            DataRow dr = dtList.NewRow();
            dr[WIP_RETURN_MAT_FIELDS.FIELD_RETURN_QUANTITY] = DBNull.Value;
            this._activity = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RETURN_MATERIAL;
            dtList.Rows.Add(dr);
            this.teRemark.Text = string.Empty;
        }
        /// <summary>
        /// 原因代码中的单元格值改变。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvList_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {

            DataTable dtList = this.gcList.DataSource as DataTable;
            //退料工序
            if (e.Column == this.gclStep)
            {
                dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_REASON_CODE_KEY] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_REASON_CODE_NAME] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_RETURN_QUANTITY] = DBNull.Value;
                string stepKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_STEP_KEY]);
                string stepName = Convert.ToString(this.rilueStep.GetDisplayValueByKeyValue(stepKey));
                int rowIndex = this.rilueStep.GetDataSourceRowIndex(POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY, stepKey);
                string enterpriseKey = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, rowIndex));
                string enterprisenName = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME, rowIndex));
                string routeKey = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY, rowIndex));
                string routeName = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME, rowIndex));
                dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_STEP_NAME] = stepName;
                dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_ENTERPRISE_KEY] = enterpriseKey;
                dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_ENTERPRISE_NAME] = enterprisenName;
                dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_ROUTE_KEY] = routeKey;
                dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_ROUTE_NAME] = routeName;
            }
            else if (e.Column == this.gclReasonCode)
            {
                string stepKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_STEP_KEY]);
                string stepName = Convert.ToString(this.rilueStep.GetDisplayValueByKeyValue(stepKey));

                string codeKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_REASON_CODE_KEY]);
                string codeName = Convert.ToString(this.rilueReasonCode.GetDisplayValueByKeyValue(codeKey));
                //判断原因代码在列表中是否存在。
                if (!string.IsNullOrEmpty(codeKey))
                {
                    int count = dtList.AsEnumerable().Count(dr => Convert.ToString(dr[WIP_RETURN_MAT_FIELDS.FIELD_REASON_CODE_KEY]) == codeKey
                                                                  && Convert.ToString(dr[WIP_RETURN_MAT_FIELDS.FIELD_STEP_KEY]) == stepKey);
                    //原因代码在列表中存在。
                    if (count > 1)
                    {
                        MessageService.ShowMessage(string.Format("退料工序【{0}】+原因代码【{1}】已在列表中存在，请重新选择。", stepName,codeName));
                        dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_REASON_CODE_KEY] = string.Empty;
                        dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_REASON_CODE_NAME] = string.Empty;
                        this.gvList.FocusedColumn = e.Column;
                        this.gvList.ShowEditor();
                        return;
                    }
                }
                dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_REASON_CODE_NAME] = codeName;
            }
            //电池片退料数量列。
            else if (e.Column == this.gclQty)
            {
                double qty = Convert.ToDouble(this.teQty.Text);
                double returnQty = dtList.AsEnumerable().Sum(dr =>dr[WIP_RETURN_MAT_FIELDS.FIELD_RETURN_QUANTITY]==DBNull.Value?0:Convert.ToDouble(dr[WIP_RETURN_MAT_FIELDS.FIELD_RETURN_QUANTITY]));
                double leftQty = qty - returnQty;
                if (leftQty < 0)
                {
                    MessageService.ShowMessage("退料数量不能超过批次的当前电池片数量。");
                    dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_RETURN_QUANTITY] = DBNull.Value;
                    this.gvList.FocusedColumn = this.gclQty;
                    this.gvList.ShowEditor();
                    return;
                }
            }
        }

        /// <summary>
        /// 重置按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbReset_Click(object sender, EventArgs e)
        {
            ResetControlValue();
        }
        /// <summary>
        /// 添加原因信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dtList = this.gcList.DataSource as DataTable;
            DataRow dr = dtList.NewRow();
            dr[WIP_RETURN_MAT_FIELDS.FIELD_RETURN_QUANTITY]=DBNull.Value;
            dtList.Rows.Add(dr);
            this.gvList.FocusedRowHandle = dtList.Rows.Count - 1;
            this.gvList.FocusedColumn = this.gclReasonCode;
            this.gvList.ShowEditor();
        }
        /// <summary>
        /// 移除原因信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = this.gvList.FocusedRowHandle;
            if (index < 0)
            {
                MessageService.ShowMessage("请选择要移除的原因信息。", "提示");
                return;
            }
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList.Rows.Count <=1)
            {
                MessageService.ShowMessage("退料原因列表中必须至少有一条记录。", "提示");
                return;
            }
            dtList.Rows.RemoveAt(index);
        }

        /// <summary>
        /// 自定义绘制单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvList_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            //设置序号。
            if (e.Column == this.gclRowNum)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
            else if (e.Column == this.gclReasonCode)
            {
                DataTable dtList = this.gcList.DataSource as DataTable;
                e.DisplayText = Convert.ToString(dtList.Rows[e.RowHandle][WIP_RETURN_MAT_FIELDS.FIELD_REASON_CODE_NAME]);
            }
        }
        /// <summary>
        /// 确认按钮事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOK_Click(object sender, EventArgs e)
        {
            if (this.gvList.State == GridState.Editing && this.gvList.IsEditorFocused && this.gvList.EditingValueModified)
            {
                this.gvList.SetFocusedRowCellValue(this.gvList.FocusedColumn, this.gvList.EditingValue);
            }
            this.gvList.UpdateCurrentRow();
            //退料原因信息。
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList==null || dtList.Rows.Count < 1)
            {
                MessageService.ShowMessage("退料原因列表中至少必须有一条记录。", "提示");
                return;
            }
            //退料工序必须全部输入
            List<DataRow>  lst = (from item in dtList.AsEnumerable()
                   where string.IsNullOrEmpty(Convert.ToString(item[WIP_RETURN_MAT_FIELDS.FIELD_STEP_KEY]))
                   select item).ToList<DataRow>();
            //如果不是组件补片批次且没有输入退料工序，则给出提示。
            if (lst.Count() > 0 && this._lotType != "L")
            {
                MessageService.ShowMessage("非组件补片批次，退料原因列表中的【退料工序】必须输入。", "提示");
                this.gvList.FocusedColumn = this.gclStep;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
                return;
            }
            //退料原因必须全部输入
            lst = (from item in dtList.AsEnumerable()
               where string.IsNullOrEmpty(Convert.ToString(item[WIP_RETURN_MAT_FIELDS.FIELD_REASON_CODE_KEY])) 
               select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("退料原因列表中的【原因名称】必须输入。", "提示");
                this.gvList.FocusedColumn = this.gclReasonCode;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
                return;
            }
            //退料原因中的数量必须输入值且大于0
            lst=(from item in dtList.AsEnumerable()
                 where string.IsNullOrEmpty(Convert.ToString(item[WIP_RETURN_MAT_FIELDS.FIELD_RETURN_QUANTITY]).Trim())  
                     || Convert.ToInt32(item[WIP_RETURN_MAT_FIELDS.FIELD_RETURN_QUANTITY]) <= 0
                 select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("数量必须输入且大于0。", "提示");
                this.gvList.FocusedColumn = this.gclQty;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
                return;
            }
            //获取当前操作的批次信息
            if(_dsLotInfo==null){
                LotQueryEntity queryEntity = new LotQueryEntity();
                _dsLotInfo = queryEntity.GetLotInfo(this._model.LotNumber);
                if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
                {
                    MessageService.ShowError(queryEntity.ErrorMsg);
                    _dsLotInfo = null;
                    return;
                }
            }
            if (_dsLotInfo.Tables[0].Rows.Count < 1)
            {
                MessageService.ShowMessage(string.Format("获取【{0}】信息失败，请重试。", this._model.LotNumber), "提示");
                _dsLotInfo=null;
                return;
            }
            DataRow drLotInfo=_dsLotInfo.Tables[0].Rows[0];
            string lotKey=Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            double qty=Convert.ToDouble(this.teQty.Text);
            //退料数量总和不能超过当前电池片数量
            double returnQty = dtList.AsEnumerable().Sum(dr => dr[WIP_RETURN_MAT_FIELDS.FIELD_RETURN_QUANTITY]==DBNull.Value ? 0: Convert.ToDouble(dr[WIP_RETURN_MAT_FIELDS.FIELD_RETURN_QUANTITY]));
            double leftQty = qty - returnQty;
            if (leftQty < 0)
            {
                MessageService.ShowMessage("电池片退料数量不能超过当前电池片数量。", "提示");
                return;
            }
            string lineKey=Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);
            string lineName=Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);
            string workOrderKey=Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
            string enterpriseKey=Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            string enterpriseName = this.teEnterpriseName.Text;
            string routeKey=Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
            string routeName = this.teRouteName.Text;
            string stepKey=Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
            int reworkFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
            string equipmentKey = Convert.ToString(drLotInfo[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);
            string edcInsKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_EDC_INS_KEY]);
            string stepName = this.teStepName.Text;
            string remark = this.teRemark.Text;
            string shiftName = this._model.ShiftName;
            string shiftKey = string.Empty;
            //Shift shiftEntity=new Shift();
            //string shiftKey = shiftEntity.IsShiftValueExists(shiftName);//班次主键。
            ////获取班次主键失败。
            //if (!string.IsNullOrEmpty(shiftEntity.ErrorMsg))
            //{
            //    MessageService.ShowError(shiftEntity.ErrorMsg);
            //    return;
            //}
            ////没有排班。
            //if (string.IsNullOrEmpty(shiftKey))
            //{
            //    MessageService.ShowMessage("请先在系统中进行排班。", "提示");
            //    return;
            //}
            string oprComputer=PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
            string timezone=PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            DataSet dsParams = new DataSet();
            //组织退料数据。
            Hashtable htTransaction=new Hashtable();
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY,lotKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, this._activity);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, qty);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, leftQty);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, enterpriseName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, routeName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, stepName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, stateFlag);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, this._model.UserName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, oprComputer);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, lineKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, lineName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, lineName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY, edcInsKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, remark);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, this._model.UserName);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME,null);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, timezone);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
            DataTable dtTransaction = CommonUtils.ParseToDataTable(htTransaction);
            dtTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
            dsParams.Tables.Add(dtTransaction);
            //组织退料原因数据
            DataTable dtReturn = dtList.Copy();
            dtReturn.TableName = WIP_RETURN_MAT_FIELDS.DATABASE_TABLE_NAME;         
            foreach (DataRow dr in dtReturn.Rows)
            {
                dr[WIP_RETURN_MAT_FIELDS.FIELD_EDITOR] = this._model.UserName;
                dr[WIP_RETURN_MAT_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                dr[WIP_RETURN_MAT_FIELDS.FIELD_EDIT_TIMEZONE] = timezone;
            }
            dsParams.Tables.Add(dtReturn);
            //组织其他附加参数数据
            Hashtable htMaindata = new Hashtable();
            htMaindata.Add(COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, this._model.LotEditTime);
            DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
            dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
            dsParams.Tables.Add(dtParams);
            //执行退料。
            this._entity.LotReturnMaterial(dsParams);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
            }
            else
            {
                //this.tsbClose_Click(sender, e);
                MessageService.ShowMessage("保存成功");
                WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
                //重新打开批次创建视图。
                LotOperationViewContent view = new LotOperationViewContent(this._model.OperationType);
                WorkbenchSingleton.Workbench.ShowView(view);
            }
            dsParams.Tables.Clear();
            dtTransaction = null;
            dtReturn = null;
            dtParams = null;
            dsParams = null;
        }
        /// <summary>
        /// 自定义显示编辑器。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvList_ShowingEditor(object sender, CancelEventArgs e)
        {
            //绑定原因代码。
            if (this.gvList.FocusedColumn == this.gclReasonCode && this.gvList.FocusedRowHandle>=0)
            {
                DataTable dtList = this.gcList.DataSource as DataTable;
                string stepKey = Convert.ToString(dtList.Rows[this.gvList.FocusedRowHandle][WIP_RETURN_MAT_FIELDS.FIELD_STEP_KEY]);
                BindReturnMaterialReasonCode(stepKey);
            }
        }
    
    }
}
