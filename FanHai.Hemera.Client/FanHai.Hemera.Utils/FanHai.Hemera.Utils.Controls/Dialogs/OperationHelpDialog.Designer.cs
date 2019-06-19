namespace FanHai.Hemera.Utils.Dialogs
{
    partial class OperationHelpDialog
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
            this.gcERS = new DevExpress.XtraGrid.GridControl();
            this.gvERS = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.enterpriseKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.enterpriseName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.routeKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ROUTE_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.stepKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.stepName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.enterpriseVersion = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gcERS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvERS)).BeginInit();
            this.SuspendLayout();
            // 
            // gcERS
            // 
            this.gcERS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcERS.Location = new System.Drawing.Point(0, 0);
            this.gcERS.LookAndFeel.SkinName = "Coffee";
            this.gcERS.MainView = this.gvERS;
            this.gcERS.Name = "gcERS";
            this.gcERS.Size = new System.Drawing.Size(403, 277);
            this.gcERS.TabIndex = 0;
            this.gcERS.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvERS});
            // 
            // gvERS
            // 
            this.gvERS.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.enterpriseKey,
            this.enterpriseName,
            this.routeKey,
            this.ROUTE_NAME,
            this.stepKey,
            this.stepName,
            this.enterpriseVersion});
            this.gvERS.GridControl = this.gcERS;
            this.gvERS.GroupCount = 2;
            this.gvERS.Name = "gvERS";
            this.gvERS.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.enterpriseName, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.ROUTE_NAME, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gvERS.DoubleClick += new System.EventHandler(this.ERSView_DoubleClick);
            // 
            // enterpriseKey
            // 
            this.enterpriseKey.Caption = "gridColumn1";
            this.enterpriseKey.FieldName = "ROUTE_ENTERPRISE_VER_KEY";
            this.enterpriseKey.Name = "enterpriseKey";
            // 
            // enterpriseName
            // 
            this.enterpriseName.Caption = "工艺流程组名称";
            this.enterpriseName.FieldName = "ENTERPRISE_NAME";
            this.enterpriseName.Name = "enterpriseName";
            this.enterpriseName.OptionsColumn.AllowEdit = false;
            this.enterpriseName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            // 
            // routeKey
            // 
            this.routeKey.Caption = "gridColumn3";
            this.routeKey.FieldName = "ROUTE_ROUTE_VER_KEY";
            this.routeKey.Name = "routeKey";
            // 
            // ROUTE_NAME
            // 
            this.ROUTE_NAME.Caption = "工艺流程名称";
            this.ROUTE_NAME.FieldName = "ROUTE_NAME";
            this.ROUTE_NAME.Name = "ROUTE_NAME";
            this.ROUTE_NAME.OptionsColumn.AllowEdit = false;
            this.ROUTE_NAME.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            // 
            // stepKey
            // 
            this.stepKey.Caption = "gridColumn5";
            this.stepKey.FieldName = "ROUTE_STEP_KEY";
            this.stepKey.Name = "stepKey";
            // 
            // stepName
            // 
            this.stepName.Caption = "工序名称";
            this.stepName.FieldName = "ROUTE_STEP_NAME";
            this.stepName.Name = "stepName";
            this.stepName.OptionsColumn.AllowEdit = false;
            this.stepName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.stepName.Visible = true;
            this.stepName.VisibleIndex = 0;
            // 
            // enterpriseVersion
            // 
            this.enterpriseVersion.Caption = "gridColumn1";
            this.enterpriseVersion.FieldName = "ENTERPRISE_VERSION";
            this.enterpriseVersion.Name = "enterpriseVersion";
            // 
            // OperationHelpDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(403, 277);
            this.ControlBox = false;
            this.Controls.Add(this.gcERS);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OperationHelpDialog";
            this.Deactivate += new System.EventHandler(this.OperationHelpDialog_Deactivate);
            this.Load += new System.EventHandler(this.OperationHelpDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcERS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvERS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcERS;
        private DevExpress.XtraGrid.Views.Grid.GridView gvERS;
        private DevExpress.XtraGrid.Columns.GridColumn enterpriseKey;
        private DevExpress.XtraGrid.Columns.GridColumn enterpriseName;
        private DevExpress.XtraGrid.Columns.GridColumn routeKey;
        private DevExpress.XtraGrid.Columns.GridColumn ROUTE_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn stepKey;
        private DevExpress.XtraGrid.Columns.GridColumn stepName;
        private DevExpress.XtraGrid.Columns.GridColumn enterpriseVersion;
    }
}