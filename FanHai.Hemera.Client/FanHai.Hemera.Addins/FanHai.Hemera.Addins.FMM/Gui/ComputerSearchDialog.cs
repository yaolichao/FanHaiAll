// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Chao.pang            2012-02-21            添加注释
// =================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.CommonControls.Dialogs;

namespace FanHai.Hemera.Addins.FMM
{
    public partial class ComputerSearchDialog : BaseDialog
    {
        //string workOrder = "";
        public DataSet searchDataSet = new DataSet();  

        #region Constructor
        public ComputerSearchDialog()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.ComputerSearchDialog.Title}"))     //计算机维护 - 查询
        {
            InitializeComponent();
            //InitEmptyWorkOrdersDataSet();
        }
        #endregion

        #region Properties
        public string ComputerKey
        {
            get
            {
                return _computerKey;
            }
        }
        public string ComputerName
        {
            get
            {
                return _computerName;
            }
        }
        #endregion  

        /// <summary>
        /// bind data to GridView
        /// </summary>
        /// <param name="dataSet"></param>
        private void BindDataSourceToGrid(DataTable dt)
        {
            gridData.MainView = gridDataView;
            gridData.DataSource = dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridDataView_ShowingEditor(object sender, CancelEventArgs e)
        {
            //gridView can't edit
            e.Cancel = true;
        }
       
        /// <summary>
        /// grid Control Double Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridData_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        #region Actions

        /// <summary>
        /// Search button click 查询按钮的单击事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearch_Click(object sender, EventArgs e)
        {
            
            Hashtable mainDataHashTable = new Hashtable();                                                      //定义hashtable对象 
            DataSet dataSet = new DataSet();
            mainDataHashTable.Add(COMPUTER_FIELDS.FIELDS_COMPUTER_NAME, txtComputerName.Text.Trim().ToUpper()); //将txtComputerName的值去空格和全部大写话到hashtable对象COMPUTER_NAME列中 
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = COMPUTER_FIELDS.DATABASE_TABLE_NAME;                                      //表明为COMPUTER_CONFIG 
            dataSet.Tables.Add(mainDataTable);
            //Call Remoting Service
            try
            {
                //远程调用技术 
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    DataSet retDS = factor.CreateIComputerEngine().SearchComputers(dataSet);                    //调用SearchComputers方法传入表集dataset获取查询数据 

                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(retDS);
                    //返回值为-1 则提示错误 
                    if (null == returnMessage || returnMessage.Length > 0)
                    {
                        MessageService.ShowError(returnMessage);
                    }
                    //返回值不为-1 
                    else
                    {
                        BindDataSourceToGrid(retDS.Tables[COMPUTER_FIELDS.DATABASE_TABLE_NAME]);
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
        private void btnCancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        #endregion Actions

        #region Private Functions

        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gridDataView.GetDataRowHandleByGroupRowHandle(gridDataView.FocusedRowHandle);
            if (rowHandle >= 0)
            {
               // _workOrderKey = gridWorkOrdersView.GetRowCellValue(rowHandle,POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY).ToString();
                _computerKey = gridDataView.GetRowCellValue(rowHandle, COMPUTER_FIELDS.FIELDS_CODE_KEY).ToString();
                _computerName = gridDataView.GetRowCellValue(rowHandle, COMPUTER_FIELDS.FIELDS_COMPUTER_NAME).ToString();
               // _workOrderNumber =gridWorkOrdersView.GetRowCellValue(rowHandle, "ORDER_NUMBER").ToString();
                return true;
            }
            return false;
        }

        private void MapWorkOrdersToGridView(DataTable dt)
        {
            gridData.DataSource = dt;
        }

        //private void InitEmptyWorkOrdersDataSet()
        //{
        //    List<string> fields = new List<string>()
        //                                            {
        //                                                COLUMN_SALES_ORDER_KET,
        //                                                COLUMN_SALES_ORDER_NUMBER,
        //                                                COLUMN_PRIORITY,
        //                                                COLUMN_STATE,
        //                                                COLUMN_CUSTOMER,
        //                                                COLUMN_SALES_ORDER_CREATE_TIME,
        //                                                COLUMN_PROMISED_SHIP_TIME
        //                                            };

        //    DataTable dt = FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns("DONTUSEIT", fields);
        //    gridData.MainView = gridDataView;
        //    gridData.DataSource = dt;

        //}
        #endregion

        #region Private Variables Definition
        private string _computerKey = "";
        private string _computerName = "";

        //#region Private Consts: Columns
        //private const string COLUMN_SALES_ORDER_KET = "clnSalesOrderKey";
        //private const string COLUMN_SALES_ORDER_NUMBER = "clnSalesOrderNumber";
        //private const string COLUMN_PRIORITY = "clnPriority";
        //private const string COLUMN_STATE = "clnState";
        //private const string COLUMN_CUSTOMER = "clnCustomer";
        //private const string COLUMN_SALES_ORDER_CREATE_TIME = "clnSalesOrderCreateTime";
        //private const string COLUMN_PROMISED_SHIP_TIME = "clnPromisedShipTime";
        //#endregion Private Consts: Columns

        private void ComputerSearchDialog_Load(object sender, EventArgs e)
        {
            #region InitUI
            //label

            //this.lblWorkOrderNumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.Gui.WorkOrderCtrl.lblWorkOrderNumber}");
            //this.lblPriority.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.WorkOrderCtrl.lblRevenueType}");
            //this.lblWorkOrderState.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.Gui.WorkOrderCtrl.lblWorkOrderState}");
            //this.lblCustomer.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.WorkOrderCtrl.lblSupplier}"); 
            
            //button

            this.btSearch.Text = StringParser.Parse("${res:Global.Query}");
            this.btnOK.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancle.Text = StringParser.Parse("${res:Global.CancelButtonText}");

            //grid
            //this.clnSalesOrderNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.Gui.WorkOrderSearchDialog.gridColumn_SalesOrderNumber}");
            //this.clnPriority.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.WorkOrderCtrl.lblRevenueType}");
            //this.clnState.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.Gui.WorkOrderSearchDialog.gridColumn_State}");
            //this.clnCustomer.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.WorkOrderCtrl.lblSupplier}"); 
            //this.clnSalesOrderCreateTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.Gui.WorkOrderSearchDialog.gridColumn_SalesCrateTime}");
            //this.clnPromisedShipTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.Gui.WorkOrderSearchDialog.gridColumn_PromiseShipTime}");
            #endregion

            #region Bind Data of Supplier and Status
            //supplier
            //string[] columns = new string[] { "CODE" };
            //KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Suppliers");
            //DataTable supplierTable = BaseData.Get(columns, category);

            //foreach (DataRow row in supplierTable.Rows)
            //{               
            //    cmbSupplier.Properties.Items.Add(row["CODE"].ToString());
            //} 

           ////status
           // DataTable statusTable = new DataTable();
           // statusTable.Columns.Add("CODE");
           // statusTable.Columns.Add("NAME");
           // statusTable.Rows.Add("0", "未激活");
           // statusTable.Rows.Add("1", "已激活");
           // statusTable.Rows.Add("2","存档");
           // lueStatus.Properties.DataSource = statusTable;
           // lueStatus.Properties.DisplayMember = "NAME";
           // lueStatus.Properties.ValueMember = "CODE";
            #endregion
        }
        #endregion Private Variables Definition
    }
}
