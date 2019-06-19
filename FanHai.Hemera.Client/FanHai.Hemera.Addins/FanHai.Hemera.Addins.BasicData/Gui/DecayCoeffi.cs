using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class DecayCoeffi : BaseUserCtrl
    {
        DecayCoeffiEntity _decayCoeffiEntity = new DecayCoeffiEntity();
        string _decaCoeffiKey = string.Empty;
        ControlState _ctrlState = new ControlState();
        public delegate void AfterStateChanged(ControlState controlState);
        public AfterStateChanged afterStateChanged = null;
        public DecayCoeffi()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            lblMenu.Text = "基础数据 > 工艺参数设置 > 衰减率";
            GridViewHelper.SetGridView(gvDecayCoeffi);

            this.btnAdd.Text = StringParser.Parse("${res:Global.New}");//新增
            this.btnModify.Text = StringParser.Parse("${res:Global.Update}");//修改
            this.btnDelete.Text = StringParser.Parse("${res:Global.Delete}");//删除
            this.btnCancel.Text = StringParser.Parse("${res:Global.Cancel}");//取消
            this.btSave.Text = StringParser.Parse("${res:Global.Save}");//保存

            groupControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.GroupControl.0001}");//衰减设置
            D_TYPE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.GridControl.0001}");//类型
            D_CODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.GridControl.0002}");//代码
            D_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.GridControl.0003}");//名称
            D_CODE_DESC.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.GridControl.0004}");//衰减对象描述
            COEFFICIENT.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.GridControl.0005}");//衰减系数
            DIT.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.GridControl.0006}");//小数位数
        }
        //Control state property
        public ControlState CtrlState
        {
            get
            {
                return _ctrlState;
            }
            set
            {
                _ctrlState = value;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        public void onChangeControlState(ControlState cState)
        {
            switch (cState)
            {
                case ControlState.Edit:
                    this.btnAdd.Enabled = false;
                    this.btnDelete.Enabled = false;
                    this.btSave.Enabled = true;
                    this.btnCancel.Enabled = true;
                    this.gvDecayCoeffi.OptionsBehavior.Editable = true;
                    break;
                case ControlState.New:
                    this.btnDelete.Enabled = false;
                    this.btSave.Enabled = true;
                    this.btnAdd.Enabled = true;
                    this.btnCancel.Enabled = true;
                    this.gvDecayCoeffi.OptionsBehavior.Editable = true;
                    AddNewRow();
                    break;
                case ControlState.ReadOnly:
                    this.btnAdd.Enabled = true;
                    this.btnDelete.Enabled = true;
                    this.btSave.Enabled = false;
                    this.btnCancel.Enabled = false;
                    this.gvDecayCoeffi.OptionsBehavior.Editable = false;
                    InitDataBind();
                    break;
                default:
                    break;
            }
        }
      
        private void AddNewRow()
        {
            DataTable dtAddRow = ((DataView)this.gvDecayCoeffi.DataSource).Table;
            DataRow dr = dtAddRow.NewRow();
            dr[BASE_DECAYCOEFFI.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            dr[BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY] =  CommonUtils.GenerateNewKey(0);
            dr[BASE_DECAYCOEFFI.FIELDS_ISNEW] = BASE_DECAYCOEFFI.FIELDS_ISNEW;
            //dr[BASE_DECAYCOEFFI.FIELDS_DECOEFFI_TYPE] = 0;
            dtAddRow.Rows.InsertAt(dr,0);            
        }

        private void SPControlPlan_Load(object sender, EventArgs e)
        {
            afterStateChanged += new AfterStateChanged(onChangeControlState);

           DataTable dtProperties =new BasePowerSetEntity().GetBasicPowerSetEngine_CommonData(string.Empty).Tables[BASE_ATTRIBUTE_FIELDS.DATABASE_TABLE_NAME];
            DataTable dtDecaySetting = dtProperties.Clone();
            DataRow[] drs01 = dtProperties.Select(string.Format("Column_type='{0}'", BASE_POWERSET.BASE_DECAYSETTING));
            foreach (DataRow dr in drs01)
                dtDecaySetting.ImportRow(dr);
            repositoryItemlue_dname.DisplayMember = "Column_Name";
            repositoryItemlue_dname.ValueMember = "Column_code";
            repositoryItemlue_dname.DataSource = dtDecaySetting;

            InitDataBind();
            this.CtrlState = ControlState.ReadOnly;
        }

        private void InitDataBind()
        {
            DataSet dsDataBind = _decayCoeffiEntity.GetDecayCoeffiData();
            if (_decayCoeffiEntity.ErrorMsg.Equals(string.Empty))
            {
                DataTable dtGvDecayCoeffi = dsDataBind.Tables[BASE_DECAYCOEFFI.DATABASE_TABLE_NAME];
                this.gcDecayCoeffi.MainView = gvDecayCoeffi;
                this.gcDecayCoeffi.DataSource = dtGvDecayCoeffi;
                this.gvDecayCoeffi.BestFitColumns();
            }
            else
            {
                MessageService.ShowMessage(_decayCoeffiEntity.ErrorMsg);
            }
            //绑定衰减系数类型。
            DataTable dtType = new DataTable();
            dtType.Columns.Add(new DataColumn("NAME", typeof(string)));
            dtType.Columns.Add(new DataColumn("VALUE", typeof(int)));
            dtType.Rows.Add("0-按比例值", 0);
            dtType.Rows.Add("1-按目标值", 1);
            this.rilueType.DataSource = dtType;
            this.rilueType.ValueMember = "VALUE";
            this.rilueType.DisplayMember = "NAME";
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.CtrlState = ControlState.New;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            this.CtrlState = ControlState.Edit;
        }
        /// <summary>
        /// 取消作业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CtrlState = ControlState.ReadOnly;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvDecayCoeffi.FocusedRowHandle < 0 || gvDecayCoeffi.RowCount < 1)
            {
                //MessageService.ShowMessage("请选择删除的数据!", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }

            if (gvDecayCoeffi.FocusedRowHandle > -1)
            {
                //if (MessageService.AskQuestion("确定要删除衰减数据么?", "提示"))
                //{
                if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}")))
                {
                    DataTable dtDecayCoeffi = ((DataView)gvDecayCoeffi.DataSource).Table;
                    DataSet dsDecayCoeffi = new DataSet();
                    string decayCoeffiKey = gvDecayCoeffi.GetRowCellValue(gvDecayCoeffi.FocusedRowHandle, BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY).ToString();

                    DataTable dtSave = dtDecayCoeffi.Clone();
                    DataRow[] drDels = dtDecayCoeffi.Select(string.Format(BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY + "='{0}'", decayCoeffiKey));
                    if (drDels.Length > 0)
                    {
                        DataRow drDel = drDels[0];
                        drDel[BASE_DECAYCOEFFI.FIELDS_ISFLAG] = 0;
                        dtSave.ImportRow(drDel);
                        dtSave.TableName = BASE_DECAYCOEFFI.DATABASE_TABLE_FORUPDATE;
                        dsDecayCoeffi.Merge(dtSave, true, MissingSchemaAction.Add);                       

                        bool bl_Bak = _decayCoeffiEntity.SaveDecayCoeffiData(dsDecayCoeffi);
                        if (!bl_Bak)
                        {
                            MessageService.ShowMessage(_decayCoeffiEntity.ErrorMsg);
                        }
                        else
                        {
                            //MessageService.ShowMessage("删除成功!");
                            MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.msg.0003}"));//删除成功!
                            dtDecayCoeffi.Rows.Remove(drDel);
                            dtDecayCoeffi.AcceptChanges();
                            gcDecayCoeffi.DataSource = dtDecayCoeffi;
                        }

                    }
                }
            }
        }

        private bool IsValidData(DataTable dtValid)
        {
            bool bl_bak = true;
            string tableName = dtValid.TableName;
            if (BASE_DECAYCOEFFI.DATABASE_TABLE_NAME == tableName)
            {
                foreach (DataRow dr in dtValid.Rows)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(dr[BASE_DECAYCOEFFI.FIELDS_DECOEFFI_TYPE])))
                    {
                        //MessageService.ShowMessage("衰减系数类型不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.msg.0004}"));//衰减系数类型不能为空!
                        bl_bak = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr[BASE_DECAYCOEFFI.FIELDS_D_CODE].ToString()))
                    {
                        //MessageService.ShowMessage("衰减系数代码不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.msg.0005}"));//衰减系数代码不能为空!
                        bl_bak = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr[BASE_DECAYCOEFFI.FIELDS_D_NAME].ToString()))
                    {
                        //MessageService.ShowMessage("名称不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.msg.0006}"));//名称不能为空!
                        bl_bak = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr[BASE_DECAYCOEFFI.FIELDS_D_CODE_DESC].ToString()))
                    {
                        //MessageService.ShowMessage("衰减对象描述不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.msg.0007}"));//衰减对象描述不能为空!
                        bl_bak = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr[BASE_DECAYCOEFFI.FIELDS_COEFFICIENT].ToString()))
                    {
                        //MessageService.ShowMessage("衰减系数不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.msg.0008}"));//衰减系数不能为空!
                        bl_bak = false;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr[BASE_DECAYCOEFFI.FIELDS_DIT].ToString()))
                    {
                        //MessageService.ShowMessage("小数位数不能为空!");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.msg.0009}"));//小数位数不能为空!
                        bl_bak = false;
                        break;
                    }
                }
            }
            return bl_bak;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            string s = this.gvDecayCoeffi.NewItemRowText;

            DataTable dtDecayCoeffi = ((DataView)gvDecayCoeffi.DataSource).Table;
            DataTable dtDecayCoeffi_Update = dtDecayCoeffi.GetChanges(DataRowState.Modified);
            DataTable dtDecayCoeffi_Insert = dtDecayCoeffi.GetChanges(DataRowState.Added);

            DataSet dsDecayCoeffi = new DataSet();

            if (dtDecayCoeffi_Update != null && dtDecayCoeffi_Update.Rows.Count > 0)
            {
                DataTable dtUpdate = dtDecayCoeffi_Update.Clone();
                foreach (DataRow dr in dtDecayCoeffi_Update.Rows)
                {
                    DataRow[] drUpdates = dtDecayCoeffi.Select(string.Format(BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY + "='{0}'", dr[BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY].ToString()));
                    DataRow drNew=dtUpdate.NewRow();
                    for (int i = 0; i < dtUpdate.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    dtUpdate.Rows.Add(drNew);
                }
                if (!IsValidData(dtUpdate)) return;               

                dtUpdate.TableName = BASE_DECAYCOEFFI.DATABASE_TABLE_FORUPDATE;
                dsDecayCoeffi.Merge(dtUpdate, true, MissingSchemaAction.Add);
            }

            if (dtDecayCoeffi_Insert != null && dtDecayCoeffi_Insert.Rows.Count > 0)
            {
                DataTable dtInsert = dtDecayCoeffi_Insert.Clone();
                foreach (DataRow dr in dtDecayCoeffi_Insert.Rows)
                {
                    DataRow[] drUpdates = dtDecayCoeffi.Select(string.Format(BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY + "='{0}'", dr[BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY].ToString()));
                    DataRow drNew = dtInsert.NewRow();
                    for (int i = 0; i < dtInsert.Columns.Count; i++)
                    {
                        drNew[i] = drUpdates[0][i];
                    }
                    dtInsert.Rows.Add(drNew);
                }
                if (!IsValidData(dtInsert)) return;
                bool b2 = _decayCoeffiEntity.IsExistDecayCoeffiData(dtInsert);
                if (!b2 && !string.IsNullOrEmpty(_decayCoeffiEntity.ErrorMsg))
                {
                    MessageService.ShowMessage(_decayCoeffiEntity.ErrorMsg);
                    return;
                }

                dtInsert.TableName = BASE_DECAYCOEFFI.DATABASE_TABLE_FORINSERT;
                dsDecayCoeffi.Merge(dtInsert, true, MissingSchemaAction.Add);
            }

            if (dsDecayCoeffi.Tables.Count > 0)
            {
                bool bl_Bak = _decayCoeffiEntity.SaveDecayCoeffiData(dsDecayCoeffi);
                if (!bl_Bak)
                {
                    //MessageService.ShowMessage("保存失败!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.msg.0010}"));//保存失败!
                }
                else
                {
                    //MessageService.ShowMessage("保存成功!");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.DecayCoeffi.msg.0011}"));//保存成功!
                    this.CtrlState = ControlState.ReadOnly;
                }
            }
        }

        private void gvDecayCoeffi_RowClick(object sender, RowClickEventArgs e)
        {
            if (((GridView)sender).GetRow(e.RowHandle) != null)
            {
                _decaCoeffiKey = ((DataRowView)(((GridView)sender).GetRow(e.RowHandle))).Row[BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY].ToString();
            }
        }

        private void gvDecayCoeffi_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_decaCoeffiKey.Trim()))
            {
                try
                {
                    DataTable dtMain = ((DataView)gvDecayCoeffi.DataSource).Table;
                    DataRow[] drs = dtMain.Select(string.Format(BASE_DECAYCOEFFI.FIELDS_DECOEFFI_KEY + "='{0}'", _decaCoeffiKey.Trim()));
                    drs[0][e.Column.FieldName] = e.Value;
                }
                catch //(Exception ex) 
                { }
            }
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
