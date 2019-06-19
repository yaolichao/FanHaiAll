using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Astronergy.MES.Tools.UpdateAssemblyInfo.Configuration
{
    /// <summary>
    /// 源代码控制的配置元素。
    /// </summary>
    /// <author>peter zd zhang</author>
    /// <mail>peter.zhang@foxmail.com</mail>
    /// <date>2011/10/13</date>
    public class SourceControlElement:ConfigurationElement
    {
        /// <summary>
        /// ctor
        /// </summary>
        public SourceControlElement()
        {
        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="type">源代码控制类型，当前支持svn和git。</param>
        public SourceControlElement(string type)
        {
            this.Type = type;
        }
        /// <summary>
        /// 源代码控制类型，当前支持svn和git。
        /// </summary>
        [ConfigurationProperty("type",IsRequired=true)]
        public string Type
        {
            get
            {
                return this["type"] as string;
            }
            set
            {
                this["type"] = value;
            }
        }
        /// <summary>
        /// 源代码控制应用程序安装目录。
        /// </summary>
        [ConfigurationProperty("installPath",IsRequired=false)]
        public string InstallPath
        {
            get
            {
                return this["installPath"] as string;
            }
            set
            {
                this["installPath"] = value;
            }
        }
        /// <summary>
        /// Git源代码控制的配置元素。
        /// </summary>
        [ConfigurationProperty("git",IsRequired=false)]
        public GitSourceControlElement Git
        {
            get
            {
                return this["git"] as GitSourceControlElement;
            }
        }

        /// <summary>
        /// SVN源代码控制的配置元素。
        /// </summary>
        [ConfigurationProperty("svn", IsRequired = false)]
        public SVNSourceControlElement SVN
        {
            get
            {
                return this["svn"] as SVNSourceControlElement;
            }
        }
    }
    /// <summary>
    /// SVN源代码控制的配置元素。
    /// </summary>
    /// <author>peter zd zhang</author>
    /// <mail>peter.zhang@foxmail.com</mail>
    /// <date>2011/10/13</date>
    public class SVNSourceControlElement : ConfigurationElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SVNSourceControlElement"/> class.
        /// </summary>
        public SVNSourceControlElement()
        {
        }

        /// <summary>
        /// Gets or sets the name of the branch.
        /// </summary>
        /// <value>
        /// The name of the branch.
        /// </value>
        [ConfigurationProperty("branchName",IsRequired=true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 0, MaxLength = 128)]
        public string BranchName
        {
            get { return this["branchName"] as string; }
            set { this["branchName"] = value; }
        }
        /// <summary>
        /// 获取或设置SVN用户名称。
        /// </summary>
        [ConfigurationProperty("userName", IsRequired = false)]
        public string UserName
        {
            get { return this["userName"] as string; }
            set { this["userName"] = value; }
        }
        /// <summary>
        /// 获取或设置SVN用户密码。
        /// </summary>
        [ConfigurationProperty("password", IsRequired = false)]
        public string Password
        {
            get { return this["password"] as string; }
            set { this["password"] = value; }
        }

    }
    /// <summary>
    /// Git源代码控制的配置元素。
    /// </summary>
    /// <author>peter zd zhang</author>
    /// <mail>peter.zhang@foxmail.com</mail>
    /// <date>2011/10/13</date>
    public class GitSourceControlElement : ConfigurationElement {

        private static ConfigurationPropertyCollection _Properties;

        private static ConfigurationProperty _BaseCommit = new ConfigurationProperty("baseCommit", 
            typeof(string),null,ConfigurationPropertyOptions.IsRequired);

        private static ConfigurationProperty _BaseCommitRev=new ConfigurationProperty("baseCommitRev",
            typeof(int),0,ConfigurationPropertyOptions.IsRequired);

        /// <summary>
        /// ctor
        /// </summary>
        public GitSourceControlElement()
        {
            _Properties = new ConfigurationPropertyCollection();
            _Properties.Add(_BaseCommit);
            _Properties.Add(_BaseCommitRev);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GitSourceControlElement"/> class.
        /// </summary>
        /// <param name="baseCommit">The base commit.</param>
        /// <param name="baseCommitRev">The base commit rev.</param>
        public GitSourceControlElement(string baseCommit, int baseCommitRev):this()
        {
            
        }

        /// <summary>
        /// 获取属性的集合。
        /// </summary>
        /// <returns>元素属性的 <see cref="T:System.Configuration.ConfigurationPropertyCollection"/>。</returns>
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _Properties;
            }
        }
        /// <summary>
        /// 查找最新版本用的基础SHA-1序列号
        /// </summary>
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 128)]
        public string BaseCommit
        {
            get { return this["baseCommit"] as string; }
            set
            {
                this["baseCommit"] = value;
            }
        }
        /// <summary>
        /// 生成修订版本号使用的基础版本号
        /// </summary>
        [IntegerValidator(MinValue=1,MaxValue=10000000,ExcludeRange=false)]
        public int BaseCommitRev
        {
            get
            {
                return (int)this["baseCommitRev"];
            }
            set
            {
                this["baseCommitRev"] = value;
            }
        }
    }
}
