using System.Collections;
using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Represents a UI element that lets the user make a single selection from a list of items.
    /// </summary>
	public class SelectList : Control, ISelectList
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
        /// The name of the <see cref="P:Items"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ItemsProperty = "Items";

        /// <summary>
        /// The name of the <see cref="P:SelectedIndex"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string SelectedIndexProperty = "SelectedIndex";

        /// <summary>
        /// The name of the <see cref="P:SelectedItem"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string SelectedItemProperty = "SelectedItem";
        #endregion

        /// <summary>
        /// Occurs when the selected item has changed.
        /// </summary>
		[NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
		public event ValueChangedEventHandler<object> SelectionChanged
		{
			add { NativeControl.SelectionChanged += value; }
			remove { NativeControl.SelectionChanged -= value; }
		}
		
        /// <summary>
        /// Gets or sets the background color of the select list.
        /// </summary>
		public Color BackgroundColor
		{
			get { return NativeControl.BackgroundColor; }
			set { NativeControl.BackgroundColor = value; }
		}
		
        /// <summary>
        /// Gets or sets the font to be used when rendering each item in the list.
        /// </summary>
		public Font Font
		{
			get { return NativeControl.Font; }
			set { NativeControl.Font = value; }
		}
		
        /// <summary>
        /// Gets or sets the color of each item in the list.
        /// </summary>
		public Color ForegroundColor
		{
			get { return NativeControl.ForegroundColor; }
			set { NativeControl.ForegroundColor = value; }
		}
		
        /// <summary>
        /// Gets or sets a collection of the items that are to be presented in the select list.
        /// The select list will render the string that is returned by each item's ToString method.
        /// </summary>
		public IEnumerable Items
		{
			get { return NativeControl.Items; }
			set { NativeControl.Items = value; }
		}
		
        /// <summary>
        /// Gets or sets the index of the selected item.
        /// </summary>
		public int SelectedIndex
		{
			get { return NativeControl.SelectedIndex; }
			set { NativeControl.SelectedIndex = value; }
		}
		
        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
		public object SelectedItem
		{
			get { return NativeControl.SelectedItem; }
			set { NativeControl.SelectedItem = value; }
		}

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		private ISelectList NativeControl
        {
            get { return (ISelectList)Pair; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.SelectList"/> class.
        /// </summary>
		public SelectList()
		{
			Pair = MXContainer.Resolve<ISelectList>();

            NativeControl.BackgroundColor = new Color();
            NativeControl.Font = Font.PreferredSelectListFont;
            NativeControl.ForegroundColor = new Color();
            NativeControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            NativeControl.VerticalAlignment = VerticalAlignment.Top;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.SelectList"/> class.
        /// </summary>
        /// <param name="items">A collection containing the items that are to be presented in the list.</param>
        public SelectList(IEnumerable items)
            : this()
        {
            Items = items;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.SelectList"/> class.
        /// </summary>
        /// <param name="items">The items that are to be presented in the list.</param>
		public SelectList(params object[] items)
			: this()
		{
			Items = items;
		}

        /// <summary>
        /// Programmatically presents the list to the user.
        /// </summary>
        public void ShowList()
        {
            NativeControl.ShowList();
        }
	}
}

