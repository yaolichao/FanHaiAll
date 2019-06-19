namespace SolarViewer.Hemera.Addins.EAP
{
    partial class EDCDetailsCtrl
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
            this.Panel01 = new System.Windows.Forms.Panel();
            this.TLP01 = new System.Windows.Forms.TableLayoutPanel();
            this.lblDeviceInfo = new DevExpress.XtraEditors.LabelControl();
            this.lblLotInfo = new DevExpress.XtraEditors.LabelControl();
            this.Panel011 = new System.Windows.Forms.Panel();
            this.Panel012 = new System.Windows.Forms.Panel();
            this.Panel02 = new System.Windows.Forms.Panel();
            this.lblFormTitle = new DevExpress.XtraEditors.LabelControl();
            this.panBoxDataArea = new System.Windows.Forms.Panel();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnTempStore = new System.Windows.Forms.ToolStripButton();
            this.btnCommit = new System.Windows.Forms.ToolStripButton();
            this.btnColse = new System.Windows.Forms.ToolStripButton();
            this.PanelTitle = new DevExpress.XtraEditors.PanelControl();
            this.Panel01.SuspendLayout();
            this.TLP01.SuspendLayout();
            this.panBoxDataArea.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).BeginInit();
            this.PanelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel01
            // 
            this.Panel01.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel01.Controls.Add(this.TLP01);
            this.Panel01.Location = new System.Drawing.Point(0, 23);
            this.Panel01.Name = "Panel01";
            this.Panel01.Size = new System.Drawing.Size(1092, 170);
            this.Panel01.TabIndex = 6;
            // 
            // TLP01
            // 
            this.TLP01.ColumnCount = 2;
            this.TLP01.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP01.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP01.Controls.Add(this.lblDeviceInfo, 1, 0);
            this.TLP01.Controls.Add(this.lblLotInfo, 0, 0);
            this.TLP01.Controls.Add(this.Panel011, 0, 1);
            this.TLP01.Controls.Add(this.Panel012, 1, 1);
            this.TLP01.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLP01.Location = new System.Drawing.Point(0, 0);
            this.TLP01.Name = "TLP01";
            this.TLP01.RowCount = 2;
            this.TLP01.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TLP01.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP01.Size = new System.Drawing.Size(1092, 170);
            this.TLP01.TabIndex = 0;
            // 
            // lblDeviceInfo
            // 
            this.lblDeviceInfo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDeviceInfo.Appearance.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDeviceInfo.Appearance.Options.UseFont = true;
            this.lblDeviceInfo.Location = new System.Drawing.Point(774, 3);
            this.lblDeviceInfo.Name = "lblDeviceInfo";
            this.lblDeviceInfo.Size = new System.Drawing.Size(90, 15);
            this.lblDeviceInfo.TabIndex = 16;
            this.lblDeviceInfo.Text = "设备配置信息";
            // 
            // lblLotInfo
            // 
            this.lblLotInfo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblLotInfo.Appearance.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLotInfo.Appearance.Options.UseFont = true;
            this.lblLotInfo.Location = new System.Drawing.Point(220, 3);
            this.lblLotInfo.Name = "lblLotInfo";
            this.lblLotInfo.Size = new System.Drawing.Size(105, 15);
            this.lblLotInfo.TabIndex = 15;
            this.lblLotInfo.Text = "流程卡基本信息";
            // 
            // Panel011
            // 
            this.Panel011.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel011.Location = new System.Drawing.Point(3, 23);
            this.Panel011.Name = "Panel011";
            this.Panel011.Size = new System.Drawing.Size(540, 144);
            this.Panel011.TabIndex = 17;
            // 
            // Panel012
            // 
            this.Panel012.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel012.Location = new System.Drawing.Point(549, 23);
            this.Panel012.Name = "Panel012";
            this.Panel012.Size = new System.Drawing.Size(540, 144);
            this.Panel012.TabIndex = 18;
            // 
            // Panel02
            // 
            this.Panel02.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel02.Font = new System.Drawing.Font("Arial", 11F);
            this.Panel02.Location = new System.Drawing.Point(0, 215);
            this.Panel02.Name = "Panel02";
            this.Panel02.Size = new System.Drawing.Size(1092, 170);
            this.Panel02.TabIndex = 8;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Appearance.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFormTitle.Appearance.Options.UseFont = true;
            this.lblFormTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFormTitle.Location = new System.Drawing.Point(8, 8);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(84, 21);
            this.lblFormTitle.TabIndex = 43;
            this.lblFormTitle.Text = "数据采集";
            // 
            // panBoxDataArea
            // 
            this.panBoxDataArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panBoxDataArea.Controls.Add(this.Panel02);
            this.panBoxDataArea.Controls.Add(this.Panel01);
            this.panBoxDataArea.Location = new System.Drawing.Point(3, 73);
            this.panBoxDataArea.Name = "panBoxDataArea";
            this.panBoxDataArea.Size = new System.Drawing.Size(1092, 650);
            this.panBoxDataArea.TabIndex = 95;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.toolStrip, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.PanelTitle, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.panBoxDataArea, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1098, 736);
            this.tableLayoutPanelMain.TabIndex = 96;
            // 
            // toolStrip
            // 
            this.toolStrip.BackgroundImage = global::SolarViewer.Hemera.Addins.EAP.Properties.Resources.toolstrip_bk;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTempStore,
            this.btnCommit,
            this.btnColse});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1098, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnTempStore
            // 
            this.btnTempStore.Image = global::SolarViewer.Hemera.Addins.EAP.Properties.Resources.edit_save;
            this.btnTempStore.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTempStore.Name = "btnTempStore";
            this.btnTempStore.Size = new System.Drawing.Size(52, 22);
            this.btnTempStore.Text = "暂存";
            // 
            // btnCommit
            // 
            this.btnCommit.Image = global::SolarViewer.Hemera.Addins.EAP.Properties.Resources.save_accept;
            this.btnCommit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(52, 22);
            this.btnCommit.Text = "提交";
            // 
            // btnColse
            // 
            this.btnColse.Image = global::SolarViewer.Hemera.Addins.EAP.Properties.Resources.cancel;
            this.btnColse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnColse.Name = "btnColse";
            this.btnColse.Size = new System.Drawing.Size(52, 22);
            this.btnColse.Text = "关闭";
            // 
            // PanelTitle
            // 
            this.PanelTitle.Controls.Add(this.lblFormTitle);
            this.PanelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelTitle.Location = new System.Drawing.Point(3, 28);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Padding = new System.Windows.Forms.Padding(5);
            this.PanelTitle.Size = new System.Drawing.Size(1092, 39);
            this.PanelTitle.TabIndex = 1;
            // 
            // EDCDetailsCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Money Twins";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "EDCDetailsCtrl";
            this.Size = new System.Drawing.Size(1098, 736);
            this.Panel01.ResumeLayout(false);
            this.TLP01.ResumeLayout(false);
            this.TLP01.PerformLayout();
            this.panBoxDataArea.ResumeLayout(false);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel01;
        private System.Windows.Forms.Panel Panel02;
        private DevExpress.XtraEditors.LabelControl lblLotInfo;
        private DevExpress.XtraEditors.LabelControl lblDeviceInfo;
        private DevExpress.XtraEditors.LabelControl lblFormTitle;
        private System.Windows.Forms.Panel panBoxDataArea;
        private System.Windows.Forms.TableLayoutPanel TLP01;
        private System.Windows.Forms.Panel Panel011;
        private System.Windows.Forms.Panel Panel012;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnTempStore;
        private System.Windows.Forms.ToolStripButton btnCommit;
        private System.Windows.Forms.ToolStripButton btnColse;
        private DevExpress.XtraEditors.PanelControl PanelTitle;
    }
}
