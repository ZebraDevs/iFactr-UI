namespace iFactr.UI
{
    /// <summary>
    /// Describes the method in which an <see cref="IListView"/> instance should lay out its contents.
    /// </summary>
    public enum ColumnMode : byte
    {
        /// <summary>
        /// The view should place everything within a single column.
        /// </summary>
        OneColumn = 0,
        /// <summary>
        /// The view should arrange its contents within two columns.  Sections will alternate between each column.
        /// Not all platforms support this.
        /// </summary>
        TwoColumns = 1
    }
}
