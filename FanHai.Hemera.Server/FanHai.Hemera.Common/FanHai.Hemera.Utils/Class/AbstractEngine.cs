using System;
using System.Collections.Generic;
using System.Collections.Specialized; 
using System.Text;
using System.Configuration;

namespace FanHai.Hemera.Utils
{
    /// <summary>
    /// 抽象的引擎类。是所有远程处理引擎类的基础类。
    /// </summary>
    public abstract class AbstractEngine:MarshalByRefObject 
    {
        private NameValueCollection globalCollection = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public AbstractEngine(){}
        /// <summary>
        /// 初始化方法。
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// 定义NameValue集合:GlobalCollection
        /// </summary>
        public NameValueCollection GlobalCollection
        {
            set
            {
                globalCollection = value;
            }
            get
            {
                return globalCollection;
            }
        }
        /// <summary>
        /// 用来确保当创建 Singleton 时, 第一个实例永远不会过期
        /// </summary>
        /// <returns>null</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
