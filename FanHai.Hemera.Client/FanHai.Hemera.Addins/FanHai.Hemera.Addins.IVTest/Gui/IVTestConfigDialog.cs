using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Common;
using System.Collections;
using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Addins.IVTest
{
    /// <summary>
    /// IV测试配置对话框。
    /// </summary>
    public partial class IVTestConfigDialog : BaseDialog
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        public IVTestConfigDialog()
        {
            InitializeComponent();
            InitializeLanguage();
        }



        private void InitializeLanguage()
        {
            this.btnCancel.Text = StringParser.Parse("${res:Global.Cancel}");//"取消";
            this.btnOk.Text = StringParser.Parse("${res:Global.OKButtonText}");//"确定";
            this.lcgContent.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOperation.lcgContent}");//"内容";
            this.lciEquipment.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.Equipments.Name}");//"设备";
            this.lciOperation.Text = StringParser.Parse("${res:Global.Step}");//"工序";
            this.lciBtnOk.Text = StringParser.Parse("${res:Global.OKButtonText}");//"确定";
            this.lciBtnCancel.Text = StringParser.Parse("${res:Global.Cancel}");//"取消";
            this.lciLoginName.Text = StringParser.Parse("${res:FanHai.Gui.Framework.Gui.Dialogs.Login.lblUserName}");//"用户名";
            this.lciLoginPassword.Text = StringParser.Parse("${res:FanHai.Gui.Framework.Gui.Dialogs.Login.lblPassword}");//"密码";

            this.lcgDataBaseConfig.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.lcgDataBaseConfig}");//"数据库链接信息配置";
            this.lciDataBaseAddress.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.lciDataBaseAddress}");//"实例地址";
            this.lciPort.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.lciPort}");//"端口号";
            this.lciDatabaseName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.lciDatabaseName}");//"数据库名称";
            this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.Text}");//"IV测试配置";
            this.lciDataFilePath.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.lciDataFilePath}");//"测试文件路径";
            this.lciRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.lciRoom}");//"车间";
            this.lciDeviceType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.lciDeviceType}");//"设备数据类型";
            this.lcgButtons.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.lcgButtons}");//"按钮";

        }




        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IVTestConfigDialog_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            BindOperations();
            BindDeviceType();
            InitControlValue();
        }
        /// <summary>
        /// 绑定车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            if (dt != null)
            {
                this.lueRoom.Properties.DataSource = dt;
                this.lueRoom.Properties.DisplayMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;
                this.lueRoom.Properties.ValueMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY;
                this.lueRoom.ItemIndex = 0;
            }
            else
            {
                this.lueRoom.Properties.DataSource = null;
                this.lueRoom.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 绑定工序。
        /// </summary>
        private void BindOperations()
        {
            //获取登录用户拥有权限的工序名称。
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            if (operations.Length > 0)//如果有拥有权限的工序名称
            {
                string[] strOperations = operations.Split(',');
                //遍历工序，并将其添加到窗体控件中。
                for (int i = 0; i < strOperations.Length; i++)
                {
                    cbOperation.Properties.Items.Add(strOperations[i]);
                }
                this.cbOperation.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 绑定设备
        /// </summary>
        private void BindEquipment()
        {
            string strOperation = this.cbOperation.Text;
            string strFactoryRoomName = this.lueRoom.Text;
            string strFactoryRoomKey = Convert.ToString(this.lueRoom.EditValue);
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            //如果工厂车间或者工序或者线别主键为空。
            if (string.IsNullOrEmpty(strFactoryRoomName)
                || string.IsNullOrEmpty(strOperation)
                || string.IsNullOrEmpty(strLines))
            {
                return;
            }
            this.lueEquipment.EditValue = string.Empty;
            this.lueEquipment.Properties.ReadOnly = false;

            EquipmentEntity entity = new EquipmentEntity();
            DataSet ds = entity.GetEquipments(strFactoryRoomKey, strOperation, strLines.Split(','));
            if (string.IsNullOrEmpty(entity.ErrorMsg))//执行成功。
            {
                DataTable dt = ds.Tables[0];
                dt.DefaultView.RowFilter = "EQUIPMENT_CODE LIKE '%MT%' OR EQUIPMENT_CODE LIKE '%TCT%' OR EQUIPMENT_CODE LIKE '%AIV%'";
                this.lueEquipment.Properties.DataSource = dt;
                this.lueEquipment.Properties.DisplayMember = "EQUIPMENT_CODE";
                this.lueEquipment.Properties.ValueMember = "EQUIPMENT_CODE";
                this.lueEquipment.ItemIndex = 0;
            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg);
            }
        }
        /// <summary>
        /// 绑定IV测试设备类型。
        /// </summary>
        private void BindDeviceType()
        {
            DataTable dtReturn = CommonUtils.ConvertEnumTypeToDataTable(typeof(IVTestDeviceType));
            this.lueDeviceType.Properties.DataSource = dtReturn;
            this.lueDeviceType.Properties.DisplayMember = COMMON_FIELDS.FIELD_COMMON_DESCRIPTION;
            this.lueDeviceType.Properties.ValueMember = COMMON_FIELDS.FIELD_COMMON_VALUE;
            this.lueDeviceType.ItemIndex = 0;
        }
        /// <summary>
        /// 初始化控件值。
        /// </summary>
        private void InitControlValue()
        {
            //初始化车间值。
            string roomKey = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_ROOM_KEY);
            if (!string.IsNullOrEmpty(roomKey))
            {
                this.lueRoom.EditValue = roomKey;
            }
            //初始化工序值。
            string operationName = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_OPERATION_NAME);
            if (!string.IsNullOrEmpty(operationName))
            {
                this.cbOperation.EditValue = operationName;
            }
            //初始化设备代码值。
            string equipmentCode = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_DEVICE);
            if (!string.IsNullOrEmpty(equipmentCode))
            {
                this.lueEquipment.EditValue = equipmentCode;
            }
            //初始化设备类型值。
            string deviceType = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_TYPE);
            if (!string.IsNullOrEmpty(deviceType))
            {
                decimal nDeviceType = 0;
                decimal.TryParse(deviceType, out nDeviceType);
                this.lueDeviceType.EditValue = nDeviceType;
            }
            //初始化数据文件路径。
            string filePath = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_PATH);
            this.beDataFilePath.Text = filePath;

            //初始化数据库配置信息；
            string databaseAddress = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_DB_ADDRESS).Trim();
            this.teDataBaseAddress.Text = databaseAddress;
            string databaseName = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_DB_NAME).Trim();
            this.teDatabaseName.Text = databaseName;
            string databasePort = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_DB_PORT).Trim();
            this.tePort.Text = databasePort;
            string databaseLoginName = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_DB_LGNAME).Trim();
            this.teLoginName.Text = databaseLoginName;
            string databaseLoginPassword = PropertyService.Get(PROPERTY_FIELDS.IVTEST_DATA_DB_LGPW).Trim();
            this.teLoginPassword.Text = databaseLoginPassword;
        }
        /// <summary>
        /// 不保存配置数据，关闭对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:Global.SystemInfo}"); //提示
        /// <summary>
        /// 保存配置数据，关闭对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            string roomKey = Convert.ToString(this.lueRoom.EditValue);
            string operationName = this.cbOperation.Text;
            string deviceNo = Convert.ToString(this.lueEquipment.EditValue);
            string deviceType = Convert.ToString(this.lueDeviceType.EditValue);
            string path = Convert.ToString(this.beDataFilePath.Text);

            //设定数据库连接对应的参数变量
            string strDatabaseAdress = teDataBaseAddress.Text.Trim();
            string strDatabaseName = teDatabaseName.Text.Trim();
            string strDatabasePort = tePort.Text.Trim();
            string strDatabaseLoginName = teLoginName.Text.Trim();
            string strDatabaseLoginPassword = teLoginPassword.Text.Trim();

            if (string.IsNullOrEmpty(roomKey))
            {              
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.Msg001}"), MESSAGEBOX_CAPTION);//车间不能为空，请确认是否有权限设置IV配置数据。
                //MessageService.ShowMessage("车间不能为空，请确认是否有权限设置IV配置数据。", "提示");
                return;
            }
            if (string.IsNullOrEmpty(operationName))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.Msg002}"), MESSAGEBOX_CAPTION);//工序不能为空，请确认是否有权限设置IV配置数据。
                //MessageService.ShowMessage("工序不能为空，请确认是否有权限设置IV配置数据。", "提示");
                return;
            }
            if (string.IsNullOrEmpty(deviceNo))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.Msg003}"), MESSAGEBOX_CAPTION);//设备不能为空，请确认是否有权限设置IV配置数据。
                //MessageService.ShowMessage("设备不能为空，请确认是否有权限设置IV配置数据。", "提示");
                return;
            }
            //针对设备类型进行数据信息配置的检查
            switch (deviceType)
            {
                case "1":
                case "2":
                case "3":
                    //数据文件地址不能为空
                    if (!string.IsNullOrEmpty(path) && !System.IO.File.Exists(path))
                    {
                        MessageService.ShowMessage(string.Format("文件[{0}]不存在，请确认。", path), "提示");
                        return;
                    }
                    break;
                case "4":
                    //判断数据库实例地址
                    if (string.IsNullOrEmpty(strDatabaseAdress))
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.Msg004}"), MESSAGEBOX_CAPTION);//数据库地址不能为空，请确认。
                        //MessageService.ShowMessage("数据库地址不能为空，请确认。", "提示");
                        return;
                    }
                    //判断数据库名称
                    if (string.IsNullOrEmpty(strDatabaseName))
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.Msg005}"), MESSAGEBOX_CAPTION);//数据库名称不能为空，请确认。
                        //MessageService.ShowMessage("数据库名称不能为空，请确认。", "提示");
                        return;
                    }
                    //判断数据库端口
                    if (string.IsNullOrEmpty(strDatabasePort))
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.Msg006}"), MESSAGEBOX_CAPTION);//数据库端口不能为空，请确认。
                        //MessageService.ShowMessage("数据库端口不能为空，请确认。", "提示");
                        return;
                    }
                    //判断登录名称
                    if (string.IsNullOrEmpty(strDatabaseLoginName))
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.Msg007}"), MESSAGEBOX_CAPTION);//数据库登录名不能为空，请确认。
                        //MessageService.ShowMessage("数据库登录名不能为空，请确认。", "提示");
                        return;
                    }
                    //登录密码
                    if (string.IsNullOrEmpty(strDatabaseLoginPassword))
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.IVTestConfigDialog.Msg008}"), MESSAGEBOX_CAPTION);//数据库登录密码不能为空，请确认。
                        //MessageService.ShowMessage("数据库登录密码不能为空，请确认。", "提示");
                        return;
                    }
                    break;
                default:
                    break;
            }

            PropertyService.Set(PROPERTY_FIELDS.IVTEST_DATA_ROOM_KEY, roomKey);
            PropertyService.Set(PROPERTY_FIELDS.IVTEST_DATA_OPERATION_NAME, operationName);
            PropertyService.Set(PROPERTY_FIELDS.IVTEST_DATA_DEVICE, deviceNo);
            PropertyService.Set(PROPERTY_FIELDS.IVTEST_DATA_PATH, path);
            PropertyService.Set(PROPERTY_FIELDS.IVTEST_DATA_TYPE, deviceType);

            //存储数据库配置信息到本地配置文件
            PropertyService.Set(PROPERTY_FIELDS.IVTEST_DATA_DB_ADDRESS, strDatabaseAdress);
            PropertyService.Set(PROPERTY_FIELDS.IVTEST_DATA_DB_NAME, strDatabaseName);
            PropertyService.Set(PROPERTY_FIELDS.IVTEST_DATA_DB_PORT, strDatabasePort);
            PropertyService.Set(PROPERTY_FIELDS.IVTEST_DATA_DB_LGNAME, strDatabaseLoginName);
            PropertyService.Set(PROPERTY_FIELDS.IVTEST_DATA_DB_LGPW, strDatabaseLoginPassword);

            PropertyService.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// 显示选择数据文件路径的对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beDataFilePath_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Access文件(*.mdb)|*.mdb";
            dlg.FilterIndex = 1;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.beDataFilePath.Text = dlg.FileName;
            }
        }
        /// <summary>
        /// 车间值改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueRoom_EditValueChanged(object sender, EventArgs e)
        {
            BindEquipment();
        }
        /// <summary>
        /// 工序值改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbOperation_EditValueChanged(object sender, EventArgs e)
        {
            BindEquipment();
        }
    }
}
