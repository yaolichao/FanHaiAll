namespace FanHai.Hemera.Addins.BasicData
{
    partial class ByProductForm
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
            this.gcByProduct = new DevExpress.XtraGrid.GridControl();
            this.gvByProduct = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcPartKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PART_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PART_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PART_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PART_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btSeach = new DevExpress.XtraEditors.SimpleButton();
            this.txtPartName = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcByProduct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvByProduct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPartName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.90909F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.09091F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(782, 488);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.gcByProduct);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(5, 53);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(5);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(772, 382);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "明细信息";
            // 
            // gcByProduct
            // 
            this.gcByProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcByProduct.Location = new System.Drawing.Point(2, 23);
            this.gcByProduct.MainView = this.gvByProduct;
            this.gcByProduct.Name = "gcByProduct";
            this.gcByProduct.Size = new System.Drawing.Size(768, 357);
            this.gcByProduct.TabIndex = 0;
            this.gcByProduct.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvByProduct});
            // 
            // gvByProduct
            // 
            this.gvByProduct.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcPartKey,
            this.gcNum,
            this.PART_ID,
            this.PART_NAME,
            this.PART_TYPE,
            this.PART_DESC});
            this.gvByProduct.GridControl = this.gcByProduct;
            this.gvByProduct.Name = "gvByProduct";
            this.gvByProduct.OptionsBehavior.Editable = false;
            this.gvByProduct.OptionsBehavior.ReadOnly = true;
            this.gvByProduct.OptionsView.ShowGroupPanel = false;
            // 
            // gcPartKey
            // 
            this.gcPartKey.Caption = "主键";
            this.gcPartKey.FieldName = "PART_KEY";
            this.gcPartKey.Name = "gcPartKey";
            // 
            // gcNum
            // 
            this.gcNum.Caption = "序号";
            this.gcNum.FieldName = "ROWNUM";
            this.gcNum.Name = "gcNum";
            this.gcNum.Visible = true;
            this.gcNum.VisibleIndex = 0;
            this.gcNum.Width = 51;
            // 
            // PART_ID
            // 
            this.PART_ID.Caption = "成品ID";
            this.PART_ID.FieldName = "PART_ID";
            this.PART_ID.Name = "PART_ID";
            this.PART_ID.Visible = true;
            this.PART_ID.VisibleIndex = 1;
            this.PART_ID.Width = 124;
            // 
            // PART_NAME
            // 
            this.PART_NAME.Caption = "成品料号";
            this.PART_NAME.FieldName = "PART_NAME";
            this.PART_NAME.Name = "PART_NAME";
            this.PART_NAME.Visible = true;
            this.PART_NAME.VisibleIndex = 2;
            this.PART_NAME.Width = 147;
            // 
            // PART_TYPE
            // 
            this.PART_TYPE.Caption = "类型";
            this.PART_TYPE.FieldName = "PART_TYPE";
            this.PART_TYPE.Name = "PART_TYPE";
            this.PART_TYPE.Visible = true;
            this.PART_TYPE.VisibleIndex = 3;
            this.PART_TYPE.Width = 66;
            // 
            // PART_DESC
            // 
            this.PART_DESC.Caption = "物料描述";
            this.PART_DESC.FieldName = "PART_DESC";
            this.PART_DESC.Name = "PART_DESC";
            this.PART_DESC.Visible = true;
            this.PART_DESC.VisibleIndex = 4;
            this.PART_DESC.Width = 363;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btSeach);
            this.layoutControl1.Controls.Add(this.txtPartName);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(3, 3);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(776, 42);
            this.layoutControl1.TabIndex = 1;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btSeach
            // 
            this.btSeach.Location = new System.Drawing.Point(682, 12);
            this.btSeach.Name = "btSeach";
            this.btSeach.Size = new System.Drawing.Size(87, 22);
            this.btSeach.StyleController = this.layoutControl1;
            this.btSeach.TabIndex = 7;
            this.btSeach.Text = "查询";
            this.btSeach.Click += new System.EventHandler(this.btSeach_Click);
            // 
            // txtPartName
            // 
            this.txtPartName.Location = new System.Drawing.Point(53, 12);
            this.txtPartName.Name = "txtPartName";
            this.txtPartName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPartName.Properties.Appearance.Options.UseFont = true;
            this.txtPartName.Size = new System.Drawing.Size(625, 23);
            this.txtPartName.StyleController = this.layoutControl1;
            this.txtPartName.TabIndex = 5;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 10, 5);
            this.layoutControlGroup1.Size = new System.Drawing.Size(776, 42);
            this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.Control = this.txtPartName;
            this.layoutControlItem2.CustomizationFormText = "物料号";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(675, 27);
            this.layoutControlItem2.Text = "物料号";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(42, 17);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btSeach;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(675, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(91, 27);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.tableLayoutPanel2);
            this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl2.Location = new System.Drawing.Point(3, 443);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.Root = this.layoutControlGroup2;
            this.layoutControl2.Size = new System.Drawing.Size(776, 42);
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
            this.tableLayoutPanel2.Size = new System.Drawing.Size(772, 38);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(666, 3);
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
            this.btnOk.Location = new System.Drawing.Point(510, 3);
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
            this.layoutControlGroup2.Size = new System.Drawing.Size(776, 42);
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
            this.layoutControlItem4.Size = new System.Drawing.Size(776, 42);
            this.layoutControlItem4.Text = "layoutControlItem4";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextToControlDistance = 0;
            this.layoutControlItem4.TextVisible = false;
            // 
            // ByProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 488);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ByProductForm";
            this.Text = "料号查询";
            this.Load += new System.EventHandler(this.ByProductForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcByProduct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvByProduct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPartName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
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
        private DevExpress.XtraGrid.GridControl gcByProduct;
        private DevExpress.XtraGrid.Views.Grid.GridView gvByProduct;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.TextEdit txtPartName;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraGrid.Columns.GridColumn PART_ID;
        private DevExpress.XtraGrid.Columns.GridColumn PART_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn PART_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn PART_DESC;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraGrid.Columns.GridColumn gcNum;
        private DevExpress.XtraGrid.Columns.GridColumn gcPartKey;
        private DevExpress.XtraEditors.SimpleButton btSeach;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}