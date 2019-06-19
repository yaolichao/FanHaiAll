/*
<FileInfo>
  <Author>donnie.hu, SolarViewer Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 SolarViewer. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Utils.Common;
using SolarViewer.Hemera.Share.Interface;
using SolarViewer.Gui.Core;
using System.Data;

namespace SolarViewer.Hemera.Utils.Entities
{
    public class LotSplitMerge
    {
        #region Private variable definition

        #region setting por_lot variable definition

        private string _lotKey = "-1";         //批次ID               
        private string _lotNumber = "";        //批次编码              
        private string _workOrderKey = "";     //工单ID               
        private string _partVerKey = "";       //产品版本ID                
        private string _priority = "";         //优先级，最高 1 -> 10 最低，默认 5               
        private string _enterTime = "";        //批次创建时间               
        private string _finishedTime = "";     //批次结束时间               
        private string _promisedTime = "";     //担保完成时间               
        private string _closeTime = "";        //批次关闭时间               
        private string _shipTime = "";         //出货时间               
        private string _isReworked = "";       //是否曾经返工              
        private string _isSplited = "";        //是否曾经分批                      
        private string _templateKey = "";            //批次模板ID               
        private string _routeEnterpriseVerKey = "";  //集团途程版本ID        
        private string _curProductionLineKey = "";   //当前产线ID                
        private string _curRouteVerKey = "";   //当前途程版本ID               
        private string _curStepVerKey = "";    //当前工序版本ID              
        private string _nextRouteVerKey = "";  //下一步途程版本ID               
        private string _nextStepVerKey = "";   //下一步工序版本ID             
        private string _templateName = "";     //批次模板名称                
        private string _templateVersion = "";  //批次模板版本号               
        private string _isMainLot = "";       //0:否,1:是               
        private string _status = "";          //工序状态：1 = Active, 0 = InActive, 2 = Archive        
        private string _lineName = "";        //线别名称        
        private string _shiftName = "";       //班别        
        private string _namingRuleName = "";  //Naming rule        
        private string _description = "";     //备注   
        private string _workorderSeq = "";     //当前批次在WorkOrder的序列号  
        private string _type = "";          //类型        
        private string _module = "";        //规格        
        private string _quantity = "";      //数量        
        private string _enterpriseName = "";    //enterprise name        
        private string _routeName = "";    //route name        
        private string _stepName = "";    //step name        
        private string _enterpriseVersion = ""; //enterprise version         
        private string _holdFlag = ""; //0:否,1:是        
        private string _stateFlag = ""; //0:WaitingForTrackIn,4:WaitingForEDC,5:InEDC,9:WaitingForTrackout
        private string _edcInsKey = ""; //InEDC的时候才会有值，用于关联EDC_MAIN_INS表  

        #endregion        

        #region setting wip_transaction variable definition

        private string _pieceType = "0"; //0:Lot,1:Wafer        
        private string _pieceKey = "";   //批次号        
        private string _activity = "";     //TrackIn,TrackOut,Hold,Release         
        private string _comment = ""; //备注        
        private string _quantityIn = "";//Lot Quantity In        
        private string _quantityOut = "";//Lot Quantity Out        

        #endregion

        #region setting wip_split variable definition

        private string _splitTransactionkey = ""; //母批split transaction key        
        private string _splitChildCreateKey = "";  //子批Create key        
        private string _splitChildTransactionkey = ""; //子批split transaction key        
        private string _splitChildQty = ""; //split qty        
        private string _splitCurrentStepKey = "";  //current step key        
        private string _splitCurrentRouteKey = "";  //current Route key        
        private string _splitCurrentEnterpriseKey = "";  //current enterprise key        

        #endregion
        
        #region setting wip_Merge variable definition

        private string _mergeTransactionkey = ""; //母批merge transaction key        
        private string _mergeMainKey = "";  //母批 key        
        private string _mergeChildTransactionkey = ""; //子批merge transaction key        
        private string _mergeChildQty = ""; //merge qty        
        private string _mergeCurrentStepKey = "";  //current step key        
        private string _mergeCurrentRouteKey = "";  //current Route key        
        private string _mergeCurrentEnterpriseKey = "";  //current enterprise key        
        private string _rowKey = "";  //Row Key       
        #endregion

        #region setting Common variable definition

        private string _creator = "";          //创建者              
        private string _createTime = "";       //创建时间        
        private string _createTimeZone = "";         //创建时间时区               
        private string _editor = "";           //编辑者               
        private string _editTime = "";         //编辑时间                
        private string _editTimeZone = "";           //编辑时间时区         
        private string _errorMsg = "";
        private string _listLotKey = "";      //lot Key List       
        private DataTable _codeTable = new DataTable();
        private Dictionary<string, string> _codeList = new Dictionary<string, string>();
        private DataTable _dtRetrun = new DataTable();        

        #endregion

        #endregion

        #region property

        #region setting por_lot property

        public string LotKey
        {
            get { return _lotKey; }
            set { _lotKey = value; }
        }

        public string LotNumber
        {
            get { return _lotNumber; }
            set { _lotNumber = value; }
        }

        public string WorkOrderKey
        {
            get { return _workOrderKey; }
            set { _workOrderKey = value; }
        }

        public string PartVerKey
        {
            get { return _partVerKey; }
            set { _partVerKey = value; }
        }

        public string Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public string EnterTime
        {
            get { return _enterTime; }
            set { _enterTime = value; }
        }

        public string FinishedTime
        {
            get { return _finishedTime; }
            set { _finishedTime = value; }
        }

        public string PromisedTime
        {
            get { return _promisedTime; }
            set { _promisedTime = value; }
        }

        public string CloseTime
        {
            get { return _closeTime; }
            set { _closeTime = value; }
        }

        public string ShipTime
        {
            get { return _shipTime; }
            set { _shipTime = value; }
        }

        public string IsReworked
        {
            get { return _isReworked; }
            set { _isReworked = value; }
        }

        public string IsSplited
        {
            get { return _isSplited; }
            set { _isSplited = value; }
        }

        public string TemplateKey
        {
            get { return _templateKey; }
            set { _templateKey = value; }
        }

        public string RouteEnterpriseVerKey
        {
            get { return _routeEnterpriseVerKey; }
            set { _routeEnterpriseVerKey = value; }
        }

        public string CurProductionLineKey
        {
            get { return _curProductionLineKey; }
            set { _curProductionLineKey = value; }
        }

        public string CurRouteVerKey
        {
            get { return _curRouteVerKey; }
            set { _curRouteVerKey = value; }
        }

        public string CurStepVerKey
        {
            get { return _curStepVerKey; }
            set { _curStepVerKey = value; }
        }

        public string NextRouteVerKey
        {
            get { return _nextRouteVerKey; }
            set { _nextRouteVerKey = value; }
        }

        public string NextStepVerKey
        {
            get { return _nextStepVerKey; }
            set { _nextStepVerKey = value; }
        }

        public string TemplateName
        {
            get { return _templateName; }
            set { _templateName = value; }
        }

        public string TemplateVersion
        {
            get { return _templateVersion; }
            set { _templateVersion = value; }
        }

        public string IsMainLot
        {
            get { return _isMainLot; }
            set { _isMainLot = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string LineName
        {
            get { return _lineName; }
            set { _lineName = value; }
        }

        public string ShiftName
        {
            get { return _shiftName; }
            set { _shiftName = value; }
        }

        public string NamingRuleName
        {
            get { return _namingRuleName; }
            set { _namingRuleName = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        
        public string WorkorderSeq
        {
            get { return _workorderSeq; }
            set { _workorderSeq = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Module
        {
            get { return _module; }
            set { _module = value; }
        }

        public string Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public string EnterpriseName
        {
            get { return _enterpriseName; }
            set { _enterpriseName = value; }
        }

        public string RouteName
        {
            get { return _routeName; }
            set { _routeName = value; }
        }

        public string StepName
        {
            get { return _stepName; }
            set { _stepName = value; }
        }

        public string EnterpriseVersion
        {
            get { return _enterpriseVersion; }
            set { _enterpriseVersion = value; }
        }

        public string HoldFlag
        {
            get { return _holdFlag; }
            set { _holdFlag = value; }
        }

        public string StateFlag
        {
            get { return _stateFlag; }
            set { _stateFlag = value; }
        }

        public string EdcInsKey
        {
            get { return _edcInsKey; }
            set { _edcInsKey = value; }
        }

        #endregion

        #region setting wip_transaction property

        public string PieceType
        {
            get { return _pieceType; }
            set { _pieceType = value; }
        }

        public string PieceKey
        {
            get { return _pieceKey; }
            set { _pieceKey = value; }
        }

        public string Activity
        {
            get { return _activity; }
            set { _activity = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public string QuantityIn
        {
            get { return _quantityIn; }
            set { _quantityIn = value; }
        }

        public string QuantityOut
        {
            get { return _quantityOut; }
            set { _quantityOut = value; }
        }

        #endregion

        #region setting wip_split property

        public string SplitTransactionkey
        {
            get { return _splitTransactionkey; }
            set { _splitTransactionkey = value; }
        }

        public string SplitChildCreateKey
        {
            get { return _splitChildCreateKey; }
            set { _splitChildCreateKey = value; }
        }

        public string SplitChildTransactionkey
        {
            get { return _splitChildTransactionkey; }
            set { _splitChildTransactionkey = value; }
        }

        public string SplitChildQty
        {
            get { return _splitChildQty; }
            set { _splitChildQty = value; }
        }

        public string SplitCurrentStepKey
        {
            get { return _splitCurrentStepKey; }
            set { _splitCurrentStepKey = value; }
        }

        public string SplitCurrentRouteKey
        {
            get { return _splitCurrentRouteKey; }
            set { _splitCurrentRouteKey = value; }
        }

        public string SplitCurrentEnterpriseKey
        {
            get { return _splitCurrentEnterpriseKey; }
            set { _splitCurrentEnterpriseKey = value; }
        }

        #endregion

        #region setting wip_Merge property

        public string MergeTransactionkey
        {
            get { return _mergeTransactionkey; }
            set { _mergeTransactionkey = value; }
        }

        public string MergeMainKey
        {
            get { return _mergeMainKey; }
            set { _mergeMainKey = value; }
        }

        public string MergeChildTransactionkey
        {
            get { return _mergeChildTransactionkey; }
            set { _mergeChildTransactionkey = value; }
        }

        public string MergeChildQty
        {
            get { return _mergeChildQty; }
            set { _mergeChildQty = value; }
        }

        public string MergeCurrentStepKey
        {
            get { return _mergeCurrentStepKey; }
            set { _mergeCurrentStepKey = value; }
        }

        public string MergeCurrentRouteKey
        {
            get { return _mergeCurrentRouteKey; }
            set { _mergeCurrentRouteKey = value; }
        }

        public string MergeCurrentEnterpriseKey
        {
            get { return _mergeCurrentEnterpriseKey; }
            set { _mergeCurrentEnterpriseKey = value; }
        }

        public string RowKey
        {
            get { return _rowKey; }
            set { _rowKey = value; }
        }

        #endregion

        #region setting Common property

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

        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
        
        public string ListLotKey
        {
            get { return _listLotKey; }
            set { _listLotKey = value; }
        }

        public Dictionary<string, string> CodeList
        {
            get { return _codeList; }
            set { _codeList = value; }
        }

        public DataTable CodeTable
        {
            get { return _codeTable; }
            set { _codeTable = value; }
        }

        public DataTable DtRetrun
        {
            get { return _dtRetrun; }
            set { _dtRetrun = value; }
        }

        #endregion

        #endregion

        #region Action

        #region Execute Split Lot
        /// <summary>
        /// 执行批次拆分。
        /// </summary>       
        public void ExecuteSplitLot()
        {
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();

            //add the split lot to wip_transaction
            if (_codeTable.Rows.Count > 0)
            {
                _codeTable.TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
                dataSet.Tables.Add(_codeTable);
            }

            //update the quantity of parent Lot ---update por_lot
            Hashtable mainDataHashTable = new Hashtable();
            DataTable mainlotDataTable = new DataTable();
            mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_LOT_KEY, _lotKey);
            mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, _pieceKey);
            mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_QUANTITY, _quantity);
            mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_STATE_FLAG,_stateFlag);
            mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY,_enterpriseVersion);
            mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY,_curRouteVerKey);
            mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY,_curStepVerKey);

            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, _quantityIn);
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, _comment);
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR,_editor);
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY,_editTimeZone);
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, _editTime);

            string operComputerName = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);

            string shiftName = _shiftName;
            string shiftKey = string.Empty;
            Shift shift = new Shift();
            if (string.IsNullOrEmpty(shiftName))
            {
                shiftName = shift.GetCurrShiftName();
            }
            shiftKey = shift.IsShiftValueExists(shiftName);

            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, operComputerName);
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
            mainlotDataTable = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainlotDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;

            dataSet.Tables.Add(mainlotDataTable);

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                   dsReturn = serverFactory.CreateIWipEngine().SplitLotTransact(dataSet);
                   _errorMsg = SolarViewer.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);

                   if (dsReturn.Tables.Contains("CHILDLOT_DATA"))
                   {
                       _dtRetrun = dsReturn.Tables["CHILDLOT_DATA"];
                   }
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
        }

        #endregion

        #region Search Conform Lot
        /// <summary>
        /// Search Conform Lot
        /// </summary>   
        public DataSet  SearchConformLot()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIWipEngine().GetConformLotsForMerge(_lotKey);
                    _errorMsg = SolarViewer.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
        
        #region Search Lots For Merge 
        /// <summary>
        /// Search Conform Lot
        /// </summary>   
        public DataSet SearchLotsForMerge()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIWipEngine().GetLotsForMerge(_lotKey);
                    _errorMsg = SolarViewer.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
        /// 执行批次合并操作。
        /// </summary>       
        public void ExecuteMergeLot()
        {
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();

            //add the Merge lot to wip_transaction
            if (_codeTable.Rows.Count > 0)
            {
                _codeTable.TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
                dataSet.Tables.Add(_codeTable);
            }

            //update the quantity of parent Lot ---update por_lot
            Hashtable mainDataHashTable = new Hashtable();
            DataTable mainlotDataTable = new DataTable();
            mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_LOT_KEY, _lotKey);
            mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, _lotNumber);
            mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_QUANTITY, _quantity);
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, _comment);
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR,_editor);
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY,_editTimeZone);
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME , _editTime);

            string operComputerName = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
            string shiftName = _shiftName;
            string shiftKey = string.Empty;
            Shift shift = new Shift();
            if (string.IsNullOrEmpty(shiftName))
            {
                shiftName = shift.GetCurrShiftName();
            }
            shiftKey = shift.IsShiftValueExists(shiftName);
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, operComputerName);
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
            mainlotDataTable = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainlotDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;

            dataSet.Tables.Add(mainlotDataTable);

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIWipEngine().MergeLot(dataSet);
                    _errorMsg = SolarViewer.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
        }


        #region Execute Merge Lot In Store
        /// <summary>
        /// 将线边仓中的批次进行合并。
        /// </summary>
        /// comment by peter 2012-2-23
        public void ExecuteMergeLotInStore()
        {
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();

            //add the Merge lot to wip_transaction
            if (_codeTable.Rows.Count > 0)
            {
                _codeTable.TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
                dataSet.Tables.Add(_codeTable);//待合并的批次返工或退库数据
            }

            Hashtable mainDataHashTable = new Hashtable();
            DataTable mainlotDataTable = new DataTable();
            mainDataHashTable.Add(WST_STORE_MAT_FIELDS.FIELD_ROW_KEY, _rowKey);                     //合并到批次的返工或退库记录的主键
            mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, _lotNumber);                     //合并到批次的批次号
            mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_QUANTITY, _quantity);                        //合并后批次的数量
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, _editor);                    //编辑人
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, _editTimeZone);   //编辑时区
            mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, _editTime);               //编辑时间
            mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_LOT_KEY, _lotKey);                           //批次主键
            mainlotDataTable = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainlotDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dataSet.Tables.Add(mainlotDataTable);
            
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIWipEngine().MergeLotInStore(dataSet);
                    _errorMsg = SolarViewer.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
        }

        #endregion

        #endregion
    }
}
