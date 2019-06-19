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
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WMS
{
    public partial class WarehouseWarrantSynCtrl : BaseUserCtrl
    {
        IViewContent _view = null;
        WarehouseWarrantOperationEntity _entity = new WarehouseWarrantOperationEntity();
        public int retIdx;

        const int COL_NUM = 10;         //默认的列数

        public WarehouseWarrantSynCtrl(IViewContent view)
        {
            InitializeComponent();
            this._view = view;
            GridViewHelper.SetGridView(gridView1);
        }

        /// <summary>
        /// 修改订单类型时获取订单类型下相应的特征名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZMMTYPChanged(string ZMMTYP)
        {
            //获取特性值
            DataSet ds = this._entity.GetATNAM(ZMMTYP);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }

            while (gridView1.Columns.Count > COL_NUM)
            {
                gridView1.Columns.RemoveAt(COL_NUM);
            }

            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DevExpress.XtraGrid.Columns.GridColumn gc = new DevExpress.XtraGrid.Columns.GridColumn();
                    gc.FieldName = dt.Rows[i][2].ToString().Remove(2, 1);
                    gc.Caption  = dt.Rows[i][4].ToString();
                    gc.Visible  = true;
                    gc.Name     = "col" + dt.Rows[i][2].ToString();
                    gc.VisibleIndex = i + COL_NUM;
                    gc.OptionsColumn.ReadOnly = true;
                    gc.Width = 100;
                    gridView1.Columns.Add(gc);
                }
                gridView1.RefreshData();
            }
        }

        /// <summary>
        /// 查询入库单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbQuery_Click(object sender, EventArgs e)
        {
            IntialScreen();

            DataSet dsHead = this._entity.QueryWarehouseWarrantHead(teZMBLNRCnd.Text.Trim(), "0", true);
            
            if (!String.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageBox.Show(this._entity.ErrorMsg, "错误", MessageBoxButtons.OK);
                return;
            }

            if (dsHead.Tables[0].Rows.Count <= 0)
            {
                MessageService.ShowMessage("入库单不存在或已被删除！", "系统提示！");
                return;
            }
            else if (dsHead.Tables[0].Rows.Count > 1)
            {
                SelectZMBLNRFrm selectZMBLNR = new SelectZMBLNRFrm(dsHead.Tables[0], 2);
                selectZMBLNR.PWarehouseWarrantSyn = this;
                selectZMBLNR.ShowDialog();
            }
            else
                retIdx = 0;

            if (retIdx >= 0)
            {
                teZMBLNRCnd.Text = dsHead.Tables[0].Rows[retIdx]["ZMBLNR"].ToString();
                DataSet dsItem = this._entity.QueryWarehouseWarrantItems(teZMBLNRCnd.Text);

                if (!String.IsNullOrEmpty(this._entity.ErrorMsg))
                {
                    MessageBox.Show(this._entity.ErrorMsg, "错误", MessageBoxButtons.OK);
                    return;
                }

                DataRow dr = dsHead.Tables[0].Rows[retIdx];
                teZMBLNR.Text = dr["ZMBLNR"].ToString();   //入库单号
                cbeWerks.Text = dr["WERKS"].ToString();     //工厂
                cbeOrderType.Text = dr["ZMMTYP"].ToString();    //订单类型
                teOrderNo.Text = dr["AUFNR"].ToString();    //工单号码
                teDept.Text = dr["DEPT"].ToString();        //部门
                cbeOEMNo.Text = dr["VBELN_OEM"].ToString(); //OEM发货单
                ZMMTYPChanged(cbeOrderType.Text);

                gcItems.DataSource = dsItem.Tables[0];
                gridView1.RefreshData();
            }
        }

        /// <summary>
        /// 查询入库单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teWWNoModify_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                sbQuery_Click(sender, e);
            }
        }

        /// <summary>
        /// 同步过账
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSyn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(teZMBLNR.Text))
            {
                MessageService.ShowMessage("未获取需同步的入库单信息！", "系统提示");
                return;
            }

            if (MessageBox.Show("确定执行过账？", "确定", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                layoutControlItem4.Visibility = LayoutVisibility.Always;
                btnSyn.Enabled = false;
                sbQuery.Enabled = false;
                teZMBLNRCnd.Enabled = false;
                
                string returnStr;
                bool isSuccess = this._entity.SynSAP(teZMBLNR.Text, PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ), out returnStr);
                MessageService.ShowMessage(returnStr, "系统提示");
                if (isSuccess)
                {
                    IntialScreen();
                }
                btnSyn.Enabled = true;
                sbQuery.Enabled = true;
                teZMBLNRCnd.Enabled = true;
                layoutControlItem4.Visibility = LayoutVisibility.Never;
            }
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void IntialScreen()
        {
            retIdx = -1;
            teZMBLNR.Text = String.Empty;
            cbeOrderType.Text = String.Empty;
            cbeWerks.Text = String.Empty;
            teOrderNo.Text = String.Empty;
            teDept.Text = String.Empty;
            cbeOEMNo.Text = String.Empty;

            while (gridView1.Columns.Count > COL_NUM)
            {
                gridView1.Columns.RemoveAt(COL_NUM);
            }
            gcItems.DataSource = null;
           
        }
        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
