namespace FanHai.Hemera.Addins.WIP.Gui
{
    partial class frmOQAQueryDailog
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
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lueFactory = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtWO = new DevExpress.XtraEditors.TextEdit();
            this.txtProID = new DevExpress.XtraEditors.TextEdit();
            this.txtEndSN = new DevExpress.XtraEditors.TextEdit();
            this.txtStartSN = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.chDefult = new DevExpress.XtraEditors.CheckEdit();
            this.chData = new DevExpress.XtraEditors.CheckEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.deStartDate = new DevExpress.XtraEditors.DateEdit();
            this.deEndDate = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactory.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndSN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartSN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chDefult.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.Location = new System.Drawing.Point(97, 132);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(47, 19);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确认";
            this.btnOK.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(10, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 15);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "工厂车间：";
            // 
            // lueFactory
            // 
            this.lueFactory.Location = new System.Drawing.Point(60, 11);
            this.lueFactory.Name = "lueFactory";
            this.lueFactory.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueFactory.Properties.Appearance.Options.UseFont = true;
            this.lueFactory.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueFactory.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LOCATION_KEY", "LOCATION_KEY", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LOCATION_NAME", "LOCATION_NAME")});
            this.lueFactory.Properties.DisplayMember = "LOCATION_NAME";
            this.lueFactory.Properties.ValueMember = "LOCATION_KEY";
            this.lueFactory.Size = new System.Drawing.Size(117, 21);
            this.lueFactory.TabIndex = 2;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(10, 75);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 15);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "工单号：";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(227, 77);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 15);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "产品ID：";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(244, 109);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(7, 15);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "~";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(7, 110);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 15);
            this.labelControl5.TabIndex = 7;
            this.labelControl5.Text = "日期范围";
            // 
            // txtWO
            // 
            this.txtWO.Location = new System.Drawing.Point(60, 75);
            this.txtWO.Margin = new System.Windows.Forms.Padding(2);
            this.txtWO.Name = "txtWO";
            this.txtWO.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWO.Properties.Appearance.Options.UseFont = true;
            this.txtWO.Size = new System.Drawing.Size(156, 21);
            this.txtWO.TabIndex = 8;
            // 
            // txtProID
            // 
            this.txtProID.Location = new System.Drawing.Point(277, 75);
            this.txtProID.Margin = new System.Windows.Forms.Padding(2);
            this.txtProID.Name = "txtProID";
            this.txtProID.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProID.Properties.Appearance.Options.UseFont = true;
            this.txtProID.Size = new System.Drawing.Size(156, 21);
            this.txtProID.TabIndex = 9;
            // 
            // txtEndSN
            // 
            this.txtEndSN.Location = new System.Drawing.Point(277, 42);
            this.txtEndSN.Margin = new System.Windows.Forms.Padding(2);
            this.txtEndSN.Name = "txtEndSN";
            this.txtEndSN.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEndSN.Properties.Appearance.Options.UseFont = true;
            this.txtEndSN.Size = new System.Drawing.Size(156, 21);
            this.txtEndSN.TabIndex = 14;
            // 
            // txtStartSN
            // 
            this.txtStartSN.Location = new System.Drawing.Point(60, 42);
            this.txtStartSN.Margin = new System.Windows.Forms.Padding(2);
            this.txtStartSN.Name = "txtStartSN";
            this.txtStartSN.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStartSN.Properties.Appearance.Options.UseFont = true;
            this.txtStartSN.Size = new System.Drawing.Size(156, 21);
            this.txtStartSN.TabIndex = 13;
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Location = new System.Drawing.Point(10, 44);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(60, 15);
            this.labelControl7.TabIndex = 11;
            this.labelControl7.Text = "组件序号：";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(244, 44);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(7, 15);
            this.labelControl6.TabIndex = 15;
            this.labelControl6.Text = "~";
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Location = new System.Drawing.Point(277, 132);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(47, 19);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // chDefult
            // 
            this.chDefult.Location = new System.Drawing.Point(321, 12);
            this.chDefult.Margin = new System.Windows.Forms.Padding(2);
            this.chDefult.Name = "chDefult";
            this.chDefult.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chDefult.Properties.Appearance.Options.UseFont = true;
            this.chDefult.Properties.Caption = "有效数据";
            this.chDefult.Size = new System.Drawing.Size(72, 20);
            this.chDefult.TabIndex = 17;
            // 
            // chData
            // 
            this.chData.AllowDrop = true;
            this.chData.Location = new System.Drawing.Point(393, 13);
            this.chData.Margin = new System.Windows.Forms.Padding(2);
            this.chData.Name = "chData";
            this.chData.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chData.Properties.Appearance.Options.UseFont = true;
            this.chData.Properties.Caption = "日期";
            this.chData.Size = new System.Drawing.Size(45, 20);
            this.chData.TabIndex = 18;
            // 
            // radioGroup1
            // 
            this.radioGroup1.EditValue = "F";
            this.radioGroup1.Location = new System.Drawing.Point(184, 10);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(2);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioGroup1.Properties.Appearance.Options.UseFont = true;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("F", "组件编号"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("C", "客户编号")});
            this.radioGroup1.Size = new System.Drawing.Size(134, 23);
            this.radioGroup1.TabIndex = 19;
            // 
            // deStartDate
            // 
            this.deStartDate.EditValue = null;
            this.deStartDate.Location = new System.Drawing.Point(61, 106);
            this.deStartDate.Name = "deStartDate";
            this.deStartDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStartDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.deStartDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStartDate.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.deStartDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStartDate.Properties.Mask.EditMask = "";
            this.deStartDate.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.deStartDate.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;
            this.deStartDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deStartDate.Size = new System.Drawing.Size(155, 21);
            this.deStartDate.TabIndex = 20;
            // 
            // deEndDate
            // 
            this.deEndDate.EditValue = null;
            this.deEndDate.Location = new System.Drawing.Point(277, 106);
            this.deEndDate.Name = "deEndDate";
            this.deEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deEndDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.deEndDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEndDate.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.deEndDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEndDate.Properties.Mask.EditMask = "";
            this.deEndDate.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.deEndDate.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;
            this.deEndDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deEndDate.Size = new System.Drawing.Size(156, 21);
            this.deEndDate.TabIndex = 21;
            // 
            // frmOQAQueryDailog
            // 
            this.Appearance.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 160);
            this.Controls.Add(this.deEndDate);
            this.Controls.Add(this.deStartDate);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.chData);
            this.Controls.Add(this.chDefult);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.txtEndSN);
            this.Controls.Add(this.txtStartSN);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.txtProID);
            this.Controls.Add(this.txtWO);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.lueFactory);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnOK);
            this.Name = "frmOQAQueryDailog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OQA查询条件";
            this.Load += new System.EventHandler(this.frmOQAQueryDailog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lueFactory.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndSN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartSN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chDefult.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LookUpEdit lueFactory;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtWO;
        private DevExpress.XtraEditors.TextEdit txtProID;
        private DevExpress.XtraEditors.TextEdit txtEndSN;
        private DevExpress.XtraEditors.TextEdit txtStartSN;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.CheckEdit chDefult;
        private DevExpress.XtraEditors.CheckEdit chData;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.DateEdit deStartDate;
        private DevExpress.XtraEditors.DateEdit deEndDate;
    }
}