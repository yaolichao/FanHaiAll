using System.Drawing;
using System.Drawing.Drawing2D;

namespace FanHai.Hemera.Utils.Common
{
    /// <summary>
    /// 图片处理辅助类
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// 缩放图片（支持等比例、拉伸）
        /// </summary>
        /// <param name="img">源图片</param>
        /// <param name="toWidth">目标宽度，若为0，表示宽度按比例缩放</param>
        /// <param name="toHeight">目标高度，若为0，表示高度按比例缩放</param>
        /// <returns>缩放处理后的图片</returns>
        public static Image GetThumbnail(Image img, int toWidth, int toHeight)
        {
            if (toWidth == 0)
            {
                toWidth = toHeight * img.Width / img.Height;
            }
            if (toHeight == 0)
            {
                toHeight = toWidth * img.Height / img.Width;
            }

            Image result = new Bitmap(toWidth, toHeight);
            Graphics g = Graphics.FromImage(result);
            g.Clear(Color.Transparent);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, new Rectangle(0, 0, toWidth, toHeight), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
            g.Dispose();
            return result;
        }
    }
}
