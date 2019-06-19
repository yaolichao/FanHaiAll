using System;
using System.Windows.Forms;

namespace FanHai.Gui.Core.WinForms
{
    public interface ISubmenuBuilder
    {
        ToolStripItem[] BuildSubmenu(Codon codon, object owner);
    }
}
