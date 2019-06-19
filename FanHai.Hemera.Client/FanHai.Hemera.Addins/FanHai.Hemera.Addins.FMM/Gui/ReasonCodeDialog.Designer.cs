namespace FanHai.Hemera.Addins.FMM
{
    partial class ReasonCodeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReasonCodeDialog));
            this.lbxCode = new System.Windows.Forms.ListBox();
            this.lueReasonCodeClass = new DevExpress.XtraEditors.LookUpEdit();
            this.lcRight = new DevExpress.XtraLayout.LayoutControl();
            this.lcgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciReasonCodeClass = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.lbxCategory = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMoveToLeft = new DevExpress.XtraEditors.SimpleButton();
            this.btnMoveToRight = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnColse = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.lueReasonCodeClass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcRight)).BeginInit();
            this.lcRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciReasonCodeClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCode)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbxCode
            // 
            this.lbxCode.DisplayMember = "REASON_CODE_NAME";
            this.lbxCode.FormattingEnabled = true;
            this.lbxCode.IntegralHeight = false;
            this.lbxCode.ItemHeight = 14;
            this.lbxCode.Location = new System.Drawing.Point(2, 26);
            this.lbxCode.Name = "lbxCode";
            this.lbxCode.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxCode.Size = new System.Drawing.Size(208, 357);
            this.lbxCode.TabIndex = 4;
            this.lbxCode.ValueMember = "CATEGORY_KEY";
            // 
            // lueReasonCodeClass
            // 
            this.lueReasonCodeClass.Location = new System.Drawing.Point(2, 2);
            this.lueReasonCodeClass.Name = "lueReasonCodeClass";
            this.lueReasonCodeClass.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueReasonCodeClass.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", " ")});
            this.lueReasonCodeClass.Properties.NullText = "";
            this.lueReasonCodeClass.Size = new System.Drawing.Size(208, 20);
            this.lueReasonCodeClass.StyleController = this.lcRight;
            this.lueReasonCodeClass.TabIndex = 7;
            this.lueReasonCodeClass.EditValueChanged += new System.EventHandler(this.lueReasonCodeClass_EditValueChanged);
            // 
            // lcRight
            // 
            this.lcRight.Controls.Add(this.lueReasonCodeClass);
            this.lcRight.Controls.Add(this.lbxCode);
            this.lcRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcRight.Location = new System.Drawing.Point(311, 3);
            this.lcRight.Name = "lcRight";
            this.lcRight.Root = this.lcgRoot;
            this.lcRight.Size = new System.Drawing.Size(212, 385);
            this.lcRight.TabIndex = 7;
            this.lcRight.Text = "layoutControl1";
            // 
            // lcgRoot
            // 
            this.lcgRoot.CustomizationFormText = "lcgRoot";
            this.lcgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgRoot.GroupBordersVisible = false;
            this.lcgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciReasonCodeClass,
            this.lciCode});
            this.lcgRoot.Name = "lcgRoot";
            this.lcgRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgRoot.Size = new System.Drawing.Size(212, 385);
            this.lcgRoot.TextVisible = false;
            // 
            // lciReasonCodeClass
            // 
            this.lciReasonCodeClass.Control = this.lueReasonCodeClass;
            this.lciReasonCodeClass.CustomizationFormText = "原因代码分类";
            this.lciReasonCodeClass.Location = new System.Drawing.Point(0, 0);
            this.lciReasonCodeClass.Name = "lciReasonCodeClass";
            this.lciReasonCodeClass.Size = new System.Drawing.Size(212, 24);
            this.lciReasonCodeClass.Text = "原因代码分类";
            this.lciReasonCodeClass.TextSize = new System.Drawing.Size(0, 0);
            this.lciReasonCodeClass.TextVisible = false;
            // 
            // lciCode
            // 
            this.lciCode.Control = this.lbxCode;
            this.lciCode.CustomizationFormText = "原因代码";
            this.lciCode.Location = new System.Drawing.Point(0, 24);
            this.lciCode.Name = "lciCode";
            this.lciCode.Size = new System.Drawing.Size(212, 361);
            this.lciCode.Text = "原因代码";
            this.lciCode.TextSize = new System.Drawing.Size(0, 0);
            this.lciCode.TextVisible = false;
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AccessibleName = "新项选择";
            this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("miniToolStrip.BackgroundImage")));
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.miniToolStrip.Location = new System.Drawing.Point(0, 0);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(601, 27);
            this.miniToolStrip.TabIndex = 6;
            // 
            // lbxCategory
            // 
            this.lbxCategory.DisplayMember = "REASON_CODE_NAME";
            this.lbxCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxCategory.FormattingEnabled = true;
            this.lbxCategory.ItemHeight = 14;
            this.lbxCategory.Location = new System.Drawing.Point(3, 3);
            this.lbxCategory.Name = "lbxCategory";
            this.lbxCategory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxCategory.Size = new System.Drawing.Size(212, 385);
            this.lbxCategory.TabIndex = 5;
            this.lbxCategory.ValueMember = "CATEGORY_KEY";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btnMoveToLeft, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnMoveToRight, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnColse, 0, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(221, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(84, 385);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // btnMoveToLeft
            // 
            this.btnMoveToLeft.Location = new System.Drawing.Point(3, 215);
            this.btnMoveToLeft.Name = "btnMoveToLeft";
            this.btnMoveToLeft.Size = new System.Drawing.Size(76, 27);
            this.btnMoveToLeft.TabIndex = 1;
            this.btnMoveToLeft.Text = "<<(&A)";
            this.btnMoveToLeft.Click += new System.EventHandler(this.btnMoveToLeft_Click);
            // 
            // btnMoveToRight
            // 
            this.btnMoveToRight.Location = new System.Drawing.Point(3, 109);
            this.btnMoveToRight.Name = "btnMoveToRight";
            this.btnMoveToRight.Size = new System.Drawing.Size(76, 27);
            this.btnMoveToRight.TabIndex = 0;
            this.btnMoveToRight.Text = ">>(&D)";
            this.btnMoveToRight.Click += new System.EventHandler(this.btnMoveToRight_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 321);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(76, 27);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnColse
            // 
            this.btnColse.Location = new System.Drawing.Point(3, 354);
            this.btnColse.Name = "btnColse";
            this.btnColse.Size = new System.Drawing.Size(76, 27);
            this.btnColse.TabIndex = 8;
            this.btnColse.Text = "关闭";
            this.btnColse.Click += new System.EventHandler(this.btnColse_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbxCategory, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lcRight, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(526, 391);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ReasonCodeDialog
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 391);
            this.Controls.Add(this.tableLayoutPanel1);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ReasonCodeDialog";
            this.Load += new System.EventHandler(this.ReasonCodeDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lueReasonCodeClass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcRight)).EndInit();
            this.lcRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciReasonCodeClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCode)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.SimpleButton btnMoveToLeft;
        private DevExpress.XtraEditors.SimpleButton btnMoveToRight;
        private System.Windows.Forms.ListBox lbxCode;
        private System.Windows.Forms.ListBox lbxCategory;
        private DevExpress.XtraEditors.LookUpEdit lueReasonCodeClass;
        private DevExpress.XtraLayout.LayoutControl lcRight;
        private DevExpress.XtraLayout.LayoutControlGroup lcgRoot;
        private DevExpress.XtraLayout.LayoutControlItem lciReasonCodeClass;
        private DevExpress.XtraLayout.LayoutControlItem lciCode;
        private System.Windows.Forms.ToolStrip miniToolStrip;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnColse;
    }
}