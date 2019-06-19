using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Utils.Entities.Exceptions
{
    public class WeighException : Exception
    {
        private string _strMessage = "";
        private Exception _innerException = null;
        public WeighException(string strMessage, Exception innerException)
        {
            _strMessage = strMessage;
            _innerException = innerException;
        }

        public override string Message
        {
            get
            {
                return _strMessage;
            }
        }

        public override Exception GetBaseException()
        {
            return _innerException;
        }
    }
}
