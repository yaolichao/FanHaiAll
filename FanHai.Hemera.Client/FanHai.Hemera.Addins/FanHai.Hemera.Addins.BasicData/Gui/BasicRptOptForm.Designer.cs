namespace FanHai.Hemera.Addins.BasicData
{
    partial class BasicRptOptForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.cbOperation = new DevExpress.XtraEditors.LookUpEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.chkOverDay = new DevExpress.XtraEditors.CheckEdit();
            this.lueShift = new DevExpress.XtraEditors.LookUpEdit();
            this.lueFactoryRoom = new DevExpress.XtraEditors.LookUpEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.spStartTime = new DevExpress.XtraEditors.SpinEdit();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.spEndTime = new DevExpress.XtraEditors.SpinEdit();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.meoRemark = new DevExpress.XtraEditors.MemoEdit();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbOperation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOverDay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueShift.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactoryRoom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spStartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.meoRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.layoutControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(536, 357);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "车间工序排班";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.meoRemark);
            this.layoutControl1.Controls.Add(this.chkOverDay);
            this.layoutControl1.Controls.Add(this.spEndTime);
            this.layoutControl1.Controls.Add(this.spStartTime);
            this.layoutControl1.Controls.Add(this.cbOperation);
            this.layoutControl1.Controls.Add(this.btnCancel);
            this.layoutControl1.Controls.Add(this.btnOk);
            this.layoutControl1.Controls.Add(this.lueShift);
            this.layoutControl1.Controls.Add(this.lueFactoryRoom);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 23);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(532, 332);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // cbOperation
            // 
            this.cbOperation.Location = new System.Drawing.Point(80, 42);
            this.cbOperation.Name = "cbOperation";
            this.cbOperation.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbOperation.Properties.Appearance.Options.UseFont = true;
            this.cbOperation.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbOperation.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ROUTE_OPERATION_NAME", "工序")});
            this.cbOperation.Properties.NullText = "";
            this.cbOperation.Size = new System.Drawing.Size(440, 26);
            this.cbOperation.StyleController = this.layoutControl1;
            this.cbOperation.TabIndex = 15;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(116, 290);
            this.btnCancel.MaximumSize = new System.Drawing.Size(100, 30);
            this.btnCancel.MinimumSize = new System.Drawing.Size(100, 30);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.StyleController = this.layoutControl1;
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(12, 290);
            this.btnOk.MaximumSize = new System.Drawing.Size(100, 30);
            this.btnOk.MinimumSize = new System.Drawing.Size(100, 30);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 30);
            this.btnOk.StyleController = this.layoutControl1;
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // chkOverDay
            // 
            this.chkOverDay.Location = new System.Drawing.Point(80, 105);
            this.chkOverDay.Margin = new System.Windows.Forms.Padding(10);
            this.chkOverDay.Name = "chkOverDay";
            this.chkOverDay.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOverDay.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.chkOverDay.Properties.Appearance.Options.UseFont = true;
            this.chkOverDay.Properties.Appearance.Options.UseForeColor = true;
            this.chkOverDay.Properties.Caption = "是否跨天";
            this.chkOverDay.Size = new System.Drawing.Size(437, 24);
            this.chkOverDay.StyleController = this.layoutControl1;
            this.chkOverDay.TabIndex = 10;
            // 
            // lueShift
            // 
            this.lueShift.Location = new System.Drawing.Point(80, 72);
            this.lueShift.Name = "lueShift";
            this.lueShift.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueShift.Properties.Appearance.Options.UseFont = true;
            this.lueShift.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueShift.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("SHIFT_NAME", "班别")});
            this.lueShift.Properties.NullText = "";
            this.lueShift.Size = new System.Drawing.Size(440, 26);
            this.lueShift.StyleController = this.layoutControl1;
            this.lueShift.TabIndex = 7;
            // 
            // lueFactoryRoom
            // 
            this.lueFactoryRoom.Location = new System.Drawing.Point(80, 12);
            this.lueFactoryRoom.Name = "lueFactoryRoom";
            this.lueFactoryRoom.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueFactoryRoom.Properties.Appearance.Options.UseFont = true;
            this.lueFactoryRoom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueFactoryRoom.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LOCATION_NAME", "车间")});
            this.lueFactoryRoom.Properties.NullText = "";
            this.lueFactoryRoom.Size = new System.Drawing.Size(440, 26);
            this.lueFactoryRoom.StyleController = this.layoutControl1;
            this.lueFactoryRoom.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem3,
            this.layoutControlItem10,
            this.layoutControlItem11,
            this.layoutControlItem4,
            this.layoutControlItem6});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(532, 332);
            this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.lueFactoryRoom;
            this.layoutControlItem1.CustomizationFormText = "车间名称";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(512, 30);
            this.layoutControlItem1.Text = "车间名称";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(64, 19);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.Control = this.lueShift;
            this.layoutControlItem2.CustomizationFormText = "班别";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 60);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(512, 30);
            this.layoutControlItem2.Text = "班别";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(64, 19);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.chkOverDay;
            this.layoutControlItem6.CustomizationFormText = "layoutControlItem6";
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 90);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Padding = new DevExpress.XtraLayout.Utils.Padding(70, 5, 5, 5);
            this.layoutControlItem6.Size = new System.Drawing.Size(512, 34);
            this.layoutControlItem6.Text = "layoutControlItem6";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextToControlDistance = 0;
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btnOk;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem7";
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 278);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(104, 34);
            this.layoutControlItem7.Text = "layoutControlItem7";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextToControlDistance = 0;
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btnCancel;
            this.layoutControlItem8.CustomizationFormText = "layoutControlItem8";
            this.layoutControlItem8.Location = new System.Drawing.Point(104, 278);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(408, 34);
            this.layoutControlItem8.Text = "layoutControlItem8";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextToControlDistance = 0;
            this.layoutControlItem8.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.Control = this.cbOperation;
            this.layoutControlItem3.CustomizationFormText = "工序";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 30);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(512, 30);
            this.layoutControlItem3.Text = "工序";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(64, 19);
            // 
            // spStartTime
            // 
            this.spStartTime.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spStartTime.Location = new System.Drawing.Point(80, 136);
            this.spStartTime.Name = "spStartTime";
            this.spStartTime.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spStartTime.Properties.Appearance.Options.UseFont = true;
            this.spStartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spStartTime.Properties.DisplayFormat.FormatString = "00:00";
            this.spStartTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spStartTime.Properties.EditFormat.FormatString = "00:00";
            this.spStartTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spStartTime.Properties.Mask.EditMask = "00:00";
            this.spStartTime.Properties.MaxLength = 5;
            this.spStartTime.Properties.NullText = "00:00";
            this.spStartTime.Size = new System.Drawing.Size(440, 26);
            this.spStartTime.StyleController = this.layoutControl1;
            this.spStartTime.TabIndex = 16;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem10.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem10.Control = this.spStartTime;
            this.layoutControlItem10.CustomizationFormText = "开始时间";
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 124);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(512, 30);
            this.layoutControlItem10.Text = "开始时间";
            this.layoutControlItem10.TextSize = new System.Drawing.Size(64, 19);
            // 
            // spEndTime
            // 
            this.spEndTime.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spEndTime.Location = new System.Drawing.Point(80, 166);
            this.spEndTime.Name = "spEndTime";
            this.spEndTime.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spEndTime.Properties.Appearance.Options.UseFont = true;
            this.spEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spEndTime.Properties.DisplayFormat.FormatString = "00:00";
            this.spEndTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spEndTime.Properties.EditFormat.FormatString = "00:00";
            this.spEndTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spEndTime.Properties.Mask.EditMask = "00:00";
            this.spEndTime.Size = new System.Drawing.Size(440, 26);
            this.spEndTime.StyleController = this.layoutControl1;
            this.spEndTime.TabIndex = 17;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem11.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem11.Control = this.spEndTime;
            this.layoutControlItem11.CustomizationFormText = "结束时间";
            this.layoutControlItem11.Location = new System.Drawing.Point(0, 154);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(512, 30);
            this.layoutControlItem11.Text = "结束时间";
            this.layoutControlItem11.TextSize = new System.Drawing.Size(64, 19);
            // 
            // meoRemark
            // 
            this.meoRemark.Location = new System.Drawing.Point(80, 196);
            this.meoRemark.Name = "meoRemark";
            this.meoRemark.Size = new System.Drawing.Size(440, 90);
            this.meoRemark.StyleController = this.layoutControl1;
            this.meoRemark.TabIndex = 18;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem4.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem4.Control = this.meoRemark;
            this.layoutControlItem4.CustomizationFormText = "备注";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 184);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(512, 94);
            this.layoutControlItem4.Text = "备注";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(64, 19);
            // 
            // BasicRptOptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 357);
            this.Controls.Add(this.groupControl1);
            this.Name = "BasicRptOptForm";
            this.Text = "车间工序排班作业设置";
            this.Load += new System.EventHandler(this.BasicRptOptForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbOperation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOverDay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueShift.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactoryRoom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spStartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.meoRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.LookUpEdit lueShift;
        private DevExpress.XtraEditors.LookUpEdit lueFactoryRoom;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.CheckEdit chkOverDay;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.LookUpEdit cbOperation;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SpinEdit spStartTime;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraEditors.SpinEdit spEndTime;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraEditors.MemoEdit meoRemark;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
    }
}