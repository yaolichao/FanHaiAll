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
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示批次操作（电池片报废/组件报废）明细的控件类。
    /// </summary>
    public partial class LotOperationScrap : BaseUserCtrl
    {
        LotOperationDetailModel _model = null;
        IViewContent _view = null;
        LotOperationEntity _entity = new LotOperationEntity();
        DataSet dsLotInfo = null;
        string MESSAGEBOX_CAPTION = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.INFORMATION}"); //提示
        LotQueryEntity _queryEntity;
        /// <summary>
        /// 暂存原因分类数据表。
        /// </summary>
        DataTable _dtReasonCodeClass = null;
        string _activity = string.Empty;                            //操作动作名称

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        public LotOperationScrap(LotOperationDetailModel model, IViewContent view)
        {
            InitializeComponent();
            this._view = view;
            this._model = model;
            this._queryEntity = new LotQueryEntity();
            GridViewHelper.SetGridView(gvList);
        }
        /// <summary>
        /// 窗体载入事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotOperationScrap_Load(object sender, EventArgs e)
        {
            this.lblMenu.Text = "质量管理>不良品处理>报废作业";
            BindFactoryRoom();
            BindShiftName();
            //this.lblMenu.Text = "批次操作";
            this.teUserNumber.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            this.teUserNumber.Enabled = false;
            //this.lblApplicationTitle.Text = _viewContent.TitleName;
            this.beLotNumber.Select();
            InitList();
            ResetControlValue();
        }
        /// <summary>
        /// 绑定原因分类。
        /// </summary>
        private void BindReasonCodeClassToDataTable()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            this._dtReasonCodeClass = BaseData.Get(columns, BASEDATA_CATEGORY_NAME.Basic_ClassOfRCode);
        }
        /// <summary>
        /// 绑定批次信息。
        /// </summary>
        private void BindLotInfo()
        {
            LotQueryEntity queryEntity = new LotQueryEntity();
            dsLotInfo = queryEntity.GetLotInfo(this._model.LotNumber);
            if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
            {
                MessageService.ShowError(queryEntity.ErrorMsg);
                dsLotInfo = null;
                return;
            }
            if (dsLotInfo.Tables[0].Rows.Count < 1)
            {
                MessageService.ShowMessage(string.Format("获取【{0}】信息失败，请重试。",this._model.LotNumber));
                dsLotInfo = null;
                return;
            }
            this.teLotNumber.Text = this._model.LotNumber;
            this.teLotNumber.Tag = Convert.ToString(dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY]);
            this.teWorkorderNo.Text = Convert.ToString(dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
            this.teProId.Text = Convert.ToString(dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_PRO_ID]);
            this.teEnterpriseName.Text = Convert.ToString(dsLotInfo.Tables[0].Rows[0][POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            this.teRouteName.Text = Convert.ToString(dsLotInfo.Tables[0].Rows[0][POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
            this.teStepName.Text = Convert.ToString(dsLotInfo.Tables[0].Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            this.teEnterpriseName.Tag = Convert.ToString(dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            string routeKey=Convert.ToString(dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
            this.teRouteName.Tag = routeKey;
            this.teStepName.Tag = Convert.ToString(dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            this.teQty.Text = Convert.ToString(dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY]);
            this.teEfficiency.Text = Convert.ToString(dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_EFFICIENCY]);
        }
        /// <summary>
        /// 绑定问题工序。
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
        /// 绑定原因代码分类信息。
        /// </summary>
        private void BindReasonCodeClass(string categoryKey)
        {
            DataSet dsReturn = this._entity.GetReasonCodeClass(categoryKey);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
                return;
            }
            DataTable dtReturn = dsReturn.Tables[0];
            string rowFilterValue = "'',";
            foreach (DataRow dr in dtReturn.Rows)
            {
                string code = Convert.ToString(dr[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_CLASS]);
                rowFilterValue += string.Format("'{0}',", code);
            }
            rowFilterValue = string.Format(" CODE IN ({0})", rowFilterValue.TrimEnd(','));
            //电池片报废,只显示电池片分类。
            if (this._model.OperationType == LotOperationType.CellScrap)
            {
                rowFilterValue = string.Format("{0} AND CODE IN ({1})", rowFilterValue, BASEDATA_CATEGORY_NAME.Basic_ClassOfRCodeValue_Cell);
            }
            if (this._dtReasonCodeClass != null)
            {
                this._dtReasonCodeClass.DefaultView.RowFilter = rowFilterValue;
                this._dtReasonCodeClass.DefaultView.Sort = "CODE ASC";
                this.rilueReasonCodeClass.Columns.Clear();
                this.rilueReasonCodeClass.Columns.Add(new LookUpColumnInfo("NAME", string.Empty));
                this.rilueReasonCodeClass.DataSource = this._dtReasonCodeClass;
                this.rilueReasonCodeClass.ValueMember = "CODE";
                this.rilueReasonCodeClass.DisplayMember = "NAME";
            }
        }
        /// <summary>
        /// 绑定原因代码信息。
        /// </summary>
        private void BindReasonCode(string categoryKey, string codeClass)
        {
            DataSet dsReasonCode = this._entity.GetReasonCode(categoryKey, codeClass);
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
            DataTable dtList = CommonUtils.CreateDataTable(new WIP_SCRAP_FIELDS());
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
            dr[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY] = 0;
            //电池片报废。
            if (this._model.OperationType == LotOperationType.CellScrap)
            {
                this._activity = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLSCRAP;
            }
            else
            {
                this._activity = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SCRAP;
            }
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
            string reasonCodeClass = Convert.ToString(dtList.Rows[e.RowHandle][WIP_DEFECT_FIELDS.FIELD_REASON_CODE_CLASS]);
            //问题工序 原因分类
            if (e.Column == this.gclStep || e.Column == this.gclReasonCodeClass)
            {
                dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME] = string.Empty;
                if (this._model.OperationType == LotOperationType.CellScrap)
                {
                    dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY] = 0;
                }
                if (e.Column == this.gclStep)
                {
                    string stepKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_STEP_KEY]);
                    string stepName = Convert.ToString(this.rilueStep.GetDisplayValueByKeyValue(stepKey));

                    int rowIndex = this.rilueStep.GetDataSourceRowIndex(POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY, stepKey);
                    string enterpriseKey = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, rowIndex));
                    string enterprisenName = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME, rowIndex));
                    string routeKey = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY, rowIndex));
                    string routeName = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME, rowIndex));
                    dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_STEP_NAME] = stepName;
                    dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_ENTERPRISE_KEY] = enterpriseKey;
                    dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_ENTERPRISE_NAME] = enterprisenName;
                    dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_ROUTE_KEY] = routeKey;
                    dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_ROUTE_NAME] = routeName;
                }
            }
            else if (e.Column == this.gclReasonCode)
            {
                string stepKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_STEP_KEY]);
                string stepName = Convert.ToString(this.rilueStep.GetDisplayValueByKeyValue(stepKey));

                string codeKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY]);
                string codeName = Convert.ToString(this.rilueReasonCode.GetDisplayValueByKeyValue(codeKey));
                //判断原因代码在列表中是否存在。
                if (!string.IsNullOrEmpty(codeKey) && !string.IsNullOrEmpty(stepKey))
                {
                    int count = dtList.AsEnumerable().Count(dr => Convert.ToString(dr[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY]) == codeKey
                                                                  && Convert.ToString(dr[WIP_SCRAP_FIELDS.FIELD_STEP_KEY]) == stepKey);
                    //原因代码在列表中存在。
                    if (count > 1)
                    {
                        MessageService.ShowMessage(string.Format("问题工序【{0}】+原因代码【{1}】已在列表中存在，请重新选择。", stepName,codeName));
                        dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY] = string.Empty;
                        dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME] = string.Empty;
                        this.gvList.FocusedColumn = e.Column;
                        this.gvList.ShowEditor();
                        return;
                    }
                }
                dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME] = codeName;
            }
            //数量列 且 原因分类是电池片。
            else if (e.Column == this.gclQty 
                 && BASEDATA_CATEGORY_NAME.Basic_ClassOfRCodeValue_Cell.IndexOf(string.Format("'{0}'", reasonCodeClass))>=0)
            {
                double qty = Convert.ToDouble(this.teQty.Text);
                double scrapQty = dtList.AsEnumerable().Sum(dr => Convert.ToDouble(dr[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY]));
                double leftQty = qty - scrapQty;
                if (leftQty < 0)
                {
                    MessageService.ShowMessage("电池片报废数量不能超过当前电池片数量。");
                    if (this._model.OperationType == LotOperationType.CellScrap)
                    {
                        dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY] = 0;
                    }
                    this.gvList.FocusedColumn = this.gclQty;
                    this.gvList.ShowEditor();
                    return;
                }
            }
        }
        /// <summary>
        /// 关闭按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {

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
            dr[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY]=0;
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
                MessageService.ShowMessage("报废原因列表中必须至少有一条记录。", "提示");
                return;
            }
            dtList.Rows.RemoveAt(index);
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
            //报废原因信息。
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList==null || dtList.Rows.Count < 1)
            {
                MessageService.ShowMessage("报废原因列表中至少必须有一条记录。", "提示");
                return;
            }
            //问题工序必须全部输入
            List<DataRow> lst = (from item in dtList.AsEnumerable()
                                 where string.IsNullOrEmpty(Convert.ToString(item[WIP_SCRAP_FIELDS.FIELD_STEP_KEY]))
                                 select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("报废原因列表中的【问题工序】必须输入。", "提示");
                this.gvList.FocusedColumn = this.gclStep;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
                return;
            }
            //报废原因必须全部输入
            lst=(from item in dtList.AsEnumerable()
               where string.IsNullOrEmpty(Convert.ToString(item[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY])) 
               select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("报废原因列表中的【原因名称】必须输入。", "提示");
                this.gvList.FocusedColumn = this.gclReasonCode;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
                return;
            }
            //报废原因中的数量必须输入值且大于0
            lst=(from item in dtList.AsEnumerable()
                 where string.IsNullOrEmpty(Convert.ToString(item[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY]).Trim())  
                     || Convert.ToInt32(item[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY]) <= 0
                 select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("数量必须输入且大于0。", "提示");
                this.gvList.FocusedColumn = this.gclQty;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
                return;
            }
            string remark = this.teRemark.Text.Trim();
            if (string.IsNullOrEmpty(remark))
            {
                MessageService.ShowMessage("备注必须输入。", "提示");
                this.teRemark.Select();
                return;
            }
            //获取当前操作的批次信息
            if(dsLotInfo==null){
                LotQueryEntity queryEntity = new LotQueryEntity();
                dsLotInfo = queryEntity.GetLotInfo(this._model.LotNumber);
                if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
                {
                    MessageService.ShowError(queryEntity.ErrorMsg);
                    dsLotInfo = null;
                    return;
                }
            }
            if (dsLotInfo.Tables[0].Rows.Count < 1)
            {
                MessageService.ShowMessage(string.Format("获取【{0}】信息失败，请重试。", this._model.LotNumber), "提示");
                dsLotInfo=null;
                return;
            }
            DataRow drLotInfo=dsLotInfo.Tables[0].Rows[0];
            string lotKey=Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            double qty=Convert.ToDouble(this.teQty.Text);
            //报废数量总和不能超过当前电池片数量
            double scrapQty = dtList.AsEnumerable()
                                    .Where(dr => BASEDATA_CATEGORY_NAME.Basic_ClassOfRCodeValue_Cell.IndexOf(string.Format("'{0}'", Convert.ToString(dr[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_CLASS]))) >= 0)
                                    .Sum(dr => Convert.ToDouble(dr[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY]));
            double leftQty = qty - scrapQty;
            if (leftQty < 0)
            {
                MessageService.ShowMessage("电池片报废数量不能超过当前电池片数量。", "提示");
                return;
            }
            //组件报废操作，电池片数量不变。
            if (this._model.OperationType == LotOperationType.Scrap)
            {
                leftQty = qty;
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
            //组织报废数据。
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
            //组织报废原因数据
            DataTable dtScrap = dtList.Copy();
            dtScrap.TableName = WIP_SCRAP_FIELDS.DATABASE_TABLE_NAME;         
            foreach (DataRow dr in dtScrap.Rows)
            {
                dr[WIP_SCRAP_FIELDS.FIELD_EDITOR] = this._model.UserName;
                dr[WIP_SCRAP_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                dr[WIP_SCRAP_FIELDS.FIELD_EDIT_TIMEZONE_KEY] = timezone;
            }
            dsParams.Tables.Add(dtScrap);
            //组织其他附加参数数据
            Hashtable htMaindata = new Hashtable();
            htMaindata.Add(COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, this._model.LotEditTime);
            DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
            dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
            dsParams.Tables.Add(dtParams);
            //执行报废。
            this._entity.LotScrap(dsParams);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
            }
            else
            {
                this.tsbClose_Click(sender, e);
            }
            dsParams.Tables.Clear();
            dtTransaction = null;
            dtScrap = null;
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
            DataTable dtList = this.gcList.DataSource as DataTable;
            //绑定原因分类
            if (this.gvList.FocusedColumn == this.gclReasonCodeClass && this.gvList.FocusedRowHandle >= 0)
            {
                string stepKey = Convert.ToString(dtList.Rows[this.gvList.FocusedRowHandle][WIP_SCRAP_FIELDS.FIELD_STEP_KEY]);
                if (!string.IsNullOrEmpty(stepKey))
                {
                    int rowIndex = this.rilueStep.GetDataSourceRowIndex(POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY, stepKey);
                    string categoryKey = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_STEP_FIELDS.FIELD_SCRAP_REASON_CODE_CATEGORY_KEY, rowIndex));
                    BindReasonCodeClass(categoryKey);
                }
                else
                {
                    this.rilueReasonCodeClass.DataSource = null;
                }
            }
            //绑定原因代码。
            else if (this.gvList.FocusedColumn == this.gclReasonCode && this.gvList.FocusedRowHandle>=0)
            {
                string stepKey = Convert.ToString(dtList.Rows[this.gvList.FocusedRowHandle][WIP_SCRAP_FIELDS.FIELD_STEP_KEY]);
                string codeClass = Convert.ToString(dtList.Rows[this.gvList.FocusedRowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_CLASS]);
                if (!string.IsNullOrEmpty(stepKey) && !string.IsNullOrEmpty(codeClass))
                {
                    int rowIndex = this.rilueStep.GetDataSourceRowIndex(POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY, stepKey);
                    string categoryKey = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_STEP_FIELDS.FIELD_SCRAP_REASON_CODE_CATEGORY_KEY, rowIndex));
                    BindReasonCode(categoryKey, codeClass);
                }
                else
                {
                    this.rilueReasonCode.DataSource = null;
                }
            }
        }

        private void gvList_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        
        /// <summary>
        /// 绑定工厂车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            //绑定工厂车间数据到窗体控件。
            if (dt != null)
            {
                this.lueFactoryRoom.Properties.DataSource = dt;
                this.lueFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
                this.lueFactoryRoom.ItemIndex = 0;
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
        /// 绑定班别数据。
        /// </summary>
        private void BindShiftName()
        {
            //获取当前班别名称。
            Shift _shift = new Shift();
            string defaultShift = _shift.GetCurrShiftName();

            string[] columns = new string[] { "CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Shift");
            //获取班别代码
            DataTable shiftTable = BaseData.Get(columns, category);
            //获取班别代码成功。
            if (null != shiftTable && shiftTable.Rows.Count > 0)
            {
                this.lueShift.Properties.DataSource = shiftTable;
                this.lueShift.Properties.DisplayMember = "CODE";
                this.lueShift.Properties.ValueMember = "CODE";
                this.lueShift.EditValue = defaultShift;
            }
        }

        /// <summary>
        /// 显示批次选择对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beLotNumber_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            LotQueryHelpModel model = new LotQueryHelpModel();
            model.RoomKey = Convert.ToString(this.lueFactoryRoom.EditValue);

            model.OperationType = LotOperationType.Scrap;

            LotQueryHelpDialog dlg = new LotQueryHelpDialog(model);
            dlg.OnValueSelected += new LotQueryValueSelectedEventHandler(LotQueryHelpDialog_OnValueSelected);
            Point i = beLotNumber.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            dlg.Width = beLotNumber.Width;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + beLotNumber.Height);

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
                    dlg.Location = new Point(i.X + beLotNumber.Width - dlg.Width, i.Y + beLotNumber.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + beLotNumber.Width - dlg.Width, i.Y - dlg.Height);
                }
            }
            dlg.Visible = true;
            dlg.Show();
        }


        /// <summary>
        /// 批号回车时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beLotNumber_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                if (Operate())
                {
                    BindLotInfo();
                    BindTroubleStep();
                    BindReasonCodeClassToDataTable();
                    InitList();
                    ResetControlValue();
                }
            }
        }


        /// <summary>
        /// 进行批次操作。
        /// </summary>
        private bool Operate()
        {
            string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            string roomName = this.lueFactoryRoom.Text.Trim();
            string lotNumber = this.beLotNumber.Text.Trim();
            //车间没有选择，给出提示。
            if (string.IsNullOrEmpty(roomKey))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg004}"), MESSAGEBOX_CAPTION);//车间名称不能为空
                //MessageService.ShowMessage("车间名称不能为空","提示");
                this.lueFactoryRoom.Select();
                return false; ;
            }
            //批号没有输入，给出提示。
            if (string.IsNullOrEmpty(lotNumber))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg008}"), MESSAGEBOX_CAPTION);//序列号不能为空
                //MessageService.ShowMessage("序列号不能为空","提示");
                this.beLotNumber.SelectAll();
                return false;
            }

            DataSet dsLot = this._queryEntity.GetLotInfo(lotNumber);
            if (!string.IsNullOrEmpty(this._queryEntity.ErrorMsg))
            {
                MessageService.ShowMessage(this._queryEntity.ErrorMsg, "提示");
                return false;
            }
            if (dsLot == null || dsLot.Tables.Count <= 0 || dsLot.Tables[0].Rows.Count <= 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg010}"), MESSAGEBOX_CAPTION);//序列号不存在
                //MessageService.ShowMessage("序列号不存在。", "提示");
                this.beLotNumber.SelectAll();
                return false;
            }
            DataRow drLotInfo = dsLot.Tables[0].Rows[0];
            //判断批次号在指定车间中是否存在。
            string currentRoomKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
            if (roomKey != currentRoomKey)
            {
                MessageService.ShowMessage(string.Format("【{0}】在当前车间中不存在，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return false;
            }
            //判断批次是否被暂停
            int holdFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_HOLD_FLAG]);
            if (holdFlag == 1 && this._model.OperationType != LotOperationType.BatchRelease
                && this._model.OperationType != LotOperationType.Release
                && this._model.OperationType != LotOperationType.Rework
                && this._model.OperationType != LotOperationType.BatchRework)

            {
                MessageService.ShowMessage(string.Format("【{0}】已被暂停，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return false;
            }
            if (holdFlag == 0 && (this._model.OperationType == LotOperationType.BatchRelease
                               || this._model.OperationType == LotOperationType.Release
                               || this._model.OperationType == LotOperationType.Rework
                               || this._model.OperationType == LotOperationType.BatchRework))
            
            {
                MessageService.ShowMessage(string.Format("【{0}】未暂停，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return false;
            }
            //判断批次是否被暂停
            string lotType = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_TYPE]);
            if (lotType == "N" && (this._model.OperationType == LotOperationType.CellPatch
                               || this._model.OperationType == LotOperationType.CellRecovered))
            {
                MessageService.ShowMessage(string.Format("批次必须是组件补片批次，请确认。"), "提示");
                this.beLotNumber.SelectAll();
                return false;
            }
            //判断批次是否被删除
            int deleteFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]);
            if (deleteFlag == 1)
            {
                MessageService.ShowMessage(string.Format("【{0}】已结束，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return false;
            }
            //判断批次是否已结束
            if (deleteFlag == 2)
            {
                MessageService.ShowMessage(string.Format("【{0}】已删除，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return false;
            }
            //判断批次是否完成。
            int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
            if (stateFlag >= 10)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.Msg001}"));//批次已完成，请确认
                //MessageBox.Show("批次已完成，请确认。");
                this.beLotNumber.SelectAll();
                return false;
            }
            string palletNo = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_PALLET_NO]);
            //如果是返修、不良、报废、批次退料、调整批次、拆分批次、合并批次或者终结批次操作。
            //批次不能被包装才能进行对应操作。


            if ((this._model.OperationType == LotOperationType.Scrap
                || this._model.OperationType == LotOperationType.Defect
                || this._model.OperationType == LotOperationType.BatchRework
                || this._model.OperationType == LotOperationType.ReturnMaterial
                || this._model.OperationType == LotOperationType.Rework
                || this._model.OperationType == LotOperationType.Adjust
                || this._model.OperationType == LotOperationType.BatchAdjust
                || this._model.OperationType == LotOperationType.Terminal
                || this._model.OperationType == LotOperationType.Split
                || this._model.OperationType == LotOperationType.Merge)
                && !string.IsNullOrEmpty(palletNo))
            {
                MessageService.ShowMessage(string.Format("【{0}】已包装，出托后才能进行对应操作。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return false;
            }

            //LotOperationDetailModel model = new LotOperationDetailModel();
            //this._model.OperationType = LotOperationType.Scrap;
            this._model.LotNumber = lotNumber;
            this._model.RoomKey = roomKey;
            this._model.RoomName = roomName;
            this._model.ShiftName = Convert.ToString(this.lueShift.EditValue);
            this._model.UserName = this.teUserNumber.Text;
            this._model.LotEditTime = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_EDIT_TIME]);
            //model.TitleName = this.lblApplicationTitle.Text;
            
            //显示结束批次对话框。
            //if (this._model.OperationType == LotOperationType.Terminal)
            //{
            //    TerminalLotDialog terminalLot = new TerminalLotDialog(model);
            //    //显示结束批次的对话框。
            //    terminalLot.ShowDialog();
            //}
            //else
            //{
            ////显示电池片操作明细界面。
            //WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            ////创建新的视图并显示
            //LotOperationDetailViewContent view = new LotOperationDetailViewContent(model);
            //WorkbenchSingleton.Workbench.ShowView(view);
            //}
            return true;
        }
        
        /// <summary>
        /// 选中批次值后的事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void LotQueryHelpDialog_OnValueSelected(object sender, LotQueryValueSelectedEventArgs args)
        {
            this.beLotNumber.Text = args.LotNumber;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (Operate())
                {
                    BindLotInfo();
                    BindTroubleStep();
                    BindReasonCodeClassToDataTable();
                    InitList();
                    ResetControlValue();
                }
            }
            finally
            {
                this.beLotNumber.Select();
                this.beLotNumber.SelectAll();
            }
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.beLotNumber.Text = string.Empty;
        }
    }
}
