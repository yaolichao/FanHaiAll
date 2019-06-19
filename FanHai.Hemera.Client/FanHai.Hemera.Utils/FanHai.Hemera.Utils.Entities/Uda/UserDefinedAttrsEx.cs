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
    public class UserDefinedAttrsEx
    {
        public UserDefinedAttrsEx()
        {
        }

        public UserDefinedAttrsEx(string linkedToItemColumnName,string linkedToTable)
        {
            _linkedToItemColumnName = linkedToItemColumnName;
            _linkedToTable = linkedToTable;
        }

        public UserDefinedAttrsEx(DataTable table)
        {
            if (null == table || table.Columns.Count != 8)
            {
                throw (new Exception("UserDefinedAttrsEx::UserDefinedAttrsEx(DataTable)"));
            }
            foreach (DataRow dataRow in table.Rows)
            {
                string linkedItemKey = dataRow[table.Columns[COLUMN_OBJECT_KEY]].ToString();
                string udaObjectType= dataRow[table.Columns[COLUMN_OBJECT_TYPE]].ToString();
                string udaKey = dataRow[table.Columns[COLUMN_ATTRIBUTE_KEY]].ToString();
                string udaName = dataRow[table.Columns[COLUMN_ATTRIBUTE_NAME]].ToString();
                string udaValue = dataRow[table.Columns[COLUMN_ATTRIBUTE_VALUE]].ToString();
                string udaEditor = dataRow[table.Columns[COLUMN_EDITOR]].ToString();
                string udaEditTime = dataRow[table.Columns[COLUMN_EDIT_TIME]].ToString();
                string udaEditTimeZone = dataRow[table.Columns[COLUMN_EDIT_TIMEZONE]].ToString();
                UserDefinedAttrEx uda = new UserDefinedAttrEx(linkedItemKey, udaKey, udaName, udaValue, "");
                 uda.OperationAction = OperationAction.Update;
                _udaList.Add(uda);

            }
        }

        public bool UserDefinedAttrAdd(UserDefinedAttrEx uda)
        {
            foreach (UserDefinedAttrEx itemUDA in _udaList)
            {
                if (itemUDA.Key == uda.Key)
                {
                    if (OperationAction.Delete == itemUDA.OperationAction)
                    {
                        itemUDA.OperationAction = OperationAction.Modified;
                        itemUDA.Value ="";
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
            foreach (UserDefinedAttrEx uda in _udaList)
            {
                if (OperationAction.None == uda.OperationAction)
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
        public bool UserDefinedAttrDelete(string key)
        {
            foreach (UserDefinedAttrEx uda in _udaList)
            {
                if (uda.Key == key)
                {
                    if (OperationAction.New == uda.OperationAction)
                    {
                        _udaList.Remove(uda);
                        return true;
                    }
                    uda.OperationAction = OperationAction.Delete;
                    return true;
                }
            }
            return false;
        }
        public bool UserDefinedAttrDelete(UserDefinedAttrEx uda)
        {
            return UserDefinedAttrDelete(uda.Key);
        }

        public void UserDefinedAttrChangeKey(string newKey)
        {
            foreach (UserDefinedAttrEx uda in _udaList)
            {
                uda.LinkToItemKey = newKey;
                uda.LinkToTable = _linkedToTable;
                uda.OperationAction = OperationAction.New;
                uda.LastEditor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            }
        }
     
        public string GetUserDefinedAttrValueByAttrKey(string attributeKey)
        {
            foreach (UserDefinedAttrEx uda in _udaList)
            {
                if (uda.Key == attributeKey)
                {
                    return uda.Value;
                }
            }
            return "";
        }
        public List<UserDefinedAttrEx> UserDefinedAttrList
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


        public string LinkedToTable
        {
            get
            {
                return _linkedToTable;
            }
            set
            {
                _linkedToTable = value;
            }
        }


        public bool Contains(string linkedToItemKey, string udaKey)
        {
            foreach (UserDefinedAttrEx uda in _udaList)
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
            foreach (UserDefinedAttrEx uda in _udaList)
            {
                if (uda.Key == udaKey && uda.LinkToItemKey == linkedToItemKey && uda.Value == udaValue)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEqualTo(UserDefinedAttrsEx udas)
        {
            if (_udaList.Count != udas.UserDefinedAttrList.Count)
            {
                return false;
            }
            foreach (UserDefinedAttrEx uda in _udaList)
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
                foreach (UserDefinedAttrEx uda in _udaList)
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
            DataTable dataTable = DataTableHelper.CreateDataTableForUDAEx(BASE_ATTRIBUTE_VALUE_FIELDS.DATABASE_TABLE_NAME,"", BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_OBJECT_TYPE);
            foreach (UserDefinedAttrEx uda in _udaList)
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
                                                        {BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_OBJECT_TYPE,_linkedToTable},
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
            foreach (UserDefinedAttrEx uda in _udaList)
            {
                Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION, Convert.ToString((int)uda.OperationAction)},
                                                        {_linkedToItemColumnName, uda.LinkToItemKey},
                                                        {BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_OBJECT_TYPE, _linkedToTable},
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
            foreach (UserDefinedAttrEx uda in _udaList)
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
                                                        {BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_OBJECT_TYPE,_linkedToTable},
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
        private string _linkedToTable = "";
        private List<UserDefinedAttrEx> _udaList = new List<UserDefinedAttrEx>();
        private Dictionary<string, string> _initDataDict = new Dictionary<string, string>();

        private const string COLUMN_OBJECT_KEY = "OBJECT_KEY";
        private const string COLUMN_OBJECT_TYPE = "OBJECT_TYPE";
        private const string COLUMN_ATTRIBUTE_KEY = "ATTRIBUTE_KEY";
        private const string COLUMN_ATTRIBUTE_NAME = "ATTRIBUTE_NAME";
        private const string COLUMN_ATTRIBUTE_VALUE = "ATTRIBUTE_VALUE";
        private const string COLUMN_EDITOR = "EDITOR";
        private const string COLUMN_EDIT_TIME = "EDIT_TIME";
        private const string COLUMN_EDIT_TIMEZONE = "EDIT_TIMEZONE";
    }
}
