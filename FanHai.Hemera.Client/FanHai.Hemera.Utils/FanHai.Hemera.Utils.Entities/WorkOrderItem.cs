using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SolarViewer.Hemera.Share.Constants;

namespace SolarViewer.Hemera.Utils.Entities
{
    public class WorkOrderItem
    {
        #region Constructor
        public WorkOrderItem(string workOrderKey, string salesOrderNumber, string salesOrderItemKey, string salesOrderItemQuantity,
                             string salesOrderItemPriority, string salesOrderItemPartName, string salesOrderItemPartNumber, string salesOrderItemPartVersion)
        {
            _workOrderKey = workOrderKey;
            _salesOrderNumber = salesOrderNumber;
            _salesOrderItemKey = salesOrderItemKey;
            _salesOrderItemQuantity = salesOrderItemQuantity;
            _salesOrderItemPriority = salesOrderItemPriority;
            _salesOrderItemPartName = salesOrderItemPartName;
            _salesOrderItemPartNumber = salesOrderItemPartNumber;
            _salesOrderItemPartVersion = salesOrderItemPartVersion;
            _operationAction = OperationAction.New;
        }
        public WorkOrderItem(string workOrderKey, string salesOrderNumber, string salesOrderItemKey, string salesOrderItemQuantity,
                             string salesOrderItemPriority, string salesOrderItemPartName, string salesOrderItemPartNumber, string salesOrderItemPartVersion,
                             string salesOrderItemJoinTime)
        {
            _workOrderKey = workOrderKey;
            _salesOrderNumber = salesOrderNumber;
            _salesOrderItemKey = salesOrderItemKey;
            _salesOrderItemQuantity = salesOrderItemQuantity;
            _salesOrderItemPriority = salesOrderItemPriority;
            _salesOrderItemPartName = salesOrderItemPartName;
            _salesOrderItemPartNumber = salesOrderItemPartNumber;
            _salesOrderItemPartVersion = salesOrderItemPartVersion;
            _salesOrderItemJoinTime = salesOrderItemJoinTime;
            _operationAction = OperationAction.Update;
        }
        #endregion

        #region Public Functions
        public void ParseUpdateDataToDataTable(ref DataTable dtWorkOrderItems)
        {
            if (null == dtWorkOrderItems || !IsDirty)
            {
                return;
            }
            object[] values = new object[] 
                                         { 
                                             _salesOrderItemKey,
                                             POR_WORK_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_QUANTITY,
                                             (_dirtyList.ContainsKey(POR_WORK_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_QUANTITY) ? _dirtyList[POR_WORK_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_QUANTITY] : ""),
                                             _salesOrderItemQuantity
                                          };
            dtWorkOrderItems.Rows.Add(values);
        }
        #region Properties
        public bool IsDirty
        {
            get
            {
                return (_dirtyList.Count > 0);
            }
        }
        public string WorkOrderKey
        {
            get
            {
                return _workOrderKey;
            }
        }
        public string SalesOrderNumber
        {
            get
            {
                return _salesOrderNumber;
            }
            set
            {
                _salesOrderNumber = value;
            }
        }
        public string SalesOrderItemKey
        {
            get
            {
                return _salesOrderItemKey;
            }
        }
        public string SalesOrderItemQuantity
        {
            get
            {
                return _salesOrderItemQuantity;
            }
            set
            {
                _salesOrderItemQuantity = value;
            }
        }
        public string SalesOrderItemPriority
        {
            get
            {
                return _salesOrderItemPriority;
            }
            set
            {
                _salesOrderItemPriority = value;
            }

        }
        public string SalesOrderItemJoinTime
        {
            get
            {
                return _salesOrderItemJoinTime;
            }
        }
        public string SalesOrderItemPartName
        {
            get
            {
                return _salesOrderItemPartName;
            }
            set
            {
                _salesOrderItemPartName = value;
            }
        }
        public string SalesOrderItemPartNumber
        {
            get
            {
                return _salesOrderItemPartNumber;
            }
            set
            {
                _salesOrderItemPartNumber = value;
            }
        }
        public string SalesOrderItemPartVersion
        {
            get
            {
                return _salesOrderItemPartVersion;
            }
            set
            {
                _salesOrderItemPartVersion = value;
            }
        }
        public OperationAction OperationAction
        {
            get
            {
                return _operationAction;
            }
            set
            {
                _operationAction = value;
            }
        }
        #endregion // Properties
        #endregion //Public Functions

        #region Private Functions
        private void ValidateDirtyList(string key, string newValue)
        {
            if (_dirtyList.ContainsKey(key))
            {
                if (newValue == _dirtyList[key])
                {
                    _dirtyList.Remove(key);
                }
                else
                {
                    _dirtyList[key] = newValue;
                }
            }
            else
            {
                string orginalValue = "";
                switch (key)
                {
                    case POR_WORK_ORDER_ITEM_FIELDS.FIELD_SALES_ORDER_ITEM_QUANTITY:
                        orginalValue = _salesOrderItemQuantity;
                        break;
                }
                if (orginalValue != newValue)
                {
                    _dirtyList.Add(key, orginalValue);
                }
            }
        }
        #endregion Private Functions

        #region Private Variables Definitions
        private string _workOrderKey = "";
        private string _salesOrderNumber = "";
        private string _salesOrderItemKey = "";
        private string _salesOrderItemQuantity = "";
        private string _salesOrderItemPriority = "";
        private string _salesOrderItemJoinTime = "";
        private string _salesOrderItemPartName = "";
        private string _salesOrderItemPartNumber = "";
        private string _salesOrderItemPartVersion = "";
        private Dictionary<string, string> _dirtyList = new Dictionary<string, string>();
        private OperationAction _operationAction = OperationAction.None;
        #endregion
    }
    public class WorkOrderItems
    {
        public WorkOrderItems()
        {
        }

        #region Public Functions
        #region Properties
        public bool IsDirty
        {
            get
            {
                foreach (WorkOrderItem woi in _workOrderItems)
                {
                    if (woi.IsDirty)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public List<WorkOrderItem> WorkOrderItemList
        {
            get
            {
                return _workOrderItems;
            }
        }
        public string WorkOrderQuantityOrdered
        {
            get
            {
                return _workOrderQuantityOrdered.ToString();
            }
        }
        #endregion // Properties
        public void WorkOrderItemAdd(WorkOrderItem woi)
        {
            int salesOrderItemQuantity = Convert.ToInt32(woi.SalesOrderItemQuantity);

            foreach (WorkOrderItem itemWoi in _workOrderItems)
            {
                if (itemWoi.WorkOrderKey == woi.WorkOrderKey && itemWoi.SalesOrderItemKey == woi.SalesOrderItemKey)
                {
                    if (OperationAction.Delete == itemWoi.OperationAction)
                    {
                        itemWoi.SalesOrderItemQuantity = salesOrderItemQuantity.ToString();
                        itemWoi.OperationAction = OperationAction.Modified;

                        _workOrderQuantityOrdered += salesOrderItemQuantity;
                    }
                    else if (OperationAction.Update == itemWoi.OperationAction)
                    {
                        salesOrderItemQuantity += salesOrderItemQuantity;
                        itemWoi.SalesOrderItemQuantity = (Convert.ToInt32(itemWoi.SalesOrderItemQuantity) + salesOrderItemQuantity).ToString();
                        itemWoi.OperationAction = OperationAction.Modified;

                        _workOrderQuantityOrdered += salesOrderItemQuantity;
                    }
                    else
                    {
                        itemWoi.SalesOrderItemQuantity = (Convert.ToInt32(itemWoi.SalesOrderItemQuantity) + salesOrderItemQuantity).ToString();

                        _workOrderQuantityOrdered += salesOrderItemQuantity;
                    }
                    return;
                }
            }
            _workOrderItems.Add(woi);

            _workOrderQuantityOrdered += salesOrderItemQuantity;
        }
        public bool WorkOrderItemUpdate(WorkOrderItem woi)
        {
            foreach (WorkOrderItem itemWoi in _workOrderItems)
            {
                if (itemWoi.WorkOrderKey == woi.WorkOrderKey && itemWoi.SalesOrderItemKey == woi.SalesOrderItemKey)
                {
                    if (OperationAction.None == itemWoi.OperationAction ||
                        OperationAction.Delete == itemWoi.OperationAction)
                    {
                        return false;
                    }
                    else if (OperationAction.New == itemWoi.OperationAction)
                    {
                        itemWoi.SalesOrderItemQuantity = woi.SalesOrderItemQuantity;
                    }
                    else if (OperationAction.Update == itemWoi.OperationAction)
                    {
                        itemWoi.SalesOrderItemQuantity = woi.SalesOrderItemQuantity;
                        itemWoi.OperationAction = OperationAction.Modified;
                    }
                    else if (OperationAction.Modified == itemWoi.OperationAction)
                    {
                        itemWoi.SalesOrderItemQuantity = woi.SalesOrderItemQuantity;
                    }

                    _workOrderQuantityOrdered = _workOrderQuantityOrdered - Convert.ToInt32(itemWoi.SalesOrderItemQuantity) + Convert.ToInt32(woi.SalesOrderItemQuantity);
                    return true;
                }
            }
            return false;
        }
        public bool WorkOrderItemDelete(string salesOrderItemKey)
        {
            foreach (WorkOrderItem itemWoi in _workOrderItems)
            {
                if (itemWoi.SalesOrderItemKey == salesOrderItemKey)
                {
                    int workOrderItemQuantityOrdered = Convert.ToInt32(itemWoi.SalesOrderItemQuantity);
                    if (OperationAction.Update == itemWoi.OperationAction ||
                        OperationAction.Modified == itemWoi.OperationAction)
                    {
                        itemWoi.OperationAction = OperationAction.Delete;
                    }
                    else if (OperationAction.New == itemWoi.OperationAction)
                    {
                        _workOrderItems.Remove(itemWoi);
                    }
                    else
                    {
                        return false;
                    }
                    _workOrderQuantityOrdered -= workOrderItemQuantityOrdered;
                    return true;
                }
            }
            return false;
        }
        #endregion // Public Functions
        #region Private Variables Definitions
        private int _workOrderQuantityOrdered = 0;
        private List<WorkOrderItem> _workOrderItems = new List<WorkOrderItem>();
        #endregion
    }
}
