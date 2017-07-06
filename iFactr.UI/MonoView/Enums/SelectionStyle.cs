namespace iFactr.UI
{
    /// <summary>
    /// Describes the visual elements to use to indicate when a cell is selectable or has been selected.
    /// </summary>
    public enum SelectionStyle : byte
    {
        /// <summary>
        /// No visual indicators of any kind will be used.
        /// </summary>
        None = 0,
        /// <summary>
        /// For platforms that support it, the cell will show a visual element to indicate
        /// that it is selectable.  The cell will not be highlighted when it is selected.
        /// </summary>
        IndicatorOnly = 1,
        /// <summary>
        /// For platforms that support it, the cell will be highlighted when it is selected.
        /// No visual indicators will be rendered to convey that the cell is selectable.
        /// </summary>
        HighlightOnly = 2,
        /// <summary>
        /// For platforms that support it, the cell will show a visual element to
        /// indicate that it is selectable and will be highlighted when it is selected.
        /// </summary>
        Default = 3,
    }
}
