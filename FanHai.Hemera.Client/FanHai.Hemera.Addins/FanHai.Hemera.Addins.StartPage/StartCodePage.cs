using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;

using FanHai.Gui.Core;
using FanHai.Gui.Framework;

namespace FanHai.Hemera.Addins.StartPage
{
	public enum ColorScheme
	{
		blue,
		red,
		green,
		brown,
		orange
	}
	
	public class MenuItem
	{
		public string Caption, URL;

		public MenuItem(string strCaption, string strUrl)
		{
			Caption = strCaption;
			URL = strUrl;
		}
	}
	
	public class StartCodePage
	{
		ColorScheme _ColorScheme;
		
		string startPageLocation;
		
		string m_strMainColColor, m_strSubColColor;

#pragma warning disable
        int m_nLeftTopImageWidth, m_nRightTopImageWidth;

		bool m_bShowMilestoneContentImage;
		
		string m_strTitle, m_strMetaDescription, m_strMetaKeywords, m_strMetaAuthor, m_strMetaCopyright;
		string m_strStaticStyleSheet, m_strRightBoxHtml;

		bool m_bShowLeftMenu, m_bShowRightBox, m_bShowContentBar;
		string m_strContentBarText, m_strTopMenuSelectedItem, m_strLeftMenuSelectedItem;
		string m_strVersionText, m_strVersionStatus;

		public string PrimaryColor
		{
			get { return m_strMainColColor; }
		}

		public string SecondaryColor
		{
			get { return m_strSubColColor; }
		}

		public string Title
		{
			get { return m_strTitle; }
			set { m_strTitle = value; }
		}

		public bool ShowMilestoneContentImage
		{
			get { return m_bShowMilestoneContentImage; }
			set { m_bShowMilestoneContentImage = value; }
		}
		
		public string MetaDescription
		{
			get { return m_strMetaDescription; }
			set { m_strMetaDescription = value; }
		}

		public string MetaKeywords
		{
			get { return m_strMetaKeywords; }
			set { m_strMetaKeywords = value; }
		}

		public string MetaAuthor
		{
			get { return m_strMetaAuthor; }
			set { m_strMetaAuthor = value; }
		}

		public string MetaCopyright
		{
			get { return m_strMetaCopyright; }
			set { m_strMetaCopyright = value; }
		}

		public string StaticStyleSheet
		{
			get { return m_strStaticStyleSheet; }
			set { m_strStaticStyleSheet = value; }
		}

		public string ContentBarText
		{
			get { return m_strContentBarText; }
			set { m_strContentBarText = value; }
		}

		public bool ShowLeftMenu
		{
			get { return m_bShowLeftMenu; }
			set { m_bShowLeftMenu = value; }
		}

		public bool ShowRightBox
		{
			get { return m_bShowRightBox; }
			set { m_bShowRightBox = value; }
		}

		public bool ShowContentBar
		{
			get { return m_bShowContentBar; }
			set { m_bShowContentBar = value; }
		}
		
		private List<MenuItem> TopMenu;

		public virtual void PopulateTopMenu()
		{
			TopMenu.Add(new MenuItem(StringParser.Parse("欢迎使用FanHai Hemera系统"),      ""));
		}
		
		public virtual void PopulateLeftMenu()
		{
		}
		
		public string TopMenuSelectedItem
		{
			get { return m_strTopMenuSelectedItem; }
			set { m_strTopMenuSelectedItem = value; }
		}

		public string LeftMenuSelectedItem
		{
			get { return m_strLeftMenuSelectedItem; }
			set { m_strLeftMenuSelectedItem = value; }
		}

		public string VersionText
		{
			get { return m_strVersionText; }
			set { m_strVersionText = value; }
		}

		public string VersionStatus
		{
			get { return m_strVersionStatus; }
			set { m_strVersionStatus = value; }
		}

		public string RightBoxHtml
		{
			get { return m_strRightBoxHtml; }
			set { m_strRightBoxHtml = value; }
		}

		public virtual void RenderRightBoxHtml(StringBuilder builder)
		{
			builder.Append(m_strRightBoxHtml);
		}

        public StartCodePage()
		{
			ColorScheme = StartPage.ColorScheme.blue;
			
			TopMenu = new List<MenuItem>();
            PopulateTopMenu();

			ShowLeftMenu = false;
            ShowRightBox = false;
            ShowContentBar = false;
            ShowMilestoneContentImage = false;
		}
		
		public ColorScheme ColorScheme
		{
			get { return _ColorScheme; }
			set
			{
				_ColorScheme = value;
				m_bShowMilestoneContentImage = false;

				switch (_ColorScheme)
				{
					case ColorScheme.blue:
						m_nLeftTopImageWidth = 292;//412;
						m_nRightTopImageWidth = 363;
						m_strSubColColor =  "#C2E0FB";
						m_strMainColColor = "#A8C6E3";
						m_bShowMilestoneContentImage = true;
						break;
					case ColorScheme.red:
						m_nLeftTopImageWidth = 214;//334;
						m_nRightTopImageWidth = 438;
						m_strSubColColor =  "#a7a9ac";
						m_strMainColColor = "#d7797d";
						break;
					case ColorScheme.brown:
						m_nLeftTopImageWidth = 294;//415;
						m_nRightTopImageWidth = 359;
						m_strSubColColor =  "#EEE9E2";
						m_strMainColColor = "#D5D0C9";
						break;
					case ColorScheme.green:
						m_nLeftTopImageWidth = 259;//450;
						m_nRightTopImageWidth = 325;
						m_strSubColColor =  "#E7EDBB";
						m_strMainColColor = "#CED4A2";
						break;
					case ColorScheme.orange:
						m_nLeftTopImageWidth = 191;//311;
						m_nRightTopImageWidth = 460;
						m_strSubColColor =  "#F4D97B";
						m_strMainColColor = "#E7CD6F";
						break;
				}
			}
		}

		public virtual void RenderHeaderSection(StringBuilder builder)
		{
            builder.Append("<html><head><title>");
            builder.Append(Title);
            builder.Append("</title>\r\n");
            builder.Append("<link rel='stylesheet' type='text/css' href='");
            builder.Append(PropertyService.DataDirectory + Path.DirectorySeparatorChar +
                           "resources" + Path.DirectorySeparatorChar +
                           "startpage" + Path.DirectorySeparatorChar +
                           "Layout" + Path.DirectorySeparatorChar +
                           "default.css");
            builder.Append("'>\r\n");
            builder.Append("<META HTTP-EQUIV=\"content-type: text/html; charset= ISO-8859-1\">\r\n");
            builder.Append("<META NAME=\"robots\" CONTENT=\"FOLLOW,INDEX\">\r\n");
            builder.Append("<meta name=\"Author\" content=\"");
            builder.Append(MetaAuthor);
            builder.Append("\">\r\n<META NAME=\"copyright\" CONTENT=\"");
            builder.Append(MetaCopyright);
            builder.Append("\">\r\n<meta http-equiv=\"Description\" name=\"Description\" content=\"");
            builder.Append(MetaDescription);
            builder.Append("\">\r\n<meta http-equiv=\"Keywords\" name=\"Keywords\" content=\"");
            builder.Append(MetaKeywords);
            builder.Append("\">\r\n<link rel=\"stylesheet\" href=\"");
            builder.Append(StaticStyleSheet);
            builder.Append("\"></head>\r\n<body bgcolor=\"#ffffff\"> ");
		}

		public virtual void RenderPageEndSection(StringBuilder builder)
		{
          
            builder.Append("</body></html>");
		}

		public virtual void RenderPageTopSection(StringBuilder builder)
		{
            builder.Append("<table height=500 border=0 cellspacing=0 cellpadding=0 >");
            builder.Append("<tr><td ><font size=3 color=green>欢迎使用FanHai Hemera系统</font>\r\n<img src=\"" + startPageLocation + "\\Layout\\common\\firstPage.bmp" + " \"");
            builder.Append(" /></td></TR>");
            builder.Append("<tr height=10><td><font size=2 color=green>本版本中提供了基础数据设置、订单管理、工单管理、Part管理、流程管理、工厂建模、Code管理、在线管理等模块......</font></td></tr>");
            builder.Append("<tr height=10><td><font size=2 color=green>当前版本：");
            builder.Append(RevisionClass.FullVersion);
            builder.Append(" 测试试用版本，谢谢使用</font></td></tr>");
            builder.Append("<tr height=10><td><font size=2 color=green>更新时间：2010-06-25 13:40:40</font></td></tr>");
            builder.Append("</table>");
		}

		public virtual void RenderLeftMenu(StringBuilder builder)
		{
			
		}

		public virtual void RenderFirstPageBodySection(StringBuilder builder)
		{
  

			
		}

		public virtual void RenderFinalPageBodySection(StringBuilder builder)
		{
           
		}

		public virtual void RenderRightBox(StringBuilder builder)
		{
			
		}

        internal string[] projectFiles = null;
		
		public void RenderSectionStartBody(StringBuilder builder)
		{
            StringBuilder projectSection = builder;
            projectSection.Append("<DIV class='tablediv'><TABLE CLASS='dtTABLE' CELLSPACING='0'>\n");
            projectSection.Append(String.Format("<TR><TH>{0}</TH><TH>{1}</TH><TH>{2}</TH></TR>\n",
                                                StringParser.Parse("${res:Global.Name}"),
                                                StringParser.Parse("${res:StartPage.StartMenu.ModifiedTable}"),
                                                StringParser.Parse("${res:StartPage.StartMenu.LocationTable}")
                                               ));
			
			try {
				 //Get the recent projects
                //projectFiles = new string[FileService.RecentOpen.RecentProject.Count];
                //for (int i = 0; i < FileService.RecentOpen.RecentProject.Count; ++i) {
                //    string fileName = FileService.RecentOpen.RecentProject[i].ToString();
                //    // if the file does not exist, goto next one
                //    if (!System.IO.File.Exists(fileName)) {
                //        continue;
                //    }
                //    projectFiles[i] = fileName;
                //    projectSection.Append("<TR><TD>");
                //    projectSection.Append("<a href=\"startpage://project/" + i + "\">");
                //    projectSection.Append(Path.GetFileNameWithoutExtension(fileName));
                //    projectSection.Append("</A>");
                //    projectSection.Append("</TD><TD>");
                //    System.IO.FileInfo fInfo = new System.IO.FileInfo(fileName);
                //    projectSection.Append(fInfo.LastWriteTime.ToShortDateString());
                //    projectSection.Append("</TD><TD>");
                //    projectSection.Append(fileName);
                //    projectSection.Append("</TD></TR>\n");
                //}
			} catch {}
            //projectSection.Append("</TABLE></DIV><BR/><BR/>");
            //projectSection.Append(String.Format("<button id=\"opencombine\">{0}</button>\n",
            //                                    StringParser.Parse("${res:StartPage.StartMenu.OpenCombineButton}")
            //                                   ));
            //projectSection.Append(String.Format("<button id=\"newcombine\">{0}</button>\n",
            //                                    StringParser.Parse("${res:StartPage.StartMenu.NewCombineButton}")
            //                                   ));
            //projectSection.Append("<BR/><BR/><BR/>");
		}
		
		public void RenderSectionAuthorBody(StringBuilder builder)
		{
			try {
                builder.Append("<iframe name=\"iframe\" src=\"http://wiki.sharpdevelop.net/Contributors.ashx\" width=\"100%\" height=\"1400\" />");



                //string html = ConvertXml.ConvertToString(Application.StartupPath +
                //                   Path.DirectorySeparatorChar + ".." +
                //                   Path.DirectorySeparatorChar + "doc" +
                //                   Path.DirectorySeparatorChar + "AUTHORS.xml",

                //                   PropertyService.DataDirectory +
                //                   Path.DirectorySeparatorChar + "ConversionStyleSheets" +
                //                   Path.DirectorySeparatorChar + "ShowAuthors.xsl");
                //builder.Append(html);
			} catch (Exception e) {
				MessageBox.Show(e.ToString());
			}
		}

		static string changeLogHtml;
		
		public void RenderSectionChangeLogBody(StringBuilder builder)
		{
			try {
                if (changeLogHtml == null)
                {
                    XslCompiledTransform transform = new XslCompiledTransform();
                    transform.Load(Path.Combine(PropertyService.DataDirectory, "ConversionStyleSheets/ShowChangeLog.xsl"));
                    StringWriter writer = new StringWriter();
                    XmlTextWriter xmlWriter = new XmlTextWriter(writer);
                    xmlWriter.Formatting = Formatting.None;
                    transform.Transform(Path.Combine(FileUtility.ApplicationRootPath, "doc/ChangeLog.xml"), xmlWriter);
                    changeLogHtml = writer.ToString().Replace("\n", "\n<br>");
                }
                builder.Append(changeLogHtml);
			} catch (Exception e) {
				MessageBox.Show(e.ToString());
			}
		}
		
		public void RenderSectionHelpWantedBody(StringBuilder builder)
		{
			try {
                builder.Append("<iframe name=\"iframe\" src=\"http://wiki.sharpdevelop.net/FeaturesWeSolicitHelpFor.ashx\"  width=\"100%\" height=\"1000\" />");
//
//				string html = ConvertXml.ConvertToString(Application.StartupPath +
//				                   Path.DirectorySeparatorChar + ".." +
//				                   Path.DirectorySeparatorChar + "doc" +
//				                   Path.DirectorySeparatorChar + "HowYouCanHelp.xml",
//
//				                   Application.StartupPath +
//				                   Path.DirectorySeparatorChar + ".." +
//				                   Path.DirectorySeparatorChar + "data" +
//				                   Path.DirectorySeparatorChar + "ConversionStyleSheets" +
//				                   Path.DirectorySeparatorChar + "ShowHowYouCanHelp.xsl");
//				builder.Append(html);
			} catch (Exception e) {
				MessageBox.Show(e.ToString());
			}
		}
		
		public string Render(string section)
		{
			startPageLocation = FileUtility.Combine(Application.StartupPath, "", "data", "resources", "startpage");
			switch (section.ToLowerInvariant()) {
				case "start":
					
					break;
				case "changelog":
					ContentBarText = StringParser.Parse("${res:StartPage.ChangeLogMenu.BarNameName}");
					break;
				case "authors":
					ContentBarText = StringParser.Parse("${res:StartPage.AuthorsMenu.BarNameName}");
					break;
				case "helpwanted":
					ContentBarText = StringParser.Parse("${res:StartPage.HelpWantedMenu.BarNameName}");
					break;
			}
			
			StringBuilder builder = new StringBuilder(2048);
			RenderHeaderSection(builder);
			RenderPageTopSection(builder);
			RenderFirstPageBodySection(builder);
			
			switch (section.ToLowerInvariant()) {
				case "start":
					
					break;
				case "changelog":
					RenderSectionChangeLogBody(builder);
					break;
				case "authors":
					RenderSectionAuthorBody(builder);
					break;
				case "helpwanted":
					RenderSectionHelpWantedBody(builder);
					break;
			}
			
			RenderFinalPageBodySection(builder);
			RenderPageEndSection(builder);
			
			return builder.ToString();
		}
	}
}
