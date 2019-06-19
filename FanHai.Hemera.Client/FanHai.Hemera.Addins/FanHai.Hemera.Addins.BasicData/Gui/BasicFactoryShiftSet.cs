using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using SolarViewer.Hemera.Utils.Entities;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Gui.Core;
using SolarViewer.Hemera.Share.CommonControls.Dialogs;
using SolarViewer.Hemera.Utils.Common;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;

namespace SolarViewer.Hemera.Addins.BasicData
{
    public partial class BasicFactoryShiftSet : UserControl
    {
        RptCommonEntity optEntity = new RptCommonEntity();
        string sFlag,sFactoryShiftSetKey;

        public BasicFactoryShiftSet()
        {
            InitializeComponent();
        }

        private void BasicFactoryShiftSet_Load(object sender, EventArgs e)
        {
            BindFactory();
            BindQFactory();
            BindShift();
            BindFactoryShift();
            ButtonState();
        }

        public void ButtonState()
        {
            switch (sFlag)
            {
                case "A":
                    tsbNew.Enabled = false;
                    tsbUpdate.Enabled = false;
                    tsbDelete.Enabled = false;
                    tsbCancel.Enabled = true;
                    tsbSave.Enabled = true;
                    lueFactory.Enabled = true;
                    lueFactory.EditValue = null;
                    deDate.Enabled = true;
                    deDate.Text = "";
                    lueShift.Enabled = true;
                    lueShift.EditValue = null;
                    lueFactoryShift.Enabled = true;
                    lueFactoryShift.EditValue = null;
                    sFactoryShiftSetKey = "";
                    break;
                case "U":
                    tsbNew.Enabled = false;
                    tsbUpdate.Enabled = false;
                    tsbDelete.Enabled = false;
                    tsbCancel.Enabled = true;
                    tsbSave.Enabled = true;
                    lueFactory.Enabled = true;
                    deDate.Enabled = true;
                    lueShift.Enabled = true;
                    lueFactoryShift.Enabled = true;
                    break;
                case "D":
                    tsbNew.Enabled = true;
                    tsbUpdate.Enabled = true;
                    tsbDelete.Enabled = true;
                    tsbCancel.Enabled = false;
                    tsbSave.Enabled = false;
                    lueFactory.Enabled = false;
                    lueFactory.EditValue = null;
                    deDate.Enabled = false;
                    deDate.Text = "";
                    lueShift.Enabled = false;
                    lueShift.EditValue = null;
                    lueFactoryShift.Enabled = false;
                    lueFactoryShift.EditValue = null;
                    sFactoryShiftSetKey = "";
                    break;
                case "C":
                    tsbNew.Enabled = true;
                    tsbUpdate.Enabled = true;
                    tsbDelete.Enabled = true;
                    tsbCancel.Enabled = false;
                    tsbSave.Enabled = false;
                    lueFactory.Enabled = false;
                    lueFactory.EditValue = null;
                    deDate.Enabled = false;
                    deDate.Text = "";
                    lueShift.Enabled = false;
                    lueShift.EditValue = null;
                    lueFactoryShift.Enabled = false;
                    lueFactoryShift.EditValue = null;
                    sFactoryShiftSetKey = "";
                    break;
                case "S":
                    tsbNew.Enabled = true;
                    tsbUpdate.Enabled = true;
                    tsbDelete.Enabled = true;
                    tsbCancel.Enabled = false;
                    tsbSave.Enabled = false;
                    lueFactory.Enabled = false;
                    lueFactory.EditValue = null;
                    deDate.Enabled = false;
                    deDate.Text = "";
                    lueShift.Enabled = false;
                    lueShift.EditValue = null;
                    lueFactoryShift.Enabled = false;
                    lueFactoryShift.EditValue = null;
                    sFactoryShiftSetKey = "";
                    break;
                default:
                    tsbNew.Enabled = true;
                    tsbUpdate.Enabled = true;
                    tsbDelete.Enabled = true;
                    tsbCancel.Enabled = false;
                    tsbSave.Enabled = false;
                    lueFactory.Enabled = false;
                    lueFactory.EditValue = null;
                    deDate.Enabled = false;
                    deDate.Text = "";
                    lueShift.Enabled = false;
                    lueShift.EditValue = null;
                    lueFactoryShift.Enabled = false;
                    lueFactoryShift.EditValue = null;
                    sFactoryShiftSetKey = "";
                    break;
            }
        }

        public void BindFactory()
        {
            DataSet dsFactory = optEntity.GetFactoryDate();
            lueFactory.Properties.DisplayMember = "LOCATION_NAME";
            lueFactory.Properties.ValueMember = "LOCATION_KEY";
            lueFactory.Properties.DataSource = dsFactory.Tables[0];
        }

        public void BindQFactory()
        {
            DataSet dsQFactory = optEntity.GetFactoryDate();
            lueQFactory.Properties.DisplayMember = "LOCATION_NAME";
            lueQFactory.Properties.ValueMember = "LOCATION_KEY";
            lueQFactory.Properties.DataSource = dsQFactory.Tables[0];
        }

        public void BindShift()
        {
            DataTable dtShift = new DataTable();
            dtShift.Columns.Add("SHIFT_VALUE");
            dtShift.Columns.Add("SHIFT_NAME");
            dtShift.Rows.Add("A", "白班");
            dtShift.Rows.Add("B", "夜班");
            lueShift.Properties.DisplayMember = "SHIFT_NAME";
            lueShift.Properties.ValueMember = "SHIFT_VALUE";
            lueShift.Properties.DataSource = dtShift;
        }

        public void BindFactoryShift()
        {
            DataTable dtFactoryShift = new DataTable();
            dtFactoryShift.Columns.Add("FACTORYSHIFT_NAME");
            dtFactoryShift.Columns.Add("FACTORYSHIFT_VALUE");
            dtFactoryShift.Rows.Add("A","A");
            dtFactoryShift.Rows.Add("B", "B");
            dtFactoryShift.Rows.Add("C", "C");
            dtFactoryShift.Rows.Add("D", "D");
            lueFactoryShift.Properties.DisplayMember = "FACTORYSHIFT_NAME";
            lueFactoryShift.Properties.ValueMember = "FACTORYSHIFT_VALUE";
            lueFactoryShift.Properties.DataSource = dtFactoryShift;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string sFactoryKey, sDataDate;

            sFactoryKey = Convert.ToString(lueQFactory.EditValue);
            sDataDate = deQDate.Text;

            DataSet dsFactoryShiftData = optEntity.GetFactoryShiftData("",sFactoryKey,sDataDate,"","");
            if (string.IsNullOrEmpty(optEntity.ErrorMsg))
            {
                gcFactoryShift.DataSource = null;
                gcFactoryShift.MainView = gvFactoryShift;
                gcFactoryShift.DataSource = dsFactoryShiftData.Tables[0];
                gvFactoryShift.BestFitColumns();
            }
            else
            {
                MessageService.ShowError(optEntity.ErrorMsg);
                return;
            }
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            sFlag = "A";
            ButtonState();
        }

        private void tsbUpdate_Click(object sender, EventArgs e)
        {
            if (gvFactoryShift.FocusedRowHandle < 0 || gvFactoryShift.RowCount < 1)
            {
                MessageService.ShowMessage("请选择要修改的数据!", "提示");
                return;
            }
            sFactoryShiftSetKey = gvFactoryShift.GetRowCellValue(gvFactoryShift.FocusedRowHandle, "FACTORYSHIFTSET_KEY").ToString();
            lueFactory.EditValue = gvFactoryShift.GetRowCellValue(gvFactoryShift.FocusedRowHandle, "FACTORYROOM_KEY").ToString();
            deDate.Text = gvFactoryShift.GetRowCellValue(gvFactoryShift.FocusedRowHandle, "DATA_DATE").ToString();
            lueShift.EditValue = gvFactoryShift.GetRowCellValue(gvFactoryShift.FocusedRowHandle, "SHIFT_VALUE").ToString();
            lueFactoryShift.EditValue = gvFactoryShift.GetRowCellValue(gvFactoryShift.FocusedRowHandle, "FACTORYSHIFT_NAME").ToString();
            sFlag = "U";
            ButtonState();
            btnQuery_Click(sender, e);
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (gvFactoryShift.FocusedRowHandle < 0 || gvFactoryShift.RowCount < 1)
            {
                MessageService.ShowMessage("请选择要删除的数据!", "提示");
                return;
            }
            if (MessageService.AskQuestion("确认要删除数据么?", "提示"))
            {
                sFactoryShiftSetKey = gvFactoryShift.GetRowCellValue(gvFactoryShift.FocusedRowHandle, "FACTORYSHIFTSET_KEY").ToString();
                if (!string.IsNullOrEmpty(sFactoryShiftSetKey) && !string.IsNullOrEmpty(sFactoryShiftSetKey))
                {
                    string sql = "DELETE FROM BASE_FACTORYSHIFT_SET WHERE FACTORYSHIFTSET_KEY='" + sFactoryShiftSetKey + "'";
                    DataSet dsDelFSS = optEntity.UpdateData(sql, "DelFactoryShiftSet");
                    int nDelRows = int.Parse(dsDelFSS.ExtendedProperties["rows"].ToString());
                    if (nDelRows < 1)
                    {
                        MessageService.ShowMessage("无数据删除！", "提示");
                        return;
                    }
                    if (!string.IsNullOrEmpty(optEntity.ErrorMsg))
                    {
                        MessageService.ShowError(optEntity.ErrorMsg);
                        return;
                    }
                    MessageService.ShowMessage("数据删除成功!", "提示");
                }
                else
                {
                    MessageService.ShowMessage("数据删除失败，请重试!", "提示");
                    return;
                }
            }
            sFlag = "D";
            ButtonState();
            btnQuery_Click(sender, e);
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            sFlag = "C";
            ButtonState();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            string sFactoryKey, sFactoryName, sDate, sShiftValue, sShiftName, sFactoryShiftName, sUID, sql;

            switch (sFlag)
            {
                case "A":
                    sFactoryKey = Convert.ToString(lueFactory.EditValue);
                    sFactoryName = lueFactory.Text;
                    sDate = deDate.Text;
                    sShiftValue = Convert.ToString(lueShift.EditValue);
                    sShiftName = lueShift.Text;
                    sFactoryShiftName = Convert.ToString(lueFactoryShift.EditValue);
                    if (string.IsNullOrEmpty(sFactoryKey) || string.IsNullOrEmpty(sFactoryName) || string.IsNullOrEmpty(sDate) || string.IsNullOrEmpty(sShiftValue) || string.IsNullOrEmpty(sShiftName))
                    {
                        MessageService.ShowMessage("厂别、日期、班别、生产排班不能为空，请确认!", "提示");
                        return;
                    }

                    DataSet dsFSSData = optEntity.GetFactoryShiftData("", sFactoryKey, sDate, sShiftValue, "");
                    if (dsFSSData.Tables[0].Rows.Count > 0)
                    {
                        MessageService.ShowMessage("数据已存在，不能添加重复数据，请确认！", "提示");
                        return;
                    }

                    sUID = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    sql = "INSERT INTO BASE_FACTORYSHIFT_SET(FACTORYSHIFTSET_KEY,FACTORYROOM_KEY,FACTORYROOM_NAME,";
                    sql += "DATA_DATE,SHIFT_VALUE,SHIFT_NAME,FACTORYSHIFT_NAME,CREATOR,CREATE_TIME)";
                    sql += " VALUES(NEWID(),'" + sFactoryKey + "','" + sFactoryName + "','" + sDate + "','" + sShiftValue + "','" + sShiftName + "','" + sFactoryShiftName + "','" + sUID  + "',GETDATE())";

                    DataSet dsAddFSS = optEntity.UpdateData(sql, "AddFactoryShiftSet");
                    int nAddRows = int.Parse(dsAddFSS.ExtendedProperties["rows"].ToString());
                    if (nAddRows < 1)
                    {
                        MessageService.ShowMessage("无数据更新！", "提示");
                        return;
                    }
                    if (!string.IsNullOrEmpty(optEntity.ErrorMsg))
                    {
                        MessageService.ShowError(optEntity.ErrorMsg);
                        return;
                    }
                    MessageService.ShowMessage("添加数据成功!", "提示");
                    break;
                case "U":
                    sFactoryKey = Convert.ToString(lueFactory.EditValue);
                    sFactoryName = lueFactory.Text;
                    sDate = deDate.Text;
                    sShiftValue = Convert.ToString(lueShift.EditValue);
                    sShiftName = lueShift.Text;
                    sFactoryShiftName = Convert.ToString(lueFactoryShift.EditValue);
                    if (string.IsNullOrEmpty(sFactoryKey) || string.IsNullOrEmpty(sFactoryName) || string.IsNullOrEmpty(sDate) || string.IsNullOrEmpty(sShiftValue) || string.IsNullOrEmpty(sShiftName))
                    {
                        MessageService.ShowMessage("厂别、日期、班别、生产排班不能为空，请确认!", "提示");
                        return;
                    }

                    sUID = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                    sql = "UPDATE BASE_FACTORYSHIFT_SET";
                    sql += " SET FACTORYROOM_KEY='" + sFactoryKey + "',FACTORYROOM_NAME='" + sFactoryName + "'";
                    sql += ",DATA_DATE='" + sDate + "',SHIFT_VALUE='" + sShiftValue + "',SHIFT_NAME='" + sShiftName + "'";
                    sql += ",FACTORYSHIFT_NAME='" + sFactoryShiftName + "',EDITOR='" + sUID + "',EDIT_TIME=GETDATE()";
                    sql += " WHERE FACTORYSHIFTSET_KEY='" + sFactoryShiftSetKey + "'";

                    DataSet dsUpdateFSS = optEntity.UpdateData(sql, "UpdateFactoryShiftSet");
                    int nUpRows = int.Parse(dsUpdateFSS.ExtendedProperties["rows"].ToString());
                    if (nUpRows < 1)
                    {
                        MessageService.ShowMessage("无数据更新！", "提示");
                        return;
                    }
                    if (!string.IsNullOrEmpty(optEntity.ErrorMsg))
                    {
                        MessageService.ShowError(optEntity.ErrorMsg);
                        return;
                    }
                    MessageService.ShowMessage("修改数据成功!", "提示");
                    break;
                case "D":
                    break;
                case "C":
                    break;
                case "S":
                    break;
                default:
                    break;
            }
            sFlag = "S";
            btnQuery_Click(sender, e);
            ButtonState();
        }
    }
}
