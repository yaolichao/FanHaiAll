using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Interface;

namespace FanHai.Hemera.Modules.Common
{
    /// <summary>
    /// 数据库操作类。
    /// </summary>
    public class DBEngine : AbstractEngine, IDBEngine
    {
        private Database db = DatabaseFactory.CreateDatabase();//数据库对象。

        /// <summary>
        ///构造函数。
        /// </summary>
        public DBEngine() { }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }

        /// <summary>
        /// 执行SQL文本，返回SQL影响的记录数。
        /// </summary>
        /// <param name="commandText">SQL字符串。</param>
        /// <returns>SQL影响的记录行数。</returns>
        public int ExecuteNonQuery(string commandText)
        {
            try
            {
                return db.ExecuteNonQuery(CommandType.Text, commandText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 执行SQL文本，返回数据集对象。
        /// </summary>
        /// <param name="commandText">SQL文本。</param>
        /// <returns>数据集对象。</returns>
        public DataSet ExecuteDataSet(string commandText)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = db.ExecuteDataSet(CommandType.Text, commandText);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 执行SQL文本，返回数据读取集对象。
        /// </summary>
        /// <param name="commandText">SQL文本。</param>
        /// <returns>前向读取的数据读取集对象。</returns>
        public IDataReader ExecuteReader(string commandText)
        {
            try
            {
                return db.ExecuteReader(CommandType.Text, commandText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
