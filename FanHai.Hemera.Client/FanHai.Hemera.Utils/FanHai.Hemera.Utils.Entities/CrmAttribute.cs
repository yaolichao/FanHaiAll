/*
<FileInfo>
  <Author>Hao.Zhang, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Utils.Entities
{
    public class CrmAttribute
    {
        #region Construction
        public CrmAttribute()
        {
            
        }
        public CrmAttribute(string attributeKey)
        {
            //_attributeKey = attributeKey;
        }
        #endregion

        #region Private variable definition

        private string _attributeKey = "";          //属性ID
        private string _attributeName = "";         //属性名称  
        private string _attributeValue = "";        //属性值
        private string _itemOrder = "";             //项序列
        private string _editor = "";                //编辑者       
        private string _editTime = "";              //编辑时间            
        private string _editTimeZone = "";          //编辑时间时区     
        private string _errorMsg = "";              //错误消息
        private string _categoryKey = "";           //category key
        private string _categoryName = "";           //category name
        private DataSet _datasetValue = new DataSet();  //used to store data of insert and delete
        private object _myCategory =string.Empty;     // parameter to pass

        #endregion

        #region Properties
        public object MyCategory
        {
            get { return _myCategory; }
            set { _myCategory = value; }
        }
        public DataSet DatasetValue
        {
            get { return _datasetValue; }
            set { _datasetValue = value; }
        }
        public string CategoryKey
        {
            get { return _categoryKey; }
            set { _categoryKey = value; }
        }
        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; }
        }
        public string AttributeKey
        {
            get { return _attributeKey; }
            set { _attributeKey = value; }
        }
        public string AttributeName
        {
            get { return _attributeName; }
            set { _attributeName = value; }
        }
        public string AttributeValue
        {
            get { return _attributeValue; }
            set { _attributeValue = value; }
        }
        public string ItemOrder
        {
            get { return _itemOrder; }
            set { _itemOrder = value; }
        }
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
        public string Editor
        {
            get { return _editor; }
            set { _editor = value; }
        }
        public string EditTime
        {
            get { return _editTime; }
            set { _editTime = value; }
        }
        public string EditTimeZone
        {
            get { return _editTimeZone; }
            set { _editTimeZone = value; }
        }
        #endregion

        #region Actions

        #region SaveCrmAttribute
        /// <summary>
        ///SaveCrmAttribute
        /// </summary>
        /// <param name="bNew"></param>
        public DataSet SaveCrmAttribute()
        {
            #region variable define
            DataSet dsReturn = new DataSet();
            #endregion

            #region detail deal
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                if (null != iServerObjFactory)
                {
                    //SaveCrmAttribute
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().SaveBasicData(_datasetValue);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            #endregion

            return dsReturn;
        }
        #endregion

        #region DeleteCrmAttribute
        /// <summary>
        /// DeleteCrmAttribute
        /// </summary>
        public void DeleteCrmAttribute()
        {
            #region variable define
            Hashtable mainDataHashTable = new Hashtable();
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();
            #endregion

            #region mainDataHashTable
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY, _attributeKey);
            #endregion

            #region add table to dataset
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dataSet.Tables.Add(mainDataTable);
            #endregion

            #region detail deal
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                if (null != iServerObjFactory)
                {
                    //DeleteCrmAttribute
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().DeleteBasicDataColumnInfo(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            #endregion
        }
        #endregion

        #region GetCrmAttribute
        /// <summary>
        /// GetCrmAttribute
        /// </summary>
        /// <returns>dataset</returns>
        public DataSet GetCrmAttribute()
        {
            #region variable define
            Hashtable mainDataHashTable = new Hashtable();
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();
            #endregion

            #region mainDataHashTable
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY, _categoryKey);
            #endregion

            #region add table to dataset
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dataSet.Tables.Add(mainDataTable);
            #endregion

            #region detail deal
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                if (null != iServerObjFactory)
                {
                    //GetCrmAttribute
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().GetBasicDataColumnIInfo(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            #endregion

            return dsReturn;

        }
        #endregion

        #region UpdateCrmAttribute
        /// <summary>
        /// UpdateCrmAttribute
        /// </summary>
        public void UpdateCrmAttribute()
        {
            #region variable define
            Hashtable mainDataHashTable = new Hashtable();
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();

            #endregion

            #region mainDataHashTable
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY, _attributeKey);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME, _attributeName);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY, _categoryKey);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE, "");
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_ORDER, "");
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_EDITOR, _editor);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_DESCRIPTIONS, "");
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_EDIT_TIME, _editTime);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_EDIT_TIMEZONE, _editTimeZone);
            #endregion

            #region add table to dataset
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dataSet.Tables.Add(mainDataTable);
            #endregion

            #region detail deal
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                if (null != iServerObjFactory)
                {
                    //UpdateCrmAttribute
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().UpdateBasicDataColumnInfo(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            #endregion
        }
        #endregion

        #region getSomeGroup's columns
        public DataSet GetGroupColumns()
        {
            #region variable define
            Hashtable mainDataHashTable = new Hashtable();
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();
            #endregion

            #region mainDataHashTable
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY, _categoryKey);
            #endregion

            #region add table to dataset
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dataSet.Tables.Add(mainDataTable);
            #endregion

            #region detail deal
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                if (null != iServerObjFactory)
                {
                    //get columns
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().GetColumns(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            #endregion

            return dsReturn;

        }
        
        #endregion

        #region get all basic data
        public DataSet GetAllData()
        {
            #region variable define
            Hashtable mainDataHashTable = new Hashtable();
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();
            #endregion

            #region mainDataHashTable
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY, _categoryKey);
            mainDataHashTable.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME, _categoryName);
            #endregion

            #region add table to dataset
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dataSet.Tables.Add(mainDataTable);
            #endregion

            #region detail deal
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                if (null != iServerObjFactory)
                {
                    //GetAllData
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().GetAllData(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            #endregion

            return dsReturn;

        }

        #endregion

        #region GetGruopBasicData
        public DataSet GetGruopBasicData()
        {
            #region variable define
            Hashtable mainDataHashTable = new Hashtable();
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();
            #endregion

            #region mainDataHashTable
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY, _categoryKey);
            mainDataHashTable.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME, _categoryName);
            #endregion

            #region add table to dataset
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dataSet.Tables.Add(mainDataTable);
            #endregion

            #region detail deal
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                if (null != iServerObjFactory)
                {
                    //GetGruopBasicData
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().GetGruopBasicData(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            #endregion

            return dsReturn;

        }

        #endregion

        #region GetDistinctColumnsData
        public DataSet GetDistinctColumnsData(DataSet dataSet)
        {
            #region variable define
            Hashtable mainDataHashTable = new Hashtable();
            DataSet dsReturn = new DataSet();
            #endregion

            //#region mainDataHashTable
            //mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY, _categoryKey);
            //mainDataHashTable.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME, _categoryName);
            //#endregion

            //#region add table to dataset
            //DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            //mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            //dataSet.Tables.Add(mainDataTable);
            //#endregion

            #region detail deal
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                if (null != iServerObjFactory)
                {
                    //GetDistinctColumnsData
                    dataSet.Tables[0].TableName = "Columns";
                    dataSet.Tables[1].TableName = "Category";
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().GetDistinctColumnsData(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            #endregion

            return dsReturn;

        }

        #endregion

        #region DeleteBasicData
        /// <summary>
        /// DeleteCrmAttribute
        /// </summary>
        public DataSet DeleteBasicData()
        {
            DataSet dsReturn = new DataSet();
            
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                if (null != iServerObjFactory)
                {
                    //DeleteBasicData
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().DeleteBasicData(_datasetValue);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            return dsReturn;
        }
        #endregion


        #region GetAttributsColumnsForSomeCategory
        public DataSet GetAttributsColumnsForSomeCategory()
        {
            #region variable define
            Hashtable mainDataHashTable = new Hashtable();
            DataSet dsReturn = new DataSet();
            DataSet dataSet = new DataSet();
            #endregion

            #region mainDataHashTable
            mainDataHashTable.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME, _myCategory);
            #endregion

            #region add table to dataset
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = BASE_ATTRIBUTE_CATEGORY_FIELDS.DATABASE_TABLE_NAME;
            dataSet.Tables.Add(mainDataTable);
            #endregion

            #region detail deal
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                if (null != iServerObjFactory)
                {
                    //GetGruopBasicData
                    
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().GetAttributsColumnsForSomeCategory(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            #endregion

            return dsReturn;

        }

        #endregion

        /// <summary>
        /// 获取线上仓数据集合。
        /// </summary>
        /// <returns>
        /// 包含线上仓数据的数据集对象。
        /// [STORE_KEY, STORE_NAME,STORE_TYPE]
        /// </returns>
        public DataTable GetStoreName()
        {
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = factor.CreateIStoreEngine().GetStoreName();
                _errorMsg= FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (string.IsNullOrEmpty(_errorMsg))
                {
                    return dsReturn.Tables[0];
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }

        public DataTable GetEDC()
        {
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    DataSet dsReturn = factor.CreateIEDCEngine().SearchEdcMain(null);
                    string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        return dsReturn.Tables[EDC_MAIN_FIELDS.DATABASE_TABLE_NAME];
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
            return null;
        }
        #endregion

       
    }
}


