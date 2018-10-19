using System;
using System.Collections.Generic;
using System.Linq;
using iFactr.Core;
using iFactr.UI.Controls;
using iFactr.UI.Instructions;

namespace iFactr.UI
{
    /// <summary>
    /// Provides methods for <see cref="IGridBase"/> objects.
    /// </summary>
    public static class GridExtensions
    {
        /// <summary>
        /// Resets the invocation list of all events within the class.
        /// </summary>
        /// <param name="cell">The cell object.</param>
        /// <param name="includeChildren">Whether or not to also nullify the events of any child elements.</param>
        public static void NullifyEvents(this IGridCell cell, bool includeChildren)
        {
            cell.NullifyEvents();
            if (includeChildren)
            {
                foreach (var control in cell.Children.OfType<IControl>())
                {
                    control.NullifyEvents();
                }
            }
        }

        /// <summary>
        /// Clears the columns of the grid and adds a number of auto-sized columns specified by <paramref name="count"/>.
        /// </summary>
        /// <param name="grid">The grid object.</param>
        /// <param name="count">The number of columns to add to the grid.</param>
        public static void SetColumns(this IGridBase grid, int count)
        {
            grid.Columns.Clear();
            for (int i = 0; i < count; i++)
            {
                grid.Columns.Add(Column.AutoSized);
            }
        }

        /// <summary>
        /// Clears the rows of the grid and adds a number of auto-sized rows specified by <paramref name="count"/>.
        /// </summary>
        /// <param name="grid">The grid object.</param>
        /// <param name="count">The number of rows to add to the grid.</param>
        public static void SetRows(this IGridBase grid, int count)
        {
            grid.Rows.Clear();
            for (int i = 0; i < count; i++)
            {
                grid.Rows.Add(Row.AutoSized);
            }
        }

        /// <summary>
        /// Positions and sizes the children of a grid using the grid's columns and rows.
        /// </summary>
        /// <param name="grid">The grid object.</param>
        /// <param name="minimumSize">The minimum size of the area in which the children are placed.</param>
        /// <param name="maximumSize">The maximum size of the area in which the children are placed.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="grid"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="minimumSize"/> or <paramref name="maximumSize"/> contains a negative value.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="minimumSize"/> has an infinite width or height -or- when the width of <paramref name="maximumSize"/> is infinite and the grid contains at least one star-sized column -or- when the height of <paramref name="maximumSize"/> is infinite and the grid contains at least one star-sized row.</exception>
        public static Size PerformLayout(this IGridBase grid, Size minimumSize, Size maximumSize)
        {
            if (grid == null)
            {
                throw new ArgumentNullException("grid");
            }

            #region ILayoutInstruction layout preprocessors

            var instruction = grid as ILayoutInstruction;
            if (instruction == null)
            {
                var pairable = grid as IPairable;
                if (pairable != null)
                {
                    instruction = pairable.Pair as ILayoutInstruction;
                }
            }

            if (instruction != null)
            {
                iApp.Factory.Instructor.Layout(instruction);

                IPairable pair = null;
                var pairable = grid as IPairable;
                if (pairable != null)
                {
                    pair = pairable.Pair;
                }

                var cell = grid as IGridCell ?? pair as IGridCell;
                if (cell != null)
                {
                    minimumSize.Height = cell.MinHeight;
                    maximumSize.Height = cell.MaxHeight;
                }
            }

            #endregion

            maximumSize.Width = Math.Max(maximumSize.Width, minimumSize.Width);
            maximumSize.Height = Math.Max(maximumSize.Height, minimumSize.Height);

            if (double.IsInfinity(minimumSize.Width) || double.IsInfinity(minimumSize.Height))
            {
                throw new ArgumentException("Minimum size must not have an infinite width or height.", "minimumSize");
            }

            if (minimumSize.Width < 0 || minimumSize.Height < 0)
            {
                throw new ArgumentOutOfRangeException("minimumSize", "The given size must not contain any negative values.");
            }

            if (maximumSize.Width < 0 || maximumSize.Height < 0)
            {
                throw new ArgumentOutOfRangeException("maximumSize", "The given size must not contain any negative values.");
            }

            // if the grid has no rows or columns, provide one
            var rows = grid.Rows.Count == 0 ? new List<Row> { new Row(1, LayoutUnitType.Auto) } : grid.Rows.ToList();
            var columns = grid.Columns.Count == 0 ? new List<Column> { new Column(1, LayoutUnitType.Auto) } : grid.Columns.ToList();

            #region Build index dictionary

            var actualIndices = new Dictionary<IElement, IndexPair>();
            foreach (var element in grid.Children)
            {
                if (element.ColumnSpan < 1 || element.RowSpan < 1)
                {
                    throw new ArgumentOutOfRangeException("grid", "UI elements must not have negative column or row spans.");
                }

                actualIndices.Add(element, new IndexPair
                {
                    ColumnIndex = Math.Min(element.ColumnIndex, columns.Count - 1),
                    RowIndex = Math.Min(element.RowIndex, rows.Count - 1)
                });
            }

            #endregion

            #region Find indices of auto controls

            if (actualIndices.Keys.Any(control => control.RowIndex < 0 || control.ColumnIndex < 0))
            {
                var columnHeights = new int[columns.Count];

                //Copy controls to an array in case the controls collection is modified
                var controls = new IElement[actualIndices.Count];
                actualIndices.Keys.CopyTo(controls, 0);

                foreach (var control in controls)
                {
                    var controlIndex = actualIndices[control];
                    var autoControl = controlIndex.RowIndex == Element.AutoLayoutIndex || controlIndex.ColumnIndex == Element.AutoLayoutIndex;

                    if (controlIndex.RowIndex == Element.AutoLayoutIndex)
                    {
                        // If we have a column index, honor it
                        if (controlIndex.ColumnIndex >= 0)
                        {
                            controlIndex.RowIndex = columnHeights.Skip(controlIndex.ColumnIndex).Take(control.ColumnSpan).Max();
                            var obstructors = GetGridControls(actualIndices, controlIndex.RowIndex, controlIndex.ColumnIndex, control).ToList();
                            while (obstructors.Any())
                            {
                                // Try the next available row
                                controlIndex.RowIndex = obstructors.Max(p => p.Value.RowIndex + p.Key.RowSpan);
                                obstructors = GetGridControls(actualIndices, controlIndex.RowIndex, controlIndex.ColumnIndex, control).ToList();
                            }
                        }
                        else
                        {
                            // Set a minimum row and drop to auto-column logic
                            controlIndex.RowIndex = columnHeights.Min();
                        }
                    }

                    // Place in specified row or one with an available span of columns
                    while (controlIndex.ColumnIndex == Element.AutoLayoutIndex)
                    {
                        controlIndex.ColumnIndex = GetColumnForRow(controlIndex.RowIndex, columns.Count, control, actualIndices);
                        if (controlIndex.ColumnIndex > Element.AutoLayoutIndex) break;
                        var candidates = columnHeights.Where(r => r > controlIndex.RowIndex).ToList();
                        controlIndex.RowIndex = candidates.Any() ? candidates.Min() : rows.Count;
                    }

                    while (control.RowSpan + controlIndex.RowIndex > rows.Count)
                    {
                        rows.Add(Row.AutoSized);
                    }

                    actualIndices[control] = controlIndex;

                    // Record high water mark for each column the control occludes.
                    for (int i = 0; i < control.ColumnSpan && controlIndex.ColumnIndex + i < columnHeights.Length; i++)
                    {
                        int nextAvailableRow = controlIndex.RowIndex + control.RowSpan;

                        // Auto controls always leave a high water mark. Manually-placed controls only occlude
                        // completely filled columns so that auto controls may be placed above them.
                        if (autoControl || controlIndex.RowIndex == columnHeights[controlIndex.ColumnIndex + i])
                        {
                            //If a manual control blocks the next row, include it in the high water mark.
                            Dictionary<IElement, IndexPair> idxs;
                            while ((idxs = actualIndices.Where(v =>
                                        v.Value.ColumnIndex <= controlIndex.ColumnIndex + i &&
                                        v.Value.ColumnIndex + v.Key.ColumnSpan > controlIndex.ColumnIndex + i &&
                                        v.Value.RowIndex <= nextAvailableRow &&
                                        v.Value.RowIndex + v.Key.RowSpan > nextAvailableRow)
                                        .ToDictionary(p => p.Key, p => p.Value)).Any())
                            {
                                nextAvailableRow = idxs.Max(p => p.Value.RowIndex + p.Key.RowSpan);
                            }
                            columnHeights[controlIndex.ColumnIndex + i] = nextAvailableRow;
                        }
                    }
                }
            }

            #endregion

            var scale = iApp.Factory.GetDisplayScale();
            maximumSize.Height *= scale;
            minimumSize.Height *= scale;

            // total grid size minus grid padding
            double totalGridWidth = double.IsInfinity(maximumSize.Width) ? double.MaxValue : maximumSize.Width;
            double totalGridHeight = double.IsInfinity(maximumSize.Height) ? double.MaxValue : maximumSize.Height;
            totalGridWidth -= (grid.Padding.Left + grid.Padding.Right) * scale;
            totalGridHeight -= (grid.Padding.Top + grid.Padding.Bottom) * scale;

            // available grid size for autos
            double availableGridWidth = totalGridWidth;
            double availableGridHeight = totalGridHeight;

            var rowSpaces = new Space[rows.Count];
            var columnSpaces = new Space[columns.Count];
            var elementSizes = new Dictionary<IElement, Size>(actualIndices.Count);

            #region Star sizing with no size limit

            // if there are any star rows or columns and the maximum size is infinite,
            // treat the star with the smallest weight as an auto and use it to size the others.
            int baseStarRowIndex = -1;
            if (double.IsInfinity(maximumSize.Height))
            {
                double minWeight = double.MaxValue;
                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    if (row.UnitType == LayoutUnitType.Star && minWeight > row.Height)
                    {
                        minWeight = row.Height;
                        baseStarRowIndex = i;
                    }
                }

                if (baseStarRowIndex >= 0)
                {
                    rows[baseStarRowIndex] = new Row(rows[baseStarRowIndex].Height, LayoutUnitType.Auto);
                }
            }

            int baseStarColumnIndex = -1;
            if (double.IsInfinity(maximumSize.Width))
            {
                double minWeight = double.MaxValue;
                for (int i = 0; i < columns.Count; i++)
                {
                    var column = columns[i];
                    if (column.UnitType == LayoutUnitType.Star && minWeight > column.Width)
                    {
                        minWeight = column.Width;
                        baseStarColumnIndex = i;
                    }
                }

                if (baseStarColumnIndex >= 0)
                {
                    columns[baseStarColumnIndex] = new Column(columns[baseStarColumnIndex].Width, LayoutUnitType.Auto);
                }
            }

            #endregion

            #region Absolute sizing

            for (int rIndex = 0; rIndex < rows.Count; rIndex++)
            {
                Row row = rows[rIndex];
                if (row.UnitType == LayoutUnitType.Absolute)
                {
                    availableGridHeight -= row.Height * scale;
                    rowSpaces[rIndex].Size = row.Height * scale;
                }
            }

            for (int cIndex = 0; cIndex < columns.Count; cIndex++)
            {
                Column column = columns[cIndex];
                if (column.UnitType == LayoutUnitType.Absolute)
                {
                    availableGridWidth -= column.Width * scale;
                    columnSpaces[cIndex].Size = column.Width * scale;
                }
            }

            #endregion

            // loop through every child element, regardless of column/row indices or spans, and determine its desired size.
            // this is considered the 'measure' pass and is just to get an idea of how much space each element wants.
            foreach (var kvp in actualIndices)
            {
                var element = kvp.Key;
                var indexPair = kvp.Value;

                if (element.Visibility == Visibility.Collapsed)
                {
                    elementSizes[element] = Size.Empty;
                    continue;
                }

                double maxElementWidth = 0;
                double maxElementHeight = 0;

                bool inAutoColumn = false, inAutoRow = false, takeAll = false;
                int lastColumn = Math.Min(columns.Count, indexPair.ColumnIndex + element.ColumnSpan);
                for (int i = indexPair.ColumnIndex; i < lastColumn; i++)
                {
                    var column = columns[i];
                    if (column.UnitType == LayoutUnitType.Absolute)
                    {
                        maxElementWidth += column.Width;
                    }
                    else if (!takeAll)
                    {
                        // if the element spans any auto or star column, its constraining width will include the
                        // entire available grid width minus any absolutes that the element is not a part of.
                        // this is because we don't know the final size of these columns yet, and we need to assume
                        // that they will take all of the grid's remaining available space.
                        takeAll = true;
                        maxElementWidth += availableGridWidth;
                    }

                    if (column.UnitType == LayoutUnitType.Auto)
                    {
                        inAutoColumn = true;
                    }
                }

                takeAll = false;
                int lastRow = Math.Min(rows.Count, indexPair.RowIndex + element.RowSpan);
                for (int i = indexPair.RowIndex; i < lastRow; i++)
                {
                    var row = rows[i];
                    if (row.UnitType == LayoutUnitType.Absolute)
                    {
                        maxElementHeight += row.Height;
                    }
                    else if (!takeAll)
                    {
                        // if the element spans any auto row, its constraining height will include the
                        // entire available grid height minus any absolutes that the element is not a part of.
                        // this is because we don't know the final size of these rows yet, and we need to assume
                        // that they will take all of the grid's remaining available space.
                        takeAll = true;
                        maxElementHeight += availableGridHeight;
                    }

                    if (row.UnitType == LayoutUnitType.Auto)
                    {
                        inAutoRow = true;
                    }
                }

                // if the element doesn't span any autos, we don't need to measure it in this pass
                if (!inAutoColumn && !inAutoRow)
                {
                    continue;
                }

                var desiredSize = element.Measure(new Size(Math.Max(0, maxElementWidth - (element.Margin.Left + element.Margin.Right) * scale),
                    Math.Max(0, maxElementHeight - (element.Margin.Top + element.Margin.Bottom) * scale)));

                elementSizes[element] = desiredSize;

                // if the element only spans one auto column, we can use it to determine the ultimate width for the column
                if (indexPair.ColumnIndex == (lastColumn - 1) && columns[indexPair.ColumnIndex].UnitType == LayoutUnitType.Auto)
                {
                    double totalWidth = Math.Max(desiredSize.Width + (element.Margin.Left + element.Margin.Right) * scale, 0);

                    var space = columnSpaces[indexPair.ColumnIndex];
                    if (space.Size < totalWidth)
                    {
                        space.Size = totalWidth;
                        columnSpaces[indexPair.ColumnIndex] = space;
                    }
                }

                // if the element only spans one auto row, we can use it to determine the ultimate height for the row
                if (indexPair.RowIndex == (lastRow - 1) && rows[indexPair.RowIndex].UnitType == LayoutUnitType.Auto)
                {
                    double totalHeight = Math.Max(desiredSize.Height + (element.Margin.Top + element.Margin.Bottom) * scale, 0);

                    var space = rowSpaces[indexPair.RowIndex];
                    if (space.Size < totalHeight)
                    {
                        space.Size = totalHeight;
                        rowSpaces[indexPair.RowIndex] = space;
                    }
                }
            }

            SizeColumns(grid, minimumSize.Width, availableGridWidth, columns, columnSpaces, baseStarColumnIndex, scale);
            SizeRows(grid, minimumSize.Height, availableGridHeight, rows, rowSpaces, baseStarRowIndex, scale);

            // this is considered the 'arrange' pass and is where we get the final size of each element before giving them an XY position.
            // at this point, all columns and rows should have a size that's pretty close to their final size.
            // some adjustments may be required if the second measurement of an element causes an auto row or column to be resized.
            foreach (var keyValue in actualIndices)
            {
                var element = keyValue.Key;
                var indexPair = keyValue.Value;

                if (element.Visibility == Visibility.Collapsed)
                {
                    element.SetLocation(new Point(columnSpaces[indexPair.ColumnIndex].Origin, rowSpaces[indexPair.RowIndex].Origin), Size.Empty);
                    continue;
                }

                var marginLeft = element.Margin.Left * scale;
                var marginTop = element.Margin.Top * scale;
                var marginRight = element.Margin.Right * scale;
                var marginBottom = element.Margin.Bottom * scale;

                Space rowSpace = rowSpaces[indexPair.RowIndex];
                for (int i = indexPair.RowIndex + 1; i < (indexPair.RowIndex + element.RowSpan) && i < rows.Count; i++)
                {
                    rowSpace.Size += rowSpaces[i].Size;
                }

                Space columnSpace = columnSpaces[indexPair.ColumnIndex];
                for (int i = indexPair.ColumnIndex + 1; i < (indexPair.ColumnIndex + element.ColumnSpan) && i < columns.Count; i++)
                {
                    columnSpace.Size += columnSpaces[i].Size;
                }

                var constraints = new Size(columnSpace.Size - (marginLeft + marginRight), rowSpace.Size - (marginTop + marginBottom));

                // labels are a bit special; as one dimension expands or contracts, the other dimension inversely contracts or expands.
                // because of this, if the column width is shorter than the label's desired width, the label will likely want more
                // vertical space than its desired height suggests.  to compensate for this, if the label spans any auto row,
                // we reset the vertical constraint before the label's final measurement and adjust any rows as needed.
                if (element is ILabel)
                {
                    int lastRow = Math.Min(rows.Count, indexPair.RowIndex + element.RowSpan);
                    for (int i = indexPair.RowIndex; i < lastRow; i++)
                    {
                        if (rows[i].UnitType == LayoutUnitType.Auto)
                        {
                            constraints.Height = double.MaxValue;
                            break;
                        }
                    }
                }

                constraints.Width = Math.Max(constraints.Width, 0);
                constraints.Height = Math.Max(constraints.Height, 0);

                var finalSize = element.Measure(constraints);
                var totalFinalSize = new Size(finalSize.Width + (marginLeft + marginRight), finalSize.Height + (marginTop + marginBottom));

                var desiredSize = elementSizes.GetValueOrDefault(element);
                var totalDesiredSize = new Size(desiredSize.Width + (marginLeft + marginRight), desiredSize.Height + (marginTop + marginBottom));

                // if the final size is different from the desired size, see if any row or column needs adjustment.
                if (Math.Abs(totalFinalSize.Width - totalDesiredSize.Width) > 0.01)
                {
                    int trueColumnSpan = Math.Min(columns.Count - indexPair.ColumnIndex, element.ColumnSpan);
                    if (trueColumnSpan == 1 && columns[indexPair.ColumnIndex].UnitType == LayoutUnitType.Auto)
                    {
                        // if the final width is larger than the current size of the column, we need to expand the column.
                        // on the other hand, if this element is what determined the column's initial size and its final
                        // width is smaller, we need to shrink the column.
                        if (totalFinalSize.Width > columnSpace.Size || (totalDesiredSize.Width == columnSpace.Size &&
                            !elementSizes.Any(e => e.Key != element && actualIndices[e.Key].ColumnIndex == indexPair.ColumnIndex
                                && e.Value.Width + (marginLeft + marginRight) >= columnSpace.Size)))
                        {
                            columnSpace.Size = totalFinalSize.Width;
                            columnSpaces[indexPair.ColumnIndex] = columnSpace;
                            SizeColumns(grid, minimumSize.Width, availableGridWidth, columns, columnSpaces, baseStarColumnIndex, scale);

                            // SizeColumns may have shrunk the column if it was too large to fit in the grid
                            totalFinalSize.Width = columnSpaces[indexPair.ColumnIndex].Size;
                        }
                    }
                }

                if (Math.Abs(totalFinalSize.Height - totalDesiredSize.Height) > 0.01)
                {
                    int trueRowSpan = Math.Min(rows.Count - indexPair.RowIndex, element.RowSpan);
                    if (trueRowSpan == 1 && rows[indexPair.RowIndex].UnitType == LayoutUnitType.Auto)
                    {
                        // if the final height is larger than the current size of the row, we need to expand the row.
                        // on the other hand, if this element is what determined the row's initial size and its final
                        // height is smaller, we need to shrink the row.
                        if (totalFinalSize.Height > rowSpace.Size || (totalDesiredSize.Height == rowSpace.Size &&
                            !elementSizes.Any(e => e.Key != element && actualIndices[e.Key].RowIndex == indexPair.RowIndex &&
                                e.Value.Height + (marginTop + marginBottom) >= rowSpace.Size)))
                        {
                            rowSpace.Size = totalFinalSize.Height;
                            rowSpaces[indexPair.RowIndex] = rowSpace;
                            SizeRows(grid, minimumSize.Height, availableGridHeight, rows, rowSpaces, baseStarRowIndex, scale);

                            // SizeRows may have shrunk the row if it was too large to fit in the grid
                            totalFinalSize.Height = rowSpaces[indexPair.RowIndex].Size;
                        }
                    }
                }

                finalSize = new Size(totalFinalSize.Width - (marginLeft + marginRight), totalFinalSize.Height - (marginTop + marginBottom));
                var availableArea = new Size(columnSpace.Size - (marginLeft + marginRight), rowSpace.Size - (marginTop + marginBottom));

                var image = element as IImage;
                if (image == null)
                {
                    var button = element as IButton;
                    if (button != null)
                    {
                        image = button.Image;
                    }
                }

                if (image != null)
                {
                    var dimensions = image.Dimensions;
                    var ratio = dimensions.Width / dimensions.Height;
                    switch (image.Stretch)
                    {
                        case ContentStretch.Fill:
                        case ContentStretch.UniformToFill:
                            // in the case of UniformToFill, it is the platform's responsibility to clip the image
                            finalSize.Width = availableArea.Width;
                            finalSize.Height = availableArea.Height;
                            break;
                        case ContentStretch.None:
                            // Stretch.None should still shrink the image if it's too large to fit in its space
                            if (finalSize.Width < dimensions.Width || finalSize.Height < dimensions.Height)
                            {
                                if (finalSize.Width > finalSize.Height * ratio)
                                {
                                    finalSize.Width = finalSize.Height * ratio;
                                }
                                else if (finalSize.Width < finalSize.Height * ratio)
                                {
                                    finalSize.Height = finalSize.Width / ratio;
                                }
                            }
                            break;
                        case ContentStretch.Uniform:
                            if (availableArea.Width > availableArea.Height * ratio)
                            {
                                finalSize.Width = availableArea.Height * ratio;
                                finalSize.Height = availableArea.Height;
                            }
                            else if (availableArea.Width < availableArea.Height * ratio)
                            {
                                finalSize.Width = availableArea.Width;
                                finalSize.Height = availableArea.Width / ratio;
                            }
                            break;
                    }
                }

                var location = new Point(columnSpace.Origin, rowSpace.Origin);

                switch (element.VerticalAlignment)
                {
                    case VerticalAlignment.Stretch:
                        location.Y = rowSpace.Origin + marginTop;
                        finalSize.Height = availableArea.Height;
                        break;
                    case VerticalAlignment.Top:
                        location.Y = rowSpace.Origin + marginTop;
                        finalSize.Height = Math.Min(finalSize.Height, availableArea.Height);
                        break;
                    case VerticalAlignment.Bottom:
                        location.Y = (rowSpace.Origin + rowSpace.Size) - (finalSize.Height + marginBottom);
                        finalSize.Height = Math.Min(finalSize.Height, availableArea.Height);
                        break;
                    case VerticalAlignment.Center:
                        location.Y = (availableArea.Height / 2f) - (finalSize.Height / 2f) + (rowSpace.Origin + marginTop);
                        finalSize.Height = Math.Min(finalSize.Height, availableArea.Height);
                        break;
                }

                switch (element.HorizontalAlignment)
                {
                    case HorizontalAlignment.Stretch:
                        location.X = columnSpace.Origin + marginLeft;
                        finalSize.Width = availableArea.Width;
                        break;
                    case HorizontalAlignment.Left:
                        location.X = columnSpace.Origin + marginLeft;
                        finalSize.Width = Math.Min(finalSize.Width, availableArea.Width);
                        break;
                    case HorizontalAlignment.Right:
                        location.X = (columnSpace.Origin + columnSpace.Size) - (finalSize.Width + marginRight);
                        finalSize.Width = Math.Min(finalSize.Width, availableArea.Width);
                        break;
                    case HorizontalAlignment.Center:
                        location.X = (availableArea.Width / 2f) - (finalSize.Width / 2f) + (columnSpace.Origin + marginLeft);
                        finalSize.Width = Math.Min(finalSize.Width, columnSpace.Size - marginLeft - marginRight);
                        break;
                }

                finalSize.Width = Math.Max(finalSize.Width, 0);
                finalSize.Height = Math.Max(finalSize.Height, 0);

                elementSizes[element] = finalSize;
                element.SetLocation(location, finalSize);
            }

            // everything is laid out to the maximum size, so we know that we haven't exceeded it
            return new Size(Math.Max(columnSpaces.Sum(c => c.Size) + scale * (grid.Padding.Left + grid.Padding.Right), minimumSize.Width),
                Math.Max(rowSpaces.Sum(r => r.Size) + (grid.Padding.Top + grid.Padding.Bottom) * scale, minimumSize.Height));
        }

        private static int GetColumnForRow(int row, int columnCount, IElement control, Dictionary<IElement, IndexPair> actualIndices)
        {
            int column = 0;
            while (column < columnCount)
            {
                // find columns where the first available row is the current row.
                var obstructors = GetGridControls(actualIndices, row, column, control).ToList();
                if (obstructors.Any())
                {
                    column = obstructors.Max(p => p.Value.ColumnIndex + p.Key.ColumnSpan);
                }
                else
                {
                    return column;
                }
            }
            return -1;
        }

        private static IEnumerable<KeyValuePair<IElement, IndexPair>> GetGridControls(Dictionary<IElement, IndexPair> actualIndices, int row, int column, IElement control)
        {
            return actualIndices.Where(c => !c.Key.Equals(control) &&
                       // If this control has been assigned actual indices
                       c.Value.RowIndex > Element.AutoLayoutIndex &&
                       c.Value.ColumnIndex > Element.AutoLayoutIndex &&
                       // And the left edge of this control is within the bounds
                       (c.Value.ColumnIndex >= column &&
                        c.Value.ColumnIndex < column + control.ColumnSpan ||
                        // Or the right edge of this control is within the bounds 
                        c.Value.ColumnIndex + c.Key.ColumnSpan > column &&
                        c.Value.ColumnIndex + c.Key.ColumnSpan <= column + control.ColumnSpan) &&
                       // And the top is in the way
                       (c.Value.RowIndex >= row &&
                        c.Value.RowIndex < row + control.RowSpan ||
                        // Or the bottom
                        c.Value.RowIndex + c.Key.RowSpan > row &&
                        c.Value.RowIndex + c.Key.RowSpan <= row + control.RowSpan));
        }

        private static void SizeColumns(IGridBase grid, double minimumWidth, double availableWidth, List<Column> columns, Space[] columnSpaces, int baseStarIndex, double scale)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].UnitType == LayoutUnitType.Auto)
                {
                    var space = columnSpaces[i];
                    if (space.Size > availableWidth)
                    {
                        space.Size = availableWidth;
                        availableWidth = 0;
                        columnSpaces[i] = space;
                    }
                    else
                    {
                        if (space.Size < 0)
                        {
                            space.Size = 0;
                            columnSpaces[i] = space;
                        }
                        availableWidth -= columnSpaces[i].Size;
                    }
                }
            }

            double weightSum = columns.Where(c => c.UnitType == LayoutUnitType.Star).Sum(c => c.Width);
            double starWidth = availableWidth / weightSum;
            if (baseStarIndex >= 0)
            {
                weightSum += columns[baseStarIndex].Width;
                starWidth = columnSpaces[baseStarIndex].Size * columns[baseStarIndex].Width;

                // check if starWidth is sufficient to cover minimum width
                double consumedWidth = starWidth * weightSum;
                for (int i = 0; i < columns.Count; i++)
                {
                    if (i == baseStarIndex)
                    {
                        continue;
                    }

                    var column = columns[i];
                    if (column.UnitType != LayoutUnitType.Star)
                    {
                        consumedWidth += columnSpaces[i].Size;
                    }
                }

                double minWidth = minimumWidth - ((grid.Padding.Left + grid.Padding.Right) * scale);
                if (consumedWidth < minWidth)
                {
                    starWidth += (minWidth - consumedWidth) / weightSum;
                    columnSpaces[baseStarIndex] = new Space() { Size = starWidth * columns[baseStarIndex].Width };
                }
            }

            for (int cIndex = 0; cIndex < columns.Count; cIndex++)
            {
                var column = columns[cIndex];
                var space = columnSpaces[cIndex];
                if (column.UnitType == LayoutUnitType.Star)
                {
                    space.Size = column.Width * starWidth;
                    columnSpaces[cIndex] = space;
                }

                if (grid.Columns.Count > cIndex)
                {
                    grid.Columns[cIndex] = new Column(column, space.Size / scale);
                }
            }

            // if width is less than minimum, stretch the last auto column that has a size > 0
            double extraCellWidth = minimumWidth - columnSpaces.Sum(c => c.Size) - ((grid.Padding.Left + grid.Padding.Right) * scale);
            if (extraCellWidth > 0)
            {
                for (int i = columns.Count - 1; i >= 0; i--)
                {
                    var column = columns[i];
                    var space = columnSpaces[i];
                    if (column.UnitType != LayoutUnitType.Auto || space.Size <= 0)
                    {
                        continue;
                    }

                    space.Size += extraCellWidth;
                    columnSpaces[i] = space;

                    if (grid.Columns.Count > i)
                    {
                        grid.Columns[i] = new Column(column, space.Size / scale);
                    }
                    break;
                }
            }

            columnSpaces[0].Origin = grid.Padding.Left * scale;
            for (int cIndex = 1; cIndex < columnSpaces.Length; cIndex++)
            {
                var previous = columnSpaces[cIndex - 1];
                columnSpaces[cIndex].Origin = previous.Origin + previous.Size;
            }
        }

        private static void SizeRows(IGridBase grid, double minimumHeight, double availableHeight, List<Row> rows, Space[] rowSpaces, int baseStarIndex, double scale)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].UnitType == LayoutUnitType.Auto)
                {
                    var space = rowSpaces[i];
                    if (space.Size > availableHeight)
                    {
                        space.Size = availableHeight;
                        availableHeight = 0;
                        rowSpaces[i] = space;
                    }
                    else
                    {
                        if (space.Size < 0)
                        {
                            space.Size = 0;
                            rowSpaces[i] = space;
                        }
                        availableHeight -= rowSpaces[i].Size;
                    }
                }
            }

            double weightSum = rows.Where(r => r.UnitType == LayoutUnitType.Star).Sum(r => r.Height);
            double starHeight = availableHeight / weightSum;
            if (baseStarIndex >= 0)
            {
                weightSum += rows[baseStarIndex].Height;
                starHeight = rowSpaces[baseStarIndex].Size * rows[baseStarIndex].Height;

                // check if starHeight is sufficient to cover minimum height
                double consumedHeight = starHeight * weightSum;
                for (int i = 0; i < rows.Count; i++)
                {
                    if (i == baseStarIndex)
                    {
                        continue;
                    }

                    var row = rows[i];
                    if (row.UnitType != LayoutUnitType.Star)
                    {
                        consumedHeight += rowSpaces[i].Size;
                    }
                }

                double minHeight = minimumHeight - ((grid.Padding.Top + grid.Padding.Bottom) * scale);
                if (consumedHeight < minHeight)
                {
                    starHeight += (minHeight - consumedHeight) / weightSum;
                    rowSpaces[baseStarIndex] = new Space() { Size = starHeight * rows[baseStarIndex].Height };
                }
            }

            for (int rIndex = 0; rIndex < rows.Count; rIndex++)
            {
                var row = rows[rIndex];
                var space = rowSpaces[rIndex];
                if (row.UnitType == LayoutUnitType.Star)
                {
                    space.Size = row.Height * starHeight;
                    rowSpaces[rIndex] = space;
                }

                if (grid.Rows.Count > rIndex)
                {
                    grid.Rows[rIndex] = new Row(row, space.Size / scale);
                }
            }

            // if height is less than minimum, stretch the last auto row that has a size
            double extraCellHeight = minimumHeight - rowSpaces.Sum(r => r.Size) - ((grid.Padding.Top + grid.Padding.Bottom) * scale);
            if (extraCellHeight > 0)
            {
                for (int i = rows.Count - 1; i >= 0; i--)
                {
                    var row = rows[i];
                    var space = rowSpaces[i];
                    if (row.UnitType != LayoutUnitType.Auto || space.Size <= 0)
                    {
                        continue;
                    }

                    space.Size += extraCellHeight;
                    rowSpaces[i] = space;

                    if (grid.Rows.Count > i)
                    {
                        grid.Rows[i] = new Row(row, space.Size / scale);
                    }
                    break;
                }
            }

            rowSpaces[0].Origin = grid.Padding.Top * scale;
            for (int rIndex = 1; rIndex < rowSpaces.Length; rIndex++)
            {
                var previous = rowSpaces[rIndex - 1];
                rowSpaces[rIndex].Origin = previous.Origin + previous.Size;
            }
        }

        private struct Space
        {
            public double Origin;
            public double Size;

            public override string ToString()
            {
                return string.Format("Origin: {0}, Size: {1}", Origin, Size);
            }
        }

        private struct IndexPair
        {
            public int ColumnIndex;
            public int RowIndex;

            public IndexPair(int rowIndex, int columnIndex)
            {
                RowIndex = rowIndex;
                ColumnIndex = columnIndex;
            }

            public override string ToString()
            {
                return string.Format("Row Index: {0}, Column Index: {1}", RowIndex, ColumnIndex);
            }
        }
    }
}