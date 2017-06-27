using iFactr.Core.Layers;

namespace iFactr.UI
{
    /// <summary>
    /// Defines a cell that parses HTML or XML into rich content.  Since not all platforms support HTML rendering
    /// or XML rendering, it is advised to only use the various Append methods to create the content.
    /// </summary>
    public interface IRichContentCell : ICell, IHtmlText
    {
        /// <summary>
        /// Gets or sets the foreground color of the cell.
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Begins parsing the content in the Text property.
        /// </summary>
        void Load();
    }
}
