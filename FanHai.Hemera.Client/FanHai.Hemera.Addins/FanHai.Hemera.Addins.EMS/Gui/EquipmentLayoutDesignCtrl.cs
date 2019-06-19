using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using FanHai.Hemera.Addins.EMS;
using DevExpress.XtraEditors;
using System.Threading;
using System.Drawing.Imaging;
using FanHai.Hemera.Addins.EMS.Gui;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils.Controls.Common;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;

namespace FanHai.Hemera.Addins.EMS
{
    public partial class EquipmentLayoutDesignCtrl : BaseUserCtrl
    {
        #region private variable
        private EquipmentLayoutEntity EquLayoutEntity = new EquipmentLayoutEntity ();       
            
        private bool validData=false;
        private MemoEdit movePictureEdit = null;
        private List<KeyValuePair<string, EquipmentLayoutDetailEntity>> detailList = new List<KeyValuePair<string, EquipmentLayoutDetailEntity>>();//用于保存明细数据    
        private List<KeyValuePair<string, string>> childParentRelationList = new List<KeyValuePair<string, string>>();//父设备与子设备关系  
        private List<string> equipmentList = new List<string>();//记录当前页面使用过的设备
        private List<EquipmentLayoutDetailEntity> tempRelationList = new List<EquipmentLayoutDetailEntity>();//用于暂存移动和缩放操作时相关联的所有控件
        int picCount = 0,start = 0, second = 5 * 60;//设定5分钟刷新一次//定时器 刷新数据
        bool blclickValue = true;
      

        private const int CONTROL_BANDWIDTH = 5;
        private const int CONTROL_MINWIDTH = 10;
        private const int CONTROL_MINHEIGHT = 10;
        private Point primaryPoint = new Point(0, 0);
        private Point secondlyPoint = new Point(0, 0);
        private enum MousePointPosition
        {
            MouseSizeNone = 0,
            MouseSizeRight = 1,
            MouseSizeLeft = 2,
            MouseSizeBottom = 3,
            MouseSizeTop = 4,
            MouseSizeTopLeft = 5,
            MouseSizeTopRight = 6,
            MouseSizeBottomLeft = 7,
            MouseSizeBottomRight = 8,
            MouseDrag = 9
        }
        private MousePointPosition mousePointPosition = MousePointPosition.MouseSizeNone;
        #endregion
        public DataRow[] SelectedData = new DataRow[] { };
        private string formType = string.Empty;
        private bool isMultiSelect = false;
        #region Constructor
        public EquipmentLayoutDesignCtrl()
        {
            InitializeComponent();               
            this.afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            this.State = ControlState.Empty;

            InitUi();

        }

        private void InitUi()
        {

            //toolStripButton1.Text = StringParser.Parse("${res:Global.Query}");
            toolStripButton2.Text = StringParser.Parse("${res:Global.New}");
            toolStripButton3.Text = StringParser.Parse("${res:Global.Save}");
            toolStripButton4.Text = StringParser.Parse("${res:Global.Delete}");
            //lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayoutDesign.title}");
            layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayoutDesign.lbl.0001}");
            layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayoutDesign.lbl.0002}");
            layoutCtlGrpEquipment.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayoutDesign.lbl.0003}");
            btnHardRefresh.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayoutDesign.lbl.0004}");
            lblAlarmTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayoutDesign.lbl.0005}");
            btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            btnOK.Text = StringParser.Parse("${res:Global.OKButtonText}");
        }
        #endregion

        #region UI Event
        private void OnAfterStateChanged()
        {
            switch (this.State)
            {               
                case ControlState.Empty:
                    InitialUIControls(false);
                    this.btnDelete.Enabled = false;
                    this.toolStripButton4.Enabled = false;
                    this.tePicName.Text = string.Empty;
                    this.tePicName.Tag = null;
                    this.meDesc.Text = string.Empty;
                    this.LayoutPic.Image = null;
                    this.LayoutPic.Controls.Clear();
                    detailList.Clear();//清除list中的记录 
                    childParentRelationList.Clear();
                    equipmentList.Clear();
                    break;                
                case ControlState.Edit:  
                    InitialUIControls(true);
                    this.btnDelete.Enabled = true;
                    this.tePicName.Properties.ReadOnly = true;
                    this.meDesc.Properties.ReadOnly =true;
                    this.toolStripButton4.Enabled = true;
                    break;
                case ControlState.New:
                    InitialUIControls(true);
                    this.tePicName.Text = string.Empty;
                    this.tePicName.Tag = null;
                    this.meDesc.Text = string.Empty;
                    this.toolStripButton4.Enabled = false;
                    this.btnDelete.Enabled = false;                  
                    break;
                case ControlState.Read:
                    InitialUIControls(false);
                    this.btnDelete.Enabled = false;
                    this.toolStripButton4.Enabled = false;
                    break;
                case ControlState.ReadOnly:
                    InitialUIControls(false);
                    this.btnDelete.Enabled = false;
                    this.toolStripButton4.Enabled = false;
                    break;
                default:
                    InitialUIControls(false);
                    this.btnDelete.Enabled = false;
                    this.toolStripButton4.Enabled = false;
                    break;
            }            
        }

        private void InitialUIControls(bool enable)
        {
            this.btnSearch.Enabled = true;
            this.btnNew.Enabled =true;
            this.EquipmentPic.Enabled = enable;
            //this.chamberPic.Enabled = enable;            
            this.btnSave.Enabled = enable;            
            this.tePicName.Properties.ReadOnly = !enable;
            this.meDesc.Properties.ReadOnly = !enable;

            timer1.Enabled = false;
        }
        #endregion
  
        #region Button Click Event
        /// <summary>
        /// 获得Layout数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (EquipmentCommonQueryDialog queryDialog = new EquipmentCommonQueryDialog("EquipmentLayout", false))
            {
                if (queryDialog.ShowDialog() == DialogResult.OK)
                {
                    if (queryDialog.SelectedData != null && queryDialog.SelectedData.Length > 0)
                    {   
                        DataRow selectedRow = queryDialog.SelectedData[0];
                        this.tePicName.Text = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME].ToString();
                        this.tePicName.Tag = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY].ToString();
                        this.meDesc.Text = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_DESC].ToString();
                        //根据Key值查询设备布局图底图及原始布局
                        string msg;
                       DataSet dsReturn=EquLayoutEntity.GetEquipmentLayoutDetail(tePicName.Tag.ToString(), out msg);
                       if (string.IsNullOrEmpty(msg))
                       {  
                           if (dsReturn != null && dsReturn.Tables.Count > 0)
                           {
                               //主表信息，则加载布局底图
                               if (dsReturn.Tables.Contains(EMS_LAYOUT_MAIN_FIELDS.DATABASE_TABLE_NAME))
                               {
                                   //读取图片数据并转换为背景底图
                                   byte[] imageData = new byte[0];
                                   imageData = (byte[])(dsReturn.Tables[EMS_LAYOUT_MAIN_FIELDS.DATABASE_TABLE_NAME].Rows[0][EMS_LAYOUT_MAIN_FIELDS.LAYOUT_PIC]);
                                   Bitmap bitMap = GetBitmap(imageData);
                                   if (bitMap != null)
                                   {
                                       if (LayoutPic.Image != null)
                                       {
                                           LayoutPic.Image = null;
                                       }
                                       LayoutPic.Image = (Image)bitMap;                                      
                                       LayoutPic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
                                       LayoutPic.Controls.Clear();
                                   }
                               }
                               //明细数据，则加载设备布局信息
                               if (dsReturn.Tables.Contains(EMS_LAYOUT_DETAIL_FIELDS.DATABASE_TABLE_NAME))
                               {
                                   //需修改picCount值
                                   DataTable detailTable = dsReturn.Tables[EMS_LAYOUT_DETAIL_FIELDS.DATABASE_TABLE_NAME];
                                   if (detailTable.Rows.Count > 0)
                                   {
                                       picCount = detailTable.Rows.Count+1;
                                       this.detailList.Clear();
                                       equipmentList.Clear();
                                       childParentRelationList.Clear();

                                       for (int i = 0; i < detailTable.Rows.Count; i++)
                                       {
                                           #region detailList 赋值
                                           EquipmentLayoutDetailEntity equDetailEntity = new EquipmentLayoutDetailEntity();
                                           equDetailEntity.LayoutKey = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.LAYOUT_KEY].ToString();
                                           equDetailEntity.PicHeight = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.PIC_HEIGHT].ToString();
                                           equDetailEntity.PicWidth = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.PIC_WIDTH].ToString();
                                           equDetailEntity.PicLeft = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.PIC_LEFT].ToString();
                                           equDetailEntity.PicTop = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.PIC_TOP].ToString();
                                           equDetailEntity.PicType = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.PIC_TYPE].ToString();
                                           equDetailEntity.EquipmentName = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.EQUIPMENT_NAME].ToString();
                                           equDetailEntity.EquipmentKey = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.EQUIPMENT_KEY].ToString();
                                           equDetailEntity.DetailColKey = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.DETAIL_COL_KEY].ToString();

                                           equDetailEntity.ChamberIndex = detailTable.Rows[i][EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX].ToString();
                                           equDetailEntity.IsMultiChamber = detailTable.Rows[i][EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER].ToString();
                                           equDetailEntity.ChamberTotal = detailTable.Rows[i][EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL].ToString();
                                           equDetailEntity.ParentEquKey = detailTable.Rows[i][EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY].ToString();//取父设备信息，便于建立父设备与子设备的关系表

                                           equDetailEntity.ColorName = detailTable.Rows[i][EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString();//当前设备状态

                                           equDetailEntity.PicName = (i + 1).ToString();
                                           KeyValuePair<string, EquipmentLayoutDetailEntity> keyValuePair = new KeyValuePair<string, EquipmentLayoutDetailEntity>((i+1).ToString(), equDetailEntity);
                                           detailList.Add(keyValuePair);

                                           if (equDetailEntity.EquipmentKey != string.Empty)
                                           {
                                               equipmentList.Add(equDetailEntity.EquipmentKey);
                                               if (equDetailEntity.ParentEquKey != string.Empty)
                                               {                                                  
                                                   childParentRelationList.Add( new KeyValuePair<string, string>(equDetailEntity.EquipmentKey,equDetailEntity.ParentEquKey));
                                               }
                                           }
                                           #endregion
                                           
                                           #region 绘制MemoEdit
                                           MemoEdit picBox = new MemoEdit();
                                           if (equDetailEntity.PicType == "E")
                                           {                                             
                                               picBox.Text = equDetailEntity.EquipmentName;                                               
                                               picBox.Properties.ReadOnly = true;
                                               //picBox.Tag = equDetailEntity.PicType;
                                           }
                                           else
                                           {
                                               MessageService.ShowMessage("绘制图类型不正确,请与系统管理员联系!");
                                               return;
                                           }
                                           picBox.Properties.ScrollBars = ScrollBars.None;
                                           picBox.Width = Convert.ToInt32(equDetailEntity.PicWidth);
                                           picBox.Height = Convert.ToInt32(equDetailEntity.PicHeight);
                                           ////add by qym 
                                           //string sText = string.Empty;
                                           //if (picBox.Width >= picBox.Height)
                                           //    sText = equDetailEntity.EquipmentName;
                                           //if (picBox.Width < picBox.Height)
                                           //{
                                           //    char[] chars = equDetailEntity.EquipmentName.ToCharArray();
                                           //    foreach (char c in chars)
                                           //        sText += "   " + c.ToString() + "\r\n";
                                           //}
                                           //picBox.Text = equDetailEntity.EquipmentName; //chongfu qym
                                           //picBox.Text = sText;
                                           ////add by qym 

                                           picBox.MouseDown += new MouseEventHandler(layoutPicBox_MouseDown);                                        
                                           picBox.Name = (i+1).ToString();//用序列号作为PicBox的名字，便于查找
                                           picBox.Tag = equDetailEntity.EquipmentKey;
                                           //设备状态颜色显示
                                           picBox.BackColor = new ColorType().GetStateColor(equDetailEntity.ColorName);
                                           //设备的Title显示内容
                                           picBox.ToolTip = equDetailEntity.EquipmentName;

                                           picBox.MouseMove += new MouseEventHandler(layoutPicBox_MouseMove);
                                           picBox.MouseUp += new MouseEventHandler(layoutPicBox_MouseUp);
                                           //picBox.Click += new EventHandler(m1_Click);
                                          
                                          // Point p = this.PointToClient(new Point(Convert.ToInt32(equDetailEntity.PicLeft), Convert.ToInt32(equDetailEntity.PicTop)+LayoutPic.Top+picBox.Height/2));                                           
                                           
                                           LayoutPic.Controls.Add(picBox);
                                           picBox.Top = Convert.ToInt32(equDetailEntity.PicTop);
                                           picBox.Left = Convert.ToInt32(equDetailEntity.PicLeft);

                                           RefreshEnabled(true);
                                           #endregion
                                       }                                       
                                   }
                               }
                           }
                       }
                       else
                       {
                           MessageService.ShowError(msg);
                           return;
                       }
                        this.State = ControlState.Edit;
                    }
                }
            }
            
        }
        private void paginationDataList_DataPaging()
        {
            LoadData(this.txtQueryValue.Text.Trim());
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.formType = "EquipmentLayout";
            this.isMultiSelect = false;
            InitialFormControls();
            LoadData(this.txtQueryValue.Text.Trim());
        }
        /// <summary>
        /// Load Data By Query Value
        /// </summary>
        /// <param name="queryValue"></param>
        private void LoadData(string queryValue)
        {
            string msg;
            int pageNo, pageSize, pages, records;
            DataTable dataTable;
            DataColumn dc;
            if (!queryValue.Trim().Equals(""))
                queryValue = queryValue.Trim().ToUpper();
            switch (this.formType)
            {
                case "EquipmentCheckItems": //设备检查项

                    #region Load Equipment Check Items Data

                    EquipmentCheckItemEntity equipmentCheckItemEntity = new EquipmentCheckItemEntity();

                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);

                    dataTable = equipmentCheckItemEntity.LoadCheckItemsData(queryValue, pageNo, pageSize, out pages, out records, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }

                    #endregion

                    break;
                case "EquipmentCheckList": //设备检查表单

                    #region Load Equipment Check List Data

                    EquipmentCheckListEntity equipmentCheckListEntity = new EquipmentCheckListEntity();

                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);

                    dataTable = equipmentCheckListEntity.LoadCheckListData(queryValue, pageNo, pageSize, out pages, out records, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }

                    #endregion

                    break;
                case "EquipmentCheckListJob": //设备检查表单任务

                    #region Load Equipment Check List Job Data

                    EquipmentCheckListJobEntity equipmentCheckListJobEntity = new EquipmentCheckListJobEntity();

                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);

                    dataTable = equipmentCheckListJobEntity.LoadCheckListJobsData(string.Empty, string.Empty, queryValue, string.Empty, pageNo, pageSize, out pages, out records, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }

                    #endregion

                    break;
                case "EquipmentPart": //设备备件

                    #region Load Equipment Part Data

                    EquipmentPartEntity equipmentPartEntity = new EquipmentPartEntity();

                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);

                    dataTable = equipmentPartEntity.LoadPartsData(queryValue, pageNo, pageSize, out pages, out records, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }

                    #endregion

                    break;
                case "EquipmentTask": //设备作业

                    #region Load Equipment Task Data

                    EquipmentTaskEntity equipmentTaskEntity = new EquipmentTaskEntity();

                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);

                    dataTable = equipmentTaskEntity.LoadTaskData(string.Empty, string.Empty, queryValue, string.Empty, pageNo, pageSize, out pages, out records, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }

                    #endregion

                    break;
                case "EquipmentLayout"://设备布局图
                    #region Load Equipment Layout Data

                    EquipmentLayoutEntity equipmentLayoutEntity = new EquipmentLayoutEntity();

                    this.paginationDataList.Visible = false;

                    dataTable = equipmentLayoutEntity.SearchEquipmentLayout(txtQueryValue.Text, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;
                        this.grdDataList.DataSource = dataTable;
                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;
                        MessageService.ShowError(msg);
                    }
                    #endregion                   
                    break;

                case "Equipment_E": //设备                    
                case "Equipment_C"://腔体
                    #region Get equipment data

                    EquipmentEntity equipmentEntity = new EquipmentEntity();
                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);
                    string equipmentType = formType.Substring(formType.Length - 1, 1);
                    if (equipmentType == "E")
                    {
                        dataTable = equipmentEntity.GetParentEquipments(queryValue, pageNo, pageSize, out pages, out records, out msg);
                    }
                    else
                    {
                        dataTable = equipmentEntity.GetChildEquipments(queryValue, pageNo, pageSize, out pages, out records, out msg);
                    }

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }
                    #endregion
                    break;
                case "Equipment_Q"://设备状态切换
                    #region Get equipment data
                    //grvDataList.OptionsBehavior.Editable = false;
                    grvDataList.Columns[1].OptionsColumn.AllowEdit = false;
                    grvDataList.Columns[2].OptionsColumn.AllowEdit = false;
                    grvDataList.Columns[3].OptionsColumn.AllowEdit = false;
                    grvDataList.Columns[4].OptionsColumn.AllowEdit = false;
                    grvDataList.Columns[5].OptionsColumn.AllowEdit = false;
                    grvDataList.Columns[6].OptionsColumn.AllowEdit = false;
                    grvDataList.Columns[7].OptionsColumn.AllowEdit = false;
                    ///////

                    EquipmentEntity equipmentEntityEvent = new EquipmentEntity();
                    this.paginationDataList.GetPaginationProperties(out pageNo, out pageSize);
                    string equipmentTypeEvent = formType.Substring(formType.Length - 1, 1);
                    dataTable = equipmentEntityEvent.GetStateEventEquipments(queryValue, pageNo, pageSize, out pages, out records, out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        this.paginationDataList.SetPaginationProperties(pageNo, pageSize, pages, records);

                        dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                        dc.Caption = string.Empty;
                        dc.DefaultValue = false;

                        this.grdDataList.DataSource = dataTable;

                        this.grvDataList.BestFitColumns();
                    }
                    else
                    {
                        this.grdDataList.DataSource = null;

                        MessageService.ShowError(msg);
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }
        private void grvDataList_DoubleClick(object sender, EventArgs e)
        {
            int rowHandle = grvDataList.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                DataTable dataTable = this.grdDataList.DataSource as DataTable;
                string s1 = grvDataList.GetRowCellValue(rowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE).ToString();

                DataRow[] dr = dataTable.Select(string.Format("EQUIPMENT_CODE='{0}'", s1));
                dr[0][COMMON_FIELDS.FIELD_COMMON_CHECKED] = true;
            }
            
            btnOK_Click(sender, e);
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            DataTable dataTable = this.grdDataList.DataSource as DataTable;

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow[] selectedRows = dataTable.Select(string.Format("{0} = True", COMMON_FIELDS.FIELD_COMMON_CHECKED));

                if (selectedRows.Length > 0)
                {
                    this.SelectedData = selectedRows;
                }
                else
                {
                    MessageService.ShowMessage("请选择数据!");
                }

                if (SelectedData != null && SelectedData.Length > 0)
                {
                    DataRow selectedRow = SelectedData[0];
                    this.tePicName.Text = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME].ToString();
                    this.tePicName.Tag = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY].ToString();
                    this.meDesc.Text = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_DESC].ToString();
                    //根据Key值查询设备布局图底图及原始布局
                    string msg;
                    DataSet dsReturn = EquLayoutEntity.GetEquipmentLayoutDetail(tePicName.Tag.ToString(), out msg);
                    if (string.IsNullOrEmpty(msg))
                    {
                        if (dsReturn != null && dsReturn.Tables.Count > 0)
                        {
                            //主表信息，则加载布局底图
                            if (dsReturn.Tables.Contains(EMS_LAYOUT_MAIN_FIELDS.DATABASE_TABLE_NAME))
                            {
                                //读取图片数据并转换为背景底图
                                byte[] imageData = new byte[0];
                                imageData = (byte[])(dsReturn.Tables[EMS_LAYOUT_MAIN_FIELDS.DATABASE_TABLE_NAME].Rows[0][EMS_LAYOUT_MAIN_FIELDS.LAYOUT_PIC]);
                                Bitmap bitMap = GetBitmap(imageData);
                                if (bitMap != null)
                                {
                                    if (LayoutPic.Image != null)
                                    {
                                        LayoutPic.Image = null;
                                    }
                                    LayoutPic.Image = (Image)bitMap;
                                    LayoutPic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
                                    LayoutPic.Controls.Clear();
                                }
                            }
                            //明细数据，则加载设备布局信息
                            if (dsReturn.Tables.Contains(EMS_LAYOUT_DETAIL_FIELDS.DATABASE_TABLE_NAME))
                            {
                                //需修改picCount值
                                DataTable detailTable = dsReturn.Tables[EMS_LAYOUT_DETAIL_FIELDS.DATABASE_TABLE_NAME];
                                if (detailTable.Rows.Count > 0)
                                {
                                    picCount = detailTable.Rows.Count + 1;
                                    this.detailList.Clear();
                                    equipmentList.Clear();
                                    childParentRelationList.Clear();

                                    for (int i = 0; i < detailTable.Rows.Count; i++)
                                    {
                                        #region detailList 赋值
                                        EquipmentLayoutDetailEntity equDetailEntity = new EquipmentLayoutDetailEntity();
                                        equDetailEntity.LayoutKey = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.LAYOUT_KEY].ToString();
                                        equDetailEntity.PicHeight = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.PIC_HEIGHT].ToString();
                                        equDetailEntity.PicWidth = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.PIC_WIDTH].ToString();
                                        equDetailEntity.PicLeft = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.PIC_LEFT].ToString();
                                        equDetailEntity.PicTop = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.PIC_TOP].ToString();
                                        equDetailEntity.PicType = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.PIC_TYPE].ToString();
                                        equDetailEntity.EquipmentName = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.EQUIPMENT_NAME].ToString();
                                        equDetailEntity.EquipmentKey = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.EQUIPMENT_KEY].ToString();
                                        equDetailEntity.DetailColKey = detailTable.Rows[i][EMS_LAYOUT_DETAIL_FIELDS.DETAIL_COL_KEY].ToString();

                                        equDetailEntity.ChamberIndex = detailTable.Rows[i][EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX].ToString();
                                        equDetailEntity.IsMultiChamber = detailTable.Rows[i][EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER].ToString();
                                        equDetailEntity.ChamberTotal = detailTable.Rows[i][EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL].ToString();
                                        equDetailEntity.ParentEquKey = detailTable.Rows[i][EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY].ToString();//取父设备信息，便于建立父设备与子设备的关系表

                                        equDetailEntity.ColorName = detailTable.Rows[i][EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString();//当前设备状态

                                        equDetailEntity.PicName = (i + 1).ToString();
                                        KeyValuePair<string, EquipmentLayoutDetailEntity> keyValuePair = new KeyValuePair<string, EquipmentLayoutDetailEntity>((i + 1).ToString(), equDetailEntity);
                                        detailList.Add(keyValuePair);

                                        if (equDetailEntity.EquipmentKey != string.Empty)
                                        {
                                            equipmentList.Add(equDetailEntity.EquipmentKey);
                                            if (equDetailEntity.ParentEquKey != string.Empty)
                                            {
                                                childParentRelationList.Add(new KeyValuePair<string, string>(equDetailEntity.EquipmentKey, equDetailEntity.ParentEquKey));
                                            }
                                        }
                                        #endregion

                                        #region 绘制MemoEdit
                                        MemoEdit picBox = new MemoEdit();
                                        if (equDetailEntity.PicType == "E")
                                        {
                                            picBox.Text = equDetailEntity.EquipmentName;
                                            picBox.Properties.ReadOnly = true;
                                            //picBox.Tag = equDetailEntity.PicType;
                                        }
                                        else
                                        {
                                            MessageService.ShowMessage("绘制图类型不正确,请与系统管理员联系!");
                                            return;
                                        }
                                        picBox.Properties.ScrollBars = ScrollBars.None;
                                        picBox.Width = Convert.ToInt32(equDetailEntity.PicWidth);
                                        picBox.Height = Convert.ToInt32(equDetailEntity.PicHeight);

                                        picBox.MouseDown += new MouseEventHandler(layoutPicBox_MouseDown);
                                        picBox.Name = (i + 1).ToString();//用序列号作为PicBox的名字，便于查找
                                        picBox.Tag = equDetailEntity.EquipmentKey;
                                        //设备状态颜色显示
                                        picBox.BackColor = new ColorType().GetStateColor(equDetailEntity.ColorName);
                                        //设备的Title显示内容
                                        picBox.ToolTip = equDetailEntity.EquipmentName;

                                        picBox.MouseMove += new MouseEventHandler(layoutPicBox_MouseMove);
                                        picBox.MouseUp += new MouseEventHandler(layoutPicBox_MouseUp);

                                        LayoutPic.Controls.Add(picBox);
                                        picBox.Top = Convert.ToInt32(equDetailEntity.PicTop);
                                        picBox.Left = Convert.ToInt32(equDetailEntity.PicLeft);

                                        RefreshEnabled(true);
                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageService.ShowError(msg);
                        return;
                    }
                    this.State = ControlState.Edit;
                }
            }
            else
            {
                MessageService.ShowMessage("请查询数据!");
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            ChoiceLayoutPicDialog choicePicDialog = new ChoiceLayoutPicDialog();
            if (DialogResult.OK == choicePicDialog.ShowDialog())
            {
                string picPath = choicePicDialog.picPath;//获取图片路径
                Bitmap bmPic = new Bitmap(picPath);
                Point ptLoction = new Point(bmPic.Size);
                LayoutPic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
                if (LayoutPic.Image != null)
                {
                    LayoutPic.Image = null;//清空底图
                    LayoutPic.Controls.Clear();//清空设备
                    detailList.Clear();//清空List  

                    equipmentList.Clear();
                    childParentRelationList.Clear();
                    picCount = 0;

                }
                LayoutPic.Image = (Image)bmPic;
                if (LayoutPic.Image != null)
                {
                    this.State = ControlState.New;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tePicName.Text.Trim().Length == 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayoutDesign.msg.0001}"));//图片名称不能为空
                return;
            }
            //新增面板
            #region 
            if (State == ControlState.New) //新增
            {
                //主表信息
                byte[] imageData = ReadImageData();                
                DataSet dsSave = new DataSet();
                DataTable mainTable = CreateMainDataTable();
                string layoutKey =  CommonUtils.GenerateNewKey(0);
                mainTable.Rows.Add(tePicName.Text, meDesc.Text, imageData, PropertyService.Get(PROPERTY_FIELDS.USER_NAME),layoutKey);                     
                dsSave.Tables.Add(mainTable);

                //明细表信息
                if (detailList.Count > 0)
                {
                    DataTable detailTable = EMS_LAYOUT_DETAIL_FIELDS.CreateDataTable();
                    
                    for (int i = 0; i < detailList.Count; i++)
                    {
                        Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                       {   
                                                           {EMS_LAYOUT_DETAIL_FIELDS.LAYOUT_KEY,layoutKey},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.EQUIPMENT_KEY,detailList[i].Value.EquipmentKey},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.EQUIPMENT_NAME,detailList[i].Value.EquipmentName},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.PIC_HEIGHT,detailList[i].Value.PicHeight},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.PIC_LEFT,detailList[i].Value.PicLeft},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.PIC_TOP,detailList[i].Value.PicTop},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.PIC_TYPE,detailList[i].Value.PicType},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.PIC_WIDTH,detailList[i].Value.PicWidth},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.DETAIL_COL_KEY,detailList[i].Value.DetailColKey},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.FLAG,"0"}
                                                       };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref detailTable, dataRow);
                    }
                    dsSave.Tables.Add(detailTable);
                }
               
                //保存操作
                if (EquLayoutEntity.InsertEquipmentLayout(dsSave))
                {
                    tePicName.Tag = EquLayoutEntity.LayoutKey;
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayoutDesign.msg.0002}"));//保存成功！
                    //equipmentList.Clear();
                    this.State = ControlState.Edit;

                    if (detailList.Count > 0)
                        RefreshEnabled(true);
                }
            }
            #endregion
            //修改面板
            #region
            else if (State == ControlState.Edit) //修改，修改时主表信息不能修改
            {
                //明细表信息
                if (detailList.Count > 0)
                {
                    DataSet reqDS = new DataSet();
                    reqDS.ExtendedProperties.Add(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY, tePicName.Tag.ToString());
                    reqDS.ExtendedProperties.Add(EMS_LAYOUT_DETAIL_FIELDS.EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));

                    DataTable layOutDetailTable = EMS_LAYOUT_DETAIL_FIELDS.CreateDataTable();

                    for (int i = 0; i < detailList.Count; i++)
                    {
                        Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                       {   
                                                           {EMS_LAYOUT_DETAIL_FIELDS.LAYOUT_KEY,tePicName.Tag.ToString()},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.EQUIPMENT_KEY,detailList[i].Value.EquipmentKey},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.EQUIPMENT_NAME,detailList[i].Value.EquipmentName},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.PIC_HEIGHT,detailList[i].Value.PicHeight},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.PIC_LEFT,detailList[i].Value.PicLeft},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.PIC_TOP,detailList[i].Value.PicTop},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.PIC_TYPE,detailList[i].Value.PicType},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.PIC_WIDTH,detailList[i].Value.PicWidth},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.DETAIL_COL_KEY,detailList[i].Value.DetailColKey},
                                                           {EMS_LAYOUT_DETAIL_FIELDS.FLAG,"0"}
                                                       };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref layOutDetailTable, dataRow);
                    }
                    reqDS.Tables.Add(layOutDetailTable);
                    if (EquLayoutEntity.UpdateEquipmentLayout(reqDS))
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayoutDesign.msg.0003}"));//更新成功！
                        //equipmentList.Clear();
                        this.State = ControlState.Edit;
                    }
                }
            #endregion
                else
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayoutDesign.msg.0004}"));//没有可更新的数据
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayoutDesign.msg.0005}")))//确定要删除吗？
            {
                if (EquLayoutEntity.DeleteEquipmentLayout(tePicName.Tag.ToString()))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayoutDesign.msg.0006}"));//删除成功！
                    this.State = ControlState.Empty; 
                }
            }
        }
        #endregion    
       
        #region 工具栏中鼠标点击事件
        /// <summary>
        /// 处理鼠标MouseDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EquipmentPic_MouseDown(object sender, MouseEventArgs e)
        {            
            PictureEdit_MouseDown(sender, e); 
        }
        private void chamberPic_MouseDown(object sender, MouseEventArgs e)
        {
            PictureEdit_MouseDown(sender, e);
        }

       /// <summary>
       /// 工具栏中鼠标点击事件
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void PictureEdit_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MemoEdit memoEdit = new MemoEdit();
                MemoEdit mo = new MemoEdit();
                memoEdit.Name = "memoEdit01";
                memoEdit.Tag = "E";
                memoEdit.BackColor = new ColorType().GetStateColor(ColorType.DEFAULT_COLOR);
                    ;
                memoEdit.Properties.ScrollBars = ScrollBars.None;
                memoEdit.Width = 115;
                memoEdit.Height = 31;

                mo = memoEdit;
                mo.DoDragDrop(mo, DragDropEffects.Copy);

                //PictureEdit picEdit = (PictureEdit)(sender);
                //if (picEdit.Image != null)
                //{
                //    if (picEdit.Name == "EquipmentPic")
                //    {
                //        picEdit.Image.Tag = "E";//用于记录图片类型
                //    }
                //    else if (picEdit.Name == "chamberPic")
                //    {
                //        picEdit.Image.Tag = "C";
                //    }
                //    picEdit.DoDragDrop(picEdit.Image, DragDropEffects.Move | DragDropEffects.Copy);
                //}
            }
        }
        #endregion

        #region 布局图中图片控件的鼠标点击事件       
        private void m1_Click(object sender, EventArgs e)
        {
            if (blclickValue)
                AddContextMenu();
        }
        /// <summary>
        /// 布局图中图片控件的鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void layoutPicBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                primaryPoint.X = e.X;
                primaryPoint.Y = e.Y;
                secondlyPoint.X = e.X;
                secondlyPoint.Y = e.Y;

                MemoEdit picEdit = (MemoEdit)(sender);

                movePictureEdit = picEdit;
                //AddContextMenu();//添加控件的右键菜单

                //if (picEdit.Image != null)
                //{
                //    movePictureEdit = picEdit;
                //    //whetherSelected = true;
                //    //p.X = Cursor.Position.X;
                //    //p.Y = Cursor.Position.Y;
                //    //picEdit.DoDragDrop(picEdit.Image, DragDropEffects.Move);
                //}
            }
            else if (e.Button == MouseButtons.Right)
            {
                movePictureEdit = (MemoEdit)(sender);
                AddContextMenu();//添加控件的右键菜单
            }
        }

        /// <summary>
        /// mouse move event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void layoutPicBox_MouseMove(object sender, MouseEventArgs e)
        {
            Control control = (sender as Control);
            if (e.Button == MouseButtons.Left)
            {

                SetMouseCursorIcon(mousePointPosition);

                MemoEdit picEdit = (MemoEdit)control;
                List<MemoEdit> relationPicList = null;
                if (picEdit.Tag != null && picEdit.Tag.ToString() != string.Empty)
                {
                    relationPicList = FindRelationEquipment(picEdit.Tag.ToString(), "E");
                }
                tempRelationList.Clear();

                switch (mousePointPosition)
                {
                     
                    case MousePointPosition.MouseDrag:

                        control.Left = control.Left + e.X - primaryPoint.X;
                        control.Top = control.Top + e.Y - primaryPoint.Y;

                        //相关联的图片移动
                        if (relationPicList != null)
                        {
                            foreach (MemoEdit relationPic in relationPicList)
                            {
                                relationPic.Left = relationPic.Left + e.X - primaryPoint.X;
                                relationPic.Top = relationPic.Top + e.Y - primaryPoint.Y;
                                EquipmentLayoutDetailEntity detailEntity = new EquipmentLayoutDetailEntity();
                                detailEntity.PicName = relationPic.Name;
                                detailEntity.PicLeft = relationPic.Left.ToString();
                                detailEntity.PicTop = relationPic.Top.ToString();
                                tempRelationList.Add(detailEntity);
                            }
                            relationPicList.Clear();
                        }                       
                        break;
                    case MousePointPosition.MouseSizeBottom:
                        //int h = control.Height;
                        //关联图片缩放缩放
                        control.Height = control.Height + (e.Y - secondlyPoint.Y);
                        //if (relationPicList != null)
                        //{
                        //    foreach (PictureEdit relationPic in relationPicList)
                        //    {
                        //        if (relationPic.Image.Tag.ToString() == "C")
                        //        {
                        //            relationPic.Height =control.Height * relationPic.Height / h;
                        //            relationPic.Top = control.Height * relationPic.Top / h;
                        //            EquipmentLayoutDetailEntity detailEntity = new EquipmentLayoutDetailEntity();
                        //            detailEntity.PicName = relationPic.Name;
                        //            detailEntity.PicLeft = relationPic.Left.ToString();
                        //            detailEntity.PicTop = relationPic.Top.ToString();
                        //            detailEntity.PicHeight = relationPic.Height.ToString();
                        //            detailEntity.PicWidth = relationPic.Width.ToString();
                        //            tempRelationList.Add(detailEntity);
                        //        }
                        //    }
                        //    relationPicList.Clear();
                        //}                       
                        secondlyPoint.X = e.X;
                        secondlyPoint.Y = e.Y;
                        
                        break;
                    case MousePointPosition.MouseSizeBottomRight:
                        control.Width = control.Width + (e.X - secondlyPoint.X);
                        control.Height = control.Height + (e.Y - secondlyPoint.Y);
                        secondlyPoint.X = e.X;
                        secondlyPoint.Y = e.Y;
                        break;
                    case MousePointPosition.MouseSizeRight:
                        control.Width = control.Width + (e.X - secondlyPoint.X);
                        secondlyPoint.X = e.X;
                        secondlyPoint.Y = e.Y;
                        break;
                    case MousePointPosition.MouseSizeTop:
                        control.Top = control.Top + (e.Y - primaryPoint.Y);
                        control.Height = control.Height - (e.Y - primaryPoint.Y);
                        break;
                    case MousePointPosition.MouseSizeLeft:
                        control.Left = control.Left + (e.X - primaryPoint.X);
                        control.Width = control.Width - (e.X - primaryPoint.X);
                        break;
                    case MousePointPosition.MouseSizeBottomLeft:
                        control.Left = control.Left + (e.X - primaryPoint.X);
                        control.Width = control.Width - (e.X - primaryPoint.X);
                        control.Height = control.Height + (e.Y - secondlyPoint.Y);
                        secondlyPoint.X = e.X;
                        secondlyPoint.Y = e.Y;
                        break;
                    case MousePointPosition.MouseSizeTopRight:
                        control.Top = control.Top + (e.Y - primaryPoint.Y);
                        control.Width = control.Width + (e.X - secondlyPoint.X);
                        control.Height = control.Height - (e.Y - primaryPoint.Y);
                        secondlyPoint.X = e.X;
                        secondlyPoint.Y = e.Y;
                        break;
                    case MousePointPosition.MouseSizeTopLeft:
                        control.Left = control.Left + (e.X - primaryPoint.X);
                        control.Top = control.Top + (e.Y - primaryPoint.Y);
                        control.Width = control.Width - (e.X - primaryPoint.X);
                        control.Height = control.Height - (e.Y - primaryPoint.Y);
                        break;
                    default:
                        break;
                }

                if (control.Width < CONTROL_MINWIDTH) control.Width = CONTROL_MINWIDTH;
                if (control.Height < CONTROL_MINHEIGHT) control.Height = CONTROL_MINHEIGHT;
            }
            else
            {
                mousePointPosition = GetMousePointPosition(control.Size, e);

                SetMouseCursorIcon(mousePointPosition);
            }
        }

        /// <summary>
        /// mouse up event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void layoutPicBox_MouseUp(object sender, MouseEventArgs e)
        {
            Control control = (sender as Control);

            if (control.Parent != null)
            {
                if (control.Left < 0) control.Left = 0;
                if (control.Top < 0) control.Top = 0;
                if (control.Right > control.Parent.ClientSize.Width) control.Left = control.Parent.ClientSize.Width - control.Width;
                if (control.Bottom > control.Parent.ClientSize.Height) control.Top = control.Parent.ClientSize.Height - control.Height;

                string _name = ((MemoEdit)sender).Name;
                foreach (KeyValuePair<string, EquipmentLayoutDetailEntity> kvp in detailList)
                {
                    EquipmentLayoutDetailEntity equLayoutDetail = kvp.Value;
                    if (equLayoutDetail.PicName.Equals(_name) && equLayoutDetail.PicLeft == control.Left.ToString()
                        && equLayoutDetail.PicTop == control.Top.ToString()
                        && equLayoutDetail.PicWidth == control.Width.ToString()
                        && equLayoutDetail.PicHeight == control.Height.ToString())
                    {
                        blclickValue = true;
                        break;
                    }
                    else
                        blclickValue = false;
                }

                control.Parent.Refresh();
            }
            if (mousePointPosition == MousePointPosition.MouseDrag)
            {
                //更新DetailList中图片的坐标位置
                UpdateImageInfo(control.Name, control.Left, control.Top);

                //更新相关联的的图片的信息
                if (tempRelationList.Count > 0)
                {
                    foreach (EquipmentLayoutDetailEntity detailEntity in tempRelationList)
                    {
                        UpdateImageInfo(detailEntity.PicName,Convert.ToInt32(detailEntity.PicLeft), Convert.ToInt32(detailEntity.PicTop));
                    }
                    tempRelationList.Clear();
                }            

            }           
            else
            {
                if (mousePointPosition != MousePointPosition.MouseSizeNone)
                {
                    //更新DetatilList中图片的坐标位置和大小
                    UpdateImageInfo(control.Name, control.Left, control.Top, control.Width, control.Height);

                    //更新相关联的其他图片的信息
                    //foreach (EquipmentLayoutDetailEntity detailEntity in tempRelationList)
                    //{
                    //    UpdateImageInfo(detailEntity.PicName, Convert.ToInt32(detailEntity.PicLeft), Convert.ToInt32(detailEntity.PicTop),
                    //        Convert.ToInt32(detailEntity.PicWidth), Convert.ToInt32(detailEntity.PicHeight));
                    //}
                    //tempRelationList.Clear();                   
                }
                
            }
            
            mousePointPosition = MousePointPosition.MouseSizeNone;

            SetMouseCursorIcon(mousePointPosition);

            if (blclickValue)
                AddContextMenu();
        }       
      
        #endregion

        #region 底图的Drag事件
        private void LayoutPic_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(MemoEdit)))
            {
                validData = true;
                e.Effect = DragDropEffects.Copy;
            }
        }       
        private void LayoutPic_DragDrop(object sender, DragEventArgs e)
        {
            if (validData)
            {
                if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
                {
                    #region 生成新的图片
                    MemoEdit memoAdd=new MemoEdit();
                    MemoEdit memoEdit02 = (MemoEdit)(e.Data.GetData(typeof(MemoEdit)));
                    memoEdit02.Tag = "E";
                    memoAdd = memoEdit02;
                    
                    memoAdd.Width = memoEdit02.Width;
                    memoAdd.Height = memoEdit02.Height;

                    memoAdd.MouseDown += new MouseEventHandler(layoutPicBox_MouseDown);
                    memoAdd.MouseMove += new MouseEventHandler(layoutPicBox_MouseMove);
                    memoAdd.MouseUp += new MouseEventHandler(layoutPicBox_MouseUp);
                    //memoAdd.Properties.ShowMenu = false;
                    memoAdd.Name = (picCount++).ToString();//用序列号作为MemoEdit的名字，便于查找

                    memoAdd.MouseUp += new MouseEventHandler(layoutPicBox_MouseUp);
                    //Point p = this.PointToClient(new Point(Cursor.Position.X - LayoutPic.Left - picBox.Width / 2, Cursor.Position.Y - LayoutPic.Top - picBox.Height / 2));
                    Point p = this.LayoutPic.PointToClient(new Point(e.X, e.Y));
                    LayoutPic.Controls.Add(memoAdd);
                    memoAdd.Left = p.X;
                    memoAdd.Top = p.Y;
                    //picBox.Location = p;
                    #endregion

                    #region 
                    EquipmentLayoutDetailEntity equDetailEntity = new EquipmentLayoutDetailEntity();
                    equDetailEntity.PicHeight = memoAdd.Height.ToString();
                    equDetailEntity.PicWidth = memoAdd.Width.ToString();
                    equDetailEntity.PicLeft = p.X.ToString();//  picBox.Left.ToString();
                    equDetailEntity.PicTop = p.Y.ToString();//picBox.Top.ToString();
                    equDetailEntity.PicType = memoAdd.Tag.ToString();             
                    equDetailEntity.DetailColKey = CommonUtils.GenerateNewKey(0);//每个设备图片唯一主键

                    equDetailEntity.PicName = memoAdd.Name;

                    KeyValuePair<string, EquipmentLayoutDetailEntity> keyValuePair = new KeyValuePair<string, EquipmentLayoutDetailEntity>(memoAdd.Name, equDetailEntity);

                    detailList.Add(keyValuePair);
                    #endregion
                }               
            }
        }
        #endregion              

        #region Other private method

        private void AddContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("选择设备", null, new EventHandler(ChooseEquipment));
            //contextMenu.Items.Add("查看XXX设备信息", null, new EventHandler(LookEquipmentMsg));
            contextMenu.Items.Add("删除设备", null, new EventHandler(DeletePic));//删除设备
            //set the context menu's show position
            Point p = new Point(Control.MousePosition.X, Control.MousePosition.Y);
            p = this.PointToClient(p);
            contextMenu.Show(this, p);
        }
        /// <summary>
        /// 查看设备信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LookEquipmentMsg(object sender, EventArgs e)
        {
            LayoutEventStateHistory lesh = new LayoutEventStateHistory();
            lesh.ShowDialog();

        }
        /// <summary>
        /// 选择设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseEquipment(object sender, EventArgs e)
        {            
            if (movePictureEdit != null)
            {
                try
                {
                    //string queryName = "Equipment_" + movePictureEdit.Tag.ToString();
                    string queryName = "Equipment_E";
                    using (EquipmentCommonQueryDialog queryDialog = new EquipmentCommonQueryDialog(queryName, false))
                    {
                        if (queryDialog.ShowDialog() == DialogResult.OK)
                        {
                            if (queryDialog.SelectedData != null && queryDialog.SelectedData.Length > 0)
                            {
                                DataRow selectedRow = queryDialog.SelectedData[0];
                                string equipmentKey = selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString();//获取设备主键并绑定
                                string equipmentName = selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME].ToString();//设备名称，在ToolTip中显示
                                string parentEquipmentKey = string.Empty;
                                string totalChamber = selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL].ToString();
                                int chamberIndex =Convert.ToInt32(selectedRow[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX]);
                                string sColor=selectedRow[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString();
                                if (equipmentList.Contains(equipmentKey))
                                {
                                    MessageService.ShowMessage("该设备已被当前看板使用");
                                    return;
                                }
                                else
                                {                                  
                                    if (movePictureEdit.Tag != null && movePictureEdit.Tag.ToString() != string.Empty)//更换设备信息
                                    {
                                        //如果有腔体则不能修改 
                                        if (isExistChild(movePictureEdit.Tag.ToString()))
                                        {
                                            MessageService.ShowWarning("该设备存在腔体，不能更换设备");
                                            return;
                                        }
                                        equipmentList.Remove(movePictureEdit.Tag.ToString());
                               
                                    }

                                    equipmentList.Add(equipmentKey);
                                }
                                
                                movePictureEdit.Text = equipmentName;
                                movePictureEdit.Tag = equipmentKey;
                                movePictureEdit.ToolTip = equipmentName;
                                movePictureEdit.BackColor = new ColorType().GetStateColor(sColor);

                                //更新list中图片的信息
                                UpdateImageInfo(movePictureEdit.Name, equipmentKey, equipmentName,totalChamber,chamberIndex.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageService.ShowError(ex.Message);
                }
            }
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePic(object sender, EventArgs e)
        {
            if (movePictureEdit != null)
            {              
                //如果有绑定设备，则从equipmentList中清除
                if (movePictureEdit.Tag != null && movePictureEdit.Tag.ToString() != string.Empty)
                {
                    for (int i = 0; i < childParentRelationList.Count; i++)
                    {
                        if (childParentRelationList[i].Value == movePictureEdit.Tag.ToString())
                        {
                            MessageService.ShowMessage("该设备存在腔体不能删除");
                            return;
                        }
                    }

                    equipmentList.Remove(movePictureEdit.Tag.ToString());
                    #region
                    //if (movePictureEdit.Image.Tag.ToString() == "C")//删除腔体
                    //{
                    //    int index = FindIndexInChildParentRelationList(movePictureEdit.Tag.ToString());
                    //    if (index != childParentRelationList.Count)
                    //    {
                    //        childParentRelationList.RemoveAt(index);
                    //    }
                    //}
                    //else//删除设备
                    //{
                        //删除设备关联的所有腔体,及设备与腔体的关系
                        //for (int i = childParentRelationList.Count-1; i>=0; i--)
                        //{
                        //    if (childParentRelationList[i].Value == movePictureEdit.Tag.ToString())
                        //    {
                        //        PictureEdit chamberPic = FindEquipmentPic(childParentRelationList[i].Key);
                        //        equipmentList.Remove(childParentRelationList[i].Key);
                        //        childParentRelationList.RemoveAt(i);
                                
                        //        detailList.RemoveAt(Convert.ToInt32(chamberPic.Properties.Tag));//在DetailList中删除记录
                        //        LayoutPic.Controls.Remove(chamberPic); //删除腔体图片                               
                        //    }
                        //}                                      

                    //}
                    #endregion
                }
                LayoutPic.Controls.Remove(movePictureEdit);
                detailList.RemoveAt(GetIndexInList(movePictureEdit.Name));
            }
        }
     

        /// <summary>
        /// 查找父设备
        /// </summary>
        /// <param name="parentEquKey"></param>
        /// <returns></returns>  
        private EquipmentLayoutDetailEntity FindEquipmentPic(string equipmentKey)
        {
            for (int i = 0; i < detailList.Count; i++)
            {
                if (detailList[i].Value.EquipmentKey == equipmentKey)
                {                   
                    return detailList[i].Value;
                }
            }
            return null;
        }


        /// <summary>
        /// 检测设备是否存在腔体
        /// </summary>
        /// <param name="parentEquKey">设备主键</param>
        /// <returns></returns>
        private bool isExistChild(string parentEquKey)
        {
            for (int i = 0; i < childParentRelationList.Count; i++)
            {
                if (childParentRelationList[i].Value == parentEquKey)
                    return true;
            }
            return false;
        }
       
        private int FindIndexInChildParentRelationList(string key)
        {
            int i;
            for (i = 0; i < childParentRelationList.Count; i++)
            {
                if (childParentRelationList[i].Key == key)
                    return i;
            }
            return i;

        }

        private List<MemoEdit> FindRelationEquipment(string equipmentKey, string picType)
        {
            List<string> relationList = new List<string>();
            List<MemoEdit> picList = new List<MemoEdit>();
            //如果是腔体
            if (picType == "E")
            {
                for (int i = 0; i < childParentRelationList.Count; i++)
                {
                    if (childParentRelationList[i].Value == equipmentKey)
                    {
                        relationList.Add(childParentRelationList[i].Key);
                    }
                }
                //relationList.Add(equipmentKey);
            }
            //如果是设备
            else if (picType == "C")
            {
                string parentEquipmentKey = string.Empty;

                for (int i = 0; i < childParentRelationList.Count; i++)
                {
                    if (childParentRelationList[i].Key == equipmentKey)
                    {                     
                        parentEquipmentKey = childParentRelationList[i].Value;
                        break;
                    }
                }
                for (int i = 0; i < childParentRelationList.Count; i++)
                {
                    if (childParentRelationList[i].Value == parentEquipmentKey)
                    {
                        relationList.Add(childParentRelationList[i].Key);
                    }
                }
                relationList.Add(parentEquipmentKey);
                relationList.Remove(equipmentKey);//剔除掉自身
            }

            //查找对象MemoEdit
            if (relationList.Count > 0)
            {
                for (int k = 0; k < relationList.Count; k++)
                {
                    MemoEdit tempPic = (MemoEdit)this.LayoutPic.Controls.Find(FindEquipmentPic(relationList[k]).PicName, false)[0];
                    picList.Add(tempPic);
                }
            }
            return picList;
        }

        /// <summary>
        /// Get position of mouse point
        /// </summary>
        /// <param name="size"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private MousePointPosition GetMousePointPosition(Size size, MouseEventArgs e)
        {
            if ((e.X >= -1 * CONTROL_BANDWIDTH) | (e.X <= size.Width) | (e.Y >= -1 * CONTROL_BANDWIDTH) | (e.Y <= size.Height))
            {
                if (e.X < CONTROL_BANDWIDTH)
                {
                    if (e.Y < CONTROL_BANDWIDTH)
                    {
                        return MousePointPosition.MouseSizeTopLeft;
                    }
                    else
                    {
                        if (e.Y > -1 * CONTROL_BANDWIDTH + size.Height)
                        {
                            return MousePointPosition.MouseSizeBottomLeft;
                        }
                        else
                        {
                            return MousePointPosition.MouseSizeLeft;
                        }
                    }
                }
                else
                {
                    if (e.X > -1 * CONTROL_BANDWIDTH + size.Width)
                    {
                        if (e.Y < CONTROL_BANDWIDTH)
                        {
                            return MousePointPosition.MouseSizeTopRight;
                        }
                        else
                        {
                            if (e.Y > -1 * CONTROL_BANDWIDTH + size.Height)
                            {
                                return MousePointPosition.MouseSizeBottomRight;
                            }
                            else
                            {
                                return MousePointPosition.MouseSizeRight;
                            }
                        }
                    }
                    else
                    {
                        if (e.Y < CONTROL_BANDWIDTH)
                        {
                            return MousePointPosition.MouseSizeTop;
                        }
                        else
                        {
                            if (e.Y > -1 * CONTROL_BANDWIDTH + size.Height)
                            {
                                return MousePointPosition.MouseSizeBottom;
                            }
                            else
                            {
                                return MousePointPosition.MouseDrag;
                            }
                        }
                    }
                }
            }
            else
            {
                return MousePointPosition.MouseSizeNone;
            }
        }

        /// <summary>
        /// Set Icon Of Mouse Cursor
        /// </summary>
        /// <param name="mpp"></param>      
        private void SetMouseCursorIcon(MousePointPosition mpp)
        {
            switch (mpp)
            {
                case MousePointPosition.MouseSizeNone:
                    Cursor.Current = Cursors.Arrow;
                    break;
                case MousePointPosition.MouseDrag:
                    Cursor.Current = Cursors.SizeAll;
                    break;
                case MousePointPosition.MouseSizeBottom:
                    Cursor.Current = Cursors.SizeNS;
                    break;
                case MousePointPosition.MouseSizeTop:
                    Cursor.Current = Cursors.SizeNS;
                    break;
                case MousePointPosition.MouseSizeLeft:
                    Cursor.Current = Cursors.SizeWE;
                    break;
                case MousePointPosition.MouseSizeRight:
                    Cursor.Current = Cursors.SizeWE;
                    break;
                case MousePointPosition.MouseSizeBottomLeft:
                    Cursor.Current = Cursors.SizeNESW;
                    break;
                case MousePointPosition.MouseSizeBottomRight:
                    Cursor.Current = Cursors.SizeNWSE;
                    break;
                case MousePointPosition.MouseSizeTopLeft:
                    Cursor.Current = Cursors.SizeNWSE;
                    break;
                case MousePointPosition.MouseSizeTopRight:
                    Cursor.Current = Cursors.SizeNESW;
                    break;
                default:
                    Cursor.Current = Cursors.Arrow;
                    break;
            }
        }        

        /// <summary>
        /// 图片位置移动而更新
        /// </summary>
        /// <param name="index">图片名称</param>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        private void UpdateImageInfo(string name,int x,int y)
        {
            int index = GetIndexInList(name);
            if (index != detailList.Count)
            {
                detailList[index].Value.PicLeft = x.ToString();
                detailList[index].Value.PicTop = y.ToString();
            }
        }

        /// <summary>
        /// 图片绑定的设备或更换绑定设备而更新
        /// </summary>
        /// <param name="index">图片名称</param>
        /// <param name="quipmentKey">设备主键</param>
        private void UpdateImageInfo(string name, string equipmentKey,string equipmentName,string chamberTotal,string chamberIndex)
        {
            int index = GetIndexInList(name);
            if (index != detailList.Count)
            {
                detailList[index].Value.EquipmentKey = equipmentKey;
                detailList[index].Value.EquipmentName = equipmentName;
                detailList[index].Value.ChamberIndex = chamberIndex;
                detailList[index].Value.ChamberTotal = chamberTotal;
            }
        }

        /// <summary>
        /// 改变图片大小而更新
        /// </summary>
        /// <param name="index">图片名称</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void UpdateImageInfo(string name, int x, int y, int width, int height)
        {
            int index = GetIndexInList(name);
            if(index!=detailList.Count)
            {
                detailList[index].Value.PicLeft = x.ToString();
                detailList[index].Value.PicTop = y.ToString();
                detailList[index].Value.PicWidth = width.ToString();
                detailList[index].Value.PicHeight = height.ToString();
            }
        }

        /// <summary>
        /// 获取图片在DetailList 中的序号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int GetIndexInList(string name)
        {
            int i;
            for (i = 0; i < detailList.Count; i++)
            {
                if (detailList[i].Key == name)
                    return i;
            }
            return i;
        }        
      
        
        /// <summary>
        /// 将Byte数据转换为图片
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private Bitmap GetBitmap(byte[] imgData)
        {
            if (imgData != null)
            {
                MemoryStream ms = new MemoryStream(imgData, true);
                ms.Read(imgData, 0, imgData.Length);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                return bmp;
            }
            else
                return null;
        }

        /// <summary>
        /// 将图片数据作为Byte数组
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        private byte[] ReadImageData()
        {
            MemoryStream ms = new MemoryStream();
            // 保存成文件流           
            LayoutPic.Image.Save("a.bmp");
            FileStream fileStream = new FileStream("a.bmp", FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            byte[] BlobData = binaryReader.ReadBytes((int)fileStream.Length);
            binaryReader.Close();
            fileStream.Close();
            File.Delete("a.bmp");
            return BlobData;
        }

        /// <summary>
        /// Create table 
        /// </summary>
        /// <returns></returns>
        private DataTable CreateMainDataTable()
        {
            DataTable table = new DataTable(EMS_LAYOUT_MAIN_FIELDS.DATABASE_TABLE_NAME);
            DataColumn dataColumn1 = new DataColumn(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME, typeof(string));
            DataColumn dataColumn2 = new DataColumn(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_DESC, typeof(string));
            DataColumn dataColumn3 = new DataColumn(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_PIC, typeof(byte[]));
            DataColumn dataColumn4 = new DataColumn(EMS_LAYOUT_MAIN_FIELDS.CREATOR, typeof(string));
            DataColumn dataColumn5 = new DataColumn(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY, typeof(string));
            table.Columns.Add(dataColumn1);
            table.Columns.Add(dataColumn2);
            table.Columns.Add(dataColumn3);
            table.Columns.Add(dataColumn4);
            table.Columns.Add(dataColumn5);
            return table;
        }
        #endregion
        private void RefreshEnabled(bool bl)
        {
            lblAlarmTitle.Text = string.Empty;
            start = 0;
            if (bl)
            {
                btnHardRefresh.Enabled = true;
                timer1.Enabled = true;
                timer1.Interval = 1000;
            }
            else
            {
                btnHardRefresh.Enabled = false;
                timer1.Enabled = false;
                lblAlarmTitle.Text = string.Empty;
            }
        }
        private void RefreshEquNameState()
        {
            DataTable dtDetailList = new DataTable(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME);
            dtDetailList.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY);
            foreach (KeyValuePair<string, EquipmentLayoutDetailEntity> ekv in detailList)
            {
                if (!ekv.Value.EquipmentKey.Equals(string.Empty))
                    dtDetailList.Rows.Add(ekv.Value.EquipmentKey);
            }
            if (dtDetailList.Rows.Count < 1) return;

            string msg = string.Empty;
            DataSet resDs = EquLayoutEntity.GetLayoutEquipmentCurrStates(dtDetailList, out msg);
            DataTable dt = resDs.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
            foreach (DataRow dr in dt.Rows)
            {
                foreach (KeyValuePair<string, EquipmentLayoutDetailEntity> ekv in detailList)
                {
                    if (ekv.Value.EquipmentKey == dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString())
                    {
                        if (this.LayoutPic.Controls.ContainsKey(ekv.Key))
                        {
                            MemoEdit mo = (MemoEdit)this.LayoutPic.Controls.Find(ekv.Key, false)[0];
                            mo.BackColor = new ColorType().GetStateColor(dr[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString());
                            mo.Text = dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME].ToString();
                            mo.Tag = dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].ToString();
                            break;
                        }
                    }
                }
            }

            RefreshEnabled(true);
        }
        /// <summary>
        /// 获得设备的颜色,标示设备当前所处状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHardRefresh_Click(object sender, EventArgs e)
        {
            RefreshEnabled(false);
            RefreshEquNameState();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            start++;
            string leaveTime = (second - start).ToString();
            lblAlarmTitle.Text = string.Format("离下次更新还有{0}秒", leaveTime);

            if (leaveTime.Trim().Equals("0"))
            {
                start = 0;
                RefreshEquNameState();
            }         
        }
        private void InitialFormControls()
        {
            GridViewHelper.SetGridView(grvDataList);
            DataTable dataTable;
            DataColumn dc;
            GridColumn gridColumn;

            switch (this.formType)
            {
                case "EquipmentCheckItems": //设备检查项

                    #region Initial Equipment Check Items Controls

                    this.Text = "设备检查项查询";
                    this.lciQueryLabel.Text = "检查项名称";

                    dataTable = new DataTable(EMS_CHECKITEMS_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = "选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ROWNUM);

                    dc.Caption = StringParser.Parse("${res:Global.RowNumber}");
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY);

                    dc.Caption = "检查项ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_NAME);

                    dc.Caption = "检查项名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_TYPE);

                    dc.Caption = "检查项类型";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_DESCRIPTION);

                    dc.Caption = "描述";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CREATOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIME);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_EDITOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME);
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.grvDataList, dataTable);

                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].Width = 35;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CHECKITEM_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CREATOR].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_CREATE_TIME].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_EDITOR].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKITEMS_FIELDS.FIELD_EDIT_TIME].Visible = false;

                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);

                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }

                    #endregion

                    break;
                case "EquipmentCheckList": //设备检查表单

                    #region Initial Equipment Check List Controls

                    this.Text = "设备检查表单查询";
                    this.lciQueryLabel.Text = "检查表单名称";

                    dataTable = new DataTable(EMS_CHECKLIST_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = "选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ROWNUM);

                    dc.Caption = StringParser.Parse("${res:Global.RowNumber}");
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY);

                    dc.Caption = "检查表单ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME);

                    dc.Caption = "检查表单名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_TYPE);

                    dc.Caption = "检查表单类型";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_DESCRIPTION);

                    dc.Caption = "描述";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CREATOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIME);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_EDITOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIME);
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.grvDataList, dataTable);

                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].Width = 35;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CREATOR].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_CREATE_TIME].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_EDITOR].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_FIELDS.FIELD_EDIT_TIME].Visible = false;

                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);

                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }

                    #endregion

                    break;
                case "EquipmentCheckListJob": //设备检查表单任务

                    #region Initial Equipment Check List Job Controls

                    this.Text = "设备检查表单任务查询";
                    this.lciQueryLabel.Text = "检查表单任务名称";

                    dataTable = new DataTable(EMS_CHECKLIST_JOBS_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = "选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ROWNUM);

                    dc.Caption = StringParser.Parse("${res:Global.RowNumber}");
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY);

                    dc.Caption = "检查表单任务ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_NAME);

                    dc.Caption = "检查表单任务名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_DESCRIPTION);

                    dc.Caption = "描述";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_STATE);

                    dc.Caption = "检查表单任务状态";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP);

                    dc.Caption = "创建时间";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP);

                    dc.Caption = "开始时间";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP);

                    dc.Caption = "完成时间";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_PM_TYPE);

                    dc.Caption = "PM类型";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_PM_KEY);

                    dc.Caption = "PMID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_PM_NAME);

                    dc.Caption = "PM名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EQUIPMENT_KEY);

                    dc.Caption = "设备ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY);

                    dc.Caption = "检查表单ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);

                    dc.Caption = "设备名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_FIELDS.FIELD_CHECKLIST_NAME);

                    dc.Caption = "检查表单名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIME);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDITOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME);
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.grvDataList, dataTable);

                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].Width = 35;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_JOB_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_START_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_PM_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CHECKLIST_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATOR].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_CREATE_TIME].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDITOR].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_CHECKLIST_JOBS_FIELDS.FIELD_EDIT_TIME].Visible = false;

                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);

                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }

                    #endregion

                    break;
                case "EquipmentPart": //设备备件

                    #region Initial Equipment Part Controls

                    this.Text = "设备备件查询";
                    this.lciQueryLabel.Text = "备件名称";

                    dataTable = new DataTable(EMS_EQUIPMENT_PARTS_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = "选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ROWNUM);

                    dc.Caption = StringParser.Parse("${res:Global.RowNumber}");
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY);

                    dc.Caption = "备件ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_NAME);

                    dc.Caption = "备件名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_DESCRIPTION);

                    dc.Caption = "描述";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_TYPE);

                    dc.Caption = "备件类型";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_MODE);

                    dc.Caption = "备件型号";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_UNIT);

                    dc.Caption = "备件单位";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIME);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDITOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME);
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.grvDataList, dataTable);

                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].Width = 35;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EQUIPMENT_PART_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATOR].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_CREATE_TIME].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDITOR].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_PARTS_FIELDS.FIELD_EDIT_TIME].Visible = false;

                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);

                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }

                    #endregion

                    break;
                case "EquipmentTask": //设备作业

                    #region Initial Equipment Task Controls

                    this.Text = "设备作业查询";
                    this.lciQueryLabel.Text = "作业名称";

                    dataTable = new DataTable(EMS_EQUIPMENT_TASKS_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = "选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_ROWNUM);

                    dc.Caption = StringParser.Parse("${res:Global.RowNumber}");
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY);

                    dc.Caption = "作业ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_NAME);

                    dc.Caption = "作业名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_DESCRIPTION);

                    dc.Caption = "描述";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_STATE);

                    dc.Caption = "作业状态";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_USER_KEY);

                    dc.Caption = "创建用户";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP);

                    dc.Caption = "创建时间";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_USER_KEY);

                    dc.Caption = "开始用户";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP);

                    dc.Caption = "开始时间";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_USER_KEY);

                    dc.Caption = "完成用户";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_TIMESTAMP);

                    dc.Caption = "完成时间";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY);

                    dc.Caption = "设备转变状态ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_STATES_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_NAME);

                    dc.Caption = "设备转变状态名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY);

                    dc.Caption = "设备转变原因ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_CHANGE_REASONS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_NAME);

                    dc.Caption = "设备转变原因名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY);

                    dc.Caption = "设备ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);

                    dc.Caption = "设备名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIME);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDITOR);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIME);
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.grvDataList, dataTable);

                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].Width = 35;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_CHECKED].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    this.grvDataList.Columns[COMMON_FIELDS.FIELD_COMMON_ROWNUM].Width = 35;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_TASK_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_TIMESTAMP].DisplayFormat.FormatString = COMMON_FORMAT.DATETIME_FORMAT;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_CHANGE_REASON_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;

                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATOR].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_TIME].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDITOR].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENT_TASKS_FIELDS.FIELD_EDIT_TIME].Visible = false;

                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);

                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }

                    string msg;

                    dataTable = new EquipmentCheckListJobEntity().LoadUsersData(out msg);

                    if (string.IsNullOrEmpty(msg))
                    {
                        RepositoryItemGridLookUpEdit usersLookUpEdit = new RepositoryItemGridLookUpEdit();

                        usersLookUpEdit.DisplayMember = RBAC_USER_FIELDS.FIELD_USERNAME;
                        usersLookUpEdit.ValueMember = RBAC_USER_FIELDS.FIELD_USER_KEY;
                        usersLookUpEdit.NullText = string.Empty;
                        usersLookUpEdit.DataSource = dataTable;

                        gridColumn = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_CREATE_USER_KEY);

                        if (gridColumn != null)
                        {
                            gridColumn.ColumnEdit = usersLookUpEdit;
                        }

                        gridColumn = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_START_USER_KEY);

                        if (gridColumn != null)
                        {
                            gridColumn.ColumnEdit = usersLookUpEdit;
                        }

                        gridColumn = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENT_TASKS_FIELDS.FIELD_COMPLETE_USER_KEY);

                        if (gridColumn != null)
                        {
                            gridColumn.ColumnEdit = usersLookUpEdit;
                        }
                    }
                    else
                    {
                        MessageService.ShowError(msg);
                    }

                    #endregion

                    break;
                case "EquipmentLayout"://设备布局图
                    #region Initial Equipment Layout Controls

                    this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.EquipmentLayout.title}"); //"设备布局图查询";
                    this.lciQueryLabel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.EquipmentLayout.lbl.0001}"); //"设备布局图名称";

                    dataTable = new DataTable(EMS_LAYOUT_MAIN_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.EquipmentLayout.Column.0001}"); //"选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY);

                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.EquipmentLayout.Column.0002}");// "布局图ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME);

                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.EquipmentLayout.Column.0003}");// "布局图名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_DESC);

                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.EquipmentLayout.Column.0004}");// "描述";
                    dc.ReadOnly = true;

                    ControlUtils.InitialGridView(this.grvDataList, dataTable);
                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);
                    this.grvDataList.Columns[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY].Visible = false;

                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }
                    #endregion
                    break;

                case "Equipment_Q"://设备状态切换
                case "Equipment_E": //设备
                    #region Equipment Grid

                    this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.title}");//设备查询
                    this.lciQueryLabel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.lbl.0001}");//设备编码

                    dataTable = new DataTable(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0001}"); //"选择";
                    dc.ReadOnly = false;


                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY);

                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0002}"); //"设备ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0003}"); //"设备名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0004}"); //"设备描述";
                    dc.ReadOnly = true;

                    //Add by qym 20120530
                    dc = dataTable.Columns.Add("EQUIPMENT_STATE_NAME");
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0005}"); //"设备状态";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0006}"); //"设备编码";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0007}"); //"设备类型";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0008}"); //"设备型号";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0009}"); //"最小加工量";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0010}"); //"最大加工量";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH, typeof(bool));
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0011}"); //"是否支持批处理";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER, typeof(bool));
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0012}"); //"是否多腔体设备";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0013}"); //"腔体个数";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX);
                    dc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentCommonQueryDialog.Equipment_Q.Column.0014}"); //"腔体编号";




                    ControlUtils.InitialGridView(this.grvDataList, dataTable);
                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);
                    GridColumn gridColumn2 = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH);
                    GridColumn gridColumn3 = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER);
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX].Visible = false;

                    //add by qym 20120530
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL].Visible = false;


                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;
                        gridColumn2.ColumnEdit = checkEdit;
                        gridColumn3.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }
                    #endregion
                    break;
                case "Equipment_C"://腔体
                    #region Equipment Grid

                    this.Text = "设备查询";
                    this.lciQueryLabel.Text = "设备编码";

                    dataTable = new DataTable(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME);

                    dc = dataTable.Columns.Add(COMMON_FIELDS.FIELD_COMMON_CHECKED, typeof(bool));

                    dc.Caption = "选择";
                    dc.ReadOnly = false;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY);

                    dc.Caption = "设备ID";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);
                    dc.Caption = "设备名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION);
                    dc.Caption = "设备描述";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE);
                    dc.Caption = "设备编码";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE);
                    dc.Caption = "设备类型";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE);
                    dc.Caption = "设备型号";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY);
                    dc.Caption = "父设备主键";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add("PARENT_EQUIPMENT_NAME");
                    dc.Caption = "父设备名称";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY);
                    dc.Caption = "最小加工量";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY);
                    dc.Caption = "最大加工量";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH, typeof(bool));
                    dc.Caption = "是否支持批处理";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER, typeof(bool));
                    dc.Caption = "是否腔体";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX);
                    dc.Caption = "腔体编号";
                    dc.ReadOnly = true;

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER);
                    dc.Caption = "是否多腔体设备";

                    dc = dataTable.Columns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL);
                    dc.Caption = "腔体数量";


                    ControlUtils.InitialGridView(this.grvDataList, dataTable);
                    gridColumn = this.grvDataList.Columns.ColumnByFieldName(COMMON_FIELDS.FIELD_COMMON_CHECKED);
                    GridColumn batchColumn = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH);
                    GridColumn chamberColumn = this.grvDataList.Columns.ColumnByFieldName(EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER);
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL].Visible = false;
                    this.grvDataList.Columns[EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER].Visible = false;


                    if (gridColumn != null)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();

                        checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
                        checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);

                        gridColumn.ColumnEdit = checkEdit;
                        batchColumn.ColumnEdit = checkEdit;
                        chamberColumn.ColumnEdit = checkEdit;

                        StyleFormatCondition checkCondition = new StyleFormatCondition();

                        checkCondition.Appearance.BackColor = Color.Green;
                        checkCondition.Appearance.Options.UseBackColor = true;
                        checkCondition.ApplyToRow = true;
                        checkCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                        checkCondition.Value1 = true;
                        checkCondition.Column = gridColumn;

                        this.grvDataList.FormatConditions.Add(checkCondition);
                    }
                    #endregion

                    break;
                default:
                    break;
            }
        }
        private void checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.isMultiSelect)
            {
                DataTable dataTable = this.grdDataList.DataSource as DataTable;

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    DataRow[] selectedRows = dataTable.Select(string.Format("{0} = True", COMMON_FIELDS.FIELD_COMMON_CHECKED));

                    foreach (DataRow selectedRow in selectedRows)
                    {
                        selectedRow[COMMON_FIELDS.FIELD_COMMON_CHECKED] = false;
                    }
                }
            }

            if (this.grvDataList.EditingValueModified)
            {
                this.grvDataList.SetFocusedValue(this.grvDataList.EditingValue);
                this.grvDataList.UpdateCurrentRow();
            }

        }
    }
}
