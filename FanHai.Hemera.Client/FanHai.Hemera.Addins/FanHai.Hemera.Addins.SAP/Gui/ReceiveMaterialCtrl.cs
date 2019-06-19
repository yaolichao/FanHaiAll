//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  yongbing.yang
//----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using System.Collections;
using FanHai.Hemera.Addins.MM;


namespace FanHai.Hemera.Addins.SAP
{
    /// <summary>
    /// 表示来料接收的窗体类。
    /// </summary>
    public partial class ReceiveMaterialCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReceiveMaterialCtrl()
        {
            InitializeComponent();

            BindShiftName();
            BindMaterialDetail();

            txtOperatorNumber.Text= PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            txtOperatorNumber.Enabled = false;
        }
        /// <summary>
        /// 根据线上仓名称获取工序名称。
        /// </summary>
        /// <param name="storeName">线上仓名称。</param>
        public string GetOperationNameByStore(string storeName)
        {
            ReceiveMaterialEntity receiveMaterial = new ReceiveMaterialEntity();
            storeName = receiveMaterial.GetOperationByLineStore(storeName);
            if (!string.IsNullOrEmpty(receiveMaterial.ErrorMsg))
            {
                MessageService.ShowError(receiveMaterial.ErrorMsg);
            }
            return storeName;
        }
        /// <summary>
        /// 获取线上仓名称的集合
        /// </summary>
        /// <returns>使用逗号分开的线上仓名称。</returns>
        public string GetStoreNameList()
        {
            string lineStore = PropertyService.Get(PROPERTY_FIELDS.STORES);
            return lineStore;
        }
        /// <summary>
        /// 获取SAP中用户拥有的工序名称
        /// </summary>
        /// <returns>SAP用户拥有工序集合</returns>
        private string GetSapOperation()
        {
            string strSapOperations = "";
            string strOperations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            strOperations = "'" + strOperations.Replace(",", "','") + "'";
            #region
            DataTable dt=BaseData.Get(new string[] { "WORK_CENTER","OPERATION_NAME" },
                new KeyValuePair<string, string>("CATEGORY_NAME", "SAP_MES_OPERATIONS"));
            DataRow[] drs=dt.Select("OPERATION_NAME in (" + strOperations + ")");
            #endregion
            if (drs.Length > 0)
            {
                for (int i = 0; i < drs.Length ; i++) //MODIBY QYM 20120428
                {
                    if (drs[i]["WORK_CENTER"].ToString().Trim() != "")
                    {
                        strSapOperations += drs[i]["WORK_CENTER"].ToString().Trim() + ",";
                    }
                }
            }
            return strSapOperations;
        }
        /// <summary>
        /// 获取物料明细记录并绑定到GridControl控件中。
        /// </summary>
        public void BindMaterialDetail()
        {
            string sapOperation = GetSapOperation();
            string storeNameList = GetStoreNameList();
            DataSet dsReturn = new DataSet();
            string msg = string.Empty;
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//工厂对象不为null
                {
                    dsReturn = serverFactory.CreatIReceiveMaterialEngine().GetMaterialDetail(sapOperation, storeNameList);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);

                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        dsReturn.Tables[0].Columns.Add(new DataColumn("IsSelected", typeof(Boolean)));
                        dsReturn.Tables[0].Columns.Add(new DataColumn("Operation", typeof(string)));
                        dsReturn.Tables[0].Columns.Add(new DataColumn("OnlineWarehouse", typeof(string)));

                        foreach (DataRow dr in dsReturn.Tables[0].Rows)
                        {
                            string strWorkOrder = dr["AUFNR"].ToString();
                            string strMaterialCode = dr["MATNR"].ToString();
                            string strStores = PropertyService.Get(PROPERTY_FIELDS.STORES);
                            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
                            DataTable dt = entity.GetStoreByMaterialCode(strWorkOrder, strMaterialCode, strStores);
                            if (entity.ErrorMsg == string.Empty)//成功获取线上仓数据。
                            {
                                string defaultLineStore = dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : string.Empty;
                                string defaultOperation = defaultLineStore != string.Empty ? GetOperationNameByStore(defaultLineStore) : string.Empty;
                                dr["OnlineWarehouse"]=defaultLineStore;
                                dr["Operation"]=defaultOperation;
                            }
                        }

                        gdcData.MainView = gdvMaterialDefault;
                        gdcData.DataSource = dsReturn.Tables[0];
                    }
                }
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }

        /// <summary>
        /// 打开物料清单页面
        /// </summary>
        private void tsbReceiveList_Click(object sender, EventArgs e)
        {
            ReceiveMaterialEntity receiveMaterial = new ReceiveMaterialEntity();
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == StringParser.Parse("来料接收清单"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
           
            ReceiveMaterialListViewContent vc = new ReceiveMaterialListViewContent();
            WorkbenchSingleton.Workbench.ShowView(vc);
        }
        /// <summary>
        /// 绑定班别信息
        /// </summary>
        public void BindShiftName()
        {
            string[] columns = new string[] { "CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Shift");

            this.lueShiftName.Properties.DataSource = BaseData.Get(columns, category);
            this.lueShiftName.Properties.DisplayMember = "CODE";
            this.lueShiftName.Properties.ValueMember = "CODE";
            this.lueShiftName.ItemIndex = 0;
            Shift shift = new Shift();
            this.lueShiftName.EditValue = shift.GetCurrShiftName();
        }

      
        private DataTable CreatTable()
        {
            //SAP_ISSURE_KEY
            //W.MBLNR          AS 来料单号,
            //W.MATNR          AS 物料批号,
            //W.MATXT          AS 物料描述,
            //W.AUFNR          AS 工单号码,
            //W.ERFME          AS 计量单位,
            //W.ERFMG          AS 实领数量,
            //W.LLIEF          AS 批次供应商
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("SAP_ISSURE_KEY");
            dataTable.Columns.Add("MBLNR");
            dataTable.Columns.Add("MATNR");
            dataTable.Columns.Add("MATXT");
            dataTable.Columns.Add("AUFNR");
            dataTable.Columns.Add("ERFME");
            dataTable.Columns.Add("ERFMG");
            dataTable.Columns.Add("LLIEF");
            dataTable.Columns.Add("CHARG");
            dataTable.Columns.Add("Operation");
            dataTable.Columns.Add("OnlineWarehouse");
            dataTable.Columns.Add("SHIFT_NAME");
            dataTable.Columns.Add("OPERATOR");
            dataTable.Columns.Add("EDIT_TIMEZONE");

            return dataTable;
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        /// <summary>
        /// 抓去SAP中间数据库的数据信息导入到MES对应的物料表中
        /// CREATOR=当前登录用户员工，
        /// CREATE_TIME=当前时间，CREATE_TIMEZONE=当前时区，
        /// EDITOR=当前登录用户员工，EDIT_TIME=当前时间，EDIT_TIMEZONE=当前时区。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRefresMaterial_Click(object sender, EventArgs e)
        {
            Hashtable hashTable = new Hashtable();
            hashTable.Add("ISRECEIVED", 0);
            hashTable.Add("CREATOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
            hashTable.Add("CREATE_TIMEZONE", PropertyService.Get(PROPERTY_FIELDS.TIMEZONE));
            hashTable.Add("EDITOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
            hashTable.Add("EDIT_TIMEZONE", PropertyService.Get(PROPERTY_FIELDS.TIMEZONE));
            DataTable tableParam = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);

            ReceiveMaterialEntity receiveMaterial = new ReceiveMaterialEntity();
            receiveMaterial.RefreshReceiveMaterial(tableParam);
            if (receiveMaterial.ErrorMsg == "")
            {
                MessageService.ShowMessage("刷新成功。", "提示");
                BindMaterialDetail();
            }
            else
            {
                MessageService.ShowError(receiveMaterial.ErrorMsg);
            }
        }


        /// <summary>
        /// 来料接收
        /// </summary>
        private void tsbReceiveMaterial_Click(object sender, EventArgs e)
        {
            ReceiveMaterialEntity receiveMaterial=new ReceiveMaterialEntity();

            if (lueShiftName.Text == "")
            {
                MessageBox.Show("班次信息不能为空！");
            }
            if (txtOperatorNumber.Text == "")
            {
                MessageBox.Show("员工号不能为空！");
            }

            #region 把视图上显示的数据填充到Table中
            int count=0;
            this.gdvMaterialDefault.RefreshData();
            DataTable dataTable = CreatTable();
            for (int i=0; i < gdvMaterialDefault.RowCount; i++)
            {
               
                bool bSelected = false;
                string isSelected = gdvMaterialDefault.GetRowCellValue(i, "IsSelected").ToString();
                if (!bool.TryParse(isSelected, out bSelected)) bSelected = false;
                if (bSelected) //如果被选中
                {
                    //SAP_ISSURE_KEY
                   //W.MBLNR          AS 来料单号,
                   //W.MATNR          AS 物料规格,
                   //W.MATXT          AS 物料描述,
                   //W.CHARG          AS 物料批号
                   //W.AUFNR          AS 工单号码,
                   //W.ERFME          AS 计量单位,
                   //W.ERFMG          AS 实领数量,
                   //W.LLIEF          AS 批次供应商
                    string strRowId = gdvMaterialDefault.GetRowCellValue(i, "ROWNUM").ToString();
                    string strSap_issureKey = gdvMaterialDefault.GetRowCellValue(i, "SAP_ISSURE_KEY").ToString();
                    string strOrderNumber = gdvMaterialDefault.GetRowCellValue(i, "AUFNR").ToString();
                    string strMATNR = gdvMaterialDefault.GetRowCellValue(i, "MATNR").ToString();
                    string strCHARG = gdvMaterialDefault.GetRowCellValue(i, "CHARG").ToString();
                    string strMATXT = gdvMaterialDefault.GetRowCellValue(i, "MATXT").ToString();
                    string strAUFNR = gdvMaterialDefault.GetRowCellValue(i, "AUFNR").ToString();
                    string strERFME = gdvMaterialDefault.GetRowCellValue(i, "ERFME").ToString();
                    string strERFMG = gdvMaterialDefault.GetRowCellValue(i, "ERFMG").ToString();
                    string strLLIEF = gdvMaterialDefault.GetRowCellValue(i, "LLIEF").ToString();
                    string strOperation = gdvMaterialDefault.GetRowCellValue(i, "Operation").ToString();
                    string strOnlineWarehouse = gdvMaterialDefault.GetRowCellValue(i, "OnlineWarehouse").ToString();

                    //工序线边仓不能为空
                    if (strOperation == ""||strOnlineWarehouse == "")
                    {
                        string erroMessage = string.Format("必须为序号为{0}的记录设置工序名称和线上仓名称！", strRowId);
                        MessageBox.Show(erroMessage);
                        return;
                    }

                    string factoryNumber = receiveMaterial.GetFactoryByOrderNumber(strOrderNumber);
                    string factoryOnline = receiveMaterial.GetFactoryByStore(strOnlineWarehouse);
                    //判断工单信息和仓位信息对应的工厂是否一致
                    if (factoryNumber != factoryOnline)
                    {
                        string erroMessage = string.Format("不能将序号{0}的物料接收到指定的线边仓！", strRowId);
                        MessageBox.Show(erroMessage);
                        return; 
                    }


                    dataTable.Rows.Add();
                    dataTable.Rows[count]["SAP_ISSURE_KEY"] = gdvMaterialDefault.GetRowCellValue(i, "SAP_ISSURE_KEY").ToString(); ;
                    dataTable.Rows[count]["MBLNR"] = gdvMaterialDefault.GetRowCellValue(i, "MBLNR").ToString();
                    dataTable.Rows[count]["MATNR"] = gdvMaterialDefault.GetRowCellValue(i, "MATNR").ToString();
                    dataTable.Rows[count]["CHARG"] = gdvMaterialDefault.GetRowCellValue(i, "CHARG").ToString();
                    dataTable.Rows[count]["MATXT"] = gdvMaterialDefault.GetRowCellValue(i, "MATXT").ToString();
                    dataTable.Rows[count]["AUFNR"] = gdvMaterialDefault.GetRowCellValue(i, "AUFNR").ToString();
                    dataTable.Rows[count]["ERFME"] = gdvMaterialDefault.GetRowCellValue(i, "ERFME").ToString();
                    dataTable.Rows[count]["ERFMG"] = gdvMaterialDefault.GetRowCellValue(i, "ERFMG").ToString();
                    dataTable.Rows[count]["LLIEF"] = gdvMaterialDefault.GetRowCellValue(i, "LLIEF").ToString();
                    dataTable.Rows[count]["Operation"] = gdvMaterialDefault.GetRowCellValue(i, "Operation").ToString();
                    dataTable.Rows[count]["OnlineWarehouse"] = gdvMaterialDefault.GetRowCellValue(i, "OnlineWarehouse").ToString();
                    dataTable.Rows[count]["SHIFT_NAME"] = lueShiftName.Text;
                    dataTable.Rows[count]["OPERATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    dataTable.Rows[count]["EDIT_TIMEZONE"] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

                    count++;
                }

            }
            #endregion


            if (dataTable.Rows.Count > 0)
            {
                receiveMaterial.ReceiveLineMaterial(dataTable);

                if (receiveMaterial.ErrorMsg == "")
                {
                    MessageBox.Show("信息保存成功！");
                    BindMaterialDetail();
                }
                else
                {
                    MessageService.ShowError(receiveMaterial.ErrorMsg);
                } 
            }
            else
            {
                MessageBox.Show("必须选择一条数据！");
            }
        }
        /// <summary>
        /// GridControl单元格值改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdvMaterialDefault_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "IsSelected")
            {
                bool b = false;
                string isSelected = e.Value.ToString();
                if (!bool.TryParse(isSelected, out b)) b = false;

                gdvMaterialDefault.SetRowCellValue(e.RowHandle, "IsSelected", b);
            }
            else if (e.Column.FieldName == "OnlineWarehouse")
            {
                string strStoreName = e.Value == null ? string.Empty : e.Value.ToString();
                string strOperationName = GetOperationNameByStore(strStoreName);
                gdvMaterialDefault.SetRowCellValue(e.RowHandle, "OnlineWarehouse", strStoreName);
                gdvMaterialDefault.SetRowCellValue(e.RowHandle, "Operation", strOperationName);
            }
            
        }
        /// <summary>
        /// 自定义绘制单元格的值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdvMaterialDefault_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
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
        /// 进行单元格编辑时的事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdvMaterialDefault_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName == "OnlineWarehouse")//正在编辑线上仓名称。
            {
                this.Param_LineStore.Items.Clear();
                string strWorkOrder = this.gdvMaterialDefault.GetRowCellValue(e.RowHandle, "AUFNR").ToString();
                string strMaterialCode = this.gdvMaterialDefault.GetRowCellValue(e.RowHandle, "MATNR").ToString();
                string strStores = PropertyService.Get(PROPERTY_FIELDS.STORES);

                ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
                DataTable dt = entity.GetStoreByMaterialCode(strWorkOrder, strMaterialCode, strStores);
                if (entity.ErrorMsg == string.Empty)//成功获取线上仓数据。
                {
                    //重新绑定线上仓名称控件。
                    foreach (DataRow dr in dt.Rows)
                    {
                        this.Param_LineStore.Items.Add(dr["STORE_NAME"]);
                    }
                }

            }
        }
    }
}
