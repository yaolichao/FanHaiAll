using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Dialogs;
using FanHai.Hemera.Utils.Entities;
using System.Collections;
using System.Net;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 返工单作业窗体对话框。
    /// </summary>
    public partial class WarehouseSelectForm : BaseDialog
    {
        public string rknumber = string.Empty;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="repair"></param>
        public WarehouseSelectForm()
        {
            InitializeComponent();
        }

        //数据绑定-------------------------------------------------------------------------------
        /// <summary>
        /// 绑定工厂车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            string stores = PropertyService.Get(PROPERTY_FIELDS.STORES);
            DataTable dt = FactoryUtils.GetFactoryRoomByStores(stores);

            string[] columns = new string[] { "MESDATASOURCE", "ERPFACTORY" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "MEScontrastERP");
            DataTable dtFac = BaseData.Get(columns, category);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drF = dt.Rows[i];
                for (int j = 0; j < dtFac.Rows.Count; j++)
                {
                    DataRow drFac = dtFac.Rows[j];
                    if (drFac["MESDATASOURCE"].ToString().Trim() == drF["LOCATION_NAME"].ToString().Trim())
                    {
                        this.cbeWerks.Text = drFac["ERPFACTORY"].ToString();
                    }
                }
            }
        }
        //---------------------------------------------------------------------------------------
        private void WarehouseSelectForm_Load(object sender, EventArgs e)
        {
            dateStartTime.EditValue = DateTime.Now.ToString("yyyy-MM-01");
            dateEndTime.EditValue = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            gcrkInf.DataSource = null;
            BindFactoryRoom();
        }

        private void btnRef_Click(object sender, EventArgs e)
        {
            dateStartTime.EditValue = DateTime.Now.ToString("yyyy-MM-01");
            dateEndTime.EditValue = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            gcrkInf.DataSource = null;
            BindFactoryRoom();
            txtRknum.Text = "";
            txtWorkOrder.Text = "";
            txtStatus.Text = "";
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                WarehouseEngine whe = new WarehouseEngine();
                DataSet ds = whe.GetRkKoInformation(txtRknum.Text.Trim(), cbeWerks.Text.Trim(),
                    txtWorkOrder.Text.Trim(), txtStatus.Text.Trim(),
                    dateStartTime.EditValue.ToString(), dateEndTime.EditValue.ToString());
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                //DataTable dt01 = new DataTable();
                //if (!dt01.Columns.Contains("ROWNUMBER"))
                //        dt01.Columns.Add("ROWNUMBER");
                //for (int i = 1; i < dt01.Rows.Count + 1; i++)
                //        dt01.Rows[i - 1]["ROWNUMBER"] = i.ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(dt.Rows[i]["RKSTATUS"].ToString()))
                    {
                        dt.Rows[i]["RKSTATUS"] = "已创建";
                        continue;
                    }
                    if (dt.Rows[i]["RKSTATUS"].ToString() == "W")
                    {
                        dt.Rows[i]["RKSTATUS"] = "未审批";
                        continue;
                    }
                    if (dt.Rows[i]["RKSTATUS"].ToString() == "A")
                    {
                        dt.Rows[i]["RKSTATUS"] = "审批通过";
                        continue;
                    }
                    if (dt.Rows[i]["RKSTATUS"].ToString() == "R")
                    {
                        dt.Rows[i]["RKSTATUS"] = "拒绝";
                        continue;
                    }
                    if (dt.Rows[i]["RKSTATUS"].ToString() == "T")
                    {
                        dt.Rows[i]["RKSTATUS"] = "已过账";
                        continue;
                    }
                    if (dt.Rows[i]["RKSTATUS"].ToString() == "D")
                    {
                        dt.Rows[i]["RKSTATUS"] = "已删除";
                        continue;
                    }
                }
                gcrkInf.DataSource = dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "系统错误提示");
                return;
            }

        }

        private void gvrkInf_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        /// <summary>
        /// 获取点击的视图上的行信息
        /// </summary>
        /// <returns></returns>
        private bool MapSelectedItemToProperties()
        {
            int rowHandle =  gvrkInf.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                rknumber = gvrkInf.GetRowCellValue(rowHandle, "ZMBLNR").ToString().Trim();
                return true;
            }
            return false;
        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}