using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Data;
using System.IO;
using System.ComponentModel;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.WIP.Gui
{
    public class FtpFileHelper
    {
        private WebRequest reqFTP;
        private long lFileLen = 10240;
        private string _userName = string.Empty;
        private string _userPassword = string.Empty;

        public FtpFileHelper(string userName, string userPassword)
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
                    //throw ex;
                    MessageService.ShowMessage(ex.Message + "-UploadFile 上传失败");
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
                Connect(uri);//连接

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
                //LoggingService.Error(ex);
                MessageService.ShowMessage(ex.Message + "-DeleteFile 删除文件失败");
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
                    //LoggingService.Error(ex);
                    MessageService.ShowMessage(ex.Message + "-MakeDir 创建目录失败");
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
                //LoggingService.Error(ex);
                MessageService.ShowMessage(ex.Message + "-ExistFile 判定文件失败");
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
                //LoggingService.Error(ex);
                MessageService.ShowMessage(ex.Message + "-ReFileName 重命名文件失败");
            }
        }

    }
}