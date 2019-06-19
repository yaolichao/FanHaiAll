using System;

namespace FanHai.Gui.Core
{
    public interface IStringTagProvider
    {
        string[] Tags
        {
            get;
        }

        string Convert(string tag);
    }
}
