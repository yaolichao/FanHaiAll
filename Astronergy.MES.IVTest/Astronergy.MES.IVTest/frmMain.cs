using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Threading;
using System.IO;
using Microsoft.Win32;
using System.Configuration;
using System.Diagnostics;
using Astronergy.MES.IVTest.BLL;
using System.ServiceProcess;
using Astronergy.MES.IVTest.Configuration;

namespace Astronergy.MES.IVTest
{
    public partial class frmMain : Form
    {
        private string ACCESS_CONNECTION_STRING = System.Configuration.ConfigurationManager.ConnectionStrings["ACCESS_STRING"].ConnectionString;
        private string SQLSERVER_STRING = System.Configuration.ConfigurationManager.ConnectionStrings["SQLSERVER_STRING"].ConnectionString;
        private string SERVICE_NAME = System.Configuration.ConfigurationManager.AppSettings["SERVICE_NAME"];
        private ServiceController _controller=null;
        IVTestConfigurationSection _section = null;
        private delegate void SetButtonEnableCallBack(Button sender, bool enable);
        private delegate void SetLableTextCallBack(Label sender, string val);
        /// <summary>
        /// 多线程下设置按钮的启用禁用。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="enable"></param>
        private void SetButtonEnable(Button obj, bool enable)
        {
            if (obj.InvokeRequired)
            {
                SetButtonEnableCallBack d = new SetButtonEnableCallBack(SetButtonEnable);
                obj.Invoke(d, obj, enable);
            }
            else
            {
                obj.Enabled = enable;
            }
        }
        /// <summary>
        /// 多线程下设置Label控件文本。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="val"></param>
        private void SetLableText(Label obj, string val)
        {
            if (obj.InvokeRequired)
            {
                SetLableTextCallBack d = new SetLableTextCallBack(SetLableText);
                obj.Invoke(d, obj, val);
            }
            else
            {
                obj.Text = val;
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            //获取配置节信息
            this._section = (IVTestConfigurationSection)ConfigurationManager.GetSection("ivtest");
            if (this._section != null)
            {
                //绑定设备
                foreach (DeviceElement device in this._section.Devices)
                {
                    this.cmbDevice.Items.Add(device.Name);
                }
            }
            InitServiceStatus();
        }
        /// <summary>
        /// 初始化服务状态。
        /// </summary>
        private void InitServiceStatus()
        {
            this._controller = new ServiceController(SERVICE_NAME);
            try
            {
                this.btnInstallService.Visible = false;
                this.btnUninstallService.Visible = true;
                this.btnStopService.Visible = true;
                this.btnResetService.Visible = true;
                if (this._controller.Status == ServiceControllerStatus.Running)
                {
                    this.btnStopService.Text = "停止服务";
                }
                else
                {
                    this.btnStopService.Text = "启动服务";
                }
            }
            catch
            {
                this.btnInstallService.Visible = true;
                this.btnUninstallService.Visible = false;
                this.btnStopService.Visible = false;
                this.btnResetService.Visible = false;
            }
            finally
            {
                this._controller.Close();
            }
        }

        /// <summary>
        /// 转数据。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDataTransfer_Click(object sender, EventArgs e)
        {
            string device = this.cmbDevice.Text;
            if (string.IsNullOrEmpty(device))
            {
                MessageBox.Show("必须选择设备。", "提示");
                return;
            }
            DeviceElement element = this._section.Devices[device];
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(TransferData);
            Thread th = new Thread(threadStart);
            th.Start(element);
        }
        /// <summary>
        /// 数据转置
        /// </summary>
        private void TransferData(object obj)
        {
            DeviceElement device = obj as DeviceElement;
            if (device == null)
            {
                return;
            }
            TransferData(device);
        }
        /// <summary>
        /// 数据转置
        /// </summary>
        private void TransferData(DeviceElement device)
        {
            try
            {
                string strFillFullName = txtFullName.Text;
                string lotNumber = this.txtLotNumber.Text;
                if (string.IsNullOrEmpty(strFillFullName))
                {
                    strFillFullName = DataTransfer.GetFullFile(device.Path, device.Format);
                }
                if (string.IsNullOrEmpty(strFillFullName))
                {
                    MessageBox.Show("请选择需要读取的Access文件！");
                    return;
                }
                DateTime dtStart = DateTime.Now;
                SetLableText(this.lblMsg, "正在运行，请稍等...");
                SetButtonEnable(btnDataTransfer, false);
                string accConString = string.Format(ACCESS_CONNECTION_STRING, strFillFullName);
                string sqlConString = SQLSERVER_STRING;
                DataTransfer sdgData = new DataTransfer(accConString, sqlConString);
                bool bAllSuccess = sdgData.AccessToSqlServer(device.Name,device.Type, lotNumber);
                if (bAllSuccess == false)
                {
                    MessageBox.Show("转置数据时出现错误，详细请查看Windows事件日志。");
                }
                DateTime dtEnd = DateTime.Now;
                string msg = string.Format("开始时间:{0}。\n结束时间:{1}。\n耗用时间:{2}秒。\n转置数据数量:{3}。",
                                            dtStart.ToString("yyyy-MM-dd HH:mm:ss"),
                                            dtEnd.ToString("yyyy-MM-dd HH:mm:ss"),
                                            (dtEnd - dtStart).TotalSeconds,
                                            sdgData.TransferCount);
                SetButtonEnable(btnDataTransfer, true);
                SetLableText(this.lblMsg, msg);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Astronergy.MES.IVTest", ex.Message, EventLogEntryType.Error);
                MessageBox.Show(ex.Message);
            }
        }
        
        /// <summary>
        /// 分选数据Access文件选取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileD = new OpenFileDialog();
            openFileD.InitialDirectory = "E:";
            openFileD.Filter = "Access文件|*.mdb|所有文件|*.*";
            openFileD.RestoreDirectory = true;
            openFileD.FilterIndex = 1;
            if (openFileD.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileD.FileName;
                txtFullName.Text = fName.ToString();
            }
        }
        /// <summary>
        /// 显示窗体。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Show();
        }
        private int WM_SYSCOMMAND = 0x112;
        private long SC_MINIMIZE = 0xF020;
        /// <summary>
        /// 响应自定义消息事件。
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                if (m.WParam.ToInt64() == SC_MINIMIZE)
                {
                    this.Hide();
                    this.ShowInTaskbar = false;
                    return;
                }
            }
            base.WndProc(ref m);
        }
        /// <summary>
        /// 安装服务。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInstallService_Click(object sender, EventArgs e)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "Install";
                p.Start();
                p.WaitForExit();
            }
            InitServiceStatus();
        }
        /// <summary>
        /// 启动/停止服务。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopService_Click(object sender, EventArgs e)
        {
            this._controller = new ServiceController(SERVICE_NAME);
            try
            {
                if (this._controller.CanStop)
                {
                    this._controller.Stop();
                    this._controller.WaitForStatus(ServiceControllerStatus.Stopped);
                    MessageBox.Show("服务已停止。");
                }
                else if (this._controller.Status == ServiceControllerStatus.Stopped)
                {
                    this._controller.Start();
                    this._controller.WaitForStatus(ServiceControllerStatus.Running);
                    MessageBox.Show("服务已启动。");
                }
                else
                {
                    MessageBox.Show(string.Format("服务目前{0}，操作失败。", this._controller.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this._controller.Close();
            }
            InitServiceStatus();
        }
        /// <summary>
        /// 暂停/继续服务。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPause_Click(object sender, EventArgs e)
        {
            this._controller = new ServiceController(SERVICE_NAME);
            try
            {
                if (this._controller.CanPauseAndContinue && this._controller.Status == ServiceControllerStatus.Running)
                {
                    this._controller.Pause();
                    this._controller.WaitForStatus(ServiceControllerStatus.Paused);
                    MessageBox.Show("服务已暂停。");
                }
                else if (this._controller.CanPauseAndContinue && this._controller.Status == ServiceControllerStatus.Paused)
                {
                    this._controller.Continue();
                    this._controller.WaitForStatus(ServiceControllerStatus.Running);
                    MessageBox.Show("服务已恢复。");
                }
                else
                {
                    MessageBox.Show(string.Format("服务目前{0}，操作失败。", this._controller.Status.ToString()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this._controller.Close();
            }
            InitServiceStatus();
        }
        /// <summary>
        /// 重启服务。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetService_Click(object sender, EventArgs e)
        {
            this._controller = new ServiceController(SERVICE_NAME);
            try
            {
                if (this._controller.CanStop)
                {
                    this._controller.Stop();
                    this._controller.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                if (this._controller.Status == ServiceControllerStatus.Stopped)
                {
                    this._controller.Start();
                    this._controller.WaitForStatus(ServiceControllerStatus.Running);
                }
                MessageBox.Show("服务已重启。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this._controller.Close();
            }
            InitServiceStatus();
        }
        /// <summary>
        /// 卸载服务。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUninstallService_Click(object sender, EventArgs e)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "Uninstall.bat";
                p.Start();
                p.WaitForExit();
            }
            InitServiceStatus();
        }
    }

}


