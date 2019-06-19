//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-11-09            添加注释 
// =================================================================================
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


namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 工艺路径查询对话框。
    /// </summary>
    public partial class RouteSearchDialog : BaseDialog
    {
        /// <summary>
        /// 工艺路径主键。
        /// </summary>
        private string _routeKey = string.Empty;
        /// <summary>
        /// 工艺路径名称。
        /// </summary>
        private string _routeName = string.Empty;
        /// <summary>
        /// 工艺路径版本号。
        /// </summary>
        private string _routeVersion = string.Empty;
        /// <summary>
        /// 工艺路径主键。
        /// </summary>
        public string RouteKey
        {
            get { return _routeKey; }
            set { _routeKey = value; }
        }
        /// <summary>
        /// 工艺路径名称。
        /// </summary>
        public string RouteName
        {
            get { return _routeName; }
            set { _routeName = value; }
        }
        /// <summary>
        /// 工艺路径版本号。
        /// </summary>
        public string RouteVersion
        {
            get { return _routeVersion; }
            set { _routeVersion = value; }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RouteSearchDialog()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteSearchDialog.Title}"))
        {
            InitializeComponent();
        }
        /// <summary>
        /// 查询按钮。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable htParams = new Hashtable();
                DataSet dsParams = new DataSet();

                string strName = this.txtName.Text.Trim();

                if (strName.Length > 0)
                {
                    htParams.Add(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME, strName);
                }

                DataTable dtParams = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(htParams);
                dtParams.TableName = POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME;
                dsParams.Tables.Add(dtParams);

                string msg = string.Empty;
                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIRouteEngine().RouteSearch(dsParams);
                msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg != string.Empty)
                {
                    MessageService.ShowError(msg);
                }
                else
                {
                    if (dsReturn.Tables[POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME].Rows.Count < 1)
                    {
                        MessageService.ShowMessage
                             ("${res:FanHai.Hemera.Addins.FMM.SearchDialog.Message}", "${res:Global.SystemInfo}");
                    }
                    gcResult.MainView = gvResult;
                    gcResult.DataSource = dsReturn.Tables[POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME];
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
        /// 名称文本框回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnQuery_Click(new object(), EventArgs.Empty);
            }
        }
        /// <summary>
        /// 选中项目双击事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvResult_DoubleClick(object sender, EventArgs e)
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
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        /// <summary>
        /// 关闭按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 映射选择项目数据到属性中。
        /// </summary>
        /// <returns></returns>
        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gvResult.GetDataRowHandleByGroupRowHandle(gvResult.FocusedRowHandle);
            if (rowHandle >= 0)
            {
                _routeKey = gvResult.GetRowCellValue(rowHandle, POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY).ToString();
                _routeName = gvResult.GetRowCellValue(rowHandle, POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME).ToString();
                _routeVersion = gvResult.GetRowCellValue(rowHandle, POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_VERSION).ToString();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        private void RouteSearchDialog_Load(object sender, EventArgs e)
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
        }


    }
}
