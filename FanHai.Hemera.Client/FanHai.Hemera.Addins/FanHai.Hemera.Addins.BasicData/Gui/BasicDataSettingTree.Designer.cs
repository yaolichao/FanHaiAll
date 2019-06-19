namespace FanHai.Hemera.Addins.BasicData
{
    partial class BasicDataSettingTree
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BasicDataSettingTree));
            this.tvBasicSettings = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblTableType = new System.Windows.Forms.Label();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(215, 60);
            this.topPanel.Visible = false;
            // 
            // lblInfos
            // 
            this.lblInfos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.lblInfos.Location = new System.Drawing.Point(-5, 0);
            this.lblInfos.Size = new System.Drawing.Size(220, 54);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t登录时间：2019-01-15 17:15:43";
            // 
            // tvBasicSettings
            // 
            this.tvBasicSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvBasicSettings.ImageIndex = 4;
            this.tvBasicSettings.ImageList = this.imageList1;
            this.tvBasicSettings.Location = new System.Drawing.Point(3, 22);
            this.tvBasicSettings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tvBasicSettings.Name = "tvBasicSettings";
            this.tvBasicSettings.SelectedImageIndex = 2;
            this.tvBasicSettings.Size = new System.Drawing.Size(209, 760);
            this.tvBasicSettings.TabIndex = 0;
            this.tvBasicSettings.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvBasicSettings_MouseUp);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "09984.ico");
            this.imageList1.Images.SetKeyName(1, "01086.ico");
            this.imageList1.Images.SetKeyName(2, "plus_16.png");
            this.imageList1.Images.SetKeyName(3, "ico161.ico");
            this.imageList1.Images.SetKeyName(4, "03485.ico");
            // 
            // lblTableType
            // 
            this.lblTableType.AutoSize = true;
            this.lblTableType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTableType.Location = new System.Drawing.Point(3, 0);
            this.lblTableType.Name = "lblTableType";
            this.lblTableType.Size = new System.Drawing.Size(209, 18);
            this.lblTableType.TabIndex = 1;
            this.lblTableType.Text = "基础数据表类型:";
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Controls.Add(this.lblTableType, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.tvBasicSettings, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(1, 0);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(215, 786);
            this.tableLayoutPanelMain.TabIndex = 2;
            // 
            // BasicDataSettingTree
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "BasicDataSettingTree";
            this.Size = new System.Drawing.Size(217, 786);
            this.Load += new System.EventHandler(this.BasicDataSettingTree_Load);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            this.Controls.SetChildIndex(this.topPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvBasicSettings;
        private System.Windows.Forms.Label lblTableType;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
    }
}
