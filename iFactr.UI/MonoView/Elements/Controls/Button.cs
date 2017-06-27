using System;
using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Represents a UI element that can perform application-defined behavior when the user clicks or taps on it.
    /// </summary>
	public class Button : Control, IButton
	{
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:BackgroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string BackgroundColorProperty = "BackgroundColor";

        /// <summary>
        /// The name of the <see cref="P:Font"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string FontProperty = "Font";

        /// <summary>
        /// The name of the <see cref="P:ForegroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ForegroundColorProperty = "ForegroundColor";

        /// <summary>
        /// The name of the <see cref="P:Image"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ImageProperty = "Image";

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
			add { NativeControl.Clicked += value; }
			remove { NativeControl.Clicked -= value; }
		}

        /// <summary>
        /// Gets or sets the background color of the button.
        /// </summary>
		public Color BackgroundColor
		{
			get { return NativeControl.BackgroundColor; }
			set { NativeControl.BackgroundColor = value; }
		}
		
        /// <summary>
        /// Gets or sets the font to be used when rendering the button's title.
        /// </summary>
		public Font Font
		{
			get { return NativeControl.Font; }
			set { NativeControl.Font = value; }
		}
		
        /// <summary>
        /// Gets or sets the color of the button's foreground content.
        /// </summary>
		public Color ForegroundColor
		{
			get { return NativeControl.ForegroundColor; }
			set { NativeControl.ForegroundColor = value; }
		}
		
        /// <summary>
        /// Gets or sets an image to display with the button.
        /// </summary>
		public IImage Image
		{
			get { return NativeControl.Image; }
			set { NativeControl.Image = value; }
		}

        /// <summary>
        /// Gets or sets the link to navigate to when the button is selected.
        /// The navigation only occurs if there is no handler for the <see cref="Clicked"/> event.
        /// </summary>
		public Link NavigationLink
		{
			get { return NativeControl.NavigationLink; }
			set { NativeControl.NavigationLink = value; }
		}
		
        /// <summary>
        /// Gets or sets the title of the button.
        /// </summary>
		public string Title
		{
			get { return NativeControl.Title; }
			set { NativeControl.Title = value; }
		}

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private IButton NativeControl
        {
            get { return (IButton)Pair; }
        }
		
        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.Button"/> class.
        /// </summary>
		public Button()
		{
			Pair = MXContainer.Resolve<IButton>();

            NativeControl.BackgroundColor = new Color();
            NativeControl.ForegroundColor = iApp.Instance.Style.TextColor;
            NativeControl.Font = Font.PreferredButtonFont;
            NativeControl.HorizontalAlignment = HorizontalAlignment.Left;
            NativeControl.VerticalAlignment = VerticalAlignment.Top;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.Button"/> class.
        /// </summary>
        /// <param name="title">The title to display with the button.</param>
		public Button(string title)
			: this()
		{
			Title = title;
		}
	}
}

