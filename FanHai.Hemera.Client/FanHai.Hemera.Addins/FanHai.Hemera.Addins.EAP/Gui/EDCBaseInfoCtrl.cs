using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.EAP
{
    public partial class EDCBaseInfoCtrl : DevExpress.XtraEditors.XtraUserControl
    {
        public EDCBaseInfoCtrl(EdcGatherData data)
        {
            InitializeComponent();
            this.lblFactoryRoomName.Text = data.FactoryRoomName;
            this.lblOperationName.Text = data.OperationName;
            this.lblLotNumber.Text = data.LotNumber;
            this.lblMaterialLot.Text = data.MaterialLot;
            this.lblPartNumber.Text = data.PartNumber;
            this.lblPartType.Text = data.PartType;
            this.lblEquipmentName.Text = data.EquipmentName;
           
            this.lblOperator.Text = data.Operator;
        }
    }
}
