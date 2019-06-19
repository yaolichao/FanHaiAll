using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Astronergy.MES.Tools.UpdateAssemblyInfo.Configuration
{
    /// <summary>
    /// 模板文件集合的配置元素。
    /// </summary>
    /// <author>peter zd zhang</author>
    /// <mail>peter.zhang@foxmail.com</mail>
    /// <date>2011/10/13</date>
    public class TemplateFileElementCollection : ConfigurationElementCollection
    {

        /// <summary>
        /// 当在派生的类中重写时，创建一个新的 <see cref="T:System.Configuration.ConfigurationElement"/>。
        /// </summary>
        /// <returns>
        /// 新的 <see cref="T:System.Configuration.ConfigurationElement"/>。
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new TemplateFileElement();
        }

        /// <summary>
        /// 在派生类中重写时获取指定配置元素的元素键。
        /// </summary>
        /// <param name="element">要为其返回键的 <see cref="T:System.Configuration.ConfigurationElement"/>。</param>
        /// <returns>
        /// 一个 <see cref="T:System.Object"/>，用作指定 <see cref="T:System.Configuration.ConfigurationElement"/> 的键。
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TemplateFileElement)element).Name;
        }

        /// <summary>
        /// 返回指定索引的配置元素
        /// </summary>
        /// <param name="index">索引号</param>
        /// <returns>配置元素</returns>
        public TemplateFileElement this[int index]{
            get{
                return (TemplateFileElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// 返回具有指定键的配置元素
        /// </summary>
        /// <param name="name">配置元素名称</param>
        /// <returns>配置元素</returns>
        new public TemplateFileElement this[string name]
        {
            get
            {
                return (TemplateFileElement)BaseGet(name);
               
            }
        }
        /// <summary>
        /// 返回配置元素的索引号
        /// </summary>
        /// <param name="element">配置元素</param>
        /// <returns>配置元素的索引号</returns>
        public int Indexof(TemplateFileElement element)
        {
            return BaseIndexOf(element);
        }
        /// <summary>
        /// 添加配置元素到集合
        /// </summary>
        /// <param name="element">指定配置元素</param>
        public void Add(TemplateFileElement element)
        {
            BaseAdd(element);
        }
        /// <summary>
        /// 添加配置元素到集合
        /// </summary>
        /// <param name="element">指定配置元素</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element,false);
        }
        /// <summary>
        /// 从集合中移除指定配置元素
        /// </summary>
        /// <param name="element">指定配置元素</param>
        public void Remove(TemplateFileElement element)
        {
            if(BaseIndexOf(element)>0)
                BaseRemove(element.Name);
        }
        /// <summary>
        /// 从集合中移除指定配置元素
        /// </summary>
        /// <param name="index">索引号</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
        /// <summary>
        /// 从集合中移除指定配置元素
        /// </summary>
        /// <param name="name">配置元素名</param>
        public void Remove(string name)
        {
            BaseRemove(name);
        }
        /// <summary>
        /// 从集合中移除所有配置元素对象。
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }
    }
    /// <summary>
    /// 模板文件的配置元素。
    /// </summary>
    /// <author>peter zd zhang</author>
    /// <mail>peter.zhang@foxmail.com</mail>
    /// <date>2011/10/13</date>
    public class TemplateFileElement:ConfigurationElement
    {
        /// <summary>
        /// ctor
        /// </summary>
        public TemplateFileElement()
        {
        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <param name="input">输入模板文件</param>
        /// <param name="output">输出文件名</param>
        public TemplateFileElement(string name,string input, string output)
        {
            this.Name = name;
            this.Input = input;
            this.Output = output;
        }
        /// <summary>
        /// 配置名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name{
            get
            {
                return this["name"] as string;
            }
            set
            {
                this["input"] = value;
            }
        }
        /// <summary>
        /// 输入模板文件
        /// </summary>
        [ConfigurationProperty("input",IsRequired=true)]
        public string Input
        {
            get
            {
                return this["input"] as string;
            }
            set
            {
                this["input"] = value;
            }
        }
        /// <summary>
        /// 输出文件名
        /// </summary>
        [ConfigurationProperty("output", IsRequired = true)]
        public string Output
        {
            get
            {
                return this["output"] as string;
            }
            set
            {
                this["output"] = value;
            }
        }
        /// <summary>
        /// 配置是否有效
        /// </summary>
        [ConfigurationProperty("isValided",DefaultValue=true)]
        public bool IsValided
        {
            get
            {
                return (bool)this["isValided"];
            }
            set
            {
                this["isValided"] = value;
            }
        }
    }
}
