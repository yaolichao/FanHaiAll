using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SolarViewer.Hemera.Utils.Controls;
using SolarViewer.Gui.Framework.Gui;
using System.Collections;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Grid;
using SolarViewer.Gui.Core;
using SolarViewer.Hemera.Utils.Entities;
using SolarViewer.Hemera.Share.Interface;
using SolarViewer.Hemera.Utils.Common;
using DevExpress.XtraEditors.Controls;

namespace Astronergy.Addins.WMS.Gui
{
    public partial class OutBoundManagementControl : BaseUserCtrl
    {
        IViewContent iview = null;
        OutboundOperationEntity _entity = new OutboundOperationEntity();
       
        public OutBoundManagementControl(IViewContent view)
        {
            InitializeComponent();
            this.iview = view;
            //取登录用户名
            string Manager = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);

        }

        public OutBoundManagementControl()
        {
            InitializeComponent();
        }

        private void OutBoundManagementControl_Load(object sender, EventArgs e)
        {
            
            ResetControlValue();
            BindShipmentType();
        }
        /// <summary>
        /// 绑定出货类型。
        /// </summary>
        private void BindShipmentType()
        {          
            string[] columns = new string[] { "CODE", "NAME" };//定义一个数组
            //结构体，定义可设置或检索的键/值对
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", BASEDATA_CATEGORY_NAME.Basic_ShipmentType);
            DataTable dt = BaseData.Get(columns, category);
            DataRow dr = dt.NewRow();
            dr["NAME"] = "";
            dr["CODE"] = "4";
            dt.Rows.InsertAt(dr,0);
            this.lueShipmentType.Properties.DataSource = dt;
            this.lueShipmentType.Properties.DisplayMember = "NAME";
            this.lueShipmentType.Properties.ValueMember = "CODE";                        
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        // 重置控件值
        private void ResetControlValue()
        {
            this.txtOUTBANDNO.Text = string.Empty;
            this.txtVBELN.Text = string.Empty;
            this.txtContainerNo.Text = string.Empty;

            this.lueShipmentType.EditValue = string.Empty;
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList != null)
            {
                dtList.Rows.Clear();
            }
            txtOUTBANDNO.Select();
        }

        private void tsbQuery_Click(object sender, EventArgs e)
        {
            //string palletNo = this.txtPalleteNo.Text.Trim();
            //if (palletNo.Length > 2048)
            //{
            //    MessageBox.Show("托盘号长度必须小于等于2048个字符。");
            //    this.txtPalleteNo.Select();
            //    return;
            //}
            string OutboundNo = this.txtOUTBANDNO.Text.Trim();
            string VbelnNo = this.txtVBELN.Text.Trim();
            string ShipmentNO = this.txtContainerNo.Text.Trim();
            string SType = Convert.ToString(this.lueShipmentType.EditValue);
        

            //根据外向交货单号或出库单号取抬头数据
            DataSet dsOutboudInfo = this._entity.getOutboudInfo(OutboundNo, VbelnNo, ShipmentNO, SType);//出库管理
            if (dsOutboudInfo.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("没有您指定查询条件下的出货单抬头信息！");
                gcView.DataSource = null;
                gcList.DataSource = null;
                return;
            }
            else if (dsOutboudInfo.Tables[0].Rows.Count >= 1)
            {
                //txtVBELN.Text = dsOutboudInfo.Tables[0].Rows[0]["VBELN"].ToString();
                //txtOUTBANDNO.Text = dsOutboudInfo.Tables[0].Rows[0]["OUTBANDNO"].ToString();
               // txtContainerNo.Text = dsOutboudInfo.Tables[0].Rows[0]["ShipmentNo"].ToString();
               // this.txtContainerNo_EditValueChanged(sender, e);

                //this.lueShipmentType.Properties.DisplayMember = "NAME";
                //this.lueShipmentType.Properties.ValueMember = "CODE";
                //lueShipmentType.EditValue= dsOutboudInfo.Tables[0].Rows[0]["ShipmentType"].ToString();
               // this.gvList.OptionsBehavior.ReadOnly = true;
                
                gcList.DataSource = dsOutboudInfo.Tables[0];
                gcList.MainView = gvList;
                gvList.BestFitColumns();
                this.gvList.OptionsView.ColumnAutoWidth = false;
            }
            
            //PagingQueryConfig config = new PagingQueryConfig()
            //{
            //    PageNo = pgnQueryResult.PageNo,
            //    PageSize = pgnQueryResult.PageSize
            //};

            DataSet dsReturn = this._entity.getOutboudItem(OutboundNo, VbelnNo, ShipmentNO, SType);

            //pgnQueryResult.Pages = config.Pages;
            //pgnQueryResult.Records = config.Records;
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }
            if (dsReturn.Tables.Count > 0)
            {
                gcView.DataSource = dsReturn.Tables[0];
                gcView.MainView = gvView;
                gvView.BestFitColumns();
                this.gvView.OptionsView.ColumnAutoWidth = false;
            }
            this.gvList.OptionsView.ColumnAutoWidth = true;
            this.gvView.OptionsView.ColumnAutoWidth = true;
        }


        private void tsbSave_Click(object sender, EventArgs e)
        {

            //if (this.gvList.State == GridState.Editing && this.gvList.IsEditorFocused && this.gvList.EditingValueModified)
            //{
            //    this.gvList.SetFocusedRowCellValue(this.gvList.FocusedColumn, this.gvList.EditingValue);
            //}
            //this.gvList.UpdateCurrentRow();

            string outboundNo = this.txtOUTBANDNO.Text.Trim();
            string vbelnNo = this.txtVBELN.Text.Trim();
            string containerNo = this.txtContainerNo.Text.Trim();
            //string ciNo = this.txtCI.Text.Trim();
           // string shipmentType = Convert.ToString(this.lueShipmentType.EditValue);
            if ((outboundNo=="")&&(vbelnNo==""))
            {
                MessageBox.Show("外向交货单号或出库单号不能为空！");
                return;
            }
           
            if (MessageBox.Show(StringParser.Parse("确定要保存吗？"),
            StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {

                if (!this._entity.UpdateConteinerNo(outboundNo,vbelnNo, containerNo))
                {
                    MessageService.ShowMessage(_entity.ErrorMsg);
                }
                else
                {
                    MessageService.ShowMessage("保存成功！");
                    ResetControlValue();
                }
            }

            this.gvList.BestFitColumns();
        }

        private void txtOUTBANDNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (!string.IsNullOrEmpty(this.txtOUTBANDNO.Text.Trim()))
                {
                    this.tsbQuery_Click(sender, e);
                }

                if (string.IsNullOrEmpty(this.txtVBELN.Text.Trim()))
                {
                    this.txtVBELN.Select();
                }

            }
        }

        private void txtVBELN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == 13) && (!string.IsNullOrEmpty(this.txtVBELN.Text.Trim())))
            {
                this.tsbQuery_Click(sender, e);
            }
        }

        //private void tsbComfirm_Click(object sender, EventArgs e)
        //{
        //    string strCom=this.txtStauts.Text.Trim();
        //    string VebelNO=this.txtOUTBANDNO.Text.Trim();
        //    string OutboundNo=this.txtVBELN.Text.Trim();
        //    string strReturn = string.Empty;
        //    string Msg = string.Empty;
        //    int flag = 0;

        //    if ((VebelNO=="")||(OutboundNo==""))
        //    {
        //        MessageBox.Show("外向交货单号或出货单号不能为空！");
        //        return;
        //    }
        //    if (!(strCom=="已检验"))
        //    {
        //        MessageBox.Show("该外向交货单不能过账！");
        //        return;
        //    }
        //    //过账    
        //    strReturn = OutboundQCControl.OUTB_DELIVERY_CONFIRM(VebelNO, OutboundNo, out flag);
        //    //更新表
        //    Msg = this._entity.UpdateTable(strReturn, VebelNO, OutboundNo, flag);
            
        //    MessageService.ShowMessage(Msg, "提示");
        //}

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            ResetControlValue();
        }
       
        //private void gvList_RowClick(object sender, RowClickEventArgs e)
        //{
        //    if ((this.gvList.RowCount > 0))
        //    {
        //        int rowIndex;         
        //        rowIndex = this.gvList.GetSelectedRows()[0];
                
        //        this.txtVBELN.Text = this.gvList.GetRowCellValue(rowIndex, "VBELN").ToString();
        //        this.txtOUTBANDNO.Text = this.gvList.GetRowCellValue(rowIndex, "OUTBANDNO").ToString();
        //        this.txtContainerNo.Text = this.gvList.GetRowCellValue(rowIndex, "ShipmentNO").ToString();
        //        this.txtContainerNo_EditValueChanged(sender, e);
        //        this.tsbQuery_Click(sender ,e) ;
        //    }
        //}
       
        private void gvList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GridView gv = sender as GridView;
            SelectedRowChanged(gv);
            this.ChangetxtContainerNo();
        }
      

        private void SelectedRowChanged(GridView View)
        {
            if (View.GetSelectedRows().Length <= 0)
            {
                gcView.DataSource = null;
                return;
            }
            int selectedRow = View.GetSelectedRows()[0];
            
            if (View.GetSelectedRows()[0] >= View.RowCount)
            {
                View.SelectRow(0);
                selectedRow = 0;
            }

            string VeblnNo = View.GetRowCellValue(selectedRow, "VBELN").ToString();
            string OutboundNO = View.GetRowCellValue(selectedRow, "OUTBANDNO").ToString();
            string ShipmentNO = Convert.ToString(View.GetRowCellValue(selectedRow, gclShipmentNO));
            string ShipmentType = Convert.ToString(View.GetRowCellValue(selectedRow, "ShipmentType"));

            string SType = string.Empty;
            switch (ShipmentType)
            {
                case "陆运":
                    SType = "0";
                    break;
                case "海运":
                    SType = "1";
                    break;
                case "空运":
                    SType = "2";
                    break;
                default :
                    SType = "";
                    break;
            }
            this.txtVBELN.Text = VeblnNo;
            this.txtOUTBANDNO.Text = OutboundNO;
            this.txtContainerNo.Text = ShipmentNO;
            this.lueShipmentType.EditValue = SType;

            DataSet dsReturn = this._entity.getOutboudItem(OutboundNO, VeblnNo, ShipmentNO, SType);
            if (dsReturn.Tables.Count > 0)
                gcView.DataSource = dsReturn.Tables[0];
        }

        private void ChangetxtContainerNo()
        {
            this.txtContainerNo.Properties.ReadOnly = true;
            if (string.IsNullOrEmpty(txtContainerNo.Text.Trim()))
            {
                this.txtContainerNo.Properties.ReadOnly = false;
            }
        }
    }
}
