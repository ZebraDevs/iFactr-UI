using System;
using System.Diagnostics;

using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a pressable button within an <see cref="iFactr.UI.IMenu"/> object.
    /// </summary>
    public class MenuButton : IMenuButton
    {
        #region Property Names
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
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        public event EventHandler Clicked
        {
            add { Pair.Clicked += value; }
            remove { Pair.Clicked -= value; }
        }

        /// <summary>
        /// Gets the title of the button.
        /// </summary>
        public string Title
        {
            get { return Pair.Title; }
        }
        
        /// <summary>
        /// Gets or sets the file path of the image to use with the button.
        /// </summary>
        public string ImagePath
        {
            get { return Pair.ImagePath; }
            set { Pair.ImagePath = value; }
        }
        
        /// <summary>
        /// Gets or sets the link to navigate to when the button is selected.
        /// The navigation only occurs if there is no handler for the <see cref="Clicked"/> event.
        /// </summary>
        public Link NavigationLink
        {
            get { return Pair.NavigationLink; }
            set { Pair.NavigationLink = value; }
        }

        /// <summary>
        /// Gets or sets the native object that is paired with the menu button.  This can be set only once.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        protected IMenuButton Pair
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
        private IMenuButton pair;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        IPairable IPairable.Pair
        {
            get { return Pair; }
            set { Pair = value as IMenuButton; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.MenuButton"/> class.
        /// </summary>
		/// <param name="title">The title to display with the button.</param>
        public MenuButton(string title)
        {
            Pair = MXContainer.Resolve<IMenuButton>(null, title);
        }

        /// <summary>
        /// Determines whether the specified <see cref="iFactr.UI.IMenuButton"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="iFactr.UI.IMenuButton"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="iFactr.UI.IMenuButton"/> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(IMenuButton other)
        {
            MenuButton item = other as MenuButton;
            if (item != null)
            {
                return Pair == item.Pair;
            }
            
            return Pair == other;
        }
    }
}

