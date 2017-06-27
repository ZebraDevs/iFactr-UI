using System;
using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a text box that can perform search queries.
    /// </summary>
    public class SearchBox : ISearchBox
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:BackgroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string BackgroundColorProperty = "BackgroundColor";

        /// <summary>
        /// The name of the <see cref="P:BorderColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string BorderColorProperty = "BorderColor";

        /// <summary>
        /// The name of the <see cref="P:ForegroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ForegroundColorProperty = "ForegroundColor";

        /// <summary>
        /// The name of the <see cref="P:Placeholder"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string PlaceholderProperty = "Placeholder";

        /// <summary>
        /// The name of the <see cref="P:TextCompletion"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TextCompletionProperty = "TextCompletion";

        /// <summary>
        /// The name of the <see cref="P:Text"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TextProperty = "Text";
        #endregion

        /// <summary>
        /// Occurs when the user executes a search through the search box.
        /// Use this event to filter any model data and reload the view's content.
        /// </summary>
        public event SearchEventHandler SearchPerformed
        {
            add { Pair.SearchPerformed += value; }
            remove { Pair.SearchPerformed -= value; }
        }

        /// <summary>
        /// Gets or sets the background color of the search box.
        /// </summary>
        public Color BackgroundColor
        {
            get { return Pair.BackgroundColor; }
            set { Pair.BackgroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the color of the border around the search box.
        /// </summary>
        public Color BorderColor
        {
            get { return Pair.BorderColor; }
            set { Pair.BorderColor = value; }
        }

        /// <summary>
        /// Gets or sets the color of the text within the search box.
        /// </summary>
        public Color ForegroundColor
        {
            get { return Pair.ForegroundColor; }
            set { Pair.ForegroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the text to display when there is no value in the search box.
        /// </summary>
        public string Placeholder
        {
            get { return Pair.Placeholder; }
            set { Pair.Placeholder = value; }
        }

        /// <summary>
        /// Gets or sets completion behavior for text that is entered into the search box.
        /// Not all platforms support all behaviors.
        /// </summary>
        public TextCompletion TextCompletion
        {
            get { return Pair.TextCompletion; }
            set { Pair.TextCompletion = value; }
        }

        /// <summary>
        /// Gets or sets the text within the search box.
        /// </summary>
        public string Text
        {
            get { return Pair.Text; }
            set { Pair.Text = value; }
        }

        /// <summary>
        /// Gets or sets the native object that is paired with the search box.  This can be set only once.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        protected ISearchBox Pair
        {
            get
            {
                if (pair == null)
                {
                    throw new InvalidOperationException("No native object was found for the current instance.");
                }
                return pair;
            }
            set
            {
                if (pair == null && value != null)
                {
                    pair = value;
                    pair.Pair = this;
                }
            }
        }
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private ISearchBox pair;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        IPairable IPairable.Pair
        {
            get { return Pair; }
            set { Pair = value as ISearchBox; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.SearchBox"/> class.
        /// </summary>
        public SearchBox()
        {
            Pair = MXContainer.Resolve<ISearchBox>();

            pair.BackgroundColor = new Color();
            pair.BorderColor = new Color();
            pair.ForegroundColor = new Color();
        }

        /// <summary>
        /// Programmatically sets input focus to the search box.
        /// </summary>
        public void Focus()
        {
            Pair.Focus();
        }

        // TODO: Uncomment for 3.6
        ///// <summary>
        ///// Performs a default search query filter on the specified <paramref name="value"/> and returns the result.
        ///// </summary>
        ///// <param name="value">The value to be filtered.</param>
        ///// <param name="searchQuery">The query with which to filter the <paramref name="value"/>.</param>
        ///// <returns>The result of the filter.</returns>
        //public static SearchFilterResult FilterValue(string value, string searchQuery)
        //{
        //    if (string.IsNullOrEmpty(searchQuery))
        //    {
        //        return SearchFilterResult.ExplicitInclusion;
        //    }

        //    if (string.IsNullOrEmpty(value))
        //    {
        //        return SearchFilterResult.ImplicitExclusion;
        //    }

        //    searchQuery = searchQuery.ToUpper();
        //    value = value.ToUpper();

        //    for (int i = searchQuery.Length - 1; i >= 0; i--)
        //    {
        //        if (searchQuery[i] == '"' && i > 0)
        //        {
        //            int index = searchQuery.LastIndexOf('"', i - 1);
        //            i = index < 0 ? i : index;
        //            continue;
        //        }

        //        if (searchQuery[i] == '-')
        //        {
        //            if (i == searchQuery.Length - 1)
        //            {
        //                searchQuery = searchQuery.Remove(i).Trim();
        //                i = searchQuery.Length;
        //                continue;
        //            }

        //            bool quoted = searchQuery[i + 1] == '"';
        //            if (quoted && i == searchQuery.Length - 2)
        //            {
        //                searchQuery = searchQuery.Remove(i).Trim();
        //                i = searchQuery.Length;
        //                continue;
        //            }

        //            int startIndex = quoted ? i + 2 : i + 1;
        //            int closingIndex = quoted ? searchQuery.IndexOf('"', startIndex) : searchQuery.IndexOf(' ', startIndex);
        //            if (closingIndex < 0)
        //            {
        //                closingIndex = searchQuery.Length;
        //            }

        //            string subquery = searchQuery.Substring(startIndex, closingIndex - startIndex);
        //            if (subquery.Length > 0)
        //            {
        //                int valueLength = value.Length;
        //                value = value.Replace(subquery, string.Empty);
        //                if (value.Length != valueLength)
        //                {
        //                    return SearchFilterResult.ExplicitExclusion;
        //                }
        //            }

        //            searchQuery = searchQuery.Remove(i, closingIndex - i).Trim();
        //            if (i > searchQuery.Length)
        //            {
        //                i = searchQuery.Length;
        //            }
        //        }
        //    }

        //    if (searchQuery.Length == 0)
        //    {
        //        return SearchFilterResult.ImplicitInclusion;
        //    }

        //    int firstIndex = 0;
        //    for (int i = 0; i < searchQuery.Length; i++)
        //    {
        //        char c = searchQuery[i];
        //        if (c == '"')
        //        {
        //            if (i == searchQuery.Length - 1)
        //            {
        //                searchQuery = searchQuery.Remove(i).Trim();
        //                if (searchQuery.Length == 0)
        //                {
        //                    return SearchFilterResult.ImplicitInclusion;
        //                }
        //                break;
        //            }

        //            int closingIndex = searchQuery.IndexOf('"', ++i);
        //            if (closingIndex < 0)
        //            {
        //                closingIndex = searchQuery.Length;
        //            }

        //            string subquery = searchQuery.Substring(i, closingIndex - i);
        //            i = closingIndex;
        //            firstIndex = i + 1;
                    
        //            if (value.Contains(subquery))
        //            {
        //                return SearchFilterResult.ExplicitInclusion;
        //            }
        //        }

        //        if (c == ' ')
        //        {
        //            if (i == searchQuery.Length - 1)
        //            {
        //                break;
        //            }

        //            string subquery = searchQuery.Substring(firstIndex, i - firstIndex);
        //            firstIndex = i + 1;
                    
        //            if (value.Contains(subquery))
        //            {
        //                return SearchFilterResult.ExplicitInclusion;
        //            }
        //        }
        //    }

        //    if (firstIndex < searchQuery.Length && value.Contains(searchQuery.Substring(firstIndex)))
        //    {
        //        return  SearchFilterResult.ExplicitInclusion;
        //    }

        //    return SearchFilterResult.ImplicitExclusion;
        //}
    }
}
