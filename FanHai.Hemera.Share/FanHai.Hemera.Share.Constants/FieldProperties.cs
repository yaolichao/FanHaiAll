using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class FieldProperties
    {
        private Type _dataType;
        private bool _canNull;
        private object _defaultValue;

        /// <summary>
        /// 参数为空的构造函数
        /// </summary>
        public FieldProperties()
        {
            init();
        }

        /// <summary>
        /// 包含三个参数的构造函数
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <param name="canNull">是否为空</param>
        /// <param name="defaultValue">默认值</param>
        public FieldProperties(Type dataType, bool canNull, object defaultValue)
        {
            _dataType = dataType;
            _canNull = canNull;
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// 数据类型 默认string。
        /// </summary>
        public Type DATATYPE
        {
            get
            {
                return _dataType;
            }
            set
            {
                _dataType = value;
            }
        }

        /// <summary>
        /// 可否为空
        /// </summary>
        public bool CanNull
        {
            get { return _canNull; }
            set { _canNull = value; }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        /// <summary>
        /// 初始化变量
        /// </summary>
        private void init()
        {
            _dataType = typeof(string);
            _canNull = true;
            _defaultValue = DBNull.Value;
        }
    }
}
