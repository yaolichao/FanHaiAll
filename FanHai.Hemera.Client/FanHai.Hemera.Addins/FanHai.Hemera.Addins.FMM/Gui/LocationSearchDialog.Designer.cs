namespace FanHai.Hemera.Addins.FMM
{
    partial class LocationSearchDialog
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
            this.lblName = new DevExpress.XtraEditors.LabelControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.grpTop = new DevExpress.XtraEditors.GroupControl();
            this.cmbLocationLevel = new System.Windows.Forms.ComboBox();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.gcGridView = new DevExpress.XtraEditors.GroupControl();
            this.gcList = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.LOCATION_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOCATION_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DESCRIPTIONS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOCATION_LEVEL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.grpTop)).BeginInit();
            this.grpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcGridView)).BeginInit();
            this.gcGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(26, 49);
            this.lblName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(50, 18);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "名    称";
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(658, 44);
            this.btnQuery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(103, 32);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // grpTop
            // 
            this.grpTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTop.Controls.Add(this.cmbLocationLevel);
            this.grpTop.Controls.Add(this.btnQuery);
            this.grpTop.Controls.Add(this.txtName);
            this.grpTop.Controls.Add(this.lblName);
            this.grpTop.Location = new System.Drawing.Point(3, 4);
            this.grpTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpTop.Name = "grpTop";
            this.grpTop.Size = new System.Drawing.Size(785, 91);
            this.grpTop.TabIndex = 3;
            // 
            // cmbLocationLevel
            // 
            this.cmbLocationLevel.FormattingEnabled = true;
            this.cmbLocationLevel.Items.AddRange(new object[] {
            "",
            "1-工厂",
            "2-楼层",
            "5-车间",
            "9-区域"});
            this.cmbLocationLevel.Location = new System.Drawing.Point(511, 46);
            this.cmbLocationLevel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbLocationLevel.Name = "cmbLocationLevel";
            this.cmbLocationLevel.Size = new System.Drawing.Size(124, 26);
            this.cmbLocationLevel.TabIndex = 3;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(98, 46);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(407, 24);
            this.txtName.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.gcGridView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.grpTop, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(791, 504);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnConfirm);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 444);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(785, 56);
            this.panelControl1.TabIndex = 2;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(534, 13);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(103, 32);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(658, 13);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(103, 32);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gcGridView
            // 
            this.gcGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcGridView.Controls.Add(this.gcList);
            this.gcGridView.Location = new System.Drawing.Point(3, 103);
            this.gcGridView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcGridView.Name = "gcGridView";
            this.gcGridView.Size = new System.Drawing.Size(785, 333);
            this.gcGridView.TabIndex = 4;
            this.gcGridView.Text = "检索到的数据";
            // 
            // gcList
            // 
            this.gcList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcList.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcList.Location = new System.Drawing.Point(2, 28);
            this.gcList.LookAndFeel.SkinName = "Coffee";
            this.gcList.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcList.MainView = this.gridView1;
            this.gcList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcList.Name = "gcList";
            this.gcList.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gcList.Size = new System.Drawing.Size(781, 303);
            this.gcList.TabIndex = 0;
            this.gcList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.LOCATION_KEY,
            this.LOCATION_NAME,
            this.DESCRIPTIONS,
            this.LOCATION_LEVEL});
            this.gridView1.DetailHeight = 450;
            this.gridView1.FixedLineWidth = 3;
            this.gridView1.GridControl = this.gcList;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // LOCATION_KEY
            // 
            this.LOCATION_KEY.Caption = "区域主键";
            this.LOCATION_KEY.FieldName = "LOCATION_KEY";
            this.LOCATION_KEY.MinWidth = 23;
            this.LOCATION_KEY.Name = "LOCATION_KEY";
            this.LOCATION_KEY.OptionsColumn.AllowEdit = false;
            this.LOCATION_KEY.OptionsColumn.AllowFocus = false;
            this.LOCATION_KEY.Width = 86;
            // 
            // LOCATION_NAME
            // 
            this.LOCATION_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.LOCATION_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LOCATION_NAME.Caption = "名称";
            this.LOCATION_NAME.FieldName = "LOCATION_NAME";
            this.LOCATION_NAME.MinWidth = 23;
            this.LOCATION_NAME.Name = "LOCATION_NAME";
            this.LOCATION_NAME.OptionsColumn.AllowEdit = false;
            this.LOCATION_NAME.Visible = true;
            this.LOCATION_NAME.VisibleIndex = 0;
            this.LOCATION_NAME.Width = 134;
            // 
            // DESCRIPTIONS
            // 
            this.DESCRIPTIONS.AppearanceHeader.Options.UseTextOptions = true;
            this.DESCRIPTIONS.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DESCRIPTIONS.Caption = "描述";
            this.DESCRIPTIONS.FieldName = "DESCRIPTIONS";
            this.DESCRIPTIONS.MinWidth = 23;
            this.DESCRIPTIONS.Name = "DESCRIPTIONS";
            this.DESCRIPTIONS.OptionsColumn.AllowEdit = false;
            this.DESCRIPTIONS.OptionsColumn.AllowFocus = false;
            this.DESCRIPTIONS.Visible = true;
            this.DESCRIPTIONS.VisibleIndex = 1;
            this.DESCRIPTIONS.Width = 237;
            // 
            // LOCATION_LEVEL
            // 
            this.LOCATION_LEVEL.AppearanceHeader.Options.UseTextOptions = true;
            this.LOCATION_LEVEL.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LOCATION_LEVEL.Caption = "车间/区域";
            this.LOCATION_LEVEL.FieldName = "LOCATION_LEVEL";
            this.LOCATION_LEVEL.MinWidth = 23;
            this.LOCATION_LEVEL.Name = "LOCATION_LEVEL";
            this.LOCATION_LEVEL.OptionsColumn.AllowEdit = false;
            this.LOCATION_LEVEL.OptionsColumn.AllowFocus = false;
            this.LOCATION_LEVEL.Visible = true;
            this.LOCATION_LEVEL.VisibleIndex = 2;
            this.LOCATION_LEVEL.Width = 231;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // LocationSearchDialog
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 504);
            this.Controls.Add(this.tableLayoutPanel1);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "LocationSearchDialog";
            this.Load += new System.EventHandler(this.LocationSearchDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpTop)).EndInit();
            this.grpTop.ResumeLayout(false);
            this.grpTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcGridView)).EndInit();
            this.gcGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblName;
        private DevExpress.XtraGrid.Columns.GridColumn LOCATION_LEVEL;
        private DevExpress.XtraGrid.Columns.GridColumn DESCRIPTIONS;
        private DevExpress.XtraGrid.Columns.GridColumn LOCATION_NAME;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.GroupControl grpTop;
        private DevExpress.XtraEditors.TextEdit txtName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.GroupControl gcGridView;
        private DevExpress.XtraGrid.GridControl gcList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn LOCATION_KEY;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private System.Windows.Forms.ComboBox cmbLocationLevel;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}
