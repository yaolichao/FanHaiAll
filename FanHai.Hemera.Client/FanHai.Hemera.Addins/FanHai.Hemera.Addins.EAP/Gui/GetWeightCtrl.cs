//----------------------------------------------------------------------------------
// Copyright (c) SolarViewer
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// E-Mail:  peter.zhang@foxmail.com
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-02-27            添加注释 
// =================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SolarViewer.Hemera.Utils.Weight;
using SolarViewer.Gui.Core;

namespace SolarViewer.Hemera.Addins.EAP
{
    /// <summary>
    /// 用于自动采集称重数据的文本控件。
    /// </summary>
    /// comment by peter 2012-2-27
    public partial class GetWeightCtrl :TextBox 
    {       
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// comment by peter 2012-2-27
        public GetWeightCtrl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 控件单击事件。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        /// comment by peter 2012-2-27
        private void GetWeightCtrl_Click(object sender, EventArgs e)
        {
            try
            {
                //获取称重值。
                PortUtility portUtil = new PortUtility();
                portUtil.Start();
                portUtil.ParserValue();
                this.Text = portUtil.WeighValue;
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }           
        }
    }
}
