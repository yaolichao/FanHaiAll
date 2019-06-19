using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 数据库操作接口。
    /// </summary>
    public interface IBasicCodeSoftLabelEngine
    {
        /// <summary>
        /// 获取CodeSoft标签参数
        /// </summary>
        /// <param name="dsProModel"></param>
        /// <returns></returns>
        DataSet GetCodeSoftLabel(string sLabelID);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="dsProModel"></param>
        /// <returns></returns>
        DataSet UpdateData(string sql, string sUpFuntionName);
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="dsProModel"></param>
        /// <returns></returns>
        DataSet AddData(string sql, string sUpFuntionName);
    }
}
