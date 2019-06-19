using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.SAP
{
    public partial class WorkOrderInfoListCtrl : BaseUserCtrl
    {
        public string Factory
        {
            get;
            set;
        }
        public string WorkOrderNo
        {
            get;
            set;
        }
        public string PartNo
        {
            get;
            set;
        }
        public string Type
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Store
        {
            get;
            set;
        }
        /// <summary>
        /// 状态。
        /// </summary>
        public string Status
        {
            get;
            set;
        }

        public WorkOrderInfoListCtrl()
        {
            InitializeComponent();
        }

        //load进来所有工单
        private void WorkOrderInfoList_Load(object sender, EventArgs e)
        {
            BindWorkOrderInfo();
        }

        //关闭
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        //查询
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //WorkOrderInfoListDialog workOrderInfoListDialog = new WorkOrderInfoListDialog();
            //workOrderInfoListDialog.pworkOrderInfoListCtrl = this;
            //if (workOrderInfoListDialog.ShowDialog() == DialogResult.OK)
            //{
            //    BindWorkOrderInfo();
            //}
            BindWorkOrderInfo();
        }
        //自动填充序号
        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "INDEX": //设置行号
                    e.DisplayText = Convert.ToString(e.RowHandle + 1);
                    break;
                default:
                    break;
            }
        }

        //分页事件
        private void pgnWorkOrderInfoList_DataPaging()
        {
            BindWorkOrderInfo();
        }

        private void BindWorkOrderInfo()
        {
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnWorkOrderInfoList.PageNo,
                PageSize = pgnWorkOrderInfoList.PageSize
            };
            WorkOrders workOrders = new WorkOrders();
            this.Factory = this.cbeFactory.Text;
            this.Status = this.cbeStatus.Text;
            this.WorkOrderNo = this.teWorkOrderNo.Text;
            this.PartNo = this.tePart.Text;
            this.Type = this.cbeType.Text;
            this.Store = this.teStore.Text;
            DataSet ds = workOrders.GetWorkOrderByCondition(this.Factory,this.WorkOrderNo, 
                this.PartNo, this.Type, this.Store, this.Status,ref config);

            pgnWorkOrderInfoList.Pages = config.Pages;
            pgnWorkOrderInfoList.Records = config.Records;
            if (string.IsNullOrEmpty(workOrders.ErrorMsg))
            {
                Content.MainView = gridView1;
                Content.DataSource = ds.Tables[0];
                gridView1.BestFitColumns();
            }
            else
            {
                MessageService.ShowMessage(workOrders.ErrorMsg);
            }
        }
    }
}
