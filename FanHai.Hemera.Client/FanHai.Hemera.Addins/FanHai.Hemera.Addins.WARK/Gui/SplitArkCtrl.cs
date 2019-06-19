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
    public partial class SplitArkCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 出货操作对象。
        /// </summary>
        ShipmentOperationEntity _entity = new ShipmentOperationEntity();
        DataTable _dtProductGrade = null;
        string _ArkCode = string.Empty;
        public SplitArkCtrl()
        {
            InitializeComponent();
        }
        //关闭
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
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
        private void GroupArkCtrl_Load(object sender, EventArgs e)
        {
            BindArkCode();
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

        public void SelectInf()
        {
            _ArkCode = cbeArkCode.Text.ToString().Trim();
            if (string.IsNullOrEmpty(_ArkCode))
            {
                MessageService.ShowMessage("请输入柜号。", "${res:Global.SystemInfo}");
                return;
            }
            gcArk.DataSource = null;
            GroupArkEntity arkEntity = new GroupArkEntity();
            DataSet ds = arkEntity.GetArkNumber(_ArkCode);
            
               
            
            if (ds.Tables[0].Rows.Count > 1)
            {
                MessageService.ShowMessage("柜号存在重复或不存在请重新输入", "${res:Global.SystemInfo}");
                return;
            }
            if (ds.Tables[0].Rows.Count == 1)
            {
                //查询明细表中柜主键为同一个且托状态为可用的托号
                DataSet dsCdetail = arkEntity.GetContainerDetailInf(ds.Tables[0].Rows[0]["CONTAINER_KEY"].ToString().Trim());
                if (dsCdetail.Tables[0] != null)
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
                else
                {
                    MessageService.ShowMessage("柜号中不存在托信息,请重新选择柜信息。", "${res:Global.SystemInfo}");
                    return;
                }
            }
            else
            {
                MessageService.ShowMessage("不存在柜信息。", "${res:Global.SystemInfo}");
                return;
            }
        }
        //选择柜号后回车事件
        #region
        private void cbeArkCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SelectInf();
            }
        }
        #endregion

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

        private void tsbSave_Click(object sender, EventArgs e)
        {
            _ArkCode = cbeArkCode.Text.Trim();
            if (string.IsNullOrEmpty(cbeArkCode.Text.ToString().Trim()))
            {
                MessageService.ShowMessage("柜号不能为空。", "${res:Global.SystemInfo}");
                return;
            }
            //获取对应柜号主键，然后修改明细表信息，保存界面托信息，修改包装表中状态为已组柜
            GroupArkEntity arkEntity = new GroupArkEntity();
            DataSet ds = arkEntity.GetArkNumber(_ArkCode);
            if (ds.Tables[0].Rows.Count < 1)
            {
                MessageService.ShowMessage("该柜未组柜不可拆柜。", "${res:Global.SystemInfo}");
                return;
            }

            if (MessageService.AskQuestion("你确定要保存吗?", "拆柜"))
            {                
                int flag = 0;
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

                if (dtCdetail == null)
                {
                    MessageService.ShowMessage("该柜不存在可拆托信息。", "${res:Global.SystemInfo}");
                    return;
                }

                DataSet dsIn = new DataSet();
                dsIn.Merge(dtCdetail);
                Hashtable hashTable = new Hashtable();
                hashTable.Add("CREATOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                hashTable.Add("CONTAINER_CODE", _ArkCode);
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
                    BindArkCode();
                    gcArk.DataSource = null;     //清空datasource      
                }
                else
                {
                    MessageService.ShowMessage("保存失败。", "${res:Global.SystemInfo}");
                }

            }
        }

        private void tsbAllSplit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbeArkCode.Text.ToString().Trim()))
            {
                MessageService.ShowMessage("请输入柜号。", "${res:Global.SystemInfo}");
                return;
            }

            _ArkCode = cbeArkCode.Text.ToString().Trim();
            //获取对应柜号主键，然后修改明细表信息，保存界面托信息，修改包装表中状态为已组柜
            GroupArkEntity arkEntity = new GroupArkEntity();
            DataSet ds = arkEntity.GetArkNumber(_ArkCode);
            SplitArkEntity splitEntity = new SplitArkEntity();

            if (ds.Tables[0].Rows.Count < 1 )
            {
                MessageService.ShowMessage("该柜未组柜不可拆柜。", "${res:Global.SystemInfo}");
                return;
            }

            //查询明细表中柜主键为同一个且托状态为可用的托号
            DataSet dsCdetail = arkEntity.GetContainerDetailInf(ds.Tables[0].Rows[0]["CONTAINER_KEY"].ToString().Trim());
            DataTable dtCdetail = dsCdetail.Tables[0];

            if (dtCdetail.Rows.Count < 1)
            {
                MessageService.ShowMessage("该柜不存在可拆托信息。", "${res:Global.SystemInfo}");
                return;
            }

            if (MessageService.AskQuestion("你确定要整柜拆除么?", "整柜拆柜"))
            {                               
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        MessageService.ShowMessage("柜号存在重复或不存在请重新输入", "${res:Global.SystemInfo}");
                        return;
                    }
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        bool bo = splitEntity.SplitArk(ds);
                        if (bo)
                        {
                            MessageService.ShowMessage("整柜拆柜成功", "${res:Global.SystemInfo}");
                            BindArkCode();
                            gcArk.DataSource = null;
                        }
                    }

                }
            }
        }

        private void smbSelect_Click(object sender, EventArgs e)
        {
            SelectInf();
        }

        private void tsbSpilt_Click(object sender, EventArgs e)
        {
            BindArkCode();
            this.cbeArkCode.Focus();
            gcArk.DataSource = null;
        }

    }
}
