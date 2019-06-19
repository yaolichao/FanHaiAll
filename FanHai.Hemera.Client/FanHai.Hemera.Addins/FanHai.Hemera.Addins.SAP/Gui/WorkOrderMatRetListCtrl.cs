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
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;

using FanHai.Hemera.Share.Interface;
using DevExpress.XtraGrid.Views.Base;
namespace FanHai.Hemera.Addins.SAP
{
    public partial class WorkOrderMatRetListCtrl:BaseUserCtrl
    {
        //构造函数
        public WorkOrderMatRetListCtrl()
        {
            InitializeComponent();
        }

        public WorkOrderMatRetListViewContent parent
        {
            get;
            set;
        }
        //窗体载入（Load）事件方法
        private void WorkOrderMatRetListCtrl_Load(object sender, EventArgs e)
        {
            //1、通过PropertyService获取PROPERTY_FIELDS.STORES的值。
              string strStore= PropertyService.Get(PROPERTY_FIELDS.STORES);
            //2、通过PropertyService获取PROPERTY_FIELDS.OPERTIONS的值。	
               string strOperation = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);		

            //3、根据步骤1获取的线上仓名称和步骤2获取的工序名称从WST_TL_ZMMLPO，WST_TL_ZMMLKO，WST_STORE_MATERIAL_DETAIL，WST_STORE_MATERIAL，WST_STORE分页获取数据显示到列表中（每页默认为20行记录）。											
              WOMaterialReturnEntity wOMaterialReturnEntity = new WOMaterialReturnEntity();
              DataSet ds = wOMaterialReturnEntity.GetRetMatInfo(strStore, strOperation); 
              Content.MainView=gvWoRetMatList;
              Content.DataSource = ds.Tables[0];
              gvWoRetMatList.BestFitColumns();
        }

        //“查询”按钮事件方法											
        private void tsbQuery_Click(object sender, EventArgs e)
        {
            //显示“工单退料查询”对话框（界面三）
            WorkOrderMatRetListQueryDialogCtrl WorkOrderMatRetListQueryDialog = new WorkOrderMatRetListQueryDialogCtrl();
            WorkOrderMatRetListQueryDialog.ParentControl = this; //???
            WorkOrderMatRetListQueryDialog.ShowDialog();
        }

        //定义一个方法，给其他函数调用，绑定数据源
        public  void bangding(DataSet ds2)
        {
            Content.DataSource = null;
            Content.RefreshDataSource();
            Content.DataSource = ds2.Tables[0];
            Content.RefreshDataSource();
            gvWoRetMatList.BestFitColumns();
           // Content.Refresh();
        }
        //“关闭”按钮事件方法
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        //退料清单列表行DoubleClick事件方法
        private void Content_DoubleClick(object sender, EventArgs e)
        {
            //1、将该行对应的退料单传递给工单退料界面（界面一）	
              int  i=gvWoRetMatList.FocusedRowHandle;
	          DataView dv=gvWoRetMatList.DataSource as DataView;
              DataTable dt=dv.Table;
              string strRetMatList = dt.Rows[i]["RETURNNO"].ToString(); //一个参数
            //2、根据退料单号从WST_TL_ZMMLKO，WST_TL_ZMMLPO，WST_STORE_MATERIAL_DETAIL，WST_STORE_MATERIAL获取退料信息。
              WOMaterialReturnEntity wOMaterialReturnEntity = new WOMaterialReturnEntity();
              DataSet ds = wOMaterialReturnEntity.GetRetMatInfo1( strRetMatList); //两个参数
			 								
            //3、如果没有获取到退料单号对应的退料信息，提示“退料单不存在。“，退出方法执行。	
              if (ds.Tables[0].Rows.Count < 1)
              {
                  MessageBox.Show("退料单不存在");
              }
            //4、将退料单信息显示在工单退料界面中（界面一），禁用保存，移除按钮。设置退料日期，退库数量为只读。
            bool savefalse=false;
            bool movedeletefalse=false;
            bool newtrue=true;
            this.parent.parent2.tiaoyong(ds, strRetMatList, savefalse, movedeletefalse,newtrue);
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        //自动填充序号
        private void gvWoRetMatList_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "INDEX1": //设置行号
                    e.DisplayText = Convert.ToString(e.RowHandle + 1);
                    break;
                default:
                    break;
            }
        }

    }
}
