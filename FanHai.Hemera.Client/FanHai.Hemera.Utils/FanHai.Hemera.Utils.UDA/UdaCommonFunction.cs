using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using System.Collections;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;



namespace FanHai.Hemera.Utils.UDA
{
    public static class UdaCommonFunction
    {
        public static DataSet GetUdaDataSetOfSomeType(string categoryKey)
        {
            #region variable define

            DataSet dataSetFromAll = new DataSet(); //all data to pass
            DataSet dataSetBackAll = new DataSet(); //all data to receive
            DataSet dataSetFromGroupInfo = new DataSet();   //group info to pass
            DataSet dataSetBackGroupInfo = new DataSet();   //group info to receive
            Hashtable mainDataHashTable = new Hashtable();
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();

            #endregion

            try
            {
                //UnregisterChannel
                CallRemotingService.UnregisterChannel();
                //get server object factory
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                //get dynamic dataset 两列ATTRIBUTE_KEY，ATTRIBUTE_VALUE
                dataSetFromAll = AddinCommonStaticFunction.GetTwoColumnsCommonDs();
                //add data to datatable 添加数据到数据集的第一张表的第一行  为CATEGORY_KEY和61 modi by chao.pang
                dataSetFromAll.Tables[0].Rows.Add();
                dataSetFromAll.Tables[0].Rows[0][0] = "CATEGORY_KEY";
                dataSetFromAll.Tables[0].Rows[0][1] = categoryKey;     //61
                //call remoting check
                if (iServerObjFactory != null)
                {
                    mainDataHashTable = new Hashtable();
                    dataSet = new DataSet();
                    #region mainDataHashTable
                    //两行两列hash表
                    mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY, categoryKey);      //第一行CATEGORY_KEY    61
                    mainDataHashTable.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME, "");     //第二行CATEGORY_NAME   空值   modi by chao.Pang
                    #endregion

                    #region add table to dataset
                    DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);   //添加到maindatatable中
                    mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;                             //表名   MAINDATA
                    dataSet.Tables.Add(mainDataTable);                                                  //数据集中添加MAINDATA表
                    #endregion

                    //get result of excute sql
                    dataSetBackAll = iServerObjFactory.CreateICrmAttributeEngine().GetAllData(dataSet);  //重3张基础数据表中获取数据 modi by chao.pang
                }
                //if there are datas
                if (dataSetBackAll.Tables.Count == 2)
                {//返回表数量为2 modi by chao.pang
                    if (dataSetBackAll.Tables[0].Rows.Count > 0)
                    {//第一张表返回数据不为空 modi by chao.Pang
                        mainDataHashTable = new Hashtable();
                        dataSet = new DataSet();
                        #region mainDataHashTable
                        mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY, categoryKey);    //第一行CATEGORY_KEY    61
                        mainDataHashTable.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME, "");   //第二行CATEGORY_NAME   空值   modi by chao.Pang
                        #endregion

                        #region add table to dataset
                        DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
                        mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                        dataSet.Tables.Add(mainDataTable);
                        #endregion

                        //get result of excute sql
                        dataSetBackAll = iServerObjFactory.CreateICrmAttributeEngine().GetGruopBasicData(dataSet);

                        //remove parameter table 
                        dataSetBackAll.Tables.Remove(dataSetBackAll.Tables[1]);
                        //add parameter to datasetfromall
                        dataSetBackAll.Merge(dataSetFromAll.Tables[0], false, MissingSchemaAction.Add);
                        dataSetBackAll.Tables[0].TableName = "Columns";
                        dataSetBackAll.Tables[1].TableName = "Category";
                        //get some group info
                        dataSetBackGroupInfo = iServerObjFactory.CreateICrmAttributeEngine().GetDistinctColumnsData(dataSetBackAll);
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                //UnregisterChannel
                CallRemotingService.UnregisterChannel();
            }
            return dataSetBackGroupInfo;
        }
    }
}
