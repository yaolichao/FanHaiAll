//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-11-09            添加注释 
// =================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using System.Collections;
using FanHai.Hemera.Share.CommonControls.Dialogs;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 线别查询对话框。
    /// </summary>
    public partial class RouteLineSearchDialog : BaseDialog
    {
        public string _RouteLineNumber = "";
        public string _RouteKey = "";
        public string _strLineKey = "";
        public string _strLineName = "";

        public RouteLineSearchDialog(string strKey)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteLineSearchDialog.Title}"))
        {
            InitializeComponent();
            _RouteKey = strKey;
            BindData();
        }

        /// <summary>
        /// bind data to gridview
        /// </summary>
        private void BindData()
        {
            IServerObjFactory serverFactory = null;

            serverFactory = CallRemotingService.GetRemoteObject();

            if (serverFactory != null)
            {
                DataSet ds = new DataSet();

                ds = (DataSet)serverFactory.CreateILineManageEngine().GetLine("");
                string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(ds);
                if (msg != "")
                {
                    MessageService.ShowError(msg);
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gcLine.MainView = gvLine;
                        gcLine.DataSource = ds.Tables[0];
                    }
                }
            }
            CallRemotingService.UnregisterChannel();
        }

        /// <summary>
        /// Button of search clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearch_Click(object sender, EventArgs e)
        {
            string txtRouteLineNumber = this.txtLine.Text.Trim();
            int foundHandle = this.gvLine.LocateByDisplayText(0, this.gvLine.Columns["LINE_CODE"], txtRouteLineNumber);
            this.gvLine.FocusedRowHandle = foundHandle;
            this.gvLine.FocusedColumn = this.gvLine.Columns["LINE_CODE"];
        }


        /// <summary>
        /// gridControl gcLine Double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcLine_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            addRouteLineRelation(_RouteKey, _strLineKey, _strLineName);
        }

        /// <summary>
        /// add Route Line Relation in DB
        /// </summary>
        private void addRouteLineRelation(string strRouteKey, string strLineKey, string strLineName)
        {
            string msg = "";
            DataSet dsRouteLineGive = new DataSet();
            DataSet dsRouteLineSearch = new DataSet();
            DataSet dsRouteLineReceve = new DataSet();
            Hashtable hsRouteLine = new Hashtable();
            DataTable dtRouteLine = new DataTable();

            hsRouteLine.Add("ROUTE_ROUTE_VER_KEY", strRouteKey);
            hsRouteLine.Add("PRODUCTION_LINE_KEY", strLineKey);
            hsRouteLine.Add("LINE_NAME", strLineName);
            dtRouteLine = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hsRouteLine);
            dsRouteLineGive.Tables.Add(dtRouteLine);

            IServerObjFactory serverFactory = null;

            serverFactory = CallRemotingService.GetRemoteObject();

            if (serverFactory != null)
            {
                dsRouteLineSearch = (DataSet)serverFactory.CreateIRouteEngine().SearchRouteLine(dsRouteLineGive);
                msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsRouteLineSearch);
                if (msg != "")
                {
                    MessageService.ShowError(msg);
                }
                else
                {
                    if (dsRouteLineSearch.Tables["POR_ROUTE_LINE"].Rows.Count == 0)
                    {
                        dsRouteLineReceve = (DataSet)serverFactory.CreateIRouteEngine().AddRouteLine(dsRouteLineGive);
                        msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsRouteLineReceve);
                        if (msg != "")
                        {
                            MessageBox.Show(msg);
                        }
                    }
                    else
                    {
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.SearchLineDialog.Message}"), 
                         StringParser.Parse("${res:Global.SystemInfo}"));
                    }
                }
            }
            CallRemotingService.UnregisterChannel();
        }

        /// <summary>
        /// 显示编辑器事件。
        /// </summary>
        private void gvLine_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
        /// <summary>
        /// 窗体载入对话框。
        /// </summary>
        private void RouteLineSearchDialog_Load(object sender, EventArgs e)
        {
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.btnConfirm.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");
            this.gridColumn_LineCode.Caption = 
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.SearchDialog.gridColumn_LineCode}");
            this.gridColumn_LineName.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.SearchDialog.gridColumn_LineName}");
            this.gridColumn_Descriptions.Caption = 
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.SearchDialog.gridColumn_Descriptions}");
        }
        /// <summary>
        /// 确定按钮。
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
            addRouteLineRelation(_RouteKey, _strLineKey, _strLineName);
        }
        /// <summary>
        /// 取消按钮。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 映射选择项目数据到属性。
        /// </summary>
        /// <returns></returns>
        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gvLine.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                _RouteLineNumber = this.gvLine.GetRowCellValue(rowHandle, "LINE_CODE").ToString();
                _strLineName = this.gvLine.GetRowCellValue(rowHandle, "LINE_NAME").ToString();
                _strLineKey = this.gvLine.GetRowCellValue(rowHandle, "PRODUCTION_LINE_KEY").ToString();
                return true;
            }
            return false;
        }
    }
}
