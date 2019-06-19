using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Astronergy.MES.Tools.AutoUpdate
{
    /// <summary>
    /// 表示自动更新数据的XML文件类。
    /// </summary>
    [XmlRoot(ElementName = "autoUpdate", Namespace = "http://www.astronergy.com/")]
    public class AutoUpdateXmlFile
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public AutoUpdateXmlFile()
        {
            this.Assemblies = new List<AutoUpdateItem>();
            this.Files = new List<AutoUpdateItem>();
        }
        /// <summary>
        /// 应用程序集信息。
        /// </summary>
        [XmlArray("assemblies")]
        [XmlArrayItem("assembly")]
        public List<AutoUpdateItem> Assemblies
        {
            get;
            set;
        }
        /// <summary>
        /// 其他文件信息。
        /// </summary>
        [XmlArray("files")]
        [XmlArrayItem("file")]
        public List<AutoUpdateItem> Files
        {
            get;
            set;
        }
    }


    /// <summary>
    /// 更新项目。
    /// </summary>
    [XmlRoot("item")]
    public class AutoUpdateItem
    {
        /// <summary>
        /// 项目名称。
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// 产品名称。
        /// </summary>
        [XmlAttribute("product")]
        public string Product { get; set; }
        /// <summary>
        /// 公司名称。
        /// </summary>
        [XmlAttribute("company")]
        public string Company { get; set; }
        /// <summary>
        /// 版本号。
        /// </summary>
        [XmlAttribute("version")]
        public string Version { get; set; }
        /// <summary>
        /// 描述。
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }
        /// <summary>
        /// 最后修改时间。
        /// </summary>
        [XmlAttribute("lastestModifyTime")]
        public DateTime LastestModifyTime { get; set; }
        /// <summary>
        /// 相对路径。
        /// </summary>
        [XmlAttribute("relativePath")]
        public string RelativePath { get; set; }
        /// <summary>
        /// 文件大小。
        /// </summary>
        [XmlAttribute("size")]
        public long Size { get; set; }
    }
}
