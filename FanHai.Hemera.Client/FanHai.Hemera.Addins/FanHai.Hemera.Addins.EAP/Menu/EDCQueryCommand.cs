/***************************************************************************************************
     日期        Author           类型       Description                           标识
   ----------  --------------    ---------  ------------------------------------  ---------------                 
 * 20120802   YONGMING.QIAO      Create      查询数据采集，导出数据               Q.001 
***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Core;
using FanHai.Hemera.Addins.EAP;

namespace FanHai.Hemera.Addins.EAP
{
    public class EDCQueryCommand : AbstractMenuCommand
    {
        /// <summary>
        /// 运行菜单命令
        /// </summary>
        public override void Run()
        {
            //遍历工作台已经打开的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //如果有数据采集视图，则选中该视图显示，返回以结束该方法的运行。
                EDCQueryexpViewContent openView = viewContent as EDCQueryexpViewContent;
                if (openView != null)
                {
                    openView.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //创建新的视图对象，并显示该视图界面。
            EDCQueryexpViewContent view = new EDCQueryexpViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}
