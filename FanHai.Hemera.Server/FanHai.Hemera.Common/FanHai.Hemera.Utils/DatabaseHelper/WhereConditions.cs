using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Utils.DatabaseHelper
{
    public class WhereConditions
    {
        public WhereConditions(string strKey, string strValue)
        {
            this.key = strKey;
            this.value = strValue;
        }

        //public void AddCondition

        public new string ToString()
        {
            return String.Empty;
        }

        private string key = string.Empty;
        private string value = string.Empty;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
