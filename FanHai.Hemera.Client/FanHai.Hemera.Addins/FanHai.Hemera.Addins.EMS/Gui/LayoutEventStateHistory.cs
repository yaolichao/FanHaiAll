using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Addins.EMS.Gui
{
    public partial class LayoutEventStateHistory : BaseDialog
    {
        private string equKey = string.Empty, startDate = string.Empty, endDate = string.Empty;
        private Dictionary<string, string> _dictionary = new Dictionary<string, string>();
        private EquipmentLayoutEntity EquLayoutEntity = new EquipmentLayoutEntity();

        public LayoutEventStateHistory()
        {
            InitializeComponent();
        }

        public LayoutEventStateHistory(Dictionary<string,string> dictionary)
        {
            _dictionary = dictionary;
            InitializeComponent();
        }

        private void LayoutEventStateHistory_Load(object sender, EventArgs e)
        {
            //this.Text = "XXX详细信息";
            if (_dictionary.Count < 1) return;

            string msg = string.Empty;
            DataSet resDs = EquLayoutEntity.GetSingleLayoutEventDoWorkHistory(_dictionary, out msg);
            if (!msg.Trim().Equals(string.Empty))
            {
                MessageService.ShowError(msg);
                return;
            }
            else
            {
                GvDataSourceBind(resDs);
            }
        }

        private void GvDataSourceBind(DataSet ds)
        {
            if (ds.Tables.Contains(EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME))
            {
                gcEvents.MainView = gvEvents;
                gcEvents.DataSource = ds.Tables[EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME];
            }
            if (ds.Tables.Contains(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME))
            {
                gcDoWorkHistory.MainView = gvDoWorkHistory;
                gcDoWorkHistory.DataSource = ds.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
            }
        }
    }
}