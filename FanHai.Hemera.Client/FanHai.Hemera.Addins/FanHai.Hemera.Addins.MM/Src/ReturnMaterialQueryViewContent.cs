/***************************************************************************************************
     日期        Author           类型       Description                           标识
   ----------  --------------    ---------  ------------------------------------  ---------------
   20120627    YONGMING.QIAO     Create     增加批次退料查询视图类.                
***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.MM
{
    public class ReturnMaterialQueryViewContent: AbstractViewContent
    {    
        //定义变量
        Control control = null;
        //重写属性
        public override Control Control
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            control.Dispose();
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        public ReturnMaterialQueryViewContent()
            : base()
        {;
            //this.TitleName = "批次退料";
            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialQueryCtrl.name}");
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;

            ReturnMaterialQueryCtrl returnMaterialQueryCtrl = new ReturnMaterialQueryCtrl();
            returnMaterialQueryCtrl.Dock = DockStyle.Fill;

            panel.Controls.Add(returnMaterialQueryCtrl);

            this.control = panel;
        }
    }
}
