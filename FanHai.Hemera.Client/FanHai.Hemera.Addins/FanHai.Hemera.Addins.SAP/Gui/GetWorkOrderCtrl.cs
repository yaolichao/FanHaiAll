using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using System.Threading;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.SAP
{
    /// <summary>
    /// 工单获取控件。
    /// </summary>
    public partial class GetWorkOrderCtrl : BaseUserCtrl
    {
        private string User = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

        /// <summary>
        /// 定义用于工单操作的对象
        /// </summary>
        private WorkOrders workOrders = new WorkOrders();
        /// <summary>
        /// 构造函数。
        /// </summary>
        public GetWorkOrderCtrl()
        {
            InitializeComponent();
            InitializeLanguage();
        }


        public void InitializeLanguage()
        {
            lblMenu.Text = "基础数据 > 工单管理 > ERP工单";
            GridViewHelper.SetGridView(gdvWorkOrderDefault);

            //this.gcSEQ.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.gcSEQ}");//"序号";
            //this.gcOrderNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.gcOrderNum}");// "工单号";
            //this.gcPartNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.gcPartNum}");//"成品编码";
            //this.gcDescriptions.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.gcDescriptions}");//"成品描述";
            //this.gcOrderType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.gcOrderType}");//"订单类型";
            //this.gcQuantityOrdered.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.gcQuantityOrdered}");//"订单数量";
            //this.gcOrderState.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.gcOrderState}");//"状态";
            //this.gcStartTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.gcStartTime}");//"开始日期";
            //this.gcFinishTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.gcFinishTime}");//"完成日期";
            //this.gcStockLocation.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.gcStockLocation}");//"入库库位";
            //this.gcFactoryName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.gcFactoryName}");//"工厂";
            //this.lbWorkOrder.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.lbWorkOrder}");//"工单号";
            //this.lciResults.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.lciResults}");//"结果";
            //this.tsbGetWorkOrderInfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.tsbGetWorkOrderInfo}");//"SAP获取工单信息";
            //this.tsbGetWorkOrderList.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.GetWorkOrderCtrl.tsbGetWorkOrderList}");//"工单信息清单";
        }

        /// <summary>
        /// SAP获取工单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbGetWorkOrderInfo_Click(object sender, EventArgs e)
        {
            GetWorkOrderInfo();
        }
        /// <summary>
        /// 获取工单信息的方法
        /// </summary>
        private void GetWorkOrderInfo()
        {
            int count=0;
            //根据工单号从接口数据库获取工单信息
            DataTable dtWorkOrder = workOrders.GetWorkOrder(User);
            //将取得的工单信息写入MES数据库，并更新SAP接口数据库的读取标志
            if (dtWorkOrder.Rows.Count > 0)
            {
                //将返回的工单信息显示到grid上。
                gdcData.DataSource = dtWorkOrder;
                count=dtWorkOrder.Rows.Count;
            }
            else
            {
                gdcData.DataSource = null;
            }
            MessageService.ShowMessage(string.Format("共获取或更新{0}条工单记录。", count),"提示");
        }
        /// <summary>
        /// 工单清单列表查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbGetWorkOrderList_Click(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == "工单信息清单")
                {
                    viewContent.WorkbenchWindow.SelectWindow();

                    return;
                }
            }
            WorkOrderInfoListViewContent workOrderInfoListViewContent = new WorkOrderInfoListViewContent();
            WorkbenchSingleton.Workbench.ShowView(workOrderInfoListViewContent);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        private void txtWorkOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                GetWorkOrderInfo();
            }
        }

        private void GetWorkOrderCtrl_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 自定义绘制单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdvWorkOrderDefault_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gcSEQ)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void gdvWorkOrderDefault_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void tsbGetWorkOrderList1_Click(object sender, EventArgs e)
        {

        }

        private void tsbGetWorkOrderInfo1_Click(object sender, EventArgs e)
        {

        }
    }
}
