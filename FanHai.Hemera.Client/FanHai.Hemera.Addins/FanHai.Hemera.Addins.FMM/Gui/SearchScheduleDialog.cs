// ================================================================================
// 修改人               修改时间              说明
// --------------------------------------------------------------------------------
// chao.pang            2012-02-23            添加注释 
// ================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.FMM
{
    public partial class SearchScheduleDialog : BaseDialog
    {
        public Schedule _shcedule = new Schedule();
        //传入参数值为“排班计划查询” 
        public SearchScheduleDialog()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.SearchScheduleDialog.Title}"))
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _shcedule.ScheduleName = this.txtName.Text;
            DataSet dsSchedule = new DataSet();
            dsSchedule = _shcedule.SearchSchedule();
            ScheduleControl.MainView = ScheduleView;
            ScheduleControl.DataSource = dsSchedule.Tables[0];
        }

        private void SearchScheduleDialog_Load(object sender, EventArgs e)
        {
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");                           //查询     
            this.btnConfirm.Text = StringParser.Parse("${res:Global.OKButtonText}");                  //确定
            this.btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");               //取消

            this.description.Caption = StringParser.Parse("${res:Global.Description}");               //描述

            this.lblName.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleCtrl.ScheduleName}"); //计划名称
            this.schedule_name.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleCtrl.ScheduleName}"); //计划名称
        }

        private void ScheduleView_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool MapSelectedItemToProperties()
        {
            int rowHandle = ScheduleView.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                _shcedule.ScheduleKey = ScheduleView.GetRowCellValue(rowHandle, schedule_key).ToString();
                _shcedule.ScheduleName = ScheduleView.GetRowCellValue(rowHandle, schedule_name).ToString();
                _shcedule.Description = ScheduleView.GetRowCellValue(rowHandle, description).ToString();
                _shcedule.MaxOverLapTime = ScheduleView.GetRowCellValue(rowHandle, MAXOVERLAPTIME).ToString();
                object strEditor = ScheduleView.GetRowCellValue(rowHandle, editor);

                if (strEditor != null)
                {
                    _shcedule.Editor = strEditor.ToString();
                }
                else
                {
                    _shcedule.Editor = string.Empty;
                }
                object editTimeZone = ScheduleView.GetRowCellValue(rowHandle, edit_timeZone);
                if (editTimeZone != null)
                {
                    _shcedule.EditTimeZone = editTimeZone.ToString();
                }
                else
                {
                    _shcedule.EditTimeZone = string.Empty;
                }
                return true;
            }
            return false;
        }
    }
}
