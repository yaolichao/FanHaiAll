using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Mask;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 表示材料耗用的窗体类。
    /// </summary>
    public partial class UseMaterialCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UseMaterialCtrl()
        {
            InitializeComponent();
            this.txtOperator.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
        }
        private new delegate void AfterStateChanged(ControlState controlState);
        private ControlState _controlState = ControlState.Empty;
        private new AfterStateChanged afterStateChanged = null;

        #region State Change
        private new ControlState State
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

        #region Override Methods

        protected override void InitUIControls()
        {
            #region Operation Grid
            int index = 0;
            DataTable dt = new DataTable();

            GridColumn column = new GridColumn();

            column = new GridColumn();
            column.Name = "grcWuLiaoZj1";
            column.FieldName = "MATERIAL_USED_KEY";
            column.Caption = "物料耗用表主键";
            column.Visible = false;

            column = new GridColumn();
            column.Name = "grcWuLiaoZj";
            column.FieldName = "MATERIAL_USED_DETAIL_KEY";
            column.Caption = "物料耗用明细表主键";
            column.Visible = false;

            this.gvMain.Columns.Add(column);

            column = new GridColumn();
            column.Name = "grcNo";
            column.FieldName = "ROWNUMBER";
            column.Caption = "序号";
            column.Visible = true;
            column.VisibleIndex = index++;
            column.OptionsColumn.AllowEdit = false;

            this.gvMain.Columns.Add(column);

            column = new GridColumn();
            column.Name = "grcWuLiao";
            column.FieldName = "MATERIAL_LOT";
            column.Caption = "物料批号";
            column.Visible = true;
            column.VisibleIndex = index++;
            column.OptionsColumn.AllowEdit = false;

            this.gvMain.Columns.Add(column);

            column = new GridColumn();

            column.Name = "grcCaiLiaoNumber";
            column.FieldName = "MATNR";
            column.Caption = "物料编号";
            column.Visible = true;
            column.VisibleIndex = index++;
            column.OptionsColumn.AllowEdit = false;

            this.gvMain.Columns.Add(column);

            column = new GridColumn();

            column.Name = "grcCaiLiaoMS";
            column.FieldName = "MATXT";
            column.Caption = "物料描述";
            column.Visible = true;
            column.VisibleIndex = index++;
            column.OptionsColumn.AllowEdit = false;

            this.gvMain.Columns.Add(column);

            column = new GridColumn();

            column.Name = "grcGongYingShang";
            column.FieldName = "LLIEF";
            column.Caption = "供应商";
            column.Visible = true;
            column.VisibleIndex = index++;
            column.OptionsColumn.AllowEdit = false;

            this.gvMain.Columns.Add(column);

            column = new GridColumn();

            column.Name = "grcShuLiang";
            column.FieldName = "CURRENT_QTY";
            column.Caption = "数量";
            column.Visible = true;
            column.VisibleIndex = index++;
            column.OptionsColumn.AllowEdit = false;

            this.gvMain.Columns.Add(column);



            column = new GridColumn();
            column.Name = "grcDanWei";
            column.FieldName = "ERFME";
            column.Caption = "单位";
            column.Visible = true;
            column.VisibleIndex = index++;
            column.OptionsColumn.AllowEdit = false;

            this.gvMain.Columns.Add(column);

            column = new GridColumn();
            column.Name = "grcLineNumber";
            column.FieldName = "STORE_NAME";
            column.Caption = "线上仓";
            column.Visible = true;
            column.VisibleIndex = index++;
            column.OptionsColumn.AllowEdit = false;

            this.gvMain.Columns.Add(column);

            column = new GridColumn();
            column.Name = "grcStirTime";
            column.FieldName = "STIR_TIME";
            column.Caption = "搅拌时间";
            column.Visible = false;
            column.ColumnEdit = repositoryItemDateEdit1;
            column.VisibleIndex = -1;
            //modi  by chao.pang 来料接收程序修改引发的只记录材料批次号，不扣减数量
            //column.VisibleIndex = index++;

            this.gvMain.Columns.Add(column);

            column = new GridColumn();
            column.Name = "grcPrintQiy";
            column.FieldName = "PRINT_QTY";
            column.Caption = "印刷数量";
            column.Visible = false;
            column.ColumnEdit = repositoryItemTextEdit1;
            repositoryItemTextEdit1.Mask.MaskType = MaskType.RegEx;
            repositoryItemTextEdit1.Mask.EditMask = "[0-9]+(\\.[0-9]{0,2})?";
            column.VisibleIndex = -1;
            //modi  by chao.pang 来料接收程序修改引发的只记录材料批次号，不扣减数量
            //column.VisibleIndex = index++;

            this.gvMain.Columns.Add(column);

            column = new GridColumn();
            column.FieldName = "STORE_MATERIAL_DETAIL_KEY";
            column.Caption = "材料耗用明细主键";
            column.Visible = true;

            this.gvMain.Columns.Add(column);
            this.gvMain.BestFitColumns();
            #endregion
        }
        #endregion
        /// <summary>
        /// Deal with state change event
        /// </summary>
        /// <param name="state"></param>
        private void OnAfterStateChanged(ControlState controlState)
        {
            switch (controlState)
            {
                #region case state of empty
                case ControlState.Empty:

                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:

                    break;
                #endregion

                #region case state of Read
                case ControlState.Read:
                    cmbOperationName.Enabled = false;
                    cmbFactoryRoom.Enabled = false;
                    cmbEquipmentName.Enabled = false;
                    txtUseDateTime.Enabled = false;
                    memoReason.Enabled = false;
                    tsbUpdate.Enabled = true;
                    tsbDelete.Enabled = true;
                    tsbNew.Enabled = true;
                    tsbSave.Enabled = false;
                    txtMaterialLot.Enabled = false;
                    break;
                #endregion

                #region case state of edit
                case ControlState.Edit:
                    cmbOperationName.Enabled = false;
                    cmbFactoryRoom.Enabled = false;
                    cmbEquipmentName.Enabled = false;
                    txtUseDateTime.Enabled = true;
                    memoReason.Enabled = true;
                    tsbUpdate.Enabled = false;
                    tsbDelete.Enabled = false;
                    tsbNew.Enabled = true;
                    tsbSave.Enabled = true;
                    txtMaterialLot.Enabled = true;
                    txtMaterialLot.Enabled = false;
                    //cmbOperationName.Enabled = true;
                    //cmbFactoryRoom.Enabled = true;
                    //cmbEquipmentName.Enabled = true;
                    //txtUseDateTime.Enabled = true;
                    //memoReason.Enabled = true;
                    //tsbUpdate.Enabled = false;
                    //tsbDelete.Enabled = false;
                    //tsbNew.Enabled = true;
                    //tsbSave.Enabled = true;
                    //txtMaterialLot.Enabled = true;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    cmbOperationName.Enabled = true;
                    cmbFactoryRoom.Enabled = true;
                    cmbEquipmentName.Enabled = true;
                    txtUseDateTime.Enabled = true;
                    memoReason.Enabled = true;
                    tsbUpdate.Enabled = false;
                    tsbDelete.Enabled = false;
                    tsbNew.Enabled = true;
                    tsbSave.Enabled = true;
                    txtMaterialLot.Enabled = true;
                    txtUseDateTime.EditValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    gcUseMaterialList.DataSource = null;
                    txtMaterialLot.Text = "";
                    break;
                #endregion
            }
        }
        #endregion

        /// <summary>
        /// 材料耗用清单显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbShowList_Click(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.WorkOrderManagement.Name}"))
                if (viewContent.TitleName == "Default Title")
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //显示材料耗用清单的用户控件
            WorkbenchSingleton.Workbench.ShowView(new UseMaterialListCtrlContent());
        }
        /// <summary>
        /// 绑定工序
        /// </summary>
        public void BindOperation()
        {
            #region Bind Operation
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            if (operations.Length > 0)
            {
                string[] strOperations = operations.Split(',');
                for (int i = 0; i < strOperations.Length; i++)
                {
                    cmbOperationName.Properties.Items.Add(strOperations[i]);
                }
                this.cmbOperationName.SelectedIndex = 0;
            }
            #endregion
        }

        /// <summary>
        /// 绑定工厂车间
        /// </summary>
        public void BindFactoryRoom()
        {
            #region
            //绑定工厂车间名称
            DataTable dt2 = FactoryUtils.GetFactoryRoomByStores(PropertyService.Get(PROPERTY_FIELDS.STORES));
            //绑定工厂车间数据到窗体控件。
            cmbFactoryRoom.Properties.DataSource = dt2;
            cmbFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
            cmbFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
            //表中有数据，设置窗体控件的默认索引为0。
            if (dt2.Rows.Count > 0)
            {
                cmbFactoryRoom.ItemIndex = 0;
            }
            #endregion
        }

        /// <summary>
        /// 绑定班次
        /// </summary>
        public void BindShiftName()
        {
            string[] columns = new string[] { "CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Shift");

            this.cmbShiftName.Properties.DataSource = BaseData.Get(columns, category);
            this.cmbShiftName.Properties.DisplayMember = "CODE";
            this.cmbShiftName.Properties.ValueMember = "CODE";
            this.cmbShiftName.ItemIndex = 0;
            Shift shift = new Shift();
            this.cmbShiftName.EditValue = shift.GetCurrShiftName();
        }
        private void UseMaterialCtrl_Load(object sender, EventArgs e)
        {
            //绑定工序
            BindOperation();

            //绑定车间
            BindFactoryRoom();
            BindShiftName();
            //绑定材料历史耗用数据表数据
            CodeGridDataBind();
            State = ControlState.New;
            OnAfterStateChanged(State);

        }

       
        /// <summary>
        /// 绑定数据表
        /// </summary>
        public void CodeGridDataBind()
        {
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            string stores = PropertyService.Get(PROPERTY_FIELDS.STORES);
            UseMaterial _material = new UseMaterial();
            gcUsedMaterialList.MainView = gvUsedMaterialListMain;
            gcUsedMaterialList.DataSource = _material.GetMaterialUsed(operations, stores).Tables[0];
            gvMain.BestFitColumns();

        }
        /// <summary>
        /// 设备名称绑定
        /// </summary>
        public void GetEquipment()
        {
            //绑定设备名称
            #region
            string operationname = cmbOperationName.SelectedItem.ToString();
            string cmbfactoryroom = cmbFactoryRoom.Text;
            UseMaterial _material = new UseMaterial();
            ////根据根据工序车间获取设备信息。
            DataSet dsMaterial = _material.GetEquipmentInfo(operationname, cmbfactoryroom);
            if (_material.ErrorMsg.Length < 1)//如果执行查询失败。
            {
                if (dsMaterial.Tables.Count > 0)//查询结果数据集中有表。
                {
                    //绑定工厂车间数据到窗体控件。
                    cmbEquipmentName.Properties.DataSource = dsMaterial.Tables[0];
                    cmbEquipmentName.Properties.DisplayMember = "EQUIPMENT_NAME";
                    cmbEquipmentName.Properties.ValueMember = "EQUIPMENT_KEY";
                    //线别表中有数据，设置窗体控件的默认索引为0。
                    if (dsMaterial.Tables[0].Rows.Count > 0)
                    {
                        cmbEquipmentName.ItemIndex = 0;
                    }
                }
            }
            else
            {
                MessageService.ShowError(_material.ErrorMsg);
            }
            #endregion
        }

        /// <summary>
        /// 工序改变改变设备名称
        /// </summary>
        private void cmbOperationName_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetEquipment();    /// 设备名称绑定
            //gcUseMaterialList.DataSource = null; 
        }
        /// <summary>
        /// 工厂车间改变设备名称
        /// </summary>
        private void cmbFactoryRoom_TextChanged(object sender, EventArgs e)
        {
            GetEquipment();    /// 设备名称绑定
            //gcUseMaterialList.DataSource = null;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbNew_Click(object sender, EventArgs e)
        {
            this.gcUseMaterialList.DataSource = null;
            State = ControlState.New;
            OnAfterStateChanged(State);
            //绑定工序
            BindOperation();

            //绑定车间
            BindFactoryRoom();

            #region
            BindShiftName();
           
            #endregion

            //绑定材料历史耗用数据表数据
            CodeGridDataBind();

            this.txtOperator.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            txtUseDateTime.EditValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        }
        /// <summary>
        /// 关闭当前用户控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        /// <summary>
        /// 物料批号回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaterialLot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string strMaterialLot = txtMaterialLot.Text.Trim();
                if (string.IsNullOrEmpty(strMaterialLot))
                {
                    MessageService.ShowMessage("请输入物料批号！", "${res:Global.SystemInfo}");
                    return;
                }
                //判断输入的物料批号是否重复？
                DataView dv = gvMain.DataSource as DataView;
                if (dv != null)
                {
                    int length=dv.Table.Select("MATERIAL_LOT='" + strMaterialLot + "'").Length;
                    if (length > 0)
                    {
                        MessageService.ShowMessage("耗用列表中已存在批号为[" + strMaterialLot + "]的物料，不能重复添加。","${res:Global.SystemInfo}");
                        return;
                    }
                }
                //获取物料批号对应的物料信息。
                UseMaterial _material = new UseMaterial();
                DataSet materialByLotOpFa = _material.GetMaterialByLotOpFa(txtMaterialLot.Text.Trim(), cmbOperationName.Text.Trim(), cmbFactoryRoom.Text.Trim());
                if (materialByLotOpFa.Tables.Count > 0)
                {
                    if (materialByLotOpFa.Tables[0].Rows.Count > 0)
                    {
                        gvMain.Columns[9].OptionsColumn.AllowEdit = true;
                        gvMain.Columns[10].OptionsColumn.AllowEdit = true;
                        materialByLotOpFa.Tables[0].Columns.Add(new DataColumn("STIR_TIME", typeof(DateTime)));
                        materialByLotOpFa.Tables[0].Columns.Add(new DataColumn("PRINT_QTY", typeof(string)));


                        gcUseMaterialList.MainView = gvMain;

                        DataTable dt01 = new DataTable();
                        if (gvMain.DataSource == null)
                        {
                            dt01 = materialByLotOpFa.Tables[0];
                            if (!dt01.Columns.Contains("ROWNUMBER"))
                                dt01.Columns.Add("ROWNUMBER");
                            dt01.Rows[0]["ROWNUMBER"] = 1;

                            gcUseMaterialList.DataSource = dt01;
                            gvMain.BestFitColumns();
                        }

                        else if (gvMain.DataSource != null)
                        {
                            DataTable dt = ((DataView)gvMain.DataSource).Table;
                            dt.ImportRow(materialByLotOpFa.Tables[0].Rows[0]);

                            if (!dt.Columns.Contains("ROWNUMBER"))
                                dt.Columns.Add("ROWNUMBER");

                            for (int i = 1; i < dt.Rows.Count + 1; i++)
                                dt.Rows[i - 1]["ROWNUMBER"] = i.ToString();

                            gcUseMaterialList.DataSource = dt;
                            gvMain.BestFitColumns();
                        }
                        else
                        {
                            gcUseMaterialList.DataSource = materialByLotOpFa.Tables[0];
                        }
                        txtMaterialLot.Text = "";
                    }
                    else
                    {
                        MessageService.ShowMessage("没有物料信息！", "${res:Global.SystemInfo}");
                    }
                }
                else
                {
                    MessageService.ShowError(_material.ErrorMsg);
                }
            }
            this.gvMain.BestFitColumns();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbUpdate_Click(object sender, EventArgs e)
        {
            UseMaterial _material = new UseMaterial();
            DataSet ds = _material.GetMaterialDetailByMaterialLot(txtMaterialLot.Text.Trim(), gvUsedMaterialListMain.GetFocusedRowCellValue("MATERIAL_USED_KEY").ToString());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    State = ControlState.Edit;
                    OnAfterStateChanged(State);
                    gvMain.Columns[9].OptionsColumn.AllowEdit = true;
                    gvMain.Columns[10].OptionsColumn.AllowEdit = true;
                }
                else
                {
                    MessageService.ShowMessage("材料耗用已结账不能修改!", "${res:Global.SystemInfo}");
                }
            }
        }
        /// <summary>
        /// 材料耗用清单行数据行单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvUsedMaterialListMain_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            this.gcUseMaterialList.DataSource = null;
            State = ControlState.Read;
            OnAfterStateChanged(State);
            UseMaterial _material = new UseMaterial();
            DataTable dt = new DataTable();
            string key = gvUsedMaterialListMain.GetFocusedRowCellValue("MATERIAL_USED_DETAIL_KEY").ToString();

            dt = _material.GetMaterialDetailByKey(key).Tables[0];
            gcUseMaterialList.DataSource = _material.GetMaterialDetailByKey(key).Tables[0];
            txtMaterialLot.Text = dt.Rows[0]["MATERIAL_LOT"].ToString();
            cmbOperationName.Text = gvUsedMaterialListMain.GetFocusedRowCellValue("ROUTE_OPERATION_NAME").ToString();
            //cmbOperationName.Text = dt.Rows[0]["ROUTE_OPERATION_NAME"].ToString();
            //cmbFactoryRoom.EditValue = gvUsedMaterialListMain.GetFocusedRowCellValue("LOCATION_NAME").ToString();
            cmbFactoryRoom.EditValue = dt.Rows[0]["LOCATION_KEY"].ToString();
            //cmbEquipmentName.EditValue = gvUsedMaterialListMain.GetFocusedRowCellValue("EQUIPMENT_NAME").ToString();
            cmbEquipmentName.EditValue = dt.Rows[0]["EQUIPMENT_KEY"].ToString();
            cmbShiftName.EditValue = dt.Rows[0]["SHIFT_NAME"].ToString();
            txtUseDateTime.Text = dt.Rows[0]["USED_TIME"].ToString();
            txtOperator.Text = dt.Rows[0]["OPERATOR"].ToString();
            //modi  by chao.pang 来料接收程序修改引发的只记录材料批次号，不扣减数量
            gvMain.Columns[9].OptionsColumn.AllowEdit = false;
            gvMain.Columns[10].OptionsColumn.AllowEdit = false;

        }
        /// <summary>
        /// 移除材料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (State == ControlState.Edit || State == ControlState.Read)
            {
                MessageService.ShowMessage("修改中，不能从物料列表中移除物料!", "${res:Global.SystemInfo}");
                return;
            }

            if (this.gvMain.GetFocusedRow() != null)
            {
                this.gvMain.DeleteRow(this.gvMain.FocusedRowHandle);

                DataTable dt = ((DataView)gvMain.DataSource).Table.GetChanges(DataRowState.Modified);
                if (dt != null)
                {
                    if (!dt.Columns.Contains("ROWNUMBER"))
                        dt.Columns.Add("ROWNUMBER");

                    for (int i = 1; i < dt.Rows.Count + 1; i++)
                        dt.Rows[i - 1]["ROWNUMBER"] = i.ToString();

                    gcUseMaterialList.DataSource = dt;
                }
                else
                {
                    gcUseMaterialList.DataSource = null;     //清空datasource                    
                }

            }
            else
            {
                MessageService.ShowMessage("本次耗用列表中必须选择至少选择一条记录", "${res:Global.SystemInfo}");
            }
            this.gvMain.BestFitColumns();

        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (this.gvMain.State == GridState.Editing && this.gvMain.IsEditorFocused
               && this.gvMain.EditingValueModified)
            {
                this.gvMain.SetFocusedRowCellValue(this.gvMain.FocusedColumn, this.gvMain.EditingValue);
            }
            this.gvMain.UpdateCurrentRow();

            if (MessageBox.Show(StringParser.Parse("确定要保存吗？"),
                    StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (gcUseMaterialList.DataSource == null)
                {
                    MessageService.ShowMessage("本次耗用列表不能为空!", "${res:Global.SystemInfo}");
                    return;
                }
                if (string.IsNullOrEmpty(cmbOperationName.Text))
                {
                    MessageService.ShowMessage("工序不能为空!", "${res:Global.SystemInfo}");
                    return;
                }
                if (string.IsNullOrEmpty(cmbFactoryRoom.Text))
                {
                    MessageService.ShowMessage("车间不能为空!", "${res:Global.SystemInfo}");
                    return;
                }
                if (string.IsNullOrEmpty(cmbEquipmentName.Text))
                {
                    MessageService.ShowMessage("设备不能为空!", "${res:Global.SystemInfo}");
                    return;
                }
                if (string.IsNullOrEmpty(txtUseDateTime.Text))
                {
                    MessageService.ShowMessage("耗用时间不能为空!", "${res:Global.SystemInfo}");
                    return;
                }
                UseMaterial _material = new UseMaterial();

                Hashtable hashTable = new Hashtable();
                hashTable.Add("CREATOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                hashTable.Add("CREATE_TIMEZONE", PropertyService.Get(PROPERTY_FIELDS.TIMEZONE));
                hashTable.Add("EDITOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                hashTable.Add("EDIT_TIMEZONE", PropertyService.Get(PROPERTY_FIELDS.TIMEZONE));
                DataTable tableParam = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                tableParam.TableName = "HASH";

                if (State == ControlState.New)
                {//判断状态为新增
                    DataTable dtDetailTable = new DataTable();
                    DataView dv = gvMain.DataSource as DataView;
                    if (dv != null) dtDetailTable = dv.Table;


                    DataTable dtMaterialUsed = new DataTable();
                    dtMaterialUsed.Columns.Add("OPERATION_NAME");
                    dtMaterialUsed.Columns.Add("EQUIPMENT_KEY");
                    dtMaterialUsed.Columns.Add("SHIFT_NAME");
                    dtMaterialUsed.Columns.Add("USED_TIME");
                    dtMaterialUsed.Columns.Add("OPERATOR");
                    dtMaterialUsed.Columns.Add("REASON");
                    DataRow dr = dtMaterialUsed.NewRow();
                    dr["OPERATION_NAME"] = cmbOperationName.EditValue.ToString();
                    string equipmentName = cmbEquipmentName.Text;
                    string equipmentKey = cmbEquipmentName.Properties.GetKeyValueByDisplayText(equipmentName).ToString();
                    dr["EQUIPMENT_KEY"] = equipmentKey;
                    dr["SHIFT_NAME"] = cmbShiftName.EditValue;
                    dr["USED_TIME"] = txtUseDateTime.EditValue;
                    dr["OPERATOR"] = txtOperator.Text;
                    dr["REASON"] = memoReason.Text;
                    dtMaterialUsed.Rows.Add(dr);

                    DataSet dsSetIn = new DataSet();
                    dtDetailTable.TableName = "MATERIAL_USED_DATAIL";
                    dsSetIn.Merge(dtDetailTable);
                    dtMaterialUsed.TableName = "MATERIAL_USED";
                    dsSetIn.Merge(dtMaterialUsed);                    
                    dsSetIn.Merge(tableParam);


                    if (!_material.InsertMaterial(dsSetIn))
                    {
                        MessageService.ShowMessage(_material.ErrorMsg);
                    }
                    else
                    {
                        MessageService.ShowMessage("新增材料耗用成功。");
                        tsbNew_Click(sender, e);
                    }

                }
                else if (State == ControlState.Edit)
                {                    
                    string materialLot = txtMaterialLot.Text.Trim();
                    string materialUsedKey = gvUsedMaterialListMain.GetFocusedRowCellValue("MATERIAL_USED_KEY").ToString();
                    string materialUsedKey1 = gvUsedMaterialListMain.GetFocusedRowCellValue("MATERIAL_USED_DETAIL_KEY").ToString();
                    string operationName = cmbOperationName.EditValue.ToString();
                    string equipmentName = cmbEquipmentName.Text;
                    string equipmentKey = cmbEquipmentName.Properties.GetKeyValueByDisplayText(equipmentName).ToString();
                    string usedDateTime = txtUseDateTime.Text;
                    string strTime = gvMain.GetRowCellValue(0, "STIR_TIME").ToString();
                    string printQty = gvMain.GetRowCellValue(0, "PRINT_QTY").ToString();
                    //修改材料耗用历史记录（材料明细表和材料表）
                    if (_material.UpDateMaterialUsedAndDetail(materialLot, materialUsedKey, materialUsedKey1, operationName, equipmentKey,
                        usedDateTime, strTime, printQty,tableParam) == true)
                    {
                        tsbNew_Click(sender, e);
                    }
                }
            }
            this.gvMain.BestFitColumns();
        }

        private void gvMain_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            gvMain.SetRowCellValue(e.RowHandle, e.Column, e.Value);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(StringParser.Parse("确定要删除吗？"),
                   StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                UseMaterial _material = new UseMaterial();
                //根据物料批次号和耗用主键获取状态是否为1  状态 0：已作废 1：未结账 2：已结账
                DataSet ds = _material.GetMaterialDetailByMaterialLot(txtMaterialLot.Text.Trim(), gvUsedMaterialListMain.GetFocusedRowCellValue("MATERIAL_USED_KEY").ToString());
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string materialLot = txtMaterialLot.Text.Trim();
                        string materialUsedKey = gvUsedMaterialListMain.GetFocusedRowCellValue("MATERIAL_USED_KEY").ToString();
                        string materialUsedKey1 = gvUsedMaterialListMain.GetFocusedRowCellValue("MATERIAL_USED_DETAIL_KEY").ToString();
                        if (_material.DeleteMaterital(materialLot, materialUsedKey, materialUsedKey1) == true)
                        {//通过物料批号和耗用主键来删除和修改表值
                            //绑定工序
                            BindOperation();

                            //绑定车间
                            BindFactoryRoom();
                            BindShiftName();

                            //绑定材料历史耗用数据表数据
                            CodeGridDataBind();
                            State = ControlState.New;
                            OnAfterStateChanged(State);
                        }
                    }
                    else
                    {
                        MessageService.ShowMessage("如果材料耗用已结账不能删除!", "${res:Global.SystemInfo}");
                    }
                }
            }
        }

    }
}
