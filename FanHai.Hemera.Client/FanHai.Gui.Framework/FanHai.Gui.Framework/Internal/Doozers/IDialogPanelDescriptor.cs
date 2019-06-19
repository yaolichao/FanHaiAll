using System;
using System.Collections.Generic;

namespace FanHai.Gui.Framework
{
    public interface IDialogPanelDescriptor
    {
        /// <value>
        /// Returns the ID of the dialog panel codon
        /// </value>
        string ID
        {
            get;
        }

        /// <value>
        /// Returns the label of the dialog panel
        /// </value>
        string Label
        {
            get;
            set;
        }

        /// <summary>
        /// The child dialog panels (e.g. for treeviews)
        /// </summary>
        IEnumerable<IDialogPanelDescriptor> ChildDialogPanelDescriptors
        {
            get;
        }

        /// <value>
        /// Returns the dialog panel object
        /// </value>
        IDialogPanel DialogPanel
        {
            get;
        }
    }
}
