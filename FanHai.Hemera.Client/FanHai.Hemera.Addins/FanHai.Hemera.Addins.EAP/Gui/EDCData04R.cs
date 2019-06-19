using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Utils.Entities;
using SolarViewer.Gui.Core;
using SolarViewer.Gui.Framework.Gui;


using DevExpress.XtraEditors;
using SolarViewer.Hemera.Utils.Common;
using SolarViewer.Hemera.Utils.Controls;

namespace SolarViewer.Hemera.Addins.EAP
{
    public partial class EDCData04R : BaseUserCtrl
    {
        #region Private variable
        private EdcGatherData _edcData = null;
        private DataSet dsParam = new DataSet();
        private DataSet dsCollectionData = new DataSet();
        private string edcInsKey = string.Empty;
        private string lotKey = string.Empty;
        private TableLayoutPanel tablePanel = null;
        private ComputerEntity computerEntity = new ComputerEntity();
        private bool allowInput = new bool();
        #endregion

        /// <summary>
        /// 需要填写的行
        /// </summary>
        private int NeedWriteH = 0;
        private bool THreadGet = true;
        //private bool IsError = false;

        public EDCData04R()
        {
            InitializeComponent();
        }
        public EDCData04R(EdcGatherData edcData)
        {
            InitializeComponent();
            btWData.Enabled = false;
            allowInput = computerEntity.GetComputerInfoOfInput("R");
            _edcData = edcData;

            DataSet dsLotInfo = new DataSet();
            dsLotInfo = _edcData.GetLotInfo();

            #region BindBaseLotInfo
            LotBaseInfoCtrl lotInfoCtrl = new LotBaseInfoCtrl();
            lotInfoCtrl.SetValueToControl(dsLotInfo);
            lotInfoCtrl.Dock = DockStyle.Fill;
            lotInfoCtrl.BackColor = Color.FromArgb(203, 219, 234);
            this.gcLotInfo.Controls.Add(lotInfoCtrl);
            #endregion

            #region BindParamInfo
            if (dsLotInfo.Tables.Count > 0 && dsLotInfo.Tables[0].Rows.Count > 0)
            {
                edcInsKey = dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_EDC_INS_KEY].ToString();
                lotKey = dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();
            }
            #endregion

            dsParam = _edcData.GetPointParams(edcInsKey);
            if (_edcData.ErrorMsg == string.Empty)
            {
                this.gcParam.MainView = gvParam;
                if (dsParam != null && dsParam.Tables.Count > 0)
                {
                    gcParam.DataSource = dsParam.Tables[0];
                    DataRow[] dataRow = dsParam.Tables[0].Select("DEVICE_TYPE='W'");
                    if (dataRow.Length > 0)
                    {
                        btWData.Enabled = true;
                    }
                }
            }
            else
            {
                MessageService.ShowError("查询参数出错" + _edcData.ErrorMsg);
            }

            if (InitializeUI())
            {
                #region BindInitialData
                BindInitialData();
                #endregion
            }
            else
            {
                this.btSubmit.Enabled = false;
                btDeleteData.Enabled = false;
            }            
            //Thread thr = new Thread(new ThreadStart(this.GetData));
            //thr.Start();

        }

        private void BindInitialData()
        {
            LotEDCEntity lotEntity = new LotEDCEntity();
            dsCollectionData = lotEntity.GetEDCCollectionData(lotKey, edcInsKey);
            if (lotEntity.ErrorMsg == string.Empty)
            {
                try
                {
                    if (dsCollectionData.Tables.Count > 0 && dsCollectionData.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsCollectionData.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 1; j < tablePanel.RowCount; j++)
                            {
                                string strName = "lblParam" + j.ToString("00");
                                LabelControl lblCtrl = (LabelControl)tablePanel.Controls.Find(strName, true)[0];
                                if (lblCtrl.Tag.ToString() == dsCollectionData.Tables[0].Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString())
                                {
                                    //j为tablePanel的行号
                                    int column = Convert.ToInt32(dsCollectionData.Tables[0].Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ]);
                                    TextEdit textEdit = (TextEdit)tablePanel.Controls.Find("txtBox" + j.ToString() + column.ToString(), true)[0];
                                    textEdit.Text = dsCollectionData.Tables[0].Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE].ToString();
                                    if (textEdit.Tag.ToString().Substring(0, 1) == _edcData.DataType && 
                                        (textEdit.Tag.ToString().Substring(1, 1) == "W" || textEdit.Tag.ToString().Substring(1,1)=="N"))
                                    {
                                        textEdit.Properties.ReadOnly = true;
                                    }
                                    break;
                                }
                            }
                        }
                        if (CheckFinish())
                        {
                            btSubmit.Enabled = false;
                            btDeleteData.Enabled = false;
                        }
                        else
                        {
                            btSubmit.Enabled = true;
                            btDeleteData.Enabled = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageService.ShowError(ex.Message);
                }
            }
            else
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
            }

        }

        private bool CheckFinish()
        {
            bool bResult = true;
            for (int i = 1; i < tablePanel.RowCount; i++)
            {
                for (int j = 1; j <= Convert.ToInt32(dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]); j++)
                {
                    TextEdit textEdit = (TextEdit)tablePanel.Controls.Find("txtBox" + i.ToString() + j.ToString(), true)[0];
                    if (textEdit.Tag.ToString().Substring(0, 1) == _edcData.DataType 
                        && (textEdit.Tag.ToString().Substring(1, 1) == "W" ||textEdit.Tag.ToString().Substring(1,1)=="N")
                        && textEdit.Text == string.Empty)
                    {
                        bResult = false;
                        return bResult;
                    }
                }
            }
            return bResult;
        }

        public delegate void SetDataToTextBoxDelegate();

        private DataTable shrTable_Q = new DataTable();
        private DataTable shrTable_H = new DataTable();

        private void GetData()
        {
            #region 骆中玉修改
            
            shrTable_Q = _edcData.GetSHRDataFromSqlServer("N");

            if (shrTable_Q != null)
            {
                if (shrTable_Q.Rows.Count == _edcData.SampQty)
                    THreadGet = false;

                for (int i = 0; i < shrTable_Q.Rows.Count; i++)
                {
                    SetText("txtBox" + NeedWriteH.ToString() + (i + 1).ToString(), shrTable_Q.Rows[i]["SHROVERALL"].ToString());
                }
            }
            #endregion


            #region 骆中玉注释

            return;
            while (true)
            {
                if (THreadGet)
                {
                    shrTable_Q = _edcData.GetSHRDataFromSqlServer("N");

                    if (shrTable_Q != null)
                    {
                        if (shrTable_Q.Rows.Count == _edcData.SampQty)
                            THreadGet = false;

                        for (int i = 0; i < shrTable_Q.Rows.Count; i++)
                        {
                            SetText("txtBox" + NeedWriteH.ToString() + (i + 1).ToString(), shrTable_Q.Rows[i]["SHROVERALL"].ToString());
                        }
                    }

                    #region 无用代码
                    /*
                    if (dsParam.Tables[0].Rows[0][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_NAME].ToString().IndexOf("刻蚀前") > 0)
                    {
                        shrTable_Q = _edcData.GetSHRDataFromSqlServer("N");

                        if (shrTable_Q != null)
                        {
                            if (shrTable_Q.Rows.Count == _edcData.SampQty)
                                THreadGet = false;
                            for (int i = 0; i < shrTable_Q.Rows.Count; i++)
                            {
                                SetText("txtBox01" + (i + 1).ToString("00"), shrTable_Q.Rows[i]["SHROVERALL"].ToString());
                                if (Convert.ToDouble(dsParam.Tables[0].Rows[0][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY]) > Convert.ToDouble(shrTable_Q.Rows[i]["SHROVERALL"]) 
                                    || Convert.ToDouble(dsParam.Tables[0].Rows[0][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY]) < Convert.ToDouble(shrTable_Q.Rows[i]["SHROVERALL"]))
                                {
                                    IsError = true;
                                    SetTextBoxColor("txtBox01" + (i + 1).ToString("00"));
                                }
                            }
                        }
                    }
                    else
                    {
                        shrTable_Q = _edcData.GetSHRDataFromSqlServer("Y");
                        shrTable_H = _edcData.GetSHRDataFromSqlServer("N");
                        if (shrTable_H != null)
                        {
                            if (shrTable_H.Rows.Count == _edcData.SampQty)
                                THreadGet = false;
                            for (int i = shrTable_Q.Rows.Count - 1; i >= 0; i--)
                            {
                                SetText("txtBox02" + (i + 1).ToString("00"), shrTable_Q.Rows[i]["SHROVERALL"].ToString());
                                if (Convert.ToDouble(dsParam.Tables[0].Rows[1][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY]) > Convert.ToDouble(shrTable_Q.Rows[i]["SHROVERALL"]) 
                                    || Convert.ToDouble(dsParam.Tables[0].Rows[1][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY]) < Convert.ToDouble(shrTable_Q.Rows[i]["SHROVERALL"]))
                                {
                                    IsError = true;
                                    SetTextBoxColor("txtBox02" + (i + 1).ToString("00"));
                                }
                                
                            }
                            for (int i = 0; i < shrTable_H.Rows.Count; i++)
                            {
                                SetText("txtBox01" + (i + 1).ToString("00"), shrTable_H.Rows[i]["SHROVERALL"].ToString());
                                if (Convert.ToDouble(dsParam.Tables[0].Rows[0][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY]) > Convert.ToDouble(shrTable_H.Rows[i]["SHROVERALL"]) 
                                    || Convert.ToDouble(dsParam.Tables[0].Rows[0][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY]) < Convert.ToDouble(shrTable_H.Rows[i]["SHROVERALL"]))
                                {
                                    IsError = true;
                                    SetTextBoxColor("txtBox01" + (i + 1).ToString("00"));
                                }
                                Double c_Date = Convert.ToDouble(shrTable_H.Rows[i]["SHROVERALL"]) - Convert.ToDouble(shrTable_Q.Rows[shrTable_Q.Rows.Count - 1 - i]["SHROVERALL"]);
                                SetText("txtBox03" + (i + 1).ToString("00"), c_Date.ToString());
                                if (Convert.ToDouble(dsParam.Tables[0].Rows[2][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY]) > c_Date
                                    || Convert.ToDouble(dsParam.Tables[0].Rows[0][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY]) < c_Date)
                                {
                                    IsError = true;
                                    SetTextBoxColor("txtBox03" + (i + 1).ToString("00"));
                                }
                            }
                        }
                     
                    }                   
                       */
                    #endregion
                }
                else
                {
                    break;
                }
                //Thread.Sleep(10 * 1000);
            }
            #endregion
        }

        delegate void SetTextCallback(string TextName, string text);

        private void SetText(string TextName, string text)
        {
            try
            {
                TextEdit textEdit = (TextEdit)Controls.Find(TextName, true)[0];
                if (textEdit.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    this.Invoke(d, new object[] { TextName, text });
                }
                else
                {
                    if (string.IsNullOrEmpty(textEdit.Text))
                    {
                        textEdit.Text = text;
                    }
                    else
                    {
                        int i = int.Parse(TextName.Substring(TextName.Length - 1, 1)) + 1;
                        string textn = TextName.Substring(0, TextName.Length - 1);
                        SetText(textn + i.ToString(), text);

                    }
                }

            }
            catch (Exception e)
            {
                //throw ex;
            }
        }

        delegate void SetTextColorCallback(string TextName);
        private void SetTextBoxColor(string TextName)
        {
            TextEdit textEdit = (TextEdit)Controls.Find(TextName, true)[0];
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (textEdit.InvokeRequired)
            {
                SetTextColorCallback d = new SetTextColorCallback(SetTextBoxColor);
                this.Invoke(d, new object[] { TextName });
            }
            else
            {
                textEdit.BackColor = Color.Red;
            }            
        }

        private bool InitializeUI()
        {
            try
            {
                int columnCount = 0;
                int rowCount = 0;
                int boxCount = 0;
                tablePanel = new TableLayoutPanel();
                tablePanel.SuspendLayout();

                tablePanel.Name = "TablePanelNew";
                tablePanel.TabIndex = 0;
                tablePanel.Font = new System.Drawing.Font("Arial", 9F);
                tablePanel.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
                tablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
                if (dsParam.Tables.Count > 0 && dsParam.Tables[0].Rows.Count > 0)
                {
                    object maxCountObj = dsParam.Tables[0].Compute("max(PARAM_COUNT)", "");
                    if (maxCountObj == null || maxCountObj == DBNull.Value)
                    {
                        throw new Exception("请配置需要抽检的电池片数量");
                    }
                    else
                    {
                        columnCount = Convert.ToInt32(maxCountObj);
                    }
                    rowCount = dsParam.Tables[0].Rows.Count;
                    tablePanel.ColumnCount = columnCount + 1;
                    tablePanel.RowCount = rowCount + 1;
                    tablePanel.Width = 160 * tablePanel.ColumnCount + 24;
                    tablePanel.Height = 25 * tablePanel.RowCount + 10;
                    for (int i = 0; i < tablePanel.ColumnCount; i++)
                    {
                        tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
                    }
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
                                lblCtrl.Appearance.Font = new System.Drawing.Font("Arial", 9F);
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
                                lblCtrl.Name = "lblParam" + i.ToString("00");
                                lblCtrl.Size = new System.Drawing.Size(80, 17);
                                lblCtrl.Anchor = AnchorStyles.Top;
                                lblCtrl.Appearance.Font = new System.Drawing.Font("Arial", 9F);
                                lblCtrl.Location = new System.Drawing.Point(0, 0);
                                lblCtrl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
                                lblCtrl.LookAndFeel.UseDefaultLookAndFeel = false;
                                lblCtrl.Appearance.Options.UseFont = true;
                                lblCtrl.TabIndex = 0;
                                lblCtrl.Tag = dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_KEY].ToString();
                                lblCtrl.Text = dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_NAME].ToString();
                                tablePanel.Controls.Add(lblCtrl, j, i);
                            }
                            else
                            {
                                if (j < Convert.ToInt32(dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]) + 1)
                                {
                                    string paramType = dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE].ToString();
                                    if (paramType.Trim().Length == 0)
                                    {
                                        throw new Exception("参数类型不能为空，请检查抽检规则是否配置参数类型");
                                    }
                                    string deviceType = dsParam.Tables[0].Rows[i - 1][BASE_PARAMETER_FIELDS.FIELD_DEVICE_TYPE].ToString();
                                    //edit by rayna 2011-4-28
                                    string dataType = dsParam.Tables[0].Rows[i - 1][BASE_PARAMETER_FIELDS.FIELD_DATA_TYPE].ToString();
                                    if (dataType == string.Empty)
                                        dataType = "6";
                                    AttributeDataType paramDataType = (AttributeDataType)Convert.ToInt32(dataType);
                                    //end
                                    TextEdit txtBox = new TextEdit();

                                    boxCount++;
                                    ((ISupportInitialize)(txtBox.Properties)).BeginInit();
                                    txtBox.Dock = System.Windows.Forms.DockStyle.Top;
                                    txtBox.Location = new System.Drawing.Point(0, 0);
                                    txtBox.Name = "txtBox" + i.ToString() + j.ToString();
                                    txtBox.Tag = deviceType + paramType;
                                    txtBox.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F);
                                    txtBox.Properties.Appearance.Options.UseFont = true;
                                    txtBox.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
                                    txtBox.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
                                    txtBox.Size = new System.Drawing.Size(160, 25);
                                    //edit by rayna 2011-4-28
                                    switch (paramDataType)
                                    {
                                        case AttributeDataType.INTEGER:
                                            txtBox.Properties.Mask.EditMask = "-?\\d+";
                                            break;
                                        case AttributeDataType.FLOAT:
                                            txtBox.Properties.Mask.EditMask = "(-?\\d+)(\\.\\d+)?";
                                            break;
                                        //case AttributeDataType.STRING:
                                        //    break;
                                        default:
                                            txtBox.Properties.Mask.EditMask = ".+";
                                            break;
                                    }
                                    txtBox.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                                    txtBox.Properties.Mask.ShowPlaceHolders = false;
                                    //end
                                    if (dsParam.Tables[0].Rows[i - 1][BASE_PARAMETER_FIELDS.FIELD_DEVICE_TYPE].ToString() != _edcData.DataType
                                        || paramType == "R" || paramType == "C")
                                    {
                                        txtBox.Properties.ReadOnly = true;
                                    }
                                    txtBox.TabIndex = boxCount;
                                    ((ISupportInitialize)(txtBox.Properties)).EndInit();
                                    tablePanel.Controls.Add(txtBox, j, i);
                                    if (dsParam.Tables[0].Rows[i - 1][BASE_PARAMETER_FIELDS.FIELD_DEVICE_TYPE].ToString() == _edcData.DataType &&
                                        dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE].ToString() == "W")
                                    {
                                        txtBox.Properties.ReadOnly =!allowInput;
                                        txtBox.BackColor = Color.White;
                                        _edcData.SampQty = Convert.ToInt32(dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]);
                                        NeedWriteH = i;
                                       
                                        txtBox.TextChanged += (s, e) =>
                                        {
                                            TextEdit txtEdit = s as TextEdit;
                                            if (txtEdit.Text != string.Empty)
                                            {
                                                CheckValidate(txtEdit);
                                                CalculateData(txtEdit);
                                            }
                                        };
                                    }
                                }
                            }
                        }
                    }
                    tablePanel.ResumeLayout(false);
                    tablePanel.PerformLayout();
                    this.pcParams.Controls.Add(tablePanel);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
                return false;
            }
            return true;
        }

        private void CheckValidate(TextEdit txtBox)
        {
            try
            {

                double upperData;
                double lowData;
                int rowIndex = Convert.ToInt32(txtBox.Name.ToString().Substring(6, 1));
                object uppperdataObj = dsParam.Tables[0].Rows[rowIndex - 1][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY];
                if (uppperdataObj == null || uppperdataObj == DBNull.Value)
                {
                    throw new Exception("请配置参数上线值");
                }
                else
                {
                    upperData = Convert.ToDouble(uppperdataObj);
                }
                object lowDataObj = dsParam.Tables[0].Rows[rowIndex - 1][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY];
                if (lowDataObj == null || lowDataObj == DBNull.Value)
                {
                    throw new Exception("请配置参数下线值");
                }
                else
                {
                    lowData = Convert.ToDouble(lowDataObj);
                } 
                if (float.Parse(txtBox.EditValue.ToString()) < lowData || float.Parse(txtBox.EditValue.ToString()) > upperData)
                {
                    txtBox.BackColor = Color.Red;
                }
                else
                {
                    txtBox.BackColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CalculateData(TextEdit textEdit)
        {
            try
            {
                int rowIndex = Convert.ToInt32(textEdit.Name.ToString().Substring(6, 1));
                LabelControl lblCtrl = (LabelControl)tablePanel.Controls.Find("lblParam" + rowIndex.ToString("00"), true)[0];
                if (lblCtrl.Text.IndexOf("刻蚀后方块电阻") != -1)
                {
                    ExcuteCalculate(textEdit, "刻蚀前方块电阻", "方块电阻提升量",false);
                }

                if (lblCtrl.Text.IndexOf("刻蚀前方块电阻") != -1)
                {
                    //ExcuteCalculate(textEdit, "刻蚀前方块电阻", "不均匀度");
                }
                                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ExcuteCalculate(TextEdit textEdit, string preParamName, string calParamName, bool isMinus)
        {
            TextEdit prevBox = null;
            TextEdit calcBox = null;
            LabelControl lblParamCtrl = null;
            int columnIndex = Convert.ToInt32(textEdit.Name.ToString().Substring(7, 1));
            for (int i = 1; i < tablePanel.RowCount; i++)
            {
                lblParamCtrl = (LabelControl)tablePanel.Controls.Find("lblParam" + i.ToString("00"), true)[0];
                if (lblParamCtrl.Text.IndexOf(preParamName) != -1)
                {
                    prevBox = this.tablePanel.Controls.Find("txtBox" + i.ToString() + columnIndex.ToString(), true)[0] as TextEdit;
                }
                if (lblParamCtrl.Text.IndexOf(calParamName) != -1)
                {
                    calcBox = this.tablePanel.Controls.Find("txtBox" + i.ToString() + columnIndex.ToString(), true)[0] as TextEdit;
                }
            }
            if (prevBox.Text != string.Empty && prevBox.Text.Length > 0)
            {
                if (isMinus)
                {
                    //被减数
                    calcBox.Text = (float.Parse(prevBox.Text) - float.Parse(textEdit.Text)).ToString();
                }
                else
                {
                    //减数
                    calcBox.Text = (float.Parse(textEdit.Text) - float.Parse(prevBox.Text)).ToString();
                }
                CheckValidate(calcBox);
            }
        }

        private void ExcuteCalculate(TextEdit textEdit, string preParamName, string calParamName)
        {
            TextEdit prevBox = null;
            TextEdit calcBox = null;
            LabelControl lblParamCtrl = null;
            int columnIndex = Convert.ToInt32(textEdit.Name.ToString().Substring(7, 1));
            for (int i = 1; i < tablePanel.RowCount; i++)
            {
                lblParamCtrl = (LabelControl)tablePanel.Controls.Find("lblParam" + i.ToString("00"), true)[0];
                if (lblParamCtrl.Text.IndexOf(preParamName) != -1)
                {
                    prevBox = this.tablePanel.Controls.Find("txtBox" + i.ToString() + columnIndex.ToString(), true)[0] as TextEdit;
                }
                if (lblParamCtrl.Text.IndexOf(calParamName) != -1)
                {
                    calcBox = this.tablePanel.Controls.Find("txtBox" + i.ToString() + columnIndex.ToString(), true)[0] as TextEdit;
                }
            }
            if (prevBox.Text != string.Empty)
            {
                calcBox.Text = (float.Parse(prevBox.Text.Trim()) - float.Parse(textEdit.Text.Trim())).ToString();
                CheckValidate(calcBox);
            }
        }

        private int GetRecord()
        {
            int count = 0;
            for (int i = 0; i < dsParam.Tables[0].Rows.Count; i++)
            {
                if (dsParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE].ToString() == "W" ||
                    dsParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE].ToString() == "C" ||
                    dsParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE].ToString() == "N")
                {
                    count = count + Convert.ToInt32(dsParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]);
                }
            }
            return count;
        }

        private string CheckUpdateOrInsert(string paramKey, string unitSeq, string value)
        {
            string result = "new";
            for (int i = 0; i < dsCollectionData.Tables[0].Rows.Count; i++)
            {
                if (dsCollectionData.Tables[0].Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString() == paramKey &&
                    dsCollectionData.Tables[0].Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ].ToString() == unitSeq)
                {
                    if (value == dsCollectionData.Tables[0].Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE].ToString())
                    {
                        result = "no";
                    }
                    else
                    {
                        result = "update";
                    }
                    return result;
                }
            }
            return result;
        }

        private void btSubmit_Click(object sender, EventArgs e)
        {
            //if (THreadGet)
            //{
            //    MessageService.ShowMessage("数据接收中......");
            //    return;
            //}

            string isHold = "false";
            DataTable saveTable = _edcData.BuildTable(EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME);
            int saveRowCount = 0;
            for (int i = 1; i < tablePanel.RowCount; i++)
            {
                LabelControl lblCtrl = (LabelControl)tablePanel.Controls.Find("lblParam" + i.ToString("00"), true)[0];
                string paramKey = lblCtrl.Tag.ToString();
                for (int j = 1; j <= Convert.ToInt32(dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]); j++)
                {
                    TextEdit textEdit = tablePanel.Controls.Find("txtBox" + i.ToString() + j.ToString(), true)[0] as TextEdit;
                    if (textEdit.Tag.ToString().Substring(0, 1) == _edcData.DataType)
                    {
                        if (textEdit.Tag.ToString().Substring(1, 1) != "R" && textEdit.Text != string.Empty)
                        {
                            string validFlag = string.Empty;
                            if (textEdit.BackColor == Color.Red)
                            {
                                validFlag = "1";
                                isHold = "true";
                            }
                            else
                            {
                                validFlag = "0";
                            }
                            string result = CheckUpdateOrInsert(paramKey, j.ToString(), textEdit.Text);
                            switch (result)
                            {
                                case "no":
                                    break;
                                case "new":
                                    saveTable.Rows.Add(saveRowCount);
                                    saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_SP_SAMP_SEQ] = "1";
                                    saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ] = j.ToString();
                                    saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY] = paramKey;
                                    saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE] = textEdit.Text;
                                    saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_EDC_INS_KEY] = edcInsKey;
                                    saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_COL_KEY] = string.Empty;
                                    saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_VALID_FLAG] = validFlag;
                                    saveTable.Rows[saveRowCount][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION] = "New";
                                    saveRowCount++;
                                    break;
                                case "update":

                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                }
            }

            LotEDCEntity edcEntity = new LotEDCEntity();
            DataSet dsSave = new DataSet();
            if (saveTable.Rows.Count > 0)
            {
                dsSave.Tables.Add(saveTable);
            }
            string question = string.Empty;
            if (isHold == "false")
            {
                question = "确定要提交数据吗？";
            }
            else
            {
                question = "数据超出范围，批次将被锁定。确定要保存数据吗？";
            }
            if (MessageService.AskQuestionSpecifyNoButton(question))
            {
                this.btSubmit.Enabled = false;
                int RCount = GetRecord();
                edcEntity.Operator = _edcData.StaffNumber;
                edcEntity.SaveEDCPause(dsSave, RCount,lotKey, edcInsKey,isHold);
                if (edcEntity.ErrorMsg == "")
                {
                    BindInitialData();
                    _edcData.UpdateSHRDataToSqlServer(shrTable_Q);
                    MessageService.ShowMessage("保存成功。");                   
                }
                else
                {
                    MessageService.ShowError(edcEntity.ErrorMsg);
                }
            }
            
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);

            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent is EDCMainViewContent)
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
        }

        private void btGetData_Click(object sender, EventArgs e)
        {
            try
            {
                GetData();
            }
            catch (Exception err)
            {
                MessageService.ShowError(err.Message);
            }
        }

        private void btWData_Click(object sender, EventArgs e)
        {
            EdcGatherData edcGatherData = new EdcGatherData();
            edcGatherData.StaffNumber = _edcData.StaffNumber;
            edcGatherData.SerialNumber = _edcData.SerialNumber;
            edcGatherData.DataType = "W";

            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == "称重")
                {
                    viewContent.WorkbenchWindow.CloseWindow(true);
                    break;
                }
            }
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            EDCData04WViewContent view = new EDCData04WViewContent(edcGatherData);
            WorkbenchSingleton.Workbench.ShowView(view);
        }

        private void btDeleteData_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageService.AskQuestion("确定要清空数据吗？"))
                {
                    _edcData.UpdateSHRDataToSqlServer(shrTable_Q);
                    if (_edcData.DeleteRData(edcInsKey))
                    {
                        //delete data
                        for (int i = 1; i < tablePanel.RowCount; i++)
                        {
                            for (int j = 1; j <= Convert.ToInt32(dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]); j++)
                            {
                                TextEdit textEdit = tablePanel.Controls.Find("txtBox" + i.ToString() + j.ToString(), true)[0] as TextEdit;
                                if (textEdit.Tag.ToString().Substring(0, 1) == _edcData.DataType && textEdit.Tag.ToString().Substring(1, 1) != "R")
                                {
                                    textEdit.Text = string.Empty;
                                    textEdit.Properties.ReadOnly = false;
                                }
                            }
                        }
                        MessageService.ShowMessage("清空数据成功");
                        BindInitialData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError("Delete data error:" + ex.Message);
            }
        } 
    }
}
