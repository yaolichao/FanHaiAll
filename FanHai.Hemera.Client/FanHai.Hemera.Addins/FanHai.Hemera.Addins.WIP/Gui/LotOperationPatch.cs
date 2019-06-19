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
    /// 表示批次操作（电池片补片）明细的控件类。
    /// </summary>
    /// <remarks>
    /// （1）只有组件补片批才能做电池片补片。
    /// （2）组件补片批次的转换效率和问题序列号的转换效率要一致。
    /// （3）同一工单号 同一产品ID号才能进行补片。
    /// </remarks>
    public partial class LotOperationPatch : BaseUserCtrl
    {
        LotOperationDetailModel _model = null;
        IViewContent _view = null;
        LotOperationEntity _entity = new LotOperationEntity();
        DataSet dsLotInfo = null;
        LotQueryEntity _queryEntity = new LotQueryEntity();
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        public LotOperationPatch(LotOperationDetailModel model, IViewContent view)
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
        private void LotOperationPatch_Load(object sender, EventArgs e)
        {
            //this.lblTitle.Text = this._view.TitleName;
            BindLotInfo();
            BindReasonCodeClass();
            InitList();
            ResetControlValue();
            lblMenu.Text = "生产管理>电池片管理>硅片补片";
        }
        /// <summary>
        /// 绑定批次信息。
        /// </summary>
        private void BindLotInfo()
        {
            dsLotInfo = this._queryEntity.GetLotInfo(this._model.LotNumber);
            if (!string.IsNullOrEmpty(this._queryEntity.ErrorMsg))
            {
                MessageService.ShowError(this._queryEntity.ErrorMsg);
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
            //BindPatchedLotNumber();
        }
        /// <summary>
        /// 绑定问题工序。
        /// </summary>
        private void BindTroubleStep(string patchedLotKey)
        {
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);  //拥有权限的工序
            DataSet dsTroubleStep = this._entity.GetTroubleStepInfo(patchedLotKey, operations);
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowError(_entity.ErrorMsg);
                return;
            }
            this.rilueStep.Columns.Clear();
            this.rilueStep.Columns.Add(new LookUpColumnInfo(POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME, "名称"));
            this.rilueStep.DataSource = dsTroubleStep.Tables[0];
            this.rilueStep.DisplayMember = POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME;
            this.rilueStep.ValueMember = POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY;
        }
        /// <summary>
        /// 绑定原因代码分类信息。
        /// </summary>
        private void BindReasonCodeClass()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            DataTable dtReturn = BaseData.Get(columns, BASEDATA_CATEGORY_NAME.Basic_ClassOfRCode);
            if (dtReturn != null)
            {
                dtReturn.DefaultView.RowFilter = string.Format("CODE IN ({0})", BASEDATA_CATEGORY_NAME.Basic_ClassOfRCodeValue_Cell);
                dtReturn.DefaultView.Sort = "CODE ASC";
                this.rilueReasonCodeClass.Columns.Clear();
                this.rilueReasonCodeClass.Columns.Add(new LookUpColumnInfo("NAME", string.Empty));
                this.rilueReasonCodeClass.DataSource = dtReturn;
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
        ///// <summary>
        ///// 绑定待补片的问题序列号。
        ///// </summary>
        //private void BindPatchedLotNumber()
        //{
        //    string workorderNo = this.teWorkorderNo.Text;
        //    string proId = this.teProId.Text;
        //    string efficiency = this.teEfficiency.Text;

        //    DataSet dsPatchedLotNo = this._entity.GetPatchedLotNumber(workorderNo, proId, efficiency);
        //    if (!string.IsNullOrEmpty(_entity.ErrorMsg))
        //    {
        //        MessageService.ShowError(_entity.ErrorMsg);
        //        return;
        //    }
        //    this.rilueLotNumber.Columns.Add(new LookUpColumnInfo("LOT_NUMBER", "序列号"));
        //    this.rilueLotNumber.DataSource = dsPatchedLotNo.Tables[0];
        //    this.rilueLotNumber.DisplayMember = "LOT_NUMBER";
        //    this.rilueLotNumber.ValueMember = "LOT_KEY";
            
        //}
        /// <summary>
        /// 初始化原因列表。
        /// </summary>
        private void InitList()
        {
            DataTable dtList = CommonUtils.CreateDataTable(new WIP_PATCH_FIELDS());
            dtList.Columns.Add(POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL);
            dtList.Columns.Add(POR_LOT_FIELDS.FIELD_QUANTITY);
            dtList.Columns.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER);
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
            dr[WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY] = DBNull.Value;
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
            //问题序列号
            if (e.Column == this.gclLotNumber)
            {
                string lotNumber = Convert.ToString(e.Value);
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_PATCHED_LOT_KEY] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_REASON_CODE_KEY] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_REASON_CODE_NAME] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_STEP_KEY] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_STEP_NAME] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_ENTERPRISE_KEY] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_ENTERPRISE_NAME] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_ROUTE_KEY] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_ROUTE_NAME] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY] = DBNull.Value;

                if (string.IsNullOrEmpty(lotNumber))
                {
                    return;
                }
                DataSet dsLot = this._queryEntity.GetLotInfo(lotNumber);
                if (!string.IsNullOrEmpty(this._queryEntity.ErrorMsg))
                {
                    MessageService.ShowError(this._queryEntity.ErrorMsg);
                    dtList.Rows[e.RowHandle][POR_LOT_FIELDS.FIELD_LOT_NUMBER] = string.Empty;
                    this.gvList.FocusedColumn = e.Column;
                    return;
                }
                //组件是否存在。
                if (dsLot == null || dsLot.Tables.Count <= 0 || dsLot.Tables[0].Rows.Count <= 0)
                {
                    MessageService.ShowMessage("组件序列号不存在。","提示");
                    dtList.Rows[e.RowHandle][POR_LOT_FIELDS.FIELD_LOT_NUMBER] = string.Empty;
                    this.gvList.FocusedColumn = e.Column;
                    return;
                }
                DataRow drLot = dsLot.Tables[0].Rows[0];
                string lotKey = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                int deletedTermFlag = Convert.ToInt32(drLot[POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]);
                if (deletedTermFlag!=0)
                {
                    MessageService.ShowMessage(string.Format("组件『{0}』已结束。", lotNumber), "提示");
                    dtList.Rows[e.RowHandle][POR_LOT_FIELDS.FIELD_LOT_NUMBER] = string.Empty;
                    this.gvList.FocusedColumn = e.Column;
                    return;
                }
                string workOrderNo = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                //组件的工单号是否和补片批次的工单号一致
                if (workOrderNo != this.teWorkorderNo.Text)
                {
                    MessageService.ShowMessage(string.Format("组件工单号『{0}』和补片批次工单号不一致。",workOrderNo), "提示");
                    dtList.Rows[e.RowHandle][POR_LOT_FIELDS.FIELD_LOT_NUMBER] = string.Empty;
                    this.gvList.FocusedColumn = e.Column;
                    return;
                }
                //string proId = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_PRO_ID]);
                ////组件的产品ID号是否和补片批次的产品ID号一致
                //if (proId != this.teProId.Text)
                //{
                //    MessageService.ShowMessage(string.Format("组件产品ID号『{0}』和补片批次产品ID号不一致。", proId), "提示");
                //    dtList.Rows[e.RowHandle][POR_LOT_FIELDS.FIELD_LOT_NUMBER] = string.Empty;
                //    this.gvList.FocusedColumn = e.Column;
                //    return;
                //}
                string efficiency = Convert.ToString(drLot[POR_LOT_FIELDS.FIELD_EFFICIENCY]);
                //组件的转换效率是否和补片批次的转换效率一致
                if (efficiency != this.teEfficiency.Text)
                {
                    MessageService.ShowMessage(string.Format("组件转换效率『{0}』和补片批次的转换效率不一致。", efficiency), "提示");
                    dtList.Rows[e.RowHandle][POR_LOT_FIELDS.FIELD_LOT_NUMBER]=string.Empty;
                    this.gvList.FocusedColumn = e.Column;
                    return;
                }
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_PATCHED_LOT_KEY] = drLot[POR_LOT_FIELDS.FIELD_LOT_KEY];
                //设置问题序列号对应的初始数量和当前数量
                dtList.Rows[e.RowHandle][POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL] = drLot[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL];
                dtList.Rows[e.RowHandle][POR_LOT_FIELDS.FIELD_QUANTITY] = drLot[POR_LOT_FIELDS.FIELD_QUANTITY];
                //设置问题序列号批次信息最后的编辑时间，以便判断问题序列号信息是否过期。
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_EDIT_TIME] = drLot[POR_LOT_FIELDS.FIELD_EDIT_TIME];
                drLot = null;
                dsLot = null;
            }
            //问题工序
            else if (e.Column == this.gclStep || e.Column == this.gclReasonCodeClass)
            {
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_REASON_CODE_KEY] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_REASON_CODE_NAME] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY] = DBNull.Value;
                if (e.Column == this.gclStep)
                {
                    string stepKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_STEP_KEY]);
                    string stepName = Convert.ToString(this.rilueStep.GetDisplayValueByKeyValue(stepKey));

                    int rowIndex = this.rilueStep.GetDataSourceRowIndex(POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY, stepKey);
                    string enterpriseKey = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, rowIndex));
                    string enterprisenName = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME, rowIndex));
                    string routeKey = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY, rowIndex));
                    string routeName = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME, rowIndex));
                    dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_STEP_NAME] = stepName;
                    dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_ENTERPRISE_KEY] = enterpriseKey;
                    dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_ENTERPRISE_NAME] = enterprisenName;
                    dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_ROUTE_KEY] = routeKey;
                    dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_ROUTE_NAME] = routeName;
                }
            }
            //如果是原因名称列
            else if (e.Column == this.gclReasonCode)
            {
                //string lotKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_PATCHED_LOT_KEY]);
                //string lotNumber = Convert.ToString(dtList.Rows[e.RowHandle][POR_LOT_FIELDS.FIELD_LOT_NUMBER]);

                string codeKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_REASON_CODE_KEY]);
                string codeName = Convert.ToString(this.rilueReasonCode.GetDisplayValueByKeyValue(codeKey));

                //string stepKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_STEP_KEY]);
                //string stepName = Convert.ToString(this.rilueStep.GetDisplayValueByKeyValue(stepKey));
                ////判断问题序列号+原因代码在列表中是否已经存在。
                //if (!string.IsNullOrEmpty(codeKey) && !string.IsNullOrEmpty(lotKey))
                //{
                //    int count = dtList.AsEnumerable().Count(dr => Convert.ToString(dr[WIP_PATCH_FIELDS.FIELD_PATCHED_LOT_KEY]) == lotKey
                //                                                  && Convert.ToString(dr[WIP_PATCH_FIELDS.FIELD_STEP_KEY]) == stepKey
                //                                                  && Convert.ToString(dr[WIP_PATCH_FIELDS.FIELD_REASON_CODE_KEY]) == codeKey);
                //    //问题序列号+原因代码在列表中已经存在
                //    if (count > 1)
                //    {
                //        MessageService.ShowMessage(string.Format("问题序列号【{0}】+问题工序【{1}】+原因代码【{2}】已在列表中存在，请重新选择。",
                //                                                lotNumber,
                //                                                stepName,
                //                                                codeName));
                //        dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_REASON_CODE_KEY] = string.Empty;
                //        dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_REASON_CODE_NAME] = string.Empty;
                //        this.gvList.FocusedColumn = e.Column;
                //        return;
                //    }
                //}
                dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_REASON_CODE_NAME] = codeName;
            }
            //如果是补片数量列
            else if (e.Column == this.gclQty)
            {
                //补片数量不能超过当前电池片数量
                double qty = Convert.ToDouble(this.teQty.Text);
                double scrapQty = dtList.AsEnumerable().Sum(dr => dr[WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY]==DBNull.Value? 0: Convert.ToDouble(dr[WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY]));
                double leftQty = qty - scrapQty;
                if (leftQty < 0)
                {
                    MessageService.ShowMessage("补片数量不能超过当前电池片数量。");
                    dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY] = DBNull.Value;
                    this.gvList.FocusedColumn = this.gclQty;
                    this.gvList.ShowEditor();
                    return;
                }
                ////判断补片数量之后是否超过了问题序列号的初始数量
                //string lotKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_PATCHED_LOT_KEY]);
                //if (string.IsNullOrEmpty(lotKey)) return;
                ////获取问题序列号的初始数量、当前数量、总的补片数量
                //var lnq=from item in dtList.AsEnumerable()
                //        where Convert.ToString(item[WIP_PATCH_FIELDS.FIELD_PATCHED_LOT_KEY])==lotKey
                //        group item by new { 
                //                            LotKey=item[WIP_PATCH_FIELDS.FIELD_PATCHED_LOT_KEY],
                //                            InitQuantity=Convert.ToDouble(item[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL]==DBNull.Value?0:item[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL]),
                //                            Quantity = Convert.ToDouble(item[POR_LOT_FIELDS.FIELD_QUANTITY] == DBNull.Value ? 0 : item[POR_LOT_FIELDS.FIELD_QUANTITY]),
                //                          } into g
                //        select new {
                //                     InitQuantity=g.Key.InitQuantity,
                //                     Quantity=g.Key.Quantity,
                //                     PatchQuantity=g.Sum(p=>Convert.ToDouble(p[WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY]))
                //                   };
                //var val = lnq.First();
                ////问题序列号当前数量+补片数量大于问题序列号初始数量
                //if (val!=null && (val.Quantity+val.PatchQuantity)>val.InitQuantity)
                //{
                //    MessageService.ShowMessage("\"补片数量\"+\"问题序列号当前数量\"不能超过\"问题序列号的初始数量\"。");
                //    dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY] = 0;
                //    this.gvList.FocusedColumn = e.Column;
                //    this.gvList.ShowEditor();
                //    return;
                //}
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
            if (dtList.Rows.Count > 0)
            {
                DataRow drLastest = dtList.Rows[dtList.Rows.Count - 1];
                dr[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL] = drLastest[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL];
                dr[POR_LOT_FIELDS.FIELD_QUANTITY] = drLastest[POR_LOT_FIELDS.FIELD_QUANTITY];
                dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = drLastest[POR_LOT_FIELDS.FIELD_LOT_NUMBER];
                dr[WIP_PATCH_FIELDS.FIELD_PATCHED_LOT_KEY] = drLastest[WIP_PATCH_FIELDS.FIELD_PATCHED_LOT_KEY];
                dr[WIP_PATCH_FIELDS.FIELD_REASON_CODE_KEY] = drLastest[WIP_PATCH_FIELDS.FIELD_REASON_CODE_KEY];
                dr[WIP_PATCH_FIELDS.FIELD_REASON_CODE_NAME] = drLastest[WIP_PATCH_FIELDS.FIELD_REASON_CODE_NAME];
                dr[WIP_PATCH_FIELDS.FIELD_REASON_CODE_CLASS] = drLastest[WIP_PATCH_FIELDS.FIELD_REASON_CODE_CLASS];
                dr[WIP_PATCH_FIELDS.FIELD_STEP_KEY] = drLastest[WIP_PATCH_FIELDS.FIELD_STEP_KEY];
                dr[WIP_PATCH_FIELDS.FIELD_STEP_NAME] = drLastest[WIP_PATCH_FIELDS.FIELD_STEP_NAME];
                dr[WIP_PATCH_FIELDS.FIELD_ENTERPRISE_KEY] = drLastest[WIP_PATCH_FIELDS.FIELD_ENTERPRISE_KEY];
                dr[WIP_PATCH_FIELDS.FIELD_ENTERPRISE_NAME] = drLastest[WIP_PATCH_FIELDS.FIELD_ENTERPRISE_NAME];
                dr[WIP_PATCH_FIELDS.FIELD_ROUTE_KEY] = drLastest[WIP_PATCH_FIELDS.FIELD_ROUTE_KEY];
                dr[WIP_PATCH_FIELDS.FIELD_ROUTE_NAME] = drLastest[WIP_PATCH_FIELDS.FIELD_ROUTE_NAME];
                dr[WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY] = drLastest[WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY];
                dr[WIP_PATCH_FIELDS.FIELD_DESCRIPTION] = drLastest[WIP_PATCH_FIELDS.FIELD_DESCRIPTION];
                dr[WIP_PATCH_FIELDS.FIELD_RESPONSIBLE_PERSON] = drLastest[WIP_PATCH_FIELDS.FIELD_RESPONSIBLE_PERSON];
            }
            else
            {
                dr[WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY] = DBNull.Value;
            }
            dtList.Rows.Add(dr);
            this.gvList.FocusedRowHandle = dtList.Rows.Count-1;
            this.gvList.FocusedColumn = this.gclLotNumber;
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
                MessageService.ShowMessage("补片原因列表中必须至少有一条记录。", "提示");
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
            if (e.Column == this.gclRowNum)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
            else if (e.Column == this.gclReasonCode)
            {
                DataTable dtList = this.gcList.DataSource as DataTable;
                e.DisplayText = Convert.ToString(dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_REASON_CODE_NAME]);
            }
            else if (e.Column == this.gclStep)
            {
                DataTable dtList = this.gcList.DataSource as DataTable;
                e.DisplayText = Convert.ToString(dtList.Rows[e.RowHandle][WIP_PATCH_FIELDS.FIELD_STEP_NAME]);
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
            //补片原因信息。
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList==null || dtList.Rows.Count < 1)
            {
                MessageService.ShowMessage("补片原因列表中至少必须有一条记录。", "提示");
                return;
            }
            //补片问题序列号必须全部输入
            List<DataRow> lst = (from item in dtList.AsEnumerable()
                                 where string.IsNullOrEmpty(Convert.ToString(item[WIP_PATCH_FIELDS.FIELD_PATCHED_LOT_KEY]))
                                 select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("补片原因列表中的【问题序列号】必须输入。", "提示");
                this.gvList.FocusedColumn = this.gclLotNumber;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
                return;
            }
            //问题工序必须全部输入
            lst = (from item in dtList.AsEnumerable()
                   where string.IsNullOrEmpty(Convert.ToString(item[WIP_PATCH_FIELDS.FIELD_STEP_KEY]))
                 select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("补片原因列表中的【问题工序】必须输入。", "提示");
                this.gvList.FocusedColumn = this.gclStep;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
                return;
            }
            //补片原因必须全部输入
            lst=(from item in dtList.AsEnumerable()
                 where string.IsNullOrEmpty(Convert.ToString(item[WIP_PATCH_FIELDS.FIELD_REASON_CODE_KEY]))
                 select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("补片原因列表中的【原因名称】必须输入。", "提示");
                this.gvList.FocusedColumn = this.gclReasonCode;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
                return;
            }
            //补片原因中的数量必须输入且大于0
            lst=(from item in dtList.AsEnumerable()
                 where string.IsNullOrEmpty(Convert.ToString(item[WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY]).Trim())
                     || Convert.ToInt32(item[WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY]) <= 0
                 select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("数量必须输入且大于0。", "提示");
                this.gvList.FocusedColumn = this.gclQty;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
                return;
            }
            //责任人必须全部输入
            lst = (from item in dtList.AsEnumerable()
                   where string.IsNullOrEmpty(Convert.ToString(item[WIP_PATCH_FIELDS.FIELD_RESPONSIBLE_PERSON]).Trim())
                   select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("补片原因列表中的【责任人】必须输入。", "提示");
                this.gvList.FocusedColumn = this.gclResponsiblePerson;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
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
            //补片数量总和不能超过当前电池片数量
            double patchQty = dtList.AsEnumerable().Sum(dr =>dr[WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY]==DBNull.Value?0:Convert.ToDouble(dr[WIP_PATCH_FIELDS.FIELD_PATCH_QUANTITY]));
            double leftQty = qty - patchQty;
            if (leftQty < 0)
            {
                MessageService.ShowMessage("补片数量不能超过当前电池片数量。", "提示");
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
            //Shift shiftEntity=new Shift();
            string shiftKey = string.Empty;// shiftEntity.IsShiftValueExists(shiftName);//班次主键。
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
            //组织补片数据。
            Hashtable htTransaction=new Hashtable();
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY,lotKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY,ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PATCH);
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
            //组织补片原因数据
            DataTable dtPatch = dtList.Copy();
            dtPatch.TableName = WIP_PATCH_FIELDS.DATABASE_TABLE_NAME;
            dtPatch.Columns.Remove(POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL);      //删除初始数量列。
            dtPatch.Columns.Remove(POR_LOT_FIELDS.FIELD_QUANTITY);              //删除当前数量列。
            dtPatch.Columns.Remove(POR_LOT_FIELDS.FIELD_LOT_NUMBER);            //删除批次号列。
            foreach (DataRow dr in dtPatch.Rows)
            {
                dr[WIP_PATCH_FIELDS.FIELD_PATCH_LOT_KEY] = lotKey;
                dr[WIP_PATCH_FIELDS.FIELD_EDITOR] = this._model.UserName;
                dr[WIP_PATCH_FIELDS.FIELD_EDIT_TIMEZONE_KEY] = timezone;
            }
            dsParams.Tables.Add(dtPatch);
            //组织其他附加参数数据
            Hashtable htMaindata = new Hashtable();
            htMaindata.Add(COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, this._model.LotEditTime);
            DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
            dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
            dsParams.Tables.Add(dtParams);
            //执行补片。
            this._entity.LotPatch(dsParams);
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
            dtPatch = null;
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
            //绑定问题工序。
            if (this.gvList.FocusedColumn == this.gclStep && this.gvList.FocusedRowHandle >= 0)
            {
                DataTable dtList = this.gcList.DataSource as DataTable;
                string patchedLotKey = Convert.ToString(dtList.Rows[this.gvList.FocusedRowHandle][WIP_PATCH_FIELDS.FIELD_PATCHED_LOT_KEY]);
                if (!string.IsNullOrEmpty(patchedLotKey))
                {
                    BindTroubleStep(patchedLotKey);
                }
                else
                {
                    this.rilueStep.DataSource = null;
                }
            }
            //绑定原因代码。
            else if (this.gvList.FocusedColumn == this.gclReasonCode && this.gvList.FocusedRowHandle >= 0)
            {
                DataTable dtList = this.gcList.DataSource as DataTable;
                string stepKey = Convert.ToString(dtList.Rows[this.gvList.FocusedRowHandle][WIP_PATCH_FIELDS.FIELD_STEP_KEY]);
                string codeClass = Convert.ToString(dtList.Rows[this.gvList.FocusedRowHandle][WIP_PATCH_FIELDS.FIELD_REASON_CODE_CLASS]);
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
        /// <summary>
        /// 处理补片列表的回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcList_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (gvList.IsGroupRow(gvList.FocusedRowHandle)) return;
            if (e.KeyCode == Keys.Enter && this.gvList.FocusedRowHandle >= 0 && this.gvList.FocusedColumn!=null)
            {
                int rowHandle = this.gvList.FocusedRowHandle;
                int colHandle = this.gvList.FocusedColumn.AbsoluteIndex;
                if (colHandle == this.gvList.VisibleColumns.Count - 1)
                {
                    this.gvList.FocusedColumn = this.gvList.VisibleColumns[0];

                    if (rowHandle == this.gvList.DataRowCount - 1)
                    {
                        this.gvList.FocusedRowHandle = 0;
                    }
                    else
                    {
                        this.gvList.FocusedRowHandle = rowHandle + 1;
                    }
                }
                else
                {
                    this.gvList.FocusedColumn = this.gvList.VisibleColumns[colHandle + 1];
                }
                this.gvList.ShowEditor();
                e.Handled = true;
            }
        }
    
    }
}
