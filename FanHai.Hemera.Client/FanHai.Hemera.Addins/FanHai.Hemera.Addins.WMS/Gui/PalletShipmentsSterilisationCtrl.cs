
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
using System.Threading;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WMS
{
    /// <summary>
    /// 表示托盘出货作业的窗体类。
    /// </summary>
    public partial class PalletShipmentsSterilisationCtrl : BaseUserCtrl
    {
        IViewContent _view = null;                                      //当前视图。
        /// <summary>
        /// 出货对象。
        /// </summary>
        ShipmentOperationEntity _entity = new ShipmentOperationEntity();
        /// <summary>
        /// 构造函数
        /// </summary>
        public PalletShipmentsSterilisationCtrl(IViewContent view)
        {
            InitializeComponent();
            this._view = view;

            InitUi();
            GridViewHelper.SetGridView(gvList);
        }

        private void InitUi()
        {
            lblMenu.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.lbl.0001}");
            lciShipmentNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.lbl.0002}");
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.lbl.0003}");

            tspSterilisation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.btn.0001}");
            tsbSelect.Text = StringParser.Parse("${res:Global.Query}");

            gcRowNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.Column.0001}");
            gclShipmentNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.Column.0002}");
            gclContainerNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.Column.0003}");
            gclPalletNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.Column.0004}");
            gclShipmentType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.Column.0005}");
            gclQuantity.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.Column.0006}");
            gclWorkorderNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.Column.0007}");
            gclSAPNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.Column.0008}");
            gclPowerLevel.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.Column.0009}");
            gcPowerRange.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.Column.0010}");
            gclShipmentDate.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.Column.0011}");
            gclShipmentOperator.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.Column.0012}");
            gclCustCheck.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.Column.0013}");
            this.lblMenu.Text = "库房管理>出货管理>出货冲销";
        }

        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PalletShipmentCtrl_Load(object sender, EventArgs e)
        {

        }
        
        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this._view.WorkbenchWindow.CloseWindow(false);
        }
        
        private void BindRowNumber()
        {
            DataTable dtSource = gcList.DataSource as DataTable;
            if (dtSource != null)
            {
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    dtSource.Rows[i]["ROWNUMBER"] = i + 1;
                }
            }
            gcList.DataSource = dtSource;
        }

        private void tsbSelect_Click(object sender, EventArgs e)
        {
            string sNum = teShipmentNo.Text.Trim();
            if (!string.IsNullOrEmpty(sNum))
            {
                bool boolen = this._entity.SelectShipmentInf(sNum);
                if (boolen == true)
                {
                    SelectInfBysNum(sNum);
                    BindRowNumber();
                }
                else
                {
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.msg.0002}"), StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.msg.0001}"));//出货单号不存在或出货单号存在重复，请确认！
                }
            }
        }
        /// <summary>
        /// 根据返回的出货单查询出货单的明细信息
        /// </summary>
        /// <param name="sNum"></param>
        public void SelectInfBysNum(string sNum)
        {
            this.teShipmentNo.Text = sNum;
            DataSet dsInf = this._entity.GetShipmentInf(sNum);
            DataTable dt = dsInf.Tables[0];

            switch (dt.Rows[0]["SHIPMENT_FLAG"].ToString().Trim())
            {
                case "0":
                    txtStatus.Text = "已保存未出货";
                    break;
                case "1":
                    txtStatus.Text = "已出货";
                    break;
                default:
                    break;
            }
            gcList.DataSource = dt;
            BindRowNumber();
        }

        private void tspSterilisation_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(teShipmentNo.Text.Trim()))
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.msg.0003}"), StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.msg.0001}"));//请先输入出货单号，然后查询！
                return;
            }
            if (txtStatus.Text.Equals("已保存未出货"))
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.msg.0004}"), StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.msg.0001}"));//该出货单现在未做出货无需冲销，请查询已经出货的信息做冲销！
                return;
            }
            else if (txtStatus.Text.Equals("已出货"))
            {
                if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.msg.0005}"), StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.msg.0001}")))//你确定要冲销当前界面的数据吗？
                {
                    if (_entity.SterilisationShipment(this.teShipmentNo.Text.Trim()))
                    {
                        this.txtStatus.Text = "";
                        this.teShipmentNo.SelectAll();
                        this.gcList.DataSource = null;
                    }
                }
            }
            else 
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.msg.0006}"), StringParser.Parse("${res:FanHai.Hemera.Addins.WMS.Gui.PalletShipmentsSterilisation.msg.0001}"));//请单击查询，查看出货单信息！
            }
        }

        private void gvList_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
