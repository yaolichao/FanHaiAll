using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Controls;
using DevExpress.XtraLayout.Utils;
using FanHai.Gui.Framework.Gui;
using DevExpress.XtraEditors.Controls;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 批次操作（电池片报废、组件报废）的界面。
    /// </summary>
    public partial class LotOperation : BaseUserCtrl
    {
        IViewContent _viewContent = null;
        private LotQueryEntity _entity;
        private LotOperationType _operationType;
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.INFORMATION}"); //提示
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotOperation(LotOperationType operationType,IViewContent content)
        {
            InitializeComponent();
            _viewContent = content;
            _operationType = operationType;
            _entity=new LotQueryEntity();
            InitializeLanguage();
        }


        private void InitializeLanguage()
        {
            this.btnOk.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.btnOk}");//"开始作业";
            this.btnClose.Text = "重置";//"重置";
            this.lcgTop.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.lcgTop}");//"表头";
            this.lciFactoryRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.lciFactoryRoom}");//"车间名称";
            //this.lcgContent.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.lcgContent}");//"内容";
            this.lciLotNumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.lciLotNumber}");//"序列号";
            this.lcgHidden.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.lcgHidden}");//"隐藏";
            this.lciUserNumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.lciUserNumber}");//"员工号";
            this.lciShift.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.lciShift}");//"班别";
            //this.lcgCommands.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.lcgCommands}");//"命令按钮";
            this.lciBtnClose.Text = "重置";//"重置";
            this.lciBtnOK.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.btnOk}");//"开始作业";
            if (this._operationType == LotOperationType.Hold)
            {
                this.lblMenu.Text = "生产管理>组件管理>单件暂停"; //暂停批次
            }
            else if (this._operationType == LotOperationType.BatchHold)
            {
                this.lblMenu.Text = "生产管理>组件管理>多件暂停"; //批量暂停批次
            }
            else if (this._operationType == LotOperationType.Release)
            {
                this.lblMenu.Text = "生产管理>组件管理>单件释放";  //释放批次
            }
            else if (this._operationType == LotOperationType.BatchRelease)
            {
                this.lblMenu.Text = "生产管理>组件管理>多件释放";  //批量释放批次
            }
            else if (this._operationType == LotOperationType.Adjust)
            {
                this.lblMenu.Text = "生产管理>组件管理>单件信息修改"; //在制品调整
            }
            else if (this._operationType == LotOperationType.BatchAdjust)
            {
                this.lblMenu.Text = "生产管理>组件管理>多件信息修改"; //批量调整批次
            }
            else if (this._operationType == LotOperationType.ReturnMaterial)
            {
                this.lblMenu.Text = "生产管理>电池片管理>组件退料"; //退料操作
            }
            else if (this._operationType == LotOperationType.CellPatch)
            {
                this.lblMenu.Text = "生产管理>电池片管理>硅片补片"; //电池片补片
            }
            else if (this._operationType == LotOperationType.CellRecovered)
            {
                this.lblMenu.Text = "生产管理>电池片管理>硅片回收"; //电池片回收
            }
            else if (this._operationType == LotOperationType.Scrap)
            {
                this.lblMenu.Text = "质量管理>不良品管理>报废作业";//组件报废
            }
            else if (this._operationType == LotOperationType.Defect)
            {
                this.lblMenu.Text = "质量管理>不良品管理>不良判定"; //组件不良
            }
            else if (this._operationType == LotOperationType.Rework)
            {
                this.lblMenu.Text = "质量管理>不良品管理>单件返修"; //返修
            }
            else if (this._operationType == LotOperationType.BatchRework)
            {
                this.lblMenu.Text = "质量管理>不良品管理>多件返修"; //批量返修
            }
            //else if (this._operationType == LotOperationType.Terminal)
            //{
            //    this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title016}"); //结批操作
            //    //this.TitleName = "结批操作";
            //}
            //else if (this._operationType == LotOperationType.Merge)
            //{
            //    this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title017}"); //合并批次
            //    //this.TitleName = "合并批次";
            //}
            //else if (this._operationType == LotOperationType.Split)
            //{
            //    this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title018}"); //拆分批次
            //    //this.TitleName = "拆分批次";
            //}
            //else if (this._operationType == LotOperationType.CellScrap)
            //{
            //    this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title001}"); //电池片报废
            //    //this.TitleName = "电池片报废";
            //}
            //else if (this._operationType == LotOperationType.CellDefect)
            //{
            //    this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title003}"); //电池片不良
            //    //this.TitleName = "电池片不良";
            //}
            //else if (this._operationType == LotOperationType.Defect)
            //{
            //    this.TitleName = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperationViewContent.Title004}"); //组件不良
            //    //this.TitleName = "组件不良";
            //}
            else
            {
                this.lblMenu.Text = "批次操作"; //批次操作
            }

        }




        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotOperation_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            BindShiftName();
            //this.lblMenu.Text = "批次操作";
            this.teUserNumber.Text=PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            this.teUserNumber.Enabled = false;
            //this.lblApplicationTitle.Text = _viewContent.TitleName;
            this.beLotNumber.Select();
        }
        /// <summary>
        /// 绑定工厂车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            //绑定工厂车间数据到窗体控件。
            if (dt != null)
            {
                this.lueFactoryRoom.Properties.DataSource = dt;
                this.lueFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
                this.lueFactoryRoom.ItemIndex = 0;
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
            //禁用领料车间
            if (dt == null || dt.Rows.Count <= 1)
            {
                this.lueFactoryRoom.Properties.ReadOnly = true;
            }
        }
        
        /// <summary>
        /// 绑定班别数据。
        /// </summary>
        private void BindShiftName()
        {
            //获取当前班别名称。
            Shift _shift = new Shift();
            string defaultShift = _shift.GetCurrShiftName();

            string[] columns = new string[] { "CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Shift");
            //获取班别代码
            DataTable shiftTable = BaseData.Get(columns, category);
            //获取班别代码成功。
            if (null != shiftTable && shiftTable.Rows.Count > 0)
            {
                this.lueShift.Properties.DataSource = shiftTable;
                this.lueShift.Properties.DisplayMember = "CODE";
                this.lueShift.Properties.ValueMember = "CODE";
                this.lueShift.EditValue = defaultShift;
            }
        }
        /// <summary>
        /// 重置按钮事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbReset_Click(object sender, EventArgs e)
        {
            this.beLotNumber.Text = string.Empty;
        }
        /// <summary>
        /// 关闭按钮事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
        }
        /// <summary>
        /// 批号回车时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beLotNumber_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                Operate();
            }
        }
        /// <summary>
        /// 确认按钮事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOK_Click(object sender, EventArgs e)
        {
            try
            {
                Operate();
            }
            finally
            {
                this.beLotNumber.Select();
                this.beLotNumber.SelectAll();
            }
        }
        /// <summary>
        /// 进行批次操作。
        /// </summary>
        private void Operate()
        {
            string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            string roomName = this.lueFactoryRoom.Text.Trim();
            string lotNumber = this.beLotNumber.Text.Trim();
            //车间没有选择，给出提示。
            if (string.IsNullOrEmpty(roomKey))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg004}"), MESSAGEBOX_CAPTION);//车间名称不能为空
                //MessageService.ShowMessage("车间名称不能为空","提示");
                this.lueFactoryRoom.Select();
                return;
            }
            //批号没有输入，给出提示。
            if (string.IsNullOrEmpty(lotNumber))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg008}"), MESSAGEBOX_CAPTION);//序列号不能为空
                //MessageService.ShowMessage("序列号不能为空","提示");
                this.beLotNumber.SelectAll();
                return;
            }

            DataSet dsLot = this._entity.GetLotInfo(lotNumber);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg, "提示");
                return;
            }
            if (dsLot == null || dsLot.Tables.Count <= 0 || dsLot.Tables[0].Rows.Count <= 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg010}"), MESSAGEBOX_CAPTION);//序列号不存在
                //MessageService.ShowMessage("序列号不存在。", "提示");
                this.beLotNumber.SelectAll();
                return;
            }
            DataRow drLotInfo = dsLot.Tables[0].Rows[0];
            //判断批次号在指定车间中是否存在。
            string currentRoomKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
            if (roomKey != currentRoomKey)
            {
                MessageService.ShowMessage(string.Format("【{0}】在当前车间中不存在，请确认。",lotNumber),"提示");
                this.beLotNumber.SelectAll();
                return;
            }
            //判断批次是否被暂停
            int holdFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_HOLD_FLAG]);
            if (holdFlag == 1 
                && this._operationType!=LotOperationType.BatchRelease
                && this._operationType!=LotOperationType.Release
                && this._operationType != LotOperationType.Rework
                && this._operationType != LotOperationType.BatchRework)
            {
                MessageService.ShowMessage(string.Format("【{0}】已被暂停，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return;
            }
            if (holdFlag == 0 && (this._operationType == LotOperationType.BatchRelease 
                                   || this._operationType == LotOperationType.Release
                                   || this._operationType == LotOperationType.Rework
                                   || this._operationType == LotOperationType.BatchRework))
            {
                MessageService.ShowMessage(string.Format("【{0}】未暂停，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return;
            }
            //判断批次是否被暂停
            string lotType = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_TYPE]);
            if (lotType == "N" && (this._operationType == LotOperationType.CellPatch 
                                   || this._operationType == LotOperationType.CellRecovered))
            {
                MessageService.ShowMessage(string.Format("批次必须是组件补片批次，请确认。"), "提示");
                this.beLotNumber.SelectAll();
                return;
            }
            //判断批次是否被删除
            int deleteFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]);
            if (deleteFlag == 1)
            {
                MessageService.ShowMessage(string.Format("【{0}】已结束，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return;
            }
            //判断批次是否已结束
            if (deleteFlag == 2)
            {
                MessageService.ShowMessage(string.Format("【{0}】已删除，请确认。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return;
            }
            //判断批次是否完成。
            int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
            if (stateFlag >= 10)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.Msg001}"));//批次已完成，请确认
                //MessageBox.Show("批次已完成，请确认。");
                this.beLotNumber.SelectAll();
                return;
            }
             string palletNo = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_PALLET_NO]);
            //如果是返修、不良、报废、批次退料、调整批次、拆分批次、合并批次或者终结批次操作。
            //批次不能被包装才能进行对应操作。
            if ((this._operationType == LotOperationType.Scrap
                || this._operationType == LotOperationType.Defect
                || this._operationType == LotOperationType.BatchRework
                || this._operationType == LotOperationType.ReturnMaterial
                || this._operationType == LotOperationType.Rework
                || this._operationType == LotOperationType.Adjust
                || this._operationType == LotOperationType.BatchAdjust
                || this._operationType == LotOperationType.Terminal
                || this._operationType == LotOperationType.Split
                || this._operationType == LotOperationType.Merge)
                && !string.IsNullOrEmpty(palletNo))
            {
                MessageService.ShowMessage(string.Format("【{0}】已包装，出托后才能进行对应操作。", lotNumber), "提示");
                this.beLotNumber.SelectAll();
                return;
            }

            LotOperationDetailModel model = new LotOperationDetailModel();
            model.OperationType=this._operationType;
            model.LotNumber = lotNumber;
            model.RoomKey = roomKey;
            model.RoomName = roomName;
            model.ShiftName = Convert.ToString(this.lueShift.EditValue);
            model.UserName = this.teUserNumber.Text;
            model.LotEditTime = Convert.ToString(dsLot.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_EDIT_TIME]);
            //model.TitleName = this.lblApplicationTitle.Text;

            //显示结束批次对话框。
            if (this._operationType == LotOperationType.Terminal)
            {
                TerminalLotDialog terminalLot = new TerminalLotDialog(model);
                //显示结束批次的对话框。
                terminalLot.ShowDialog();
            }
            else
            {
                //显示电池片操作明细界面。
                WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
                //创建新的视图并显示
                LotOperationDetailViewContent view = new LotOperationDetailViewContent(model);
                view.Control.Size = new Size(1000,500);
                WorkbenchSingleton.Workbench.ShowView(view);
            }
        }
        /// <summary>
        /// 显示批次选择对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beLotNumber_ButtonClick(object sender,ButtonPressedEventArgs e)
        {
            LotQueryHelpModel model = new LotQueryHelpModel();
            model.RoomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            model.OperationType = this._operationType;
            LotQueryHelpDialog dlg = new LotQueryHelpDialog(model);
            dlg.OnValueSelected += new LotQueryValueSelectedEventHandler(LotQueryHelpDialog_OnValueSelected);
            Point i = beLotNumber.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            dlg.Width = beLotNumber.Width;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + beLotNumber.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X, i.Y - dlg.Height);
                }
            }
            else
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X + beLotNumber.Width - dlg.Width, i.Y + beLotNumber.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + beLotNumber.Width - dlg.Width, i.Y - dlg.Height);
                }
            }
            dlg.Visible = true;
            dlg.Show();
        }
        /// <summary>
        /// 选中批次值后的事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void LotQueryHelpDialog_OnValueSelected(object sender, LotQueryValueSelectedEventArgs args)
        {
            this.beLotNumber.Text = args.LotNumber;
        }
    }
}
