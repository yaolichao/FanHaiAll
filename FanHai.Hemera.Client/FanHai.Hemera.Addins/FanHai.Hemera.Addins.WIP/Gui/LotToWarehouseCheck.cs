using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Core;
using System.Collections;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraLayout.Utils;
using System.Net;
using System.IO;
using System.Reflection;
using System.Linq;
using FanHai.Hemera.Share.Common;
using System.Threading;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotToWarehouseCheck : BaseUserCtrl
    {
        LotDispatchDetailModel _model = null;                           //参数数据。
        IViewContent _view = null;
        LotOperationEntity _lotEntity = new LotOperationEntity();
        /// <summary>
        /// 列名 GRADE_NAME
        /// </summary>
        private const string COLNAME_GRADE_NAME = "GRADE_NAME";
        /// <summary>
        /// 列名 SEQ
        /// </summary>
        private const string COLNAME_SEQ = "SEQ";
        /// <summary>
        /// 是否完成入库检验。
        /// </summary>
        private const string COLNAME_IS_CHECK = "IS_CHECK";
        /// <summary>
        /// 暂存包装数据。
        /// </summary>
        DataTable dtPackage = null;
        /// <summary>
        /// 暂存包装明细数据。
        /// </summary>
        DataTable dtPackageDetail = null;
        /// <summary>
        /// 存放工序主键。
        /// </summary>
        private string _operationKey = string.Empty;
        /// <summary>
        /// 入库检验时间。
        /// </summary>
        private DateTime _checkTime = DateTime.Now;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotToWarehouseCheck()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="view"></param>
        public LotToWarehouseCheck(LotDispatchDetailModel model, IViewContent view):this()
        {
            _model = model;
            _view = view;
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotToWarehouseCheck_Load(object sender, EventArgs e)
        {
            lblMenu.Text = "生产管理>过站管理>过站作业-入库检验";
            //绑定线别，站别和设备
            lblLine.Text = this._model.LineName;
            lblWork.Text = this._model.OperationName;
            lblEquipment.Text = this._model.EquipmentName;
            SetOperationKey();
            InitControlsValue();
            txtPalletNo.SelectAll();
            txtPalletNo.Select();
        }
        /// <summary>
        /// 根据工序名称设置工序主键。
        /// </summary>
        private void SetOperationKey()
        {
            this._operationKey = this._lotEntity.GetOperationKey(this._model.OperationName);
            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            {
                MessageService.ShowError(this._lotEntity.ErrorMsg);
            }
        }
        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlsValue()
        {
            this.btnSave.Enabled = true;
            this._checkTime = Utils.Common.Utils.GetCurrentDateTime();
            this.txtPalletNo.Properties.ReadOnly = false;
            //this.lblPalletNoTitle.Visible = false;
            //this.lblLotNoTitle.Visible = false;
            //this.lblSumLotNo.Visible = false;
            //this.lblSumPalletNo.Visible = false;
            //this.lblSumPalletNo.Text = string.Empty;
            //this.lblSumLotNo.Text = string.Empty;
            this.txtLotNum.Text = string.Empty;
            this.txtPallet_Qty.Text = string.Empty;
            this.txtCheck_Qty.Text = string.Empty;
            if (dtPackage != null)
            {
                dtPackage.Clear();
                dtPackage = null;
            }
            if (dtPackageDetail != null)
            {
                dtPackageDetail.Clear();
                dtPackageDetail = null;
            }
            this.gcWipConsigment.DataSource = null;
        }
        /// <summary>
        /// 托盘号回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPalletNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtPalletNo.BackColor = Color.White;
            if (e.KeyChar != 13 || this.txtPalletNo.Properties.ReadOnly==true)
            {
                return;
            }
            if (!InitPackageData())
            {
                InitControlsValue();
                this.txtPalletNo.Select();
                this.txtPalletNo.SelectAll();
                return;
            }
            //根据托号直接带出序列号，遍历执行txtLotNum事件
            if (Is_OneKeyDispatch.Checked)
            {
                foreach (DataRow row in dtPackageDetail.Rows)
                {
                    string lotNum = row["LOT_NUMBER"].ToString();
                    checkLotNum(lotNum);
                }
            }
            else
            {
                //this.txtLotNum.Select();
                //this.txtLotNum.SelectAll();
            }
        }
        /// <summary>
        /// 初始化并检查包装数据。
        /// </summary>
        /// <returns></returns>
        private bool InitPackageData()
        {
            InitControlsValue();
            string palletNo = this.txtPalletNo.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(palletNo))
            {
                MessageService.ShowMessage("托盘号不能为空。", "提示");
                return false;
            }
            //获取包装数据。
            DataSet dsReturn = this._lotEntity.GetPackageData(palletNo);
            if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
            {
                MessageService.ShowError(this._lotEntity.ErrorMsg);
                return false;
            }
            //查询包装数据出错
            if (dsReturn == null
                || dsReturn.Tables.Contains(WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME) == false
                || dsReturn.Tables.Contains(POR_LOT_FIELDS.DATABASE_TABLE_NAME) == false)
            {
                MessageService.ShowError(string.Format("查询包装数据出错，请重试", palletNo));
                return false;
            }
            dtPackage = dsReturn.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
            //绑定明细数据。
            dtPackageDetail = dsReturn.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
            if (dtPackage.Rows.Count <= 0)
            {//不存在托盘数据，给出对应提示。
                MessageService.ShowError(string.Format("托号【{0}】不正确，请确认!", txtPalletNo.Text.Trim()));
                return false;
            }
            DataRow drPackage = dtPackage.Rows[0];
            int csDataGroup = Convert.ToInt32(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]);
            //检查包装状态。
            if (csDataGroup == 0)
            {
                MessageService.ShowError(string.Format("托号【{0}】还未包装完成，请确认。", palletNo));
                return false;
            }
            else if (csDataGroup == 10)
            {
                MessageService.ShowError(string.Format("托号【{0}】已经从入库检返到【包装】工序，请通知包装出托作业。", palletNo));
                return false;
            }
            else if (csDataGroup == 2)
            {
                MessageService.ShowError(string.Format("托号【{0}】已经检验，请确认。", palletNo));
                return false;
            }
            else if (csDataGroup == 3)
            {
                MessageService.ShowError(string.Format("托号【{0}】已经【入库】，请确认。", palletNo));
                return false;
            }
            else if (csDataGroup == 4)
            {
                MessageService.ShowError(string.Format("托号【{0}】已经【出货】，请确认。", palletNo));
                return false;
            }
            //设置包装数量。
            string palletQty = Convert.ToString(drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_NUMBER_QTY]);
            this.txtPallet_Qty.Text = palletQty;
            dtPackageDetail.Columns.Add(COLNAME_IS_CHECK, typeof(bool));
            foreach (DataRow dr in dtPackageDetail.Rows)
            {
                dr[COLNAME_IS_CHECK] = false;
            }
            //初始化检验明细。
            this.gcWipConsigment.DataSource = dtPackageDetail;
            this.gvWipConsigment.BestFitColumns();
            this.txtPalletNo.Properties.ReadOnly = true;
            return true;
        }
        /// <summary>
        /// 批次号回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLotNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            string lotNumber = this.txtLotNum.Text.Trim();
            if (e.KeyChar == 13 && !string.IsNullOrEmpty(lotNumber))
            {
                //if (dtPackage == null && !InitPackageData())
                //{
                //    this.txtLotNum.Text = string.Empty;
                //    this.txtPalletNo.BackColor = Color.Red;
                //    this.txtPalletNo.Select();
                //    this.txtPalletNo.SelectAll();
                //    return;
                //}

                //if (!InitCheckDetailData())
                //{
                //    txtLotNum.SelectAll();
                //    txtLotNum.Select();
                //    return;
                //}

                //txtLotNum.SelectAll();
                //txtLotNum.Select();

                //string checkQty = this.txtCheck_Qty.Text.Trim();
                //string palletQty = this.txtPallet_Qty.Text.Trim();

                //if (checkQty==palletQty
                //      && !string.IsNullOrEmpty(palletQty)
                //      && Convert.ToInt32(palletQty) == gvWipConsigment.RowCount)
                //{
                //    SaveToWarehouseCheckData();
                //}
                checkLotNum(lotNumber);
            }
        }

        /// <summary>
        /// 新建组件信息方法， ruhu.yu
        /// </summary>
        /// <param name="lotNum"></param>
        private void checkLotNum(string lotNum)
        {

            if (dtPackage == null && !InitPackageData())
            {
                if (!Is_OneKeyDispatch.Checked)
                {
                    this.txtLotNum.Text = string.Empty;
                }
                this.txtPalletNo.BackColor = Color.Red;
                this.txtPalletNo.Select();
                this.txtPalletNo.SelectAll();
                return;
            }

            if (!InitCheckDetailData(lotNum))
            {
                if (!Is_OneKeyDispatch.Checked)
                {
                    //txtLotNum.SelectAll();
                    //txtLotNum.Select();
                }
                return;
            }
            if (!Is_OneKeyDispatch.Checked)
            {
                //txtLotNum.SelectAll();
                //txtLotNum.Select();
            }
            string checkQty = this.txtCheck_Qty.Text.Trim();
            string palletQty = this.txtPallet_Qty.Text.Trim();

            if (checkQty == palletQty
                  && !string.IsNullOrEmpty(palletQty)
                  && Convert.ToInt32(palletQty) == gvWipConsigment.RowCount)
            {
                SaveToWarehouseCheckData();
            }
        }
        
        /// <summary>
        /// 初始化入库检验数据。
        /// </summary>
        private bool InitCheckDetailData(string lotNum)
        {
            string val = lotNum;
            string palletNo=this.txtPalletNo.Text.Trim();
            DataRow drPackageDetail=null;
            //不检验客户编码
            if (!chkCustNo.Checked)
            {
                //判断批次号是否在包装明细中存在。
                drPackageDetail = dtPackageDetail.AsEnumerable()
                                           .Where(dr => Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]) == val)
                                           .SingleOrDefault();
                if(drPackageDetail==null)
                {
                    MessageService.ShowError(string.Format("批次【{0}】不在托号【{1}】中。", val, palletNo));
                    return false;
                }
                //判断批次号是否已经检验。
                int nCount = dtPackageDetail.AsEnumerable()
                                   .Where(dr => Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]) == val && Convert.ToBoolean(dr[COLNAME_IS_CHECK])==true)
                                   .Count();
                if (nCount > 0)
                {
                    MessageService.ShowError(string.Format("批次【{0}】已检验。", val));
                    return false;               }
            }
            //检验客户编码
            else
            {
                //判断客户编码是否在包装明细中存在。
                drPackageDetail  = dtPackageDetail.AsEnumerable()
                                           .Where(dr => Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE]) == val)
                                           .SingleOrDefault();
                if(drPackageDetail==null)
                {
                    MessageService.ShowError(string.Format("客户编码【{0}】不在托号【{1}】中。", val, palletNo));
                    return false;
                }
                //判断客户编码是否已经检验。
                int nCount = dtPackageDetail.AsEnumerable()
                              .Where(dr => Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE]) == val && Convert.ToBoolean(dr[COLNAME_IS_CHECK]) == true)
                              .Count();
                if (nCount > 0)
                {
                    MessageService.ShowError(string.Format("客户编码【{0}】已检验。", val));
                    return false;
                }
            }
            string lotNumber = Convert.ToString(drPackageDetail[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
            //检查流程卡是否拍照。
            //根据设定确定车间是否需要检查流程卡图片
            string[] cols = new string[] { "FactoryName", "IsCheckPic" };
            List<KeyValuePair<string, string>> lstConditions = new List<KeyValuePair<string, string>>();
            lstConditions.Add(new KeyValuePair<string, string>("FactoryName", this._model.RoomName));
            DataTable dtReturn = BaseData.GetBasicDataByCondition(cols, BASEDATA_CATEGORY_NAME.Basic_CheckPic_ByBeforeWareHouse, lstConditions);
            if (dtReturn != null && dtReturn.Rows.Count > 0)
            {
                string checkModuleProcessCardImage = Convert.ToString(dtReturn.Rows[0]["IsCheckPic"]);
                bool bCheckModuleProcessCardImage = false;
                if (!bool.TryParse(checkModuleProcessCardImage, out bCheckModuleProcessCardImage))
                {
                    MessageService.ShowError("基础数据表入库检工序是否检查流程卡图片设置有误,请与系统管理员联系");
                    return false;
                }
                if (bCheckModuleProcessCardImage)
                {
                    bool bCheckProcessCard=this._lotEntity.CheckProcessCard(lotNumber);
                    if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
                    {
                        MessageService.ShowError(string.Format("检查批次{0}流程时出错：{1}", txtLotNum.Text, this._lotEntity.ErrorMsg));
                        return false;
                    }
                    if (bCheckProcessCard==false)
                    {
                        MessageService.ShowError(string.Format("批次【{0}】流程卡图片未保存，请确认!", txtLotNum.Text));
                        return false;
                    }
                }
            }
            drPackageDetail[WIP_CONSIGNMENT_FIELDS.FIELDS_CHECKER] = this._model.UserName;
            drPackageDetail[WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME] = this._checkTime;
            drPackageDetail[COLNAME_IS_CHECK] = true;
            int datarowIndex = dtPackageDetail.Rows.IndexOf(drPackageDetail);
            int rowHandle = this.gvWipConsigment.GetRowHandle(datarowIndex);
            this.gvWipConsigment.FocusedRowHandle = rowHandle;
            //设置入库检验数量。
            this.txtCheck_Qty.Text = dtPackageDetail.AsEnumerable()
                                                    .Where(dr => Convert.ToBoolean(dr[COLNAME_IS_CHECK]) == true)
                                                    .Count().ToString();
            this.gvWipConsigment.BestFitColumns();
            return true;
        }
        /// <summary>
        /// 保存入库检验数据。
        /// </summary>
        private void SaveToWarehouseCheckData()
        {
            if (dtPackage == null && !InitPackageData())
            {
                this.txtPalletNo.Select();
                this.txtPalletNo.SelectAll();
                return;
            }
            string checkQty = this.txtCheck_Qty.Text.Trim();
            string palletQty = this.txtPallet_Qty.Text.Trim();
            int nCheckQty = dtPackageDetail.AsEnumerable()
                                        .Where(dr => Convert.ToBoolean(dr[COLNAME_IS_CHECK]) == true)
                                        .Count();
            if (nCheckQty < 1)
            {
                MessageService.ShowError(string.Format("表格中没有检验数据，不能保存!"));
                if (!Is_OneKeyDispatch.Checked)
                {
                    //this.txtLotNum.Select();
                    //this.txtLotNum.Focus();
                }
                return;
            }

            if (checkQty != palletQty
                  || Convert.ToInt32(palletQty) != nCheckQty)
            {
                MessageService.ShowError(string.Format("检验数量【{0}】和托数量【{1}】不一致，请确认!",
                                                       checkQty, palletQty));
                if (!Is_OneKeyDispatch.Checked)
                {
                    //this.txtLotNum.Select();
                    //this.txtLotNum.SelectAll();
                }
                return;
            }
            
            

            string oprComputer = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
            string timezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            //组织保存数据。
            //将包装明细添加到数据集中。
            DataSet dsParams = new DataSet();
            dtPackageDetail.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
            dsParams.Merge(dtPackageDetail, true, MissingSchemaAction.Add);
            //设置包装数据。
            DataRow drPackage = dtPackage.Rows[0];
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CHECKER] = this._model.UserName;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME] = this._checkTime;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_EDITOR] = this._model.UserName;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = 2;
            dsParams.Merge(dtPackage, true, MissingSchemaAction.Add);
            //组织其他附加参数数据
            Hashtable htMaindata = new Hashtable();
            htMaindata.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY, this._operationKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, this._model.EquipmentKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, this._model.LineKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, this._model.LineName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, this._model.ShiftKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, this._model.ShiftName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, this._model.UserName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, this._model.UserName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, timezone);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, oprComputer);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, string.Empty);
            DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
            dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
            dsParams.Merge(dtParams, true, MissingSchemaAction.Add);

            //执行入库检验作业。
            ParameterizedThreadStart start = new ParameterizedThreadStart(ToWarehouseCheck);
            Thread t = new Thread(start);
            t.Start(dsParams);            
        }

        private void ToWarehouseCheck(object obj)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                //this.lblMsg.Visible = true;
                this.lblMsg.Visibility = LayoutVisibility.Always;
                this.lblMsg.Text = string.Format("正在执行入库检验操作，请勿关闭界面，等待...");
                this.tableLayoutPanelMain.Enabled = false;
            }));
            try
            {
                DataSet dsParam = obj as DataSet;
                DataSet dsReturn = this._lotEntity.LotToWarehouseCheck(dsParam);
                this.Invoke(new MethodInvoker(() =>
                {
                    if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
                    {
                        MessageService.ShowError(this._lotEntity.ErrorMsg);
                        return;
                    }
                    MessageService.ShowMessage(string.Format("托号【{0}】入库检验成功", txtPalletNo.Text.Trim()), "提示");
                    InitControlsValue();
                }));
            }
            finally
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    //this.lblMsg.Visible = false;
                    this.lblMsg.Visibility = LayoutVisibility.Never;
                    this.lblMsg.Text = string.Empty;
                    this.tableLayoutPanelMain.Enabled = true;
                    this.txtPalletNo.Select();
                    this.txtPalletNo.SelectAll();
                }));
            }
        }
        /// <summary>
        /// 保存按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOK_Click(object sender, EventArgs e)
        {
            SaveToWarehouseCheckData();
        }
        /// <summary>
        /// 重置按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbReset_Click(object sender, EventArgs e)
        {
            InitControlsValue();
            this.txtPalletNo.SelectAll();
            this.txtPalletNo.Select();
        }
        /// <summary>
        /// 关闭按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this._view.WorkbenchWindow.CloseWindow(false);
            //显示工作站作业界面。
            LotDispathViewContent view = new LotDispathViewContent(_model);
            WorkbenchSingleton.Workbench.ShowView(view);

        }
        /// <summary>
        /// 检验客户编码选择改变事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkCustNo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCustNo.Checked)
            {
                lciLotNumber.Text = "客户编码";
            }
            else
            {
                lciLotNumber.Text = "组件序列号";
            }
        }

        /// <summary>
        /// 查询按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbQuery_Click(object sender, EventArgs e)
        {
            //string subTitle = "入库检查询";
            //LotDispatchForPltChkDialog ldfp = new LotDispatchForPltChkDialog(0, _model, subTitle);

            //if (DialogResult.OK == ldfp.ShowDialog())
            //{
            //    InitControlsValue();
            //    this.txtPalletNo.Text = string.Empty;

            //    if (ldfp.dtCommon.Rows.Count > 0)
            //    {
            //        DataTable dtCommon = ldfp.dtCommon;
            //        this.lblPalletNoTitle.Visible = true;
            //        this.lblLotNoTitle.Visible = true;
            //        this.lblSumLotNo.Visible = true;
            //        this.lblSumPalletNo.Visible = true;

            //        DataView dv = dtCommon.DefaultView;
            //        DataTable dtPalletNo = dv.ToTable(WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO, true, new string[] { WIP_CONSIGNMENT_FIELDS.FIELDS_PALLET_NO });
            //        DataTable dtLotNo = dv.ToTable(POR_LOT_FIELDS.FIELD_LOT_NUMBER, true, new string[] { POR_LOT_FIELDS.FIELD_LOT_NUMBER });
            //        this.lblSumLotNo.Text = dtLotNo.Rows.Count.ToString();
            //        this.lblSumPalletNo.Text = dtPalletNo.Rows.Count.ToString();

            //        gcWipConsigment.DataSource = null;
            //        gcWipConsigment.DataSource = ldfp.dtCommon;
            //        this.gvWipConsigment.BestFitColumns();
            //    }
            //    this.tsbOK.Enabled = false;
            //}
        }
        /// <summary>
        /// 导出EXCEL按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbExportExecl_Click(object sender, EventArgs e)
        {
            //if (this.gvWipConsigment.RowCount > 0)
            //{
            //    print();
            //}
        }
        ///// <summary>
        ///// 导出表格数据
        ///// </summary>
        //private void print()
        //{
        //    //导出到execl  
        //    try
        //    {
        //        DevExpress.XtraGrid.Views.Grid.GridView gvExport = this.gvWipConsigment;
        //        SaveFileDialog saveExcelDialog = new SaveFileDialog();
        //        string fieldpath = string.Empty, fileNameExt = string.Empty;
        //        saveExcelDialog.Filter = "excel文件(*.xls)|*.xls";
        //        saveExcelDialog.DefaultExt = "xls";
        //        saveExcelDialog.InitialDirectory = Directory.GetCurrentDirectory();

        //        saveExcelDialog.RestoreDirectory = true;

        //        if (DialogResult.OK == saveExcelDialog.ShowDialog())
        //        {
        //            int rowscount = gvExport.RowCount;
        //            int colscount = gvExport.Columns.Count;

        //            if (rowscount > 65536)
        //            {
        //                MessageBox.Show("数据记录数太多(最多不能超过65536条)，不能保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                return;
        //            }

        //            //列数不可以大于255
        //            if (colscount > 255)
        //            {
        //                MessageBox.Show("数据记录行数太多，不能保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                return;
        //            }

        //            fieldpath = saveExcelDialog.FileName;
        //            fileNameExt = fieldpath.Substring(fieldpath.LastIndexOf("\\") + 1);

        //            //验证以fileNameString命名的文件是否存在，如果存在删除它
        //            FileInfo file = new FileInfo(fieldpath);
        //            if (file.Exists)
        //            {
        //                try
        //                {
        //                    file.Delete();
        //                }
        //                catch (Exception error)
        //                {
        //                    MessageBox.Show(error.Message, "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                    return;
        //                }
        //            }
        //            Microsoft.Office.Interop.Excel.Application objExcel = new Microsoft.Office.Interop.Excel.Application();
        //            Microsoft.Office.Interop.Excel.Workbook objWorkbook = objExcel.Workbooks.Add(Missing.Value);
        //            Microsoft.Office.Interop.Excel.Worksheet objsheet = (Microsoft.Office.Interop.Excel.Worksheet)objWorkbook.ActiveSheet;
        //            try
        //            {
        //                //设置EXCEL不可见
        //                objExcel.Visible = false;

        //                //向Excel中写入表格的表头
        //                int displayColumnsCount = 1;
        //                for (int i = 0; i < gvExport.Columns.Count; i++)
        //                {
        //                    if (gvExport.Columns[i].Visible == true)
        //                    {
        //                        string tmp = gvExport.Columns[i].Caption;
        //                        //objExcel.Cells[1, displayColumnsCount] = tmp;
        //                        objsheet.Cells[1, displayColumnsCount] = tmp;
        //                        displayColumnsCount++;
        //                    }
        //                }
        //                System.Windows.Forms.ProgressBar tempProgressBar = new System.Windows.Forms.ProgressBar();
        //                //设置进度条
        //                tempProgressBar.Refresh();
        //                tempProgressBar.Visible = true;
        //                tempProgressBar.Minimum = 1;
        //                tempProgressBar.Maximum = gvExport.RowCount;
        //                tempProgressBar.Step = 1;

        //                //向Excel中逐行逐列写入表格中的数据
        //                for (int row = 0; row < gvExport.RowCount; row++)
        //                {
        //                    tempProgressBar.PerformStep();

        //                    displayColumnsCount = 1;
        //                    for (int col = 0; col < colscount; col++)
        //                    {
        //                        if (gvExport.Columns[col].Visible == true)
        //                        {
        //                           objsheet.Cells[row + 2, displayColumnsCount] = gvExport.GetRowCellValue(row, gvExport.Columns[col].FieldName).ToString().Trim();
        //                            displayColumnsCount++;
        //                        }
        //                    }
        //                }
        //                //隐藏进度条
        //                tempProgressBar.Visible = false;

        //                //保存文件
        //                objWorkbook.SaveAs(fieldpath, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
        //                    Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, Missing.Value, Missing.Value, Missing.Value,
        //                    Missing.Value, Missing.Value);


        //            }
        //            catch (Exception error)
        //            {
        //                MessageBox.Show(error.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return;
        //            }
        //            finally
        //            {
        //                //关闭Excel应用
        //                if (objWorkbook != null) objWorkbook.Close(Missing.Value, Missing.Value, Missing.Value);
        //                if (objExcel.Workbooks != null) objExcel.Workbooks.Close();
        //                if (objExcel != null) objExcel.Quit();

        //                objsheet = null;
        //                objWorkbook = null;
        //                objExcel = null;
        //            }

        //            MessageBox.Show(fieldpath + "\n\n导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "错误提示");
        //    }
        //}
        /// <summary>
        /// 入库检返到包装事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbnBackPackage_Click(object sender, EventArgs e)
        {
            if (!InitPackageData())
            {
                this.txtPalletNo.Select();
                this.txtPalletNo.SelectAll();
                return;
            }
            if (MessageService.AskQuestionSpecifyNoButton(string.Format("是否将托盘{0}返回到包装重新包装？", this.txtPalletNo.Text), "询问"))
            {
                RejectPackageData();
            }
        }

        /// <summary>
        /// 返到包装。
        /// </summary>
        private void RejectPackageData()
        {
            string oprComputer = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
            string timezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            //组织保存数据。
            //将包装明细添加到数据集中。
            DataSet dsParams = new DataSet();
            dtPackageDetail.TableName = POR_LOT_FIELDS.DATABASE_TABLE_NAME;
            dsParams.Merge(dtPackageDetail, true, MissingSchemaAction.Add);
            //设置包装数据。
            DataRow drPackage = dtPackage.Rows[0];
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CHECKER] = this._model.UserName;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CHECK_TIME] = this._checkTime;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_EDITOR] = this._model.UserName;
            drPackage[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP] = 0;
            dsParams.Merge(dtPackage, true, MissingSchemaAction.Add);
            //组织其他附加参数数据
            Hashtable htMaindata = new Hashtable();
            htMaindata.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY, this._operationKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, this._model.EquipmentKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, this._model.LineKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, this._model.LineName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, this._model.ShiftKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, this._model.ShiftName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, this._model.UserName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, this._model.UserName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, timezone);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, oprComputer);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, string.Empty);
            DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
            dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
            dsParams.Merge(dtParams, true, MissingSchemaAction.Add);

            //执行入库检验作业。
            ParameterizedThreadStart start = new ParameterizedThreadStart(RejectPackageData);
            Thread t = new Thread(start);
            t.Start(dsParams);
        }

        private void RejectPackageData(object obj)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                //this.lblMsg2.Visible = true;
                this.lblMsg.Visibility = LayoutVisibility.Always;
                this.lblMsg.Text = string.Format("正在执行入库检返到包装操作，请勿关闭界面，等待...");
                this.tableLayoutPanelMain.Enabled = false;
            }));
            try
            {
                DataSet dsParam = obj as DataSet;
                DataSet dsReturn = this._lotEntity.LotToWarehouseCheckReject(dsParam);
                this.Invoke(new MethodInvoker(() =>
                {
                    if (!string.IsNullOrEmpty(this._lotEntity.ErrorMsg))
                    {
                        MessageService.ShowError(this._lotEntity.ErrorMsg);
                        return;
                    }
                    MessageService.ShowMessage(string.Format("托号【{0}】入库检返到包装操作成功", txtPalletNo.Text.Trim()), "提示");
                    InitControlsValue();
                }));
            }
            finally
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    //this.lblMsg2.Visible = false;
                    this.lblMsg.Visibility = LayoutVisibility.Never;
                    this.lblMsg.Text = string.Empty;
                    this.tableLayoutPanelMain.Enabled = true;
                    this.txtPalletNo.Select();
                    this.txtPalletNo.SelectAll();
                }));
            }
        }
        /// <summary>
        /// 检验明细自动绘制单元格背景。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvWipConsigment_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                DataRow dr = this.gvWipConsigment.GetDataRow(e.RowHandle);
                bool isCheck = Convert.ToBoolean(dr[COLNAME_IS_CHECK]);
                if (isCheck)
                {
                    e.Appearance.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    e.Appearance.BackColor = System.Drawing.Color.Gray;
                }
            }
        }

        private void Is_OneKeyDispatch_CheckedChanged(object sender, EventArgs e)
        {
            //if (Is_OneKeyDispatch.Checked)
            //{
            //    txtLotNum.Enabled = false;
            //}
            //else
            //{
            //    txtLotNum.Enabled = true;
            //}
        }
    }
}