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
using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraGrid.Columns;
using FanHai.Gui.Core;
using DevExpress.XtraEditors.Repository;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Share.Interface;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using DevExpress.XtraLayout.Utils;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.EMS
{
    /// <summary>
    /// Equipments GUI
    /// </summary>
    public partial class Equipments : BaseUserCtrl
    {
        #region EquipmentEntity Object
        private const string REAL_EQUIPMENT_DEFAULT_VALUE = " ";
        private EquipmentEntity equipmentEntity = null;
        private DataTable parentEquipmentTable = null;
        DataSet dsStateEvent = new DataSet();      

        #endregion

        #region Constructor

        public Equipments()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events
        /// <summary>
        /// Load登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Equipments_Load(object sender, EventArgs e)
        {
            afterStateChanged += new AfterStateChanged(Equipments_afterStateChanged);
            //
            LoadEquipmentTypeData();                      //设备类型数据绑定
            LoadEquipmentGroupData();                     //绑定设备组数据
            LoadEquipmentLocationData();                  //厂区区域数据绑定
            LoadEquipmentStateData();                     //设备状态数据绑定
            LoadEquipmentChangeStateData();               //设备状态改变数据绑定
            BindEquipmentDataToControl();
            //状态为Read
            State = ControlState.Read;
        }

        private void Equipments_afterStateChanged()
        {
            switch (State)
            {
                case ControlState.Read:
                    this.tsbNew.Enabled = true;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = true;

                    this.txtEquipmentName.Enabled = false;
                    this.txtDescription.Enabled = false;
                    this.txtEquipmentCode.Enabled = false;
                    this.txtEquipmentMode.Enabled = false;
                    this.txtMaxQuantity.Enabled = false;
                    this.txtMinQuantity.Enabled = false;
                    this.cmbEquipmentType.Enabled = false;
                    this.cmbEquipmentGroup.Enabled = false;
                    this.cmbEquipmentLocation.Enabled = false;
                    this.chkIsBatch.Enabled = false;
                    this.chkIsChamber.Enabled = false;
                    this.chKIsMultiChamber.Enabled = false;
                    this.cmbParentEquipment.Enabled = false;
                    this.cmbChamberIndex.Enabled = false;
                    this.txtChamberTotal.Enabled = false;

                    this.txtAssetsNo.Enabled = false;
                    this.txtAvTime.Enabled = false;
                    this.txtWph.Enabled = false;
                    this.txtTractTime.Enabled = false;
                    this.lueRealEquipment.Enabled = false;
                    break;
                case ControlState.ReadOnly:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = false;

                    this.txtEquipmentName.Enabled = false;
                    this.txtDescription.Enabled = false;
                    this.txtEquipmentCode.Enabled = false;
                    this.txtEquipmentMode.Enabled = false;
                    this.txtMaxQuantity.Enabled = false;
                    this.txtMinQuantity.Enabled = false;
                    this.cmbEquipmentType.Enabled = false;
                    this.cmbEquipmentGroup.Enabled = false;
                    this.cmbEquipmentLocation.Enabled = false;
                    this.chkIsBatch.Enabled = false;
                    this.chkIsChamber.Enabled = false;
                    this.chKIsMultiChamber.Enabled = false;
                    this.cmbParentEquipment.Enabled = false;
                    this.cmbChamberIndex.Enabled = false;
                    this.txtChamberTotal.Enabled = false;

                    this.txtAssetsNo.Enabled = false;
                    this.txtAvTime.Enabled = false;
                    this.txtWph.Enabled = false;
                    this.txtTractTime.Enabled = false;
                    this.lueRealEquipment.Enabled = false;
                    break;
                case ControlState.New:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtEquipmentName.Enabled = true;
                    this.txtDescription.Enabled = true;
                    this.txtEquipmentCode.Enabled = true;
                    this.txtEquipmentMode.Enabled = true;
                    this.txtMaxQuantity.Enabled = true;
                    this.txtMinQuantity.Enabled = true;
                    this.cmbEquipmentType.Enabled = true;
                    this.cmbEquipmentGroup.Enabled = true;
                    this.cmbEquipmentLocation.Enabled = true;
                    this.chkIsBatch.Enabled = true;
                    this.chkIsChamber.Enabled = true;
                    this.chKIsMultiChamber.Enabled = true;
                    this.cmbParentEquipment.Enabled = true;
                    this.cmbChamberIndex.Enabled = true;
                    this.txtChamberTotal.Enabled = true;

                    this.txtEquipmentName.Text = string.Empty;
                    this.txtDescription.Text = string.Empty;
                    this.txtEquipmentCode.Text = string.Empty;
                    this.txtEquipmentMode.Text = string.Empty;
                    this.txtMaxQuantity.EditValue = 0;
                    this.txtMinQuantity.EditValue = 0;

                    this.txtAssetsNo.Enabled = true;
                    this.txtAvTime.Enabled = true;
                    this.txtWph.Enabled = true;
                    this.txtTractTime.Enabled = true;
                    this.lueRealEquipment.Enabled = true;
                    //Initial Equipment Type is Physical
                    this.cmbEquipmentType.EditValue = "物理设备";

                    this.cmbEquipmentGroup.EditValue = string.Empty;
                    this.cmbEquipmentLocation.EditValue = string.Empty;
                    this.chkIsBatch.Checked = false;
                    this.chkIsChamber.Checked = false;
                    this.chKIsMultiChamber.Checked = false;
                    this.cmbParentEquipment.EditValue = string.Empty;                    
                    this.cmbEquipmentChangeState.EditValue = string.Empty;
                    this.lueRealEquipment.EditValue = REAL_EQUIPMENT_DEFAULT_VALUE;
                    this.txtTractTime.Text = string.Empty;
                    this.txtWph.Text = string.Empty;
                    this.txtAvTime.Text = string.Empty;
                    this.txtAssetsNo.Text = string.Empty;

                    break;
                case ControlState.Edit:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtEquipmentName.Enabled = true;
                    this.txtDescription.Enabled = true;
                    this.txtEquipmentCode.Enabled = true;
                    this.txtEquipmentMode.Enabled = true;
                    this.txtMaxQuantity.Enabled = true;
                    this.txtMinQuantity.Enabled = true;
                    this.cmbEquipmentType.Enabled = true;
                    this.cmbEquipmentGroup.Enabled = true;
                    this.cmbEquipmentLocation.Enabled = true;
                    this.chkIsBatch.Enabled = true;
                    this.chkIsChamber.Enabled = true;
                    this.chKIsMultiChamber.Enabled = true;
                    this.cmbParentEquipment.Enabled = true;
                    this.cmbChamberIndex.Enabled = true;
                    this.txtChamberTotal.Enabled = true;
                    this.lueRealEquipment.Enabled = true;
                    if (string.IsNullOrEmpty(this.txtAssetsNo.Text.Trim()))
                        this.txtAssetsNo.Enabled = true;
                    else
                        this.txtAssetsNo.Enabled = false;

                    this.txtAvTime.Enabled = true;
                    this.txtWph.Enabled = true;
                    this.txtTractTime.Enabled = true;

                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load Equipment Type Data 设备类型数据绑定
        /// </summary>
        private void LoadEquipmentTypeData()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Build Input Parameters

            DataTable dataTable = AddinCommonStaticFunction.GetTwoColumnsDataTable(); //定义数据表两列 Key和Value

            dataTable.TableName = TRANS_TABLES.TABLE_PARAM;          //表名PARAM

            dataTable.Rows.Add(new object[] { "Name", EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE });     //EQUIPMENT_TYPE

            dataTable.AcceptChanges();

            reqDS.Tables.Add(dataTable);

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateICrmAttributeEngine().GetSpecifyAttributeData(reqDS); //获取设备类型传入参数为reqds数据集
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
                BindDataToEquipmentTypeList(resDS.Tables[TRANS_TABLES.TABLE_UDAS]);     //UDAS
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Bind Data to Equipment Type List
        /// </summary>
        /// <param name="dataTable"></param>
        private void BindDataToEquipmentTypeList(DataTable dataTable)
        {
            this.cmbEquipmentType.Properties.DataSource = null;
            this.cmbEquipmentType.Properties.DataSource = dataTable;
        }

        /// <summary>
        /// Load Equipment Group Data
        /// </summary>
        private void LoadEquipmentGroupData()
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
                    resDS = serverFactory.CreateIEquipmentGroups().GetEquipmentGroups(reqDS);    //获取设备组信息
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
                BindDataToEquipmentGroupList(resDS.Tables[EMS_EQUIPMENT_GROUPS_FIELDS.DATABASE_TABLE_NAME]);    //绑定设备组控件数据方法
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Bind Data to Equipment Group List
        /// </summary>
        /// <param name="dataTable"></param>
        private void BindDataToEquipmentGroupList(DataTable dataTable)
        {
            this.cmbEquipmentGroup.Properties.DataSource = null;
            this.cmbEquipmentGroup.Properties.DataSource = dataTable;    //设备组控件数据绑定
        }

        /// <summary>
        /// Load Equipment Location Data
        /// </summary>
        private void LoadEquipmentLocationData()
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
                    resDS = serverFactory.CreateILocationEngine().GetAllLoactions(reqDS);        //获取厂区 区域
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
                BindDataToEquipmentLocationList(resDS.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Bind Data To Equipment Location List
        /// </summary>
        /// <param name="dataTable"></param>
        private void BindDataToEquipmentLocationList(DataTable dataTable)
        {
            this.cmbEquipmentLocation.Properties.DataSource = null;
            this.cmbEquipmentLocation.Properties.DataSource = dataTable;
        }

        /// <summary>
        /// Load Equipment State Data  设备状态数据绑定
        /// </summary>
        private void LoadEquipmentStateData()
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
                    resDS = serverFactory.CreateIEquipmentStates().GetEquipmentStates(reqDS);   //获取设备状态
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
                BindDataToEquipmentStateList(resDS.Tables[EMS_EQUIPMENT_STATES_FIELDS.DATABASE_TABLE_NAME]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Bind Data To Equipment State List
        /// </summary>
        /// <param name="dataTable"></param>
        private void BindDataToEquipmentStateList(DataTable dataTable)
        {
            this.cmbEquipmentState.Properties.DataSource = null;
            this.cmbEquipmentState.Properties.DataSource = dataTable;

            DataRow[] drArr = dataTable.Select("EQUIPMENT_STATE_NAME='LOST'");
            if (drArr.Length > 0)
                this.cmbEquipmentState.EditValue = drArr[0][EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY].ToString();
        }

        /// <summary>
        /// Load Equipment Change State Data
        /// </summary>
        private void LoadEquipmentChangeStateData()
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
                    resDS = serverFactory.CreateIEquipmentChangeStates().GetEquipmentChangeStates(reqDS);     //获取状态改变
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
                BindDataToEquipmentChangeStateList(resDS.Tables[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.DATABASE_TABLE_NAME]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Bind Data To Equipment Change State List
        /// </summary>
        /// <param name="dataTable"></param>
        private void BindDataToEquipmentChangeStateList(DataTable dataTable)
        {
            this.cmbEquipmentChangeState.Properties.DataSource = null;
            this.cmbEquipmentChangeState.Properties.DataSource = dataTable;
        }

        /// <summary>
        /// Load Parent Equipment Data
        /// </summary>
        private void LoadParentEquipmentData()
        {
            #region Variables

            DataSet resDS = new DataSet();

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipments().GetAllParentEquipments();
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
                BindDataToParentEquipmentList(resDS.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME]);
                parentEquipmentTable = resDS.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Bind Data To Parent Equipment List
        /// </summary>
        /// <param name="dataTable"></param>
        private void BindDataToParentEquipmentList(DataTable dataTable)
        {
            this.cmbParentEquipment.Properties.DataSource = null;
            this.cmbParentEquipment.Properties.DataSource = dataTable;
        }

        /// <summary>
        /// Load Equipment Data  获取数据
        /// </summary>
        /// <param name="equipmentName"></param>
        private void LoadEquipmentsData(string equipmentName)
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
                    int pages, records, pageNo, pageSize; 
                    this.paginationEquipments.GetPaginationProperties(out pageNo, out pageSize);

                    if (pageNo <= 0)
                    {
                        pageNo = 1;
                    }

                    if(pageSize <= 0)
                    {
                        pageSize = PaginationControl.DEFAULT_PAGESIZE;                          //每页行数DEFAULT_PAGESIZE=20
                    }
                    //获取数据
                    if (!equipmentName.Trim().Equals(""))
                        equipmentName = equipmentName.Trim().ToUpper();

                    resDS = serverFactory.CreateIEquipments().GetEquipments(reqDS, equipmentName, pageNo, pageSize, out pages, out records);
                    try
                    {
                        if (pages > 0 && records > 0)
                        {
                            this.paginationEquipments.PageNo = pageNo > pages ? pages : pageNo;
                            this.paginationEquipments.PageSize = pageSize;
                            this.paginationEquipments.Pages = pages;
                            this.paginationEquipments.Records = records;
                        }
                        else
                        {
                            this.paginationEquipments.PageNo = 0;
                            this.paginationEquipments.PageSize = PaginationControl.DEFAULT_PAGESIZE;
                            this.paginationEquipments.Pages = 0;
                            this.paginationEquipments.Records = 0;
                        }
                    }
                    catch //(Exception ex)
                    { }
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
                //绑定返回表的数据到设备维护数据表中
                BindDataToEquipmentsGrid(resDS.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Bind Data to Equipments Grid
        /// </summary>
        /// <param name="dataTable"></param>
        private void BindDataToEquipmentsGrid(DataTable dataTable)
        {
            this.grdEquipments.MainView = this.grdViewEquipments;
            this.grdEquipments.DataSource = null;
            this.grdEquipments.DataSource = dataTable;
        }

        #endregion

        #region Override Methods

        protected override void InitUIControls()
        {
            LookUpColumnInfo columnInfo = null;

            #region Equipment Type List

            this.cmbEquipmentType.Properties.Columns.Clear();

            columnInfo = new LookUpColumnInfo(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE);

            this.cmbEquipmentType.Properties.Columns.Add(columnInfo);
            this.cmbEquipmentType.Properties.DisplayMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE;
            this.cmbEquipmentType.Properties.ValueMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE;

            #endregion

            #region Equipment Group List

            this.cmbEquipmentGroup.Properties.Columns.Clear();

            columnInfo = new LookUpColumnInfo(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME);

            this.cmbEquipmentGroup.Properties.Columns.Add(columnInfo);
            this.cmbEquipmentGroup.Properties.DisplayMember = EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME;
            this.cmbEquipmentGroup.Properties.ValueMember = EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY;

            #endregion

            #region Equipment Location List

            this.cmbEquipmentLocation.Properties.Columns.Clear();

            columnInfo = new LookUpColumnInfo(FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME);

            this.cmbEquipmentLocation.Properties.Columns.Add(columnInfo);
            this.cmbEquipmentLocation.Properties.DisplayMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;
            this.cmbEquipmentLocation.Properties.ValueMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY;

            #endregion

            #region Parent Equipment List

            this.cmbParentEquipment.Properties.Columns.Clear();

            columnInfo = new LookUpColumnInfo(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);

            this.cmbParentEquipment.Properties.Columns.Add(columnInfo);
            this.cmbParentEquipment.Properties.DisplayMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME;
            this.cmbParentEquipment.Properties.ValueMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY;

            this.cmbParentEquipment.Visible = false;

            #endregion

            #region Equipment State List

            this.cmbEquipmentState.Properties.Columns.Clear();

            columnInfo = new LookUpColumnInfo(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME);

            this.cmbEquipmentState.Properties.Columns.Add(columnInfo);
            this.cmbEquipmentState.Properties.DisplayMember = EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME;
            this.cmbEquipmentState.Properties.ValueMember = EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY;

            this.cmbEquipmentState.Enabled = false;

            #endregion

            #region Equipment Change State List

            this.cmbEquipmentChangeState.Properties.Columns.Clear();

            columnInfo = new LookUpColumnInfo(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME);

            this.cmbEquipmentChangeState.Properties.Columns.Add(columnInfo);
            this.cmbEquipmentChangeState.Properties.DisplayMember = EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME;
            this.cmbEquipmentChangeState.Properties.ValueMember = EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY;

            this.cmbEquipmentChangeState.Enabled = false;

            #endregion

            #region Equipment Grid

            this.grdViewEquipments.Columns.Clear();

            EMS_EQUIPMENTS_FIELDS equipmentsFields = new EMS_EQUIPMENTS_FIELDS();

            int index = 0;

            GridColumn column = new GridColumn();

            column.Name = "RN";
            column.FieldName = "RN";
            column.Visible = true;
            column.VisibleIndex = index++;

            this.grdViewEquipments.Columns.Add(column);

            foreach (KeyValuePair<string, FieldProperties> field in equipmentsFields.FIELDS)
            {
                column = new GridColumn();

                column.Name = field.Key;
                column.FieldName = field.Key;
                column.Visible = true;
                column.VisibleIndex = index++;

                this.grdViewEquipments.Columns.Add(column);
            }

            RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

            checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);

            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER].ColumnEdit = checkEdit;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH].ColumnEdit = checkEdit;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER].ColumnEdit = checkEdit;           

            #endregion
        }

        protected override void InitUIResourcesByCulture()
        {
            GridViewHelper.SetGridView(grdViewEquipments);
            this.tsbNew.Text = StringParser.Parse("${res:Global.New}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbCancel.Text = StringParser.Parse("${res:Global.Cancel}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");

            //tsbNew.Image = (Image)ResourceService.GetImageResource("Icons.32x32.EmptyFileIcon");
            //tsbSave.Image = (Image)ResourceService.GetImageResource("Icons.16x16.SaveIcon");
            //tsbCancel.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Cancel");
            //tsbDelete.Image = (Image)ResourceService.GetImageResource("Icons.16x16.DeleteIcon");
            //this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.Name}");

            this.grpEquipmentQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentQuery}");
            //this.lblQueryName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentName}");
            this.lblQueryName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0001}");//"设备编码";
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.grpEquipmentInfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentInfo}");
            this.lblEquipmentName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentName}");
            this.lblDescription.Text = StringParser.Parse("${res:Global.Description}");
            this.lblEquipmentCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentCode}");
            this.lblEquipmentMode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentMode}");
            this.lblMaxQuantity.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.MaxQuantity}");
            this.lblMinQuantity.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.MinQuantity}");
            this.lblEquipmentType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentType}");
            this.lblEquipmentGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentGroup}");
            this.chkIsBatch.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.IsBatch}");
            this.lblEquipmentLocation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentLocation}");
            this.chkIsChamber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0002}");//"是否腔体"
            this.lblParentEquipment.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.ParentEquipment}");
            this.lblParentEquipment.Visibility = LayoutVisibility.Never;
            this.lblEquipmentState.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentState}");
            this.lblEquipmentChangeState.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentChangeState}");

            this.cmbEquipmentType.Properties.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentType}");
            this.cmbEquipmentGroup.Properties.Columns[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentGroup}");
            this.cmbEquipmentLocation.Properties.Columns[FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentLocation}");
            this.cmbParentEquipment.Properties.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentName}");
            this.cmbEquipmentState.Properties.Columns[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentState}");
            this.cmbEquipmentChangeState.Properties.Columns[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentChangeState}");

            this.grdViewEquipments.Columns["RN"].Caption = StringParser.Parse("${res:Global.RowNumber}");
            this.grdViewEquipments.Columns["RN"].Width = 35;

            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_ASSETSNO].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0003}"); //"资产编号";
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentName}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION].Caption = StringParser.Parse("${res:Global.Description}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentType}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentCode}");

            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.EquipmentMode}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.MinQuantity}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.MaxQuantity}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_STATE_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0002}"); //"是否腔体"
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.IsBatch}");
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0004}"); //"是否多腔体设备";
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0005}"); //"腔体数量";
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0006}"); //"腔体编号";            
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_AV_TIME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0007}"); //"设备AvTime";
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TRACT_TIME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0008}"); //"设备TractTime";
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_WPH].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.lbl.0009}"); //"设备WPH值";
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CREATOR].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIME].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EDITOR].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIME].Visible = false;
            this.grdViewEquipments.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_REAL_KEY].Visible = false;
        }

        protected override void InitUIAuthoritiesByUser()
        {
            
        }
        /// <summary>
        /// 赋值equipmentEntity对象的值
        /// </summary>
        protected override void MapControlsToEntity()
        {
            if (equipmentEntity != null)
            {
                equipmentEntity.ClearData();

                switch (State)
                {
                    case ControlState.New:
                        equipmentEntity.EquipmentKey =  CommonUtils.GenerateNewKey(0);

                        equipmentEntity.EquipmentStateKey = this.cmbEquipmentState.EditValue.ToString();

                        equipmentEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                        equipmentEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                        equipmentEntity.CreateTime = string.Empty;
                        //获得初始化设备事件号
                        DataSet dsStates = equipmentEntity.GetInitEquipmentChangeState();
                        DataRow drState = dsStates.Tables[EMS_EQUIPMENT_STATES_FIELDS.DATABASE_TABLE_NAME].Rows[0];
                        equipmentEntity.EquipmentChangeStateKey = drState[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].ToString();
                        dsStateEvent.Tables.Clear();
                       DataTable dtInitState = EMS_STATE_EVENT_FIELDS.CreateDataTable();
                        DataTable dtStateEventUpdate = dtInitState.Clone();
                        dtStateEventUpdate.TableName = EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME_UPDATE;
                        DataRow drInitState = dtInitState.NewRow();
                        drInitState[EMS_STATE_EVENT_FIELDS.EVENT_KEY] =  CommonUtils.GenerateNewKey(0);
                        drInitState[EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY] = equipmentEntity.EquipmentKey;
                        drInitState[EMS_STATE_EVENT_FIELDS.EQUIPMENT_CHANGE_STATE_KEY] = equipmentEntity.EquipmentChangeStateKey;
                        drInitState[EMS_STATE_EVENT_FIELDS.EQUIPMENT_FROM_STATE_KEY] = drState[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_FROM_STATE_KEY].ToString();
                        drInitState[EMS_STATE_EVENT_FIELDS.EQUIPMENT_TO_STATE_KEY] = drState[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY].ToString();
                        drInitState[EMS_STATE_EVENT_FIELDS.DESCRIPTION] = drState[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString();
                        drInitState[EMS_STATE_EVENT_FIELDS.ISCURRENT] = "0";
                        drInitState[EMS_STATE_EVENT_FIELDS.CREATOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                        drInitState[EMS_STATE_EVENT_FIELDS.EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                        dtInitState.Rows.Add(drInitState);
                        dsStateEvent.Tables.Add(dtInitState);
                        DataRow drStateEventUpdate = dtStateEventUpdate.NewRow();
                        drStateEventUpdate[EMS_STATE_EVENT_FIELDS.EVENT_KEY] = CommonUtils.GenerateNewKey(0);
                        drStateEventUpdate[EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY] = equipmentEntity.EquipmentKey;
                        drStateEventUpdate[EMS_STATE_EVENT_FIELDS.EQUIPMENT_FROM_STATE_KEY] = drState[EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_TO_STATE_KEY].ToString();
                        drStateEventUpdate[EMS_STATE_EVENT_FIELDS.ISCURRENT] = "1";
                        drStateEventUpdate[EMS_STATE_EVENT_FIELDS.CREATOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                        dtStateEventUpdate.Rows.Add(drStateEventUpdate);
                        dsStateEvent.Tables.Add(dtStateEventUpdate);
                        break;
                    case ControlState.Edit:
                    case ControlState.Delete:
                        equipmentEntity.EquipmentKey = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY).ToString();
                        equipmentEntity.EquipmentName = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME).ToString();
                        equipmentEntity.Description = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION).ToString();
                        equipmentEntity.EquipmentCode = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE).ToString();
                        equipmentEntity.EquipmentMode = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE).ToString();
                        equipmentEntity.MaxQuantity = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY).ToString();
                        equipmentEntity.MinQuantity = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY).ToString();
                        equipmentEntity.EquipmentType = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE).ToString();
                        equipmentEntity.EquipmentGroupKey = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY).ToString();
                        equipmentEntity.EquipmentLocationKey = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY).ToString();
                        equipmentEntity.IsBatch = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH).ToString();
                        equipmentEntity.IsChamber = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER).ToString();
                        equipmentEntity.IsMultiChamber = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER).ToString();
                        equipmentEntity.ChamberIndex = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX).ToString();
                        equipmentEntity.ChamberTotal = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL).ToString();

                        equipmentEntity.ParentEquipmentKey = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY).ToString();

                        equipmentEntity.Editor = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EDITOR).ToString();
                        equipmentEntity.EditTimeZone = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIMEZONE_KEY).ToString();
                        equipmentEntity.EditTime = this.grdViewEquipments.GetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIME).ToString();
                        break;
                }

                equipmentEntity.IsInitializeFinished = true;

                equipmentEntity.EquipmentName = this.txtEquipmentName.Text.Trim();
                equipmentEntity.Description = this.txtDescription.Text.Trim();
                equipmentEntity.EquipmentCode = this.txtEquipmentCode.Text.Trim().ToUpper();
                equipmentEntity.EquipmentMode = this.txtEquipmentMode.Text.Trim();
                equipmentEntity.MaxQuantity = this.txtMaxQuantity.Text.Trim();
                equipmentEntity.MinQuantity = this.txtMinQuantity.Text.Trim();
                equipmentEntity.EquipmentType = this.cmbEquipmentType.EditValue.ToString();
                equipmentEntity.EquipmentGroupKey = this.cmbEquipmentGroup.EditValue.ToString();
                equipmentEntity.EquipmentLocationKey = this.cmbEquipmentLocation.EditValue.ToString();
                equipmentEntity.IsBatch = this.chkIsBatch.Checked ? "1" : "0";

                equipmentEntity.Equipment_AssetsNo = txtAssetsNo.Text.Trim();
                equipmentEntity.Equipment_Av_Time = txtAvTime.Text.Trim();
                equipmentEntity.Equipment_Tract_Time = txtTractTime.Text.Trim();
                equipmentEntity.Equipment_WPH = txtWph.Text.Trim();
                equipmentEntity.EquipmentRealKey = Convert.ToString(this.lueRealEquipment.EditValue);
                if (string.IsNullOrEmpty(equipmentEntity.EquipmentRealKey))
                {
                    equipmentEntity.EquipmentRealKey = REAL_EQUIPMENT_DEFAULT_VALUE;
                }
                if (this.chkIsChamber.Checked)
                {
                    equipmentEntity.IsChamber = "1";
                    equipmentEntity.ParentEquipmentKey = this.cmbParentEquipment.EditValue.ToString();
                    equipmentEntity.ChamberIndex = this.cmbChamberIndex.Text;
                }
                else
                {
                    equipmentEntity.IsChamber = "0";
                    equipmentEntity.ParentEquipmentKey = string.Empty;
                    equipmentEntity.ChamberIndex ="0";
                }

                if (this.chKIsMultiChamber.Checked)
                {
                    equipmentEntity.IsMultiChamber = "1";
                    equipmentEntity.ChamberTotal = this.txtChamberTotal.Text.Trim();
                }
                else
                {
                    equipmentEntity.IsMultiChamber = "0";
                    equipmentEntity.ChamberTotal = "0";
                }
            }
        }

        protected override void MapEntityToControls()
        {
            if (equipmentEntity != null)
            {
                switch (State)
                {
                    case ControlState.New:
                        this.grdViewEquipments.AddNewRow();

                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentEntity.EquipmentKey);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME, equipmentEntity.EquipmentName);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION, equipmentEntity.Description);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE, equipmentEntity.EquipmentCode);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE, equipmentEntity.EquipmentMode);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY, equipmentEntity.MaxQuantity);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY, equipmentEntity.MinQuantity);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE, equipmentEntity.EquipmentType);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY, equipmentEntity.EquipmentGroupKey);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY, equipmentEntity.EquipmentLocationKey);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH, equipmentEntity.IsBatch);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER, equipmentEntity.IsChamber);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER, equipmentEntity.IsMultiChamber);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL, equipmentEntity.ChamberTotal);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX, equipmentEntity.ChamberIndex);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY, equipmentEntity.ParentEquipmentKey);

                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_STATE_KEY, equipmentEntity.EquipmentStateKey);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_REAL_KEY, equipmentEntity.EquipmentRealKey);

                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_CREATOR, equipmentEntity.Creator);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY, equipmentEntity.CreateTimeZone);
                        this.grdViewEquipments.SetFocusedRowCellValue(EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIME, equipmentEntity.CreateTime);

                        this.grdViewEquipments.UpdateCurrentRow();
                        break;
                    case ControlState.Edit:
                        foreach (KeyValuePair<string, DirtyItem> keyValue in equipmentEntity.DirtyList)
                        {
                            this.grdViewEquipments.SetFocusedRowCellValue(keyValue.Key, keyValue.Value.FieldNewValue);
                        }

                        this.grdViewEquipments.UpdateCurrentRow();
                        break;
                    case ControlState.Delete:
                        this.grdViewEquipments.DeleteRow(this.grdViewEquipments.FocusedRowHandle);

                        this.grdViewEquipments.UpdateCurrentRow();
                        break;
                }

                grdViewEquipments_FocusedRowChanged(this.grdViewEquipments, new FocusedRowChangedEventArgs(-1, this.grdViewEquipments.FocusedRowHandle));
            }
        }

        #endregion

        #region Control Events

        private void paginationEquipments_DataPaging()
        {
            LoadEquipmentsData(this.txtQueryValue.Text.Trim());
            //修改状态为Read
            State = ControlState.Read;
            //获取行焦点
            grdViewEquipments_FocusedRowChanged(this.grdViewEquipments, new FocusedRowChangedEventArgs(-1, this.grdViewEquipments.FocusedRowHandle));
        }
        /// <summary>
        /// 是否胚体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkIsChamber_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkIsChamber.Checked)
            {
                LoadParentEquipmentData();

                this.lblParentEquipment.Visibility = LayoutVisibility.Always;
                this.cmbParentEquipment.Visible = true;
                this.chKIsMultiChamber.Enabled = false;
            }
            else
            {
                this.lblParentEquipment.Visibility = LayoutVisibility.Never;
                this.cmbParentEquipment.Visible = false;
                this.lblChamberIndex.Visibility = LayoutVisibility.Always;
                this.cmbChamberIndex.Visible = false;
                if (State == ControlState.Edit || State == ControlState.New)
                {
                    this.chKIsMultiChamber.Enabled = true;
                }
            }
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            //初始化对象数值
            equipmentEntity = new EquipmentEntity();
            //状态为修改为new
            State = ControlState.New;
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtEquipmentName.Text))
            {
                //设备名称不允许为空!
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.Equipments.M0001}", "${res:Global.InformationText}");
                this.txtEquipmentName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.cmbEquipmentType.Text))
            {
                //设备类型不允许为空!
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.Equipments.M0002}", "${res:Global.InformationText}");
                this.cmbEquipmentType.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtEquipmentCode.Text))
            {
                //设备编码不允许为空!
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.msg.0001}"), "${res:Global.InformationText}");//设备编码不许为空
                this.txtEquipmentCode.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.cmbEquipmentGroup.Text))
            {
                //设备组不允许为空!
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.Equipments.M0003}", "${res:Global.InformationText}");
                this.cmbEquipmentGroup.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.cmbEquipmentLocation.Text))
            {
                //设备区域不允许为空!
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.Equipments.M0004}", "${res:Global.InformationText}");
                this.cmbEquipmentLocation.Focus();
                return;
            }
            if (!string.IsNullOrEmpty(this.txtMinQuantity.Text.Trim()) && !string.IsNullOrEmpty(this.txtMaxQuantity.Text.Trim()))
            {
                if (Convert.ToInt32(this.txtMinQuantity.Text) > Convert.ToInt32(this.txtMaxQuantity.Text))
                {
                    //设备最小加工量不允许大于最大加工量!
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.Equipments.M0005}", "${res:Global.InformationText}");
                    this.txtMinQuantity.Focus();
                    return;
                }
            }
            if (this.chkIsChamber.Checked)
            {
                if (this.txtEquipmentName.Text.Trim() == this.cmbParentEquipment.Text.Trim())
                {
                    //设备不能选择自已为父设备!
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.Equipments.M0012}", "${res:Global.InformationText}");
                    this.cmbParentEquipment.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(this.cmbParentEquipment.Text))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.msg.0002}"));//"父设备不能为空!"
                    this.cmbParentEquipment.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(cmbChamberIndex.Text))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.msg.0003}"));//"腔体编号不能为空!"
                    this.cmbChamberIndex.Focus();
                    return;
                }
               
            }
            if (this.chKIsMultiChamber.Checked)
            {
                if (string.IsNullOrEmpty(txtChamberTotal.Text.Trim()))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.msg.0004}"));//"腔体数量不能为空!"
                    this.txtChamberTotal.Focus();
                    return;
                }
            }

            if (txtAssetsNo.Text.Trim().Length > 30)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.msg.0005}"));//"资产编号最大长度为30!"
                this.txtAssetsNo.Focus();
                return;
            }

            if (txtAvTime.Text.Trim().Length > 0)
            {
                if (Convert.ToDouble(txtAvTime.Text.Trim()) > 999)
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.msg.0006}"));//"设备实际AvTime设定不合理!"
                    this.txtAvTime.Focus();
                    return;
                }
                if (txtAvTime.Text.Trim().Length > 5)
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.msg.0007}"));//"设备AvTime最大长度为5!"
                    this.txtAvTime.Focus();
                    return;
                }
            }
           
            if ( txtTractTime.Text.Trim().Length > 8)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.msg.0008}"));//"设备TractTime最大长度为8!"
                this.txtTractTime.Focus();
                return;
            }

            if (equipmentEntity != null)
            {
                MapControlsToEntity();

                if (State == ControlState.New)
                {
                    if (dsStateEvent.Tables.Count < 1)
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.msg.0009}"));//"事件关联数据报错,请与管理员联系!"
                        return;
                    }
                    if (equipmentEntity.Insert(dsStateEvent))
                    {
                        //MapEntityToControls();
                        //增加数据成功!
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.Equipments.M0006}", "${res:Global.InformationText}");

                        //State = ControlState.Read;

                        paginationEquipments_DataPaging();
                    }
                }
                else if (State == ControlState.Edit)
                {
                    if (equipmentEntity.IsDirty)
                    {
                        if (equipmentEntity.Update())
                        {
                            MapEntityToControls();
                            //更新数据成功!
                            MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.Equipments.M0007}", "${res:Global.InformationText}");

                            State = ControlState.Read;
                        }
                    }
                    else
                    {
                        //当前设备没有数据修改!
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.Equipments.M0008}", "${res:Global.InformationText}");
                    }
                }
            }
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbCancel_Click(object sender, EventArgs e)
        {
            equipmentEntity = null;
            //状态为Read
            State = ControlState.Read;
            //焦点行信息改变
            grdViewEquipments_FocusedRowChanged(this.grdViewEquipments, new FocusedRowChangedEventArgs(-1, this.grdViewEquipments.FocusedRowHandle));
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (this.grdViewEquipments.FocusedRowHandle < 0)
            {//没有选中行
                //请选择需要删除的数据!
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.Equipments.M0009}", "${res:Global.InformationText}");

                return;
            }

            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.EMS.Equipments.M0010}", "${res:Global.QuestionText}"))
            {//系统提示确定删除吗?返回值为true
                equipmentEntity = new EquipmentEntity();
                //状态改为delete
                State = ControlState.Delete;
                //赋值equipmentEntity对象的值
                MapControlsToEntity();

                if (equipmentEntity.Delete())
                {//删除成功返回true
                    //MapEntityToControls();
                    //删除数据成功!
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.Equipments.M0011}", "${res:Global.InformationText}");

                    //State = ControlState.Read;

                    paginationEquipments_DataPaging();
                }
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            //查询数据传入参数为设备名称
            LoadEquipmentsData(this.txtQueryValue.Text.Trim());
            //状态修改为Read
            State = ControlState.Read;
            //行焦点改变事件获取选中行数据到控件中
            grdViewEquipments_FocusedRowChanged(this.grdViewEquipments, new FocusedRowChangedEventArgs(-1, this.grdViewEquipments.FocusedRowHandle));
        }

        private void grdEquipments_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo gridHitInfo = this.grdViewEquipments.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));

            if (gridHitInfo.RowHandle >= 0)
            {
                equipmentEntity = new EquipmentEntity();
                //修改状态为edit
                State = ControlState.Edit;
                //行焦点改变事件
                grdViewEquipments_FocusedRowChanged(this.grdViewEquipments, new FocusedRowChangedEventArgs(-1, this.grdViewEquipments.FocusedRowHandle));
            }
        }

        private void grdViewEquipments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {//选中行数据
                this.txtEquipmentName.Text = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME).ToString();
                this.txtDescription.Text = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION).ToString();
                this.txtEquipmentCode.Text = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE).ToString();
                this.txtEquipmentMode.Text = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE).ToString();
                this.txtMaxQuantity.EditValue = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY);
                this.txtMinQuantity.EditValue = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY);
                this.cmbEquipmentType.EditValue = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE);
                this.cmbEquipmentGroup.EditValue = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY);
                this.cmbEquipmentLocation.EditValue = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY);
                this.chkIsBatch.Checked = Convert.ToInt32(this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH)) == 1 ? true : false;
                this.chkIsChamber.Checked = Convert.ToInt32(this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER)) == 1 ? true : false;
                this.cmbParentEquipment.EditValue = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY);
                this.chKIsMultiChamber.Checked = Convert.ToInt32(this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER)) == 1 ? true : false;
                this.txtChamberTotal.Text =this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL).ToString();
                this.cmbChamberIndex.Text = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX).ToString();
                this.txtWph.Text = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_WPH).ToString();
                this.txtTractTime.Text = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TRACT_TIME).ToString();
                this.txtAvTime.Text = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_AV_TIME).ToString();
                this.txtAssetsNo.Text = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_ASSETSNO).ToString();
                this.lueRealEquipment.EditValue = Convert.ToString(this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_REAL_KEY));
                if (this.State != ControlState.New)
                {//状态不为new
                    this.cmbEquipmentState.EditValue = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_STATE_KEY);
                    this.cmbEquipmentChangeState.EditValue = this.grdViewEquipments.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY);
                }
                if (State == ControlState.Edit || State == ControlState.New)
                {//状态为edit和new
                    this.chKIsMultiChamber.Enabled = !this.chkIsChamber.Checked;
                    this.chkIsChamber.Enabled = !this.chKIsMultiChamber.Checked;
                }
            }
            else
            {
                this.txtEquipmentName.Text = string.Empty;
                this.txtDescription.Text = string.Empty;
                this.txtEquipmentCode.Text = string.Empty;
                this.txtEquipmentMode.Text = string.Empty;
                this.txtMaxQuantity.EditValue = 0;
                this.txtMinQuantity.EditValue = 0;
                this.cmbEquipmentType.EditValue = string.Empty;
                this.cmbEquipmentGroup.EditValue = string.Empty;
                this.cmbEquipmentLocation.EditValue = string.Empty;
                this.chkIsBatch.Checked = false;
                this.chkIsChamber.Checked = false;
                this.chKIsMultiChamber.Checked = false;
                this.cmbParentEquipment.EditValue = string.Empty;
                this.lueRealEquipment.EditValue = REAL_EQUIPMENT_DEFAULT_VALUE;
                if (this.State != ControlState.New)
                {
                    this.cmbEquipmentState.EditValue = string.Empty;
                    this.cmbEquipmentChangeState.EditValue = string.Empty;
                }
            }
        }

        #endregion

        private void chKIsMultiChamber_CheckedChanged(object sender, EventArgs e)
        {
            if (chKIsMultiChamber.Checked)
            {
                this.chkIsChamber.Enabled = false;
                this.lblChamberTotal.Visibility = LayoutVisibility.Always;
                this.txtChamberTotal.Visible = true;
            }
            else
            {
                if (State == ControlState.Edit || State == ControlState.New)
                {
                    this.chkIsChamber.Enabled = true;
                }
                this.lblChamberTotal.Visibility = LayoutVisibility.Never;
                this.txtChamberTotal.Visible = false;
            }
        }

        private void cmbParentEquipment_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbParentEquipment.EditValue != null && cmbParentEquipment.EditValue.ToString() != string.Empty)
            {
                this.lblChamberIndex.Visibility = LayoutVisibility.Always;
                this.cmbChamberIndex.Visible = true;
                if (parentEquipmentTable != null && parentEquipmentTable.Rows.Count > 0)
                {
                    DataRow[] rows = parentEquipmentTable.Select("EQUIPMENT_KEY='"+cmbParentEquipment.EditValue.ToString()+"'");
                    if (rows.Length > 0)
                    {
                        int totalChamber =Convert.ToInt32(rows[0][EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL]);
                        cmbChamberIndex.Properties.Items.Clear();
                        cmbChamberIndex.Text = string.Empty;
                        for (int i = 1; i < totalChamber + 1; i++)
                        {
                            cmbChamberIndex.Properties.Items.Add(i.ToString());
                        }                        
                    }
                }
            }
            else
            {
                this.lblChamberIndex.Visibility = LayoutVisibility.Always;
                this.cmbChamberIndex.Visible = false;
            }
        }
        /// <summary>
        /// 设备类型文本改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbEquipmentType_EditValueChanged(object sender, EventArgs e)
        {
            string type = Convert.ToString(this.cmbEquipmentType.EditValue);
            if (type == "物理设备")
            {
                this.lueRealEquipment.EditValue = REAL_EQUIPMENT_DEFAULT_VALUE;
                this.lciRealEquipment.Visibility = LayoutVisibility.Never;
            }
            else
            {
                this.lueRealEquipment.EditValue = REAL_EQUIPMENT_DEFAULT_VALUE;
                this.lciRealEquipment.Visibility = LayoutVisibility.Always;
            }
        }

        /// <summary>
        /// 绑定所有物理设备数据到控件。
        /// </summary>
        private void BindEquipmentDataToControl()
        {
            EquipmentEntity entity = new EquipmentEntity();
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo =0,
                PageSize =Int32.MaxValue
            };
            DataSet ds = entity.GetEquipments(string.Empty,string.Empty, "物理设备", ref config);

            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                DataRow drNew = ds.Tables[0].NewRow();
                drNew["EQUIPMENT_KEY"] = " ";
                ds.Tables[0].Rows.InsertAt(drNew, 0);
                this.lueRealEquipment.Properties.DataSource = ds.Tables[0];
                this.lueRealEquipment.Properties.DisplayMember = "EQUIPMENT_CODE";
                this.lueRealEquipment.Properties.ValueMember = "EQUIPMENT_KEY";
            }
            else
            {
                this.lueRealEquipment.Properties.DataSource = null;
                MessageService.ShowMessage(entity.ErrorMsg, StringParser.Parse("${res:Global.SystemInfo}"));
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
