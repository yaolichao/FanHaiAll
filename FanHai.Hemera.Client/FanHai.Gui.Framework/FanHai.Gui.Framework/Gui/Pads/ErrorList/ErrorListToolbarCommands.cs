using System;
using FanHai.Gui.Core;

namespace FanHai.Gui.Framework.Gui
{
    public class ShowErrorsToggleButton : AbstractCheckableMenuCommand
    {
        public override bool IsChecked
        {
            get
            {
                return ErrorListPad.Instance.ShowErrors;
            }
            set
            {
                ErrorListPad.Instance.ShowErrors = value;
            }
        }
    }

    public class ShowWarningsToggleButton : AbstractCheckableMenuCommand
    {
        public override bool IsChecked
        {
            get
            {
                return ErrorListPad.Instance.ShowWarnings;
            }
            set
            {
                ErrorListPad.Instance.ShowWarnings = value;
            }
        }
    }

    public class ShowMessagesToggleButton : AbstractCheckableMenuCommand
    {
        public override bool IsChecked
        {
            get
            {
                return ErrorListPad.Instance.ShowMessages;
            }
            set
            {
                ErrorListPad.Instance.ShowMessages = value;
            }
        }
    }
}
