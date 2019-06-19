namespace FanHai.Hemera.Addins.WIP
{
    partial class EquipmentPickDialog
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
            this.grpEquipmentList = new DevExpress.XtraEditors.GroupControl();
            this.grdEquipments = new DevExpress.XtraGrid.GridControl();
            this.grdViewEquipments = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcEquipmentKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcEquipmentName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcEquipmentCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcEquipmentState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcLineName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcLineKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlBottom = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.grpEquipmentList)).BeginInit();
            this.grpEquipmentList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEquipments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewEquipments)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottom)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpEquipmentList
            // 
            this.grpEquipmentList.Controls.Add(this.grdEquipments);
            this.grpEquipmentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEquipmentList.Location = new System.Drawing.Point(3, 3);
            this.grpEquipmentList.Name = "grpEquipmentList";
            this.grpEquipmentList.Size = new System.Drawing.Size(877, 438);
            this.grpEquipmentList.TabIndex = 59;
            this.grpEquipmentList.Text = "设备列表";
            // 
            // grdEquipments
            // 
            this.grdEquipments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdEquipments.Location = new System.Drawing.Point(2, 23);
            this.grdEquipments.LookAndFeel.SkinName = "Coffee";
            this.grdEquipments.MainView = this.grdViewEquipments;
            this.grdEquipments.Name = "grdEquipments";
            this.grdEquipments.Size = new System.Drawing.Size(873, 413);
            this.grdEquipments.TabIndex = 4;
            this.grdEquipments.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewEquipments});
            this.grdEquipments.DoubleClick += new System.EventHandler(this.grdEquipments_DoubleClick);
            // 
            // grdViewEquipments
            // 
            this.grdViewEquipments.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcEquipmentKey,
            this.gcEquipmentName,
            this.gcEquipmentCode,
            this.gcEquipmentState,
            this.gcLineName,
            this.gcLineKey});
            this.grdViewEquipments.GridControl = this.grdEquipments;
            this.grdViewEquipments.Name = "grdViewEquipments";
            this.grdViewEquipments.OptionsBehavior.Editable = false;
            this.grdViewEquipments.OptionsBehavior.ReadOnly = true;
            this.grdViewEquipments.OptionsView.RowAutoHeight = true;
            this.grdViewEquipments.OptionsView.ShowGroupPanel = false;
            this.grdViewEquipments.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.grdViewEquipments_CustomDrawCell);
            // 
            // gcEquipmentKey
            // 
            this.gcEquipmentKey.Caption = "设备主键";
            this.gcEquipmentKey.FieldName = "EQUIPMENT_KEY";
            this.gcEquipmentKey.Name = "gcEquipmentKey";
            // 
            // gcEquipmentName
            // 
            this.gcEquipmentName.Caption = "设备名称";
            this.gcEquipmentName.FieldName = "EQUIPMENT_NAME";
            this.gcEquipmentName.Name = "gcEquipmentName";
            this.gcEquipmentName.Visible = true;
            this.gcEquipmentName.VisibleIndex = 0;
            // 
            // gcEquipmentCode
            // 
            this.gcEquipmentCode.Caption = "设备代码";
            this.gcEquipmentCode.FieldName = "EQUIPMENT_CODE";
            this.gcEquipmentCode.Name = "gcEquipmentCode";
            this.gcEquipmentCode.Visible = true;
            this.gcEquipmentCode.VisibleIndex = 1;
            // 
            // gcEquipmentState
            // 
            this.gcEquipmentState.Caption = "设备状态";
            this.gcEquipmentState.FieldName = "EQUIPMENT_STATE_NAME";
            this.gcEquipmentState.Name = "gcEquipmentState";
            this.gcEquipmentState.Visible = true;
            this.gcEquipmentState.VisibleIndex = 2;
            // 
            // gcLineName
            // 
            this.gcLineName.Caption = "线别名称";
            this.gcLineName.FieldName = "LINE_NAME";
            this.gcLineName.Name = "gcLineName";
            this.gcLineName.Visible = true;
            this.gcLineName.VisibleIndex = 3;
            // 
            // gcLineKey
            // 
            this.gcLineKey.Caption = "线别主键";
            this.gcLineKey.FieldName = "LINE_KEY";
            this.gcLineKey.Name = "gcLineKey";
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.grpEquipmentList, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.pnlBottom, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(883, 492);
            this.tableLayoutPanelMain.TabIndex = 61;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnConfirm);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(3, 447);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(877, 42);
            this.pnlBottom.TabIndex = 82;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(779, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(679, 8);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(90, 25);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // EquipmentPickDialog
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(883, 492);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "EquipmentPickDialog";
            this.ShowInTaskbar = false;
            this.Text = "选择设备";
            this.Load += new System.EventHandler(this.EquipmentQuery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpEquipmentList)).EndInit();
            this.grpEquipmentList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdEquipments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewEquipments)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottom)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl grpEquipmentList;
        private DevExpress.XtraGrid.GridControl grdEquipments;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewEquipments;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.PanelControl pnlBottom;
        private DevExpress.XtraGrid.Columns.GridColumn gcEquipmentKey;
        private DevExpress.XtraGrid.Columns.GridColumn gcEquipmentName;
        private DevExpress.XtraGrid.Columns.GridColumn gcEquipmentCode;
        private DevExpress.XtraGrid.Columns.GridColumn gcEquipmentState;
        private DevExpress.XtraGrid.Columns.GridColumn gcLineName;
        private DevExpress.XtraGrid.Columns.GridColumn gcLineKey;
    }
}