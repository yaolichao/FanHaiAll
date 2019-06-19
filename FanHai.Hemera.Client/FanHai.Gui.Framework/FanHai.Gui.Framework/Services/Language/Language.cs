using System;

namespace FanHai.Gui.Framework
{
    public class Language
    {
        string name;
        string code;
        int imageIndex;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Code
        {
            get
            {
                return code;
            }
        }

        public int ImageIndex
        {
            get
            {
                return imageIndex;
            }
        }

        public Language(string name, string code, int imageIndex)
        {
            this.name = name;
            this.code = code;
            this.imageIndex = imageIndex;
        }
    }
}
