
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.FMM
{
    public partial class LineSearchDialog : BaseDialog
    {
        //string workOrder = "";
        public DataSet searchDataSet = new DataSet();

        #region Constructor
        public LineSearchDialog()
            : base("线别查询")
        {
            InitializeComponent();
            //InitEmptyWorkOrdersDataSet();
        }
        #endregion

        #region Properties
        public string ObjectKey
        {
            get
            {
                return _objectKey;
            }
        }
        public string ObjectName
        {
            get
            {
                return _objectName;
            }
        }
        #endregion  

        /// <summary>
        /// bind data to GridView
        /// </summary>
        /// <param name="dataSet"></param>
        private void BindDataSourceToGrid(DataTable dt)
        {
            gridData.MainView = gridDataView;
            gridData.DataSource = dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridDataView_ShowingEditor(object sender, CancelEventArgs e)
        {
            //gridView can't edit
            e.Cancel = true;
        }

        /// <summary>
        /// grid Control Double Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridData_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                // Close();
            }
        }
        #region Actions

        /// <summary>
        /// Search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearch_Click(object sender, EventArgs e)
        {

            Hashtable mainDataHashTable = new Hashtable();
            DataSet dataSet = new DataSet();
            mainDataHashTable.Add(FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME, this.txtAttributeName.Text.Trim());
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME;
            dataSet.Tables.Add(mainDataTable);
            //Call Remoting Service
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    DataSet retDS = factor.CreateIUdaCommonControlEx().SearchLineAttribute(dataSet);

                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(retDS);
                    if (null == returnMessage || returnMessage.Length > 0)
                    {
                        MessageService.ShowError(returnMessage);
                    }
                    else
                    {
                        BindDataSourceToGrid(retDS.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME]);
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
        private void btnCancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        #endregion Actions

        #region Private Functions

        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gridDataView.GetDataRowHandleByGroupRowHandle(gridDataView.FocusedRowHandle);
            if (rowHandle >= 0)
            {

                _objectKey = gridDataView.GetRowCellValue(rowHandle, FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY).ToString();
                _objectName = gridDataView.GetRowCellValue(rowHandle, FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME).ToString();

                return true;
            }
            return false;
        }

        private void MapWorkOrdersToGridView(DataTable dt)
        {
            gridData.DataSource = dt;
        }


        #endregion

        #region Private Variables Definition
        private string _objectKey = "";
        private string _objectName = "";

        private void LineSearchDialog_Load(object sender, EventArgs e)
        {
            #region InitUI

            this.btSearch.Text = StringParser.Parse("${res:Global.Query}");
            this.btnOK.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancle.Text = StringParser.Parse("${res:Global.CancelButtonText}");

            GridViewHelper.SetGridView(gridDataView);
            #endregion
        }
        #endregion Private Variables Definition

        private void gridDataView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
