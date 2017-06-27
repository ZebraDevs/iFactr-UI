using System;

namespace iFactr.UI
{
    /// <summary>
    /// Provides data for the <see cref="IBrowserView.LoadFinished"/> event.
    /// </summary>
    public class LoadFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the URL that was navigated to.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadFinishedEventArgs"/> class.
        /// </summary>
        /// <param name="url">The URL that was navigated to.</param>
        public LoadFinishedEventArgs(string url)
        {
            Url = url;
        }
    }
}
