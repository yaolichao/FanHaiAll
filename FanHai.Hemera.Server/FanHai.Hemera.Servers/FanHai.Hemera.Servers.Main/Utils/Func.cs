using System;
using System.Collections.Generic;
using System.Xml; 
using System.Text;

namespace SolarViewer.Hemera.Servers.Main.Utils
{
    class Func
    {
        public static string getNodeText(XmlNode pNode, string childNodeName)
        {
            string strReturn = "";
            XmlNode cNode = pNode.SelectSingleNode(childNodeName);
            if (cNode != null)
            {
                strReturn = cNode.InnerText;
            }
            return strReturn;
        }

        public static string getNodeAttributeValue(XmlNode pNode, string attributeName)
        {
            string strReturn = "";
            try
            {
                strReturn = pNode.Attributes[attributeName].Value;
            }
            catch
            {
                strReturn = "";
            }
            return strReturn;
        }
    }
}
