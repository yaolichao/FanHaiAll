using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FanHai.Hemera.Utils.Common
{
    /// <summary>
    /// PDF文件操作辅助类（基于iTextSharp）
    /// </summary>
    public class PdfHelper
    {
        /// <summary>
        /// 生成PDF文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="source">数据源</param>
        public static void CreatePdf(string filePath, List<System.Drawing.Image> source)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            Rectangle rect = PageSize.A4;
            Document document = new Document(rect, 0, 0, 0, 0);
            //document.SetMargins(0, 0, 0, 0);// 设置边距
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create, FileAccess.Write));
            document.Open();

            float contentWidth = rect.Width - document.LeftMargin - document.RightMargin;
            float contentHeight = rect.Height - document.TopMargin - document.BottomMargin;
            foreach (var img in source)
            {
                //Image element = Image.GetInstance(path);
                Image element = Image.GetInstance(img, ImageFormat.Jpeg);
                element.Alignment = 1;// 对齐方式（0为居左，1为居中，2为居右）
                if (element.Width > contentWidth || element.Height > contentHeight)
                {
                    // 等比率缩放
                    float width = contentWidth;
                    float height = width * element.Height / element.Width;
                    if (height > contentHeight)
                    {
                        height = contentHeight;
                        width = height * element.Width / element.Height;
                    }
                    element.ScaleAbsolute(width, height);
                }

                document.NewPage();
                document.Add(element);
            }

            document.Close();
            if (writer != null) writer.Close();
        }
    }
}
