using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SolarViewer.Hemera.Utils.Controls;
using SolarViewer.Gui.Framework.Gui;
using System.Collections;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Share.Common;
using SolarViewer.Hemera.Share.Interface;
using SolarViewer.Hemera.Utils.Entities;
using SolarViewer.Gui.Core;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using SolarViewer.Hemera.Share.Interface.RFC;
using Astronergy.Addins.WMS.Gui;

namespace Astronergy.Addins.WMS
{
    public partial class OutboundQCControl : BaseUserCtrl
    {
        IViewContent iview = null;
        OutboundOperationEntity _entity = new OutboundOperationEntity();
        string Fusername = string.Empty;
        string IsEdit = "N";
        public int retIdx;

        public OutboundQCControl(IViewContent view)
        {
            InitializeComponent();
            this.iview = view;
            //取登录用户名
            Fusername = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) 
            {                         
                this.txtOutboundNo.Select();
            }

        }
        //查询按钮事件
        private void tsbSearch_Click(object sender, EventArgs e)
        {
            string OutboundNo = this.txtOutboundNo.Text.Trim();
            string VbelnNo = this.txtVbeln.Text.Trim();
            bool QcResult = true;
            if ((OutboundNo.Length == 0) && (VbelnNo.Length == 0))
            {
                MessageBox.Show("请输入出货单号或外向交货单号！");
                gcList.DataSource = null;
                this.txtOutboundNo.Select();
                return;
            }
     
            //根据外向交货单号或出库单号取抬头数据
            DataSet dsOutboudInfo = this._entity.getOutboudInfo(OutboundNo, VbelnNo,"","");
           if (dsOutboudInfo.Tables[0].Rows.Count <= 0)
            {
                MessageBox.Show("没有您指定查询条件下的出货单抬头信息！");
                return;
            }
           else if (dsOutboudInfo.Tables[0].Rows.Count > 1)
            {
                ShowBillNo SBillNo = new ShowBillNo(dsOutboudInfo.Tables[0]);
                SBillNo.pOutboundQCControl = this;
                SBillNo.ShowDialog();
            }
            else 
            {
                retIdx = 0;
                
            }
           if (retIdx >= 0)
           {
               DataRow dr = dsOutboudInfo.Tables[0].Rows[retIdx];
               txtCreatedate.Text = (dr["ERDAT"].ToString()).Substring(0, 10);//
               txtCreator.Text = dr["CREATED_BY"].ToString();//
               txtSalesto.Text = dr["SALESTO"].ToString();//
               txtSalestoName.Text = dr["SHIPTO"].ToString();//
               txtShipto.Text = dr["SALESTO_NAME"].ToString();//
               txtShiptoName.Text = dr["SHIPTO_NAME"].ToString();//
               txtVbeln.Text = dr["VBELN"].ToString();//
               txtOutboundNo.Text = dr["OUTBANDNO"].ToString();//
               txtStatus.Text = dr["status"].ToString();//            
           }
           
           
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            //根据查询条件取外向交货单明细数据
            DataSet dsParam = this.GetQueryCondition();

            DataSet dsReturn = this._entity.Query(dsParam, ref config);
            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;

            if(!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }
            if (dsReturn.Tables.Count > 0)
            {
                //DataSet dsView=null;
                //if (this.txtStatus.Text.Trim() == "已确认")
                //{
                //    dsView = this.AddColumnDataSet(dsReturn);
                //}
                //else
                //    dsView = dsReturn;     

                gcList.DataSource = dsReturn.Tables[0];
                gcList.MainView = gvList;
                gvList.BestFitColumns();
                this.gvList.OptionsView.ColumnAutoWidth = false;
                this.SetComboBox();
               
                if (this.txtStatus.Text.Trim() == "已检验")
                {
                    gvList.OptionsBehavior.ReadOnly = true;
                    DataTable dt = dsReturn.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToString(dr["QC_RESULT"]) == "不合格")
                        {
                            QcResult = false;
                        }
                    }
                    this.tsbSave.Enabled = false;
                    this.tsbEdit.Enabled = true;
                    this.tsbDelete.Enabled = true;
                    this.tsbComfirm.Enabled = false;
                    if (QcResult == true)
                    {
                        this.tsbComfirm.Enabled = true;
                    }
                }
                else if (this.txtStatus.Text.Trim() == "已确认")
                {
                    gvList.OptionsBehavior.ReadOnly = false;
                    this.tsbSave.Enabled = true;
                    this.tsbComfirm.Enabled = false;
                    this.tsbEdit.Enabled = false;
                    this.tsbDelete.Enabled = false;
                }
                else if (this.txtStatus.Text.Trim() == "已过账")
                {
                    gvList.OptionsBehavior.ReadOnly = true;
                    this.tsbSave.Enabled = false;
                    this.tsbComfirm.Enabled = false;
                    this.tsbEdit.Enabled = false;
                    this.tsbDelete.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("没有您指定查询条件下的出货单明细！");
                return;
            }
        }
        private void SetComboBox()
        {

            RepositoryItemComboBox combobox = new RepositoryItemComboBox();
            combobox.Items.Add("合格");
            combobox.Items.Add("不合格");
            //combobox.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            gcList.RepositoryItems.Add(combobox);
            this.gvList.Columns["QC_RESULT"].ColumnEdit = combobox;
            this.gvList.Columns["PKG_MAT"].ColumnEdit = combobox;
            this.gvList.Columns["BILL_BRND"].ColumnEdit = combobox;
            this.gvList.Columns["CANTR"].ColumnEdit = combobox;
            this.gvList.Columns["F_OUTB_CUSTM"].ColumnEdit = combobox;
            this.gvList.Columns["EL"].ColumnEdit = combobox;
            this.gvList.Columns["DATA_FORMT"].ColumnEdit = combobox;
            this.gvList.Columns["LIST_ABSENCE"].ColumnEdit = combobox;
            this.gvList.Columns["LiST_ERR"].ColumnEdit = combobox;
            this.gvList.Columns["CELL"].ColumnEdit = combobox;
            this.gvList.Columns["MOD_ERR"].ColumnEdit = combobox;
            this.gvList.Columns["QLVL_ERR"].ColumnEdit = combobox;
            this.gvList.Columns["FRAME"].ColumnEdit = combobox;
            this.gvList.Columns["BRND_PARM_ERR"].ColumnEdit = combobox;
            this.gvList.Columns["CONT_LOCK_BRK"].ColumnEdit = combobox;
            this.gvList.Columns["CUSTM_CK"].ColumnEdit = combobox;
        }

        //取查询条件
        private DataSet GetQueryCondition() 
        {
            string VbelnNo = txtVbeln.Text.Trim();
            string OutboundNo = txtOutboundNo.Text.Trim();
            Hashtable ht=new Hashtable();
            if (!string.IsNullOrEmpty(VbelnNo)) 
            {
                ht.Add(AWMS_OUTB_ITEM_FIELDS.Field_VBELN, VbelnNo);
            }
            if (!string.IsNullOrEmpty(OutboundNo))
            {
                ht.Add(AWMS_OUTB_ITEM_FIELDS.Field_OUTBANDNO, OutboundNo);
            }
            DataTable dt = CommonUtils.ParseToDataTable(ht);
            DataSet ds = new DataSet();
            dt.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            ds.Tables.Add(dt) ;
            return ds;
        }
        private DataSet AddColumnDataSet(DataSet dsParam)
        {
            DataSet ds = dsParam;
            DataTable dt = dsParam.Tables[0];

            dt.Columns.Add("PKG_MAT");         //包材缺陷数
            dt.Columns.Add("BILL_BRND");       //清单标识缺陷数
            dt.Columns.Add("CANTR");           //货柜缺陷数
            dt.Columns.Add("F_OUTB_CUSTM");    //组件符合出厂或客户要求缺陷数
            dt.Columns.Add("EL");              //EL
            dt.Columns.Add("DATA_FORMT");      //数据格式错误
            dt.Columns.Add("LIST_ABSENCE");    //清单缺失
            dt.Columns.Add("LiST_ERR");        //清单错误
            dt.Columns.Add("CELL");            //电池片
            dt.Columns.Add("MOD_ERR");         //型号错误
            dt.Columns.Add("QLVL_ERR");        //质量等级不符合要求
            dt.Columns.Add("FRAME");           //边框
            dt.Columns.Add("BRND_PARM_ERR");   //铭牌参数错误
            dt.Columns.Add("CONT_LOCK_BRK");   //集装箱锁破损
            dt.Columns.Add("CUSTM_CK");        //客户验货缺陷

            //ds.Tables.Add(dt);//向ds中添加Table  
            return ds;
        }
       
        private void tsbCancel_Click(object sender, EventArgs e)
        {
            this.iview.WorkbenchWindow.CloseWindow(false);
        }
        //取消按钮事件
        private void tsbUndo_Click(object sender, EventArgs e)
        {
            ResetControlValue(); 
        }
        //保存
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定保存质检信息并过账？", "确定", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (this.gvList.State == GridState.Editing && this.gvList.IsEditorFocused && this.gvList.EditingValueModified)
                {
                    this.gvList.SetFocusedRowCellValue(this.gvList.FocusedColumn, this.gvList.EditingValue);
                }
                this.gvList.UpdateCurrentRow();

                bool result = true;
                string Msg = string.Empty;
                string strReturn = string.Empty;
                string VbelnNo = txtVbeln.Text.Trim();
                string OutboundNo = txtOutboundNo.Text.Trim();
                string QC_PERSON = Fusername;
                int flag = 0;

                //string QcNo=txtQcNo.Text.Trim();
                DataTable dtList = this.gcList.DataSource as DataTable;
                if (dtList == null || dtList.Rows.Count <= 0)
                {
                    MessageService.ShowMessage("出货检验明细不能为空！", "提示");
                    this.txtVbeln.Select();
                    this.txtVbeln.SelectAll();
                    return;
                }
                dtList.TableName = AWMS_OUTB_QC_FIELDS.TABLE_NAME_OUTB_QC;
                DataSet dsParams = new DataSet();
                DataTable dtParams = dtList.Copy();

                //合并数据集到dsParams
                dsParams.Merge(dtParams);

                //保存质检数据
                Msg = this._entity.SetQcResult(dsParams, OutboundNo, VbelnNo, QC_PERSON,  IsEdit,out result);
                if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                {
                    MessageService.ShowError(this._entity.ErrorMsg);
                }
                else
                {
                    this.txtStatus.Text = "已检验";
                    MessageService.ShowMessage(Msg, "提示");

                    if (result == true)//检验合格后过账
                    {
                        this.tsbComfirm.Enabled = true;

                        //过账    
                        OUTB_DELIVERY_CONFIRM(VbelnNo, OutboundNo, out flag);
                    }
                    
                    ResetControlValue();

                }

            }
            //int flag = 0;
            //外向交货单过账
            //strReturn = OUTB_DELIVERY_CONFIRM(VbelnNo, OutboundNo, out flag);

            //Msg = this._entity.UpdateTable(strReturn, VbelnNo, OutboundNo, flag);
            
            //MessageService.ShowMessage(Msg, "提示");
        }
        /// <summary>
        /// 进行外向交货过帐
        /// </summary>
        /// <param name="VebelNO"></param>

        private void  OUTB_DELIVERY_CONFIRM(string VebelNO, string OutboundNo,out int flag)
        {               
            DataSet ds = new DataSet();
            DataTable H_DATA =new DataTable();
            DataTable H_CONTROL =new DataTable();
            DataTable returnTable;
            string strReturn = string.Empty;
            string Mes = string.Empty;

           
            flag = 1;

            string rfcFuntionName = "BAPI_OUTB_DELIVERY_CONFIRM_DEC";
            IServerObjFactory sapserverFactory = CallRemotingService.GetRemoteObject();

            if (null != sapserverFactory)
            {
                IRFCEngine rfcCallObj = sapserverFactory.Get<IRFCEngine>();
            
                DataSet dsParams = new DataSet();
                DataRow HEADER_DATA ;
                DataRow HEADER_CONTROL;

                //定义一个table用于传入HEADER_DATA
                H_DATA.Columns.Add("DELIV_NUMB", typeof(string));
                
                //传入HEADER_DATA值
                HEADER_DATA = H_DATA.NewRow();                
                HEADER_DATA["DELIV_NUMB"] = VebelNO;
                H_DATA.Rows.Add(HEADER_DATA);
                
                //定义一个table用于传入HEADER_CONTROL
                H_CONTROL.Columns.Add("DELIV_NUMB", typeof(string));
                H_CONTROL.Columns.Add("POST_GI_FLG", typeof(string));
                H_CONTROL.Columns.Add("DELIV_DATE_FLG", typeof(string));
                H_CONTROL.Columns.Add("GROSS_WT_FLG", typeof(string));
                
                //传入HEADER_CONTROL值
                HEADER_CONTROL = H_CONTROL.NewRow();
                HEADER_CONTROL["DELIV_NUMB"] = VebelNO;
                HEADER_CONTROL["POST_GI_FLG"] = "X";
                HEADER_CONTROL["DELIV_DATE_FLG"] = "X";
                HEADER_CONTROL["GROSS_WT_FLG"] = "X";
                H_CONTROL.Rows.Add(HEADER_CONTROL);

                //将table加入dataset
                dsParams.Tables.Add(H_DATA);
                dsParams.Tables[0].TableName = "HEADER_DATA";
                dsParams.Tables.Add(H_CONTROL);
                dsParams.Tables[1].TableName = "HEADER_CONTROL";

                //dsParams.ExtendedProperties.Add("HEADER_DATA", HEADER_DATA);
                //dsParams.ExtendedProperties.Add("HEADER_CONTROL", HEADER_CONTROL);
                dsParams.ExtendedProperties.Add("DELIVERY", VebelNO);

                //调用BAPI
                ds = rfcCallObj.ExecuteRFC(rfcFuntionName, dsParams);
                returnTable=ds.Tables["RETURN"];
                
                if (returnTable.Rows.Count>0)
                {               
                    for (int i = 0; i < returnTable.Rows.Count; i++)
                    {
                        //失败的产品信息dsOutboudInfo.Tables[0].Rows[0][0].ToString()
                        string type = returnTable.Rows[i]["TYPE"].ToString();
                        string id = returnTable.Rows[i]["id"].ToString();
                        string number = returnTable.Rows[i]["NUMBER"].ToString();
                        string message = returnTable.Rows[i]["MESSAGE"].ToString();
                        string system = returnTable.Rows[i]["SYSTEM"].ToString();
                        string REF_EXT1 = returnTable.Rows[i]["MESSAGE_V1"].ToString();
                        string REF_EXT2 = returnTable.Rows[i]["MESSAGE_V2"].ToString();
                        string REF_EXT3 = returnTable.Rows[i]["MESSAGE_V3"].ToString();

                        Mes = Mes + type + message;
                        if ((type=="E")||(type=="A"))
                        {

                            flag = 0;
                        }           
                    }
                    
                }
                if (flag == 0)
                {
                    Mes = "外向交货单过账失败:" + Mes;
                }
                else
                {
                    Mes = "外向交货单过账成功！";
                }
            }
            //return Mes;
            MessageService.ShowMessage(Mes, "提示");
            this._entity.UpdateTable(strReturn, VebelNO, OutboundNo, flag);
            
        }

        private void OUTB_DELIVERY_Cancel(string VebelNO, string OutboundNo,out int flag)
        {
            string Mes = string.Empty;
            DataSet dsReturn = new DataSet();
            DataTable returnTable;
            
            string rfcFuntionName = "ZDZSW_XSDD";
            IServerObjFactory sapserverFactory = CallRemotingService.GetRemoteObject();

            flag = 1;
            if (null != sapserverFactory)
            {
                IRFCEngine rfcCallObj = sapserverFactory.Get<IRFCEngine>();

                DataSet dsParams = new DataSet();
                dsParams.ExtendedProperties.Add("WK_VBELN", VebelNO);
                dsParams.ExtendedProperties.Add("BZ", "WF");

                //调用BAPI
                dsReturn = rfcCallObj.ExecuteRFC(rfcFuntionName, dsParams);
                returnTable = dsReturn.Tables["LI_RETURN"];

                if (returnTable.Rows.Count > 0)
                {
                    for (int i = 0; i < returnTable.Rows.Count; i++)
                    {
                        //失败的产品信息
                        string type = returnTable.Rows[i]["TYPE"].ToString();
                        string id = returnTable.Rows[i]["id"].ToString();
                        string number = returnTable.Rows[i]["NUMBER"].ToString();
                        string message = returnTable.Rows[i]["MESSAGE"].ToString();
                        string system = returnTable.Rows[i]["SYSTEM"].ToString();
                        string REF_EXT1 = returnTable.Rows[i]["MESSAGE_V1"].ToString();
                        string REF_EXT2 = returnTable.Rows[i]["MESSAGE_V2"].ToString();
                        string REF_EXT3 = returnTable.Rows[i]["MESSAGE_V3"].ToString();

                        Mes = Mes + type + message + "\r\n";
                        if ((type == "E") || (type == "A"))
                        {
                            flag = 0;
                        }
                    }

                }
                if (flag == 0)
                {
                    Mes = "外向交货单过账取消失败:" + Mes;
                }
                else
                {
                    Mes = "外向交货单过账取消成功！";
                }
                MessageService.ShowMessage(Mes, "提示");
            }

        }
        private void ResetControlValue()
        {
            this.txtCreatedate.Text = string.Empty;
            this.txtCreator.Text = string.Empty;
            this.txtSalesto.Text = string.Empty;
            this.txtSalestoName.Text = string.Empty;
            this.txtShipto.Text = string.Empty;
            this.txtShiptoName.Text = string.Empty;
            this.txtOutboundNo.Text = string.Empty;
            this.txtVbeln.Text = string.Empty;
            this.txtStatus.Text = string.Empty;
            this.tsbSave.Enabled = false;
            this.tsbComfirm.Enabled = false;
            this.tsbEdit.Enabled = false;
            this.tsbDelete.Enabled = false;
                      
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList != null)
            {
                dtList.Rows.Clear();
            }
            txtVbeln.Select();
        }

        private void txtOutboundNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == 13) &&(!string.IsNullOrEmpty(this.txtOutboundNo.Text.Trim())))
            {
                this.tsbSearch_Click(sender, e);
            }
        }

        private void txtVbeln_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (!string.IsNullOrEmpty(this.txtVbeln.Text.Trim()))
                {
                    this.tsbSearch_Click(sender, e);
                }

                if (string.IsNullOrEmpty(this.txtOutboundNo.Text.Trim()))
                {
                    this.txtOutboundNo.Select();  
                }
            }
        }

        private void tsbComfirm_Click(object sender, EventArgs e)
        {
            string strCom = this.txtStatus.Text.Trim();
            string VebelNO = this.txtVbeln.Text.Trim();
            string OutboundNo = this.txtOutboundNo.Text.Trim();
            string strReturn = string.Empty;
            string Msg = string.Empty;
            int flag = 0;

            if ((VebelNO == "") || (OutboundNo == ""))
            {
                MessageBox.Show("外向交货单号或出货单号不能为空！");
                return;
            }
            if (strCom == "已确认")
            {
                MessageBox.Show("请先检验再过账！");
                return;
            }
            if (MessageBox.Show("确定执行外向交货单过账？", "确定", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                //过账    
                OUTB_DELIVERY_CONFIRM(VebelNO, OutboundNo, out flag);

                //更新表
                //Msg = this._entity.UpdateTable(strReturn, VebelNO, OutboundNo, flag);
                ResetControlValue();
            }  
            
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            string strCom = this.txtStatus.Text.Trim();
            string VebelNO = this.txtVbeln.Text.Trim();
            string OutboundNo = this.txtOutboundNo.Text.Trim();
            string Del_Person=Fusername;
            if ((VebelNO == "") || (OutboundNo == ""))
            {
                MessageBox.Show("外向交货单号或出货单号不能为空！");
                return;
            }
            if ((strCom == "已检验") || (strCom == "已过账"))
            {
                if (MessageBox.Show("确定删除质检信息？", "确定", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    //如果已过账，则先取消过账，成功后再删除检验信息
                    if (strCom == "已过账")
                    {
                        int flag = 1;
                        OUTB_DELIVERY_Cancel(VebelNO, OutboundNo, out flag);
                        if (flag == 1)
                        {
                            this._entity.DeleteQcResult(OutboundNo, VebelNO, Del_Person);
                            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                            {
                                MessageService.ShowError(this._entity.ErrorMsg);
                            }
                            else 
                            {
                                MessageService.ShowMessage("检验结果删除成功！", "提示");
                            }
                        }
                    }
                    //否则直接删除检验信息
                    else
                    {
                        this._entity.DeleteQcResult(OutboundNo, VebelNO, Del_Person);
                        if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                        {
                            MessageService.ShowError(this._entity.ErrorMsg);
                        }
                        else
                        {
                            MessageService.ShowMessage("检验结果删除成功！", "提示");
                        }
                    }
                    ResetControlValue();
                }

            }
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            gvList.OptionsBehavior.ReadOnly = false;
            this.tsbSave.Enabled = true;
            IsEdit = "Y";
        }

       
    }
}
