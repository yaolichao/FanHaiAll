//----------------------------------------------------------------------------------
// Copyright (c) ASTRONERGY
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-04-13            修改
// =================================================================================
#region using
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
#endregion

namespace FanHai.Hemera.Utils.Common
{
    /// <summary>
    /// 获取基础数据的工具类。
    /// </summary>
    public class BaseData
    { 
        /// <summary>
        /// 从数据库中查询基础数据。
        /// 即查询CRM_ATTRIBUTE（属性数据表）中的数据并进行行列转换，
        /// 然后将以ATTRIBUTE_NAME中的值为列，以ATTRIBUTE_VALUE的值为数据行的数据表对象返回给调用函数。
        /// </summary>
        /// <param name="columnNames">
        /// 字符串数组。
        /// 数组中的数据来源于数据库表CRM_ATTRIBUTE的ATTRIBUTE_NAME栏位中的值。
        /// </param>
        /// <param name="categoryName">基础数据分类名称。</param>
        /// <returns>包含基础数据信息的数据集对象。包含<paramref name="columnNames"/>中的列名。</returns>
        public static DataTable Get(string[] columnNames,string categoryName)
        {
            KeyValuePair<string, string> category = new KeyValuePair<string, string>(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME, categoryName);
            return Get(columnNames, category);
        }
        /// <summary>
        /// 从数据库中查询基础数据。
        /// 即查询CRM_ATTRIBUTE（属性数据表）中的数据并进行行列转换，
        /// 然后将以ATTRIBUTE_NAME中的值为列，以ATTRIBUTE_VALUE的值为数据行的数据表对象返回给调用函数。
        /// </summary>
        /// <param name="columnNames">
        /// 字符串数组。
        /// 数组中的数据来源于数据库表CRM_ATTRIBUTE的ATTRIBUTE_NAME栏位中的值。
        /// </param>
        /// <param name="baseCategory">键值对。
        /// 键数据来源于数据表 BASE_ATTRIBUTE_CATEGORY 的列名（一般设置为"CATEGORY_NAME"），
        /// 值数据来源于列对应的具体的值（一般为"CATEGORY_NAME"栏位中的值）。</param>
        /// <returns>包含基础数据信息的数据集对象。包含<paramref name="columnNames"/>中的列名。</returns>
        public static DataTable Get(string[] columnNames, 
            KeyValuePair<string, string> baseCategory)
        {
            //列集合为null，列集合没有数据，存放基础数据分类名的键值对对象的键或值无数据
            if (null == columnNames || columnNames.Length < 1 || baseCategory.Key.Length < 1 || baseCategory.Value.Length < 1)
                return null;

            DataSet baseData = new DataSet();

            //存放列名的数据表，为远程调用函数提供参数。
            DataTable columnTable = new DataTable("Columns");
            columnTable.Columns.Add("ColumnName");
            //遍历列名
            for (int i = 0; i < columnNames.Length; i++)
            {
                columnTable.Rows.Add();
                columnTable.Rows[i][0] = columnNames[i];
            }
            //存放分类名的数据表，为远程调用函数提供参数。
            DataTable categoryTable = new DataTable("Category");
            categoryTable.Columns.Add("ColumnName");
            categoryTable.Columns.Add("ColumnValue");
            categoryTable.Rows.Add();
            categoryTable.Rows[0][0] = baseCategory.Key;
            categoryTable.Rows[0][1] = baseCategory.Value;
            //将存放列名的数据表和存放分类名的数据表添加到数据集中。为远程调用函数提供参数。
            DataSet paramData = new DataSet();
            paramData.Merge(columnTable, false, MissingSchemaAction.Add);
            paramData.Merge(categoryTable, false, MissingSchemaAction.Add);

            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//工厂对象不为null
                {
                    //调用远程方法，根据分类名和列名从数据库中查询基础数据。
                    baseData = serverFactory.CreateICrmAttributeEngine().GetDistinctColumnsData(paramData);
                    //解析远程调用的结果消息。
                    string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(baseData);
                    if (msg != string.Empty)//消息不为空
                    {
                        MessageService.ShowError(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            //返回存放基础数据的数据表对象。
            return baseData.Tables[0];
        }
        /// <summary>
        /// 根据查询条件获取基础数据。
        /// </summary>
        /// <param name="columnNames">列名。</param>
        /// <param name="categoryName">基础数据分类名称。</param>
        /// <param name="whereConditons">包含查询条件的键值对集合。</param>
        /// <returns> 包含基础数据信息的数据集对象。列名参考<paramref name="columnNames"/></returns>
        public static DataTable GetBasicDataByCondition(string[] columnNames,
                                                        string categoryName,
                                                        List<KeyValuePair<string, string>> whereConditons)
        {
            KeyValuePair<string, string> category = new KeyValuePair<string, string>(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME, categoryName);
            return GetBasicDataByCondition(columnNames, category,whereConditons);
        }
        /// <summary>
        /// 根据查询条件获取基础数据。
        /// </summary>
        /// <param name="columnNames">列名。</param>
        /// <param name="categoryName">类别名的键值对。
        /// 键数据来源于数据表 BASE_ATTRIBUTE_CATEGORY 的列名（一般设置为"CATEGORY_NAME"），
        /// 值数据来源于列对应的具体的值（一般为"CATEGORY_NAME"栏位中的值）。
        /// </param>
        /// <param name="whereConditons">包含查询条件的键值对集合。</param>
        /// <returns> 包含基础数据信息的数据集对象。列名参考<paramref name="columnNames"/></returns>
        public static DataTable GetBasicDataByCondition(string[] columnNames, 
            KeyValuePair<string, string> baseCategory, 
            List<KeyValuePair<string, string>> whereConditons)
        {
            if (null == columnNames || columnNames.Length < 1 || baseCategory.Key.Length < 1 || baseCategory.Value.Length < 1)
                return null;

            DataSet paramData = new DataSet();
            DataSet baseData = new DataSet();
            DataTable columnTable = new DataTable(BASIC_CONST.PARAM_TABLENAME_COLUMNS);
            columnTable.Columns.Add(BASIC_CONST.PARAM_COL_COLUMN_NAME);

            foreach (string columnName in columnNames)
            {
                columnTable.Rows.Add();
                columnTable.Rows[columnTable.Rows.Count - 1][BASIC_CONST.PARAM_COL_COLUMN_NAME] = columnName;
            }


            DataTable categoryTable = new DataTable(BASIC_CONST.PARAM_TABLENAME_CATEGORY);
            categoryTable.Columns.Add(BASIC_CONST.PARAM_COL_COLUMN_NAME);
            categoryTable.Columns.Add(BASIC_CONST.PARAM_COL_COLUMN_VALUE);
            categoryTable.Rows.Add();
            categoryTable.Rows[0][BASIC_CONST.PARAM_COL_COLUMN_NAME] = baseCategory.Key;
            categoryTable.Rows[0][BASIC_CONST.PARAM_COL_COLUMN_VALUE] = baseCategory.Value;

            paramData.Merge(columnTable, false, MissingSchemaAction.Add);
            paramData.Merge(categoryTable, false, MissingSchemaAction.Add);

            if (null != whereConditons || whereConditons.Count > 0)
            {
                DataTable conditionTable = new DataTable(BASIC_CONST.PARAM_TABLENAME_CONDITIONS);
                conditionTable.Columns.Add(BASIC_CONST.PARAM_COL_COLUMN_NAME);
                conditionTable.Columns.Add(BASIC_CONST.PARAM_COL_COLUMN_VALUE);

                foreach (KeyValuePair<string, string> condition in whereConditons)
                {
                    conditionTable.Rows.Add();
                    conditionTable.Rows[conditionTable.Rows.Count - 1][BASIC_CONST.PARAM_COL_COLUMN_NAME] = condition.Key;
                    conditionTable.Rows[conditionTable.Rows.Count - 1][BASIC_CONST.PARAM_COL_COLUMN_VALUE] = condition.Value;
                }
                paramData.Merge(conditionTable, false, MissingSchemaAction.Add);
            }

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    baseData = serverFactory.CreateICrmAttributeEngine().GetBasicDataByConditons(paramData);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            return baseData.Tables[0];
        }
    }
}
