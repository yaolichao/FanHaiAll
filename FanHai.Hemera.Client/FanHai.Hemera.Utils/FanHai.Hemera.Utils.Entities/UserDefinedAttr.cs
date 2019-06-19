using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.Entities
{
    public class UserDefinedAttr : BaseUserDefinedAttr
    {
        public UserDefinedAttr(string linkedItemKey, string udaKey, string udaName, string udaDescription)
            : base(udaKey, udaName, UDA_DATA_TYPE.STRING, udaDescription)
        {
            _linkToItemKey = linkedItemKey;
        }
        public UserDefinedAttr(string linkedItemKey, string udaKey, string udaName, string udaValue, string udaDescription)
            : base(udaKey, udaName, UDA_DATA_TYPE.STRING, udaDescription)
        {
            _linkToItemKey = linkedItemKey;
            _udaValue = udaValue;
        }
        public UserDefinedAttr(string linkedItemKey, BaseUserDefinedAttr baseUDA)
            : base(baseUDA.Key, baseUDA.Name, baseUDA.DataType, baseUDA.Description)
        {
            _linkToItemKey = linkedItemKey;
        }

        public UserDefinedAttr(string linkedItemKey, string udaValue, BaseUserDefinedAttr baseUDA) 
            :base(baseUDA.Key, baseUDA.Name, baseUDA.DataType, baseUDA.Description)
        {
            _linkToItemKey = linkedItemKey;
            _udaValue = udaValue;
        }

        //public UserDefinedAttr(string linkedItemKey, string udaKey, string udaName, string udaValue, string udaDesc, OperationAction operationAction)
        //    : base(udaKey, udaName, 
        //{
        //}


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
        private string _udaValue = "";
        private string _linkToItemKey = "";
        private OperationAction _udaAction = OperationAction.None;
    }
}
