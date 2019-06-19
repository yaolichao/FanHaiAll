//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// peter.zhang          2012-03-22            添加注释 
// =================================================================================
#region using
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Hemera.Share.CommonControls.Dialogs;
#endregion


namespace FanHai.Hemera.Addins.EDC
{
    /// <summary>
    /// 参数组查询对话框
    /// </summary>
    public partial class EDCSearchDialog : BaseDialog
    {
        public EDCSearchDialog()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.EDCSearchDialog.Title}"))
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable edcHashTable = new Hashtable();
                DataSet dataSet = new DataSet();

                string edcName = this.txtEdcName.Text.Trim();

                if (edcName.Length > 0)
                {
                    edcHashTable.Add(EDC_MAIN_FIELDS.FIELD_EDC_NAME, edcName);
                }

                DataTable edcDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(edcHashTable);
                edcDataTable.TableName = EDC_MAIN_FIELDS.DATABASE_TABLE_NAME;
                dataSet.Tables.Add(edcDataTable);

                string msg = string.Empty;
                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    dsReturn = serverFactory.CreateIEDCEngine().SearchEdcMain(dataSet);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        grdCtrlEdc.MainView = gridViewEdc;
                        grdCtrlEdc.DataSource = dsReturn.Tables[EDC_MAIN_FIELDS.DATABASE_TABLE_NAME];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
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

        private void gridViewEdc_DoubleClick(object sender, EventArgs e)
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
            int rowHandle = gridViewEdc.GetDataRowHandleByGroupRowHandle(gridViewEdc.FocusedRowHandle);
            if (rowHandle >= 0)
            {
                _edcKey = gridViewEdc.GetRowCellValue(rowHandle, EDC_MAIN_FIELDS.FIELD_EDC_KEY).ToString();
                _edcName = gridViewEdc.GetRowCellValue(rowHandle, EDC_MAIN_FIELDS.FIELD_EDC_NAME).ToString();

                return true;
            }
            return false;
        }

        #region Load Resource file data change UI languages
        /// <summary>
        /// Load Resource file data change UI languages
        /// </summary>
        private void EDCSearchDialog_Load(object sender, EventArgs e)
        {
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.btnConfirm.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");
            this.lblName.Text = StringParser.Parse("${res:Global.NameText}");
            this.gridColumn_EdcName.Caption = StringParser.Parse("${res:Global.NameText}");
            this.gridColumn_EdcDescription.Caption = StringParser.Parse("${res:Global.Description}");
        }
        #endregion

        private string _edcKey = string.Empty;
        private string _edcName = string.Empty;

        public string EdcKey
        {
            get { return _edcKey; }
            set { _edcKey = value; }
        }

        public string EdcName
        {
            get { return _edcName; }
            set { _edcName = value; }
        }

    }
}
