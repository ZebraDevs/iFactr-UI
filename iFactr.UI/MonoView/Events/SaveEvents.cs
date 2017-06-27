using System;

namespace iFactr.UI
{
    /// <summary>
    /// Represents the method that will handle the <see cref="ICanvasView.DrawingSaved"/> event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    public delegate void SaveEventHandler(object sender, SaveEventArgs args);

    /// <summary>
    /// Provides data for the <see cref="ICanvasView.DrawingSaved"/> event.
    /// </summary>
    public class SaveEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the path of the file that was saved.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.SaveEventArgs"/> class.
        /// </summary>
        /// <param name="filePath">The path of the file that was saved.</param>
        public SaveEventArgs(string filePath)
        {
            FilePath = filePath;
        }
    }
}
