using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Utils;
using System.Data;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Share.Interface.RFC;

namespace FanHai.Hemera.Modules.Common
{
    /// <summary>
    /// 执行RFC函数的具体类。
    /// </summary>
    public class RFCEngine : AbstractEngine, IRFCEngine
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize(){}
        /// <summary>
        /// 执行SAP远程函数（RFC)。
        /// </summary>
        /// <param name="functionName">RFC函数名称。</param>
        /// <param name="inputData">>RFC函数输入数据。</param>
        /// <returns>RFC函数返回数据。</returns>
        public DataSet ExecuteRFC(string functionName, DataSet inputData)
        {
            DateTime startTime =DateTime.Now;
            DataSet outputData = null;

            try
            {
                AllCommonFunctions.SAPRemoteFunctionCall(functionName, inputData, out outputData);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(outputData, ex.Message);
                LogService.LogError("Execute RFC Error: " + ex.Message);
            }

            DateTime endTime = DateTime.Now;
            LogService.LogInfo("Cal SAP Function: " +functionName+" Total Time:"+(endTime - startTime).TotalMilliseconds.ToString());
            return outputData;
        }
    }
}
