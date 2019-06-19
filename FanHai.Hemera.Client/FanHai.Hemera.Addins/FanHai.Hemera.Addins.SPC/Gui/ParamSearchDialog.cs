/*
<FileInfo>
  <Author>Rayna Liu, SolarViewer Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 SolarViewer. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region using
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

using DevExpress.XtraEditors;
using SolarViewer.Hemera.Utils.Entities;
using SolarViewer.Gui.Core;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Share.CommonControls.Dialogs;
using SolarViewer.Hemera.Utils.Common;
#endregion


namespace SolarViewer.Hemera.Addins.SPC
{
    public partial class ParamSearchDialog : BaseDialog
    {
        public ParamSearchDialog()
            : base(StringParser.Parse("${res:SolarViewer.Hemera.Addins.EDC.ParamSearchDialog.Title}"))
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            SpcEntity spcEntity = new SpcEntity();
            spcEntity.ParamName = this.txtParamName.Text.Trim();
            DataSet dsParams = spcEntity.SearchParams();
            if (spcEntity.ErrorMsg.Length > 0)
            {
                MessageService.ShowError("查询参数出错！错误信息："+spcEntity.ErrorMsg);
            }
            else
            {
                if (dsParams != null && dsParams.Tables.Count > 0 && dsParams.Tables.Contains(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME))
                {
                    try
                    {
                        DataTable paramTable = dsParams.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME];
                        //paramTable.Rows.Add("EFF","EFF","");
                        //paramTable.Rows.Add("UOC", "UOC","");
                        //paramTable.Rows.Add("ISC", "ISC","");
                        //paramTable.Rows.Add("FF", "FF","");
                        //paramTable.Rows.Add("IREV1", "IREV1","");
                        //paramTable.Rows.Add("IREV2", "IREV2","");
                        //paramTable.Rows.Add("RSH", "RSH","");
                        //paramTable.Rows.Add("RSER", "RSER","");
                        string[] columns = new string[] { "PARAMETER_NAME" };
                        KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "SPC_PARAMETER");
                        DataTable baseParamTable = BaseData.Get(columns, category);
                        if (baseParamTable != null)
                        {
                            foreach (DataRow row in baseParamTable.Rows)
                            {
                                paramTable.Rows.Add(row["PARAMETER_NAME"].ToString(), row["PARAMETER_NAME"].ToString(), "");
                            }
                        }
                        grdCtrlParam.MainView = gridViewParam;
                        grdCtrlParam.DataSource = paramTable;
                    }
                    catch (Exception ex)
                    {
                        MessageService.ShowError(ex.Message);
                    }
                }
            }               
           
        }

        private void gridViewParam_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btConfirm_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }


        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gridViewParam.GetDataRowHandleByGroupRowHandle(gridViewParam.FocusedRowHandle);
            if (rowHandle >= 0)
            {
                _paramKey = gridViewParam.GetRowCellValue(rowHandle, BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY).ToString();
                _paramName = gridViewParam.GetRowCellValue(rowHandle, BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME).ToString();
                
                return true;
            }
            return false;
        }

        
        #region Load Resource file data change UI languages
        /// <summary>
        /// Load Resource file data change UI languages
        /// </summary>
        private void ParamSearchDialog_Load(object sender, EventArgs e)
        {
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.btnConfirm.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");
            this.lblParamName.Text = StringParser.Parse("${res:Global.NameText}");
            this.gridColumn_paramName.Caption = StringParser.Parse("${res:Global.NameText}");
            this.gridColumn_paramDescription.Caption = StringParser.Parse("${res:Global.Description}");
       
        }
        #endregion

        #region Properties
        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; }
        }

        public string ParamKey
        {
            get { return _paramKey; }
            set { _paramKey = value; }
        }
        #endregion

        #region Private variable definition
        private string _paramKey = string.Empty;
        private string _paramName = string.Empty;
        #endregion

    }
}
