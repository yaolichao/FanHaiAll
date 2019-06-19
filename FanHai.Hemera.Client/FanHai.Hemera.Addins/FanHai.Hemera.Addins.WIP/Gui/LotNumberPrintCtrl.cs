using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using BarCodePrint;
using FanHai.Hemera.Utils.Controls;
using System.IO;
using System.Drawing.Printing;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示批次号打印机的界面。
    /// </summary>
    public partial class LotNumberPrintCtrl : BaseUserCtrl
    {
        private ContextMenuStrip contextMenu = null;
        private string _printFlag = string.Empty;
        private string _lotNumber = string.Empty;
        private int rowHandle = -1;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotNumberPrintCtrl() 
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotNumberPrintCtrl_Load(object sender, EventArgs e)
        {
            try
            {
                #region 打印类型：0：未打印。1：已打印
                DataTable dtType = new DataTable();

                dtType.Columns.Add("NAME", Type.GetType("System.String"));
                dtType.Columns.Add("CODE", Type.GetType("System.String"));

                dtType.Rows.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.Sorting.CategoyBatchPrintCtrl.NotPrint}"), "0");
                dtType.Rows.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.Sorting.CategoyBatchPrintCtrl.AlreadyPrint}"), "1");
                lueType.Properties.DataSource = dtType;
                lueType.Properties.DisplayMember = "NAME";
                lueType.Properties.ValueMember = "CODE";
                lueType.ItemIndex = 0;
                #endregion
                // 批次类型 0：正常批次 1：返工批次。
                DataTable lotTypeTab = new DataTable();
                lotTypeTab.Columns.Add("NAME");
                lotTypeTab.Columns.Add("CODE");
                lotTypeTab.Rows.Add("正常批次","0");
                lotTypeTab.Rows.Add("返工批次","1");
                lueLotType.Properties.DataSource = lotTypeTab;
                lueLotType.Properties.DisplayMember = "NAME";
                lueLotType.Properties.ValueMember = "CODE";

                this.deStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.deEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage(ex.Message);
            }
        }
        /// <summary>
        /// 查询按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearch_Click(object sender, EventArgs e)
        {
            //工单号必须输入。
            //if (string.IsNullOrEmpty(this.teWorkOrder.Text))
            //{
            //    MessageService.ShowWarning("${res:FanHai.Hemera.Addins.Sorting.Message.WorkOrderIsNull}");
            //    this.teWorkOrder.Focus();
            //    return;
            //}

            //开始时间大于结束时间。
            if (!string.IsNullOrEmpty(deStartTime.Text) && !string.IsNullOrEmpty(deEndTime.Text) &&
                deStartTime.DateTime > deEndTime.DateTime)
            {
                MessageService.ShowWarning("结束时间不能小于开始时间。");
                this.deEndTime.Focus();
                return;
            }

            Hashtable hashTable = new Hashtable();

            if (teWorkOrder.Text != string.Empty)
            {
                hashTable.Add(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER, teWorkOrder.Text);
            }
            if (txtLotNumber.Text != string.Empty)
            {
                hashTable.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, txtLotNumber.Text.ToUpper().Trim());
            }
            if (lueType.Text != string.Empty)
            {
                hashTable.Add(POR_LOT_FIELDS.FIELD_IS_PRINT, lueType.EditValue.ToString());
            }
            if (lueLotType.Text.Trim().Length>0)
            {
                hashTable.Add(POR_LOT_FIELDS.FIELD_IS_REWORKED, lueLotType.EditValue.ToString());
            }
            if (!string.IsNullOrEmpty(deStartTime.Text))
            {
                hashTable.Add("CREATE_TIME_START", deStartTime.Text);
            }
            if (!string.IsNullOrEmpty(deEndTime.Text))
            {
                hashTable.Add("CREATE_TIME_END", deEndTime.Text);
            }

            DataTable dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
            LotQueryEntity queryEntity = new LotQueryEntity();
            DataSet dsLotInfo = queryEntity.GetLotNumberForPrint(dataTable);
            if (string.IsNullOrEmpty(queryEntity.ErrorMsg))
            {
                if (dsLotInfo != null && dsLotInfo.Tables.Count > 0)
                {
                    gcLotInfo.DataSource = dsLotInfo.Tables[0];
                }
            }
            else
            {
                MessageService.ShowError(queryEntity.ErrorMsg);
            }
        }
        /// <summary>
        /// 批次数据行Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLotInfo_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                object printFlag =gvLotInfo.GetRowCellValue(e.RowHandle, POR_LOT_FIELDS.FIELD_IS_PRINT);
                if (printFlag == null || printFlag.ToString() == string.Empty)
                {
                    return;
                }
                _printFlag = printFlag.ToString();
                _lotNumber = gvLotInfo.GetRowCellValue(e.RowHandle, POR_LOT_FIELDS.FIELD_LOT_NUMBER).ToString();
                rowHandle = e.RowHandle;
                if (e.Button == MouseButtons.Right)
                {
                    AddContextMenu();
                }                
            }
        }

        /// <summary>
        /// 为数据行Click事件添加右键菜单。
        /// </summary>
        private void AddContextMenu()
        {
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("打印", null, new EventHandler(NumberPrint));
            contextMenu.Items.Add("批量打印",null,new EventHandler(BatchPrint));
            //set the context menu's show position
            Point p = new Point(Control.MousePosition.X, Control.MousePosition.Y);
            p = this.PointToClient(p);
            //show context menu
            contextMenu.Show(this, p);
        }
        /// <summary>
        /// 批次号打印。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberPrint(object sender, EventArgs e)
        {
            Print(false);
        }
        /// <summary>
        /// 批量打印批次号。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BatchPrint(object sender,EventArgs e)
        {
            Print(true);
        }

        /// <summary>
        /// 打印条码。
        /// </summary>
        /// <param name="batch">是否批量打印。</param>
        private void Print(bool batch)
        {
            try
            {
                ComputerEntity computerEntity = new ComputerEntity();
                if (computerEntity.GetComputerPrinterInfo(PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME)))
                {
                    if (computerEntity.PrinterType.Length < 1 || computerEntity.BarcodeModule.Length < 1)
                    {
                        MessageService.ShowError("请配置打印机类型或标签模板");
                        return;
                    }
                    else
                    {
                        if (Convert.ToInt32(computerEntity.PrinterType) == (int)PortType.Local)
                        {
                            if (computerEntity.PrinterName.Length == 0)
                            {
                                MessageService.ShowMessage("请配置打印机名称");
                                return;
                            }
                        }
                        else
                        {
                            if (computerEntity.PrinterPort.Length == 0)
                            {
                                MessageService.ShowMessage("请配置打印机端口");
                                return;
                            }
                            if (Convert.ToInt32(computerEntity.PrinterType) == (int)PortType.Network && computerEntity.PrinterIP.Length == 0)
                            {
                                MessageService.ShowError("请配置打印机Ip地址");
                                return;
                            }
                        }
                    }
                    if (rowHandle > -1)
                    {
                        //组织要打印的数据。
                        List<BarCode> barcodeList = new List<BarCode>();
                        if (batch)//如果要批量打印。
                        {
                            foreach (int i in gvLotInfo.GetSelectedRows())
                            {
                                string lotNumber = gvLotInfo.GetRowCellValue(i, POR_LOT_FIELDS.FIELD_LOT_NUMBER).ToString();
                                BarCode barcode = new BarCode(lotNumber);
                                barcodeList.Add(barcode);
                            }
                        }
                        else
                        {
                            BarCode barcode = new BarCode(_lotNumber);
                            barcodeList.Add(barcode);
                        }
                        //打印
                        int printNumber = 0;
                        try
                        {
                            printNumber = CodePrint.BarCodePrint(barcodeList, computerEntity.BarcodeModule, computerEntity.PrinterName, computerEntity.PrinterIP, computerEntity.PrinterPort, (PortType)(Convert.ToInt32(computerEntity.PrinterType)));
                        }
                        catch (Exception ex)
                        {
                            MessageService.ShowWarning("打印失败：" + ex.Message);
                            return;
                        }
                        //更新批次状态。
                        LotEntity entity = new LotEntity();
                        List<string> lotNumbers = new List<string>();
                        for (int i = 0; i < printNumber; i++)
                        {
                            lotNumbers.Add(barcodeList[i].BatteryCellCode);
                        }
                        if (!entity.UpdatePrintFlag(lotNumbers))
                        {
                            MessageService.ShowError("更新批次号打印机状态失败。");
                            return;
                        }
                        else 
                        {
                            if (batch)//如果要批量打印。
                            {
                                foreach (int i in gvLotInfo.GetSelectedRows())
                                {
                                    gvLotInfo.SetRowCellValue(i, POR_LOT_FIELDS.FIELD_IS_PRINT, (Convert.ToInt32(_printFlag) + 1).ToString());
                                }
                            }
                            else
                            {
                                gvLotInfo.SetRowCellValue(rowHandle, POR_LOT_FIELDS.FIELD_IS_PRINT, (Convert.ToInt32(_printFlag) + 1).ToString());
                            }
                        }

                        if (printNumber == barcodeList.Count)//打印成功
                        {
                            MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Sorting.Message.PrintFinished}", "提示");
                        }
                        else
                        {
                            MessageService.ShowMessage("部分条码打印未成功。", "提示");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
    }
}
