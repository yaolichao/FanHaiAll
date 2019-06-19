using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using Microsoft.Reporting.WinForms;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using System.Threading;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class WarehousePrintDialog : BaseDialog
    {
        public string ReportName = string.Empty;
        public DataSet ReportData = null;

        public WarehousePrintDialog(string title)
            : base(title)
        {
            InitializeComponent();
        }

        private void WarehousePrintDialog_Load(object sender, EventArgs e)
        {
            //获取或设置嵌入报表的资源的名称
            this.rptViewer.LocalReport.ReportEmbeddedResource = this.ReportName;
            if (this.ReportData != null)
            {
                this.rptViewer.LocalReport.DataSources.Clear();
                if (this.ReportData.ExtendedProperties.Count > 0)
                {
                    List<ReportParameter> rps = new List<ReportParameter>();

                    foreach (string key in this.ReportData.ExtendedProperties.Keys)
                    {
                        ReportParameter rp = new ReportParameter(key, this.ReportData.ExtendedProperties[key].ToString());
                        rps.Add(rp);
                    }

                    //设置本地报表的报表参数属性
                    this.rptViewer.LocalReport.SetParameters(rps);
                }

                if (this.ReportData.Tables.Count > 0)
                {
                    this.rptViewer.LocalReport.DataSources.Clear();

                    string dsName = this.ReportData.DataSetName;

                    foreach (DataTable table in this.ReportData.Tables)
                    {
                        ReportDataSource rds = new ReportDataSource(string.Format("{0}_{1}", dsName, table.TableName), table);

                        this.rptViewer.LocalReport.DataSources.Add(rds);
                    }
                }
                this.rptViewer.SetDisplayMode(DisplayMode.PrintLayout);
                this.rptViewer.RefreshReport();
            }

            Thread.Sleep(100);
            this.rptViewer.Refresh();
        }

        private void rptViewer_Print(object sender, CancelEventArgs e)
        {
            //update 打印表 新增一笔记录
            WarehouseEngine wEngine = new WarehouseEngine();
            string inWarehouseNo = Convert.ToString(this.ReportData.ExtendedProperties["Report_InwarehouseNo"]);
            string name = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
            wEngine.InsertPrintInf(inWarehouseNo, name);
        }
    }
}
