using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;

namespace FanHai.Hemera.Addins.WMS.Gui
{
    public partial class OutDeliveryQuerryCtrl : BaseUserCtrl
    {
        //IViewContent _view = null;
        OutDeliveryQuerryEngine _entity = new OutDeliveryQuerryEngine();

        public OutDeliveryQuerryCtrl()//IViewContent view
        {
            InitializeComponent();
            //this._view = view;
        }

        private void OutDeliveryQuerryCtrl_Load(object sender, EventArgs e)
        {
            try
            {
                string[] columns = new string[] { "Werks", "Desc" };
                KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Werks");
                DataTable dt = BaseData.Get(columns, category);
                DataRow dr = dt.Rows.Add();
                dr["Werks"] = string.Empty;
                dr["Desc"] = string.Empty;
                //dt.Rows.Add(dr);
                this.lookUpPLANT.Properties.DataSource = dt;//BaseData.Get(columns, category);
                this.lookUpPLANT.Properties.DisplayMember = "Desc";
                this.lookUpPLANT.Properties.ValueMember = "Werks";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }

        private DataSet GetQueryCondition()
        {
            Hashtable htParams = new Hashtable();
            if (!(teOUTBANDNO.Text == ""))
            {
                htParams.Add("OUTBANDNO", teOUTBANDNO.Text);
            }
            if (!(teVBELN.Text == ""))
            {
                htParams.Add("VBELN", teVBELN.Text);
            }
            if (!(teCREATED_BY.Text == ""))
            {
                htParams.Add("CREATED_BY", teCREATED_BY.Text);
            }
            if (!(teSALESTO.Text == ""))
            {
                htParams.Add("SALESTO", teSALESTO.Text);
            }
            if (!(teXHGG.Text == ""))
            {
                htParams.Add("XHGG", teXHGG.Text);
            }
            if (!(lookUpPLANT.Text == ""))//if (!(lookUpPLANT.Text == ""))
            {
                htParams.Add("PLANT", lookUpPLANT.EditValue.ToString());
            }
            if (!(comboBoxSTATUS.Text == ""))
            {
                htParams.Add("STATUS", comboBoxSTATUS.Text);
            }
            if (!(teBATCHNO.Text == ""))
            {
                htParams.Add("BATCHNO", teBATCHNO.Text);
            }
            if (!(teCabinet_NO.Text == ""))
            {
                htParams.Add("Cabinet_NO", teCabinet_NO.Text);
            }
            if (!(teCI.Text == ""))
            {
                htParams.Add("CI", teCI.Text);
            }
            if (!(teShipmentNo.Text == ""))
            {
                htParams.Add("ShipmentNo", teShipmentNo.Text);
            }
            if (!(dateLFDAT1.Text == ""))
            {
                htParams.Add("LFDAT1", dateLFDAT1.Text);
            }
            if (!(dateLFDAT2.Text == ""))
            {
                htParams.Add("LFDAT2", dateLFDAT2.Text);
            }
            if (!(dateQCDAT1.Text == ""))
            {
                htParams.Add("QCDAT1", dateQCDAT1.Text);
            }
            if (!(dateQCDAT2.Text == ""))
            {
                htParams.Add("QCDAT2", dateQCDAT2.Text);
            }
            DataTable dtParams = CommonUtils.ParseToDataTable(htParams);
            DataSet dsParams = new DataSet();
            dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dsParams.Tables.Add(dtParams);
            return dsParams;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            teOUTBANDNO.Text = String.Empty;
            teVBELN.Text = String.Empty;
            teCREATED_BY.Text = String.Empty;
            teSALESTO.Text = String.Empty;
            teXHGG.Text = String.Empty;
            lookUpPLANT.SelectedText = String.Empty;
            comboBoxSTATUS.SelectedItem = 0;
            teBATCHNO.Text = String.Empty;
            teCabinet_NO.Text = String.Empty;
            teCI.Text = String.Empty;
            teShipmentNo.Text = String.Empty;
            dateLFDAT1.Text = String.Empty;
            dateLFDAT2.Text = String.Empty;
            dateQCDAT1.Text = String.Empty;
            dateQCDAT2.Text = String.Empty;
            gridControl1.DataSource = null;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DataSet dsParams = this.GetQueryCondition();
            DataSet dsReturn = this._entity.Query(dsParams);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                gridControl1.DataSource = null;
                //gridControl1.Refresh();
                gridView1.RefreshData();
                return;
            }
            if (dsReturn.Tables[0].Rows.Count > 0)
            {
                DataTable dt = dsReturn.Tables[0];
                gridControl1.DataSource = dt;
                //this.gridControl1.MainView = gridView1;
            }

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //this._view.WorkbenchWindow.CloseWindow(false);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if ((gridView1.RowCount > 0) || (gridView1.GetSelectedRows().Length > 0))
            {
                int selectedRow = gridView1.GetSelectedRows()[0];
                if (gridView1.GetSelectedRows()[0] >= gridView1.RowCount)
                {
                    gridView1.SelectRow(0);
                    selectedRow = 0;
                }
                string celvalue1 = gridView1.GetRowCellValue(selectedRow, VBELN).ToString();
                string celvalue2 = gridView1.GetRowCellValue(selectedRow, POSNR).ToString();

                DataSet dsReturn = this._entity.GetQCItem(celvalue1, celvalue2);
                if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                {
                    MessageService.ShowMessage(this._entity.ErrorMsg);
                    return;
                }
                if (dsReturn.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsReturn.Tables[0];
                    OutDeliveryQCQuerry f = new OutDeliveryQCQuerry(dt);
                    //f.MdiParent = this;
                    f.Show();
                    
                }
            }
        }

    }
}
