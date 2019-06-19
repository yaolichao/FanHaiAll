using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.Utils;
using System.Collections;
using DevExpress.XtraGrid;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.Utils.Drawing;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.EMS
{
    /// <summary>
    /// Equipment Change States GUI
    /// </summary>
    public partial class EquipmentChangeStates : BaseUserCtrl
    {
        #region EquipmentStateEntity Object

        private EquipmentChangeStatesEntity equipmentChangeStatesEntity = null;
        private string equipmentInitStateKey = string.Empty;
        #endregion

        #region Constructor

        public EquipmentChangeStates()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void EquipmentChangeStates_Load(object sender, EventArgs e)
        {
            afterStateChanged += new AfterStateChanged(EquipmentChangeStates_afterStateChanged);

            LoadEquipmentStatesData();
            LoadEquipmentChangeStatesData();

            State = ControlState.Read;
        }

        private void EquipmentChangeStates_afterStateChanged()
        {
            switch (State)
            {
                case ControlState.Read:
                    this.tsbEdit.Enabled = true;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    break;
                case ControlState.ReadOnly:
                    this.tsbEdit.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    break;
                case ControlState.Edit:
                    this.tsbEdit.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Override Methods

        protected override void InitUIControls()
        {
            
        }

        protected override void InitUIResourcesByCulture()
        {
            GridViewHelper.SetGridView(bandedGridViewChangeStates);
            this.tsbRefresh.Text = StringParser.Parse("${res:Global.Refresh}");
            this.tsbEdit.Text = StringParser.Parse("${res:Global.Edit}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbCancel.Text = StringParser.Parse("${res:Global.Cancel}");

            //this.tsbRefresh.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Refresh");
            //this.tsbEdit.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Edit");
            //this.tsbSave.Image = (Image)ResourceService.GetImageResource("Icons.16x16.SaveIcon");
            //this.tsbCancel.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Cancel");
            //this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeStates.Name}");
            //this.grpChangeStates.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeStates.Name}");
        }

        protected override void InitUIAuthoritiesByUser()
        {
            
        }

        protected override void MapControlsToEntity()
        {
            for (int i = 0; i < this.bandedGridViewChangeStates.RowCount; i++)
            {
                foreach (BandedGridColumn column in this.bandedGridViewChangeStates.Columns)
                {
                    if (column.Tag != null)
                    {
                        bool cellChecked = (bool)this.bandedGridViewChangeStates.GetRowCellValue(i, column);

                        if (cellChecked)
                        {
                            if (equipmentChangeStatesEntity != null)
                            {
                                string leftStateKey = this.bandedGridViewChangeStates.GetRowCellValue(i, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY).ToString();
                                string topStateKey = column.Tag.ToString();
                                string leftStateName = this.bandedGridViewChangeStates.GetRowCellValue(i, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME).ToString();
                                string topStateName = column.Name;

                                EquipmentChangeStateEntity equipmentChangeStateEntity = new EquipmentChangeStateEntity();

                                equipmentChangeStateEntity.EquipmentChangeStateKey =  CommonUtils.GenerateNewKey(0);
                                equipmentChangeStateEntity.EquipmentChangeStateName = leftStateName + " -> " + topStateName;
                                equipmentChangeStateEntity.EquipmentFromStateKey = leftStateKey;
                                equipmentChangeStateEntity.EquipmentToStateKey = topStateKey;

                                equipmentChangeStatesEntity.Add(equipmentChangeStateEntity);
                            }
                        }
                    }
                }
            }
        }

        protected override void MapEntityToControls()
        {
            if (equipmentChangeStatesEntity != null)
            {
                equipmentChangeStatesEntity.UpdateEquipmentChangeStatesEntity();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generate Change States Data Grid
        /// </summary>
        /// <param name="statesDataTable"></param>
        private void GenerateChangeStatesDataGrid(DataTable statesDataTable01)
        {
            DataTable statesDataTable = statesDataTable01.Clone();
            equipmentInitStateKey = statesDataTable01.Select("EQUIPMENT_STATE_NAME='INIT'")[0][EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY].ToString();
            DataRow[] drArr = statesDataTable01.Select("EQUIPMENT_STATE_NAME<>'INIT'");
            foreach (DataRow dr in drArr)
                statesDataTable.ImportRow(dr);

            #region Create Change States DataTable Columns

            DataTable changeStatesDataTable = new DataTable();

            foreach (DataColumn column in statesDataTable.Columns)
            {
                changeStatesDataTable.Columns.Add(column.ColumnName, typeof(string));
            }

            foreach (DataRow row in statesDataTable.Rows)
            {
                DataColumn column = new DataColumn(row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString(), typeof(bool));

                column.DefaultValue = false;

                changeStatesDataTable.Columns.Add(column);
            }

            #endregion

            #region Left Columns On The Data Grid

            this.bandedGridViewChangeStates.Bands.Clear();
            this.bandedGridViewChangeStates.Columns.Clear();

            GridBand leftGridBand = new GridBand();

            leftGridBand.OptionsBand.AllowMove = false;
            leftGridBand.OptionsBand.AllowPress = false;
            leftGridBand.OptionsBand.FixedWidth = false;

            leftGridBand.AppearanceHeader.Options.UseTextOptions = true;
            leftGridBand.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            leftGridBand.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;

            leftGridBand.Visible = true;

            #region State Key Grid Column

            BandedGridColumn leftStateKeyGridColumn = new BandedGridColumn();

            leftStateKeyGridColumn.Caption = "";
            leftStateKeyGridColumn.Name = EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY;
            leftStateKeyGridColumn.FieldName = EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY;

            leftStateKeyGridColumn.OptionsColumn.AllowMerge = DefaultBoolean.False;

            leftStateKeyGridColumn.OptionsColumn.AllowEdit = false;
            leftStateKeyGridColumn.OptionsColumn.AllowMove = false;
            leftStateKeyGridColumn.OptionsColumn.FixedWidth = false;
            leftStateKeyGridColumn.OptionsColumn.ShowCaption = false;
            leftStateKeyGridColumn.OptionsColumn.AllowGroup = DefaultBoolean.False;
            leftStateKeyGridColumn.OptionsColumn.AllowSort = DefaultBoolean.False;

            leftStateKeyGridColumn.OptionsFilter.AllowFilter = false;

            leftStateKeyGridColumn.AppearanceHeader.Options.UseTextOptions = true;
            leftStateKeyGridColumn.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            leftStateKeyGridColumn.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;

            leftStateKeyGridColumn.Visible = false;

            this.bandedGridViewChangeStates.Columns.Add(leftStateKeyGridColumn);

            leftGridBand.Columns.Add(leftStateKeyGridColumn);

            #endregion

            #region State Category Grid Column

            BandedGridColumn leftStateCategoryGridColumn = new BandedGridColumn();

            leftStateCategoryGridColumn.Caption = "";
            leftStateCategoryGridColumn.Name = EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY;
            leftStateCategoryGridColumn.FieldName = EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY;

            leftStateCategoryGridColumn.OptionsColumn.AllowMerge = DefaultBoolean.True;

            leftStateCategoryGridColumn.OptionsColumn.AllowEdit = false;
            leftStateCategoryGridColumn.OptionsColumn.AllowMove = false;
            leftStateCategoryGridColumn.OptionsColumn.FixedWidth = false;
            leftStateCategoryGridColumn.OptionsColumn.ShowCaption = false;
            leftStateCategoryGridColumn.OptionsColumn.AllowGroup = DefaultBoolean.False;
            leftStateCategoryGridColumn.OptionsColumn.AllowSort = DefaultBoolean.False;

            leftStateCategoryGridColumn.OptionsFilter.AllowFilter = false;

            leftStateCategoryGridColumn.AppearanceHeader.Options.UseTextOptions = true;
            leftStateCategoryGridColumn.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            leftStateCategoryGridColumn.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;

            leftStateCategoryGridColumn.Visible = true;

            this.bandedGridViewChangeStates.Columns.Add(leftStateCategoryGridColumn);

            leftGridBand.Columns.Add(leftStateCategoryGridColumn);

            #endregion

            #region State Type Grid Column

            BandedGridColumn leftStateTypeGridColumn = new BandedGridColumn();

            leftStateTypeGridColumn.Caption = "";
            leftStateTypeGridColumn.Name = EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE;
            leftStateTypeGridColumn.FieldName = EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE;

            leftStateTypeGridColumn.OptionsColumn.AllowMerge = DefaultBoolean.True;

            leftStateTypeGridColumn.OptionsColumn.AllowEdit = false;
            leftStateTypeGridColumn.OptionsColumn.AllowMove = false;
            leftStateTypeGridColumn.OptionsColumn.FixedWidth = false;
            leftStateTypeGridColumn.OptionsColumn.ShowCaption = false;
            leftStateTypeGridColumn.OptionsColumn.AllowGroup = DefaultBoolean.False;
            leftStateTypeGridColumn.OptionsColumn.AllowSort = DefaultBoolean.False;

            leftStateTypeGridColumn.OptionsFilter.AllowFilter = false;

            leftStateTypeGridColumn.AppearanceHeader.Options.UseTextOptions = true;
            leftStateTypeGridColumn.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            leftStateTypeGridColumn.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;

            leftStateTypeGridColumn.Visible = true;

            this.bandedGridViewChangeStates.Columns.Add(leftStateTypeGridColumn);

            leftGridBand.Columns.Add(leftStateTypeGridColumn);

            #endregion

            #region State Name Grid Column

            BandedGridColumn leftStateNameGridColumn = new BandedGridColumn();

            leftStateNameGridColumn.Caption = "";
            leftStateNameGridColumn.Name = EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME;
            leftStateNameGridColumn.FieldName = EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME;

            leftStateNameGridColumn.OptionsColumn.AllowMerge = DefaultBoolean.True;

            leftStateNameGridColumn.OptionsColumn.AllowEdit = false;
            leftStateNameGridColumn.OptionsColumn.AllowMove = false;
            leftStateNameGridColumn.OptionsColumn.FixedWidth = false;
            leftStateNameGridColumn.OptionsColumn.ShowCaption = false;
            leftStateNameGridColumn.OptionsColumn.AllowGroup = DefaultBoolean.False;
            leftStateNameGridColumn.OptionsColumn.AllowSort = DefaultBoolean.False;

            leftStateNameGridColumn.OptionsFilter.AllowFilter = false;
            leftStateNameGridColumn.OptionsFilter.AllowAutoFilter = false;

            leftStateNameGridColumn.AppearanceHeader.Options.UseTextOptions = true;
            leftStateNameGridColumn.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            leftStateNameGridColumn.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;

            leftStateNameGridColumn.Visible = true;

            this.bandedGridViewChangeStates.Columns.Add(leftStateNameGridColumn);

            leftGridBand.Columns.Add(leftStateNameGridColumn);

            #endregion

            this.bandedGridViewChangeStates.Bands.Add(leftGridBand);

            #endregion

            #region Top Columns On the Data Grid

            #region State Category Grid Band

            statesDataTable.DefaultView.RowFilter = "";
            statesDataTable.DefaultView.Sort = string.Format("{0} ASC", EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY);

            List<string> stateCategorys = FanHai.Hemera.Utils.Common.Utils.GetDistinctValueList(statesDataTable.DefaultView, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY);

            foreach (string stateCategory in stateCategorys)
            {
                GridBand topStateCategoryGridBand = new GridBand();

                topStateCategoryGridBand.Name = stateCategory;
                topStateCategoryGridBand.Caption = stateCategory;

                topStateCategoryGridBand.OptionsBand.AllowMove = false;
                topStateCategoryGridBand.OptionsBand.AllowPress = false;
                topStateCategoryGridBand.OptionsBand.FixedWidth = false;

                topStateCategoryGridBand.AppearanceHeader.Options.UseTextOptions = true;
                topStateCategoryGridBand.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                topStateCategoryGridBand.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;

                topStateCategoryGridBand.Visible = true;

                #region State Type Grid Band

                statesDataTable.DefaultView.RowFilter = string.Format("{0} = '{1}'", EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY, stateCategory);
                statesDataTable.DefaultView.Sort = string.Format("{0} ASC", EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE);

                List<string> stateTypes = FanHai.Hemera.Utils.Common.Utils.GetDistinctValueList(statesDataTable.DefaultView, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE);

                foreach (string stateType in stateTypes)
                {
                    GridBand topStateTypeGridBand = new GridBand();

                    topStateTypeGridBand.Name = stateType;
                    topStateTypeGridBand.Caption = stateType;

                    topStateTypeGridBand.OptionsBand.AllowMove = false;
                    topStateTypeGridBand.OptionsBand.AllowPress = false;
                    topStateTypeGridBand.OptionsBand.FixedWidth = false;

                    topStateTypeGridBand.AppearanceHeader.Options.UseTextOptions = true;
                    topStateTypeGridBand.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    topStateTypeGridBand.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;

                    topStateTypeGridBand.Visible = true;

                    #region State Name Grid Band

                    statesDataTable.DefaultView.RowFilter = string.Format("{0} = '{1}'", EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE, stateType);
                    statesDataTable.DefaultView.Sort = string.Format("{0} ASC", EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME);

                    Hashtable stateNames = FanHai.Hemera.Utils.Common.Utils.GetDistinctValueList(statesDataTable.DefaultView, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME);

                    foreach (DictionaryEntry stateName in stateNames)
                    {
                        GridBand topStateNameGridBand = new GridBand();

                        topStateNameGridBand.Name = stateName.Value.ToString();
                        topStateNameGridBand.Caption = stateName.Value.ToString();

                        topStateNameGridBand.OptionsBand.AllowMove = false;
                        topStateNameGridBand.OptionsBand.AllowPress = false;
                        topStateNameGridBand.OptionsBand.FixedWidth = false;

                        topStateNameGridBand.AppearanceHeader.Options.UseTextOptions = true;
                        topStateNameGridBand.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                        topStateNameGridBand.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;

                        topStateNameGridBand.Visible = true;

                        #region State Name Grid Column

                        BandedGridColumn stateNameGridColumn = new BandedGridColumn();

                        stateNameGridColumn.Caption = stateName.Value.ToString();
                        stateNameGridColumn.Name = stateName.Value.ToString();
                        stateNameGridColumn.FieldName = stateName.Value.ToString();

                        stateNameGridColumn.OptionsColumn.AllowMerge = DefaultBoolean.False;
                        stateNameGridColumn.OptionsColumn.AllowEdit = true;
                        stateNameGridColumn.OptionsColumn.AllowMove = false;
                        stateNameGridColumn.OptionsColumn.FixedWidth = false;
                        stateNameGridColumn.OptionsColumn.ShowCaption = true;
                        stateNameGridColumn.OptionsColumn.AllowGroup = DefaultBoolean.False;
                        stateNameGridColumn.OptionsColumn.AllowSort = DefaultBoolean.False;
                        stateNameGridColumn.OptionsFilter.AllowFilter = false;

                        stateNameGridColumn.AppearanceHeader.Options.UseTextOptions = true;
                        stateNameGridColumn.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                        stateNameGridColumn.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;

                        stateNameGridColumn.Visible = true;

                        stateNameGridColumn.Tag = stateName.Key;

                        #region Selected Cell Style Format Condition

                        StyleFormatCondition SelectedCellSFC = new StyleFormatCondition();

                        SelectedCellSFC.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                        SelectedCellSFC.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                        SelectedCellSFC.Appearance.ForeColor = System.Drawing.Color.Black;
                        SelectedCellSFC.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                        SelectedCellSFC.Appearance.Options.UseBackColor = true;
                        SelectedCellSFC.Appearance.Options.UseForeColor = true;

                        SelectedCellSFC.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;

                        SelectedCellSFC.Value1 = true;

                        SelectedCellSFC.Column = stateNameGridColumn;

                        this.bandedGridViewChangeStates.FormatConditions.Add(SelectedCellSFC);

                        #endregion

                        this.bandedGridViewChangeStates.Columns.Add(stateNameGridColumn);

                        #region Add Change State Data

                        DataRow row = changeStatesDataTable.Rows.Add();

                        row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY] = stateName.Key;
                        row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY] = stateCategory;
                        row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE] = stateType;
                        row[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME] = stateName.Value;

                        changeStatesDataTable.AcceptChanges();

                        #endregion

                        #endregion

                        topStateNameGridBand.Columns.Add(stateNameGridColumn);

                        topStateTypeGridBand.Children.Add(topStateNameGridBand);
                    }

                    #endregion

                    topStateCategoryGridBand.Children.Add(topStateTypeGridBand);
                }

                #endregion

                this.bandedGridViewChangeStates.Bands.Add(topStateCategoryGridBand);
            }

            #endregion

            #endregion

            #region Band Change States Data To Grid

            this.grdChangeStates.MainView = this.bandedGridViewChangeStates;
            this.grdChangeStates.DataSource = null;
            this.grdChangeStates.DataSource = changeStatesDataTable;

            #endregion
        }

        /// <summary>
        /// Fill Change States Data
        /// </summary>
        /// <param name="changeStatesDataTable"></param>
        private void FillChangeStatesData(DataTable changeStatesDataTable01)
        {
            DataTable changeStatesDataTable = changeStatesDataTable01.Clone();
            DataRow[] drArr = changeStatesDataTable01.Select(string.Format("EQUIPMENT_FROM_STATE_KEY<>'{0}'", equipmentInitStateKey));
            foreach (DataRow dr in drArr)
                changeStatesDataTable.ImportRow(dr);

            equipmentChangeStatesEntity = new EquipmentChangeStatesEntity();

            equipmentChangeStatesEntity.InitEquipmentChangeStateList(changeStatesDataTable);

            if (changeStatesDataTable != null && changeStatesDataTable.Rows.Count > 0)
            {
                foreach (DataRow row in changeStatesDataTable.Rows)
                {
                    bool isReturn = false;

                    string fromStateKey = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_FROM_STATE_KEY].ToString();
                    string toStateKey = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY].ToString();

                    for (int i = 0; i < this.bandedGridViewChangeStates.RowCount; i++)
                    {
                        string stateKey = this.bandedGridViewChangeStates.GetRowCellValue(i, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY).ToString();

                        if (stateKey == fromStateKey)
                        {
                            foreach (BandedGridColumn column in this.bandedGridViewChangeStates.Columns)
                            {
                                if (column.Tag != null && column.Tag.ToString() == toStateKey)
                                {
                                    this.bandedGridViewChangeStates.SetRowCellValue(i, column, true);

                                    this.bandedGridViewChangeStates.RefreshRowCell(i, column);

                                    isReturn = true;

                                    break;
                                }
                            }
                        }

                        if (isReturn)
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resume the Chnage States Data
        /// </summary>
        private void ResumeChangeStatesData()
        {
            if (equipmentChangeStatesEntity != null)
            {
                DataTable changeStatesDataTable = equipmentChangeStatesEntity.GetEquipmentChangeStatesDataTable();

                if (changeStatesDataTable != null && changeStatesDataTable.Rows.Count > 0)
                {
                    bool isReset = true;

                    foreach (DataRow row in changeStatesDataTable.Rows)
                    {
                        string fromStateKey = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_FROM_STATE_KEY].ToString();
                        string toStateKey = row[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY].ToString();

                        for (int i = 0; i < this.bandedGridViewChangeStates.RowCount; i++)
                        {
                            string stateKey = this.bandedGridViewChangeStates.GetRowCellValue(i, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY).ToString();

                            foreach (BandedGridColumn column in this.bandedGridViewChangeStates.Columns)
                            {
                                if (column.Tag != null)
                                {
                                    if (isReset)
                                    {
                                        this.bandedGridViewChangeStates.SetRowCellValue(i, column, false);

                                        this.bandedGridViewChangeStates.RefreshRowCell(i, column);
                                    }

                                    if (stateKey == fromStateKey && column.Tag.ToString() == toStateKey)
                                    {
                                        this.bandedGridViewChangeStates.SetRowCellValue(i, column, true);

                                        this.bandedGridViewChangeStates.RefreshRowCell(i, column);
                                    }
                                }
                            }
                        }

                        isReset = false;
                    }
                }
            }
        }

        /// <summary>
        /// Load Equipment States Data
        /// </summary>
        private void LoadEquipmentStatesData()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentStates().GetEquipmentStates(reqDS);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);

                return;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            #region Process Output Parameters

            string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            if (string.IsNullOrEmpty(returnMsg))
            {
                GenerateChangeStatesDataGrid(resDS.Tables[EMS_EQUIPMENT_STATES_FIELDS.DATABASE_TABLE_NAME]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Load Equipment Change States Data
        /// </summary>
        private void LoadEquipmentChangeStatesData()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentChangeStates().GetEquipmentChangeStates(reqDS);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);

                return;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            #region Process Output Parameters

            string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            if (string.IsNullOrEmpty(returnMsg))
            {
                FillChangeStatesData(resDS.Tables[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        #endregion

        #region Controls Events

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            LoadEquipmentStatesData();
            LoadEquipmentChangeStatesData();

            State = ControlState.Read;
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            if (this.bandedGridViewChangeStates.RowCount > 0)
            {
                State = ControlState.Edit;
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (equipmentChangeStatesEntity != null)
            {
                MapControlsToEntity();

                if (equipmentChangeStatesEntity.IsEquipmentChangeStatesUpdated())
                {
                    if (equipmentChangeStatesEntity.Save())
                    {
                        MapEntityToControls();

                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeStates.M0001}", "${res:Global.InformationText}");

                        State = ControlState.Read;
                    }
                    else
                    {
                        equipmentChangeStatesEntity.ResumeEquipmentChangeStatesEntity();
                    }
                }
                else
                {
                    equipmentChangeStatesEntity.ResumeEquipmentChangeStatesEntity();

                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentChangeStates.M0002}", "${res:Global.InformationText}");
                }
            }
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            ResumeChangeStatesData();

            State = ControlState.Read;
        }

        private void grdChangeStates_MouseMove(object sender, MouseEventArgs e)
        {
            GridHitInfo hi = this.bandedGridViewChangeStates.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));

            if (hi != null && hi.InRowCell && hi.Column.Tag != null)
            {
                string leftStateName = this.bandedGridViewChangeStates.GetRowCellValue(hi.RowHandle, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME)==null?"": this.bandedGridViewChangeStates.GetRowCellValue(hi.RowHandle, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME).ToString();
                string topStateName = hi.Column.Name;

                this.toolTipChangeStateName.Show(leftStateName + " -> " + topStateName, this, hi.HitPoint);
            }
            else
            {
                this.toolTipChangeStateName.RemoveAll();
            }
        }

        private void bandedGridViewChangeStates_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (State == ControlState.Edit)
            {
                if (e.Column.Tag != null && this.bandedGridViewChangeStates.GetRowCellValue(e.RowHandle, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY) != null)
                {
                    string topStateKey = e.Column.Tag.ToString();

                    string leftStateKey = this.bandedGridViewChangeStates.GetRowCellValue(e.RowHandle, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY).ToString();

                    if (topStateKey == leftStateKey)
                    {
                        this.bandedGridViewChangeStates.SetRowCellValue(e.RowHandle, e.Column, false);
                    }
                    else
                    {
                        bool currentCellValue = (bool)this.bandedGridViewChangeStates.GetRowCellValue(e.RowHandle, e.Column);

                        this.bandedGridViewChangeStates.SetRowCellValue(e.RowHandle, e.Column, !currentCellValue);
                    }
                }

                this.bandedGridViewChangeStates.RefreshRowCell(e.RowHandle, e.Column);
            }
        }

        private void bandedGridViewChangeStates_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Tag != null && this.bandedGridViewChangeStates.GetRowCellValue(e.RowHandle, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY) != null)
            {
                string topStateKey = e.Column.Tag.ToString();

                string leftStateKey = this.bandedGridViewChangeStates.GetRowCellValue(e.RowHandle, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY).ToString();

                if (topStateKey == leftStateKey)
                {
                    #region Disable Grid Cell

                    GridCellInfo cellInfo = e.Cell as GridCellInfo;

                    CheckEditViewInfo checkEditInfo = cellInfo.ViewInfo as CheckEditViewInfo;

                    checkEditInfo.AllowOverridedState = true;
                    checkEditInfo.OverridedState = ObjectState.Disabled;
                    checkEditInfo.CalcViewInfo(e.Graphics);

                    #endregion

                    #region Set Grid Cell Color

                    e.Appearance.BackColor = Color.OrangeRed;
                    e.Appearance.Options.UseBackColor = true;

                    #endregion
                }
            }
        }
        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        #endregion
    }
}
