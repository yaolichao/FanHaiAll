using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicTestRule : BaseUserCtrl
    {
        DecayCoeffiEntity _decayCoeffiEntity = new DecayCoeffiEntity();
        BaseTestRuleEntity _baseTestRuleEntity = new BaseTestRuleEntity();
        DataTable dtDecay = new DataTable();
        private string _baseTestRule_Key = string.Empty;

        string _loadKey = string.Empty;
        public BasicTestRule()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            lblMenu.Text = "基础数据 > 工艺参数设置 > 效率管理";
            GridViewHelper.SetGridView(gvDecayDtl);
            GridViewHelper.SetGridView(gvTestRule);

            this.btnAdd.Text = StringParser.Parse("${res:Global.New}");//新增
            this.btnModify.Text = StringParser.Parse("${res:Global.Update}");//修改
            this.btnDelete.Text = StringParser.Parse("${res:Global.Delete}");//删除
            
            layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.lbl.0002}");//规则代码
            layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.lbl.0003}");//规则名称
            layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.lbl.0004}");//分档规则
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.lbl.0005}");//创建日期
            btnQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.btn.0001}");//查询

            groupControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.lbl.0006}");//测试规则
            TESTRULE_CODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.GridControl.0001}");//测试规则代码
            TESTRULE_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.GridControl.0002}");//测试规则名称
            MEMO.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.GridControl.0003}");//备注
            PS_CODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.GridControl.0004}");//功率分档
            POWER_DEGREE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.GridControl.0005}");//功率精度
            LAST_TEST_TYPE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.GridControl.0006}");//终检类型
            FULL_PALLET_QTY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.GridControl.0007}");//包装满托数
            CREATE_TIME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.GridControl.0008}");//创建时间

            groupControl2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.lbl.0007}");//衰减明细
            DECAY_SQL.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.GridControl.0009}");//序号
            DECOEFFI_KEY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.GridControl.0010}");//衰减系数
            DECAY_POWER_MIN.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.GridControl.0011}");//最小功率
            DECAY_POWER_MAX.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.GridControl.0012}");//最大功率
        }

        private void BasicTestRule_Load(object sender, EventArgs e)
        {
            gvTestRule.OptionsBehavior.Editable = false;
            gvDecayDtl.OptionsBehavior.Editable = false;
            BindDecayLueData();
            InitDataBind();
        }
        private void BindDecayLueData()
        {
            DataSet dsDataBind = _decayCoeffiEntity.GetDecayCoeffiData();
            if (_decayCoeffiEntity.ErrorMsg.Equals(string.Empty))
            {
                DataTable dtGvDecayCoeffi = dsDataBind.Tables[BASE_DECAYCOEFFI.DATABASE_TABLE_NAME];
                this.repositoryItemLookUpEdit_decoeffi.DisplayMember = BASE_DECAYCOEFFI.FIELDS_D_CODE;
                this.repositoryItemLookUpEdit_decoeffi.ValueMember = BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY;
                this.repositoryItemLookUpEdit_decoeffi.DataSource = dtGvDecayCoeffi;   
             
            }
            else
                MessageService.ShowMessage(_decayCoeffiEntity.ErrorMsg);

        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            InitDataBind();
        }
        private void InitDataBind()
        {
            //规则代码，规则名称，分档规则，创建日期          
            Hashtable hashTable = new Hashtable();
            if (!string.IsNullOrEmpty(txtTestRule_Code.Text.Trim()))
                hashTable[BASE_TESTRULE.FIELDS_TESTRULE_CODE] = txtTestRule_Code.Text.Trim();
            if (!string.IsNullOrEmpty(txtTestRule_Name.Text.Trim()))
                hashTable[BASE_TESTRULE.FIELDS_TESTRULE_NAME] = txtTestRule_Name.Text.Trim();
            if (!string.IsNullOrEmpty(txtPs_Code.Text.Trim()))
                hashTable[BASE_TESTRULE.FIELDS_PS_CODE] = txtPs_Code.Text.Trim();
            if (!string.IsNullOrEmpty(dateStart.Text.Trim()))
                hashTable[BASE_TESTRULE.FIELDS_CREATE_TIME_START] = dateStart.Text.Trim();
            if (!string.IsNullOrEmpty(dateEnd.Text.Trim()))
                hashTable[BASE_TESTRULE.FIELDS_CREATE_TIME_END] = dateEnd.Text.Trim();


            DataSet dsDataBind = _baseTestRuleEntity.GetTestRuleMainData(hashTable);
            if (_baseTestRuleEntity.ErrorMsg.Equals(string.Empty))
            {
                dtDecay = dsDataBind.Tables[BASE_TESTRULE_DECAY.DATABASE_TABLE_NAME];
                gcDecayDtl.DataSource = null;

                DataTable dtTestRule = dsDataBind.Tables[BASE_TESTRULE.DATABASE_TABLE_NAME];
                this.gcTestRule.MainView = gvTestRule;
                this.gcTestRule.DataSource = dtTestRule;
                this.gvTestRule.BestFitColumns();

                if (!string.IsNullOrEmpty(_loadKey))
                {
                    for (int i = 0; i <gvTestRule.RowCount; i++)
                    {
                        string sk = Convert.ToString(((DataRowView)(this.gvTestRule.GetRow(i))).Row[BASE_TESTRULE.FIELDS_TESTRULE_KEY]);
                        if (_loadKey.Equals(sk.Trim()))
                        {
                            this.gvTestRule.FocusedRowHandle = i;
                            break;
                        }
                    }
                }

                BindGvDtl();
            }
            else
                MessageService.ShowMessage(_baseTestRuleEntity.ErrorMsg);
        }

        private void gvTestRule_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            BindGvDtl();
        }

        private void BindGvDtl()
        {
            try
            {
                if (gvTestRule.FocusedRowHandle > -1)
                {
                    _baseTestRule_Key = this.gvTestRule.GetRowCellValue(gvTestRule.FocusedRowHandle, BASE_TESTRULE.FIELDS_TESTRULE_KEY).ToString().Trim();
                    DataTable dtCommonSetDtl = dtDecay.Clone();
                    DataRow[] drDecays = dtDecay.Select(string.Format(BASE_TESTRULE.FIELDS_TESTRULE_KEY + "='{0}'", _baseTestRule_Key));

                    foreach (DataRow dr in drDecays)
                        dtCommonSetDtl.ImportRow(dr);

                    this.gcDecayDtl.DataSource = null;
                    this.gcDecayDtl.DataSource = dtCommonSetDtl;
                }
            }
            catch //(Exception ex) 
            { }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvTestRule.FocusedRowHandle < 0 || gvTestRule.RowCount < 1)
            {
                //MessageService.ShowMessage("请选择删除的数据!", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }

            //if (MessageService.AskQuestion("确认删除数据么?", "提示"))
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                //删除处理
                DataSet dsTestRule = new DataSet();
                DataTable dtTestRule = ((DataView)gvTestRule.DataSource).Table;
                string pk = gvTestRule.GetRowCellValue(gvTestRule.FocusedRowHandle, BASE_TESTRULE.FIELDS_TESTRULE_KEY).ToString();
                DataRow[] drTestRules = dtTestRule.Select(string.Format(BASE_TESTRULE.FIELDS_TESTRULE_KEY + "='{0}'", pk));
                DataTable dtDel = dtTestRule.Clone();
                DataRow drDel = drTestRules[0];
                drDel[BASE_TESTRULE.FIELDS_ISFLAG] = 0;
                dtDel.ImportRow(drDel);
                dtDel.TableName = BASE_TESTRULE.DATABASE_TABLE_FORUPDATE;
                dsTestRule.Merge(dtDel, true, MissingSchemaAction.Add);
                bool bl_bak = _baseTestRuleEntity.SavePowerSetData(dsTestRule);
                if (!bl_bak)
                {
                    //MessageService.ShowMessage("删除失败!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));
                }
                else
                {
                    //MessageService.ShowMessage("删除成功!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    InitDataBind();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BasicTestRuleForm btrf = new BasicTestRuleForm();
            btrf.isEdit = false;

            if (DialogResult.OK == btrf.ShowDialog())
            {
                this._loadKey = btrf.testRule_key;
                InitDataBind();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (gvTestRule.FocusedRowHandle < 0 || gvTestRule.RowCount < 1)
            {
                //MessageService.ShowMessage("请选择修改的数据!", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicTestRule.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            DataTable dtEdit = ((DataView)this.gvTestRule.DataSource).Table;
            string pk = gvTestRule.GetRowCellValue(gvTestRule.FocusedRowHandle, BASE_TESTRULE.FIELDS_TESTRULE_KEY).ToString();
            this._loadKey = pk;
            BasicTestRuleForm btrf = new BasicTestRuleForm();
            btrf.testRule_key = pk;
            btrf.isEdit = true;
            if (DialogResult.OK == btrf.ShowDialog())
            {
                InitDataBind();
            }            
        }

        private void gvTestRule_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvDecayDtl_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
