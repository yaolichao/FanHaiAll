using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Astronergy.MES.Tools.UpdateAssemblyInfo.Configuration
{
    /// <summary>
    /// 更新应用程序集信息的配置节。
    /// </summary>
    /// <author>peter zd zhang</author>
    /// <mail>peter.zhang@foxmail.com</mail>
    /// <date>2011/10/13</date>
    public class UpdateAssemblyInfoSection:ConfigurationSection
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        [ConfigurationProperty("fileName", IsRequired = true)]
        public string FileName
        {
            get
            {
                return this["fileName"] as string;
            }
            set
            {
                this["fileName"] = value;
            }
        }
        /// <summary>
        /// 工作目录
        /// </summary>
        [ConfigurationProperty("workDirectory", IsRequired = true)]
        public string WorkDirectory
        {
            get { return this["workDirectory"] as string; }
            set { this["workDirectory"] = value; }
        }
        /// <summary>
        /// 源代码控制元素
        /// </summary>
        [ConfigurationProperty("sourceControl", IsRequired = true)]
        public SourceControlElement SourceControl
        {
            get { return this["sourceControl"] as SourceControlElement; }
            set { this["sourceControl"]=value;}
        }
        /// <summary>
        /// 除全局应用程序模板文件之外的模板文件集合元素，自定义的模板文件放在该配置集合内。
        /// </summary>
        [ConfigurationProperty("templateFiles",IsDefaultCollection=false)]
        public TemplateFileElementCollection TemplateFiles
        {
            get
            {
                TemplateFileElementCollection collections = this["templateFiles"] as TemplateFileElementCollection;
                return collections;
            }
        }
        /// <summary>
        /// 全局应用程序模板文件元素
        /// </summary>
        [ConfigurationProperty("globalAssemblyInfo",IsRequired=true)]
        public TemplateFileElement GlobalAssemblyInfo
        {
            get { return this["globalAssemblyInfo"] as TemplateFileElement; }
            set { this["globalAssemblyInfo"] = value; }
        }
    }
}
