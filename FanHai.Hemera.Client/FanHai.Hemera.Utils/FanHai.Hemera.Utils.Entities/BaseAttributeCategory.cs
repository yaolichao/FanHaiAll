/*
<FileInfo>
  <Author>Hao.Zhang, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
// ----------------------------------------------------------------------------------
// Copyright (c) FanHai
// ----------------------------------------------------------------------------------
// ==================================================================================
// 修改人               修改时间              说明
// ----------------------------------------------------------------------------------
// chao.pang          2012-02-13            添加注释 
// ==================================================================================
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
using FanHai.Hemera.Utils.StaticFuncs;


namespace FanHai.Hemera.Utils.Entities
{
    public class BaseAttributeCategory
    {
        #region Construction
        public BaseAttributeCategory()
        {
            
        }
        public BaseAttributeCategory(string categoryKey)
        {
            _categoryKey = categoryKey;
 
        }
        #endregion

        #region Private variable definition
        private string _categoryKey = "";         //组别ID      
        private string _categoryName = "";        //组别名称      
        private string _descriptions = "";     //描述       
        private string _editor = "";       //编辑者
        private string _editTime = "";         //编辑时间      
        private string _editTimeZone = "";        //编辑时间时区    
        private string _creator = "";     //创建者      
        private string _createTime = "";     //创建时间
        private string _createTimeZone = "";        //创建时间时区
        private string _errorMsg = "";        //错误消息
        
        private DataSet dsReturnValue = new DataSet();  //dataset

        #endregion

        #region Properties
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
        public string Descriptions
        {
            get { return _descriptions; }
            set { _descriptions = value; }
        }
        public string Editor
        {
            get { return _editor; }
            set { _editor = value; }
        }
        public string EditTime
        {
            get { return _editTime; }
            set 
            {
                _editTime = value;
            }
        }
        public string EditTimeZone
        {
            get { return _editTimeZone; }
            set { _editTimeZone = value; }
        }
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        public string CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        public string CreateTimeZone
        {
            get { return _createTimeZone; }
            set { _createTimeZone = value; }
        }

        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
        #endregion

        #region Actions
        #region SaveBaseCategory
        /// <summary>
        ///SaveBaseCategory
        /// </summary>
        /// <param name="bNew"></param>
        ///  <summary>
        public void SaveBaseCategory()
        {
            #region variable define
            Hashtable mainDataHashTable = new Hashtable();      //根据哈希表创建一个数据表对象 modi by chao.pang
            DataSet dataSet = new DataSet();
            DataSet dsReturn= new DataSet();
            #endregion

            #region mainDataHashTable
            mainDataHashTable.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY, _categoryKey);          //主键添加到mainDataHashTable      modi by chao.pang
            mainDataHashTable.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_NAME, _categoryName);        //类别名称添加到mainDataHashTable  modi by chao.pang
            mainDataHashTable.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CREATOR, _creator);                   //编辑人添加到mainDataHashTable    modi by chao.pang
            mainDataHashTable.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_DESCRIPTIONS, _descriptions);         //描述添加到mainDataHashTable      modi by chao.pang
            mainDataHashTable.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CREATE_TIME, _createTime);            //创建时间添加到mainDataHashTable  modi by chao.pang
            mainDataHashTable.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CREATE_TIMEZONE, _createTimeZone);    //创建时区添加到mainDataHashTable  modi by chao.pang
            #endregion

            #region add table to dataset
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable); //传入参数mainDataHashTable生成数据表到datatable modi chao.pang
            mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dataSet.Tables.Add(mainDataTable);
            #endregion

            #region detail deal
            try
            {
                //使用远程调用技术 modi by chao.pang
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                if (null != iServerObjFactory)
                {
                    //get category information 传入参数dataset返回数据表ATTRIBUTE_KEY和ATTRIBUTE_VALUE两列值 modi by chao.pang
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().AddBasicDataCategoryInfo(dataSet);
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

        #region DeleteBaseCategory
        /// <summary>
        /// Delete Lot
        /// </summary>
        public void DeleteBaseCategory()
        {
            #region variable define
            Hashtable mainDataHashTable = new Hashtable();           //根据哈希表创建一个数据表对象 modi by chao.pang
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();
            #endregion

            #region mainDataHashTable
            mainDataHashTable.Add(BASE_ATTRIBUTE_CATEGORY_FIELDS.FIELDS_CATEGORY_KEY, _categoryKey);              //主键添加到mainDataHashTable      modi by chao.pang
            #endregion

            #region add table to dataset
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);  //传入参数mainDataHashTable生成数据表到datatable modi chao.pang
            mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dataSet.Tables.Add(mainDataTable);
            #endregion

            #region detail deal
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                if (null != iServerObjFactory)
                {
                    //get category information 传入参数dataset返回数据表ATTRIBUTE_KEY和ATTRIBUTE_VALUE两列值 modi by chao.pang
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().DeleteBasicDataCategoryInfo(dataSet);
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

        #region GetBaseCategory
        /// <summary>
        /// Get information of Lot
        /// </summary>
        /// <returns>dataset</returns>
        public DataSet GetBaseCategory()
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
                    //get category information
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().GetBasicDataCategoryInfo(null);
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
        #endregion

    }
}

