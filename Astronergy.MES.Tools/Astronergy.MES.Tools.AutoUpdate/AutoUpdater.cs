using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Astronergy.MES.Tools.AutoUpdate
{
    /// <summary>
    /// 自动更新的动作枚举。
    /// </summary>
    public enum EnumAutoUpdateAction
    {
        Delete=0,
        Update=1,
        Add=2
    }
    /// <summary>
    /// 自动更新类。
    /// </summary>
    class AutoUpdater
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="dir">工作目录。</param>
        /// <param name="url">自动更新的基础URL地址。</param>
        public AutoUpdater(string dir, string url)
        {
            this.WorkDirectory = dir;
            this.BaseUrl = url;
        }
        /// <summary>
        /// 工作目录。
        /// </summary>
        public string WorkDirectory
        {
            get;
            set;
        }
        /// <summary>
        /// 自动更新的URL地址。
        /// </summary>
        public string BaseUrl
        {
            get;
            set;
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
                string fileFullPath = Path.Combine(this.WorkDirectory, filePath);
                string url=Path.Combine(this.BaseUrl,filePath.Replace('\\','/'));

                string dir = Path.GetDirectoryName(fileFullPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                WebRequest req = WebRequest.Create(url);
                //局域网内，不使用代理。
                req.Proxy = new WebProxy();
                WebResponse res = req.GetResponse();
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
                            //float part = (float)startByte / 1024;
                            //float total = (float)bufferbyte.Length / 1024;
                            //int percent = Convert.ToInt32((part / total) * 100);
                        }
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 自动更新文件。对比文件判断是否需要自动更新。
        /// </summary>
        /// <param name="newFile">新文件。</param>
        /// <param name="oldFile">旧文件。</param>
        /// <returns>更新成功，true。更新失败，false。</returns>
        public bool Update(AutoUpdateXmlFile newFile,AutoUpdateXmlFile oldFile)
        {
            bool bSuccess = true;
            if (newFile==null || oldFile==null)
            {
                bSuccess = false;
                return bSuccess;
            }
            Dictionary<AutoUpdateItem, EnumAutoUpdateAction> updateItems = new Dictionary<AutoUpdateItem, EnumAutoUpdateAction>();

            #region 检查程序集
            Console.WriteLine("正在检查需要更新的程序集...");
            foreach (AutoUpdateItem oldItem in oldFile.Assemblies)
            {
                bool bMatch = false;
                bool bIsExits = false;
                foreach (AutoUpdateItem newItem in newFile.Assemblies)
                {
                    if (newItem.RelativePath == oldItem.RelativePath)
                    {
                        bIsExits = true;
                        string oldItemVersion = oldItem.Version ?? string.Empty;
                        string newItemVersion = newItem.Version ?? string.Empty;
                        if (oldItemVersion == newItemVersion
                            && oldItem.Size==newItem.Size)
                        {
                            bMatch = true;
                        }
                        break;
                    }
                }
                if (!bIsExits)
                {
                    //updateItems.Add(oldItem, EnumAutoUpdateAction.Delete);
                }
                else if(!bMatch)
                {
                    updateItems.Add(oldItem, EnumAutoUpdateAction.Update);
                }
            }

            foreach (AutoUpdateItem newItem in newFile.Assemblies)
            {
                bool bIsExits = false;
                foreach (AutoUpdateItem oldItem in oldFile.Assemblies)
                {
                    if (newItem.RelativePath == oldItem.RelativePath)
                    {
                       bIsExits=true;
                       break;
                    }
                }
                if (!bIsExits)
                {
                    updateItems.Add(newItem, EnumAutoUpdateAction.Add);
                }
            }
            #endregion

            #region 检查文件
            Console.WriteLine("正在检查需要更新的文件...");
            foreach (AutoUpdateItem oldItem in oldFile.Files)
            {
                bool bMatch = false;
                bool bIsExits = false;
                foreach (AutoUpdateItem newItem in newFile.Files)
                {
                    if (newItem.RelativePath == oldItem.RelativePath)
                    {
                        bIsExits = true;
                        string oldItemVersion = oldItem.Version ?? string.Empty;
                        string newItemVersion = newItem.Version ?? string.Empty;
                        if (oldItemVersion == newItemVersion
                           && oldItem.Size == newItem.Size)
                        {
                            bMatch = true;
                            if (oldItem.LastestModifyTime < newItem.LastestModifyTime)
                            {
                                bMatch = false;
                            }
                        }
                        break;
                    }
                }
                if (!bIsExits)
                {
                    //updateItems.Add(oldItem, EnumAutoUpdateAction.Delete);
                }
                else if (!bMatch)
                {
                    updateItems.Add(oldItem, EnumAutoUpdateAction.Update);
                }
            }

            foreach (AutoUpdateItem newItem in newFile.Files)
            {
                bool bIsExits = false;
                foreach (AutoUpdateItem oldItem in oldFile.Files)
                {
                    if (newItem.RelativePath == oldItem.RelativePath)
                    {
                        bIsExits = true;
                        break;
                    }
                }
                if (!bIsExits)
                {
                    updateItems.Add(newItem, EnumAutoUpdateAction.Add);
                }
            }
            #endregion

            foreach (AutoUpdateItem item in updateItems.Keys)
            {
                EnumAutoUpdateAction action = updateItems[item];
                if (action == EnumAutoUpdateAction.Add || action == EnumAutoUpdateAction.Update)
                {
                    Console.WriteLine("正在更新：" + item.RelativePath);
                    if (!DownloadFile(item.RelativePath) && bSuccess)
                    {
                        bSuccess = false;
                    }
                }
            }
            return bSuccess;
        }
    }
}
