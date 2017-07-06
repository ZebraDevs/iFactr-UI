using System;
using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a set of items that provide support functions for an
    /// <see cref="iFactr.UI.IListView"/> or <see cref="iFactr.UI.IBrowserView"/>.
    /// </summary>
    public class Menu : IMenu
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:BackgroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string BackgroundColorProperty = "BackgroundColor";

        /// <summary>
        /// The name of the <see cref="P:ForegroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ForegroundColorProperty = "ForegroundColor";

        /// <summary>
        /// The name of the <see cref="P:ImagePath"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ImagePathProperty = "ImagePath";

        /// <summary>
        /// The name of the <see cref="P:SelectionColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string SelectionColorProperty = "SelectionColor";

        /// <summary>
        /// The name of the <see cref="P:Title"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TitleProperty = "Title";

        /// <summary>
        /// The name of the <see cref="P:ButtonCount"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ButtonCountProperty = "ButtonCount";
        #endregion

        /// <summary>
        /// Gets or sets the background color of the menu on supported platforms.
        /// </summary>
        public Color BackgroundColor
        {
            get { return Pair.BackgroundColor; }
            set { Pair.BackgroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the foreground color of the menu on supported platforms.
        /// </summary>
        public Color ForegroundColor
        {
            get { return Pair.ForegroundColor; }
            set { Pair.ForegroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the file path of the image to use for the button that activates the menu.
        /// Not all platforms have this button available.
        /// </summary>
        public string ImagePath
        {
            get { return Pair.ImagePath; }
            set { Pair.ImagePath = value; }
        }

        /// <summary>
        /// Gets or sets the color to highlight the menu when it is selected.
        /// </summary>
        public Color SelectionColor
        {
            get { return Pair.SelectionColor; }
            set { Pair.SelectionColor = value; }
        }

        /// <summary>
        /// Gets or sets the title of the button that activates the menu.
        /// Not all platforms have this button available.
        /// </summary>
        public string Title
        {
            get { return Pair.Title; }
            set { Pair.Title = value; }
        }

		/// <summary>
		/// Gets the number of menu buttons that are currently on the menu.
		/// </summary>
		public int ButtonCount
		{
			get { return Pair.ButtonCount; }
		}

        /// <summary>
        /// Gets or sets the native object that is paired with the menu.  This can be set only once.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        protected IMenu Pair
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
        private IMenu pair;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        IPairable IPairable.Pair
        {
            get { return Pair; }
            set { Pair = value as IMenu; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Menu"/> class.
        /// </summary>
        public Menu()
        {
            Pair = MXContainer.Resolve<IMenu>();

            pair.BackgroundColor = iApp.Instance.Style.HeaderColor;
            pair.ForegroundColor = iApp.Instance.Style.HeaderTextColor;
            pair.SelectionColor = new Color();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Menu"/> class.
        /// </summary>
        /// <param name="menuButtons">The buttons to add to the menu.</param>
        public Menu(params IMenuButton[] menuButtons)
            : this()
        {
            if (menuButtons != null)
            {
                foreach (var item in menuButtons)
                {
                    Add(item);
                }
            }
        }

        /// <summary>
        /// Adds the specified <see cref="iFactr.UI.IMenuButton"/> to the menu.
        /// </summary>
        /// <param name="menuButton">The button to add.</param>
        public void Add(IMenuButton menuButton)
        {
            Pair.Add(menuButton);
        }

		/// <summary>
		/// Returns the <see cref="iFactr.UI.IMenuButton"/> at the specified index.
		/// </summary>
		/// <param name="index">The index of the button to return.</param>
		/// <returns>The <see cref="iFactr.UI.IMenuButton"/> at the specified index.</returns>
		/// <exception cref="IndexOutOfRangeException">Thrown when the index equals or exceeds the
		/// number of buttons in the menu -or- when the index is less than 0.</exception>
		public IMenuButton GetButton(int index)
		{
			if (index < 0 || index >= ButtonCount)
			{
				throw new IndexOutOfRangeException();
			}
			return Pair.GetButton(index);
		}

        /// <summary>
        /// Determines whether the specified <see cref="iFactr.UI.IMenu"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="iFactr.UI.IMenu"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="iFactr.UI.IMenu"/> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(IMenu other)
        {
            Menu menu = other as Menu;
            if (menu != null)
            {
                return Pair == menu.Pair;
            }
            
            return Pair == other;
        }
    }
}

