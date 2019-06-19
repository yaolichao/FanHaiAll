/*
<FileInfo>
  <Author>Rayna Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;



namespace FanHai.Hemera.Utils.Entities
{
    public class Line : EntityObject
    {        
        #region define attribute
        private string _lineKey="";
        private string _lineName = "";
        private string _lineCode = "";
        private string _descriptions = "";       
        private string _errorMsg = "";       
        #endregion

        public string LineKey
        {
            get
            {
                return this._lineKey;
            }
            set
            {
                this._lineKey = value;
            }
        }
        public string LineName
        {
            get
            {
                return this._lineName;
            }
            set
            {
                this._lineName = value;
                ValidateDirtyList(FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME,value);
            }
        }
        public string LineCode
        {
            get
            {
                return this._lineCode;
            }
            set
            {
                this._lineCode = value;
                ValidateDirtyList(FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_CODE,value);
            }
        }

        public string Descriptions
        {
            get
            {
                return this._descriptions;
            }
            set
            {
                this._descriptions = value;
                ValidateDirtyList(FMM_PRODUCTION_LINE_FIELDS.FIELD_DESCRIPTIONS,value);
            }
        }
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                return this._errorMsg;
            }
            set
            {
                this._errorMsg = value;
            }
        }


        #region Construct function
        public Line()
        {

        }
         /// <summary>
        /// One param construct function
        /// </summary>
        public Line(string lineKey)
        {
            this._lineKey = lineKey;
        }
        #endregion

        #region Action
        #region Insert
        public override bool Insert()
        {           
            DataSet dataSet = new DataSet();
            DataTable lineTable = DataTableHelper.CreateDataTableForInsertLine();
            Dictionary<string, string> dataRow = new Dictionary<string, string>()
                                                {
                                                     {FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY,_lineKey},
                                                     {FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME,_lineName},
                                                     {FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_CODE,_lineCode},
                                                     {FMM_PRODUCTION_LINE_FIELDS.FIELD_DESCRIPTIONS,_descriptions},
                                                     {COMMON_FIELDS.FIELD_COMMON_CREATOR,Creator},
                                                     {COMMON_FIELDS.FIELD_COMMON_CREATE_TIMEZONE,CreateTimeZone}
                                                };
            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref lineTable, dataRow);
            dataSet.Tables.Add(lineTable);
            try
            {
                 DataSet dsReturn = null;               
                 IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                 if (null != serverFactory)
                 {
                     dsReturn = serverFactory.CreateILineManageEngine().AddLine(dataSet);
                     _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                     if (_errorMsg != "")
                     {
                         MessageService.ShowError(_errorMsg);
                         return false;
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
            return true;
        }
        #endregion

        #region Update
        public override bool Update()
        {           
            if (IsDirty)
            {
                DataSet dataSet = new DataSet();
                if (this.DirtyList.Count > 0)
                {
                    DataTable lineTable = DataTableHelper.CreateDataTableForUpdateBasicData(FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME);

                    foreach (string Key in this.DirtyList.Keys)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _lineKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[Key].FieldNewValue}
                                                    };
                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref lineTable, rowData);
                    }
                    if (lineTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(lineTable);
                    }
                }
                try
                {
                    DataSet dsReturn = null;
                    IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                    if (null != serverFactory)
                    {
                        dsReturn = serverFactory.CreateILineManageEngine().UpdateLine(dataSet);
                        _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        if (_errorMsg != "")
                        {
                            MessageService.ShowError(_errorMsg);
                            return false;
                        }
                        else
                        {
                            this.ResetDirtyList();
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
            }
            else
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.FMM.Msg.UpdateError}", "${res:Global.SystemInfo}");
            }
            return true;
        }
        #endregion

        #region Delete
        public override bool Delete()
        {           
            try
            {
                DataSet dsReturn = null;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILineManageEngine().DeleteLine(_lineKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (_errorMsg != "")
                    {
                        MessageService.ShowError(_errorMsg);
                        return false;
                    }
                    else
                    {
                        this.ClearData();
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
            return true;
        }
        #endregion

        #region GetLines
        public DataSet GetLines()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILineManageEngine().GetLine(_lineKey);
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
        /// 根据线别名称获取线别信息。
        /// </summary>
        /// <param name="lines">使用“逗号(,)”分开的线别名称字符串。“X01,X02,C01...”</param>
        /// <returns>包含线别信息的数据集对象。
        /// 该数据集对象包含两个数据表对象。
        /// 一个是包含线别信息的数据表对象，数据表中包含3列：PRODUCTION_LINE_KEY,LINE_NAME,LINE_CODE。
        /// 一个名称为paraTable的数据表对象。paraTable的数据表对象存放查询的执行结果，
        /// paraTable数据表对象中包含两列“ATTRIBUTE_KEY”和“ATTRIBUTE_VALUE”。
        /// 最多包含两行：“ATTRIBUTE_KEY”列等于“CODE”的行和“ATTRIBUTE_KEY”列等于“MESSAGE”的行。
        /// </returns>
        /// comment by peter 2012-2-12
        public DataSet GetLinesInfo(string lines)
        {
            DataSet dsLines = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsLines = serverFactory.CreateILineManageEngine().GetLinesInfo(lines);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsLines);
                }
            }
            catch(Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsLines;
                 
        }
        /// <summary>
        /// 根据线别名称和工厂车间主键获取线别信息。
        /// </summary>
        /// <param name="factoryRoomKey">工厂车间主键。</param>
        /// <param name="lines">使用“逗号(,)”分开的线别名称字符串。“X01,X02,C01...”</param>
        /// <returns>
        /// 包含线别信息的数据集对象。
        /// [LINE_NAME,PRODUCTION_LINE_KEY,LINE_CODE,ROOM_NAME,ROOM_KEY]
        /// </returns>
        public DataSet GetLinesInfo(string factoryRoomKey, string lines)
        {
            DataSet dsLines = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsLines = serverFactory.CreateILineManageEngine().GetLinesInfo(factoryRoomKey,lines);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsLines);
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
            return dsLines;
        }

        /// <summary>
        /// 根据工厂车间获取线别名称
        /// </summary>
        /// <param name="factoryroom"></param>
        /// <returns></returns>
        /// comment by jing.xie 2012-3-30
        public DataSet GetLinesByFactoryRoom(string factoryroom)
        {
            DataSet dsLines = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsLines = serverFactory.CreateILineManageEngine().GetLinesByFactoryRoom(factoryroom);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsLines);
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
            return dsLines;

        }


        #region Get Line Info Contain Empty
        public DataSet GetLinesInfoContainEmpty(string userKey)
        {
            DataSet dsLines = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsLines = serverFactory.CreateILineManageEngine().GetLinesInfoContainEmpty(userKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsLines);
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
            return dsLines;

        }

        #endregion

        #endregion
    }
}
