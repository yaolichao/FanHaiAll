using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Astronergy.MES.IVTest.BLL;

namespace Astronergy.MES.IVTest.Configuration
{
    /// <summary>
    /// IV测试配置节。
    /// </summary>
    public class IVTestConfigurationSection:ConfigurationSection
    {
        /// <summary>
        /// IV测试设备集合。
        /// </summary>
        [ConfigurationProperty("devices", IsDefaultCollection = false)]
        public DeviceElementCollection Devices
        {
            get
            {
                DeviceElementCollection collections = this["devices"] as DeviceElementCollection;
                return collections;
            }
        }

    }
    /// <summary>
    /// IV测试设备集合。
    /// </summary>
    public class DeviceElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 当在派生的类中重写时，创建一个新的 <see cref="T:System.Configuration.ConfigurationElement"/>。
        /// </summary>
        /// <returns>
        /// 新的 <see cref="T:System.Configuration.ConfigurationElement"/>。
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new DeviceElement();
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
            return ((DeviceElement)element).Name;
        }

        /// <summary>
        /// 返回指定索引的配置元素
        /// </summary>
        /// <param name="index">索引号</param>
        /// <returns>配置元素</returns>
        public DeviceElement this[int index]
        {
            get
            {
                return (DeviceElement)BaseGet(index);
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
        new public DeviceElement this[string name]
        {
            get
            {
                return (DeviceElement)BaseGet(name);

            }
        }
        /// <summary>
        /// 返回配置元素的索引号
        /// </summary>
        /// <param name="element">配置元素</param>
        /// <returns>配置元素的索引号</returns>
        public int Indexof(DeviceElement element)
        {
            return BaseIndexOf(element);
        }
        /// <summary>
        /// 添加配置元素到集合
        /// </summary>
        /// <param name="element">指定配置元素</param>
        public void Add(DeviceElement element)
        {
            BaseAdd(element);
        }
        /// <summary>
        /// 添加配置元素到集合
        /// </summary>
        /// <param name="element">指定配置元素</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }
        /// <summary>
        /// 从集合中移除指定配置元素
        /// </summary>
        /// <param name="element">指定配置元素</param>
        public void Remove(DeviceElement element)
        {
            if (BaseIndexOf(element) > 0)
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
    /// IV测试设备。
    /// </summary>
    public class DeviceElement:ConfigurationElement
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public DeviceElement()
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="name">设备代码名称。</param>
        /// <param name="path">设备数据所在文件夹路径。</param>
        public DeviceElement(string name,string path){
            this.Name = name;
            this.Path = path;
        }
        /// <summary>
        /// 设备代码名称。唯一标识设备的属性。
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
            set
            {
                this["name"] = value;
            }
        }
        /// <summary>
        /// 设备数据所在文件夹路径。
        /// </summary>
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get
            {
                return this["path"] as string;
            }
            set
            {
                this["path"] = value;
            }
        }
        /// <summary>
        /// 设备类型。
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public DeviceType Type
        {
            get
            {
                return (DeviceType)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }
        /// <summary>
        /// 设备数据所在文件的格式化字符串。
        /// </summary>
        [ConfigurationProperty("format", IsRequired = false)]
        public string Format
        {
            get
            {
                return this["format"] as string;
            }
            set
            {
                this["format"] = value;
            }
        }
    }
}
