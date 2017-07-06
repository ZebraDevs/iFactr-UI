using System;

namespace iFactr.UI
{
    /// <summary>
    /// Represents the method that will handle the <see cref="ISearchBox.SearchPerformed"/> event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    public delegate void SearchEventHandler(object sender, SearchEventArgs args);

    /// <summary>
    /// Provides data for the <see cref="ISearchBox.SearchPerformed"/> event.
    /// </summary>
    public class SearchEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the text inside of the search box that fired the event.
        /// </summary>
        public string SearchText { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.SearchEventArgs"/> class.
        /// </summary>
        /// <param name="searchText">The text inside of the search box that fired the event.</param>
        public SearchEventArgs(string searchText)
        {
            SearchText = searchText;
        }
    }
}
