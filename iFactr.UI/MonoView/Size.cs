namespace iFactr.UI
{
    /// <summary>
    /// Represents the width and the height of an object.
    /// </summary>
    public struct Size
    {
        /// <summary>
        /// A <see cref="iFactr.UI.Size"/> structure with no width and no height.
        /// </summary>
        public static readonly Size Empty = new Size();

        /// <summary>
        /// The height of this instance.
        /// </summary>
        public double Height;

        /// <summary>
        /// The width of this instance.
        /// </summary>
        public double Width;

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return Equals(Empty); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Size"/> structure.
        /// </summary>
        /// <param name="width">The initial width of the structure.</param>
        /// <param name="height">The initial height of the structure.</param>
        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Determines whether the specified <see cref="iFactr.UI.Size"/> is equal to the current <see cref="iFactr.UI.Size"/>.
        /// </summary>
        /// <param name="other">The <see cref="iFactr.UI.Size"/> to compare with the current <see cref="iFactr.UI.Size"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="iFactr.UI.Size"/> is equal to the current
        /// <see cref="iFactr.UI.Size"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Size other)
        {
            return Height == other.Height && Width == other.Width;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="iFactr.UI.Size"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="iFactr.UI.Size"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="iFactr.UI.Size"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Size)
            {
                return Equals((Size)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="iFactr.UI.Size"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
		public override int GetHashCode()
		{
			return Width.GetHashCode() ^ Height.GetHashCode();
		}

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="iFactr.UI.Size"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="iFactr.UI.Size"/>.</returns>
        public override string ToString()
        {
            return string.Format("Width: {0}, Height: {1}", Width, Height);
        }

        /// <summary>
        /// Determines whether two <see cref="iFactr.UI.Size"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Size value1, Size value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="iFactr.UI.Size"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Size value1, Size value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Adds the widths and heights of two <see cref="iFactr.UI.Size"/> objects together.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="iFactr.UI.Size"/> object containing the sum
        /// of both widths and the sum of both heights.</returns>
        public static Size operator +(Size value1, Size value2)
        {
            return new Size(value1.Width + value2.Width, value1.Height + value2.Height);
        }

        /// <summary>
        /// Subtracts the width and height of a <see cref="iFactr.UI.Size"/> object
        /// by the width and height of another <see cref="iFactr.UI.Size"/> object.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="iFactr.UI.Size"/> object containing the difference
        /// of both widths and the difference of both heights.</returns>
        public static Size operator -(Size value1, Size value2)
        {
            return new Size(value1.Width - value2.Width, value1.Height - value2.Height);
        }

        /// <summary>
        /// Multiplies the widths and heights of two <see cref="iFactr.UI.Size"/> objects together.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="iFactr.UI.Size"/> object containing the product
        /// of both widths and the product of both heights.</returns>
        public static Size operator *(Size value1, Size value2)
        {
            return new Size(value1.Width * value2.Width, value1.Height * value2.Height);
        }

        /// <summary>
        /// Multiplies the width and height of a <see cref="Size"/> object by a scalar value.
        /// </summary>
        /// <param name="value">The object whose width and height are to be scaled.</param>
        /// <param name="scalar">The amount to multiply the width and height by.</param>
        /// <returns>A <see cref="iFactr.UI.Size"/> object containing the product of the width
        /// and scalar value and the product of the height and scalar value.</returns>
        public static Size operator *(Size value, double scalar)
        {
            return new Size(value.Width * scalar, value.Height * scalar);
        }

        /// <summary>
        /// Divides the width and height of a <see cref="iFactr.UI.Size"/> object by
        /// the width and height of another <see cref="iFactr.UI.Size"/> object.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="iFactr.UI.Size"/> object containing the quotient
        /// of both widths and the quotient of both heights.</returns>
        public static Size operator /(Size value1, Size value2)
        {
            return new Size(value1.Width / value2.Width, value1.Height / value2.Height);
        }

        /// <summary>
        /// Divides the width and height of a <see cref="Size"/> object by a scalar value.
        /// </summary>
        /// <param name="value">The object whose width and height are to be scaled.</param>
        /// <param name="scalar">The amount to divide the width and height by.</param>
        /// <returns>A <see cref="iFactr.UI.Size"/> object containing the quotient of the width
        /// and scalar value and the quotient of the height and scalar value.</returns>
        public static Size operator /(Size value, double scalar)
        {
            return new Size(value.Width / scalar, value.Height / scalar);
        }
    }
}

