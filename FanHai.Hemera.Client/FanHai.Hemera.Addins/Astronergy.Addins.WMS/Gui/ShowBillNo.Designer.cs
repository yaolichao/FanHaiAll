namespace Astronergy.Addins.WMS.Gui
{
    partial class ShowBillNo
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gcShowBillNo = new DevExpress.XtraGrid.GridControl();
            this.gvShowBillNo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colVBELN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOUTBANDNO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSTATUS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcShowBillNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvShowBillNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.gcShowBillNo);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(841, 345);
            this.layoutControl1.TabIndex = 1;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gcShowBillNo
            // 
            this.gcShowBillNo.Location = new System.Drawing.Point(2, 2);
            this.gcShowBillNo.MainView = this.gvShowBillNo;
            this.gcShowBillNo.Name = "gcShowBillNo";
            this.gcShowBillNo.Size = new System.Drawing.Size(837, 341);
            this.gcShowBillNo.TabIndex = 6;
            this.gcShowBillNo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvShowBillNo});
            // 
            // gvShowBillNo
            // 
            this.gvShowBillNo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colVBELN,
            this.colOUTBANDNO,
            this.colSTATUS,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8});
            this.gvShowBillNo.GridControl = this.gcShowBillNo;
            this.gvShowBillNo.Name = "gvShowBillNo";
            this.gvShowBillNo.OptionsView.ShowGroupPanel = false;
            this.gvShowBillNo.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvShowBillNo_RowClick);
            this.gvShowBillNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gvShowBillNo_KeyPress);
            // 
            // colVBELN
            // 
            this.colVBELN.Caption = "外向交货单号";
            this.colVBELN.FieldName = "VBELN";
            this.colVBELN.Name = "colVBELN";
            this.colVBELN.OptionsColumn.AllowEdit = false;
            this.colVBELN.OptionsColumn.ReadOnly = true;
            this.colVBELN.Visible = true;
            this.colVBELN.VisibleIndex = 0;
            // 
            // colOUTBANDNO
            // 
            this.colOUTBANDNO.Caption = "出货单号";
            this.colOUTBANDNO.FieldName = "OUTBANDNO";
            this.colOUTBANDNO.Name = "colOUTBANDNO";
            this.colOUTBANDNO.OptionsColumn.AllowEdit = false;
            this.colOUTBANDNO.OptionsColumn.ReadOnly = true;
            this.colOUTBANDNO.Visible = true;
            this.colOUTBANDNO.VisibleIndex = 1;
            // 
            // colSTATUS
            // 
            this.colSTATUS.Caption = "状态";
            this.colSTATUS.FieldName = "STATUS";
            this.colSTATUS.Name = "colSTATUS";
            this.colSTATUS.OptionsColumn.AllowEdit = false;
            this.colSTATUS.OptionsColumn.ReadOnly = true;
            this.colSTATUS.Visible = true;
            this.colSTATUS.VisibleIndex = 2;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "售达方";
            this.gridColumn5.FieldName = "SALESTO";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "售达方地址";
            this.gridColumn6.FieldName = "SALESTO_NAME";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "送达方";
            this.gridColumn7.FieldName = "SHIPTO";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 5;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "送达方名称";
            this.gridColumn8.FieldName = "SHIPTO_NAME";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 6;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(841, 345);
            this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.gcShowBillNo;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(841, 345);
            this.layoutControlItem3.Text = "layoutControlItem3";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // ShowBillNo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 345);
            this.Controls.Add(this.layoutControl1);
            this.Name = "ShowBillNo";
            this.Text = "ShowBillNo";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcShowBillNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvShowBillNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraGrid.GridControl gcShowBillNo;
        private DevExpress.XtraGrid.Views.Grid.GridView gvShowBillNo;
        private DevExpress.XtraGrid.Columns.GridColumn colVBELN;
        private DevExpress.XtraGrid.Columns.GridColumn colOUTBANDNO;
        private DevExpress.XtraGrid.Columns.GridColumn colSTATUS;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
    }
}