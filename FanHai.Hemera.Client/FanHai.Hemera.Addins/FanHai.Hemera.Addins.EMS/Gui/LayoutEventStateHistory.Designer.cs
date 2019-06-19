namespace FanHai.Hemera.Addins.EMS.Gui
{
    partial class LayoutEventStateHistory
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
            this.eventHistory = new DevExpress.XtraTab.XtraTabPage();
            this.gcEvents = new DevExpress.XtraGrid.GridControl();
            this.gvEvents = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.EQUIPMENT_STATE_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CREATE_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EDIT_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CONTINUEHOURS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CREATOR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.workHistory = new DevExpress.XtraTab.XtraTabPage();
            this.gcDoWorkHistory = new DevExpress.XtraGrid.GridControl();
            this.gvDoWorkHistory = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.LOT_NUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PART_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.QUANTITY_IN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.IN_EDIT_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MOVE_IN_OPERATOR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.QUANTITY_OUT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.OUT_EDIT_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MOVE_OUT_OPERATOR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOSS_QUANTITY_OUT = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.eventHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcEvents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEvents)).BeginInit();
            this.workHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDoWorkHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDoWorkHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.eventHistory;
            this.xtraTabControl1.Size = new System.Drawing.Size(808, 408);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.eventHistory,
            this.workHistory});
            // 
            // eventHistory
            // 
            this.eventHistory.Controls.Add(this.gcEvents);
            this.eventHistory.Name = "eventHistory";
            this.eventHistory.Size = new System.Drawing.Size(804, 382);
            this.eventHistory.Text = "事件历史";
            // 
            // gcEvents
            // 
            this.gcEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcEvents.Location = new System.Drawing.Point(0, 0);
            this.gcEvents.MainView = this.gvEvents;
            this.gcEvents.Name = "gcEvents";
            this.gcEvents.Size = new System.Drawing.Size(804, 382);
            this.gcEvents.TabIndex = 0;
            this.gcEvents.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvEvents});
            // 
            // gvEvents
            // 
            this.gvEvents.ColumnPanelRowHeight = 30;
            this.gvEvents.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.EQUIPMENT_STATE_NAME,
            this.CREATE_TIME,
            this.EDIT_TIME,
            this.CONTINUEHOURS,
            this.CREATOR});
            this.gvEvents.GridControl = this.gcEvents;
            this.gvEvents.Name = "gvEvents";
            this.gvEvents.OptionsBehavior.Editable = false;
            // 
            // EQUIPMENT_STATE_NAME
            // 
            this.EQUIPMENT_STATE_NAME.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.EQUIPMENT_STATE_NAME.AppearanceHeader.Options.UseFont = true;
            this.EQUIPMENT_STATE_NAME.Caption = "设备状态";
            this.EQUIPMENT_STATE_NAME.FieldName = "EQUIPMENT_STATE_NAME";
            this.EQUIPMENT_STATE_NAME.Name = "EQUIPMENT_STATE_NAME";
            this.EQUIPMENT_STATE_NAME.Visible = true;
            this.EQUIPMENT_STATE_NAME.VisibleIndex = 0;
            // 
            // CREATE_TIME
            // 
            this.CREATE_TIME.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.CREATE_TIME.AppearanceHeader.Options.UseFont = true;
            this.CREATE_TIME.Caption = "开始时间";
            this.CREATE_TIME.FieldName = "START_DATE";
            this.CREATE_TIME.Name = "CREATE_TIME";
            this.CREATE_TIME.Visible = true;
            this.CREATE_TIME.VisibleIndex = 1;
            // 
            // EDIT_TIME
            // 
            this.EDIT_TIME.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.EDIT_TIME.AppearanceHeader.Options.UseFont = true;
            this.EDIT_TIME.Caption = "结束时间";
            this.EDIT_TIME.FieldName = "END_DATE";
            this.EDIT_TIME.Name = "EDIT_TIME";
            this.EDIT_TIME.Visible = true;
            this.EDIT_TIME.VisibleIndex = 2;
            // 
            // CONTINUEHOURS
            // 
            this.CONTINUEHOURS.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.CONTINUEHOURS.AppearanceHeader.Options.UseFont = true;
            this.CONTINUEHOURS.Caption = "状态保持时间(小时)";
            this.CONTINUEHOURS.FieldName = "CONTINUEHOURS";
            this.CONTINUEHOURS.Name = "CONTINUEHOURS";
            this.CONTINUEHOURS.Visible = true;
            this.CONTINUEHOURS.VisibleIndex = 3;
            // 
            // CREATOR
            // 
            this.CREATOR.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.CREATOR.AppearanceHeader.Options.UseFont = true;
            this.CREATOR.Caption = "操作者";
            this.CREATOR.FieldName = "CREATOR";
            this.CREATOR.Name = "CREATOR";
            this.CREATOR.Visible = true;
            this.CREATOR.VisibleIndex = 4;
            // 
            // workHistory
            // 
            this.workHistory.Controls.Add(this.gcDoWorkHistory);
            this.workHistory.Name = "workHistory";
            this.workHistory.Size = new System.Drawing.Size(804, 382);
            this.workHistory.Text = "加工历史";
            // 
            // gcDoWorkHistory
            // 
            this.gcDoWorkHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDoWorkHistory.Location = new System.Drawing.Point(0, 0);
            this.gcDoWorkHistory.MainView = this.gvDoWorkHistory;
            this.gcDoWorkHistory.Name = "gcDoWorkHistory";
            this.gcDoWorkHistory.Size = new System.Drawing.Size(804, 382);
            this.gcDoWorkHistory.TabIndex = 0;
            this.gcDoWorkHistory.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDoWorkHistory});
            // 
            // gvDoWorkHistory
            // 
            this.gvDoWorkHistory.ColumnPanelRowHeight = 30;
            this.gvDoWorkHistory.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.LOT_NUMBER,
            this.PART_NAME,
            this.QUANTITY_IN,
            this.IN_EDIT_TIME,
            this.MOVE_IN_OPERATOR,
            this.QUANTITY_OUT,
            this.OUT_EDIT_TIME,
            this.MOVE_OUT_OPERATOR,
            this.LOSS_QUANTITY_OUT});
            this.gvDoWorkHistory.GridControl = this.gcDoWorkHistory;
            this.gvDoWorkHistory.Name = "gvDoWorkHistory";
            this.gvDoWorkHistory.OptionsBehavior.Editable = false;
            // 
            // LOT_NUMBER
            // 
            this.LOT_NUMBER.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LOT_NUMBER.AppearanceHeader.Options.UseFont = true;
            this.LOT_NUMBER.Caption = "批次号";
            this.LOT_NUMBER.FieldName = "LOT_NUMBER";
            this.LOT_NUMBER.Name = "LOT_NUMBER";
            this.LOT_NUMBER.Visible = true;
            this.LOT_NUMBER.VisibleIndex = 0;
            // 
            // PART_NAME
            // 
            this.PART_NAME.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10F);
            this.PART_NAME.AppearanceHeader.Options.UseFont = true;
            this.PART_NAME.Caption = "产品";
            this.PART_NAME.FieldName = "PART_NAME";
            this.PART_NAME.Name = "PART_NAME";
            this.PART_NAME.Visible = true;
            this.PART_NAME.VisibleIndex = 1;
            // 
            // QUANTITY_IN
            // 
            this.QUANTITY_IN.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10F);
            this.QUANTITY_IN.AppearanceHeader.Options.UseFont = true;
            this.QUANTITY_IN.Caption = "送入数";
            this.QUANTITY_IN.FieldName = "QUANTITY_IN";
            this.QUANTITY_IN.Name = "QUANTITY_IN";
            this.QUANTITY_IN.Visible = true;
            this.QUANTITY_IN.VisibleIndex = 2;
            // 
            // IN_EDIT_TIME
            // 
            this.IN_EDIT_TIME.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10F);
            this.IN_EDIT_TIME.AppearanceHeader.Options.UseFont = true;
            this.IN_EDIT_TIME.Caption = "送入时间";
            this.IN_EDIT_TIME.FieldName = "IN_EDIT_TIME";
            this.IN_EDIT_TIME.Name = "IN_EDIT_TIME";
            this.IN_EDIT_TIME.Visible = true;
            this.IN_EDIT_TIME.VisibleIndex = 3;
            // 
            // MOVE_IN_OPERATOR
            // 
            this.MOVE_IN_OPERATOR.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10F);
            this.MOVE_IN_OPERATOR.AppearanceHeader.Options.UseFont = true;
            this.MOVE_IN_OPERATOR.Caption = "送入操作者";
            this.MOVE_IN_OPERATOR.FieldName = "MOVE_IN_OPERATOR";
            this.MOVE_IN_OPERATOR.Name = "MOVE_IN_OPERATOR";
            this.MOVE_IN_OPERATOR.Visible = true;
            this.MOVE_IN_OPERATOR.VisibleIndex = 4;
            // 
            // QUANTITY_OUT
            // 
            this.QUANTITY_OUT.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10F);
            this.QUANTITY_OUT.AppearanceHeader.Options.UseFont = true;
            this.QUANTITY_OUT.Caption = "送出数";
            this.QUANTITY_OUT.FieldName = "QUANTITY_OUT";
            this.QUANTITY_OUT.Name = "QUANTITY_OUT";
            this.QUANTITY_OUT.Visible = true;
            this.QUANTITY_OUT.VisibleIndex = 5;
            // 
            // OUT_EDIT_TIME
            // 
            this.OUT_EDIT_TIME.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10F);
            this.OUT_EDIT_TIME.AppearanceHeader.Options.UseFont = true;
            this.OUT_EDIT_TIME.Caption = "送出时间";
            this.OUT_EDIT_TIME.FieldName = "OUT_EDIT_TIME";
            this.OUT_EDIT_TIME.Name = "OUT_EDIT_TIME";
            this.OUT_EDIT_TIME.Visible = true;
            this.OUT_EDIT_TIME.VisibleIndex = 6;
            // 
            // MOVE_OUT_OPERATOR
            // 
            this.MOVE_OUT_OPERATOR.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10F);
            this.MOVE_OUT_OPERATOR.AppearanceHeader.Options.UseFont = true;
            this.MOVE_OUT_OPERATOR.Caption = "送出操作者";
            this.MOVE_OUT_OPERATOR.FieldName = "MOVE_OUT_OPERATOR";
            this.MOVE_OUT_OPERATOR.Name = "MOVE_OUT_OPERATOR";
            this.MOVE_OUT_OPERATOR.Visible = true;
            this.MOVE_OUT_OPERATOR.VisibleIndex = 7;
            // 
            // LOSS_QUANTITY_OUT
            // 
            this.LOSS_QUANTITY_OUT.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LOSS_QUANTITY_OUT.AppearanceHeader.Options.UseFont = true;
            this.LOSS_QUANTITY_OUT.Caption = "报废数";
            this.LOSS_QUANTITY_OUT.FieldName = "LOSS_QUANTITY_OUT";
            this.LOSS_QUANTITY_OUT.Name = "LOSS_QUANTITY_OUT";
            this.LOSS_QUANTITY_OUT.Visible = true;
            this.LOSS_QUANTITY_OUT.VisibleIndex = 8;
            // 
            // LayoutEventStateHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 408);
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "LayoutEventStateHistory";
            this.Text = "LayoutEventStateHistory";
            this.Load += new System.EventHandler(this.LayoutEventStateHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.eventHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcEvents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEvents)).EndInit();
            this.workHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDoWorkHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDoWorkHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage eventHistory;
        private DevExpress.XtraTab.XtraTabPage workHistory;
        private DevExpress.XtraGrid.GridControl gcEvents;
        private DevExpress.XtraGrid.Views.Grid.GridView gvEvents;
        private DevExpress.XtraGrid.GridControl gcDoWorkHistory;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDoWorkHistory;
        private DevExpress.XtraGrid.Columns.GridColumn EQUIPMENT_STATE_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn CREATE_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn EDIT_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn CONTINUEHOURS;
        private DevExpress.XtraGrid.Columns.GridColumn CREATOR;
        private DevExpress.XtraGrid.Columns.GridColumn LOT_NUMBER;
        private DevExpress.XtraGrid.Columns.GridColumn PART_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn QUANTITY_IN;
        private DevExpress.XtraGrid.Columns.GridColumn IN_EDIT_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn MOVE_IN_OPERATOR;
        private DevExpress.XtraGrid.Columns.GridColumn QUANTITY_OUT;
        private DevExpress.XtraGrid.Columns.GridColumn OUT_EDIT_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn MOVE_OUT_OPERATOR;
        private DevExpress.XtraGrid.Columns.GridColumn LOSS_QUANTITY_OUT;
    }
}