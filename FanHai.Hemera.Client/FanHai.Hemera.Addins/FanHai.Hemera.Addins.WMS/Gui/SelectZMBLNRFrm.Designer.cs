namespace FanHai.Hemera.Addins.WMS
{
    partial class SelectZMBLNRFrm
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
            this.gcShowZMBLNR = new DevExpress.XtraGrid.GridControl();
            this.gvShowZMBLNR = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gcShowZMBLNR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvShowZMBLNR)).BeginInit();
            this.SuspendLayout();
            // 
            // gcShowZMBLNR
            // 
            this.gcShowZMBLNR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcShowZMBLNR.Location = new System.Drawing.Point(0, 0);
            this.gcShowZMBLNR.MainView = this.gvShowZMBLNR;
            this.gcShowZMBLNR.Name = "gcShowZMBLNR";
            this.gcShowZMBLNR.Size = new System.Drawing.Size(394, 277);
            this.gcShowZMBLNR.TabIndex = 0;
            this.gcShowZMBLNR.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvShowZMBLNR});
            // 
            // gvShowZMBLNR
            // 
            this.gvShowZMBLNR.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6});
            this.gvShowZMBLNR.GridControl = this.gcShowZMBLNR;
            this.gvShowZMBLNR.Name = "gvShowZMBLNR";
            this.gvShowZMBLNR.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvShowZMBLNR.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvShowZMBLNR.OptionsBehavior.Editable = false;
            this.gvShowZMBLNR.OptionsBehavior.ReadOnly = true;
            this.gvShowZMBLNR.OptionsView.ColumnAutoWidth = false;
            this.gvShowZMBLNR.OptionsView.ShowGroupPanel = false;
            this.gvShowZMBLNR.OptionsView.ShowHorzLines = false;
            this.gvShowZMBLNR.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvShowZMBLNR_RowClick);
            this.gvShowZMBLNR.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gvShowZMBLNR_KeyPress);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "入库单号";
            this.gridColumn1.FieldName = "ZMBLNR";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 85;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "工厂";
            this.gridColumn2.FieldName = "WERKS";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 50;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "工单号";
            this.gridColumn4.FieldName = "AUFNR";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 80;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "创建日期";
            this.gridColumn5.FieldName = "CDATE";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            this.gridColumn5.Width = 80;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "创建者";
            this.gridColumn6.FieldName = "CREATOR";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            this.gridColumn6.Width = 70;
            // 
            // SelectZMBLNRFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 277);
            this.Controls.Add(this.gcShowZMBLNR);
            this.Name = "SelectZMBLNRFrm";
            ((System.ComponentModel.ISupportInitialize)(this.gcShowZMBLNR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvShowZMBLNR)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcShowZMBLNR;
        private DevExpress.XtraGrid.Views.Grid.GridView gvShowZMBLNR;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
    }
}