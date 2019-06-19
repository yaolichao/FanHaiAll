using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Utils.Barcode
{
    public class PrintLabelParameterData
    {
        private StringBuilder _errMessage = null;
        public PrintLabelParameterData()
        {
            this.PrintQty = 1;
            this.IsEnableByProduct = false;
            this._errMessage = new StringBuilder();
        }
        public int Darkness { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Dpi { get; set; }

        /// <summary>
        /// 使用打印机的类型 
        /// 0：立像打印机
        /// 1：斑马ZPL打印
        /// 2：斑马网口打印
        /// </summary>
        public string LablePrinterType { get; set; }
        /// <summary>
        /// 打印机名称
        /// </summary>
        public string LablePrinterName { get; set; }
        /// <summary>
        /// 打印机IP地址
        /// </summary>
        public string LabelPrinterIP { get; set; }
        /// <summary>
        /// 打印机端口
        /// </summary>
        public string LablePrinterPort { get; set; }

        /// <summary>
        /// 打印铭牌 true：打印铭牌 false：打印功率标签。
        /// </summary>
        public bool IsPrintNameplate { get; set; }
        /// <summary>
        /// 打印机选择 true：打印时弹出窗体选择打印机 false：直接打印。
        /// </summary>
        public bool IsChoosePrint { get; set; }
        /// <summary>
        /// 是否启用联副产品入库。true是 false否
        /// </summary>
        public bool IsEnableByProduct { get; set; }
        public string IVTestDataKey { get; set; }
        /// <summary>
        /// 工单主键。
        /// </summary>
        public string WorkOrderKey { get; set; }
        /// <summary>
        /// 产品主键。
        /// </summary>
        public string ProductKey { get; set; }
        /// <summary>
        /// 功率分档主键。
        /// </summary>
        public string PowersetKey { get; set; }
        public string FactoryName { get; set; }
        public string ProductModel { get; set; }
        public string LabelNo { get; set; }
        public string PSign { get; set; }
        public string LotNo { get; set; }
        public string WorkOrderNumber { get; set; }
        public string PartNumber { get; set; }
        public string ProductCode { get; set; }
        public int FullPalletQty { get; set; }
        public int LotCellQty { get; set; }
        public string ArticleNo { get; set; }
        public string Celleff { get; set; }
        public string CalibrationNo { get; set; }
        public string StandCalibration { get; set; }
        public string DeviceNum { get; set; }
        public string TestRuleCode { get; set; }
        public decimal MinPower { get; set; }
        public decimal MaxPower { get; set; }
        public int Digits { get; set; }
        public int PrintQty { get; set; }
        public decimal PM { get; set; }
        public decimal IPM { get; set; }
        public decimal ISC { get; set; }
        public decimal VOC { get; set; }
        public decimal VPM { get; set; }
        public decimal FF { get; set; }
        public decimal CTM { get; set; }
        public decimal CoefPM { get; set; }
        public decimal CoefISC { get; set; }
        public decimal CoefIPM { get; set; }
        public decimal CoefVOC { get; set; }
        public decimal CoefVPM { get; set; }
        public decimal CoefFF { get; set; }
        public decimal Imp_Isc { get; set; }
        public string ImpIsc_Control { get; set; }
        public DateTime Cretate_Time { get; set; }
        public decimal PowerDifferent { get; set; }
        public decimal TestTemperature { get; set; }
        public string ErrorMessage
        {
            get
            {
                return this._errMessage.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this._errMessage.AppendLine(string.Format(">> {0}", value));
                }
            }
        }
        public bool IsPrintErrorMessage { get; set; }
        public DateTime TestTime { get; set; }
        public double CalibrationCycle { get; set; }
        public bool IsBCPData { get; set; }
        /// <summary>
        /// 分档规则代码。
        /// </summary>
        public string PowersetCode { get; set; }
        public int PowersetSeq { get; set; }
        public decimal PowersetDemandQty { get; set; }
        /// <summary>
        /// 档位名称。
        /// </summary>
        public string PowersetModuleName { get; set; }
        /// <summary>
        /// 档位编码。
        /// </summary>
        public string PowersetModuleCode { get; set; }
        public string PowersetStandardPM { get; set; }
        public decimal PowersetStandardISC { get; set; }
        public decimal PowersetStandardIPM { get; set; }
        public decimal PowersetStandardVOC { get; set; }
        public decimal PowersetStandardVPM { get; set; }
        public decimal PowersetStandardFuse { get; set; }
        public string PowersetPowerDifferent { get; set; }
        /// <summary>
        /// 子分档方式。
        /// </summary>
        public string PowersetSubWay { get; set; }
        public string PowersetSubPowerLevel { get; set; }
        public string PowersetSubCode { get; set; }
        public string CustomerCode
        {
            get
            {
                return this.PowersetModuleCode + this.LotNo;
            }
        }
        /// <summary>
        /// 侧板标签。
        /// </summary>
        public string SlideCode
        {
            get;
            set;
        }
        public decimal Round(decimal d)
        {
            return Math.Round(d, Digits, MidpointRounding.AwayFromZero);
        }
    }

}
