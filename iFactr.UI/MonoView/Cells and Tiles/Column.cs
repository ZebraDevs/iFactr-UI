using System;
using System.Collections.Generic;
using System.Diagnostics;
using iFactr.UI.Controls;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a vertical partition of space in an <see cref="IGridBase"/> object.
    /// </summary>
    public struct Column
    {
        /// <summary>
        /// Gets a <see cref="iFactr.UI.Column"/> that automatically sizes itself to fit its contents.
        /// </summary>
        public static Column AutoSized
        {
            get { return new Column(1, LayoutUnitType.Auto); }
        }

        /// <summary>
        /// Gets a <see cref="iFactr.UI.Column"/> with a star unit type and a weight of 1.
        /// </summary>
        public static Column OneStar
        {
            get { return new Column(1, LayoutUnitType.Star); }
        }

        /// <summary>
        /// The actual width of the column after layout, expressed in absolute units.  This field is read-only.
        /// </summary>
        public readonly double ActualWidth;

        /// <summary>
        /// The width of the column, expressed in units specified by <see cref="UnitType"/>.  This field is read-only.
        /// </summary>
        public readonly double Width;

        /// <summary>
        /// The type of unit that the <see cref="Width"/> value represents.  This field is read-only.
        /// </summary>
        public readonly LayoutUnitType UnitType;

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Column"/> structure.
        /// </summary>
        /// <param name="width">The width of the column, expressed in units specified by <paramref name="unitType"/>.</param>
        /// <param name="unitType">The type of unit that the <paramref name="width"/> value represents.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/> is less than 0.</exception>
        public Column(double width, LayoutUnitType unitType)
        {
            if (width < 0 && unitType != LayoutUnitType.Auto)
            {
                throw new ArgumentOutOfRangeException("width", "Value must be no less than 0 when using Star or Absolute unit types.");
            }

            ActualWidth = 0;
            Width = width;
            UnitType = unitType;
        }

        internal Column(Column column, double actualWidth)
        {
            ActualWidth = actualWidth;
            Width = column.Width;
            UnitType = column.UnitType;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Column"/> is equal to the current <see cref="Column"/>.
        /// </summary>
        /// <param name="column">The <see cref="Column"/> to compare with the current <see cref="Column"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Column"/> is equal to the current
        /// <see cref="Column"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Column column)
        {
            return column.Width == Width && column.UnitType == UnitType;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Column"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Column"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Column"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Column)
            {
                return Equals((Column)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Column"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return Width.GetHashCode() ^ UnitType.GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Width: {0}, Unit Type: {1}", Width, UnitType);
        }

        /// <summary>
        /// Determines whether two <see cref="Column"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Column value1, Column value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="Column"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Column value1, Column value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Adds the specified <paramref name="amount"/> to the width of a <see cref="Column"/>.
        /// </summary>
        /// <param name="column">The column whose width is to be added to.</param>
        /// <param name="amount">The amount to add to the width of the column.</param>
        /// <returns>The result of adding the <paramref name="amount"/> to the column's width.</returns>
        public static Column operator +(Column column, double amount)
        {
            return new Column(column.Width + amount, column.UnitType);
        }

        /// <summary>
        /// Subtracts the specified <paramref name="amount"/> from the width of a <see cref="Column"/>.
        /// </summary>
        /// <param name="column">The column whose width is to be subtracted from.</param>
        /// <param name="amount">The amount to subtract from the width of the column.</param>
        /// <returns>The result of subtracting the <paramref name="amount"/> from the column's width.</returns>
        public static Column operator -(Column column, double amount)
        {
            return new Column(column.Width - amount, column.UnitType);
        }

        /// <summary>
        /// Multiplies the width of a <see cref="Column"/> by the specified <paramref name="amount"/>.
        /// </summary>
        /// <param name="column">The column whose width is to be multiplied.</param>
        /// <param name="amount">The amount to multiply the width of the column.</param>
        /// <returns>The result of multiplying the column's width by the <paramref name="amount"/>.</returns>
        public static Column operator *(Column column, double amount)
        {
            return new Column(column.Width * amount, column.UnitType);
        }

        /// <summary>
        /// Divides the width of a <see cref="Column"/> by the specified <paramref name="amount"/>.
        /// </summary>
        /// <param name="column">The column whose width is to be divided.</param>
        /// <param name="amount">The amount to divide the width of the column.</param>
        /// <returns>The result of dividing the column's width by the <paramref name="amount"/>.</returns>
        public static Column operator /(Column column, double amount)
        {
            return new Column(column.Width / amount, column.UnitType);
        }
    }

    /// <summary>
    /// Represents a collection of <see cref="iFactr.UI.Column"/> objects.
    /// </summary>
#if !NETCF
    [DebuggerDisplay("Count = {Count}")]
#endif
    public sealed class ColumnCollection : List<Column>
    {
        /// <summary>
        /// Adds a <see cref="iFactr.UI.Column"/> object to the end of the collection.
        /// </summary>
        /// <param name="width">The width of the column, expressed in units specified by <paramref name="unitType"/>.</param>
        /// <param name="unitType">The type of unit that the <paramref name="width"/> value represents.</param>
        public void Add(double width, LayoutUnitType unitType)
        {
            Add(new Column(width, unitType));
        }

        /// <summary>
        /// Inserts a <see cref="iFactr.UI.Column"/> object into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the item should be inserted.</param>
        /// <param name="width">The width of the column, expressed in units specified by <paramref name="unitType"/>.</param>
        /// <param name="unitType">The type of unit that the <paramref name="width"/> value represents.</param>
        public void Insert(int index, double width, LayoutUnitType unitType)
        {
            Insert(index, new Column(width, unitType));
        }
    }
}
