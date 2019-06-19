//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-02-27            添加注释 
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using FanHai.Hemera.Utils.Entities;
using System.IO;

namespace FanHai.Hemera.Addins.EAP
{
    /// <summary>
    /// 数据文件读取配置数据的读取类。
    /// </summary>
    public class DataFileReadConfigReader
    {
        /// <summary>
        /// 获取反射率的配置数据。
        /// </summary>
        /// <returns></returns>
        public static DataFileReadConfig GetRflectanceConfig()
        {
            //1	F_STARTROW	    反射率数据在文件中的起始行
            //2	F_FILEPATH	    反射率读取的文件存放路径
            //3	F_DATACOLUMN	反射率数据行中列号
            //4 F_SearchPattern 反射率文件搜索模式
            //5	F_IdentityName	反射率数据在行中标识符
            //6 F_Separator     反射率数据在行中分隔符
            //7 F_Direction     反射率数据的搜索方向
            ComputerEntity entity = new ComputerEntity();
            DataTable dtComputerUDA = entity.GetComputerInfo();
            string startRow     = entity.GetComputerUDAAttributeValue(dtComputerUDA, "F_STARTROW");
            string filePath     = entity.GetComputerUDAAttributeValue(dtComputerUDA, "F_FILEPATH");
            string dataColumn   = entity.GetComputerUDAAttributeValue(dtComputerUDA, "F_DATACOLUMN");
            string identifyName = entity.GetComputerUDAAttributeValue(dtComputerUDA, "F_IdentityName");
            string pattern = entity.GetComputerUDAAttributeValue(dtComputerUDA, "F_SearchPattern");
            string separator    = entity.GetComputerUDAAttributeValue(dtComputerUDA, "F_Separator");
            string direction = entity.GetComputerUDAAttributeValue(dtComputerUDA, "F_Direction");
            string fileName     = string.Empty;
            pattern = pattern == string.Empty ? "*.*" : pattern;
            if (!string.IsNullOrEmpty(filePath) && System.IO.Directory.Exists(filePath))
            {
                fileName = GetFileNameByLastTime(filePath, pattern);
            }
            DataFileReadConfig config = new DataFileReadConfig();
            config.Column = string.IsNullOrEmpty(dataColumn) ?  9 : Convert.ToInt32(dataColumn);
            config.StartRow = string.IsNullOrEmpty(startRow) ?  0 : Convert.ToInt32(startRow);
            config.FileName = fileName;
            config.IdentityName = string.IsNullOrEmpty(identifyName) ? "POINT" : identifyName;
            config.Separator = string.IsNullOrEmpty(separator)?',':separator.ToCharArray()[0];
            config.SearchPattern = pattern;
            config.Direction = string.IsNullOrEmpty(direction) ? "F": direction;
            return config;
        }

        /// <summary>
        /// 获取折射率的配置数据。
        /// </summary>
        /// <returns></returns>
        public static DataFileReadConfig GetRefractionConfig()
        {
            //1 A_STARTROW	    折射率数据在文件中的起始行
            //2	A_FILEPATH	    折射率读取的文件存放路径
            //3	A_DATACOLUMN	折射率数据行中列号
            //4 A_SearchPattern 折射率文件扩展名
            //5	A_IdentityName	折射率数据在行中标识符
            //6 A_Separator     折射率数据在行中分隔符
            //7 A_Direction     折射率数据的搜索方向
            ComputerEntity entity = new ComputerEntity();
            DataTable dtComputerUDA = entity.GetComputerInfo();
            string startRow     = entity.GetComputerUDAAttributeValue(dtComputerUDA, "A_STARTROW");
            string filePath     = entity.GetComputerUDAAttributeValue(dtComputerUDA, "A_FILEPATH");
            string dataColumn   = entity.GetComputerUDAAttributeValue(dtComputerUDA, "A_DATACOLUMN");
            string identifyName = entity.GetComputerUDAAttributeValue(dtComputerUDA, "A_IdentityName");
            string pattern      = entity.GetComputerUDAAttributeValue(dtComputerUDA, "A_SearchPattern");
            string separator    = entity.GetComputerUDAAttributeValue(dtComputerUDA, "A_Separator");
            string direction = entity.GetComputerUDAAttributeValue(dtComputerUDA, "A_Direction");
            string fileName     = string.Empty;
            pattern = pattern == string.Empty ? "*.*" : pattern;
            if (!string.IsNullOrEmpty(filePath) && System.IO.Directory.Exists(filePath))
            {
                fileName = GetFileNameByLastTime(filePath, pattern);
            }
            DataFileReadConfig config = new DataFileReadConfig();
            config.Column = string.IsNullOrEmpty(dataColumn) ? 2 : Convert.ToInt32(dataColumn);
            config.StartRow = string.IsNullOrEmpty(startRow) ? 0 : Convert.ToInt32(startRow);
            config.FileName = fileName;
            config.IdentityName = string.IsNullOrEmpty(identifyName) ? "n:" : identifyName;
            config.Separator = string.IsNullOrEmpty(separator) ? ' ' : separator.ToCharArray()[0];
            config.SearchPattern = pattern;
            config.Direction = string.IsNullOrEmpty(direction) ? "F" : direction;
            return config;
        }

        /// <summary>
        /// 获取膜厚的配置数据。
        /// </summary>
        /// <returns></returns>
        public static DataFileReadConfig GetThicknessConfig()
        {
            //1 T_STARTROW	    膜厚数据在文件中的起始行
            //2	T_FILEPATH	    膜厚读取的文件存放路径
            //3	T_DATACOLUMN	膜厚数据行中列号
            //4 T_SearchPattern 膜厚文件扩展名
            //5	T_IdentityName	膜厚数据在行中标识符
            //6 T_Separator     膜厚数据在行中分隔符
            //7 T_Direction     膜厚数据的搜索方向
            ComputerEntity entity = new ComputerEntity();
            DataTable dtComputerUDA = entity.GetComputerInfo();
            string startRow     = entity.GetComputerUDAAttributeValue(dtComputerUDA, "T_STARTROW");
            string filePath     = entity.GetComputerUDAAttributeValue(dtComputerUDA, "T_FILEPATH");
            string dataColumn   = entity.GetComputerUDAAttributeValue(dtComputerUDA, "T_DATACOLUMN");
            string identifyName = entity.GetComputerUDAAttributeValue(dtComputerUDA, "T_IdentityName");
            string pattern = entity.GetComputerUDAAttributeValue(dtComputerUDA, "T_SearchPattern");
            string separator    = entity.GetComputerUDAAttributeValue(dtComputerUDA, "T_Separator");
            string direction = entity.GetComputerUDAAttributeValue(dtComputerUDA, "T_Direction");
            string fileName     = string.Empty;
            pattern = pattern == string.Empty ? "*.*" : pattern;
            if (!string.IsNullOrEmpty(filePath) && System.IO.Directory.Exists(filePath))
            {
                fileName = GetFileNameByLastTime(filePath, pattern);
            }
            DataFileReadConfig config = new DataFileReadConfig();
            config.Column = string.IsNullOrEmpty(dataColumn) ? 2 : Convert.ToInt32(dataColumn);
            config.StartRow = string.IsNullOrEmpty(startRow) ? 0 : Convert.ToInt32(startRow);
            config.FileName = fileName;
            config.IdentityName = string.IsNullOrEmpty(identifyName) ? "Thickness:" : identifyName;
            config.Separator = string.IsNullOrEmpty(separator) ? ' ' : separator.ToCharArray()[0];
            config.SearchPattern = pattern;
            config.Direction = string.IsNullOrEmpty(direction) ? "F" : direction;
            return config;
        }
        /// <summary>
        /// 获取方阻的配置数据。
        /// </summary>
        /// <returns></returns>
        public static DataFileReadConfig GetResistanceConfig(DateTime startTime)
        {
            //1 R_STARTROW	    方租数据在文件中的起始行
            //2	R_FILEPATH	    方租读取的文件存放路径
            //3	R_DATACOLUMN	方租数据行中列号
            //4 R_SearchPattern 方租文件扩展名
            //5	R_IdentityName	方租数据在行中标识符
            //6 R_Separator     方租数据在行中分隔符
            //7 R_Direction     方租数据的搜索方向
            ComputerEntity entity = new ComputerEntity();
            DataTable dtComputerUDA = entity.GetComputerInfo();
            string startRow     = entity.GetComputerUDAAttributeValue(dtComputerUDA, "R_STARTROW");
            string filePath     = entity.GetComputerUDAAttributeValue(dtComputerUDA, "R_FILEPATH");
            string dataColumn   = entity.GetComputerUDAAttributeValue(dtComputerUDA, "R_DATACOLUMN");
            string identifyName = entity.GetComputerUDAAttributeValue(dtComputerUDA, "R_IdentityName");
            string pattern = entity.GetComputerUDAAttributeValue(dtComputerUDA, "R_SearchPattern");
            string separator    = entity.GetComputerUDAAttributeValue(dtComputerUDA, "R_Separator");
            string direction = entity.GetComputerUDAAttributeValue(dtComputerUDA, "R_Direction");
            string fileName     = string.Empty;
            pattern = pattern==string.Empty ? "*.*" : pattern;
            if (!string.IsNullOrEmpty(filePath) && System.IO.Directory.Exists(filePath))
            {
                fileName = GetFileNameByLastTime(filePath, pattern, startTime);
            }
            DataFileReadConfig config = new DataFileReadConfig();
            config.Column = string.IsNullOrEmpty(dataColumn) ? 9 : Convert.ToInt32(dataColumn);
            config.StartRow = string.IsNullOrEmpty(startRow) ? 0 : Convert.ToInt32(startRow);
            config.FileName = fileName;
            config.IdentityName = string.IsNullOrEmpty(identifyName) ? "POINT" : identifyName;
            config.Separator = string.IsNullOrEmpty(separator) ? ',' : separator.ToCharArray()[0];
            config.SearchPattern = pattern;
            config.Direction = string.IsNullOrEmpty(direction) ? "F" : direction;
            if (System.IO.File.Exists(fileName))
            {
                config.FileLastWriteTime = System.IO.File.GetLastWriteTime(fileName);
            }
            return config;
        }
        /// <summary>
        /// 获取文件路径中的最新的更新的文件。
        /// </summary>
        /// <param name="filePath">文件夹路径。</param>
        /// <returns>最新的文件名称。</returns>
        private static string GetFileNameByLastTime(string filePath, string pattern, DateTime startTime)
        {
            DateTime lastestTime = startTime;
            string lastestFileName = string.Empty;
            string[] files = System.IO.Directory.GetFiles(filePath, pattern, SearchOption.AllDirectories);
            foreach (string file in files)
            {
                FileAttributes attribute = File.GetAttributes(file);
                if ((attribute & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    continue;
                }
                if ((attribute & FileAttributes.System) == FileAttributes.System)
                {
                    continue;
                }
                if ((attribute & FileAttributes.Temporary) == FileAttributes.Temporary)
                {
                    continue;
                }
                if ((attribute & FileAttributes.SparseFile) == FileAttributes.SparseFile)
                {
                    continue;
                }
                DateTime fileTime = System.IO.File.GetLastWriteTime(file);
                if (lastestTime < fileTime)
                {
                    lastestTime = fileTime;
                    lastestFileName = file;
                }
            }
            return lastestFileName;
        }
        /// <summary>
        /// 获取文件路径中的最新的更新的文件。
        /// </summary>
        /// <param name="filePath">文件夹路径。</param>
        /// <returns>最新的文件名称。</returns>
        private static string GetFileNameByLastTime(string filePath,string pattern)
        {
            return GetFileNameByLastTime(filePath, pattern, DateTime.MinValue);
        }
    }
    /// <summary>
    /// 数据文件读取的值。
    /// </summary>
    public class DataFileReadValue
    {
        public string Value = string.Empty;
    }
    /// <summary>
    /// 数据文件读取的配置类。
    /// </summary>
    public class DataFileReadConfig
    {
        public string FileName = string.Empty;
        public int StartRow = 0;
        public int PointCount = 0;
        public char Separator = ',';
        public string IdentityName = "";
        public int Column = 0;
        public string SearchPattern ="*.*";
        /// <summary>
        /// 搜索方向。F：(向前，Forward）;B：（向后，Backword）。默认为F。
        /// </summary>
        public string Direction = "F";
        public DateTime FileLastWriteTime = DateTime.MinValue;
    }
    /// <summary>
    /// 数据文件读取接口。
    /// </summary>
    public interface IDataFileReader
    {
        /// <summary>
        /// 根据配置文件读取数据值集合。
        /// </summary>
        /// <param name="config">配置数据。</param>
        /// <returns>获取的数据集合。</returns>
        IList<DataFileReadValue> Reader(DataFileReadConfig config);
    }
    /// <summary>
    /// 数据文件读取的实现类。
    /// </summary>
    public class DataFileReader : IDataFileReader
    {
        /// <summary>
        /// 根据配置文件读取数据值集合。
        /// </summary>
        /// <param name="config">配置数据。</param>
        /// <returns>获取的数据集合。</returns>
        public IList<DataFileReadValue> Reader(DataFileReadConfig config)
        {
            if (string.IsNullOrEmpty(config.FileName))
            {
                throw new NoNullAllowedException("FileName 不能为空。");
            }
            if (!System.IO.File.Exists(config.FileName))
            {
                throw new System.IO.FileNotFoundException(config.FileName);
            }
            List<DataFileReadValue> lst = new List<DataFileReadValue>();
            List<string> lstFileContent = new List<string>();
            using (System.IO.StreamReader reader = new System.IO.StreamReader(config.FileName, Encoding.Default))
            {
                string strLine = string.Empty;
                while ((strLine = reader.ReadLine()) != null)
                {
                    if (config.Separator == ' ')
                    {
                        string pattern = string.Format("{0}+", config.Separator);
                        strLine = Regex.Replace(strLine, pattern, config.Separator.ToString());
                    }
                    lstFileContent.Add(strLine);
                }
            }
            int pointCount = config.PointCount;
            int startRow = config.StartRow;
            int count = 0;

            if (config.Direction == "B")//倒序读取
            {
                if (startRow < 0)
                {
                    startRow = lstFileContent.Count + startRow;
                }
                
                for (int i = startRow; i < lstFileContent.Count && i >= 0 && count < pointCount; i--)
                {
                    string[] arr = lstFileContent[i].Split(config.Separator);
                    if (arr.Contains(config.IdentityName))
                    {
                        DataFileReadValue data = new DataFileReadValue();
                        if (config.Column < arr.Length)
                        {
                            data.Value = arr[config.Column];
                        }
                        lst.Insert(0,data);
                        count++;
                    }
                }
            }
            else//正序读取
            {
                if (startRow < 0)
                {
                    startRow = lstFileContent.Count + startRow;
                }

                for (int i = startRow; i < lstFileContent.Count && i >= 0 && count < pointCount; i++)
                {
                    string[] arr = lstFileContent[i].Split(config.Separator);
                    if (arr.Contains(config.IdentityName))
                    {
                        DataFileReadValue data = new DataFileReadValue();
                        if (config.Column < arr.Length)
                        {
                            data.Value = arr[config.Column];
                        }
                        lst.Add(data);
                        count++;
                    }
                }
            }
            return lst;
        }
    }

}
