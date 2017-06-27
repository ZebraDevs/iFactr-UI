using System.Collections.Generic;
using iFactr.UI.Controls;

namespace iFactr.UI.Controls
{
//    /// <summary>
//    /// Defines an object that acts as a grid made up of columns and rows for laying out various UI elements.
//    /// </summary>
//    public interface IGrid : IGridBase, IElement
//    {
//    }
}

namespace iFactr.UI
{
    /// <summary>
    /// Defines a common base interface for all grid objects.
    /// </summary>
    /// At some point, the IGridView and IGridCell interfaces will no longer be necessary.
    /// Once they are gone, this base interface will not be necessary either.
    public interface IGridBase : IElementHost
    {
        /// <summary>
        /// Gets a collection of the columns that currently make up the grid.
        /// </summary>
        ColumnCollection Columns { get; }

        /// <summary>
        /// Gets or sets the amount of spacing between the edges of the grid and the children within it.
        /// </summary>
        Thickness Padding { get; set; }

        /// <summary>
        /// Gets a collection of the rows that currently make up the grid.
        /// </summary>
        RowCollection Rows { get; }
    }
}
