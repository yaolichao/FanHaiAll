using System;
using System.Collections; 
using System.Collections.Generic;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Common;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Reflection;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Common
{
    public static class Utils
    {

        /// <summary>
        /// Add KeyValues To DataTable
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="keyValues"></param>
        public static void AddKeyValuesToDataTable(ref DataTable dataTable, Dictionary<string, DirtyItem> keyValues)
        {
            if (dataTable != null && keyValues != null && keyValues.Count > 0)
            {
                DataRow row = dataTable.Rows.Add();

                foreach (KeyValuePair<string, DirtyItem> keyValue in keyValues)
                {
                    if (dataTable.Columns.Contains(keyValue.Key))
                    {
                        row[keyValue.Key] = keyValue.Value.FieldNewValue;
                    }
                }

                dataTable.AcceptChanges();
            }
        }

        /// <summary>
        /// Add KeyValues To DataTable
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="keyValues"></param>
        /// Owner:Andy Gao 2010-07-12 10:29:08
        public static void AddKeyValuesToDataTable(ref DataTable dataTable, Dictionary<string, string> keyValues)
        {
            if (dataTable != null && keyValues != null && keyValues.Count > 0)
            {
                DataRow row = dataTable.Rows.Add();

                foreach (KeyValuePair<string, string> keyValue in keyValues)
                {
                    if (dataTable.Columns.Contains(keyValue.Key))
                    {
                        row[keyValue.Key] = keyValue.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Get Distinct Value List
        /// </summary>
        /// <param name="dataView"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-14 09:50:26
        public static List<string> GetDistinctValueList(DataView dataView, string field)
        {
            List<string> distinctValueList = new List<string>();

            if (dataView != null && !string.IsNullOrEmpty(field))
            {
                DataTable distinctDataTable = dataView.ToTable(true, field);

                foreach (DataRow row in distinctDataTable.Rows)
                {
                    distinctValueList.Add(row[field].ToString());
                }
            }

            return distinctValueList;
        }

        /// <summary>
        /// Get Distinct Value List
        /// </summary>
        /// <param name="dataView"></param>
        /// <param name="field1"></param>
        /// <param name="field2"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-14 14:21:23
        public static Hashtable GetDistinctValueList(DataView dataView, string field1, string field2)
        {
            Hashtable distinctValueList = new Hashtable();

            if (dataView != null && !string.IsNullOrEmpty(field1) && !string.IsNullOrEmpty(field2))
            {
                DataTable distinctDataTable = dataView.ToTable(true, field1, field2);

                foreach (DataRow row in distinctDataTable.Rows)
                {
                    distinctValueList.Add(row[field1].ToString(), row[field2].ToString());
                }
            }

            return distinctValueList;
        }
       
        /// <summary>
        /// 使用指定字段名创建数据表对象。
        /// </summary>
        /// <param name="tableName">创建的数据表的名称。</param>
        /// <param name="fields">数据表中包含的字段集合。</param>
        /// <returns>数据表对象。</returns>
        public static DataTable CreateDataTableWithColumns(string tableName, List<string> fields)
        {
            //数据表名为空或空字符串，或者字段集合没有值
            if (string.IsNullOrEmpty(tableName) || fields.Count < 1)
            {
                return null;
            }
            DataTable dt = new DataTable(tableName);
            //遍历字段，添加到数据表对象中。
            foreach(string field in fields)
            {
                dt.Columns.Add(field);
            }
            return dt;
        }
        /// <summary>
        /// 向数据库表中添加一行。
        /// </summary>
        /// <param name="dt">数据库表。</param>
        /// <param name="keyValues">新增行包含的值。</param>
        public static void AddRowDataToDataTable(ref DataTable dt, Dictionary<string, string> keyValues)
        {
            if (null == dt || dt.Columns.Count != keyValues.Count)
            {
                throw (new Exception("Utils: AddRowDataToDataTable - init"));
            }
            dt.Rows.Add();
            foreach (KeyValuePair<string, string> keyValue in keyValues)
            {
                if (! dt.Columns.Contains(keyValue.Key))
                {
                    throw (new Exception("Utils: AddRowDataToDataTable - Add row data"));
                }
                dt.Rows[dt.Rows.Count - 1][keyValue.Key] = keyValue.Value;
            }
        }
        /// <summary>
        /// 向数据库表中添加一行。
        /// </summary>
        /// <param name="dt">数据库表。</param>
        /// <param name="keyValues">新增行包含的值。</param>
        public static void AddRowDataToTable(DataTable dt, Dictionary<string, string> keyValues)
        {
            if (null == dt)
            {
                throw (new Exception("Utils: AddRowDataToTable - Is Null"));
            }
            dt.Rows.Add();
            foreach (KeyValuePair<string, string> keyValue in keyValues)
            {
                if (!dt.Columns.Contains(keyValue.Key))
                {
                    dt.Columns.Add(keyValue.Key);
                }
                dt.Rows[dt.Rows.Count - 1][keyValue.Key] = keyValue.Value;
            }
        }


      

        public static bool ValidateDataByType(string attributeValue, string attributeDataType)
        {
            AttributeDataType dataType = (AttributeDataType)Convert.ToInt32(attributeDataType);
            Regex regex;
            switch (dataType)
            {
                case AttributeDataType.INTEGER:
                    regex = new Regex("^-?\\d+$");
                    return regex.IsMatch(attributeValue);
                case AttributeDataType.DATE:
                    regex = new Regex("^((((1[6-9]|[2-9]\\d)\\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\\d|3[01]))|" +
                                      "(((1[6-9]|[2-9]\\d)\\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\\d|30))|" +
                                      "(((1[6-9]|[2-9]\\d)\\d{2})-0?2-(0?[1-9]|1\\d|2[0-8]))|" +
                                      "(((1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|" +
                                      "((16|[2468][048]|[3579][26])00))-0?2-29-)) " +
                                      "(20|21|22|23|[0-1]?\\d):[0-5]?\\d:[0-5]?\\d$");
                    return regex.IsMatch(attributeValue);
                case AttributeDataType.DATETIME:
                    regex = new Regex("^((((1[6-9]|[2-9]\\d)\\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\\d|3[01]))|" +
                                      "(((1[6-9]|[2-9]\\d)\\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\\d|30))|" +
                                      "(((1[6-9]|[2-9]\\d)\\d{2})-0?2-(0?[1-9]|1\\d|2[0-8]))|" +
                                      "(((1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|" +
                                      "((16|[2468][048]|[3579][26])00))-0?2-29-)) " +
                                      "(20|21|22|23|[0-1]?\\d):[0-5]?\\d:[0-5]?\\d$");
                    return regex.IsMatch(attributeValue);
                case AttributeDataType.BOOLEAN:
                    if (attributeValue == "true" || attributeValue == "false")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case AttributeDataType.STRING:
                    return true;
                case AttributeDataType.FLOAT:
                    regex = new Regex("^(-?\\d+)(\\.\\d+)?$");
                    return regex.IsMatch(attributeValue);
                case AttributeDataType.SETTING:
                    return true;
                case AttributeDataType.LINKED:
                    return true;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Get Location IP Address
        /// </summary>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-04-27 15:45:28
        public static string GetLocationIPAddress()
        {
            IPAddress locationIPAddress = IPAddress.Loopback;

            IPAddress[] ipAddresses = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (IPAddress ipAddress in ipAddresses)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    locationIPAddress = ipAddress;

                    break;
                }
            }

            return locationIPAddress.ToString();
        }

       

        /// <summary>
        /// Reset DataTable Sequence Field
        /// </summary>
        /// <param name="dataTable"></param>
        /// Owner:Andy Gao 2011-05-26 09:19:19
        public static void ResetDataTableSequenceField(DataTable dt)
        {
            if (dt != null)
            {
                if (!dt.Columns.Contains("Sequence"))
                {
                    dt.Columns.Add("Sequence", typeof(int));
                }

                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    dt.Rows[i - 1]["Sequence"] = i;
                }
            }
        }

        /// <summary>
        /// Execute SAP Remote Function Call from Remoting Server
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="inputData"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-05-31 10:39:48
        public static DataSet ExecuteRFC(string functionName, DataSet inputData, out string msg)
        {
            msg = string.Empty;

            DataSet outputData = null;

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory == null)
                {
                    msg = "The Server Factory is null.";
                }
                else
                {
                    outputData = serverFactory.CreateIRFCEngine().ExecuteRFC(functionName, inputData);

                    msg =ReturnMessageUtils.GetServerReturnMessage(outputData);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            return outputData;
        }

        /// <summary>
        /// Create Specify Columns Data Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-05-31 11:06:31
        public static DataTable CreateSpecifyColumnsDataTable(string tableName, params string[] columnNames)
        {
            DataTable dataTable = new DataTable(tableName);

            foreach (String columnName in columnNames)
            {
                dataTable.Columns.Add(columnName);
            }

            return dataTable;
        }

        /// <summary>
        /// 检查服务器端是否停止服务或者程序版本是否有效。
        /// </summary>
        /// <returns>返回1表示服务器停止，返回2表示应用程序版本无效，返回0表示服务器未停止和应用程序版本有效。</returns>
        public static int CheckServerStopServiceOrVersionInvalid()
        {
            int iReturn = 0;                                                                                            //返回结果
            bool bIsStopService = false;                                                                                //服务是否停止
            bool bIsVersionInvalid = false;                                                                             //版本是否有效
            string site = PropertyService.Get(PROPERTY_FIELDS.SITE);                                                    //当前登录的站点名称

            string[] columns = new string[] { "SITE_NAME", "SITE_VALUE", "STOP_SERVICE", "VERSION" };                   //站点属性的列名集合
            KeyValuePair<string, string>  category = new KeyValuePair<string, string>("CATEGORY_NAME", "Site");         //站点属性的分类名（Site）。

            DataTable siteTable = BaseData.Get(columns, category);                                                      //通过分类名和列名集合获取数据表对象。
            if (siteTable.Rows.Count > 0)
            {
                string sql = string.Format(@"SITE_NAME ='{0}' OR SITE_VALUE='{0}'", site);     
                DataRow[] dataRow = siteTable.Select(sql); //根据登陆的站点名获取该站点对应的数据行。
                //数据行大于0
                if (dataRow.Length > 0)
                {
                    string strIsStopService = dataRow[0]["STOP_SERVICE"].ToString();            //是否停止服务
                    string strSiteVersion = dataRow[0]["VERSION"].ToString();                   //服务器上存储版本号
                    if (!string.IsNullOrEmpty(strIsStopService))//不是null或空。
                        bIsStopService = strIsStopService == "1" ? true : false;                //1：为停止服务，否则为未停止服务。

                    //版本号不为null或空字符串，且数据库中设置的版本号大于当前登陆的版本
                    if (!string.IsNullOrEmpty(strSiteVersion) && string.Compare(strSiteVersion, PropertyService.Get("VERSION")) > 0)
                        bIsVersionInvalid = true;
                }
            }
            if (bIsStopService)//true为已停止服务
                iReturn = 1;
            else if (bIsVersionInvalid)//true为版本无效。
                iReturn = 2;

            return iReturn;
        }
        /// <summary>
        /// check is the newest version
        /// </summary>
        /// <returns></returns>
        /// Owner:xiaoai.zhang 2011-08-18
        public static bool CheckIsVersionValid()
        {
            bool bIsVersionValid = true;

            string site = PropertyService.Get("SITE");

            string[] columns = new string[] { "LANGUAGE_NAME", "LANGUAGE_SIGN" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Language");


            columns = new string[] { "SITE_NAME", "SITE_VALUE", "VERSION" };
            category = new KeyValuePair<string, string>("CATEGORY_NAME", "Site");
            DataTable siteTable = BaseData.Get(columns, category);
            if (siteTable.Rows.Count > 0)
            {
                string sql = string.Format(@"SITE_NAME ='{0}' OR SITE_VALUE='{0}'", site);
                DataRow[] dataRow = siteTable.Select(sql);

                if (dataRow.Length > 0)
                {
                    string strVersion = dataRow[0]["VERSION"].ToString();
                    string strcurversion = PropertyService.Get("CURVERSION");
                    if (!string.IsNullOrEmpty(strVersion) && strVersion != PropertyService.Get("VERSION"))
                        bIsVersionValid = false;
                }
            }


            return bIsVersionValid;
        }

        /// <summary>
        /// Execute Factory Engine Method from Remoting Server
        /// </summary>
        /// <param name="engineName"></param>
        /// <param name="engineMethod"></param>
        /// <param name="engineParamValue"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-07-12 08:14:19
        public static DataSet ExecuteEngineMethod(string engineName, string engineMethod, DataSet engineParamValue, out string msg)
        {
            msg = string.Empty;

            DataSet resDS = null;

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory == null)
                {
                    msg = "The Server Factory is null.";
                }
                else
                {
                    resDS = serverFactory.ExecuteEngineMethod(engineName, engineMethod, engineParamValue);

                    msg = resDS.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_MESSAGE) ? resDS.ExtendedProperties[PARAMETERS.OUTPUT_MESSAGE].ToString() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            return resDS;
        }
        /// <summary>
        /// 根据枚举值获取枚举值描述。
        /// </summary>
        /// <param name="val">枚举值。</param>
        /// <returns>枚举值描述。</returns>
        public static string GetEnumValueDescription(object val)
        {
            if (val != null)
            {
                FieldInfo [] fileds=val.GetType().GetFields();
                foreach (FieldInfo f in fileds)
                {
                    if (f.Name==val.ToString())
                    { 
                        object[] attrs =f.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attrs.Length > 0)
                        {
                            DescriptionAttribute attr = attrs[0] as DescriptionAttribute;
                            return attr.Description;
                        }
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据设备状态返回代表设备状态的颜色。
        /// </summary>
        /// <param name="stateName">设备状态名称。</param>
        /// <returns>代表设备状态的颜色。</returns>
        public static System.Drawing.Color GetEquipmentStateColor(string stateName)
        {
            Dictionary<string, System.Drawing.Color> Colors =new Dictionary<string,System.Drawing.Color>()
            {
                    {"RUN",System.Drawing.Color.Lime},
                    {"LOST",System.Drawing.Color.Yellow},
                    {"T_MD",System.Drawing.Color.Green},
                    {"W_MF",System.Drawing.Color.Blue},
                    {"DOWN",System.Drawing.Color.Red},
                    {"CIMD",System.Drawing.Color.Fuchsia},
                    {"W_EN",System.Drawing.Color.FromArgb(0, 192, 192)},
                    {"FACD",System.Drawing.Color.Olive},
                    {"PM",System.Drawing.Color.Maroon},
                    {"STEUP",System.Drawing.Color.RoyalBlue},
                    {"TEST",System.Drawing.Color.Orchid},
                    {"OFF",System.Drawing.Color.DimGray},
                    {"P_DOWN",System.Drawing.Color.DarkOliveGreen},
                    {"MON",System.Drawing.Color.OrangeRed},
                    {"T_DOWN",System.Drawing.Color.DarkOrange}
            };

            if (Colors.ContainsKey(stateName))
            {
                return Colors[stateName];
            }
            return System.Drawing.Color.White;
        }
        /// <summary>
        /// 生成全球唯一字符串。
        /// </summary>
        /// <param name="i">后缀码。</param>
        /// <returns>全球唯一字符串。</returns>
        [Obsolete("使用FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey方法替代。")]
        public static string GenerateNewKey(int i)
        {
            return System.Guid.NewGuid() + "-" + i.ToString("000");
        }
        /// <summary>
        /// 获取当前日期。
        /// </summary>
        /// <returns>返回当期日期时间</returns>
        public static DateTime GetCurrentDateTime()
        {
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.CreateILotEngine().GetSysdate();
            }
            catch
            {
                return DateTime.Now;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }
    }
}
