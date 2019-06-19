using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.Entities
{
    public class BaseUserDefinedAttrs
    {
        public BaseUserDefinedAttrs()
        {
        }
        public BaseUserDefinedAttrs(DataTable table)
        {
            if (null == table || table.Columns.Count != 5)
            {
                throw (new Exception("BaseUserDefinedAttrs::BaseUserDefinedAttrs(DataTable)"));
            }
            foreach (DataRow dataRow in table.Rows)
            {
                string udaKey       = dataRow[table.Columns[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY]].ToString();
                string udaName      = dataRow[table.Columns[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME]].ToString();
                string udaDataType  = dataRow[table.Columns[BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE]].ToString();
                string udaDescription = dataRow[table.Columns[BASE_ATTRIBUTE_FIELDS.FIELDS_DESCRIPTIONS]].ToString();
                BaseUserDefinedAttr uda = new BaseUserDefinedAttr(udaKey, udaName, udaDataType, udaDescription);
                _udaList.Add(uda);
            }
        }

        public bool UserDefinedAttrAdd(BaseUserDefinedAttr uda)
        {
            foreach (BaseUserDefinedAttr itemUDA in _udaList)
            {
                if (itemUDA.Key == uda.Key)
                {
                    return false;
                }
            }
            _udaList.Add(uda);
            return true;
        }

        public bool UserDefinedAttrUpdate(BaseUserDefinedAttr uda)
        {
            int iSeq = 0;
            foreach (BaseUserDefinedAttr itemUDA in _udaList)
            {
                if (itemUDA.Key == uda.Key)
                {
                    _udaList[iSeq] = uda;
                    return true;
                }
                iSeq++;
            }
            return false;
        }
        public bool UserDefinedAttrDelete(string attributeKey)
        {
            foreach (BaseUserDefinedAttr itemUDA in _udaList)
            {
                if (itemUDA.Key == attributeKey)
                {
                    return _udaList.Remove(itemUDA);
                }
            }
            return false;
        }
        public bool UserDefinedAttrDelete(BaseUserDefinedAttr uda)
        {
            foreach (BaseUserDefinedAttr itemUDA in _udaList)
            {
                if (itemUDA.Key == uda.Key)
                {
                    return _udaList.Remove(itemUDA);
                }
            }
            return false;
        }
        public void UserDefinedAttrDelete(List<string> udas)
        {
            if (null == _udaList || null == udas)
            {
                return;
            }
            foreach (string udaKey in udas)
            {
                foreach (BaseUserDefinedAttr uda in _udaList)
                {
                    if (uda.Key == udaKey)
                    {
                        _udaList.Remove(uda);
                        break;
                    }
                }
            }
        }
        public void UserDefinedAttrDelete(BaseUserDefinedAttrs udas)
        {
            if (null == _udaList || null == udas)
            {
                return;
            }
            foreach (BaseUserDefinedAttr removeUDA in udas.BaseUserDefinedAttrList) 
            {
                foreach (BaseUserDefinedAttr uda in _udaList)
                {
                    if (uda.Key == removeUDA.Key)
                    {
                        _udaList.Remove(uda);
                        break;
                    }
                }
            }
        }

        public List<BaseUserDefinedAttr> BaseUserDefinedAttrList
        {
            get
            {
                return _udaList;
            }
        }

        private List<BaseUserDefinedAttr> _udaList = new List<BaseUserDefinedAttr>();
    }
}
