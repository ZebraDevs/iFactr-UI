using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Represents a UI element that can display a string of read-only text.
    /// </summary>
	public class Label : Control, ILabel
	{
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:Font"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string FontProperty = "Font";

        /// <summary>
        /// The name of the <see cref="P:ForegroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ForegroundColorProperty = "ForegroundColor";

        /// <summary>
        /// The name of the <see cref="P:HighlightColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string HighlightColorProperty = "HighlightColor";

        /// <summary>
        /// The name of the <see cref="P:Lines"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string LinesProperty = "Lines";

        /// <summary>
        /// The name of the <see cref="P:Text"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TextProperty = "Text";

        /// <summary>
        /// The name of the <see cref="P:TextAlignment"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TextAlignmentProperty = "TextAlignment";
        #endregion

        /// <summary>
        /// Gets or sets the font to be used when rendering the text.
        /// </summary>
		public Font Font
		{
			get { return NativeControl.Font; }
			set { NativeControl.Font = value; }
		}

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
		public Color ForegroundColor
		{
			get { return NativeControl.ForegroundColor; }
			set { NativeControl.ForegroundColor = value; }
		}

        /// <summary>
        /// Gets or sets the color of the text when the label is in a cell that is being highlighted.
        /// </summary>
        public Color HighlightColor
        {
            get { return NativeControl.HighlightColor; }
            set { NativeControl.HighlightColor = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of lines of text that the label is allowed to display.
        /// A value equal to or less than 0 means that there is no limit.
        /// </summary>
		public int Lines
		{
			get { return NativeControl.Lines; }
			set { NativeControl.Lines = value; }
		}

        /// <summary>
        /// Gets or sets the text to be rendered.
        /// </summary>
		public string Text
		{
			get { return NativeControl.Text; }
			set { NativeControl.Text = value; }
		}

        /// <summary>
        /// Gets or sets how the text is aligned within the text box.
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return NativeControl.TextAlignment; }
            set { NativeControl.TextAlignment = value; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		private ILabel NativeControl
        {
            get { return (ILabel)Pair; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.Label"/> class.
        /// </summary>
		public Label()
		{
			Pair = MXContainer.Resolve<ILabel>();
            
            NativeControl.Font = Font.PreferredLabelFont;
            NativeControl.ForegroundColor = iApp.Instance.Style.TextColor;
            NativeControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            NativeControl.VerticalAlignment = VerticalAlignment.Stretch;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.Label"/> class.
        /// </summary>
        /// <param name="text">The text to be rendered.</param>
		public Label(string text)
			: this()
		{
			Text = text;
		}
	}
}

