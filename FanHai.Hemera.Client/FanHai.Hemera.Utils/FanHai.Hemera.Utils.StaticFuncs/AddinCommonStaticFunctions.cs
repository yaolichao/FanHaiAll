using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.StaticFuncs
{
    public static class AddinCommonStaticFunction
    {
        //终检数据输入送检人（全局变量）
        public static string cChecker;

        /// <summary>
        /// 获取数据集对象。该数据集包含一个数据表，数据表中包含两列。
        /// </summary>
        /// <returns>
        /// 数据集对象。数据集包含一个数据表，数据表中包含两列：<see cref="COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_KEY"/>
        /// 和<see cref="COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE"/>。
        /// </returns>
        /// comment by peter 2012-2-20
        public static DataSet GetTwoColumnsCommonDs()
        {
            //define dataset
            DataSet dataSet = new DataSet();
            //define datatable
            DataTable dataTable = new DataTable();
            //define datacolumn
            DataColumn dataColumnKey = new DataColumn(COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_KEY);
            //define datacolumn
            DataColumn dataColumnValue = new DataColumn(COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE);

            //add datacolumn to datatable
            dataTable.Columns.Add(dataColumnKey);
            dataTable.Columns.Add(dataColumnValue);
            //add datatable to dataset
            dataSet.Tables.Add(dataTable);
            //return dataset
            return dataSet;
        }

        /// <summary>
        /// 获取数据表对象。数据表中包含两列。
        /// </summary>
        /// <returns>
        /// 数据表对象，数据表中包含两列：<see cref="COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_KEY"/>
        /// 和<see cref="COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE"/>。
        /// </returns>
        /// Owner:Andy Gao 2010-07-07 15:32:18
        /// comment by peter 2012-2-20
        public static DataTable GetTwoColumnsDataTable()
        {
            DataTable dataTable = new DataTable();
            //新增两列 KEY VALUE modi by chao.pang
            dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_KEY);
            dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE);

            return dataTable;
        }
        /// <summary>
        /// Get Dynamic constructor DataSet
        /// </summary>
        /// <returns>dataset</returns>
        public static DataSet GetThreeColumnsCommonDs()
        {
            //define dataset
            DataSet dataSet = new DataSet();
            //define datatable
            DataTable dataTable = new DataTable();
            //define datacolumn
            DataColumn dataColumnKey = new DataColumn("ATTRIBUTE_KEY");
            //define datacolumn
            DataColumn dataColumnValue = new DataColumn("ATTRIBUTE_VALUE");
            //define datacolumn
            DataColumn dataColumnType = new DataColumn("DATA_TYPE");

            //add datacolumn to datatable
            dataTable.Columns.Add(dataColumnKey);
            dataTable.Columns.Add(dataColumnValue);
            dataTable.Columns.Add(dataColumnType);
            //add datatable to dataset
            dataSet.Tables.Add(dataTable);
            //return dataset
            return dataSet;
        }
        /// <summary>
        /// SetDataTypeAccordingToAttributeName
        /// </summary>
        /// <param name="dtDatatype">data type's info</param>
        /// <param name="dtData">datatable need update data_type info</param>
        public static void SetDataTypeAccordingToAttributeName(DataTable dtDatatype,ref DataTable dtData)
        {
            //get count of data_type ifno
            int dtCount = dtDatatype.Rows.Count;
            //get count of data
            int dtDataCount = dtData.Rows.Count;
            //check count
            if (dtCount <= 0 || dtDataCount <= 0)
            {
                return;
            }
            //update data's data_type
            for (int i = 0; i < dtDataCount; i++)
            {
                for (int j = 0; j < dtCount; j++)
                {
                    if (dtDatatype.Rows[j]["ATTRIBUTE_NAME"].ToString() == dtData.Rows[i]["ATTRIBUTE_NAME"].ToString())
                    {
                        //set data_type info
                        dtData.Rows[i]["DATA_TYPE"] = dtDatatype.Rows[j]["DATA_TYPE"].ToString();
                        break;
                    }
                }
            }
        }
    }
}
