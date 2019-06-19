namespace FanHai.Hemera.Addins.WMS.Gui
{
    partial class OutDeliveryQCQuerry
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.QC_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.QC_ITEM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PKG_MAT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BILL_BRND = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CANTR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.F_OUTB_CUSTM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DATA_FORMT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LIST_ABSENCE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LiST_ERR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CELL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MOD_ERR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.QLVL_ERR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FRAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BRND_PARM_ERR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CONT_LOCK_BRK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CUSTM_CK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.QC_PERSON = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BATCHNO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STATUS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VBELN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.POSNR = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(898, 594);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.QC_NO,
            this.QC_ITEM,
            this.PKG_MAT,
            this.BILL_BRND,
            this.CANTR,
            this.F_OUTB_CUSTM,
            this.EL,
            this.DATA_FORMT,
            this.LIST_ABSENCE,
            this.LiST_ERR,
            this.CELL,
            this.MOD_ERR,
            this.QLVL_ERR,
            this.FRAME,
            this.BRND_PARM_ERR,
            this.CONT_LOCK_BRK,
            this.CUSTM_CK,
            this.QC_PERSON,
            this.BATCHNO,
            this.STATUS,
            this.VBELN,
            this.POSNR});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // QC_NO
            // 
            this.QC_NO.Caption = "检验单单号";
            this.QC_NO.FieldName = "QC_NO";
            this.QC_NO.Name = "QC_NO";
            this.QC_NO.Visible = true;
            this.QC_NO.VisibleIndex = 0;
            this.QC_NO.Width = 105;
            // 
            // QC_ITEM
            // 
            this.QC_ITEM.Caption = "检验单行号";
            this.QC_ITEM.FieldName = "QC_ITEM";
            this.QC_ITEM.Name = "QC_ITEM";
            this.QC_ITEM.Visible = true;
            this.QC_ITEM.VisibleIndex = 1;
            this.QC_ITEM.Width = 80;
            // 
            // PKG_MAT
            // 
            this.PKG_MAT.Caption = "包材缺陷数";
            this.PKG_MAT.FieldName = "PKG_MAT";
            this.PKG_MAT.Name = "PKG_MAT";
            this.PKG_MAT.Visible = true;
            this.PKG_MAT.VisibleIndex = 2;
            this.PKG_MAT.Width = 80;
            // 
            // BILL_BRND
            // 
            this.BILL_BRND.Caption = "清单标识缺陷数";
            this.BILL_BRND.FieldName = "BILL_BRND";
            this.BILL_BRND.Name = "BILL_BRND";
            this.BILL_BRND.Visible = true;
            this.BILL_BRND.VisibleIndex = 3;
            this.BILL_BRND.Width = 90;
            // 
            // CANTR
            // 
            this.CANTR.Caption = "货柜缺陷数";
            this.CANTR.FieldName = "CANTR";
            this.CANTR.Name = "CANTR";
            this.CANTR.Visible = true;
            this.CANTR.VisibleIndex = 4;
            this.CANTR.Width = 80;
            // 
            // F_OUTB_CUSTM
            // 
            this.F_OUTB_CUSTM.Caption = "组件符合出厂或客户要求缺陷数";
            this.F_OUTB_CUSTM.FieldName = "F_OUTB_CUSTM";
            this.F_OUTB_CUSTM.Name = "F_OUTB_CUSTM";
            this.F_OUTB_CUSTM.Visible = true;
            this.F_OUTB_CUSTM.VisibleIndex = 5;
            this.F_OUTB_CUSTM.Width = 80;
            // 
            // EL
            // 
            this.EL.Caption = "EL";
            this.EL.FieldName = "EL";
            this.EL.Name = "EL";
            this.EL.Visible = true;
            this.EL.VisibleIndex = 6;
            // 
            // DATA_FORMT
            // 
            this.DATA_FORMT.Caption = "数据格式错误";
            this.DATA_FORMT.FieldName = "DATA_FORMT";
            this.DATA_FORMT.Name = "DATA_FORMT";
            this.DATA_FORMT.Visible = true;
            this.DATA_FORMT.VisibleIndex = 7;
            this.DATA_FORMT.Width = 80;
            // 
            // LIST_ABSENCE
            // 
            this.LIST_ABSENCE.Caption = "清单缺失";
            this.LIST_ABSENCE.FieldName = "LIST_ABSENCE";
            this.LIST_ABSENCE.Name = "LIST_ABSENCE";
            this.LIST_ABSENCE.Visible = true;
            this.LIST_ABSENCE.VisibleIndex = 8;
            this.LIST_ABSENCE.Width = 80;
            // 
            // LiST_ERR
            // 
            this.LiST_ERR.Caption = "清单错误";
            this.LiST_ERR.FieldName = "LiST_ERR";
            this.LiST_ERR.Name = "LiST_ERR";
            this.LiST_ERR.Visible = true;
            this.LiST_ERR.VisibleIndex = 9;
            this.LiST_ERR.Width = 80;
            // 
            // CELL
            // 
            this.CELL.Caption = "电池片";
            this.CELL.FieldName = "CELL";
            this.CELL.Name = "CELL";
            this.CELL.Visible = true;
            this.CELL.VisibleIndex = 10;
            // 
            // MOD_ERR
            // 
            this.MOD_ERR.Caption = "型号错误";
            this.MOD_ERR.FieldName = "MOD_ERR";
            this.MOD_ERR.Name = "MOD_ERR";
            this.MOD_ERR.Visible = true;
            this.MOD_ERR.VisibleIndex = 11;
            this.MOD_ERR.Width = 80;
            // 
            // QLVL_ERR
            // 
            this.QLVL_ERR.Caption = "质量等级不符合要求";
            this.QLVL_ERR.FieldName = "QLVL_ERR";
            this.QLVL_ERR.Name = "QLVL_ERR";
            this.QLVL_ERR.Visible = true;
            this.QLVL_ERR.VisibleIndex = 12;
            this.QLVL_ERR.Width = 90;
            // 
            // FRAME
            // 
            this.FRAME.Caption = "边框";
            this.FRAME.FieldName = "FRAME";
            this.FRAME.Name = "FRAME";
            this.FRAME.Visible = true;
            this.FRAME.VisibleIndex = 13;
            // 
            // BRND_PARM_ERR
            // 
            this.BRND_PARM_ERR.Caption = "铭牌参数错误";
            this.BRND_PARM_ERR.FieldName = "BRND_PARM_ERR";
            this.BRND_PARM_ERR.Name = "BRND_PARM_ERR";
            this.BRND_PARM_ERR.Visible = true;
            this.BRND_PARM_ERR.VisibleIndex = 14;
            this.BRND_PARM_ERR.Width = 90;
            // 
            // CONT_LOCK_BRK
            // 
            this.CONT_LOCK_BRK.Caption = "集装箱锁破损";
            this.CONT_LOCK_BRK.FieldName = "CONT_LOCK_BRK";
            this.CONT_LOCK_BRK.Name = "CONT_LOCK_BRK";
            this.CONT_LOCK_BRK.Visible = true;
            this.CONT_LOCK_BRK.VisibleIndex = 15;
            this.CONT_LOCK_BRK.Width = 90;
            // 
            // CUSTM_CK
            // 
            this.CUSTM_CK.Caption = "客户验货缺陷";
            this.CUSTM_CK.FieldName = "CUSTM_CK";
            this.CUSTM_CK.Name = "CUSTM_CK";
            this.CUSTM_CK.Visible = true;
            this.CUSTM_CK.VisibleIndex = 16;
            this.CUSTM_CK.Width = 90;
            // 
            // QC_PERSON
            // 
            this.QC_PERSON.Caption = "检验人";
            this.QC_PERSON.FieldName = "QC_PERSON";
            this.QC_PERSON.Name = "QC_PERSON";
            this.QC_PERSON.Visible = true;
            this.QC_PERSON.VisibleIndex = 17;
            this.QC_PERSON.Width = 60;
            // 
            // BATCHNO
            // 
            this.BATCHNO.Caption = "批次号";
            this.BATCHNO.FieldName = "BATCHNO";
            this.BATCHNO.Name = "BATCHNO";
            this.BATCHNO.Visible = true;
            this.BATCHNO.VisibleIndex = 18;
            // 
            // STATUS
            // 
            this.STATUS.Caption = "状态";
            this.STATUS.FieldName = "STATUS";
            this.STATUS.Name = "STATUS";
            this.STATUS.Visible = true;
            this.STATUS.VisibleIndex = 19;
            this.STATUS.Width = 45;
            // 
            // VBELN
            // 
            this.VBELN.Caption = "外向交货单号";
            this.VBELN.FieldName = "VBELN";
            this.VBELN.Name = "VBELN";
            this.VBELN.Visible = true;
            this.VBELN.VisibleIndex = 20;
            this.VBELN.Width = 80;
            // 
            // POSNR
            // 
            this.POSNR.Caption = "行号";
            this.POSNR.FieldName = "POSNR";
            this.POSNR.Name = "POSNR";
            this.POSNR.Visible = true;
            this.POSNR.VisibleIndex = 21;
            this.POSNR.Width = 60;
            // 
            // OutDeliveryQCQuerry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 594);
            this.Controls.Add(this.gridControl1);
            this.Name = "OutDeliveryQCQuerry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "质检明细显示";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn QC_NO;
        private DevExpress.XtraGrid.Columns.GridColumn QC_ITEM;
        private DevExpress.XtraGrid.Columns.GridColumn PKG_MAT;
        private DevExpress.XtraGrid.Columns.GridColumn BILL_BRND;
        private DevExpress.XtraGrid.Columns.GridColumn CANTR;
        private DevExpress.XtraGrid.Columns.GridColumn F_OUTB_CUSTM;
        private DevExpress.XtraGrid.Columns.GridColumn EL;
        private DevExpress.XtraGrid.Columns.GridColumn DATA_FORMT;
        private DevExpress.XtraGrid.Columns.GridColumn LIST_ABSENCE;
        private DevExpress.XtraGrid.Columns.GridColumn LiST_ERR;
        private DevExpress.XtraGrid.Columns.GridColumn CELL;
        private DevExpress.XtraGrid.Columns.GridColumn MOD_ERR;
        private DevExpress.XtraGrid.Columns.GridColumn QLVL_ERR;
        private DevExpress.XtraGrid.Columns.GridColumn FRAME;
        private DevExpress.XtraGrid.Columns.GridColumn BRND_PARM_ERR;
        private DevExpress.XtraGrid.Columns.GridColumn CONT_LOCK_BRK;
        private DevExpress.XtraGrid.Columns.GridColumn CUSTM_CK;
        private DevExpress.XtraGrid.Columns.GridColumn QC_PERSON;
        private DevExpress.XtraGrid.Columns.GridColumn BATCHNO;
        private DevExpress.XtraGrid.Columns.GridColumn STATUS;
        private DevExpress.XtraGrid.Columns.GridColumn VBELN;
        private DevExpress.XtraGrid.Columns.GridColumn POSNR;
    }
}