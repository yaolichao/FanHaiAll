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
    public class UserDefinedAttrEx : BaseUserDefinedAttrEx
    {
        public UserDefinedAttrEx(string linkedItemKey, string udaKey, string udaName, string udaDescription)
            : base(udaKey, udaName, UDA_DATA_TYPE.STRING, udaDescription)
        {
            _linkToItemKey = linkedItemKey;
        }
        public UserDefinedAttrEx(string linkedItemKey, string udaKey, string udaName, string udaValue, string udaDescription)
            : base(udaKey, udaName, UDA_DATA_TYPE.STRING, udaDescription)
        {
            _linkToItemKey = linkedItemKey;
            _udaValue = udaValue;
        }

        public UserDefinedAttrEx(string linkedItemKey,string linkedToTable, string udaKey, string udaName, string udaValue, string udaDescription)
            : base(udaKey, udaName, UDA_DATA_TYPE.STRING, udaDescription)
        {
            _linkToItemKey = linkedItemKey;
            _udaValue = udaValue;
            _linkToTable = linkedToTable;
        }
        public UserDefinedAttrEx(string linkedItemKey, BaseUserDefinedAttr baseUDA)
            : base(baseUDA.Key, baseUDA.Name, baseUDA.DataType, baseUDA.Description)
        {
            _linkToItemKey = linkedItemKey;
        }

    
        public UserDefinedAttrEx(string linkedItemKey, string udaValue, BaseUserDefinedAttr baseUDA)
            : base(baseUDA.Key, baseUDA.Name, baseUDA.DataType, baseUDA.Description)
        {
            _linkToItemKey = linkedItemKey;
            _udaValue = udaValue;
        }

        public UserDefinedAttrEx(string linkedItemKey, string udaValue, string linkedToTable, BaseUserDefinedAttr baseUDA)
            : base(baseUDA.Key, baseUDA.Name, baseUDA.DataType, baseUDA.Description)
        {
            _linkToItemKey = linkedItemKey;
            _udaValue = udaValue;
            _linkToTable = linkedToTable;
        }

        //public UserDefinedAttr(string linkedItemKey, string udaKey, string udaName, string udaValue, string udaDesc, OperationAction operationAction)
        //    : base(udaKey, udaName, 
        //{
        //}

        #region Propertiy


        public string Value
        {
            get
            {
                return _udaValue;
            }
            set
            {
                _udaValue = value;
            }
        }
        public string LinkToItemKey
        {
            get
            {
                return _linkToItemKey;
            }
            set
            {
                _linkToItemKey = value;
            }
        }

        public string LinkToTable
        {
            get
            {
                return _linkToTable;
            }
            set
            {
                _linkToTable = value;
            }
        }
        public OperationAction OperationAction
        {
            get
            {
                return _udaAction;
            }
            set
            {
                _udaAction = value;
            }
        }
        #endregion

        #region Private variable define
        private string _udaValue = "";
        private string _linkToItemKey = "";
        private string _linkToTable = "";
        private OperationAction _udaAction = OperationAction.None;
        #endregion
    }
}
