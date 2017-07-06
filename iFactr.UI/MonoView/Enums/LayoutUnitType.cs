namespace iFactr.UI
{
    /// <summary>
    /// Describes the kind of value that a column or row is being sized with.
    /// </summary>
    public enum LayoutUnitType : byte
    {
        /// <summary>
        /// The column or row is automatically sized to fit its contents.  The value is ignored.
        /// </summary>
        Auto = 0,
        /// <summary>
        /// The value represents units in the platform's coordinate system (pixels, points, etc).
        /// This unit type should be avoided whenever possible to ensure a consistent experience across all platforms.
        /// </summary>
        Absolute = 1,
        /// <summary>
        /// The value represents a weighted proportion of the available space after all other columns or rows have been sized.
        /// </summary>
        Star = 2
    }
}
