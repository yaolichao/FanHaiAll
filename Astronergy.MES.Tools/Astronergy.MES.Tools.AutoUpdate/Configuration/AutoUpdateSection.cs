using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Astronergy.MES.Tools.AutoUpdate.Configuration
{
    /// <summary>
    /// 自动更新应用程序集信息的配置节。
    /// </summary>
    public class AutoUpdateSection:ConfigurationSection
    {
        /// <summary>
        /// 忽略文件集合。
        /// </summary>
        [ConfigurationProperty("ignoreFiles",IsDefaultCollection=false)]
        public IgnoreFileElementCollection IgnoreFiles
        {
            get
            {
                IgnoreFileElementCollection collections = this["ignoreFiles"] as IgnoreFileElementCollection;
                return collections;
            }
        }
       
    }
}
