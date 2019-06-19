//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// E-Mail:  peter.zhang@foxmail.com
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// chao.pang            2012-04-11            新建 
// =================================================================================

using System;
using System.Data;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;


namespace FanHai.Hemera.Addins.SAP
{
    /// <summary>
    /// 显示工单报工界面的用户控件类。
    /// </summary>
    public partial class WorkOrderWorkForCtrl : BaseUserCtrl
    {
        /// <summary>
        /// Construct function
        /// </summary>
        public WorkOrderWorkForCtrl()
        {
            InitializeComponent();
            //报工人员
            this.lpWordOrderPeople.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
         
        }
        /// <summary>
        /// 绑定工厂车间
        /// </summary>
        public void BindFactoryRoom()
        {
            #region
            //绑定工厂车间名称
            DataTable dt2 = FactoryUtils.GetFactoryRoomByStores(PropertyService.Get(PROPERTY_FIELDS.STORES));
            //绑定工厂车间数据到窗体控件。
            lpFacName.Properties.DataSource = dt2;
            lpFacName.Properties.DisplayMember = "LOCATION_NAME";
            lpFacName.Properties.ValueMember = "LOCATION_KEY";
            //表中有数据，设置窗体控件的默认索引为0。
            if (dt2.Rows.Count > 0)
            {
                lpFacName.ItemIndex = 0;
            }
            #endregion
        }

        /// <summary>
        /// 绑定班次
        /// </summary>
        public void BindShiftName()
        {
            string[] columns = new string[] { "CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Shift");

            this.lpShift.Properties.DataSource = BaseData.Get(columns, category);
            this.lpShift.Properties.DisplayMember = "CODE";
            this.lpShift.Properties.ValueMember = "CODE";
            Shift shift = new Shift();
            this.lpShift.EditValue = shift.GetCurrShiftName();
        }
        /// <summary>
        /// 绑定工单名称
        /// </summary>
        private void BindWorkOrder(string factoryroom)
        {
            WorkOrders works = new WorkOrders();
            DataTable dt = works.GetWorkOrderByFactoryRoom(factoryroom).Tables[0];

            //绑定工单号数据到窗体控件。
            lpWordNumber.Properties.DataSource = dt;
            lpWordNumber.Properties.DisplayMember = "ORDER_NUMBER";
            lpWordNumber.Properties.ValueMember = "ORDER_NUMBER";
            //线别表中有数据，设置窗体控件的默认索引为0。
            if (dt.Rows.Count > 0)
            {
                lpWordNumber.ItemIndex = 0;
            }
        }

        /// <summary>
        /// 关闭工单报工界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        /// <summary>
        /// 视图载入Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkOrderWorkForCtrl_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();//工厂车间
            BindShiftName();  //班次
            BindWorkOrder(lpFacName.Text);//通过工厂车间获取工单号
            LoadWorkOrderListData(); 
        }

        private void lpFacName_TextChanged(object sender, EventArgs e)
        {
            string factoryroom = lpFacName.Text;
            BindWorkOrder(factoryroom);
        }

        private void gvWordOrderList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "ROWNUM": //设置行号
                    e.DisplayText = Convert.ToString(e.RowHandle + 1);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 工单报工按钮确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbWorkOrderWorkFor_Click(object sender, EventArgs e)
        {
            if (lpFacName.Text == "")
            {
                MessageService.ShowMessage("工厂车间不能为空!", "${res:Global.SystemInfo}");
                return;
            }
            if (lpWordNumber.Text == "")
            {
                MessageService.ShowMessage("工单号不能为空!", "${res:Global.SystemInfo}");
                return;
            }
            if (lpShift.Text == "")
            {
                MessageService.ShowMessage("班次不能为空!", "${res:Global.SystemInfo}");
                return;
            }
            if (lpWordOrderPeople.Text == "")
            {
                MessageService.ShowMessage("报工人员不能为空!", "${res:Global.SystemInfo}");
                return;
            }
            string _maxTimeStamp = string.Empty;
            WorkOrderWorkFor _workOrderWorkFor = new WorkOrderWorkFor();
            //DataSet maxTimeStamp = _workOrderWorkFor.GetMaxTimeStamp();                   //返回起始时间戳和结束时间戳
            //if (maxTimeStamp.Tables["timestamp"].Rows.Count > 0)
            //{
            //    if (!string.IsNullOrEmpty(maxTimeStamp.Tables["timestamp"].Rows[0]["TIME_STAMP"].ToString()))
            //    {
            //        _maxTimeStamp = maxTimeStamp.Tables["timestamp"].Rows[0]["TIME_STAMP"].ToString();          //起始时间戳
            //    }           
            //}
            //string _time = maxTimeStamp.Tables["timestamp"].Rows[0]["systimestamp"].ToString();                   //结束时间戳

            //string _orderNumber = lpWordNumber.EditValue.ToString();                       //工单号
            //DataSet heGeNumber = new DataSet();
            //DataSet baoFeiNumber = new DataSet();

            //heGeNumber = _workOrderWorkFor.GetHeGeNumber(_maxTimeStamp, _time, _orderNumber); //传入参数起始时间戳结束时间戳工单号获取合格品数量
            //baoFeiNumber = _workOrderWorkFor.GetBaoFeiNumber(_maxTimeStamp, _time, _orderNumber);//传入参数起始时间戳结束时间戳工单号获取报废数量

            //if (heGeNumber.Tables[0].Rows.Count > 0 && baoFeiNumber.Tables[0].Rows.Count > 0)
            //{
            //    if (heGeNumber.Tables[0].Rows[0][0].ToString() == "0" && baoFeiNumber.Tables[0].Rows[0][0].ToString() == "0")
            //    {
            //        MessageService.ShowMessage("合格品数量=0 并且报废数量=0，无法报工。", "${res:Global.SystemInfo}");
            //    }
            //    else
            //    {
            Hashtable hashTable = new Hashtable();
            hashTable.Add("AUFNR", lpWordNumber.EditValue.ToString());
            //hashTable.Add("GMNGA", heGeNumber.Tables[0].Rows[0][0].ToString());
            //hashTable.Add("XMNGA", baoFeiNumber.Tables[0].Rows[0][0].ToString());
            //hashTable.Add("START_TIME_STAMP", _maxTimeStamp);
            //hashTable.Add("TIME_STAMP", _time);
            string facName = lpFacName.Text;
            string facRoomKey = lpFacName.Properties.GetKeyValueByDisplayText(facName).ToString();
            hashTable.Add("ROOM_KEY",facRoomKey);
            hashTable.Add("CREATOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
            hashTable.Add("CREATE_TIMEZONE", PropertyService.Get(PROPERTY_FIELDS.TIMEZONE));
            hashTable.Add("REPORTOR", lpWordOrderPeople.Text);
            hashTable.Add("SHIFT_NAME", lpShift.EditValue.ToString());
           
            DataTable tableParam = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
            tableParam.TableName = "HASH";

            //if (!_workOrderWorkFor.InsertPorWorkOrderReport(tableParam))
            if (!_workOrderWorkFor.GongDanBaoGong(tableParam))
            {//bool类型添加数据到POR_WORK_ORDER_REPORT表中
                MessageService.ShowMessage(_workOrderWorkFor.ErrorMsg);
            }
            else
            {
                LoadWorkOrderListData();
               
            }
                    
                    
            //    }
            //}
            //else
            //{
            //    MessageService.ShowMessage("合格品数量=0 并且报废数量=0，无法报工", "${res:Global.SystemInfo}");
            //}
        }
        /// <summary>
        /// 翻页
        /// </summary>
        private void paginationWorkOrderWorkFors_DataPaging()
        {
            LoadWorkOrderListData();
        }
        /// <summary>
        /// 载入数据
        /// </summary>
        public void LoadWorkOrderListData()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    int pages;
                    int records;
                    int pageNo = this.paginationWorkOrderWorkFors.PageNo;
                    int pageSize = this.paginationWorkOrderWorkFors.PageSize;

                    if (pageNo <= 0)
                    {
                        pageNo = 1;
                    }

                    if (pageSize <= 0)
                    {
                        pageSize = PaginationControl.DEFAULT_PAGESIZE;                          //每页行数DEFAULT_PAGESIZE=20
                    }
                    //获取数据
                    resDS = serverFactory.CreateIWorkOrderWorkForEngine().GetWorkOrderWorkFor(reqDS, pageNo, pageSize, out pages, out records);

                    if (pages > 0 && records > 0)
                    {
                        this.paginationWorkOrderWorkFors.PageNo = pageNo > pages ? pages : pageNo;
                        this.paginationWorkOrderWorkFors.PageSize = pageSize;
                        this.paginationWorkOrderWorkFors.Pages = pages;
                        this.paginationWorkOrderWorkFors.Records = records;
                    }
                    else
                    {
                        this.paginationWorkOrderWorkFors.PageNo = 0;
                        this.paginationWorkOrderWorkFors.PageSize = PaginationControl.DEFAULT_PAGESIZE;
                        this.paginationWorkOrderWorkFors.Pages = 0;
                        this.paginationWorkOrderWorkFors.Records = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);

                return;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            #region Process Output Parameters

            string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            if (string.IsNullOrEmpty(returnMsg))
            {
                //绑定返回表的数据到设备维护数据表中
                BindDataToWorkOrderGrid(resDS.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }
        /// <summary>
        /// 绑定工单报工列表
        /// </summary>
        /// <param name="dataTable">分页查询的结果表</param>
        private void BindDataToWorkOrderGrid(DataTable dataTable)
        {
            this.grdCrtlCode.MainView = this.gvWordOrderList;
            this.grdCrtlCode.DataSource = null;
            this.grdCrtlCode.DataSource = dataTable;
        }
    }
}
