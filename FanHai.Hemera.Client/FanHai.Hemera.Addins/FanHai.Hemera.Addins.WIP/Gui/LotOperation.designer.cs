namespace FanHai.Hemera.Addins.WIP
{
    partial class LotOperation
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
            this.Content = new DevExpress.XtraLayout.LayoutControl();
            this.beLotNumber = new DevExpress.XtraEditors.ButtonEdit();
            this.lueShift = new DevExpress.XtraEditors.LookUpEdit();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.lueFactoryRoom = new DevExpress.XtraEditors.LookUpEdit();
            this.teUserNumber = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroupMain = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgTop = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lciFactoryRoom = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciLotNumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgHidden = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciUserNumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciShift = new DevExpress.XtraLayout.LayoutControlItem();
            this.esiCommandsBefore = new DevExpress.XtraLayout.EmptySpaceItem();
            this.esiCommandsAfter = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lciBtnClose = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciBtnOK = new DevExpress.XtraLayout.LayoutControlItem();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beLotNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueShift.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactoryRoom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teUserNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFactoryRoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLotNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgHidden)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciUserNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiCommandsBefore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiCommandsAfter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnOK)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(738, 47);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(553, 0);
            this.lblInfos.Size = new System.Drawing.Size(185, 42);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t登录时间：2019-01-15 09:06:54";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            // 
            // Content
            // 
            this.Content.AllowCustomization = false;
            this.Content.Controls.Add(this.beLotNumber);
            this.Content.Controls.Add(this.lueShift);
            this.Content.Controls.Add(this.btnOk);
            this.Content.Controls.Add(this.btnClose);
            this.Content.Controls.Add(this.lueFactoryRoom);
            this.Content.Controls.Add(this.teUserNumber);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.Content.Location = new System.Drawing.Point(3, 3);
            this.Content.Name = "Content";
            this.Content.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(996, 284, 650, 482);
            this.Content.Root = this.layoutControlGroupMain;
            this.Content.Size = new System.Drawing.Size(732, 382);
            this.Content.TabIndex = 62;
            this.Content.Text = "layoutControl1";
            // 
            // beLotNumber
            // 
            this.beLotNumber.Location = new System.Drawing.Point(275, 53);
            this.beLotNumber.Name = "beLotNumber";
            this.beLotNumber.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F);
            this.beLotNumber.Properties.Appearance.Options.UseFont = true;
            this.beLotNumber.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beLotNumber.Size = new System.Drawing.Size(429, 22);
            this.beLotNumber.StyleController = this.Content;
            this.beLotNumber.TabIndex = 3;
            this.beLotNumber.TabStop = false;
            this.beLotNumber.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beLotNumber_ButtonClick);
            this.beLotNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.beLotNumber_KeyPress);
            // 
            // lueShift
            // 
            this.lueShift.Location = new System.Drawing.Point(409, 119);
            this.lueShift.Name = "lueShift";
            this.lueShift.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueShift.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", " ")});
            this.lueShift.Properties.NullText = "";
            this.lueShift.Properties.ReadOnly = true;
            this.lueShift.Size = new System.Drawing.Size(315, 20);
            this.lueShift.StyleController = this.Content;
            this.lueShift.TabIndex = 62;
            // 
            // btnOk
            // 
            this.btnOk.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.Location = new System.Drawing.Point(334, 84);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(86, 26);
            this.btnOk.StyleController = this.Content;
            this.btnOk.TabIndex = 63;
            this.btnOk.Text = "开始作业";
            this.btnOk.Click += new System.EventHandler(this.tsbOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.Location = new System.Drawing.Point(424, 84);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(66, 26);
            this.btnClose.StyleController = this.Content;
            this.btnClose.TabIndex = 64;
            this.btnClose.Text = "重置";
            this.btnClose.Click += new System.EventHandler(this.tsbReset_Click);
            // 
            // lueFactoryRoom
            // 
            this.lueFactoryRoom.Location = new System.Drawing.Point(275, 29);
            this.lueFactoryRoom.Name = "lueFactoryRoom";
            this.lueFactoryRoom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueFactoryRoom.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LOCATION_NAME", " ")});
            this.lueFactoryRoom.Properties.NullText = "";
            this.lueFactoryRoom.Size = new System.Drawing.Size(429, 20);
            this.lueFactoryRoom.StyleController = this.Content;
            this.lueFactoryRoom.TabIndex = 62;
            // 
            // teUserNumber
            // 
            this.teUserNumber.Location = new System.Drawing.Point(75, 119);
            this.teUserNumber.Name = "teUserNumber";
            this.teUserNumber.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F);
            this.teUserNumber.Properties.Appearance.Options.UseFont = true;
            this.teUserNumber.Properties.Mask.EditMask = "\\d{7}";
            this.teUserNumber.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.teUserNumber.Size = new System.Drawing.Size(263, 22);
            this.teUserNumber.StyleController = this.Content;
            this.teUserNumber.TabIndex = 1;
            // 
            // layoutControlGroupMain
            // 
            this.layoutControlGroupMain.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 10F);
            this.layoutControlGroupMain.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlGroupMain.CustomizationFormText = " ";
            this.layoutControlGroupMain.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroupMain.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgTop,
            this.lcgHidden,
            this.esiCommandsBefore,
            this.esiCommandsAfter,
            this.lciBtnClose,
            this.lciBtnOK});
            this.layoutControlGroupMain.Name = "Root";
            this.layoutControlGroupMain.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupMain.Size = new System.Drawing.Size(732, 382);
            this.layoutControlGroupMain.Text = " ";
            // 
            // lcgTop
            // 
            this.lcgTop.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lcgTop.AppearanceItemCaption.Options.UseFont = true;
            this.lcgTop.CustomizationFormText = "表头";
            this.lcgTop.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.emptySpaceItem2,
            this.lciFactoryRoom,
            this.lciLotNumber});
            this.lcgTop.Location = new System.Drawing.Point(0, 0);
            this.lcgTop.Name = "lcgTop";
            this.lcgTop.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.lcgTop.Size = new System.Drawing.Size(730, 60);
            this.lcgTop.Text = "表头";
            this.lcgTop.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(200, 50);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(700, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(20, 50);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lciFactoryRoom
            // 
            this.lciFactoryRoom.Control = this.lueFactoryRoom;
            this.lciFactoryRoom.CustomizationFormText = "车间名称";
            this.lciFactoryRoom.Location = new System.Drawing.Point(200, 0);
            this.lciFactoryRoom.MaxSize = new System.Drawing.Size(500, 24);
            this.lciFactoryRoom.MinSize = new System.Drawing.Size(121, 24);
            this.lciFactoryRoom.Name = "lciFactoryRoom";
            this.lciFactoryRoom.Size = new System.Drawing.Size(500, 24);
            this.lciFactoryRoom.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciFactoryRoom.Text = "车间名称";
            this.lciFactoryRoom.TextSize = new System.Drawing.Size(64, 19);
            // 
            // lciLotNumber
            // 
            this.lciLotNumber.Control = this.beLotNumber;
            this.lciLotNumber.CustomizationFormText = "序列号";
            this.lciLotNumber.Location = new System.Drawing.Point(200, 24);
            this.lciLotNumber.MaxSize = new System.Drawing.Size(500, 26);
            this.lciLotNumber.MinSize = new System.Drawing.Size(121, 26);
            this.lciLotNumber.Name = "lciLotNumber";
            this.lciLotNumber.Size = new System.Drawing.Size(500, 26);
            this.lciLotNumber.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciLotNumber.Text = "序列号";
            this.lciLotNumber.TextSize = new System.Drawing.Size(64, 19);
            // 
            // lcgHidden
            // 
            this.lcgHidden.CustomizationFormText = "隐藏";
            this.lcgHidden.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciUserNumber,
            this.lciShift});
            this.lcgHidden.Location = new System.Drawing.Point(0, 90);
            this.lcgHidden.Name = "lcgHidden";
            this.lcgHidden.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.lcgHidden.Size = new System.Drawing.Size(730, 269);
            this.lcgHidden.Text = "隐藏";
            this.lcgHidden.TextVisible = false;
            this.lcgHidden.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lciUserNumber
            // 
            this.lciUserNumber.Control = this.teUserNumber;
            this.lciUserNumber.CustomizationFormText = "员工号";
            this.lciUserNumber.Location = new System.Drawing.Point(0, 0);
            this.lciUserNumber.Name = "lciUserNumber";
            this.lciUserNumber.Size = new System.Drawing.Size(334, 259);
            this.lciUserNumber.Text = "员工号";
            this.lciUserNumber.TextSize = new System.Drawing.Size(64, 17);
            // 
            // lciShift
            // 
            this.lciShift.Control = this.lueShift;
            this.lciShift.CustomizationFormText = "班别";
            this.lciShift.Location = new System.Drawing.Point(334, 0);
            this.lciShift.Name = "lciShift";
            this.lciShift.Size = new System.Drawing.Size(386, 259);
            this.lciShift.Text = "班别";
            this.lciShift.TextSize = new System.Drawing.Size(64, 17);
            // 
            // esiCommandsBefore
            // 
            this.esiCommandsBefore.AllowHotTrack = false;
            this.esiCommandsBefore.CustomizationFormText = "esiCommandsBefore";
            this.esiCommandsBefore.Location = new System.Drawing.Point(0, 60);
            this.esiCommandsBefore.Name = "esiCommandsBefore";
            this.esiCommandsBefore.Size = new System.Drawing.Size(331, 30);
            this.esiCommandsBefore.TextSize = new System.Drawing.Size(0, 0);
            // 
            // esiCommandsAfter
            // 
            this.esiCommandsAfter.AllowHotTrack = false;
            this.esiCommandsAfter.CustomizationFormText = "esiCommandsAfter";
            this.esiCommandsAfter.Location = new System.Drawing.Point(491, 60);
            this.esiCommandsAfter.MinSize = new System.Drawing.Size(104, 24);
            this.esiCommandsAfter.Name = "esiCommandsAfter";
            this.esiCommandsAfter.Size = new System.Drawing.Size(239, 30);
            this.esiCommandsAfter.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.esiCommandsAfter.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lciBtnClose
            // 
            this.lciBtnClose.Control = this.btnClose;
            this.lciBtnClose.CustomizationFormText = "关闭";
            this.lciBtnClose.Location = new System.Drawing.Point(421, 60);
            this.lciBtnClose.MaxSize = new System.Drawing.Size(70, 30);
            this.lciBtnClose.MinSize = new System.Drawing.Size(39, 30);
            this.lciBtnClose.Name = "lciBtnClose";
            this.lciBtnClose.Size = new System.Drawing.Size(70, 30);
            this.lciBtnClose.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciBtnClose.Text = "关闭";
            this.lciBtnClose.TextSize = new System.Drawing.Size(0, 0);
            this.lciBtnClose.TextVisible = false;
            // 
            // lciBtnOK
            // 
            this.lciBtnOK.Control = this.btnOk;
            this.lciBtnOK.CustomizationFormText = "开始作业";
            this.lciBtnOK.Location = new System.Drawing.Point(331, 60);
            this.lciBtnOK.MaxSize = new System.Drawing.Size(90, 0);
            this.lciBtnOK.MinSize = new System.Drawing.Size(90, 30);
            this.lciBtnOK.Name = "lciBtnOK";
            this.lciBtnOK.Size = new System.Drawing.Size(90, 30);
            this.lciBtnOK.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciBtnOK.Text = "开始作业";
            this.lciBtnOK.TextSize = new System.Drawing.Size(0, 0);
            this.lciBtnOK.TextVisible = false;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.Content, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(1, 47);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 1;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 388F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(738, 388);
            this.tableLayoutPanelMain.TabIndex = 61;
            // 
            // LotOperation
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Name = "LotOperation";
            this.Size = new System.Drawing.Size(740, 435);
            this.Load += new System.EventHandler(this.LotOperation_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.beLotNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueShift.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactoryRoom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teUserNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFactoryRoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLotNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgHidden)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciUserNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiCommandsBefore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiCommandsAfter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnOK)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit teUserNumber;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraLayout.LayoutControl Content;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupMain;
        private DevExpress.XtraLayout.LayoutControlItem lciUserNumber;
        private DevExpress.XtraEditors.LookUpEdit lueFactoryRoom;
        private DevExpress.XtraLayout.LayoutControlItem lciFactoryRoom;
        private DevExpress.XtraEditors.LookUpEdit lueShift;
        private DevExpress.XtraLayout.LayoutControlItem lciShift;
        private DevExpress.XtraLayout.LayoutControlGroup lcgTop;
        private DevExpress.XtraLayout.LayoutControlGroup lcgHidden;
        private DevExpress.XtraEditors.ButtonEdit beLotNumber;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem lciLotNumber;
        private DevExpress.XtraLayout.EmptySpaceItem esiCommandsBefore;
        private DevExpress.XtraLayout.EmptySpaceItem esiCommandsAfter;
        private DevExpress.XtraLayout.LayoutControlItem lciBtnClose;
        private DevExpress.XtraLayout.LayoutControlItem lciBtnOK;
    }
}
