using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;
using FanHai.Hemera.Utils.Controls;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicCodeSoftLabel : BaseUserCtrl
    {
        BasicCodeSoftLabelEntity BasicCodeSoftLabelOject = new BasicCodeSoftLabelEntity();
        string sFlag;

        public BasicCodeSoftLabel()
        {
            InitializeComponent();
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            sFlag = "A";
            ButtonState();
        }

        private void tsbUpdate_Click(object sender, EventArgs e)
        {
            if (gvCodeSoftLabel.FocusedRowHandle < 0 || gvCodeSoftLabel.RowCount < 1)
            {
                MessageService.ShowMessage("请选择要修改的数据!", "提示");
                return;
            }
            txtLabelID.Text = gvCodeSoftLabel.GetRowCellValue(gvCodeSoftLabel.FocusedRowHandle, "LABEL_ID").ToString();
            txtLabelDisc.Text = gvCodeSoftLabel.GetRowCellValue(gvCodeSoftLabel.FocusedRowHandle, "LABEL_DISC").ToString();
            txtParameter.Text = gvCodeSoftLabel.GetRowCellValue(gvCodeSoftLabel.FocusedRowHandle, "PARAMETER").ToString();
            txtParamDisc.Text = gvCodeSoftLabel.GetRowCellValue(gvCodeSoftLabel.FocusedRowHandle, "PARAMETER_DISC").ToString();
            txtParamType.Text = gvCodeSoftLabel.GetRowCellValue(gvCodeSoftLabel.FocusedRowHandle, "PARAMETER_TYPE").ToString();
            sFlag = "U";
            ButtonState();
            btnQuery_Click(sender, e);
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (gvCodeSoftLabel.FocusedRowHandle < 0 || gvCodeSoftLabel.RowCount < 1)
            {
                MessageService.ShowMessage("请选择要删除的数据!", "提示");
                return;
            }
            if (MessageService.AskQuestion("确认要删除数据么?", "提示"))
            {
                string sLabelID = gvCodeSoftLabel.GetRowCellValue(gvCodeSoftLabel.FocusedRowHandle, "LABEL_ID").ToString();
                string sParameter = gvCodeSoftLabel.GetRowCellValue(gvCodeSoftLabel.FocusedRowHandle, "PARAMETER").ToString();
                if (!string.IsNullOrEmpty(sLabelID) && !string.IsNullOrEmpty(sParameter))
                {
                    string sql = "DELETE FROM BASE_CODESOFT_LABEL_SET WHERE LABEL_ID='" + sLabelID + "' AND PARAMETER='" + sParameter + "'";
                    DataSet dsDelCodeSoftLabel = BasicCodeSoftLabelOject.UpdateData(sql, "DelCodeSoftLabel");
                    int nDelRows = int.Parse(dsDelCodeSoftLabel.ExtendedProperties["rows"].ToString());
                    if (nDelRows < 1)
                    {
                        MessageService.ShowMessage("无数据删除！", "提示");
                        return;
                    }
                    if (!string.IsNullOrEmpty(BasicCodeSoftLabelOject.ErrorMsg))
                    {
                        MessageService.ShowError(BasicCodeSoftLabelOject.ErrorMsg);
                        return;
                    }
                    MessageService.ShowMessage("数据删除成功!", "提示");
                }
                else
                {
                    MessageService.ShowMessage("标签ID和参数名称为空，请重试!", "提示");
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
            string sLabelID, sLabelDisc, sParameter, sParamDisc, sParamType,sql;

            switch (sFlag)
            { 
                case "A":
                    sLabelID = txtLabelID.Text.Trim();
                    sLabelDisc = txtLabelDisc.Text.Trim();
                    sParameter = txtParameter.Text.Trim();
                    sParamDisc = txtParamDisc.Text.Trim();
                    sParamType = txtParamType.Text.Trim();
                    if (string.IsNullOrEmpty(sLabelID) || string.IsNullOrEmpty(sParameter))
                    {
                        MessageService.ShowMessage("标签ID、参数名称不能为空，请确认!", "提示");
                        return;
                    }
                    sql = "INSERT INTO BASE_CODESOFT_LABEL_SET(LABEL_ID,LABEL_DISC,PARAMETER,PARAMETER_DISC,PARAMETER_TYPE)";
                    sql += " VALUES('" + sLabelID + "','" + sLabelDisc + "','" + sParameter + "','" + sParamDisc + "','" + sParamType + "')";

                    DataSet dsAddCodeSoftLabel = BasicCodeSoftLabelOject.AddData(sql, "AddCodeSoftLabel");
                    int nAddRows = int.Parse(dsAddCodeSoftLabel.ExtendedProperties["rows"].ToString());
                    if (nAddRows < 1)
                    {
                        MessageService.ShowMessage("无数据更新！", "提示");
                        return;
                    }
                    if (!string.IsNullOrEmpty(BasicCodeSoftLabelOject.ErrorMsg))
                    {
                        MessageService.ShowError(BasicCodeSoftLabelOject.ErrorMsg);
                        return;
                    }
                    MessageService.ShowMessage("添加数据成功!", "提示");
                    break;
                case "U":
                    sLabelID = txtLabelID.Text.Trim();
                    sLabelDisc = txtLabelDisc.Text.Trim();
                    sParameter = txtParameter.Text.Trim();
                    sParamDisc = txtParamDisc.Text.Trim();
                    sParamType = txtParamType.Text.Trim();
                    if (string.IsNullOrEmpty(sLabelID) || string.IsNullOrEmpty(sParameter))
                    {
                        MessageService.ShowMessage("标签ID、参数名称不能为空，请确认!", "提示");
                        return;
                    }
                    sql = "UPDATE BASE_CODESOFT_LABEL_SET SET LABEL_DISC='" + sLabelDisc + "',PARAMETER_DISC='" + sParamDisc + "',PARAMETER_TYPE='" + sParamType + "'";
                    sql += " WHERE LABEL_ID='" + sLabelID + " ' AND PARAMETER='" + sParameter + "'";

                    DataSet dsUpCodeSoftLabel = BasicCodeSoftLabelOject.UpdateData(sql, "UpCodeSoftLabel");
                    int nUpRows = int.Parse(dsUpCodeSoftLabel.ExtendedProperties["rows"].ToString());
                    if (nUpRows < 1)
                    {
                        MessageService.ShowMessage("无数据更新！", "提示");
                        return;
                    }
                    if (!string.IsNullOrEmpty(BasicCodeSoftLabelOject.ErrorMsg))
                    {
                        MessageService.ShowError(BasicCodeSoftLabelOject.ErrorMsg);
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

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string sLabelID;

            sLabelID = txtQLabelID.Text.Trim();

            DataSet dsCodeSoftLabel = BasicCodeSoftLabelOject.GetCodeSoftLabel(sLabelID);
            if (string.IsNullOrEmpty(BasicCodeSoftLabelOject.ErrorMsg))
            {
                gcCodeSoftLabel.DataSource = null;
                gcCodeSoftLabel.MainView = gvCodeSoftLabel;
                gcCodeSoftLabel.DataSource = dsCodeSoftLabel.Tables[0];
                gvCodeSoftLabel.BestFitColumns();
            }
            else
            {
                MessageService.ShowError(BasicCodeSoftLabelOject.ErrorMsg);
                return;
            }
        }

        private void BasicCodeSoftLabel_Load(object sender, EventArgs e)
        {
            ButtonState();
        }

        public void ButtonState()
        { 
            switch(sFlag)
            {
                case "A":
                    tsbNew.Enabled = false;
                    tsbUpdate.Enabled = false;
                    tsbDelete.Enabled = false;
                    tsbCancel.Enabled = true;
                    tsbSave.Enabled = true;
                    txtLabelID.Enabled = true;
                    txtLabelID.Text = "";
                    txtLabelDisc.Enabled = true;
                    txtLabelDisc.Text = "";
                    txtParameter.Enabled = true;
                    txtParameter.Text = "";
                    txtParamDisc.Enabled = true;
                    txtParamDisc.Text = "";
                    txtParamType.Enabled = true;
                    txtParamType.Text = "";
                    break;
                case "U":
                    tsbNew.Enabled = false;
                    tsbUpdate.Enabled = false;
                    tsbDelete.Enabled = false;
                    tsbCancel.Enabled = true;
                    tsbSave.Enabled = true;
                    txtLabelID.Enabled = false;
                    txtLabelDisc.Enabled = true;
                    txtParameter.Enabled = false;
                    txtParamDisc.Enabled = true;
                    txtParamType.Enabled = true;
                    break;
                case "D":
                    tsbNew.Enabled = true;
                    tsbUpdate.Enabled = true;
                    tsbDelete.Enabled = true;
                    tsbCancel.Enabled = false;
                    tsbSave.Enabled = false;
                    txtLabelID.Enabled = false;
                    txtLabelID.Text = "";
                    txtLabelDisc.Enabled = false;
                    txtLabelDisc.Text = "";
                    txtParameter.Enabled = false;
                    txtParameter.Text = "";
                    txtParamDisc.Enabled = false;
                    txtParamDisc.Text = "";
                    txtParamType.Enabled = false;
                    txtParamType.Text = "";
                    break;
                case "C":
                    tsbNew.Enabled = true;
                    tsbUpdate.Enabled = true;
                    tsbDelete.Enabled = true;
                    tsbCancel.Enabled = false;
                    tsbSave.Enabled = false;
                    txtLabelID.Enabled = false;
                    txtLabelID.Text = "";
                    txtLabelDisc.Enabled = false;
                    txtLabelDisc.Text = "";
                    txtParameter.Enabled = false;
                    txtParameter.Text = "";
                    txtParamDisc.Enabled = false;
                    txtParamDisc.Text = "";
                    txtParamType.Enabled = false;
                    txtParamType.Text = "";
                    break;
                case "S":
                    tsbNew.Enabled = true;
                    tsbUpdate.Enabled = true;
                    tsbDelete.Enabled = true;
                    tsbCancel.Enabled = false;
                    tsbSave.Enabled = false;
                    txtLabelID.Enabled = false;
                    txtLabelID.Text = "";
                    txtLabelDisc.Enabled = false;
                    txtLabelDisc.Text = "";
                    txtParameter.Enabled = false;
                    txtParameter.Text = "";
                    txtParamDisc.Enabled = false;
                    txtParamDisc.Text = "";
                    txtParamType.Enabled = false;
                    txtParamType.Text = "";
                    break;
                default:
                    tsbNew.Enabled = true;
                    tsbUpdate.Enabled = true;
                    tsbDelete.Enabled = true;
                    tsbCancel.Enabled = false;
                    tsbSave.Enabled = false;
                    txtLabelID.Enabled = false;
                    txtLabelID.Text = "";
                    txtLabelDisc.Enabled = false;
                    txtLabelDisc.Text = "";
                    txtParameter.Enabled = false;
                    txtParameter.Text = "";
                    txtParamDisc.Enabled = false;
                    txtParamDisc.Text = "";
                    txtParamType.Enabled = false;
                    txtParamType.Text = "";
                    break;
            }
        }

        /// <summary>
        /// 设置行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvCodeSoftLabel_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
