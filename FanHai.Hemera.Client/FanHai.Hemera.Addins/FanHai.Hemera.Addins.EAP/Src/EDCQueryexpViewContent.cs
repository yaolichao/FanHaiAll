//查询数据采集，导出数据

 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.EAP
{
    public class EDCQueryexpViewContent : AbstractViewContent
    {                       
        Control control = null;                                

        public override Control Control
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// 释放使用的所有资源。
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            control.Dispose();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="edcData">表示设备数据采集的数据对象。</param>
        public EDCQueryexpViewContent()
            : base()
        {
            this.TitleName = "数据采集查询";  //视图标题。
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BorderStyle = BorderStyle.FixedSingle;
            EDCQueryexpCtrl eDCQueryexpCtrl = new EDCQueryexpCtrl();
            eDCQueryexpCtrl.Dock = DockStyle.Fill;
            panel.Controls.Add(eDCQueryexpCtrl);
            this.control = panel;
        }
    }
}
