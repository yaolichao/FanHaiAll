using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using System.Collections;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicRptOptSetting : BaseUserCtrl
    {
        RptCommonEntity optSettingEntity = new RptCommonEntity();

        public BasicRptOptSetting()
        {
            InitializeComponent(); 
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            this.btnAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.lbl.0009}");//新增
            this.btnModify.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.lbl.0010}");//修改
            this.btnDel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.lbl.0003}");//删除

            lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.lbl.0004}");//工序排班数据维护(报表类)
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.lbl.0005}");//工厂车间
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.lbl.0006}");//工序
            btnQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.lbl.0007}");//查询
            groupControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.lbl.0008}");//工序排班列表
            LOCATION_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.GridControl.0001}");//车间
            OPERATION_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.GridControl.0002}");//工序
            SHIFT_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.GridControl.0003}");//班别
            START_TIME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.GridControl.0004}");//开始时间
            END_TIME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.GridControl.0005}");//结束时间
            OVER_DAY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.GridControl.0006}");//是否跨天
            REMARK.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.GridControl.0007}");//备注

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            InitGridView();
        }

        private void BasicRptOptSetting_Load(object sender, EventArgs e)
        {
            //绑定车间
            BindFactoryRoom();
            //绑定工序
            BindOperations();

            InitGridView();
        }

        /// <summary>
        /// 绑定车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);

            if (dt != null && dt.Rows.Count > 0)
            {
                this.lueFactoryRoom.Properties.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    string locationame = Convert.ToString(dr[FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME]);
                    string locationkey = Convert.ToString(dr[FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY]);

                    this.lueFactoryRoom.Properties.Items.Add(locationkey.Trim(), locationame);
                }
            }
            else
            {
                this.lueFactoryRoom.Properties.Items.Clear();
            }
        }

        /// <summary>
        /// 绑定工序。
        /// </summary>
        private void BindOperations()
        {

            DataSet dsOpt = optSettingEntity.GetOperation();
            cbOperation.Properties.DisplayMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME;
            cbOperation.Properties.ValueMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY;
            cbOperation.Properties.DataSource = dsOpt.Tables[0];

            ////获取登录用户拥有权限的工序名称。
            //string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            //if (operations.Length > 0)//如果有拥有权限的工序名称
            //{
            //    string[] strOperations = operations.Split(',');
            //    //遍历工序，并将其添加到窗体控件中。
            //    for (int i = 0; i < strOperations.Length; i++)
            //    {
            //        cbOperation.Properties.Items.Add(strOperations[i]);
            //    }                
            //}
        }

        private void InitGridView()
        {
            Hashtable hsParams = new Hashtable();

            //查询条件
            #region          
            if (!string.IsNullOrEmpty(Convert.ToString(lueFactoryRoom.Text)))
            {
                string[] s_array = Convert.ToString(lueFactoryRoom.Text).Split(',');
                string locationkey = string.Empty;
                foreach (string s in s_array)
                {
                    locationkey += "'" + s.Trim() + "',";
                }
                if (!string.IsNullOrEmpty(locationkey))
                {
                    locationkey = locationkey.TrimEnd(',');
                }

                hsParams.Add(BASE_OPT_SETTING.FIELDS_LOCATION_NAME, locationkey);
            }

           
            if (!string.IsNullOrEmpty(cbOperation.Text))
            {
                string[] s_array = Convert.ToString(cbOperation.Text).Split(',');
                string opts = string.Empty;
                foreach (string s in s_array)
                {
                    opts += "'" + s.Trim() + "',";
                }
                if (!string.IsNullOrEmpty(opts))
                {
                    opts = opts.TrimEnd(',');
                }

                hsParams.Add(BASE_OPT_SETTING.FIELDS_OPERATION_NAME, opts);
            }
          
            #endregion

            DataSet dsReturn = optSettingEntity.GetOptSettingData(hsParams);
            if (!string.IsNullOrEmpty(optSettingEntity.ErrorMsg))
            {
                MessageService.ShowError(optSettingEntity.ErrorMsg);
                return;
            }
            this.gcOptSetting.MainView = this.gvOptSetting;
            this.gcOptSetting.DataSource = dsReturn.Tables[BASE_OPT_SETTING.DATABASE_TABLE_NAME];
            //this.gvPlan.BestFitColumns();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (this.gvOptSetting.FocusedRowHandle > -1)
            {
                //if (MessageService.AskQuestion("确定删除选择的这笔数据么？", "提示"))
                if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}")))
                {
                    DataSet dsSave = new DataSet();
                    DataTable dtDel = (this.gcOptSetting.DataSource as DataTable).Clone();
                    DataRow dr = this.gvOptSetting.GetDataRow(this.gvOptSetting.FocusedRowHandle);
                    dr[BASE_OPT_SETTING.FIELDS_ISFLAG] = 0;
                    dtDel.Rows.Add(dr.ItemArray);
                    dtDel.TableName = BASE_OPT_SETTING.DATABASE_TABLE_FORUPDATE;

                    dsSave.Merge(dtDel, false, MissingSchemaAction.Add);
                    bool bck = optSettingEntity.SaveOptSettingData(dsSave);
                    if (!string.IsNullOrEmpty(optSettingEntity.ErrorMsg))
                    {
                        MessageService.ShowMessage(optSettingEntity.ErrorMsg);
                        return;
                    }
                    else
                    {
                        InitGridView();
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BasicRptOptForm optForm = new BasicRptOptForm();
          
            DataRow drNew = (this.gcOptSetting.DataSource as DataTable).NewRow();
            drNew[BASE_OPT_SETTING.FIELDS_OPTSETTING_KEY] = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
            drNew[BASE_OPT_SETTING.FIELDS_ISFLAG] = 1;
            drNew[BASE_OPT_SETTING.FIELDS_CREATOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            drNew[BASE_OPT_SETTING.FIELDS_OVER_DAY] = 0;
            optForm.drCommon = drNew;
            optForm.isEdit = false;

            if (optForm.ShowDialog() == DialogResult.OK)
            {
                InitGridView();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (this.gvOptSetting.FocusedRowHandle < 0)
            {
                //MessageService.ShowMessage("请选择需要编辑的数据!", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptSetting.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}"));
                
                return;
            }

            BasicRptOptForm optForm = new BasicRptOptForm();
            DataRow drUpdate = this.gvOptSetting.GetDataRow(this.gvOptSetting.FocusedRowHandle);
            drUpdate[BASE_OPT_SETTING.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            optForm.drCommon = drUpdate;
            optForm.isEdit = true;

            if (optForm.ShowDialog() == DialogResult.OK)
            {
                InitGridView();
            }
        }

      
    }
}
