using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface.RFC
{
    /// <summary>
    /// 执行RFC函数的接口。
    /// </summary>
    public interface IRFCEngine
    {
        /// <summary>
        /// 执行SAP远程函数（RFC)。
        /// </summary>
        /// <param name="functionName">RFC函数名称。</param>
        /// <param name="inputData">>RFC函数输入数据。</param>
        /// <returns>RFC函数返回数据。</returns>
        DataSet ExecuteRFC(string functionName, DataSet inputData);
    }
}
