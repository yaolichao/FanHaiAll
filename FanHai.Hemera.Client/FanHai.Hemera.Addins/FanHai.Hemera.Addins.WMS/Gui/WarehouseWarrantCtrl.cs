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
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WMS
{
    public partial class WarehouseWarrantCtrl : BaseUserCtrl
    {
        IViewContent _view = null;
        WarehouseWarrantOperationEntity _entity = new WarehouseWarrantOperationEntity();

        DataTable dtEntryItems = new DataTable();
        DataTable _dtEntryItems = new DataTable();
        DataTable _dtProductGrade = null;
        const int COL_NUM = 10;         //默认的列数
        public string LGORTVal;
        public int retIdx;

        public DialogResult DialogResult { get; private set; }

        public WarehouseWarrantCtrl(IViewContent view)
        {
            InitializeComponent();
            this._view = view;
        }

        /// <summary>
        /// 窗口登录 获取工厂数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WarehouseWarrantCtrl_Load(object sender, EventArgs e)
        {
            try
            {
                string[] columns = new string[] { "Werks", "Desc" };
                KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Werks");
                this.cbeWerks.Properties.DataSource = BaseData.Get(columns, category);
                this.cbeWerks.Properties.DisplayMember = "Desc";
                this.cbeWerks.Properties.ValueMember = "Werks";

                gcItems.DataSource = dtEntryItems;
                cbeOrderType_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }

        /// <summary>
        /// 修改订单类型时获取订单类型下相应的特征名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbeOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //获取特性值
            DataSet ds = this._entity.GetATNAM(cbeOrderType.Text);
            if (!String.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }

            while (gridView1.Columns.Count > COL_NUM)
            {
                gridView1.Columns.RemoveAt(COL_NUM);
            }

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

                if (gc.FieldName.Equals("XP003"))
                {
                    RepositoryItemComboBox cbxXP003 = new RepositoryItemComboBox();
                    cbxXP003.Items.Add("H");
                    cbxXP003.Items.Add("L");
                    cbxXP003.Items.Add("否");
                    gc.ColumnEdit = cbxXP003;
                    gc.OptionsColumn.ReadOnly = false;
                }
                gridView1.Columns.Add(gc);
            }
            gridView1.RefreshData();
        }

        /// <summary>
        /// 保存入库单 新增、修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //数据检查
            if (cbeOrderType.Text.Equals(String.Empty))
            {
                MessageService.ShowMessage("订单类型不能为空！", "提示");
                return;
            }
            if (cbeWerks.Text.Equals(String.Empty))
            {
                MessageService.ShowMessage("工厂不能为空！", "提示");
                return;
            }
            for (int i = 0; i < dtEntryItems.Rows.Count; i++)
            {
                DataRow dr = dtEntryItems.Rows[i];
                if (String.IsNullOrEmpty(dr["LGORT"].ToString()))
                {
                    MessageService.ShowMessage("库位不能为空！", "提示");
                    return;
                }
            }

            //抬头数据
            DataTable dtHead = new DataTable("AWMS_WH_ENTRY");
            dtHead.Columns.Add("ZMBLNR", Type.GetType("System.String"));
            dtHead.Columns.Add("WERKS", Type.GetType("System.String"));
            dtHead.Columns.Add("ZMMTYP", Type.GetType("System.String"));
            dtHead.Columns.Add("AUFNR", Type.GetType("System.String"));
            dtHead.Columns.Add("VBELN_OEM", Type.GetType("System.String"));
            dtHead.Columns.Add("DEPT", Type.GetType("System.String"));
            dtHead.Columns.Add("CREATOR", Type.GetType("System.String"));
            dtHead.Columns.Add("ISMODIFY", Type.GetType("System.String"));

            DataRow drHead = dtHead.NewRow();
            drHead["WERKS"] = cbeWerks.Properties.GetKeyValueByDisplayText(cbeWerks.Text);
            drHead["ZMMTYP"] = cbeOrderType.Text;
            drHead["AUFNR"] = teOrderNo.Text.Trim();
            drHead["VBELN_OEM"] = cbeOEMNo.Text;
            drHead["DEPT"] = teDept.Text.Trim();
            drHead["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
            drHead["ZMBLNR"] = teWWNoAdd.Text;
            dtHead.Rows.Add(drHead);

            string returnInfo;
            int returnVal = this._entity.SaveWarehouseWarrant(dtHead, dtEntryItems, out returnInfo);
            MessageService.ShowMessage(returnInfo);
            if (returnVal == 0)
            {
                IntialScreen();
            }
        }


        /// <summary>
        /// 输入托号后回车获取工单数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tePALNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnAddPal_Click(sender, e);
            }
        }

        /// <summary>
        /// 窗口重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            IntialScreen();
        }

        /// <summary>
        /// 查询入库单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbQuery_Click(object sender, EventArgs e)
        {
            IntialScreen();
            DataSet dsHead = this._entity.QueryWarehouseWarrantHead(teWWNoModify.Text.Trim(), "0", false);
            if (!String.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageBox.Show(this._entity.ErrorMsg, "系统提示");
                return;
            }

            retIdx = -1;
            if (dsHead.Tables[0].Rows.Count <= 0)
            {
                MessageService.ShowMessage("入库单不存在或已删除！", "系统提示！");
                return;
            }
            else if (dsHead.Tables[0].Rows.Count > 1)
            {
                SelectZMBLNRFrm selectZMBLNRFrm = new SelectZMBLNRFrm(dsHead.Tables[0], 1);
                selectZMBLNRFrm.PWarehouseWarrant = this;
                selectZMBLNRFrm.ShowDialog();
            }
            else
                retIdx = 0;

            if (retIdx >= 0)
            {
                DataRow dr = dsHead.Tables[0].Rows[retIdx];
                teWWNoModify.Text = dr["ZMBLNR"].ToString();
                DataSet dsItem = this._entity.QueryWarehouseWarrantItems(teWWNoModify.Text);
                if (!String.IsNullOrEmpty(this._entity.ErrorMsg))
                {
                    MessageBox.Show(this._entity.ErrorMsg, "系统提示");
                    return;
                }

                cbeWerks.Properties.GetKeyValueByDisplayText(cbeWerks.Text);
                teWWNoAdd.Text = dr["ZMBLNR"].ToString();   //入库单号
                cbeWerks.EditValue = dr["WERKS"].ToString();     //工厂
                cbeOrderType.Text = dr["ZMMTYP"].ToString();    //订单类型
                teOrderNo.Text = dr["AUFNR"].ToString();    //工单号码
                teDept.Text = dr["DEPT"].ToString();        //部门
                cbeOEMNo.Text = dr["VBELN_OEM"].ToString(); //OEM发货单

                _dtEntryItems = dsItem.Tables[0].Clone();
                _dtEntryItems.Clear();
                dtEntryItems.Clear();
                dtEntryItems.Merge(dsItem.Tables[0]);
                gridView1.RefreshData();
                cbeOrderType.Properties.ReadOnly = true;
                btnDel.Visible = true;
            }
        }

        /// <summary>
        /// 移除选中的托盘数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemovePal_Click(object sender, EventArgs e)
        {
            int rowHandle = this.gridView1.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MessageService.ShowMessage("请选择要移除的托盘信息。", "提示");
                return;
            }
            int rowIndex = this.gridView1.GetDataSourceRowIndex(rowHandle);
            if (!String.IsNullOrEmpty(teWWNoAdd.Text))
            {
                DataRow dr = dtEntryItems.Rows[rowIndex];
                _dtEntryItems.ImportRow(dr);
            }
            dtEntryItems.Rows.RemoveAt(rowIndex);
            
            reCalRowNum();
            if (dtEntryItems.Rows.Count <= 0)
                teOrderNo.Text = String.Empty;
        }

        /// <summary>
        /// 新增托盘号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPal_Click(object sender, EventArgs e)
        {
            tePALNO.Text = tePALNO.Text.Trim();

            if (String.IsNullOrEmpty(tePALNO.Text))
            {
                MessageService.ShowMessage("托盘号为空！", "系统提示");
                return;
            }
            if (String.IsNullOrEmpty(cbeOrderType.Text))
            {
                MessageService.ShowMessage("订单类型不能为空！", "系统提示");
                return;
            }
            if (String.IsNullOrEmpty(cbeWerks.Text))
            {
                MessageService.ShowMessage("工厂不能为空！", "系统提示");
                return;
            }

            for (int i = 0; i < dtEntryItems.Rows.Count; i++)
            {
                DataRow dr = dtEntryItems.Rows[i];
                if (dr["XP004"].ToString().Equals(tePALNO.Text))
                {
                    MessageService.ShowMessage("托盘号对应明细已添加！", "系统提示");
                    return;
                }
            }
            if (!String.IsNullOrEmpty(teWWNoAdd.Text))
            {
                for (int i = 0; i < _dtEntryItems.Rows.Count; i++)
                {
                    DataRow dr = _dtEntryItems.Rows[i];
                    if (dr["XP004"].ToString().Equals(tePALNO.Text))
                    {
                        if (String.IsNullOrEmpty(teOrderNo.Text) || teOrderNo.Text.Equals(dr["AUFNR"].ToString()))
                        {
                            teOrderNo.Text = dr["AUFNR"].ToString();
                        }
                        else
                        {
                            MessageService.ShowMessage("该托盘号对应工单号与其他托盘号对应的工单号不符！", "系统提示");
                            return;
                        }
                        dtEntryItems.ImportRow(dr);
                        _dtEntryItems.Rows.RemoveAt(i);
                        reCalRowNum();
                        tePALNO.Text = String.Empty;
                        return;
                    }
                }
            }
            
            DataSet ds = this._entity.GetWorkItems(tePALNO.Text);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            { 
                DataRow dr = ds.Tables[0].Rows[i];
                string AUFNR = dr["AUFNR"].ToString();
                //转换XP001
                string XP001 = Convert.ToString(dr["XP001"]);
                string tXP001 = GetProductGradeDisplayText(XP001);
                string[] columns = new string[] { "MES_DESC", "SAP_DESC" };
                KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_XP001");
                DataTable dtXP001 = BaseData.Get(columns, category);
                for (int j = 0; j < dtXP001.Rows.Count; j++)
                {
                    DataRow drXP001 = dtXP001.Rows[j];
                    if (drXP001["MES_DESC"].ToString().Equals(tXP001))
                    {
                        dr["XP001"] = drXP001["SAP_DESC"].ToString();
                        break;
                    }
                }

                if (String.IsNullOrEmpty(teOrderNo.Text) || teOrderNo.Text.Equals(AUFNR))
                {
                    teOrderNo.Text = AUFNR;
                }
                else
                {
                    MessageService.ShowMessage("该托盘号对应工单号与其他托盘号对应的工单号不符！", "系统提示");
                    return;
                }
            }

            dtEntryItems.Merge(ds.Tables[0]);
            reCalRowNum();
            tePALNO.Text = String.Empty;
        }

        /// <summary>
        /// 重新分配行号
        /// </summary>
        private void reCalRowNum()
        {
            for (int i = 0; i < dtEntryItems.Rows.Count; i++)
            {
                DataRow dr = dtEntryItems.Rows[i];
                dr["ZEILE"] = i + 1;
            }
            gridView1.RefreshData();
        }

        /// <summary>
        /// 入库单加删除标记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除入库单？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool isExists = this._entity.AddLvorm(teWWNoAdd.Text);
                if (!isExists)
                    MessageService.ShowMessage("删除失败！", "系统提示");
                else
                {
                    MessageService.ShowMessage("删除成功！", "系统提示");
                    IntialScreen();
                }
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
        /// 点击库位，弹出库位选择框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Column.FieldName.Equals("LGORT"))
            {
                LGORTVal = String.Empty;
                ShowLGORT FrmShowLGORT = new ShowLGORT(cbeWerks.Properties.GetKeyValueByDisplayText(cbeWerks.Text).ToString());
                FrmShowLGORT.pwarehouseWarrantCtrl = this;
                FrmShowLGORT.ShowDialog();
                if (!String.IsNullOrEmpty(LGORTVal))
                {
                    DataTable dt = (DataTable)gcItems.DataSource;
                    DataRow dr = dt.Rows[e.RowHandle];
                    dr["LGORT"] = LGORTVal;
                    gridView1.RefreshData();
                }
            }
        }

        /// <summary>
        /// 界面窗口初始化
        /// </summary>
        private void IntialScreen()
        {
            teWWNoAdd.Text = String.Empty;
            cbeOrderType.Text = "J1";
            cbeWerks.EditValue = String.Empty;
            teOrderNo.Text = String.Empty;
            teDept.Text = "晶硅组件生产";
            cbeOEMNo.Text = String.Empty;
            tePALNO.Text = String.Empty;
            while (gridView1.Columns.Count > COL_NUM)
            {
                gridView1.Columns.RemoveAt(COL_NUM);
            }
            cbeOrderType.Properties.ReadOnly = false;
            dtEntryItems.Clear();
            _dtEntryItems.Clear();
            gridView1.RefreshData();
            btnDel.Visible = false;
            cbeOrderType_SelectedIndexChanged(new object(), new EventArgs());

            GridViewHelper.SetGridView(gridView1);
        }
        //行数据筛选
        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void sbQuery_Click_1(object sender, EventArgs e)
        {
            string ShipmentNum = string.Empty;
            ShipmentNum = teWWNoAdd.Text.Trim();
            if (!string.IsNullOrEmpty(ShipmentNum))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("查询条件出货单号不能为空！", "系统提示");
            }
        }
    }
}
