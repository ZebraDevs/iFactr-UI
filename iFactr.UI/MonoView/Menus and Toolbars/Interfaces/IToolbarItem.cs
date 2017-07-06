namespace iFactr.UI
{
    /// <summary>
    /// Defines a base interface for the <see cref="iFactr.UI.IToolbarButton"/> and <see cref="iFactr.UI.IToolbarSeparator"/> interfaces.
    /// </summary>
    public interface IToolbarItem : IPairable
    {
        /// <summary>
        /// Gets or sets the foreground color of the item.
        /// </summary>
        Color ForegroundColor { get; set; }
    }
}
