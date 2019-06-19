using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.SAP
{
    public partial class WorkOrderInfoListDialog : BaseDialog
    {
        public WorkOrderInfoListCtrl pworkOrderInfoListCtrl
        {
            get;
            set;
        }

        public WorkOrderInfoListDialog()
        {
            InitializeComponent();
        }

        private void WorkOrderInfoListDialog_Load(object sender, EventArgs e)
        {
            this.cbeFactory.Text = pworkOrderInfoListCtrl.Factory;
            this.cbeStatus.Text = pworkOrderInfoListCtrl.Status;
            this.teWorkOrderNo.Text = pworkOrderInfoListCtrl.WorkOrderNo;
            this.tePart.Text = pworkOrderInfoListCtrl.PartNo;
            this.cbeType.Text = pworkOrderInfoListCtrl.Type;
            this.teStore.Text = pworkOrderInfoListCtrl.Store;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            pworkOrderInfoListCtrl.Factory = this.cbeFactory.Text;
            pworkOrderInfoListCtrl.Status = this.cbeStatus.Text;
            pworkOrderInfoListCtrl.WorkOrderNo = this.teWorkOrderNo.Text;
            pworkOrderInfoListCtrl.PartNo = this.tePart.Text;
            pworkOrderInfoListCtrl.Type = this.cbeType.Text;
            pworkOrderInfoListCtrl.Store = this.teStore.Text;
            this.Close();
        }
    }
}
