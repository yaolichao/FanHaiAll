using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using FanHai.Hemera.Utils.Helper;
using System.IO;

namespace FanHai.Hemera.Utils.Barcode
{
    public sealed class ZebraNetPrinterHelper
    {
        public ZebraNetPrinterHelper()
        {
        }

        public static void zb_print_errorlable(PrintLabelParameterData data)
        {
            throw new NotImplementedException();
        }

        public static bool PrintLabel(PrintLabelParameterData data)
        {
            MethodInfo method = null;
            try
            {
                string methodName = "zb_printlable" + data.LabelNo.PadLeft(2, '0');

                method = typeof(ZebraNetPrinterHelper).GetMethod(methodName);
                if (method == null)
                {
                    data.ErrorMessage = string.Format("请IT确认标签【{0}】对应的方法是否实现或发布！", data.LabelNo);
                    return false;
                }

                if (method != null)
                {
                    int parameterLength = method.GetParameters().Length;
                    object[] objs = new object[parameterLength];

                    objs[0] = data;

                    object retObj = method.Invoke(null, objs);
                    if (retObj != null)
                    {
                        return Convert.ToBoolean(retObj);
                    }
                }

            }
            catch (Exception ex)
            {
                data.ErrorMessage = ex.Message;
            }
            return true;
        }

        /// <summary>
        /// SE侧板标签
        /// </summary>
        public static bool zb_printlable57(PrintLabelParameterData data)
        {
            bool isTrue = false;

            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Resource\ZebraDemo\" + "ZebraCode" + data.LabelNo + ".txt";

                if (File.Exists(filePath))
                {
                    string zebraCodeTemplate = string.Empty;

                    using (StreamReader myReader = new StreamReader(filePath))
                    {
                        zebraCodeTemplate = myReader.ReadToEnd();
                        myReader.Close();
                    }

                    zebraCodeTemplate = string.Format(zebraCodeTemplate, data.LotNo.Substring(0, 4), data.LotNo.Substring(4, 10), data.LotNo);

                    isTrue = NetWorkPrinterHelper.SendStringToPrinter(data.LabelPrinterIP, data.LablePrinterPort, zebraCodeTemplate);
                }
                else
                {
                    data.ErrorMessage = string.Format("不存在文件：{0}。请确认", filePath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isTrue;
        }

        /// <summary>
        /// add by chao.pang 
        /// </summary>
        public static bool zb_printlable58(PrintLabelParameterData data)
        {
            bool isTrue = false;

            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Resource\ZebraDemo\" + "ZebraCode" + data.LabelNo + ".txt";

                if (File.Exists(filePath))
                {
                    string zebraCodeTemplate = string.Empty;

                    using (StreamReader myReader = new StreamReader(filePath))
                    {
                        zebraCodeTemplate = myReader.ReadToEnd();
                        myReader.Close();
                    }

                    zebraCodeTemplate = string.Format(zebraCodeTemplate, data.LotNo.Substring(0, 4), data.LotNo.Substring(4, 10), data.LotNo);

                    isTrue = NetWorkPrinterHelper.SendStringToPrinter(data.LabelPrinterIP, data.LablePrinterPort, zebraCodeTemplate);
                }
                else
                {
                    data.ErrorMessage = string.Format("不存在文件：{0}。请确认", filePath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isTrue;
        }

        /// <summary>
        /// 添加斑马功率标签打印 体现功率
        /// </summary>
        public static bool zb_printlable59(PrintLabelParameterData data)
        {
            bool isTrue = false;

            try
            {
                string coefPM = data.CoefPM.ToString();
                string coefVOC = data.CoefVOC.ToString("#,##0.00");
                string coefISC = data.CoefISC.ToString("#,##0.00");
                string coefVPM = data.CoefVPM.ToString("#,##0.00");
                string coefIPM = data.CoefIPM.ToString("#,##0.00");

                string SubPowerLevel= data.PowersetSubPowerLevel ?? string.Empty;

                if (!string.IsNullOrEmpty(SubPowerLevel) && SubPowerLevel.Length >= 2 && SubPowerLevel.LastIndexOf('-') >= 0)
                {
                    coefPM = data.CoefPM.ToString("#,##0.00");
                    SubPowerLevel = SubPowerLevel.Substring(SubPowerLevel.LastIndexOf('-'), 2);
                }
                else
                {
                    coefPM = data.CoefPM.ToString("#,##0.00");
                }


                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Resource\ZebraDemo\" + "ZebraCode" + data.LabelNo + ".txt";

                if (File.Exists(filePath))
                {
                    string zebraCodeTemplate = string.Empty;

                    using (StreamReader myReader = new StreamReader(filePath))
                    {
                        zebraCodeTemplate = myReader.ReadToEnd();
                        myReader.Close();
                    }

                    zebraCodeTemplate = string.Format(zebraCodeTemplate, 
                                                                        data.LotNo.Substring(0, 4), 
                                                                        data.LotNo.Substring(4, data.LotNo.Length - 4), 
                                                                        data.LotNo,
                                                                        coefPM,
                                                                        SubPowerLevel,
                                                                        coefVOC,
                                                                        coefISC,
                                                                        coefVPM,
                                                                        coefIPM);

                    isTrue = NetWorkPrinterHelper.SendStringToPrinter(data.LabelPrinterIP, data.LablePrinterPort, zebraCodeTemplate);
                }
                else
                {
                    data.ErrorMessage = string.Format("不存在文件：{0}。请确认", filePath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isTrue;
        }

        /// <summary>
        /// 添加斑马功率标签打印 不体现功率
        /// </summary>
        public static bool zb_printlable61(PrintLabelParameterData data)
        {
            bool isTrue = false;

            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Resource\ZebraDemo\" + "ZebraCode" + data.LabelNo + ".txt";

                if (File.Exists(filePath))
                {
                    string zebraCodeTemplate = string.Empty;

                    using (StreamReader myReader = new StreamReader(filePath))
                    {
                        zebraCodeTemplate = myReader.ReadToEnd();
                        myReader.Close();
                    }

                    zebraCodeTemplate = string.Format(zebraCodeTemplate,
                                                                        data.LotNo.Substring(0, 4),
                                                                        data.LotNo.Substring(4, data.LotNo.Length - 4),
                                                                        data.LotNo);

                    isTrue = NetWorkPrinterHelper.SendStringToPrinter(data.LabelPrinterIP, data.LablePrinterPort, zebraCodeTemplate);
                }
                else
                {
                    data.ErrorMessage = string.Format("不存在文件：{0}。请确认", filePath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isTrue;
        }

    }
}
