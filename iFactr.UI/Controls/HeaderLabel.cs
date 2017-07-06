using iFactr.Core.Styles;

namespace iFactr.Core.Controls
{
    /// <summary>
    /// Represents a Header Label control.
    /// </summary>
    public class HeaderLabel : Label
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderLabel"/> class.
        /// </summary>
        public HeaderLabel() { Style.HeaderLevel = 1; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderLabel"/> class.
        /// </summary>
        /// <param name="style">The <see cref="Style" /> instance to style this object with.</param>
        public HeaderLabel(Style style) : base(style) { Style.HeaderLevel = 1; }
    }
}