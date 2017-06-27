namespace iFactr.UI.Controls
{
    /// <summary>
    /// Defines a UI element that can display a string of read-only text.
    /// </summary>
    public interface ILabel : IControl
    {
        /// <summary>
        /// Gets or sets the font to be used when rendering the text.
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the text when the label is in a cell that is being highlighted.
        /// </summary>
        Color HighlightColor { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of lines of text that the label is allowed to display.
        /// A value equal to or less than 0 means that there is no limit.
        /// </summary>
        int Lines { get; set; }

        /// <summary>
        /// Gets or sets the text to be rendered.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets how the text is aligned within the label.
        /// </summary>
        TextAlignment TextAlignment { get; set; }
    }
}

