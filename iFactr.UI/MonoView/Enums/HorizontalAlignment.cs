namespace iFactr.UI
{
    /// <summary>
    /// Describes how an element is horizontally aligned within the space that is allotted for it.
    /// </summary>
    public enum HorizontalAlignment : byte
    {
        /// <summary>
        /// The element is aligned to the left edge of the space.
        /// </summary>
        Left = 0,
        /// <summary>
        /// The element is centered within the space.
        /// </summary>
        Center = 1,
        /// <summary>
        /// The element is aligned to the right edge of the space.
        /// </summary>
        Right = 2,
        /// <summary>
        /// The element is stretched to fill the space.
        /// </summary>
        Stretch = 3
    }
}
