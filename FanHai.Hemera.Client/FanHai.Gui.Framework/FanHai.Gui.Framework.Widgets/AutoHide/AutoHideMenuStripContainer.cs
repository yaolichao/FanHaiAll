using System;
using System.Windows.Forms;
using System.Drawing;

namespace FanHai.Gui.Framework.Widgets.AutoHide
{
    /// <summary>
    /// AutoHideMenuStripContainer can be used instead of MenuStrip to get a menu
    /// which is automaticaly hiden and shown. It is especially useful in fullscreen.
    /// </summary>
    public class AutoHideMenuStripContainer : AutoHideContainer
    {
        protected bool dropDownOpened;

        Padding? defaultPadding;

        protected override void Reformat()
        {
            if (defaultPadding == null)
            {
                defaultPadding = ((MenuStrip)control).Padding;
            }
            ((MenuStrip)control).Padding = AutoHide ? Padding.Empty : (Padding)defaultPadding;
            base.Reformat();
        }   
        public AutoHideMenuStripContainer(MenuStrip menuStrip)
            : base(menuStrip)
        {
            menuStrip.AutoSize = false;
            menuStrip.ItemAdded += OnMenuItemAdded;
            menuStrip.BackColor = System.Drawing.Color.FromArgb(251, 248, 240); //SystemColors.GradientInactiveCaption;  
            menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            foreach (ToolStripMenuItem menuItem in menuStrip.Items)
            {                           
                AddEventHandlersForItem(menuItem);
            }
        }

        void OnMenuItemAdded(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                AddEventHandlersForItem(menuItem);
            }
        }

        void AddEventHandlersForItem(ToolStripMenuItem menuItem)
        {
            menuItem.DropDownOpened += delegate { dropDownOpened = true; };
            menuItem.DropDownClosed += delegate { dropDownOpened = false; if (!mouseIn) ShowOverlay = false; };
        }

        protected override void OnControlMouseLeave(object sender, EventArgs e)
        {
            mouseIn = false;
            if (!dropDownOpened) ShowOverlay = false;
        }
    }
}
