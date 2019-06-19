using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using System.Threading;
using FanHai.Hemera.Share.Constants;
using System.IO;
using System.Data.OleDb;
using System.Data;
using FanHai.Hemera.Utils.Entities;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.IVTest
{
    /// <summary>
    /// 上传EL图片的命令类。
    /// </summary>
    public class UploadELPictureCommand : UploadFileCommand
    {
        private static UploadELPictureCommand _commandObject = null;

        public static UploadELPictureCommand CommandObject
        {
            get
            {
                return _commandObject;
            }
        }

        public UploadELPictureCommand()
        {
            if (_commandObject == null)
            {
                _commandObject = this;
            }
        }

        protected override FileUploadConfigElement GetFileUploadConfigElement()
        {
            string srcRootPath = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_SOURCE_ROOT_PATH).Trim();
            string srcPathFormat = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_SOURCE_PATH_FORMAT).Trim();
            string destRootPath = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_DEST_ROOT_PATH).Trim();
            string destPathFormat = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_DEST_PATH_FORMAT).Trim();
            string fileExistension = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_EXISTENSION).Trim();
            string userName = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_USER_NAME).Trim();
            string userPassword = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_USER_PASSWORD).Trim();
            string isDeleteLocalFile = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_IS_DELETE_LOCAL_FILE);
            string isSemimanufactures = PropertyService.Get(PROPERTY_FIELDS.EL_PICTURE_IS_SEMIMANUFACTURES);
            FileUploadConfigElement cfg = new FileUploadConfigElement(srcRootPath, destRootPath, fileExistension);
            cfg.UserName = userName;
            cfg.UserPassword = userPassword;

            if (!string.IsNullOrEmpty(srcPathFormat))
            {
                cfg.SourcePathFormat = Path.Combine(srcRootPath, srcPathFormat);
            }
            if (!string.IsNullOrEmpty(destPathFormat))
            {
                cfg.DestPathFormat = Path.Combine(destRootPath, destPathFormat);
            }
            if (!string.IsNullOrEmpty(isDeleteLocalFile))
            {
                cfg.IsDeleteLocalFile = Convert.ToBoolean(isDeleteLocalFile);
            }
            if (!string.IsNullOrEmpty(isSemimanufactures))
            {
                cfg.IsSemimanufactures = Convert.ToBoolean(isSemimanufactures);
            }
            return cfg;
        }
    }

    /// <summary>
    /// 上传IV图片的命令类。
    /// </summary>
    public class UploadIVPictureCommand : UploadFileCommand
    {
        private static UploadIVPictureCommand _commandObject = null;

        public static UploadIVPictureCommand CommandObject
        {
            get
            {
                return _commandObject;
            }
        }

        public UploadIVPictureCommand()
        {
            if (_commandObject == null)
            {
                _commandObject = this;
            }
        }

        protected override FileUploadConfigElement GetFileUploadConfigElement()
        {
            string srcRootPath = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_SOURCE_ROOT_PATH).Trim();
            string srcPathFormat = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_SOURCE_PATH_FORMAT).Trim();
            string destRootPath = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_DEST_ROOT_PATH).Trim();
            string destPathFormat = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_DEST_PATH_FORMAT).Trim();
            string fileExistension = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_EXISTENSION).Trim();
            string userName = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_USER_NAME).Trim();
            string userPassword = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_USER_PASSWORD).Trim();
            string isDeleteLocalFile = PropertyService.Get(PROPERTY_FIELDS.IV_PICTURE_IS_DELETE_LOCAL_FILE);
            FileUploadConfigElement cfg = new FileUploadConfigElement(srcRootPath, destRootPath, fileExistension);
            cfg.UserName = userName;
            cfg.UserPassword = userPassword;
            if (!string.IsNullOrEmpty(srcPathFormat))
            {
                cfg.SourcePathFormat = Path.Combine(srcRootPath, srcPathFormat);
            }
            if (!string.IsNullOrEmpty(destPathFormat))
            {
                cfg.DestPathFormat = Path.Combine(destRootPath, destPathFormat);
            }
            if (!string.IsNullOrEmpty(isDeleteLocalFile))
            {
                cfg.IsDeleteLocalFile = Convert.ToBoolean(isDeleteLocalFile);
            }
            return cfg;
        }
    }
    /// <summary>
    /// 上传ELNG图片的命令类。
    /// </summary>
    public class UploadELNGPictureCommand : UploadFileCommand
    {
        private static UploadELNGPictureCommand _commandObject = null;

        public static UploadELNGPictureCommand CommandObject
        {
            get
            {
                return _commandObject;
            }
        }

        public UploadELNGPictureCommand()
        {
            if (_commandObject == null)
            {
                _commandObject = this;
            }
        }

        protected override FileUploadConfigElement GetFileUploadConfigElement()
        {
            string srcRootPath = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_SOURCE_ROOT_PATH).Trim();
            string srcPathFormat = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_SOURCE_PATH_FORMAT).Trim();
            string destRootPath = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_DEST_ROOT_PATH).Trim();
            string destPathFormat = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_DEST_PATH_FORMAT).Trim();
            string fileExistension = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_EXISTENSION).Trim();
            string userName = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_USER_NAME).Trim();
            string userPassword = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_USER_PASSWORD).Trim();
            string isDeleteLocalFile = PropertyService.Get(PROPERTY_FIELDS.ELNG_PICTURE_IS_DELETE_LOCAL_FILE);
            FileUploadConfigElement cfg = new FileUploadConfigElement(srcRootPath, destRootPath, fileExistension);
            cfg.UserName = userName;
            cfg.UserPassword = userPassword;

            if (!string.IsNullOrEmpty(srcPathFormat))
            {
                cfg.SourcePathFormat = Path.Combine(srcRootPath, srcPathFormat);
            }
            if (!string.IsNullOrEmpty(destPathFormat))
            {
                cfg.DestPathFormat = Path.Combine(destRootPath, destPathFormat);
            }
            if (!string.IsNullOrEmpty(isDeleteLocalFile))
            {
                cfg.IsDeleteLocalFile = Convert.ToBoolean(isDeleteLocalFile);
            }
            return cfg;
        }
    }

    /// <summary>
    /// 启动文件上传线程的命令类。
    /// </summary>
    public abstract class UploadFileCommand : AbstractMenuCommand
    {
        private readonly object objLock = new object();
        private FileUploadThreadWrapper _wrapper = null;

        /// <summary>
        /// 获取IV测试数据转置的线程对象。
        /// </summary>
        private FileUploadThreadWrapper ThreadWrapper
        {
            get
            {
                if (_wrapper == null)
                {
                    lock (objLock)
                    {
                        if (_wrapper == null)
                        {
                            //主窗体关闭时清理IV测试线程资源。
                            WorkbenchSingleton.Workbench.MainForm.Disposed += new EventHandler((sender, args) =>
                                                                                {
                                                                                    if (_wrapper != null)
                                                                                    {
                                                                                        _wrapper.Stop();
                                                                                        _wrapper.Dispose();
                                                                                        _wrapper = null;
                                                                                    }
                                                                                });
                            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(UploadFile);
                            _wrapper = new FileUploadThreadWrapper(threadStart);
                        }
                    }
                }
                return _wrapper;
            }
        }
        /// <summary>
        /// 文件上传。
        /// </summary>
        private void UploadFile(object obj)
        {
            FileUploadThreadWrapper wrapper = obj as FileUploadThreadWrapper;
            if (wrapper == null)
            {
                return;
            }
            UploadFile(wrapper);
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        private void UploadFile(FileUploadThreadWrapper wrapper)
        {
            while (wrapper.Loop)
            {

                FileUploadConfigElement cfg = wrapper.Config;
                FileUpload fileUpload = new FileUpload(cfg.UserName, cfg.UserPassword);
                int count = 0;
                try
                {
                    DateTime dtStartTime = DateTime.Now;
                    DateTime nowDateTime = DateTime.Now;
                    string msg = string.Empty;

                    string sourceFilePath = string.Empty;
                    string destFilePath = string.Empty;

                    //获取当前时间对应的 小时、分钟、秒
                    int hour = nowDateTime.Hour;
                    int minute = nowDateTime.Minute;
                    int second = nowDateTime.Second;

                    //根据抓去时间初始化源文件夹目录和目标地址;如果满足条件的捕抓 前一天的图片，如果不满足获取当天图片上传                    
                    if (hour % 5 == 0
                        && minute % 15 == 0
                        && (30 < second) 
                        && cfg.IsSemimanufactures)
                    {
                        sourceFilePath = GetSourcePath(cfg, nowDateTime.AddDays(-1));
                        destFilePath = GetDestPath(cfg, nowDateTime.AddDays(-1));
                    }
                    else
                    {
                        sourceFilePath = GetSourcePath(cfg, nowDateTime);
                        destFilePath = GetDestPath(cfg, nowDateTime);
                    }

                    //源文件夹存在。
                    if (Directory.Exists(sourceFilePath))
                    {
                        DateTime lastTime = new DateTime(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, 23, 50, 00);
                        DateTime startTime = new DateTime(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, 00, 10, 00);

                        string[] existensions = cfg.FileExistension.Split(new char[] { ',', ';' });
                        List<string> fileNames = new List<string>();
                        foreach (string existension in existensions)
                        {
                            fileNames.AddRange(Directory.GetFiles(sourceFilePath, existension));
                        }
                        if (fileNames.Count > 0)
                        {
                            fileUpload.MakeDir(cfg.DestRootPath, destFilePath);
                        }

                        foreach (string fileName in fileNames)
                        {
                            FileInfo fileInfo = new FileInfo(fileName);
                            TimeSpan ts = nowDateTime - fileInfo.LastWriteTime;

                            if (fileInfo.Name.IndexOf("_") >= 0)
                            {
                                LoggingService.Warn(string.Format("组件序号[{0}]错误！", fileInfo.Name));
                                File.Delete(fileName);
                            }
                            else
                            {
                                if (ts.TotalSeconds >= 30 && File.Exists(fileName))
                                {
                                    if (nowDateTime >= lastTime || nowDateTime <= startTime)
                                    {
                                        string sn = Path.GetFileNameWithoutExtension(fileInfo.Name);
                                        //获取IV测试日期
                                        IVTestDataEntity entity = new IVTestDataEntity();
                                        DataSet dsIVTestData = entity.GetIVTestDateInfo(sn, "1");
                                        if (!string.IsNullOrEmpty(entity.ErrorMsg))
                                        {
                                            LoggingService.Error(entity.ErrorMsg);
                                        }
                                        else if (dsIVTestData.Tables[0].Rows.Count > 0)
                                        {
                                            DateTime dTestDate = Convert.ToDateTime(dsIVTestData.Tables[0].Rows[0]["T_DATE"]);
                                            //生成目标文件夹。
                                            destFilePath = GetDestPath(cfg, dTestDate);
                                            fileUpload.MakeDir(cfg.DestRootPath, destFilePath);
                                        }
                                    }

                                    string reFileName = fileUpload.ExistFile(fileName, destFilePath);

                                    if (!string.IsNullOrEmpty(reFileName))
                                    {
                                        fileUpload.ReFileName(fileName, destFilePath, reFileName);
                                    }

                                    if (fileUpload.UploadFile(fileName, destFilePath) == true)
                                    {
                                        count++;

                                        //获取对应文件的数据文件
                                        string dataFileName = fileName.Replace(".GIF", ".daq").Replace(".JPEG", ".daq");


                                        //直接删除本地文件。
                                        if (cfg.IsDeleteLocalFile)
                                        {
                                            File.Delete(fileName);
                                            //判断属否存在图片对应的数据文件若存进行删除
                                            if (File.Exists(dataFileName))
                                            {
                                                File.Delete(dataFileName);
                                            }
                                        }
                                        else
                                        {
                                            //移动本地文件到当前目录下的LocalFiles文件夹下。
                                            string savePath = Path.Combine(sourceFilePath, "LocalFiles");
                                            if (!Directory.Exists(savePath))
                                            {
                                                Directory.CreateDirectory(savePath);
                                            }
                                            string destFileName = Path.Combine(savePath, fileInfo.Name);
                                            if (File.Exists(destFileName))
                                            {
                                                File.Delete(destFileName);
                                            }
                                            File.Move(fileName, destFileName);

                                            if (File.Exists(dataFileName))
                                            {
                                                FileInfo dataFileInfo = new FileInfo(dataFileName);
                                                string destDataFileName = Path.Combine(savePath, dataFileInfo.Name);
                                                if (File.Exists(destDataFileName))
                                                {
                                                    File.Delete(destDataFileName);
                                                }
                                                File.Move(dataFileName, destDataFileName);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //获取对应文件的数据文件
                                        string dataFileName = fileName.Replace(".GIF", "black.daq").Replace(".JPEG", "black.daq");

                                        //移动本地文件到当前目录下的LocalFiles文件夹下。
                                        string savePath = Path.Combine(sourceFilePath, "LocalFiles");
                                        if (!Directory.Exists(savePath))
                                        {
                                            Directory.CreateDirectory(savePath);
                                        }
                                        string destFileName = Path.Combine(savePath, fileInfo.Name);
                                        if (File.Exists(destFileName))
                                        {
                                            File.Delete(destFileName);
                                        }
                                        File.Move(fileName, destFileName);

                                        if (File.Exists(dataFileName))
                                        {
                                            FileInfo dataFileInfo = new FileInfo(dataFileName);
                                            string destDataFileName = Path.Combine(savePath, dataFileInfo.Name);
                                            if (File.Exists(destDataFileName))
                                            {
                                                File.Delete(destDataFileName);
                                            }
                                            File.Move(dataFileName, destDataFileName);
                                        }

                                        MessageBox.Show(string.Format("组件序号[{0}]对应图片为黑片，请重拍！", fileInfo.Name.Replace(".GIF", "").Replace(".JPEG", "")), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                            }
                        }
                        DateTime dtEndTime = DateTime.Now;
                        if (count > 0)
                        {
                            msg = string.Format("FileUpload--开始时间:{0};结束时间:{1};耗用时间:{2}秒;上传文件数量:{3}。{4}",
                                                 dtStartTime, dtEndTime, (dtEndTime - dtStartTime).TotalSeconds, count, destFilePath);
                            LoggingService.Info(msg);
                        }
                    }
                    else
                    {
                        msg = string.Format("FileUpload--开始时间:{0};源文件夹（{1}）不存在。", dtStartTime, sourceFilePath);
                        LoggingService.Info(msg);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Error(string.Format("FileUpload--Error:{0}", ex.Message));
                }
                if (wrapper.Loop)
                {
                    Thread.Sleep(cfg.Millisecond);
                }
            }
            wrapper.AutoResetEvent.Set();
        }
        /// <summary>
        /// 获取文件上传的路径。
        /// </summary>
        /// <param name="cfg">文件上传配置元素。</param>
        /// <param name="d">文件时间，即有效IV测试数据的日期。</param>
        /// <returns>文件上传的路径。</returns>
        private string GetDestPath(FileUploadConfigElement cfg, DateTime d)
        {
            string path = string.Format(cfg.DestPathFormat, d);
            return path;
        }

        /// <summary>
        /// 获取源文件路径。
        /// </summary>
        /// <param name="cfg">文件上传配置元素。</param>
        /// <returns>源文件路径。</returns>
        private string GetSourcePath(FileUploadConfigElement cfg, DateTime d)
        {
            string path = string.Format(cfg.SourcePathFormat, d);
            return path;
        }

        protected abstract FileUploadConfigElement GetFileUploadConfigElement();
        /// <summary>
        /// 执行命令。
        /// </summary>
        public override void Run()
        {
            FileUploadConfigElement cfg = GetFileUploadConfigElement();

            if (!string.IsNullOrEmpty(cfg.SourceRootPath))
            {
                if (ThreadWrapper.Loop)
                {
                    ThreadWrapper.Stop();
                }
                ThreadWrapper.Start(cfg);
            }
            else
            {
                if (_wrapper != null)
                {
                    _wrapper.Stop();
                    _wrapper.Dispose();
                    _wrapper = null;
                }
            }
        }
    }

    /// <summary>
    /// 文件上传类。
    /// </summary>
    public class FileUpload
    {
        private WebRequest reqFTP;
        private long lFileLen = 10240;
        private string _userName = string.Empty;
        private string _userPassword = string.Empty;

        public FileUpload(string userName, string userPassword)
        {
            this._userName = userName;
            this._userPassword = userPassword;
        }
        //连接FTP
        private void Connect(string path)
        {
            // 根据uri创建FtpWebRequest对象
            reqFTP = WebRequest.Create(new Uri(path));
            // 指定数据传输类型
            if (reqFTP is FtpWebRequest)
            {
                ((FtpWebRequest)reqFTP).UseBinary = true;
            }
            // ftp用户名和密码
            if (!string.IsNullOrEmpty(this._userName))
            {
                reqFTP.Credentials = new NetworkCredential(this._userName, this._userPassword);
            }
        }
        /// <summary>
        /// 上传文件。
        /// </summary>
        /// <param name="filename">文件名称。</param>
        /// <param name="destFilePath">目标文件夹路径。</param>
        /// <returns>true：上传成功,false：上传失败。</returns>
        public bool UploadFile(string filename, string destFilePath)
        {
            bool Result = false;
            FileInfo fileInfo = new FileInfo(filename);
            string uri = string.Format("{0}\\{1}", destFilePath, fileInfo.Name);
            Connect(uri);//连接
            // 默认为true，连接不会被关闭
            // 在一个命令之后被执行
            if (reqFTP is FtpWebRequest)
            {
                ((FtpWebRequest)reqFTP).KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            }
            else
            {
                reqFTP.Method = WebRequestMethods.File.UploadFile;
            }

            if (fileInfo.Length < lFileLen)
            {
                Result = false;
            }
            else
            {
                //上传文件时通知服务器文件的大小
                reqFTP.ContentLength = fileInfo.Length;
                // 缓冲大小设置为kb 
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                // 打开一个文件流(System.IO.FileStream) 去读上传的文件
                FileStream fs = fileInfo.OpenRead();
                try
                {
                    // 把上传的文件写入流
                    Stream strm = reqFTP.GetRequestStream();
                    // 每次读文件流的kb 
                    contentLen = fs.Read(buff, 0, buffLength);
                    // 流内容没有结束
                    while (contentLen != 0)
                    {
                        // 把内容从file stream 写入upload stream 
                        strm.Write(buff, 0, contentLen);
                        contentLen = fs.Read(buff, 0, buffLength);
                    }
                    // 关闭两个流
                    strm.Close();
                    fs.Close();
                    Result = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return Result;
        }
        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="fileName">目标文件</param>
        public void DeleteFile(string fileName)
        {
            try
            {
                string uri = fileName;
                if (reqFTP == null)
                {
                    Connect(uri);//连接
                }

                if (reqFTP is FtpWebRequest)
                {
                    // 默认为true，连接不会被关闭
                    // 在一个命令之后被执行
                    ((FtpWebRequest)reqFTP).KeepAlive = false;
                    // 指定执行什么命令
                    reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                    WebResponse response = reqFTP.GetResponse();
                    response.Close();
                }
                else
                {
                    File.Delete(uri);
                }

            }
            catch (Exception ex)
            {
                LoggingService.Error(ex);
            }
        }
        /// <summary>
        /// 创建目录。
        /// </summary>
        /// <param name="filePath">目录。</param>
        public void MakeDir(string rootPath, string filePath)
        {
            string childPath = filePath.Replace(rootPath, string.Empty);
            string[] childPaths = childPath.Split('/');
            string path = rootPath;
            for (int i = 0; i < childPaths.Length; i++)
            {
                try
                {
                    path = path.TrimEnd('/') + "/" + childPaths[i] + "/";
                    if (reqFTP == null)
                    {
                        Connect(path);//连接 
                    }

                    if (reqFTP is FtpWebRequest)
                    {
                        bool isExists = true;
                        ((FtpWebRequest)reqFTP).KeepAlive = false;
                        ((FtpWebRequest)reqFTP).Method = WebRequestMethods.Ftp.ListDirectory;
                        WebResponse response = null;
                        try
                        {
                            response = ((FtpWebRequest)reqFTP).GetResponse() as FtpWebResponse;
                        }
                        catch //目录不存在
                        {
                            isExists = false;
                        }
                        finally
                        {
                            if (response != null)
                            {
                                response.Close();
                                response = null;
                            }
                            reqFTP = null;
                        }

                        if (isExists == false)
                        {
                            Connect(path);//连接 
                            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                            response = (FtpWebResponse)reqFTP.GetResponse();
                            if (response != null)
                            {
                                response.Close();
                                response = null;
                            }
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.Error(ex);
                }
                finally
                {
                    reqFTP = null;
                }
            }
        }
        /// <summary>
        /// 删除目录。
        /// </summary>
        /// <param name="dirName">目录名称。</param>
        public void DelDir(string dirName)
        {
            try
            {
                if (reqFTP == null)
                {
                    Connect(dirName);//连接 
                }

                if (reqFTP is FtpWebRequest)
                {
                    ((FtpWebRequest)reqFTP).KeepAlive = false;
                    reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    response.Close();
                }
                else
                {
                    Directory.Delete(dirName);
                }
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex);
            }
        }

        /// <summary>
        /// 判断上传的图片名称是否已经存在
        /// 如果存在的话返回一个新的文件名称
        /// </summary>
        /// <param name="fileName">要上传的文件名称</param>
        /// <param name="destFilePath">上传文件夹路径</param>
        /// <returns>新的文件的名称</returns>
        public string ExistFile(string fileName, string destFilePath)
        {

            string reFileName = string.Empty;
            try
            {
                FileInfo fileInfo = new FileInfo(fileName);
                string uri = destFilePath;

                Connect(uri);//连接 

                if (reqFTP is FtpWebRequest)
                {
                    ((FtpWebRequest)reqFTP).KeepAlive = false;
                    ((FtpWebRequest)reqFTP).Method = WebRequestMethods.Ftp.ListDirectory;

                    bool success = false;

                    WebResponse webResponse = null;
                    StreamReader reader = null;

                    //存储文件夹中文件的名称列表
                    List<string> lsDirectory = new List<string>();

                    webResponse = ((FtpWebRequest)reqFTP).GetResponse();
                    reader = new StreamReader(webResponse.GetResponseStream());
                    string line = reader.ReadLine();

                    while (line != null)
                    {
                        lsDirectory.Add(line);

                        if (!success)
                        {
                            if (line == fileInfo.Name)
                            {
                                success = true;
                            }
                        }

                        line = reader.ReadLine();
                    }

                    //判断是否存在同名文件，若存在遍历获取一个新的文件名称
                    if (success)
                    {
                        string fileFirstName = fileInfo.Name.Replace(fileInfo.Extension, "");
                        string fileLastName = fileInfo.Extension;
                        string Name = string.Empty;

                        for (int i = 1; i < i + 1; i++)
                        {
                            Name = fileFirstName + "-" + i.ToString() + fileLastName;

                            success = false;
                            foreach (string lineName in lsDirectory)
                            {
                                if (lineName == Name)
                                {
                                    success = true;
                                    break;
                                }
                            }

                            if (!success)
                            {
                                return reFileName = Name;
                            }
                        }

                    }
                    else
                    {
                        return reFileName;
                    }

                }
                else
                {
                    //判断是否存在同名文件，若存在遍历获取一个新的文件名称
                    if (File.Exists(destFilePath + "\\" + fileInfo.Name))
                    {
                        string fileFirstName = fileInfo.Name.Replace(fileInfo.Extension, "");
                        string fileLastName = fileInfo.Extension;
                        string Name = string.Empty;

                        for (int i = 1; i < i + 1; i++)
                        {
                            Name = fileFirstName + "-" + i.ToString() + fileLastName;

                            if (!File.Exists(destFilePath + "\\" + Name))
                            {
                                return reFileName = Name;
                            }
                        }

                    }
                    else
                    {
                        return reFileName;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex);
            }

            return reFileName;
        }
        /// <summary>
        /// 重命名文件。
        /// </summary>
        /// <param name="filename">文件名称。</param>
        /// <param name="destFilePath">目标文件夹路径。</param>
        public void ReFileName(string filename, string destFilePath, string reFileName)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filename);
                string uri = string.Format("{0}\\{1}", destFilePath, fileInfo.Name);

                Connect(uri);//连接 

                if (reqFTP is FtpWebRequest)
                {
                    FtpWebResponse ftpWebResponse = null;
                    Stream ftpResponseStream = null;

                    ((FtpWebRequest)reqFTP).UseBinary = true;
                    ((FtpWebRequest)reqFTP).Method = WebRequestMethods.Ftp.Rename;
                    ((FtpWebRequest)reqFTP).RenameTo = reFileName;

                    ftpWebResponse = (FtpWebResponse)((FtpWebRequest)reqFTP).GetResponse();
                    ftpResponseStream = ftpWebResponse.GetResponseStream();

                }
                else
                {
                    File.Move(destFilePath + "\\" + fileInfo.Name, destFilePath + "\\" + reFileName);
                }
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex);
            }
        }
    }

    /// <summary>
    /// 文件上传线程的封装类。
    /// </summary>
    public class FileUploadThreadWrapper : IDisposable
    {
        /// <summary>
        /// 获取线程执行的循环标志。
        /// </summary>
        public bool Loop { get; private set; }
        /// <summary>
        /// 获取线程执行异步事件。
        /// </summary>
        public AutoResetEvent AutoResetEvent { get; private set; }
        /// <summary>
        /// 获取线程对象。
        /// </summary>
        public Thread Thread { get; private set; }
        /// <summary>
        /// 获取文件上传配置对象。
        /// </summary>
        public FileUploadConfigElement Config { get; set; }

        private ParameterizedThreadStart _threadStart = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public FileUploadThreadWrapper(ParameterizedThreadStart threadStart)
        {
            this.Loop = false;
            this.AutoResetEvent = new AutoResetEvent(false);
            this._threadStart = threadStart;
        }
        /// <summary>
        /// 启动线程。
        /// </summary>
        public void Start(FileUploadConfigElement cfg)
        {
            LoggingService.Info(string.Format("FileUpload--Start {0}", DateTime.Now));
            this.Loop = true;
            this.Config = cfg;
            this.Thread = new Thread(this._threadStart);
            this.Thread.Start(this);
        }
        /// <summary>
        /// 停止线程。
        /// </summary>
        public void Stop()
        {
            this.Loop = false;
            if (!this.AutoResetEvent.WaitOne(10000))
            {
                this.Thread.Abort();
            }
            this.Thread = null;
            LoggingService.Info(string.Format("FileUpload--Stop {0}", DateTime.Now));
        }
        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            this.AutoResetEvent.Close();
            this.AutoResetEvent = null;
            this.Thread = null;
        }
    }
    /// <summary>
    /// 文件上传配置。
    /// </summary>
    public class FileUploadConfigElement
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="srcPath">源文件夹根目录。</param>
        /// <param name="destPath">目标文件夹根目录。</param>
        /// <param name="fileExistension">文件扩展名。</param>
        public FileUploadConfigElement(string srcRootPath, string destRootPath, string fileExistension)
        {
            this.SourceRootPath = srcRootPath;
            this.SourcePathFormat = Path.Combine(this.SourceRootPath, "{0:yyyy年}\\{0:M月}\\{0:yyyy-MM-dd}");
            this.DestRootPath = destRootPath;
            this.DestPathFormat = Path.Combine(this.DestRootPath, "{0:yyyy年}/{0:M月}/{0:yyyy-MM-dd}");
            this.FileExistension = fileExistension;
            this.Millisecond = 5000;
            this.UserName = string.Empty;
            this.UserPassword = string.Empty;
            this.IsDeleteLocalFile = true;
            this.IsSemimanufactures = false;
        }
        /// <summary>
        /// 源文件夹根目录。
        /// </summary>
        public string SourceRootPath
        {
            get;
            set;
        }
        /// <summary>
        /// 源文件夹格式。this.SourceRootPath\{0:yyyy年}\{0:M月}\{0:yyyy-MM-dd}
        /// </summary>
        public string SourcePathFormat
        {
            get;
            set;
        }
        /// <summary>
        /// 目标文件夹根目录。
        /// </summary>
        public string DestRootPath
        {
            get;
            set;
        }
        /// <summary>
        /// 目标文件夹格式。this.DestRootPath\{0:yyyy年}\{0:M月}\{0:yyyy-MM-dd}
        /// </summary>
        public string DestPathFormat
        {
            get;
            set;
        }
        /// <summary>
        /// 文件扩展名。
        /// </summary>
        public string FileExistension
        {
            get;
            set;
        }
        /// <summary>
        /// 间隔的毫秒数。
        /// </summary>
        public int Millisecond
        {
            get;
            set;
        }
        /// <summary>
        /// 验证用户名。
        /// </summary>
        public string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// 验证用户密码。
        /// </summary>
        public string UserPassword
        {
            get;
            set;
        }

        /// <summary>
        /// 是否删除本地文件。
        /// </summary>
        public bool IsDeleteLocalFile
        {
            get;
            set;
        }
        /// <summary>
        /// 上传图片是否为半成品图片
        /// </summary>
        public bool IsSemimanufactures
        {
            get;
            set;
        }
    }
}
