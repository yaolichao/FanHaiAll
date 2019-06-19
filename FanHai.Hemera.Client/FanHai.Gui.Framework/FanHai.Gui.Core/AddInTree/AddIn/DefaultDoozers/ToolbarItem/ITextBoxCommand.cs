using System;

namespace FanHai.Gui.Core
{
    public interface ITextBoxCommand : ICommand
    {
        bool IsEnabled
        {
            get;
            set;
        }
    }
}
