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
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using System.Collections;
using FanHai.Hemera.Share.Interface.WarehouseManagement;
using FanHai.Hemera.Share.Interface;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 在线物料查询。
    /// </summary>
    public partial class OnlineMaterialQueryCtrl : BaseUserCtrl
    {
        OnlineMaterialQueryEntity _entity = new OnlineMaterialQueryEntity();
        //OnlineMaterialQueryDialog _dialog = null;

        public OnlineMaterialQueryModel Model
        {
            get;
            private set;
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        public OnlineMaterialQueryCtrl()
        {
            InitializeComponent();
            this.Model = new OnlineMaterialQueryModel();
            InitializeLanguage();
            GridViewHelper.SetGridView(gvResult);
            GridViewHelper.SetGridView(gvResultDetail);
        }
        public void InitializeLanguage()
        {
            gcolIndex.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryCtrl.GridControl.0001}");//序号
            gcolStore.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryCtrl.GridControl.0002}");//线上仓
            gcolMatCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryCtrl.GridControl.0003}");//物料编码
            gcolMatDes.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryCtrl.GridControl.0004}");//物料描述
            gcolUnit.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryCtrl.GridControl.0005}");//单位
            gcolMatQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryCtrl.GridControl.0006}");//物料数量
            gcolOperation.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryCtrl.GridControl.0007}");//工序名称
            gcolFacRoom.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryCtrl.GridControl.0008}");//工厂车间
        }

        /// <summary>
        /// 绑定车间。
        /// </summary>
        private void BindRoom()
        {
            //根据PropertyService获取PROPERTY_FIELDS.STORES的值。从WST_STORES,FMM_LOCATION,FMM_LOCATION_RET
            //根据线边仓名称获取用户拥有权限的工厂车间名称集合绑定到窗体控件中，设置空为控件默认值。											
            DataTable dtFacRoom = FactoryUtils.GetFactoryRoomByStores(PropertyService.Get(PROPERTY_FIELDS.STORES));
            DataRow dr = dtFacRoom.NewRow();
            dr["LOCATION_NAME"] = string.Empty;
            dtFacRoom.Rows.Add(dr);
            lueRoom.Properties.DataSource = dtFacRoom;
            lueRoom.Properties.DisplayMember = "LOCATION_NAME";
            lueRoom.Properties.ValueMember = "LOCATION_KEY";
        }
        /// <summary>
        /// 绑定工序。
        /// </summary>
        private void BindOpeation()
        {
            lueOperationName.Properties.Items.Clear();
            //根据登录用户将登录用户拥有权限的工序名称绑定到窗体控件中，设置空为控件默认值。
            //通过PropertyService获取PROPERTY_FIELDS.OPERATIONS的值"
            string strOperation = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            string[] strArrOperation = strOperation.Split(',');
            List<string> listOperation = strArrOperation.ToList<string>();
            listOperation.Insert(0, string.Empty);
            lueOperationName.Properties.Items.AddRange(listOperation);
        }
        /// <summary>
        /// 绑定线上仓。
        /// </summary>
        private void BindStore()
        {
            lueStoreName.Properties.Items.Clear();
            //根据登录用户将登录用户拥有权限的线边仓名称绑定到窗体控件中，设置空为控件默认值。
            //通过PropertyService获取PROPERTY_FIELDS.STORES的值"	
            string strStore = PropertyService.Get(PROPERTY_FIELDS.STORES);
            string[] strArrStore = strStore.Split(',');
            List<string> listStore = strArrStore.ToList<string>();
            listStore.Insert(0, string.Empty);
            lueStoreName.Properties.Items.AddRange(listStore);
        }
        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlValue()
        {
            this.lueRoom.EditValue = this.Model.RoomKey;
            this.teMaterialCode.Text = this.Model.MaterialCode;
            this.teMaterilLot.Text = this.Model.MaterialLot;
            this.lueOperationName.EditValue = this.Model.OperationName;
            this.teSupplierName.Text = this.Model.SupplierName;
            this.lueStoreName.EditValue = this.Model.StoreName;
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
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnlineMaterialQueryCtrl_Load(object sender, EventArgs e)
        {
            BindRoom();
            BindOpeation();
            BindStore();
            InitControlValue();
        }
        /// <summary>
        /// 显示查询条件的对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbQuery_Click(object sender, EventArgs e)
        {
            //if (this._dialog == null)
            //{
            //    this._dialog = new OnlineMaterialQueryDialog();
            //}

            //if (this._dialog!=null && this._dialog.ShowDialog() == DialogResult.OK)
            //{
            //}
            this.Model.MaterialCode = this.teMaterialCode.Text;
            this.Model.MaterialLot = this.teMaterilLot.Text;
            this.Model.OperationName = this.lueOperationName.Text;
            this.Model.RoomKey = Convert.ToString(this.lueRoom.EditValue);
            this.Model.RoomName = this.lueRoom.Text;
            this.Model.StoreName = this.lueStoreName.Text;
            this.Model.SupplierName = this.teSupplierName.Text;
            BindData();
        }
        /// <summary>
        /// 绑定查询数据。
        /// </summary>
        public void BindData()
        {
            OnlineMaterialQueryModel model = new OnlineMaterialQueryModel(this.Model);
            if (string.IsNullOrEmpty(model.StoreName))
            {
                model.StoreName=PropertyService.Get(PROPERTY_FIELDS.STORES);
            }
            if (string.IsNullOrEmpty(model.OperationName))
            {
                model.OperationName = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            }
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            DataSet dsReturn = this._entity.Query(model, ref config);
            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }
            if (dsReturn.Tables.Count > 0)
            {
                gcResult.DataSource = dsReturn.Tables[0];
                gcResult.MainView = gvResult;
                gvResult.BestFitColumns();
            }
        }
        /// <summary>
        /// 自定义显示查询结果中的单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvResult_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "INDEX1": //设置行号
                case "INDEX2": //设置行号
                    e.DisplayText = Convert.ToString(e.RowHandle + 1);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 分页导航。
        /// </summary>
        private void pgnQueryResult_DataPaging()
        {
            BindData();
        }
        /// <summary>
        /// 显示数据前面的+号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvResult_MasterRowGetRelationCount(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 2;
        }

        private void gvResult_MasterRowGetChildList(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventArgs e)
        {
            OnlineMaterialQueryModel model = new OnlineMaterialQueryModel(this.Model);
            if (string.IsNullOrEmpty(model.StoreName))
            {
                model.StoreName = PropertyService.Get(PROPERTY_FIELDS.STORES);
            }
            if (string.IsNullOrEmpty(model.OperationName))
            {
                model.OperationName = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            }
            DataRow dr=this.gvResult.GetDataRow(e.RowHandle);
            string storeMaterialKey = Convert.ToString(dr["STORE_MATERIAL_KEY"]);
            DataSet dsReturn = this._entity.QueryDetail(model, storeMaterialKey);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }
             
            e.ChildList = dsReturn.Tables[0].DefaultView;
        }

        private void gvResult_MasterRowEmpty(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowEmptyEventArgs e)
        {
            e.IsEmpty = false;
        }

        private void gvResult_MasterRowGetRelationName(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
           e.RelationName = "MasterDetail";
        }

        private void gvResult_MasterRowGetRelationDisplayCaption(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            //e.RelationName = "明细数据";
            e.RelationName = StringParser.Parse("${res:FanHai.Hemera.Addins.OnlineMaterialQueryCtrl.lbl.0004}");
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
