using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils.Controls;

using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotExceptionProcess : BaseUserCtrl
    {
        LotExceptionEngine _entity = new LotExceptionEngine();
        string _state;
        int _lotExceptionID = 0; string _lotNo;


        public LotExceptionProcess()
        {
            InitializeComponent();
            InitializeLanguage();
            GridViewHelper.SetGridView(gvLot);
        }



        private void InitializeLanguage()
        {
            this.btnQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.101}");// "查询";
            this.btnNew.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.102}");// "新增";
            this.btnEdit.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.103}");// "修改";
            this.btnDel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.104}");// "删除";
            this.btnSave.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.105}");// "保存";
            this.lcgLotInfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.107}");// "批次信息";
            this.layoutControlGroup1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.108}");// "批次信息";
            this.layoutControlGroup2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.109}");// "批次信息";
            this.layoutControlGroup3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.110}");// "批次信息";
            this.layoutControlGroup4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.111}");// "批次信息";
            this.layoutControlGroup5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.112}");// "批次信息";
            this.LOT_EXCEPTION_ID.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.113}");// "序号";
            this.LOT_NUMBER.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.114}");// "序列号";
            this.CREATE_OPERATION_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.115}");// "登记人";
            this.CREATE_DATE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.116}");// "时间";
            this.DOPERATION.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.117}");// "发现工序";
            this.FAB.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.118}");// "厂别";
            this.CLASS.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.119}");// "班别";
            this.CUSTOMER.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.120}");// "客户";
            this.PORDUCT_TYPE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.121}");// "组件型号";
            this.ADVERSE_POSITION.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.122}");// "不良位置";
            this.ADVERSE_DESC.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.123}");// "不良描述";
            this.REASON.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.124}");// "原因分析";
            this.IMPROVE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.125}");// "改善对策";
            this.ADVERSE_CLASS.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.126}");// "不良分类";
            this.RESOULT.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.127}");// "判定结果";
            this.DutyLine.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.128}");// "责任线别";
            this.RESPONSE_CLASS.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.129}");// "责任班别";
            this.RESPONSE_POSITION.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.130}");// "责任工序";
            this.ATTRIBUTION_RESPONSE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.131}");// "责任归属";
            this.REMARK.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.132}");// "备注";
            this.SUPPLIER.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.133}");// "供应商";
            this.CELL_GREAD.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.134}");// "电池等级";
            this.WELDING_TYPE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.135}");// "焊带型号";
            this.FRAME_TYPE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.136}");// "边框型号";
            this.PARTIALLY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.137}");// "半成品状况";
            this.JudgeTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.138}");// "判定时间";
            this.AMES_GREAD.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.139}");// "AMES等级";
            this.EQUIPMENT1.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.140}");// "串焊机台";
            this.EQUIPMENT2.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.141}");// "层压机";
            this.EQUIPMENT3.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.142}");// "装框班";
            this.EQUIPMENT4.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.143}");// "装框机";
            this.EQUIPMENT5.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.144}");// "清洁班";
            this.EQUIPMENT6.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.145}");// "清洁台";
            this.EQUIPMENT7.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.146}");// "测试仪";
            this.LOT_CREATE_TIME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.147}");// "创批时间";
            this.layoutControlGroup7.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.148}");// "不良信息";
            this.layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.149}");// "序列号";
            this.layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.150}");// "记录人";
            this.layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.151}");// "时间";
            this.layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.152}");// "发现工序";
            this.layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.153}");// "厂别";
            this.layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.154}");// "班别";
            this.layoutControlItem7.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.155}");// "客户";
            this.layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.156}");// "组件型号";
            this.layoutControlItem9.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.157}");// "不良位置";
            this.layoutControlItem11.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.158}");// "责任工序";
            this.layoutControlItem12.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.159}");// "责任归属";
            this.layoutControlItem13.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.160}");// "不良分类";
            this.layoutControlItem14.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.161}");// "判定结果";
            this.layoutControlItem15.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.162}");// "电池等级";
            this.layoutControlItem17.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.163}");// "焊带型号";
            this.layoutControlItem18.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.164}");// "边框型号";
            this.layoutControlItem19.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.165}");// "厂商";
            this.layoutControlItem20.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.166}");// "边框班";
            this.layoutControlItem21.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.167}");// "半成品";
            this.layoutControlItem22.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.168}");// "装框机";
            this.layoutControlItem23.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.169}");// "清洁班";
            this.layoutControlItem24.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.170}");// "串焊机台";
            this.layoutControlItem25.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.171}");// "层压机";
            this.layoutControlItem26.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.172}");// "清洗台";
            this.layoutControlItem27.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.173}");// "测试仪";
            this.layoutControlItem28.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.174}");// "AMES等级";
            this.layoutControlItem29.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.175}");// "创建时间";
            this.layoutControlItem30.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.176}");// "不良描述";
            this.layoutControlItem31.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.177}");// "原因分析";
            this.layoutControlItem32.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.178}");// "改善对策";
            this.layoutControlItem33.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.179}");// "备注";
            this.txtExceptionTime.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.180}");// "发现时间";
            this.layoutControlItem35.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.181}");// "责任班别";
            this.layoutControlItem10.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExceptionProcess.182}");// "责任线别";
            this.lblMenu.Text = "质量管理>质量作业>手工不良记录";

            this.eTime.Text = DateTime.Now.AddDays(1).ToShortDateString();
        }




        private void btnClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ActionState(0);
        }
        /// <summary>
        /// 0 new,1 update
        /// </summary>
        /// <param name="state"></param>
        private void ActionState(int state)
        {
            if (state == 0)
            {
                clareText();

                enableControl(true);
                _state = "NEW";
            }
            else if (state == 1)
            {
                enableControl(true);
                _state = "UPDATE";
            }
            else
            {
                enableControl(false);
                _state = "None";
            }

        }
        private void clareText()
        {
            txtADVERSE.Text = "";
            txtADVERSE_DESC.Text = "";
            txtCELLGREAD.Text = "";
            txtEQUIPMENT1.Text = "";
            txtEQUIPMENT2.Text = "";
            txtEQUIPMENT3.Text = "";
            txtEQUIPMENT4.Text = "";
            txtEQUIPMENT5.Text = "";
            txtEQUIPMENT6.Text = "";
            txtEQUIPMENT7.Text = "";
            txtFRAMETYPE.Text = "";
            txtIMPROVE.Text = "";
            txtLotNo.Text = "";
            //txtOperater.Text = "";
            txtPARTIALLY.Text = "";
            txtProductType.Text = "";
            txtREASON.Text = "";
            txtRemark.Text = "";

            txtSUPPLIER.Text = "";
            //txtTime.Text = "";
            txtWELDINGTYPE.Text = "";
            txtLotExceptionTime.Text = "";
            txtAMESGREAD.Text = "";
            txtCreateLotTime.Text = "";

            txtTime.Text = DateTime.Now.ToString();
            txtOperater.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

        }

        void enableControl(bool state)
        {
            txtADVERSE.Enabled = state;
            txtADVERSE_DESC.Enabled = state;
            txtCELLGREAD.Enabled = state;
            txtEQUIPMENT1.Enabled = state;
            txtEQUIPMENT2.Enabled = state;
            txtEQUIPMENT3.Enabled = state;
            txtEQUIPMENT4.Enabled = state;
            txtEQUIPMENT5.Enabled = state;
            txtEQUIPMENT6.Enabled = state;
            txtEQUIPMENT7.Enabled = state;
            txtFRAMETYPE.Enabled = state;
            txtIMPROVE.Enabled = state;
            txtLotNo.Enabled = state;
            txtOperater.Enabled = false;
            txtPARTIALLY.Enabled = state;
            txtProductType.Enabled = state;
            txtREASON.Enabled = state;
            txtRemark.Enabled = state;

            txtSUPPLIER.Enabled = state;
            txtTime.Enabled = false;
            txtWELDINGTYPE.Enabled = state;
            txtCreateLotTime.Enabled = false;
            txtAMESGREAD.Enabled = state;
            txtLotExceptionTime.Enabled = state;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (_lotNo == "" || _lotNo != txtLotNo.Text || _lotExceptionID == 0)
            {
                MessageBox.Show("请选择要删除的行");
            }

            DataSet ds = _entity.DeleteLotExceptionDetail(_lotExceptionID.ToString());
            //if ()
            //{

            //    MessageBox.Show("删除成功");
            //}
            //else
            //{
            //    MessageBox.Show("删除失败");
            //}

        }

        private void gvLot_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (gvLot.FocusedRowHandle >= 0)
            {
                try
                {
                    _lotExceptionID = int.Parse(gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "LOT_EXCEPTION_ID").ToString());
                    txtLotNo.Text = _lotNo = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "LOT_NUMBER").ToString();
                    txtOperater.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "CREATE_OPERATION_NAME").ToString();
                    txtTime.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "CREATE_DATE").ToString();
                    listOperation.SelectedText = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "DOPERATION").ToString();
                    listFab.SelectedText = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "FAB").ToString();
                    listClass.SelectedText = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "CLASS").ToString();
                    listCustomer.SelectedText = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "CUSTOMER").ToString();
                    txtProductType.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "PORDUCT_TYPE").ToString();
                    txtADVERSE.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "ADVERSE_POSITION").ToString();
                    txtADVERSE_DESC.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "ADVERSE_DESC").ToString();
                    txtREASON.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "REASON").ToString();
                    txtIMPROVE.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "IMPROVE").ToString();
                    lsitADVERSECLASS.SelectedText = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "ADVERSE_CLASS").ToString();
                    listRESOULT.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "RESOULT").ToString();
                    ListRESPONSECLASS.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "RESPONSE_CLASS").ToString();
                    listRESPONSEPOSITION.SelectedText = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "RESPONSE_POSITION").ToString();
                    listATTRIBUTIONRESPONSE.SelectedText = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "ATTRIBUTION_RESPONSE").ToString();
                    txtRemark.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "REMARK").ToString();
                    txtSUPPLIER.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "SUPPLIER").ToString();
                    txtCELLGREAD.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "CELL_GREAD").ToString();
                    txtWELDINGTYPE.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "WELDING_TYPE").ToString();
                    txtFRAMETYPE.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "FRAME_TYPE").ToString();
                    txtPARTIALLY.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "PARTIALLY").ToString();
                    txtAMESGREAD.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "AMES_GREAD").ToString();
                    txtCreateLotTime.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "LOT_CREATE_TIME").ToString();
                    txtEQUIPMENT1.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "EQUIPMENT1").ToString();
                    txtEQUIPMENT2.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "EQUIPMENT2").ToString();
                    txtEQUIPMENT3.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "EQUIPMENT3").ToString();
                    txtEQUIPMENT4.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "EQUIPMENT4").ToString();
                    txtEQUIPMENT5.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "EQUIPMENT5").ToString();
                    txtEQUIPMENT6.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "EQUIPMENT6").ToString();
                    txtEQUIPMENT7.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "EQUIPMENT7").ToString();
                    txtLotExceptionTime.Text = gvLot.GetRowCellValue(gvLot.FocusedRowHandle, "JudgeTime").ToString();


                }
                catch
                { }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ActionState(1);
        }

        private void bindGridView(string lotno, string startTime, string endTime)
        {
            DataTable data = _entity.GetLotExceptionData(lotno, startTime, endTime).Tables[0];
            gcLot.DataSource = data;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtLotNo.Text.Trim() == "")
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg008}"));//序列号不能为空
                //MessageBox.Show("序列号不能为空");
                return;
            }

            DataTable lotException = new DataTable();
            lotException.Columns.Add("LOT_EXCEPTION_ID");
            lotException.Columns.Add("LOT_NUMBER");
            lotException.Columns.Add("CREATE_OPERATION_NAME");
            lotException.Columns.Add("CREATE_DATE");
            lotException.Columns.Add("DOPERATION");
            lotException.Columns.Add("FAB");
            lotException.Columns.Add("CLASS");
            lotException.Columns.Add("CUSTOMER");
            lotException.Columns.Add("PORDUCT_TYPE");

            lotException.Columns.Add("ADVERSE_POSITION");
            lotException.Columns.Add("ADVERSE_DESC");
            lotException.Columns.Add("REASON");
            lotException.Columns.Add("IMPROVE");
            lotException.Columns.Add("ADVERSE_CLASS");
            lotException.Columns.Add("RESOULT");
            lotException.Columns.Add("RESPONSE_CLASS");
            lotException.Columns.Add("RESPONSE_POSITION");


            lotException.Columns.Add("ATTRIBUTION_RESPONSE");
            lotException.Columns.Add("REMARK");
            lotException.Columns.Add("SUPPLIER");
            lotException.Columns.Add("CELL_GREAD");
            lotException.Columns.Add("WELDING_TYPE");
            lotException.Columns.Add("FRAME_TYPE");
            lotException.Columns.Add("PARTIALLY");
            lotException.Columns.Add("EQUIPMENT1");

            lotException.Columns.Add("EQUIPMENT2");
            lotException.Columns.Add("EQUIPMENT3");
            lotException.Columns.Add("EQUIPMENT4");
            lotException.Columns.Add("EQUIPMENT5");
            lotException.Columns.Add("EQUIPMENT6");
            lotException.Columns.Add("EQUIPMENT7");
            lotException.Columns.Add("AMES_GREAD");
            lotException.Columns.Add("LOT_CREATE_TIME");
            lotException.Columns.Add("JudgeTime");
            lotException.Columns.Add("DutyLine");

            DataRow row = lotException.NewRow();
            row["LOT_EXCEPTION_ID"] = _lotExceptionID;
            row["LOT_NUMBER"] = txtLotNo.Text;
            row["CREATE_OPERATION_NAME"] = txtOperater.Text;
            row["CREATE_DATE"] = txtTime.Text;
            row["DOPERATION"] = listOperation.Text;
            row["FAB"] = listFab.Text;
            row["CLASS"] = listClass.Text;
            row["CUSTOMER"] = listCustomer.Text;
            row["PORDUCT_TYPE"] = txtProductType.Text;
            row["ADVERSE_POSITION"] = txtADVERSE.Text;
            row["ADVERSE_DESC"] = txtADVERSE_DESC.Text;
            row["REASON"] = txtREASON.Text;
            row["IMPROVE"] = txtIMPROVE.Text;
            row["ADVERSE_CLASS"] = lsitADVERSECLASS.Text;
            row["RESOULT"] = listRESOULT.Text;
            row["RESPONSE_CLASS"] = ListRESPONSECLASS.Text;
            row["RESPONSE_POSITION"] = listRESPONSEPOSITION.Text;
            row["REMARK"] = txtRemark.Text;
            row["SUPPLIER"] = txtSUPPLIER.Text;
            row["CELL_GREAD"] = txtCELLGREAD.Text;
            row["WELDING_TYPE"] = txtWELDINGTYPE.Text;
            row["FRAME_TYPE"] = txtFRAMETYPE.Text;
            row["PARTIALLY"] = txtPARTIALLY.Text;
            row["EQUIPMENT1"] = txtEQUIPMENT1.Text;
            row["EQUIPMENT2"] = txtEQUIPMENT2.Text;
            row["EQUIPMENT3"] = txtEQUIPMENT3.Text;
            row["EQUIPMENT4"] = txtEQUIPMENT4.Text;
            row["EQUIPMENT5"] = txtEQUIPMENT5.Text;
            row["EQUIPMENT6"] = txtEQUIPMENT6.Text;
            row["EQUIPMENT7"] = txtEQUIPMENT7.Text;
            row["AMES_GREAD"] = txtAMESGREAD.Text;
            row["LOT_CREATE_TIME"] = txtCreateLotTime.Text;
            row["JudgeTime"] = txtLotExceptionTime.Text;
            row["DutyLine"] = listDutyLine.Text;
            lotException.Rows.Add(row);
            DataSet ds = new DataSet();
            ds.Tables.Add(lotException);

            if (_state == "NEW")
            {
                DataSet ret = _entity.InsertLotExceptionDetail(ds);
                //if (ret)
                //{
                //    MessageBox.Show("插入数据成功");
                //}
                //else
                //{
                //    MessageBox.Show("插入数据失败");
                //}
            }
            else if (_state == "UPDATE")
            {
                DataSet ret = _entity.UpdateLotExceptionDatail(ds);
                //if (ret)
                //{
                //    MessageBox.Show("更新数据成功");
                //}
                //else
                //{
                //    MessageBox.Show("更新数据失败");
                //}
            }
            bindGridView(txtLotNo.Text, DateTime.Now.ToShortDateString(), DateTime.Now.AddDays(1).ToShortDateString());
            clareText();
            //_state = "None";

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            //LotExecption lotexption = new LotExecption();
            string lotNo = string.Empty;
            string startTime = string.Empty;
            string endTime = string.Empty;
            if (txtLotNo.Text.Trim() != "" || (sTime.Text.Trim() != "" && eTime.Text.Trim() != ""))
            {
                lotNo = txtLotNo.Text;
                startTime = sTime.Text;
                endTime = eTime.Text;
                bindGridView(lotNo, startTime, endTime);
                enableControl(false);
            }
        }

        private void BindingListItem()
        {
            DataSet data = _entity.GetAllOperations();
            listOperation.Properties.Items.Clear();

            if (data != null)
            {
                DataTable dt = data.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listOperation.Properties.Items.Add(dt.Rows[i]["ROUTE_OPERATION_NAME"]);
                    }
                }
            }

            data = _entity.GetAllFab();
            listFab.Properties.Items.Clear();
            if (data != null)
            {
                DataTable dt = data.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listFab.Properties.Items.Add(dt.Rows[i]["LOCATION_NAME"]);
                    }
                }
            }

            data = _entity.GetExtendParam("班别");
            listClass.Properties.Items.Clear();
            if (data != null)
            {
                DataTable dt = data.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listClass.Properties.Items.Add(dt.Rows[i]["PARAM_VALUE"]);
                    }
                }
            }

            data = _entity.GetExtendParam("班别");
            ListRESPONSECLASS.Properties.Items.Clear();
            if (data != null)
            {
                DataTable dt = data.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListRESPONSECLASS.Properties.Items.Add(dt.Rows[i]["PARAM_VALUE"]);
                    }
                }
            }

            data = _entity.GetExtendParam("客户");
            listCustomer.Properties.Items.Clear();
            if (data != null)
            {
                DataTable dt = data.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listCustomer.Properties.Items.Add(dt.Rows[i]["PARAM_VALUE"]);
                    }
                }
            }


            data = _entity.GetAllOperations();
            listRESPONSEPOSITION.Properties.Items.Clear();
            if (data != null)
            {
                DataTable dt = data.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listRESPONSEPOSITION.Properties.Items.Add(dt.Rows[i]["ROUTE_OPERATION_NAME"]);
                    }
                }
            }

            data = _entity.GetExtendParam("责任归属");
            listATTRIBUTIONRESPONSE.Properties.Items.Clear();
            if (data != null)
            {
                DataTable dt = data.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listATTRIBUTIONRESPONSE.Properties.Items.Add(dt.Rows[i]["PARAM_VALUE"]);
                    }
                }
            }


            data = _entity.GetExtendParam("不良分类");
            lsitADVERSECLASS.Properties.Items.Clear();
            if (data != null)
            {
                DataTable dt = data.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        lsitADVERSECLASS.Properties.Items.Add(dt.Rows[i]["PARAM_VALUE"]);
                    }
                }
            }

            data = _entity.GetExtendParam("判定结果");
            listRESOULT.Properties.Items.Clear();
            if (data != null)
            {
                DataTable dt = data.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listRESOULT.Properties.Items.Add(dt.Rows[i]["PARAM_VALUE"]);
                    }
                }
            }

            data = _entity.GetExtendParam("责任线别");
            listDutyLine.Properties.Items.Clear();
            if (data != null)
            {
                DataTable dt = data.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listDutyLine.Properties.Items.Add(dt.Rows[i]["PARAM_VALUE"]);
                    }
                }
            }



        }

        private void LotExceptionProcess_Load(object sender, EventArgs e)
        {
            BindingListItem();
            txtOperater.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            txtTime.Text = DateTime.Now.ToString();
            enableControl(false);
        }

        private void txtLotNo_EditValueChanged(object sender, EventArgs e)
        {
            if (txtLotNo.Text.Trim() != "")
            {
                DataSet ds = _entity.GetLotInfo(txtLotNo.Text.Trim());
                if (ds != null && ds.Tables[0] != null)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtProductType.Text = dt.Rows[0]["PART_TYPE"].ToString();
                        txtCreateLotTime.Text = dt.Rows[0]["CREATE_TIME"].ToString();
                    }
                    else
                    {
                        txtLotNo.Text = "";
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg010}"));//序列号不存在
                        //MessageBox.Show("序列号不存在");
                    }
                }
            }
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if ((ActiveControl is LayoutControl || ActiveControl is ComboBox) &&
                keyData == Keys.Enter)
            {
                keyData = Keys.Tab;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void gvLot_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
