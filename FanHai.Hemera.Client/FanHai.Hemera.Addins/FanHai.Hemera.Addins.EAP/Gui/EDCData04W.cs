//采集数据保存成功不提示成功
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using DevExpress.XtraEditors;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities.Exceptions;
using System.Threading;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using System.Collections;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.EAP
{
    /// <summary>
    /// 数据采集控件。
    /// </summary>

    public partial class EDCData04W : BaseUserCtrl
    {
        private const int DIGITS = 4;                                               //小点后位数。
        private EdcGatherData _edcData = null;                                      //聚集数据采集数据的对象。
        private DataSet dsParam = new DataSet();                                    //存放数据采集参数的对象。
        private DataSet dsCollectionData = new DataSet();                           //存放数据采集数据的对象。
        bool bIsEDCAllowManualInput = true;                                         //是否允许手工输入设备自动采集的数据。
        SerialPortDataReader balanceReader = null;                                  //天平秤串行端口数据读取类。
        private string edcInsKey = string.Empty;                                    //数据采集主键。
        private string lotKey = string.Empty;                                       //批次主键。
        private string partKey = string.Empty;                                      //成品主键。
        private string materialLot = string.Empty;                                  //原材料批号。
        private string workOrderKey = string.Empty;                                 //工单主键。
        private bool isFirstLoading = false;                                        //是否是第一次载入
        private DateTime dtResistanceData = DateTime.MinValue;                      //方阻数据文件的起始搜索时间。
        private Thread tReadResistanceData = null;                                  //读取方阻数据的线程实例。
        private string lastFileName = string.Empty;                                 //最后读取的方阻文件名称。                     
        private ManualResetEvent mreReadResistanceData = new ManualResetEvent(false);
        private string holdDescription = string.Empty;                              //批次HOLD原因描述。
        private TableLayoutPanel tablePanel = null;
        private ComputerEntity computerEntity = new ComputerEntity();
        /// <summary>
        /// 反射率抽检参数的数量
        /// </summary>
        private int fParamCount = 0;
        /// <summary>
        /// 折射率抽检参数的数量
        /// </summary>
        private int aParamCount = 0;
        /// <summary>
        /// 膜厚抽检参数的数量
        /// </summary>
        private int tParamCount = 0;
        /// <summary>
        /// 方阻抽检参数的数量
        /// </summary>
        private int rParamCount = 0;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="edcData">数据采集的实体类对象。</param>
        /// comment by peter 2012-2-27
        public EDCData04W(EdcGatherData edcData)
        {
            InitializeComponent();
            _edcData = edcData;
            _edcData.Operator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
        }
        /// <summary>
        /// 根据区域化特性初始化UI界面资源。
        /// </summary>
        protected override void InitUIResourcesByCulture()
        {
            base.InitUIResourcesByCulture();
            //this.lblTitle.Text = _edcData.OperationName + "-" + "数据采集";
        }
        /// <summary>
        /// 初始化界面。
        /// </summary>
        /// <returns>是否初始化成功。true：成功。false：失败。</returns>
        /// comment by peter 2012-2-27
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
                tablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
                if (dsParam.Tables.Count > 0 && dsParam.Tables[0].Rows.Count > 0)
                {
                    object maxCount = dsParam.Tables[0].Compute("max(PARAM_COUNT)", "");//获取最大抽检数量。
                    if (maxCount == DBNull.Value)
                    {
                        throw new Exception("请配置需要抽检的电池片数量。");
                    }
                    else
                    {
                        //根据采样参数点个数（行）和每点抽检片数(列）为TableLayoutPanel增加行列。
                        columnCount = Convert.ToInt32(maxCount);
                        rowCount = dsParam.Tables[0].Rows.Count;
                        tablePanel.ColumnCount = columnCount + 1;
                        tablePanel.RowCount = rowCount + 1;
                        //tablePanel.Width = 100 * tablePanel.ColumnCount;
                        tablePanel.AutoSize = true;
                        //为TableLayoutPanel的单元格增加控件。
                        for (int i = 0; i < tablePanel.RowCount; i++)
                        {
                            for (int j = 0; j < tablePanel.ColumnCount; j++)
                            {
                                if (i == 0 && j == 0)//第一行第一列跳过。
                                {
                                    continue;
                                }
                                if (i == 0)//行数为第一行，显示每点采样片数的标题。
                                {
                                    LabelControl lblCtrl = new LabelControl();
                                    lblCtrl.Anchor = AnchorStyles.Top;
                                    lblCtrl.Location = new System.Drawing.Point(0, 0);
                                    lblCtrl.Appearance.Options.UseFont = true;
                                    lblCtrl.LookAndFeel.UseDefaultLookAndFeel = true;
                                    lblCtrl.Dock = DockStyle.Fill;
                                    lblCtrl.TabIndex = 0;
                                    //lblCtrl.Text = j.ToString() + "片";
                                    lblCtrl.Text = "数据" + j.ToString();
                                    lblCtrl.Width = 100;
                                    lblCtrl.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
                                    tablePanel.Controls.Add(lblCtrl, j, i);
                                }
                                else if (j == 0)//如果为第一列，显示电池片采样参数名称。
                                {
                                    LabelControl lblCtrl = new LabelControl();
                                    lblCtrl.Name = "lblParam" + i.ToString("00");
                                    lblCtrl.Anchor = AnchorStyles.Top;
                                    lblCtrl.Location = new System.Drawing.Point(0, 0);
                                    lblCtrl.Appearance.Options.UseFont = true;
                                    lblCtrl.LookAndFeel.UseDefaultLookAndFeel = true;
                                    lblCtrl.TabIndex = 0;
                                    lblCtrl.Dock = DockStyle.Fill;
                                    lblCtrl.Width = 100;
                                    lblCtrl.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
                                    lblCtrl.Tag = dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_KEY].ToString();
                                    lblCtrl.Text = dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_NAME].ToString();
                                    tablePanel.Controls.Add(lblCtrl, j, i);
                                }
                                else
                                {
                                    //根据采样点数生成控件。
                                    if (j < Convert.ToInt32(dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]) + 1)
                                    {
                                        string paramType = Convert.ToString(dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE]);//采样参数类型。
                                        if (string.IsNullOrEmpty(paramType.Trim()))
                                        {
                                            throw new Exception("参数类型不能为空，请检查抽检规则是否配置参数类型");
                                        }
                                        string deviceType = dsParam.Tables[0].Rows[i - 1][BASE_PARAMETER_FIELDS.FIELD_DEVICE_TYPE].ToString();//设备类型。
                                        string dataType = dsParam.Tables[0].Rows[i - 1][BASE_PARAMETER_FIELDS.FIELD_DATA_TYPE].ToString();    //数据类型
                                        string isDerived = dsParam.Tables[0].Rows[i - 1][BASE_PARAMETER_FIELDS.FIELD_ISDERIVED].ToString();   //是否运算
                                        string calculateType = dsParam.Tables[0].Rows[i - 1][BASE_PARAMETER_FIELDS.FIELD_CALCULATE_TYPE].ToString();   //是否运算
                                        AttributeDataType paramDataType = AttributeDataType.FLOAT;
                                        if (!string.IsNullOrEmpty(dataType))
                                        {
                                            paramDataType = (AttributeDataType)Convert.ToInt32(dataType);
                                        }
                                        TextEdit txtBox = new TextEdit();
                                        boxCount++;
                                        ((ISupportInitialize)(txtBox.Properties)).BeginInit();
                                        txtBox.Dock = System.Windows.Forms.DockStyle.Top;
                                        txtBox.Width = 100;
                                        txtBox.Location = new System.Drawing.Point(0, 0);
                                        txtBox.Name = "txtBox" + i.ToString("00") + j.ToString("00");
                                        txtBox.Dock = DockStyle.Fill;
                                        txtBox.Tag = deviceType + paramType; //设备类型+参数类型+是否运算
                                        txtBox.Properties.LookAndFeel.UseDefaultLookAndFeel = true;
                                        txtBox.Properties.MaxLength = 10;
                                        txtBox.KeyPress += new KeyPressEventHandler(txtBox_KeyPress);
                                        switch (paramDataType)//根据参数数据类型，设置文本框可以输入的值。
                                        {
                                            case AttributeDataType.INTEGER:
                                                txtBox.Properties.Mask.EditMask = "-?\\d{0,10}";
                                                txtBox.ImeMode = ImeMode.Off;
                                                break;
                                            case AttributeDataType.FLOAT:
                                                txtBox.Properties.Mask.EditMask = "(-?\\d{0,10})(\\.\\d{0," + DIGITS.ToString() + "})?";
                                                txtBox.ImeMode = ImeMode.Off;
                                                break;
                                            default:
                                                txtBox.Properties.Mask.EditMask = ".{0,10}";
                                                break;
                                        }
                                        txtBox.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                                        txtBox.ImeMode = ImeMode.Off;
                                        txtBox.Properties.Mask.ShowPlaceHolders = false;
                                        //如果参数类型是只读参数或计算的参数，文本框设置为只读类型。
                                        if (paramType == "R" || paramType == "C")
                                        {
                                            txtBox.Properties.ReadOnly = true;
                                        }
                                        txtBox.TabIndex = boxCount;
                                        ((ISupportInitialize)(txtBox.Properties)).EndInit();
                                        tablePanel.Controls.Add(txtBox, j, i);
                                        //如果参数类型正常或是可写。。
                                        if (paramType == "N" || paramType == "W")
                                        {
                                            txtBox.BackColor = Color.White;
                                            //当文本框中数据改变时触发。
                                            txtBox.TextChanged += new EventHandler(TextEditChanged);
                                        }
                                        //如果参数类型是可写。
                                        if (paramType == "W")
                                        {
                                            txtBox.Properties.ReadOnly = !bIsEDCAllowManualInput;
                                            if (deviceType == "F") { fParamCount = fParamCount + 1; }
                                            if (deviceType == "A") { aParamCount = aParamCount + 1; }
                                            if (deviceType == "T") { tParamCount = tParamCount + 1; }
                                            if (deviceType == "R") { rParamCount = rParamCount + 1; }

                                            #region 设备类型为称重设备
                                            //如果是称重设备。
                                            if (deviceType == "W")
                                            {
                                                balanceReader = WeightDataReadHelper.BalanceDataReader;
                                                //当文本框单击时触发的事件方法。
                                                txtBox.Click += (s, e) =>
                                                {
                                                    //已保存过的称重数据不能再获取新值
                                                    if (txtBox.Properties.Tag != null && txtBox.Properties.Tag.ToString() == "FALSE")
                                                    {
                                                        return;
                                                    }
                                                    txtBox.SelectAll();
                                                    try
                                                    {
                                                        this.Cursor = Cursors.WaitCursor;

                                                        string data = balanceReader.StartRead();
                                                        if (balanceReader.IsReadDataSuccess)
                                                        {
                                                            double val = 0;
                                                            double.TryParse(data, out val);
                                                            val = Math.Round(val, DIGITS, MidpointRounding.AwayFromZero);
                                                            txtBox.Text = val.ToString();
                                                            LoggingService.Info(data);
                                                        }
                                                        else
                                                        {
                                                            LoggingService.Error(balanceReader.ErrorMessage);
                                                            MessageService.ShowError("天平秤数据格式不正确。");
                                                        }
                                                    }
                                                    catch (Exception ex)//程序执行出错，给出错误提示。
                                                    {
                                                        LoggingService.Error(ex.Message, ex);
                                                        //MessageService.ShowError(ex.Message);
                                                    }
                                                    this.Cursor = Cursors.Default;
                                                };
                                            }
                                            #endregion
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
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 文本框回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.tablePanel.SelectNextControl(ActiveControl, true, false, true, true);
            }
        }
        /// <summary>
        /// 绑定初始化的数据到窗体控件中。
        /// </summary>
        /// comment by peter 2012-2-27
        private void BindInitialData()
        {
            LotEDCEntity lotEntity = new LotEDCEntity();
            //获取批次已采集的数据。
            dsCollectionData = lotEntity.GetEDCCollectionData(lotKey, edcInsKey);
            if (lotEntity.ErrorMsg == string.Empty)//获取成功。
            {
                if (dsCollectionData.Tables.Count > 0 && dsCollectionData.Tables[0].Rows.Count > 0)
                {
                    //遍历已采集的数据
                    for (int i = 0; i < dsCollectionData.Tables[0].Rows.Count; i++)
                    {
                        //遍历TableLayoutPnael的行
                        for (int j = 1; j < tablePanel.RowCount; j++)
                        {
                            string strName = "lblParam" + j.ToString("00");
                            LabelControl lblCtrl = (LabelControl)tablePanel.Controls.Find(strName, true)[0];
                            if (lblCtrl.Tag.ToString() == dsCollectionData.Tables[0].Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString())
                            {
                                //j为tablePanel的行号。设置值。
                                int column = Convert.ToInt32(dsCollectionData.Tables[0].Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ]);
                                TextEdit textEdit = (TextEdit)tablePanel.Controls.Find("txtBox" + j.ToString("00") + column.ToString("00"), true)[0];
                                textEdit.Text = dsCollectionData.Tables[0].Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE].ToString();
                                //如果是参数类型是"W:可写"或者"N:正常",则不能再输入值。
                                if ((textEdit.Tag.ToString().Substring(1, 1) == "W" || textEdit.Tag.ToString().Substring(1, 1) == "N"))
                                {
                                    textEdit.Properties.ReadOnly = true;
                                    textEdit.Properties.Tag = "FALSE";
                                }
                                break;
                            }
                        }
                    }
                    ////检查是否完成了数据采集，如果是则禁用按钮。
                    //if (CheckFinish())
                    //{
                    //    btSubmit.Enabled = false;
                    //}
                    //else//否则不禁用，按钮。
                    //{
                    //    btSubmit.Enabled = true;
                    //}
                }
            }
            else//获取批次已采集的数据的失败，给出错误提示。
            {
                MessageService.ShowError(lotEntity.ErrorMsg);
            }

        }
        /// <summary>
        /// 检查是否已经完成了数据采集。
        /// </summary>
        /// <returns>true：已经完成数据采集。false：没有完成数据采集。</returns>
        /// comment by peter 2012-2-27
        private bool CheckFinish()
        {
            bool bResult = true;
            //遍历TableLayout的每行。
            for (int i = 1; i < tablePanel.RowCount; i++)
            {
                //遍历每列。
                for (int j = 1; j <= Convert.ToInt32(dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]); j++)
                {
                    //如果参数类型是W（可写）或N（正常）的类型 并且文本框的值为空，则没有完成数据采集。
                    TextEdit textEdit = (TextEdit)tablePanel.Controls.Find("txtBox" + i.ToString("00") + j.ToString("00"), true)[0];
                    if ((textEdit.Tag.ToString().Substring(1, 1) == "W" || textEdit.Tag.ToString().Substring(1, 1) == "N")
                        && textEdit.Text == string.Empty)
                    {
                        bResult = false;
                        return bResult;
                    }
                }
            }
            //数据采集已经完成。
            return bResult;
        }
        /// <summary>
        /// 数据采集文本框改变时触发的事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextEditChanged(object sender, EventArgs e)
        {
            TextEdit txtEdit = sender as TextEdit;
            txtEdit.TextChanged -= new EventHandler(TextEditChanged);
            try
            {
                if (!isFirstLoading)
                {
                    CalculateData(txtEdit);
                }
                CheckValidate(txtEdit);
            }
            finally
            {
                txtEdit.TextChanged += new EventHandler(TextEditChanged);
            }
        }
        /// <summary>
        /// 验证文本框输入的值是否符合。
        /// </summary>
        /// <param name="txtBox">文本框。</param>
        /// comment by peter 2012-2-27
        private void CheckValidate(TextEdit txtBox)
        {
            if (txtBox == null) return;
            if (string.IsNullOrEmpty(txtBox.Text.Trim()))
            {
                txtBox.BackColor = txtBox.Properties.ReadOnly ? Color.LightGray : Color.White;
                return;
            }
            string curDataVal = Convert.ToString(txtBox.EditValue);

            //根据文本框名称获取参数在参数基本信息列表中的索引值。
            int rowIndex = Convert.ToInt32(txtBox.Name.ToString().Substring(6, 2));
            string paramName = Convert.ToString(dsParam.Tables[0].Rows[rowIndex - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_NAME]);
            int paramCount = Convert.ToInt32(dsParam.Tables[0].Rows[rowIndex - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]);
            double curData = 0;
            if (!double.TryParse(curDataVal, out curData))
            {
                curData = 0;
            }
            //允许输入的最小值和允许输入的最大值
            double allowMinValue = double.MinValue;
            double allowMaxValue = double.MaxValue;
            object allowMinValueObj = dsParam.Tables[0].Rows[rowIndex - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_ALLOW_MIN_VALUE];   //允许输入的最小值
            object allowMaxValueObj = dsParam.Tables[0].Rows[rowIndex - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_ALLOW_MAX_VALUE];   //允许输入的最大值
            if (allowMinValueObj != null && allowMinValueObj != DBNull.Value)
            {
                allowMinValue = Convert.ToDouble(allowMinValueObj);
            }
            if (allowMaxValueObj != null && allowMaxValueObj != DBNull.Value)
            {
                allowMaxValue = Convert.ToDouble(allowMaxValueObj);
            }
            //是否满足上下允许输入的最大值。
            if (curData < allowMinValue || curData > allowMaxValue)
            {
                txtBox.BackColor = Color.Yellow;//不满足使用黄色标识出来。
                txtBox.ToolTip = string.Format("({0})数据值超过允许输入的值。", paramName);
                return; //不需要继续判断。
            }
            //数据单值是否超过规格线
            double upperData = double.MaxValue;
            double lowData = double.MinValue;
            //获取上限规格值。
            object uppperdataObj = dsParam.Tables[0].Rows[rowIndex - 1][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY];
            if (uppperdataObj != null && uppperdataObj != DBNull.Value)
            {
                upperData = Convert.ToDouble(uppperdataObj);
            }
            //获取下限规格值。
            object lowDataObj = dsParam.Tables[0].Rows[rowIndex - 1][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY];
            if (lowDataObj != null && lowDataObj != DBNull.Value)
            {
                lowData = Convert.ToDouble(lowDataObj);
            }
            //是否满足上下限规格值。
            if (curData < lowData || curData > upperData)
            {
                txtBox.BackColor = Color.Red;//不满足使用红色标识出来。
                txtBox.ToolTip = string.Format("({0})数据单值超过规格线。", paramName);
                return; //不需要继续判断。
            }
            else
            {
                txtBox.ToolTip = string.Empty;
                txtBox.BackColor = txtBox.Properties.ReadOnly ? Color.LightGray : Color.White;
            }
            //判断所有值的均值是否超过控制线。
            int valCount = 0;     //输入值的个数。
            double valSum = 0f;   //输入值的加总。
            double valAvg = 0f;   //输入值的平均值。
            double upperControl = double.MaxValue;
            double lowControl = double.MinValue;
            //获取上限控制值。
            object upperControlObj = dsParam.Tables[0].Rows[rowIndex - 1][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_SPEC];
            if (upperControlObj != null && upperControlObj != DBNull.Value)
            {
                upperControl = Convert.ToDouble(upperControlObj);
            }
            //获取下限控制值。
            object lowControlObj = dsParam.Tables[0].Rows[rowIndex - 1][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_SPEC];
            if (lowControlObj != null && lowControlObj != DBNull.Value)
            {
                lowControl = Convert.ToDouble(lowControlObj);
            }
            //加总输入值 
            for (int i = 1; i <= paramCount; i++)
            {
                TextEdit txtValueControl = null;
                string name = "txtBox" + rowIndex.ToString("00") + i.ToString("00");
                if (name == txtBox.Name)
                {
                    txtValueControl = txtBox;
                }
                else
                {
                    Control[] ctrls = this.tablePanel.Controls.Find(name, true);
                    if (ctrls.Length > 0)
                    {
                        txtValueControl = ctrls[0] as TextEdit;
                    }
                }
                //如果没有文本编辑框。
                if (txtValueControl == null) continue;

                string valObj = Convert.ToString(txtValueControl.EditValue);
                double val = 0;
                if (!string.IsNullOrEmpty(valObj) && double.TryParse(valObj, out val))
                {
                    valCount = valCount + 1;
                    valSum = valSum + val;
                }
            }
            if (valCount > 0)
            {
                valAvg = Math.Round(valSum / valCount, DIGITS, MidpointRounding.AwayFromZero);
                //是否满足上下限控制值。
                if (valAvg < lowControl || valAvg > upperControl)
                {
                    for (int i = 1; i <= paramCount; i++)
                    {
                        TextEdit txtValueControl = null;
                        string name = "txtBox" + rowIndex.ToString("00") + i.ToString("00");
                        if (name == txtBox.Name)
                        {
                            txtValueControl = txtBox;
                        }
                        else
                        {
                            Control[] ctrls = this.tablePanel.Controls.Find(name, true);
                            if (ctrls.Length > 0)
                            {
                                txtValueControl = ctrls[0] as TextEdit;
                            }
                        }

                        txtValueControl.BackColor = Color.Red;//不满足使用红色标识出来。
                        txtValueControl.ToolTip = string.Format("({0})该组数据均值超过控制线。", paramName);
                    }
                }
                else
                {
                    for (int i = 1; i <= paramCount; i++)
                    {
                        TextEdit txtValueControl = null;
                        string name = "txtBox" + rowIndex.ToString("00") + i.ToString("00");
                        if (name == txtBox.Name)
                        {
                            txtValueControl = txtBox;
                        }
                        else
                        {
                            Control[] ctrls = this.tablePanel.Controls.Find(name, true);
                            if (ctrls.Length > 0)
                            {
                                txtValueControl = ctrls[0] as TextEdit;
                            }
                        }
                        txtValueControl.ToolTip = string.Empty;
                        txtValueControl.BackColor = txtBox.Properties.ReadOnly ? Color.LightGray : Color.White;
                    }

                }
            }
        }
        /// <summary>
        /// 根据采集的数据计算得到数据。
        /// </summary>
        /// <param name="textEdit">文本控件。</param>
        /// comment by peter 2012-2-27
        private void CalculateData(TextEdit textEdit)
        {
            try
            {
                int rowIndex = Convert.ToInt32(textEdit.Name.ToString().Substring(6, 2));               //行号
                int columnIndex = Convert.ToInt32(textEdit.Name.ToString().Substring(8, 2));            //列号
                double val = 0;

                string calcFormula = Convert.ToString(dsParam.Tables[0].Rows[rowIndex - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_FORMULA]);
                if (!string.IsNullOrEmpty(textEdit.Text) && !string.IsNullOrEmpty(calcFormula))
                {
                    double.TryParse(textEdit.Text, out val);
                    calcFormula = string.Format(calcFormula, val);
                    Calc calc = new Calc();
                    val = calc.CalcMath(calcFormula);
                    val = Math.Round(val, DIGITS, MidpointRounding.AwayFromZero);
                    textEdit.Text = val.ToString();
                }

                //遍历采样参数，对参数类型=C：计算类型的进行计算。
                for (int i = 1; i < tablePanel.RowCount; i++)
                {
                    TextEdit calcBox = null;
                    LabelControl lblCalcCtrl = null;
                    Control[] ctrls = this.tablePanel.Controls.Find("txtBox" + i.ToString("00") + columnIndex.ToString("00"), true);
                    if (ctrls.Length > 0)
                    {
                        calcBox = ctrls[0] as TextEdit;
                    }
                    //如果没有文本编辑框。
                    if (calcBox == null) continue;

                    string paramType = calcBox.Tag.ToString().Substring(1, 1);
                    #region 如果是参数类型=C：计算类型。
                    if (paramType == "C")//如果是参数类型=C：计算类型。
                    {
                        //获取参数主键。
                        ctrls = tablePanel.Controls.Find("lblParam" + i.ToString("00"), true);
                        if (ctrls.Length > 0)
                        {
                            lblCalcCtrl = ctrls[0] as LabelControl;
                        }
                        //如果没有获取到控件
                        if (lblCalcCtrl == null) continue;
                        string paramKey = lblCalcCtrl.Tag.ToString();
                        //通过参数主键获取是否可以计算 、计算类型和计算参数主键。
                        string isDerive = Convert.ToString(dsParam.Tables[0].Rows[i - 1][BASE_PARAMETER_FIELDS.FIELD_ISDERIVED]);
                        string calcType = Convert.ToString(dsParam.Tables[0].Rows[i - 1][BASE_PARAMETER_FIELDS.FIELD_CALCULATE_TYPE]);
                        //计算类型=1 并且计算类型不为空。
                        if (isDerive == "1" && !string.IsNullOrEmpty(calcType))
                        {
                            //根据计算参数获取参与计算的参数主键。
                            DataSet dsDerivationParams = _edcData.GetParamDerivationByKey(paramKey);
                            if (string.IsNullOrEmpty(_edcData.ErrorMsg)
                                && dsDerivationParams.Tables.Count > 0
                                && dsDerivationParams.Tables[0].Rows.Count > 0)
                            {
                                List<double> lstValue = new List<double>();
                                TextEdit valueBox = null;
                                LabelControl lblValueCtrl = null;
                                //遍历采集数据，获取计算参数主键对应的数值。
                                for (int j = 1; j < tablePanel.RowCount; j++)
                                {
                                    if (i == j) continue;
                                    //获取参数主键。
                                    ctrls = tablePanel.Controls.Find("lblParam" + j.ToString("00"), true);
                                    if (ctrls.Length > 0)
                                    {
                                        lblValueCtrl = ctrls[0] as LabelControl;
                                    }
                                    //如果没有获取到控件
                                    if (lblValueCtrl == null) continue;
                                    string valueParamKey = lblValueCtrl.Tag.ToString();
                                    DataRow[] drs = dsDerivationParams.Tables[0].Select("PARAM_KEY='" + valueParamKey + "'");
                                    if (drs.Length <= 0) continue;

                                    ctrls = this.tablePanel.Controls.Find("txtBox" + j.ToString("00") + columnIndex.ToString("00"), true);
                                    if (ctrls.Length > 0)
                                    {
                                        valueBox = ctrls[0] as TextEdit;
                                    }
                                    //如果没有获取到控件
                                    if (valueBox == null) continue;
                                    //设置参与计算的参数值。
                                    string paramValue = valueBox.Text.Trim();
                                    if (!string.IsNullOrEmpty(paramValue))
                                    {
                                        //如果计算文本框等于触发事件的文本框 并且计算公式不为空，使用计算后的结果。
                                        if (valueBox == textEdit && !string.IsNullOrEmpty(calcFormula))
                                        {
                                            lstValue.Add(val);
                                        }
                                        else
                                        {
                                            lstValue.Add(double.Parse(paramValue));
                                        }
                                    }
                                    else
                                    {
                                        lstValue.Clear();
                                        break;
                                    }
                                }
                                if (lstValue.Count > 0)
                                {
                                    double resultValue = 0;
                                    //根据计算类型，进行计算。如果有一个参数主键没有值则默认为0。
                                    if (calcType == "-")//减薄量计算
                                    {
                                        foreach (double tmp in lstValue)
                                        {
                                            resultValue = tmp - resultValue;
                                        }
                                    }
                                    else if (calcType == "/")//均匀度计算
                                    {
                                        resultValue = lstValue.Sum() / lstValue.Count;
                                    }
                                    resultValue = Math.Abs(resultValue);
                                    resultValue = Math.Round(resultValue, DIGITS, MidpointRounding.AwayFromZero);
                                    //设置calcBox的值=计算结果。
                                    calcBox.Text = resultValue.ToString();
                                }
                                else
                                {
                                    calcBox.Text = string.Empty;
                                }
                            }
                            else
                            {
                                MessageBox.Show(_edcData.ErrorMsg);
                            }
                        }
                    }
                    #endregion
                    CheckValidate(calcBox);
                }
            }
            catch (Exception ex)//程序出错，抛出异常。
            {
                MessageService.ShowMessage(ex.Message);
            }
        }
        /// <summary>
        /// 检查是更新数据采集记录还是新增数据采集记录。
        /// </summary>
        /// <param name="paramKey">数据采集参数主键。</param>
        /// <param name="unitSeq">单位序号。</param>
        /// <param name="value">参数值。</param>
        /// <returns>
        /// new：新增；update：更新；no：不做任何操作。
        /// </returns>
        /// comment by peter 2012-2-27
        private string CheckUpdateOrInsert(string paramKey, string unitSeq, string value)
        {
            string result = "new";
            if (dsCollectionData == null || dsCollectionData.Tables.Count <= 0)
            {
                return result;
            }
            //遍历获取的已采集的数据记录数。
            for (int i = 0; i < dsCollectionData.Tables[0].Rows.Count; i++)
            {
                //如果参数主键=指定的参数主键，单位序号=指定的单位序号
                if (dsCollectionData.Tables[0].Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString() == paramKey &&
                    dsCollectionData.Tables[0].Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ].ToString() == unitSeq)
                {
                    //如果值=指定的值，不做任何操作。
                    if (value == dsCollectionData.Tables[0].Rows[i][EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE].ToString())
                    {
                        result = "no";
                    }
                    else//更新采集的值。
                    {
                        result = "update";
                    }
                    return result;
                }
            }
            return result;
        }
        /// <summary>
        /// 保存按钮Click事件方法。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        /// comment by peter 2012-2-27
        private void btSubmit_Click(object sender, EventArgs e)
        {
            //保存前先结束方阻数据自动获取的线程。
            if (tReadResistanceData != null)
            {
                this.chkIsAutoGetData.Checked = false;
                if (!mreReadResistanceData.WaitOne(1000))
                {
                    tReadResistanceData.Abort();
                }
                tReadResistanceData = null;
            }
            bool isHold = false;
            DataTable saveTable = _edcData.BuildTable(EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME);
            int saveRowCount = 0;
            int paramCount = 0;
            //遍历TableLayoutPanel的行数。
            for (int i = 1; i < tablePanel.RowCount; i++)
            {
                LabelControl lblCtrl = (LabelControl)tablePanel.Controls.Find("lblParam" + i.ToString("00"), true)[0];
                string paramKey = lblCtrl.Tag.ToString();
                int maxParamCount = Convert.ToInt32(dsParam.Tables[0].Rows[i - 1][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]);
                bool bParamInputValue = false;          //指定参数是否有输入一个值。
                bool bParamInputAllValue = true;        //指定参数是否输入了全部的值。
                TextEdit teParamNoInputValue = null;    //没有输入值的文本框。
                //遍历列
                for (int j = 1; j <= maxParamCount; j++)
                {
                    paramCount++;
                    TextEdit textEdit = tablePanel.Controls.Find("txtBox" + i.ToString("00") + j.ToString("00"), true)[0] as TextEdit;
                    string value = Convert.ToString(textEdit.EditValue);
                    //如果文本框的值为空字符串。
                    if (!string.IsNullOrEmpty(value))
                    {
                        bParamInputValue = true;
                        string validFlag = string.Empty;
                        //如果背景颜色为红色，文本框中的值没有通过验证。
                        if (textEdit.BackColor == Color.Red)
                        {
                            string toolTip = textEdit.ToolTip;
                            if (holdDescription.IndexOf(toolTip) < 0)
                            {
                                holdDescription += toolTip;
                            }
                            validFlag = "1";
                            isHold = true;
                        }
                        //数值超过允许输入的最大值和最小值。
                        else if (textEdit.BackColor == Color.Yellow)
                        {
                            MessageBox.Show("不允许保存：" + textEdit.ToolTip);
                            this.chkIsAutoGetData.Checked = true;
                            textEdit.Focus();
                            return;
                        }
                        else
                        {
                            validFlag = "0";
                        }
                        //检查是更新数据，还是新增数据，还是不做任何操作。
                        string result = CheckUpdateOrInsert(paramKey, j.ToString(), value);
                        switch (result)
                        {
                            case "no":
                                break;
                            case "new":
                                saveTable.Rows.Add(saveRowCount);
                                saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_SP_SAMP_SEQ] = "1";
                                saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ] = j.ToString();
                                saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY] = paramKey;
                                saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE] = value;
                                saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_EDC_INS_KEY] = edcInsKey;
                                saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_COL_KEY] = string.Empty;
                                saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_VALID_FLAG] = validFlag;
                                saveTable.Rows[saveRowCount][COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION] = Convert.ToString((int)OperationAction.New);
                                saveTable.Rows[saveRowCount][EDC_COLLECTION_DATA_FIELDS.FIELD_EDITOR] = _edcData.Operator;
                                saveRowCount++;
                                break;
                            case "update":
                                break;
                            default:
                                break;
                        }
                    }
                    else//如果有一个值没有输入
                    {
                        bParamInputAllValue = false;
                        if (teParamNoInputValue == null)
                        {
                            teParamNoInputValue = textEdit;
                        }
                    }
                }
                //如果对应参数有输入值，但对应参数需要输入的值没有全部输入，则提示错误。
                if (bParamInputValue && !bParamInputAllValue)
                {
                    MessageBox.Show(string.Format("参数【{0}】需要输入({1})个数据值之后才能保存。", lblCtrl.Text, maxParamCount), "提示");
                    if (teParamNoInputValue != null)
                    {
                        teParamNoInputValue.Select();
                    }
                    this.chkIsAutoGetData.Checked = true;
                    return;
                }
            }


            DataSet dsSave = new DataSet();
            if (saveTable.Rows.Count > 0 || paramCount == 0)
            {
                dsSave.Tables.Add(saveTable);
            }
            else
            {
                MessageService.ShowMessage("数据没有变动不需要进行保存", "提示");
                this.chkIsAutoGetData.Checked = true;
                return;
            }
            LotEDCEntity edcEntity = new LotEDCEntity();
            string question = string.Empty;
            if (isHold == false)//数据值没有超过上下限值。
            {
                question = "确定要提交数据吗？";
            }
            else//数据值超过上下限值。
            {
                question = string.Format("{0}批次将被锁定。确定要保存数据吗？", holdDescription.Replace("。", "。\n"));
            }

            if (MessageService.AskQuestion(question, "提示"))
            {
                ExcuteSaveEDCData(edcEntity, dsSave, isHold);
            }
            else
            {
                this.chkIsAutoGetData.Checked = true;
            }
        }
        /// <summary>
        /// 关闭按钮的Click事件方法。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        /// comment by peter 2012-2-27
        private void btClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
            //关闭当前视图。
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            //遍历工作台中的视图。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (_edcData.EDCActionName == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_NONE)//离线数据采集。
                {
                    //如果视图是数据采集的主视图类。则选中该视图并显示视图界面。
                    if (viewContent is EDCMainViewContent && string.IsNullOrEmpty(_edcData.EDCMainInsKey))
                    {
                        viewContent.WorkbenchWindow.SelectWindow();
                        return;
                    }
                    //如果视图是数据采集查询的主视图类。则选中该视图并显示视图界面。
                    else if (viewContent is EDCQueryViewContent && !string.IsNullOrEmpty(_edcData.EDCMainInsKey))
                    {
                        viewContent.WorkbenchWindow.SelectWindow();
                        return;
                    }
                }
                else //在线数据采集。
                {
                    if (viewContent.TitleName == "过站作业")
                    {
                        viewContent.WorkbenchWindow.SelectWindow();
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 保存采集的数据。
        /// </summary>
        /// <param name="edcEntity">数据采集的实体类对象。</param>
        /// <param name="dsSave">待保存的采集得到的数据。</param>
        /// <param name="isHold">是否锁定批次。true：锁定，false：不锁定。</param>
        /// comment by peter 2012-2-27
        private void ExcuteSaveEDCData(LotEDCEntity edcEntity, DataSet dsSave, bool isHold)
        {
            this.btSubmit.Enabled = false;
            int count = GetRecord();
            edcEntity.Operator = _edcData.Operator;
            if (_edcData.EDCActionName == "NONE")//离线采集
            {
                //如果没有传入数据实例主键，则新增一个数据采集实例。
                if (string.IsNullOrEmpty(_edcData.EDCMainInsKey))
                {
                    Hashtable mainTable = new Hashtable();
                    EDC_MAIN_INS_FIELDS edcMainField = new EDC_MAIN_INS_FIELDS();
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_INS_KEY, edcInsKey);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_KEY, _edcData.EDCKey);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_SP_KEY, _edcData.EDCSPKey);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKey);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_LOT_KEY, lotKey);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_LOT_NUMBER, _edcData.LotNumber);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_PART_KEY, partKey);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_POINT_KEY, _edcData.EDCPointKey);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EQUIPMENT_KEY, _edcData.EquipmentKey);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_STEP_NAME, _edcData.OperationName);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_PART_NO, _edcData.PartNumber);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_SUPPLIER, _edcData.SupplierName);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_MATERIAL_LOT, materialLot);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_LOCATION_KEY, _edcData.FactoryRoomKey);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_NAME, _edcData.EDCName);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_PART_TYPE, _edcData.PartType);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_CREATOR, _edcData.Operator);
                    mainTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDITOR, _edcData.Operator);

                    DataTable dt = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainTable);
                    dt.TableName = EDC_MAIN_INS_FIELDS.DATABASE_TABLE_NAME;
                    dsSave.Tables.Add(dt);
                }
                edcEntity.SaveOfflineEDCData(dsSave);
            }
            else
            {
                if (holdDescription.Length > 255)
                {
                    holdDescription = holdDescription.Substring(0, 255);
                }
                //保存在线采集到的数据。
                edcEntity.SaveOnlineEDCData(dsSave, count, lotKey, edcInsKey, isHold, holdDescription);
            }
            if (edcEntity.ErrorMsg == "")//保存采集得到的数据。
            {
                BindInitialData();

                #region SPC 保存数据
                dsSave.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.CREATOR, _edcData.Operator);
                dsSave.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.ORDER_NUMBER, _edcData.OrderNumber);
                dsSave.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.SHIFT_VALUE, _edcData.ShiftName);
                dsSave.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.EDC_INS_KEY, edcInsKey);
                SpcEntity spcEntiry = new SpcEntity();
                spcEntiry.SaveParamData(dsSave);
                #endregion

                //MessageService.ShowMessage("保存成功。","提示"); //Q.001  注释掉，不让提示保存成功
                //保存成功关闭窗口。
                btClose_Click(btSubmit, null);
            }
            else
            {
                MessageService.ShowError(edcEntity.ErrorMsg);
                this.btSubmit.Enabled = true;
            }
        }
        /// <summary>
        /// 获取采集数据的个数。
        /// </summary>
        /// <returns>采集数据的个数。</returns>
        private int GetRecord()
        {
            int count = 0;
            //遍历采集参数的行数。
            for (int i = 0; i < dsParam.Tables[0].Rows.Count; i++)
            {
                //如果参数类型是W（称重参数）,C（计算的参数）,N（折射率），加总。
                if (dsParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE].ToString() == "W" ||
                    dsParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE].ToString() == "C" ||
                    dsParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE].ToString() == "N")
                {
                    count = count + Convert.ToInt32(dsParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]);
                }
            }
            return count;
        }
        /// <summary>
        /// 获取反射率数据。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetRflectance_Click(object sender, EventArgs e)
        {
            DataFileReadConfig config = DataFileReadConfigReader.GetRflectanceConfig();
            config.PointCount = fParamCount;
            if (string.IsNullOrEmpty(config.FileName))
            {
                MessageService.ShowMessage("获取反射率数据失败，没有找到反射率数据文件。", "提示");
                return;
            }
            DataFileReader reader = new DataFileReader();
            IList<DataFileReadValue> lst = reader.Reader(config);
            if (lst.Count == 0)
            {
                MessageService.ShowMessage("没有获取到反射率数据。", "提示");
                return;
            }
            int i = 0;
            //遍历TableLayoutPnael的行
            for (int j = 1; j < tablePanel.RowCount; j++)
            {
                //遍历的列
                for (int column = 1; column < tablePanel.ColumnCount; column++)
                {
                    //j为tablePanel的行号。设置值。
                    string txtName = string.Format("txtBox{0}{1}", j.ToString("00"), column.ToString("00"));
                    Control[] controls = tablePanel.Controls.Find(txtName, true);
                    TextEdit textEdit = controls.Length == 0 ? null : controls[0] as TextEdit;
                    string deviceType = textEdit == null ? string.Empty : textEdit.Tag.ToString().Substring(0, 1);
                    if (deviceType == "F")//设备类型是反射率设备且数据为空。
                    {
                        if (textEdit.Text.Trim() == string.Empty)
                        {
                            double val = 0;
                            double.TryParse(lst[i].Value, out val);
                            val = Math.Round(val, DIGITS, MidpointRounding.AwayFromZero);
                            textEdit.Text = val.ToString();
                            i++;
                        }
                        if (i >= lst.Count)
                        {
                            return;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取折射率数据。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetRefraction_Click(object sender, EventArgs e)
        {
            DataFileReadConfig config = DataFileReadConfigReader.GetRefractionConfig();
            config.PointCount = aParamCount;
            if (string.IsNullOrEmpty(config.FileName))
            {
                MessageService.ShowMessage("获取折射率数据失败，没有找到折射率数据文件。", "提示");
                return;
            }
            DataFileReader reader = new DataFileReader();
            IList<DataFileReadValue> lst = reader.Reader(config);
            if (lst.Count == 0)
            {
                MessageService.ShowMessage("没有获取到折射率数据。", "提示");
                return;
            }
            int i = 0;
            //遍历TableLayoutPnael的行
            for (int j = 1; j < tablePanel.RowCount; j++)
            {
                //遍历的列
                for (int column = 1; column < tablePanel.ColumnCount; column++)
                {
                    //j为tablePanel的行号。设置值。
                    string txtName = string.Format("txtBox{0}{1}", j.ToString("00"), column.ToString("00"));
                    Control[] controls = tablePanel.Controls.Find(txtName, true);
                    TextEdit textEdit = controls.Length == 0 ? null : controls[0] as TextEdit;
                    string deviceType = textEdit == null ? string.Empty : textEdit.Tag.ToString().Substring(0, 1);
                    if (deviceType == "A")//设备类型是折射率设备且数据为空。
                    {
                        if (textEdit.Text.Trim() == string.Empty)
                        {
                            double val = 0;
                            double.TryParse(lst[i].Value, out val);
                            val = Math.Round(val, DIGITS, MidpointRounding.AwayFromZero);
                            textEdit.Text = val.ToString();
                            i++;
                        }
                        if (i >= lst.Count)
                        {
                            return;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取膜厚数据。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetThickness_Click(object sender, EventArgs e)
        {
            DataFileReadConfig config = DataFileReadConfigReader.GetThicknessConfig();
            config.PointCount = tParamCount;
            if (string.IsNullOrEmpty(config.FileName))
            {
                MessageService.ShowMessage("获取膜厚数据失败，没有找到膜厚数据文件。", "提示");
                return;
            }
            DataFileReader reader = new DataFileReader();
            IList<DataFileReadValue> lst = reader.Reader(config);
            if (lst.Count == 0)
            {
                MessageService.ShowMessage("没有获取到膜厚数据。", "提示");
                return;
            }
            int i = 0;
            //遍历TableLayoutPnael的行
            for (int j = 1; j < tablePanel.RowCount; j++)
            {
                //遍历的列
                for (int column = 1; column < tablePanel.ColumnCount; column++)
                {
                    //j为tablePanel的行号。设置值。
                    string txtName = string.Format("txtBox{0}{1}", j.ToString("00"), column.ToString("00"));
                    Control[] controls = tablePanel.Controls.Find(txtName, true);
                    TextEdit textEdit = controls.Length == 0 ? null : controls[0] as TextEdit;
                    string deviceType = textEdit == null ? string.Empty : textEdit.Tag.ToString().Substring(0, 1);
                    if (deviceType == "T")//设备类型是折射率设备且数据为空。
                    {
                        if (textEdit.Text.Trim() == string.Empty)
                        {
                            double val = 0;
                            double.TryParse(lst[i].Value, out val);
                            val = Math.Round(val, DIGITS, MidpointRounding.AwayFromZero);
                            textEdit.Text = val.ToString();
                            i++;
                        }
                        if (i >= lst.Count)
                        {
                            return;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取方阻数据。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetResistance_Click(object sender, EventArgs e)
        {
            int result = ReadResistanceData(DateTime.MinValue, false);
            if (result == 1)
            {
                MessageService.ShowMessage("获取方阻数据失败，没有找到方阻数据文件。", "提示");
                return;
            }
            else if (result == 2)
            {
                MessageService.ShowMessage("没有获取到方阻数据。", "提示");
                return;
            }
        }
        /// <summary>
        /// 是否自动读取方租数据。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkIsAutoGetData_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsAutoGetData.Checked)
            {
                mreReadResistanceData.Reset();
                dtResistanceData = DateTime.Now;
                tReadResistanceData = new Thread(StartReadResistanceData);
                tReadResistanceData.Start();
            }
        }
        /// <summary>
        /// 启动读取方租数据的线程。
        /// </summary>
        private void StartReadResistanceData()
        {
            int count = 0;
            //启用自动读取方租数据时，一直进行循环读取判断。
            while (chkIsAutoGetData.Checked)
            {
                DateTime dtStartTime = dtResistanceData;
                int result = ReadResistanceData(dtStartTime, true);
                if (result == 0)
                {
                    count++;
                    SetLabelControlText(this.lblIsAutoGetData, string.Format("(已自动获取{0}片方阻数据。)", count));
                }
                Thread.Sleep(1000);
            }
            mreReadResistanceData.Set();
        }
        /// <summary>
        /// 读取方租数据。
        /// </summary>
        /// <returns>
        /// 1：获取方阻数据失败，没有找到方阻数据文件。
        /// 2：没有获取到方阻数据。
        /// 3：自动读取状况下，如果抓取到相同文件则不再继续读取。
        /// 0：读取方租数据成功。
        /// </returns>
        private int ReadResistanceData(DateTime startTime, bool bAutoRead)
        {
            DataFileReadConfig config = DataFileReadConfigReader.GetResistanceConfig(startTime);
            config.PointCount = rParamCount;
            if (string.IsNullOrEmpty(config.FileName))
            {
                return 1;
            }
            if (bAutoRead && lastFileName == config.FileName)
            {
                return 3;
            }
            if (bAutoRead)
            {
                dtResistanceData = config.FileLastWriteTime;
                lastFileName = config.FileName;
            }

            DataFileReader reader = new DataFileReader();
            IList<DataFileReadValue> lst = reader.Reader(config);
            if (lst.Count == 0)
            {
                return 2;
            }
            int i = 0;
            //遍历TableLayoutPnael的行
            for (int j = 1; j < tablePanel.RowCount; j++)
            {
                //遍历的列
                for (int column = 1; column < tablePanel.ColumnCount; column++)
                {
                    //j为tablePanel的行号。设置值。
                    string txtName = string.Format("txtBox{0}{1}", j.ToString("00"), column.ToString("00"));
                    Control[] controls = tablePanel.Controls.Find(txtName, true);
                    TextEdit textEdit = controls.Length == 0 ? null : controls[0] as TextEdit;
                    string deviceType = textEdit == null ? string.Empty : textEdit.Tag.ToString().Substring(0, 1);
                    if (deviceType == "R")//设备类型是折射率设备且数据为空。
                    {
                        if (textEdit.Text.Trim() == string.Empty)
                        {
                            double val = 0;
                            double.TryParse(lst[i].Value, out val);
                            val = Math.Round(val, DIGITS, MidpointRounding.AwayFromZero);
                            //textEdit.Text = val.ToString();
                            SetTextEditValue(textEdit, val.ToString());
                            i++;
                        }
                        if (i >= lst.Count)
                        {
                            return 0;
                        }
                    }
                }
            }
            return 0;
        }
        /// <summary>
        /// 设置TextEdit控件文本的委托，用于多线程。
        /// </summary>
        /// <param name="text"></param>
        public delegate void SetTextEditTextCallBack(TextEdit te, string text);
        /// <summary>
        /// 设置LabelControl控件文本的委托，用于多线程。
        /// </summary>
        /// <param name="text"></param>
        public delegate void SetLabelControlTextCallBack(LabelControl lc, string text);
        /// <summary>
        /// 设置文本框文本。
        /// </summary>
        /// <param name="te">文本框对象。</param>
        /// <param name="val">文本值。</param>
        private void SetTextEditValue(TextEdit te, string val)
        {
            if (te.InvokeRequired)
            {
                SetTextEditTextCallBack d = new SetTextEditTextCallBack(SetTextEditValue);
                te.Invoke(d, te, val);
            }
            else
            {
                te.Text = val;
                te.Focus();
            }
        }
        /// <summary>
        /// 设置LabelControl的文本值。
        /// </summary>
        /// <param name="lc">LabelControl对象。</param>
        /// <param name="val">文本值。</param>
        private void SetLabelControlText(LabelControl lc, string val)
        {
            if (lc.InvokeRequired)
            {
                SetLabelControlTextCallBack d = new SetLabelControlTextCallBack(SetLabelControlText);
                lc.Invoke(d, lc, val);
            }
            else
            {
                lc.Text = val;
            }
        }
        /// <summary>
        /// 窗体载入事件函数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EDCData04W_Load(object sender, EventArgs e)
        {
            isFirstLoading = true;
            LotQueryEntity queryEntity = new LotQueryEntity();
            if (_edcData.EDCActionName != "NONE") //如果数据采集=在线数据采集。
            {
                DataSet dsLotInfo = new DataSet();
                dsLotInfo = queryEntity.GetLotInfo(_edcData.LotNumber);
                //显示批次的基础信息。
                LotBaseInfoCtrl lotInfoCtrl = new LotBaseInfoCtrl();
                lotInfoCtrl.SetValueToControl(dsLotInfo);
                lotInfoCtrl.Dock = DockStyle.Fill;
                this.gcLotInfo.Controls.Add(lotInfoCtrl);

                if (dsLotInfo.Tables.Count > 0 && dsLotInfo.Tables[0].Rows.Count > 0)
                {
                    edcInsKey = dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_EDC_INS_KEY].ToString();  //批次数据采集主键
                    lotKey = dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();         //批次主键。
                }
                //如果有传入数据采集实例主键。
                if (!string.IsNullOrEmpty(_edcData.EDCMainInsKey))
                {
                    edcInsKey = _edcData.EDCMainInsKey;
                }
                //批次进行数据采集的主键为null或空字符串，禁用提交按钮。
                if (string.IsNullOrEmpty(edcInsKey))
                {
                    btSubmit.Enabled = false;
                    btnSave.Enabled = false;
                }
                dsParam = _edcData.GetPointParamsByEDCInsKey(edcInsKey);
            }
            else//如果数据采集是离线数据采集。
            {
                EDCBaseInfoCtrl baseInfoCtrl = new EDCBaseInfoCtrl(_edcData);
                baseInfoCtrl.Dock = DockStyle.Fill;
                this.gcLotInfo.Controls.Add(baseInfoCtrl);

                if (!string.IsNullOrEmpty(_edcData.LotNumber))//如果批次号不为空。
                {
                    DataSet dsLotInfo = queryEntity.GetLotInfo(_edcData.LotNumber);
                    if (string.IsNullOrEmpty(_edcData.ErrorMsg))//如果成功获取。
                    {
                        if (dsLotInfo.Tables.Count > 0 && dsLotInfo.Tables[0].Rows.Count > 0)
                        {
                            lotKey = dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();              //批次主键。
                            partKey = dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_PART_VER_KEY].ToString();        //成品主键。
                            materialLot = dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_MATERIAL_LOT].ToString();    //原材料批号。
                            workOrderKey = dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY].ToString(); //工单主键。
                        }
                    }
                }
                //如果有传入数据采集实例主键。
                if (!string.IsNullOrEmpty(_edcData.EDCMainInsKey))
                {
                    edcInsKey = _edcData.EDCMainInsKey;
                    dsParam = _edcData.GetPointParamsByEDCInsKey(edcInsKey);
                }
                else
                {
                    edcInsKey = CommonUtils.GenerateNewKey(0);
                    dsParam = _edcData.GetPointParamsByPointKey(_edcData.EDCPointKey);
                }
            }

            bIsEDCAllowManualInput = computerEntity.GetIsEDCAllowManualInputValue();
            if (_edcData.ErrorMsg == string.Empty)//获取批次采样参数点成功。
            {
                this.gcParam.MainView = gvParam;
                //显示数据采集
                gcParam.DataSource = dsParam.Tables[0];
                gvParam.BestFitColumns();
                int rCount = dsParam.Tables[0].Select("DEVICE_TYPE='R'").Count(); //方阻设备
                int aCount = dsParam.Tables[0].Select("DEVICE_TYPE='A'").Count(); //折射率设备
                int tCount = dsParam.Tables[0].Select("DEVICE_TYPE='T'").Count(); //膜厚设备
                int fCount = dsParam.Tables[0].Select("DEVICE_TYPE='F'").Count(); //反射率设备
                this.btnGetResistance.Visible = rCount > 0;                       //方阻设备
                this.pnlIsAutoGetData.Visible = rCount > 0;
                this.btnGetRefraction.Visible = aCount > 0;                       //折射率设备
                this.btnGetThickness.Visible = tCount > 0;                        //膜厚设备
                this.btnGetRflectance.Visible = fCount > 0;                       //反射率
            }
            else //获取批次采样参数点失败，给出错误提示。
            {
                MessageService.ShowError("查询参数出错：" + _edcData.ErrorMsg);
            }

            if (InitializeUI())
            {
                if (_edcData.EDCActionName != "NONE"                     //如果数据采集=在线数据采集。
                    || !string.IsNullOrEmpty(_edcData.EDCMainInsKey))
                {
                    BindInitialData();
                }
            }
            else
            {
                btSubmit.Enabled = false;           //禁用保存按钮
            }
            isFirstLoading = false;

            //如果自动抓取方阻数据显示，则自动获取的数据选中。
            if (this.pnlIsAutoGetData.Visible)
            {
                this.chkIsAutoGetData.Checked = true;
            }
            this.tablePanel.SelectNextControl(ActiveControl, true, true, true, true);
        }
    }
}
