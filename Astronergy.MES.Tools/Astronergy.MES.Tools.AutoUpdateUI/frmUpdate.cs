using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Net;

namespace Astronergy.MES.Tools.AutoUpdateUI
{

    /// <summary>
    /// 自动更新的窗体。
    /// </summary>
    public partial class frmUpdate : Form
    {
        /// <summary>
        /// 线程等待事件。
        /// </summary>
        AutoResetEvent[] autoEvents = new AutoResetEvent[]
        {
            new AutoResetEvent(false),
            new AutoResetEvent(false),
            new AutoResetEvent(false),
            new AutoResetEvent(false)
        };

        readonly object obj = new object();
        /// <summary>
        /// 取消更新。
        /// </summary>
        bool bCancleUpdate = false;        
        /// <summary>
        /// 更新完成。
        /// </summary>
        bool bUpdateFinish = false;
        Process processAutoUpdate = null;
        AutoResetEvent autoEventAutoUpdate = new AutoResetEvent(false);
        Process processApplication = null;
        AutoResetEvent autoEventApplication = new AutoResetEvent(false);
        /// <summary>
        /// 追加文本到文本框信息，用于多线程。
        /// </summary>
        /// <param name="text"></param>
        public delegate void AppendTextCallback(string text);
        /// <summary>
        /// 关闭窗体，用于多线程。
        /// </summary>
        public delegate void CloseFormCallback();
        /// <summary>
        /// 设置控件文本，用于多线程。
        /// </summary>
        /// <param name="text"></param>
        public delegate void SetControlTextCallBack(string text);

        string _baseUrl = string.Empty;
        IList<string> _lstUrl = new List<string>();
        string _xmlName = string.Empty;
        string _applicationName = string.Empty;
        string _workDirectory = string.Empty;
        int _timeout = 100000;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public frmUpdate(string baseUrl,string applicationName)
        {
            InitializeComponent();
            _applicationName=System.Configuration.ConfigurationSettings.AppSettings["ApplicationName"];
            string timeout = System.Configuration.ConfigurationSettings.AppSettings["Timeout"];
            string url=System.Configuration.ConfigurationSettings.AppSettings["BaseUrl"];
            _workDirectory = Directory.GetCurrentDirectory();
            if (!string.IsNullOrEmpty(baseUrl))
            {
                url = baseUrl;
            }
            if (!string.IsNullOrEmpty(applicationName))
            {
                _applicationName = applicationName;
            }

            if (string.IsNullOrEmpty(timeout) || !int.TryParse(timeout, out _timeout))
            {
                _timeout = 100000;
            }

            _lstUrl = url.Split('|').ToList<string>();
        }
        /// <summary>
        /// 启动自动更新。
        /// </summary>
        public void StartAutoUpdate()
        {
            bool bUpdateSuccess = true;
            try
            {
                Thread.Sleep(100);
                string autoupdateName="autoupdate.exe";
                string autoupdateNameConfig = "autoupdate.exe.config";
                if (!bCancleUpdate && bUpdateSuccess)
                {
                    AppendText(string.Format("正在更新{0}...\r\n", autoupdateNameConfig));
                    bUpdateSuccess = DownloadFile(autoupdateNameConfig);
                }
                autoEvents[0].Set();
                if (!bCancleUpdate && bUpdateSuccess)
                {
                    AppendText(string.Format("正在更新{0}...\r\n", autoupdateName));
                    bUpdateSuccess = DownloadFile(autoupdateName);
                }
                autoEvents[1].Set();
                int exitCode = 0;
                if (!bCancleUpdate && bUpdateSuccess)
                {
                    using (processAutoUpdate = new System.Diagnostics.Process())
                    {
                        AppendText(string.Format("准备更新{0}...\r\n", _applicationName));
                        string args = string.Format("--baseurl {0}", _baseUrl);
                        processAutoUpdate.StartInfo.FileName = "autoupdate.exe";
                        processAutoUpdate.StartInfo.Arguments = args;
                        // 必须禁用操作系统外壳程序  
                        processAutoUpdate.StartInfo.UseShellExecute = false;
                        processAutoUpdate.StartInfo.CreateNoWindow = true;
                        processAutoUpdate.StartInfo.RedirectStandardOutput = true;
                        processAutoUpdate.Start();
                        autoEventAutoUpdate.Set();
                        // 异步获取命令行内容  
                        processAutoUpdate.BeginOutputReadLine();
                        // 为异步获取订阅事件  
                        processAutoUpdate.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
                        processAutoUpdate.WaitForExit();
                        exitCode = processAutoUpdate.ExitCode;
                        processAutoUpdate.Close();
                        processAutoUpdate = null;
                    }
                }
                autoEvents[2].Set();
                if (bCancleUpdate)
                {
                    AppendText("自动更新取消。\r\n");
                }
                if (!bCancleUpdate && bUpdateSuccess)
                {
                    if (exitCode != 0)
                    {
                        AppendText("自动更新失败，请联系系统管理员。\r\n");
                        bUpdateSuccess = false;
                        //this.CloseForm();
                    }
                    else //启动CHINT MES程序。
                    {
                        AppendText(string.Format("正在启动{0}...\r\n", _applicationName));
                        using (processApplication = new System.Diagnostics.Process())
                        {
                            processApplication.StartInfo.FileName = _applicationName;
                            processApplication.StartInfo.Arguments = " /noupdate";
                            if (processApplication.Start())
                            {
                                this.CloseForm();
                            }
                            else
                            {
                                AppendText("启动应用程序失败，请联系系统管理员。\r\n");
                                bUpdateSuccess = false;
                            }
                            autoEventApplication.Set();
                        }
                    }
                }
                autoEvents[3].Set();
            }
            catch (Exception ex)
            {
                AppendText("自动更新失败，请联系系统管理员。" + ex.Message+"\r\n");
                AppendText("请检查网络是否已连接。\r\n");
                bUpdateSuccess = false;
            }
            if (bUpdateSuccess)
            {
                SetControlText(string.Format("自动更新完成({0})。", bCancleUpdate ? "用户取消" : "成功"));
            }
            else
            {
                SetControlText(string.Format("自动更新完成({0})。", bCancleUpdate ? "用户取消" : "失败"));
            }
            bUpdateFinish = true;
        }
        /// <summary>
        /// 接收自动更新消息，并显示。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Data) == false)
            {
                this.AppendText(e.Data + "\r\n");
            }
        }
        /// <summary>
        /// 关闭窗体。
        /// </summary>
        public void CloseForm()
        {
            if (this.InvokeRequired)
            {
                CloseFormCallback d = new CloseFormCallback(Close);
                this.Invoke(d);
            }
            else
            {
                this.Close();
            }
        }
        /// <summary>
        /// 显示更新信息。
        /// </summary>
        /// <param name="text"></param>
        public void AppendText(string text)
        {
            if (this.txtHint.InvokeRequired)
            {
                AppendTextCallback d = new AppendTextCallback(AppendText);
                this.txtHint.Invoke(d, text);
            }
            else
            {
                this.txtHint.AppendText(text);
            }
        }
        /// <summary>
        /// 显示更新信息。
        /// </summary>
        /// <param name="text"></param>
        public void SetControlText(string text)
        {
            if (this.lblHint.InvokeRequired)
            {
                SetControlTextCallBack d = new SetControlTextCallBack(SetControlText);
                this.lblHint.Invoke(d,text);
            }
            else
            {
                this.lblHint.Text = text;
            }
        }
        /// <summary>
        /// 取消更新。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            lock (obj)
            {
                //如果自动更新完成，直接关闭窗口。
                if (bUpdateFinish)
                {
                    this.CloseForm();
                }
                if (bCancleUpdate) return;
                SetControlText("正在取消更新，请等待......\r\n");
                Thread t = new Thread(new ThreadStart(CancleAutoUpdate));
                t.Start();
            }
        }
        /// <summary>
        /// 取消自动更新。
        /// </summary>
        public void CancleAutoUpdate()
        {
            bCancleUpdate = true;
            if (processAutoUpdate != null)
            {
                autoEventAutoUpdate.WaitOne(1000);
                try
                {
                    processAutoUpdate.Kill();
                }
                catch{}
            }
            WaitHandle.WaitAll(autoEvents);
            //this.CloseForm();
        }
        /// <summary>
        /// 下载更新文件。
        /// </summary>
        /// <param name="url">更新的文件地址。</param>
        /// <param name="downPath">文件存放路径。</param>
        /// <param name="fileName">文件名称。</param>
        public bool DownloadFile(string fileName)
        {
            try
            {
                string filePath = fileName.TrimStart('\\');
                string fileFullPath = Path.Combine(_workDirectory, filePath);

                string dir = Path.GetDirectoryName(fileFullPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                WebRequest req = null;
                WebResponse res =null;
                foreach (string url in _lstUrl)
                {
                    if (!string.IsNullOrEmpty(_baseUrl) && _baseUrl != url)
                    {
                        continue;
                    }
                    string tmpUrl = Path.Combine(url, filePath.Replace('\\', '/'));
                    try
                    {
                        req = WebRequest.Create(tmpUrl);
                        req.Timeout = _timeout;
                        //局域网内，不使用代理。
                        req.Proxy = new WebProxy();
                        if (bCancleUpdate) return false;
                        res = req.GetResponse();
                        _baseUrl = url;
                        break;
                    }
                    catch(Exception ex)//如果网络不通，换另外的地址继续。
                    {
                        AppendText(ex.Message + "\r\n");
                        AppendText("正在重新尝试连接...\r\n");
                        continue;
                    }
                }
                //网络地址全部不通。
                if (string.IsNullOrEmpty(_baseUrl))
                {
                    return false;
                }
                long fileLength = res.ContentLength;
                if (fileLength > 0)
                {
                    using (Stream srm = res.GetResponseStream())
                    {
                        StreamReader srmReader = new StreamReader(srm);
                        byte[] bufferbyte = new byte[fileLength];
                        int allByte = (int)bufferbyte.Length;
                        int startByte = 0;
                        while (fileLength > 0)
                        {
                            int downByte = srm.Read(bufferbyte, startByte, allByte);
                            if (downByte == 0) { break; };
                            startByte += downByte;
                            allByte -= downByte;
                        }
                        if (bCancleUpdate) return false;
                        using (FileStream fs = new FileStream(fileFullPath, FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(bufferbyte, 0, bufferbyte.Length);
                            fs.Close();
                        }
                        srmReader.Close();
                        srm.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                AppendText(ex.Message+"\r\n");
                return false;
            }
        }

        private void frmUpdate_Shown(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(StartAutoUpdate));
            t.Start();
        }
    }
}
