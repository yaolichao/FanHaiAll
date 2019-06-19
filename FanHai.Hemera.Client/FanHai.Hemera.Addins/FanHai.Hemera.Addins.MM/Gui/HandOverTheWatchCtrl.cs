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

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;


namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 表示材料耗用的窗体类。
    /// </summary>
    public partial class HandOverTheWatchCtrl : BaseUserCtrl
    {
        #region define variables
        public static int Flag = 0;
        #endregion

        #region define variables
        string _lupFactoryRoom = string.Empty;
        string _cmbGongXuName = string.Empty;
        string _lupJiaoBanShife = string.Empty;
        string _lupJieBanShife = string.Empty;
        string _timJiaoBanStart = string.Empty;
        string _timJiaoBanEnd = string.Empty;
        string _lupZhuangTai = string.Empty;
        #endregion

        #region define variables
        private static string operationHandoverKey = string.Empty;
        private static string dangQianBanBie = string.Empty;
        private static string factoryRoom = string.Empty;
        private static string gongXu = string.Empty;
        private static string jiaoBanShife = string.Empty;
        private static string jieBanShife = string.Empty;
        private static string jiaoJieDate = string.Empty;
        private static string zhuangTai = string.Empty;
        private static string jiaoBanPeople = string.Empty;
        private static string jieBanPeople = string.Empty;
        #endregion

        #region define variables
        public static string factRoom = string.Empty;
        public static string operation = string.Empty;
        public static string shift = string.Empty;
        public static string sendoperator = string.Empty;
        public static string receiveoperator = string.Empty;
        #endregion

        #region Properties

        public int Flag2
        {
            get
            {
                return Flag;
            }
            set
            {
                Flag = value;
            }
        }


        public string OperationHandoverKey
        {
            get
            {
                return operationHandoverKey;
            }
            set
            {
                operationHandoverKey = value;
            }
        }
        public string DangQianBanBie
        {
            get
            {
                return dangQianBanBie;
            }
            set
            {
                dangQianBanBie = value;
            }
        }
        public string FactoryName
        {
            get
            {
                return factoryRoom;
            }
            set
            {
                factoryRoom = value;
            }
        }
        public string GongXu
        {
            get
            {
                return gongXu;
            }
            set
            {
                gongXu = value;
            }
        }
        public string JiaoBanShifeName
        {
            get
            {
                return jiaoBanShife;
            }
            set
            {
                jiaoBanShife = value;
            }
        }
        public string JieBanShifeName
        {
            get
            {
                return jieBanShife;
            }
            set
            {
                jieBanShife = value;
            }
        }
        public string JiaoJieDate
        {
            get
            {
                return jiaoJieDate;
            }
            set
            {
                jiaoJieDate = value;
            }
        }

        public string ZhuangTai
        {
            get
            {
                return zhuangTai;
            }
            set
            {
                zhuangTai = value;
            }
        }
        public string JiaoBanPeople
        {
            get
            {
                return jiaoBanPeople;
            }
            set
            {
                jiaoBanPeople = value;
            }
        }

        public string JieBanPeople
        {
            get
            {
                return jieBanPeople;
            }
            set
            {
                jieBanPeople = value;
            }
        }
        #endregion

        #region Properties
        public string FactRoom
        {
            get
            {
                return factRoom;
            }
            set
            {
                factRoom = value;
            }
        }
        public string Operation
        {
            get
            {
                return operation;
            }
            set
            {
                operation = value;
            }
        }

        public string Shift
        {
            get
            {
                return shift;
            }
            set
            {
                shift = value;
            }
        }
        public string Sendoperator
        {
            get
            {
                return sendoperator;
            }
            set
            {
                sendoperator = value;
            }
        }
        public string Receiveoperator
        {
            get
            {
                return receiveoperator;
            }
            set
            {
                receiveoperator = value;
            }
        }

        #endregion


        /// <summary>
        /// 构造函数
        /// </summary>
        public HandOverTheWatchCtrl()
        {
            int flag1 = Flag;
            string factRoom1 = factRoom;
            string operation1 = operation;
            string shift1 = shift;
            InitializeComponent();
            GetFacRoomByStores();
            GetGongXu();
            BindShiftName();
            this.txtJobNumber.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            BindGridViewSource();
        }
        /// <summary>
        /// 通过用户拥有的线边仓获取工厂名称到控件中
        /// </summary>
        public void GetFacRoomByStores()
        {
            #region
            //绑定工厂车间名称
            DataTable dt2 = FactoryUtils.GetFactoryRoomByStores(PropertyService.Get(PROPERTY_FIELDS.STORES));
            //绑定工厂车间数据到窗体控件。
            lupFacRoom.Properties.DataSource = dt2;
            lupFacRoom.Properties.DisplayMember = "LOCATION_NAME";
            lupFacRoom.Properties.ValueMember = "LOCATION_KEY";
            //有数据，设置窗体控件的默认索引为0。
            if (dt2.Rows.Count > 0)
            {
                lupFacRoom.ItemIndex = 0;
            }
            #endregion
        }

        /// <summary>
        /// 获取登陆用户拥有权限的工序名称
        /// </summary>
        public void GetGongXu()
        {
            //绑定工序
            #region Bind Operation
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            if (operations.Length > 0)
            {
                string[] strOperations = operations.Split(',');
                for (int i = 0; i < strOperations.Length; i++)
                {
                    lupGongXu.Properties.Items.Add(strOperations[i]);
                }
                this.lupGongXu.SelectedIndex = 0;
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

            this.lupShift.Properties.DataSource = BaseData.Get(columns, category);
            this.lupShift.Properties.DisplayMember = "CODE";
            this.lupShift.Properties.ValueMember = "CODE";
            Shift shift = new Shift();
            this.lupShift.EditValue = shift.GetCurrShiftName();
        }
        /// <summary>
        /// 绑定数据表数据
        /// </summary>
        public void BindGridViewSource()
        {
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            //string stores = PropertyService.Get(PROPERTY_FIELDS.STORES);  
            //获取用户拥有权限的工厂名称
            DataTable dt2 = FactoryUtils.GetFactoryRoomByStores(PropertyService.Get(PROPERTY_FIELDS.STORES));
            OperationHandover _operationHandover = new OperationHandover();
            gdcData.MainView = gdvHandOverTheWatch;
            DataSet ds = _operationHandover.GetOperationHandoverBySAndF(operations, dt2);
            gdcData.DataSource = ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        /// <summary>
        /// 自定义绘制单元格的值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdvHandOverTheWatch_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "ROWNUM": //设置行号
                    e.DisplayText = Convert.ToString(e.RowHandle + 1);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 对查询界面返回值进行取值赋值到变量中
        /// </summary>
        /// <param name="ud">窗体对象</param>
        public void SetRetrunToDefine(HandOverTheWatchDialog handDialog)
        {
            this._lupFactoryRoom = handDialog.FactoryRoom;
            this._cmbGongXuName = handDialog.GongXuName;
            this._lupJiaoBanShife = handDialog.JiaoBanShife;
            this._lupJieBanShife = handDialog.JieBanShife;
            this._timJiaoBanStart = handDialog.JiaoBanStart.ToString();
            this._timJiaoBanEnd = handDialog.JiaoBanEnd.ToString();
            this._lupZhuangTai = handDialog.ZhuangTai;
        }
        /// <summary>
        /// 交班按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRefresMaterial_Click(object sender, EventArgs e)
            {
            Flag = 1;
            
            if (lupFacRoom.Text == "")
            {
                MessageService.ShowMessage("请选择工厂名称?", "${res:Global.SystemInfo}");
                return;
            }
            if (lupGongXu.Text == "")
            {
                MessageService.ShowMessage("请选择工序名称?", "${res:Global.SystemInfo}");
                return;
            }
            factRoom = this.lupFacRoom.EditValue.ToString();
            operation = this.lupGongXu.EditValue.ToString();
            shift = this.lupShift.EditValue.ToString();
            sendoperator = this.txtJobNumber.EditValue.ToString();
            OperationHandover _operationHandover = new OperationHandover();
            string _lupShift = lupShift.Text.Trim();
            string _lupGongXu = lupGongXu.Text.Trim();
            string facRoom = lupFacRoom.Text;
            string _lupFacRoomKey = lupFacRoom.Properties.GetKeyValueByDisplayText(facRoom).ToString();
            //根据当前班次和当前日期获取上一班次和上一班的交班日期。根据上一班次、工厂车间、工序名称、上一班的交班日期获取上一班的交班记录
            DataSet dsGetShangBanShift = _operationHandover.GetShangBanShift(_lupShift, _lupGongXu, _lupFacRoomKey);
            if (dsGetShangBanShift.Tables[0].Rows.Count > 0)
            {
                if (dsGetShangBanShift.Tables[0].Rows[0][0].ToString() != "2")
                {//如果上一班次的工序交接班记录状态不为”2：已接班“，则提示用户”不能交班，您还没有进行接班操作，是否执行接班操作?"
                    //MessageService.ShowMessage("不能交班，您还没有进行接班操作，是否执行接班操作?", "${res:Global.SystemInfo}");
                    if (MessageBox.Show(StringParser.Parse("不能交班，您还没有进行接班操作，是否执行接班操作?"),
                         StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        tsbJieBan_Click(sender, e);
                    }
                }
                else
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
                    WorkbenchSingleton.Workbench.ShowView(new HandOverTheWatchListContent());
                }
            }
            else
            {
                DataSet dsGetShangBan = _operationHandover.GetShangBan(_lupGongXu, _lupFacRoomKey);
                if (dsGetShangBan.Tables[0].Rows.Count > 0)
                {//如果上一班次的工序交接班记录不存在，继续根据工厂车间、工序名称判断是否存在工序交接班记录
                    MessageService.ShowMessage("上一班次未执行交接班操作，请补齐交接班记录", "${res:Global.SystemInfo}");
                }
                else
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
                    WorkbenchSingleton.Workbench.ShowView(new HandOverTheWatchListContent());
                }
            }
        }

        /// <summary>
        /// 查询交接班记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSelect_Click(object sender, EventArgs e)
        {
            HandOverTheWatchDialog handOverTheWatchDialog = new HandOverTheWatchDialog();
            if (handOverTheWatchDialog.ShowDialog() == DialogResult.OK)
            {
                SetRetrunToDefine(handOverTheWatchDialog);
                string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
                //string stores = PropertyService.Get(PROPERTY_FIELDS.STORES);  
                //获取用户拥有权限的工厂名称
                DataTable dt2 = FactoryUtils.GetFactoryRoomByStores(PropertyService.Get(PROPERTY_FIELDS.STORES));
                OperationHandover _operationHandover = new OperationHandover();
                gdcData.MainView = gdvHandOverTheWatch;
                //查询
                DataSet ds = _operationHandover.GetOperationHandoverByReturn(_lupFactoryRoom, _cmbGongXuName, _lupJiaoBanShife, _lupJieBanShife, _timJiaoBanStart, _timJiaoBanEnd, _lupZhuangTai, operations, dt2);
                gdcData.DataSource = ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }
        }


        /// <summary>
        /// 接班按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbJieBan_Click(object sender, EventArgs e)
        {
            Flag = 2;
            if (lupFacRoom.Text == "")
            {
                MessageService.ShowMessage("请选择工厂名称?", "${res:Global.SystemInfo}");
                return;
            }
            if (lupGongXu.Text == "")
            {
                MessageService.ShowMessage("请选择工序名称?", "${res:Global.SystemInfo}");
                return;
            }
            OperationHandover _operationHandover = new OperationHandover();

            factRoom = this.lupFacRoom.EditValue.ToString();
            operation = this.lupGongXu.EditValue.ToString();
            shift = this.lupShift.EditValue.ToString();
            receiveoperator = this.txtJobNumber.EditValue.ToString();
            sendoperator = this.txtJobNumber.EditValue.ToString();

            string _lupShift = lupShift.Text.Trim();
            string _lupGongXu = lupGongXu.Text.Trim();
            string facRoom = lupFacRoom.Text;
            string _lupFacRoomKey = lupFacRoom.Properties.GetKeyValueByDisplayText(facRoom).ToString();
            //根据当前班次和当前日期获取上一班次和上一班的交班日期。根据上一班次、工厂车间、工序名称、上一班的交班日期获取上一班的交班记录
            DataSet dsGetShangBanShift = _operationHandover.GetShangBanShift(_lupShift, _lupGongXu, _lupFacRoomKey);
            if (dsGetShangBanShift.Tables[0].Rows.Count > 0)
            {
                if (dsGetShangBanShift.Tables[0].Rows[0][0].ToString() == "1")
                {//如果存在上一班交班记录且交班记录状态为已交班，打开“工序交接班清单”界面
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
                    WorkbenchSingleton.Workbench.ShowView(new HandOverTheWatchListContent());
                }
                else if (dsGetShangBanShift.Tables[0].Rows[0][0].ToString() == "2")
                {
                    MessageService.ShowMessage("已进行接班，不能再次接班。", "${res:Global.SystemInfo}");
                }
                else if (dsGetShangBanShift.Tables[0].Rows[0][0].ToString() == "0")
                {//如果存在上一班交班记录且交班记录状态为未交班，给出提示上一班没有确认交班，不能进行接班。

                    MessageService.ShowMessage("上一班没有确认交班，不能进行接班。", "${res:Global.SystemInfo}");
                }
            }
            else
            {
                MessageService.ShowMessage("上一班没有交班记录，不能进行接班。", "${res:Global.SystemInfo}");
            }
        }

        private void gdvHandOverTheWatch_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
        }

        /// <summary>
        /// 数据表双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdcData_DoubleClick(object sender, EventArgs e)
        {
            //获取视图的鼠标的焦点
            //GridHitInfo gridHitInfo = this.gdvHandOverTheWatch.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));
            int rowHandle = gdvHandOverTheWatch.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                operationHandoverKey = this.gdvHandOverTheWatch.GetRowCellValue(rowHandle, "OPERATION_HANDOVER_KEY").ToString();
                factoryRoom = this.gdvHandOverTheWatch.GetRowCellValue(rowHandle, "LOCATION_NAME").ToString();
                gongXu = this.gdvHandOverTheWatch.GetRowCellValue(rowHandle, "ROUTE_OPERATION_NAME").ToString();
                jiaoBanShife = this.gdvHandOverTheWatch.GetRowCellValue(rowHandle, "SEND_SHIFT_VALUE").ToString();
                jieBanShife = this.gdvHandOverTheWatch.GetRowCellValue(rowHandle, "RECEIVE_SHIFT_VALUE").ToString();
                jiaoJieDate = this.gdvHandOverTheWatch.GetRowCellValue(rowHandle, "HANDOVER_DATE").ToString();
                zhuangTai = this.gdvHandOverTheWatch.GetRowCellValue(rowHandle, "STATUS").ToString();
                jiaoBanPeople = this.gdvHandOverTheWatch.GetRowCellValue(rowHandle, "SEND_OPERATOR").ToString();
                jieBanPeople = this.gdvHandOverTheWatch.GetRowCellValue(rowHandle, "RECEIVE_OPERATOR").ToString();
                //if (!string.IsNullOrEmpty(lupShift.EditValue.ToString()))
                dangQianBanBie = lupShift.EditValue.ToString();

                //通过工厂名获取工厂主键
                OperationHandover _operationHandover = new OperationHandover();
                DataSet dsFacKeyByName = new DataSet();
                dsFacKeyByName = _operationHandover.GetFacKeyByFacName(this.gdvHandOverTheWatch.GetRowCellValue(rowHandle, "LOCATION_NAME").ToString());
                factRoom = dsFacKeyByName.Tables[0].Rows[0][0].ToString();
                operation = this.gdvHandOverTheWatch.GetRowCellValue(rowHandle, "ROUTE_OPERATION_NAME").ToString();
                shift = this.lupShift.EditValue.ToString();
                receiveoperator = this.txtJobNumber.EditValue.ToString();
            }

            Flag = 0;
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
            WorkbenchSingleton.Workbench.ShowView(new HandOverTheWatchListContent());

        }

        private void HandOverTheWatchCtrl_Load(object sender, EventArgs e)
        {
            GetFacRoomByStores();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            BindGridViewSource();
        }
    }
}
