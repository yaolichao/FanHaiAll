//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 创建人               修改时间              说明
// ---------------------------------------------------------------------------------
// chao.pang           2013-10-16             新增
// =================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using SolarViewer.Hemera.Utils.Common;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Gui.Core;
using SolarViewer.Hemera.Utils.Entities;
using SolarViewer.Gui.Framework.Gui;
using SolarViewer.Hemera.Share.Interface;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using SolarViewer.Hemera.Utils.Controls;
using SolarViewer.Hemera.Share.CommonControls.Dialogs;
using System.Collections;
using SolarViewer.Hemera.Utils.Dialogs;
using System.Linq;
using SolarViewer.Hemera.Share.Common;

namespace SolarViewer.Hemera.Addins.WARK
{
    /// <summary>
    /// 表示托盘出货查询作业的窗体类。
    /// </summary>
    public partial class HaveArkedInfQueryCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 出货操作对象。
        /// </summary>
        ShipmentOperationEntity _entity = new ShipmentOperationEntity();
        public string _teEntryNo = string.Empty;
        public string _lciStatus = string.Empty;
        /// <summary>
        /// 构造函数
        /// </summary>
        public HaveArkedInfQueryCtrl()
        {
            InitializeComponent();
        }
 
        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        private void tscSelect_Click(object sender, EventArgs e)
        {
            _teEntryNo = teEntryNo.Text.Trim();
            _lciStatus = txtArkCode.Text.Trim();
            GroupArkEntity queryEntity = new GroupArkEntity();
            DataSet dsReturn = queryEntity.QueryInfHaveArked(_teEntryNo, _lciStatus);
            DataTable dt = new DataTable();
            dt = dsReturn.Tables["AWMS_WH_ENTRY"];

            if (dt.Rows.Count > 0)
            {
                //if (!dt.Columns.Contains("ROWNUMBER"))
                //    dt.Columns.Add("ROWNUMBER");
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    dt.Rows[i]["ROWNUMBER"] = i + 1;
                //}
                gcList.DataSource = dt;
                gcList.MainView = gvList;
            }
        }

        private void gvList_MasterRowGetChildList(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventArgs e)
        {
            DataRow dr = this.gvList.GetDataRow(e.RowHandle);
            string Key = Convert.ToString(dr["ZMBLNR"]);
            GroupArkEntity queryEntity = new GroupArkEntity();
            DataSet dsReturn = queryEntity.QueryInfHaveArked(Key, _lciStatus);
            DataTable dt = new DataTable();
            dt = dsReturn.Tables["ARK_PALLET"];



            if (dt.Rows.Count > 0)
            {
                //if (!dt.Columns.Contains("ROWNUMBER"))
                //    dt.Columns.Add("ROWNUMBER");
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    dt.Rows[i]["ROWNUMBER"] = i + 1;
                //}
                e.ChildList = dt.DefaultView;
            }
        }
 
        private void gvList_MasterRowGetRelationCount(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 2;
        }

        private void gvList_MasterRowEmpty(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowEmptyEventArgs e)
        {
            e.IsEmpty = false;
        }
        /// <summary>
        /// 自定义显示查询结果中的单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvList_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "INDEX1": //设置行号
                case "INDEX2": //设置行号
                    e.DisplayText = Convert.ToString(e.RowHandle + 1);
                    break;
                default:
                    break;
            }
        }

        
    }
}
