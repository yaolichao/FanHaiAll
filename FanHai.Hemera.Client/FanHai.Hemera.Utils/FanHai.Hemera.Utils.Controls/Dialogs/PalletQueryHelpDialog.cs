//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// ============================================================================
// 修改人               修改时间              说明
// ----------------------------------------------------------------------------
//  Peter               2013-01-08            添加
// ============================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Utils.Dialogs
{
    /// <summary>
    /// 查询托盘的对话框。
    /// </summary>
    public partial class PalletQueryHelpDialog : Form
    {
        /// <summary>
        /// 托盘查询操作所需参数。
        /// </summary>
        private PalletQueryHelpModel _model;
        /// <summary>
        /// 托盘查询实体类。
        /// </summary>
        private PalletQueryEntity _entity = new PalletQueryEntity();
        /// <summary>
        /// 值被选中事件。
        /// </summary>
        public event PalletQueryValueSelectedEventHandler OnValueSelected;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model">批次查询操作所需参数。</param>
        public PalletQueryHelpDialog(PalletQueryHelpModel model)
        {
            InitializeComponent();
            this._model = model;
        }
        /// <summary>
        /// 触发非激活事件，隐藏并关闭对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PalletQueryHelpDialog_Deactivate(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Close();
        }
        /// <summary>
        /// 窗体载入事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PalletQueryHelpDialog_Load(object sender, EventArgs e)
        {
            BindGrade();
            //BindQueryResult();
        }
        private void BindGrade()
        {
            //绑定产品等级
            string[] l_s = new string[] { "Column_Name", "Column_Index", "Column_type", "Column_code" };
            string category = "Basic_TestRule_PowerSet";
            DataTable dtProLevel = BaseData.Get(l_s, category);
            DataTable dtLevel = dtProLevel.Clone();
            dtLevel.TableName = "Level";
            DataRow[] drs = dtProLevel.Select(string.Format("Column_type='{0}'", BASE_POWERSET.PRODUCT_GRADE));
            foreach (DataRow dr in drs)
                dtLevel.ImportRow(dr);
            DataView dview = dtLevel.DefaultView;
            dview.Sort = "Column_Index asc";
            respostior_Grade.DisplayMember = "Column_Name";
            respostior_Grade.ValueMember = "Column_code";
            respostior_Grade.DataSource = dview.Table;
        }
        /// <summary>
        /// 绑定查询结果。
        /// </summary>
        private void BindQueryResult()
        {
            string palletNo=this.tePalletNo.Text.Trim();
            //查询批次
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            //组织查询参数。
            DataSet dsParams = new DataSet();
            Hashtable htParams = new Hashtable();
            htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO,palletNo);
            if (!string.IsNullOrEmpty(this._model.RoomKey))
            {
                htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY, this._model.RoomKey);
            }
            htParams.Add(WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP, (int)this._model.PalletState);
            DataTable dtParams=CommonUtils.ParseToDataTable(htParams);
            dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dsParams.Tables.Add(dtParams);
            //进行查询
            DataSet dsReturn = _entity.Query(dsParams, ref config);
            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowMessage(_entity.ErrorMsg);
                return;
            }
            else
            {
                gcResult.DataSource = dsReturn.Tables[0];
                gcResult.MainView = gvResult;
            }
        }
        /// <summary>
        /// 根据批次查询。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            BindQueryResult();
        }
        /// <summary>
        /// 双击选择批次信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvResult_DoubleClick(object sender, EventArgs e)
        {
            int rowIndex = gvResult.FocusedRowHandle;
            if (rowIndex >= 0)
            {
                if (this.OnValueSelected!=null)
                {
                    DataRow dr = gvResult.GetFocusedDataRow();
                    PalletQueryValueSelectedEventArgs args=new PalletQueryValueSelectedEventArgs();
                    args.PalletNo=Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
                    args.PalletKey = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY]);
                    this.OnValueSelected(sender, args);
                    if (args.Cancel == true)
                    {
                        return;
                    }
                }
                this.Visible = false;
                this.Close();
            }
        }
        /// <summary>
        /// 自定义绘制单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvResult_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gclRowNum)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
            else if (e.Column == this.gclCsDataGroup)
            {
                PalletState state = (PalletState)Convert.ToInt32(e.CellValue);
                e.DisplayText = CommonUtils.GetEnumValueDescription(state);
            }
        }
        /// <summary>
        /// 分页查询。
        /// </summary>
        private void pgnQueryResult_DataPaging()
        {
            BindQueryResult();
        }
    }
    /// <summary>
    /// 查询托盘的参数数据
    /// </summary>
    public class PalletQueryHelpModel
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public PalletQueryHelpModel()
        {
        }
        /// <summary>
        /// 车间主键。
        /// </summary>
        public string RoomKey { get; set; }
        /// <summary>
        /// 托盘状态（1：包装；2：入库检；3:已入库；4：已出货）。
        /// </summary>
        public PalletState PalletState { get; set; }
    }
    /// <summary>
    /// 托盘查询被选中的参数类。
    /// </summary>
    public class PalletQueryValueSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// 托盘号。
        /// </summary>
        public string PalletNo { get; set; }
        /// <summary>
        /// 托盘主键。
        /// </summary>
        public string PalletKey { get; set; }
        /// <summary>
        /// 是否取消选中值。
        /// </summary>
        public bool Cancel { get; set; }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public PalletQueryValueSelectedEventArgs()
        {
            this.Cancel = false;
        }
    }
    /// <summary>
    /// 托盘查询被选中的事件委托类。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void PalletQueryValueSelectedEventHandler(object sender, PalletQueryValueSelectedEventArgs e);
}
