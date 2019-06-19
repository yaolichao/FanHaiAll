using System;

namespace FanHai.Gui.Core
{
    public interface ICheckableMenuCommand : IMenuCommand
    {
        bool IsChecked
        {
            get;
            set;
        }
    }
}
