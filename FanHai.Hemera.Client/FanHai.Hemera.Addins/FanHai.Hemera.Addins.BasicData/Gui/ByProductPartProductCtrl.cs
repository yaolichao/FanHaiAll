//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// chao.pang            2013-0-12            add 
// =================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities.BasicData;
using FanHai.Hemera.Share.Interface;

namespace FanHai.Hemera.Addins.BasicData
{

    public partial class ByProductPartProductCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 状态改变事件委托。
        /// </summary>
        /// <param name="controlState"></param>
        public new delegate void AfterStateChanged(ControlState controlState);
        /// <summary>
        /// 状态改变事件。
        /// </summary>
        public new AfterStateChanged afterStateChanged = null;
        /// <summary>
        /// 控件状态。
        /// </summary>
        private ControlState _ctrlState = ControlState.Empty;

        /// <summary>
        /// 控件状态。
        /// </summary>
        public ControlState CtrlState
        {
            get
            {
                return _ctrlState;
            }
            set
            {
                _ctrlState = value;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        /// <summary>
        /// Control state change method
        /// </summary>
        /// <param name="state">Control state</param>
        private void OnAfterStateChanged(ControlState state)
        {
            switch (state)
            {
                #region case state of empty
                case ControlState.Empty:

                    break;
                #endregion

                #region case state of editer
                case ControlState.Edit:

                    break;
                #endregion

                #region case state of New
                case ControlState.New:

                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:

                    break;
                #endregion

                #region case state of del
                case ControlState.Delete:

                    break;
                #endregion
            }
        }


        /// <summary>
        /// 构造函数。
        /// </summary>
        public ByProductPartProductCtrl()
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
        }

        //事件
        //  关闭窗体
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
       
        //窗体载入
        private void ByProductCtrl_Load(object sender, EventArgs e)
        {

        }
    }
}
 