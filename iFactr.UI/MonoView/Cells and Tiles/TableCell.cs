using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using iFactr.Core;
using iFactr.UI.Controls;
using iFactr.UI.Instructions;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a cell that lays its contents out in a table of rows and columns.
    /// The contents are automatically laid out in a manner appropriate to the target platform.
    /// </summary>
    internal sealed class TableCell : Cell, ILayoutInstruction
    {
        /// <summary>
        /// Gets the header label for the table.
        /// </summary>
        public Label TableHeader { get; private set; }

        /// <summary>
        /// Gets the header labels for each row in the table.
        /// </summary>
        public ReadOnlyCollection<Label> RowHeaders { get; private set; }

        /// <summary>
        /// Gets the header labels for each column in the table.
        /// </summary>
        public ReadOnlyCollection<Label> ColumnHeaders { get; private set; }

        /// <summary>
        /// Gets the number of rows in the table.
        /// </summary>
        public int RowCount
        {
            get { return RowHeaders.Count; }
        }

        /// <summary>
        /// Gets the number of columns in the table.
        /// </summary>
        public int ColumnCount
        {
            get { return ColumnHeaders.Count; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private IGridCell NativeCell
        {
            get { return (IGridCell)base.Pair; }
        }

        private IControl[][] columnControls;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCell"/> class.
        /// </summary>
        /// <param name="rowCount">The number of rows with which to initially set up the table.</param>
        /// <param name="columnCount">The number of columns with which to initially set up the table.</param>
        public TableCell(int rowCount, int columnCount)
            : this(string.Empty, new string[rowCount], new string[columnCount])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCell"/> class.
        /// </summary>
        /// <param name="rowHeaders">The strings to be used as headers for each row.  The number of rows will equal the number of strings provided.</param>
        /// <param name="columnHeaders">The strings to be used as headers for each column.  The number of columns will equal the number of strings provided.</param>
        public TableCell(IEnumerable<string> rowHeaders, IEnumerable<string> columnHeaders)
            : this(string.Empty, rowHeaders, columnHeaders)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCell"/> class.
        /// </summary>
        /// <param name="headerText">The text to display in the table header.</param>
        /// <param name="rowCount">The number of rows with which to initially set up the table.</param>
        /// <param name="columnCount">The number of columns with which to initially set up the table.</param>
        public TableCell(string headerText, int rowCount, int columnCount)
            : this(headerText, new string[rowCount], new string[columnCount])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCell"/> class.
        /// </summary>
        /// <param name="headerText">The text to display in the table header.</param>
        /// <param name="rowHeaders">The strings to be used as headers for each row.  The number of rows will equal the number of strings provided.</param>
        /// <param name="columnHeaders">The strings to be used as headers for each column.  The number of columns will equal the number of strings provided.</param>
        public TableCell(string headerText, IEnumerable<string> rowHeaders, IEnumerable<string> columnHeaders)
        {
            if (rowHeaders == null)
            {
                throw new ArgumentNullException("rowHeaders");
            }

            if (columnHeaders == null)
            {
                throw new ArgumentNullException("columnHeaders");
            }

            base.Pair = MXContainer.Resolve<IGridCell>();

            TableHeader = new Label();
            TableHeader.Font = Font.PreferredHeaderFont;
            TableHeader.Text = headerText;
            TableHeader.RowIndex = 0;
            TableHeader.ColumnIndex = 0;

            NativeCell.AddChild(TableHeader);

            RowHeaders = new ReadOnlyCollection<Label>(rowHeaders.Select(h =>
            {
                var label = new Label();
                label.Text = h;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Center;

                NativeCell.AddChild(label);
                return label;
            }).ToList());

            ColumnHeaders = new ReadOnlyCollection<Label>(columnHeaders.Select(h =>
            {
                var label = new Label();
                label.Text = h;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Center;

                NativeCell.AddChild(label);
                return label;
            }).ToList());

            columnControls = new IControl[ColumnHeaders.Count][];
            for (int i = 0; i < columnControls.Length; i++)
            {
                columnControls[i] = new IControl[RowCount];
            }
        }

        /// <summary>
        /// Returns the <see cref="IControl"/> instances that currently reside within the specified column.
        /// </summary>
        /// <param name="columnIndex">The zero-based index of the column in which to retrieve the controls.</param>
        /// <returns>An <see cref="Array"/> containing the controls currently within the column.</returns>
        public IControl[] GetControlsForColumn(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= columnControls.Length)
            {
                throw new ArgumentOutOfRangeException("columnIndex", "Value cannot be less than 0 or greater than or equal to the number of columns in the table.");
            }

            var array = new IControl[RowCount];
            columnControls[columnIndex].CopyTo(array, 0);
            return array;
        }

        /// <summary>
        /// Adds the specified <see cref="IControl"/> instances to the specified column, replacing any controls that were already in the column.
        /// </summary>
        /// <param name="columnIndex">The zero-based index of the column in which to place the controls.</param>
        /// <param name="controls">The controls to be placed in the column specified by <paramref name="columnIndex"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="columnIndex"/> is less than 0 -or-
        /// when <paramref name="columnIndex"/> is greater than or equal to the number of columns currently in the table.</exception>
        public void SetControlsForColumn(int columnIndex, params IControl[] controls)
        {
            if (columnIndex < 0 || columnIndex >= columnControls.Length)
            {
                throw new ArgumentOutOfRangeException("columnIndex", "Value cannot be less than 0 or greater than or equal to the number of columns in the table.");
            }

            var cc = columnControls[columnIndex];
            for (int i = 0; i < cc.Length; i++)
            {
                NativeCell.RemoveChild(cc[i]);
                cc[i] = null;
            }

            if (controls == null)
            {
                return;
            }

            for (int i = 0; i < Math.Min(controls.Length, cc.Length); i++)
            {
                var control = controls[i];
                if (control != null)
                {
                    NativeCell.AddChild(control);
                }
                
                cc[i] = control;
            }
        }

        /// <summary>
        /// Adds the specified number of rows to the bottom of the table.
        /// </summary>
        /// <param name="count">The number of rows to add to the bottom of the table.</param>
        /// <param name="headerTexts">Optional strings to apply to the header labels of the new rows.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is less than 0.</exception>
        public void AddRows(int count, params string[] headerTexts)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "Value cannot be less than 0.");
            }

            var list = RowHeaders.ToList();
            int currentCount = list.Count;

            for (int i = 0; i < count; i++)
            {
                var label = new Label();
                label.Text = headerTexts != null && headerTexts.Length > i ? headerTexts[i] : null;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Center;

                NativeCell.AddChild(label);
                list.Add(label);
            }

            RowHeaders = new ReadOnlyCollection<Label>(list);

            for (int i = 0; i < columnControls.Length; i++)
            {
                var cc = columnControls[i];
                Array.Resize(ref cc, RowCount);
            }
        }

        /// <summary>
        /// Adds the specified number of columns to the right side of the table.
        /// </summary>
        /// <param name="count">The number of columns to add to the right side of the table.</param>
        /// <param name="headerTexts">Optional strings to apply to the header labels of the new columns.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is less than 0.</exception>
        public void AddColumns(int count, params string[] headerTexts)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "Value cannot be less than 0.");
            }

            var list = ColumnHeaders.ToList();
            int currentCount = ColumnCount;
            Array.Resize(ref columnControls, currentCount + count);

            for (int i = 0; i < count; i++)
            {
                var label = new Label();
                label.Text = headerTexts != null && headerTexts.Length > i ? headerTexts[i] : null;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Center;

                NativeCell.AddChild(label);
                list.Add(label);
                columnControls[currentCount + i] = new IControl[RowCount];
            }

            ColumnHeaders = new ReadOnlyCollection<Label>(list);
        }

        /// <summary>
        /// Removes the specified number of rows from the bottom of the table.
        /// </summary>
        /// <param name="count">The number of rows to remove from the bottom of the table.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is less than 0 -or-
        /// when <paramref name="count"/> is greater than the number of rows currently in the table.</exception>
        public void RemoveRows(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "Value cannot be less than 0.");
            }

            if (count > RowCount)
            {
                throw new ArgumentOutOfRangeException("count", "Value cannot be greater than the number of rows currently in the table.");
            }

            var list = RowHeaders.ToList();
            for (int i = list.Count - 1; i >= list.Count - count; i--)
            {
                NativeCell.RemoveChild(list[i]);
                list.RemoveAt(i);

                foreach (var cc in columnControls)
                {
                    NativeCell.RemoveChild(cc[i]);
                }
            }

            for (int i = 0; i < columnControls.Length; i++)
            {
                Array.Resize(ref columnControls[i], RowCount - count);
            }

            RowHeaders = new ReadOnlyCollection<Label>(list);
        }

        /// <summary>
        /// Removes the specified number of columns from the right side of the table.
        /// </summary>
        /// <param name="count">The number of columns to remove from the right side of the table.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is less than 0 -or-
        /// when <paramref name="count"/> is greater than the number of columns currently in the table.</exception>
        public void RemoveColumns(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "Value cannot be less than 0.");
            }

            if (count > ColumnCount)
            {
                throw new ArgumentOutOfRangeException("count", "Value cannot be greater than the number of columns currently in the table.");
            }

            var list = ColumnHeaders.ToList();
            for (int i = list.Count - 1; i >= list.Count - count; i--)
            {
                NativeCell.RemoveChild(list[i]);
                list.RemoveAt(i);

                var cc = columnControls[i];
                foreach (var control in cc)
                {
                    NativeCell.RemoveChild(control);
                }
            }

            Array.Resize(ref columnControls, ColumnCount - count);
            ColumnHeaders = new ReadOnlyCollection<Label>(list);
        }

        void ILayoutInstruction.Layout()
        {
            NativeCell.SetRows(2);
            NativeCell.SetColumns(1);

            for (int i = 0; i < RowCount; i++)
            {
                NativeCell.Rows.Add(Row.AutoSized);

                var rowHeader = RowHeaders[i];
                rowHeader.HorizontalAlignment = HorizontalAlignment.Right;
                rowHeader.Margin = new Thickness(0, Thickness.SmallVerticalSpacing, Thickness.SmallHorizontalSpacing, 0);
                rowHeader.ColumnIndex = 0;
                rowHeader.RowIndex = NativeCell.Rows.Count - 1;
            }

            for (int i = 0; i < ColumnCount; i++)
            {
                NativeCell.Columns.Add(Column.OneStar);

                var columnHeader = ColumnHeaders[i];
                columnHeader.Margin = new Thickness(Thickness.SmallHorizontalSpacing, Thickness.LargeVerticalSpacing, 0, 0);
                columnHeader.ColumnIndex = NativeCell.Columns.Count - 1;
                columnHeader.RowIndex = 1;

                var controls = columnControls[i];
                for (int j = 0; j < controls.Length; j++)
                {
                    var control = controls[j];
                    if (control == null)
                    {
                        continue;
                    }

                    control.ColumnIndex = NativeCell.Columns.Count - 1;
                    control.RowIndex = 2 + j;
                }
            }

            TableHeader.ColumnSpan = NativeCell.Columns.Count;
        }
    }
}
