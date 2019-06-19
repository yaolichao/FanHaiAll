//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 创建人               修改时间              说明
// ---------------------------------------------------------------------------------
// chao.pang            2013-07-15            新增
// =================================================================================
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
using DevExpress.XtraGrid.Views.Grid;
namespace FanHai.Hemera.Addins.WMS
{
    /// <summary>
    /// 表示托盘出货查询作业的窗体类。
    /// </summary>
    public partial class ShipManagementCtrl : BaseUserCtrl
    {
        DataTable _dtProductGrade = null;                               //暂存产品等级。
        /// <summary>
        /// 出货操作对象。
        /// </summary>
        ShipmentOperationEntity _entity = new ShipmentOperationEntity();
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShipManagementCtrl()
        {
            InitializeComponent();
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
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
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
                MessageBox.Show("托盘号长度必须小于等于2048个字符。");
                this.tePalletNo.Select();
                return;
            }
            DataSet dsParams = this.GetQueryCondition();
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            DataSet dsReturn = this._entity.Query(dsParams, ref config);
            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }
            if (dsReturn.Tables.Count > 0)
            {
                DataTable dt = dsReturn.Tables[0];
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    string strtest = dt.Rows[i]["SHIPMENT_TYPE"].ToString().Equals("0").ToString();
                    if (dt.Rows[i]["CUSTCHECK"].ToString() == "0")
                    {
                        dt.Rows[i]["CUSTCHECK"] = "未客检";
                    }
                    else if (dt.Rows[i]["CUSTCHECK"].ToString() == "1")
                    {
                        dt.Rows[i]["CUSTCHECK"] = "已客检";
                    }

                    if (dt.Rows[i]["CONTAINER_NO"].ToString().Equals(""))
                    {
                        dt.Rows[i]["HFLAG"] = "1";
                    }
                    //if (dt.Rows[i]["SHIPMENT_NO"].ToString().Equals(""))
                    //{
                    //    dt.Rows[i].Delete();
                    //}
                    //if (dt.Rows[i]["SHIPMENT_TYPE"].ToString().Equals("0"))
                    //{
                    //    dt.Rows[i]["SHIPMENT_TYPE"] = "陆运";
                    //}
                    //else if(dt.Rows[i]["SHIPMENT_TYPE"].ToString().Equals("1"))
                    //{
                    //    dt.Rows[i]["SHIPMENT_TYPE"] = "海运";
                    //}
                    //else if(dt.Rows[i]["SHIPMENT_TYPE"].ToString().Equals("2"))
                    //{
                    //    dt.Rows[i]["SHIPMENT_TYPE"] = "空运";
                    //}
                    switch (dt.Rows[i]["SHIPMENT_TYPE"].ToString())
                    {
                        case "0":
                            dt.Rows[i]["SHIPMENT_TYPE"] = "陆运";
                            break;
                        case "1":
                            dt.Rows[i]["SHIPMENT_TYPE"] = "海运";
                            break;
                        case "2":
                            dt.Rows[i]["SHIPMENT_TYPE"] = "空运";
                            break;
                        default:
                            dt.Rows[i]["SHIPMENT_TYPE"] = "";
                            break;
                    }      
                }
                gcList.DataSource = dt;
                gcList.MainView = gvList;
                gvList.BestFitColumns();

            }
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

        private void tspSave_Click(object sender, EventArgs e)
        {
            if (this.gvList.State == GridState.Editing && this.gvList.IsEditorFocused && this.gvList.EditingValueModified)
            {
                this.gvList.SetFocusedRowCellValue(this.gvList.FocusedColumn, this.gvList.EditingValue);
            }
            this.gvList.UpdateCurrentRow();

            if (MessageBox.Show(StringParser.Parse("确定要保存吗？"),
            StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {

                //UseMaterial _material = new UseMaterial();

                Hashtable hashTable = new Hashtable();
                hashTable.Add("EDITOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                hashTable.Add("EDIT_TIMEZONE", PropertyService.Get(PROPERTY_FIELDS.TIMEZONE));
                DataTable tableParam = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                tableParam.TableName = "HASH";

                DataTable dtUpdate = new DataTable();
                DataView dv = gvList.DataSource as DataView;
                DataTable dtv = dv.Table;
                if (dv.Table.Rows.Count > 0)
                {
                    for (int i = 0; i < dtv.Rows.Count; i++)
                    {
                        if (dtv.Rows[i]["HFLAG"].ToString().Equals("0"))
                        {
                            dtv.Rows[i].Delete();
                        }
                    }
                 }
                if (dv != null) dtUpdate = dtv;

                DataSet dsSetIn = new DataSet();
                dtUpdate.TableName = "WMS_SHIPMENT";
                dsSetIn.Merge(dtUpdate);
                dsSetIn.Merge(tableParam);

                if (!_entity.UpdateConteinerNo(dsSetIn))
                {
                    MessageService.ShowMessage(_entity.ErrorMsg);
                }
                else
                {
                    MessageService.ShowMessage("保存成功");
                }
            }

            this.gvList.BestFitColumns();
        }

        //行选中后修改gclContainerNo货柜号列中值为空的行
        private void gvList_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView View = sender as GridView;
            string cellValue = View.GetRowCellValue(View.FocusedRowHandle, gclFlag).ToString();
            if (cellValue.Equals("1"))
                e.Cancel = false;
            else
                e.Cancel = true;

        }

        private void gvList_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            
        }
    }
}
