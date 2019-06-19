namespace FanHai.Hemera.Addins.SAP
{
    partial class WorkOrderInfoListDialog
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
            this.cmdCancel = new DevExpress.XtraEditors.SimpleButton();
            this.cbeStatus = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbeType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbeFactory = new DevExpress.XtraEditors.ComboBoxEdit();
            this.teStore = new DevExpress.XtraEditors.TextEdit();
            this.tePart = new DevExpress.XtraEditors.TextEdit();
            this.teWorkOrderNo = new DevExpress.XtraEditors.TextEdit();
            this.cmdOK = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgButtons = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciButtonOk = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciButtonCancel = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbeStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeFactory.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teStore.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teWorkOrderNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciButtonOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciButtonCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // Content
            // 
            this.Content.Controls.Add(this.cmdCancel);
            this.Content.Controls.Add(this.cbeStatus);
            this.Content.Controls.Add(this.cbeType);
            this.Content.Controls.Add(this.cbeFactory);
            this.Content.Controls.Add(this.teStore);
            this.Content.Controls.Add(this.tePart);
            this.Content.Controls.Add(this.teWorkOrderNo);
            this.Content.Controls.Add(this.cmdOK);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(0, 0);
            this.Content.Name = "Content";
            this.Content.Root = this.layoutControlGroup1;
            this.Content.Size = new System.Drawing.Size(557, 130);
            this.Content.TabIndex = 0;
            this.Content.Text = "layoutControl1";
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(470, 86);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(76, 31);
            this.cmdCancel.StyleController = this.Content;
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "取消";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cbeStatus
            // 
            this.cbeStatus.Location = new System.Drawing.Point(333, 56);
            this.cbeStatus.Name = "cbeStatus";
            this.cbeStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeStatus.Size = new System.Drawing.Size(218, 21);
            this.cbeStatus.StyleController = this.Content;
            this.cbeStatus.TabIndex = 12;
            // 
            // cbeType
            // 
            this.cbeType.Location = new System.Drawing.Point(333, 31);
            this.cbeType.Name = "cbeType";
            this.cbeType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeType.Size = new System.Drawing.Size(218, 21);
            this.cbeType.StyleController = this.Content;
            this.cbeType.TabIndex = 11;
            // 
            // cbeFactory
            // 
            this.cbeFactory.Location = new System.Drawing.Point(58, 6);
            this.cbeFactory.Name = "cbeFactory";
            this.cbeFactory.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeFactory.Size = new System.Drawing.Size(219, 21);
            this.cbeFactory.StyleController = this.Content;
            this.cbeFactory.TabIndex = 10;
            // 
            // teStore
            // 
            this.teStore.Location = new System.Drawing.Point(58, 56);
            this.teStore.Name = "teStore";
            this.teStore.Size = new System.Drawing.Size(219, 21);
            this.teStore.StyleController = this.Content;
            this.teStore.TabIndex = 8;
            // 
            // tePart
            // 
            this.tePart.Location = new System.Drawing.Point(58, 31);
            this.tePart.Name = "tePart";
            this.tePart.Size = new System.Drawing.Size(219, 21);
            this.tePart.StyleController = this.Content;
            this.tePart.TabIndex = 6;
            // 
            // teWorkOrderNo
            // 
            this.teWorkOrderNo.Location = new System.Drawing.Point(333, 6);
            this.teWorkOrderNo.Name = "teWorkOrderNo";
            this.teWorkOrderNo.Size = new System.Drawing.Size(218, 21);
            this.teWorkOrderNo.StyleController = this.Content;
            this.teWorkOrderNo.TabIndex = 5;
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(390, 86);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(76, 31);
            this.cmdOK.StyleController = this.Content;
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "确定";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.layoutControlItem5,
            this.layoutControlItem2,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.lcgButtons});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.layoutControlGroup1.Size = new System.Drawing.Size(557, 130);
            this.layoutControlGroup1.Text = "工单信息查询";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.tePart;
            this.layoutControlItem3.CustomizationFormText = "产品料号";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 25);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(275, 25);
            this.layoutControlItem3.Text = "产品料号";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.teStore;
            this.layoutControlItem5.CustomizationFormText = "入库库位";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 50);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(275, 25);
            this.layoutControlItem5.Text = "入库库位";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.teWorkOrderNo;
            this.layoutControlItem2.CustomizationFormText = "工单号";
            this.layoutControlItem2.Location = new System.Drawing.Point(275, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(274, 25);
            this.layoutControlItem2.Text = "工单号";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.cbeFactory;
            this.layoutControlItem6.CustomizationFormText = "车间名称";
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(275, 25);
            this.layoutControlItem6.Text = "车间名称";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.cbeType;
            this.layoutControlItem7.CustomizationFormText = "工单类型";
            this.layoutControlItem7.Location = new System.Drawing.Point(275, 25);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(274, 25);
            this.layoutControlItem7.Text = "工单类型";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.cbeStatus;
            this.layoutControlItem8.CustomizationFormText = "状态";
            this.layoutControlItem8.Location = new System.Drawing.Point(275, 50);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(274, 25);
            this.layoutControlItem8.Text = "状态";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(48, 14);
            // 
            // lcgButtons
            // 
            this.lcgButtons.CustomizationFormText = "lcgButtons";
            this.lcgButtons.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciButtonOk,
            this.lciButtonCancel,
            this.emptySpaceItem1});
            this.lcgButtons.Location = new System.Drawing.Point(0, 75);
            this.lcgButtons.Name = "lcgButtons";
            this.lcgButtons.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgButtons.Size = new System.Drawing.Size(549, 47);
            this.lcgButtons.Text = "lcgButtons";
            this.lcgButtons.TextVisible = false;
            // 
            // lciButtonOk
            // 
            this.lciButtonOk.Control = this.cmdOK;
            this.lciButtonOk.CustomizationFormText = "确定";
            this.lciButtonOk.Location = new System.Drawing.Point(379, 0);
            this.lciButtonOk.MaxSize = new System.Drawing.Size(80, 35);
            this.lciButtonOk.MinSize = new System.Drawing.Size(80, 35);
            this.lciButtonOk.Name = "lciButtonOk";
            this.lciButtonOk.Size = new System.Drawing.Size(80, 37);
            this.lciButtonOk.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciButtonOk.Text = "确定";
            this.lciButtonOk.TextSize = new System.Drawing.Size(0, 0);
            this.lciButtonOk.TextToControlDistance = 0;
            this.lciButtonOk.TextVisible = false;
            // 
            // lciButtonCancel
            // 
            this.lciButtonCancel.Control = this.cmdCancel;
            this.lciButtonCancel.CustomizationFormText = "取消";
            this.lciButtonCancel.Location = new System.Drawing.Point(459, 0);
            this.lciButtonCancel.MaxSize = new System.Drawing.Size(80, 35);
            this.lciButtonCancel.MinSize = new System.Drawing.Size(80, 35);
            this.lciButtonCancel.Name = "lciButtonCancel";
            this.lciButtonCancel.Size = new System.Drawing.Size(80, 37);
            this.lciButtonCancel.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciButtonCancel.Text = "取消";
            this.lciButtonCancel.TextSize = new System.Drawing.Size(0, 0);
            this.lciButtonCancel.TextToControlDistance = 0;
            this.lciButtonCancel.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(379, 37);
            this.emptySpaceItem1.Text = "emptySpaceItem1";
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // WorkOrderInfoListDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 130);
            this.Controls.Add(this.Content);
            this.Name = "WorkOrderInfoListDialog";
            this.Load += new System.EventHandler(this.WorkOrderInfoListDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbeStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeFactory.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teStore.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teWorkOrderNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciButtonOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciButtonCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl Content;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.TextEdit teStore;
        private DevExpress.XtraEditors.TextEdit tePart;
        private DevExpress.XtraEditors.TextEdit teWorkOrderNo;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.SimpleButton cmdCancel;
        private DevExpress.XtraEditors.SimpleButton cmdOK;
        private DevExpress.XtraEditors.ComboBoxEdit cbeStatus;
        private DevExpress.XtraEditors.ComboBoxEdit cbeType;
        private DevExpress.XtraEditors.ComboBoxEdit cbeFactory;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControlItem lciButtonCancel;
        private DevExpress.XtraLayout.LayoutControlItem lciButtonOk;
        private DevExpress.XtraLayout.LayoutControlGroup lcgButtons;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}
