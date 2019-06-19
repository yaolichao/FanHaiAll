namespace FanHai.Hemera.Addins.FMM
{
    partial class ShiftCtrl
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
            this.groupShift = new DevExpress.XtraEditors.GroupControl();
            this.shiftControl = new DevExpress.XtraGrid.GridControl();
            this.shiftView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupInfo = new DevExpress.XtraEditors.GroupControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.tsbDelete = new DevExpress.XtraEditors.SimpleButton();
            this.tsbNew = new DevExpress.XtraEditors.SimpleButton();
            this.tsbSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.lblYear = new DevExpress.XtraEditors.LabelControl();
            this.lueSchedule = new DevExpress.XtraEditors.LookUpEdit();
            this.cbeMonth = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lueYear = new DevExpress.XtraEditors.LookUpEdit();
            this.lblSchedule = new DevExpress.XtraEditors.LabelControl();
            this.lblMonth = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupShift)).BeginInit();
            this.groupShift.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shiftControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shiftView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupInfo)).BeginInit();
            this.groupInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueSchedule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueYear.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(913, 47);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(728, 0);
            this.lblInfos.Size = new System.Drawing.Size(185, 42);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t登录时间：2019-01-15 14:11:58";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            this.lblMenu.Size = new System.Drawing.Size(256, 23);
            this.lblMenu.Text = "平台管理>车间配置>人员班别";
            // 
            // groupShift
            // 
            this.groupShift.Controls.Add(this.shiftControl);
            this.groupShift.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupShift.Location = new System.Drawing.Point(0, 0);
            this.groupShift.Name = "groupShift";
            this.groupShift.Size = new System.Drawing.Size(589, 477);
            this.groupShift.TabIndex = 1;
            this.groupShift.Text = "排班计划";
            // 
            // shiftControl
            // 
            this.shiftControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shiftControl.Location = new System.Drawing.Point(2, 23);
            this.shiftControl.LookAndFeel.SkinName = "Coffee";
            this.shiftControl.MainView = this.shiftView;
            this.shiftControl.Name = "shiftControl";
            this.shiftControl.Size = new System.Drawing.Size(585, 452);
            this.shiftControl.TabIndex = 0;
            this.shiftControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.shiftView});
            // 
            // shiftView
            // 
            this.shiftView.GridControl = this.shiftControl;
            this.shiftView.Name = "shiftView";
            this.shiftView.OptionsView.ShowGroupPanel = false;
            this.shiftView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.shiftView_CustomDrawRowIndicator);
            // 
            // groupInfo
            // 
            this.groupInfo.Controls.Add(this.panelControl2);
            this.groupInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupInfo.Location = new System.Drawing.Point(0, 0);
            this.groupInfo.Name = "groupInfo";
            this.groupInfo.Size = new System.Drawing.Size(314, 477);
            this.groupInfo.TabIndex = 0;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.panelControl3);
            this.panelControl2.Controls.Add(this.btnQuery);
            this.panelControl2.Controls.Add(this.lblYear);
            this.panelControl2.Controls.Add(this.lueSchedule);
            this.panelControl2.Controls.Add(this.cbeMonth);
            this.panelControl2.Controls.Add(this.lueYear);
            this.panelControl2.Controls.Add(this.lblSchedule);
            this.panelControl2.Controls.Add(this.lblMonth);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(2, 23);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(310, 452);
            this.panelControl2.TabIndex = 0;
            // 
            // panelControl3
            // 
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.Controls.Add(this.tsbDelete);
            this.panelControl3.Controls.Add(this.tsbNew);
            this.panelControl3.Controls.Add(this.tsbSave);
            this.panelControl3.Location = new System.Drawing.Point(28, 374);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(235, 32);
            this.panelControl3.TabIndex = 2;
            // 
            // tsbDelete
            // 
            this.tsbDelete.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightBottom;
            this.tsbDelete.Location = new System.Drawing.Point(162, 5);
            this.tsbDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(61, 23);
            this.tsbDelete.TabIndex = 1;
            this.tsbDelete.Text = "删除";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tsbNew
            // 
            this.tsbNew.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.tsbNew.Location = new System.Drawing.Point(10, 5);
            this.tsbNew.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(61, 23);
            this.tsbNew.TabIndex = 1;
            this.tsbNew.Text = "新增";
            this.tsbNew.Click += new System.EventHandler(this.tsbNew_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.tsbSave.Location = new System.Drawing.Point(87, 6);
            this.tsbSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(61, 23);
            this.tsbSave.TabIndex = 1;
            this.tsbSave.Text = "保存";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.AutoWidthInLayoutControl = true;
            this.btnQuery.Location = new System.Drawing.Point(80, 196);
            this.btnQuery.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(88, 26);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "生成排班表";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lblYear
            // 
            this.lblYear.Location = new System.Drawing.Point(38, 65);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(24, 14);
            this.lblYear.TabIndex = 0;
            this.lblYear.Text = "年份";
            // 
            // lueSchedule
            // 
            this.lueSchedule.Location = new System.Drawing.Point(80, 144);
            this.lueSchedule.Name = "lueSchedule";
            this.lueSchedule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueSchedule.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("SCHEDULE_NAME", " ")});
            this.lueSchedule.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.lueSchedule.Properties.NullText = "";
            this.lueSchedule.Size = new System.Drawing.Size(171, 20);
            this.lueSchedule.TabIndex = 3;
            // 
            // cbeMonth
            // 
            this.cbeMonth.Location = new System.Drawing.Point(80, 101);
            this.cbeMonth.Name = "cbeMonth";
            this.cbeMonth.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeMonth.Properties.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cbeMonth.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.cbeMonth.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbeMonth.Size = new System.Drawing.Size(120, 20);
            this.cbeMonth.TabIndex = 5;
            this.cbeMonth.SelectedIndexChanged += new System.EventHandler(this.cbeMonth_SelectedIndexChanged);
            // 
            // lueYear
            // 
            this.lueYear.Location = new System.Drawing.Point(80, 63);
            this.lueYear.Name = "lueYear";
            this.lueYear.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueYear.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", " ")});
            this.lueYear.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.lueYear.Properties.NullText = "";
            this.lueYear.Size = new System.Drawing.Size(122, 20);
            this.lueYear.TabIndex = 1;
            this.lueYear.EditValueChanged += new System.EventHandler(this.lueYear_EditValueChanged);
            // 
            // lblSchedule
            // 
            this.lblSchedule.Location = new System.Drawing.Point(12, 146);
            this.lblSchedule.Name = "lblSchedule";
            this.lblSchedule.Size = new System.Drawing.Size(48, 14);
            this.lblSchedule.TabIndex = 2;
            this.lblSchedule.Text = "计划名称";
            // 
            // lblMonth
            // 
            this.lblMonth.Location = new System.Drawing.Point(38, 103);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(24, 14);
            this.lblMonth.TabIndex = 4;
            this.lblMonth.Text = "月份";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.splitContainerControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(1, 47);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(913, 481);
            this.panelControl1.TabIndex = 5;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.groupInfo);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.groupShift);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(909, 477);
            this.splitContainerControl1.SplitterPosition = 314;
            this.splitContainerControl1.TabIndex = 4;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // ShiftCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ShiftCtrl";
            this.Size = new System.Drawing.Size(915, 528);
            this.Load += new System.EventHandler(this.ShiftCtrl_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupShift)).EndInit();
            this.groupShift.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shiftControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shiftView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupInfo)).EndInit();
            this.groupInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lueSchedule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueYear.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl groupInfo;
        private DevExpress.XtraEditors.LookUpEdit lueYear;
        private DevExpress.XtraEditors.LabelControl lblYear;
        private DevExpress.XtraEditors.LookUpEdit lueSchedule;
        private DevExpress.XtraEditors.LabelControl lblSchedule;
        private DevExpress.XtraEditors.GroupControl groupShift;
        private DevExpress.XtraEditors.ComboBoxEdit cbeMonth;
        private DevExpress.XtraEditors.LabelControl lblMonth;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraGrid.GridControl shiftControl;
        private DevExpress.XtraGrid.Views.Grid.GridView shiftView;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton tsbDelete;
        private DevExpress.XtraEditors.SimpleButton tsbSave;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton tsbNew;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
    }
}
