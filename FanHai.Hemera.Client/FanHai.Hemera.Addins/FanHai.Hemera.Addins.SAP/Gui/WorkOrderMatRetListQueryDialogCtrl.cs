using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;


using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.SAP
{
    public partial class WorkOrderMatRetListQueryDialogCtrl : BaseDialog
    {
        //定义一个属性  peter
        public WorkOrderMatRetListCtrl ParentControl
        {
            get;
            set;
        }

        //构造函数
        public WorkOrderMatRetListQueryDialogCtrl()
        {
            InitializeComponent();
        }

        //窗体载入（Load）事件方法
        private void WorkOrderMatRetListQueryDialogCtrl_Load(object sender, EventArgs e)
        {
            //1、根据登录用户将登录用户拥有权限的工序名称绑定到窗体控件中，设置空为控件默认值。         
            string StrOperation = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            cmbBEOperation.Properties.Items.Add("");
            if (StrOperation.Length > 0)
            {
                string[] StrArrOperation = StrOperation.Split(',');
                for (int i = 0; i < StrArrOperation.Length; i++)
                {
                    cmbBEOperation.Properties.Items.Add(StrArrOperation[i]);
                }
            }
      		
            //"2、根据登录用户将登录用户拥有权限的线边仓名称绑定到窗体控件中，设置空为控件默认值。
            string strStore = PropertyService.Get(PROPERTY_FIELDS.STORES);
            cmbBEStore.Properties.Items.Add("");
            if (strStore.Length > 0)
            {
                string[] strArrStore = strStore.Split(',');
                for (int i = 0; i < strArrStore.Length; i++)
                {
                    cmbBEStore.Properties.Items.Add(strArrStore[i]);
                }
            }				
            //3、根据PropertyService获取PROPERTY_FIELDS.STORES的值。从WST_STORES,FMM_LOCATION,FMM_LOCATION_RET根据线边仓名称获取用户拥有权限的工厂车间名称集合绑定到窗体控件中，设置空为控件默认值。											
            DataTable dtStore = FactoryUtils.GetFactoryRoomByStores(PropertyService.Get(PROPERTY_FIELDS.STORES));
            cmbBERoom.Properties.Items.Add("");
            for (int i = 0; i < dtStore.Rows.Count; i++)
            {
                cmbBERoom.Properties.Items.Add(dtStore.Rows[i]["LOCATION_NAME"]);
            }

            //没有不需要了
            //"4、根据工序名称和工厂车间获取设备名称数据绑定到窗体控件中，设置空为控件默认值。
            //从FMM_LOCATION c , EMS_EQUIPMENTS d ,EMS_OPERATION_EQUIPMENT e , POR_ROUTE_OPERATION_VER f表中获取。"	
									
            //5、绑定班别数据到控件，设置空为控件默认值。从CRM_ATTRIBUTE a,BASE_ATTRIBUTE b,BASE_ATTRIBUTE_CATEGORY c数据表中查询c.CATEGORY_NAME=Basic_Shift的数据。显示a.NAME=CODE对应的属性值。											
            string[] columns = new string[] { "CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Shift");
            DataTable dtShift= BaseData.Get(columns, category);
            cmbBEShift.Properties.Items.Add("");
            for (int i = 0; i < dtShift.Rows.Count; i++)
            {
                cmbBEShift.Properties.Items.Add(dtShift.Rows[i]["CODE"]);
            }

            //6、退料日期默认为当月月初到当前日期
            dateEStart.DateTime = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
            dateEEnd.DateTime = DateTime.Now;
        }

        //”确定“按钮Click事件方法
        private void cmdOK_Click(object sender, EventArgs e)
        {
            //1、根据物料批号（左匹配模糊查询），物料编码（左匹配模糊查询），物料描述（左匹配模糊查询），工序名称，线上仓名称，工厂车间，设备名称，
            //供应商（左匹配模糊查询），班次，员工号（左匹配模糊查询），退料单号
            //退料日期区间从WST_TL_ZMMLKO，WST_TL_ZMMLPO，WST_STORE_MATERIAL_DETAIL，WST_STORE_MATERIAL分页获取数据（每页默认为20行记录）。											
            //2、若工序名称为空以PROPERTY_FIELDS.OPERATIONS的值作为工序名称的查询条件，若线上仓名称为空以PROPERTY_FIELDS.STORES的值作为线上仓名称的查询条件，
            //若其他窗体栏位值为空，则不作为查询条件。	
            string strMatLot = txtEMatLot.Text ;
            string strMatCode=txtEMatCode.Text ;
            string strMatDes=txtEMatDes.Text ;
            string strOperation = cmbBEOperation.Text ;
            string strStore=cmbBEStore.Text ;
            string strFacRoom=cmbBERoom.Text ;         
            string strSupplier=txtEsupplier.Text ;
            string strShift=cmbBEShift.Text ;
            string strOperator=txtEOperator.Text ;
            string strFromRetDate = dateEStart.Text ;
            string strToRetDate = dateEEnd.Text ;
            string strRetMatNo = txtERetMatNo.Text;//1
            string strOperationALL=PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            string strStoreALL=PropertyService.Get(PROPERTY_FIELDS.STORES);
         	WOMaterialReturnEntity OMaterialReturnEntity=new WOMaterialReturnEntity();
            DataSet dsWoRetMatInof = OMaterialReturnEntity.GetWoRetMatInof( strMatLot, strMatCode, strMatDes, 
                strOperation , strStore, strFacRoom,   strSupplier, strShift, strOperator, strFromRetDate  , strToRetDate,strRetMatNo,
                strOperationALL, strStoreALL);
            // DataSet GetWoRetMatInof(string strMatLot,string strMatCode,string strMatDes,string strOperation ,string strStore,string strFacRoom, string strSupplier,string strShift,string strOperator,string strFromRetDate   ,string strToRetMat );
						
            //"3、将步骤1获取的数据显示到界面二的列表中（每页默认为20行记录）。
            //调用显示类的方法，传递dataset
            this.ParentControl.bangding(dsWoRetMatInof);

            //4、关闭查询对话框。
            this.Close();
            //5、若程序执行出错，则给出对应的错误提示。"											     
        }

        //”取消“按钮Click事件方法
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            //1、直接关闭查询对话框。
            this.Close();
        }

       
    }
}
