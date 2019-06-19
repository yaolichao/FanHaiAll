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
    /// 表示批次操作（电池片回收）明细的控件类。电池片回收是用于撤销电池片报废和电池片补片。
    /// </summary>
    /// <remarks>
    /// （1）电池片回收对应的批次号必须做过电池片报废或电池片补片。
    /// （2）回收补片造成缺失的电池片：对应的问题序列号必须是电池片补片的问题序列号。
    /// （3）回收报废造成缺失的电池片：对应的问题序列号是电池片报废的问题序列号。
    /// （4）回收补片造成缺失的电池片：补片批次号将缺失的电池片回加。
    /// （5）回收报废造成缺失的电池片：报废批次号将缺失的电池片回加。
    /// （6）电池片回收的问题工序要和"报废"/"补片"的问题工序相一致。
    /// （7）被回收批次未结束、未删除、未暂停。
    /// </remarks>
    public partial class LotOperationRecovered : BaseUserCtrl
    {
        /// <summary>
        /// 回收原因列表中使用的临时字段。用于存放问题的数量。
        /// </summary>
        private const string LIST_TEMP_FIELD_SCRAP_PATCH_SUM_QUANTITY = "SCRAP_PATCH_SUM_QUANTITY";
        /// <summary>
        /// 回收原因列表中使用的临时字段。用于存放已回收的数量。
        /// </summary>
        private const string LIST_TEMP_FIELD_RECOVERED_SUM_QUANTITY = "RECOVERED_SUM_QUANTITY";
        /// <summary>
        /// 回收原因列表中使用的临时字段。用于存放待回收批次信息主键。
        /// </summary>
        private const string LIST_TEMP_FIELD_P_KEY = "P_KEY";
        /// <summary>
        /// 回收原因列表中使用的临时字段。用于存放待回收批次号。
        /// </summary>
        private const string LIST_TEMP_FIELD_LOT_NUMBER = "LOT_NUMBER";

        LotOperationDetailModel _model = null;
        IViewContent _view = null;
        LotOperationEntity _entity = new LotOperationEntity();
        DataSet dsLotInfo = null;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        public LotOperationRecovered(LotOperationDetailModel model, IViewContent view)
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
        private void LotOperationRecovered_Load(object sender, EventArgs e)
        {
            //this.lblTitle.Text = this._view.TitleName;
            BindLotInfo();
            InitList();
            ResetControlValue();
            lblMenu.Text = "生产管理>电池片管理>硅片回收";
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
            BindRecoveredTroubleStepInfo();
            BindReasonCode();
        }
        /// <summary>
        /// 绑定问题工序。
        /// </summary>
        private void BindRecoveredTroubleStepInfo()
        {
            string lotKey = Convert.ToString(this.teLotNumber.Tag);
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);  //拥有权限的工序
            DataSet dsTroubleStep = this._entity.GetRecoveredTroubleStepInfo(lotKey, operations);
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
        /// 绑定原因代码信息。
        /// </summary>
        private void BindReasonCode()
        {
            DataSet dsReasonCode=this._entity.GetRecoverReasonCode();
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
        /// 绑定待回收的问题序列号。
        /// </summary>
        /// <param name="operationKey">问题工序主键。</param>
        private void BindBeRecoveredLotNumber(string operationKey)
        {
            string recoverdLotKey = Convert.ToString(this.teLotNumber.Tag);
            DataSet dsBeRecoveredLotNo = this._entity.GetBeRecoverdLotNumber(recoverdLotKey, operationKey);
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowError(_entity.ErrorMsg);
                return;
            }
            this.rilueLotNumber.Columns.Clear();
            this.rilueLotNumber.Columns.Add(new LookUpColumnInfo(POR_LOT_FIELDS.FIELD_LOT_NUMBER, "序列号"));
            this.rilueLotNumber.Columns.Add(new LookUpColumnInfo(WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME, "问题原因"));
            this.rilueLotNumber.Columns.Add(new LookUpColumnInfo(LIST_TEMP_FIELD_SCRAP_PATCH_SUM_QUANTITY, "问题数量"));
            this.rilueLotNumber.DataSource = dsBeRecoveredLotNo.Tables[0];
            this.rilueLotNumber.DisplayMember = POR_LOT_FIELDS.FIELD_LOT_NUMBER;
            this.rilueLotNumber.ValueMember = LIST_TEMP_FIELD_P_KEY;
        }
        /// <summary>
        /// 初始化原因列表。
        /// </summary>
        private void InitList()
        {
            DataTable dtList = CommonUtils.CreateDataTable(new WIP_RECOVERED_FIELDS());
            dtList.Columns.Add(LIST_TEMP_FIELD_P_KEY);
            dtList.Columns.Add(LIST_TEMP_FIELD_LOT_NUMBER);
            dtList.Columns.Add(LIST_TEMP_FIELD_SCRAP_PATCH_SUM_QUANTITY);
            dtList.Columns.Add(LIST_TEMP_FIELD_RECOVERED_SUM_QUANTITY);
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
            dr[WIP_RECOVERED_FIELDS.FIELD_RECOVERED_QUANTITY] = 0;
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
            //问题工序
            if (e.Column == this.gclStep)
            {
                dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_P_KEY] = string.Empty;
                dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_LOT_NUMBER] = string.Empty;
                dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_RECOVERED_SUM_QUANTITY] = DBNull.Value;
                dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_SCRAP_PATCH_SUM_QUANTITY] = DBNull.Value;
                dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_LOT_KEY] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_REASON_CODE_KEY] = DBNull.Value;
                dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_REASON_CODE_NAME] = DBNull.Value;
                dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_RECOVERED_TYPE] = DBNull.Value;
                dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_REASON_CODE_KEY] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_REASON_CODE_NAME] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_RECOVERED_QUANTITY] = 0;
                string stepKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_STEP_KEY]);
                string stepName = Convert.ToString(this.rilueStep.GetDisplayValueByKeyValue(stepKey));

                int rowIndex = this.rilueStep.GetDataSourceRowIndex(POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY, stepKey);
                string enterpriseKey = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, rowIndex));
                string enterprisenName = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME, rowIndex));
                string routeKey = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY, rowIndex));
                string routeName = Convert.ToString(this.rilueStep.GetDataSourceValue(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME, rowIndex));
                dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_STEP_NAME] = stepName;
                dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_ENTERPRISE_KEY] = enterpriseKey;
                dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_ENTERPRISE_NAME] = enterprisenName;
                dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_ROUTE_KEY] = routeKey;
                dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_ROUTE_NAME] = routeName;
            }
            //如果是问题序列号列和原因名称列
            else if (e.Column == this.gclReasonCode || e.Column==this.gclLotNumber)
            {
                //回收原因信息
                string codeKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_REASON_CODE_KEY]);                //回收原因主键
                string codeName = Convert.ToString(this.rilueReasonCode.GetDisplayValueByKeyValue(codeKey));                            //回收原因名称
                //被回收信息
                string pkey = Convert.ToString(dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_P_KEY]);                                        //被回收信息的主键
                int rowIndex = this.rilueLotNumber.GetDataSourceRowIndex(LIST_TEMP_FIELD_P_KEY, pkey);
                string lotNumber = Convert.ToString(this.rilueLotNumber.GetDisplayValueByKeyValue(pkey));                               //批次号
                string lotKey = Convert.ToString(this.rilueLotNumber.GetDataSourceValue(POR_LOT_FIELDS.FIELD_LOT_KEY, rowIndex));       //批次主键
                string beRecoveredCodeKey = Convert.ToString(this.rilueLotNumber.GetDataSourceValue(WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY, rowIndex));  //问题原因主键
                string beRecoveredCodeName = Convert.ToString(this.rilueLotNumber.GetDataSourceValue(WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME, rowIndex));//问题原因名称
                //问题工序信息
                string stepKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_STEP_KEY]);
                string stepName = Convert.ToString(this.rilueStep.GetDisplayValueByKeyValue(stepKey));
                //判断问题工序+问题序列号+问题原因+回收原因代码在列表中是否已经存在。
                if (!string.IsNullOrEmpty(pkey) && !string.IsNullOrEmpty(beRecoveredCodeKey) && !string.IsNullOrEmpty(codeKey))
                {
                    int count = dtList.AsEnumerable().Count(dr => Convert.ToString(dr[WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_LOT_KEY]) == lotKey
                                                                  && Convert.ToString(dr[WIP_RECOVERED_FIELDS.FIELD_STEP_KEY]) == stepKey
                                                                  && Convert.ToString(dr[WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_REASON_CODE_KEY]) == beRecoveredCodeKey
                                                                  && Convert.ToString(dr[WIP_RECOVERED_FIELDS.FIELD_REASON_CODE_KEY]) == codeKey);
                    //问题序列号+原因代码在列表中已经存在
                    if (count > 1)
                    {
                        MessageService.ShowMessage(string.Format("问题工序【{0}】+问题序列号【{1}】+问题原因【{2}】+回收原因【{3}】已在列表中存在，请重新选择。",
                                                                stepName,
                                                                lotNumber,
                                                                beRecoveredCodeName,
                                                                codeName), "提示");
                        //如果是原因名称列，设置原因名称为空。
                        if (e.Column == this.gclReasonCode)
                        {
                            dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_REASON_CODE_KEY] = string.Empty;
                            dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_REASON_CODE_NAME] = string.Empty;
                        }
                        else//问题序列号列
                        {
                            dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_P_KEY] = string.Empty;
                            dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_LOT_NUMBER] = string.Empty;
                            dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_RECOVERED_SUM_QUANTITY] = DBNull.Value;
                            dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_SCRAP_PATCH_SUM_QUANTITY] = DBNull.Value;
                            dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_LOT_KEY] = string.Empty;
                            dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_REASON_CODE_KEY] = DBNull.Value;
                            dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_REASON_CODE_NAME] = DBNull.Value;
                            dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_RECOVERED_TYPE] = DBNull.Value;
                            dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                        }
                        this.gvList.FocusedColumn = e.Column;
                        this.gvList.ShowEditor();
                        return;
                    }
                }
                //如果是原因名称列，设置原因名称。
                if (e.Column == this.gclReasonCode)
                {
                    dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_REASON_CODE_NAME] = codeName;
                }
                //如果是问题序列号列，设置问题序列号对应的初始数量和当前数量
                if (e.Column == this.gclLotNumber && rowIndex>=0)
                {
                    string editTime = Convert.ToString(this.rilueLotNumber.GetDataSourceValue(POR_LOT_FIELDS.FIELD_EDIT_TIME, rowIndex));
                    object recoveredSumQty = Convert.ToString(this.rilueLotNumber.GetDataSourceValue(LIST_TEMP_FIELD_RECOVERED_SUM_QUANTITY, rowIndex));
                    object scrapPatchSumQty = Convert.ToString(this.rilueLotNumber.GetDataSourceValue(LIST_TEMP_FIELD_SCRAP_PATCH_SUM_QUANTITY, rowIndex));
                    object activity = Convert.ToString(this.rilueLotNumber.GetDataSourceValue(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, rowIndex));
                    dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_LOT_KEY] = lotKey;
                    dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_LOT_NUMBER] = lotNumber;
                    dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_RECOVERED_TYPE] = activity;
                    dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_REASON_CODE_KEY] = beRecoveredCodeKey;
                    dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_REASON_CODE_NAME] = beRecoveredCodeName;
                    dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_RECOVERED_SUM_QUANTITY] = recoveredSumQty;
                    dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_SCRAP_PATCH_SUM_QUANTITY] = scrapPatchSumQty;
                    //设置问题序列号批次信息最后的编辑时间，以便判断问题序列号信息是否过期。
                    dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_EDIT_TIME] = editTime;
                }
            }
            //判断回收数量+已回收数量是否超过了回收信息对应的问题数量
            if (e.Column == this.gclLotNumber || e.Column == this.gclQty)
            {
                string pkey = Convert.ToString(dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_P_KEY]);                                        //被回收信息的主键
                if (string.IsNullOrEmpty(pkey)) return;
                string lotNumber = Convert.ToString(this.rilueLotNumber.GetDisplayValueByKeyValue(pkey));                               //批次号
                //获取问题序列号的初始数量，当前数量和总的回收数量
                var lnq=from item in dtList.AsEnumerable()
                        where Convert.ToString(item[LIST_TEMP_FIELD_P_KEY]) == pkey
                        group item by new {
                                            ScrapPatchSumQuantity = Convert.ToDouble(item[LIST_TEMP_FIELD_SCRAP_PATCH_SUM_QUANTITY] == DBNull.Value ? 0 : item[LIST_TEMP_FIELD_SCRAP_PATCH_SUM_QUANTITY]),
                                            RecoveredSumQuantity = Convert.ToDouble(item[LIST_TEMP_FIELD_RECOVERED_SUM_QUANTITY] == DBNull.Value ? 0 : item[LIST_TEMP_FIELD_RECOVERED_SUM_QUANTITY]),
                                          } into g
                        select new {
                                     ScrapPatchSumQuantity = g.Key.ScrapPatchSumQuantity,
                                     RecoveredSumQuantity = g.Key.RecoveredSumQuantity,
                                     RecoveredQuantity=g.Sum(p=>Convert.ToDouble(p[WIP_RECOVERED_FIELDS.FIELD_RECOVERED_QUANTITY]))
                                   };
                var val = lnq.First();
                //回收数量+已回收数量是否超过了回收信息对应的问题数量
                if (val != null && (val.ScrapPatchSumQuantity - val.RecoveredSumQuantity) < val.RecoveredQuantity)
                {
                    MessageService.ShowMessage(string.Format("【{0}】的\"回收数量\"不能大于\"问题数量\"-\"已回收数量\"，请确认。", lotNumber), "提示");
                    if (e.Column == this.gclQty)
                    {
                        dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_RECOVERED_QUANTITY] = 0;
                    }
                    else
                    {
                        dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_P_KEY] = string.Empty;
                        dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_RECOVERED_SUM_QUANTITY] = DBNull.Value;
                        dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_SCRAP_PATCH_SUM_QUANTITY] = DBNull.Value;
                        dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_LOT_KEY] = string.Empty;
                        dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_REASON_CODE_KEY] = DBNull.Value;
                        dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_REASON_CODE_NAME] = DBNull.Value;
                        dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_RECOVERED_TYPE] = DBNull.Value;
                        dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                    }
                    this.gvList.FocusedColumn = e.Column;
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
            dr[WIP_RECOVERED_FIELDS.FIELD_RECOVERED_QUANTITY] = 0;
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
                MessageService.ShowMessage("请选择要移除的回收原因信息。", "提示");
                return;
            }
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList.Rows.Count <=1)
            {
                MessageService.ShowMessage("回收原因列表中必须至少有一条记录。", "提示");
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
                e.DisplayText = Convert.ToString(dtList.Rows[e.RowHandle][WIP_RECOVERED_FIELDS.FIELD_REASON_CODE_NAME]);
            }
            else if (e.Column == this.gclLotNumber)
            {
                DataTable dtList = this.gcList.DataSource as DataTable;
                e.DisplayText = Convert.ToString(dtList.Rows[e.RowHandle][LIST_TEMP_FIELD_LOT_NUMBER]);
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
            //回收原因信息。
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList==null || dtList.Rows.Count < 1)
            {
                MessageService.ShowMessage("回收原因列表中至少必须有一条记录。", "提示");
                return;
            }
            //回收问题工序必须全部输入
            List<DataRow> lst = (from item in dtList.AsEnumerable()
                                 where string.IsNullOrEmpty(Convert.ToString(item[WIP_RECOVERED_FIELDS.FIELD_STEP_KEY]))
                                 select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("回收原因列表中的【问题工序】必须输入。", "提示");
                this.gvList.FocusedColumn = this.gclStep;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
                return;
            }
            //回收问题序列号必须全部输入
            lst = (from item in dtList.AsEnumerable()
                 where string.IsNullOrEmpty(Convert.ToString(item[WIP_RECOVERED_FIELDS.FIELD_BERECOVERED_LOT_KEY]))
                 select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("回收原因列表中的【问题序列号】必须输入。", "提示");
                this.gvList.FocusedColumn = this.gclLotNumber;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
                return;
            }
            //回收原因必须全部输入
            lst=(from item in dtList.AsEnumerable()
                 where string.IsNullOrEmpty(Convert.ToString(item[WIP_RECOVERED_FIELDS.FIELD_REASON_CODE_KEY]))
                 select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("回收原因列表中的【回收原因】必须输入。", "提示");
                this.gvList.FocusedColumn = this.gclReasonCode;
                this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                this.gvList.ShowEditor();
                return;
            }
            //回收原因中的数量必须输入且大于0
            lst=(from item in dtList.AsEnumerable()
                 where string.IsNullOrEmpty(Convert.ToString(item[WIP_RECOVERED_FIELDS.FIELD_RECOVERED_QUANTITY]).Trim())
                     || Convert.ToInt32(item[WIP_RECOVERED_FIELDS.FIELD_RECOVERED_QUANTITY]) <= 0
                 select item).ToList<DataRow>();
            if (lst.Count() > 0)
            {
                MessageService.ShowMessage("回收数量必须输入且大于0。", "提示");
                this.gvList.FocusedColumn = this.gclQty;
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
            double initQuantity = Convert.ToDouble(drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL]);
            double qty = Convert.ToDouble(drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
            //当前电池片数量+回收电池片报废的数量总和不能超过初始数量。
            double recoveredQty = dtList.AsEnumerable()
                                        .Sum(dr => Convert.ToDouble(dr[WIP_RECOVERED_FIELDS.FIELD_RECOVERED_QUANTITY]));
            double leftQty = qty + recoveredQty;
            if (leftQty > initQuantity)
            {
                MessageService.ShowMessage(string.Format("当前电池片数量+回收电池片报废数量的总和不能超过【{0}】的初始数量({1})。", this.teLotNumber.Text, initQuantity), "提示");
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
            //组织回收数据。
            Hashtable htTransaction=new Hashtable();
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY,lotKey);
            htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY,ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RECOVERED);
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
            //组织回收原因数据
            DataTable dtRecovery = dtList.Copy();
            dtRecovery.TableName = WIP_RECOVERED_FIELDS.DATABASE_TABLE_NAME;
            dtRecovery.Columns.Remove(LIST_TEMP_FIELD_P_KEY);                      //删除临时列
            dtRecovery.Columns.Remove(LIST_TEMP_FIELD_SCRAP_PATCH_SUM_QUANTITY);
            dtRecovery.Columns.Remove(LIST_TEMP_FIELD_RECOVERED_SUM_QUANTITY);
            dtRecovery.Columns.Remove(LIST_TEMP_FIELD_LOT_NUMBER);
            foreach (DataRow dr in dtRecovery.Rows)
            {
                dr[WIP_RECOVERED_FIELDS.FIELD_RECOVERED_LOT_KEY] = lotKey;
                dr[WIP_RECOVERED_FIELDS.FIELD_EDITOR] = this._model.UserName;
                dr[WIP_RECOVERED_FIELDS.FIELD_EDIT_TIMEZONE_KEY] = timezone;
            }
            dsParams.Tables.Add(dtRecovery);
            //组织其他附加参数数据
            Hashtable htMaindata = new Hashtable();
            htMaindata.Add(COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, this._model.LotEditTime);
            DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
            dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
            dsParams.Tables.Add(dtParams);
            //执行回收。
            this._entity.LotRecovered(dsParams);
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
            dtRecovery = null;
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
            //绑定问题序列号。
            if (this.gvList.FocusedColumn == this.gclLotNumber && this.gvList.FocusedRowHandle >= 0)
            {
                DataTable dtList = this.gcList.DataSource as DataTable;
                string stepKey = Convert.ToString(dtList.Rows[this.gvList.FocusedRowHandle][WIP_RECOVERED_FIELDS.FIELD_STEP_KEY]);
                if (!string.IsNullOrEmpty(stepKey))
                {
                    BindBeRecoveredLotNumber(stepKey);
                }
                else
                {
                    this.rilueLotNumber.DataSource = null;
                }
            }
        }
    }
}
