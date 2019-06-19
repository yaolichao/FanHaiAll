using System;

namespace FanHai.Gui.Core
{
    public delegate void FileNameEventHandler(object sender, FileNameEventArgs e);

    /// <summary>
    /// Description of FileEventHandler.
    /// </summary>
    public class FileNameEventArgs : System.EventArgs
    {
        string fileName;

        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        public FileNameEventArgs(string fileName)
        {
            this.fileName = fileName;
        }
    }
}
