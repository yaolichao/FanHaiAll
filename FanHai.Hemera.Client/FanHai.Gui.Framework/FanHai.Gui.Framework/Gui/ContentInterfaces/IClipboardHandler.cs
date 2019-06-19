using System;

namespace FanHai.Gui.Framework.Gui
{
    public interface IClipboardHandler
    {
        bool EnableCut
        {
            get;
        }
        bool EnableCopy
        {
            get;
        }
        bool EnablePaste
        {
            get;
        }
        bool EnableDelete
        {
            get;
        }
        bool EnableSelectAll
        {
            get;
        }

        void Cut();
        void Copy();
        void Paste();
        void Delete();
        void SelectAll();
    }
}
