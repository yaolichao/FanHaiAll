using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.UDA;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Grid;

namespace FanHai.Hemera.Addins.BasicData.Gui
{
    public partial class LineSetting : UserControl
    {
        LineSettingEntity lineSettingEntity = new LineSettingEntity();

        private delegate void AfterStateChanged(ControlState controlState);
        private AfterStateChanged afterStateChanged = null;
        private ControlState _controlState = ControlState.Empty;

        private DataTable _dtSubLine = null;

        private string mainLineKey = string.Empty;

        public LineSetting()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            this.btnSave.Text = StringParser.Parse("${res:Global.Save}");//保存
            this.btnModify.Text = StringParser.Parse("${res:Global.Update}");//修改
            this.btnCancel.Text = StringParser.Parse("${res:Global.Cancel}");//取消
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");//查询

            lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.lbl.0001}");//线别维护
            xtpLineBaseInfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.lbl.0002}");//基本信息
            lciLineCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.lbl.0003}");//线别代码
            lciLineName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.lbl.0004}");//线别名称
            lciLineDescription.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.lbl.0005}");//线别描述
            xtpSubLineInfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.lbl.0006}");//子线信息
            sbtnSubLineAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.lbl.0007}");//新增
            sbtnDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.lbl.0008}");//删除
            gcSubLineKey.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.GridControl.0001}");//线别代码
            gcSubLineName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.GridControl.0002}");//线别名称
            gcOperationName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.GridControl.0003}");//工序名称
        }

        #region State Change
        private ControlState State
        {
            get
            {
                return _controlState;
            }
            set
            {
                _controlState = value;

                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        /// <summary>
        /// Deal with state change event
        /// </summary>
        /// <param name="state"></param>
        private void OnAfterStateChanged(ControlState controlState)
        {
            switch (controlState)
            {

                case ControlState.ReadOnly:
                    this.btnModify.Enabled = true;
                    this.btnSave.Enabled = false;
                    this.btnCancel.Enabled = false;

                    this.sbtnSubLineAdd.Enabled = false;
                    this.sbtnDelete.Enabled = false;

                    this.gvSubLine.OptionsBehavior.ReadOnly = true;

                    break;
                case ControlState.Edit:
                    this.btnModify.Enabled = false;
                    this.btnSave.Enabled = true;
                    this.btnCancel.Enabled = true;

                    this.sbtnSubLineAdd.Enabled = true;
                    this.sbtnDelete.Enabled = true;

                    this.gvSubLine.OptionsBehavior.ReadOnly = false;
                    break;
            }
        }

        #endregion


        /// <summary>
        /// 界面加载时加载事件
        /// </summary>
        private void LineSetting_Load(object sender, EventArgs e)
        {
            afterStateChanged += new AfterStateChanged(OnAfterStateChanged);

            //绑定工序下拉数据
            BindOperationList();

            //绑定子线下拉数据
            BindSubLineList();

            //绑定子线别信息
            GetSubLineInfo(string.Empty);
        }

        /// <summary>
        /// 线别下拉绑定
        /// </summary>
        private void BindSubLineList()
        {
            string userName = string.Empty;

            DataSet dsSubLine = null;

            try
            {
                userName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);

                dsSubLine = lineSettingEntity.GetLineByUserNameAndLineName(userName, string.Empty);

                if (string.IsNullOrEmpty(lineSettingEntity.ErrorMsg))
                {
                    //绑定子线下拉数据

                    DataTable dtSubLineBind = dsSubLine.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME];


                    riglueSubLine.DisplayMember = FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_CODE;
                    riglueSubLine.ValueMember = FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY;
                    riglueSubLine.DataSource = dtSubLineBind;
                }

            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }

        /// <summary>
        /// 工序信息绑定
        /// </summary>
        private void BindOperationList()
        {
            string operationName = string.Empty;
            string userName = string.Empty;

            DataSet dsOperation = null;

            try
            {
                userName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);

                dsOperation = lineSettingEntity.GetOperationByUserNameAndOperationName(userName, string.Empty);

                if (string.IsNullOrEmpty(lineSettingEntity.ErrorMsg))
                {
                    //绑定工序下拉数据

                    DataTable dtOperationBind = dsOperation.Tables[RBAC_ROLE_OWN_OPERATION_FIELDS.DATABASE_TABLE_NAME];

                    rilueOperationName.DisplayMember = RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_OPERATION_NAME;
                    rilueOperationName.ValueMember = RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_OPERATION_NAME;
                    rilueOperationName.DataSource = dtOperationBind;
                }

            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }

        /// <summary>
        /// 绑定主线别信息到对应的栏位
        /// </summary>
        /// <param name="dr">包含主线信息的行</param>
        private void BindMainLineInfo(DataRow dr)
        {
            teLineCode.Text = dr[FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_CODE].ToString();
            teLineName.Text = dr[FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME].ToString();
            meLineDescription.Text = dr[FMM_PRODUCTION_LINE_FIELDS.FIELD_DESCRIPTIONS].ToString();
        }

        /// <summary>
        /// 获取主线对应的子线别信息
        /// </summary>
        /// <param name="mainLineKey">主线主键</param>
        private void GetSubLineInfo(string mainLineKey)
        {
            DataSet dsSubLine = lineSettingEntity.GetSubLineByLineKey(mainLineKey);

            if (string.IsNullOrEmpty(lineSettingEntity.ErrorMsg))
            {
                _dtSubLine = dsSubLine.Tables[FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_NAME];
            }

            //绑定子线别信息
            BindSubLineInfo();
        }

        /// <summary>
        /// 绑定子线别信息到对应的视图
        /// </summary>
        /// <param name="dt">包含子线别信息的表</param>
        private void BindSubLineInfo()
        {
            this.gcSubLine.MainView = gvSubLine;
            this.gcSubLine.DataSource = null;
            this.gcSubLine.DataSource = _dtSubLine;
        }

        /// <summary>
        /// 线别下拉选择后出发 线别主键 名称更新
        /// </summary>
        private void riglueSubLine_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            if (this.gvSubLine.State == GridState.Editing && this.gvSubLine.IsEditorFocused)
            {
                string newSubLineKey = Convert.ToString(e.Value);
                string currenSubLineKey = Convert.ToString(this.gvSubLine.EditingValue);
                if (newSubLineKey != currenSubLineKey)
                {
                    DataRow dr = ((DataTable)riglueSubLine.DataSource).Select(string.Format(" PRODUCTION_LINE_KEY ='{0}' ", newSubLineKey))[0];
                    gvSubLine.SetFocusedRowCellValue("PRODUCTION_LINE_SUB_KEY", newSubLineKey);
                    gvSubLine.SetFocusedRowCellValue("LINE_NAME", dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_LINE_NAME]);
                }
            }
            else
            {
                e.Value = string.Empty;
            }

            this.gvSubLine.UpdateCurrentRow();
        }

        /// <summary>
        /// 工序下拉选择 后触发
        /// </summary>
        private void rilueOperationName_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            if (this.gvSubLine.State == GridState.Editing && this.gvSubLine.IsEditorFocused)
            {
                string newOperationName = Convert.ToString(e.Value);
                string currenOperationName = Convert.ToString(this.gvSubLine.EditingValue);
                if (newOperationName != currenOperationName)
                {
                    //获取选中行的子线主键
                    string subLineKey = gvSubLine.GetFocusedRowCellValue("PRODUCTION_LINE_SUB_KEY").ToString();

                    //获取当前行对应的线别工序信息是否存在设定
                    DataRow[] drs = ((DataTable)gcSubLine.DataSource).Select(string.Format(" PRODUCTION_LINE_SUB_KEY ='{0}' AND OPERATION_NAME = '{1}' ", subLineKey, newOperationName));

                    //判断同工序同线别的设定是否存在
                    if (drs.Length == 0)
                    {
                        gvSubLine.SetFocusedRowCellValue("OPERATION_NAME", newOperationName);
                    }
                    else
                    {
                        e.Value = string.Empty;
                        //MessageBox.Show(string.Format("不允许存在线别【{0}】,工序【{1}】相同的设定", drs[0][FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_LINE_NAME], newOperationName));
                        MessageBox.Show(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.msg.0001}"), drs[0][FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_LINE_NAME], newOperationName));
                    }
                }
            }
            else
            {
                e.Value = string.Empty;
            }

            this.gvSubLine.UpdateCurrentRow();
        }

        /// <summary>
        /// 查询选择要进行设定的线别信息
        /// </summary>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            LineSettingForm lineSettingForm = new LineSettingForm();
            if (DialogResult.OK == lineSettingForm.ShowDialog())
            {
                mainLineKey = lineSettingForm.drLine[FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY].ToString();

                //绑定主线信息
                BindMainLineInfo(lineSettingForm.drLine);

                //通过主线主键获取对应的子线别信息
                GetSubLineInfo(mainLineKey);

                //设置控件状态为只读
                this.State = ControlState.ReadOnly;
            }
        }

        /// <summary>
        /// 设定控件状态为可修改
        /// </summary>
        private void btnModify_Click(object sender, EventArgs e)
        {
            this.State = ControlState.Edit;
        }

        /// <summary>
        /// 撤销修改部分的操作
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            GetSubLineInfo(mainLineKey);
            this.State = ControlState.ReadOnly;
        }

        /// <summary>
        /// 对信息进行保存
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //检查信息是否完整
            if (!IsValidData())
            {
                return;
            }

            DataSet dsLine = new DataSet();

            #region 对子线别信息进行更新
            
            DataTable _dtSubLine_Insert = _dtSubLine.GetChanges(DataRowState.Added);
            DataTable _dtSubLine_Update = _dtSubLine.GetChanges(DataRowState.Modified);
            DataTable _dtSubLine_Delete = _dtSubLine.GetChanges(DataRowState.Deleted);


            if (_dtSubLine_Insert != null && _dtSubLine_Insert.Rows.Count > 0)
            {
                
                //遍历把更新的信息写入到临时表dtInsert中
                foreach (DataRow dr in _dtSubLine_Insert.Rows)
                {
                    //对需要进行更新的列信息进行补充更新
                    dr["IS_USED"] = "Y";
                    dr["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                }
                _dtSubLine_Insert.TableName = FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_FORINSERT;
                dsLine.Merge(_dtSubLine_Insert, true, MissingSchemaAction.Add);
            }

            if (_dtSubLine_Update != null && _dtSubLine_Update.Rows.Count > 0)
            { 
                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtSubLine_Update.Rows)
                {  
                    //对需要进行更新的列信息进行补充更新
                    dr["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                }
                _dtSubLine_Update.TableName = FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_FORUPDATE;
                dsLine.Merge(_dtSubLine_Update, true, MissingSchemaAction.Add);
            }

            if (_dtSubLine_Delete != null && _dtSubLine_Delete.Rows.Count > 0)
            {
                _dtSubLine_Delete.RejectChanges();

                //遍历把更新的信息写入到临时表dtUpdate中
                foreach (DataRow dr in _dtSubLine_Delete.Rows)
                {
                    //对需要进行更新的列信息进行补充更新
                    dr["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                }
                _dtSubLine_Delete.TableName = FMM_PRODUCTION_LINE_SUB_FIELDS.DATABASE_TABLE_FORDELETE;
                dsLine.Merge(_dtSubLine_Delete, true, MissingSchemaAction.Add);
            }

            #endregion

            lineSettingEntity.SaveLineInfo(dsLine);

            if (string.IsNullOrEmpty(lineSettingEntity.ErrorMsg))
            {
                //通过主线主键获取对应的子线别信息
                GetSubLineInfo(mainLineKey);

                //设置控件状态为只读
                this.State = ControlState.ReadOnly;
            }
            else 
            {
                //MessageBox.Show("保存失败！");
                 MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.msg.0002}"));
                
            }
        }

        private bool IsValidData()
        { 
            //对产品信息进行判断
            for (int i = 0; i < gvSubLine.RowCount; i++)
            { 
                if (_dtSubLine.Rows[i].RowState != DataRowState.Deleted && _dtSubLine.Rows[i].RowState != DataRowState.Detached)
                {
                    string subLineKey = _dtSubLine.Rows[i]["PRODUCTION_LINE_SUB_KEY"].ToString();
                    string operationName = _dtSubLine.Rows[i]["OPERATION_NAME"].ToString();

                    if (string.IsNullOrEmpty(subLineKey))
                    {
                        //MessageService.ShowMessage("线别名称不能为空！");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.msg.0003}"));
                        return false;
                    }

                    if (string.IsNullOrEmpty(operationName))
                    {
                        //MessageService.ShowMessage("工序名称不能为空！");
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.LineSetting.msg.0004}"));
                        return false;
                    }
                    
                }
            }

            return true;
        }


        /// <summary>
        /// 子线别新增
        /// </summary>
        private void sbtnSubLineAdd_Click(object sender, EventArgs e)
        {
            DataRow dr = _dtSubLine.NewRow();

            dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_SUB_LINE_KEY] = CommonUtils.GenerateNewKey(0);
            dr[FMM_PRODUCTION_LINE_SUB_FIELDS.FIELDS_PRODUCTION_LINE_KEY] = mainLineKey;

            _dtSubLine.Rows.Add(dr);
        }

        /// <summary>
        /// 子线别删除
        /// </summary>
        private void sbtnDelete_Click(object sender, EventArgs e)
        {
            if (this.gvSubLine.FocusedRowHandle > -1)
            {
                DataRow dr = this.gvSubLine.GetFocusedDataRow();
                dr.Delete();
            }
        }



    }
}
