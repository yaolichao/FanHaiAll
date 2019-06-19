namespace FanHai.Hemera.Addins.EDC
{
    partial class NewEdcPoint
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
            this.luePartName = new DevExpress.XtraEditors.LookUpEdit();
            this.lcInfo = new DevExpress.XtraLayout.LayoutControl();
            this.lueRoute = new DevExpress.XtraEditors.LookUpEdit();
            this.pnlBottom = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.luePartType = new DevExpress.XtraEditors.LookUpEdit();
            this.cmbActionName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lueEDCName = new DevExpress.XtraEditors.LookUpEdit();
            this.lueOperationName = new DevExpress.XtraEditors.LookUpEdit();
            this.lueSPName = new DevExpress.XtraEditors.LookUpEdit();
            this.lueEquipmentKey = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.lcgInfo = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciSPName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPartType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciEDCNname = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciOperationName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciEquipmentKey = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPartName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciActionName = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciRoute = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.luePartName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcInfo)).BeginInit();
            this.lcInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueRoute.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottom)).BeginInit();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.luePartType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbActionName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueEDCName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueOperationName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSPName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueEquipmentKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSPName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPartType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEDCNname)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOperationName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEquipmentKey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPartName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciActionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciRoute)).BeginInit();
            this.SuspendLayout();
            // 
            // luePartName
            // 
            this.luePartName.Location = new System.Drawing.Point(77, 113);
            this.luePartName.Name = "luePartName";
            this.luePartName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.luePartName.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("PART_NAME", " "),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("PART_TYPE", " ", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.luePartName.Properties.NullText = "";
            this.luePartName.Size = new System.Drawing.Size(600, 21);
            this.luePartName.StyleController = this.lcInfo;
            this.luePartName.TabIndex = 79;
            this.luePartName.EditValueChanged += new System.EventHandler(this.luePartName_EditValueChanged);
            // 
            // lcInfo
            // 
            this.lcInfo.AllowCustomizationMenu = false;
            this.lcInfo.Controls.Add(this.lueRoute);
            this.lcInfo.Controls.Add(this.pnlBottom);
            this.lcInfo.Controls.Add(this.luePartType);
            this.lcInfo.Controls.Add(this.cmbActionName);
            this.lcInfo.Controls.Add(this.luePartName);
            this.lcInfo.Controls.Add(this.lueEDCName);
            this.lcInfo.Controls.Add(this.lueOperationName);
            this.lcInfo.Controls.Add(this.lueSPName);
            this.lcInfo.Controls.Add(this.lueEquipmentKey);
            this.lcInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcInfo.Location = new System.Drawing.Point(0, 0);
            this.lcInfo.Name = "lcInfo";
            this.lcInfo.Root = this.lcgInfo;
            this.lcInfo.Size = new System.Drawing.Size(690, 197);
            this.lcInfo.TabIndex = 2;
            this.lcInfo.Text = "layoutControl1";
            // 
            // lueRoute
            // 
            this.lueRoute.Location = new System.Drawing.Point(77, 38);
            this.lueRoute.Name = "lueRoute";
            this.lueRoute.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueRoute.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ROUTE_NAME", "名称"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DESCRIPTION", "描述")});
            this.lueRoute.Properties.NullText = "";
            this.lueRoute.Size = new System.Drawing.Size(600, 21);
            this.lueRoute.StyleController = this.lcInfo;
            this.lueRoute.TabIndex = 96;
            this.lueRoute.EditValueChanged += new System.EventHandler(this.lueRoute_EditValueChanged);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnConfirm);
            this.pnlBottom.Location = new System.Drawing.Point(13, 138);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(664, 43);
            this.pnlBottom.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(559, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 95;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(462, 8);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(90, 25);
            this.btnConfirm.TabIndex = 94;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // luePartType
            // 
            this.luePartType.Location = new System.Drawing.Point(77, 13);
            this.luePartType.Name = "luePartType";
            this.luePartType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.luePartType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", " ")});
            this.luePartType.Properties.NullText = "";
            this.luePartType.Size = new System.Drawing.Size(259, 21);
            this.luePartType.StyleController = this.lcInfo;
            this.luePartType.TabIndex = 95;
            // 
            // cmbActionName
            // 
            this.cmbActionName.Location = new System.Drawing.Point(404, 13);
            this.cmbActionName.Name = "cmbActionName";
            this.cmbActionName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbActionName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbActionName.Size = new System.Drawing.Size(273, 21);
            this.cmbActionName.StyleController = this.lcInfo;
            this.cmbActionName.TabIndex = 94;
            // 
            // lueEDCName
            // 
            this.lueEDCName.Location = new System.Drawing.Point(404, 88);
            this.lueEDCName.Name = "lueEDCName";
            this.lueEDCName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueEDCName.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EDC_NAME", " ")});
            this.lueEDCName.Properties.NullText = "";
            this.lueEDCName.Size = new System.Drawing.Size(273, 21);
            this.lueEDCName.StyleController = this.lcInfo;
            this.lueEDCName.TabIndex = 93;
            // 
            // lueOperationName
            // 
            this.lueOperationName.Location = new System.Drawing.Point(77, 63);
            this.lueOperationName.Name = "lueOperationName";
            this.lueOperationName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueOperationName.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ROUTE_OPERATION_NAME", " "),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ROUTE_STEP_KEY", "", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.lueOperationName.Properties.NullText = "";
            this.lueOperationName.Size = new System.Drawing.Size(259, 21);
            this.lueOperationName.StyleController = this.lcInfo;
            this.lueOperationName.TabIndex = 84;
            this.lueOperationName.EditValueChanged += new System.EventHandler(this.OperationName_EditValueChanged);
            // 
            // lueSPName
            // 
            this.lueSPName.Location = new System.Drawing.Point(77, 88);
            this.lueSPName.Name = "lueSPName";
            this.lueSPName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueSPName.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("SP_NAME", "名称"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DESCRIPTIONS", "描述"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("STRATEGY_SIZE", "策略值"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("SAMPLING_MODE", "模式")});
            this.lueSPName.Properties.NullText = "";
            this.lueSPName.Size = new System.Drawing.Size(259, 21);
            this.lueSPName.StyleController = this.lcInfo;
            this.lueSPName.TabIndex = 91;
            // 
            // lueEquipmentKey
            // 
            this.lueEquipmentKey.Location = new System.Drawing.Point(404, 63);
            this.lueEquipmentKey.Name = "lueEquipmentKey";
            this.lueEquipmentKey.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueEquipmentKey.Properties.DropDownRows = 20;
            this.lueEquipmentKey.Properties.PopupFormSize = new System.Drawing.Size(0, 200);
            this.lueEquipmentKey.Properties.SelectAllItemCaption = "全选";
            this.lueEquipmentKey.Properties.ShowButtons = false;
            this.lueEquipmentKey.Size = new System.Drawing.Size(273, 21);
            this.lueEquipmentKey.StyleController = this.lcInfo;
            this.lueEquipmentKey.TabIndex = 87;
            // 
            // lcgInfo
            // 
            this.lcgInfo.CustomizationFormText = "lcgInfo";
            this.lcgInfo.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgInfo.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciSPName,
            this.lciPartType,
            this.lciEDCNname,
            this.lciOperationName,
            this.lciEquipmentKey,
            this.lciPartName,
            this.lciActionName,
            this.layoutControlItem1,
            this.lciRoute});
            this.lcgInfo.Location = new System.Drawing.Point(0, 0);
            this.lcgInfo.Name = "lcgInfo";
            this.lcgInfo.Size = new System.Drawing.Size(690, 197);
            this.lcgInfo.Text = "lcgInfo";
            this.lcgInfo.TextVisible = false;
            // 
            // lciSPName
            // 
            this.lciSPName.Control = this.lueSPName;
            this.lciSPName.CustomizationFormText = "抽检规则";
            this.lciSPName.Location = new System.Drawing.Point(0, 75);
            this.lciSPName.Name = "lciSPName";
            this.lciSPName.Size = new System.Drawing.Size(327, 25);
            this.lciSPName.Text = "抽检规则";
            this.lciSPName.TextSize = new System.Drawing.Size(60, 14);
            // 
            // lciPartType
            // 
            this.lciPartType.Control = this.luePartType;
            this.lciPartType.CustomizationFormText = "成品类型";
            this.lciPartType.Location = new System.Drawing.Point(0, 0);
            this.lciPartType.Name = "lciPartType";
            this.lciPartType.Size = new System.Drawing.Size(327, 25);
            this.lciPartType.Text = "成品类型";
            this.lciPartType.TextSize = new System.Drawing.Size(60, 14);
            // 
            // lciEDCNname
            // 
            this.lciEDCNname.Control = this.lueEDCName;
            this.lciEDCNname.CustomizationFormText = "参数组名称";
            this.lciEDCNname.Location = new System.Drawing.Point(327, 75);
            this.lciEDCNname.Name = "lciEDCNname";
            this.lciEDCNname.Size = new System.Drawing.Size(341, 25);
            this.lciEDCNname.Text = "参数组名称";
            this.lciEDCNname.TextSize = new System.Drawing.Size(60, 14);
            // 
            // lciOperationName
            // 
            this.lciOperationName.Control = this.lueOperationName;
            this.lciOperationName.CustomizationFormText = "工序名称";
            this.lciOperationName.Location = new System.Drawing.Point(0, 50);
            this.lciOperationName.Name = "lciOperationName";
            this.lciOperationName.Size = new System.Drawing.Size(327, 25);
            this.lciOperationName.Text = "工序名称";
            this.lciOperationName.TextSize = new System.Drawing.Size(60, 14);
            // 
            // lciEquipmentKey
            // 
            this.lciEquipmentKey.Control = this.lueEquipmentKey;
            this.lciEquipmentKey.CustomizationFormText = "设备名称";
            this.lciEquipmentKey.Location = new System.Drawing.Point(327, 50);
            this.lciEquipmentKey.Name = "lciEquipmentKey";
            this.lciEquipmentKey.Size = new System.Drawing.Size(341, 25);
            this.lciEquipmentKey.Text = "设备名称";
            this.lciEquipmentKey.TextSize = new System.Drawing.Size(60, 14);
            // 
            // lciPartName
            // 
            this.lciPartName.Control = this.luePartName;
            this.lciPartName.CustomizationFormText = "成品编号";
            this.lciPartName.Location = new System.Drawing.Point(0, 100);
            this.lciPartName.Name = "lciPartName";
            this.lciPartName.Size = new System.Drawing.Size(668, 25);
            this.lciPartName.Text = "成品编号";
            this.lciPartName.TextSize = new System.Drawing.Size(60, 14);
            this.lciPartName.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lciActionName
            // 
            this.lciActionName.Control = this.cmbActionName;
            this.lciActionName.CustomizationFormText = "动作";
            this.lciActionName.Location = new System.Drawing.Point(327, 0);
            this.lciActionName.Name = "lciActionName";
            this.lciActionName.Size = new System.Drawing.Size(341, 25);
            this.lciActionName.Text = "动作";
            this.lciActionName.TextSize = new System.Drawing.Size(60, 14);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.pnlBottom;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 125);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(0, 47);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(104, 47);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(668, 50);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // lciRoute
            // 
            this.lciRoute.Control = this.lueRoute;
            this.lciRoute.CustomizationFormText = "工艺流程";
            this.lciRoute.Location = new System.Drawing.Point(0, 25);
            this.lciRoute.Name = "lciRoute";
            this.lciRoute.Size = new System.Drawing.Size(668, 25);
            this.lciRoute.Text = "工艺流程";
            this.lciRoute.TextSize = new System.Drawing.Size(60, 14);
            // 
            // NewEdcPoint
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 197);
            this.Controls.Add(this.lcInfo);
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "NewEdcPoint";
            this.Load += new System.EventHandler(this.NewEdcPoint_Load);
            ((System.ComponentModel.ISupportInitialize)(this.luePartName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcInfo)).EndInit();
            this.lcInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lueRoute.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottom)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.luePartType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbActionName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueEDCName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueOperationName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSPName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueEquipmentKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSPName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPartType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEDCNname)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOperationName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEquipmentKey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPartName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciActionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciRoute)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit luePartName;
        private DevExpress.XtraEditors.LookUpEdit lueOperationName;
        private DevExpress.XtraEditors.LookUpEdit lueSPName;
        private DevExpress.XtraEditors.LookUpEdit lueEDCName;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.PanelControl pnlBottom;
        private DevExpress.XtraEditors.ComboBoxEdit cmbActionName;
        private DevExpress.XtraLayout.LayoutControl lcInfo;
        private DevExpress.XtraLayout.LayoutControlGroup lcgInfo;
        private DevExpress.XtraLayout.LayoutControlItem lciPartName;
        private DevExpress.XtraLayout.LayoutControlItem lciOperationName;
        private DevExpress.XtraLayout.LayoutControlItem lciEquipmentKey;
        private DevExpress.XtraLayout.LayoutControlItem lciActionName;
        private DevExpress.XtraLayout.LayoutControlItem lciSPName;
        private DevExpress.XtraLayout.LayoutControlItem lciEDCNname;
        private DevExpress.XtraEditors.LookUpEdit luePartType;
        private DevExpress.XtraLayout.LayoutControlItem lciPartType;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.CheckedComboBoxEdit lueEquipmentKey;
        private DevExpress.XtraEditors.LookUpEdit lueRoute;
        private DevExpress.XtraLayout.LayoutControlItem lciRoute;
    }
}