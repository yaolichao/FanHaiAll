/*
<FileInfo>
  <Author>Alfred.Liu, SolarViewer Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 SolarViewer. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region using
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;

using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using SolarViewer.Gui.Core;
using SolarViewer.Hemera.Utils.Entities;
using SolarViewer.Hemera.Addins.WIP;
using SolarViewer.Hemera.Utils.Controls;

using DevExpress.XtraEditors;
#endregion

namespace SolarViewer.Hemera.Addins.EAP
{
    public partial class EDCDetailsCtrl : BaseUserCtrl
    {
        public EDCDetailsCtrl()
        {
            InitializeComponent();
        }

        public EDCDetailsCtrl(EdcGatherData edcData)
        {
            this._edcData = edcData;

            InitializeComponent();
            InitializeTablePanel();
            MapSHRDataToControls();

            LotBaseInfoCtrl lotInfoCtrl = new LotBaseInfoCtrl();
            lotInfoCtrl.SetValueToControl(_edcData.GetLotInfo());
            lotInfoCtrl.Dock = DockStyle.Fill;
            lotInfoCtrl.BackColor = Color.FromArgb(203, 219, 234);
            Panel011.Controls.Add(lotInfoCtrl);
        }

        private void btnTempStore_Click(object sender, EventArgs e)
        {
            if (_edcData.Insert() && _edcData.UpdateSHRDataToSqlServer())
            {
                MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");
            }
        }

        protected override void InitUIControls()
        {

        }
                
        private void InitializeTablePanel()
        {
            int boxCount = 0;
            TableLayoutPanel tablePanel = new TableLayoutPanel();
            tablePanel.SuspendLayout();

            tablePanel.Name = "TablePanelNew";
            tablePanel.TabIndex = 0;
            tablePanel.Font = new System.Drawing.Font("Arial", 11F);
            tablePanel.Size = new System.Drawing.Size(950, 130);
            tablePanel.Location = new System.Drawing.Point((this.Panel02.Width - tablePanel.Width) / 2, 19);
            tablePanel.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            tablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            tablePanel.ColumnCount = sampleQty + 1;
            for (int i = 0; i < tablePanel.ColumnCount; i++)
            {
                tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            }

            tablePanel.RowCount = paramQty + 1;
            for (int i = 0; i <= tablePanel.RowCount; i++)
            {
                tablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            }


            for (int i = 0; i < tablePanel.RowCount; i++)
            {
                for (int j = 0; j < tablePanel.ColumnCount; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    if (i == 0)
                    {
                        LabelControl lblCtrl = new LabelControl();
                        lblCtrl.Size = new System.Drawing.Size(24, 17);
                        lblCtrl.Anchor = AnchorStyles.Top;
                        lblCtrl.Appearance.Font = new System.Drawing.Font("Arial", 11F);
                        lblCtrl.Location = new System.Drawing.Point(0, 0);
                        lblCtrl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
                        lblCtrl.LookAndFeel.UseDefaultLookAndFeel = false;
                        lblCtrl.Appearance.Options.UseFont = true;
                        lblCtrl.TabIndex = 0;
                        lblCtrl.Text = j.ToString() + "片";
                        tablePanel.Controls.Add(lblCtrl, j, i);
                    }
                    else if (j == 0)
                    {
                        LabelControl lblCtrl = new LabelControl();
                        lblCtrl.Name = "lblCtrl" + i.ToString("00");
                        lblCtrl.Size = new System.Drawing.Size(80, 17);
                        lblCtrl.Anchor = AnchorStyles.Top;
                        lblCtrl.Appearance.Font = new System.Drawing.Font("Arial", 11F);
                        lblCtrl.Location = new System.Drawing.Point(0, 0);
                        lblCtrl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
                        lblCtrl.LookAndFeel.UseDefaultLookAndFeel = false;
                        lblCtrl.Appearance.Options.UseFont = true;
                        lblCtrl.TabIndex = 0;

                        switch (_edcData.DataType)
                        {
                            case "R":
                                switch (i)
                                {
                                    case 1:
                                        lblCtrl.Tag = "1";
                                        lblCtrl.Text = "刻蚀前电阻";
                                        break;
                                    case 2:
                                        lblCtrl.Tag = "2";
                                        lblCtrl.Text = "刻蚀后电阻";
                                        break;
                                    case 3:
                                        lblCtrl.Tag = "3";
                                        lblCtrl.Text = "电阻提升量";
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "W":
                                switch (i)
                                {
                                    case 1:
                                        lblCtrl.Tag = "1";
                                        lblCtrl.Text = "清洗前称重";
                                        break;
                                    case 2:
                                        lblCtrl.Tag = "2";
                                        lblCtrl.Text = "清洗后称重";
                                        break;
                                    case 3:
                                        lblCtrl.Tag = "3";
                                        lblCtrl.Text = "清洗减薄量";
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }

                        tablePanel.Controls.Add(lblCtrl, j, i);
                    }
                    else
                    {

                        TextEdit txtBox = new TextEdit();
                        boxCount++;
                        ((ISupportInitialize)(txtBox.Properties)).BeginInit();
                        txtBox.Dock = System.Windows.Forms.DockStyle.Top;
                        txtBox.Location = new System.Drawing.Point(0, 0);
                        txtBox.Name = "txtBox" + boxCount.ToString("00");
                        txtBox.Properties.Appearance.Font = new System.Drawing.Font("Arial", 12F);
                        txtBox.Properties.Appearance.Options.UseFont = true;
                        txtBox.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
                        txtBox.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
                        txtBox.Size = new System.Drawing.Size(160, 25);
                        txtBox.Properties.ReadOnly = true;
                        txtBox.TabIndex = boxCount;
                        ((ISupportInitialize)(txtBox.Properties)).EndInit();

                        txtBox.TextChanged += (s, e) =>
                        {
                            TextEdit txtEdit = s as TextEdit;
                            if (txtEdit.Text != string.Empty)
                            {
                                int boxIndex = int.Parse(txtEdit.Name.Substring(txtEdit.Name.Length - 2));
                                if (boxIndex > sampleQty * (paramQty - 2) && boxIndex <= sampleQty * (paramQty - 1))
                                {
                                    TextEdit prevBox = this.Panel02.Controls.Find("txtBox" + ((boxIndex - sampleQty * (paramQty - 2))).ToString("00"), true)[0] as TextEdit;
                                    TextEdit nextBox = this.Panel02.Controls.Find("txtBox" + boxIndex.ToString("00"), true)[0] as TextEdit;
                                    TextEdit calcBox = this.Panel02.Controls.Find("txtBox" + ((sampleQty * (paramQty - 1)) +
                                                                                 (boxIndex - sampleQty * (paramQty - 2))).ToString("00"), true)[0] as TextEdit;

                                    calcBox.Text = (Convert.ToDouble(prevBox.Text) - Convert.ToDouble(nextBox.Text)).ToString(); 
                                }

                                int lblIndex = ((boxIndex - 1) / sampleQty) + 1;
                                LabelControl lblCtrl = this.Panel02.Controls.Find("lblCtrl" + lblIndex.ToString("00"), true)[0] as LabelControl;
                                List<string> listValue;
                                if (_edcData.SHRDic.TryGetValue(lblCtrl.Tag.ToString(), out listValue))
                                {
                                    listValue.Add(txtEdit.Text);
                                }
                                else
                                {
                                    _edcData.SHRDic.Add(lblCtrl.Tag.ToString(), new List<string>() { txtEdit.Text });
                                }
                            }
                        };

                        tablePanel.Controls.Add(txtBox, j, i);
                    }
                }
            }

            tablePanel.ResumeLayout(false);
            tablePanel.PerformLayout();
            this.Panel02.Controls.Add(tablePanel);
        }

        private void MapSHRDataToControls()
        {
            DataTable shrTable = _edcData.GetSHRDataFromSqlServer();
            for (int i = 0; i < shrTable.Rows.Count; i++)
            {
                TextEdit txtBox = this.Panel02.Controls.Find("txtBox" + (i + 1).ToString("00"), true)[0] as TextEdit;
                if (txtBox.Text != string.Empty)
                {
                    txtBox.Text = shrTable.Rows[i]["SHROVERALL"].ToString();
                }
            }
            
        }

        private int sampleQty = 5;
        private int paramQty = 3;

        EdcGatherData _edcData = null;
        

    }
}
