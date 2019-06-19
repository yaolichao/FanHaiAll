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
    public class StoreMat:EntityObject
    {
        #region Properties
        private string _rowKey = "";      
        private string _itemNo = "";         //material number or lot number       
        private string _itemDescription = "";//description of material
        private string _itemType = "";       //material or lot
        private string _itemQty = "";        //quantity of material
        private string _itemUnit = "";       //unit of material
        private string _objectStatus = "";
        private string _storeKey = "";       //key value of store
        private string _storeName = "";
        private string _storeType = "";
        private string _billNumber = "";
        private string _workOrderNumber = "";
        private string _sertalNumber = "";
        private string _barcode = "";
        private string _errorMsg = "";
        private string _lineKey = "";
        private string _enterpriseKey = "";
        private string _routeKey = "";
        private string _stepKey = "";
        private string _request_flag = "1";
        #endregion

        #region Define Attribute
        public string RowKey
        {
            get { return _rowKey; }
            set { _rowKey = value; }
        }
        public string ItemNo
        {
            get { return _itemNo; }
            set { _itemNo = value; }
        }
        public string ItemDescription
        {
            get { return _itemDescription; }
            set { _itemDescription = value; }
        }
        public string ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }
        public string ItemQty
        {
            get { return _itemQty; }
            set { _itemQty = value; }
        }
        /// <summary>
        /// 申请过账标记。1：线上仓需申请过账 0：线上仓不需要申请过账。
        /// </summary>
        public string RequestFlag
        {
            get { return _request_flag; }
            set { _request_flag = value; }
        }

        public string ItemUnit
        {
            get { return _itemUnit; }
            set { _itemUnit = value; }
        }
        public string ObjectStatus
        {
            get { return _objectStatus; }
            set { _objectStatus = value; }
        }
        public string StoreKey
        {
            get { return _storeKey; }
            set { _storeKey = value; }
        }
        public string BillNumber
        {
            get { return _billNumber; }
            set { _billNumber = value; }
        }
        public string WorkOrderNumber
        {
            get { return _workOrderNumber; }
            set { _workOrderNumber = value; }
        }
        public string SertalNumber
        {
            get { return _sertalNumber; }
            set { _sertalNumber = value; }
        }
        public string Barcode
        {
            get { return _barcode; }
            set { _barcode = value; }
        }
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
        public string StoreName
        {
            set { this._storeName = value; }
            get { return this._storeName; }
        }
        public string StoreType
        {
            get { return _storeType; }
            set { _storeType = value; }
        }

        public string LineKey
        {
            get { return this._lineKey; }
            set { this.LineKey = value; }
        }
        public string EnterpriseKey
        {
            get { return this._enterpriseKey; }
            set { this._enterpriseKey = value; }
        }
        public string RouteKey
        {
            get { return this._routeKey; }
            set { this._routeKey = value; }
        }
        public string StepKey
        {
            get { return this._stepKey; }
            set { this._stepKey = value; }
        }
        #endregion

        #region Action

        #region GetLotsInStore
        public DataSet GetLotsInStore()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIStoreEngine().GetLotsInStore(_storeKey);
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

        #region TransferFromStore
        public void TransferFromStore()
        {
            DataSet dsReturn = new DataSet();
            #region build DataSet
            DataSet dsFrom = new DataSet();
            Hashtable hashData = new Hashtable();
            DataTable dataTable = new DataTable();
            if (_rowKey != "")
            {
                hashData.Add(WST_STORE_MAT_FIELDS.FIELD_ROW_KEY, _rowKey);
            }
            if (_itemNo != "")
            {
                hashData.Add(WST_STORE_MAT_FIELDS.FIELD_ITEM_NO, _itemNo);
            }            
            if (hashData.Count > 0)
            {
                hashData.Add(WST_STORE_MAT_FIELDS.FIELD_EDITOR, Editor);
                hashData.Add(WST_STORE_MAT_FIELDS.FIELD_EDIT_TIMEZONE, EditTimeZone);
                dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashData);
                dataTable.TableName = WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME;
                dsFrom.Tables.Add(dataTable);
            }
            #endregion
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIStoreEngine().TransferFromStore(dsFrom);
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
        }
        #endregion

        #region Get storemat info
        /// <summary>
        /// Get storemat info
        /// </summary>
        /// <returns>DataTable for storemat info</returns>
        public DataSet GetStoreMatInfo(DataTable paramTable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIStoreEngine().GetStoreMatInfo(paramTable);
                    string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                        return null;
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

            return dsReturn;
        }
        #endregion

        #endregion
    }
}
