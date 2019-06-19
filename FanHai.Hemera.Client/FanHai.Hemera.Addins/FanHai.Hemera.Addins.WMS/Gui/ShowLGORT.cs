using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraGrid.Views.Grid;

namespace FanHai.Hemera.Addins.WMS
{
    public partial class ShowLGORT : BaseDialog
    {
        string werks;
        public ShowLGORT(string werks)
        {
            InitializeComponent();
            this.werks = werks;
        }

        public WarehouseWarrantCtrl pwarehouseWarrantCtrl
        {
            get;
            set;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            teLGORT.Text = teLGORT.Text.Trim();
            teDESC.Text = teDESC.Text.Trim();
            string LGORT = teLGORT.Text;
            string DESC = teDESC.Text;

            string[] columns = new string[] { "LGORT", "DESC", "WERKS"};

            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_LGORT");
            DataTable dt = BaseData.Get(columns, category);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (!String.IsNullOrEmpty(werks))
                {
                    string cWERKS = dr["WERKS"].ToString();
                    if (cWERKS.IndexOf(werks) == -1)
                    {
                        dt.Rows.RemoveAt(i--);
                        continue;
                    }
                }
                if (!String.IsNullOrEmpty(LGORT))
                {
                    string cLGORT = dr["LGORT"].ToString();
                    if (cLGORT.IndexOf(LGORT) == -1)
                    {
                        dt.Rows.RemoveAt(i--);
                        continue;
                    }
                }
                if (!String.IsNullOrEmpty(DESC))
                {
                    string cDesc = dr["DESC"].ToString();
                    if (cDesc.IndexOf(DESC) == -1)
                        dt.Rows.RemoveAt(i--);
                }
            }
            this.gridControl1.DataSource = dt;
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                string cellValue = gridView1.GetRowCellValue(e.RowHandle, colLGORT).ToString();
                pwarehouseWarrantCtrl.LGORTVal = cellValue;
                this.Close();
            }
        }

        private void ShowLGORT_Shown(object sender, EventArgs e)
        {
            btnQuery_Click(sender, e);
        }
    }
}
