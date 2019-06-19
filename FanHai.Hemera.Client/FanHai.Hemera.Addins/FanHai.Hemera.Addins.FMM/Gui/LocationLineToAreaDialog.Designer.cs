namespace FanHai.Hemera.Addins.FMM
{
    partial class LineToAreaDialog
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.UnSelectGroup = new DevExpress.XtraEditors.GroupControl();
            this.clbUnSelectLine = new System.Windows.Forms.CheckedListBox();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.selectGroup = new DevExpress.XtraEditors.GroupControl();
            this.clbSelectLine = new System.Windows.Forms.CheckedListBox();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnDeleteLine = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddLine = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnSelectGroup)).BeginInit();
            this.UnSelectGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectGroup)).BeginInit();
            this.selectGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.UnSelectGroup);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Controls.Add(this.selectGroup);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(477, 359);
            this.panelControl1.TabIndex = 10;
            // 
            // UnSelectGroup
            // 
            this.UnSelectGroup.Controls.Add(this.clbUnSelectLine);
            this.UnSelectGroup.Dock = System.Windows.Forms.DockStyle.Right;
            this.UnSelectGroup.Location = new System.Drawing.Point(300, 2);
            this.UnSelectGroup.Name = "UnSelectGroup";
            this.UnSelectGroup.Size = new System.Drawing.Size(175, 355);
            this.UnSelectGroup.TabIndex = 6;
            this.UnSelectGroup.Text = "未选线别";
            // 
            // clbUnSelectLine
            // 
            this.clbUnSelectLine.CheckOnClick = true;
            this.clbUnSelectLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbUnSelectLine.FormattingEnabled = true;
            this.clbUnSelectLine.Location = new System.Drawing.Point(2, 23);
            this.clbUnSelectLine.MultiColumn = true;
            this.clbUnSelectLine.Name = "clbUnSelectLine";
            this.clbUnSelectLine.Size = new System.Drawing.Size(171, 330);
            this.clbUnSelectLine.TabIndex = 12;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btnDeleteLine);
            this.groupControl1.Controls.Add(this.btnAddLine);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(177, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.ShowCaption = false;
            this.groupControl1.Size = new System.Drawing.Size(298, 355);
            this.groupControl1.TabIndex = 13;
            this.groupControl1.Text = "groupControl1";
            // 
            // selectGroup
            // 
            this.selectGroup.Controls.Add(this.clbSelectLine);
            this.selectGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.selectGroup.Location = new System.Drawing.Point(2, 2);
            this.selectGroup.Name = "selectGroup";
            this.selectGroup.Size = new System.Drawing.Size(175, 355);
            this.selectGroup.TabIndex = 13;
            this.selectGroup.Text = "已有线别";
            // 
            // clbSelectLine
            // 
            this.clbSelectLine.CheckOnClick = true;
            this.clbSelectLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbSelectLine.FormattingEnabled = true;
            this.clbSelectLine.Location = new System.Drawing.Point(2, 23);
            this.clbSelectLine.Name = "clbSelectLine";
            this.clbSelectLine.Size = new System.Drawing.Size(171, 330);
            this.clbSelectLine.TabIndex = 13;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnClose);
            this.panelControl2.Controls.Add(this.btnSave);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 359);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(477, 34);
            this.panelControl2.TabIndex = 11;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(382, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 25);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(300, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(77, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDeleteLine
            // 
            this.btnDeleteLine.Location = new System.Drawing.Point(32, 130);
            this.btnDeleteLine.Name = "btnDeleteLine";
            this.btnDeleteLine.Size = new System.Drawing.Size(68, 31);
            this.btnDeleteLine.TabIndex = 6;
            this.btnDeleteLine.Text = ">>";
            this.btnDeleteLine.Click += new System.EventHandler(this.btnDeleteLine_Click);
            // 
            // btnAddLine
            // 
            this.btnAddLine.Location = new System.Drawing.Point(32, 180);
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.Size = new System.Drawing.Size(69, 31);
            this.btnAddLine.TabIndex = 7;
            this.btnAddLine.Text = "<<";
            this.btnAddLine.Click += new System.EventHandler(this.btnAddLine_Click);
            // 
            // LineToAreaDialog
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 393);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LineToAreaDialog";
            this.Load += new System.EventHandler(this.ContentPrivilege_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UnSelectGroup)).EndInit();
            this.UnSelectGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.selectGroup)).EndInit();
            this.selectGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.GroupControl UnSelectGroup;
        private System.Windows.Forms.CheckedListBox clbUnSelectLine;
        private DevExpress.XtraEditors.GroupControl selectGroup;
        private System.Windows.Forms.CheckedListBox clbSelectLine;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnDeleteLine;
        private DevExpress.XtraEditors.SimpleButton btnAddLine;
    }
}