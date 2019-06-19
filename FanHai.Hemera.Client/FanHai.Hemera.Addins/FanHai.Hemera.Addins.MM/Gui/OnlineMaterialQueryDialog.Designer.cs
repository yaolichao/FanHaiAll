namespace FanHai.Hemera.Addins.MM
{
    partial class OnlineMaterialQueryDialog
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Content = new DevExpress.XtraLayout.LayoutControl();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.teSupplierName = new DevExpress.XtraEditors.TextEdit();
            this.teMaterilLot = new DevExpress.XtraEditors.TextEdit();
            this.teMaterialCode = new DevExpress.XtraEditors.TextEdit();
            this.lueRoom = new DevExpress.XtraEditors.LookUpEdit();
            this.lcgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgTop = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciRoom = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciMaterialLot = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciMaterialCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciOperationName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciSupplierName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciStoreName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgButtons = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciButtonCancel = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciButtonOk = new DevExpress.XtraLayout.LayoutControlItem();
            this.esiPrefixButtons = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lueOperationName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lueStoreName = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teSupplierName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teMaterilLot.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teMaterialCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueRoom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciRoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciMaterialLot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciMaterialCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOperationName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSupplierName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStoreName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciButtonCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciButtonOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiPrefixButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueOperationName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueStoreName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // Content
            // 
            this.Content.Appearance.Control.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Content.Appearance.Control.Options.UseFont = true;
            this.Content.Controls.Add(this.cmdCancel);
            this.Content.Controls.Add(this.cmdOK);
            this.Content.Controls.Add(this.teSupplierName);
            this.Content.Controls.Add(this.teMaterilLot);
            this.Content.Controls.Add(this.teMaterialCode);
            this.Content.Controls.Add(this.lueRoom);
            this.Content.Controls.Add(this.lueOperationName);
            this.Content.Controls.Add(this.lueStoreName);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(0, 0);
            this.Content.Name = "Content";
            this.Content.Root = this.lcgRoot;
            this.Content.Size = new System.Drawing.Size(584, 151);
            this.Content.TabIndex = 0;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(500, 110);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(76, 31);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "取消";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(420, 110);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(76, 31);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "确定";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // teSupplierName
            // 
            this.teSupplierName.Location = new System.Drawing.Point(92, 68);
            this.teSupplierName.Name = "teSupplierName";
            this.teSupplierName.Size = new System.Drawing.Size(198, 26);
            this.teSupplierName.StyleController = this.Content;
            this.teSupplierName.TabIndex = 9;
            // 
            // teMaterilLot
            // 
            this.teMaterilLot.Location = new System.Drawing.Point(92, 38);
            this.teMaterilLot.Name = "teMaterilLot";
            this.teMaterilLot.Size = new System.Drawing.Size(198, 26);
            this.teMaterilLot.StyleController = this.Content;
            this.teMaterilLot.TabIndex = 7;
            // 
            // teMaterialCode
            // 
            this.teMaterialCode.Location = new System.Drawing.Point(378, 8);
            this.teMaterialCode.Name = "teMaterialCode";
            this.teMaterialCode.Size = new System.Drawing.Size(198, 26);
            this.teMaterialCode.StyleController = this.Content;
            this.teMaterialCode.TabIndex = 8;
            // 
            // lueRoom
            // 
            this.lueRoom.Location = new System.Drawing.Point(92, 8);
            this.lueRoom.Name = "lueRoom";
            this.lueRoom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueRoom.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LOCATION_NAME", "车间")});
            this.lueRoom.Properties.NullText = "";
            this.lueRoom.Size = new System.Drawing.Size(198, 26);
            this.lueRoom.StyleController = this.Content;
            this.lueRoom.TabIndex = 4;
            this.lueRoom.TabStop = false;
            // 
            // lcgRoot
            // 
            this.lcgRoot.CustomizationFormText = "在线库存查询";
            this.lcgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgRoot.GroupBordersVisible = false;
            this.lcgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgTop,
            this.lcgButtons});
            this.lcgRoot.Location = new System.Drawing.Point(0, 0);
            this.lcgRoot.Name = "Root";
            this.lcgRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgRoot.Size = new System.Drawing.Size(584, 151);
            this.lcgRoot.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgRoot.Text = "在线库存查询";
            this.lcgRoot.TextVisible = false;
            // 
            // lcgTop
            // 
            this.lcgTop.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lcgTop.AppearanceItemCaption.Options.UseFont = true;
            this.lcgTop.CustomizationFormText = "lcgTop";
            this.lcgTop.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciRoom,
            this.lciMaterialLot,
            this.lciMaterialCode,
            this.lciOperationName,
            this.lciSupplierName,
            this.lciStoreName});
            this.lcgTop.Location = new System.Drawing.Point(0, 0);
            this.lcgTop.Name = "lcgTop";
            this.lcgTop.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.lcgTop.Size = new System.Drawing.Size(584, 102);
            this.lcgTop.Text = "lcgTop";
            this.lcgTop.TextVisible = false;
            // 
            // lciRoom
            // 
            this.lciRoom.Control = this.lueRoom;
            this.lciRoom.CustomizationFormText = "工厂车间";
            this.lciRoom.Location = new System.Drawing.Point(0, 0);
            this.lciRoom.Name = "lciRoom";
            this.lciRoom.Size = new System.Drawing.Size(286, 30);
            this.lciRoom.Text = "工厂车间";
            this.lciRoom.TextSize = new System.Drawing.Size(80, 19);
            // 
            // lciMaterialLot
            // 
            this.lciMaterialLot.Control = this.teMaterilLot;
            this.lciMaterialLot.CustomizationFormText = "物料批号";
            this.lciMaterialLot.Location = new System.Drawing.Point(0, 30);
            this.lciMaterialLot.Name = "lciMaterialLot";
            this.lciMaterialLot.Size = new System.Drawing.Size(286, 30);
            this.lciMaterialLot.Text = "物料批号";
            this.lciMaterialLot.TextSize = new System.Drawing.Size(80, 19);
            // 
            // lciMaterialCode
            // 
            this.lciMaterialCode.Control = this.teMaterialCode;
            this.lciMaterialCode.CustomizationFormText = "物料编码";
            this.lciMaterialCode.Location = new System.Drawing.Point(286, 0);
            this.lciMaterialCode.Name = "lciMaterialCode";
            this.lciMaterialCode.Size = new System.Drawing.Size(286, 30);
            this.lciMaterialCode.Text = "物料编码";
            this.lciMaterialCode.TextSize = new System.Drawing.Size(80, 19);
            // 
            // lciOperationName
            // 
            this.lciOperationName.Control = this.lueOperationName;
            this.lciOperationName.CustomizationFormText = "工序名称";
            this.lciOperationName.Location = new System.Drawing.Point(286, 30);
            this.lciOperationName.Name = "lciOperationName";
            this.lciOperationName.Size = new System.Drawing.Size(286, 30);
            this.lciOperationName.Text = "工序名称";
            this.lciOperationName.TextSize = new System.Drawing.Size(80, 19);
            // 
            // lciSupplierName
            // 
            this.lciSupplierName.Control = this.teSupplierName;
            this.lciSupplierName.CustomizationFormText = "供应商";
            this.lciSupplierName.Location = new System.Drawing.Point(0, 60);
            this.lciSupplierName.Name = "lciSupplierName";
            this.lciSupplierName.Size = new System.Drawing.Size(286, 30);
            this.lciSupplierName.Text = "供应商";
            this.lciSupplierName.TextSize = new System.Drawing.Size(80, 19);
            // 
            // lciStoreName
            // 
            this.lciStoreName.Control = this.lueStoreName;
            this.lciStoreName.CustomizationFormText = "线上仓名称";
            this.lciStoreName.Location = new System.Drawing.Point(286, 60);
            this.lciStoreName.Name = "lciStoreName";
            this.lciStoreName.Size = new System.Drawing.Size(286, 30);
            this.lciStoreName.Text = "线上仓名称";
            this.lciStoreName.TextSize = new System.Drawing.Size(80, 19);
            // 
            // lcgButtons
            // 
            this.lcgButtons.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lcgButtons.AppearanceItemCaption.Options.UseFont = true;
            this.lcgButtons.CustomizationFormText = "layoutControlGroup2";
            this.lcgButtons.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciButtonCancel,
            this.lciButtonOk,
            this.esiPrefixButtons});
            this.lcgButtons.Location = new System.Drawing.Point(0, 102);
            this.lcgButtons.Name = "lcgButtons";
            this.lcgButtons.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.lcgButtons.Size = new System.Drawing.Size(584, 49);
            this.lcgButtons.Text = "lcgButtons";
            this.lcgButtons.TextVisible = false;
            // 
            // lciButtonCancel
            // 
            this.lciButtonCancel.Control = this.cmdCancel;
            this.lciButtonCancel.CustomizationFormText = "Cancel";
            this.lciButtonCancel.Location = new System.Drawing.Point(492, 0);
            this.lciButtonCancel.MaxSize = new System.Drawing.Size(80, 35);
            this.lciButtonCancel.MinSize = new System.Drawing.Size(80, 35);
            this.lciButtonCancel.Name = "lciButtonCancel";
            this.lciButtonCancel.Size = new System.Drawing.Size(80, 37);
            this.lciButtonCancel.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciButtonCancel.Text = "Cancel";
            this.lciButtonCancel.TextSize = new System.Drawing.Size(0, 0);
            this.lciButtonCancel.TextToControlDistance = 0;
            this.lciButtonCancel.TextVisible = false;
            // 
            // lciButtonOk
            // 
            this.lciButtonOk.Control = this.cmdOK;
            this.lciButtonOk.CustomizationFormText = "OK";
            this.lciButtonOk.Location = new System.Drawing.Point(412, 0);
            this.lciButtonOk.MaxSize = new System.Drawing.Size(80, 35);
            this.lciButtonOk.MinSize = new System.Drawing.Size(80, 35);
            this.lciButtonOk.Name = "lciButtonOk";
            this.lciButtonOk.Size = new System.Drawing.Size(80, 37);
            this.lciButtonOk.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciButtonOk.Text = "OK";
            this.lciButtonOk.TextSize = new System.Drawing.Size(0, 0);
            this.lciButtonOk.TextToControlDistance = 0;
            this.lciButtonOk.TextVisible = false;
            // 
            // esiPrefixButtons
            // 
            this.esiPrefixButtons.CustomizationFormText = "esiPrefixButtons";
            this.esiPrefixButtons.Location = new System.Drawing.Point(0, 0);
            this.esiPrefixButtons.Name = "esiPrefixButtons";
            this.esiPrefixButtons.Size = new System.Drawing.Size(412, 37);
            this.esiPrefixButtons.Text = "esiPrefixButtons";
            this.esiPrefixButtons.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lueOperationName
            // 
            this.lueOperationName.Location = new System.Drawing.Point(378, 38);
            this.lueOperationName.Name = "lueOperationName";
            this.lueOperationName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueOperationName.Properties.PopupSizeable = true;
            this.lueOperationName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.lueOperationName.Size = new System.Drawing.Size(198, 26);
            this.lueOperationName.StyleController = this.Content;
            this.lueOperationName.TabIndex = 5;
            this.lueOperationName.TabStop = false;
            // 
            // lueStoreName
            // 
            this.lueStoreName.Location = new System.Drawing.Point(378, 68);
            this.lueStoreName.Name = "lueStoreName";
            this.lueStoreName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueStoreName.Properties.PopupSizeable = true;
            this.lueStoreName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.lueStoreName.Size = new System.Drawing.Size(198, 26);
            this.lueStoreName.StyleController = this.Content;
            this.lueStoreName.TabIndex = 6;
            this.lueStoreName.TabStop = false;
            // 
            // OnlineMaterialQueryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 151);
            this.Controls.Add(this.Content);
            this.Name = "OnlineMaterialQueryDialog";
            this.Text = "在线库存查询";
            this.Load += new System.EventHandler(this.OnlineMaterialQueryDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.teSupplierName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teMaterilLot.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teMaterialCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueRoom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciRoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciMaterialLot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciMaterialCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOperationName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSupplierName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStoreName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciButtonCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciButtonOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiPrefixButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueOperationName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueStoreName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl Content;
        private DevExpress.XtraLayout.LayoutControlGroup lcgRoot;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private DevExpress.XtraEditors.TextEdit teSupplierName;
        private DevExpress.XtraEditors.TextEdit teMaterialCode;
        private DevExpress.XtraEditors.TextEdit teMaterilLot;
        private DevExpress.XtraEditors.LookUpEdit lueRoom;
        private DevExpress.XtraLayout.LayoutControlItem lciRoom;
        private DevExpress.XtraLayout.LayoutControlItem lciOperationName;
        private DevExpress.XtraLayout.LayoutControlItem lciMaterialLot;
        private DevExpress.XtraLayout.LayoutControlItem lciMaterialCode;
        private DevExpress.XtraLayout.LayoutControlItem lciSupplierName;
        private DevExpress.XtraLayout.LayoutControlItem lciStoreName;
        private DevExpress.XtraLayout.LayoutControlItem lciButtonOk;
        private DevExpress.XtraLayout.LayoutControlItem lciButtonCancel;
        private DevExpress.XtraLayout.LayoutControlGroup lcgTop;
        private DevExpress.XtraLayout.LayoutControlGroup lcgButtons;
        private DevExpress.XtraLayout.EmptySpaceItem esiPrefixButtons;
        private DevExpress.XtraEditors.ComboBoxEdit lueOperationName;
        private DevExpress.XtraEditors.ComboBoxEdit lueStoreName;




    }
}
