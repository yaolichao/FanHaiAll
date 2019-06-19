/*
<FileInfo>
  <Author>Hao.Zhang, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
/// ================================================================================
/// 修改人               修改时间              说明
/// --------------------------------------------------------------------------------
/// chao.pang            2012-02-16            添加注释
/// ================================================================================
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
    public class LocationLine : SEntity
    {
        #region Construction
        public LocationLine()
        {

        }
        public LocationLine(string locationKey)
        {
            _locationKey = locationKey;

        }
        #endregion

        #region Private variable definition
        private string _locationKey = "";           //区域key  
        private string _lineKey = "";          //线别KEY   
        private string _lineName = "";          //线别Name
        private string _errorMsg = "";

        public Dictionary<string, string> _dirtyList = new Dictionary<string, string>();
        #endregion

        #region Properties
        public string LineName
        {
            get { return _lineName; }
            set { _lineName = value; }
        }
        public string LocationKey
        {
            get { return _locationKey; }
            set { _locationKey = value; }
        }
        public string LineKey
        {
            get { return _lineKey; }
            set { _lineKey = value; }
        }
        public string ErrorMsg
        {
            get { return this._errorMsg; }
            set { this._errorMsg = value; }
        }

        #endregion

        #region Actions

        #region GetLinesAndLocationLine
        public DataSet GetLinesAndLocationLine()
        {
            #region variable define
            DataSet dsReturn = new DataSet();
            DataSet dsFrom = new DataSet();
            DataTable dataTable = new DataTable();
            Hashtable hashTable = new Hashtable();
            hashTable.Add(FMM_LOCATION_LINE_FIELDS.FIELD_LOCATION_KEY, _locationKey);
            dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
            dataTable.TableName=FMM_LOCATION_LINE_FIELDS.DATABASE_TABLE_NAME;
            dsFrom.Tables.Add(dataTable);
            #endregion

            #region detail deal
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILocationEngine().GetLinesAndLocationLine(dsFrom);
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

            #endregion
        }
        #endregion

        #region SaveLocationLineRelation
        public DataSet SaveLocationLineRelation(DataSet dataset)
        {
            #region variable define
            DataSet dsReturn = new DataSet();
            #endregion

            #region detail deal
            try
            {
                //远程调用技术 modi by chao.pang
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILocationEngine().SaveLocationLineRelation(dataset);
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

            #endregion
        }
        #endregion

        public override string ToString()
        {
            return _lineName;
        }
        #endregion


    }
}