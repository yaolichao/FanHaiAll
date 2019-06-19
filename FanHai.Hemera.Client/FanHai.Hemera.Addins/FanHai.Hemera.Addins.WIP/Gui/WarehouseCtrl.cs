using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Addins.WIP.Report;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class WarehouseCtrl : BaseUserCtrl
    {
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:Global.SystemInfo}"); //提示
        public WarehouseCtrl()
        {
            InitializeComponent();
            InitializeLanguage();
        }


        private void InitializeLanguage()
        {
            GridViewHelper.SetGridView(gvRkInf);
            this.toolStripButton3.Text = StringParser.Parse("${res:Global.New}");// "新增";
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");// "保存";
            this.tsbEdit.Text = StringParser.Parse("${res:Global.Update}");// "修改";
            this.tsbDel.Text = StringParser.Parse("${res:Global.Delete}");// "删除";
            this.tsbSelect.Text = StringParser.Parse("${res:Global.Query}");// "查询";
            this.tsbClose.Text = StringParser.Parse("${res:Global.CloseButtonText}");// "关闭";
            this.btnAddPal.Text = StringParser.Parse("${res:Global.New}");// "新增";
            this.btnRemovePal.Text = StringParser.Parse("${res:Global.Remove}");// "移除";
            this.gcWordNum.Caption = StringParser.Parse("${res:Global.WorkNumber}");// "工单号";
            this.gcNum.Caption = StringParser.Parse("${res:Global.RowNumber}");// "序号";

            this.toolStripButton1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.toolStripButton1}");// "打印入库单";
            //this.lblApplicationTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.lblApplicationTitle}");// "生成入库单";
            this.cedLast.Properties.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.cedLast}");// "是否尾单入库";
            this.gcConsignmentKey.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcConsignmentKey}");// "包装主键";
            this.gcPartNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcPartNumber}");// "物料号";
            this.gcXP004.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcXP004}");// "托盘号";
            this.gcLgort.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcLgort}");// "库位";
            this.gcBWART.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcBWART}");// "移动类型";
            this.gcCharg.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcCharg}");// "批次号";
            this.gcMenge.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcMenge}");// "数量";
            this.gcMatnr.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcMatnr}");// "物料号";
            this.gcDec.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcDec}");// "描述";
            this.gcRemark.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcRemark}");// "备注";
            this.gcXP001.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcXP001}");// "质量等级";
            this.gcXP002.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcXP002}");// "保税手册号";
            this.gcXP003.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcXP003}");// "电流分档";
            this.gcXP005.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcXP005}");// "实际功率";
            this.gcXP006.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcXP006}");// "标称功率";
            this.gcXP007.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcXP007}");// "分档方式";
            this.gcXP011.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcXP011}");// "晶硅功率范围";
            this.gcXP008.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcXP008}");// "红外等级下线";
            this.gcXP009.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcXP009}");// "花色";
            this.gcXP010.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcXP010}");// "FF下线%";
            this.gcLineName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcLineName}");// "线别";
            this.gcOrderType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.gcOrderType}");// "工单类型";
            this.layoutControlItem10.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.layoutControlItem10}");// "托号";
            this.lciWorkPNum.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.lciWorkPNum}");// "工单料号";
            this.layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.layoutControlItem3}");// "入库单号";
            this.layoutControlItem7.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.layoutControlItem7}");// "工单号";
            this.layoutControlItem9.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.layoutControlItem9}");// "备注";
            this.layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.layoutControlItem6}");// "工厂号";
            this.layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.layoutControlItem8}");// "部门";
            this.LciRkWorker.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.LciRkWorker}");// "入库人";
            this.layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.layoutControlItem4}");// "入库单状态";
            this.txtRkDate.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.txtRkDate}");// "入库时间";
            this.layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.layoutControlItem5}");// "线别";
            this.layoutControlItem14.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.layoutControlItem14}");// "工单物料描述";

        }







        //全局变量定义
        #region
        public string work_order = string.Empty;
        public int Flag = 0;                             //0：默认  1：保存  2:查询  3:删除  4：修改
        public string _status = "Empty";                 //状态表明是新增的还是修改还是查询删除
        DataTable _dtProductGrade = null;
        public string koRkNumber = string.Empty;
        DataTable dtLinshi = new DataTable();
        string part_number = string.Empty;
        double menge = 0;
        #endregion

        //绑定数据
        #region
               /// <summary>
        /// 绑定入库人名字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bind_Rk_People()
        {
            try
            {
                string name = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
                RkWorker.Text = name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error");
            }
        }

        //方法定义
        public void BindWorkOrderChanged()
        {
            string workNumber = teOrderNo.Text.Trim();
            if (string.IsNullOrEmpty(workNumber))
            {

                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), "System error info");//工单号不能为空
                return;
            }
            else
            {
                WarehouseEngine whe = new WarehouseEngine();
                DataSet ds = whe.GetPorWorkOrderInfByWorkNo(workNumber);
                try
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        txtMat.Text = ds.Tables[0].Rows[0]["PART_NUMBER"].ToString().Trim();
                        txtDes.Text = ds.Tables[0].Rows[0]["PART_DESC"].ToString().Trim();
                        txtLineName.Text = ds.Tables[0].Rows[0]["LINE_NAME"].ToString().Trim();
                        tePALNO.Focus();
                        tePALNO.Select();
                        gcItems.DataSource = null;     //清空datasource  
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                    return;
                }
            }
        }

        public void BindWorkNumber()
        {
            teOrderNo.Properties.Items.Clear();
            WarehouseEngine whe = new WarehouseEngine();
            DataSet ds = whe.GetWorkNumber();
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    teOrderNo.Properties.Items.Add(ds.Tables[0].Rows[i]["ORDER_NUMBER"]);
                }
            }
        }

        /// <summary>
        /// 绑定工厂车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            string stores = PropertyService.Get(PROPERTY_FIELDS.STORES);
            this.cbeWerks.Properties.ReadOnly = false;
            DataTable dt = FactoryUtils.GetFactoryRoomByStores(stores);

            string[] columns = new string[] { "MESDATASOURCE", "ERPFACTORY" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "MEScontrastERP");
            DataTable dtFac = BaseData.Get(columns, category);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drF = dt.Rows[i];
                for (int j = 0; j < dtFac.Rows.Count; j++)
                {
                    DataRow drFac = dtFac.Rows[j];
                    if (drFac["MESDATASOURCE"].ToString().Trim() == drF["LOCATION_NAME"].ToString().Trim())
                    {
                        this.cbeWerks.Text = drFac["ERPFACTORY"].ToString();
                    }
                }
            }

            this.cbeWerks.Properties.ReadOnly = true;

        }

        /// <summary>
        /// 绑定部门。
        /// </summary>
        private void BindDept()
        {
            string stores = PropertyService.Get(PROPERTY_FIELDS.STORES);
            this.cbeWerks.Properties.ReadOnly = false;
            DataTable dt = FactoryUtils.GetFactoryRoomByStores(stores);

            string[] columns = new string[] { "MESDATASOURCE", "DEPT" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "MEScontrastERP");
            DataTable dtFac = BaseData.Get(columns, category);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drF = dt.Rows[i];
                for (int j = 0; j < dtFac.Rows.Count; j++)
                {
                    DataRow drFac = dtFac.Rows[j];
                    if (drFac["MESDATASOURCE"].ToString().Trim() == drF["LOCATION_NAME"].ToString().Trim())
                    {
                        this.teDept.Text = drFac["DEPT"].ToString();
                    }
                }
            }
        }

        #endregion

        //方法
        #region
        private void GetPalletInfAndInsertToList()
        {
            if (_status == "EDIT")
            {
                string pallet_no = tePALNO.Text.Trim();
                string rkNum = txtRknum.Text.Trim();
                WarehouseEngine whe = new WarehouseEngine();
                DataSet dsGetPo = whe.GetPo(rkNum,pallet_no);
                if (dsGetPo.Tables[0].Rows.Count > 0)
                {
                    //判断输入的托号是否重复？
                    DataTable dtSource = this.gcItems.DataSource as DataTable;
                    if (dtSource != null)
                    {
                        DataRow[] drs = dtSource.Select("XP004='" + pallet_no + "'");
                        for (int i = 0; i < drs.Length; i++)
                        {
                            dtSource.Rows.Remove(drs[i]);
                        }
                    }
                    GetPattetNoInfToGvlist(pallet_no);
                    return;
                }                
            }
            //抓取入库包装表中的托信息查看托是否符合刷入需求
            try
            {
                //1.1判定界面工单号输入框是否为空，为空系统提示
                //1.2不为空则判断托是否存在，不存在系统提示。存在需要审核托是否满足入库条件
                //1.3满足条件：第二个托刷入时则需要判定输入托的工单是否和第一笔托带出的工单号一致(同一个工单的托才能入库到一张入库单上)
                work_order = teOrderNo.Text.Trim();               //将工单号值赋值到全局变量上
                string paNo = tePALNO.Text.Trim();
                part_number = txtMat.Text.Trim();
                //判定界面工单号输入框和托号是否为空，为空系统提示
                if (string.IsNullOrEmpty(work_order))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg001}"), MESSAGEBOX_CAPTION);//请输入工单号！
                    //MessageService.ShowMessage("请输入工单号！", "系统提示");
                    return;
                }
                if (string.IsNullOrEmpty(paNo))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg002}"), MESSAGEBOX_CAPTION);//请输入托盘号！
                    MessageService.ShowMessage("请输入托盘号！", "系统提示");
                    return;
                }
                //判定托是否存在,且满足入库条件
                WarehouseEngine whe = new WarehouseEngine();
                DataSet ds = whe.SearchPallet(paNo);
                if (ds.Tables[0].Rows.Count < 1 || ds == null)
                {
                    MessageBox.Show("没有托:" + paNo + "的信息,请重新输入！", "系统错误提示");
                    tePALNO.Select();
                    tePALNO.SelectAll();
                    return;
                }
                DataTable dt = new DataTable();
                try
                {
                    dt = ds.Tables[0];
                    DataSet dsGetPalletWorkorder = whe.GetPalletWorkorder(paNo, work_order);

                    //判定是否为尾单
                    if (cedLast.Checked == false)
                    {//不是尾单则判定工单号和入库工单号是否匹配
                        if (dsGetPalletWorkorder.Tables[0].Rows.Count < 1)
                        {
                            MessageBox.Show("托:" + paNo + "的工单号不匹配,请重新输入符合要求的托号！", "系统错误提示");
                            tePALNO.Select();
                            tePALNO.SelectAll();
                            return;
                        }
                    }
                    else
                    {//是尾单则判定料号是否相同 相同才能进行尾单入库
                        if (dt.Rows[0]["SAP_NO"].ToString().Trim() != txtMat.Text.Trim())
                        {
                            MessageBox.Show("托:" + paNo + "的料号与该工单料号不匹配,请重新输入符合要求的托号！", "系统错误提示");
                            tePALNO.Select();
                            tePALNO.SelectAll();
                            return;
                        }
                    }

                    //判断输入的托号是否重复？
                    DataTable dtSource = this.gcItems.DataSource as DataTable;
                    if (dtSource != null)
                    {
                        DataRow[] drs = dtSource.Select("XP004='" + paNo + "'");
                        for (int i = 0; i < drs.Length; i++)
                        {
                            dtSource.Rows.Remove(drs[i]);
                        }
                    }
                    if (dt.Rows[0]["CS_DATA_GROUP"].ToString() == "0" || dt.Rows[0]["CS_DATA_GROUP"].ToString() == "1")
                    {
                        MessageBox.Show(paNo + "包装中,请重新输入托号！", "系统错误提示");
                        tePALNO.Select();
                        tePALNO.SelectAll();
                        return;
                    }
                    if (Convert.ToInt32(dt.Rows[0]["LOT_NUMBER_QTY"].ToString().Trim()) <= Convert.ToInt32(dt.Rows[0]["INWH_QTY"].ToString().Trim()))
                    {
                        MessageBox.Show(paNo + "的入库数量超过或等于托的实际数量不能入库,请重新输入托号！", "系统错误提示");
                        tePALNO.Select();
                        tePALNO.SelectAll();
                        return;
                    }

                    //满足条件，获取信息到列表中
                    GetPattetNoInfToGvlist(paNo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System error info");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "System error info");
            }
        }

        private void GetPattetNoInfToGvlist(string PalletNo)
        {
            try
            {
                WarehouseEngine whe = new WarehouseEngine();
                DataSet ds = new DataSet();
                if (cedLast.Checked == true)
                {
                    ds = whe.SearchPelletInfToList(PalletNo, "", part_number);
                }
                else
                {
                    ds = whe.SearchPelletInfToList(PalletNo, work_order,"");
                }
                DataTable dt = ds.Tables[0];
                dt.Columns.Add("ROWNUMBER");
                dt.Columns.Add("REMARK");
                dt.Columns.Add("XP008");
                //dt.Columns.Add("XP009");
                dt.Columns.Add("XP010");
                //dt.Columns.Add("XP011");
                //dt.Columns.Add("DL");
                dt.Columns.Add("BWART");



                string[] columns = new string[] { "MES_DESC", "SAP_DESC" };
                KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_XP001");
                DataTable dtXP001 = BaseData.Get(columns, category);

                for(int k = 0; k < dt.Rows.Count; k++)
                {
                    //转换XP001
                    string XP001 = Convert.ToString(dt.Rows[k]["XP001"]);
                    string tXP001 = GetProductGradeDisplayText(XP001);

                    for (int j = 0; j < dtXP001.Rows.Count; j++)
                    {
                        DataRow drXP001 = dtXP001.Rows[j];
                        if (drXP001["MES_DESC"].ToString().Equals(tXP001))
                        {
                            dt.Rows[k]["XP001"] = drXP001["SAP_DESC"].ToString();
                            break;
                        }
                    }
                    DataTable dtBWART=whe.GetBwart(dt.Rows[k]["MATNR"].ToString()).Tables["BWART"];
                    dt.Rows[k]["BWART"] = dtBWART.Rows[0]["BWART"].ToString();
                    //dt.Rows[k]["DL"] = "否";
                    DataRow dr = dt.Rows[k];
                    string lgorts = Convert.ToString(dr["LGORT"]);
                    if (!string.IsNullOrEmpty(lgorts))
                    {
                        string[] arrTemp = lgorts.Split(',');                           ///数组取值 库位
                        string lgort = arrTemp[0];
                        dr["LGORT"] = lgort;
                    }
                }
                DataTable dtSource = gcItems.DataSource as DataTable;
                if (dtSource == null)
                {
                    dtSource = dt;
                }
                else
                {
                    dtSource.Merge(dt,true,MissingSchemaAction.Ignore);
                }
                for (int i = 0; i < dtSource.Rows.Count ; i++)
                {
                    dtSource.Rows[i]["ROWNUMBER"] = i + 1;
                }
                gcItems.DataSource = dtSource;
                if(cedLast.Checked == true)
                {
                    menge = Convert.ToDouble(dtSource.Rows[0]["MENGE"].ToString().Trim());
                }
                tePALNO.Select();
                tePALNO.SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System error info");
                return;
            }
            
        }

        /// <summary>
        /// 获取产品等级的显示值。
        /// </summary>
        /// <returns>产品等级的显示值</returns>
        private string GetProductGradeDisplayText(string value)
        {
            string displayText = value;
            try
            {
                if (this._dtProductGrade == null)
                {
                    string[] columns = new string[] { "Column_Name", "Column_code" };
                    KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_TestRule_PowerSet");
                    List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();
                    whereCondition.Add(new KeyValuePair<string, string>("Column_type", "ProductGrade"));
                    this._dtProductGrade = BaseData.GetBasicDataByCondition(columns, category, whereCondition);
                }
                if (null != this._dtProductGrade)
                {
                    DataRow[] drs = this._dtProductGrade.Select(string.Format("Column_code='{0}'", value));
                    if (drs.Length > 0)
                    {
                        displayText = Convert.ToString(drs[0]["Column_Name"]);
                    }
                }
            }
            catch
            {
                displayText = value;
            }
            return displayText;
        }

        private void FormStatusBySelectRkNum(string rkNum)
        {
            WarehouseEngine whe = new WarehouseEngine();
            DataSet dsKoPo = whe.GetKoPoByRkNumber(rkNum);
            txtRknum.Text = dsKoPo.Tables["WIP_RK_KO"].Rows[0]["ZMBLNR"].ToString().Trim();
            cbeWerks.Text = dsKoPo.Tables["WIP_RK_KO"].Rows[0]["WERKS"].ToString().Trim();
            teOrderNo.Text = dsKoPo.Tables["WIP_RK_KO"].Rows[0]["AUFNR"].ToString().Trim();
            teDept.Text = dsKoPo.Tables["WIP_RK_KO"].Rows[0]["DEPT"].ToString().Trim();
            RkWorker.Text = dsKoPo.Tables["WIP_RK_KO"].Rows[0]["CREATOR"].ToString().Trim();
            txtMat.Text = dsKoPo.Tables["WIP_RK_KO"].Rows[0]["WORK_ORDER_MATNR"].ToString().Trim();
            txtDes.Text = dsKoPo.Tables["WIP_RK_KO"].Rows[0]["MAKTX"].ToString().Trim();
            cbeMemo.Text = dsKoPo.Tables["WIP_RK_KO"].Rows[0]["MEMO1"].ToString().Trim();
            txtLineName.Text = dsKoPo.Tables["WIP_RK_KO"].Rows[0]["LINE"].ToString().Trim();
            txtRkDateTime.EditValue = dsKoPo.Tables["WIP_RK_KO"].Rows[0]["CDATE"].ToString().Trim();
            string status = dsKoPo.Tables["WIP_RK_KO"].Rows[0]["RKSTATUS"].ToString().Trim();
            RkStastusChange(status);
            

            gcItems.DataSource = dsKoPo.Tables["WIP_RK_PO"];
        }

        private void Status(int Flag)
        {
            if (Flag == 0)  //新增
            {
                txtRknum.Text = "";
                txtMat.Text = "";
                txtStatus.Text = "";
                cbeMemo.Text = "";
                tePALNO.Text = "";
                txtLineName.Text = "";
                txtDes.Text = "";
                gcItems.DataSource = null;
                //保存，修改，删除
                tsbSave.Enabled = true;
                tsbEdit.Enabled = false;
                tsbDel.Enabled = false;
                //输入按键可用
                cbeMemo.Enabled = true;
                teDept.Enabled = true;
                tePALNO.Enabled = true;
                //增加、移除按钮可用
                btnAddPal.Enabled = true;
                btnRemovePal.Enabled = true;

                cedLast.Enabled = true;
                teOrderNo.Enabled = true;
                _status = "NEW";

                gvRkInf.OptionsBehavior.ReadOnly = false;
                

            }
            if (Flag == 4)     //修改
            {
                //保存，修改，删除
                tsbSave.Enabled = true;
                tsbEdit.Enabled = false;
                tsbDel.Enabled = false;
                //输入按键可用
                cbeMemo.Enabled = false;
                teDept.Enabled = false;
                tePALNO.Enabled = true;
                //增加、移除按钮可用
                btnAddPal.Enabled = true;
                btnRemovePal.Enabled = true;

                cedLast.Enabled = false;

                _status = "EDIT";
                gvRkInf.OptionsBehavior.ReadOnly = false;
 
            }
            if (Flag == 2)    //查询
            {
                //禁用保存按钮
                tsbSave.Enabled = false;
                tsbDel.Enabled = true;
                tsbEdit.Enabled = true;

                //禁用输入按键
                cbeMemo.Enabled = false;
                teDept.Enabled = false;
                tePALNO.Enabled = false;

                //禁用增加、移除行
                btnAddPal.Enabled = false;
                btnRemovePal.Enabled = false;

                cedLast.Enabled = false;

                _status = "SELECT";
                gvRkInf.OptionsBehavior.ReadOnly = true;

                
            } 
            if (Flag == 3)   //删除
            {
                //禁用保存按钮
                tsbSave.Enabled = false;
                tsbEdit.Enabled = false;
                tsbDel.Enabled = false;
                toolStripButton3.Enabled = true;
                //禁用输入按键
                cbeMemo.Enabled = false;
                teDept.Enabled = false;
                tePALNO.Enabled = false;

                //禁用增加、移除行
                btnAddPal.Enabled = false;
                btnRemovePal.Enabled = false;
                cedLast.Enabled = false;

                _status = "DELETE";
                gvRkInf.OptionsBehavior.ReadOnly = true;
            }

        }

        private void RkStastusChange(string status)
        {
            if (status == "")
            {
                txtStatus.Text = "已创建";
            }
            if (status == "W")
            {
                txtStatus.Text = "未审批";
            }
            if (status == "A")
            {
                txtStatus.Text = "审批通过";
            }
            if (status == "R")
            {
                txtStatus.Text = "拒绝";
            }
            if (status == "T")
            {
                txtStatus.Text = "已过账";
            }
            if (status == "D")
            {
                txtStatus.Text = "已删除";
            }
        }
        #endregion


        //事件
        #region
        //关闭
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        
        //界面载入
        private void WarehouseCtrl_Load(object sender, EventArgs e)
        {
            tePALNO.Focus();
            tePALNO.Select();
            Bind_Rk_People();
            txtRkDateTime.EditValue = DateTime.Now;
            BindFactoryRoom();
            BindDept();
            BindWorkNumber();
            Status(0);
        }
       
        //托回车事件
        private void tePALNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                //调用方法判定托信息是否符合标准，符合载入到列表中，不符合提示不符合原因
                GetPalletInfAndInsertToList();
            }
        }
        
        //托号新增按钮事件
        private void btnAddPal_Click(object sender, EventArgs e)
        {
            //调用方法判定托信息是否符合标准，符合载入到列表中，不符合提示不符合原因
            GetPalletInfAndInsertToList();
        }
        
        //移除行数据
        private void btnRemovePal_Click(object sender, EventArgs e)
        {

            if (this.gvRkInf.GetFocusedRow() != null)
            {
                this.gvRkInf.DeleteRow(this.gvRkInf.FocusedRowHandle);
                ((DataView)gvRkInf.DataSource).Table.AcceptChanges();
                DataTable dt = ((DataView)gvRkInf.DataSource).Table;
                if (dt != null)
                {
                    if (!dt.Columns.Contains("ROWNUMBER"))
                        dt.Columns.Add("ROWNUMBER");

                    for (int i = 1; i < dt.Rows.Count + 1; i++)
                        dt.Rows[i - 1]["ROWNUMBER"] = i.ToString();
                    gcItems.Refresh();
                    gcItems.DataSource = dt;
                }
                else
                {
                    gcItems.DataSource = null;     //清空datasource  
                    teOrderNo.Focus();
                    teOrderNo.Select();
                    cbeMemo.Text = "";
                }

            }
            else
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotFutureHold.Msg001}"), "${res:Global.SystemInfo}");//"必须选择至少选择一条记录"
            }

        }
        
        //新增
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Status(0);
        }
        
        //保存
        private void tsbSave_Click_1(object sender, EventArgs e)
        {
            if (this.gvRkInf.State == GridState.Editing && this.gvRkInf.IsEditorFocused && this.gvRkInf.EditingValueModified)
            {
                this.gvRkInf.SetFocusedRowCellValue(this.gvRkInf.FocusedColumn, this.gvRkInf.EditingValue);
            }
            try
            {
                ((DataView)gvRkInf.DataSource).Table.AcceptChanges();
            }
            catch (Exception ex)
            { }

            Flag = 1;
            if (this.gvRkInf.State == GridState.Editing && this.gvRkInf.IsEditorFocused && this.gvRkInf.EditingValueModified)
            {
                this.gvRkInf.SetFocusedRowCellValue(this.gvRkInf.FocusedColumn, this.gvRkInf.EditingValue);
            }
            this.gvRkInf.UpdateCurrentRow();
            if (Flag == 1)
            {
                if (gcItems.DataSource == null)
                {
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg003}"), MESSAGEBOX_CAPTION);//入库信息为空,不能入库,请刷入托重新入库！
                    //MessageBox.Show("入库信息为空,不能入库,请刷入托重新入库！", "系统错误提示");
                    tePALNO.Select();
                    tePALNO.SelectAll();
                    return;
                }
                if (string.IsNullOrEmpty(cbeWerks.Text.ToString().Trim()))
                {
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg004}"), MESSAGEBOX_CAPTION);//工厂信息不能为空！
                    //MessageBox.Show("工厂信息不能为空！", "系统错误提示");
                    tePALNO.Select();
                    tePALNO.SelectAll();
                    return;
                }
                if (string.IsNullOrEmpty(teOrderNo.Text.ToString().Trim()))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SAP.Msg001}"), MESSAGEBOX_CAPTION);//工单号不能为空
                    //MessageBox.Show("工单号不能为空！", "系统错误提示");
                    tePALNO.Select();
                    tePALNO.SelectAll();
                    return;
                }
                if (string.IsNullOrEmpty(txtMat.Text.ToString().Trim()))
                {
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg005}"), MESSAGEBOX_CAPTION);//工单料号信息不能为空！
                    //MessageBox.Show("工单料号信息不能为空！", "系统错误提示");
                    tePALNO.Select();
                    tePALNO.SelectAll();
                    return;
                }
                if (string.IsNullOrEmpty(txtLineName.Text.ToString().Trim()))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg017}"), MESSAGEBOX_CAPTION);//线别不能为空
                    //MessageBox.Show("线别不能为空！", "系统错误提示");
                    tePALNO.Select();
                    tePALNO.SelectAll();
                    return;
                }
                try
                {
                    if (MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg006}"),
                       StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK) //是否保存入库单信息？
                    {
                        if (_status == "NEW")
                        {
                            //数据绑定
                            DataTable dtInKo = new DataTable();
                            dtInKo.Columns.Add("WERKS");
                            dtInKo.Columns.Add("WORK_ORDER");
                            dtInKo.Columns.Add("DEPT");
                            dtInKo.Columns.Add("CREATOR");
                            dtInKo.Columns.Add("WORK_ORDER_MATNR");
                            dtInKo.Columns.Add("MAKTX");
                            dtInKo.Columns.Add("MEM01");
                            dtInKo.Columns.Add("LINE");
                            DataRow dr = dtInKo.NewRow();
                            dr["WERKS"] = cbeWerks.Text.Trim();
                            dr["WORK_ORDER"] = teOrderNo.Text.Trim();
                            dr["DEPT"] = teDept.Text.Trim();
                            dr["CREATOR"] = RkWorker.Text.Trim();
                            dr["WORK_ORDER_MATNR"] = txtMat.Text.Trim();
                            dr["MAKTX"] = txtDes.Text.Trim();
                            dr["MEM01"] = cbeMemo.Text.Trim();
                            dr["LINE"] = txtLineName.Text.Trim();
                            dtInKo.Rows.Add(dr);
                            dtInKo.TableName = "WIP_RK_KO";

                            DataTable dtInPo = new DataTable();
                            DataView dv = gvRkInf.DataSource as DataView;
                            if (dv != null) dtInPo = dv.Table;
                            dtInPo.TableName = "WIP_RK_PO";

                            DataSet dsIn = new DataSet();
                            dsIn.Merge(dtInKo);
                            dsIn.Merge(dtInPo);

                            WarehouseEngine whe = new WarehouseEngine();
                            DataSet dsReturn = whe.CreateRkKoPo(dsIn);
                            if (dsReturn.Tables["OUTPUT_PARAM_TABLE"].Rows[0]["CODE"].ToString() == "0")
                            {
                                if (dsReturn.Tables["RETURN"].Rows[0]["NUMBER"].ToString() == "0")
                                {
                                    string ZMBLNR = dsReturn.Tables["RETURN_ZMBLNR"].Rows[0]["ZMBLNR"].ToString().Trim();
                                    FormStatusBySelectRkNum(ZMBLNR);
                                    MessageBox.Show("保存成功!入库单号为:" + ZMBLNR, "系统错误提示");
                                    Status(2);
                                }
                                else
                                {
                                    MessageBox.Show("SAP信息反馈:SAP中"+dsReturn.Tables["RETURN"].Rows[0]["MESSAGE"].ToString()+" 请联系相关负责人", "系统错误提示");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show(dsReturn.Tables["OUTPUT_PARAM_TABLE"].Rows[0]["MESSAGE"].ToString().Trim(), "系统错误提示");
                                return;
                            }
                        }
                        else if(_status == "EDIT")
                        {
                            if (gvRkInf.DataSource == null)
                            {
                                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg007}"), MESSAGEBOX_CAPTION);//没有要保存的信息，请刷入托信息进行入库。
                                //MessageBox.Show("没有要保存的信息，请刷入托信息进行入库。", "系统错误提示");
                                return;
                            }
                            if (string.IsNullOrEmpty(txtRknum.Text.Trim()))
                            {
                                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg008}"), MESSAGEBOX_CAPTION);//入库单号不能为空!
                                //MessageBox.Show("入库单号不能为空！", "系统错误提示");
                                return;
                            }
                            ///////////////////////////////////////////////////////////////////////////////
                            //判定单据在mes修改后sap中是否有做修改
                            WarehouseEngine whe01 = new WarehouseEngine();
                            string rkNum = txtRknum.Text.Trim();
                            string werks = cbeWerks.Text.Trim();
                            DataSet ds01 = whe01.RfcToDeleteRk(rkNum, werks);
                            try
                            {
                                if (ds01.Tables["STATUS"].Rows[0]["E_SUBRC"].ToString() == "0")
                                {
                                    string status = ds01.Tables["STATUS"].Rows[0]["E_STAT"].ToString().Trim();
                                    RkStastusChange(status);
                                }
                                if (txtStatus.Text.Trim() == "已过账" || txtStatus.Text.Trim() == "已删除")
                                {
                                    MessageBox.Show("入库单:" + txtRknum.Text.Trim() + " 已经删除或过账不能再修改！", "系统错误提示");
                                    Status(3);
                                    return;
                                }
 
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "System error info");
                                return;
                            }
                            ///////////////////////////////////////////////////////////////////////////////
                            //数据绑定
                            DataTable dtInKo = new DataTable();
                            dtInKo.Columns.Add("ZMBLNR");
                            dtInKo.Columns.Add("WERKS");
                            dtInKo.Columns.Add("WORK_ORDER");
                            dtInKo.Columns.Add("DEPT");
                            dtInKo.Columns.Add("CREATOR");
                            dtInKo.Columns.Add("WORK_ORDER_MATNR");
                            dtInKo.Columns.Add("MAKTX");
                            dtInKo.Columns.Add("MEM01");
                            dtInKo.Columns.Add("LINE");
                            DataRow dr = dtInKo.NewRow();
                            dr["ZMBLNR"] = txtRknum.Text.Trim();
                            dr["WERKS"] = cbeWerks.Text.Trim();
                            dr["WORK_ORDER"] = teOrderNo.Text.Trim();
                            dr["DEPT"] = teDept.Text.Trim();
                            dr["CREATOR"] = RkWorker.Text.Trim();
                            dr["WORK_ORDER_MATNR"] = txtMat.Text.Trim();
                            dr["MAKTX"] = txtDes.Text.Trim();
                            dr["MEM01"] = cbeMemo.Text.Trim();
                            dr["LINE"] = txtLineName.Text.Trim();
                            dtInKo.Rows.Add(dr);
                            dtInKo.TableName = "WIP_RK_KO";

                            DataTable dtInPo = new DataTable();
                            DataView dv = gvRkInf.DataSource as DataView;
                            if (dv != null) dtInPo = dv.Table;
                            dtInPo.TableName = "WIP_RK_PO";

                            DataSet dsIn = new DataSet();
                            dsIn.Merge(dtInKo);
                            dsIn.Merge(dtInPo);

                            WarehouseEngine whe = new WarehouseEngine();
                            DataSet dsReturn = whe.EditRkKoPo(dsIn);
                            try
                            {
                                if (dsReturn.Tables["OUTPUT_PARAM_TABLE"].Rows[0]["CODE"].ToString() == "0")
                                {
                                    if (dsReturn.Tables["RETURN"].Rows[0]["NUMBER"].ToString() == "0")
                                    {
                                        string ZMBLNR = dsReturn.Tables["RETURN_ZMBLNR"].Rows[0]["ZMBLNR"].ToString().Trim();
                                        FormStatusBySelectRkNum(ZMBLNR);
                                        MessageBox.Show("修改入库单:" + ZMBLNR + " 成功！", "系统错误提示");
                                        Status(2);
                                    }
                                    else
                                    {
                                        MessageBox.Show("SAP信息反馈:SAP中" + dsReturn.Tables["RETURN"].Rows[0]["MESSAGE"].ToString() + " 请联系相关负责人", "系统错误提示");
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg009}"), MESSAGEBOX_CAPTION);//修改失败,入库单信息不变！
                                    //MessageBox.Show("修改失败,入库单信息不变！", "系统错误提示");
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "system error info");
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "system error info");
                    Flag = 0;
                    return;
                }
            }
            Flag = 0;
        }
        

        private void tsbSelect_Click(object sender, EventArgs e)
        {
            Flag = 2;
            WarehouseSelectForm form = new WarehouseSelectForm();
            if (Flag == 2)
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Status(Flag);
                    
                    string rknum = form.rknumber;
                    if (!string.IsNullOrEmpty(rknum))
                    {
                        FormStatusBySelectRkNum(rknum);
                    }
                    else
                    {
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg010}"), MESSAGEBOX_CAPTION);//返回入库单为空,不存在,请重新查询！
                        //MessageBox.Show("返回入库单为空,不存在,请重新查询！", "系统错误提示");
                        return;
                    }
                }


            }
            Flag = 0;
        }


        private void tsbDel_Click(object sender, EventArgs e)
        {
            if(txtRknum.Text.Trim() == "")
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg011}"), MESSAGEBOX_CAPTION);//入库单号为空,不能删除！
                //MessageBox.Show("入库单号为空,不能删除！", "系统错误提示");
                return;
            }
            if (cbeWerks.Text.Trim() == "")
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg012}"), MESSAGEBOX_CAPTION);//工厂号为空,不能删除！
                //MessageBox.Show("工厂号为空,不能删除！", "系统错误提示");
                return;
            }


            if (MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg013}"),
                      StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)//是否确认要删除入库单信息？
            {
                //调用rfc查看入库单状态，如果和sap状态不一样那么修改mes入库单状态和sap保持一致,然后判定状态是否可以被删除
                WarehouseEngine whe = new WarehouseEngine();
                string rkNum = txtRknum.Text.Trim();
                string werks = cbeWerks.Text.Trim();
                DataSet ds = whe.RfcToDeleteRk(rkNum, werks);
                try
                {
                    if (ds.Tables["STATUS"].Rows[0]["E_SUBRC"].ToString() == "0")
                    {
                        if (ds.Tables["STATUS"].Rows[0]["E_STAT"].ToString().Trim() == "D")
                        {
                            string status = ds.Tables["STATUS"].Rows[0]["E_STAT"].ToString().Trim();
                            RkStastusChange(status);

                            MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg014}"), MESSAGEBOX_CAPTION);//入库单删除成功！
                            //MessageBox.Show("入库单删除成功！", "系统错误提示");
                            Status(3);
                            return;

                        }
                        else
                        {
                            string status = ds.Tables["STATUS"].Rows[0]["E_STAT"].ToString().Trim();
                            RkStastusChange(status);
                            MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg015}"), MESSAGEBOX_CAPTION);//入库单在SAP中未删除或入库单状态有误！
                            //MessageBox.Show("入库单在SAP中未删除或入库单状态有误！", "系统错误提示");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("删除失败,原因为：" + ds.Tables["STATUS"].Rows[0]["E_MSG"].ToString(), "系统错误提示");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "系统错误提示");
                    return;
                }
            }
        }
        
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (txtRknum.Text.Trim() == "")
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg016}"), MESSAGEBOX_CAPTION);//入库单为空,请选择入库单进行打印！
                //MessageBox.Show("入库单为空,请选择入库单进行打印！", "系统错误提示");
                return;
            }
            if (txtStatus.Text.Trim() != "已删除")
            {
                WareHouseDataSet ds = new WareHouseDataSet();
                koRkNumber = txtRknum.Text.Trim();
                WarehouseEngine whe = new WarehouseEngine();
                DataSet dsKoPo = whe.GetKoPoByRkNumberForPrint(koRkNumber);

                DataTable dtKo = new DataTable();
                dtKo = dsKoPo.Tables["WIP_RK_KO"];
                DataTable dtPo = new DataTable();
                dtPo = dsKoPo.Tables["WIP_RK_PO"];

                koRkNumber = dtKo.Rows[0]["ZMBLNR"].ToString();
                if (dtKo.Rows.Count > 0)
                {
                    string _title = string.Empty;
                    _title = "山东泛海阳光能源有限公司";
                    DataRow dataRow = dtKo.Rows[0];
                    ds.ExtendedProperties.Add("Report", _title);
                    ds.ExtendedProperties.Add("Report_Title", "晶硅组件入库单");
                    ds.ExtendedProperties.Add("Report_InboundInfo", dataRow["DEPT"].ToString()); //入库部门
                    ds.ExtendedProperties.Add("Report_InwarehouseNo", dataRow["ZMBLNR"].ToString());       //入库单号
                    ds.ExtendedProperties.Add("Report_OrderNumber", dataRow["AUFNR"].ToString());  //生产工单号
                    ds.ExtendedProperties.Add("Report_InwarehouseDate", Convert.ToDateTime(dataRow["CDATE"].ToString()).ToString("yyyy.MM.dd")); //入库日期

                }

                foreach (DataRow dataRow in dtPo.Rows)
                {
                    string matnr = dataRow["MATNR"].ToString();
                    string partDesc = dataRow["PART_DESC"].ToString();
                    string charg = dataRow["CHARG"].ToString();
                    string XP006 = dataRow["XP006"].ToString();
                    string XP001 = dataRow["XP001"].ToString();
                    string bonded = dataRow["XP002"].ToString();
                    string XP002 = whe.GetBonded(dataRow["XP002"].ToString());
                    string XP003 = dataRow["XP003"].ToString();
                    string XP004 = dataRow["XP004"].ToString();
                    string XP005 = dataRow["XP005"].ToString();
                    string JUNCTION_BOX = dataRow["JUNCTION_BOX"].ToString();
                    string XP007 = dataRow["XP007"].ToString();
                    string XP009 = dataRow["XP009"].ToString();
                    string XP011 = dataRow["XP011"].ToString();
                    int menge = Convert.ToInt32(dataRow["MENGE"] == DBNull.Value ? 0 : dataRow["MENGE"]);
                    ds.WIPRKPO.Rows.Add(new object[] { matnr, partDesc, charg, XP006, XP001, XP002, XP003, XP004, XP005, JUNCTION_BOX, XP007, menge, XP009, XP011 });
                }
                WarehousePrintDialog formDialg = new WarehousePrintDialog("入库单");
                {
                    formDialg.ReportName = "FanHai.Hemera.Addins.WIP.Report.WareHouse.rdlc";
                    formDialg.ReportData = ds;
                    formDialg.ShowDialog();
                    formDialg.ReportName = null;
                    formDialg.ReportData = null;
                    formDialg.Dispose();
                }
            }
            else
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.WarehouseCtrl.Msg017}"), MESSAGEBOX_CAPTION);//入库单已经删除不能进行打印！
                //MessageBox.Show("入库单已经删除不能进行打印！", "系统错误提示");
                return;
            }

        }
        #endregion

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            WarehouseEngine whe = new WarehouseEngine();
            string rkNum = txtRknum.Text.Trim();
            string werks = cbeWerks.Text.Trim();
            DataSet ds = whe.RfcToDeleteRk(rkNum, werks);
            try
            {
                if (ds.Tables["STATUS"].Rows[0]["E_SUBRC"].ToString() == "0")
                {
                    string status = ds.Tables["STATUS"].Rows[0]["E_STAT"].ToString().Trim();
                    RkStastusChange(status);
                }

                if (txtStatus.Text.Trim() == "已过账" || txtStatus.Text.Trim() == "已删除")
                {
                    MessageBox.Show("入库单:" + txtRknum.Text.Trim() + " 已经删除或过账不能再修改！", "系统错误提示");
                    Status(3);
                    return;
                }
                else
                {
                    Status(4);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System error info");
                return;
            }
        }

        private void teOrderNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                BindWorkOrderChanged();
            }
        }

        private void teOrderNo_EditValueChanged(object sender, EventArgs e)
        {
            BindWorkOrderChanged();
        }

        /// <summary>
        /// 判定是否尾单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cedLast_CheckedChanged(object sender, EventArgs e)
        {
            if (cedLast.Checked == true)
            {//且数据可以手动输入
                //this.gvRkInf.Columns["MENGE"].OptionsColumn.ReadOnly = false;
                gcItems.DataSource = null;
                tePALNO.Select();
                tePALNO.SelectAll();
            }
            else
            {//数量一列不可输入为只读
                //this.gvRkInf.Columns["MENGE"].OptionsColumn.ReadOnly = true;
                gcItems.DataSource = null;
                tePALNO.Select();
                tePALNO.SelectAll();
            }

        }


        private void gvRkInf_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //if (cedLast.Checked == true)
            //{
            //    if (e.Column.FieldName == "MENGE")
            //    {
            //        DataRow handle = gvRkInf.GetDataRow(e.RowHandle);
            //        string palletNo = handle["XP004"].ToString();
            //        //string l = Convert.ToString(e.Value);
            //        //if(!string.IsNullOrEmpty(l))
            //        //{
            //        //    menge = Convert.ToDouble(l);
            //        WarehouseEngine whe = new WarehouseEngine();
            //        DataSet ds = whe.SearchPelletInfToList(palletNo, "", part_number);
            //        DataTable dt = ds.Tables[0];
            //        DataRow[] drs01 = dt.Select(string.Format(@"XP004 ='{0}' AND MATNR='{1}'", palletNo, part_number));
            //        menge = Convert.ToInt32(drs01[0]["MENGE"]);

            //        //}
            //    }
            //}
        }

        private void gvRkInf_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //if (cedLast.Checked == true)
            //{
            //    ((DataView)gvRkInf.DataSource).Table.AcceptChanges();
            //    DevExpress.Utils.AppearanceDefault appRed = new DevExpress.Utils.AppearanceDefault
            //                           (Color.Black, Color.Red, Color.Empty, Color.SeaShell, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);

            //    double mengeValue = Convert.ToDouble(gvRkInf.GetRowCellValue(e.RowHandle, "MENGE").ToString().Trim());

            //    if (e.Column.FieldName == "MENGE")
            //    {
            //        if (mengeValue > menge || mengeValue < 0 || string.IsNullOrEmpty(mengeValue.ToString()))
            //            DevExpress.Utils.AppearanceHelper.Apply(e.Appearance, appRed);
            //    }
            //}
            
        }

        private void teDept_EditValueChanged(object sender, EventArgs e)
        {

        }
        //添加行筛选
        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }                                            
}
