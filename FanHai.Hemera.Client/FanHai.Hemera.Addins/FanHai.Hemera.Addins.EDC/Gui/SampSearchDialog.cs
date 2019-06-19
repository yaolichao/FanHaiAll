
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
    /// 采样规则查询对话框。
    /// </summary>
    public partial class SampSearchDialog : BaseDialog
    {
        public SampSearchDialog()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampSearchDialog.Title}"))
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable spHashTable = new Hashtable();
                DataSet dataSet = new DataSet();

                string spName = this.txtSpName.Text.Trim();

                if (spName.Length > 0)
                {
                    spHashTable.Add(EDC_SP_FIELDS.FIELD_SP_NAME, spName);
                }

                DataTable spDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(spHashTable);
                spDataTable.TableName = EDC_SP_FIELDS.DATABASE_TABLE_NAME;
                dataSet.Tables.Add(spDataTable);

                string msg = string.Empty;
                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    dsReturn = serverFactory.CreateISampEngine().SearchSamp(dataSet);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        grdCtrlSamp.MainView = gridViewSamp;
                        grdCtrlSamp.DataSource = dsReturn.Tables[EDC_SP_FIELDS.DATABASE_TABLE_NAME];
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
            int rowHandle = gridViewSamp.GetDataRowHandleByGroupRowHandle(gridViewSamp.FocusedRowHandle);
            if (rowHandle >= 0)
            {
                _spKey = gridViewSamp.GetRowCellValue(rowHandle, EDC_SP_FIELDS.FIELD_SP_KEY).ToString();
                _spName = gridViewSamp.GetRowCellValue(rowHandle, EDC_SP_FIELDS.FIELD_SP_NAME).ToString();

                return true;
            }
            return false;
        }

        #region Load Resource file data change UI languages
        /// <summary>
        /// Load Resource file data change UI languages
        /// </summary>
        private void SampSearchDialog_Load(object sender, EventArgs e)
        {
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.btnConfirm.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");
            this.lblSpName.Text = StringParser.Parse("${res:Global.NameText}");
            this.gridColumn_SpName.Caption = StringParser.Parse("${res:Global.NameText}");
            this.gridColumn_SpDescription.Caption = StringParser.Parse("${res:Global.Description}");
        }

        #endregion

        private string _spKey = string.Empty;
        private string _spName = string.Empty;

        public string SpKey
        {
            get { return _spKey; }
            set { _spKey = value; }
        }

        public string SpName
        {
            get { return _spName; }
            set { _spName = value; }
        }

    }
}
