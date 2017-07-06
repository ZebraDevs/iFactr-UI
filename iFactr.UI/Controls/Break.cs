using iFactr.Core.Styles;

namespace iFactr.Core.Controls
{
    /// <summary>
    /// A <see cref="IBlockPanelItem"/> for rendering line breaks and horizontal rules.
    /// </summary>
    public class Break : Label
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Break"/> class.
        /// </summary>
        public Break() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Break"/> class.
        /// </summary>
        /// <param name="style">The <see cref="Style" /> instance to style this object with.</param>
        public Break(Style style) : base(style) { }

        /// <summary>
        /// Gets or sets a value indicating whether to render a horizontal rule.
        /// </summary>
        /// <value>
        /// <c>true</c> to render a horizontal rule; otherwise, <c>false</c> to render a normal line break.
        /// </value>
        public bool IsHorizontalRule { get; set; }

        /// <summary>
        /// Returns an HTML representation of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> representing this instance in HTML.
        /// </returns>
        public override string GetHtml()
        {
            return IsHorizontalRule ? "<hr/>" : "<br/>";
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new Break Clone()
        {
            return (Break)CloneOverride();
        }
    }
}