using System;
using System.Collections.Generic;
using System.Diagnostics;
using iFactr.UI.Controls;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a horizontal partition of space in an <see cref="IGridBase"/> object.
    /// </summary>
    public struct Row
    {
        /// <summary>
        /// Gets a <see cref="iFactr.UI.Row"/> that automatically sizes itself to fit its contents.
        /// </summary>
        public static Row AutoSized
        {
            get { return new Row(1, LayoutUnitType.Auto); }
        }

        /// <summary>
        /// Gets a <see cref="iFactr.UI.Row"/> with a star unit type and a weight of 1.
        /// </summary>
        public static Row OneStar
        {
            get { return new Row(1, LayoutUnitType.Star); }
        }

        /// <summary>
        /// The actual height of the row after layout, expressed in absolute units.  This field is read-only.
        /// </summary>
        public readonly double ActualHeight;

        /// <summary>
        /// The height of the row, expressed in units specified by <see cref="UnitType"/>.  This field is read-only.
        /// </summary>
        public readonly double Height;

        /// <summary>
        /// The type of unit that the <see cref="Height"/> value represents.  This field is read-only.
        /// </summary>
        public readonly LayoutUnitType UnitType;

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Row"/> structure.
        /// </summary>
        /// <param name="height">The height of the row, expressed in units specified by <paramref name="unitType"/>.</param>
        /// <param name="unitType">The type of unit that the <paramref name="height"/> value represents.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="height"/> is less than zero.</exception>
        public Row(double height, LayoutUnitType unitType)
        {
            if (height < 0 && unitType != LayoutUnitType.Auto)
            {
                throw new ArgumentOutOfRangeException("height", "Value must be no less than 0 when using Star or Absolute unit types.");
            }

            ActualHeight = 0;
            Height = height;
            UnitType = unitType;
        }

        internal Row(Row row, double actualHeight)
        {
            ActualHeight = actualHeight;
            Height = row.Height;
            UnitType = row.UnitType;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Row"/> is equal to the current <see cref="Row"/>.
        /// </summary>
        /// <param name="row">The <see cref="Row"/> to compare with the current <see cref="Row"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Row"/> is equal to the current
        /// <see cref="Row"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Row row)
        {
            return row.Height == Height && row.UnitType == UnitType;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Row"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Row"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Row"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Row)
            {
                return Equals((Row)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Row"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return Height.GetHashCode() ^ UnitType.GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Height: {0}, Unit Type: {1}", Height, UnitType);
        }

        /// <summary>
        /// Determines whether two <see cref="Row"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Row value1, Row value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="Row"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Row value1, Row value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Adds the specified <paramref name="amount"/> to the height of a <see cref="Row"/>.
        /// </summary>
        /// <param name="row">The row whose height is to be added to.</param>
        /// <param name="amount">The amount to add to the height of the row.</param>
        /// <returns>The result of adding the <paramref name="amount"/> to the row's height.</returns>
        public static Row operator +(Row row, double amount)
        {
            return new Row(row.Height + amount, row.UnitType);
        }

        /// <summary>
        /// Subtracts the specified <paramref name="amount"/> from the height of a <see cref="Row"/>.
        /// </summary>
        /// <param name="row">The row whose height is to be subtracted from.</param>
        /// <param name="amount">The amount to subtract from the height of the row.</param>
        /// <returns>The result of subtracting the <paramref name="amount"/> from the row's height.</returns>
        public static Row operator -(Row row, double amount)
        {
            return new Row(row.Height - amount, row.UnitType);
        }

        /// <summary>
        /// Multiplies the height of a <see cref="Row"/> by the specified <paramref name="amount"/>.
        /// </summary>
        /// <param name="row">The row whose height is to be multiplied.</param>
        /// <param name="amount">The amount to multiply the height of the row.</param>
        /// <returns>The result of multiplying the row's height by the <paramref name="amount"/>.</returns>
        public static Row operator *(Row row, double amount)
        {
            return new Row(row.Height * amount, row.UnitType);
        }

        /// <summary>
        /// Divides the height of a <see cref="Row"/> by the specified <paramref name="amount"/>.
        /// </summary>
        /// <param name="row">The row whose height is to be divided.</param>
        /// <param name="amount">The amount to divide the height of the row.</param>
        /// <returns>The result of dividing the row's height by the <paramref name="amount"/>.</returns>
        public static Row operator /(Row row, double amount)
        {
            return new Row(row.Height / amount, row.UnitType);
        }
    }

    /// <summary>
    /// Represents a collection of <see cref="iFactr.UI.Row"/> objects.
    /// </summary>
#if !NETCF
    [DebuggerDisplay("Count = {Count}")]
#endif
    public sealed class RowCollection : List<Row>
    {
        /// <summary>
        /// Adds a <see cref="iFactr.UI.Row"/> object to the end of the collection.
        /// </summary>
        /// <param name="height">The height of the row, expressed in units specified by <paramref name="unitType"/>.</param>
        /// <param name="unitType">The type of unit that the <paramref name="height"/> value represents.</param>
        public void Add(double height, LayoutUnitType unitType)
        {
            Add(new Row(height, unitType));
        }

        /// <summary>
        /// Inserts a <see cref="iFactr.UI.Row"/> object into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the item should be inserted.</param>
        /// <param name="height">The height of the row, expressed in units specified by <paramref name="unitType"/>.</param>
        /// <param name="unitType">The type of unit that the <paramref name="height"/> value represents.</param>
        public void Insert(int index, double height, LayoutUnitType unitType)
        {
            Insert(index, new Row(height, unitType));
        }
    }
}
