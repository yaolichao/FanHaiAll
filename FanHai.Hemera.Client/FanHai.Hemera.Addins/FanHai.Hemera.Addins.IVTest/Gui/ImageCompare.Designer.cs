namespace FanHai.Hemera.Addins.IVTest
{
    partial class ImageCompare
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.PanelTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblMsg = new DevExpress.XtraEditors.LabelControl();
            this.lblApplicationTitle = new DevExpress.XtraEditors.LabelControl();
            this.Content = new DevExpress.XtraLayout.LayoutControl();
            this.pnlShowImage = new System.Windows.Forms.Panel();
            this.picShowImage = new System.Windows.Forms.PictureBox();
            this.teFilePath = new DevExpress.XtraEditors.TextEdit();
            this.chkIncludeSubFolder = new DevExpress.XtraEditors.CheckEdit();
            this.tvCompareResult = new System.Windows.Forms.TreeView();
            this.btnStopCompare = new DevExpress.XtraEditors.SimpleButton();
            this.btnStartCompare = new DevExpress.XtraEditors.SimpleButton();
            this.beSelectFolder = new DevExpress.XtraEditors.ButtonEdit();
            this.lcgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciSelectFolder = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciStartCompare = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciStopCompare = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciCompareResult = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciIncludeSubFolder = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciFilePath = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciShowImage = new DevExpress.XtraLayout.LayoutControlItem();
            this.dlgFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.cmsTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).BeginInit();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            this.pnlShowImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picShowImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teFilePath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIncludeSubFolder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beSelectFolder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSelectFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStartCompare)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStopCompare)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCompareResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciIncludeSubFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFilePath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciShowImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.toolStripMain, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.PanelTitle, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.Content, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(634, 474);
            this.tableLayoutPanelMain.TabIndex = 62;
            // 
            // toolStripMain
            // 
            this.toolStripMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(634, 25);
            this.toolStripMain.TabIndex = 0;
            // 
            // PanelTitle
            // 
            this.PanelTitle.Controls.Add(this.lblMsg);
            this.PanelTitle.Controls.Add(this.lblApplicationTitle);
            this.PanelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelTitle.Location = new System.Drawing.Point(3, 28);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Padding = new System.Windows.Forms.Padding(5);
            this.PanelTitle.Size = new System.Drawing.Size(628, 39);
            this.PanelTitle.TabIndex = 0;
            // 
            // lblMsg
            // 
            this.lblMsg.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.lblMsg.Appearance.Options.UseForeColor = true;
            this.lblMsg.Location = new System.Drawing.Point(140, 14);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 14);
            this.lblMsg.TabIndex = 42;
            // 
            // lblApplicationTitle
            // 
            this.lblApplicationTitle.Appearance.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblApplicationTitle.Appearance.Options.UseFont = true;
            this.lblApplicationTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblApplicationTitle.Location = new System.Drawing.Point(8, 8);
            this.lblApplicationTitle.Name = "lblApplicationTitle";
            this.lblApplicationTitle.Size = new System.Drawing.Size(84, 21);
            this.lblApplicationTitle.TabIndex = 41;
            this.lblApplicationTitle.Text = "图片比对";
            // 
            // Content
            // 
            this.Content.Controls.Add(this.pnlShowImage);
            this.Content.Controls.Add(this.teFilePath);
            this.Content.Controls.Add(this.chkIncludeSubFolder);
            this.Content.Controls.Add(this.tvCompareResult);
            this.Content.Controls.Add(this.btnStopCompare);
            this.Content.Controls.Add(this.btnStartCompare);
            this.Content.Controls.Add(this.beSelectFolder);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(3, 73);
            this.Content.Name = "Content";
            this.Content.Root = this.lcgRoot;
            this.Content.Size = new System.Drawing.Size(628, 398);
            this.Content.TabIndex = 1;
            this.Content.Text = "layoutControl1";
            // 
            // pnlShowImage
            // 
            this.pnlShowImage.AutoScroll = true;
            this.pnlShowImage.Controls.Add(this.picShowImage);
            this.pnlShowImage.Location = new System.Drawing.Point(170, 55);
            this.pnlShowImage.Name = "pnlShowImage";
            this.pnlShowImage.Size = new System.Drawing.Size(454, 339);
            this.pnlShowImage.TabIndex = 11;
            // 
            // picShowImage
            // 
            this.picShowImage.Location = new System.Drawing.Point(18, 25);
            this.picShowImage.Name = "picShowImage";
            this.picShowImage.Size = new System.Drawing.Size(100, 50);
            this.picShowImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picShowImage.TabIndex = 0;
            this.picShowImage.TabStop = false;
            // 
            // teFilePath
            // 
            this.teFilePath.Location = new System.Drawing.Point(234, 30);
            this.teFilePath.Name = "teFilePath";
            this.teFilePath.Properties.ReadOnly = true;
            this.teFilePath.Size = new System.Drawing.Size(390, 21);
            this.teFilePath.StyleController = this.Content;
            this.teFilePath.TabIndex = 10;
            // 
            // chkIncludeSubFolder
            // 
            this.chkIncludeSubFolder.Location = new System.Drawing.Point(333, 4);
            this.chkIncludeSubFolder.Name = "chkIncludeSubFolder";
            this.chkIncludeSubFolder.Properties.Caption = "包含子文件夹";
            this.chkIncludeSubFolder.Size = new System.Drawing.Size(123, 19);
            this.chkIncludeSubFolder.StyleController = this.Content;
            this.chkIncludeSubFolder.TabIndex = 9;
            // 
            // tvCompareResult
            // 
            this.tvCompareResult.Location = new System.Drawing.Point(4, 30);
            this.tvCompareResult.Name = "tvCompareResult";
            this.tvCompareResult.Size = new System.Drawing.Size(162, 364);
            this.tvCompareResult.TabIndex = 7;
            this.tvCompareResult.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvCompareResult_NodeMouseDoubleClick);
            this.tvCompareResult.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvCompareResult_NodeMouseClick);
            // 
            // btnStopCompare
            // 
            this.btnStopCompare.Location = new System.Drawing.Point(544, 4);
            this.btnStopCompare.Name = "btnStopCompare";
            this.btnStopCompare.Size = new System.Drawing.Size(80, 22);
            this.btnStopCompare.StyleController = this.Content;
            this.btnStopCompare.TabIndex = 6;
            this.btnStopCompare.Text = "停止比对";
            this.btnStopCompare.Click += new System.EventHandler(this.btnStopCompare_Click);
            // 
            // btnStartCompare
            // 
            this.btnStartCompare.Location = new System.Drawing.Point(460, 4);
            this.btnStartCompare.Name = "btnStartCompare";
            this.btnStartCompare.Size = new System.Drawing.Size(80, 22);
            this.btnStartCompare.StyleController = this.Content;
            this.btnStartCompare.TabIndex = 5;
            this.btnStartCompare.Text = "开始比对";
            this.btnStartCompare.Click += new System.EventHandler(this.btnStartCompare_Click);
            // 
            // beSelectFolder
            // 
            this.beSelectFolder.Location = new System.Drawing.Point(68, 4);
            this.beSelectFolder.Name = "beSelectFolder";
            this.beSelectFolder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beSelectFolder.Size = new System.Drawing.Size(261, 21);
            this.beSelectFolder.StyleController = this.Content;
            this.beSelectFolder.TabIndex = 4;
            this.beSelectFolder.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beSelectFolder_ButtonClick);
            // 
            // lcgRoot
            // 
            this.lcgRoot.CustomizationFormText = "lcgRoot";
            this.lcgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgRoot.GroupBordersVisible = false;
            this.lcgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciSelectFolder,
            this.lciStartCompare,
            this.lciStopCompare,
            this.lciCompareResult,
            this.lciIncludeSubFolder,
            this.lciFilePath,
            this.lciShowImage});
            this.lcgRoot.Location = new System.Drawing.Point(0, 0);
            this.lcgRoot.Name = "lcgRoot";
            this.lcgRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.lcgRoot.Size = new System.Drawing.Size(628, 398);
            this.lcgRoot.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgRoot.Text = "lcgRoot";
            this.lcgRoot.TextVisible = false;
            // 
            // lciSelectFolder
            // 
            this.lciSelectFolder.Control = this.beSelectFolder;
            this.lciSelectFolder.CustomizationFormText = "选择文件夹";
            this.lciSelectFolder.Location = new System.Drawing.Point(0, 0);
            this.lciSelectFolder.Name = "lciSelectFolder";
            this.lciSelectFolder.Size = new System.Drawing.Size(329, 26);
            this.lciSelectFolder.Text = "选择文件夹";
            this.lciSelectFolder.TextSize = new System.Drawing.Size(60, 14);
            // 
            // lciStartCompare
            // 
            this.lciStartCompare.Control = this.btnStartCompare;
            this.lciStartCompare.CustomizationFormText = "StartCompare";
            this.lciStartCompare.Location = new System.Drawing.Point(456, 0);
            this.lciStartCompare.MaxSize = new System.Drawing.Size(84, 26);
            this.lciStartCompare.MinSize = new System.Drawing.Size(84, 26);
            this.lciStartCompare.Name = "lciStartCompare";
            this.lciStartCompare.Size = new System.Drawing.Size(84, 26);
            this.lciStartCompare.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciStartCompare.Text = "StartCompare";
            this.lciStartCompare.TextSize = new System.Drawing.Size(0, 0);
            this.lciStartCompare.TextToControlDistance = 0;
            this.lciStartCompare.TextVisible = false;
            // 
            // lciStopCompare
            // 
            this.lciStopCompare.Control = this.btnStopCompare;
            this.lciStopCompare.CustomizationFormText = "StopCompare";
            this.lciStopCompare.Location = new System.Drawing.Point(540, 0);
            this.lciStopCompare.MaxSize = new System.Drawing.Size(84, 26);
            this.lciStopCompare.MinSize = new System.Drawing.Size(84, 26);
            this.lciStopCompare.Name = "lciStopCompare";
            this.lciStopCompare.Size = new System.Drawing.Size(84, 26);
            this.lciStopCompare.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciStopCompare.Text = "StopCompare";
            this.lciStopCompare.TextSize = new System.Drawing.Size(0, 0);
            this.lciStopCompare.TextToControlDistance = 0;
            this.lciStopCompare.TextVisible = false;
            // 
            // lciCompareResult
            // 
            this.lciCompareResult.Control = this.tvCompareResult;
            this.lciCompareResult.CustomizationFormText = "比对结果";
            this.lciCompareResult.Location = new System.Drawing.Point(0, 26);
            this.lciCompareResult.Name = "lciCompareResult";
            this.lciCompareResult.Size = new System.Drawing.Size(166, 368);
            this.lciCompareResult.Text = "比对结果";
            this.lciCompareResult.TextSize = new System.Drawing.Size(0, 0);
            this.lciCompareResult.TextToControlDistance = 0;
            this.lciCompareResult.TextVisible = false;
            // 
            // lciIncludeSubFolder
            // 
            this.lciIncludeSubFolder.Control = this.chkIncludeSubFolder;
            this.lciIncludeSubFolder.CustomizationFormText = "IncludeSubFolder";
            this.lciIncludeSubFolder.Location = new System.Drawing.Point(329, 0);
            this.lciIncludeSubFolder.MaxSize = new System.Drawing.Size(127, 0);
            this.lciIncludeSubFolder.MinSize = new System.Drawing.Size(127, 23);
            this.lciIncludeSubFolder.Name = "lciIncludeSubFolder";
            this.lciIncludeSubFolder.Size = new System.Drawing.Size(127, 26);
            this.lciIncludeSubFolder.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciIncludeSubFolder.Text = "IncludeSubFolder";
            this.lciIncludeSubFolder.TextSize = new System.Drawing.Size(0, 0);
            this.lciIncludeSubFolder.TextToControlDistance = 0;
            this.lciIncludeSubFolder.TextVisible = false;
            // 
            // lciFilePath
            // 
            this.lciFilePath.Control = this.teFilePath;
            this.lciFilePath.CustomizationFormText = "文件路径";
            this.lciFilePath.Location = new System.Drawing.Point(166, 26);
            this.lciFilePath.Name = "lciFilePath";
            this.lciFilePath.Size = new System.Drawing.Size(458, 25);
            this.lciFilePath.Text = "文件路径";
            this.lciFilePath.TextSize = new System.Drawing.Size(60, 14);
            // 
            // lciShowImage
            // 
            this.lciShowImage.Control = this.pnlShowImage;
            this.lciShowImage.CustomizationFormText = "ShowImage";
            this.lciShowImage.Location = new System.Drawing.Point(166, 51);
            this.lciShowImage.Name = "lciShowImage";
            this.lciShowImage.Size = new System.Drawing.Size(458, 343);
            this.lciShowImage.Text = "ShowImage";
            this.lciShowImage.TextSize = new System.Drawing.Size(0, 0);
            this.lciShowImage.TextToControlDistance = 0;
            this.lciShowImage.TextVisible = false;
            // 
            // dlgFolderBrowser
            // 
            this.dlgFolderBrowser.ShowNewFolderButton = false;
            // 
            // cmsTreeView
            // 
            this.cmsTreeView.Name = "cmsTreeView";
            this.cmsTreeView.Size = new System.Drawing.Size(153, 26);
            // 
            // ImageCompare
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Name = "ImageCompare";
            this.Size = new System.Drawing.Size(634, 474);
            this.Load += new System.EventHandler(this.ImageCompare_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            this.pnlShowImage.ResumeLayout(false);
            this.pnlShowImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picShowImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teFilePath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIncludeSubFolder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beSelectFolder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSelectFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStartCompare)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStopCompare)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCompareResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciIncludeSubFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFilePath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciShowImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private DevExpress.XtraEditors.PanelControl PanelTitle;
        private DevExpress.XtraEditors.LabelControl lblApplicationTitle;
        private DevExpress.XtraLayout.LayoutControl Content;
        private DevExpress.XtraLayout.LayoutControlGroup lcgRoot;
        private DevExpress.XtraEditors.ButtonEdit beSelectFolder;
        private DevExpress.XtraLayout.LayoutControlItem lciSelectFolder;
        private DevExpress.XtraEditors.SimpleButton btnStopCompare;
        private DevExpress.XtraEditors.SimpleButton btnStartCompare;
        private DevExpress.XtraLayout.LayoutControlItem lciStartCompare;
        private DevExpress.XtraLayout.LayoutControlItem lciStopCompare;
        private System.Windows.Forms.TreeView tvCompareResult;
        private DevExpress.XtraLayout.LayoutControlItem lciCompareResult;
        private DevExpress.XtraEditors.CheckEdit chkIncludeSubFolder;
        private DevExpress.XtraLayout.LayoutControlItem lciIncludeSubFolder;
        private System.Windows.Forms.FolderBrowserDialog dlgFolderBrowser;
        private DevExpress.XtraEditors.LabelControl lblMsg;
        private DevExpress.XtraEditors.TextEdit teFilePath;
        private DevExpress.XtraLayout.LayoutControlItem lciFilePath;
        private System.Windows.Forms.Panel pnlShowImage;
        private DevExpress.XtraLayout.LayoutControlItem lciShowImage;
        private System.Windows.Forms.PictureBox picShowImage;
        private System.Windows.Forms.ContextMenuStrip cmsTreeView;
    }
}
