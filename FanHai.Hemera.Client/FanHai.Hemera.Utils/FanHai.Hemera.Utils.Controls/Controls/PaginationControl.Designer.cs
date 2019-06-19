namespace FanHai.Hemera.Utils.Controls
{
    partial class PaginationControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaginationControl));
            this.currentPage = new DevExpress.XtraEditors.SpinEdit();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.titlePage = new DevExpress.XtraEditors.SpinEdit();
            this.btnRefresh1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.everyPageQty = new DevExpress.XtraEditors.SpinEdit();
            this.txtRowQty = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.currentPage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titlePage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.everyPageQty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRowQty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // currentPage
            // 
            this.currentPage.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.currentPage.Location = new System.Drawing.Point(230, 12);
            this.currentPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.currentPage.Name = "currentPage";
            this.currentPage.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.currentPage.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.currentPage.Properties.Appearance.Options.UseFont = true;
            this.currentPage.Properties.Appearance.Options.UseForeColor = true;
            this.currentPage.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.currentPage.Properties.IsFloatValue = false;
            this.currentPage.Properties.Mask.EditMask = "N00";
            this.currentPage.Properties.MaxLength = 5;
            this.currentPage.Properties.MaxValue = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.currentPage.Size = new System.Drawing.Size(62, 28);
            this.currentPage.StyleController = this.layoutControl1;
            this.currentPage.TabIndex = 9;
            this.currentPage.EditValueChanged += new System.EventHandler(this.necPageNo_ValueChanged);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.simpleButton5);
            this.layoutControl1.Controls.Add(this.titlePage);
            this.layoutControl1.Controls.Add(this.btnRefresh1);
            this.layoutControl1.Controls.Add(this.simpleButton4);
            this.layoutControl1.Controls.Add(this.currentPage);
            this.layoutControl1.Controls.Add(this.simpleButton3);
            this.layoutControl1.Controls.Add(this.simpleButton2);
            this.layoutControl1.Controls.Add(this.groupControl1);
            this.layoutControl1.Controls.Add(this.everyPageQty);
            this.layoutControl1.Controls.Add(this.txtRowQty);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControl1.HiddenItems.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(3054, 405, 812, 500);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(632, 91);
            this.layoutControl1.TabIndex = 11;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // simpleButton5
            // 
            this.simpleButton5.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton5.ImageOptions.Image")));
            this.simpleButton5.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.BottomCenter;
            this.simpleButton5.Location = new System.Drawing.Point(126, 12);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(24, 27);
            this.simpleButton5.StyleController = this.layoutControl1;
            this.simpleButton5.TabIndex = 12;
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // titlePage
            // 
            this.titlePage.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.titlePage.Location = new System.Drawing.Point(60, 12);
            this.titlePage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.titlePage.Name = "titlePage";
            this.titlePage.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.titlePage.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.titlePage.Properties.Appearance.Options.UseFont = true;
            this.titlePage.Properties.Appearance.Options.UseForeColor = true;
            this.titlePage.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.titlePage.Properties.IsFloatValue = false;
            this.titlePage.Properties.Mask.EditMask = "N00";
            this.titlePage.Properties.MaxLength = 5;
            this.titlePage.Properties.MaxValue = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.titlePage.Properties.ReadOnly = true;
            this.titlePage.Size = new System.Drawing.Size(62, 28);
            this.titlePage.StyleController = this.layoutControl1;
            this.titlePage.TabIndex = 11;
            // 
            // btnRefresh1
            // 
            this.btnRefresh1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh1.ImageOptions.Image")));
            this.btnRefresh1.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.BottomCenter;
            this.btnRefresh1.Location = new System.Drawing.Point(352, 12);
            this.btnRefresh1.Name = "btnRefresh1";
            this.btnRefresh1.Size = new System.Drawing.Size(24, 27);
            this.btnRefresh1.StyleController = this.layoutControl1;
            this.btnRefresh1.TabIndex = 9;
            this.btnRefresh1.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.simpleButton4.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.ImageOptions.Image")));
            this.simpleButton4.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.BottomCenter;
            this.simpleButton4.Location = new System.Drawing.Point(324, 12);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(24, 27);
            this.simpleButton4.StyleController = this.layoutControl1;
            this.simpleButton4.TabIndex = 8;
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.ImageOptions.Image")));
            this.simpleButton3.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.BottomCenter;
            this.simpleButton3.Location = new System.Drawing.Point(154, 12);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(24, 27);
            this.simpleButton3.StyleController = this.layoutControl1;
            this.simpleButton3.TabIndex = 7;
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.ImageOptions.Image")));
            this.simpleButton2.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.BottomCenter;
            this.simpleButton2.Location = new System.Drawing.Point(296, 12);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(24, 27);
            this.simpleButton2.StyleController = this.layoutControl1;
            this.simpleButton2.TabIndex = 6;
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(655, 28);
            this.groupControl1.TabIndex = 4;
            this.groupControl1.Text = "groupControl1";
            // 
            // everyPageQty
            // 
            this.everyPageQty.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.everyPageQty.Location = new System.Drawing.Point(426, 10);
            this.everyPageQty.Margin = new System.Windows.Forms.Padding(6, 4, 3, 4);
            this.everyPageQty.Name = "everyPageQty";
            this.everyPageQty.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.everyPageQty.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.everyPageQty.Properties.Appearance.Options.UseFont = true;
            this.everyPageQty.Properties.Appearance.Options.UseForeColor = true;
            this.everyPageQty.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.everyPageQty.Properties.IsFloatValue = false;
            this.everyPageQty.Properties.Mask.EditMask = "N00";
            this.everyPageQty.Properties.MaxLength = 5;
            this.everyPageQty.Properties.MaxValue = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.everyPageQty.Size = new System.Drawing.Size(62, 28);
            this.everyPageQty.StyleController = this.layoutControl1;
            this.everyPageQty.TabIndex = 9;
            this.everyPageQty.EditValueChanged += new System.EventHandler(this.necPageSize_ValueChanged);
            // 
            // txtRowQty
            // 
            this.txtRowQty.Location = new System.Drawing.Point(538, 12);
            this.txtRowQty.Name = "txtRowQty";
            this.txtRowQty.Properties.ReadOnly = true;
            this.txtRowQty.Size = new System.Drawing.Size(82, 24);
            this.txtRowQty.StyleController = this.layoutControl1;
            this.txtRowQty.TabIndex = 13;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.groupControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(659, 32);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem3,
            this.layoutControlItem9,
            this.layoutControlItem6,
            this.layoutControlItem10,
            this.layoutControlItem8,
            this.layoutControlItem7,
            this.layoutControlItem2});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(632, 91);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.simpleButton3;
            this.layoutControlItem4.Location = new System.Drawing.Point(142, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(28, 71);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.simpleButton4;
            this.layoutControlItem5.Location = new System.Drawing.Point(312, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(28, 71);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.simpleButton2;
            this.layoutControlItem3.Location = new System.Drawing.Point(284, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(28, 71);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.currentPage;
            this.layoutControlItem9.Location = new System.Drawing.Point(170, 0);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(114, 71);
            this.layoutControlItem9.Text = "当前";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(45, 18);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btnRefresh1;
            this.layoutControlItem6.Location = new System.Drawing.Point(340, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(28, 71);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.everyPageQty;
            this.layoutControlItem10.CustomizationFormText = "共";
            this.layoutControlItem10.Location = new System.Drawing.Point(368, 0);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItem10.Size = new System.Drawing.Size(110, 71);
            this.layoutControlItem10.Text = "每页数";
            this.layoutControlItem10.TextSize = new System.Drawing.Size(45, 18);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.titlePage;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(114, 71);
            this.layoutControlItem8.Text = "总页数";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(45, 18);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.simpleButton5;
            this.layoutControlItem7.Location = new System.Drawing.Point(114, 0);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(28, 71);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtRowQty;
            this.layoutControlItem2.Location = new System.Drawing.Point(478, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(134, 71);
            this.layoutControlItem2.Text = "总数";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(45, 18);
            // 
            // PaginationControl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PaginationControl";
            this.Size = new System.Drawing.Size(632, 91);
            this.Load += new System.EventHandler(this.PaginationControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.currentPage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.titlePage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.everyPageQty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRowQty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SpinEdit currentPage;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SimpleButton btnRefresh1;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraEditors.SpinEdit everyPageQty;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraEditors.SpinEdit titlePage;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraEditors.TextEdit txtRowQty;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    }
}
