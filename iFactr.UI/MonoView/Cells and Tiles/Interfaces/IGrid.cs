using System.Collections.Generic;
using iFactr.UI.Controls;

namespace iFactr.UI
{
    /// <summary>
    /// Defines an object that acts as a grid made up of columns and rows for laying out various UI elements.
    /// </summary>
    public interface IGrid
    {
        /// <summary>
        /// Gets a collection of the columns that currently make up the grid.
        /// </summary>
        ColumnCollection Columns { get; }

        /// <summary>
        /// Gets a collection of the UI elements that currently reside within the grid.
        /// </summary>
        IEnumerable<IElement> Children { get; }

        /// <summary>
        /// Gets or sets the amount of spacing between the edges of the grid and the children within it.
        /// </summary>
        Thickness Padding { get; set; }

        /// <summary>
        /// Gets a collection of the rows that currently make up the grid.
        /// </summary>
        RowCollection Rows { get; }

        /// <summary>
        /// Adds the specified <see cref="IElement"/> instance to the grid.
        /// </summary>
        /// <param name="element">The element to be added to the grid.</param>
        void AddChild(IElement element);

        /// <summary>
        /// Removes the specified <see cref="IElement"/> instance from the grid.
        /// </summary>
        /// <param name="element">The element to be removed from the grid.</param>
        void RemoveChild(IElement element);
    }
}
