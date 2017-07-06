namespace iFactr.UI
{
    /// <summary>
    /// Describes how text is aligned within the element that it belongs to.
    /// </summary>
    public enum TextAlignment : byte
    {
        /// <summary>
        /// Text is aligned to the left edge of the element.
        /// </summary>
        Left,
        /// <summary>
        /// Text is centered within the element.
        /// </summary>
        Center,
        /// <summary>
        /// Text is aligned to the right edge of the element.
        /// </summary>
        Right,
        /// <summary>
        /// Text is justified within the element so that the lines in a paragraph are evenly aligned.
        /// </summary>
        Justified,
    }
}
