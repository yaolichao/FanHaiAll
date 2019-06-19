//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// chao.pang            2012-07-30            添加注释 
// =================================================================================
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

namespace FanHai.Hemera.Addins.BasicData
{
    /// <summary>
    /// 管理供应商的控件类。
    /// </summary>
    public partial class SpecialMatTeamCtrl : BaseUserCtrl
    {
        private string _workOrder = string.Empty;
        private string _material = string.Empty;
        private string _paramTeam = string.Empty;
        SpecialMatTeamEntity specialMatTeamEntity = new SpecialMatTeamEntity();
        /// <summary>
        /// 状态改变事件委托。
        /// </summary>
        /// <param name="controlState"></param>
        public new delegate void AfterStateChanged(ControlState controlState);
        /// <summary>
        /// 状态改变事件。
        /// </summary>
        public new AfterStateChanged afterStateChanged = null;
        /// <summary>
        /// 控件状态。
        /// </summary>
        private ControlState _ctrlState = ControlState.Empty;
        /// <summary>
        /// 控件状态。
        /// </summary>
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

        /// <summary>
        /// Control state change method
        /// </summary>
        /// <param name="state">Control state</param>
        private void OnAfterStateChanged(ControlState state)
        {
            tsbNew.Enabled = true;
            switch (state)
            {
                #region case state of editer
                case ControlState.Edit:

                    lueMat.Enabled = true;
                    cmbWorkOrder.Enabled = true;
                    lueParamer.Enabled = true;

                    tsbSave.Enabled = true;
                    tsbDel.Enabled = true;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    BindData();
                    lueMat.Enabled = true;
                    cmbWorkOrder.Enabled = true;
                    lueParamer.Enabled = true;

                    tsbSave.Enabled = true;
                    tsbDel.Enabled = false;
                    tsbNew.Enabled = true;
                    lblCode.Text = "";
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:

                    lueMat.Enabled = false;
                    cmbWorkOrder.Enabled = false;
                    lueParamer.Enabled = false;

                    tsbSave.Enabled = false;
                    tsbDel.Enabled = true;
                    lblCode.Text = "";
                    break;
                #endregion
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public SpecialMatTeamCtrl()
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            this.tsbRefresh.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.btn.0001}");//刷新
            this.tsbNew.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.btn.0002}");//新增
            this.tsbSave.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.btn.0003}");//保存
            this.tsbDel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.btn.0004}");//删除
            this.tsbClose.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.btn.0005}");//关闭

            lblApplicationTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.lbl.0001}");//特殊物料管控
            lblName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.lbl.0002}");//工单号
            lciCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.lbl.0003}");//物料号
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.lbl.0004}");//参数组
            btnQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.lbl.0005}");//查询
            lblWorkOrder.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.lbl.0006}");//工单号
            lblMat.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.lbl.0007}");//物料号
            lblDesc.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.lbl.0008}");//物料描述
            lblCodeDescription.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.lbl.0009}");//参数组

            gcWorkOrder.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.GridControl.0001}");//工单号
            gcMaterial.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.GridControl.0002}");//物料号
            gcMatDesc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.GridControl.0003}");//物料描述
            gcParamTeam.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.GridControl.0004}");//参数组
        }
        //  关闭窗体
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        /// <summary>
        /// 查询数据通过工单，物料，参数组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataSet dataSet = new DataSet();
            
            _workOrder = this.txtSelectWorkOrder.Text.ToString().Trim();
            _material = this.txtSelectMat.Text.ToString();
            _paramTeam = this.txtSelectParamTeam.Text.ToString();
            dataSet = specialMatTeamEntity.GetMatSpecialInf(_workOrder,_material,_paramTeam);
            if (dataSet != null)
            {
                grdCrtlCode.MainView = gridViewCode;
                grdCrtlCode.DataSource = dataSet.Tables[0];
                gridViewCode.BestFitColumns();
            }
        }
        /// <summary>
        /// 新增特殊物料管控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbNew_Click(object sender, EventArgs e)
        {
            CtrlState = ControlState.New;
        }

        private void SpecialMatTeamCtrl_Load(object sender, EventArgs e)
        {
            CtrlState = ControlState.New;
            BindData();
        }
        private void BindData()
        {
            BindWorkOrder();
            BindParamerTeam();
            BindMatSpecialInf();
        }

        private void BindParamerTeam()
        {
            //string[] columns = new string[] { "PARAMER_TEAM_CODE", "PARAMER_DESC" };
            //KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "PARAMER_TEAM");
            //DataTable dtFac = BaseData.Get(columns, category);
            //this.lueParamer.Properties.DataSource = dtFac;
            //this.lueParamer.Properties.DisplayMember = "PARAMER_TEAM_CODE";
            //this.lueParamer.Properties.ValueMember = "PARAMER_TEAM_CODE";
            DataSet ds = specialMatTeamEntity.GetParamerTeam();
            if (ds != null || ds.Tables[0].Rows.Count > 0)
            {
                lueParamer.Properties.DataSource = ds.Tables[0];
                this.lueParamer.Properties.DisplayMember = "EDC_NAME";
                this.lueParamer.Properties.ValueMember = "EDC_NAME";
            }
            else
            {
                this.lueParamer.Properties.DataSource = null;
                this.lueParamer.EditValue = string.Empty;
            }
        }

        private void BindMaterial()
        {
            DataSet ds = specialMatTeamEntity.GetMaterialByWorkOrder(this.cmbWorkOrder.Text.ToString().Trim());
            if (ds != null || ds.Tables[0].Rows.Count > 0)
            {
                lueMat.Properties.DataSource = ds.Tables[0];
                this.lueMat.Properties.DisplayMember = "MATERIAL_CODE";
                this.lueMat.Properties.ValueMember = "MATERIAL_CODE";
            }
            else
            {
                this.lueMat.Properties.DataSource = null;
                this.lueMat.EditValue = string.Empty;
            }
        }

        private void BindWorkOrder()
        {
            cmbWorkOrder.Properties.Items.Clear();
            DataSet ds = specialMatTeamEntity.GetWorkNumber();
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cmbWorkOrder.Properties.Items.Add(ds.Tables[0].Rows[i]["ORDER_NUMBER"]);
                }
            }
        }

        private void cmbWorkOrder_EditValueChanged(object sender, EventArgs e)
        {
            BindMaterial();
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            specialMatTeamEntity = new SpecialMatTeamEntity();
            CtrlState = ControlState.New;
        }

        private void tsbDel_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}")))//确定删除吗?
            {//系统提示你确定要删除吗？
                if (specialMatTeamEntity.Delete())
                {
                    //数据表数据重新绑定 modi my chao.pang
                    BindMatSpecialInf();
                    CtrlState = ControlState.New;
                }
            }
        }

        private void BindMatSpecialInf()
        {
            DataSet dataSet = new DataSet();
            specialMatTeamEntity = new SpecialMatTeamEntity();
            dataSet = specialMatTeamEntity.GetMatSpecialInf();
            if (dataSet != null)
            {
                grdCrtlCode.MainView = gridViewCode;
                grdCrtlCode.DataSource = dataSet.Tables[0];
                gridViewCode.BestFitColumns();
            }
        }

        private void gridViewCode_RowClick(object sender, RowClickEventArgs e)
        {
            CtrlState = ControlState.Edit;

            specialMatTeamEntity = new SpecialMatTeamEntity(this.gridViewCode.GetRowCellValue(e.RowHandle, "ORDER_NUMBER").ToString(), this.gridViewCode.GetRowCellValue(e.RowHandle, "MATERIAL_CODE").ToString(), this.gridViewCode.GetRowCellValue(e.RowHandle, "MATKL").ToString());
            this.cmbWorkOrder.Text = this.gridViewCode.GetRowCellValue(e.RowHandle, "ORDER_NUMBER").ToString();
            this.lueMat.EditValue = this.gridViewCode.GetRowCellValue(e.RowHandle, "MATERIAL_CODE").ToString();
            this.txtDesc.Text = this.gridViewCode.GetRowCellValue(e.RowHandle, "DESCRIPTION").ToString();
            this.lueParamer.EditValue = this.gridViewCode.GetRowCellValue(e.RowHandle, "MATKL").ToString();
            this.lblCode.Text = this.gridViewCode.GetRowCellValue(e.RowHandle, "EXTENSION_KEY").ToString();
            specialMatTeamEntity.WorkOrderNum = this.cmbWorkOrder.Text;
            specialMatTeamEntity.Mat = this.lueMat.Text;
            specialMatTeamEntity.ParamerTeam = this.lueParamer.Text;
            specialMatTeamEntity.ResetDirtyList();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            //if (MessageService.AskQuestion("你确定要保存当前界面的数据吗？", "保存"))
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                MapControlsToMat();
                bool IsTrue = false;

                if (specialMatTeamEntity.WorkOrderNum == string.Empty)
                {
                    //MessageService.ShowMessage("工单不为空", "保存");    //名称不能为空!
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}")); 
                    return;
                }

                if (specialMatTeamEntity.Mat == string.Empty)
                {
                    //MessageService.ShowMessage("物料不为空", "保存");    //名称不能为空!
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
                if (specialMatTeamEntity.ParamerTeam == string.Empty)
                {
                    //MessageService.ShowMessage("参数组不为空", "保存");    //名称不能为空!
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}")); 
                    return;
                }

                if (CtrlState == ControlState.New && specialMatTeamEntity.GetMatSpecialInf(specialMatTeamEntity.WorkOrderNum, specialMatTeamEntity.Mat, specialMatTeamEntity.ParamerTeam).Tables[0].Rows.Count > 0)
                {
                    //MessageService.ShowMessage("当前数据信息已经存在,请重新设置", "保存");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.msg.0006}"), StringParser.Parse("${res:Global.SystemInfo}"));  
                    return;
                }

                //if (CtrlState == ControlState.New)
                //{//状态为new
                //    if (specialMatTeamEntity.Update())
                //    {//新增成功
                //        IsTrue = true;
                //    }
                //}
                //else
                //{//状态不为new
                //    if (specialMatTeamEntity.Update())
                //    {//修改成功
                //        IsTrue = true;
                //    }
                //    else
                //    {
                //        MessageService.ShowMessage("请选择要修改的信息", "保存"); 
                //    }

                //}

                if (specialMatTeamEntity.Update(lblCode.Text.Trim()))
                {//新增成功
                    IsTrue = true;
                }
                else
                {
                    //MessageService.ShowMessage("请选择要修改的信息", "保存");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.SpecialMatTeamCtrl.msg.0007}"), StringParser.Parse("${res:Global.SystemInfo}"));  
                }
                if (IsTrue)
                {//值为true
                    BindMatSpecialInf();                                //数据表数据重新绑定
                    CtrlState = ControlState.ReadOnly;                 //状态为readonly
                }
            }
        }
        private void MapControlsToMat()
        {
            if (null == specialMatTeamEntity)
            {
                throw (new Exception("Error Reason Code Set"));
            }
            specialMatTeamEntity.WorkOrderNum = this.cmbWorkOrder.Text.Trim();
            specialMatTeamEntity.Mat = this.lueMat.Text.Trim();
            specialMatTeamEntity.MatDesc = this.txtDesc.Text.Trim();
            specialMatTeamEntity.ParamerTeam= this.lueParamer.Text.Trim();
        }

        private void lueMat_EditValueChanged(object sender, EventArgs e)
        {
            this.txtDesc.Text = this.lueMat.GetColumnValue("DESCRIPTION").ToString();
        }
    }
}
 