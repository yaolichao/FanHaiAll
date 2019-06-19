using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 用户自定义属性实体类。
    /// </summary>
    public class UserDefinedAttrs
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public UserDefinedAttrs()
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="linkedToItemColumnName">唯一标识用户自定义属性关联项的数据列名。</param>
        public UserDefinedAttrs(string linkedToItemColumnName)
        {
            _linkedToItemColumnName = linkedToItemColumnName;
        }

        public UserDefinedAttrs(DataTable table)
        {
            if (null == table || table.Columns.Count != 7)
            {
                throw (new Exception("UserDefinedAttrs::UserDefinedAttrs(DataTable)"));
            }
            foreach (DataRow dataRow in table.Rows)
            {
                string linkedItemKey        = dataRow[table.Columns[COLUMN_LINKED_ITEM_KEY]].ToString();
                string udaKey               = dataRow[table.Columns[COLUMN_ATTRIBUTE_KEY]].ToString();
                string udaName              = dataRow[table.Columns[COLUMN_ATTRIBUTE_NAME]].ToString();
                string udaValue             = dataRow[table.Columns[COLUMN_ATTRIBUTE_VALUE]].ToString();
                string udaLastUpdateTime    = dataRow[table.Columns[COLUMN_LAST_UPDATE_TIME]].ToString();
                string udaDataType          = dataRow[table.Columns[COLUMN_ATTRIBUTE_DATE_TYPE]].ToString();
                UserDefinedAttr uda = new UserDefinedAttr(linkedItemKey, udaKey, udaName, udaValue, "");
                uda.LastEditTime = udaLastUpdateTime;
                uda.DataType = udaDataType;
                uda.OperationAction = OperationAction.Update;
                _udaList.Add(uda);
            }
        }

        public bool UserDefinedAttrAdd(UserDefinedAttr uda)
        {
            foreach (UserDefinedAttr itemUDA in _udaList)
            {
                if (itemUDA.Key == uda.Key)
                {
                    if (OperationAction.Delete == itemUDA.OperationAction)
                    {
                        itemUDA.OperationAction = OperationAction.Modified;
                        itemUDA.Value = "";
                        return true;
                    }
                    return false;
                }
            }
            if (OperationAction.None == uda.OperationAction)
            {
                uda.OperationAction = OperationAction.New;
            }
            _udaList.Add(uda);
            AddToInitDictionary(uda.Key, uda.Value);
            return true;
        }

        public bool UserDefinedAttrUpdate(string key, string value)
        {
            foreach (UserDefinedAttr uda in _udaList)
            {
                if(OperationAction.None == uda.OperationAction)
                {
                    return false;
                }
           
                if (uda.Key == key)
                {
                    uda.Value = value;
                    if (OperationAction.Update == uda.OperationAction || 
                        OperationAction.Delete == uda.OperationAction
                        )
                    {
                        uda.OperationAction = OperationAction.Modified;
                    }
                    else if (OperationAction.Modified == uda.OperationAction)
                    {
                        // equal to old value
                        if (_initDataDict.ContainsKey(key) && _initDataDict[key] == value)
                        {
                            uda.OperationAction = OperationAction.Update;
                        }
                    }
                    return true;
                }
                
            }
            return false;
        }
        /// <summary>
        /// 移除列表行，移除成功返回值为true，否则为false
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool UserDefinedAttrDelete(string key)
        {
            foreach (UserDefinedAttr uda in _udaList)
            {
                if (uda.Key == key)
                {
                    if (OperationAction.New == uda.OperationAction)
                    {
                        _udaList.Remove(uda);  //移除列表的选中行
                        return true;
                    }
                    uda.OperationAction = OperationAction.Delete;
                    return true;
                }
            }
            return false;
        }
        public bool UserDefinedAttrDelete(UserDefinedAttr uda)
        {
            return UserDefinedAttrDelete(uda.Key);
        }

        public void UserDefinedAttrChangeKey(string newKey)
        {
            foreach (UserDefinedAttr uda in _udaList)
            {
                uda.LinkToItemKey = newKey;
                uda.OperationAction = OperationAction.New;
                uda.LastEditor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            }
        }
        public string GetUserDefinedAttrValueByAttrKey(string attributeKey)
        {
            foreach (UserDefinedAttr uda in _udaList)
            {
                if (uda.Key == attributeKey)
                {
                    return uda.Value;
                }
            }
            return "";
        }
        public List<UserDefinedAttr> UserDefinedAttrList
        {
            get
            {
                return _udaList;
            }
        }

        public string LinkedToItemColumnName
        {
            get
            {
                return _linkedToItemColumnName;
            }
            set
            {
                _linkedToItemColumnName = value;
            }
        }
        

        public bool Contains(string linkedToItemKey, string udaKey)
        {
            foreach (UserDefinedAttr uda in _udaList)
            {
                if (uda.Key == udaKey && uda.LinkToItemKey == linkedToItemKey)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEqualTo(string linkedToItemKey, string udaKey, string udaValue)
        {
            foreach (UserDefinedAttr uda in _udaList)
            {
                if (uda.Key == udaKey && uda.LinkToItemKey == linkedToItemKey && uda.Value == udaValue)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEqualTo(UserDefinedAttrs udas)
        {
            if (_udaList.Count != udas.UserDefinedAttrList.Count)
            {
                return false;
            }
            foreach (UserDefinedAttr uda in _udaList)
            {
                if (!udas.IsEqualTo(uda.LinkToItemKey, uda.Key, uda.Value))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsDirty
        {
            get
            {
                foreach (UserDefinedAttr uda in _udaList)
                {
                    if (OperationAction.New == uda.OperationAction ||
                        OperationAction.Modified == uda.OperationAction ||
                        OperationAction.Delete == uda.OperationAction)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public DataTable GetDataTable(string tableName)
        {
            DataTable dataTable = DataTableHelper.CreateDataTableForUDA(POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME, _linkedToItemColumnName);
            foreach (UserDefinedAttr uda in _udaList)
            {
                if (OperationAction.None == uda.OperationAction ||
                    OperationAction.Update == uda.OperationAction)
                {
                    continue;
                }
                Dictionary<string, string> rowData = new Dictionary<string, string>()
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION, Convert.ToString((int)uda.OperationAction)},
                                                        {_linkedToItemColumnName, uda.LinkToItemKey},
                                                        {BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY, uda.Key},
                                                        {BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME, uda.Name},
                                                        {COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE, uda.Value},
                                                        {COMMON_FIELDS.FIELD_COMMON_EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME)}
                                                    };
                FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
            }
            if (dataTable.Rows.Count > 0)
            {
                dataTable.TableName = tableName;
                return dataTable;
            }
            return null;
        }
        public void ParseInsertDataToDataTable(ref DataTable dtUDAs)
        {
            foreach (UserDefinedAttr uda in _udaList)
            {
                Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION, Convert.ToString((int)uda.OperationAction)},
                                                        {_linkedToItemColumnName, uda.LinkToItemKey},
                                                        {BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY, uda.Key},
                                                        {BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME, uda.Name},
                                                        {COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE, uda.Value},
                                                        {COMMON_FIELDS.FIELD_COMMON_EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME)}
                                                    };
                FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dtUDAs, rowData);
            }
        }
        public void ParseUpdateDataToDataTable(ref DataTable dtUDAs)
        {
            foreach (UserDefinedAttr uda in _udaList)
            {
                if (OperationAction.None == uda.OperationAction ||
                    OperationAction.Update == uda.OperationAction)
                {
                    continue;
                }
                Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION, Convert.ToString((int)uda.OperationAction)},
                                                        {_linkedToItemColumnName, uda.LinkToItemKey},
                                                        {BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY, uda.Key},
                                                        {BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME, uda.Name},
                                                        {COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE, uda.Value},
                                                        {COMMON_FIELDS.FIELD_COMMON_EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME)}
                                                    };
                FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dtUDAs, rowData);
            }
        }

        private void AddToInitDictionary(string key, string value)
        {
            if (_initDataDict.ContainsKey(key))
            {
                return;
            }
            _initDataDict.Add(key, value);
        }


        private string _linkedToItemColumnName = "";
        private List<UserDefinedAttr> _udaList = new List<UserDefinedAttr>();
        private Dictionary<string, string> _initDataDict = new Dictionary<string, string>();

        private const string COLUMN_LINKED_ITEM_KEY     = "ORDERKEY";
        private const string COLUMN_ATTRIBUTE_KEY       = "ATTRIBUTE_KEY";
        private const string COLUMN_ATTRIBUTE_NAME      = "ATTRIBUTE_NAME";
        private const string COLUMN_ATTRIBUTE_VALUE     = "ATTRIBUTE_VALUE";
        private const string COLUMN_LAST_UPDATE_TIME    = "LAST_UPDATETIME";
        private const string COLUMN_ATTRIBUTE_DATE_TYPE = "DATA_TYPE";
    }
}
