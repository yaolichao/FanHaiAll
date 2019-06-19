//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// ============================================================================
// 修改人               修改时间              说明
// ----------------------------------------------------------------------------
//  Peter             2012-10-22              修改程序
// ============================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 选择工艺流程的对话框。
    /// </summary>
    public partial class OperationHelpDialog : Form
    {
        /// <summary>
        /// 车间名称。
        /// </summary>
        public string FactoryRoom
        {
            set;
            get;
        }
        /// <summary>
        /// 产品类型。
        /// </summary>
        public string  ProductType
        {
            set;
            get;
        }
        /// <summary>
        /// 是否重工。
        /// </summary>
        public bool IsRework
        {
            set;
            get;
        }
        /// <summary>
        /// 存储工艺流程组名称（Text）和主键（EditValue）。
        /// </summary>
        public DevExpress.XtraEditors.TextEdit EnterpriseName
        {
            set;
            get;
        }
        /// <summary>
        /// 存储工艺流程名称（Text）和主键（EditValue）。
        /// </summary>
        public  DevExpress.XtraEditors.TextEdit RouteName
        {
            set;
            get;
        }
        /// <summary>
        /// 存储工步名称（Text）和主键（EditValue）。
        /// </summary>
        public  DevExpress.XtraEditors.TextEdit StepName
        {
            set;
            get;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OperationHelpDialog()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationHelpDialog_Load(object sender, EventArgs e)
        {
            try
            {
                RouteQueryEntity entity = new RouteQueryEntity();
                DataSet dsOperation = entity.GetProcessPlan(this.FactoryRoom, this.ProductType, this.IsRework); 
                string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsOperation);
                if (string.IsNullOrEmpty(msg))
                {
                    this.gcERS.MainView = this.gvERS;
                    this.gcERS.DataSource = dsOperation.Tables[0];
                }
                else
                {
                    MessageService.ShowError(msg);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// 双击选择工艺流程组、工艺流程及工步。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ERSView_DoubleClick(object sender, EventArgs e)
        {
            int rowIndex = gvERS.FocusedRowHandle;
            if (rowIndex >= 0)
            {

                this.RouteName.Tag = Convert.ToString(gvERS.GetRowCellValue(rowIndex, POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY));
                this.StepName.Tag = Convert.ToString(gvERS.GetRowCellValue(rowIndex, POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY));
                this.EnterpriseName.Tag = Convert.ToString(gvERS.GetRowCellValue(rowIndex, POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY));
                this.RouteName.Text = Convert.ToString(gvERS.GetRowCellValue(rowIndex, POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME));
                this.StepName.Text = Convert.ToString(gvERS.GetRowCellValue(rowIndex, POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME));
                this.EnterpriseName.Text = Convert.ToString(gvERS.GetRowCellValue(rowIndex, POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME));              
                this.Visible = false;
                this.Close();
            }
        }
        /// <summary>
        /// 触发非激活事件，隐藏并关闭对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationHelpDialog_Deactivate(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Close();
        }
    }
}
