using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;

using SolarViewer.Hemera.Utils.Common;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Gui.Core;
using SolarViewer.Hemera.Utils.Entities;
using SolarViewer.Gui.Framework.Gui;
using SolarViewer.Hemera.Share.Interface;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using SolarViewer.Hemera.Utils.Controls;
using SolarViewer.Hemera.Share.CommonControls.Dialogs;
using System.Collections;
using SolarViewer.Hemera.Utils.Dialogs;
using SolarViewer.Hemera.Share.Common;
using System.Threading;

namespace SolarViewer.Hemera.Addins.WARK
{
    public partial class GroupArkCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 出货操作对象。
        /// </summary>
        ShipmentOperationEntity _entity = new ShipmentOperationEntity();
        DataTable _dtProductGrade = null;
        public GroupArkCtrl()
        {
            InitializeComponent();
        }
        //关闭
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        //方法定义
        public void GetPalletInf()
        {
            GroupArkEntity gae = new GroupArkEntity();
            string strPalletNo = txtPalletNo.Text.Trim();
            //判定是否输入信息为空
            if (string.IsNullOrEmpty(txtPalletNo.Text.ToString()))
            {
                MessageService.ShowMessage("请输入托号！", "${res:Global.SystemInfo}");
                return;
            }
            //判断输入的托号是否重复？
            DataView dv = gvArk.DataSource as DataView;
            if (dv != null)
            {
                int length = dv.Table.Select("PALLET_NO='" + strPalletNo + "'").Length;
                if (length > 0)
                {
                    MessageService.ShowMessage("列表中已存在托号为[" + strPalletNo + "]的信息，不能重复添加。", "${res:Global.SystemInfo}");
                    return;
                }
            }
            //判定托是否存在
            DataTable dt = gae.GetPalletInf(strPalletNo).Tables[0];
            if (dt.Rows.Count > 0)
            {
                //判定输入托号是否已经入库根据托状态信息栏判定
                if (dt.Rows[0]["CS_DATA_GROUP"].ToString().Trim() == "3")
                {
                    if (dt.Rows[0]["ARK_FLAG"].ToString().Trim() == "0")
                    {
                        DataTable dt01 = new DataTable();
                        dt01 = dt;
                        for (int i = 0; i < dt01.Rows.Count; i++)
                        {
                            DataRow dr = dt01.Rows[i];
                            string grade = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE]);
                            dr[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE] = GetProductGradeDisplayText(grade);
                        }


                        if (gvArk.DataSource == null)
                        {
                            dt01 = dt;
                            if (!dt01.Columns.Contains("ROWNUMBER"))
                                dt01.Columns.Add("ROWNUMBER");
                            dt01.Rows[0]["ROWNUMBER"] = 1;

                            gcArk.DataSource = dt01;
                        }
                        else if (gvArk.DataSource != null)
                        {
                            dt01 = ((DataView)gvArk.DataSource).Table;
                            dt01.ImportRow(dt.Rows[0]);

                            if (!dt01.Columns.Contains("ROWNUMBER"))
                                dt01.Columns.Add("ROWNUMBER");

                            for (int i = 1; i < dt01.Rows.Count + 1; i++)
                                dt01.Rows[i - 1]["ROWNUMBER"] = i.ToString();

                            gcArk.DataSource = dt01;
                        }


                        else
                        {
                            gcArk.DataSource = dt;
                        }
                        txtPalletNo.Text = "";
                    }
                    else
                    {
                        MessageService.ShowMessage("托号[" + strPalletNo + "]已经组柜,不能添加。", "${res:Global.SystemInfo}");
                    }
                }
                else
                {
                    MessageService.ShowMessage("托号[" + strPalletNo + "]未入库或已出库，不能添加。", "${res:Global.SystemInfo}");
                    return;
                }
            }
            else
            {
                MessageService.ShowMessage("没有[" + strPalletNo + "]的托号，请重新输入。", "${res:Global.SystemInfo}");
                return;
            }
        }
        //方法定义
        public void BindArkCode()
        {
            cbeArkCode.Properties.Items.Clear();
            GroupArkEntity arkEntity = new GroupArkEntity();
            DataSet ds = arkEntity.GetArkNumber("");
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cbeArkCode.Properties.Items.Add(ds.Tables[0].Rows[i]["CONTAINER_CODE"]);
                }
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
        //获取托信息载入到界面数据表
        #region
        //托号回车带出托信息和添加功能相同
        private void txtPalletNo_KeyPress(object sender, KeyPressEventArgs e)
        {
           
            if (e.KeyChar == 13)
            {
                if (cbeArkCode.Enabled == false)
                {
                    GetPalletInf();
                }
                else
                {
                    MessageService.ShowMessage("请在选择柜号后回车。", "${res:Global.SystemInfo}");
                }
            }    
        }
        //添加功能带出托信息和输入托号回车功能相同
        private void sbtAdd_Click(object sender, EventArgs e)
        {
            GetPalletInf();
        }
        #endregion

        private void GroupArkCtrl_Load(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //dt.DefaultView.RowFilter = " NAME LIKE 'Z%' AND AGE=17";
            //var linq = from item in dt.AsEnumerable()
            //           where item["NAME"].ToString().StartsWith
            //this.gvArk.DataSource = dt.DefaultView;
            BindArkCode();
            gcArk.DataSource = null;     //清空datasource
            this.cbeArkCode.Focus();

        }
        //移除选中行信息
        private void sbtDel_Click(object sender, EventArgs e)
        {
            if (this.gvArk.GetFocusedRow() != null)
            {
                this.gvArk.DeleteRow(this.gvArk.FocusedRowHandle);

                DataTable dt = ((DataView)gvArk.DataSource).Table.GetChanges(DataRowState.Modified);
                if (dt != null)
                {
                    if (!dt.Columns.Contains("ROWNUMBER"))
                        dt.Columns.Add("ROWNUMBER");

                    for (int i = 1; i < dt.Rows.Count + 1; i++)
                        dt.Rows[i - 1]["ROWNUMBER"] = i.ToString();

                    gcArk.DataSource = dt;
                }
                else
                {
                    gcArk.DataSource = null;     //清空datasource                    
                }

            }
            else
            {
                MessageService.ShowMessage("必须选择至少选择一条记录", "${res:Global.SystemInfo}");
            }
            
        }
        private void cbeArkCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                gcArk.DataSource = null;
                GroupArkEntity arkEntity = new GroupArkEntity();
                DataSet ds = arkEntity.GetArkNumber(cbeArkCode.Text.Trim());
                if (ds.Tables[0].Rows.Count > 1)
                {
                    MessageService.ShowMessage("柜号存在重复或不存在请重新输入", "${res:Global.SystemInfo}");
                    return;
                }
                if (ds.Tables[0].Rows.Count == 1)
                {
                    //查询明细表中柜主键为同一个且托状态为可用的托号
                    DataSet dsCdetail = arkEntity.GetContainerDetailInf(ds.Tables[0].Rows[0]["CONTAINER_KEY"].ToString().Trim());
                    if (dsCdetail != null)
                    {
                        //根据查询出来的托号,在包装表中查询托对应的详细信息
                        DataSet dsWipCInf = arkEntity.GetWipConInf(dsCdetail);
                        DataTable dt01 = new DataTable();
                        dt01 = dsWipCInf.Tables[0];
                        for (int i = 0; i < dt01.Rows.Count; i++)
                        {
                            DataRow dr = dt01.Rows[i];
                            string grade = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE]);
                            dr[WIP_CONSIGNMENT_FIELDS.FIELDS_GRADE] = GetProductGradeDisplayText(grade);                            
                        }
                        

                        if (gvArk.DataSource == null)
                        {
                            
                            if (dt01 != null)
                            {
                                if (!dt01.Columns.Contains("ROWNUMBER"))
                                    dt01.Columns.Add("ROWNUMBER");
                                for (int i = 0; i < dt01.Rows.Count; i++)
                                {
                                    dt01.Rows[i]["ROWNUMBER"] = i + 1;
                                }
                                gcArk.DataSource = dt01;
                                
                            }
                            else
                                gcArk.DataSource = null;
                        }
                    }
                    cbeArkCode.Enabled = false;
                }
                else if (ds.Tables[0].Rows.Count < 1)
                {
                    gcArk.DataSource = null;
                    cbeArkCode.Enabled = false;
                }

            }
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
                BindArkCode();
                cbeArkCode.Enabled = true;
                gcArk.DataSource = null;
                txtPalletNo.Text = "";         

        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbeArkCode.Text.ToString().Trim()))
            {
                MessageService.ShowMessage("请输入柜号。", "${res:Global.SystemInfo}");
                return;
            }
            if (cbeArkCode.Enabled == false)
            {
                if (MessageService.AskQuestion("你确定要保存当前界面的数据吗？", "保存"))
                {
                    //获取对应柜号主键，然后修改明细表信息，保存界面托信息，修改包装表中状态为已组柜
                    GroupArkEntity arkEntity = new GroupArkEntity();
                    DataSet ds = arkEntity.GetArkNumber(cbeArkCode.Text.Trim());
                    int flag = 0;
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        MessageService.ShowMessage("柜号存在重复或不存在请重新输入", "${res:Global.SystemInfo}");
                        return;
                    }
                    #region
                    //可以查到抬头表中存在柜
                    else if (ds.Tables[0].Rows.Count == 1)
                    {
                        flag = 0;
                        DataTable dt = new DataTable();
                        //获取界面数据表信息
                        if (gvArk.DataSource != null)
                        {
                            dt = ((DataView)gvArk.DataSource).Table.GetChanges(DataRowState.Modified);
                        }
                        else
                            dt = null;
                       
                        //查询明细表中柜主键为同一个且托状态为可用的托号
                        DataSet dsCdetail = arkEntity.GetContainerDetailInf(ds.Tables[0].Rows[0]["CONTAINER_KEY"].ToString().Trim());
                        DataTable dtCdetail = dsCdetail.Tables[0];
                        dtCdetail.TableName = "DETAILPALLNO";

                        DataSet dsIn = new DataSet();
                        dsIn.Merge(dtCdetail);
                        Hashtable hashTable = new Hashtable();
                        hashTable.Add("CREATOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                        hashTable.Add("CONTAINER_CODE", cbeArkCode.Text.Trim());
                        DataTable tableParam = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                        tableParam.TableName = "HASH";
                        dsIn.Merge(tableParam);
                        DataTable dtArkKey = ds.Tables[0];
                        dtArkKey.TableName = "ARKKEY";
                        dsIn.Merge(dtArkKey);

                        bool bo = arkEntity.UpdateArkInf(dsIn, dt, flag);
                        if (bo)
                        {
                            MessageService.ShowMessage("保存成功。", "${res:Global.SystemInfo}");
                            cbeArkCode.Enabled = true;
                            BindArkCode();
                            gcArk.DataSource = null;     //清空datasource      
                        }
                        else
                        {
                            MessageService.ShowMessage("保存失败。", "${res:Global.SystemInfo}");
                        }
                    }
                    #endregion
                    #region
                    else if (ds.Tables[0].Rows.Count < 1)
                    {
                        flag = 1;
                        DataTable dt = new DataTable();
                        //获取界面数据表信息
                        if (gvArk.DataSource != null)
                        {
                            dt = ((DataView)gvArk.DataSource).Table.GetChanges(DataRowState.Modified);
                        }
                        else
                            dt = null;

                        if (dt != null)
                        {
                            DataSet dsIn = new DataSet();

                            Hashtable hashTable = new Hashtable();
                            hashTable.Add("CREATOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                            hashTable.Add("CONTAINER_CODE", cbeArkCode.Text.Trim());
                            DataTable tableParam = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                            tableParam.TableName = "HASH";
                            dsIn.Merge(tableParam);
                            bool bo = arkEntity.UpdateArkInf(dsIn, dt, flag);
                            if (bo)
                            {
                                MessageService.ShowMessage("保存成功。", "${res:Global.SystemInfo}");
                                cbeArkCode.Enabled = true;
                                BindArkCode();
                                gcArk.DataSource = null;     //清空datasource   
                            }
                            else
                            {
                                MessageService.ShowMessage("保存失败。", "${res:Global.SystemInfo}");
                            }
                        }
                        else
                        {
                            MessageService.ShowMessage("无保存信息", "${res:Global.SystemInfo}");
                            return;
                        }
                    }
                    #endregion
                }
                    
            }
            else
            {
                MessageService.ShowMessage("请在选择柜号后回车。", "${res:Global.SystemInfo}");
            }
        }
    }
}
