namespace FanHai.Hemera.Utils.UDA
{
    partial class UdaColumnsSelect
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
            this.gridAttributeSelect = new DevExpress.XtraGrid.GridControl();
            this.gridAttributeSelectView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.clnAttributeKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clnAttributeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clnAttributeDataType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clnAttributeDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clnAttributeDataTypeStr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControlMain = new DevExpress.XtraEditors.GroupControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributeSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributeSelectView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMain)).BeginInit();
            this.groupControlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridAttributeSelect
            // 
            this.gridAttributeSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAttributeSelect.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridAttributeSelect.Location = new System.Drawing.Point(2, 28);
            this.gridAttributeSelect.LookAndFeel.SkinName = "Coffee";
            this.gridAttributeSelect.MainView = this.gridAttributeSelectView;
            this.gridAttributeSelect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridAttributeSelect.Name = "gridAttributeSelect";
            this.gridAttributeSelect.Size = new System.Drawing.Size(782, 489);
            this.gridAttributeSelect.TabIndex = 0;
            this.gridAttributeSelect.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridAttributeSelectView});
            this.gridAttributeSelect.DoubleClick += new System.EventHandler(this.gridAttributeSelect_DoubleClick);
            // 
            // gridAttributeSelectView
            // 
            this.gridAttributeSelectView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.clnAttributeKey,
            this.clnAttributeName,
            this.clnAttributeDataType,
            this.clnAttributeDesc,
            this.clnAttributeDataTypeStr});
            this.gridAttributeSelectView.DetailHeight = 450;
            this.gridAttributeSelectView.FixedLineWidth = 3;
            this.gridAttributeSelectView.GridControl = this.gridAttributeSelect;
            this.gridAttributeSelectView.Name = "gridAttributeSelectView";
            this.gridAttributeSelectView.OptionsView.ShowGroupPanel = false;
            this.gridAttributeSelectView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridAttributeSelectView_CustomDrawRowIndicator);
            // 
            // clnAttributeKey
            // 
            this.clnAttributeKey.Caption = "attribute key";
            this.clnAttributeKey.FieldName = "clnAttributeKey";
            this.clnAttributeKey.MinWidth = 23;
            this.clnAttributeKey.Name = "clnAttributeKey";
            this.clnAttributeKey.OptionsColumn.AllowEdit = false;
            this.clnAttributeKey.Width = 86;
            // 
            // clnAttributeName
            // 
            this.clnAttributeName.AppearanceHeader.Options.UseTextOptions = true;
            this.clnAttributeName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.clnAttributeName.Caption = "属性名称";
            this.clnAttributeName.FieldName = "clnAttributeName";
            this.clnAttributeName.MinWidth = 23;
            this.clnAttributeName.Name = "clnAttributeName";
            this.clnAttributeName.OptionsColumn.AllowEdit = false;
            this.clnAttributeName.Visible = true;
            this.clnAttributeName.VisibleIndex = 0;
            this.clnAttributeName.Width = 86;
            // 
            // clnAttributeDataType
            // 
            this.clnAttributeDataType.AppearanceHeader.Options.UseTextOptions = true;
            this.clnAttributeDataType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.clnAttributeDataType.Caption = "数据类型";
            this.clnAttributeDataType.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.clnAttributeDataType.FieldName = "clnAttributeDataType";
            this.clnAttributeDataType.MinWidth = 23;
            this.clnAttributeDataType.Name = "clnAttributeDataType";
            this.clnAttributeDataType.OptionsColumn.AllowEdit = false;
            this.clnAttributeDataType.Width = 86;
            // 
            // clnAttributeDesc
            // 
            this.clnAttributeDesc.AppearanceHeader.Options.UseTextOptions = true;
            this.clnAttributeDesc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.clnAttributeDesc.Caption = "描述";
            this.clnAttributeDesc.FieldName = "clnAttributeDesc";
            this.clnAttributeDesc.MinWidth = 23;
            this.clnAttributeDesc.Name = "clnAttributeDesc";
            this.clnAttributeDesc.OptionsColumn.AllowEdit = false;
            this.clnAttributeDesc.Visible = true;
            this.clnAttributeDesc.VisibleIndex = 2;
            this.clnAttributeDesc.Width = 86;
            // 
            // clnAttributeDataTypeStr
            // 
            this.clnAttributeDataTypeStr.AppearanceHeader.Options.UseTextOptions = true;
            this.clnAttributeDataTypeStr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.clnAttributeDataTypeStr.Caption = "数据类型";
            this.clnAttributeDataTypeStr.FieldName = "clnAttributeDataTypeStr";
            this.clnAttributeDataTypeStr.MinWidth = 23;
            this.clnAttributeDataTypeStr.Name = "clnAttributeDataTypeStr";
            this.clnAttributeDataTypeStr.OptionsColumn.AllowEdit = false;
            this.clnAttributeDataTypeStr.Visible = true;
            this.clnAttributeDataTypeStr.VisibleIndex = 1;
            this.clnAttributeDataTypeStr.Width = 86;
            // 
            // groupControlMain
            // 
            this.groupControlMain.Controls.Add(this.gridAttributeSelect);
            this.groupControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlMain.Location = new System.Drawing.Point(2, 2);
            this.groupControlMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControlMain.Name = "groupControlMain";
            this.groupControlMain.Size = new System.Drawing.Size(786, 519);
            this.groupControlMain.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(587, 8);
            this.btnOK.LookAndFeel.SkinName = "Coffee";
            this.btnOK.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(78, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确 定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(707, 8);
            this.btnCancel.LookAndFeel.SkinName = "Coffee";
            this.btnCancel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取 消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControlMain);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(790, 523);
            this.panelControl1.TabIndex = 2;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Controls.Add(this.btnOK);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 482);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(790, 41);
            this.panelControl2.TabIndex = 3;
            // 
            // UdaColumnsSelect
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 523);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "UdaColumnsSelect";
            this.Load += new System.EventHandler(this.UdaColumnsSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributeSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttributeSelectView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMain)).EndInit();
            this.groupControlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridAttributeSelect;
        private DevExpress.XtraGrid.Views.Grid.GridView gridAttributeSelectView;
        private DevExpress.XtraGrid.Columns.GridColumn clnAttributeKey;
        private DevExpress.XtraGrid.Columns.GridColumn clnAttributeName;
        private DevExpress.XtraGrid.Columns.GridColumn clnAttributeDataType;
        private DevExpress.XtraGrid.Columns.GridColumn clnAttributeDesc;
        private DevExpress.XtraGrid.Columns.GridColumn clnAttributeDataTypeStr;
        private DevExpress.XtraEditors.GroupControl groupControlMain;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
    }
}