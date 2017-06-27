using System.Collections.Generic;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Defines a native view that acts as a grid for laying out various UI elements.
    /// </summary>
    public interface IGridView : IView, IGridBase, IHistoryEntry
    {
        /// <summary>
        /// Gets or sets a value indicating whether horizontal scrolling is enabled.
        /// If <c>false</c>, the content will be locked to the width of the view.
        /// </summary>
        bool HorizontalScrollingEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether vertical scrolling is enabled.
        /// If <c>false</c>, the content will be locked to the height of the view.
        /// </summary>
        bool VerticalScrollingEnabled { get; set; }

        /// <summary>
        /// Gets or sets a menu of selectable items that provide support functions for the view.
        /// </summary>
        IMenu Menu { get; set; }

        /// <summary>
        /// Gets a collection of errors that have occurred during control validation.
        /// </summary>
        ValidationErrorCollection ValidationErrors { get; }

        /// <summary>
        /// Occurs when the view is being submitted and the values of its controls are being gathered.
        /// Use this event to manipulate a submitted value prior to navigation or to cancel the submission.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event SubmissionEventHandler Submitting;

        /// <summary>
        /// Returns the control values that would be submitted as parameters if the <see cref="Submit(string)"/> or <see cref="Submit(Link)"/> methods are called.
        /// </summary>
        /// <returns>The control values as an <see cref="T:IDictionary&lt;string, string&gt;"/>.</returns>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        IDictionary<string, string> GetSubmissionValues();

        /// <summary>
        /// Submits the view, gathering the values of its controls and navigating
        /// to the specified URL with the gathered control values as parameters.
        /// </summary>
        /// <param name="url">The URL to navigate to after all control values have been gathered.</param>
        void Submit(string url);

        /// <summary>
        /// Submits the view, gathering the values of its controls and navigating to the specified
        /// <see cref="Link"/> with the gathered control values as parameters.
        /// </summary>
        /// <param name="link">The link to navigate to after all control values have been gathered.</param>
        void Submit(Link link);
    }

    /// <summary>
    /// Defines a native view that acts as a grid for laying out various UI elements.
    /// </summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    public interface IGridView<T> : IGridView, IMXView<T> { }
}
