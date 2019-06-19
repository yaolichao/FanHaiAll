namespace FanHai.Hemera.Addins.EMS
{
    partial class EquipmentCommonQueryDialog
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
            this.lcgForm = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgDataList = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciDataList = new DevExpress.XtraLayout.LayoutControlItem();
            this.grdDataList = new DevExpress.XtraGrid.GridControl();
            this.grvDataList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lciPaginationDataList = new DevExpress.XtraLayout.LayoutControlItem();
            this.paginationDataList = new FanHai.Hemera.Utils.Controls.PaginationControl();
            this.lcgDataQuery = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciQueryLabel = new DevExpress.XtraLayout.LayoutControlItem();
            this.txtQueryValue = new DevExpress.XtraEditors.TextEdit();
            this.lcForm = new DevExpress.XtraLayout.LayoutControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.lciQuery = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lciOK = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciCancel = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.lcgForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgDataList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDataList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDataList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvDataList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPaginationDataList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgDataQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQueryLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcForm)).BeginInit();
            this.lcForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // lcgForm
            // 
            this.lcgForm.CustomizationFormText = "Root";
            this.lcgForm.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgDataList,
            this.lcgDataQuery,
            this.emptySpaceItem1,
            this.lciOK,
            this.lciCancel});
            this.lcgForm.Name = "Root";
            this.lcgForm.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.lcgForm.Size = new System.Drawing.Size(791, 663);
            this.lcgForm.Spacing = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.lcgForm.TextVisible = false;
            // 
            // lcgDataList
            // 
            this.lcgDataList.CustomizationFormText = "数据列表";
            this.lcgDataList.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciDataList,
            this.lciPaginationDataList});
            this.lcgDataList.Location = new System.Drawing.Point(0, 79);
            this.lcgDataList.Name = "lcgDataList";
            this.lcgDataList.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgDataList.Size = new System.Drawing.Size(785, 545);
            this.lcgDataList.Text = "数据列表";
            // 
            // lciDataList
            // 
            this.lciDataList.Control = this.grdDataList;
            this.lciDataList.CustomizationFormText = "数据列表";
            this.lciDataList.Location = new System.Drawing.Point(0, 0);
            this.lciDataList.Name = "lciDataList";
            this.lciDataList.Size = new System.Drawing.Size(779, 460);
            this.lciDataList.Text = "数据列表";
            this.lciDataList.TextSize = new System.Drawing.Size(0, 0);
            this.lciDataList.TextVisible = false;
            // 
            // grdDataList
            // 
            this.grdDataList.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grdDataList.Location = new System.Drawing.Point(8, 115);
            this.grdDataList.MainView = this.grvDataList;
            this.grdDataList.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.grdDataList.Name = "grdDataList";
            this.grdDataList.Size = new System.Drawing.Size(775, 454);
            this.grdDataList.TabIndex = 6;
            this.grdDataList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvDataList});
            // 
            // grvDataList
            // 
            this.grvDataList.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Red;
            this.grvDataList.Appearance.FooterPanel.Options.UseForeColor = true;
            this.grvDataList.DetailHeight = 450;
            this.grvDataList.FixedLineWidth = 3;
            this.grvDataList.GridControl = this.grdDataList;
            this.grvDataList.Name = "grvDataList";
            this.grvDataList.OptionsCustomization.AllowFilter = false;
            this.grvDataList.OptionsCustomization.AllowGroup = false;
            this.grvDataList.OptionsMenu.EnableFooterMenu = false;
            this.grvDataList.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.grvDataList.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.grvDataList.OptionsView.ColumnAutoWidth = false;
            this.grvDataList.OptionsView.ShowGroupPanel = false;
            this.grvDataList.DoubleClick += new System.EventHandler(this.grvDataList_DoubleClick);
            // 
            // lciPaginationDataList
            // 
            this.lciPaginationDataList.Control = this.paginationDataList;
            this.lciPaginationDataList.CustomizationFormText = "数据列表分页";
            this.lciPaginationDataList.Location = new System.Drawing.Point(0, 460);
            this.lciPaginationDataList.MaxSize = new System.Drawing.Size(0, 51);
            this.lciPaginationDataList.MinSize = new System.Drawing.Size(119, 51);
            this.lciPaginationDataList.Name = "lciPaginationDataList";
            this.lciPaginationDataList.Size = new System.Drawing.Size(779, 51);
            this.lciPaginationDataList.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciPaginationDataList.Text = "数据列表分页";
            this.lciPaginationDataList.TextSize = new System.Drawing.Size(0, 0);
            this.lciPaginationDataList.TextVisible = false;
            // 
            // paginationDataList
            // 
            this.paginationDataList.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.paginationDataList.Appearance.Options.UseBackColor = true;
            this.paginationDataList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.paginationDataList.Location = new System.Drawing.Point(8, 575);
            this.paginationDataList.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.paginationDataList.LookAndFeel.UseDefaultLookAndFeel = false;
            this.paginationDataList.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.paginationDataList.Name = "paginationDataList";
            this.paginationDataList.PageNo = 1;
            this.paginationDataList.Pages = 0;
            this.paginationDataList.PageSize = 20;
            this.paginationDataList.Records = 0;
            this.paginationDataList.Size = new System.Drawing.Size(775, 45);
            this.paginationDataList.TabIndex = 7;
            this.paginationDataList.DataPaging += new FanHai.Hemera.Utils.Controls.Paging(this.paginationDataList_DataPaging);
            // 
            // lcgDataQuery
            // 
            this.lcgDataQuery.CustomizationFormText = "数据查询";
            this.lcgDataQuery.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciQueryLabel,
            this.lciQuery});
            this.lcgDataQuery.Location = new System.Drawing.Point(0, 0);
            this.lcgDataQuery.Name = "lcgQuery";
            this.lcgDataQuery.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.lcgDataQuery.Size = new System.Drawing.Size(785, 79);
            this.lcgDataQuery.Text = "数据查询";
            // 
            // lciQueryLabel
            // 
            this.lciQueryLabel.Control = this.txtQueryValue;
            this.lciQueryLabel.CustomizationFormText = "查询设备编码";
            this.lciQueryLabel.Location = new System.Drawing.Point(0, 0);
            this.lciQueryLabel.Name = "lciQueryLabel";
            this.lciQueryLabel.Size = new System.Drawing.Size(653, 33);
            this.lciQueryLabel.Text = "查询设备编码";
            this.lciQueryLabel.TextSize = new System.Drawing.Size(90, 18);
            // 
            // txtQueryValue
            // 
            this.txtQueryValue.EditValue = "";
            this.txtQueryValue.EnterMoveNextControl = true;
            this.txtQueryValue.Location = new System.Drawing.Point(107, 42);
            this.txtQueryValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtQueryValue.Name = "txtQueryValue";
            this.txtQueryValue.Properties.MaxLength = 50;
            this.txtQueryValue.Size = new System.Drawing.Size(556, 24);
            this.txtQueryValue.StyleController = this.lcForm;
            this.txtQueryValue.TabIndex = 1;
            // 
            // lcForm
            // 
            this.lcForm.Controls.Add(this.paginationDataList);
            this.lcForm.Controls.Add(this.btnQuery);
            this.lcForm.Controls.Add(this.txtQueryValue);
            this.lcForm.Controls.Add(this.grdDataList);
            this.lcForm.Controls.Add(this.btnOK);
            this.lcForm.Controls.Add(this.btnCancel);
            this.lcForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcForm.Location = new System.Drawing.Point(0, 0);
            this.lcForm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcForm.Name = "lcForm";
            this.lcForm.Root = this.lcgForm;
            this.lcForm.Size = new System.Drawing.Size(791, 663);
            this.lcForm.TabIndex = 1;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(667, 42);
            this.btnQuery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(110, 27);
            this.btnQuery.StyleController = this.lcForm;
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(562, 630);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(110, 27);
            this.btnOK.StyleController = this.lcForm;
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(676, 630);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 27);
            this.btnCancel.StyleController = this.lcForm;
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lciQuery
            // 
            this.lciQuery.Control = this.btnQuery;
            this.lciQuery.CustomizationFormText = "查询";
            this.lciQuery.Location = new System.Drawing.Point(653, 0);
            this.lciQuery.MaxSize = new System.Drawing.Size(114, 33);
            this.lciQuery.MinSize = new System.Drawing.Size(114, 33);
            this.lciQuery.Name = "lciQuery";
            this.lciQuery.Size = new System.Drawing.Size(114, 33);
            this.lciQuery.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciQuery.Text = "查询";
            this.lciQuery.TextSize = new System.Drawing.Size(0, 0);
            this.lciQuery.TextVisible = false;
            this.lciQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 624);
            this.emptySpaceItem1.MinSize = new System.Drawing.Size(119, 31);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(557, 33);
            this.emptySpaceItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lciOK
            // 
            this.lciOK.Control = this.btnOK;
            this.lciOK.CustomizationFormText = "确定";
            this.lciOK.Location = new System.Drawing.Point(557, 624);
            this.lciOK.MaxSize = new System.Drawing.Size(114, 33);
            this.lciOK.MinSize = new System.Drawing.Size(114, 33);
            this.lciOK.Name = "lciOK";
            this.lciOK.Size = new System.Drawing.Size(114, 33);
            this.lciOK.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciOK.Text = "确定";
            this.lciOK.TextSize = new System.Drawing.Size(0, 0);
            this.lciOK.TextVisible = false;
            // 
            // lciCancel
            // 
            this.lciCancel.Control = this.btnCancel;
            this.lciCancel.CustomizationFormText = "取消";
            this.lciCancel.Location = new System.Drawing.Point(671, 624);
            this.lciCancel.MaxSize = new System.Drawing.Size(114, 33);
            this.lciCancel.MinSize = new System.Drawing.Size(114, 33);
            this.lciCancel.Name = "lciCancel";
            this.lciCancel.Size = new System.Drawing.Size(114, 33);
            this.lciCancel.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciCancel.Text = "取消";
            this.lciCancel.TextSize = new System.Drawing.Size(0, 0);
            this.lciCancel.TextVisible = false;
            // 
            // EquipmentCommonQueryDialog
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 663);
            this.Controls.Add(this.lcForm);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "EquipmentCommonQueryDialog";
            this.ShowInTaskbar = false;
            this.Text = "数据公共查询页面";
            this.Load += new System.EventHandler(this.EquipmentCommonQueryDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lcgForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgDataList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDataList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDataList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvDataList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPaginationDataList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgDataQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQueryLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcForm)).EndInit();
            this.lcForm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCancel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControlGroup lcgForm;
        private DevExpress.XtraLayout.LayoutControl lcForm;
        private FanHai.Hemera.Utils.Controls.PaginationControl paginationDataList;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit txtQueryValue;
        private DevExpress.XtraGrid.GridControl grdDataList;
        private DevExpress.XtraGrid.Views.Grid.GridView grvDataList;
        private DevExpress.XtraLayout.LayoutControlGroup lcgDataList;
        private DevExpress.XtraLayout.LayoutControlItem lciDataList;
        private DevExpress.XtraLayout.LayoutControlItem lciPaginationDataList;
        private DevExpress.XtraLayout.LayoutControlGroup lcgDataQuery;
        private DevExpress.XtraLayout.LayoutControlItem lciQueryLabel;
        private DevExpress.XtraLayout.LayoutControlItem lciQuery;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraLayout.LayoutControlItem lciOK;
        private DevExpress.XtraLayout.LayoutControlItem lciCancel;

    }
}