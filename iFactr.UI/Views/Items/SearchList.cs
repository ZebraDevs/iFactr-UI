using MonoCross.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a list of <see cref="iItem"/>s on an <see cref="iLayer"/> that is searchable.
    /// </summary>
    public class SearchList : iPagedList
    {
        /// <summary>
        /// The original Items.
        /// </summary>
        protected List<iItem> ItemsSource = null;

        /// <summary>
        /// Gets or sets an optional string to prepopulate the search bar with.
        /// A search will automatically be performed on this string when this instance is displayed.
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// Gets or sets whether the search bar should automatically receive focus when displayed.
        /// </summary>
        public bool AutoFocus { get; set; }

        /// <summary>
        /// Gets or sets whether the search bar should automatically receive focus if the list is empty.
        /// </summary>
        [Obsolete("Use AutoFocus instead.")]
        public bool AutoFocusOnEmptyList
        {
            get { return AutoFocus; }
            set { AutoFocus = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchList"/> class.
        /// </summary>
        public SearchList() { ShowOnLoad = false; }
        private bool ShowOnLoad { get; set; }

        /// <summary>
        /// Performs a search of the contents of the list using the term provided and a navigation URI for linking.
        /// </summary>
        /// <param name="navigationUri">A <see cref="String"/> representing the navigation URI value.</param>
        /// <param name="searchTerm">A <see cref="String"/> representing the term that is being searched.</param>
        public virtual void PerformSearch(string navigationUri, string searchTerm)
        {
            if (ItemsSource == null)
                ItemsSource = Items;
            if (searchTerm != null)
                searchTerm = searchTerm.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                Items = ItemsSource;
                return;
            }

            string[] delim = new string[] { " " };
#if !NETCF
            string[] terms = searchTerm.ToLower().Split(delim, StringSplitOptions.RemoveEmptyEntries);
#else
            string[] terms = searchTerm.ToLower().Split(delim.First().ToCharArray());
#endif
            List<iItem> results = ItemsSource.Where(item =>
                                       item.Text.Clean().Contains(terms[0]) ||
                                       item.Subtext.Clean().Contains(terms[0])).ToList();

            //filter the result set by each subsequent keyword
            for (int i = 1; i < terms.Length; i++)
            {
                results = results.Where(item =>
                                       item.Text.Clean().Contains(terms[i]) ||
                                       item.Subtext.Clean().Contains(terms[i])).ToList();
            }
            Items = results;
        }
    }
}