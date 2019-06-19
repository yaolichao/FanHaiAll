
#region using
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.CommonControls.Dialogs;

using DevExpress.XtraEditors;
#endregion

namespace FanHai.Hemera.Addins.FMM
{
    public partial class EnterpriseSearchDialog : BaseDialog
    {
        public EnterpriseSearchDialog()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseSearchDialog.Title}"))
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable hashTable = new Hashtable();
                DataSet dataSet = new DataSet();

                string strName = this.txtName.Text.Trim();

                if (strName.Length > 0)
                {
                    hashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME, strName);
                }

                DataTable dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                dataTable.TableName = POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME;
                dataSet.Tables.Add(dataTable);

                string msg = string.Empty;
                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    dsReturn = serverFactory.CreateIEnterpriseEngine().EnterpriseSearch(dataSet);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        if (dsReturn.Tables[POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME].Rows.Count < 1)
                            MessageService.ShowMessage
                                ("${res:FanHai.Hemera.Addins.FMM.SearchDialog.Message}", "${res:Global.SystemInfo}");

                        grdCtrlCommon.MainView = grdViewCommon;
                        grdCtrlCommon.DataSource = dsReturn.Tables[POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME];
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


        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnQuery_Click(new object(), EventArgs.Empty);
            }
        }

        private void gridViewCommon_DoubleClick(object sender, EventArgs e)
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
            int rowHandle = grdViewCommon.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                _enterpriseKey = grdViewCommon.GetRowCellValue(rowHandle, POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY).ToString();
                _enterpriseName = grdViewCommon.GetRowCellValue(rowHandle, POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME).ToString();
                _enterpriseVersion = grdViewCommon.GetRowCellValue(rowHandle, POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_VERSION).ToString();
                return true;
            }
            return false;
        }

        #region Load Resource file data change UI languages
        /// <summary>
        /// Load Resource file data change UI languages
        /// </summary>
        private void EnterpriseSearchDialog_Load(object sender, EventArgs e)
        {
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.btnConfirm.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");

            this.lblName.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.SearchDialog.lblName}");
            this.gridColumn_Name.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.SearchDialog.gridColumn_Name}");
            this.gridColumn_Description.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.SearchDialog.gridColumn_Description}");
            this.gridColumn_Version.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.SearchDialog.gridColumn_Version}");
            this.gridColumn_Creator.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.SearchDialog.gridColumn_Creator}");
            this.gridColumn_CreateTime.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.SearchDialog.gridColumn_CreateTime}");
        }
        #endregion

        #region Properties
        public string EnterpriseKey
        {
            get { return _enterpriseKey; }
            set { _enterpriseKey = value; }
        }

        public string EnterpriseName
        {
            get { return _enterpriseName; }
            set { _enterpriseName = value; }
        }

        public string EnterpriseVersion
        {
            get { return _enterpriseVersion; }
            set { _enterpriseVersion = value; }
        }
        #endregion

        #region Private variable definition
        private string _enterpriseKey = string.Empty;
        private string _enterpriseName = string.Empty;
        private string _enterpriseVersion = string.Empty;
        #endregion
    }
}
