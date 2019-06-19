//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-11-08            添加注释 
// =================================================================================
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.CommonControls.Dialogs;

using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 工序查询对话框。
    /// </summary>
    public partial class OperationSearchDialog : BaseDialog
    {
        /// <summary>
        /// 工序主键。
        /// </summary>
        private string _operationKey = string.Empty;
        /// <summary>
        /// 工序名称。
        /// </summary>
        private string _operationName = string.Empty;
        /// <summary>
        /// 工序版本号。
        /// </summary>
        private string _operationVersion = string.Empty;
        /// <summary>
        /// 工序名称。
        /// </summary>
        public string OperationName
        {
            get { return _operationName; }
            set { _operationName = value; }
        }
        /// <summary>
        /// 工序主键。
        /// </summary>
        public string OperationKey
        {
            get { return _operationKey; }
            set { _operationKey = value; }
        }
        /// <summary>
        /// 工序版本。
        /// </summary>
        public string OperationVersion
        {
            get { return _operationVersion; }
            set { _operationVersion = value; }
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        public OperationSearchDialog()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.OperationSearchDialog.Title}"))
        {
            InitializeComponent();
            chkRwk.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
        }
        /// <summary>
        /// 查询按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable htParams = new Hashtable();
                DataSet dsParams = new DataSet();

                string operationName = this.txtOperationName.Text.Trim();

                if (operationName.Length > 0)
                {
                    htParams.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME, operationName);
                }

                DataTable dtParams = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(htParams);
                dtParams.TableName = POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME;
                dsParams.Tables.Add(dtParams);

                string msg = string.Empty;
                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIOperationEngine().OperationSearch(dsParams);
                msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg != string.Empty)
                {
                    MessageService.ShowError(msg);
                }
                else
                {
                    if (dsReturn.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME].Rows.Count < 1)
                        MessageService.ShowMessage("没有找到相关数据!", "系统提示");

                    grdCtrlOperation.MainView = gridViewOperation;
                    grdCtrlOperation.DataSource = dsReturn.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME];
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
        /// <summary>
        /// 双击选中项目。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewOperation_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        /// <summary>
        /// 确定按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btConfirm_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        /// <summary>
        /// 工序名称回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperationName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnQuery_Click(new object(), EventArgs.Empty);
            }
        }
        /// <summary>
        /// 取消按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 映射选择项目到属性。
        /// </summary>
        /// <returns></returns>
        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gridViewOperation.GetDataRowHandleByGroupRowHandle(gridViewOperation.FocusedRowHandle);
            if (rowHandle >= 0)
            {
                _operationKey = gridViewOperation.GetRowCellValue(rowHandle, POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY).ToString();
                _operationName = gridViewOperation.GetRowCellValue(rowHandle, POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME).ToString();
                _operationVersion = gridViewOperation.GetRowCellValue(rowHandle, POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_VERSION).ToString();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        private void OperationSearchDialog_Load(object sender, EventArgs e)
        {
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.btnConfirm.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");
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
            GridViewHelper.SetGridView(gridViewOperation);
        }

        private void gridViewOperation_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
