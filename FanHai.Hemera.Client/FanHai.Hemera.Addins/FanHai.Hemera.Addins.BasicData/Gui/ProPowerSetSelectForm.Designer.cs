namespace FanHai.Hemera.Addins.BasicData
{
    partial class ProPowerSetSelectForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.gcProPowerSet = new DevExpress.XtraGrid.GridControl();
            this.gvProPowerSet = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gdProPowerSetCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdProPowerSetRule = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdPowerMin = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdPowerMax = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdSubPowerSetWay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gdProPowerSet_key = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.sbtnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.teProPowerSetCode = new DevExpress.XtraEditors.TextEdit();
            this.teProPowerSetRule = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciProPowerSetRule = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciProPowerSetCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcProPowerSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProPowerSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teProPowerSetCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teProPowerSetRule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciProPowerSetRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciProPowerSetCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            this.layoutControl2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.layoutControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.layoutControl2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.38356F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.61644F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(601, 438);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.gcProPowerSet);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(3, 59);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(595, 328);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "明细信息";
            // 
            // gcProPowerSet
            // 
            this.gcProPowerSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcProPowerSet.Location = new System.Drawing.Point(2, 23);
            this.gcProPowerSet.MainView = this.gvProPowerSet;
            this.gcProPowerSet.Name = "gcProPowerSet";
            this.gcProPowerSet.Size = new System.Drawing.Size(591, 303);
            this.gcProPowerSet.TabIndex = 0;
            this.gcProPowerSet.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvProPowerSet});
            // 
            // gvProPowerSet
            // 
            this.gvProPowerSet.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gdProPowerSetCode,
            this.gdProPowerSetRule,
            this.gdPowerMin,
            this.gdPowerMax,
            this.gdSubPowerSetWay,
            this.gdProPowerSet_key});
            this.gvProPowerSet.GridControl = this.gcProPowerSet;
            this.gvProPowerSet.Name = "gvProPowerSet";
            this.gvProPowerSet.OptionsBehavior.Editable = false;
            this.gvProPowerSet.OptionsView.ShowGroupPanel = false;
            this.gvProPowerSet.DoubleClick += new System.EventHandler(this.gvProPowerSet_DoubleClick);
            // 
            // gdProPowerSetCode
            // 
            this.gdProPowerSetCode.Caption = "分档代码";
            this.gdProPowerSetCode.FieldName = "PS_CODE";
            this.gdProPowerSetCode.Name = "gdProPowerSetCode";
            this.gdProPowerSetCode.Visible = true;
            this.gdProPowerSetCode.VisibleIndex = 0;
            // 
            // gdProPowerSetRule
            // 
            this.gdProPowerSetRule.Caption = "分档规则";
            this.gdProPowerSetRule.FieldName = "PS_RULE";
            this.gdProPowerSetRule.Name = "gdProPowerSetRule";
            this.gdProPowerSetRule.Visible = true;
            this.gdProPowerSetRule.VisibleIndex = 1;
            // 
            // gdPowerMin
            // 
            this.gdPowerMin.Caption = "功率下限";
            this.gdPowerMin.FieldName = "P_MIN";
            this.gdPowerMin.Name = "gdPowerMin";
            this.gdPowerMin.Visible = true;
            this.gdPowerMin.VisibleIndex = 2;
            // 
            // gdPowerMax
            // 
            this.gdPowerMax.Caption = "功率上限";
            this.gdPowerMax.FieldName = "P_MAX";
            this.gdPowerMax.Name = "gdPowerMax";
            this.gdPowerMax.Visible = true;
            this.gdPowerMax.VisibleIndex = 3;
            // 
            // gdSubPowerSetWay
            // 
            this.gdSubPowerSetWay.Caption = "子分档方式";
            this.gdSubPowerSetWay.FieldName = "SUB_PS_WAY";
            this.gdSubPowerSetWay.Name = "gdSubPowerSetWay";
            this.gdSubPowerSetWay.Visible = true;
            this.gdSubPowerSetWay.VisibleIndex = 4;
            // 
            // gdProPowerSet_key
            // 
            this.gdProPowerSet_key.Caption = "功率分档Key";
            this.gdProPowerSet_key.FieldName = "POWERSET_KEY";
            this.gdProPowerSet_key.Name = "gdProPowerSet_key";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.sbtnQuery);
            this.layoutControl1.Controls.Add(this.teProPowerSetCode);
            this.layoutControl1.Controls.Add(this.teProPowerSetRule);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(3, 3);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(595, 50);
            this.layoutControl1.TabIndex = 1;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // sbtnQuery
            // 
            this.sbtnQuery.Location = new System.Drawing.Point(488, 12);
            this.sbtnQuery.MaximumSize = new System.Drawing.Size(100, 30);
            this.sbtnQuery.MinimumSize = new System.Drawing.Size(100, 30);
            this.sbtnQuery.Name = "sbtnQuery";
            this.sbtnQuery.Size = new System.Drawing.Size(100, 30);
            this.sbtnQuery.StyleController = this.layoutControl1;
            this.sbtnQuery.TabIndex = 6;
            this.sbtnQuery.Text = "查询";
            this.sbtnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // teProPowerSetCode
            // 
            this.teProPowerSetCode.Location = new System.Drawing.Point(67, 12);
            this.teProPowerSetCode.Name = "teProPowerSetCode";
            this.teProPowerSetCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teProPowerSetCode.Properties.Appearance.Options.UseFont = true;
            this.teProPowerSetCode.Size = new System.Drawing.Size(164, 23);
            this.teProPowerSetCode.StyleController = this.layoutControl1;
            this.teProPowerSetCode.TabIndex = 5;
            // 
            // teProPowerSetRule
            // 
            this.teProPowerSetRule.Location = new System.Drawing.Point(295, 12);
            this.teProPowerSetRule.Name = "teProPowerSetRule";
            this.teProPowerSetRule.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teProPowerSetRule.Properties.Appearance.Options.UseFont = true;
            this.teProPowerSetRule.Size = new System.Drawing.Size(189, 23);
            this.teProPowerSetRule.StyleController = this.layoutControl1;
            this.teProPowerSetRule.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciProPowerSetRule,
            this.lciProPowerSetCode,
            this.layoutControlItem3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 10, 5);
            this.layoutControlGroup1.Size = new System.Drawing.Size(595, 50);
            this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // lciProPowerSetRule
            // 
            this.lciProPowerSetRule.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lciProPowerSetRule.AppearanceItemCaption.Options.UseFont = true;
            this.lciProPowerSetRule.Control = this.teProPowerSetRule;
            this.lciProPowerSetRule.CustomizationFormText = "分档规则";
            this.lciProPowerSetRule.Location = new System.Drawing.Point(228, 0);
            this.lciProPowerSetRule.Name = "lciProPowerSetRule";
            this.lciProPowerSetRule.Size = new System.Drawing.Size(253, 35);
            this.lciProPowerSetRule.Text = "分档规则";
            this.lciProPowerSetRule.TextSize = new System.Drawing.Size(56, 17);
            // 
            // lciProPowerSetCode
            // 
            this.lciProPowerSetCode.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lciProPowerSetCode.AppearanceItemCaption.Options.UseFont = true;
            this.lciProPowerSetCode.Control = this.teProPowerSetCode;
            this.lciProPowerSetCode.CustomizationFormText = "分档代码";
            this.lciProPowerSetCode.Location = new System.Drawing.Point(0, 0);
            this.lciProPowerSetCode.Name = "lciProPowerSetCode";
            this.lciProPowerSetCode.Size = new System.Drawing.Size(228, 35);
            this.lciProPowerSetCode.Text = "分档代码";
            this.lciProPowerSetCode.TextSize = new System.Drawing.Size(56, 17);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.sbtnQuery;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(481, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(104, 35);
            this.layoutControlItem3.Text = "layoutControlItem3";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.tableLayoutPanel2);
            this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl2.Location = new System.Drawing.Point(3, 393);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.Root = this.layoutControlGroup2;
            this.layoutControl2.Size = new System.Drawing.Size(595, 42);
            this.layoutControl2.TabIndex = 2;
            this.layoutControl2.Text = "layoutControl2";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.43443F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.56557F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnOk, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(591, 38);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(485, 3);
            this.btnCancel.MaximumSize = new System.Drawing.Size(100, 30);
            this.btnCancel.MinimumSize = new System.Drawing.Size(100, 30);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(372, 3);
            this.btnOk.MaximumSize = new System.Drawing.Size(100, 30);
            this.btnOk.MinimumSize = new System.Drawing.Size(100, 30);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 30);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup2";
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.Size = new System.Drawing.Size(595, 42);
            this.layoutControlGroup2.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.Text = "layoutControlGroup2";
            this.layoutControlGroup2.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.tableLayoutPanel2;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(595, 42);
            this.layoutControlItem4.Text = "layoutControlItem4";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextToControlDistance = 0;
            this.layoutControlItem4.TextVisible = false;
            // 
            // ProPowerSetSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 438);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ProPowerSetSelectForm";
            this.Text = "分档查询";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcProPowerSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProPowerSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.teProPowerSetCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teProPowerSetRule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciProPowerSetRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciProPowerSetCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            this.layoutControl2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.GridControl gcProPowerSet;
        private DevExpress.XtraGrid.Views.Grid.GridView gvProPowerSet;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.SimpleButton sbtnQuery;
        private DevExpress.XtraEditors.TextEdit teProPowerSetCode;
        private DevExpress.XtraEditors.TextEdit teProPowerSetRule;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem lciProPowerSetRule;
        private DevExpress.XtraLayout.LayoutControlItem lciProPowerSetCode;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraGrid.Columns.GridColumn gdProPowerSetCode;
        private DevExpress.XtraGrid.Columns.GridColumn gdProPowerSetRule;
        private DevExpress.XtraGrid.Columns.GridColumn gdSubPowerSetWay;
        private DevExpress.XtraGrid.Columns.GridColumn gdProPowerSet_key;
        private DevExpress.XtraGrid.Columns.GridColumn gdPowerMin;
        private DevExpress.XtraGrid.Columns.GridColumn gdPowerMax;
    }
}