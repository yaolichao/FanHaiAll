/*
<FileInfo>
  <Author>Hao.Zhang, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 冯旭                 2012-02-10            添加注释 
// chao.pang            2012-02-14            添加注释
//================================================================================
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
    public class LocationEntity : SEntity
    {
        #region Construction
        public LocationEntity()
        {

        }
        public LocationEntity(string locationKey)
        {
            _locationKey = locationKey;

        }
        #endregion

        #region Private variable definition
        private string _locationKey = "";           //区域key  
        private string _parentLocationKey = "";           //父区域key 
        private string _parentLocationLevel = "";       //父类型
        private string _locationName = "";          //区域名称       
        private string _descriptions = "";          //描述
        private string _siteNumber = "";            //site number
        private string _localLevel = "";            //类型
        private string _errorMsg = "";

        public Dictionary<string, string> _dirtyList = new Dictionary<string, string>();
        #endregion

        #region Properties
        public string ParentLocationLevel
        {
            get { return _parentLocationLevel; }
            set { _parentLocationLevel = value; }
        }
        public string ParentLocationKey
        {
            get { return _parentLocationKey; }
            set { _parentLocationKey = value; }
        }
        public string LocationKey
        {
            get { return _locationKey; }
            set { _locationKey = value; }
        }
        public string LocationName
        {
            get { return _locationName; }
            set { _locationName = value; }
        }
        public string Descriptions
        {
            get { return _descriptions; }
            set { _descriptions = value; }
        }
        public string SiteNumber
        {
            get { return _siteNumber; }
            set { _siteNumber = value; }
        }
        public string LocalLevel
        {
            get { return _localLevel; }
            set { _localLevel = value; }
        }
        public string ErrorMsg
        {
            get { return this._errorMsg; }
            set { this._errorMsg = value; }
        }

        #endregion

        /// <summary>
        /// 获取数据返回表集 modi by chao.pang
        /// </summary>
        /// <param name="level">值为0初始值,1工厂,2楼层,5车间 modi by chao.pang</param>
        /// <returns></returns>
        #region GetWorkshopBindData
        public DataSet GetWorkshopBindData(int level)
        {
            DataSet dsReturn = new DataSet();                             //创建表集,返回值  modi by chao.pang
            DataSet dsFrom = new DataSet();                               //创建表集 modi by chao.pang
            DataTable dataTable = new DataTable();                        //创建表 modi by chao.pang
            Hashtable hashTable = new Hashtable();                        //创建Hash表 modi by chao.pang
            try
            {
                //远程调用技术 modi by chao.pang
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    //结果表集,获取相关数据 modi by chao.pang
                    dsReturn = serverFactory.CreateILocationEngine().GetWorkshopBindData(level);
                    //返回错误信息 modi by chao.pang
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

        #region SaveNewLocation
        public DataSet SaveNewLocation()
        {
            #region variable define
            DataSet DSReceveServer = new DataSet();
            DataSet dataSet = new DataSet();
            Hashtable mainHashTable = new Hashtable();
            Hashtable relationHashTable = new Hashtable();
            DataTable mainTable = new DataTable();
            DataTable relationTable = new DataTable();
            #endregion

            #region add main table  of location to dataset
            //add main table  of location to dataset
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY, _locationKey);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME, _locationName);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_DESCRIPTIONS, _descriptions);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL, _localLevel);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_CREATOR, base.Creator);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_CREATE_TIME, base.CreateTime);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_CREATE_TIMEZONE, base.CreateTimeZone);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_EDITOR, base.Editor);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_EDIT_TIME, base.EditTime);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_EDIT_TIMEZONE, base.EditTimeZone);

            mainTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainHashTable);
            mainTable.TableName = FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME;
            dataSet.Tables.Add(mainTable);
            #endregion

            #region add relation table to dataset
            //if the location is "area"
            if (_localLevel == "9" || _localLevel == "5" || _localLevel == "2")
            {
                //add relation table to dataset 
                relationHashTable.Add(FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_KEY, _parentLocationKey);
                relationHashTable.Add(FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_LEVEL, _parentLocationLevel);
                relationHashTable.Add(FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_KEY, _locationKey);
                relationHashTable.Add(FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_LEVEL, _localLevel);

                relationTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(relationHashTable);
                relationTable.TableName = FMM_LOCATION_RET_FIELDS.DATABASE_TABLE_NAME;
                dataSet.Tables.Add(relationTable);
            }
            #endregion

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    DSReceveServer = serverFactory.CreateILocationEngine().SaveNewLocation(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(DSReceveServer);
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

            return DSReceveServer;
        }
        #endregion

        #region SaveUpdateLocation
        public DataSet SaveUpdateLocation()
        {
            #region variable define
            DataSet DSReceveServer = new DataSet();
            DataSet dataSet = new DataSet();
            Hashtable mainHashTable = new Hashtable();
            Hashtable relationHashTable = new Hashtable();
            DataTable mainTable = new DataTable();
            DataTable relationTable = new DataTable();
            #endregion

            #region add main table  of location to dataset
            //add main table  of location to dataset
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY, _locationKey);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME, _locationName);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_DESCRIPTIONS, _descriptions);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL, _localLevel);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_EDITOR, base.Editor);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_EDIT_TIME, base.EditTime);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_EDIT_TIMEZONE, base.EditTimeZone);

            mainTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainHashTable);
            mainTable.TableName = FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME;
            dataSet.Tables.Add(mainTable);
            #endregion

            #region add relation table to dataset
            //if the location is "area"
            if (_localLevel == "9" || _localLevel == "5" || _localLevel == "2")
            {
                //add relation table to dataset 
                relationHashTable.Add(FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_KEY, _parentLocationKey);
                relationHashTable.Add(FMM_LOCATION_RET_FIELDS.FIELD_PARENT_LOC_LEVEL, _parentLocationLevel);
                relationHashTable.Add(FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_KEY, _locationKey);
                relationHashTable.Add(FMM_LOCATION_RET_FIELDS.FIELD_LOCATION_LEVEL, _localLevel);

                relationTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(relationHashTable);
                relationTable.TableName = FMM_LOCATION_RET_FIELDS.DATABASE_TABLE_NAME;
                dataSet.Tables.Add(relationTable);
            }
            #endregion

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    DSReceveServer = serverFactory.CreateILocationEngine().UpdateLocation(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(DSReceveServer);
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

            return DSReceveServer;
        }
        #endregion

        #region DeleteLocation
        /// <summary>
        /// 调用远程对象的方法，根据区域名称和区域类型进行删除 modi by chao.pang
        /// </summary>
        /// <returns></returns>
        public DataSet DeleteLocation()
        {
            DataSet dsReturn = new DataSet();
            DataSet dataSet = new DataSet();
            Hashtable mainHashTable = new Hashtable();
            DataTable mainDataTable = new DataTable();

            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY, _locationKey);
            mainHashTable.Add(FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL, _localLevel);
            mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainHashTable);
            mainDataTable.TableName = FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME;
            dataSet.Tables.Add(mainDataTable);

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    //调用远程对象的方法，根据区域名称和区域类型为参数 modi by chao.pang
                    dsReturn = serverFactory.CreateILocationEngine().DeleteLocation(dataSet);
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

        #region SearchLocation
        /// <summary>
        /// 调用远程对象的方法，根据区域名称和区域类型查询区域信息
        /// </summary>
        /// <returns>区域信息</returns>
        public DataSet SearchLocation()
        {
            DataSet dsReturn = new DataSet();
            DataSet dsFrom = new DataSet();
            DataTable dataTable = new DataTable();
            Hashtable hashTable = new Hashtable();
            hashTable.Add(FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME, _locationName);            //将区域名称添加到LOCATION_NAME列下  modi by chao.pang
            hashTable.Add(FMM_LOCATION_FIELDS.FIELD_LOCATION_LEVEL, _localLevel);             //将类型添加到LOCATION_LEVEL列下  modi by chao.pang   
            dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);    //根据哈希表创建一个数据表对象，数据表的名称为"param"，数据表包含两个列"name"和"value"。列name存放哈希表的键名，列value存放哈希表键对应的键值。  modi by chao.pang
            dataTable.TableName = FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME;                    //表名为FMM_LOCATION modi by chao.pang
            //若区域名称不为空获取区域类型不为空
            if (_locationName != "" || _localLevel != "")
            {
                //将dataTable添加到表集dsfrom中 modi by chao.pang
                dsFrom.Tables.Add(dataTable);
            }
            try
            {
                //远程调用技术 modi by chao.pang
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    //调用远程对象的方法，根据区域名称和区域类型查询区域信息
                    dsReturn = serverFactory.CreateILocationEngine().SearchLocation(dsFrom);
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

        /// <summary>
        /// 查询结果返回后回去该条数据的信息 modi by chao.pang
        /// </summary>
        /// <returns></returns>
        public DataSet GetLocation()
        {
            DataSet dsReturn = new DataSet();
            DataSet dsFrom = new DataSet();
            DataTable dataTable = new DataTable();
            Hashtable hashTable = new Hashtable();
            hashTable.Add(FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY, _locationKey);
            dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
            dataTable.TableName = FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME;
            dsFrom.Tables.Add(dataTable);

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILocationEngine().GetLocation(dsFrom);
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

        /// <summary>
        /// 根据名称获取车间信息。
        /// </summary>
        /// <param name="name">车间名称。</param>
        /// <returns>包含车间信息的数据集。</returns>
        public DataSet GetFactoryRoom(string name)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILocationEngine().GetFactoryRoom(name);
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
    }
}