using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using System.Collections;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls.Common;
using FanHai.Hemera.Utils.Controls;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicPowerSet : BaseUserCtrl
    {
        BasePowerSetEntity _powerSetEntity = new BasePowerSetEntity();
        DataTable dtPowerSet = new DataTable(), dtPowerSetDtl = new DataTable(), dtPowerSetColorDtl = new DataTable();
        string _powerSet_Key = string.Empty;
        /// <summary>
        /// 定位到修改或新增的行数据
        /// </summary>
        string _loadKey = string.Empty;
        public BasicPowerSet()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            InitDataBind();
        }

        private void BasicPowerSet_Load(object sender, EventArgs e)
        {
            InitDataBind();

            GridViewHelper.SetGridView(gvPowerSet);
            GridViewHelper.SetGridView(gvPowerSetDtl);
            GridViewHelper.SetGridView(gvPowerSetColorDtl);

            lblMenu.Text = "基础数据 > 工艺参数设置 > 档位管理";
        }

        private void InitDataBind()
        {
            //分档代码，分档规则，功率上下线，是否存在子分档            
            Hashtable hashTable = new Hashtable();
            if (!string.IsNullOrEmpty(txtPs_code.Text.Trim()))
                hashTable[BASE_POWERSET.FIELDS_PS_CODE] = txtPs_code.Text.Trim();
            if (!string.IsNullOrEmpty(txtPs_rule.Text.Trim()))
                hashTable[BASE_POWERSET.FIELDS_PS_RULE] = txtPs_rule.Text.Trim();
            if (!string.IsNullOrEmpty(txtPowerStart.Text.Trim()))
                hashTable[BASE_POWERSET.FIELDS_P_MIN] = txtPowerStart.Text.Trim();
            if (!string.IsNullOrEmpty(txtPowerEnd.Text.Trim()))
                hashTable[BASE_POWERSET.FIELDS_P_MAX] = txtPowerEnd.Text.Trim();
            if (chkLevel.Checked)
                hashTable[BASE_POWERSET.FIELDS_SUB_PS_WAY] = "NULL";


            DataSet dsDataBind = _powerSetEntity.GetPowerSetData(hashTable);
            if (_powerSetEntity.ErrorMsg.Equals(string.Empty))
            {
                dtPowerSetDtl = dsDataBind.Tables[BASE_POWERSET_DETAIL.DATABASE_TABLE_NAME];
                gcPowerSetDtl.DataSource = null;
                dtPowerSetColorDtl = dsDataBind.Tables[BASE_POWERSET_COLORATCNO.DATABASE_TABLE_NAME];
                gcPowerSetColorDtl.DataSource = null;

                dtPowerSet = dsDataBind.Tables[BASE_POWERSET.DATABASE_TABLE_NAME];
                this.gcPowerSet.MainView = gvPowerSet;
                this.gcPowerSet.DataSource = dtPowerSet;
                this.gvPowerSet.BestFitColumns();

                if (!string.IsNullOrEmpty(_loadKey))
                {
                    for (int i = 0; i < gvPowerSet.RowCount; i++)
                    {
                        string sk = Convert.ToString(((DataRowView)(this.gvPowerSet.GetRow(i))).Row[BASE_POWERSET.FIELDS_POWERSET_KEY]);
                        if (_loadKey.Equals(sk.Trim()))
                        {
                            this.gvPowerSet.FocusedRowHandle = i;
                            break;
                        }
                    }
                }

                BindGvPowerSetDtl();
                BindGvPowerSetColorDtl();
            }
            else
                MessageService.ShowMessage(_powerSetEntity.ErrorMsg);

        }

        private void gvPowerSet_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            BindGvPowerSetDtl();
            BindGvPowerSetColorDtl();
        }
        private void BindGvPowerSetDtl()
        {
            try
            {
                if (gvPowerSet.FocusedRowHandle > -1)
                {
                    _powerSet_Key = this.gvPowerSet.GetRowCellValue(gvPowerSet.FocusedRowHandle, BASE_POWERSET.FIELDS_POWERSET_KEY).ToString().Trim();
                    DataTable dtCommonSetDtl = dtPowerSetDtl.Clone();
                    DataRow[] drPowerAttrs = dtPowerSetDtl.Select(string.Format(BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY + "='{0}'", _powerSet_Key));

                    foreach (DataRow dr in drPowerAttrs)
                        dtCommonSetDtl.ImportRow(dr);

                    this.gcPowerSetDtl.DataSource = null;
                    this.gcPowerSetDtl.DataSource = dtCommonSetDtl;
                }
            }
            catch //(Exception ex) 
            { }
        }
        private void BindGvPowerSetColorDtl()
        {
            try
            {
                if (gvPowerSet.FocusedRowHandle > -1)
                {
                    _powerSet_Key = this.gvPowerSet.GetRowCellValue(gvPowerSet.FocusedRowHandle, BASE_POWERSET.FIELDS_POWERSET_KEY).ToString().Trim();
                    DataTable dtCommonSetColorDtl = dtPowerSetColorDtl.Clone();
                    DataRow[] drPowerColorAttrs = dtPowerSetColorDtl.Select(string.Format(BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY + "='{0}'", _powerSet_Key));

                    foreach (DataRow dr in drPowerColorAttrs)
                        dtCommonSetColorDtl.ImportRow(dr);

                    this.gcPowerSetColorDtl.DataSource = null;
                    this.gcPowerSetColorDtl.DataSource = dtCommonSetColorDtl;
                }
            }
            catch //(Exception ex) 
            { }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dtEditPowerSet = ((DataView)gvPowerSet.DataSource).Table;
            DataRow drNew = dtEditPowerSet.NewRow();

            this._loadKey = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
            drNew[BASE_POWERSET.FIELDS_POWERSET_KEY] = this._loadKey;
            drNew[BASE_POWERSET.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            PowerSetForm psf = new PowerSetForm();
            psf.isEdit = false;
            psf.drCommon = drNew;
            if (DialogResult.OK == psf.ShowDialog())
            {
                InitDataBind();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (gvPowerSet.FocusedRowHandle < 0 || gvPowerSet.RowCount < 1)
            {
                MessageService.ShowMessage("请选择编辑的数据!", "提示");
                return;
            }

            DataTable dtEditPowerSet = ((DataView)gvPowerSet.DataSource).Table;
            string pk = gvPowerSet.GetRowCellValue(gvPowerSet.FocusedRowHandle, BASE_POWERSET.FIELDS_POWERSET_KEY).ToString();
            this._loadKey = pk;
            DataRow[] drEditPowerSets = dtEditPowerSet.Select(string.Format(BASE_POWERSET.FIELDS_POWERSET_KEY + "='{0}'", pk));

            if (drEditPowerSets.Length > 0)
            {
                PowerSetForm psf = new PowerSetForm();
                psf.isEdit = true;
                psf.drCommon = drEditPowerSets[0];
                if (DialogResult.OK == psf.ShowDialog())
                {
                    InitDataBind();
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (gvPowerSet.FocusedRowHandle < 0 || gvPowerSet.RowCount < 1)
            {
                MessageService.ShowMessage("请选择删除的数据!", "提示");
                return;
            }

            if (MessageService.AskQuestion("确认删除数据么?", "提示"))
            {
                //删除处理
                DataSet dsPowerSet = new DataSet();
                DataTable dtDelPowerSet = ((DataView)gvPowerSet.DataSource).Table;
                string pk = gvPowerSet.GetRowCellValue(gvPowerSet.FocusedRowHandle, BASE_POWERSET.FIELDS_POWERSET_KEY).ToString();
                DataRow[] drDtlPowerSets = dtDelPowerSet.Select(string.Format(BASE_POWERSET.FIELDS_POWERSET_KEY + "='{0}'", pk));
                DataTable dtDel = dtDelPowerSet.Clone();
                DataRow drDel = drDtlPowerSets[0];
                drDel[BASE_POWERSET.FIELDS_ISFLAG] = 0;
                dtDel.ImportRow(drDel);
                dtDel.TableName = BASE_POWERSET.DATABASE_TABLE_FORUPDATE;
                dsPowerSet.Merge(dtDel, true, MissingSchemaAction.Add);
                bool bl_bak = _powerSetEntity.SavePowerSetData(dsPowerSet);
                if (!bl_bak)
                {
                    MessageService.ShowMessage("删除失败!");
                }
                else
                {
                    MessageService.ShowMessage("删除成功!");
                    InitDataBind();
                }
            }
        }

        private void btnAddDtl_Click(object sender, EventArgs e)
        {
            if (gvPowerSetDtl.DataSource == null) return;
            DataTable dtEditPowerSetDtl = ((DataView)gvPowerSetDtl.DataSource).Table;
            DataRow drNew = dtEditPowerSetDtl.NewRow();

            drNew[BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY_DTL] = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
            drNew[BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY] = _powerSet_Key;
            PowerSetDtlForm psdf = new PowerSetDtlForm();
            psdf.isEdit = false;
            psdf.drCommon = drNew;
            if (DialogResult.OK == psdf.ShowDialog())
            {
                InitDataBind();
            }
        }

        private void btnModifyDtl_Click(object sender, EventArgs e)
        {
            if (gvPowerSetDtl.FocusedRowHandle < 0 || gvPowerSetDtl.RowCount < 1)
            {
                MessageService.ShowMessage("请选择编辑的子分档数据!", "提示");
                return;
            }

            DataTable dtEditPowerSetDtl = ((DataView)gvPowerSetDtl.DataSource).Table;
            string pk = gvPowerSetDtl.GetRowCellValue(gvPowerSetDtl.FocusedRowHandle, BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY_DTL).ToString();
            DataRow[] drEditPowerSetDtls = dtEditPowerSetDtl.Select(string.Format(BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY_DTL + "='{0}'", pk));

            if (drEditPowerSetDtls.Length > 0)
            {
                PowerSetDtlForm psdf = new PowerSetDtlForm();
                psdf.isEdit = true;
                psdf.drCommon = drEditPowerSetDtls[0];
                if (DialogResult.OK == psdf.ShowDialog())
                {
                    InitDataBind();
                }
            }
        }

        private void btnDtlDel_Click(object sender, EventArgs e)
        {
            if (gvPowerSetDtl.FocusedRowHandle < 0 || gvPowerSetDtl.RowCount < 1)
            {
                MessageService.ShowMessage("请选择删除的子分档数据!", "提示");
                return;
            }

            if (MessageService.AskQuestion("确认删除数据么?", "提示"))
            {
                //删除处理
                DataSet dsPowerSetDtl = new DataSet();
                DataTable dtDelPowerSet = ((DataView)gvPowerSetDtl.DataSource).Table;
                string pk = gvPowerSetDtl.GetRowCellValue(gvPowerSetDtl.FocusedRowHandle, BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY_DTL).ToString();
                DataRow[] drDtlPowerSets = dtDelPowerSet.Select(string.Format(BASE_POWERSET_DETAIL.FIELDS_POWERSET_KEY_DTL + "='{0}'", pk));
                DataTable dtDel = dtDelPowerSet.Clone();
                DataRow drDel = drDtlPowerSets[0];
                drDel[BASE_POWERSET_DETAIL.FIELDS_ISFLAG] = 0;
                dtDel.ImportRow(drDel);
                dtDel.TableName = BASE_POWERSET_DETAIL.DATABASE_TABLE_FORUPDATE;
                dsPowerSetDtl.Merge(dtDel, true, MissingSchemaAction.Add);
                bool bl_bak = _powerSetEntity.SavePowerSetData(dsPowerSetDtl);
                if (!bl_bak)
                {
                    MessageService.ShowMessage("删除失败!");
                }
                else
                {
                    MessageService.ShowMessage("删除成功!");
                    InitDataBind();
                }
            }
        }

        private void btnAddColor_Click(object sender, EventArgs e)
        {
            if (gcPowerSetColorDtl.DataSource == null) return;
            DataTable dtEditPowerSetColorDtl = gcPowerSetColorDtl.DataSource as DataTable;
            DataRow drNew = dtEditPowerSetColorDtl.NewRow();

            drNew[BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY_ATC] = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
            drNew[BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY] = _powerSet_Key;
            PowerSetDtlColorForm psdf = new PowerSetDtlColorForm();
            psdf.isEdit = false;
            psdf.drCommon = drNew;
            if (DialogResult.OK == psdf.ShowDialog())
            {
                InitDataBind();
            }
        }

        /// <summary>
        /// 设置行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPowerSet_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 设置行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPowerSetDtl_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 设置行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPowerSetColorDtl_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

        }

        private void btnModifyColor_Click(object sender, EventArgs e)
        {
            if (gvPowerSetColorDtl.FocusedRowHandle < 0 || gvPowerSetColorDtl.RowCount < 1)
            {
                MessageService.ShowMessage("请选择编辑的【花色】数据!", "提示");
                return;
            }

            DataTable dtEditPowerSetColorDtl = gcPowerSetColorDtl.DataSource as DataTable;
            string pk = gvPowerSetColorDtl.GetRowCellValue(gvPowerSetColorDtl.FocusedRowHandle, BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY_ATC).ToString();
            DataRow[] drEditPowerSetColorDtls = dtEditPowerSetColorDtl.Select(string.Format(BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY_ATC + "='{0}'", pk));

            if (drEditPowerSetColorDtls.Length > 0)
            {
                PowerSetDtlColorForm psdf = new PowerSetDtlColorForm();
                psdf.isEdit = true;
                psdf.drCommon = drEditPowerSetColorDtls[0];
                if (DialogResult.OK == psdf.ShowDialog())
                {
                    InitDataBind();
                }
            }
        }

        private void btnColorDel_Click(object sender, EventArgs e)
        {
            if (gvPowerSetColorDtl.FocusedRowHandle < 0 || gvPowerSetColorDtl.RowCount < 1)
            {
                MessageService.ShowMessage("请选择删除的花色数据!", "提示");
                return;
            }

            if (MessageService.AskQuestion("确认删除数据么?", "提示"))
            {
                //删除处理
                DataSet dsPowerSetColorDtl = new DataSet();
                DataTable dtDelPowerSetColor = gcPowerSetColorDtl.DataSource as DataTable;
                string pk = gvPowerSetColorDtl.GetRowCellValue(gvPowerSetColorDtl.FocusedRowHandle, BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY_ATC).ToString();
                DataRow[] drDtlPowerSets = dtDelPowerSetColor.Select(string.Format(BASE_POWERSET_COLORATCNO.FIELDS_POWERSET_KEY_ATC + "='{0}'", pk));
                DataTable dtDel = dtDelPowerSetColor.Clone();
                DataRow drDel = drDtlPowerSets[0];
                drDel[BASE_POWERSET_COLORATCNO.FIELDS_ISFLAG] = 0;
                dtDel.ImportRow(drDel);
                dtDel.TableName = BASE_POWERSET_COLORATCNO.DATABASE_TABLE_FORUPDATE;
                dsPowerSetColorDtl.Merge(dtDel, true, MissingSchemaAction.Add);
                bool bl_bak = _powerSetEntity.SavePowerSetData(dsPowerSetColorDtl);
                if (!bl_bak)
                {
                    MessageService.ShowMessage("删除失败!");
                }
                else
                {
                    MessageService.ShowMessage("删除成功!");
                    InitDataBind();
                }
            }
        }
    }
}
