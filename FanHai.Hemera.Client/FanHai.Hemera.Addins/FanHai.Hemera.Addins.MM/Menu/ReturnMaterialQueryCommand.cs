/***************************************************************************************************
     日期        Author           类型       Description                           标识
   ----------  --------------    ---------  ------------------------------------  ---------------
   20120627    YONGMING.QIAO     Create     增加批次退料查询菜单类.                
***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;


namespace FanHai.Hemera.Addins.MM
{
    class ReturnMaterialQueryCommand : AbstractMenuCommand
    {
        public override void Run()
        {          
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == StringParser.Parse("批次退料"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            ReturnMaterialQueryViewContent view = new ReturnMaterialQueryViewContent();
            WorkbenchSingleton.Workbench.ShowView(view);
        }
    }
}