using System;

namespace iFactr.UI
{
    /// <summary>
    /// Represents the method that will handle the <see cref="IListView.Submitting"/> event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    public delegate void SubmissionEventHandler(object sender, SubmissionEventArgs args);

    /// <summary>
    /// Provides data for the <see cref="IListView.Submitting"/> event.
    /// </summary>
    public class SubmissionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the submission should be halted.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets the <see cref="Link"/> that will be navigated to once submission is completed.
        /// </summary>
        public Link DestinationLink { get; private set; }

        /// <summary>
        /// Gets a collection of the validation errors that have occurred.
        /// </summary>
        public ValidationErrorCollection ValidationErrors { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.SubmissionEventArgs"/> class.
        /// </summary>
        /// <param name="destinationLink">The link that will be navigated to once submission is complete.</param>
        /// <param name="validationErrors">A collection of the validation errors that have occurred.</param>
        public SubmissionEventArgs(Link destinationLink, ValidationErrorCollection validationErrors)
        {
            DestinationLink = destinationLink;
            ValidationErrors = validationErrors;
        }
    }
}