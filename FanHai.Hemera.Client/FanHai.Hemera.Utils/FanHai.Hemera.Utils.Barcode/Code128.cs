using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace FanHai.Hemera.Utils.Barcode
{
    public  class  Code128
    {
        private const int _itemSepHeight = 3;

        SizeF _titleSize = SizeF.Empty;
        SizeF _barCodeSize = SizeF.Empty;
        SizeF _codeStringSize = SizeF.Empty;

        public Code128()
        {
            _titleFont = new Font("Arial", 10);
            _codeStringFont = new Font("Arial", 10);
        }

        #region Barcode Title

        private string _titleString = null;
        private Font _titleFont = null;
        private string _rightString = null;

        public string RightString
        {
            get { return _rightString; }
            set { _rightString = value; }
        }

        public string Title
        {
            get { return _titleString; }
            set { _titleString = value; }
        }

        public Font TitleFont
        {
            get { return _titleFont; }
            set { _titleFont = value; }
        }
        #endregion

        #region Barcode code string

        private bool _showCodeString = false;
        private Font _codeStringFont = null;

        public bool ShowCodeString
        {
            get { return _showCodeString; }
            set { _showCodeString = value; }
        }

        public Font CodeStringFont
        {
            get { return _codeStringFont; }
            set { _codeStringFont = value; }
        }
        #endregion

        #region Barcode Font

        private Font _c128Font = null;
        private float _c128FontSize = 12;
        private string _c128FontFileName = null;
        private string _c128FontFamilyName = null;

        public string FontFileName
        {
            get { return _c128FontFileName; }
            set { _c128FontFileName = value; }
        }

        public string FontFamilyName
        {
            get { return _c128FontFamilyName; }
            set { _c128FontFamilyName = value; }
        }

        public float FontSize
        {
            get { return _c128FontSize; }
            set { _c128FontSize = value; }
        }

        private Font Code128Font
        {
            get
            {
                if (_c128Font == null)
                {
                    // Load the barcode font			
                    PrivateFontCollection pfc = new PrivateFontCollection();
                    pfc.AddFontFile(_c128FontFileName);
                    FontFamily family = new FontFamily(_c128FontFamilyName, pfc);
                    _c128Font = new Font(family, _c128FontSize);
                }
                return _c128Font;
            }
        }

        #endregion

        #region Auxiliary Methods

        private int Max(int v1, int v2)
        {
            return (v1 > v2 ? v1 : v2);
        }

        private int XCentered(int localWidth, int globalWidth)
        {
            return ((globalWidth - localWidth) / 2);
        }

        #endregion

        public string Get128CodeString(string inputData)
        {
            string result;
            int checksum = 104;
            for (int ii = 0; ii < inputData.Length; ii++)
            {
                if (inputData[ii] >= 32)
                {
                    checksum += (inputData[ii] - 32) * (ii + 1);
                }
                else
                {
                    checksum += (inputData[ii] + 64) * (ii + 1);
                }
            }
            checksum = checksum % 103;
            if (checksum < 95)
            {
                checksum += 32;
            }
            else
            {
                checksum += 100;
            }

            result = Convert.ToChar(204) + inputData.ToString() + Convert.ToChar(checksum) + Convert.ToChar(206);
            return result;

        }

        public Bitmap GenerateBarcode(string barCode)
        {
            Font ft1 = new System.Drawing.Font("Times New Roman", 18, FontStyle.Regular, GraphicsUnit.World);
            Font ft2 = new System.Drawing.Font("Code 128", 64, FontStyle.Regular, GraphicsUnit.World);
            Brush br = new SolidBrush(Color.Black);

            int bcodeWidth = 0;
            int bcodeHeight = 0;

            // Get the image container...
            Bitmap bcodeBitmap = CreateImageContainer(Get128CodeString(barCode), ref bcodeWidth, ref bcodeHeight);
            Graphics objGraphics = Graphics.FromImage(bcodeBitmap);

            // Fill the background			
            objGraphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, bcodeWidth, bcodeHeight));

            int vpos = 0;
            // Draw the barcode
            //vpos += (((int)_codeStringFont.Height));
            objGraphics.DrawString(Get128CodeString(barCode), Code128Font, new SolidBrush(Color.Black), 2, vpos);

            int hpos = 0;
            if (_rightString != null)
            {
                hpos = (int)objGraphics.MeasureString(Get128CodeString(barCode), Code128Font).Width;
                objGraphics.DrawString(_rightString, _codeStringFont, new SolidBrush(Color.Black), hpos, vpos);
            }

            // Draw the barcode string
            if (_showCodeString)
            {
                vpos += (((int)_barCodeSize.Height));
                //objGraphics.DrawString(barCode, _codeStringFont, new SolidBrush(Color.Black), XCentered((int)_codeStringSize.Width, bcodeWidth), vpos);
                objGraphics.DrawString(barCode, _codeStringFont, new SolidBrush(Color.Black), 10, vpos);
            }

            // Draw the title string
            if (_titleString != null)
            {
                //vpos += (((int)_titleSize.Height) + _itemSepHeight);
                vpos += (((int)_codeStringFont.Height) + _itemSepHeight);
                objGraphics.DrawString(_titleString, _titleFont, new SolidBrush(Color.Black), 10, vpos);
            }

            //objGraphics.DrawString(Get128CodeString(barCode), ft2, br, 50, -3);
            //objGraphics.DrawString(barCode, ft1, br, 110, 60);
            return bcodeBitmap;

        }

        private Bitmap CreateImageContainer(string barCode, ref int bcodeWidth, ref int bcodeHeight)
        {

            Graphics objGraphics;

            // Create a temporary bitmap...
            Bitmap tmpBitmap = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
            objGraphics = Graphics.FromImage(tmpBitmap);

            // calculate size of the barcode items...
            if (_titleString != null)
            {
                _titleSize = objGraphics.MeasureString(_titleString, _titleFont);
                bcodeWidth = (int)_titleSize.Width;
                bcodeHeight = (int)_titleSize.Height + _itemSepHeight;
            }

            _barCodeSize = objGraphics.MeasureString(barCode, Code128Font);
            bcodeWidth = Max(bcodeWidth, (int)_barCodeSize.Width);
            bcodeHeight += (int)_barCodeSize.Height;

            if (_showCodeString)
            {
                _codeStringSize = objGraphics.MeasureString(barCode, _codeStringFont);
                bcodeWidth = Max(bcodeWidth, (int)_codeStringSize.Width);
                bcodeHeight += (_itemSepHeight + (int)_codeStringSize.Height);
            }

            // dispose temporary objects...
            objGraphics.Dispose();
            tmpBitmap.Dispose();

            return (new Bitmap(bcodeWidth, bcodeHeight, PixelFormat.Format32bppArgb));
        }
    }
}
