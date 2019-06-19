namespace FanHai.Hemera.Addins.SPC.Gui
{
    partial class AbnormalControlPlan
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraEditors.Repository.RepositoryItemColorEdit repositoryItemColorEdit1;
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.OVERRULEPOINTS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.WATCHPOINTS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COMPARESIGN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.COMPARERULE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.RULEVALUE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcMainAbnormal = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.isSelected = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryisSelected = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.ARULECODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ABNORMALDESC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ABNORMALCOLOR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutView1 = new DevExpress.XtraGrid.Views.Layout.LayoutView();
            this.colARULECODE = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_colARULECODE = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.colABNORMALDESC = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_colABNORMALDESC = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.ABNORMALID = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_ABNORMALID = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.FLAGMAIN = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_FLAGMAIN = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.layoutViewCard1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewCard();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            repositoryItemColorEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemColorEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMainAbnormal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryisSelected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(repositoryItemColorEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colARULECODE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colABNORMALDESC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_ABNORMALID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_FLAGMAIN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridView2
            // 
            this.gridView2.ColumnPanelRowHeight = 30;
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.OVERRULEPOINTS,
            this.WATCHPOINTS,
            this.COMPARESIGN,
            this.COMPARERULE,
            this.RULEVALUE});
            this.gridView2.GridControl = this.gcMainAbnormal;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView2.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsView.EnableAppearanceEvenRow = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // OVERRULEPOINTS
            // 
            this.OVERRULEPOINTS.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.OVERRULEPOINTS.AppearanceHeader.Options.UseFont = true;
            this.OVERRULEPOINTS.Caption = "超规点数";
            this.OVERRULEPOINTS.FieldName = "OVERRULEPOINTS";
            this.OVERRULEPOINTS.Name = "OVERRULEPOINTS";
            this.OVERRULEPOINTS.Visible = true;
            this.OVERRULEPOINTS.VisibleIndex = 0;
            // 
            // WATCHPOINTS
            // 
            this.WATCHPOINTS.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.WATCHPOINTS.AppearanceHeader.Options.UseFont = true;
            this.WATCHPOINTS.Caption = "监控点数";
            this.WATCHPOINTS.FieldName = "WATCHPOINTS";
            this.WATCHPOINTS.Name = "WATCHPOINTS";
            this.WATCHPOINTS.Visible = true;
            this.WATCHPOINTS.VisibleIndex = 1;
            // 
            // COMPARESIGN
            // 
            this.COMPARESIGN.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.COMPARESIGN.AppearanceHeader.Options.UseFont = true;
            this.COMPARESIGN.Caption = "比较符";
            this.COMPARESIGN.ColumnEdit = this.repositoryItemComboBox1;
            this.COMPARESIGN.FieldName = "COMPARESIGN";
            this.COMPARESIGN.Name = "COMPARESIGN";
            this.COMPARESIGN.Visible = true;
            this.COMPARESIGN.VisibleIndex = 2;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "Equal",
            "NotEqual",
            "GreateThan",
            "GreateThanOrEqual",
            "LessThan",
            "LessThanOrEqual",
            "InSide",
            "OutSide",
            "Increasing",
            "Decreasing"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // COMPARERULE
            // 
            this.COMPARERULE.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.COMPARERULE.AppearanceHeader.Options.UseFont = true;
            this.COMPARERULE.Caption = "比较规格";
            this.COMPARERULE.ColumnEdit = this.repositoryItemComboBox2;
            this.COMPARERULE.FieldName = "COMPARERULE";
            this.COMPARERULE.Name = "COMPARERULE";
            this.COMPARERULE.Visible = true;
            this.COMPARERULE.VisibleIndex = 3;
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Items.AddRange(new object[] {
            "USL",
            "CSL",
            "UCL",
            "CL",
            "LCL",
            "Sigma",
            "ZoneA",
            "ZoneB",
            "ZoneC"});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // RULEVALUE
            // 
            this.RULEVALUE.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.RULEVALUE.AppearanceHeader.Options.UseFont = true;
            this.RULEVALUE.Caption = "规格值";
            this.RULEVALUE.FieldName = "RULEVALUE";
            this.RULEVALUE.Name = "RULEVALUE";
            this.RULEVALUE.Visible = true;
            this.RULEVALUE.VisibleIndex = 4;
            // 
            // gcMainAbnormal
            // 
            this.gcMainAbnormal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMainAbnormal.Font = new System.Drawing.Font("Tahoma", 12F);
            gridLevelNode1.LevelTemplate = this.gridView2;
            gridLevelNode1.RelationName = "子规则明细";
            this.gcMainAbnormal.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcMainAbnormal.Location = new System.Drawing.Point(3, 3);
            this.gcMainAbnormal.LookAndFeel.SkinName = "Coffee";
            this.gcMainAbnormal.MainView = this.gridView1;
            this.gcMainAbnormal.Name = "gcMainAbnormal";
            this.gcMainAbnormal.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2,
            repositoryItemColorEdit1,
            this.repositoryisSelected});
            this.gcMainAbnormal.ShowOnlyPredefinedDetails = true;
            this.gcMainAbnormal.Size = new System.Drawing.Size(742, 298);
            this.gcMainAbnormal.TabIndex = 5;
            this.gcMainAbnormal.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1,
            this.layoutView1,
            this.gridView2});
            // 
            // gridView1
            // 
            this.gridView1.ColumnPanelRowHeight = 30;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.isSelected,
            this.ARULECODE,
            this.ABNORMALDESC,
            this.ABNORMALCOLOR});
            this.gridView1.GridControl = this.gcMainAbnormal;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.Default;
            this.gridView1.ViewCaption = "qq";
            this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
            // 
            // isSelected
            // 
            this.isSelected.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 12F);
            this.isSelected.AppearanceCell.Options.UseFont = true;
            this.isSelected.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.isSelected.AppearanceHeader.Options.UseFont = true;
            this.isSelected.Caption = "选择";
            this.isSelected.ColumnEdit = this.repositoryisSelected;
            this.isSelected.FieldName = "isSelected";
            this.isSelected.Name = "isSelected";
            this.isSelected.Visible = true;
            this.isSelected.VisibleIndex = 0;
            // 
            // repositoryisSelected
            // 
            this.repositoryisSelected.AutoHeight = false;
            this.repositoryisSelected.Name = "repositoryisSelected";
            this.repositoryisSelected.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // ARULECODE
            // 
            this.ARULECODE.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11F);
            this.ARULECODE.AppearanceCell.Options.UseFont = true;
            this.ARULECODE.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ARULECODE.AppearanceHeader.Options.UseFont = true;
            this.ARULECODE.Caption = "规则代码";
            this.ARULECODE.FieldName = "ARULECODE";
            this.ARULECODE.Name = "ARULECODE";
            this.ARULECODE.Visible = true;
            this.ARULECODE.VisibleIndex = 1;
            // 
            // ABNORMALDESC
            // 
            this.ABNORMALDESC.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11F);
            this.ABNORMALDESC.AppearanceCell.Options.UseFont = true;
            this.ABNORMALDESC.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ABNORMALDESC.AppearanceHeader.Options.UseFont = true;
            this.ABNORMALDESC.Caption = "规则描述";
            this.ABNORMALDESC.FieldName = "ABNORMALDESC";
            this.ABNORMALDESC.Name = "ABNORMALDESC";
            this.ABNORMALDESC.Visible = true;
            this.ABNORMALDESC.VisibleIndex = 2;
            // 
            // ABNORMALCOLOR
            // 
            this.ABNORMALCOLOR.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ABNORMALCOLOR.AppearanceHeader.Options.UseFont = true;
            this.ABNORMALCOLOR.Caption = "颜色";
            this.ABNORMALCOLOR.FieldName = "ABNORMALCOLOR";
            this.ABNORMALCOLOR.Name = "ABNORMALCOLOR";
            this.ABNORMALCOLOR.Visible = true;
            this.ABNORMALCOLOR.VisibleIndex = 3;
            // 
            // repositoryItemColorEdit1
            // 
            repositoryItemColorEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            repositoryItemColorEdit1.Name = "repositoryItemColorEdit1";
            // 
            // layoutView1
            // 
            this.layoutView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.LayoutViewColumn[] {
            this.colARULECODE,
            this.colABNORMALDESC,
            this.ABNORMALID,
            this.FLAGMAIN});
            this.layoutView1.GridControl = this.gcMainAbnormal;
            this.layoutView1.Name = "layoutView1";
            this.layoutView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.layoutView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.layoutView1.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.layoutView1.TemplateCard = this.layoutViewCard1;
            this.layoutView1.ViewCaption = "qq";
            // 
            // colARULECODE
            // 
            this.colARULECODE.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11F);
            this.colARULECODE.AppearanceCell.Options.UseFont = true;
            this.colARULECODE.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colARULECODE.AppearanceHeader.Options.UseFont = true;
            this.colARULECODE.Caption = "规则代码";
            this.colARULECODE.FieldName = "ARULECODE";
            this.colARULECODE.LayoutViewField = this.layoutViewField_colARULECODE;
            this.colARULECODE.Name = "colARULECODE";
            // 
            // layoutViewField_colARULECODE
            // 
            this.layoutViewField_colARULECODE.EditorPreferredWidth = 119;
            this.layoutViewField_colARULECODE.Location = new System.Drawing.Point(0, 0);
            this.layoutViewField_colARULECODE.Name = "layoutViewField_colARULECODE";
            this.layoutViewField_colARULECODE.Size = new System.Drawing.Size(203, 20);
            this.layoutViewField_colARULECODE.TextSize = new System.Drawing.Size(76, 14);
            // 
            // colABNORMALDESC
            // 
            this.colABNORMALDESC.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11F);
            this.colABNORMALDESC.AppearanceCell.Options.UseFont = true;
            this.colABNORMALDESC.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colABNORMALDESC.AppearanceHeader.Options.UseFont = true;
            this.colABNORMALDESC.Caption = "规则描述";
            this.colABNORMALDESC.FieldName = "ABNORMALDESC";
            this.colABNORMALDESC.LayoutViewField = this.layoutViewField_colABNORMALDESC;
            this.colABNORMALDESC.Name = "colABNORMALDESC";
            // 
            // layoutViewField_colABNORMALDESC
            // 
            this.layoutViewField_colABNORMALDESC.EditorPreferredWidth = 119;
            this.layoutViewField_colABNORMALDESC.Location = new System.Drawing.Point(0, 20);
            this.layoutViewField_colABNORMALDESC.Name = "layoutViewField_colABNORMALDESC";
            this.layoutViewField_colABNORMALDESC.Size = new System.Drawing.Size(203, 20);
            this.layoutViewField_colABNORMALDESC.TextSize = new System.Drawing.Size(76, 14);
            // 
            // ABNORMALID
            // 
            this.ABNORMALID.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 11F);
            this.ABNORMALID.AppearanceCell.Options.UseFont = true;
            this.ABNORMALID.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ABNORMALID.AppearanceHeader.Options.UseFont = true;
            this.ABNORMALID.Caption = "异常规则主键";
            this.ABNORMALID.FieldName = "ABNORMALID";
            this.ABNORMALID.LayoutViewField = this.layoutViewField_ABNORMALID;
            this.ABNORMALID.Name = "ABNORMALID";
            // 
            // layoutViewField_ABNORMALID
            // 
            this.layoutViewField_ABNORMALID.EditorPreferredWidth = 119;
            this.layoutViewField_ABNORMALID.Location = new System.Drawing.Point(0, 40);
            this.layoutViewField_ABNORMALID.Name = "layoutViewField_ABNORMALID";
            this.layoutViewField_ABNORMALID.Size = new System.Drawing.Size(203, 20);
            this.layoutViewField_ABNORMALID.TextSize = new System.Drawing.Size(76, 14);
            // 
            // FLAGMAIN
            // 
            this.FLAGMAIN.Caption = "FLAGMAIN";
            this.FLAGMAIN.FieldName = "FLAGMAIN";
            this.FLAGMAIN.LayoutViewField = this.layoutViewField_FLAGMAIN;
            this.FLAGMAIN.Name = "FLAGMAIN";
            // 
            // layoutViewField_FLAGMAIN
            // 
            this.layoutViewField_FLAGMAIN.EditorPreferredWidth = 119;
            this.layoutViewField_FLAGMAIN.Location = new System.Drawing.Point(0, 60);
            this.layoutViewField_FLAGMAIN.Name = "layoutViewField_FLAGMAIN";
            this.layoutViewField_FLAGMAIN.Size = new System.Drawing.Size(203, 20);
            this.layoutViewField_FLAGMAIN.TextSize = new System.Drawing.Size(76, 14);
            // 
            // layoutViewCard1
            // 
            this.layoutViewCard1.ExpandButtonLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.layoutViewCard1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutViewField_colARULECODE,
            this.layoutViewField_colABNORMALDESC,
            this.layoutViewField_ABNORMALID,
            this.layoutViewField_FLAGMAIN});
            this.layoutViewCard1.Name = "layoutViewTemplateCard";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.gcMainAbnormal, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.layoutControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 88.62974F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.37026F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(748, 343);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutControl1.Controls.Add(this.btnCancel);
            this.layoutControl1.Controls.Add(this.btnOK);
            this.layoutControl1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.layoutControl1.Location = new System.Drawing.Point(565, 307);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(180, 33);
            this.layoutControl1.TabIndex = 6;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Location = new System.Drawing.Point(97, 2);
            this.btnCancel.MaximumSize = new System.Drawing.Size(100, 30);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 29);
            this.btnCancel.StyleController = this.layoutControl1;
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.Location = new System.Drawing.Point(2, 2);
            this.btnOK.MaximumSize = new System.Drawing.Size(100, 30);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(91, 29);
            this.btnOK.StyleController = this.layoutControl1;
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOk_Click);
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
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(180, 33);
            this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnOK;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(95, 33);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnCancel;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(95, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(85, 33);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // AbnormalControlPlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 343);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AbnormalControlPlan";
            this.Text = "AbnormalControlPlan";
            this.Load += new System.EventHandler(this.AbnormalControlPlan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMainAbnormal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryisSelected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(repositoryItemColorEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colARULECODE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colABNORMALDESC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_ABNORMALID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_FLAGMAIN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMainAbnormal;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn OVERRULEPOINTS;
        private DevExpress.XtraGrid.Columns.GridColumn WATCHPOINTS;
        private DevExpress.XtraGrid.Columns.GridColumn COMPARESIGN;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraGrid.Columns.GridColumn COMPARERULE;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraGrid.Columns.GridColumn RULEVALUE;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn ARULECODE;
        private DevExpress.XtraGrid.Columns.GridColumn ABNORMALDESC;
        private DevExpress.XtraGrid.Columns.GridColumn ABNORMALCOLOR;
        private DevExpress.XtraGrid.Views.Layout.LayoutView layoutView1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colARULECODE;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colARULECODE;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colABNORMALDESC;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colABNORMALDESC;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn ABNORMALID;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_ABNORMALID;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn FLAGMAIN;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_FLAGMAIN;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewCard layoutViewCard1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn isSelected;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryisSelected;
    }
}