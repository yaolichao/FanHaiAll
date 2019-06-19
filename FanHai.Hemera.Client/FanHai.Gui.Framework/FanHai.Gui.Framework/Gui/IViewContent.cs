using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FanHai.Gui.Framework.Gui
{
    public delegate void SaveEventHandler(object sender, SaveEventArgs e);

    public class SaveEventArgs : System.EventArgs
    {
        bool successful;

        public bool Successful
        {
            get
            {
                return successful;
            }
        }

        public SaveEventArgs(bool successful)
        {
            this.successful = successful;
        }
    }

    /// <summary>
    /// IViewContent is the base interface for "windows" in the document area of SolarViewerFramework.
    /// A view content is a view onto multiple files, or other content that opens like a document
    /// (e.g. the start page).
    /// </summary>
    public interface IViewContent : IDisposable, ICanBeDirty
    {
        /// <summary>
        /// This is the Windows.Forms control for the view.
        /// </summary>
        Control Control
        {
            get;
        }

        /// <summary>
        /// The workbench window in which this view is displayed.
        /// </summary>
        IWorkbenchWindow WorkbenchWindow
        {
            get;
            set;
        }

        /// <summary>
        /// Is raised when the value of the TabPageText property changes.
        /// </summary>
        event EventHandler TabPageTextChanged;

        /// <summary>
        /// The text on the tab page when more than one view content
        /// is attached to a single window.
        /// </summary>
        string TabPageText
        {
            get;
        }

        /// <summary>
        /// Reinitializes the content. (Re-initializes all add-in tree stuff)
        /// and redraws the content.
        /// Called on certain actions like changing the UI language.
        /// </summary>
        void RedrawContent();

        /// <summary>
        /// The title of the view content. This normally is the title of the primary file being edited.
        /// </summary>
        string TitleName
        {
            get;
            set;
        }

        /// <summary>
        /// Is called each time the name for the content has changed.
        /// </summary>
        event EventHandler TitleNameChanged;

        /// <summary>
        /// Gets the name of the primary file being edited. Might return null if no file is edited.
        /// </summary>
        string PrimaryFileName { get; }

        /// <summary>
        /// Builds an <see cref="INavigationPoint"/> for the current position.
        /// </summary>
        INavigationPoint BuildNavPoint();

        bool IsDisposed { get; }

        event EventHandler Disposed;

        /// <summary>
        /// Gets if the view content is read-only (can be saved only when choosing another file name).
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets if the view content is view-only (cannot be saved at all).
        /// </summary>
        bool IsViewOnly { get; }

        #region Secondary view content support
        /// <summary>
        /// Gets the collection that stores the secondary view contents.
        /// </summary>
        ICollection<IViewContent> SecondaryViewContents { get; }

        #endregion
    }
}
