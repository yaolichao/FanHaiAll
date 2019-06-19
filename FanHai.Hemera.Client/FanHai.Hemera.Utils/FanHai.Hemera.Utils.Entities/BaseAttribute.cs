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


namespace FanHai.Hemera.Utils.Entities
{
    public class BaseAttribute
    {
        #region Construction
        public BaseAttribute()
        {
            
        }
        public BaseAttribute(string baseAttributeKey)
        {
            _attributeKey = baseAttributeKey;
 
        }
        #endregion

        #region Private variable definition

        private string _attributeKey = "";          //属性ID
        private string _attributeName = "";         //属性名称
        private string _descriptions = "";          //描述       
        private string _dataType = "";              //属性数据类型
        private string _defaultValue = "";          //默认值
        private string _attributeUnit = "";         //单位
        private string _validationType = "";        //属性类型：None、MinMax、Set     
        private string _creator = "";               //创建者      
        private string _createTime = "";            //创建时间     
        private string _createTimeZone = "";        //创建者时区
        private string _editor = "";                //编辑者       
        private string _editTime = "";              //编辑时间            
        private string _editTimeZone = "";          //编辑时间时区
        private string _categoryKey = "";           //组别     
        private string _attributeOrder = "";        //属性列表顺序
        private string _isPriaryKey = "";           //是否为主键       
        private string _errorMsg = "";              //错误消息

        #endregion

        #region Properties
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
        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }
        public string AttributeUnit
        {
            get { return _attributeUnit; }
            set 
            {
                _attributeUnit = value;
            }
        }
        public string ValidationType
        {
            get { return _validationType; }
            set { _validationType = value; }
        }
        public string CategoryKey
        {
            get { return _categoryKey; }
            set { _categoryKey = value; }
        }
        public string AttributeOrder
        {
            get { return _attributeOrder; }
            set { _attributeOrder = value; }
        }
        public string IsPriaryKey
        {
            get { return _isPriaryKey; }
            set { _isPriaryKey = value; }
        }
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
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
        public string Descriptions
        {
            get { return _descriptions; }
            set { _descriptions = value; }
        }
   

     
        
        
        #endregion

        #region Actions

        #region SaveBaseAttribute
        /// <summary>
        ///SaveBaseAttribute
        /// </summary>
        /// <param name="bNew"></param>
        /// 基础表新增列视图保存数据 modi by chao.pang
        public void SaveBaseAttribute()
        {
            #region variable define
            Hashtable mainDataHashTable = new Hashtable();   //根据哈希表创建一个数据表对象 modi by chao.pang
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();
            #endregion

            //添加数据到hashtable modi by chao.pang
            #region mainDataHashTable
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY, _attributeKey);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME, _attributeName);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_CATEGORY_KEY, _categoryKey);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE,_dataType);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_ORDER,_attributeOrder);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_CREATOR, _creator);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_DESCRIPTIONS, _descriptions);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_CREATE_TIME, _createTime);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_CREATE_TIMEZONE, _createTimeZone);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_EDITOR, _editor);
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
                //使用远程调用技术 modi by chao.pang
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                if (null != iServerObjFactory)
                {
                    //get category information 通过远程调用调用server端的AddBasicDataColumnInfo方法添加数据 mido by chao.pang
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().AddBasicDataColumnInfo(dataSet);
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

        #region DeleteBaseAttribute
        /// <summary>
        /// DeleteBaseAttribute
        /// </summary>
        public void DeleteBaseAttribute()
        {
            #region variable define
            Hashtable mainDataHashTable = new Hashtable();          //根据哈希表创建一个数据表对象 modi by chao.pang
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();
            #endregion

            #region mainDataHashTable
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY, _attributeKey);    //将参数_attributeKey值传入哈希表中 modi by chao.pang
            #endregion

            #region add table to dataset
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);   
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
                    //get category information 返回到table中 modi by chao.pang
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().DeleteBasicDataColumnInfo(dataSet);
                    //获取错误信息 modi by chao.pang
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

        #region GetBaseAttribute
        /// <summary>
        /// GetBaseAttribute
        /// </summary>
        /// <returns>dataset</returns>
        public DataSet GetBaseAttribute()
        {
            #region variable define
            Hashtable mainDataHashTable = new Hashtable();                  //
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
                    //get category information
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

        #region UpdateBaseAttribute
        /// <summary>
        /// UpdateBaseAttribute
        /// </summary>
        public void UpdateBaseAttribute()
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
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE, _dataType);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_ORDER, _attributeOrder);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_EDITOR, _editor);
            mainDataHashTable.Add(BASE_ATTRIBUTE_FIELDS.FIELDS_DESCRIPTIONS, _descriptions);
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
                    //get category information
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

        #endregion

        #region GetColumnInfoByAttributeKey
        public DataSet GetColumnInfoByAttributeKey()
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
                    //get category information
                    dsReturn = iServerObjFactory.CreateICrmAttributeEngine().GetColumnInfoByAttributeKey(dataSet);
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
    }
}

