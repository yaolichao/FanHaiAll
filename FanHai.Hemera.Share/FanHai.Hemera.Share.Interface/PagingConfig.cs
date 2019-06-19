using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 数据分页配置类。
    /// </summary>
    [Obsolete]
    public class PagingConfig:MarshalByRefObject
    {
        /// <summary>
        /// 每页大小。
        /// </summary>
        public int PageSize{ get; set; }
        /// <summary>
        /// 当前页号。
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// 总页数。输出。
        /// </summary>
        public int Pages { get; set; }
        /// <summary>
        /// 总的记录数。输出。
        /// </summary>
        public int Records { get; set; }
    }

    /// <summary>
    /// 数据分页查询的配置类。
    /// </summary>
    [Serializable]
    public class PagingQueryConfig
    {
        /// <summary>
        /// 每页大小。
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页号。
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// 总页数。输出。
        /// </summary>
        public int Pages { get; set; }
        /// <summary>
        /// 总的记录数。输出。
        /// </summary>
        public int Records { get; set; }
    }
}
