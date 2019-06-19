using System;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework.BrowserDisplayBinding;
using FanHai.Gui.Framework;
using System.Xml;
using FanHai.Hemera.Utils.Common;
using System.Data;


namespace FanHai.Hemera.Addins.StartPage
{
    /// <summary>
    /// 用于获取URL的帮助类。
    /// </summary>
    internal class UrlHelper{
        /// <summary>
        /// 根据名称获取URL地址。
        /// </summary>
        /// <param name="name">URL名称。</param>
        /// <returns>URL地址。</returns>
        public static string GetUrl(string name) {
            string url = "about:blank";
            string[] columns = new string[] { "URL" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "URL");
            KeyValuePair<string, string> condition = new KeyValuePair<string, string>("NAME", name);
            List<KeyValuePair<string, string>> lstWhere = new List<KeyValuePair<string, string>>();
            lstWhere.Add(condition);
            DataTable dt = BaseData.GetBasicDataByCondition(columns, category, lstWhere);
            if (dt.Rows.Count > 0)
            {
                url = Convert.ToString(dt.Rows[0]["URL"]) + "?id=" + PropertyService.Get("USER_NAME");
            }
            if (string.IsNullOrEmpty(url))
            {
                url = "about:blank";
            }
            return url;
        }
    }
    /// <summary>
    /// 显示网页的命令类。
    /// </summary>
	public abstract class ShowPageCommand : AbstractMenuCommand
	{
        string _url = "about:blank";
        string _titleName = string.Empty;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="url"></param>
        protected ShowPageCommand(string url,string titleName)
        {
            _url = url;
            _titleName = titleName;
        }
        /// <summary>
        /// 执行显示网页的命令。
        /// </summary>
		public override void Run()
		{
            try
            {
                foreach (IViewContent view in WorkbenchSingleton.Workbench.ViewContentCollection)
                {
                    if (view.TitleName == _titleName)
                    {
                        view.WorkbenchWindow.SelectWindow();
                        return;
                    }
                }
                
                BrowserPane pane=new BrowserPane(false);
                pane.TitleName = _titleName;
                pane.Navigate(_url);
                pane.TitleNameChanged += new EventHandler((o,e) =>
                {
                    _titleName = pane.TitleName;
                });
                WorkbenchSingleton.Workbench.ShowView(pane);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
		}
	}
    /// <summary>
    /// 显示起始页的命令类。
    /// </summary>
    public class ShowStartPageCommand : ShowPageCommand
    {
        public ShowStartPageCommand()
            : base(UrlHelper.GetUrl("STARTPAGE"),"起始页")
        {
        }
    }
    /// <summary>
    /// 显示报表页的命令类。
    /// </summary>
    public class ShowReportPageCommand : ShowPageCommand
    {
        public ShowReportPageCommand()
            : base(UrlHelper.GetUrl("REPORT"), "报表模块")
        {
        }
    }
    /// <summary>
    /// 显示帮助页的命令类。
    /// </summary>
    public class ShowHelpPageCommand : ShowPageCommand
    {
        public ShowHelpPageCommand()
            : base(UrlHelper.GetUrl("HELP"), "升级日志")
        {
        }
    }

    public struct STARTPAGE_CONST
    {
        public const string URL = "about:blank";
    }
}
