using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 数据库操作接口。
    /// </summary>
    public interface IBasicDataEngine
    {
        /// <summary>
        /// 执行SQL文本，返回SQL影响的记录数。
        /// </summary>
        /// <param name="commandText">SQL字符串。</param>
        /// <returns>SQL影响的记录行数。</returns>
        int ExecuteNonQuery(string commandText);

 
    }
}
