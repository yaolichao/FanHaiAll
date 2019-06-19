using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Utils.Entities
{
    public class BaseUserDefinedAttr
    {
        public enum UDA_DATA_TYPE { INTEGER = 1, DATE = 2, DATETIME = 3, BOOLEAN = 4, STRING = 5, FLOAT = 6, SETTING = 7, LINKED = 8 }
        public BaseUserDefinedAttr(string udaKey, string udaName, string udaDataType, string udaDescription)
        {
            _udaKey = udaKey;
            _udaName = udaName;
            _udaDataType = udaDataType;
            _udaDescription = udaDescription;
        }
        public BaseUserDefinedAttr(string udaKey, string udaName, UDA_DATA_TYPE dataType, string udaDescription)
        {
            _udaKey = udaKey;
            _udaName = udaName;
            _udaDataType = Convert.ToInt32(dataType).ToString();
            _udaDescription = udaDescription;
        }
        public string Name
        {
            get
            {
                return _udaName;
            }
            set
            {
                _udaName = value;
            }
        }
        public string Key
        {
            get
            {
                return _udaKey;
            }
            set
            {
                _udaKey = value;
            }
        }
        public string DataType
        {
            get
            {
                return _udaDataType;
            }
            set
            {
                _udaDataType = value;
            }
        }
        public UDA_DATA_TYPE DataTypeEnum
        {
            get
            {
                return (UDA_DATA_TYPE)(Convert.ToInt32(_udaDataType));
            }
        }
        public string DataTypeString
        {
            get
            {
                string udaDataTypeStr = "String";
                int iUdaDataType = Convert.ToInt32(_udaDataType);
                switch ((UDA_DATA_TYPE)iUdaDataType)
                {
                    case UDA_DATA_TYPE.INTEGER:
                        udaDataTypeStr = "Integer";
                        break;
                    case UDA_DATA_TYPE.DATE:
                        udaDataTypeStr = "Date";
                        break;
                    case UDA_DATA_TYPE.DATETIME:
                        udaDataTypeStr = "DateTime";
                        break;
                    case UDA_DATA_TYPE.BOOLEAN:
                        udaDataTypeStr = "Boolean";
                        break;
                    case UDA_DATA_TYPE.FLOAT:
                        udaDataTypeStr = "Float";
                        break;
                    case UDA_DATA_TYPE.SETTING:
                        udaDataTypeStr = "Setting";
                        break;
                    case UDA_DATA_TYPE.LINKED:
                        udaDataTypeStr = "Linked";
                        break;
                    default:
                        udaDataTypeStr = "String";
                        break;
                }
                return udaDataTypeStr;
            }
        }
        public string Description
        {
            get
            {
                return _udaDescription;
            }
            set
            {
                _udaDescription = value;
            }
        }
        public string LastEditor
        {
            get
            {
                return _lastEditor;
            }
            set
            {
                _lastEditor = value;
            }
        }
        public string LastEditTime
        {
            get
            {
                return _lastEditTime;
            }
            set
            {
                _lastEditTime = value;
            }
        }
        public string LastEditTimezone
        {
            get
            {
                return _lastEditTimezone;
            }
            set
            {
                _lastEditTimezone = value;
            }
        }
        private string _udaKey = "";
        private string _udaName = "";
        private string _udaDataType = "";
        private string _udaDescription = "";
        private string _lastEditor = "";
        private string _lastEditTime = "";
        private string _lastEditTimezone = "";
    }
}
