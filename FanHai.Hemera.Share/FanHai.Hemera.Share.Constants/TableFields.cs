using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 公共类，用来定义数据库表中字段名称的实体类
    /// </summary>
    public class TableFields
    {
        /// <summary>
        /// 数据库表名称
        /// </summary>
        public virtual string TABLE_NAME
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 数据库表字段名称集合
        /// </summary>
        public virtual Dictionary<string, FieldProperties> FIELDS
        {
            get
            {
                return null;
            }
        }
    }
}
