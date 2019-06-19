using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Astronergy.MES.Report.DataAccess
{
    /// <summary>
    /// 基础的数据库访问类。
    /// </summary>
    public class BaseDBAccess
    {
        /// <summary>
        /// 数据库对象。
        /// </summary>
        protected Database _db;
        protected Database _dbNow;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public BaseDBAccess()
        {
            this._db = DatabaseFactory.CreateDatabase();
            this._dbNow = DatabaseFactory.CreateDatabase("MESDB");
        }
    }
}
