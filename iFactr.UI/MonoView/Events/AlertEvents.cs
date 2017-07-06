using System;

namespace iFactr.UI
{
    /// <summary>
    /// Represents the method that will handle the <see cref="iFactr.UI.IAlert.Dismissed"/> event.
    /// </summary>
    public delegate void AlertResultEventHandler(object sender, AlertResultEventArgs args);

    /// <summary>
    /// Provides data for the <see cref="iFactr.UI.IAlert.Dismissed"/> event.
    /// </summary>
    public class AlertResultEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the result of the alert dialog.
        /// </summary>
        public AlertResult Result { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.AlertResultEventArgs"/> class.
        /// </summary>
        /// <param name="result">The result of the alert dialog.</param>
        public AlertResultEventArgs(AlertResult result)
        {
            Result = result;
        }
    }
}
