using System;
using System.Drawing;
using System.Windows.Forms;

using FanHai.Gui.Core.WinForms;
using FanHai.Gui.Framework.Gui.XmlForms;

namespace FanHai.Gui.Framework.Gui
{
    public class ScrollBox : UserControl
    {
        string[] text;
        int[] textHeights;

        Image image;
        Timer timer;
        int scroll = -220;

        public int ScrollY
        {
            get
            {
                return scroll;
            }
            set
            {
                scroll = value;
            }
        }

        public Image Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                timer.Stop();
                foreach (Control ctrl in Controls)
                {
                    ctrl.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public ScrollBox()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //Image = IconService.GetBitmap("Icons.AboutImage");
            Image = null;

            Font = WinFormsResourceService.LoadFont("Tahoma", 10);
            text = new string[] {
                "FanHai Hemera MES System"
                //"\"Habit 1: Be Proactive",
                //"\"Habit 2: Begin with the End in Mind",
                //"\"Habit 3: put First Things First",
                //"\"Habit 4: Think Win/Win",
                //"\"Habit 5: Put First to Understand, Then To be Understood",
                //"\"Habit 6: Synergize",
                //"\"Habit 7: Sharpen the Saw"
			};

            // randomize the order in which the texts are displayed
            //Random rnd = new Random();
            //for (int i = 0; i < text.Length; i++)
            //{
            //    Swap(ref text[i], ref text[rnd.Next(i, text.Length)]);
            //}

            timer = new Timer();
            timer.Interval = 40;
            timer.Tick += new EventHandler(ScrollDown);
            timer.Start();
        }

        void Swap(ref string a, ref string b)
        {
            string c = a;
            a = b;
            b = c;
        }

        void ScrollDown(object sender, EventArgs e)
        {
            ++scroll;
            Refresh();
        }

        protected override void OnPaintBackground(PaintEventArgs pe)
        {
            if (image != null)
            {
                pe.Graphics.DrawImage(image, 0, 0, Width, Height);
            }
        }
        int curText = 0;
        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            if (textHeights == null)
            {
                textHeights = new int[text.Length];
                for (int i = 0; i < text.Length; ++i)
                {
                    textHeights[i] = (int)g.MeasureString(text[i], Font, new SizeF(Width / 2, Height * 2)).Height;
                }
            }
            g.DrawString(text[curText],
                         Font,
                         Brushes.Black,
                         new Rectangle(Width / 2, 0 - scroll, Width / 2, Height * 2));

            if (scroll > textHeights[curText])
            {
                curText = (curText + 1) % text.Length;
                scroll = -textHeights[curText] - Height;
            }
        }
    }

    public class CommonAboutDialog : XmlForm
    {
        public ScrollBox ScrollBox
        {
            get
            {
                return (ScrollBox)ControlDictionary["aboutPictureScrollBox"];
            }
        }

        public CommonAboutDialog()
        {
            string[] names = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
            SetupFromXmlStream(this.GetType().Assembly.GetManifestResourceStream("FanHai.Gui.Framework.Resources.CommonAboutDialog.xfrm"));
        }

        protected override void SetupXmlLoader()
        {
            xmlLoader.StringValueFilter = new SolarViewerFrameworkStringValueFilter();
            xmlLoader.PropertyValueCreator = new SolarViewerFrameworkPropertyValueCreator();
        }
    }
}
