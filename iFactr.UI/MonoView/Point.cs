namespace iFactr.UI
{
    /// <summary>
    /// Represents an X- and Y-coordinate pair in two-dimensional space.
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// The X coordinate of this instance.
        /// </summary>
        public double X;

        /// <summary>
        /// The Y coordinate of this instance.
        /// </summary>
        public double Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> structure.
        /// </summary>
        /// <param name="x">The initial X coordinate of the structure.</param>
        /// <param name="y">The initial Y coordinate of the structure.</param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Point"/> is equal to the current <see cref="Point"/>.
        /// </summary>
        /// <param name="other">The <see cref="Point"/> to compare with the current <see cref="Point"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Point"/> is equal to the current
        /// <see cref="Point"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Point"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Point"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Point"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                return Equals((Point)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Point"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Point"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Point"/>.</returns>
        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", X, Y);
        }

        /// <summary>
        /// Determines whether two <see cref="Point"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Point value1, Point value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="Point"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Point value1, Point value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Adds the Xs and Ys of two <see cref="Point"/> objects together.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Point"/> object containing the sum of both Xs and the sum of both Ys.</returns>
        public static Point operator +(Point value1, Point value2)
        {
            return new Point(value1.X + value2.X, value1.Y + value2.Y);
        }

        /// <summary>
        /// Subtracts the X and Y of a <see cref="Point"/> object from the X and Y of another <see cref="Point"/> object.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Point"/> object containing the difference of both Xs and the difference of both Ys.</returns>
        public static Point operator -(Point value1, Point value2)
        {
            return new Point(value1.X - value2.X, value1.Y - value2.Y);
        }

        /// <summary>
        /// Multiplies the Xs and Ys of two <see cref="Point"/> objects together.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Point"/> object containing the product of both Xs and the product of both Ys.</returns>
        public static Point operator *(Point value1, Point value2)
        {
            return new Point(value1.X * value2.X, value1.Y * value2.Y);
        }

        /// <summary>
        /// Divides the X and Y of a <see cref="Point"/> object by the X and Y of another <see cref="Point"/> object.
        /// </summary>
        /// <param name="value1">The left argument.</param>
        /// <param name="value2">The right argument.</param>
        /// <returns>A <see cref="Point"/> object containing the quotient of both Xs and the quotient of both Ys.</returns>
        public static Point operator /(Point value1, Point value2)
        {
            return new Point(value1.X / value2.X, value1.Y / value2.Y);
        }
    }
}
