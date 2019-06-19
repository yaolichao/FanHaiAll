using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using System.Collections;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Controls.Common;
using DevExpress.XtraGrid.Views.Grid;

namespace FanHai.Hemera.Addins.MM
{
    public partial class ReturnMaterialQueryCtrl : BaseUserCtrl
    {
        public ReturnMaterialQueryCtrl()
        {
            InitializeComponent();
            InitializeLanguage();
            GridViewHelper.SetGridView(gridView1);
        }
        public void InitializeLanguage()
        {
            gridColumn1.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialQueryCtrl.GridControl.0001}");//退料时间
            gridColumn2.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialQueryCtrl.GridControl.0002}");//硅片料号
            gridColumn3.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialQueryCtrl.GridControl.0003}");//物料批次
            gridColumn4.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialQueryCtrl.GridControl.0004}");//退料原因
            gridColumn5.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialQueryCtrl.GridControl.0005}");//退料数量

            gridColumn6.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialQueryCtrl.GridControl.0006}");//退料站点
            gridColumn7.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialQueryCtrl.GridControl.0007}");//班别
            gridColumn8.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialQueryCtrl.GridControl.0008}");//生产批号
            gridColumn9.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialQueryCtrl.GridControl.0009}");//用户名字
            gridColumn10.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialQueryCtrl.GridControl.0010}");//用户编码
        }

        private void tsbQuery_Click(object sender, EventArgs e)
        {
            DataSet ds=new DataSet();         
            ds = GetReturnMaterialRecord();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gridControl1.MainView = gridView1;
                gridControl1.DataSource = ds.Tables[0];
                gridView1.BestFitColumns();
            }
            else
            {
                //MessageBox.Show("没有查到任何资料");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialQueryCtrl.msg.0001}"));
            }
        }

        private void ReturnMaterialQueryCtrl_Load(object sender, EventArgs e)
        {
            gridControl1.MainView = gridView1;
            gridControl1.DataSource = null;
            this.startDate.DateTime = DateTime.Now.AddDays(-1);
            this.endDate.DateTime = DateTime.Now;
        }

        private DataSet GetReturnMaterialRecord()
        {
            DataSet ds = new DataSet();
            DataSet dsSearch = new DataSet();
            Hashtable hashTable = new Hashtable();
            DataTable searchTable = new DataTable();
            DateTime? dtStartTime = null;
            DateTime? dtEndTime = null;
            //开始日期不为空
            if (startDate.Text.Length > 0)
            {
                dtStartTime = DateTime.Parse(startDate.Text);
                hashTable.Add("START_TIME", dtStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            //结束日期不为空
            if (endDate.Text.Length > 0)
            {
                dtEndTime = DateTime.Parse(endDate.Text);
                hashTable.Add("END_TIME", dtEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            //开始时间不为空 并且 结束时间不为空 并且 开始时间>结束时间
            if (dtStartTime != null && dtEndTime != null && dtStartTime > dtEndTime)
            {
                //MessageService.ShowMessage("开始时间必须小于结束时间。", "系统提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialQueryCtrl.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.startDate.Focus();
                return null;
            }

            if (hashTable.Count > 0)
            {
                searchTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                searchTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                dsSearch.Tables.Add(searchTable);
            }
            ReturnMaterialQueryEntity returnMaterialQueryEntity = new ReturnMaterialQueryEntity();            
            ds = returnMaterialQueryEntity.GetReturnMaterial(dsSearch);
            return ds;
        }

        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
