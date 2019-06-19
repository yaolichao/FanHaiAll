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
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities.BasicData;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.BasicData
{
    /// <summary>
    /// 控件类。
    /// </summary>
    public partial class MaterialBuckleControlCtrl : BaseUserCtrl
    {
        MaterialBuckleControlEntity materialBuckleControlEntityEntity = new MaterialBuckleControlEntity();
        public string _status = "EMPTY";
        /// <summary>设定状态绑定控件
        /// 设定状态绑定控件
        /// </summary>
        /// <param name="_status"></param>
        private void Status(string _status)
        {
            if (_status.Equals("EMPTY"))  //保存新建删除相同状态
            {
                BtnAdd.Enabled = true;
                BtnSave.Enabled = true;
                BtnDelete.Enabled = false;

                txtUseQty.Text = "";
                txtUseConrtastQty.Text = "";

                BindParameter();
                BindSelectParameter();
                BindUnit();

                DataSet dataSet = materialBuckleControlEntityEntity.GetInfByParameter("ALL");
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    dataSet.Tables[0].Rows[i]["ROWNUMBER"] = i + 1;
                }
                grdCrtlCode.MainView = gridViewCode;
                grdCrtlCode.DataSource = dataSet.Tables[0];
                gridViewCode.BestFitColumns();
                lueParameter.Properties.ReadOnly = false;
            }
            if (_status.Equals("EDIT"))   //修改时状态
            {
                BtnAdd.Enabled = true;
                BtnSave.Enabled = true;
                BtnDelete.Enabled = true;

                lueParameter.Properties.ReadOnly = true;
            }
            if (_status.Equals("SELECT")) //查询状态
            {
                BtnAdd.Enabled = true;
                BtnSave.Enabled = false;
                BtnDelete.Enabled = false;
                lueParameter.Properties.ReadOnly = false;
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public MaterialBuckleControlCtrl()
        {
            InitializeComponent();
            InitializeLanguage();
            GridViewHelper.SetGridView(gridViewCode);
        }
        public void InitializeLanguage()
        {
            lciCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.lbl.0002}");//参数名称
            btnQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.lbl.0003}");//查询
            lblCodeName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.lbl.0004}");//参数名称
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.lbl.0005}");//扣料量
            lblCodeDescription.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.lbl.0006}");//单位
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.lbl.0007}");//对应扣料量
            layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.lbl.0008}");//对应扣料单位

            gcNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.GridControl.0001}");//序号
            gcParameter.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.GridControl.0002}");//参数名称
            gcUseQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.GridControl.0003}");//扣料数量
            gcUseUnit.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.GridControl.0004}");//扣料单位
            gcUseConrtastQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.GridControl.0005}");//对应扣料量
            gcUseConrtastUnit.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.GridControl.0006}");//对应扣料单位
            gcCreator.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.GridControl.0007}");//创建人
            gcCreateTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.GridControl.0008}");//创建时间
            gcEditer.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.GridControl.0009}");//修改人
            gcEditTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.GridControl.0010}");//修改时间
        }
        
        //  关闭窗体
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            _status = "EMPTY";
            Status(_status);
        }

        private void tsbDel_Click(object sender, EventArgs e)
        {
            //if (MessageService.AskQuestion("确定删除吗?", "系统提示"))
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {//系统提示你确定要删除吗？
                if (materialBuckleControlEntityEntity.DeleteInf(this.lueParameter.Text.Trim()))
                {
                    _status = "EMPTY";
                    Status(_status);
                }
            }
        }
        /// <summary>
        /// 行单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewCode_RowClick(object sender, RowClickEventArgs e)
        {
            _status = "EDIT";
            Status(_status);
            this.lueParameter.EditValue = this.gridViewCode.GetRowCellValue(e.RowHandle, "PARAMETER").ToString();
            this.txtUseQty.Text = this.gridViewCode.GetRowCellValue(e.RowHandle, "USE_QTY").ToString();
            this.txtUseUnit.EditValue = this.gridViewCode.GetRowCellValue(e.RowHandle, "USE_UNIT").ToString();
            this.txtUseConrtastQty.Text = this.gridViewCode.GetRowCellValue(e.RowHandle, "USE_CONRTAST_QTY").ToString();
            this.txtUseConrtastUnit.EditValue = this.gridViewCode.GetRowCellValue(e.RowHandle, "USE_CONRTAST_UNIT").ToString();
        }
        /// <summary>根据参数名称查询扣料对应关系记录
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataSet dataSet = new DataSet();
            _status = "SELECT";
            Status(_status);
            ///根据参数名称查询扣料对应关系记录
            dataSet = materialBuckleControlEntityEntity.GetInfByParameter(lueSelectParameter.Text.Trim());
            if (dataSet != null)
            {
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    dataSet.Tables[0].Rows[i]["ROWNUMBER"] = i + 1;
                }
                grdCrtlCode.MainView = gridViewCode;
                grdCrtlCode.DataSource = dataSet.Tables[0];
                gridViewCode.BestFitColumns();
            }
           
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            bool IsTrue = false;
            //if (MessageService.AskQuestion("你确定要保存当前界面的数据吗？", "保存"))
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}")))
            
            {
                if (string.IsNullOrEmpty(lueParameter.Text.Trim()))
                {
                    //MessageService.ShowMessage("参数不为空", "保存");  
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));    
                    return;
                }

                if (string.IsNullOrEmpty(txtUseQty.Text.Trim()))
                {
                    //MessageService.ShowMessage("扣料数量不为空", "保存");   
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"));   
                    return;
                }
                if (string.IsNullOrEmpty(txtUseUnit.Text.Trim()))
                {
                    //MessageService.ShowMessage("扣料单位不为空", "保存");   
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}"));   
                    return;
                }
                if (string.IsNullOrEmpty(txtUseConrtastQty.Text.Trim()))
                {
                    //MessageService.ShowMessage("扣料对照数量不为空", "保存");   
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.msg.0006}"), StringParser.Parse("${res:Global.SystemInfo}"));    
                    return;
                }
                if (string.IsNullOrEmpty(txtUseConrtastUnit.Text.Trim()))
                {
                    //MessageService.ShowMessage("扣料对照数量单位不为空", "保存");    
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialBuckleControlCtrl.msg.0007}"), StringParser.Parse("${res:Global.SystemInfo}"));   
                    return;
                }
                string parameter = lueParameter.Text.Trim();
                string useqty = txtUseQty.Text.Trim();
                string useunit = txtUseUnit.Text.Trim();
                string conrtastQty = txtUseConrtastQty.Text.Trim();
                string conrtastUnt = txtUseConrtastUnit.Text.Trim();
                string name = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
                if (_status.Equals("EMPTY"))
                {//状态为new
                    if (materialBuckleControlEntityEntity.InsertNewInf(parameter, useqty, useunit, conrtastQty, conrtastUnt, name))
                    {//新增成功
                        IsTrue = true;
                    }
                }
                else
                {//状态不为new
                    if (materialBuckleControlEntityEntity.UpdateParameterInf(parameter, useqty, useunit, conrtastQty, conrtastUnt,name))
                    {//修改成功
                        IsTrue = true;
                    }                
                }
                if (IsTrue)
                {//值为true
                    _status = "EMPTY";
                    Status(_status);
                }
            }
        }

        private void SupplierCtrl_Load(object sender, EventArgs e)
        {
            _status = "EMPTY";
            Status(_status);
        }
        private void BindParameter()
        {
            ///获取参数值
            DataSet dsParameter = materialBuckleControlEntityEntity.GetParameter();

            lueParameter.Properties.DataSource = dsParameter.Tables[0];
            this.lueParameter.Properties.DisplayMember = "PARAM_NAME";
            this.lueParameter.Properties.ValueMember = "PARAM_NAME";
            this.lueParameter.ItemIndex = 0;
        }
        private void BindSelectParameter()
        {
            ///获取参数值
            DataSet dsParameter = materialBuckleControlEntityEntity.GetParameter();
            DataRow dr = dsParameter.Tables[0].NewRow();
            dr["PARAM_NAME"] = "ALL";
            dsParameter.Tables[0].Rows.InsertAt(dr, 0);
            lueSelectParameter.Properties.DataSource = dsParameter.Tables[0];
            this.lueSelectParameter.Properties.DisplayMember = "PARAM_NAME";
            this.lueSelectParameter.Properties.ValueMember = "PARAM_NAME";
            this.lueSelectParameter.ItemIndex = 0;
        }
        private void BindUnit()
        {
            ///获取单位

            string[] columns = new string[] { "GROUP", "UNIT" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Unit");
            DataTable dtUnit = BaseData.Get(columns, category);
            txtUseUnit.Properties.DataSource = dtUnit;
            this.txtUseUnit.Properties.DisplayMember = "UNIT";
            this.txtUseUnit.Properties.ValueMember = "UNIT";
            this.txtUseUnit.ItemIndex = 0;

            txtUseConrtastUnit.Properties.DataSource = dtUnit;
            this.txtUseConrtastUnit.Properties.DisplayMember = "UNIT";
            this.txtUseConrtastUnit.Properties.ValueMember = "UNIT";
            this.txtUseConrtastUnit.ItemIndex = 0;
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
 