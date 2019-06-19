
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using System.Collections;
using FanHai.Hemera.Utils.Dialogs;
using System.Linq;
using FanHai.Hemera.Share.Common;
using org.in2bits.MyXls;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WMS
{
    /// <summary>
    /// 表示托盘出货查询作业的窗体类。
    /// </summary>
    public partial class PalletShipmentQueryNewCtrl : BaseUserCtrl
    {
        IViewContent _view = null;                                      //当前视图。
        DataTable _dtProductGrade = null;                               //暂存产品等级。
        /// <summary>
        /// 出货操作对象。
        /// </summary>
        ShipmentOperationEntity _entity = new ShipmentOperationEntity();
        /// <summary>
        /// 构造函数
        /// </summary>
        public PalletShipmentQueryNewCtrl(IViewContent view)
        {
            InitializeComponent();
            this._view = view;

            InitUi();
            GridViewHelper.SetGridView(gvList);
            this.lblMenu.Text = "库房管理>出货管理>出货查询";
        }
        private void InitUi()
        {
            tsbExport.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.btn.0001}");
            tsbExportNew.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.btn.0002}");
            tsbExportIncludeArtNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.btn.0003}");
            btnFromBCP.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.btn.0004}");
            btnQuery.Text = StringParser.Parse("${res:Global.Query}");

            lblMenu.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.lbl.0001}");
            lciShipmentNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.lbl.0002}");
            lciContainerNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.lbl.0003}");
            lciCINumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.lbl.0004}");
            lciShipmentType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.lbl.0005}");
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.lbl.0006}");
            layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.lbl.0007}");
            layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.lbl.0008}");
            lciPalletNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.lbl.0009}");

            gcRowNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.Column.0001}");
            gclPalletNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.Column.0002}");
            gclQuantity.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.Column.0003}");
            gclWorkorderNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.Column.0004}");
            gclSAPNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.Column.0005}");
            gclPowerLevel.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.Column.0006}");
            gclShipmentNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.Column.0007}");
            gclContainerNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.Column.0008}");
            gclShipmentType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.Column.0009}");
            gcPowerRange.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.Column.0010}");
            gclShipmentDate.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.Column.0011}");
            gclShipmentOperator.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.Column.0012}");
            gclArtNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.Column.0013}");
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PalletShipmentQueryCtrl_Load(object sender, EventArgs e)
        {
            BindShipmentType();
            ResetControlValue();
            BindProductGrade();
            timeEditStart.EditValue = DateTime.Now.ToString("yyyy-MM-01");
            timeEditEnd.EditValue = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 绑定产品等级。
        /// </summary>
        private void BindProductGrade()
        {
            string[] columns = new string[] { "Column_code", "Column_Name" };
            List<KeyValuePair<string, string>> where = new List<KeyValuePair<string, string>>();
            where.Add(new KeyValuePair<string, string>("Column_type", "ProductGrade"));
            this._dtProductGrade = BaseData.GetBasicDataByCondition(columns, BASEDATA_CATEGORY_NAME.Basic_TestRule_PowerSet, where);
        }
        /// <summary>
        /// 重置控件值。
        /// </summary>
        private void ResetControlValue()
        {
            this.teShipmentNo.Text = string.Empty;
            this.teContainerNo.Text = string.Empty;
            this.lueShipmentType.EditValue = string.Empty;
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList != null)
            {
                dtList.Rows.Clear();
            }
        }
        /// <summary>
        /// 绑定出货类型。
        /// </summary>
        private void BindShipmentType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", BASEDATA_CATEGORY_NAME.Basic_ShipmentType);
            this.lueShipmentType.Properties.DataSource = BaseData.Get(columns, category);
            this.lueShipmentType.Properties.DisplayMember="NAME";
            this.lueShipmentType.Properties.ValueMember = "CODE";
        }
        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this._view.WorkbenchWindow.CloseWindow(false);
        }
        /// <summary>
        /// 控件响应Ctrl+Enter事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyEventArgs args = new KeyEventArgs(keyData);
            if (args.Control && args.KeyCode == Keys.Enter)
            {
                btnQuery_Click(btnQuery, args);
                args.Handled = true;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        /// <summary>
        /// 查询按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            string palletNo = this.tePalletNo.Text.Trim();
            if (palletNo.Length > 2048)
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.msg.0001}"));//托盘号长度必须小于等于2048个字符。
                this.tePalletNo.Select();
                return;
            }
            DataSet dsParams = this.GetQueryCondition();
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            DataSet dsGetReturn = this._entity.Query(dsParams, ref config);
            DataSet dsReturn = null;
            if (dsGetReturn != null || dsGetReturn.Tables[0].Rows.Count > 0)
            {
                dsReturn = this._entity.GetPalletShipInf(dsGetReturn);
            }
            else
                dsReturn = dsGetReturn;

            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }
            if (dsReturn.Tables.Count > 0)
            {
                gcList.DataSource = dsReturn.Tables[0];
                gcList.MainView = gvList;
                gvList.BestFitColumns();
            }
        }
        /// <summary>
        /// 导出按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbExport_Click(object sender, EventArgs e)
        {
            ExportExcel(false,false);
        }

        private void ExportExcel(bool isNew,bool includeArtNo)
        {
            DataSet dsParams = this.GetQueryCondition();
            DataSet dsReturn = this._entity.Query(dsParams);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }

            var lnq = from item in dsReturn.Tables[0].AsEnumerable()
                      group item by new
                      {
                          ShipmentNo = item[WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_NO],
                          ContainerNo = item[WMS_SHIPMENT_FIELDS.FIELDS_CONTAINER_NO]
                      } into g
                      select g.Key;

            XlsDocument xls = new XlsDocument();
            xls.SummaryInformation.Author = PropertyService.Get(PROPERTY_FIELDS.USER_NAME); //填加Excel文件作者信息
            xls.SummaryInformation.Subject = "出货清单";//填加文件主题信息
            xls.DocumentSummaryInformation.Company = "Astronerg Inc.";//填加文件公司信息 
            //按照出货单号+货柜号导出出货单记录。
            foreach (var item in lnq)
            {
                string sheetName = string.Format("出货单");
                //string filter = string.Format("{0} IS NULL AND {1} IS NULL", WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_NO, WMS_SHIPMENT_FIELDS.FIELDS_CONTAINER_NO);
                string filter = "";
                if (item.ShipmentNo != DBNull.Value)
                {
                    sheetName = string.Format("{0}_{1}出货单", item.ShipmentNo, item.ContainerNo);

                    //filter = string.Format("{0}='{1}' AND {2}='{3}'",WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_NO, item.ShipmentNo, WMS_SHIPMENT_FIELDS.FIELDS_CONTAINER_NO, item.ContainerNo);

                    filter = string.Format("{0}='{1}'", WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_NO, item.ShipmentNo);

                }
                DataRow[] drs = dsReturn.Tables[0].Select(filter);

                if (drs.Length <= 0)
                {
                    continue;
                }
                if (isNew)
                {
                    CreateExcelSheetNew(xls, drs, sheetName, includeArtNo);
                }
                else
                {
                    CreateExcelSheet(xls, drs, sheetName, includeArtNo);
                }
            }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件(*.xls)|*.xls";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                xls.FileName = dlg.FileName;
                xls.Save(true);
            }
            xls = null;
        }

        private void CreateExcelSheet(XlsDocument xls,DataRow []drs,string sheetName,bool includeArtNo)
        {
            Worksheet sheet = xls.Workbook.Worksheets.Add(sheetName);
            sheet.SheetType = WorksheetTypes.Worksheet;
            //循环放入列名
            for (int i = 1; i <= 8; i++)
            {
                ColumnInfo colInfo = new ColumnInfo(xls, sheet);
                colInfo.ColumnIndexStart = (ushort)(i - 1);
                colInfo.ColumnIndexEnd = (ushort)i;
                if (i == 1)
                {
                    colInfo.Width = 8 * 256;
                }
                else
                {
                    colInfo.Width = 15 * 256;
                }
                sheet.AddColumnInfo(colInfo);
            }
            Cells cells = sheet.Cells;
            int rowIndex = 1;
            XF xfDataHead = xls.NewXF();
            xfDataHead.HorizontalAlignment = HorizontalAlignments.Left;
            xfDataHead.Font.FontName = "宋体";
            xfDataHead.Font.Bold = true;
            xfDataHead.Font.Height = 11 * 20;
            xfDataHead.UseBorder = true;
            xfDataHead.BottomLineStyle = 1;
            xfDataHead.TopLineStyle = 1;
            xfDataHead.LeftLineStyle = 1;
            xfDataHead.RightLineStyle = 1;
            xfDataHead.CellLocked = false;
            xfDataHead.UseProtection = false;
            xfDataHead.UseNumber = true;
            //第一行
            cells.Add(rowIndex, 1, "船名航次Vessel：", xfDataHead);
            cells.Add(rowIndex, 2, string.Empty, xfDataHead);
            cells.Add(rowIndex, 3, string.Empty, xfDataHead);
            cells.Add(rowIndex, 4, string.Empty, xfDataHead);
            cells.Add(rowIndex, 5, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 1, 5);
            cells.Add(rowIndex, 6, "客户名称Customer：", xfDataHead);
            cells.Add(rowIndex, 7, string.Empty, xfDataHead);
            cells.Add(rowIndex, 8, string.Empty, xfDataHead);
            cells.Add(rowIndex, 9, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 6, 9);
            //第二行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "提单号B/L：", xfDataHead);
            cells.Add(rowIndex, 2, string.Empty, xfDataHead);
            cells.Add(rowIndex, 3, string.Empty, xfDataHead);
            cells.Add(rowIndex, 4, string.Empty, xfDataHead);
            cells.Add(rowIndex, 5, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 1, 5);
            cells.Add(rowIndex, 6, "发票号Invoice：", xfDataHead);
            cells.Add(rowIndex, 7, string.Empty, xfDataHead);
            cells.Add(rowIndex, 8, string.Empty, xfDataHead);
            cells.Add(rowIndex, 9, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 6, 9);
            //第三行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "关单号Customs order：", xfDataHead);
            cells.Add(rowIndex, 2, string.Empty, xfDataHead);
            cells.Add(rowIndex, 3, string.Empty, xfDataHead);
            cells.Add(rowIndex, 4, string.Empty, xfDataHead);
            cells.Add(rowIndex, 5, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 1, 5);
            cells.Add(rowIndex, 6, "目的地Destination：", xfDataHead);
            cells.Add(rowIndex, 7, string.Empty, xfDataHead);
            cells.Add(rowIndex, 8, string.Empty, xfDataHead);
            cells.Add(rowIndex, 9, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 6, 9);
            //第四行
            rowIndex += 1;
            cells.Add(rowIndex, 1,string.Format("集装箱号Container No：{0}",drs[0][WMS_SHIPMENT_FIELDS.FIELDS_CONTAINER_NO]), xfDataHead);
            cells.Add(rowIndex, 2, string.Empty, xfDataHead);
            cells.Add(rowIndex, 3, string.Empty, xfDataHead);
            cells.Add(rowIndex, 4, string.Empty, xfDataHead);
            cells.Add(rowIndex, 5, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 1, 5);
            cells.Add(rowIndex, 6, "封条号Seal No：", xfDataHead);
            cells.Add(rowIndex, 7, string.Empty, xfDataHead);
            cells.Add(rowIndex, 8, string.Empty, xfDataHead);
            cells.Add(rowIndex, 9, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 6, 9);
            //第五行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "车牌号Truck No：", xfDataHead);
            cells.Add(rowIndex, 2, string.Empty, xfDataHead);
            cells.Add(rowIndex, 3, string.Empty, xfDataHead);
            cells.Add(rowIndex, 4, string.Empty, xfDataHead);
            cells.Add(rowIndex, 5, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 1, 5);
            cells.Add(rowIndex, 6, "档位要求Power rate：", xfDataHead);
            cells.Add(rowIndex, 7, string.Empty, xfDataHead);
            cells.Add(rowIndex, 8, string.Empty, xfDataHead);
            cells.Add(rowIndex, 9, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 6, 9);

            XF xfContentHead = xls.NewXF();
            xfContentHead.HorizontalAlignment = HorizontalAlignments.Centered;
            xfContentHead.Font.FontName = "宋体";
            xfContentHead.Font.Bold = true;
            xfContentHead.Font.Height = 10 * 20;
            xfContentHead.UseBorder = true;
            xfContentHead.BottomLineStyle = 1;
            xfContentHead.TopLineStyle = 1;
            xfContentHead.LeftLineStyle = 1;
            xfContentHead.RightLineStyle = 1;
            xfContentHead.CellLocked = false;
            xfContentHead.UseProtection = false;
            xfContentHead.UseNumber = true;
            xfContentHead.UseBackground = true;
            xfContentHead.Pattern = 1;
            xfContentHead.PatternColor = Colors.Default37;
            //第七行
            rowIndex += 2;
            cells.Add(rowIndex, 1, "No.", xfContentHead);
            cells.Add(rowIndex, 2, "Module Type", xfContentHead);
            cells.Add(rowIndex, 3, "Pnom(w)", xfContentHead);
            cells.Add(rowIndex, 4, "PowerRange", xfContentHead);
            cells.Add(rowIndex, 5, "Material Code", xfContentHead);
            cells.Add(rowIndex, 6, "Grade", xfContentHead);
            cells.Add(rowIndex, 7, "Pallet No", xfContentHead);
            cells.Add(rowIndex, 8, "Wp", xfContentHead);
            cells.Add(rowIndex, 9, "Piece", xfContentHead);
            if (includeArtNo)
            {
                cells.Add(rowIndex, 10, "Art.No.", xfContentHead);
            }
            //第 8 行以后开始填充数据
            rowIndex += 1;
            int count = drs.Length;
            XF xfContent = xls.NewXF();
            xfContent.HorizontalAlignment = HorizontalAlignments.Centered;
            xfContent.Font.FontName = "宋体";
            xfContent.Font.Bold = false;
            xfContent.Font.Height = 10 * 20;
            xfContent.UseBorder = true;
            xfContent.BottomLineStyle = 1;
            xfContent.TopLineStyle = 1;
            xfContent.LeftLineStyle = 1;
            xfContent.RightLineStyle = 1;
            xfContent.CellLocked = false;
            xfContent.UseProtection = false;
            xfContent.UseNumber = true;
            xfContent.UseMisc = true;
            double sumWp = 0;
            double sumPiece = 0;
            for (int i = 0; i < count; i++)
            {
                cells.Add(i + rowIndex, 1, i + 1, xfContent);
                //Module Type
                string proId=Convert.ToString(drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_PRO_ID]);
                string[] tmpProId = proId.Split('-');
                string moduleType = proId;
                if (tmpProId.Length > 0)
                {
                    moduleType = tmpProId[0];
                }
                cells.Add(i + rowIndex, 2, moduleType, xfContent);
                //Pnom
                object pnom = drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL];
                if (pnom != DBNull.Value && pnom != null)
                {
                    cells.Add(i + rowIndex, 3, pnom, xfContent);
                }
                else
                {
                    cells.Add(i + rowIndex, 3, string.Empty, xfContent);
                }
                //PowerRange add by chao.pang 20140515
                //object powerrange = drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_RANGE];
                object powerrange = drs[i]["POWERRANGE"];
                if (powerrange != DBNull.Value && powerrange != null)
                {
                    cells.Add(i + rowIndex, 4, powerrange, xfContent);
                }
                else
                {
                    cells.Add(i + rowIndex, 4, string.Empty, xfContent);
                }
                //Material Code
                object mcode = drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO];
                if (mcode != DBNull.Value && mcode != null)
                {
                    cells.Add(i + rowIndex, 5, mcode, xfContent);
                }
                else
                {
                    cells.Add(i + rowIndex, 5, string.Empty, xfContent);
                }
                //Grade
                string gradeName = Convert.ToString(drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE]);
                if (this._dtProductGrade != null)
                {
                    DataRow[] drGrades = this._dtProductGrade.Select(string.Format("Column_code='{0}'", gradeName));
                    if (drGrades.Length > 0)
                    {
                        gradeName = Convert.ToString(drGrades[0]["Column_Name"]);
                    }
                } 
                cells.Add(i + rowIndex, 6, gradeName, xfContent);
                //Pallet No
                cells.Add(i + rowIndex, 7, Convert.ToString(drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]), xfContent);
                //Wp
                double wp = 0;
                if (drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_TOTLE_POWER] != DBNull.Value
                    && drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_TOTLE_POWER]!=null)
                {
                    wp=Convert.ToDouble(drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_TOTLE_POWER]);
                }
                sumWp += wp;
                cells.Add(i + rowIndex, 8, Math.Round(wp, 2, MidpointRounding.AwayFromZero), xfContent);
                //Piece
                double piece = Convert.ToDouble(drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_NUMBER_QTY]);
                sumPiece += piece;
                cells.Add(i + rowIndex, 9, Math.Round(piece, 2, MidpointRounding.AwayFromZero), xfContent);
                //Art.No.
                if (includeArtNo)
                {
                    string artNo = Convert.ToString(drs[i][BASE_POWERSET_COLORATCNO.FIELDS_ARTICNO]);
                    cells.Add(i + rowIndex, 10, artNo, xfContent);
                }
            }
            //第9+count行
            rowIndex += count;
            xfContent.FormulaHidden = true;
            cells.Add(rowIndex, 7, Math.Round(sumWp / sumPiece, 2, MidpointRounding.AwayFromZero), xfContent);
            cells.Add(rowIndex, 8, Math.Round(sumWp, 2, MidpointRounding.AwayFromZero), xfContent);
            cells.Add(rowIndex, 9, Math.Round(sumPiece, 2, MidpointRounding.AwayFromZero), xfContent);
            //第11+count行
            rowIndex += 2;
            xfContent.HorizontalAlignment = HorizontalAlignments.Left;
            cells.Add(rowIndex, 8, "库位：", xfContent);
            cells.Add(rowIndex, 9, " ", xfContent);

            XF xfFooter = xls.NewXF();
            xfFooter.HorizontalAlignment = HorizontalAlignments.Left;
            xfFooter.Font.FontName = "宋体";
            xfFooter.Font.Bold = true;
            xfFooter.Font.Height = 11 * 20;
            xfFooter.UseBorder = true;
            xfFooter.BottomLineStyle = 1;
            xfFooter.TopLineStyle = 1;
            xfFooter.LeftLineStyle = 1;
            xfFooter.RightLineStyle = 1;
            xfFooter.CellLocked = false;
            xfFooter.UseProtection = false;
            xfFooter.UseNumber = true;
            //第12+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "制单人签名：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            //第13+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "数据  确认: 仓管：(    )  QC: (    ）", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 6, "客检托确认: 仓管:(     )  QC:(       ）", xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 6, 9);
            //第14+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "表头信息 确认: 仓管：(    )QC: (    ）", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 6, "正公差确认：仓管:(     )  QC:(       )", xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 6, 9);
            //第15+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "首托确认时间/签名：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 6, "QC确认签名：", xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 6, 9);
            //第16+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "尾托确认时间/签名：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 6, "QC确认签名：", xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 6, 9);
            //第17+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "审核签名：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 6, "司机确认：", xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 6, 9);
        }

        private void CreateExcelSheetNew(XlsDocument xls, DataRow[] drs, string sheetName, bool includeArtNo)
        {
            Worksheet sheet = xls.Workbook.Worksheets.Add(sheetName);
            sheet.SheetType = WorksheetTypes.Worksheet;
            //循环放入列名
            for (int i = 1; i <= 8; i++)
            {
                ColumnInfo colInfo = new ColumnInfo(xls, sheet);
                colInfo.ColumnIndexStart = (ushort)(i - 1);
                colInfo.ColumnIndexEnd = (ushort)i;
                if (i == 1)
                {
                    colInfo.Width = 8 * 256;
                }
                else
                {
                    colInfo.Width = 15 * 256;
                }
                sheet.AddColumnInfo(colInfo);
            }
            Cells cells = sheet.Cells;
            int rowIndex = 1;
            XF xfDataHead = xls.NewXF();
            xfDataHead.HorizontalAlignment = HorizontalAlignments.Left;
            xfDataHead.Font.FontName = "宋体";
            xfDataHead.Font.Bold = true;
            xfDataHead.Font.Height = 11 * 20;
            xfDataHead.UseBorder = true;
            xfDataHead.BottomLineStyle = 1;
            xfDataHead.TopLineStyle = 1;
            xfDataHead.LeftLineStyle = 1;
            xfDataHead.RightLineStyle = 1;
            xfDataHead.CellLocked = false;
            xfDataHead.UseProtection = false;
            xfDataHead.UseNumber = true;
            //第一行
            cells.Add(rowIndex, 1, "船名航次Vessel：", xfDataHead);
            cells.Add(rowIndex, 2, string.Empty, xfDataHead);
            cells.Add(rowIndex, 3, string.Empty, xfDataHead);
            cells.Add(rowIndex, 4, string.Empty, xfDataHead);            
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "客户名称Customer：", xfDataHead);
            cells.Add(rowIndex, 6, string.Empty, xfDataHead);
            cells.Add(rowIndex, 7, string.Empty, xfDataHead);
            cells.Add(rowIndex, 8, string.Empty, xfDataHead);
            cells.Add(rowIndex, 9, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第二行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "提单号B/L：", xfDataHead);
            cells.Add(rowIndex, 2, string.Empty, xfDataHead);
            cells.Add(rowIndex, 3, string.Empty, xfDataHead);
            cells.Add(rowIndex, 4, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "发票号Invoice：", xfDataHead);
            cells.Add(rowIndex, 6, string.Empty, xfDataHead);
            cells.Add(rowIndex, 7, string.Empty, xfDataHead);
            cells.Add(rowIndex, 8, string.Empty, xfDataHead);
            cells.Add(rowIndex, 9, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第三行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "关单号Customs order：", xfDataHead);
            cells.Add(rowIndex, 2, string.Empty, xfDataHead);
            cells.Add(rowIndex, 3, string.Empty, xfDataHead);
            cells.Add(rowIndex, 4, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "目的地Destination：", xfDataHead);
            cells.Add(rowIndex, 6, string.Empty, xfDataHead);
            cells.Add(rowIndex, 7, string.Empty, xfDataHead);
            cells.Add(rowIndex, 8, string.Empty, xfDataHead);
            cells.Add(rowIndex, 9, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第四行
            rowIndex += 1;
            cells.Add(rowIndex, 1, string.Format("集装箱号Container No：{0}", drs[0][WMS_SHIPMENT_FIELDS.FIELDS_CONTAINER_NO]), xfDataHead);
            cells.Add(rowIndex, 2, string.Empty, xfDataHead);
            cells.Add(rowIndex, 3, string.Empty, xfDataHead);
            cells.Add(rowIndex, 4, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "封条号Seal No：", xfDataHead);
            cells.Add(rowIndex, 6, string.Empty, xfDataHead);
            cells.Add(rowIndex, 7, string.Empty, xfDataHead);
            cells.Add(rowIndex, 8, string.Empty, xfDataHead);
            cells.Add(rowIndex, 9, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第五行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "车牌号Truck No：", xfDataHead);
            cells.Add(rowIndex, 2, string.Empty, xfDataHead);
            cells.Add(rowIndex, 3, string.Empty, xfDataHead);
            cells.Add(rowIndex, 4, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "档位要求Power rate：", xfDataHead);
            cells.Add(rowIndex, 6, string.Empty, xfDataHead);
            cells.Add(rowIndex, 7, string.Empty, xfDataHead);
            cells.Add(rowIndex, 8, string.Empty, xfDataHead);
            cells.Add(rowIndex, 9, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第六行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "表头信息仓管确认人：", xfDataHead);
            cells.Add(rowIndex, 2, string.Empty, xfDataHead);
            cells.Add(rowIndex, 3, string.Empty, xfDataHead);
            cells.Add(rowIndex, 4, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "出货日期：", xfDataHead);
            cells.Add(rowIndex, 6, string.Empty, xfDataHead);
            cells.Add(rowIndex, 7, string.Empty, xfDataHead);
            cells.Add(rowIndex, 8, string.Empty, xfDataHead);
            cells.Add(rowIndex, 9, string.Empty, xfDataHead);
            cells.Merge(rowIndex, rowIndex, 5, 9);

            XF xfContentHead = xls.NewXF();
            xfContentHead.HorizontalAlignment = HorizontalAlignments.Centered;
            xfContentHead.Font.FontName = "宋体";
            xfContentHead.Font.Bold = true;
            xfContentHead.Font.Height = 10 * 20;
            xfContentHead.UseBorder = true;
            xfContentHead.BottomLineStyle = 1;
            xfContentHead.TopLineStyle = 1;
            xfContentHead.LeftLineStyle = 1;
            xfContentHead.RightLineStyle = 1;
            xfContentHead.CellLocked = false;
            xfContentHead.UseProtection = false;
            xfContentHead.UseNumber = true;
            xfContentHead.UseBackground = true;
            xfContentHead.Pattern = 1;
            xfContentHead.PatternColor = Colors.Default37;
            //第8行
            rowIndex += 2;
            cells.Add(rowIndex, 1, "No.", xfContentHead);
            cells.Add(rowIndex, 2, "Module Type", xfContentHead);
            cells.Add(rowIndex, 3, "Pnom(w)", xfContentHead);
            cells.Add(rowIndex, 4, "PowerRange", xfContentHead);
            cells.Add(rowIndex, 5, "Material Code", xfContentHead);
            cells.Add(rowIndex, 6, "Grade", xfContentHead);
            cells.Add(rowIndex, 7, "Pallet No", xfContentHead);
            cells.Add(rowIndex, 8, "Wp", xfContentHead);
            cells.Add(rowIndex, 9, "Piece", xfContentHead);
            if (includeArtNo)
            {
                cells.Add(rowIndex, 10, "Art.No.", xfContentHead);
            }
            //第 9 行以后开始填充数据
            rowIndex += 1;
            int count = drs.Length;
            XF xfContent = xls.NewXF();
            xfContent.HorizontalAlignment = HorizontalAlignments.Centered;
            xfContent.Font.FontName = "宋体";
            xfContent.Font.Bold = false;
            xfContent.Font.Height = 10 * 20;
            xfContent.UseBorder = true;
            xfContent.BottomLineStyle = 1;
            xfContent.TopLineStyle = 1;
            xfContent.LeftLineStyle = 1;
            xfContent.RightLineStyle = 1;
            xfContent.CellLocked = false;
            xfContent.UseProtection = false;
            xfContent.UseNumber = true;
            xfContent.UseMisc = true;
            double sumWp = 0;
            double sumPiece = 0;
            for (int i = 0; i < count; i++)
            {
                cells.Add(i + rowIndex, 1, i + 1, xfContent);
                //Module Type
                string proId = Convert.ToString(drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_PRO_ID]);
                string[] tmpProId = proId.Split('-');
                string moduleType = proId;
                if (tmpProId.Length > 0)
                {
                    moduleType = tmpProId[0];
                }
                cells.Add(i + rowIndex, 2, moduleType, xfContent);
                //Pnom
                object pnom = drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_LEVEL];
                if (pnom != DBNull.Value && pnom != null)
                {
                    cells.Add(i + rowIndex, 3, pnom, xfContent);
                }
                else
                {
                    cells.Add(i + rowIndex, 3, string.Empty, xfContent);
                }
                //PowerRange add by chao.pang 20140515
                //object powerrange = drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_POWER_RANGE];
                object powerrange = drs[i]["POWERRANGE"];
                if (powerrange != DBNull.Value && powerrange != null)
                {
                    cells.Add(i + rowIndex, 4, powerrange, xfContent);
                }
                else
                {
                    cells.Add(i + rowIndex, 4, string.Empty, xfContent);
                }
                //Material Code
                object mcode = drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_SAP_NO];
                if (mcode != DBNull.Value && mcode != null)
                {
                    cells.Add(i + rowIndex, 5, mcode, xfContent);
                }
                else
                {
                    cells.Add(i + rowIndex, 5, string.Empty, xfContent);
                }
                //Grade
                string gradeName = Convert.ToString(drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE]);
                if (this._dtProductGrade != null)
                {
                    DataRow[] drGrades = this._dtProductGrade.Select(string.Format("Column_code='{0}'", gradeName));
                    if (drGrades.Length > 0)
                    {
                        gradeName = Convert.ToString(drGrades[0]["Column_Name"]);
                    }
                }
                cells.Add(i + rowIndex, 6, gradeName, xfContent);
                //Pallet No
                cells.Add(i + rowIndex, 7, Convert.ToString(drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]), xfContent);
                //Wp
                double wp = 0;
                if (drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_TOTLE_POWER] != DBNull.Value
                    && drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_TOTLE_POWER] != null)
                {
                    wp = Convert.ToDouble(drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_TOTLE_POWER]);
                }
                sumWp += wp;
                cells.Add(i + rowIndex, 8, Math.Round(wp, 2, MidpointRounding.AwayFromZero), xfContent);
                //Piece
                double piece = Convert.ToDouble(drs[i][WIP_CONSIGNMENT_FIELDS.FIELDS_LOT_NUMBER_QTY]);
                sumPiece += piece;
                cells.Add(i + rowIndex, 9, Math.Round(piece, 2, MidpointRounding.AwayFromZero), xfContent);
                //Art.No.
                if (includeArtNo)
                {
                    string artNo = Convert.ToString(drs[i][BASE_POWERSET_COLORATCNO.FIELDS_ARTICNO]);
                    cells.Add(i + rowIndex, 10, artNo, xfContent);
                }
            }
            //第9+count行
            rowIndex += count;
            xfContent.FormulaHidden = true;
            cells.Add(rowIndex, 7, Math.Round(sumWp / sumPiece, 2, MidpointRounding.AwayFromZero), xfContent);
            cells.Add(rowIndex, 8, Math.Round(sumWp, 2, MidpointRounding.AwayFromZero), xfContent);
            cells.Add(rowIndex, 9, Math.Round(sumPiece, 2, MidpointRounding.AwayFromZero), xfContent);
            //第10+count行
            rowIndex += 2;
            xfContent.HorizontalAlignment = HorizontalAlignments.Left;
            cells.Add(rowIndex, 1, "内部柜号：", xfContent);
            cells.Add(rowIndex, 2, string.Empty, xfContent);
            cells.Add(rowIndex, 3, string.Empty, xfContent);
            cells.Add(rowIndex, 4, string.Empty, xfContent);
            cells.Merge(rowIndex, rowIndex, 1, 4);            
            cells.Add(rowIndex, 5, "库位：", xfContent);
            cells.Add(rowIndex, 6, string.Empty, xfContent);
            cells.Merge(rowIndex, rowIndex, 5, 6);
            cells.Add(rowIndex, 7, "制单人签名：", xfContent);
            cells.Add(rowIndex, 8, string.Empty, xfContent);
            cells.Add(rowIndex, 9, string.Empty, xfContent);
            cells.Merge(rowIndex, rowIndex, 7, 9);


            XF xfFooter = xls.NewXF();
            xfFooter.HorizontalAlignment = HorizontalAlignments.Left;
            xfFooter.Font.FontName = "宋体";
            xfFooter.Font.Bold = true;
            xfFooter.Font.Height = 11 * 20;
            xfFooter.UseBorder = true;
            xfFooter.BottomLineStyle = 1;
            xfFooter.TopLineStyle = 1;
            xfFooter.LeftLineStyle = 1;
            xfFooter.RightLineStyle = 1;
            xfFooter.CellLocked = false;
            xfFooter.UseProtection = false;
            xfFooter.UseNumber = true;
            //第12+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "仓管确认", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Add(rowIndex, 5, string.Empty, xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 9);
            //第13+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "料号确认：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Add(rowIndex, 5, string.Empty, xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 9);
            //第14+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "档位功率范围确认：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "叉车司机：", xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第15+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "客检托确认：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "托号确认：", xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第16+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "首托确认：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "尾托确认：", xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第17+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "质检确认", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Add(rowIndex, 5, string.Empty, xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 9);
            //第18+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "船名航次确认：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "客户名称确认：", xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 5, 9);

            //第19+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "提单号确认：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "发票号确认：", xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第20+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "关单号确认：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "目的地确认：", xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第21+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "集装箱号确认：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "封条号确认：", xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第22+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "车牌号确认：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "档位功率范围确认：", xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第23+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "料号确认：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Add(rowIndex, 5, string.Empty, xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 9);
            //第24+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "客检托确认：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "托号确认：", xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第25+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "首托确认：", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 4);
            cells.Add(rowIndex, 5, "尾托确认：", xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 5, 9);
            //第26+count行
            rowIndex += 1;
            cells.Add(rowIndex, 1, "备注:信息确认需要填写实际内容并签名,核对范围包括提货单、出货DN、出货清单、出货实物上的相关信息.", xfFooter);
            cells.Add(rowIndex, 2, string.Empty, xfFooter);
            cells.Add(rowIndex, 3, string.Empty, xfFooter);
            cells.Add(rowIndex, 4, string.Empty, xfFooter);
            cells.Add(rowIndex, 5, string.Empty, xfFooter);
            cells.Add(rowIndex, 6, string.Empty, xfFooter);
            cells.Add(rowIndex, 7, string.Empty, xfFooter);
            cells.Add(rowIndex, 8, string.Empty, xfFooter);
            cells.Add(rowIndex, 9, string.Empty, xfFooter);
            cells.Merge(rowIndex, rowIndex, 1, 9);
            //第27+count行
            rowIndex += 1;
            xfContent.HorizontalAlignment = HorizontalAlignments.Left;
            cells.Add(rowIndex, 1, "司机签名：", xfContent);
            cells.Add(rowIndex, 2, string.Empty, xfContent);
            cells.Add(rowIndex, 3, string.Empty, xfContent);
            cells.Merge(rowIndex, rowIndex, 1, 3);
            cells.Add(rowIndex, 4, "仓储领班审核：", xfContent);
            cells.Add(rowIndex, 5, string.Empty, xfContent);
            cells.Add(rowIndex, 6, string.Empty, xfContent);
            cells.Merge(rowIndex, rowIndex, 4, 6);
            cells.Add(rowIndex, 7, "QC领班审核：", xfContent);
            cells.Add(rowIndex, 8, string.Empty, xfContent);
            cells.Add(rowIndex, 9, string.Empty, xfContent);
            cells.Merge(rowIndex, rowIndex, 7, 9);
        }

        /// <summary>
        /// 获取查询条件。
        /// </summary>
        /// <returns>包含查询条件的数据集对象。</returns>
        private DataSet GetQueryCondition()
        {
            string shipmentNo = this.teShipmentNo.Text.Trim();
            string containerNo = this.teContainerNo.Text.Trim();
            string ciNo = this.teCINumber.Text.Trim();
            string shipmentType = Convert.ToString(this.lueShipmentType.EditValue);
            string palletNo = this.tePalletNo.Text.Trim();
            string status = this.cmbStatus.Text.Trim();
            string timeStart = this.timeEditStart.EditValue == null ? "" : this.timeEditStart.EditValue.ToString();
            string timeEnd = this.timeEditEnd.EditValue == null ? "" : this.timeEditEnd.EditValue.ToString();
            Hashtable htParams = new Hashtable();
            if (!string.IsNullOrEmpty(shipmentNo))
            {
                htParams.Add(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_NO, shipmentNo);
            }
            if (!string.IsNullOrEmpty(containerNo))
            {
                htParams.Add(WMS_SHIPMENT_FIELDS.FIELDS_CONTAINER_NO, containerNo);
            }
            if (!string.IsNullOrEmpty(shipmentType))
            {
                htParams.Add(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_TYPE, shipmentType);
            }
            if (!string.IsNullOrEmpty(palletNo))
            {
                htParams.Add(WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO, palletNo);
            }
            if (!string.IsNullOrEmpty(ciNo))
            {
                htParams.Add(WMS_SHIPMENT_FIELDS.FIELDS_CI_NO, ciNo);
            }
            if (!string.IsNullOrEmpty(status))
            {
                htParams.Add(WMS_SHIPMENT_FIELDS.FIELDS_STATUS , status);
            }
            if (!string.IsNullOrEmpty(timeStart))
            {
                htParams.Add(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_DATE + "_START", timeStart);
            }
            if (!string.IsNullOrEmpty(timeEnd))
            {
                htParams.Add(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_DATE + "_END", timeEnd);
            }
            DataTable dtParams = CommonUtils.ParseToDataTable(htParams);
            DataSet dsParams = new DataSet();
            dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dsParams.Tables.Add(dtParams);
            return dsParams;
        }
        /// <summary>
        /// 分页导航事件。
        /// </summary>
        private void pgnQueryResult_DataPaging()
        {
            btnQuery_Click(null, null);
        }
        /// <summary>
        /// 绘制自定义单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvList_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gclShipmentType)
            {
                DataTable dtShipmentType = this.lueShipmentType.Properties.DataSource as DataTable;
                DataRow []drs=dtShipmentType.Select(string.Format("CODE='{0}'",e.CellValue));
                if(drs.Length>0){
                    e.DisplayText=Convert.ToString(drs[0]["NAME"]);
                }
            }
        }
        /// <summary>
        /// 根据托盘号导入BCP数据。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFromBCP_Click(object sender, EventArgs e)
        {
            string palletNo = this.tePalletNo.Text.Trim();
            if (string.IsNullOrEmpty(palletNo))
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.msg.0002}"));//请输入托盘号。
                this.tePalletNo.Select();
                return;
            }
            if (palletNo.Length > 2048)
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.msg.0001}"));//托盘号长度必须小于等于2048个字符。
                this.tePalletNo.Select();
                return;
            }
            this._entity.ImportPalletDataFromBCP(palletNo);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageBox.Show(this._entity.ErrorMsg);
                this.tePalletNo.Select();
            }
            else
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentQueryNewCtrl.msg.0003}"));//已从BCP数据库导入指定托盘数据。
            }
        }

        private void tsbExportIncludeArtNo_Click(object sender, EventArgs e)
        {
            ExportExcel(false,true);
        }

        private void tsbExportNew_Click(object sender, EventArgs e)
        {
            ExportExcel(true, false);
        }

        private void gvList_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gcList_Click(object sender, EventArgs e)
        {

        }
    }
}
