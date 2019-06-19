namespace FanHai.Hemera.Addins.EMS
{
    partial class EquipmentChangeReasons
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tsOperation = new System.Windows.Forms.ToolStrip();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.grpChangeStates = new DevExpress.XtraEditors.GroupControl();
            this.grdChangeStates = new DevExpress.XtraGrid.GridControl();
            this.grdViewChangeStates = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grpChangeReasons = new DevExpress.XtraEditors.GroupControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.grdChangeReasons = new DevExpress.XtraGrid.GridControl();
            this.grdViewChangeReasons = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layOperation = new System.Windows.Forms.TableLayoutPanel();
            this.btnDelReason = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddReason = new DevExpress.XtraEditors.SimpleButton();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tsOperation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpChangeStates)).BeginInit();
            this.grpChangeStates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdChangeStates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewChangeStates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpChangeReasons)).BeginInit();
            this.grpChangeReasons.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdChangeReasons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewChangeReasons)).BeginInit();
            this.layOperation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.tsOperation, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.grpChangeStates, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.grpChangeReasons, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.pnlTitle, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(744, 610);
            this.tableLayoutPanelMain.TabIndex = 2;
            // 
            // tsOperation
            // 
            this.tsOperation.BackgroundImage = global::FanHai.Hemera.Addins.EMS.Properties.Resources.toolstrip_bk;
            this.tsOperation.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsOperation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRefresh,
            this.tsbEdit,
            this.tsbSave,
            this.tsbCancel});
            this.tsOperation.Location = new System.Drawing.Point(0, 0);
            this.tsOperation.Name = "tsOperation";
            this.tsOperation.Size = new System.Drawing.Size(744, 25);
            this.tsOperation.TabIndex = 1;
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.Image = global::FanHai.Hemera.Addins.EMS.Properties.Resources.arrow_refresh;
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(72, 22);
            this.tsbRefresh.Text = "Refresh";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // tsbEdit
            // 
            this.tsbEdit.Image = global::FanHai.Hemera.Addins.EMS.Properties.Resources.edit_save;
            this.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEdit.Name = "tsbEdit";
            this.tsbEdit.Size = new System.Drawing.Size(50, 22);
            this.tsbEdit.Text = "Edit";
            this.tsbEdit.Click += new System.EventHandler(this.tsbEdit_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.Image = global::FanHai.Hemera.Addins.EMS.Properties.Resources.save_accept;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(55, 22);
            this.tsbSave.Text = "Save";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbCancel
            // 
            this.tsbCancel.Image = global::FanHai.Hemera.Addins.EMS.Properties.Resources.cancel;
            this.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Size = new System.Drawing.Size(66, 22);
            this.tsbCancel.Text = "Cancel";
            this.tsbCancel.Click += new System.EventHandler(this.tsbCancel_Click);
            // 
            // grpChangeStates
            // 
            this.grpChangeStates.Controls.Add(this.grdChangeStates);
            this.grpChangeStates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpChangeStates.Location = new System.Drawing.Point(3, 73);
            this.grpChangeStates.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.grpChangeStates.Name = "grpChangeStates";
            this.grpChangeStates.Size = new System.Drawing.Size(738, 264);
            this.grpChangeStates.TabIndex = 2;
            // 
            // grdChangeStates
            // 
            this.grdChangeStates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdChangeStates.Location = new System.Drawing.Point(2, 23);
            this.grdChangeStates.LookAndFeel.SkinName = "Coffee";
            this.grdChangeStates.MainView = this.grdViewChangeStates;
            this.grdChangeStates.Name = "grdChangeStates";
            this.grdChangeStates.Size = new System.Drawing.Size(734, 239);
            this.grdChangeStates.TabIndex = 1;
            this.grdChangeStates.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewChangeStates});
            // 
            // grdViewChangeStates
            // 
            this.grdViewChangeStates.GridControl = this.grdChangeStates;
            this.grdViewChangeStates.Name = "grdViewChangeStates";
            this.grdViewChangeStates.OptionsCustomization.AllowColumnMoving = false;
            this.grdViewChangeStates.OptionsCustomization.AllowFilter = false;
            this.grdViewChangeStates.OptionsMenu.EnableColumnMenu = false;
            this.grdViewChangeStates.OptionsNavigation.AutoFocusNewRow = true;
            this.grdViewChangeStates.OptionsView.ShowGroupPanel = false;
            this.grdViewChangeStates.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.grdViewChangeStates_FocusedRowChanged);
            this.grdViewChangeStates.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.grdViewChangeStates_ValidateRow);
            // 
            // grpChangeReasons
            // 
            this.grpChangeReasons.Controls.Add(this.tableLayoutPanel2);
            this.grpChangeReasons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpChangeReasons.Location = new System.Drawing.Point(3, 343);
            this.grpChangeReasons.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.grpChangeReasons.Name = "grpChangeReasons";
            this.grpChangeReasons.Size = new System.Drawing.Size(738, 264);
            this.grpChangeReasons.TabIndex = 3;
            this.grpChangeReasons.Text = "Change Reasons";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.grdChangeReasons, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.layOperation, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 23);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(734, 239);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // grdChangeReasons
            // 
            this.grdChangeReasons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdChangeReasons.Location = new System.Drawing.Point(3, 40);
            this.grdChangeReasons.LookAndFeel.SkinName = "Coffee";
            this.grdChangeReasons.MainView = this.grdViewChangeReasons;
            this.grdChangeReasons.Name = "grdChangeReasons";
            this.grdChangeReasons.Size = new System.Drawing.Size(728, 196);
            this.grdChangeReasons.TabIndex = 2;
            this.grdChangeReasons.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewChangeReasons});
            // 
            // grdViewChangeReasons
            // 
            this.grdViewChangeReasons.GridControl = this.grdChangeReasons;
            this.grdViewChangeReasons.Name = "grdViewChangeReasons";
            this.grdViewChangeReasons.OptionsCustomization.AllowColumnMoving = false;
            this.grdViewChangeReasons.OptionsCustomization.AllowFilter = false;
            this.grdViewChangeReasons.OptionsMenu.EnableColumnMenu = false;
            this.grdViewChangeReasons.OptionsNavigation.AutoFocusNewRow = true;
            this.grdViewChangeReasons.OptionsView.ShowGroupPanel = false;
            this.grdViewChangeReasons.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.grdViewChangeReasons_ValidateRow);
            // 
            // layOperation
            // 
            this.layOperation.BackColor = System.Drawing.Color.Transparent;
            this.layOperation.ColumnCount = 3;
            this.layOperation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layOperation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.layOperation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.layOperation.Controls.Add(this.btnDelReason, 1, 0);
            this.layOperation.Controls.Add(this.btnAddReason, 1, 0);
            this.layOperation.Dock = System.Windows.Forms.DockStyle.Top;
            this.layOperation.Location = new System.Drawing.Point(3, 3);
            this.layOperation.Name = "layOperation";
            this.layOperation.RowCount = 1;
            this.layOperation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layOperation.Size = new System.Drawing.Size(728, 30);
            this.layOperation.TabIndex = 1;
            // 
            // btnDelReason
            // 
            this.btnDelReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelReason.Location = new System.Drawing.Point(614, 3);
            this.btnDelReason.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.btnDelReason.Name = "btnDelReason";
            this.btnDelReason.Size = new System.Drawing.Size(111, 24);
            this.btnDelReason.TabIndex = 1;
            this.btnDelReason.Text = "Delete Reason";
            this.btnDelReason.Click += new System.EventHandler(this.btnDelReason_Click);
            // 
            // btnAddReason
            // 
            this.btnAddReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddReason.Location = new System.Drawing.Point(497, 3);
            this.btnAddReason.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.btnAddReason.Name = "btnAddReason";
            this.btnAddReason.Size = new System.Drawing.Size(111, 24);
            this.btnAddReason.TabIndex = 0;
            this.btnAddReason.Text = "Add Reason";
            this.btnAddReason.Click += new System.EventHandler(this.btnAddReason_Click);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitle.Location = new System.Drawing.Point(3, 28);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Padding = new System.Windows.Forms.Padding(5);
            this.pnlTitle.Size = new System.Drawing.Size(738, 39);
            this.pnlTitle.TabIndex = 4;
            // 
            // lblTitle
            // 
            this.lblTitle.Appearance.Font = new System.Drawing.Font("Arial", 16F);
            this.lblTitle.Appearance.Options.UseFont = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Location = new System.Drawing.Point(8, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(265, 24);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Equipment Change Reasons";
            // 
            // EquipmentChangeReasons
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Name = "EquipmentChangeReasons";
            this.Size = new System.Drawing.Size(744, 610);
            this.Load += new System.EventHandler(this.EquipmentChangeReasons_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.tsOperation.ResumeLayout(false);
            this.tsOperation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpChangeStates)).EndInit();
            this.grpChangeStates.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdChangeStates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewChangeStates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpChangeReasons)).EndInit();
            this.grpChangeReasons.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdChangeReasons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewChangeReasons)).EndInit();
            this.layOperation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ToolStrip tsOperation;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbCancel;
        private DevExpress.XtraEditors.GroupControl grpChangeStates;
        private DevExpress.XtraGrid.GridControl grdChangeStates;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewChangeStates;
        private DevExpress.XtraEditors.GroupControl grpChangeReasons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraGrid.GridControl grdChangeReasons;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewChangeReasons;
        private System.Windows.Forms.TableLayoutPanel layOperation;
        private DevExpress.XtraEditors.SimpleButton btnDelReason;
        private DevExpress.XtraEditors.SimpleButton btnAddReason;
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private DevExpress.XtraEditors.LabelControl lblTitle;


    }
}
