//查询数据采集，导出数据
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
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Hemera.Utils.StaticFuncs;

namespace FanHai.Hemera.Addins.EAP
{
    public partial class EDCQueryexpCtrl : BaseUserCtrl
    {
        public EDCQueryexpCtrl()
        {
            InitializeComponent();
        }

        private void EDCQueryexpCtrl_Load(object sender, EventArgs e)
        {
            this.deStartDate.DateTime = DateTime.Now;
            this.deEndDate.DateTime = DateTime.Now.AddDays(1);
            BingEdcPoint();
            BingEdcEMS();
            BingEdcParam();
        }

        /// <summary>
        /// 所有抽检点
        /// </summary>
        private void BingEdcPoint()
        {
            DataSet ds = new DataSet();
            EdcQuery edcQuery = new EdcQuery();
            ds =edcQuery.SearchEdcPoint(); //查询抽检点
            luePoint.Properties.DataSource = ds.Tables[0];
            luePoint.Properties.ValueMember = "GROUP_KEY";
            luePoint.Properties.DisplayMember = "EDC_NAME";
        }

        /// <summary>
        /// 按照车间绑定所有设备
        /// </summary>
        private void BingEdcEMS()
        {
            //车间
            string strFactoryRoom = PropertyService.Get(PROPERTY_FIELDS.FACTORY_CODE);
            //strFactoryRoom="C3C1";
            DataSet ds = new DataSet();
            EdcQuery edcQuery = new EdcQuery();
            ds = edcQuery.SearchEMS(strFactoryRoom);
            lueEqp.Properties.DataSource  = ds.Tables[0];
            lueEqp.Properties.ValueMember = "EQUIPMENT_KEY";
            lueEqp.Properties.DisplayMember = "EQUIPMENT_NAME";
        }
        /// <summary>
        /// 绑定所有抽检参数
        /// </summary>
        private void BingEdcParam()
        {
            DataSet ds = new DataSet();
            EdcQuery edcQuery = new EdcQuery();
            ds = edcQuery.SearchParam();
            lueParam.Properties.DataSource = ds.Tables[0];
            lueParam.Properties.ValueMember = "PARAM_KEY";
            lueParam.Properties.DisplayMember = "PARAM_NAME";
        }

        /// <summary>
        /// 根据抽检点设置变化变更车间和设备，车间的设备和这个抽检点的设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void luePoint_EditValueChanged(object sender, EventArgs e)
        {
            //1.按照车间绑定所有设备,group_Key ,equipment_key
            //2.如果，equipment_key有值则直接用需加车间，没有就根据group_key和车间
            string strFactoryRoom = PropertyService.Get(PROPERTY_FIELDS.FACTORY_CODE);
            //strFactoryRoom = "C3C1";

            EdcQuery edcQuery = new EdcQuery();
            string equipmentKey = this.luePoint.GetColumnValue("EQUIPMENT_KEY").ToString();
            string groupKey = this.luePoint.GetColumnValue("GROUP_KEY").ToString();
            DataSet dsEMS = edcQuery.SearchEMS(strFactoryRoom, groupKey, equipmentKey);

            lueEqp.Properties.DataSource = dsEMS.Tables[0];
            lueEqp.Properties.ValueMember = "EQUIPMENT_KEY";
            lueEqp.Properties.DisplayMember = "EQUIPMENT_NAME";

           
            // 3.根据选择的DEC_key获取
            string edcKey = this.luePoint.GetColumnValue("EDC_KEY").ToString();
            DataSet dsParma = edcQuery.SearchParam(edcKey);
            lueParam.Properties.DataSource = dsParma.Tables[0];
            lueParam.Properties.ValueMember = "PARAM_KEY";
            lueParam.Properties.DisplayMember = "PARAM_NAME";
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
        /// 根据条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbQuery_Click(object sender, EventArgs e)
        {
            Hashtable hashTable = new Hashtable();
            if (luePoint.Text == string.Empty)
            {
                MessageBox.Show("请选择抽检抽点");
                return;
            }
            string rowKey = this.luePoint.GetColumnValue("ROW_KEY").ToString();          
           
            //时间
            hashTable.Add("ROW_KEY", rowKey);
              if (!string.IsNullOrEmpty(deStartDate.Text))
            {
                hashTable.Add("START_DATE", deStartDate.Text );
            }
            if (!string.IsNullOrEmpty(deStartDate.Text))
            {
                hashTable.Add("END_DATE", deEndDate.Text );
            }

            if (!string.IsNullOrEmpty(lueEqp.Text ))
            {
                hashTable.Add("EQUIPMENT_NAME", lueEqp.Text);
            }
            if (!string.IsNullOrEmpty(lueParam.Text))
            {
                hashTable.Add("PARAM_NAME", lueParam.Text);
            }

            DataTable dtParams = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
            EdcQuery edcQuery = new EdcQuery();
            DataSet ds = edcQuery.EDCValueQuery(dtParams);
            gcPointValue.MainView  = gvPointValue;
            gcPointValue.DataSource  = ds.Tables[0];
            gvPointValue.BestFitColumns();
                        
        }

        private void tsbExp_Click(object sender, EventArgs e)
        {
            DataTable table = gcPointValue.DataSource as DataTable;

            string[] title = { "抽检时间", "站点", "机台",  "参数", "序号","值" };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //获得文件路径
                string strinlocalFilePath = saveFileDialog1.FileName.ToString();

                //获取文件名，不带路径
                string fileNameExt = strinlocalFilePath.Substring(strinlocalFilePath.LastIndexOf("\\") + 1);
                //获取文件路径，不带文件名
                string FilePath = strinlocalFilePath.Substring(0, strinlocalFilePath.LastIndexOf("\\"));
                string newFileName = fileNameExt + DateTime.Now.ToString("yyyyMMdd");
                
                SaveToExcel.SaveExcel(table, newFileName, FilePath, "抽检点数据", title);
                
            }
            else
            {
                //这里放对取消的处理
            }
        }
        private void pntcPoint_Load(object sender, EventArgs e)
        {

        }
    }
}
