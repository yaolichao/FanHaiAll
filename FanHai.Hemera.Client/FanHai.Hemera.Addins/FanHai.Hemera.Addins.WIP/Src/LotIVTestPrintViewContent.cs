using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// IV测试数据铭牌标签打印类
    /// </summary>
    public class LotIVTestPrintViewContent : AbstractViewContent
    {
         //define control
        Control control = null;

        /// <summary>
        /// Control
        /// </summary>
        public override Control Control
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// IsViewOnly
        /// </summary>
        public override bool IsViewOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            control.Dispose();
            control = null;
        }


        public LotIVTestPrintViewContent()
            : base()
        {


            this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintViewContent.Title01}");//"终测IV数据";
           
           
            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            LotIVTestPrint lotIvTestPrint = new LotIVTestPrint();
            lotIvTestPrint.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(lotIvTestPrint);
            //set panel to view content
            this.control = panel;
           
        }
    }




    public class LotIVTestPrintViewContent2 : AbstractViewContent
    {
        //define control
        Control control = null;

        /// <summary>
        /// Control
        /// </summary>
        public override Control Control
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// IsViewOnly
        /// </summary>
        public override bool IsViewOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            control.Dispose();
            control = null;
        }


        public LotIVTestPrintViewContent2()
            : base()
        {


            this.TitleName = "IVTEST打印";


            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            //LotIVTestPrintDialog4 lotIvTestPrint = new LotIVTestPrintDialog4();
            LotIVTestPrint lotIvTestPrint = new LotIVTestPrint();
            lotIvTestPrint.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(lotIvTestPrint);
            //set panel to view content
            this.control = panel;

        }
    }
    /// <summary>
    /// IV测试数据铭牌标签打印类
    /// </summary>
    public class LotCustCheckQueryViewContent : AbstractViewContent
    {
        //define control
        Control control = null;

        /// <summary>
        /// Control
        /// </summary>
        public override Control Control
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// IsViewOnly
        /// </summary>
        public override bool IsViewOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            control.Dispose();
        }


        public LotCustCheckQueryViewContent()
            : base()
        {


            this.TitleName = "终检数据查询";


            //define panel 
            Panel panel = new Panel();

            //set panel dock style
            panel.Dock = DockStyle.Fill;
            //set panel BorderStyle
            panel.BorderStyle = BorderStyle.FixedSingle;

            LotQueryCustCheck lotIvTestPrint = new LotQueryCustCheck();
            lotIvTestPrint.Dock = DockStyle.Fill;
            //add control to panle
            panel.Controls.Add(lotIvTestPrint);
            //set panel to view content
            this.control = panel;

        }
    }
}
