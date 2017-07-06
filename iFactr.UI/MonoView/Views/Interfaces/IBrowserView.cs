using System;
using MonoCross.Navigation;

namespace iFactr.UI
{
	/// <summary>
	/// Defines a native view that supports HTML rendering and web page browsing.
	/// </summary>
    public interface IBrowserView : IView, IHistoryEntry
    {
        /// <summary>
        /// Occurs when the page has been loaded.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event EventHandler<LoadFinishedEventArgs> LoadFinished;

        /// <summary>
        /// Gets a value indicating whether the browser can navigate back a page in the browsing history.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// Gets a value indicating whether the browser can navigate forward a page in the browsing history.
        /// </summary>
        bool CanGoForward { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the view should automatically create 'Back' and 'Forward' buttons.
        /// </summary>
        bool EnableDefaultControls { get; set; }

        /// <summary>
        /// Gets or sets a menu of selectable items that provide support functions for the view.
        /// </summary>
        IMenu Menu { get; set; }

		/// <summary>
		/// Navigates back a page in the browser's history.
		/// </summary>
        void GoBack();

		/// <summary>
		/// Navigates forward a page in the browser's history.
		/// </summary>
        void GoForward();

		/// <summary>
		/// Launches an external browser application and navigates to the specified URL.
		/// </summary>
		/// <param name="url">The URL to navigate to.</param>
        void LaunchExternal(string url);

		/// <summary>
		/// Loads the specified URL in the browser.
		/// </summary>
		/// <param name="url">The URL to load.</param>
        void Load(string url);

        /// <summary>
        /// Reads the specified string as HTML and loads the result in the browser.
        /// </summary>
        /// <param name="html">The HTML to load.</param>
        void LoadFromString(string html);

        /// <summary>
        /// Refreshes the contents of the browser.
        /// </summary>
        void Refresh();
    }

    /// <summary>
    /// Defines a native view that supports HTML rendering and web page browsing.
    /// </summary>
    /// <typeparam name="T">The type of the Model.</typeparam>
    public interface IBrowserView<T> : IBrowserView, IMXView<T> { }
}