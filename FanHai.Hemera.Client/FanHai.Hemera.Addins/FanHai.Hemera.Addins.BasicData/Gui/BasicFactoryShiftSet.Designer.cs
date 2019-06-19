namespace SolarViewer.Hemera.Addins.BasicData
{
    partial class BasicFactoryShiftSet
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbUpdate = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.lueFactoryShift = new DevExpress.XtraEditors.LookUpEdit();
            this.lueShift = new DevExpress.XtraEditors.LookUpEdit();
            this.lueFactory = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.deDate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.lueQFactory = new DevExpress.XtraEditors.LookUpEdit();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.deQDate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.gcFactoryShift = new DevExpress.XtraGrid.GridControl();
            this.gvFactoryShift = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.FACTORYSHIFTSET_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FACTORYROOM_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DATA_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SHIFT_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FACTORYSHIFT_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactoryShift.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueShift.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactory.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueQFactory.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deQDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deQDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcFactoryShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFactoryShift)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNew,
            this.tsbUpdate,
            this.tsbDelete,
            this.tsbCancel,
            this.tsbSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(660, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNew
            // 
            this.tsbNew.Image = global::SolarViewer.Hemera.Addins.BasicData.Properties.Resources.document_add;
            this.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(49, 22);
            this.tsbNew.Text = "新增";
            this.tsbNew.Click += new System.EventHandler(this.tsbNew_Click);
            // 
            // tsbUpdate
            // 
            this.tsbUpdate.Image = global::SolarViewer.Hemera.Addins.BasicData.Properties.Resources.edit_save;
            this.tsbUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUpdate.Name = "tsbUpdate";
            this.tsbUpdate.Size = new System.Drawing.Size(49, 22);
            this.tsbUpdate.Text = "修改";
            this.tsbUpdate.Click += new System.EventHandler(this.tsbUpdate_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.Image = global::SolarViewer.Hemera.Addins.BasicData.Properties.Resources.document_delete;
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(49, 22);
            this.tsbDelete.Text = "删除";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tsbCancel
            // 
            this.tsbCancel.Image = global::SolarViewer.Hemera.Addins.BasicData.Properties.Resources.cancel;
            this.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Size = new System.Drawing.Size(49, 22);
            this.tsbCancel.Text = "取消";
            this.tsbCancel.Click += new System.EventHandler(this.tsbCancel_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = global::SolarViewer.Hemera.Addins.BasicData.Properties.Resources.save_accept;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(23, 22);
            this.tsbSave.Text = "保存";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 25);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(660, 44);
            this.panelControl1.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(8, 8);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(144, 29);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "生产排班维护";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.lueFactoryShift);
            this.panelControl2.Controls.Add(this.lueShift);
            this.panelControl2.Controls.Add(this.lueFactory);
            this.panelControl2.Controls.Add(this.labelControl5);
            this.panelControl2.Controls.Add(this.labelControl4);
            this.panelControl2.Controls.Add(this.labelControl3);
            this.panelControl2.Controls.Add(this.deDate);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 69);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(660, 47);
            this.panelControl2.TabIndex = 2;
            // 
            // lueFactoryShift
            // 
            this.lueFactoryShift.Location = new System.Drawing.Point(546, 12);
            this.lueFactoryShift.Name = "lueFactoryShift";
            this.lueFactoryShift.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueFactoryShift.Properties.NullText = "";
            this.lueFactoryShift.Size = new System.Drawing.Size(100, 21);
            this.lueFactoryShift.TabIndex = 10;
            // 
            // lueShift
            // 
            this.lueShift.Location = new System.Drawing.Point(355, 12);
            this.lueShift.Name = "lueShift";
            this.lueShift.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueShift.Properties.NullText = "";
            this.lueShift.Size = new System.Drawing.Size(100, 21);
            this.lueShift.TabIndex = 9;
            // 
            // lueFactory
            // 
            this.lueFactory.Location = new System.Drawing.Point(48, 12);
            this.lueFactory.Name = "lueFactory";
            this.lueFactory.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueFactory.Properties.NullText = "";
            this.lueFactory.Size = new System.Drawing.Size(100, 21);
            this.lueFactory.TabIndex = 8;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(482, 17);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 14);
            this.labelControl5.TabIndex = 5;
            this.labelControl5.Text = "生产排班：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(165, 17);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 14);
            this.labelControl4.TabIndex = 4;
            this.labelControl4.Text = "日期：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(324, 17);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 14);
            this.labelControl3.TabIndex = 3;
            this.labelControl3.Text = "班别：";
            // 
            // deDate
            // 
            this.deDate.EditValue = null;
            this.deDate.Location = new System.Drawing.Point(205, 12);
            this.deDate.Name = "deDate";
            this.deDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.deDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deDate.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.deDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deDate.Size = new System.Drawing.Size(100, 21);
            this.deDate.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(8, 17);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 14);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "厂别：";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.lueQFactory);
            this.panelControl3.Controls.Add(this.btnQuery);
            this.panelControl3.Controls.Add(this.labelControl7);
            this.panelControl3.Controls.Add(this.deQDate);
            this.panelControl3.Controls.Add(this.labelControl6);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 116);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(660, 44);
            this.panelControl3.TabIndex = 3;
            // 
            // lueQFactory
            // 
            this.lueQFactory.Location = new System.Drawing.Point(48, 10);
            this.lueQFactory.Name = "lueQFactory";
            this.lueQFactory.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueQFactory.Properties.NullText = "";
            this.lueQFactory.Size = new System.Drawing.Size(100, 21);
            this.lueQFactory.TabIndex = 9;
            // 
            // btnQuery
            // 
            this.btnQuery.Image = global::SolarViewer.Hemera.Addins.BasicData.Properties.Resources.select;
            this.btnQuery.Location = new System.Drawing.Point(364, 8);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(61, 23);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(165, 15);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(36, 14);
            this.labelControl7.TabIndex = 3;
            this.labelControl7.Text = "日期：";
            // 
            // deQDate
            // 
            this.deQDate.EditValue = null;
            this.deQDate.Location = new System.Drawing.Point(205, 10);
            this.deQDate.Name = "deQDate";
            this.deQDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deQDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.deQDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deQDate.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.deQDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deQDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deQDate.Size = new System.Drawing.Size(100, 21);
            this.deQDate.TabIndex = 2;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(8, 15);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(36, 14);
            this.labelControl6.TabIndex = 0;
            this.labelControl6.Text = "厂别：";
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.gcFactoryShift);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl4.Location = new System.Drawing.Point(0, 160);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(660, 259);
            this.panelControl4.TabIndex = 4;
            // 
            // gcFactoryShift
            // 
            this.gcFactoryShift.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcFactoryShift.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gcFactoryShift.Location = new System.Drawing.Point(3, 3);
            this.gcFactoryShift.LookAndFeel.SkinName = "Blue";
            this.gcFactoryShift.MainView = this.gvFactoryShift;
            this.gcFactoryShift.Name = "gcFactoryShift";
            this.gcFactoryShift.Size = new System.Drawing.Size(654, 253);
            this.gcFactoryShift.TabIndex = 3;
            this.gcFactoryShift.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvFactoryShift});
            // 
            // gvFactoryShift
            // 
            this.gvFactoryShift.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.FACTORYSHIFTSET_KEY,
            this.FACTORYROOM_NAME,
            this.DATA_DATE,
            this.SHIFT_NAME,
            this.FACTORYSHIFT_NAME});
            this.gvFactoryShift.GridControl = this.gcFactoryShift;
            this.gvFactoryShift.Name = "gvFactoryShift";
            this.gvFactoryShift.OptionsBehavior.Editable = false;
            this.gvFactoryShift.OptionsSelection.MultiSelect = true;
            this.gvFactoryShift.OptionsView.ColumnAutoWidth = false;
            this.gvFactoryShift.OptionsView.ShowGroupPanel = false;
            // 
            // FACTORYSHIFTSET_KEY
            // 
            this.FACTORYSHIFTSET_KEY.Caption = "生产排班记录主键";
            this.FACTORYSHIFTSET_KEY.FieldName = "FACTORYSHIFTSET_KEY";
            this.FACTORYSHIFTSET_KEY.Name = "FACTORYSHIFTSET_KEY";
            // 
            // FACTORYROOM_NAME
            // 
            this.FACTORYROOM_NAME.Caption = "厂别";
            this.FACTORYROOM_NAME.FieldName = "FACTORYROOM_NAME";
            this.FACTORYROOM_NAME.Name = "FACTORYROOM_NAME";
            this.FACTORYROOM_NAME.Visible = true;
            this.FACTORYROOM_NAME.VisibleIndex = 0;
            // 
            // DATA_DATE
            // 
            this.DATA_DATE.Caption = "日期";
            this.DATA_DATE.FieldName = "DATA_DATE";
            this.DATA_DATE.Name = "DATA_DATE";
            this.DATA_DATE.Visible = true;
            this.DATA_DATE.VisibleIndex = 1;
            // 
            // SHIFT_NAME
            // 
            this.SHIFT_NAME.Caption = "班别";
            this.SHIFT_NAME.FieldName = "SHIFT_NAME";
            this.SHIFT_NAME.Name = "SHIFT_NAME";
            this.SHIFT_NAME.Visible = true;
            this.SHIFT_NAME.VisibleIndex = 2;
            // 
            // FACTORYSHIFT_NAME
            // 
            this.FACTORYSHIFT_NAME.Caption = "生产排班";
            this.FACTORYSHIFT_NAME.FieldName = "FACTORYSHIFT_NAME";
            this.FACTORYSHIFT_NAME.Name = "FACTORYSHIFT_NAME";
            this.FACTORYSHIFT_NAME.Visible = true;
            this.FACTORYSHIFT_NAME.VisibleIndex = 3;
            // 
            // BasicFactoryShiftSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl4);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "BasicFactoryShiftSet";
            this.Size = new System.Drawing.Size(660, 419);
            this.Load += new System.EventHandler(this.BasicFactoryShiftSet_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactoryShift.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueShift.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactory.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueQFactory.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deQDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deQDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcFactoryShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFactoryShift)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbUpdate;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripButton tsbCancel;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit deDate;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.DateEdit deQDate;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraGrid.GridControl gcFactoryShift;
        private DevExpress.XtraGrid.Views.Grid.GridView gvFactoryShift;
        private DevExpress.XtraGrid.Columns.GridColumn FACTORYROOM_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn DATA_DATE;
        private DevExpress.XtraGrid.Columns.GridColumn SHIFT_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn FACTORYSHIFT_NAME;
        private DevExpress.XtraEditors.LookUpEdit lueFactory;
        private DevExpress.XtraEditors.LookUpEdit lueFactoryShift;
        private DevExpress.XtraEditors.LookUpEdit lueShift;
        private DevExpress.XtraEditors.LookUpEdit lueQFactory;
        private DevExpress.XtraGrid.Columns.GridColumn FACTORYSHIFTSET_KEY;
    }
}
