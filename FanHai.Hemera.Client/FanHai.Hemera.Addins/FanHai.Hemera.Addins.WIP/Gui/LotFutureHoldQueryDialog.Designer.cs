namespace FanHai.Hemera.Addins.WIP
{
    partial class LotFutureHoldQueryDialog
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
            this.lcContent = new DevExpress.XtraLayout.LayoutControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.lueAction = new DevExpress.XtraEditors.LookUpEdit();
            this.lueOperationName = new DevExpress.XtraEditors.LookUpEdit();
            this.txtWorkOrderNo = new DevExpress.XtraEditors.TextEdit();
            this.txtLotNo = new DevExpress.XtraEditors.TextEdit();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.lcgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgQueryCondition = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciLotNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciWorkOrderNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciOperationName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciAction = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lcgCommands = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lciOkButton = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciCancelButton = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.lcContent)).BeginInit();
            this.lcContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueAction.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueOperationName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWorkOrderNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLotNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQueryCondition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLotNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciWorkOrderNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOperationName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgCommands)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOkButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCancelButton)).BeginInit();
            this.SuspendLayout();
            // 
            // lcContent
            // 
            this.lcContent.AllowCustomization = false;
            this.lcContent.Controls.Add(this.btnCancel);
            this.lcContent.Controls.Add(this.lueAction);
            this.lcContent.Controls.Add(this.lueOperationName);
            this.lcContent.Controls.Add(this.txtWorkOrderNo);
            this.lcContent.Controls.Add(this.txtLotNo);
            this.lcContent.Controls.Add(this.btnOk);
            this.lcContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcContent.Location = new System.Drawing.Point(0, 0);
            this.lcContent.Name = "lcContent";
            this.lcContent.Root = this.lcgRoot;
            this.lcContent.Size = new System.Drawing.Size(472, 118);
            this.lcContent.TabIndex = 0;
            this.lcContent.Text = "layoutControl1";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(382, 88);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 22);
            this.btnCancel.StyleController = this.lcContent;
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lueAction
            // 
            this.lueAction.Location = new System.Drawing.Point(283, 31);
            this.lueAction.Name = "lueAction";
            this.lueAction.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueAction.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ACTION_NAME", "名称")});
            this.lueAction.Properties.NullText = "";
            this.lueAction.Size = new System.Drawing.Size(182, 20);
            this.lueAction.StyleController = this.lcContent;
            this.lueAction.TabIndex = 7;
            // 
            // lueOperationName
            // 
            this.lueOperationName.Location = new System.Drawing.Point(46, 31);
            this.lueOperationName.Name = "lueOperationName";
            this.lueOperationName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueOperationName.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ROUTE_OPERATION_NAME", "名称")});
            this.lueOperationName.Properties.NullText = "";
            this.lueOperationName.Size = new System.Drawing.Size(194, 20);
            this.lueOperationName.StyleController = this.lcContent;
            this.lueOperationName.TabIndex = 6;
            // 
            // txtWorkOrderNo
            // 
            this.txtWorkOrderNo.Location = new System.Drawing.Point(283, 7);
            this.txtWorkOrderNo.Name = "txtWorkOrderNo";
            this.txtWorkOrderNo.Properties.MaxLength = 20;
            this.txtWorkOrderNo.Size = new System.Drawing.Size(182, 20);
            this.txtWorkOrderNo.StyleController = this.lcContent;
            this.txtWorkOrderNo.TabIndex = 5;
            // 
            // txtLotNo
            // 
            this.txtLotNo.Location = new System.Drawing.Point(46, 7);
            this.txtLotNo.Name = "txtLotNo";
            this.txtLotNo.Properties.MaxLength = 20;
            this.txtLotNo.Size = new System.Drawing.Size(194, 20);
            this.txtLotNo.StyleController = this.lcContent;
            this.txtLotNo.TabIndex = 4;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(298, 88);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 22);
            this.btnOk.StyleController = this.lcContent;
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lcgRoot
            // 
            this.lcgRoot.CustomizationFormText = "lcgRoot";
            this.lcgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgRoot.GroupBordersVisible = false;
            this.lcgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgQueryCondition,
            this.lcgCommands});
            this.lcgRoot.Name = "Root";
            this.lcgRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.lcgRoot.Size = new System.Drawing.Size(472, 118);
            this.lcgRoot.TextVisible = false;
            // 
            // lcgQueryCondition
            // 
            this.lcgQueryCondition.CustomizationFormText = " ";
            this.lcgQueryCondition.GroupBordersVisible = false;
            this.lcgQueryCondition.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciLotNo,
            this.lciWorkOrderNo,
            this.lciOperationName,
            this.lciAction,
            this.emptySpaceItem2});
            this.lcgQueryCondition.Location = new System.Drawing.Point(0, 0);
            this.lcgQueryCondition.Name = "lcgQueryCondition";
            this.lcgQueryCondition.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgQueryCondition.Size = new System.Drawing.Size(462, 80);
            this.lcgQueryCondition.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgQueryCondition.Text = " ";
            this.lcgQueryCondition.TextVisible = false;
            // 
            // lciLotNo
            // 
            this.lciLotNo.Control = this.txtLotNo;
            this.lciLotNo.CustomizationFormText = "序列号";
            this.lciLotNo.Location = new System.Drawing.Point(0, 0);
            this.lciLotNo.Name = "lciLotNo";
            this.lciLotNo.Size = new System.Drawing.Size(237, 24);
            this.lciLotNo.Text = "序列号";
            this.lciLotNo.TextSize = new System.Drawing.Size(36, 14);
            // 
            // lciWorkOrderNo
            // 
            this.lciWorkOrderNo.Control = this.txtWorkOrderNo;
            this.lciWorkOrderNo.CustomizationFormText = "工单号";
            this.lciWorkOrderNo.Location = new System.Drawing.Point(237, 0);
            this.lciWorkOrderNo.Name = "lciWorkOrderNo";
            this.lciWorkOrderNo.Size = new System.Drawing.Size(225, 24);
            this.lciWorkOrderNo.Text = "工单号";
            this.lciWorkOrderNo.TextSize = new System.Drawing.Size(36, 14);
            // 
            // lciOperationName
            // 
            this.lciOperationName.Control = this.lueOperationName;
            this.lciOperationName.CustomizationFormText = "工序";
            this.lciOperationName.Location = new System.Drawing.Point(0, 24);
            this.lciOperationName.Name = "lciOperationName";
            this.lciOperationName.Size = new System.Drawing.Size(237, 24);
            this.lciOperationName.Text = "工序";
            this.lciOperationName.TextSize = new System.Drawing.Size(36, 14);
            // 
            // lciAction
            // 
            this.lciAction.Control = this.lueAction;
            this.lciAction.CustomizationFormText = "动作";
            this.lciAction.Location = new System.Drawing.Point(237, 24);
            this.lciAction.Name = "lciAction";
            this.lciAction.Size = new System.Drawing.Size(225, 24);
            this.lciAction.Text = "动作";
            this.lciAction.TextSize = new System.Drawing.Size(36, 14);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 48);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(462, 32);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lcgCommands
            // 
            this.lcgCommands.CustomizationFormText = " ";
            this.lcgCommands.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.lciOkButton,
            this.lciCancelButton});
            this.lcgCommands.Location = new System.Drawing.Point(0, 80);
            this.lcgCommands.Name = "lcgCommands";
            this.lcgCommands.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgCommands.Size = new System.Drawing.Size(462, 28);
            this.lcgCommands.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgCommands.Text = " ";
            this.lcgCommands.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem1.MinSize = new System.Drawing.Size(104, 24);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(290, 26);
            this.emptySpaceItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lciOkButton
            // 
            this.lciOkButton.Control = this.btnOk;
            this.lciOkButton.CustomizationFormText = "layoutControlItem1";
            this.lciOkButton.Location = new System.Drawing.Point(290, 0);
            this.lciOkButton.MaxSize = new System.Drawing.Size(84, 26);
            this.lciOkButton.MinSize = new System.Drawing.Size(84, 26);
            this.lciOkButton.Name = "lciOkButton";
            this.lciOkButton.Size = new System.Drawing.Size(84, 26);
            this.lciOkButton.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciOkButton.TextSize = new System.Drawing.Size(0, 0);
            this.lciOkButton.TextVisible = false;
            // 
            // lciCancelButton
            // 
            this.lciCancelButton.Control = this.btnCancel;
            this.lciCancelButton.CustomizationFormText = "lciCancelButton";
            this.lciCancelButton.Location = new System.Drawing.Point(374, 0);
            this.lciCancelButton.MaxSize = new System.Drawing.Size(86, 26);
            this.lciCancelButton.MinSize = new System.Drawing.Size(86, 26);
            this.lciCancelButton.Name = "lciCancelButton";
            this.lciCancelButton.Size = new System.Drawing.Size(86, 26);
            this.lciCancelButton.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciCancelButton.TextSize = new System.Drawing.Size(0, 0);
            this.lciCancelButton.TextVisible = false;
            // 
            // LotFutureHoldQueryDialog
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 118);
            this.Controls.Add(this.lcContent);
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "LotFutureHoldQueryDialog";
            this.Text = "查询预设暂停";
            this.Load += new System.EventHandler(this.LotFutureHoldQueryDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lcContent)).EndInit();
            this.lcContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lueAction.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueOperationName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWorkOrderNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLotNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQueryCondition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLotNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciWorkOrderNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOperationName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgCommands)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOkButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCancelButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl lcContent;
        private DevExpress.XtraLayout.LayoutControlGroup lcgRoot;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LookUpEdit lueAction;
        private DevExpress.XtraEditors.LookUpEdit lueOperationName;
        private DevExpress.XtraEditors.TextEdit txtWorkOrderNo;
        private DevExpress.XtraEditors.TextEdit txtLotNo;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraLayout.LayoutControlItem lciLotNo;
        private DevExpress.XtraLayout.LayoutControlItem lciWorkOrderNo;
        private DevExpress.XtraLayout.LayoutControlItem lciOperationName;
        private DevExpress.XtraLayout.LayoutControlItem lciAction;
        private DevExpress.XtraLayout.LayoutControlItem lciOkButton;
        private DevExpress.XtraLayout.LayoutControlItem lciCancelButton;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlGroup lcgQueryCondition;
        private DevExpress.XtraLayout.LayoutControlGroup lcgCommands;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;

    }
}