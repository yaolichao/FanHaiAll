using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 产品认证维护表
    /// </summary>
    public class BaseCertification : TableFields
    {
        #region Fields
        /// <summary>
        /// 版本
        /// </summary>
        public const string FieldVersion = "VERSION";
        /// <summary>
        /// 创建人
        /// </summary>
        public const string FieldCreator = "CREATOR";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string FieldCreateTime = "CREATE_TIME";
        /// <summary>
        /// 修改人
        /// </summary>
        public const string FieldEditor = "EDITOR";
        /// <summary>
        /// 修改时间
        /// </summary>
        public const string FieldEditTime = "EDIT_TIME";
        /// <summary>
        /// 是否可用（Y：可用，N：不可用）
        /// </summary>
        public const string FieldIsUsed = "IS_USED";

        /// <summary>
        /// INSERT表名
        /// </summary>
        public const string TableForInsert = "BASE_CERTIFICATION_INSERT";
        /// <summary>
        /// DELETE表名
        /// </summary>
        public const string TableForDelete = "BASE_CERTIFICATION_DELETE";
        /// <summary>
        /// UPDATE表名
        /// </summary>
        public const string TableForUpdate = "BASE_CERTIFICATION_UPDATE";

        private readonly string _tableName = "BASE_CERTIFICATION";
        private readonly Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
        {
            { FieldVersion, new FieldProperties(typeof(int), false, 1) }, 
            { FieldCreator, new FieldProperties(typeof(string), false, "Unknown") }, 
            { FieldCreateTime, new FieldProperties(typeof(DateTime), false, null) }, 
            { FieldEditor, new FieldProperties() }, 
            { FieldEditTime, new FieldProperties(typeof(DateTime), true, null) },
            { FieldIsUsed, new FieldProperties(typeof(string), false, "Y") }
        };
        #endregion

        #region Properties
        /// <summary>
        /// 数据库表名称
        /// </summary>
        public override string TABLE_NAME
        {
            get { return _tableName; }
        }
        /// <summary>
        /// 数据库表字段
        /// </summary>
        public override Dictionary<string, FieldProperties> FIELDS
        {
            get { return _fields; }
        }
        #endregion
    }
}

  