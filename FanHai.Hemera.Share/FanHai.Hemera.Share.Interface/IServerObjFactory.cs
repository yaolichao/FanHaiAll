using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface IServerObjFactory
    {
        /// <summary>
        /// 获取指定类型的对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>指定类型的对象。如果为NULL则代表没有进行注册。</returns>
        T Get<T>();
        /// <summary>
        /// 执行指定类的指定方法。
        /// </summary>
        /// <param name="engineName">指定类的名称。</param>
        /// <param name="engineMethod">指定类中的方法。</param>
        /// <param name="engineParamValue">指定方法使用的参数。</param>
        /// <returns>包含执行结果的数据及对象。</returns>
        [Obsolete("已过时，请使用IServerObjFactory.Get<T>()方法代替。")]
        DataSet ExecuteEngineMethod(string engineName, string engineMethod, DataSet engineParamValue);
    }
}
