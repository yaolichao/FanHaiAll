using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraTab;
using DevExpress.XtraGrid.Views.Base;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示批次事务处理历史记录的对话框类。
    /// </summary>
    public partial class LotTransactionHistoryDialog : BaseDialog
    {
        LotQueryEntity _queryEntity = new LotQueryEntity();     //批次查询对象
        private string _lotNumber = "";                         //批次号
        private DataTable _dtReasonCodeClass = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="workorderKey">工单主键。</param>
        /// <param name="lotNumber">批次号。</param>
        public LotTransactionHistoryDialog(string lotNumber)
            : base(StringParser.Parse("批次详细信息"))
        {
            InitializeComponent();
            _lotNumber = lotNumber;             //批次号
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotTransactionHistoryDialog_Load(object sender, EventArgs e)
        {
            BindLotBaseInfo();
        }
        /// <summary>
        /// 绑定批次基本信息。
        /// </summary>
        private void BindLotBaseInfo()
        {
            DataSet dsLotInfo = _queryEntity.GetLotInfo(this._lotNumber);
            if (!string.IsNullOrEmpty(_queryEntity.ErrorMsg))
            {
                MessageService.ShowError(_queryEntity.ErrorMsg);
                return;
            }
            if(null==dsLotInfo
                || dsLotInfo.Tables.Count<1
                || dsLotInfo.Tables[0].Rows.Count==0)
            {
                MessageService.ShowMessage("没有获取到相应信息。", "提示");
                return;
            }
            DataRow drLotInfo = dsLotInfo.Tables[0].Rows[0];
            this.teLotNumber.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
            this.teLotNumber.Tag = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            this.teQuantityInit.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL]);
            this.teQuantity.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
            string lotType = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_TYPE]);
            this.teLotType.Text = this.GetBaseDataDisplayText(BASEDATA_CATEGORY_NAME.Lot_Type, "NAME", "CODE", lotType);
            string createType=Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CREATE_TYPE]);
            this.teCreateType.Text = this.GetBaseDataDisplayText(BASEDATA_CATEGORY_NAME.Lot_CreateType, "NAME", "CODE", createType);
            this.teEfficiency.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_EFFICIENCY]);
            string priority = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_PRIORITY]);
            this.tePriority.Text = this.GetBaseDataDisplayText(BASEDATA_CATEGORY_NAME.Lot_Priority, "NAME", "CODE", priority);
            int reworkFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
            this.teReworkFlag.Text = reworkFlag == 0 ? "否" : Convert.ToString(reworkFlag);
            this.tePalletNo.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_PALLET_NO]);
            if(drLotInfo[POR_LOT_FIELDS.FIELD_PALLET_TIME]!=null && drLotInfo[POR_LOT_FIELDS.FIELD_PALLET_TIME]!=DBNull.Value)
            {
                DateTime dtPalletTime = Convert.ToDateTime(drLotInfo[POR_LOT_FIELDS.FIELD_PALLET_TIME]);
                this.tePalletTime.Text = dtPalletTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            string proLevel = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_PRO_LEVEL]);
            if (!string.IsNullOrEmpty(proLevel))
            {
                this.teProductGrade.Text = GetProductGradeDisplayText(proLevel);
            }
            int holdFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_HOLD_FLAG]);
            this.teHoldFlag.Text = holdFlag == 0 ? "正常" : "暂停";
            if(holdFlag == 1){
                this.teHoldFlag.BackColor = System.Drawing.Color.Red;
            }
            LotStateFlag stateFlag = (LotStateFlag)Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
            this.teStateFlag.Text = CommonUtils.GetEnumValueDescription(stateFlag);
            int deletedTermFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]);
            this.teDeletedTermFlag.Text = deletedTermFlag == 0 ? "正常" : (deletedTermFlag == 1 ? "已结束" : "已删除");
            int shippedFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_SHIPPED_FLAG]);
            this.teShippedFlag.Text = shippedFlag == 0 ? "未出货" : "已出货";
            this.teWorkOrderNumber.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
            this.teProId.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_PRO_ID]);
            this.tePartNumber.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_PART_NUMBER]);
            this.teSILot.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_SI_LOT]);
            this.teMaterialLot.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_MATERIAL_LOT]);
            this.teMaterialCode.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_MATERIAL_CODE]);
            this.teSupplier.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_SUPPLIER_NAME]);
            this.teEnterpriseName.Text = Convert.ToString(drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            this.teRouteName.Text = Convert.ToString(drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
            this.teStepName.Text = Convert.ToString(drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
            this.teCreator.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CREATOR]);
            DateTime dtCreateTime = Convert.ToDateTime(drLotInfo[POR_LOT_FIELDS.FIELD_CREATE_TIME]);
            this.teCreateTime.Text = dtCreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.teCreateOperation.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME]);
            this.teRoomName.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_FACTORYROOM_NAME]);
            this.teLineName.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);
            this.teOperateLinePre.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_OPR_LINE_PRE]);
            this.teOperateComputer.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_OPR_COMPUTER]);
            this.teEditor.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_EDITOR]);
            DateTime dtEditTime = Convert.ToDateTime(drLotInfo[POR_LOT_FIELDS.FIELD_CREATE_TIME]);
            this.teEditTime.Text = dtEditTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.teDescription.Text = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_DESCRIPTIONS]);
        }
        /// <summary>
        /// 获取基础数据的显示值。
        /// </summary>
        /// <param name="categoryName">基础数据类别名称。</param>
        /// <param name="displayColumnName">显示文本对应的列名。</param>
        /// <param name="keyColumnName">值对应的列名</param>
        /// <returns>基础数据值对应的显示值</returns>
        private string GetBaseDataDisplayText(string categoryName,string displayColumnName,string valueColumnName,string value)
        {
            string displayText = value;
            try
            {
                string[] columns = new string[] { displayColumnName };
                KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", categoryName);
                List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();
                whereCondition.Add(new KeyValuePair<string, string>(valueColumnName, value));
                DataTable dtBaseData = BaseData.GetBasicDataByCondition(columns, category, whereCondition);
                if (null != dtBaseData && dtBaseData.Rows.Count > 0)
                {
                    displayText = Convert.ToString(dtBaseData.Rows[0][displayColumnName]);
                }
            }
            catch
            {
                displayText = value;
            }
            return displayText;
        }
        /// <summary>
        /// 获取产品等级的显示值。
        /// </summary>
        /// <returns>产品等级的显示值</returns>
        private string GetProductGradeDisplayText(string value)
        {
            string displayText = value;
            try
            {
                string[] columns = new string[] { "Column_Name" };
                KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_TestRule_PowerSet");
                List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();
                whereCondition.Add(new KeyValuePair<string, string>("Column_code", value));
                whereCondition.Add(new KeyValuePair<string, string>("Column_type", "ProductGrade"));
                DataTable dtBaseData = BaseData.GetBasicDataByCondition(columns, category, whereCondition);
                if (null != dtBaseData && dtBaseData.Rows.Count > 0)
                {
                    displayText = Convert.ToString(dtBaseData.Rows[0][columns[0]]);
                }
            }
            catch
            {
                displayText = value;
            }
            return displayText;
        }
        /// <summary>
        /// 绑定历史操作记录。
        /// </summary>
        private void BindHistoryTransaction()
        {
            string lotKey = Convert.ToString(this.teLotNumber.Tag);
            DataSet dsHisTable = this._queryEntity.GetInfoForLotHistory(lotKey);
            gcMain.MainView = gvMain;
            gcMain.DataSource = dsHisTable.Tables[0];
            gvMain.BestFitColumns(); 
        }
        /// <summary>
        /// 绑定批次不良和报废信息。
        /// </summary>
        private void BindScrapAndDefectInfo()
        {
            string []columns=new string[]{"CODE","NAME"};
            this._dtReasonCodeClass=BaseData.Get(columns, BASEDATA_CATEGORY_NAME.Basic_ClassOfRCode);

            string lotKey = Convert.ToString(this.teLotNumber.Tag);
            DataSet dsScrapAndDefectQtyTable = this._queryEntity.GetScrapAndDefectQty(lotKey);
            gcScrapAndDefectQty.MainView = gvScrapAndDefectQty;
            gcScrapAndDefectQty.DataSource = dsScrapAndDefectQtyTable.Tables[0];
            gvScrapAndDefectQty.BestFitColumns(); 
        }
        /// <summary>
        /// 绑定批次工序参数数据。
        /// </summary>
        private void BindParamInfo()
        {
            string lotKey = Convert.ToString(this.teLotNumber.Tag);
            DataSet dsParamInfo = this._queryEntity.GetParamInfo(lotKey);
            gcParamInfo.MainView = gvParamInfo;
            gcParamInfo.DataSource = dsParamInfo.Tables[0];
            gvParamInfo.BestFitColumns();
        }
        /// <summary>
        /// 自定义绘制单元格。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string undoFlag = Convert.ToString(this.gvMain.GetRowCellValue(e.RowHandle, "UNDO_FLAG"));
            string activity = Convert.ToString(this.gvMain.GetRowCellValue(e.RowHandle, "ACTIVITY"));
            if (undoFlag == "1")
            {
                e.Appearance.BackColor = System.Drawing.Color.Red;
                e.Appearance.ForeColor = System.Drawing.Color.White;
            }
            if (e.Column==this.gcUndoFlag)
            {
                e.DisplayText = undoFlag == "1" ? "是" : "否";
            }
        }
        /// <summary>
        /// 关闭对话框。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 页签改变事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcBasicData_SelectedPageChanged(object sender,TabPageChangedEventArgs e)
        {
            if (e.Page == this.tbHis)
            {
                if (this.gcMain.DataSource == null)
                {
                    BindHistoryTransaction();
                }
            }
            else if (e.Page == this.tbBaoFeiAndBuLiang)
            {
                if (this.gcScrapAndDefectQty.DataSource == null)
                {
                    BindScrapAndDefectInfo();
                }
            }
            else if (e.Page == this.xtpParamInfo)
            {
                if (this.gcParamInfo.DataSource == null)
                {
                    BindParamInfo();
                }
            }
        }
        /// <summary>
        /// 自定义绘制报废和不良明细单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvScrapAndDefectQty_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gcolReasonClass && this._dtReasonCodeClass != null)
            {
                DataRow[] drs = this._dtReasonCodeClass.Select(string.Format("CODE='{0}'", e.CellValue));
                if(drs.Length>0){
                    e.DisplayText = Convert.ToString(drs[0]["NAME"]);
                }
            }
        }

       
    }
} 
