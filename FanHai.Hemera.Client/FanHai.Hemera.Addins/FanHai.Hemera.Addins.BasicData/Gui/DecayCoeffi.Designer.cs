namespace FanHai.Hemera.Addins.BasicData
{
    partial class DecayCoeffi
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
            this.gcDecayCoeffi = new DevExpress.XtraGrid.GridControl();
            this.gvDecayCoeffi = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.D_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rilueType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.D_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.D_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemlue_dname = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.D_CODE_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COEFFICIENT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COEFFICIENT_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DIT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.CODEDESC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.WERKS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STEP_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PARAMENTID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PRODUCTCODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CONTROLTYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDecayCoeffi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDecayCoeffi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemlue_dname)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(1012, 47);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(827, 0);
            this.lblInfos.Size = new System.Drawing.Size(185, 42);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t登录时间：2019-01-15 09:29:17";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            // 
            // gcDecayCoeffi
            // 
            this.gcDecayCoeffi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDecayCoeffi.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcDecayCoeffi.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.gcDecayCoeffi.Location = new System.Drawing.Point(2, 23);
            this.gcDecayCoeffi.MainView = this.gvDecayCoeffi;
            this.gcDecayCoeffi.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcDecayCoeffi.Name = "gcDecayCoeffi";
            this.gcDecayCoeffi.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemlue_dname,
            this.rilueType});
            this.gcDecayCoeffi.Size = new System.Drawing.Size(1000, 561);
            this.gcDecayCoeffi.TabIndex = 4;
            this.gcDecayCoeffi.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDecayCoeffi});
            // 
            // gvDecayCoeffi
            // 
            this.gvDecayCoeffi.Appearance.HeaderPanel.Font = new System.Drawing.Font("宋体", 12F);
            this.gvDecayCoeffi.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDecayCoeffi.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.D_TYPE,
            this.D_CODE,
            this.D_NAME,
            this.D_CODE_DESC,
            this.COEFFICIENT,
            this.COEFFICIENT_DESC,
            this.DIT});
            this.gvDecayCoeffi.DetailHeight = 408;
            this.gvDecayCoeffi.GridControl = this.gcDecayCoeffi;
            this.gvDecayCoeffi.Name = "gvDecayCoeffi";
            this.gvDecayCoeffi.OptionsBehavior.Editable = false;
            this.gvDecayCoeffi.OptionsMenu.EnableColumnMenu = false;
            this.gvDecayCoeffi.OptionsView.RowAutoHeight = true;
            this.gvDecayCoeffi.OptionsView.ShowGroupPanel = false;
            this.gvDecayCoeffi.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvDecayCoeffi_RowClick);
            this.gvDecayCoeffi.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvDecayCoeffi_CustomDrawRowIndicator);
            this.gvDecayCoeffi.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDecayCoeffi_CellValueChanging);
            // 
            // D_TYPE
            // 
            this.D_TYPE.Caption = "类型";
            this.D_TYPE.ColumnEdit = this.rilueType;
            this.D_TYPE.FieldName = "DECOEFFI_TYPE";
            this.D_TYPE.MinWidth = 24;
            this.D_TYPE.Name = "D_TYPE";
            this.D_TYPE.Visible = true;
            this.D_TYPE.VisibleIndex = 0;
            this.D_TYPE.Width = 87;
            // 
            // rilueType
            // 
            this.rilueType.AutoHeight = false;
            this.rilueType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rilueType.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "类型")});
            this.rilueType.Name = "rilueType";
            this.rilueType.NullText = "";
            // 
            // D_CODE
            // 
            this.D_CODE.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F);
            this.D_CODE.AppearanceHeader.Options.UseFont = true;
            this.D_CODE.Caption = "代码";
            this.D_CODE.FieldName = "D_CODE";
            this.D_CODE.MinWidth = 24;
            this.D_CODE.Name = "D_CODE";
            this.D_CODE.Visible = true;
            this.D_CODE.VisibleIndex = 1;
            this.D_CODE.Width = 87;
            // 
            // D_NAME
            // 
            this.D_NAME.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F);
            this.D_NAME.AppearanceHeader.Options.UseFont = true;
            this.D_NAME.Caption = "名称";
            this.D_NAME.ColumnEdit = this.repositoryItemlue_dname;
            this.D_NAME.FieldName = "D_NAME";
            this.D_NAME.MinWidth = 24;
            this.D_NAME.Name = "D_NAME";
            this.D_NAME.Visible = true;
            this.D_NAME.VisibleIndex = 2;
            this.D_NAME.Width = 87;
            // 
            // repositoryItemlue_dname
            // 
            this.repositoryItemlue_dname.AutoHeight = false;
            this.repositoryItemlue_dname.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemlue_dname.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Column_Name", "名称")});
            this.repositoryItemlue_dname.Name = "repositoryItemlue_dname";
            this.repositoryItemlue_dname.NullText = "";
            // 
            // D_CODE_DESC
            // 
            this.D_CODE_DESC.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F);
            this.D_CODE_DESC.AppearanceHeader.Options.UseFont = true;
            this.D_CODE_DESC.Caption = "衰减对象描述";
            this.D_CODE_DESC.FieldName = "D_CODE_DESC";
            this.D_CODE_DESC.MinWidth = 24;
            this.D_CODE_DESC.Name = "D_CODE_DESC";
            this.D_CODE_DESC.Visible = true;
            this.D_CODE_DESC.VisibleIndex = 3;
            this.D_CODE_DESC.Width = 87;
            // 
            // COEFFICIENT
            // 
            this.COEFFICIENT.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F);
            this.COEFFICIENT.AppearanceHeader.Options.UseFont = true;
            this.COEFFICIENT.Caption = "衰减系数";
            this.COEFFICIENT.FieldName = "COEFFICIENT";
            this.COEFFICIENT.MinWidth = 24;
            this.COEFFICIENT.Name = "COEFFICIENT";
            this.COEFFICIENT.Visible = true;
            this.COEFFICIENT.VisibleIndex = 4;
            this.COEFFICIENT.Width = 87;
            // 
            // COEFFICIENT_DESC
            // 
            this.COEFFICIENT_DESC.Caption = "衰减系数描述";
            this.COEFFICIENT_DESC.FieldName = "COEFFICIENT_DESC";
            this.COEFFICIENT_DESC.MinWidth = 24;
            this.COEFFICIENT_DESC.Name = "COEFFICIENT_DESC";
            this.COEFFICIENT_DESC.Width = 87;
            // 
            // DIT
            // 
            this.DIT.Caption = "小数位数";
            this.DIT.FieldName = "DIT";
            this.DIT.MinWidth = 24;
            this.DIT.Name = "DIT";
            this.DIT.Visible = true;
            this.DIT.VisibleIndex = 5;
            this.DIT.Width = 87;
            // 
            // pnlTitle
            // 
            this.pnlTitle.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlTitle.Controls.Add(this.btnCancel);
            this.pnlTitle.Controls.Add(this.btSave);
            this.pnlTitle.Controls.Add(this.btnDelete);
            this.pnlTitle.Controls.Add(this.btnModify);
            this.pnlTitle.Controls.Add(this.btnAdd);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTitle.Location = new System.Drawing.Point(1, 641);
            this.pnlTitle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.pnlTitle.Size = new System.Drawing.Size(1012, 31);
            this.pnlTitle.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(919, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Location = new System.Drawing.Point(828, 4);
            this.btSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(82, 23);
            this.btSave.TabIndex = 13;
            this.btSave.Text = "保存";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(737, 4);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(82, 23);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Location = new System.Drawing.Point(646, 4);
            this.btnModify.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(82, 23);
            this.btnModify.TabIndex = 11;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(555, 4);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(82, 23);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "新增";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.groupControl1, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(1, 47);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 1;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 594F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 594F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1012, 594);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.gcDecayCoeffi);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(4, 4);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1004, 586);
            this.groupControl1.TabIndex = 12;
            this.groupControl1.Text = "衰减设置";
            // 
            // CODEDESC
            // 
            this.CODEDESC.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CODEDESC.AppearanceHeader.Options.UseFont = true;
            this.CODEDESC.Caption = "计划描述";
            this.CODEDESC.FieldName = "CODEDESC";
            this.CODEDESC.MinWidth = 100;
            this.CODEDESC.Name = "CODEDESC";
            this.CODEDESC.Visible = true;
            this.CODEDESC.VisibleIndex = 1;
            this.CODEDESC.Width = 100;
            // 
            // WERKS
            // 
            this.WERKS.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WERKS.AppearanceHeader.Options.UseFont = true;
            this.WERKS.Caption = "车间";
            this.WERKS.FieldName = "WERKS";
            this.WERKS.MinWidth = 100;
            this.WERKS.Name = "WERKS";
            this.WERKS.Visible = true;
            this.WERKS.VisibleIndex = 2;
            this.WERKS.Width = 100;
            // 
            // STEP_KEY
            // 
            this.STEP_KEY.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.STEP_KEY.AppearanceHeader.Options.UseFont = true;
            this.STEP_KEY.Caption = "工序";
            this.STEP_KEY.FieldName = "STEP_KEY";
            this.STEP_KEY.MinWidth = 80;
            this.STEP_KEY.Name = "STEP_KEY";
            this.STEP_KEY.Visible = true;
            this.STEP_KEY.VisibleIndex = 3;
            this.STEP_KEY.Width = 80;
            // 
            // PARAMENTID
            // 
            this.PARAMENTID.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PARAMENTID.AppearanceHeader.Options.UseFont = true;
            this.PARAMENTID.Caption = "参数";
            this.PARAMENTID.FieldName = "PARAMENTID";
            this.PARAMENTID.MinWidth = 80;
            this.PARAMENTID.Name = "PARAMENTID";
            this.PARAMENTID.Visible = true;
            this.PARAMENTID.VisibleIndex = 4;
            this.PARAMENTID.Width = 80;
            // 
            // PRODUCTCODE
            // 
            this.PRODUCTCODE.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PRODUCTCODE.AppearanceHeader.Options.UseFont = true;
            this.PRODUCTCODE.Caption = "成品类型";
            this.PRODUCTCODE.FieldName = "PRODUCTCODE";
            this.PRODUCTCODE.MinWidth = 100;
            this.PRODUCTCODE.Name = "PRODUCTCODE";
            this.PRODUCTCODE.Visible = true;
            this.PRODUCTCODE.VisibleIndex = 5;
            this.PRODUCTCODE.Width = 100;
            // 
            // CONTROLTYPE
            // 
            this.CONTROLTYPE.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CONTROLTYPE.AppearanceHeader.Options.UseFont = true;
            this.CONTROLTYPE.Caption = "控制图类型";
            this.CONTROLTYPE.FieldName = "CONTROLTYPE";
            this.CONTROLTYPE.MinWidth = 120;
            this.CONTROLTYPE.Name = "CONTROLTYPE";
            this.CONTROLTYPE.Visible = true;
            this.CONTROLTYPE.VisibleIndex = 6;
            this.CONTROLTYPE.Width = 120;
            // 
            // DecayCoeffi
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.pnlTitle);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DecayCoeffi";
            this.Size = new System.Drawing.Size(1014, 672);
            this.Load += new System.EventHandler(this.SPControlPlan_Load);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDecayCoeffi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDecayCoeffi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemlue_dname)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraGrid.GridControl gcDecayCoeffi;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDecayCoeffi;
        private DevExpress.XtraGrid.Columns.GridColumn D_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn D_CODE;
        private DevExpress.XtraGrid.Columns.GridColumn D_CODE_DESC;
        private DevExpress.XtraGrid.Columns.GridColumn COEFFICIENT;
        private DevExpress.XtraGrid.Columns.GridColumn COEFFICIENT_DESC;
        private DevExpress.XtraGrid.Columns.GridColumn CODEDESC;
        private DevExpress.XtraGrid.Columns.GridColumn WERKS;
        private DevExpress.XtraGrid.Columns.GridColumn STEP_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn PARAMENTID;
        private DevExpress.XtraGrid.Columns.GridColumn PRODUCTCODE;
        private DevExpress.XtraGrid.Columns.GridColumn CONTROLTYPE;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.Columns.GridColumn DIT;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemlue_dname;
        private DevExpress.XtraGrid.Columns.GridColumn D_TYPE;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rilueType;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btSave;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnModify;
    }
}
