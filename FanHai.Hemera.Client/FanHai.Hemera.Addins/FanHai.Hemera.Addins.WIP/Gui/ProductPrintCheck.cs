using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;

using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class ProductPrintCheck : BaseUserCtrl
    {
        public DataSet dsIVTest;
        public ProductPrintCheck()
        {
            InitializeComponent();
            //deStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //deEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

 
        private void btnQuery_Click(object sender, EventArgs e)
        {
            string sSLotNum, sELotNum, sSTestTime, sETestTime, sSDevice, sEDevice, sDefult, sVC_CONTROL, sWorkNum;

            sSLotNum = txtSLotNum.Text.Trim();
            sELotNum = txtELotNum.Text.Trim();
            sSTestTime = deStartTime.Text;
            sETestTime = deEndTime.Text;
            sSDevice = "";
            sEDevice = "";
            sDefult = "1";
            sVC_CONTROL = "N";
            sWorkNum = txtWorkNum.Text.Trim();
            //sDefult = "1";

            IVTestDataEntity IVTestDateObject = new IVTestDataEntity();
            dsIVTest = IVTestDateObject.GetIVTestData2(sWorkNum,sSLotNum, sELotNum, sSDevice, sEDevice, sSTestTime, sETestTime, sDefult, sVC_CONTROL);
            if (string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
            {
                gcTestData.DataSource = null;
                gcTestData.MainView = gvTestData;
                gcTestData.DataSource = dsIVTest.Tables[0];
                gvTestData.BestFitColumns();//自动调整列宽度
                gvTestData.IndicatorWidth = 50;//自动调整行容器宽度
            }
            else
            {
                MessageService.ShowError(IVTestDateObject.ErrorMsg);
                return;
            }
            chDefault.Checked = false;
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            string sRowKey = string.Empty;
            string sql;

            if (gvTestData.FocusedRowHandle < 0 || gvTestData.RowCount < 1)
            {
                MessageService.ShowMessage("请选择编辑的数据!", "提示");
                return;
            }
            //DataRow drEdit = gvTestData.GetFocusedDataRow();
            //sRowKey = drEdit["IV_TEST_KEY"].ToString().Trim();
            //int sRowcount = gvTestData.DataRowCount;
            for (int j = 0; j < gvTestData.DataRowCount; j++)
            {
                if (gvTestData.GetRowCellValue(j, gvTestData.Columns["ImpIsc_Control"]).ToString() == "Y")
                {
                    sRowKey += "'" + gvTestData.GetRowCellValue(j, gvTestData.Columns["IV_TEST_KEY"]).ToString() + "',";
                }
            }
            if (!string.IsNullOrEmpty(sRowKey))
            {
                sRowKey = sRowKey.Substring(0, sRowKey.Length - 1);
                IVTestDataEntity IVTestDateObject = new IVTestDataEntity();
                if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
                {
                    MessageService.ShowError(IVTestDateObject.ErrorMsg);
                    return;
                }

                sql = "UPDATE WIP_IV_TEST SET ImpIsc_Control='Y',ImpIsc_ReleaseUserId=" + PropertyService.Get(PROPERTY_FIELDS.USER_NAME) + " WHERE IV_TEST_KEY in(" + sRowKey + ")";//PropertyService.Get(PROPERTY_FIELDS.USER_NAME)
                DataSet dsSetDefult = IVTestDateObject.UpdateData(sql, "UpdateIVTestData");
                int nSetDefult = int.Parse(dsSetDefult.ExtendedProperties["rows"].ToString());
                if (nSetDefult < 1)
                {
                    MessageService.ShowMessage("无数据更新！", "提示");
                    return;
                }
                else
                {
                    MessageService.ShowMessage("已释放卡控！", "提示");
                }
                if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
                {
                    MessageService.ShowError(IVTestDateObject.ErrorMsg);
                    return;
                }
                btnQuery_Click(null, null);
            }
            else {
                MessageService.ShowMessage("请选择！", "提示");
                return;
            }
        }

        private void chDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (chDefault.Checked == true)
            {
                //int[] rows = gvTestData.GetSelectedRows(); //选中多行   
                //string aa = gvTestData.GetRowCellDisplayText(rows[0], "列字段名").ToString();
                int columnscount = gvTestData.DataRowCount;
                for (int i = 0; i < columnscount; i++)
                {
                    gvTestData.SetRowCellValue(i, gvTestData.Columns["ImpIsc_Control"], "Y");
                }
            }
            else
            {
                int columnscount = gvTestData.DataRowCount;
                for (int i = 0; i < columnscount; i++)
                {
                    gvTestData.SetRowCellValue(i, gvTestData.Columns["ImpIsc_Control"], "N");
                }
            }
            //else
            //{
                    //int columnscount = gridView1.DataRowCount;
                    //for (int i = 0; i < columnscount; i++)
                    //{
                    //    gridView1.SetRowCellValue(i, gridView1.Columns["selected"], true);
                    //}
                    //gridControl1.Refresh();
            //}
        }


        //private void VC_CONTROLToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    string sRowKey = string.Empty;
        //    string sql;

        //    if (gvTestData.FocusedRowHandle < 0 || gvTestData.RowCount < 1)
        //    {
        //        MessageService.ShowMessage("请选择编辑的数据!", "提示");
        //        return;
        //    }
        //    DataRow drEdit = gvTestData.GetFocusedDataRow();
        //    sRowKey = drEdit["IV_TEST_KEY"].ToString().Trim();
        //    if (!string.IsNullOrEmpty(sRowKey))
        //    {
        //        IVTestDataEntity IVTestDateObject = new IVTestDataEntity();
        //        if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
        //        {
        //            MessageService.ShowError(IVTestDateObject.ErrorMsg);
        //            return;
        //        }

        //        sql = "UPDATE WIP_IV_TEST SET VC_CONTROL='Y' WHERE IV_TEST_KEY='" + sRowKey + "'";
        //        DataSet dsSetDefult = IVTestDateObject.UpdateData(sql, "UpdateIVTestData");
        //        int nSetDefult = int.Parse(dsSetDefult.ExtendedProperties["rows"].ToString());
        //        if (nSetDefult < 1)
        //        {
        //            MessageService.ShowMessage("无数据更新！", "提示");
        //            return;
        //        }
        //        else
        //        {
        //            MessageService.ShowMessage("已释放卡控！", "提示");
        //        }
        //        if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
        //        {
        //            MessageService.ShowError(IVTestDateObject.ErrorMsg);
        //            return;
        //        }

        //    }
        //}

        //private void gvTestData_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        //{
            //e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;//行号居中
            //if (e.Info.IsRowIndicator)
            //{
            //    if (e.RowHandle >= 0)
            //    {
            //        e.Info.DisplayText = (e.RowHandle + 1).ToString();//添加行号
            //    }
            //    else if (e.RowHandle < 0 && e.RowHandle > -1000)
            //    {
            //        e.Info.Appearance.BackColor = System.Drawing.Color.AntiqueWhite;
            //        e.Info.DisplayText = "G" + e.RowHandle.ToString();
            //    }
            //}
        //}

    }
}
