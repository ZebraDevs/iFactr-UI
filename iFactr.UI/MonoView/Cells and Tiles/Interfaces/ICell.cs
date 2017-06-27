using System;

namespace iFactr.UI
{
    /// <summary>
    /// Defines an entry in an <see cref="iFactr.UI.IListView"/> instance.  This is the base interface for
    /// the <see cref="iFactr.UI.IGridCell"/> and <see cref="iFactr.UI.IRichContentCell"/> interfaces.
    /// </summary>
    public interface ICell : IPairable, IEquatable<ICell>
    {
        /// <summary>
        /// Gets or sets the background color of the cell.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of vertical space that the cell is allowed to consume.
        /// </summary>
        double MaxHeight { get; set; }

        /// <summary>
        /// Gets a collection for storing arbitrary data on the object.
        /// </summary>
        MetadataCollection Metadata { get; }

        /// <summary>
        /// Gets or sets the minimum amount of vertical space that the cell is allowed to consume.
        /// </summary>
        double MinHeight { get; set; }
    }
}

