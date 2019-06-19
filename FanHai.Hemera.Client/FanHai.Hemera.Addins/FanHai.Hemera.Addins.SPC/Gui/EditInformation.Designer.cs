namespace FanHai.Hemera.Addins.SPC
{
    partial class EditInformation
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
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gcPoint = new DevExpress.XtraGrid.GridControl();
            this.gvPoint = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.LOCATION_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ORDER_NUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PART_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PART_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SHIFT_VALUE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EQUIPMENT_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MATERIAL_LOT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOT_NUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SUPPLIER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.QTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CREATOR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CREATE_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.REWORK_FLAG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPointEdit = new DevExpress.XtraGrid.GridControl();
            this.gvPointEdit = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.EDIT_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EDITOR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EDIT_REASON = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPointEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPointEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(634, 330);
            this.xtraTabControl1.TabIndex = 1;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.tableLayoutPanel1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(630, 304);
            this.xtraTabPage1.Text = "基本信息";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gcPoint, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gcPointEdit, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.40789F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43.42105F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.8421F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(630, 304);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gcPoint
            // 
            this.gcPoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcPoint.Location = new System.Drawing.Point(3, 3);
            this.gcPoint.MainView = this.gvPoint;
            this.gcPoint.Name = "gcPoint";
            this.gcPoint.Size = new System.Drawing.Size(624, 129);
            this.gcPoint.TabIndex = 3;
            this.gcPoint.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPoint});
            // 
            // gvPoint
            // 
            this.gvPoint.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.LOCATION_NAME,
            this.ORDER_NUMBER,
            this.PART_NO,
            this.PART_TYPE,
            this.SHIFT_VALUE,
            this.EQUIPMENT_NAME,
            this.MATERIAL_LOT,
            this.LOT_NUMBER,
            this.SUPPLIER,
            this.QTY,
            this.CREATOR,
            this.CREATE_TIME,
            this.REWORK_FLAG});
            this.gvPoint.GridControl = this.gcPoint;
            this.gvPoint.Name = "gvPoint";
            this.gvPoint.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvPoint.OptionsBehavior.Editable = false;
            this.gvPoint.OptionsView.ColumnAutoWidth = false;
            this.gvPoint.OptionsView.ShowGroupPanel = false;
            // 
            // LOCATION_NAME
            // 
            this.LOCATION_NAME.Caption = "车间";
            this.LOCATION_NAME.FieldName = "LOCATION_NAME";
            this.LOCATION_NAME.Name = "LOCATION_NAME";
            this.LOCATION_NAME.Visible = true;
            this.LOCATION_NAME.VisibleIndex = 6;
            // 
            // ORDER_NUMBER
            // 
            this.ORDER_NUMBER.Caption = "工单号";
            this.ORDER_NUMBER.FieldName = "ORDER_NUMBER";
            this.ORDER_NUMBER.Name = "ORDER_NUMBER";
            this.ORDER_NUMBER.Visible = true;
            this.ORDER_NUMBER.VisibleIndex = 5;
            // 
            // PART_NO
            // 
            this.PART_NO.Caption = "产品编号";
            this.PART_NO.FieldName = "PART_NO";
            this.PART_NO.Name = "PART_NO";
            this.PART_NO.Visible = true;
            this.PART_NO.VisibleIndex = 0;
            // 
            // PART_TYPE
            // 
            this.PART_TYPE.Caption = "产品类型";
            this.PART_TYPE.FieldName = "PART_TYPE";
            this.PART_TYPE.Name = "PART_TYPE";
            this.PART_TYPE.Visible = true;
            this.PART_TYPE.VisibleIndex = 1;
            // 
            // SHIFT_VALUE
            // 
            this.SHIFT_VALUE.Caption = "班别";
            this.SHIFT_VALUE.FieldName = "SHIFT_VALUE";
            this.SHIFT_VALUE.Name = "SHIFT_VALUE";
            this.SHIFT_VALUE.Visible = true;
            this.SHIFT_VALUE.VisibleIndex = 2;
            // 
            // EQUIPMENT_NAME
            // 
            this.EQUIPMENT_NAME.Caption = "设备名称";
            this.EQUIPMENT_NAME.FieldName = "EQUIPMENT_NAME";
            this.EQUIPMENT_NAME.Name = "EQUIPMENT_NAME";
            this.EQUIPMENT_NAME.Visible = true;
            this.EQUIPMENT_NAME.VisibleIndex = 12;
            // 
            // MATERIAL_LOT
            // 
            this.MATERIAL_LOT.Caption = "供应商批次";
            this.MATERIAL_LOT.FieldName = "MATERIAL_LOT";
            this.MATERIAL_LOT.Name = "MATERIAL_LOT";
            this.MATERIAL_LOT.Visible = true;
            this.MATERIAL_LOT.VisibleIndex = 4;
            // 
            // LOT_NUMBER
            // 
            this.LOT_NUMBER.Caption = "生产批次";
            this.LOT_NUMBER.FieldName = "LOT_NUMBER";
            this.LOT_NUMBER.Name = "LOT_NUMBER";
            this.LOT_NUMBER.Visible = true;
            this.LOT_NUMBER.VisibleIndex = 7;
            // 
            // SUPPLIER
            // 
            this.SUPPLIER.Caption = "供应商";
            this.SUPPLIER.FieldName = "SUPPLIER";
            this.SUPPLIER.Name = "SUPPLIER";
            this.SUPPLIER.Visible = true;
            this.SUPPLIER.VisibleIndex = 3;
            // 
            // QTY
            // 
            this.QTY.Caption = "数量";
            this.QTY.FieldName = "QTY";
            this.QTY.Name = "QTY";
            this.QTY.Visible = true;
            this.QTY.VisibleIndex = 8;
            // 
            // CREATOR
            // 
            this.CREATOR.Caption = "创建人";
            this.CREATOR.FieldName = "CREATOR";
            this.CREATOR.Name = "CREATOR";
            this.CREATOR.Visible = true;
            this.CREATOR.VisibleIndex = 9;
            // 
            // CREATE_TIME
            // 
            this.CREATE_TIME.Caption = "创建时间";
            this.CREATE_TIME.FieldName = "CREATE_TIME";
            this.CREATE_TIME.Name = "CREATE_TIME";
            this.CREATE_TIME.Visible = true;
            this.CREATE_TIME.VisibleIndex = 10;
            // 
            // REWORK_FLAG
            // 
            this.REWORK_FLAG.Caption = "类型";
            this.REWORK_FLAG.FieldName = "REWORK_FLAG";
            this.REWORK_FLAG.Name = "REWORK_FLAG";
            this.REWORK_FLAG.Visible = true;
            this.REWORK_FLAG.VisibleIndex = 11;
            // 
            // gcPointEdit
            // 
            this.gcPointEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcPointEdit.Location = new System.Drawing.Point(3, 138);
            this.gcPointEdit.MainView = this.gvPointEdit;
            this.gcPointEdit.Name = "gcPointEdit";
            this.gcPointEdit.Size = new System.Drawing.Size(624, 126);
            this.gcPointEdit.TabIndex = 4;
            this.gcPointEdit.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPointEdit});
            // 
            // gvPointEdit
            // 
            this.gvPointEdit.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.EDIT_TIME,
            this.EDITOR,
            this.EDIT_REASON});
            this.gvPointEdit.GridControl = this.gcPointEdit;
            this.gvPointEdit.Name = "gvPointEdit";
            this.gvPointEdit.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvPointEdit.OptionsBehavior.Editable = false;
            this.gvPointEdit.OptionsView.ShowGroupPanel = false;
            // 
            // EDIT_TIME
            // 
            this.EDIT_TIME.Caption = "编辑时间";
            this.EDIT_TIME.FieldName = "EDIT_TIME";
            this.EDIT_TIME.Name = "EDIT_TIME";
            this.EDIT_TIME.Visible = true;
            this.EDIT_TIME.VisibleIndex = 0;
            // 
            // EDITOR
            // 
            this.EDITOR.Caption = "编辑人员";
            this.EDITOR.FieldName = "EDITOR";
            this.EDITOR.Name = "EDITOR";
            this.EDITOR.Visible = true;
            this.EDITOR.VisibleIndex = 1;
            // 
            // EDIT_REASON
            // 
            this.EDIT_REASON.Caption = "备注/原因";
            this.EDIT_REASON.FieldName = "EDIT_REASON";
            this.EDIT_REASON.Name = "EDIT_REASON";
            this.EDIT_REASON.Visible = true;
            this.EDIT_REASON.VisibleIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.Location = new System.Drawing.Point(540, 271);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 29);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取 消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // EditInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 330);
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "EditInformation";
            this.Text = "EditInformation";
            this.Load += new System.EventHandler(this.EditInformation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPointEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPointEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraGrid.GridControl gcPoint;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPoint;
        private DevExpress.XtraGrid.GridControl gcPointEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPointEdit;
        private DevExpress.XtraGrid.Columns.GridColumn EDIT_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn EDITOR;
        private DevExpress.XtraGrid.Columns.GridColumn EDIT_REASON;
        private DevExpress.XtraGrid.Columns.GridColumn PART_NO;
        private DevExpress.XtraGrid.Columns.GridColumn PART_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn SHIFT_VALUE;
        private DevExpress.XtraGrid.Columns.GridColumn SUPPLIER;
        private DevExpress.XtraGrid.Columns.GridColumn MATERIAL_LOT;
        private DevExpress.XtraGrid.Columns.GridColumn ORDER_NUMBER;
        private DevExpress.XtraGrid.Columns.GridColumn LOCATION_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn LOT_NUMBER;
        private DevExpress.XtraGrid.Columns.GridColumn QTY;
        private DevExpress.XtraGrid.Columns.GridColumn CREATOR;
        private DevExpress.XtraGrid.Columns.GridColumn CREATE_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn REWORK_FLAG;
        private DevExpress.XtraGrid.Columns.GridColumn EQUIPMENT_NAME;
    }
}