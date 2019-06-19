namespace FanHai.Hemera.Addins.FMM
{
    partial class PartSearch
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
            this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.txtPart = new DevExpress.XtraEditors.TextEdit();
            this.lblMatNumber = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.part_number = new DevExpress.XtraGrid.Columns.GridColumn();
            this.part_revision = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPartType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPartModule = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPartClass = new DevExpress.XtraGrid.Columns.GridColumn();
            this.part_key = new DevExpress.XtraGrid.Columns.GridColumn();
            this.description = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelControlBottom = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBottom)).BeginInit();
            this.panelControlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl1.Controls.Add(this.btnSearch);
            this.groupControl1.Controls.Add(this.txtPart);
            this.groupControl1.Controls.Add(this.lblMatNumber);
            this.groupControl1.Location = new System.Drawing.Point(3, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(822, 74);
            this.groupControl1.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(724, 36);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 25);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // txtPart
            // 
            this.txtPart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPart.Location = new System.Drawing.Point(87, 38);
            this.txtPart.Name = "txtPart";
            this.txtPart.Size = new System.Drawing.Size(631, 20);
            this.txtPart.TabIndex = 1;
            // 
            // lblMatNumber
            // 
            this.lblMatNumber.Location = new System.Drawing.Point(18, 41);
            this.lblMatNumber.Name = "lblMatNumber";
            this.lblMatNumber.Size = new System.Drawing.Size(40, 14);
            this.lblMatNumber.TabIndex = 0;
            this.lblMatNumber.Text = "物料号:";
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.Location = new System.Drawing.Point(3, 83);
            this.gridControl1.LookAndFeel.SkinName = "Coffee";
            this.gridControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(822, 303);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.DoubleClick += new System.EventHandler(this.gridControl1_DoubleClick);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.part_number,
            this.part_revision,
            this.gcPartType,
            this.gcPartModule,
            this.gcPartClass,
            this.part_key,
            this.description});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridView1_ShowingEditor);
            // 
            // part_number
            // 
            this.part_number.AppearanceHeader.Options.UseTextOptions = true;
            this.part_number.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.part_number.Caption = "物料号";
            this.part_number.FieldName = "PART_NAME";
            this.part_number.Name = "part_number";
            this.part_number.OptionsColumn.AllowEdit = false;
            this.part_number.Visible = true;
            this.part_number.VisibleIndex = 0;
            this.part_number.Width = 101;
            // 
            // part_revision
            // 
            this.part_revision.AppearanceHeader.Options.UseTextOptions = true;
            this.part_revision.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.part_revision.Caption = "版本号";
            this.part_revision.FieldName = "PART_VERSION";
            this.part_revision.Name = "part_revision";
            this.part_revision.OptionsColumn.AllowEdit = false;
            this.part_revision.Visible = true;
            this.part_revision.VisibleIndex = 1;
            this.part_revision.Width = 78;
            // 
            // gcPartType
            // 
            this.gcPartType.Caption = "产品类型";
            this.gcPartType.FieldName = "PART_TYPE";
            this.gcPartType.Name = "gcPartType";
            this.gcPartType.Visible = true;
            this.gcPartType.VisibleIndex = 2;
            this.gcPartType.Width = 90;
            // 
            // gcPartModule
            // 
            this.gcPartModule.Caption = "产品型号";
            this.gcPartModule.FieldName = "PART_MODULE";
            this.gcPartModule.Name = "gcPartModule";
            this.gcPartModule.Visible = true;
            this.gcPartModule.VisibleIndex = 3;
            this.gcPartModule.Width = 98;
            // 
            // gcPartClass
            // 
            this.gcPartClass.Caption = "产品分类";
            this.gcPartClass.FieldName = "PART_CLASS";
            this.gcPartClass.Name = "gcPartClass";
            this.gcPartClass.Visible = true;
            this.gcPartClass.VisibleIndex = 4;
            this.gcPartClass.Width = 97;
            // 
            // part_key
            // 
            this.part_key.Caption = "物料主键";
            this.part_key.FieldName = "PART_KEY";
            this.part_key.Name = "part_key";
            // 
            // description
            // 
            this.description.AppearanceHeader.Options.UseTextOptions = true;
            this.description.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.description.Caption = "描述";
            this.description.FieldName = "PART_DESC";
            this.description.Name = "description";
            this.description.OptionsColumn.AllowEdit = false;
            this.description.Visible = true;
            this.description.VisibleIndex = 5;
            this.description.Width = 337;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.panelControlBottom, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.groupControl1, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.gridControl1, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(828, 442);
            this.tableLayoutPanelMain.TabIndex = 3;
            // 
            // panelControlBottom
            // 
            this.panelControlBottom.Controls.Add(this.btnCancel);
            this.panelControlBottom.Controls.Add(this.btnConfirm);
            this.panelControlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlBottom.Location = new System.Drawing.Point(3, 392);
            this.panelControlBottom.Name = "panelControlBottom";
            this.panelControlBottom.Size = new System.Drawing.Size(822, 47);
            this.panelControlBottom.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(724, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(628, 13);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(90, 25);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // PartSearch
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 442);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PartSearch";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBottom)).EndInit();
            this.panelControlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraEditors.TextEdit txtPart;
        private DevExpress.XtraEditors.LabelControl lblMatNumber;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn part_number;
        private DevExpress.XtraGrid.Columns.GridColumn part_revision;
        private DevExpress.XtraGrid.Columns.GridColumn description;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraGrid.Columns.GridColumn part_key;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.PanelControl panelControlBottom;
        private DevExpress.XtraGrid.Columns.GridColumn gcPartType;
        private DevExpress.XtraGrid.Columns.GridColumn gcPartModule;
        private DevExpress.XtraGrid.Columns.GridColumn gcPartClass;

    }
}