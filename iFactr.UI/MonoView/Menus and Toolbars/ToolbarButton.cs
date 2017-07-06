using System;
using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a clickable button in an <see cref="iFactr.UI.IToolbar"/> object.
    /// </summary>
    public class ToolbarButton : IToolbarButton
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:ForegroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ForegroundColorProperty = "ForegroundColor";

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
        #endregion

        /// <summary>
        /// Occurs when the user clicks or taps on the button.
        /// </summary>
        public event EventHandler Clicked
        {
            add { Pair.Clicked += value; }
            remove { Pair.Clicked -= value; }
        }

        /// <summary>
        /// Gets or sets the foreground color of the button.
        /// </summary>
        public Color ForegroundColor
        {
            get { return Pair.ForegroundColor; }
            set { Pair.ForegroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the file path to an image to display as part of the button.
        /// </summary>
        public string ImagePath
        {
            get { return Pair.ImagePath; }
            set { Pair.ImagePath = value; }
        }

        /// <summary>
        /// Gets or sets the link to navigate to when the user clicks or taps the button.
        /// The navigation only occurs if there is no handler for the <see cref="Clicked"/> event.
        /// </summary>
        public Link NavigationLink
        {
            get { return Pair.NavigationLink; }
            set { Pair.NavigationLink = value; }
        }

        /// <summary>
        /// Gets or sets the title of the button.
        /// </summary>
        public string Title
        {
            get { return Pair.Title; }
            set { Pair.Title = value; }
        }

        /// <summary>
        /// Gets or sets the native object that is paired with the button.  This can be set only once.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        protected IToolbarButton Pair
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
        private IToolbarButton pair;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        IPairable IPairable.Pair
        {
            get { return Pair; }
            set { Pair = value as IToolbarButton; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.ToolbarButton"/> class.
        /// </summary>
        public ToolbarButton()
        {
            Pair = MXContainer.Resolve<IToolbarButton>();

            pair.ForegroundColor = new Color();
        }

        /// <summary>
        /// Determines whether the specified <see cref="iFactr.UI.IToolbarButton"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="iFactr.UI.IToolbarButton"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="iFactr.UI.IToolbarButton"/> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(IToolbarButton other)
        {
            ToolbarButton button = other as ToolbarButton;
            if (button != null)
            {
                return Pair == button.Pair;
            }

            return Pair == other;
        }
    }
}
