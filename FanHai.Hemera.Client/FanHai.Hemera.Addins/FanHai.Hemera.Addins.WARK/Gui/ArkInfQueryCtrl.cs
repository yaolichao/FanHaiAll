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
    public partial class ArkInfQueryCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 出货操作对象。
        /// </summary>
        ShipmentOperationEntity _entity = new ShipmentOperationEntity();
        string _arkCode = string.Empty;
        string _status = string.Empty;
        string _palletNo = string.Empty;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ArkInfQueryCtrl()
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
            _arkCode = teShipmentNo.Text.Trim();
            _status = cbeStatus.Text.Trim();
            _palletNo = tePalletNo.Text.Trim();
            GroupArkEntity queryEntity = new GroupArkEntity();
            DataSet dsReturn = queryEntity.QueryInf(_arkCode, _status, _palletNo);
            DataTable dt = new DataTable();
            dt= dsReturn.Tables[0];

            if (dt.Rows.Count > 0)
            {
                if (!dt.Columns.Contains("ROWNUMBER"))
                    dt.Columns.Add("ROWNUMBER");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["ROWNUMBER"] = i + 1;
                }
                gcList.DataSource = dt;
            }
            else
                gcList.DataSource = null;
                                    
        }
        
    }
}
