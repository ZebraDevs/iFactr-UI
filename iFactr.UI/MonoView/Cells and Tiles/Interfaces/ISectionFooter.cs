namespace iFactr.UI
{
    /// <summary>
    /// Defines a UI element that acts as a footer for a <see cref="Section"/>.
    /// </summary>
    public interface ISectionFooter : IPairable
    {
        /// <summary>
        /// Gets or sets the background color of the footer.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the foreground content of the footer.
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the font to be used when rendering the text.
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets the text that will be displayed on screen.
        /// </summary>
        string Text { get; set; }
    }
}
