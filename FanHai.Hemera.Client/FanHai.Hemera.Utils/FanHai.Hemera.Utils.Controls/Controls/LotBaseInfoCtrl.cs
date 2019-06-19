using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.Controls
{
    /// <summary>
    /// 表示批次基本信息的控件类。
    /// </summary>
    public partial class LotBaseInfoCtrl : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 工单号。
        /// </summary>
        public string OrderNumber
        {
            private set;
            get;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotBaseInfoCtrl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 设置值到窗体控件上。
        /// </summary>
        /// <param name="dataset">包含批次信息的数据集。</param>
        public void SetValueToControl(DataSet dsLot)
        {
            if (dsLot != null && dsLot.Tables.Count > 0 && dsLot.Tables[0].Rows.Count > 0)
            {
                this.lblLotName.Text = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString();
                this.lblWorkOrder.Text = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_NO].ToString();
                OrderNumber = this.lblWorkOrder.Text.Trim();
                this.lblPartName.Text = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_PRO_ID].ToString();
                this.lblQuantity.Text = dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY].ToString();
                this.lblRoute.Text = dsLot.Tables[0].Rows[0][POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME].ToString();
                this.lblStep.Text = dsLot.Tables[0].Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME].ToString();
            }
        }
    }
}
