using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using Astronergy.MES.Tools.AutoUpdate.Configuration;

namespace Astronergy.MES.Tools.AutoUpdate
{
    /// <summary>
    /// 自动更新XML序列器的工具类。
    /// </summary>
    public class AutoUpdateXmlSerializerUtil
    {
        public static AutoUpdateSection section=(AutoUpdateSection)
            System.Configuration.ConfigurationManager.GetSection("astronergy.mes.tools/autoupdate");
        /// <summary>
        /// 根据指定文件夹中的文件信息并在此文件夹下输出自动更新的XML文件。
        /// </summary>
        /// <param name="dir">指定文件夹。</param>
        /// <param name="fileName">自动更新的文件名。</param>
        public static void Write(string dir,string fileName)
        {
            AutoUpdateXmlFile au = Get(dir);
            //根据文件格式生成XML文件。
            XmlSerializer serializer = new XmlSerializer(typeof(AutoUpdateXmlFile));
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                serializer.Serialize(stream, au);
            }
        }
        /// <summary>
        /// 检查是否忽略文件。
        /// </summary>
        /// <param name="relativePath">文件的相对路径。</param>
        /// <returns>true 忽略 false 不忽略。</returns>
        public static bool CheckIsIgnoreFile(string relativePath)
        {
            //获取配置节信息
            if (section != null
                && section.IgnoreFiles !=null
                && section.IgnoreFiles[relativePath] != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据指定文件夹中的文件信息获取自动更新的XML文件类。
        /// </summary>
        /// <param name="dir">指定文件夹。</param>
        public static AutoUpdateXmlFile Get(string dir)
        {
            //获取指定目录下的所有文件。
            string[] files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
            AutoUpdateXmlFile au = new AutoUpdateXmlFile();
            foreach (string file in files)
            {
                //忽略指定文件。
                string relativePath = file.Replace(dir, "").TrimStart('\\');
                Console.WriteLine(string.Format("检测本地文件：{0}",relativePath));
                if (CheckIsIgnoreFile(relativePath))
                {
                    continue;
                }
                //添加需要自动更新的文件。
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(file);
                AutoUpdateItem item = new AutoUpdateItem();
                item.LastestModifyTime = File.GetLastWriteTime(file);
                item.RelativePath = relativePath;
                item.Name = versionInfo.OriginalFilename;
                item.Version = versionInfo.FileVersion;
                item.Description = versionInfo.FileDescription;
                item.Product = versionInfo.ProductName;
                item.Company = versionInfo.CompanyName;
                FileInfo fileInfo = new FileInfo(file);
                item.Size = fileInfo.Length;

                string extendName = Path.GetExtension(file);
                if (extendName.ToLower() == ".dll" || extendName.ToLower() == ".exe")
                {
                    au.Assemblies.Add(item);
                }
                else
                {
                    au.Files.Add(item);
                }
            }
            return au;
        }
        /// <summary>
        /// 从指定文件夹中恢复读取自动更新的XML文件。
        /// </summary>
        /// <param name="dir">指定文件夹。</param>
        /// <param name="fileName">自动更新的文件名。</param>
        /// <returns><see cref="AutoUpdate"/>对象。</returns>
        public static AutoUpdateXmlFile Read(string dir,string fileName)
        {
            string filePath = Path.Combine(dir, fileName);
            if (!File.Exists(filePath))
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(AutoUpdateXmlFile));
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                return serializer.Deserialize(stream) as AutoUpdateXmlFile;
            }
        }
    }
}
