// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter Zhang          2012-03-19             新建 
// qym                  2012-03-26             新建，编写 
// =================================================================================
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
using DevExpress.XtraGrid.Views.Grid;


namespace FanHai.Hemera.Addins.SAP
{    
    // 表示工单退料的窗体类
    public partial class WorkOrderMaterialReturnCtrl : BaseUserCtrl
    {
        //构造函数
        public WorkOrderMaterialReturnCtrl()
        {
            InitializeComponent();

        }

        //画面加载的时候需要初始化的
        private void WorkOrderMaterialReturnCtrl_Load(object sender, EventArgs e)
        {
            OnAfterStateChanged(ControlState.Empty);

            //1、获取系统中设置的班次值，绑定班别数据到控件。
            //1-1、从CRM_ATTRIBUTE a,BASE_ATTRIBUTE b,BASE_ATTRIBUTE_CATEGORY c数据表中查询c.CATEGORY_NAME=Basic_Shift的数据。显示a.NAME=CODE对应的属性值。
            string[] columns = new string[] { "CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Shift");
            this.lueShiftName.Properties.DataSource = BaseData.Get(columns, category);
            this.lueShiftName.Properties.DisplayMember = "CODE";
            lueShiftName.Properties.ValueMember= "CODE";

            //1-2、根据当前时间从cal_schedule_day获取当前班次值设为其默认值，不能修改。
            BindingCurrentShift();
            
            //2、根据登录用户自动填充员工号，不能修改。
            this.txtOperator.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            this.txtOperator.Enabled = false;
            
            //3、退料日期默认值设置为当前日期。
            deReturnMatDate.DateTime = DateTime.Now;

            txtMaterialLot.Enabled = false; //没有点新增，不能刷Lot
            txtReturnMatReason.Enabled = false;
        }

        //捞起当前时间的班别,并且绑定数据
        public void BindingCurrentShift()
        {
             DataTable dt = new DataTable();
             //调用实体的方法GetCurrentShift
            WOMaterialReturnEntity wOMaterialReturnEntity = new WOMaterialReturnEntity();
            dt = wOMaterialReturnEntity.GetCurrentShift().Tables[0];
            try
            {               
                lueShiftName.EditValue = dt.Rows[0]["SHIFT"].ToString().Trim();
                lueShiftName.Enabled = false;
            }
            finally
            {
            }
        }

        //判断状态
        private void OnAfterStateChanged(ControlState controlState)
        {
            switch (controlState)
            {

                case ControlState.New:

                    tsbNew.Enabled = true;
                    tsbSave.Enabled = true;
                    tsbDelete.Enabled = false;
                    tsbClose.Enabled = true;
                    tsbReturnMatList.Enabled = true; //退料清单按钮

                    txtReturnMatReason.Enabled = true; //新增启用原因和刷批号
                    txtMaterialLot.Enabled = true;

                    txtReturnMatReason.Text = "";//新增情况单号和原因
                    txtReturnNo.Text = "";
                    break;
                case ControlState.Delete:
                    break;
                case ControlState.Empty:
                    tsbNew.Enabled = true;
                    tsbSave.Enabled = false;
                    tsbDelete.Enabled = false;
                    tsbClose.Enabled = true;
                    tsbReturnMatList.Enabled = true;
                    break;
                case ControlState.ReadOnly:
                    break;
                case ControlState.Read:
                    break;
                case ControlState.Edit:
                    break;
            }
        }

        //新增按钮Click事件方法
        private void tsbNew_Click(object sender, EventArgs e)
        {
            //1、根据当前时间从cal_schedule_day获取当前班次值设为其默认值，不能修改。
            BindingCurrentShift();

            //2、根据登录用户自动填充员工号，不能修改。
            this.txtOperator.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            txtOperator.Enabled = false;

            //3、退料日期默认值设置为当前日期。
            deReturnMatDate.DateTime = DateTime.Now;

            //4、清空物料明细列表。
            gcMaterialList.DataSource = null;
            gcMaterialList.RefreshDataSource();
            

            //5、启用保存，移除按钮。设置退料日期，退库数量为可写。
            //新增状态
            State = ControlState.New;
            OnAfterStateChanged(State);

            //设置退料日期，退库数量为可写    
            gcReturnQty.OptionsColumn.AllowEdit = true ;//列可编辑

            txtMaterialLot.Enabled = true; //新增以后Lot可以用了
            txtReturnMatReason.Enabled = true;
        }
     
        //物料批号KeyPress事件方法
        private void txtMaterialLot_KeyPress(object sender, KeyPressEventArgs e)
        {
            //1、如果不是回车按钮，退出方法执行。
            if (e.KeyChar != 13)
            {
                return;
            }
            
            //2、如果是回车按钮，根据输入的物料批号从WST_STORE_MATERIAL_DETAIL，WST_STORE_MATERIAL获取物品批号对应的物料信息。
            if (txtMaterialLot.Text.Trim().Length==0)
            {
                MessageBox.Show("输入的物料批号为空,请重新输入");
                return ;
            }
            //调用实体的方法GetMatLotInfo
            WOMaterialReturnEntity wOMaterialReturnEntity = new WOMaterialReturnEntity();
            DataSet dsMatLotInfo = wOMaterialReturnEntity.GetMatLotInfo(txtMaterialLot.Text.Trim());              
            //判断查询结果有数据吗
            if (dsMatLotInfo.Tables[0].Rows.Count < 1)
            {
                MessageBox.Show("这个LOT不存在");
                return;
            }

            //3、判断物料是否存在，如果不存在，给出提示，“物料批号对应的物料不存在。
            for (int i = 0; i < dsMatLotInfo.Tables[0].Rows.Count; i++)
            {
                if (dsMatLotInfo.Tables[0].Rows[i]["MATCODE"].ToString().Length == 0)
                {
                    MessageBox.Show("物料批号对应的物料编码（料号）不存在");
                    return;
                }
            }

            //4、判断物料所在线上仓是否是登录用户拥有权限的线上仓。如果不是，给出提示“您没有操作物料所在线上仓的权限，不能进行退料。”，退出方法执行。
            string strStore = PropertyService.Get(PROPERTY_FIELDS.STORES);//得到了用户所有的线上仓
            bool blTmpFlg=false;
            if (strStore.Length <= 0)
            {
                blTmpFlg=false;
            }
            else
            {
                blTmpFlg=true;
            }
            Dictionary<int, bool> blTmpFlg2 = new Dictionary<int, bool>();
            if (blTmpFlg == true)
            {
                string[] strArrStore = strStore.Split(',');
                for (int j = 0; j < dsMatLotInfo.Tables[0].Rows.Count; j++)
                {
                    blTmpFlg2.Add(j, false);
                    for (int i = 0; i < strArrStore.Length; i++)
                    {
                        if (strArrStore[i] == dsMatLotInfo.Tables[0].Rows[j]["STORE"].ToString())
                        {                            
                            blTmpFlg2.Remove(j);
                            blTmpFlg2.Add(j, true);
                            break;
                        }
                    }
                }
            }

            if (blTmpFlg == false || blTmpFlg2.ContainsValue(false))
            {
                MessageBox.Show("您没有操作物料所在线上仓的权限，不能进行退料。");
                return;
            }

            //5、判断物料所在工序是否是登录用户拥有权限的工序，如果不是，给出提示“您没有操作物料所在工序的权限，不能进行退料。”，退出方法执行。
            string stroperation = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            blTmpFlg = false;
            blTmpFlg2.Clear();
            if (stroperation.Length <= 0)
            {
                blTmpFlg = false;
            }
            else
            {
                blTmpFlg = true;
            }
            if (blTmpFlg == true)
            {
                string[] strarroperation = stroperation.Split(',');
                for (int j = 0; j < dsMatLotInfo.Tables[0].Rows.Count; j++)
                {
                    blTmpFlg2.Add(j, false);
                    for (int i = 0; i < strarroperation.Length; i++)
                    {
                        if (strarroperation[i] == dsMatLotInfo.Tables[0].Rows[j]["OPERATION"].ToString())
                        {
                            blTmpFlg2.Remove(j);
                            blTmpFlg2.Add(j, true);
                            break;
                        }
                    }
                }
            }

            if (blTmpFlg == false || blTmpFlg2.ContainsValue(false))
            {
                MessageBox.Show("您没有操作物料所在工序的权限，不能进行退料。");
                return;
            }
            //6、判断物料当前数量是否是0，如果是，给出提示”当前物料批次对应的物料数量为0，不能进行退料。”，退出方法执行。
            for (int j = 0; j < dsMatLotInfo.Tables[0].Rows.Count; j++)
            {
                if (int.Parse(dsMatLotInfo.Tables[0].Rows[j]["CURRENTQTY"].ToString().Trim()) == 0)
                {
                    MessageBox.Show("当前物料批次对应的物料数量为0，不能进行退料");
                    return;
                }
            }
            DataTable dtMatLotInfo = dsMatLotInfo.Tables[0];
            DataView dv = this.gvMaterialList.DataSource as DataView;
            if (!(dv == null))
            {
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (txtMaterialLot.Text == dv.Table.Rows[i]["MATLOT"].ToString())
                    {
                        MessageBox.Show("批次重复了");
                        return;
                    }
                }
            }

            for (int j = 0; j < dsMatLotInfo.Tables[0].Rows.Count; j++)
            {
                dv = this.gvMaterialList.DataSource as DataView;
                if (dv == null)
                {
                    DataTable dttemp = new DataTable();
                    dttemp.Columns.Add("INDEX");
                    dttemp.Columns.Add("MATLOT");
                    dttemp.Columns.Add("MATCODE");
                    dttemp.Columns.Add("MATDES");

                    dttemp.Columns.Add("WORKORDERNO");
                    dttemp.Columns.Add("UNIT");
                    dttemp.Columns.Add("CURRENTQTY");

                    dttemp.Columns.Add("RETURNQTY");
                    dttemp.Columns.Add("SUPPLIER");

                    dttemp.Columns.Add("OPERATION");
                    dttemp.Columns.Add("STORE");
                    dttemp.Columns.Add("FACROOM");

                    DataRow drtemp = dttemp.NewRow();
                    drtemp["INDEX"] = 1;
                    drtemp["MATLOT"] = dtMatLotInfo.Rows[j]["MATLOT"];
                    drtemp["MATCODE"] = dtMatLotInfo.Rows[j]["MATCODE"];
                    drtemp["MATDES"] = dtMatLotInfo.Rows[j]["MATDES"];

                    drtemp["WORKORDERNO"] = dtMatLotInfo.Rows[j]["WORKORDERNO"];
                    drtemp["UNIT"] = dtMatLotInfo.Rows[j]["UNIT"];
                    drtemp["CURRENTQTY"] = dtMatLotInfo.Rows[j]["CURRENTQTY"];

                    drtemp["RETURNQTY"] = dtMatLotInfo.Rows[j]["RETURNQTY"];
                    drtemp["SUPPLIER"] = dtMatLotInfo.Rows[j]["SUPPLIER"];

                    drtemp["OPERATION"] = dtMatLotInfo.Rows[j]["OPERATION"];
                    drtemp["STORE"] = dtMatLotInfo.Rows[j]["STORE"];
                    drtemp["FACROOM"] = dtMatLotInfo.Rows[j]["FACROOM"];
                    dttemp.Rows.Add(drtemp);
                    gcMaterialList.DataSource = dttemp;
                    gcMaterialList.RefreshDataSource();
                }
                else if (dv != null)//判断转换成功没
                {
                    DataTable dt = dv.Table;                   
                    DataRow dr = dt.NewRow();
                    dr["INDEX"] = dt.Rows.Count + 1;
                    dr["MATLOT"] = dtMatLotInfo.Rows[j]["MATLOT"];
                    dr["MATCODE"] = dtMatLotInfo.Rows[j]["MATCODE"];
                    dr["MATDES"] = dtMatLotInfo.Rows[j]["MATDES"];

                    dr["WORKORDERNO"] = dtMatLotInfo.Rows[j]["WORKORDERNO"];
                    dr["UNIT"] = dtMatLotInfo.Rows[j]["UNIT"];
                    dr["CURRENTQTY"] = dtMatLotInfo.Rows[j]["CURRENTQTY"];

                    dr["RETURNQTY"] = dtMatLotInfo.Rows[j]["RETURNQTY"];
                    dr["SUPPLIER"] = dtMatLotInfo.Rows[j]["SUPPLIER"];

                    dr["OPERATION"] = dtMatLotInfo.Rows[j]["OPERATION"];
                    dr["STORE"] = dtMatLotInfo.Rows[j]["STORE"];
                    dr["FACROOM"] = dtMatLotInfo.Rows[j]["FACROOM"];
                    dt.Rows.Add(dr);
                }
            }

            this.gvMaterialList.RefreshData();
            gvMaterialList.BestFitColumns();
            txtMaterialLot.Text = "";
        }

        //移除按钮Click事件方法。
        private void btnRemoveMaterial_Click(object sender, EventArgs e)
        {
            //1、判断物料明细列表中的选中记录个数是否=0，如果是，提示用户“必须选择一条物料明细记录，才能移除。”，退出方法执行。
            if (gvMaterialList.GetFocusedRow() == null)
            {
                MessageBox.Show("焦点要放在删除的一列上");
                return;
            }

            //2、提示用户“确定要移除选中的物料记录？”，取消，退出方法执行。
            DialogResult resulte = MessageBox.Show("确定移除选中的物料记录？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (resulte == DialogResult.Yes)
            {
                //3、确定，将选中的所有物料记录从物料明细列表中移除。
                this.gvMaterialList.DeleteRow(this.gvMaterialList.FocusedRowHandle);
                UpdateIndex(this.gvMaterialList.DataSource as DataView);
                this.gvMaterialList.RefreshData();
            }
            else if (resulte == DialogResult.No)
            {
                return;
            }  

        }
        
        //删除以后更新序号
        private void UpdateIndex(DataView dv)
        {
            DataTable dt=dv.Table;
            for(int i=0;i<dt.Rows.Count;i++)
            {
                dt.Rows[i]["INDEX"] = i + 1;
            }
        }

        //“保存”按钮的Click事件方法。
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (gvMaterialList.State == GridState.Editing && gvMaterialList.IsEditorFocused
             && gvMaterialList.EditingValueModified)
            {
                gvMaterialList.SetFocusedRowCellValue(gvMaterialList.FocusedColumn, gvMaterialList.EditingValue);
            }
            this.gvMaterialList.UpdateCurrentRow();

            gvMaterialList.RefreshData();
            gcMaterialList.Refresh();
            //1、判断退料日期、退料班别、员工号是否为空，如果为空，给出提示，退出方法执行。
            if (deReturnMatDate.Text == "" || lueShiftName.Text == "" || txtOperator.Text == "")
            {
                MessageBox.Show("退料日期，退料班别，员工号有为空的，三个都不能为空才可以保存");
                return;
            }

            //2、判断物料明细列表中的记录是否>0，如果不大于0，则给出提示”物料明细中必须有一条记录。“，退出方法执行。
            if (gvMaterialList.DataRowCount < 1)
            {
                MessageBox.Show("物料明细中必须有一条记录");
                return;
            }

            //3、判断物料明细列表中物料的退库数量是否>数据库中该物料的当前数量，如果是，则给出提示”物料：{物料批号}的退库数量大于当前数量。“，退出方法执行。											
            //gvMaterialList.
            DataView dvMatLotList = gvMaterialList.DataSource as DataView;
            DataTable dtMatLotList = dvMatLotList.Table;
            for (int i = 0; i < dtMatLotList.Rows.Count; i++)
            {
                if (float.Parse(dtMatLotList.Rows[i]["RETURNQTY"].ToString()) > float.Parse(dtMatLotList.Rows[i]["CURRENTQTY"].ToString()))
                {
                    // string tmpMatLot=dtMatList.Rows[i]["MaterialLot"].ToString();
                    MessageBox.Show("物料批号" + dtMatLotList.Rows[i]["MATLOT"].ToString() + "退库数量大于当前数量");
                    return;
                }
            }
            //modify by qym 20120408
            for (int i = 0; i < dtMatLotList.Rows.Count; i++)
            {
                if (int.Parse(dtMatLotList.Rows[i]["RETURNQTY"].ToString()) ==0)
                {
                    MessageBox.Show("物料批号" + dtMatLotList.Rows[i]["MATLOT"].ToString() + "退库数量为0，请输入一个大于0的数");
                    return;
                }
            }
            //modify by qym 20120408
            if (txtReturnMatReason.Text.Trim().Length == 0)
            {
                MessageBox.Show("退料原因为空，请填写退料原因");
                return;
            }


            //4、向WST_TL_ZMMLKO，WST_TL_ZMMLPO插入退料记录。根据规则生成退料单号。    
            //5、更新WST_STORE_MATERIAL_DETAIL物料批号对应物料的当前数量=当前数量-退库数量。    			
            //6、调用SAP RFC将退料单写到SAP中。	
            string strRetMatNo = CreateReturnMatNo(); //生成退料单

            string strRetMatDate= String.Format("{0:yyyy-MM-dd}", deReturnMatDate.DateTime )  ; //退库日期
            string strShift=lueShiftName.Text;  //班别
            string strOperator=txtOperator.Text;   //作业员
            string strRetMatReason=txtReturnMatReason.Text ; //退料原因

            bool tmpbool = false;
            WOMaterialReturnEntity wOMaterialReturnEntity = new WOMaterialReturnEntity();   
            tmpbool=wOMaterialReturnEntity.Save(strRetMatNo,strRetMatDate,strShift,strOperator,strRetMatReason,dtMatLotList);
 
		    //7、生成退料单成功，提示"退料成功。”，将退料单号显示在控件中，禁用保存，移除按钮。设置退料日期，退库数量为只读。	
            if (tmpbool==true)    
            {  
                MessageBox.Show("退料成功");
                txtReturnNo.Text = strRetMatNo;
                tsbSave.Enabled =false;
                btnRemoveMaterial.Enabled =false;
                //设置退料日期               
                //退库数量为只读
                gcReturnQty.OptionsColumn.AllowEdit = false ;
                //保存成功以后显示保存信息，不能随便修改。
                txtMaterialLot.Enabled = false;
                txtReturnMatReason.Enabled = false;
            }
            //8、如果执行中失败，给出错误提示。
            else
            {
                MessageBox.Show("退料失败");
            } 
        }

        // 根据MES工厂取得ERP线别
        private static string GetFromMESFacToERPLine(string strMESFactory)
        {
            string strFacCode=null;
            string[] columns = new string[] { "M_RT_CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", BASEDATA_CATEGORY_NAME.MEScontrastERP);
            KeyValuePair<string, string> condition = new KeyValuePair<string, string>("MESFACTORY", strMESFactory);
            List<KeyValuePair<string, string>> lstCondition=new List<KeyValuePair<string, string>>();
            lstCondition.Add(condition);
            DataTable dt = BaseData.GetBasicDataByCondition(columns, category, lstCondition);
            strFacCode = dt.Rows[0]["M_RT_CODE"].ToString();
            return strFacCode;
        }
        
        //取得YYMD
        private static string GetYYMD()
        {
            string strYYMd = null;
            string strDate = DateTime.Now.ToString("yyMMdd");
            string strYear = strDate.Substring(0, 2);
            string strMonth = strDate.Substring(2, 2);
            string strDay = strDate.Substring(4, 2);
            strYYMd = strYear;
            if (strMonth.Equals("10"))
            {
                strYYMd = strYYMd + "A";
            }
            else if (strMonth.Equals("11"))
            {
                strYYMd = strYYMd + "B";
            }
            else if (strMonth.Equals("12"))
            {
                strYYMd = strYYMd + "C";
            }
            else                //等于本身了
            {
                strYYMd = strYYMd + strMonth.Substring(1,1);
            }
            string strAllDay = "123456789ABCDEFGHJKLMNPQRSTUVWXYZ";//截取第几码
            strYYMd = strYYMd + strAllDay.Substring(int.Parse(strDay) - 1, 1);
            return strYYMd;
        }
        
        //生成单号
        private  string CreateReturnMatNo()
        {
            string strRetMatNo = null;
            //厂别
            DataView dvMatLotList = gvMaterialList.DataSource as DataView;
            DataTable dtMatLotList = dvMatLotList.Table;
            string strFacRoom = dtMatLotList.Rows[0]["FACROOM"].ToString();//工厂车间
            //调用实体方法
            WOMaterialReturnEntity wOMaterialReturnEntity = new WOMaterialReturnEntity();
            DataSet dsFactory = wOMaterialReturnEntity.GetFacRoomtoFac(strFacRoom);

            string strFactory = dsFactory.Tables[0].Rows[0]["FACTORY"].ToString();
            string tmpFacCode = GetFromMESFacToERPLine(strFactory);

            //4.2 年月日
            string tmpYYMD = GetYYMD();

            //4.3流水码
            string strPrex = tmpFacCode + tmpYYMD;
            // WOMaterialReturnEntity wOMaterialReturnEntity = new WOMaterialReturnEntity();
            DataSet dsMaxRetMatNo = wOMaterialReturnEntity.GetRetMatNo(strPrex);
            string strNum = "00000";
            if (dsMaxRetMatNo.Tables[0].Rows.Count < 1)
            {
                strNum = "00001";
            }
            else
            {
                int itmpNum = int.Parse(dsMaxRetMatNo.Tables[0].Rows[0]["SERIALNO"].ToString());
                itmpNum = itmpNum + 1;
                strNum = string.Format("{0:D5}", itmpNum); 
            }
            //退料单号
            strRetMatNo = strPrex + strNum;
            return strRetMatNo;
        }

        //删除    ,代表不退库了，要加上去在物料明细中
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            //1、如果退料单号为空，则提示”退料单号为空，不能删除。“。，退出方法执行。	
            if (lcBaseInfo.Text == "")
            {
                MessageBox.Show("退料单号为空，不能删除");
                return;
            }

            //2、退料单号不为空，根据退料单号，调用SAP RFC接口判断SAP中的退料单是否删除？	
             DataSet dsIMPORT = new DataSet();
             DataTable dtIMPORT = new DataTable();
             dtIMPORT.Columns.Add("I_WERKS");
             dtIMPORT.Columns.Add("I_DATB");
             dtIMPORT.Columns.Add("I_DATE");
             dtIMPORT.Columns.Add("I_STAT");
             dtIMPORT.Columns.Add("I_TYP");
             dtIMPORT.Columns.Add("I_ART");
             DataRow drIMPORT = dtIMPORT.NewRow();
             drIMPORT["I_WERKS"] = "";  //ERP的工厂
             drIMPORT["I_DATB"] = "";    //不知道
             drIMPORT["I_DATE"] = "";
             drIMPORT["I_STAT"] = "";
             drIMPORT["I_TYP"] = "";
             drIMPORT["I_ART"] = "";
             dtIMPORT.Rows.Add(drIMPORT);
             dsIMPORT.Tables.Add(dtIMPORT);

            //I_WERKS        type     WERKS_D           工厂
            //I_DATB         type     DATS              创建日期—起始
            //I_DATE         type     DATS              创建日期—终止
            //I_STAT         type     ZMMAPRED          单据状态
            //I_TYP          type     ZMMTYP            订单类别
            //I_ART          type     ZMMART            订单类型

             bool bltmp = false;//true代表删除，flase 代表没有删除
             bool bltmp2 = false;//SAP已经删除，删除退料表示是否出错
             //判断SAP是否删除
             WOMaterialReturnEntity wOMaterialReturnEntity = new WOMaterialReturnEntity();
             bltmp = wOMaterialReturnEntity.DeleteMat(dsIMPORT );
             
            //3、如果SAP中退料单已删除，则从WST_TL_ZMMLKO，WST_TL_ZMMLPO删除退料单号对应的退料单记录,更新明细
            if (bltmp==true)//删除了
            {
                bltmp2 = wOMaterialReturnEntity.DeleteMat2(txtReturnNo.Text );
                if (bltmp2 == true)
                {
                    MessageBox.Show("删除成功");
                }
                else
                {
                    MessageBox.Show("删除失败，请检查以后重新删除");
                }
            }								
            //4、如果SAP中退料单未删除，则提示用户”需要SAP中删除该退料单后，才能执行删除。“。
            else if (bltmp==false )//没有删除
            {
                MessageBox.Show("需要SAP中删除该退料单后，才能执行删除。");
                return;
            }
        }

        //关闭按钮的事件
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        //工单退料清单
        private void tsbReturnMatList_Click(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == "工单退料清单")
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    
                    return;
                }
            }
            //显示材料耗用清单的用户控件
            WorkOrderMatRetListViewContent a = new WorkOrderMatRetListViewContent();
            a.parent2 = this;
            WorkbenchSingleton.Workbench.ShowView(a);

        }

        //双击以后返回
        public void tiaoyong(DataSet ds, string strRetMatList,bool savefalse,bool movedeletefalse,bool newtrue)
        {
            txtReturnNo.Text =strRetMatList;
            gcMaterialList.DataSource=ds.Tables[0];
            gcMaterialList.Refresh();
            btnRemoveMaterial.Enabled = movedeletefalse;
            tsbNew.Enabled = newtrue;
            tsbSave.Enabled = savefalse;
            //tsbNew.Enabled = false;
            tsbDelete.Enabled = true;
        }

        ////退库数量及时更新
        //private void gvMaterialList_CellValueChanging(object sender, CellValueChangedEventArgs e)
        //{
        //    if (e.Column.FieldName == "RETURNQTY")
        //    {
        //        string strtemp = e.Value.ToString();

        //        gvMaterialList.SetRowCellValue(e.RowHandle, "RETURNQTY", strtemp);
        //    }
        //}

        //退库数量及时更新
        //private void gvMaterialList_CellValueChanged(object sender, CellValueChangedEventArgs e)
        //{
        //    if (e.Column.FieldName == "RETURNQTY")
        //    {
        //        string strtemp = e.Value.ToString();

        //        gvMaterialList.SetRowCellValue(e.RowHandle, "RETURNQTY", strtemp);
        //    }

        //}

    }
}
