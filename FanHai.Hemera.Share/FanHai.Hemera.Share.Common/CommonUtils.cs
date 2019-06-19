using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using System.Reflection;
using System.ComponentModel;

namespace FanHai.Hemera.Share.Common
{
    public sealed class ReturnMessageUtils
    {
        /// <summary>
        /// 添加消息到数据集合对象中。参考<see cref="AddServerReturnMessage"/>方法。
        /// </summary>
        /// <param name="dataSet">数据集对象。</param>
        /// <param name="messageInfo">消息信息字符串。添加到“ATTRIBUTE_KEY”列等于“MESSAGE”的行的“ATTRIBUTE_VALUE”列中。</param>
        [Obsolete]
        public static void AddMessageInforTwoColumn(DataSet dataSet, string messageInfo)
        {
            AddServerReturnMessage(dataSet, messageInfo);
        }
        /// <summary>
        /// 添加服务器端返回消息到数据集合对象中。
        /// 向数据集对象中添加名称为“<see cref="PARAMETERS_OUTPUT.DATABASE_TABLE_NAME"/>”的数据表对象。
        /// 数据表对象中包含的列信息见<see cref="PARAMETERS_OUTPUT.FILEDS"/>。
        /// <see cref="PARAMETERS_OUTPUT.FIELD_CODE"/>列存放服务器端返回的消息代码。
        /// <see cref="PARAMETERS_OUTPUT.FIELD_MESSAGE"/>列存放服务器端返回的消息字符串。
        /// </summary>
        /// <param name="dataSet">数据集对象。</param>
        /// <param name="code">消息代码。<see cref="PARAMETERS_OUTPUT.FIELD_CODE"/></param>
        /// <param name="message">消息信息字符串。 <see cref=">PARAMETERS_OUTPUT.FIELD_MESSAGE"/></param>
        public static void AddServerReturnMessage(DataSet resDS, int code, string message)
        {
            DataTable outputParamDataTable = null;
            if (!resDS.Tables.Contains(PARAMETERS_OUTPUT.DATABASE_TABLE_NAME))
            {
                outputParamDataTable = PARAMETERS_OUTPUT.CreateDataTable();
            }
            else
            {
                outputParamDataTable = resDS.Tables[PARAMETERS_OUTPUT.DATABASE_TABLE_NAME].Clone();
            }

            DataRow row = outputParamDataTable.Rows.Add();

            row[PARAMETERS_OUTPUT.FIELD_CODE] = code;
            row[PARAMETERS_OUTPUT.FIELD_MESSAGE] = message;

            resDS.Merge(outputParamDataTable, false, MissingSchemaAction.Add);
        }
        /// <summary>
        /// 添加服务器端返回消息到数据集合对象中。
        /// 向数据集对象中添加名称为“<see cref="PARAMETERS_OUTPUT.DATABASE_TABLE_NAME"/>”的数据表对象。
        /// 数据表对象中包含的列信息见<see cref="PARAMETERS_OUTPUT.FILEDS"/>。
        /// <see cref="PARAMETERS_OUTPUT.FIELD_CODE"/>列的值等于0表示执行成功，-1 表示执行失败。
        /// <see cref="PARAMETERS_OUTPUT.FIELD_MESSAGE"/>列存放服务器端返回的消息字符串。
        /// </summary>
        /// <param name="dataSet">数据集对象。</param>
        /// <param name="messageInfo">消息信息字符串。
        /// 添加到数据表对象的<see cref=">PARAMETERS_OUTPUT.FIELD_MESSAGE"/>列中。
        /// </param>
        /// Owner:Andy Gao 2010-07-07 09:14:58
        /// comment by peter 2012-2-12
        public static void AddServerReturnMessage(DataSet resDS, string msg)
        {
            DataTable outputParamDataTable = null;

            if (!resDS.Tables.Contains(PARAMETERS_OUTPUT.DATABASE_TABLE_NAME))
            {
                outputParamDataTable = PARAMETERS_OUTPUT.CreateDataTable();
            }
            else
            {
                outputParamDataTable = resDS.Tables[PARAMETERS_OUTPUT.DATABASE_TABLE_NAME].Clone();
            }

            DataRow row = outputParamDataTable.Rows.Add();

            row[PARAMETERS_OUTPUT.FIELD_CODE] = string.IsNullOrEmpty(msg) ? "0" : "-1";
            row[PARAMETERS_OUTPUT.FIELD_MESSAGE] = msg;

            resDS.Merge(outputParamDataTable, false, MissingSchemaAction.Add);
        }
        /// <summary>
        /// Add Server Return Message
        /// </summary>
        /// <param name="resDS"></param>
        /// <param name="msg"></param>
        /// <param name="editTime"></param>
        /// Owner:Andy Gao 2010-07-13 09:23:20
        public static void AddServerReturnMessage(DataSet resDS, string msg, string editTime)
        {
            DataTable outputParamDataTable = null;

            if (!resDS.Tables.Contains(PARAMETERS_OUTPUT.DATABASE_TABLE_NAME))
            {
                outputParamDataTable = PARAMETERS_OUTPUT.CreateDataTable();
            }
            else
            {
                outputParamDataTable = resDS.Tables[PARAMETERS_OUTPUT.DATABASE_TABLE_NAME].Clone();
            }

            DataRow row = outputParamDataTable.Rows.Add();

            row[PARAMETERS_OUTPUT.FIELD_CODE] = string.IsNullOrEmpty(msg) ? "0" : "-1";
            row[PARAMETERS_OUTPUT.FIELD_MESSAGE] = msg;
            row[PARAMETERS_OUTPUT.FIELD_EDIT_TIME] = editTime;

            resDS.Merge(outputParamDataTable, false, MissingSchemaAction.Add);
        }

        /// <summary>
        /// Add Server Return Message
        /// </summary>
        /// <param name="resDS"></param>
        /// <param name="msg"></param>
        /// <param name="editor"></param>
        /// <param name="editTime"></param>
        /// Owner:Andy Gao 2010-07-07 09:17:10
        public static void AddServerReturnMessage(DataSet resDS, string msg, string editor, string editTime)
        {
            DataTable outputParamDataTable = null;

            if (!resDS.Tables.Contains(PARAMETERS_OUTPUT.DATABASE_TABLE_NAME))
            {
                outputParamDataTable = PARAMETERS_OUTPUT.CreateDataTable();
            }
            else
            {
                outputParamDataTable = resDS.Tables[PARAMETERS_OUTPUT.DATABASE_TABLE_NAME].Clone();
            }

            DataRow row = outputParamDataTable.Rows.Add();

            row[PARAMETERS_OUTPUT.FIELD_CODE] = string.IsNullOrEmpty(msg) ? "0" : "-1";
            row[PARAMETERS_OUTPUT.FIELD_MESSAGE] = msg;
            row[PARAMETERS_OUTPUT.FIELD_EDITOR] = editor;
            row[PARAMETERS_OUTPUT.FIELD_EDIT_TIME] = editTime;

            resDS.Merge(outputParamDataTable, false, MissingSchemaAction.Add);
        }

        /// <summary>
        /// 获取服务器返回的消息字符串。空字符串表示执行成功。
        /// </summary>
        /// <param name="resDS">该数据集对象包含一个名称为<see cref="PARAMETERS_OUTPUT.DATABASE_TABLE_NAME"/>的数据表对象,该数据表对象存放查询的执行结果，
        /// 数据表对象中包含的列信息见<see cref="PARAMETERS_OUTPUT.FILEDS"/>。
        /// <see cref="PARAMETERS_OUTPUT.FIELD_CODE"/>列的值等于0表示执行成功，否则表示执行失败。
        /// <see cref="PARAMETERS_OUTPUT.FIELD_MESSAGE"/>列存放服务器端返回的消息字符串。 
        /// </param>
        /// <returns>服务器端返回的消息字符串。空字符串表示执行成功</returns>
        /// Owner:Andy Gao 2010-07-07 10:20:06
        /// comment by peter 2012-2-12
        public static string GetServerReturnMessage(DataSet resDS)
        {
            string msg = string.Empty;

            if (resDS != null && resDS.Tables.Contains(PARAMETERS_OUTPUT.DATABASE_TABLE_NAME))
            {
                DataTable outputParamDataTable = resDS.Tables[PARAMETERS_OUTPUT.DATABASE_TABLE_NAME];

                if (outputParamDataTable.Rows.Count > 0)
                {
                    msg = outputParamDataTable.Rows[0][PARAMETERS_OUTPUT.FIELD_MESSAGE].ToString();
                }
            }

            return msg;
        }

        /// <summary>
        /// Get Server Return Message
        /// </summary>
        /// <param name="resDS"></param>
        /// <param name="editTime"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-13 09:15:18
        public static string GetServerReturnMessage(DataSet resDS, ref string editTime)
        {
            string msg = string.Empty;

            if (resDS != null && resDS.Tables.Contains(PARAMETERS_OUTPUT.DATABASE_TABLE_NAME))
            {
                DataTable outputParamDataTable = resDS.Tables[PARAMETERS_OUTPUT.DATABASE_TABLE_NAME];

                if (outputParamDataTable.Rows.Count > 0)
                {
                    msg = outputParamDataTable.Rows[0][PARAMETERS_OUTPUT.FIELD_MESSAGE].ToString();
                    editTime = outputParamDataTable.Rows[0][PARAMETERS_OUTPUT.FIELD_EDIT_TIME].ToString();
                }
            }

            return msg;
        }

        /// <summary>
        /// Get Server Return Message
        /// </summary>
        /// <param name="resDS"></param>
        /// <param name="editor"></param>
        /// <param name="editTime"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-12 16:14:32
        public static string GetServerReturnMessage(DataSet resDS, ref string editor, ref string editTime)
        {
            string msg = string.Empty;

            if (resDS != null && resDS.Tables.Contains(PARAMETERS_OUTPUT.DATABASE_TABLE_NAME))
            {
                DataTable outputParamDataTable = resDS.Tables[PARAMETERS_OUTPUT.DATABASE_TABLE_NAME];

                if (outputParamDataTable.Rows.Count > 0)
                {
                    msg = outputParamDataTable.Rows[0][PARAMETERS_OUTPUT.FIELD_MESSAGE].ToString();
                    editor = outputParamDataTable.Rows[0][PARAMETERS_OUTPUT.FIELD_EDITOR].ToString();
                    editTime = outputParamDataTable.Rows[0][PARAMETERS_OUTPUT.FIELD_EDIT_TIME].ToString();
                }
            }

            return msg;
        }

        /// <summary>
        /// Get Server Return Message
        /// </summary>
        /// <param name="resDS"></param>
        /// <param name="code"></param>
        /// <param name="editor"></param>
        /// <param name="editTime"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2010-07-07 10:17:58
        public static string GetServerReturnMessage(DataSet resDS, ref string code, ref string editor, ref string editTime)
        {
            string msg = string.Empty;

            if (resDS != null && resDS.Tables.Contains(PARAMETERS_OUTPUT.DATABASE_TABLE_NAME))
            {
                DataTable outputParamDataTable = resDS.Tables[PARAMETERS_OUTPUT.DATABASE_TABLE_NAME];

                if (outputParamDataTable.Rows.Count > 0)
                {
                    msg = outputParamDataTable.Rows[0][PARAMETERS_OUTPUT.FIELD_MESSAGE].ToString();
                    code = outputParamDataTable.Rows[0][PARAMETERS_OUTPUT.FIELD_CODE].ToString();
                    editor = outputParamDataTable.Rows[0][PARAMETERS_OUTPUT.FIELD_EDITOR].ToString();
                    editTime = outputParamDataTable.Rows[0][PARAMETERS_OUTPUT.FIELD_EDIT_TIME].ToString();
                }
            }

            return msg;
        }
        /// <summary>
        /// Get Server Return Message
        /// </summary>
        /// <param name="resDS"></param>
        /// <param name="code"></param>
        /// <param name="editor"></param>
        /// <param name="editTime"></param>
        /// <returns></returns>
        public static string GetServerReturnMessage(DataSet resDS, ref int code)
        {
            string msg = string.Empty;

            if (resDS != null && resDS.Tables.Contains(PARAMETERS_OUTPUT.DATABASE_TABLE_NAME))
            {
                DataTable outputParamDataTable = resDS.Tables[PARAMETERS_OUTPUT.DATABASE_TABLE_NAME];

                if (outputParamDataTable.Rows.Count > 0)
                {
                    msg = outputParamDataTable.Rows[0][PARAMETERS_OUTPUT.FIELD_MESSAGE].ToString();
                    code = Convert.ToInt32(outputParamDataTable.Rows[0][PARAMETERS_OUTPUT.FIELD_CODE]);
                }
            }

            return msg;
        }
        /// <summary>
        /// 获取执行远程调用后的结果。如果远程调用执行成功则返回空字符串。参考<see cref="GetServerReturnMessage"/>方法。
        /// </summary>
        /// <param name="dataset">远程调用返回的结果数据集。</param>
        /// <returns>用字符串形式表现的远程调用的执行结果。</returns>
        [Obsolete]
        public static string GetResultOfCallServiceMessage(DataSet dataset)
        {
            return GetServerReturnMessage(dataset);
        }
        /// <summary>
        /// 获取执行远程调用后的结果。如果远程调用执行成功则返回空字符串。参考<see cref="GetServerReturnMessage"/>方法。
        /// </summary>
        /// <param name="code">返回执行远程调用后的代码。</param>
        /// <param name="dataset">远程调用返回的结果数据集。</param>
        /// <returns>用字符串形式表现的远程调用的执行结果。</returns>
        [Obsolete]
        public static string GetServiceMessage(ref int code, DataSet dataset)
        {
            return GetServerReturnMessage(dataset, ref code);
        }
    }

    public sealed class CommonUtils
    {
        /// <summary>
        /// 根据数据表创建哈希表。
        /// </summary>
        /// <param name="dataTable">数据表对象，数据表中必须包含两个列"name"和"value"。列name存放哈希表的键名，列value存放哈希表键对应的键值。</param>
        /// <returns>哈希表对象。</returns>
        public static Hashtable ConvertToHashtable(DataTable dataTable)
        {
            Hashtable hsData = new Hashtable();
            //如果数据表对象为null
            if (dataTable == null)
            {
                return null;
            }
            //如果数据表中不包含列name或value
            if (dataTable.Columns.Contains("name") == false || dataTable.Columns.Contains("value") == false)
            {
                return null;
            }
            string strKey = "";
            //遍历数据表中的数据行。
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                strKey = dataTable.Rows[i]["name"].ToString();
                //如果哈希表中不存在该键值。
                if (hsData.ContainsKey(strKey) == false)
                {
                    hsData.Add(strKey, dataTable.Rows[i]["value"]);
                }
                else
                {
                    hsData[strKey] = dataTable.Rows[i]["value"];
                }
            }
            return hsData;
        }

        /// <summary>
        /// 根据哈希表创建一个数据表对象，数据表的名称为"param"，数据表包含两个列"name"和"value"。列name存放哈希表的键名，列value存放哈希表键对应的键值。
        /// </summary>
        /// <param name="hsData">哈希表对象</param>
        /// <returns>数据表对象，数据表的名称为"param"，数据表包含两个列"name"和"value"。列name存放哈希表的键名，列value存放哈希表键对应的键值。</returns>
        public static DataTable ParseToDataTable(Hashtable hsData)
        {
            DataTable param = new DataTable("param");
            param.Columns.Add(new DataColumn("name"));
            param.Columns.Add(new DataColumn("value"));
            //遍历哈希表中的所有键
            foreach (string key in hsData.Keys)
            {
                object objValue = hsData[key];
                DataRow nRow = param.NewRow();
                nRow["name"] = key;
                nRow["value"] = objValue;
                param.Rows.Add(nRow);
            }
            return param;
        }
        
        /// <summary>
        /// 转换数据行为哈希表。
        /// </summary>
        /// <param name="dataRow">待转换的数据行。</param>
        /// <returns>转换后的哈希表。</returns>
        public static Hashtable ConvertRowToHashtable(DataRow dataRow)
        {
            Hashtable hsData = new Hashtable();
            if (dataRow == null)
            {
                return null;
            }
            for (int i = 0; i < dataRow.Table.Columns.Count; i++)
            {
                string key = dataRow.Table.Columns[i].ColumnName;
                string val = Convert.ToString(dataRow[i]);
                if (hsData.ContainsKey(key) == false)
                {
                    hsData.Add(key, val);
                }
                else
                {
                    hsData[key] = val;
                }
            }
            return hsData;
        }
        /// <summary>
        /// 根据数据库表字段创建数据表对象。
        /// </summary>
        /// <returns>包含数据库表字段的数据表对象。</returns>
        public static DataTable CreateDataTable(TableFields fields)
        {
            DataTable dtRetrun = new DataTable();
            dtRetrun.TableName = fields.TABLE_NAME;
            foreach(string key in fields.FIELDS.Keys)
            {
               FieldProperties properties=fields.FIELDS[key];
               DataColumn dc=dtRetrun.Columns.Add(key, properties.DATATYPE);
               dc.AllowDBNull = properties.CanNull;
               dc.DefaultValue = properties.DefaultValue;
            }
            return dtRetrun;
        }
        /// <summary>
        /// 生成全球唯一字符串。
        /// </summary>
        /// <param name="i">后缀码。</param>
        /// <returns>全球唯一字符串。</returns>
        public static string GenerateNewKey(int i)
        {
            return System.Guid.NewGuid() + "-" + i.ToString("000");
        }
        /// <summary>
        /// 转换枚举类型到数据表对象。
        /// </summary>
        /// <param name="t">枚举类型。</param>
        /// <returns>包含枚举类型值名称及其描述的数据表对象。</returns>
        public static DataTable ConvertEnumTypeToDataTable(Type t)
        {
            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add(COMMON_FIELDS.FIELD_COMMON_NAME, typeof(string));
            dtReturn.Columns.Add(COMMON_FIELDS.FIELD_COMMON_VALUE, typeof(decimal));
            dtReturn.Columns.Add(COMMON_FIELDS.FIELD_COMMON_DESCRIPTION, typeof(string));

            if (t.BaseType != typeof(Enum)) return dtReturn;

            FieldInfo[] fileds = t.GetFields();

            foreach (FieldInfo f in fileds)
            {
                if (f.FieldType.BaseType != t.BaseType) continue;
                DataRow dr = dtReturn.NewRow();
                dr[COMMON_FIELDS.FIELD_COMMON_NAME]= f.Name;
                dr[COMMON_FIELDS.FIELD_COMMON_VALUE] = Convert.ToDecimal(Enum.Parse(f.FieldType, f.Name));
                dr[COMMON_FIELDS.FIELD_COMMON_DESCRIPTION]= string.Empty;
                object[] attrs = f.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    DescriptionAttribute attr = attrs[0] as DescriptionAttribute;
                    dr[COMMON_FIELDS.FIELD_COMMON_DESCRIPTION] = attr.Description;
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
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
                FieldInfo[] fileds = val.GetType().GetFields();
                foreach (FieldInfo f in fileds)
                {
                    if (f.Name == val.ToString())
                    {
                        object[] attrs = f.GetCustomAttributes(typeof(DescriptionAttribute), false);
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
    }

}
