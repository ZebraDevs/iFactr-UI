using iFactr.Core.Controls;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// This class represents a tab.
    /// </summary>
    public class Tab : iItem
    {
        /// <summary>
        /// Gets a value indicating whether this instance should
        /// perform a navigation to the root layer whenever it is selected.
        /// </summary>
        /// <value><c>true</c> if this tab refreshes on focus; otherwise <c>false</c>.</value>
        public bool RefreshOnFocus { get; set; }

        /// <summary>
        /// Gets or sets the text of the badge displayed over the tab.
        /// If no text is set, the badge is not displayed.
        /// </summary>
        public string Badge
        {
            get { return Subtext; }
            set { Subtext = value; if (TabBadgeChanged != null) TabBadgeChanged(Subtext); }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Tab"/> class.
        /// </summary>
        public Tab() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tab"/> class.
        /// </summary>
        /// <param name="title">The label to display on the tab.</param>
        /// <param name="navigationUrl">The URL to navigate to when the tab is selected for the first time.</param>
        public Tab(string title, string navigationUrl)
            : base(navigationUrl, title, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tab"/> class.
        /// </summary>
        /// <param name="title">The label to display on the tab.</param>
        /// <param name="navigationUrl">The URL to navigate to when the tab is selected for the first time.</param>
        /// <param name="refreshOnFocus">Whether to navigate to the navigationUrl every time the tab is selected.</param>
        public Tab(string title, string navigationUrl, bool refreshOnFocus)
            : base(navigationUrl, title, null) { RefreshOnFocus = refreshOnFocus; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tab"/> class.
        /// </summary>
        /// <param name="title">The label to display on the tab.</param>
        /// <param name="navigationUrl">The URL to navigate to when the tab is selected for the first time.</param>
        /// <param name="icon">The path of the icon to use.</param>
        public Tab(string title, string navigationUrl, string icon)
            : this(title, navigationUrl, icon, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tab"/> class.
        /// </summary>
        /// <param name="title">The label to display on the tab.</param>
        /// <param name="navigationUrl">The URL to navigate to when the tab is selected for the first time.</param>
        /// <param name="icon">The path of the icon to use.</param>
        /// <param name="refreshOnFocus">Whether to navigate to the navigationUrl every time the tab is selected.</param>
        public Tab(string title, string navigationUrl, string icon, bool refreshOnFocus)
            : this(title, navigationUrl, icon, null) { RefreshOnFocus = refreshOnFocus; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tab"/> class.
        /// </summary>
        /// <param name="title">The label to display on the tab.</param>
        /// <param name="navigationUrl">The URL to navigate to when the tab is selected for the first time.</param>
        /// <param name="icon">The path of the icon to use.</param>
        /// <param name="badge">The text to display in the badge.  If null, the badge will not display.</param>
        public Tab(string title, string navigationUrl, string icon, string badge)
            : base(navigationUrl, title, badge)
        {
            Icon = string.IsNullOrEmpty(icon) ? null : new Icon() { Location = icon };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tab"/> class.
        /// </summary>
        /// <param name="title">The label to display on the tab.</param>
        /// <param name="navigationUrl">The URL to navigate to when the tab is selected for the first time.</param>
        /// <param name="icon">The path of the icon to use.</param>
        /// <param name="badge">The text to display in the badge.  If null, the badge will not display.</param>
        /// <param name="refreshOnFocus">Whether to navigate to the navigationUrl every time the tab is selected.</param>
        public Tab(string title, string navigationUrl, string icon, string badge, bool refreshOnFocus)
            : base(navigationUrl, title, badge)
        {
            Icon = string.IsNullOrEmpty(icon) ? null : new Icon() { Location = icon };
            RefreshOnFocus = refreshOnFocus;
        }

        #endregion

        /// <summary>
        /// Represents the method that will handle badge changed events.
        /// </summary>
        /// <param name="value">The new value of the tab's badge text.</param>
        public delegate void TabBadgeChangedDelegate(string value);

        /// <summary>
        /// Occurs when the tab badge is changed.
        /// </summary>
        public event TabBadgeChangedDelegate TabBadgeChanged;
    }
}