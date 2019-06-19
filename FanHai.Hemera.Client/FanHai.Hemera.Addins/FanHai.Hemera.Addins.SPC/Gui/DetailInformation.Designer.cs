namespace FanHai.Hemera.Addins.SPC
{
    partial class DetailInformation
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
            this.gcControl = new DevExpress.XtraGrid.GridControl();
            this.gvControl = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ROWNUM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOCATION_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STEP_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOT_NUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PARAM_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PARAM_VALUE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CREATE_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ORDER_NUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SHIFT_VALUE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PART_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PART_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MATERIAL_LOT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SUPPLIER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvControl)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(661, 323);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.tableLayoutPanel1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(657, 297);
            this.xtraTabPage1.Text = "基本信息";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.gcControl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.70905F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.290954F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(657, 297);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gcControl
            // 
            this.gcControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcControl.Location = new System.Drawing.Point(3, 3);
            this.gcControl.MainView = this.gvControl;
            this.gcControl.Name = "gcControl";
            this.gcControl.Size = new System.Drawing.Size(651, 263);
            this.gcControl.TabIndex = 0;
            this.gcControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvControl});
            // 
            // gvControl
            // 
            this.gvControl.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ROWNUM,
            this.LOCATION_NAME,
            this.STEP_NAME,
            this.LOT_NUMBER,
            this.PARAM_NAME,
            this.PARAM_VALUE,
            this.CREATE_TIME,
            this.ORDER_NUMBER,
            this.SHIFT_VALUE,
            this.PART_NO,
            this.PART_TYPE,
            this.MATERIAL_LOT,
            this.SUPPLIER});
            this.gvControl.GridControl = this.gcControl;
            this.gvControl.Name = "gvControl";
            this.gvControl.OptionsBehavior.Editable = false;
            this.gvControl.OptionsView.ColumnAutoWidth = false;
            // 
            // ROWNUM
            // 
            this.ROWNUM.Caption = "序号";
            this.ROWNUM.FieldName = "ROWNUM";
            this.ROWNUM.Name = "ROWNUM";
            this.ROWNUM.Visible = true;
            this.ROWNUM.VisibleIndex = 0;
            // 
            // LOCATION_NAME
            // 
            this.LOCATION_NAME.Caption = "车间";
            this.LOCATION_NAME.FieldName = "LOCATION_NAME";
            this.LOCATION_NAME.Name = "LOCATION_NAME";
            this.LOCATION_NAME.Visible = true;
            this.LOCATION_NAME.VisibleIndex = 1;
            // 
            // STEP_NAME
            // 
            this.STEP_NAME.Caption = "工步";
            this.STEP_NAME.FieldName = "STEP_NAME";
            this.STEP_NAME.Name = "STEP_NAME";
            this.STEP_NAME.Visible = true;
            this.STEP_NAME.VisibleIndex = 2;
            // 
            // LOT_NUMBER
            // 
            this.LOT_NUMBER.Caption = "生产批次";
            this.LOT_NUMBER.FieldName = "LOT_NUMBER";
            this.LOT_NUMBER.Name = "LOT_NUMBER";
            this.LOT_NUMBER.Visible = true;
            this.LOT_NUMBER.VisibleIndex = 3;
            // 
            // PARAM_NAME
            // 
            this.PARAM_NAME.Caption = "采集参数";
            this.PARAM_NAME.FieldName = "PARAM_NAME";
            this.PARAM_NAME.Name = "PARAM_NAME";
            this.PARAM_NAME.Visible = true;
            this.PARAM_NAME.VisibleIndex = 4;
            // 
            // PARAM_VALUE
            // 
            this.PARAM_VALUE.Caption = "采集值";
            this.PARAM_VALUE.FieldName = "PARAM_VALUE";
            this.PARAM_VALUE.Name = "PARAM_VALUE";
            this.PARAM_VALUE.Visible = true;
            this.PARAM_VALUE.VisibleIndex = 5;
            // 
            // CREATE_TIME
            // 
            this.CREATE_TIME.Caption = "采集时间";
            this.CREATE_TIME.FieldName = "CREATE_TIME";
            this.CREATE_TIME.Name = "CREATE_TIME";
            this.CREATE_TIME.Visible = true;
            this.CREATE_TIME.VisibleIndex = 6;
            // 
            // ORDER_NUMBER
            // 
            this.ORDER_NUMBER.Caption = "工单号";
            this.ORDER_NUMBER.FieldName = "ORDER_NUMBER";
            this.ORDER_NUMBER.Name = "ORDER_NUMBER";
            this.ORDER_NUMBER.Visible = true;
            this.ORDER_NUMBER.VisibleIndex = 7;
            // 
            // SHIFT_VALUE
            // 
            this.SHIFT_VALUE.Caption = "班别";
            this.SHIFT_VALUE.FieldName = "SHIFT_VALUE";
            this.SHIFT_VALUE.Name = "SHIFT_VALUE";
            this.SHIFT_VALUE.Visible = true;
            this.SHIFT_VALUE.VisibleIndex = 8;
            // 
            // PART_NO
            // 
            this.PART_NO.Caption = "成品料号";
            this.PART_NO.FieldName = "PART_NO";
            this.PART_NO.Name = "PART_NO";
            this.PART_NO.Visible = true;
            this.PART_NO.VisibleIndex = 9;
            // 
            // PART_TYPE
            // 
            this.PART_TYPE.Caption = "产品类别";
            this.PART_TYPE.FieldName = "PART_TYPE";
            this.PART_TYPE.Name = "PART_TYPE";
            this.PART_TYPE.Visible = true;
            this.PART_TYPE.VisibleIndex = 10;
            // 
            // MATERIAL_LOT
            // 
            this.MATERIAL_LOT.Caption = "供应商批次";
            this.MATERIAL_LOT.FieldName = "MATERIAL_LOT";
            this.MATERIAL_LOT.Name = "MATERIAL_LOT";
            this.MATERIAL_LOT.Visible = true;
            this.MATERIAL_LOT.VisibleIndex = 11;
            // 
            // SUPPLIER
            // 
            this.SUPPLIER.Caption = "供应商";
            this.SUPPLIER.FieldName = "SUPPLIER";
            this.SUPPLIER.Name = "SUPPLIER";
            this.SUPPLIER.Visible = true;
            this.SUPPLIER.VisibleIndex = 12;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.Location = new System.Drawing.Point(579, 272);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 22);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DetailInformation
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 323);
            this.Controls.Add(this.xtraTabControl1);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "DetailInformation";
            this.Load += new System.EventHandler(this.PointInformation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraGrid.GridControl gcControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gvControl;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraGrid.Columns.GridColumn LOCATION_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn STEP_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn LOT_NUMBER;
        private DevExpress.XtraGrid.Columns.GridColumn PARAM_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn PARAM_VALUE;
        private DevExpress.XtraGrid.Columns.GridColumn CREATE_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn ORDER_NUMBER;
        private DevExpress.XtraGrid.Columns.GridColumn SHIFT_VALUE;
        private DevExpress.XtraGrid.Columns.GridColumn PART_NO;
        private DevExpress.XtraGrid.Columns.GridColumn PART_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn MATERIAL_LOT;
        private DevExpress.XtraGrid.Columns.GridColumn SUPPLIER;
        private DevExpress.XtraGrid.Columns.GridColumn ROWNUM;

    }
}