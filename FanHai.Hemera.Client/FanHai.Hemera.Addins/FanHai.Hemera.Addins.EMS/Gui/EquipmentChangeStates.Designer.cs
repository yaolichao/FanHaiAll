namespace FanHai.Hemera.Addins.EMS
{
    partial class EquipmentChangeStates
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
            this.toolTipChangeStateName = new System.Windows.Forms.ToolTip();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.grpChangeStates = new DevExpress.XtraEditors.GroupControl();
            this.grdChangeStates = new DevExpress.XtraGrid.GridControl();
            this.bandedGridViewChangeStates = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.tsbCancel = new DevExpress.XtraEditors.SimpleButton();
            this.tsbSave = new DevExpress.XtraEditors.SimpleButton();
            this.tsbEdit = new DevExpress.XtraEditors.SimpleButton();
            this.tsbRefresh = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpChangeStates)).BeginInit();
            this.grpChangeStates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdChangeStates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridViewChangeStates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(725, 47);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(540, 0);
            this.lblInfos.Size = new System.Drawing.Size(185, 42);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t登录时间：2019-01-15 17:18:52";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            this.lblMenu.Size = new System.Drawing.Size(270, 23);
            this.lblMenu.Text = "设备管理->设备->设备转变状态";
            // 
            // toolTipChangeStateName
            // 
            this.toolTipChangeStateName.AutoPopDelay = 5000;
            this.toolTipChangeStateName.InitialDelay = 500;
            this.toolTipChangeStateName.ReshowDelay = 100;
            this.toolTipChangeStateName.ShowAlways = true;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.grpChangeStates, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.pnlTitle, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(1, 47);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(725, 457);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // grpChangeStates
            // 
            this.grpChangeStates.Controls.Add(this.grdChangeStates);
            this.grpChangeStates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpChangeStates.Location = new System.Drawing.Point(3, 48);
            this.grpChangeStates.Name = "grpChangeStates";
            this.grpChangeStates.Size = new System.Drawing.Size(719, 406);
            this.grpChangeStates.TabIndex = 2;
            // 
            // grdChangeStates
            // 
            this.grdChangeStates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdChangeStates.Location = new System.Drawing.Point(2, 23);
            this.grdChangeStates.LookAndFeel.SkinName = "Coffee";
            this.grdChangeStates.MainView = this.bandedGridViewChangeStates;
            this.grdChangeStates.Name = "grdChangeStates";
            this.grdChangeStates.Size = new System.Drawing.Size(715, 381);
            this.grdChangeStates.TabIndex = 1;
            this.grdChangeStates.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bandedGridViewChangeStates});
            this.grdChangeStates.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdChangeStates_MouseMove);
            // 
            // bandedGridViewChangeStates
            // 
            this.bandedGridViewChangeStates.GridControl = this.grdChangeStates;
            this.bandedGridViewChangeStates.Name = "bandedGridViewChangeStates";
            this.bandedGridViewChangeStates.OptionsBehavior.Editable = false;
            this.bandedGridViewChangeStates.OptionsMenu.EnableColumnMenu = false;
            this.bandedGridViewChangeStates.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.bandedGridViewChangeStates.OptionsView.AllowCellMerge = true;
            this.bandedGridViewChangeStates.OptionsView.ShowColumnHeaders = false;
            this.bandedGridViewChangeStates.OptionsView.ShowGroupPanel = false;
            this.bandedGridViewChangeStates.OptionsView.ShowIndicator = false;
            this.bandedGridViewChangeStates.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.bandedGridViewChangeStates_RowCellClick);
            this.bandedGridViewChangeStates.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.bandedGridViewChangeStates_CustomDrawCell);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Controls.Add(this.tsbCancel);
            this.pnlTitle.Controls.Add(this.tsbSave);
            this.pnlTitle.Controls.Add(this.tsbEdit);
            this.pnlTitle.Controls.Add(this.tsbRefresh);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitle.Location = new System.Drawing.Point(3, 3);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Padding = new System.Windows.Forms.Padding(5);
            this.pnlTitle.Size = new System.Drawing.Size(719, 39);
            this.pnlTitle.TabIndex = 3;
            // 
            // tsbCancel
            // 
            this.tsbCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tsbCancel.Location = new System.Drawing.Point(627, 8);
            this.tsbCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Size = new System.Drawing.Size(61, 23);
            this.tsbCancel.TabIndex = 3;
            this.tsbCancel.Text = "Cancel";
            this.tsbCancel.Click += new System.EventHandler(this.tsbCancel_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tsbSave.Location = new System.Drawing.Point(552, 8);
            this.tsbSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(61, 23);
            this.tsbSave.TabIndex = 2;
            this.tsbSave.Text = "Save";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbEdit
            // 
            this.tsbEdit.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tsbEdit.Location = new System.Drawing.Point(477, 8);
            this.tsbEdit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbEdit.Name = "tsbEdit";
            this.tsbEdit.Size = new System.Drawing.Size(61, 23);
            this.tsbEdit.TabIndex = 1;
            this.tsbEdit.Text = "Edit";
            this.tsbEdit.Click += new System.EventHandler(this.tsbEdit_Click);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tsbRefresh.Location = new System.Drawing.Point(401, 8);
            this.tsbRefresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(61, 23);
            this.tsbRefresh.TabIndex = 0;
            this.tsbRefresh.Text = "Refresh";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // EquipmentChangeStates
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "EquipmentChangeStates";
            this.Size = new System.Drawing.Size(727, 504);
            this.Load += new System.EventHandler(this.EquipmentChangeStates_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpChangeStates)).EndInit();
            this.grpChangeStates.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdChangeStates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridViewChangeStates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTipChangeStateName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraEditors.GroupControl grpChangeStates;
        private DevExpress.XtraGrid.GridControl grdChangeStates;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedGridViewChangeStates;
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private DevExpress.XtraEditors.SimpleButton tsbCancel;
        private DevExpress.XtraEditors.SimpleButton tsbSave;
        private DevExpress.XtraEditors.SimpleButton tsbEdit;
        private DevExpress.XtraEditors.SimpleButton tsbRefresh;
    }
}
