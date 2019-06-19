using System;

namespace FanHai.Gui.Core
{
    public interface IComboBoxCommand : ICommand
    {
        bool IsEnabled
        {
            get;
            set;
        }
    }
}
