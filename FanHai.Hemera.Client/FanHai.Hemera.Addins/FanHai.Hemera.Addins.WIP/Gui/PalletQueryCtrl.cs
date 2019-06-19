using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core.Services;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.CommonControls;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.Controls;
using DevExpress.XtraEditors.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraPrinting;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Printing;
using DevExpress.XtraGrid.Columns;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WIP
{
    
    /// <summary>
    /// 表示批次查询的用户控件。
    /// </summary>
    public partial class PalletQueryCtrl :BaseUserCtrl
    {
        IViewContent _viewContent;
        PalletQueryEntity _entity = new PalletQueryEntity();
        //PalletQueryConditionDialog _queryConditionDlg = null;
        PalletQueryConditionModel Model = null;
        DataTable _dtProductGrade = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public PalletQueryCtrl(IViewContent viewContent)
        {
            InitializeComponent();
            _viewContent = viewContent;
            InitializeLanguage();
            GridViewHelper.SetGridView(gvPallet);
            GridViewHelper.SetGridView(gvPalletDetail);
        }


        private void InitializeLanguage()
        {
            this.gcolLotRowNum.Caption = StringParser.Parse("${res:Global.RowNumber}");//"序号";
            this.gcolLotNumber.Caption = StringParser.Parse("${res:Global.LotNumber}");//"批次号";
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");//"查询";
            this.gcolWorkOrderNumber.Caption = StringParser.Parse("${res:Global.WorkNumber}");//工单号
            this.gcolRowNum.Caption = StringParser.Parse("${res:Global.RowNumber}");//"序号";
            this.gcolLotWorkorderNo.Caption = StringParser.Parse("${res:Global.WorkNumber}");//工单号

            this.gcolLotQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolLotQty}");//"电池片数量";
            this.gcolEFFICIENCY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolEFFICIENCY}");//"转换效率";
            this.gcolLotProId.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolLotProId}");//"产品ID号";
            this.gcolLotPartNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolLotPartNumber}");//"产品料号";
            this.gcolLotSideCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolLotSideCode}");//"侧板编号";
            this.gcolLotCustomerCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolLotCustomerCode}");//"客户编码";
            this.gcolLotLotColor.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolLotLotColor}");//"组件花色";
            this.gcolLotGradeName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolLotGradeName}");//"组件等级";
            this.gcolTTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolTTime}");//"测试时间";
            this.gcolDeviceNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolDeviceNum}");//"测试设备";
            this.gcolMODName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolMODName}");//"档位名称";
            this.gcolPalletNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolPalletNo}");//"托盘号";
            this.gcolSAPNO.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolSAPNO}");//"产品料号";
            this.gcolProId.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolProId}");//"产品ID号";
            this.gcolProLevel.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolProLevel}");//"产品等级";
            this.gcolQuantity.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolQuantity}");//"组件数量";
            this.gcolPowerLevel.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolPowerLevel}");//"功率档位";
            this.gcolTotlePower.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolTotlePower}");//"总瓦数(总功率)";
            this.gcolAvgPower.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolAvgPower}");//"平均功率";
            this.gcolLotColor.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolLotColor}");//"花色";
            this.gcolStateFlag.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolStateFlag}");//"状态";
            this.gcolPalletTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolPalletTime}");//"包装时间";
            this.gcolPalletPerson.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolPalletPerson}");//"包装人员";
            this.gcolCheckTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolCheckTime}");//"入库检时间";
            this.gcolChecker.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolChecker}");//"入库检人员";
            this.gcolToWHTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolToWHTime}");//"入库时间";
            this.gcolToWHPerson.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.gcolToWHPerson}");//"入库人员";
            //this.lciQueryResult.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.lciQueryResult}");//"查询结果";
            //this.lciPaging.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.lciPaging}");//"分页";
            this.btnExportExcel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.btnExportExcel}");//"导出本页数据到EXCEL";
            this.tsbExportDetail.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.tsbExportDetail}");//"导出本页明细数据到EXCEL";
            this.lblMenu.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PalletQueryCtrl.lblTitle}");//"托盘信息查询";
            this.lblMenu.Text = "质量管理>质量作业>托盘查询";//"托盘信息查询";
            this.Model = new PalletQueryConditionModel();
            BindFactoryRoom();
            BindStateFlag();
            InitControlValue();
        }
        
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PalletQueryCtrl_Load(object sender, EventArgs e)
        {
            this.lblMenu.Text = _viewContent.TitleName;
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
                    this.lueFactoryRoom.EditValue = Convert.ToString(dt.Rows[0]["LOCATION_KEY"]);
                    this.Model.RoomKey = Convert.ToString(dt.Rows[0]["LOCATION_KEY"]);
                }
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
            //禁用车间
            if (dt == null || dt.Rows.Count <= 1)
            {
                this.lueFactoryRoom.Properties.ReadOnly = true;
            }
        }

        /// <summary>
        /// 绑定批次状态。
        /// </summary>
        private void BindStateFlag()
        {
            //0：包装中；1：包装；2：入库检；3:已入库；4：已出货
            DataTable dtState = new DataTable();
            dtState.Columns.Add("NAME", typeof(string));
            dtState.Columns.Add("CODE", typeof(string));
            dtState.Rows.Add("全部", string.Empty);
            dtState.Rows.Add("包装中", 0);
            dtState.Rows.Add("待入库检", 1);
            dtState.Rows.Add("待入库", 2);
            dtState.Rows.Add("已入库", 3);
            dtState.Rows.Add("已出货", 4);
            lueState.Properties.DataSource = dtState;
            lueState.Properties.DisplayMember = "NAME";
            lueState.Properties.ValueMember = "CODE";
        }
        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlValue()
        {
            if (!string.IsNullOrEmpty(this.Model.RoomKey))
            {
                this.lueFactoryRoom.EditValue = this.Model.RoomKey;
            }
            this.tePalletNo.Text = this.Model.PalletNo;
            this.tePalletNoEnd.Text = this.Model.PalletNo1;
            this.lueState.EditValue = this.Model.StateFlag;
            this.teWorkOrderNumber.Text = this.Model.WorkOrderNumber;
            this.teProId.Text = this.Model.ProId;

            this.deStartCreateDate.Text = this.Model.StartCreateDate;
            this.deEndCreateDate.Text = this.Model.EndCreateDate;

            this.deToWarehouseCheckDateStart.Text = this.Model.StartCheckDate;
            this.deToWarehouseCheckDateEnd.Text = this.Model.EndCheckDate;

            this.deToWarehouseDateStart.Text = this.Model.StartTowarehouseDate;
            this.deToWarehoseDateEnd.Text = this.Model.EndTowarehouseDate;
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
                if (this._dtProductGrade == null)
                {
                    string[] columns = new string[] { "Column_Name", "Column_code" };
                    KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_TestRule_PowerSet");
                    List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();
                    whereCondition.Add(new KeyValuePair<string, string>("Column_type", "ProductGrade"));
                    this._dtProductGrade = BaseData.GetBasicDataByCondition(columns, category, whereCondition);
                }
                if (null != this._dtProductGrade)
                {
                    DataRow[] drs = this._dtProductGrade.Select(string.Format("Column_code='{0}'", value));
                    if (drs.Length > 0)
                    {
                        displayText = Convert.ToString(drs[0]["Column_Name"]);
                    }
                }
            }
            catch
            {
                displayText = value;
            }
            return displayText;
        }
        /// <summary>
        /// 绑定托盘信息。
        /// </summary>
        private void BindPalletInfo()
        {
            PalletQueryConditionModel model = this.Model;
            Hashtable htParams = new Hashtable();
            DataSet dsParams = new DataSet();
            //车间主键。
            htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY, model.RoomKey);
            //批次号不为空
            if (!string.IsNullOrEmpty(model.PalletNo1) && !string.IsNullOrEmpty(model.PalletNo))
            {
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO + "_START", model.PalletNo);
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO + "_END", model.PalletNo1);
            }
            //批次号不为空
            else if (!string.IsNullOrEmpty(model.PalletNo))
            {
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO, model.PalletNo);
            }
            //工单号不为空
            if (!string.IsNullOrEmpty(model.WorkOrderNumber))
            {
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_WORKNUMBER, model.WorkOrderNumber);
            }
            //产品ID号不为空
            if (!string.IsNullOrEmpty(model.ProId))
            {
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_PRO_ID, model.ProId);
            }
            //产品料号不为空
            if (!string.IsNullOrEmpty(model.PartNumber))
            {
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO, model.PartNumber);
            }
            //状态不为空
            if (!string.IsNullOrEmpty(model.StateFlag))
            {
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP, model.StateFlag);
            }
            //创建日期-起不为空
            if (!string.IsNullOrEmpty(model.StartCreateDate))
            {
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME + "_START", model.StartCreateDate);
            }
            //创建日期-止不为空
            if (!string.IsNullOrEmpty(model.EndCreateDate))
            {
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_CREATE_TIME + "_END", model.EndCreateDate);
            }
            //入库检日期-起不为空
            if (!string.IsNullOrEmpty(model.StartCheckDate))
            {
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME + "_START", model.StartCheckDate);
            }
            //入库检日期-止不为空
            if (!string.IsNullOrEmpty(model.EndCheckDate))
            {
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME + "_END", model.EndCheckDate);
            }
            //入库日期-起不为空
            if (!string.IsNullOrEmpty(model.StartTowarehouseDate))
            {
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_TO_WH_TIME + "_START", model.StartTowarehouseDate);
            }
            //入库日期-止不为空
            if (!string.IsNullOrEmpty(model.EndTowarehouseDate))
            {
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_TO_WH_TIME + "_END", model.EndTowarehouseDate);
            }

            DataTable dtParams = CommonUtils.ParseToDataTable(htParams);
            dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dsParams.Tables.Add(dtParams);
            //查询批次
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            DataSet dsReturn = _entity.Query(dsParams, ref config);
            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowMessage(_entity.ErrorMsg);
                return;
            }
            if (dsReturn.Tables.Count > 0)
            {
                dsReturn.Tables[0].Columns.Add("ROW_NUM",typeof(Decimal));
                for(int i=0;i<dsReturn.Tables[0].Rows.Count;i++)
                {
                    DataRow dr=dsReturn.Tables[0].Rows[i];

                    dr["ROW_NUM"]= i + 1;

                    string grade = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE]);
                    dr[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE]=GetProductGradeDisplayText(grade);

                    object val = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP];
                    PalletState state = (PalletState)Convert.ToInt32(val);
                    dr[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE] = GetProductGradeDisplayText(grade);
                    dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = CommonUtils.GetEnumValueDescription(state);
                }
                gcPallet.DataSource = dsReturn.Tables[0];
                gcPallet.MainView = gvPallet;
                gvPallet.BestFitColumns();
            }
        }

        /// <summary>
        /// 批次查询Click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_queryConditionDlg == null)
                //{
                //    _queryConditionDlg = new PalletQueryConditionDialog();
                //}
                //if (_queryConditionDlg.ShowDialog() == DialogResult.OK)
                //{
                //    BindPalletInfo();
                //}
                Model = new PalletQueryConditionModel();

                string palletNo = this.tePalletNo.Text.Trim();
                string factoryRoomKey = Convert.ToString(this.lueFactoryRoom.EditValue); ;
                if (string.IsNullOrEmpty(factoryRoomKey))
                {
                    MessageService.ShowMessage("请选择车间。", "系统提示");
                    this.lueFactoryRoom.Select();
                    return;
                }
                if (palletNo.Length > 2048)
                {
                    MessageService.ShowMessage("托盘号长度不能超过2048个字符。", "系统提示");
                    this.tePalletNo.Select();
                    return;
                }
                //开始时间>结束时间
                if (this.deStartCreateDate.DateTime > this.deEndCreateDate.DateTime)
                {
                    MessageService.ShowMessage("开始时间必须小于结束时间。", "系统提示");
                    this.deStartCreateDate.Select();
                    return;
                }
                this.Model.RoomKey = factoryRoomKey;
                this.Model.RoomName = Convert.ToString(this.lueFactoryRoom.Text);
                this.Model.PalletNo = this.tePalletNo.Text.Trim();
                this.Model.PalletNo1 = this.tePalletNoEnd.Text.Trim();
                this.Model.WorkOrderNumber = this.teWorkOrderNumber.Text.ToUpper().Trim();
                this.Model.ProId = this.teProId.Text.ToUpper().Trim();
                this.Model.StateFlag = Convert.ToString(this.lueState.EditValue);

                this.Model.StartCreateDate = this.deStartCreateDate.Text;
                this.Model.EndCreateDate = this.deEndCreateDate.Text;
                this.Model.PartNumber = this.tePartNumber.Text.Trim();
                this.Model.StartTowarehouseDate = this.deToWarehouseDateStart.Text;
                this.Model.EndTowarehouseDate = this.deToWarehoseDateEnd.Text;

                this.Model.StartCheckDate = this.deToWarehouseCheckDateStart.Text;
                this.Model.EndCheckDate = this.deToWarehouseCheckDateEnd.Text;

                BindPalletInfo();
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// 关闭按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
        }
        /// <summary>
        /// 分页查询。
        /// </summary>
        private void pgnQueryResult_DataPaging()
        {
            BindPalletInfo();
        }
        /// <summary>
        /// 导出为EXCEL。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件(*.xls)|*.xls";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                XlsExportOptions options = new XlsExportOptions();
                this.gvPallet.ExportToXls(dlg.FileName,options);
            }
        }
        /// <summary>
        /// 显示数据前面的+号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvResult_MasterRowGetRelationCount(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 1;
        }

        private void gvResult_MasterRowGetChildList(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventArgs e)
        {
            DataRow dr = this.gvPallet.GetDataRow(e.RowHandle);
            string palletNo = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
            Hashtable htParams = new Hashtable();
            DataSet dsParams = new DataSet();
            htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO + "_START", palletNo);
            htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO + "_END", palletNo);
            DataTable dtParams = CommonUtils.ParseToDataTable(htParams);
            dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dsParams.Tables.Add(dtParams);
            DataSet dsLotInfo = this._entity.QueryDetail(dsParams);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }
            //dsLotInfo.Tables[0].Columns.Add("ROW_NUM",typeof(decimal));
            //for (int i = 0; i < dsLotInfo.Tables[0].Rows.Count; i++)
            //{
            //    DataRow drLot = dsLotInfo.Tables[0].Rows[i];
            //    drLot["ROW_NUM"] = i + 1;
            //}
            e.ChildList = dsLotInfo.Tables[0].DefaultView;
            this.gvPalletDetail.OptionsView.ColumnAutoWidth = false;
            this.gvPalletDetail.BestFitColumns();

        }

        private void gvResult_MasterRowEmpty(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowEmptyEventArgs e)
        {
            e.IsEmpty = false;
        }

        private void gvResult_MasterRowGetRelationName(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            e.RelationName = "Detail";
        }

        private void gvResult_MasterRowGetRelationDisplayCaption(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            e.RelationName = "Detail data";
        }
        /// <summary>
        /// 导出明细数据到EXCEL。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbExportDetail_Click(object sender, EventArgs e)
        {
            StringBuilder sbPalletNo = new StringBuilder();
            DataTable dt = this.gcPallet.DataSource as DataTable;
            foreach (DataRow dr in dt.Rows)
            {
                sbPalletNo.AppendFormat("{0},", dr[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
            }
            Hashtable htParams = new Hashtable();
            DataSet dsParams = new DataSet();
            htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO, sbPalletNo.ToString().TrimEnd(','));
            DataTable dtParams = CommonUtils.ParseToDataTable(htParams);
            dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dsParams.Tables.Add(dtParams);
            DataSet dsReturn = this._entity.QueryDetail(dsParams);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件(*.xls)|*.xls";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            List<string> lst = new List<string>();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DataTable dtReturn = dsReturn.Tables[0];
                //StringBuilder sbFieldNames = new StringBuilder();
                foreach (GridColumn gc in this.gvPallet.Columns)
                {
                    if (dtReturn.Columns.Contains(gc.FieldName))
                    {
                        dtReturn.Columns[gc.FieldName].Caption = gc.Caption;
                        if (gc.FieldName ==this.gcolPalletNo.FieldName)
                        {
                            lst.Insert(0, gc.FieldName);
                        }
                        else
                        {
                            lst.Add(gc.FieldName);
                        }
                    }
                }
                foreach (GridColumn gc in this.gvPalletDetail.Columns)
                {
                    if (dtReturn.Columns.Contains(gc.FieldName))
                    {
                        dtReturn.Columns[gc.FieldName].Caption = gc.Caption;
                        if (gc.FieldName == this.gcolLotNumber.FieldName)
                        {
                            lst.Insert(1, gc.FieldName);
                        }
                        else
                        {
                            lst.Add(gc.FieldName);
                        }
                    }
                }
                for (int i = 0; i < dtReturn.Rows.Count; i++)
                {
                    DataRow dr = dsReturn.Tables[0].Rows[i];
                    string grade = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE]);
                    dr[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE] = GetProductGradeDisplayText(grade);
                    object val = dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP];
                    PalletState state = (PalletState)Convert.ToInt32(val);
                    dr[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE] = GetProductGradeDisplayText(grade);
                    dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = CommonUtils.GetEnumValueDescription(state);
                }
                //string fileNames = sbFieldNames.ToString().TrimEnd(',');
                Export.ExportToExcel(dlg.FileName, dtReturn.DefaultView.ToTable(false, lst.ToArray()));
            }

        }

        private void gvPallet_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvPalletDetail_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}