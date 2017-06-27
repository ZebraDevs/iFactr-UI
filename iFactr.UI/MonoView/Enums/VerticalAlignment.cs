namespace iFactr.UI
{
    /// <summary>
    /// Describes how an element is vertically aligned within the space that is allotted for it.
    /// </summary>
    public enum VerticalAlignment : byte
    {
        /// <summary>
        /// The element is aligned to the top edge of the space.
        /// </summary>
        Top = 0,
        /// <summary>
        /// The element is centered within the space.
        /// </summary>
        Center = 1,
        /// <summary>
        /// The element is aligned to the bottom edge of the space.
        /// </summary>
        Bottom = 2,
        /// <summary>
        /// The element is stretched to fill the space.
        /// </summary>
        Stretch = 3
    }
}
