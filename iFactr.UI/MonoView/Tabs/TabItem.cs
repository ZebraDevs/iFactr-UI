using System;
using System.Diagnostics;

using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a selectable item within an <see cref="iFactr.UI.ITabView"/>.
    /// </summary>
    public class TabItem : ITabItem
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:BadgeValue"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string BadgeValueProperty = "BadgeValue";

        /// <summary>
        /// The name of the <see cref="P:ImagePath"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ImagePathProperty = "ImagePath";

        /// <summary>
        /// The name of the <see cref="P:NavigationLink"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string NavigationLinkProperty = "NavigationLink";

        /// <summary>
        /// The name of the <see cref="P:Title"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TitleProperty = "Title";

        /// <summary>
        /// The name of the <see cref="P:TitleColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TitleColorProperty = "TitleColor";
        
        /// <summary>
        /// The name of the <see cref="P:TitleFont"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TitleFontProperty = "TitleFont";
        #endregion

        /// <summary>
        /// Occurs when the item is selected by the user.
        /// </summary>
        public event EventHandler Selected
        {
            add { Pair.Selected += value; }
            remove { Pair.Selected -= value; }
        }

        /// <summary>
        /// Gets or sets any auxiliary information to display alongside the item.
        /// </summary>
        public string BadgeValue
        {
            get { return Pair.BadgeValue; }
            set { Pair.BadgeValue = value; }
        }

        /// <summary>
        /// Gets or sets the file path of the image to use for the item.
        /// </summary>
        public string ImagePath
        {
            get { return Pair.ImagePath; }
            set { Pair.ImagePath = value; }
        }

        /// <summary>
        /// Gets or sets the link to navigate to when the item is selected and the view stack associated with the item is empty.
        /// The navigation only occurs if there is no handler for the <see cref="Selected"/> event.
        /// </summary>
        public Link NavigationLink
        {
            get { return Pair.NavigationLink; }
            set { Pair.NavigationLink = value; }
        }

        /// <summary>
        /// Gets or sets the title for the item.
        /// </summary>
        public string Title
        {
            get { return Pair.Title; }
            set { Pair.Title = value; }
        }

        /// <summary>
        /// Gets or sets the color with which to display the title.
        /// </summary>
        public Color TitleColor
        {
            get { return Pair.TitleColor; }
            set { Pair.TitleColor = value; }
        }

        /// <summary>
        /// Gets or sets the font to be used when rendering the title.
        /// </summary>
        public Font TitleFont
        {
            get { return Pair.TitleFont; }
            set { Pair.TitleFont = value; }
        }

        /// <summary>
        /// Gets or sets the native object that is paired with the toolbar.  This can be set only once.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        protected ITabItem Pair
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
        private ITabItem pair;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        IPairable IPairable.Pair
        {
            get { return Pair; }
            set { Pair = value as ITabItem; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.TabItem"/> class.
        /// </summary>
        public TabItem()
        {
            Pair = MXContainer.Resolve<ITabItem>();

            pair.TitleColor = new Color();
            pair.TitleFont = Font.PreferredTabFont;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.TabItem"/> class.
        /// </summary>
        /// <param name="title">The title for the item.</param>
        /// <param name="navigationLink">The link to navigate to when the item is selected.</param>
        public TabItem(string title, Link navigationLink)
            : this()
        {
            Title = title;
            NavigationLink = navigationLink;
        }

        /// <summary>
        /// Determines whether the specified <see cref="iFactr.UI.ITabItem"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="iFactr.UI.ITabItem"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="iFactr.UI.ITabItem"/> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(ITabItem other)
        {
            TabItem item = other as TabItem;
            if (item != null)
            {
                return Pair == item.Pair;
            }
            
            return Pair == other;
        }
    }
}

