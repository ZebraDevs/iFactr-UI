namespace iFactr.UI
{
    /// <summary>
    /// Describes the style in which an <see cref="iFactr.UI.IListView"/> instance should be rendered.
    /// </summary>
    public enum ListViewStyle : byte
    {
        /// <summary>
        /// The view should be rendered in the default style for each platform.
        /// </summary>
        Default = 0,
        /// <summary>
        /// The view should render its sections as groups.  Not all platforms support this.
        /// </summary>
        Grouped = 1
    }
}
