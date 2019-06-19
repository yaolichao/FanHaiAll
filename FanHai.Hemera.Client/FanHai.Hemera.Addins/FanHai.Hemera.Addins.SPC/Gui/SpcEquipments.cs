using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.SPC.Gui
{
    public partial class SpcEquipments : DevExpress.XtraEditors.XtraForm
    {
        DataTable dtEquipment = null;
        public string equipmentCodes = string.Empty;
        public string equipmentKeys = string.Empty;
        public SpcEquipments(DataTable dataEquipment)
        {
            dtEquipment = dataEquipment;
            
            InitializeComponent();
        }
        public SpcEquipments()
        {
            InitializeComponent();
        }
        private void SpcEquipments_Load(object sender, EventArgs e)
        {
            InitialData();
        }
        private void InitialData()
        {
            try
            {
                if (dtEquipment.Rows.Count > 0)
                {
                    this.gcEquipment.MainView = gvEquipment;
                    this.gcEquipment.DataSource = dtEquipment;
                    gvEquipment.BestFitColumns();

                    this.Text = string.Format("可选择的设备有{0}笔", dtEquipment.Rows.Count.ToString());
                }
                else
                    this.Text = string.Format("没有可选择的设备");
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage(ex.Message);
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            int rowHandle = gvEquipment.FocusedRowHandle;
            if (rowHandle > 0)
            {
                equipmentCodes = gvEquipment.GetRowCellValue(rowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE).ToString();
                equipmentKeys = gvEquipment.GetRowCellValue(rowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY).ToString();
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageService.ShowMessage("未选择数据,请确认!");
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void gcEquipment_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gvEquipment.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                equipmentCodes = gvEquipment.GetRowCellValue(rowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE).ToString();
                equipmentKeys = gvEquipment.GetRowCellValue(rowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY).ToString();

                return true;
            }
            return false;
        }


    }
}