namespace SolarViewer.Hemera.Addins.SPC
{
    partial class ParamSearchDialog
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
            this.gridColumn_paramName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.gridColumn_paramKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridViewParam = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_paramDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdCtrlParam = new DevExpress.XtraGrid.GridControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.txtParamName = new DevExpress.XtraEditors.TextEdit();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.layoutControlMain = new DevExpress.XtraLayout.LayoutControl();
            this.layoutControlGroupMain = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblParamName = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemQuery = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.pnlBottom = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCtrlParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParamName.Properties)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlMain)).BeginInit();
            this.layoutControlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblParamName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottom)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridColumn_paramName
            // 
            this.gridColumn_paramName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_paramName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_paramName.Caption = "名称";
            this.gridColumn_paramName.FieldName = "PARAM_NAME";
            this.gridColumn_paramName.Name = "gridColumn_paramName";
            this.gridColumn_paramName.Visible = true;
            this.gridColumn_paramName.VisibleIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(621, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(500, 10);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(83, 25);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btConfirm_Click);
            // 
            // gridColumn_paramKey
            // 
            this.gridColumn_paramKey.Caption = "PARAM_KEY";
            this.gridColumn_paramKey.FieldName = "PARAM_KEY";
            this.gridColumn_paramKey.Name = "gridColumn_paramKey";
            // 
            // gridViewParam
            // 
            this.gridViewParam.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_paramKey,
            this.gridColumn_paramName,
            this.gridColumn_paramDescription});
            this.gridViewParam.GridControl = this.grdCtrlParam;
            this.gridViewParam.Name = "gridViewParam";
            this.gridViewParam.OptionsBehavior.Editable = false;
            this.gridViewParam.OptionsView.ShowGroupPanel = false;
            this.gridViewParam.DoubleClick += new System.EventHandler(this.gridViewParam_DoubleClick);
            // 
            // gridColumn_paramDescription
            // 
            this.gridColumn_paramDescription.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_paramDescription.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_paramDescription.Caption = "描述";
            this.gridColumn_paramDescription.FieldName = "DESCRIPTIONS";
            this.gridColumn_paramDescription.Name = "gridColumn_paramDescription";
            this.gridColumn_paramDescription.Visible = true;
            this.gridColumn_paramDescription.VisibleIndex = 1;
            // 
            // grdCtrlParam
            // 
            this.grdCtrlParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCtrlParam.Location = new System.Drawing.Point(3, 78);
            this.grdCtrlParam.MainView = this.gridViewParam;
            this.grdCtrlParam.Name = "grdCtrlParam";
            this.grdCtrlParam.Size = new System.Drawing.Size(711, 323);
            this.grdCtrlParam.TabIndex = 9;
            this.grdCtrlParam.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewParam});
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(621, 33);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(78, 22);
            this.btnQuery.StyleController = this.layoutControlMain;
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtParamName
            // 
            this.txtParamName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParamName.Location = new System.Drawing.Point(40, 33);
            this.txtParamName.Name = "txtParamName";
            this.txtParamName.Size = new System.Drawing.Size(543, 21);
            this.txtParamName.StyleController = this.layoutControlMain;
            this.txtParamName.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grdCtrlParam, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.layoutControlMain, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlBottom, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(717, 455);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // layoutControlMain
            // 
            this.layoutControlMain.Controls.Add(this.btnQuery);
            this.layoutControlMain.Controls.Add(this.txtParamName);
            this.layoutControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlMain.Location = new System.Drawing.Point(3, 3);
            this.layoutControlMain.Name = "layoutControlMain";
            this.layoutControlMain.Root = this.layoutControlGroupMain;
            this.layoutControlMain.Size = new System.Drawing.Size(711, 69);
            this.layoutControlMain.TabIndex = 11;
            this.layoutControlMain.Text = "layoutControl1";
            // 
            // layoutControlGroupMain
            // 
            this.layoutControlGroupMain.CustomizationFormText = " ";
            this.layoutControlGroupMain.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroupMain.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblParamName,
            this.layoutControlItemQuery,
            this.emptySpaceItem1});
            this.layoutControlGroupMain.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupMain.Name = "Root";
            this.layoutControlGroupMain.Size = new System.Drawing.Size(711, 69);
            this.layoutControlGroupMain.Text = " ";
            // 
            // lblParamName
            // 
            this.lblParamName.Control = this.txtParamName;
            this.lblParamName.CustomizationFormText = "名称";
            this.lblParamName.Location = new System.Drawing.Point(0, 0);
            this.lblParamName.Name = "lblParamName";
            this.lblParamName.Size = new System.Drawing.Size(575, 28);
            this.lblParamName.Text = "名称";
            this.lblParamName.TextSize = new System.Drawing.Size(24, 14);
            // 
            // layoutControlItemQuery
            // 
            this.layoutControlItemQuery.Control = this.btnQuery;
            this.layoutControlItemQuery.CustomizationFormText = "查询";
            this.layoutControlItemQuery.Location = new System.Drawing.Point(609, 0);
            this.layoutControlItemQuery.MaxSize = new System.Drawing.Size(82, 26);
            this.layoutControlItemQuery.MinSize = new System.Drawing.Size(82, 26);
            this.layoutControlItemQuery.Name = "layoutControlItemQuery";
            this.layoutControlItemQuery.Size = new System.Drawing.Size(82, 28);
            this.layoutControlItemQuery.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItemQuery.Text = "查询";
            this.layoutControlItemQuery.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemQuery.TextToControlDistance = 0;
            this.layoutControlItemQuery.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(575, 0);
            this.emptySpaceItem1.MaxSize = new System.Drawing.Size(34, 0);
            this.emptySpaceItem1.MinSize = new System.Drawing.Size(34, 10);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(34, 28);
            this.emptySpaceItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem1.Text = "emptySpaceItem1";
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnConfirm);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(3, 407);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(711, 45);
            this.pnlBottom.TabIndex = 12;
            // 
            // ParamSearchDialog
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 455);
            this.Controls.Add(this.tableLayoutPanel1);
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "ParamSearchDialog";
            this.Load += new System.EventHandler(this.ParamSearchDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCtrlParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParamName.Properties)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlMain)).EndInit();
            this.layoutControlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblParamName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottom)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_paramName;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_paramKey;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewParam;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_paramDescription;
        private DevExpress.XtraGrid.GridControl grdCtrlParam;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit txtParamName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraLayout.LayoutControl layoutControlMain;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupMain;
        private DevExpress.XtraLayout.LayoutControlItem lblParamName;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemQuery;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.PanelControl pnlBottom;
    }
}