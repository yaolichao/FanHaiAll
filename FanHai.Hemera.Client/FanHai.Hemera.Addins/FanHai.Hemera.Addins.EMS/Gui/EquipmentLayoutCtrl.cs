using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Addins.EMS;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using FanHai.Hemera.Utils.Controls;
using System.IO;
using DevExpress.XtraEditors;
using FanHai.Gui.Core;
using FanHai.Hemera.Addins.EMS.Gui;

namespace FanHai.Hemera.Addins.EMS
{
    public partial class EquipmentLayoutCtrl : BaseUserCtrl
    {
        //定义变量
        private EquipmentLayoutEntity EquLayoutEntity = new EquipmentLayoutEntity();
        int picCount = 0;
        private List<KeyValuePair<string, EquipmentLayoutDetailEntity>> detailList = new List<KeyValuePair<string, EquipmentLayoutDetailEntity>>();//用于保存明细数据 

        private List<KeyValuePair<string, string>> childParentRelationList = new List<KeyValuePair<string, string>>();//父设备与子设备关系  

        private List<string> equipmentList = new List<string>();//记录当前布局图中的所有设备
        private string equLayoutKey = string.Empty;
        private MemoEdit ViewPictureEdit = null;
        int start = 0, second = 5 * 60;//设定5分钟刷新一次//定时器 刷新数据
        private Dictionary<string, string> _dictionary = new Dictionary<string, string>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentLayoutCtrl()
        {
            InitializeComponent();
            this.btRefresh.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Refresh");

            InitUi();
        }

        private void InitUi()
        {
            //lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayout.title}");
            btnOpenFile.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayout.btn.0001}");
            btnHardRefresh.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayout.btn.0002}");
            btRefresh.Text = StringParser.Parse("${res:Global.Refresh}");
            btnSearch.Text = StringParser.Parse("${res:Global.Query}");
            picNameItem.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayout.lbl.0001}");
            StartDateItem.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayout.lbl.0002}");
            EndDateItem.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayout.lbl.0003}");
            lblAlarmTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayout.lbl.0004}");
            this.LayoutPic.Properties.NullText = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayout.lbl.0005}"); ;
        }

        /// <summary>
        /// 已经注释掉了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBox_MouseHover(object sender, EventArgs e)
        {

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
        /// 根据名字，日期查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPicName.Text.Trim())) return;
            if (string.IsNullOrEmpty(txtPicName.Tag.ToString().Trim())) return;
            if (string.IsNullOrEmpty(StartDate.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayout.msg.0002}"));//开始日期不能为空!
                StartDate.Focus();
                return;
            }
            if (string.IsNullOrEmpty(EndDate.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayout.msg.0003}"));//结束日期不能为空!
                EndDate.Focus();
                return;
            }
            if (Convert.ToDateTime(StartDate.Text) > Convert.ToDateTime(EndDate.Text))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayout.msg.0004}"));//开始日期不能大于结束日期!
                return;
            }

            string msg = string.Empty;
            DataSet dsReturn = EquLayoutEntity.GetEquipmentLayoutDetail(txtPicName.Tag.ToString(), out msg);
            if (string.IsNullOrEmpty(msg))
                QueryEquipmentData(dsReturn);
        }

        /// <summary>
        /// 根据layout查询里边个设备的状态灯数据
        /// </summary>
        /// <param name="dsReturn"></param>
        private void QueryEquipmentData(DataSet dsReturn)
        {
            //if (SelectedData == null) return;
            //DataRow selectedRow = SelectedData;
            //equLayoutKey = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY].ToString();
            //this.txtPicName.Text = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME].ToString();
            //this.txtPicName.Tag = equLayoutKey;

            //string msg;
            //DataSet dsReturn = EquLayoutEntity.GetEquipmentLayoutDetail(equLayoutKey, out msg);
            //if (string.IsNullOrEmpty(msg))
            //{
            if (dsReturn != null && dsReturn.Tables.Count > 0)
            {
                SetControlState(true);
                int layout_width = 1000, layout_height = 500;//默认是500高度
                #region 主表信息，则加载布局底图
                if (dsReturn.Tables.Contains(EMS_LAYOUT_MAIN_FIELDS.DATABASE_TABLE_NAME))
                {
                    //读取图片数据并转换为图片
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
                        //LayoutPic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze; 
                        LayoutPic.Controls.Clear();
                    }
                }
                #endregion

                #region 明细数据，则加载设备布局信息
                if (dsReturn.Tables.Contains(EMS_LAYOUT_DETAIL_FIELDS.DATABASE_TABLE_NAME))
                {
                    //需修改picCount值
                    DataTable detailTable = dsReturn.Tables[EMS_LAYOUT_DETAIL_FIELDS.DATABASE_TABLE_NAME];
                    if (detailTable.Rows.Count > 0)
                    {
                        picCount = detailTable.Rows.Count + 1;
                        this.detailList.Clear();
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
                            equDetailEntity.ColorName = detailTable.Rows[i][EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString();//读取当前设备状态

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

                            #region 绘制图片
                            MemoEdit picBox = new MemoEdit();

                            //添加设备提示
                            #region
                            try
                            {
                                if (picBox != null)
                                {
                                    if (string.IsNullOrEmpty(StartDate.Text.Trim())) return;
                                    if (string.IsNullOrEmpty(EndDate.Text.Trim())) return;
                                    Dictionary<string, string> _dictionary = new Dictionary<string, string>();

                                    _dictionary.Add(EMS_STATE_EVENT_FIELDS.CREATE_TIME, StartDate.Text.Trim() + " 00:00:01");
                                    _dictionary.Add(EMS_STATE_EVENT_FIELDS.EDIT_TIME, EndDate.Text.Trim() + " 23:59:59");
                                    _dictionary.Add(EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY, equDetailEntity.EquipmentKey);
                                    string msg = string.Empty;
                                    DataSet dsEquipmentWork = EquLayoutEntity.GetCurrentEquWorkList(_dictionary, out msg);
                                    int sum01 = 0, sum02 = 0;
                                    //int sum_temp = 0;
                                    string sTitle = string.Empty;
                                    DataRow dr00 = null;
                                    string s156m = "156M", s156p = "156P", s125m = "125M", s156q = "156Q";

                                    DataTable dt00 = dsEquipmentWork.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
                                    if (dt00.Rows.Count > 0)
                                        dr00 = dt00.Rows[0];
                                    DataTable dt01 = dsEquipmentWork.Tables["RUN_LOT"];
                                    DataTable dt02 = dsEquipmentWork.Tables["HOLD_LOT"];
                                    DataTable dt03 = dsEquipmentWork.Tables["WAITING_LOT"];

                                    if (dr00 != null)
                                    {
                                        sTitle += string.Format("资产编号:{0}",
                                            dr00[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_ASSETSNO].ToString());
                                        sTitle += string.Format("\r\n WPH目标值:{0}",
                                            dr00[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_WPH].ToString());
                                        sTitle += string.Format("\r\n Av_time:{0}",
                                            dr00[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_AV_TIME].ToString());
                                        sTitle += string.Format("\r\n Tract_time:{0}",
                                          dr00[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TRACT_TIME].ToString());

                                        sTitle += string.Format("\r\n 设备编码:{0},描述:{1}", dr00[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE].ToString(), dr00[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME].ToString());
                                        sTitle += string.Format("\r\n 设备状态:{0}", dr00[EMS_EQUIPMENT_STATES_FIELDS.FIELD_DESCRIPTION].ToString());
                                    }
                                    sTitle += "\n -----------------------------------------------------";

                                    sTitle += string.Format("\r\n 在制产品信息");
                                    string s01 = string.Empty;
                                    string s02 = string.Empty;
                                    int t01 = 0, t02 = 0, t03 = 0, t001 = 0, t002 = 0, t003 = 0;
                                    foreach (DataRow dr01 in dt01.Rows)
                                    {
                                        t01 += Convert.ToInt32(dr01["RUN_CELL_NUMBER"].ToString());
                                        t001 += Convert.ToInt32(dr01["RUN_LOC_COUNT"].ToString());
                                        sum01 += t01;
                                        sum02 += t001;
                                    }
                                    foreach (DataRow dr02 in dt02.Rows)
                                    {
                                        t02 += Convert.ToInt32(dr02["HOLD_CELL_NUMBER"].ToString());
                                        t002 += Convert.ToInt32(dr02["HOLD_LOC_COUNT"].ToString());
                                        sum01 += t02;
                                        sum02 += t002;
                                    }
                                    foreach (DataRow dr03 in dt03.Rows)
                                    {
                                        t03 += Convert.ToInt32(dr03["WAIT_CELL_NUMBER"].ToString());
                                        t003 += Convert.ToInt32(dr03["WAIT_LOC_COUNT"].ToString());
                                        sum01 += t03;
                                        sum02 += t003;
                                    }

                                    s01 = string.Format("\r\n 批次数:{0}(运行:{1} 暂停:{2} 等待: {3})", sum02.ToString(), t001.ToString(), t002.ToString(), t003.ToString());
                                    s02 = string.Format("\r\n 电池片数:{0}(运行:{1} 暂停:{2} 等待: {3})", sum01.ToString(), t01.ToString(), t02.ToString(), t03.ToString());
                                    sTitle += s01 + s02;
                                    sTitle += string.Format("\r\n 产品分布:批次数(电池片数)");
                                    List<string> lst = new List<string>() { s125m, s156m, s156p, s156q };
                                    foreach (string s in lst)
                                    {
                                        int v_totle_qty = 0, v_sub_qty = 0;
                                        DataRow[] drs01 = dt01.Select(string.Format("TYPE='{0}'", s));
                                        DataRow[] drs02 = dt02.Select(string.Format("TYPE='{0}'", s));
                                        DataRow[] drs03 = dt03.Select(string.Format("TYPE='{0}'", s));
                                        foreach (DataRow dr in drs01)
                                        {
                                            v_totle_qty += Convert.ToInt32(dr["RUN_LOC_COUNT"].ToString());
                                            v_sub_qty += Convert.ToInt32(dr["RUN_CELL_NUMBER"].ToString());
                                        }
                                        foreach (DataRow dr in drs02)
                                        {
                                            v_totle_qty += Convert.ToInt32(dr["HOLD_LOC_COUNT"].ToString());
                                            v_sub_qty += Convert.ToInt32(dr["HOLD_CELL_NUMBER"].ToString());
                                        }
                                        foreach (DataRow dr in drs03)
                                        {
                                            v_totle_qty += Convert.ToInt32(dr["WAIT_LOC_COUNT"].ToString());
                                            v_sub_qty += Convert.ToInt32(dr["WAIT_CELL_NUMBER"].ToString());
                                        }
                                        if (v_totle_qty > 0 && v_sub_qty > 0)
                                            sTitle += string.Format("\r\n {0}:{1}({2})", s, v_totle_qty.ToString(), v_sub_qty.ToString());
                                    }
                                    sTitle += "\n -----------------------------------------------------";

                                    if (dr00 != null)
                                    {
                                        sTitle += string.Format("\r\n 最后操作人员:{0}", dr00["EDITOR"].ToString());
                                        sTitle += string.Format("\r\n 最后操作人员:{0}", dr00["USERNAME"].ToString());
                                        sTitle += string.Format("\r\n 最后操作时间:{0}", dr00["EDIT_TIME"].ToString());
                                        sTitle += string.Format("\r\n 事件名称:{0}", dr00["EQUIPMENT_STATE_NAME"].ToString());
                                        sTitle += string.Format("\r\n 事件备注:{0}", dr00["REMARK"].ToString());
                                    }
                                    picBox.ToolTip = sTitle;
                                    picBox.ToolTipController.AutoPopDelay = 20000;
                                }
                            }
                            catch //(Exception ex)
                            { }

                            #endregion

                            picBox.Width = Convert.ToInt32(equDetailEntity.PicWidth);
                            picBox.Height = Convert.ToInt32(equDetailEntity.PicHeight);

                            string sText = string.Empty;
                            if (picBox.Width >= picBox.Height)
                                sText = equDetailEntity.EquipmentName;
                            if (picBox.Width < picBox.Height)
                            {
                                char[] chars = equDetailEntity.EquipmentName.ToCharArray();
                                foreach (char c in chars)
                                    sText += "  " + c.ToString() + "\r\n";
                            }

                            //picBox.Properties.ShowMenu = false;
                            picBox.Name = (i + 1).ToString();//用序列号作为PicBox的名字，便于查找
                            picBox.Tag = equDetailEntity.EquipmentKey;
                            picBox.Text = equDetailEntity.EquipmentName;
                            picBox.Text = sText;
                            picBox.Font = new Font("Tahoma", 8f);
                            picBox.Properties.ScrollBars = ScrollBars.None;

                            picBox.Properties.ReadOnly = true;
                            picBox.BackColor = new ColorType().GetStateColor(equDetailEntity.ColorName);
                            //picBox.MouseHover += new EventHandler(picBox_MouseHover);
                            picBox.MouseDown += new MouseEventHandler(layoutPicBox_MouseDown);

                            LayoutPic.Controls.Add(picBox);
                            picBox.Top = Convert.ToInt32(equDetailEntity.PicTop);
                            picBox.Left = Convert.ToInt32(equDetailEntity.PicLeft);

                            if ((picBox.Top + picBox.Height) > layout_height)
                                layout_height = picBox.Top + picBox.Height;
                            if ((picBox.Width + picBox.Left) > layout_width)
                                layout_width = picBox.Width + picBox.Left;

                            #endregion
                        }
                        RefreshEnabled(true);
                    }
                    else
                        RefreshEnabled(false);

                }
                this.LayoutPic.Width = layout_width + 30;
                this.LayoutPic.Height = layout_height + 30;
                #endregion
                //}
            }
        }

        /// <summary>
        /// 画面loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EquipmentLayoutCtrl_Load(object sender, EventArgs e)
        {
            this.StartDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
            this.EndDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
            //this.cmbShift.SelectedIndex = 0;        

            SetControlState(false);
            //根据厂别初始化画面
            IniFoad();

        }

        /// <summary>
        /// SetControlState
        /// </summary>
        /// <param name="enable"></param>
        private void SetControlState(bool enable)
        {
            this.btRefresh.Enabled = enable;
            this.txtPicName.Enabled = enable;
            this.StartDate.Enabled = enable;
            this.EndDate.Enabled = enable;
            //this.cmbShift.Enabled = enable;
            //this.btnSearch.Enabled = enable;
        }

        /// <summary>
        ///  根据厂别初始化机台状态画面
        /// </summary>
        private void IniFoad() 
        {
            EquipmentLayoutEntity equipmentLayoutEntity = new EquipmentLayoutEntity();
            string msg = string.Empty;
            DataTable dataTable = new DataTable();
            dataTable = equipmentLayoutEntity.SearchEquipmentLayout("", out msg);
            //if (string.IsNullOrEmpty(msg)) //返回是否有错误
            //{
            //}
            //else
            //{
            //}
            string strFactoryRoom = PropertyService.Get(PROPERTY_FIELDS.FACTORY_CODE);
            DataRow[] selectedRows = dataTable.Select("LAYOUT_NAME ='" + strFactoryRoom + "'");
            if (selectedRows.Length > 0)
            {
                DataRow selectedRow = selectedRows[0];


                equLayoutKey = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY].ToString();
                this.txtPicName.Text = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME].ToString();
                this.txtPicName.Tag = equLayoutKey;
                DataSet dsReturn = EquLayoutEntity.GetEquipmentLayoutDetail(equLayoutKey, out msg);
                if (string.IsNullOrEmpty(msg))
                {
                    QueryEquipmentData(dsReturn);
                }
            }
            else
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentLayout.msg.0001}"));//没有和当前车间代码一样的设备看板图,请自己导入
            }

        }

        /// <summary>
        /// 导入布局图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (EquipmentCommonQueryDialog queryDialog = new EquipmentCommonQueryDialog("EquipmentLayout", false))
            {
                if (queryDialog.ShowDialog() == DialogResult.OK)
                {
                    if (queryDialog.SelectedData != null && queryDialog.SelectedData.Length > 0)
                    {
                        DataRow selectedRow = queryDialog.SelectedData[0];
                        equLayoutKey = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY].ToString();
                        this.txtPicName.Text = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME].ToString();
                        this.txtPicName.Tag = equLayoutKey;

                        string msg;
                        DataSet dsReturn = EquLayoutEntity.GetEquipmentLayoutDetail(equLayoutKey, out msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            QueryEquipmentData(dsReturn);
                        }

                    }
                }
            }
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
                //primaryPoint.X = e.X;
                //primaryPoint.Y = e.Y;
                //secondlyPoint.X = e.X;
                //secondlyPoint.Y = e.Y;

                MemoEdit picEdit = (MemoEdit)(sender);

                ViewPictureEdit = picEdit;
                if (picEdit.Tag.ToString().Equals(string.Empty)) return;

                AddContextMenu();

            }

        }

        /// <summary>
        /// AddContextMenu
        /// </summary>
        private void AddContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("设备信息", null, new EventHandler(LookEquipmentMsg));

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
            if (this.StartDate.Text.Trim() == string.Empty)
            {
                MessageService.ShowMessage("请选择开始时间");
                this.StartDate.Focus();
                return;
            }
            if (this.EndDate.Text.Trim() == string.Empty)
            {
                MessageService.ShowMessage("请选择结束时间");
                this.EndDate.Focus();
                return;
            }
            if (Convert.ToDateTime(this.EndDate.Text).CompareTo(Convert.ToDateTime(this.StartDate.Text)) < 0)
            {
                MessageService.ShowMessage("结束时间不能小于开始时间");
                this.EndDate.Focus();
                return;
            }
            if (ViewPictureEdit != null)
            {
                _dictionary.Clear();

                _dictionary.Add(EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY, ViewPictureEdit.Tag.ToString());
                _dictionary.Add(EMS_STATE_EVENT_FIELDS.CREATE_TIME, StartDate.Text.Trim() + " 00:00:00");
                _dictionary.Add(EMS_STATE_EVENT_FIELDS.EDIT_TIME, EndDate.Text.Trim() + " 23:59:59");

                LayoutEventStateHistory lesh = new LayoutEventStateHistory(_dictionary);
                lesh.Text = ViewPictureEdit.Text + "详细信息";
                lesh.ShowDialog();
            }
        }

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

        private void RefreshEquNameState()
        {


            btnSearch_Click(null, null);
            RefreshEnabled(true);
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }


        private void btnC1A1_Click(object sender, EventArgs e)
        {
            EquipmentLayoutEntity equipmentLayoutEntity = new EquipmentLayoutEntity();
            string msg = string.Empty;
            DataTable dataTable = new DataTable();
            dataTable = equipmentLayoutEntity.SearchEquipmentLayout("", out msg);
            DataRow[] selectedRows = dataTable.Select("LAYOUT_NAME ='F5M7'");
            if (selectedRows.Length > 0)
            {
                DataRow selectedRow = selectedRows[0];
                equLayoutKey = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY].ToString();
                this.txtPicName.Text = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME].ToString();
                this.txtPicName.Tag = equLayoutKey;
                DataSet dsReturn = EquLayoutEntity.GetEquipmentLayoutDetail(equLayoutKey, out msg);
                if (string.IsNullOrEmpty(msg))
                {
                    QueryEquipmentData(dsReturn);
                }
            }

        }


        private void btnC1B1_Click(object sender, EventArgs e)
        {
            EquipmentLayoutEntity equipmentLayoutEntity = new EquipmentLayoutEntity();
            string msg = string.Empty;
            DataTable dataTable = new DataTable();
            dataTable = equipmentLayoutEntity.SearchEquipmentLayout("", out msg);
            DataRow[] selectedRows = dataTable.Select("LAYOUT_NAME ='F5M9'");
            if (selectedRows.Length > 0)
            {
                DataRow selectedRow = selectedRows[0];
                equLayoutKey = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY].ToString();
                this.txtPicName.Text = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME].ToString();
                this.txtPicName.Tag = equLayoutKey;
                DataSet dsReturn = EquLayoutEntity.GetEquipmentLayoutDetail(equLayoutKey, out msg);
                if (string.IsNullOrEmpty(msg))
                {
                    QueryEquipmentData(dsReturn);
                }
            }

        }


        private void btnC3C1_Click(object sender, EventArgs e)
        {
            EquipmentLayoutEntity equipmentLayoutEntity = new EquipmentLayoutEntity();
            string msg = string.Empty;
            DataTable dataTable = new DataTable();
            dataTable = equipmentLayoutEntity.SearchEquipmentLayout("", out msg);
            DataRow[] selectedRows = dataTable.Select("LAYOUT_NAME ='C3C1'");
            if (selectedRows.Length > 0)
            {
                DataRow selectedRow = selectedRows[0];
                equLayoutKey = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY].ToString();
                this.txtPicName.Text = selectedRow[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME].ToString();
                this.txtPicName.Tag = equLayoutKey;
                DataSet dsReturn = EquLayoutEntity.GetEquipmentLayoutDetail(equLayoutKey, out msg);
                if (string.IsNullOrEmpty(msg))
                {
                    QueryEquipmentData(dsReturn);
                }
            }
        }
    }
}
