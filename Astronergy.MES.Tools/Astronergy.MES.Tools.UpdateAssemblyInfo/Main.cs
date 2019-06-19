// Copyright (c) SolarViewer

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;
using Astronergy.MES.Tools.UpdateAssemblyInfo.Configuration;

// 程序集的版本信息由下面四个值组成:
//
//      主版本
//      次版本 
//      内部版本号
//      修订号
//
// 可以指定所有这些值，也可以使用“内部版本号”和“修订号”的默认值，
// 方法是按如下所示使用“*”:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]

namespace Astronergy.MES.Tools.UpdateAssemblyInfo
{

	// Updates the version numbers in the assembly information.
	class MainClass
	{
        static string BaseCommit;
        static int BaseCommitRev;
        static string fileName;
        static string workDirectory;
        static SourceControlElement srcCtrl;
        static TemplateFileElement globalAssembylInfo;
        static TemplateFileElementCollection templateFiles;
        static string globalAssemblyInfoTemplateFile;
        /// <summary>
        /// 执行程序版本号自动更新。
        /// </summary>
        /// <param name="args">
        /// --workdirectory
        /// --solutionname
        /// --branchname
        /// --REVISION
        /// </param>
        /// <returns></returns>
		public static int Main(string[] args)
		{
			try {
				string exeDir = Path.GetDirectoryName(typeof(MainClass).Assembly.Location);
				bool createdNew;
				using (Mutex mutex = new Mutex(true, "UpdateAssemblyInfo" + exeDir.GetHashCode(), out createdNew)) {
					if (!createdNew) {
						// multiple calls in parallel?
						// it's sufficient to let one call run, so just wait for the other call to finish
						try {
							mutex.WaitOne(10000);
						} catch (AbandonedMutexException) {
						}
						return 0;
					}
                    //获取配置节信息
                    UpdateAssemblyInfoSection section=(UpdateAssemblyInfoSection)
                    System.Configuration.ConfigurationManager.GetSection("astronergy.mes.tools/updateAssemblyInfo");

                    fileName = section.FileName;
                    workDirectory = section.WorkDirectory;
                    srcCtrl = section.SourceControl;
                    globalAssembylInfo = section.GlobalAssemblyInfo;
                    templateFiles = section.TemplateFiles;
                    globalAssemblyInfoTemplateFile = globalAssembylInfo.Input;
                    for (int i = 0; i < args.Length; i++)
                    {
                        if ( args[i] == "--workdirectory" 
                                && i + 1 < args.Length 
                                && !string.IsNullOrEmpty(args[i + 1]))
                            workDirectory = args[i + 1];
                        else if (args[i] == "--filename" 
                                && i + 1 < args.Length 
                                && !string.IsNullOrEmpty(args[i + 1]))
                            fileName = args[i + 1];
                    }
					string mainDir = Path.GetFullPath(workDirectory);
                    if (File.Exists(mainDir + "\\" + fileName))
                    {
						Directory.SetCurrentDirectory(mainDir);
					}
					if (!File.Exists(fileName)) {
                        Console.WriteLine("Working directory must be " + fileName + "!");
						return 2;
					}
                    if (!System.IO.File.Exists(globalAssemblyInfoTemplateFile))
                    {
                        globalAssemblyInfoTemplateFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, globalAssemblyInfoTemplateFile);
                    }

					RetrieveRevisionNumber();
					for (int i = 0; i < args.Length; i++) {
						if (args[i] == "--branchname" && i + 1 < args.Length && !string.IsNullOrEmpty(args[i+1]))
							branchName = args[i + 1];
					}
					UpdateFiles();
					if (args.Contains("--REVISION")) {
						var doc = new XDocument(new XElement(
							"versionInfo",
							new XElement("version", fullVersionNumber),
							new XElement("revision", revisionNumber),
							new XElement("commitHash", commitHash),
							new XElement("branchName", branchName),
							new XElement("versionName", versionName)
						));
						doc.Save("REVISION");
					}
					return 0;
				}
			} catch (Exception ex) {
				Console.WriteLine(ex);
				return 3;
			}
		}
		/// <summary>
		/// 更新所有文件。
		/// </summary>
		static void UpdateFiles()
		{
            UpdateFile(globalAssembylInfo);
			foreach (TemplateFileElement file in templateFiles) {
                UpdateFile(file);
			}
		}
        /// <summary>
        /// 更新文件，被UpdateFiles调用。
        /// </summary>
        /// <param name="file"></param>
        static void UpdateFile(TemplateFileElement file)
        {
            if (!file.IsValided) return;
            string input = file.Input;
            if (!System.IO.File.Exists(input))
            {
                input = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, input);
            }

            string content;
            using (StreamReader r = new StreamReader(input))
            {
                content = r.ReadToEnd();
            }
            //版本
            content = content.Replace("$INSERTVERSION$", fullVersionNumber);
            //主版本
            content = content.Replace("$INSERTMAJORVERSION$", majorVersionNumber);
            //修订版本号
            content = content.Replace("$INSERTREVISION$", revisionNumber);
            //GIT提交HASH码
            content = content.Replace("$INSERTCOMMITHASH$", commitHash);
            content = content.Replace("$INSERTSHORTCOMMITHASH$", commitHash.Length<8?commitHash:commitHash.Substring(0, 8));
            //插入日期
            content = content.Replace("$INSERTDATE$", DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
            //插入年份
            content = content.Replace("$INSERTYEAR$", DateTime.Now.Year.ToString());
            //Git分支名
            content = content.Replace("$INSERTBRANCHNAME$", branchName);
            //是否git默认分支
            bool isDefaultBranch = string.IsNullOrEmpty(branchName) || branchName == "master" || char.IsDigit(branchName, 0);
            //git分支
            content = content.Replace("$INSERTBRANCHPOSTFIX$", isDefaultBranch ? "" : ("-" + branchName));
            //版本名
            content = content.Replace("$INSERTVERSIONNAME$", versionName ?? "");
            //版本名后缀
            content = content.Replace("$INSERTVERSIONNAMEPOSTFIX$", string.IsNullOrEmpty(versionName) ? "" : "-" + versionName);

            if (File.Exists(file.Output))
            {
                using (StreamReader r = new StreamReader(file.Output))
                {
                    if (r.ReadToEnd() == content)
                    {
                        // nothing changed, do not overwrite file to prevent recompilation
                        // every time.
                        return;
                    }
                }
            }
            using (StreamWriter w = new StreamWriter(file.Output, false, Encoding.UTF8))
            {
                w.Write(content);
            }
        }
		/// <summary>
		/// 获取主版本号。
		/// </summary>
		static void GetMajorVersion()
		{
			majorVersionNumber = "?";
			fullVersionNumber = "?";
			versionName = null;
			// Get main version from startup
            using (StreamReader r = new StreamReader(globalAssemblyInfoTemplateFile))
            {
				string line;
				while ((line = r.ReadLine()) != null) {
					string search = "string Major = \"";
					int pos = line.IndexOf(search);
					if (pos >= 0) {
						int e = line.IndexOf('"', pos + search.Length + 1);
						majorVersionNumber = line.Substring(pos + search.Length, e - pos - search.Length);
					}
					search = "string Minor = \"";
					pos = line.IndexOf(search);
					if (pos >= 0) {
						int e = line.IndexOf('"', pos + search.Length + 1);
						majorVersionNumber = majorVersionNumber + "." + line.Substring(pos + search.Length, e - pos - search.Length);
					}
					search = "string Build = \"";
					pos = line.IndexOf(search);
					if (pos >= 0) {
						int e = line.IndexOf('"', pos + search.Length + 1);
						fullVersionNumber = majorVersionNumber + "." + line.Substring(pos + search.Length, e - pos - search.Length) + "." + revisionNumber;
					}
					search = "string VersionName = \"";
					pos = line.IndexOf(search);
					if (pos >= 0) {
						int e = line.IndexOf('"', pos + search.Length + 1);
						versionName = line.Substring(pos + search.Length, e - pos - search.Length);
					}
				}
			}
		}

		#region Retrieve Revision Number
		static string revisionNumber;               //修订版版本号
        static string majorVersionNumber;           //主版本号 从globalAssemblyInfoTemplateFile获取
        static string fullVersionNumber;            //完整版本号 从globalAssemblyInfoTemplateFile获取
		/// <summary>
		/// Descriptive version name, e.g. 'Beta 3'
		/// </summary>
		static string versionName;                  //版本名，
		static string commitHash;                   //提交时版本号
		static string branchName;                   //版本分支名
		/// <summary>
		/// 获取修订版本号、版本分支名、主版本号、完整版本号、版本名等信息。
		/// </summary>
		static void RetrieveRevisionNumber()
		{
			if (revisionNumber == null) {
				if (srcCtrl.Type=="git") {
                    if (Directory.Exists(".git"))
                    {
                        BaseCommit = srcCtrl.Git.BaseCommit;
                        BaseCommitRev = srcCtrl.Git.BaseCommitRev;

                        ReadRevisionNumberFromGit();
                        ReadBranchNameFromGit();
                    }
                    else
                    {
                        Console.WriteLine("There's no git working copy in " + Path.GetFullPath("."));
                    }
				}
                else if (srcCtrl.Type == "svn")
                {
                    //if (Directory.Exists(".svn"))
                    //{
                        ReadRevisionNumberFromSVN();
                        ReadBranchNameFromSVN();
                    //}
                    //else
                    //{
                    //    Console.WriteLine("There's no svn working copy in " + Path.GetFullPath("."));
                    //}
                }
			}
			
			if (revisionNumber == null) {
				ReadRevisionFromFile();
			}
			GetMajorVersion();
		}
		/// <summary>
        /// 如果源代码控制是Git，读取修订版本号。
		/// </summary>
		static void ReadRevisionNumberFromGit()
		{
			ProcessStartInfo info = new ProcessStartInfo("cmd", "/c git rev-list " + BaseCommit + "..HEAD");
			string path = Environment.GetEnvironmentVariable("PATH");
			path += ";" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "git\\bin");
			info.EnvironmentVariables["PATH"] =  path;
			info.RedirectStandardOutput = true;
			info.UseShellExecute = false;
			using (Process p = Process.Start(info)) {
				string line;
				int revNum = BaseCommitRev;
				while ((line = p.StandardOutput.ReadLine()) != null) {
					if (commitHash == null) {
						// first entry is HEAD
						commitHash = line;
					}
					revNum++;
				}
				revisionNumber = revNum.ToString();
				p.WaitForExit();
				if (p.ExitCode != 0)
					throw new Exception("git-rev-list exit code was " + p.ExitCode);
			}
		}
		/// <summary>
        /// 如果源代码控制是Git，读取分支名称。
		/// </summary>
		static void ReadBranchNameFromGit()
		{
			ProcessStartInfo info = new ProcessStartInfo("cmd", "/c git branch --no-color");
			string path = Environment.GetEnvironmentVariable("PATH");
			path += ";" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "git\\bin");
			info.EnvironmentVariables["PATH"] =  path;
			info.RedirectStandardOutput = true;
			info.UseShellExecute = false;
			using (Process p = Process.Start(info)) {
				string line;
				branchName = "(no branch)";
				while ((line = p.StandardOutput.ReadLine()) != null) {
					if (line.StartsWith("* ", StringComparison.Ordinal)) {
						branchName = line.Substring(2);
					}
				}
				p.WaitForExit();
				if (p.ExitCode != 0)
					throw new Exception("git-branch exit code was " + p.ExitCode);
			}
		}
        /// <summary>
        /// 如果源代码控制是SVN，读取修订版本号。
        /// </summary>
        static void ReadRevisionNumberFromSVN()
        {

            string svnCommand = string.Format("/c echo t|svn log -r COMMITTED --no-auth-cache");
            if (!string.IsNullOrEmpty(srcCtrl.SVN.UserName))
            {
                svnCommand += " --username " + srcCtrl.SVN.UserName;
            }
            if (!string.IsNullOrEmpty(srcCtrl.SVN.Password))
            {
                svnCommand += " --password " + srcCtrl.SVN.Password;
            }
            ProcessStartInfo info = new ProcessStartInfo("cmd", svnCommand);

            string path = Environment.GetEnvironmentVariable("PATH");
            if (!string.IsNullOrEmpty(srcCtrl.InstallPath))
            {
                path += ";" + Path.GetFullPath(srcCtrl.InstallPath);
            }
            info.EnvironmentVariables["PATH"] = path;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            using (Process p = Process.Start(info))
            {
                string line;
                while ((line = p.StandardOutput.ReadLine()) != null)
                {
                    if (commitHash == null)
                    {
                        if (line.StartsWith("r", StringComparison.Ordinal))
                        {
                            int len=line.IndexOf('|')-1;
                            commitHash=line.Substring(1, len).Trim();
                            revisionNumber = commitHash;
                            break;
                        }
                    }
                }
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    revisionNumber = "0";
                    commitHash = "0000000000000000000000000000000000000000";
                    //throw new Exception("svn exit code was " + p.ExitCode);
                }
            }
        }
        /// <summary>
        /// 如果源代码控制是SVN，读取分支名称。
        /// </summary>
        static void ReadBranchNameFromSVN()
        {
            branchName = srcCtrl.SVN.BranchName;
        }
        /// <summary>
        /// 从REVISION文件中读取修订版本号，提交版本号，分支版本名称。
        /// </summary>
		static void ReadRevisionFromFile()
		{
			try {
				XDocument doc = XDocument.Load("REVISION");
				revisionNumber = (string)doc.Root.Element("revision");
				commitHash = (string)doc.Root.Element("commitHash");
				branchName = (string)doc.Root.Element("branchName");
			} catch (Exception e) {
				Console.WriteLine(e.Message);
				Console.WriteLine();
				Console.WriteLine("The revision number of the " + fileName + " version being compiled could not be retrieved.");
				Console.WriteLine();
				Console.WriteLine("Build continues with revision number '0'...");
				
				revisionNumber = "0";
				commitHash = "0000000000000000000000000000000000000000";
			}
			if (revisionNumber == null || revisionNumber.Length == 0) {
				revisionNumber = "0";
				//throw new ApplicationException("Error reading revision number");
			}
		}


		#endregion
	}
}
