using System;

namespace FanHai.Gui.Core
{
    public abstract class AbstractCheckableMenuCommand : AbstractMenuCommand, ICheckableMenuCommand
    {
        bool isChecked = false;

        public virtual bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
            }
        }
        public override void Run()
        {
            IsChecked = !IsChecked;
        }
    }
}
