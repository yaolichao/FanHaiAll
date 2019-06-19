using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.IVTest
{
    /// <summary>
    /// 图片比对窗体类。找出指定文件夹中相同的文件。
    /// </summary>
    public partial class ImageCompare :  BaseUserCtrl
    {
        /// <summary>
        /// 存放文件哈希值的类。
        /// </summary>
        class FileHash
        {
            public string FilePath { get; set; }
            public string HashValue { get; set; }
        }

        /// <summary>
        /// 是否停止比对。
        /// </summary>
        private bool _isStop = false;
        /// <summary>
        /// 表示图片比对视图对象。
        /// </summary>
        private IViewContent _viewContent = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="view"></param>
        public ImageCompare(IViewContent view)
        {
            InitializeComponent();
            this._viewContent = view;
            InitializeLanguage();
        }


        private void InitializeLanguage()
        {
            this.lblApplicationTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.lblApplicationTitle}");//"图片比对";
            this.chkIncludeSubFolder.Properties.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.chkIncludeSubFolder}");//"包含子文件夹";
            this.btnStopCompare.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.btnStopCompare}");//"停止比对";
            this.btnStartCompare.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.btnStartCompare}");//"开始比对";
            this.lciSelectFolder.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.lciSelectFolder}");//"选择文件夹";
            this.lciCompareResult.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.lciCompareResult}");//"比对结果";
            this.lciFilePath.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.lciFilePath}");//"文件路径";
        }




        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageCompare_Load(object sender, EventArgs e)
        {
            this.lblMsg.Text = string.Empty;
            this.btnStartCompare.Enabled = true;
            this.beSelectFolder.Enabled = true;
            this.btnStopCompare.Enabled = false;
            this.lblApplicationTitle.Text = this._viewContent.TitleName;
        }
        /// <summary>
        /// 设置图片文件夹。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beSelectFolder_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            this.dlgFolderBrowser.ShowNewFolderButton = false;
            if (this.dlgFolderBrowser.ShowDialog() == DialogResult.OK)
            {
                this.beSelectFolder.Text = this.dlgFolderBrowser.SelectedPath;
            }
        }
        /// <summary>
        /// 开始比对按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartCompare_Click(object sender, EventArgs e)
        {
            this.tvCompareResult.Nodes.Clear();
            this.picShowImage.Image = null;
            this.teFilePath.Text = string.Empty;
            this._isStop = false;

            string path = this.beSelectFolder.Text;
            //如果path为空，返回
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            if (!Directory.Exists(path))
            {
                MessageBox.Show(string.Format("文件夹{0}不存在或没有权限访问，请确认。", path), "错误");
                return;
            }

            Thread t = new Thread(new ThreadStart(CompareFile));
            t.Start();
        }
        /// <summary>
        /// 比对文件。
        /// </summary>
        public void CompareFile()
        {
            lock (this)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    
                    this.lblMsg.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.lblMsg1}");//"开始比对";
                    this.btnStartCompare.Enabled = false;
                    this.beSelectFolder.Enabled = false;
                    this.btnStopCompare.Enabled = true;
                }));

                try
                {
                    string path = this.beSelectFolder.Text;
                    //如果path为空，返回null
                    if (string.IsNullOrEmpty(path)
                        || !Directory.Exists(path))
                    {
                        return;
                    }

                    IList<FileHash> lstFileHash = new List<FileHash>();
                    string[] searchPatterns = new string[] { "*.jpg", "*.gif", "*.png", "*.bmp" };
                    SearchOption searchOption = this.chkIncludeSubFolder.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                    foreach (string searchPattern in searchPatterns)
                    {
                        string[] files = Directory.GetFiles(path, searchPattern, searchOption);
                        //从path文件夹中获取所有文件,
                        for (int i = 0; i < files.Length; i++)
                        {
                            string file = files[i];
                            this.Invoke(new MethodInvoker(() =>
                            {
                                this.lblMsg.Text = string.Format("计算文件:{0}的哈希值", file);
                                this.btnStartCompare.Enabled = false;
                                this.beSelectFolder.Enabled = false;
                            }));
                            //计算文件Hash值。
                            string hash = ComputeHash(file);

                            lstFileHash.Add(new FileHash()
                            {
                                FilePath = file,
                                HashValue = hash
                            });

                            if (this._isStop == true)
                            {
                                break;
                            }
                        }
                        if (this._isStop == true)
                        {
                            break;
                        }
                    }
                    if (this._isStop == false)
                    {
                        //文件分组，相同Hash值。
                        var qry = from item in lstFileHash
                                  group item by item.HashValue into g
                                  where g.Count() > 1
                                  select g;
                        foreach (IGrouping<string, FileHash> hashs in qry)
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                TreeNode tn = this.tvCompareResult.Nodes.Add(hashs.Key);
                                foreach (FileHash fh in hashs)
                                {
                                    tn.Nodes.Add(fh.FilePath);
                                }
                            }));
                            if (this._isStop == true)
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        MessageBox.Show(ex.Message, "错误");
                    }));
                }
                finally
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        this.lblMsg.Text = string.Empty;
                        this.btnStartCompare.Enabled = true;
                        this.beSelectFolder.Enabled = true;
                        this.btnStopCompare.Enabled = false;
                    }));
                }
            }
        }

        /// <summary>
        /// 计算文件Hash值。
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string ComputeHash(string file)
        {
            StringBuilder sbHash = new StringBuilder();
            Byte[] b = null;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    using (Image img = Image.FromFile(file))
                    {
                        foreach (int propId in img.PropertyIdList)
                        {
                            img.RemovePropertyItem(propId);
                        }
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    b = md5.ComputeHash(ms);
                }
                catch
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        b = md5.ComputeHash(fs);
                        fs.Close();
                    }
                }
            }
            for (int i = 0; i < b.Length; i++)
            {
                sbHash.Append(b[i].ToString("X2"));
            }
            return sbHash.ToString();
        }

        /// <summary>
        /// 停止比对按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopCompare_Click(object sender, EventArgs e)
        {
            this._isStop = true;
        }
        /// <summary>
        /// 鼠标单击节点事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvCompareResult_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //叶子节点
            if (e.Button==MouseButtons.Left
                && e.Node != null 
                && e.Node.Nodes.Count == 0
                && File.Exists(e.Node.Text))
            {
                this.teFilePath.Text = e.Node.Text;
                this.picShowImage.Location = new Point(0, 0); 
                this.picShowImage.Image = Image.FromFile(e.Node.Text);
            }
            else if (e.Button == MouseButtons.Right
                && e.Node!=null
                && e.Node.Nodes.Count>0)
            {
                if (this.cmsTreeView.Items.Count == 0)
                {
                    this.cmsTreeView.Items.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.cmsTreeView.Items1}"), null, (obj, args) =>//导出文件名到文本文件
                    {
                        ExportFileInfo(e.Node, false, false);
                    });
                    this.cmsTreeView.Items.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.cmsTreeView.Items2}"), null, (obj, args) =>//导出全部文件名到文本文件
                    {
                        ExportFileInfo(null, false, false);
                    });
                    this.cmsTreeView.Items.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.cmsTreeView.Items3}"), null, (obj, args) =>//导出文件名到文本文件（含路径）
                    {
                        ExportFileInfo(e.Node, true, false);
                    });
                    this.cmsTreeView.Items.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.cmsTreeView.Items4}"), null, (obj, args) =>//导出全部文件名到文本文件（含路径）
                    {
                        ExportFileInfo(null, true, false);
                    });
                    this.cmsTreeView.Items.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.cmsTreeView.Items5}"), null, (obj, args) =>//导出文件到指定路径
                    {
                        ExportFileInfo(e.Node, false, true);
                    });
                    this.cmsTreeView.Items.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ImageCompare.cmsTreeView.Items6}"), null, (obj, args) =>//导出全部文件到指定路径
                    {
                        ExportFileInfo(null, false, true);
                    });
                }
                Point p1 = new Point(Control.MousePosition.X, Control.MousePosition.Y);
                this.cmsTreeView.Show(this, this.PointToClient(p1));
            }
        }
        /// <summary>
        /// 导出文件
        /// </summary>
        private void ExportFileInfo(TreeNode node,bool isIncludeFilePath,bool isIncludeFileContent)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "文本文件(*.txt)|*.txt";
            dlg.FilterIndex = 0;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.StreamWriter writer = new StreamWriter(dlg.FileName,false, Encoding.UTF8))
                {
                    for (int i = 0; i < this.tvCompareResult.Nodes.Count; i++)
                    {
                        TreeNode n = null;
                        if (node != null)
                        {
                            n = node;
                        }
                        else
                        {
                            n = this.tvCompareResult.Nodes[i];
                            writer.WriteLine(n.Text);
                        }
                        foreach (TreeNode n1 in n.Nodes)
                        {
                            if (isIncludeFilePath)
                            {
                                writer.WriteLine("     {0}:{1}", Path.GetFileNameWithoutExtension(n1.Text), n1.Text);
                            }
                            else
                            {
                                writer.WriteLine("     {0}", Path.GetFileNameWithoutExtension(n1.Text));
                            }
                        }
                        //只导出指定NODE
                        if (n == node)
                        {
                            break;
                        }
                    }
                    writer.Flush();
                    writer.Close();
                }

                if (isIncludeFileContent)
                {
                    for (int i = 0; i < this.tvCompareResult.Nodes.Count; i++)
                    {
                        TreeNode n = null;
                        if (node != null)
                        {
                            n = node;
                        }
                        else
                        {
                            n = this.tvCompareResult.Nodes[i];
                        }
                        string path =Path.Combine(Path.GetDirectoryName(dlg.FileName),n.Text);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        foreach (TreeNode n1 in n.Nodes)
                        {
                            File.Copy(n1.Text, Path.Combine(path, Path.GetFileName(n1.Text)), true);
                        }
                        //只导出指定NODE
                        if (n == node)
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// 鼠标双击节点事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvCompareResult_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
        }
    }
}
