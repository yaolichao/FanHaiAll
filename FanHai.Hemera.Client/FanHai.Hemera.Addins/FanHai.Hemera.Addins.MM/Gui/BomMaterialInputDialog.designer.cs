namespace FanHai.Hemera.Addins.MM
{
    partial class BomMaterialInputDialog
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
            this.Content = new DevExpress.XtraLayout.LayoutControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.txtMaterialDescription = new DevExpress.XtraEditors.TextEdit();
            this.teOrderNumber = new DevExpress.XtraEditors.TextEdit();
            this.cmbMaterialCode = new DevExpress.XtraEditors.ComboBoxEdit();
            this.root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciOk = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciCancel = new DevExpress.XtraLayout.LayoutControlItem();
            this.esiPrefix = new DevExpress.XtraLayout.EmptySpaceItem();
            this.esiSuffix = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lciOrderNumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciMaterialNumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciMaterialDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.esiMidlle = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterialDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teOrderNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMaterialCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiPrefix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiSuffix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOrderNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciMaterialNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciMaterialDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiMidlle)).BeginInit();
            this.SuspendLayout();
            // 
            // Content
            // 
            this.Content.Controls.Add(this.btnCancel);
            this.Content.Controls.Add(this.btnOK);
            this.Content.Controls.Add(this.txtMaterialDescription);
            this.Content.Controls.Add(this.teOrderNumber);
            this.Content.Controls.Add(this.cmbMaterialCode);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Content.Location = new System.Drawing.Point(0, 0);
            this.Content.Name = "Content";
            this.Content.Root = this.root;
            this.Content.Size = new System.Drawing.Size(383, 151);
            this.Content.TabIndex = 0;
            this.Content.Text = "layoutControl1";
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Location = new System.Drawing.Point(182, 122);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 22);
            this.btnCancel.StyleController = this.Content;
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.Location = new System.Drawing.Point(93, 122);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(85, 22);
            this.btnOK.StyleController = this.Content;
            this.btnOK.TabIndex = 18;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtMaterialDescription
            // 
            this.txtMaterialDescription.Location = new System.Drawing.Point(91, 67);
            this.txtMaterialDescription.Name = "txtMaterialDescription";
            this.txtMaterialDescription.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtMaterialDescription.Properties.Appearance.Options.UseFont = true;
            this.txtMaterialDescription.Properties.MaxLength = 150;
            this.txtMaterialDescription.Size = new System.Drawing.Size(285, 26);
            this.txtMaterialDescription.StyleController = this.Content;
            this.txtMaterialDescription.TabIndex = 6;
            // 
            // teOrderNumber
            // 
            this.teOrderNumber.Location = new System.Drawing.Point(91, 7);
            this.teOrderNumber.Name = "teOrderNumber";
            this.teOrderNumber.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.teOrderNumber.Properties.Appearance.Options.UseFont = true;
            this.teOrderNumber.Size = new System.Drawing.Size(285, 26);
            this.teOrderNumber.StyleController = this.Content;
            this.teOrderNumber.TabIndex = 7;
            this.teOrderNumber.TabStop = false;
            // 
            // cmbMaterialCode
            // 
            this.cmbMaterialCode.Location = new System.Drawing.Point(91, 37);
            this.cmbMaterialCode.Name = "cmbMaterialCode";
            this.cmbMaterialCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.cmbMaterialCode.Properties.Appearance.Options.UseFont = true;
            this.cmbMaterialCode.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 12F);
            this.cmbMaterialCode.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cmbMaterialCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbMaterialCode.Properties.MaxLength = 10;
            this.cmbMaterialCode.Size = new System.Drawing.Size(285, 26);
            this.cmbMaterialCode.StyleController = this.Content;
            this.cmbMaterialCode.TabIndex = 11;
            this.cmbMaterialCode.TabStop = false;
            this.cmbMaterialCode.EditValueChanged += new System.EventHandler(this.cmbMaterialCode_EditValueChanged);
            // 
            // root
            // 
            this.root.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.root.AppearanceItemCaption.Options.UseFont = true;
            this.root.CustomizationFormText = "root";
            this.root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.root.GroupBordersVisible = false;
            this.root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciOk,
            this.lciCancel,
            this.esiPrefix,
            this.esiSuffix,
            this.lciOrderNumber,
            this.lciMaterialNumber,
            this.lciMaterialDescription,
            this.esiMidlle});
            this.root.Location = new System.Drawing.Point(0, 0);
            this.root.Name = "root";
            this.root.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.root.Size = new System.Drawing.Size(383, 151);
            this.root.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.root.Text = "root";
            this.root.TextVisible = false;
            // 
            // lciOk
            // 
            this.lciOk.Control = this.btnOK;
            this.lciOk.CustomizationFormText = "确定";
            this.lciOk.Location = new System.Drawing.Point(86, 115);
            this.lciOk.MaxSize = new System.Drawing.Size(89, 26);
            this.lciOk.MinSize = new System.Drawing.Size(89, 26);
            this.lciOk.Name = "lciOk";
            this.lciOk.Size = new System.Drawing.Size(89, 26);
            this.lciOk.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciOk.Text = "确定";
            this.lciOk.TextSize = new System.Drawing.Size(0, 0);
            this.lciOk.TextToControlDistance = 0;
            this.lciOk.TextVisible = false;
            // 
            // lciCancel
            // 
            this.lciCancel.Control = this.btnCancel;
            this.lciCancel.CustomizationFormText = "取消";
            this.lciCancel.Location = new System.Drawing.Point(175, 115);
            this.lciCancel.MaxSize = new System.Drawing.Size(86, 26);
            this.lciCancel.MinSize = new System.Drawing.Size(86, 26);
            this.lciCancel.Name = "lciCancel";
            this.lciCancel.Size = new System.Drawing.Size(86, 26);
            this.lciCancel.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciCancel.Text = "取消";
            this.lciCancel.TextSize = new System.Drawing.Size(0, 0);
            this.lciCancel.TextToControlDistance = 0;
            this.lciCancel.TextVisible = false;
            // 
            // esiPrefix
            // 
            this.esiPrefix.CustomizationFormText = "emptySpaceItem3";
            this.esiPrefix.Location = new System.Drawing.Point(0, 115);
            this.esiPrefix.Name = "esiPrefix";
            this.esiPrefix.Size = new System.Drawing.Size(86, 26);
            this.esiPrefix.Text = "esiPrefix";
            this.esiPrefix.TextSize = new System.Drawing.Size(0, 0);
            // 
            // esiSuffix
            // 
            this.esiSuffix.CustomizationFormText = "emptySpaceItem4";
            this.esiSuffix.Location = new System.Drawing.Point(261, 115);
            this.esiSuffix.Name = "esiSuffix";
            this.esiSuffix.Size = new System.Drawing.Size(112, 26);
            this.esiSuffix.Text = "esiSuffix";
            this.esiSuffix.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lciOrderNumber
            // 
            this.lciOrderNumber.Control = this.teOrderNumber;
            this.lciOrderNumber.CustomizationFormText = "工单号";
            this.lciOrderNumber.Location = new System.Drawing.Point(0, 0);
            this.lciOrderNumber.Name = "lciOrderNumber";
            this.lciOrderNumber.Size = new System.Drawing.Size(373, 30);
            this.lciOrderNumber.Text = "工单号";
            this.lciOrderNumber.TextSize = new System.Drawing.Size(80, 19);
            // 
            // lciMaterialNumber
            // 
            this.lciMaterialNumber.Control = this.cmbMaterialCode;
            this.lciMaterialNumber.CustomizationFormText = "原材料编码";
            this.lciMaterialNumber.Location = new System.Drawing.Point(0, 30);
            this.lciMaterialNumber.Name = "lciMaterialNumber";
            this.lciMaterialNumber.Size = new System.Drawing.Size(373, 30);
            this.lciMaterialNumber.Text = "原材料编码";
            this.lciMaterialNumber.TextSize = new System.Drawing.Size(80, 19);
            // 
            // lciMaterialDescription
            // 
            this.lciMaterialDescription.Control = this.txtMaterialDescription;
            this.lciMaterialDescription.CustomizationFormText = "原材料描述";
            this.lciMaterialDescription.Location = new System.Drawing.Point(0, 60);
            this.lciMaterialDescription.Name = "lciMaterialDescription";
            this.lciMaterialDescription.Size = new System.Drawing.Size(373, 30);
            this.lciMaterialDescription.Text = "原材料描述";
            this.lciMaterialDescription.TextSize = new System.Drawing.Size(80, 19);
            // 
            // esiMidlle
            // 
            this.esiMidlle.CustomizationFormText = "emptySpaceItem1";
            this.esiMidlle.Location = new System.Drawing.Point(0, 90);
            this.esiMidlle.MaxSize = new System.Drawing.Size(49, 0);
            this.esiMidlle.MinSize = new System.Drawing.Size(49, 10);
            this.esiMidlle.Name = "esiMidlle";
            this.esiMidlle.Size = new System.Drawing.Size(373, 25);
            this.esiMidlle.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.esiMidlle.Text = "esiMidlle";
            this.esiMidlle.TextSize = new System.Drawing.Size(0, 0);
            // 
            // BomMaterialInputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 151);
            this.Controls.Add(this.Content);
            this.Name = "BomMaterialInputDialog";
            this.Text = "工单自备料";
            this.Load += new System.EventHandler(this.BomMaterialInputDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterialDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teOrderNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMaterialCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiPrefix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiSuffix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOrderNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciMaterialNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciMaterialDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiMidlle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl Content;
        private DevExpress.XtraLayout.LayoutControlGroup root;
        private DevExpress.XtraEditors.TextEdit txtMaterialDescription;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraLayout.LayoutControlItem lciOk;
        private DevExpress.XtraLayout.LayoutControlItem lciCancel;
        private DevExpress.XtraLayout.EmptySpaceItem esiMidlle;
        private DevExpress.XtraLayout.EmptySpaceItem esiPrefix;
        private DevExpress.XtraLayout.EmptySpaceItem esiSuffix;
        private DevExpress.XtraEditors.TextEdit teOrderNumber;
        private DevExpress.XtraEditors.ComboBoxEdit cmbMaterialCode;
        private DevExpress.XtraLayout.LayoutControlItem lciOrderNumber;
        private DevExpress.XtraLayout.LayoutControlItem lciMaterialNumber;
        private DevExpress.XtraLayout.LayoutControlItem lciMaterialDescription;
    }
}