using iFactr.Core;

namespace iFactr.UI
{
    /// <summary>
    /// Represents the thickness of the frame around a rectangle.
    /// </summary>
    public struct Thickness
    {
        #region Platform Default Accessors
        /// <summary>
        /// Gets a value that is appropriate for spacing between an element and the bottom edge of its parent container.
        /// This value may differ between platforms.
        /// </summary>
        public static double BottomMargin
        {
            get { return iApp.Factory.PlatformDefaults.BottomMargin; }
        }

        /// <summary>
        /// Gets a value that is appropriate for spacing between an element and the left edge of its parent container.
        /// This value may differ between platforms.
        /// </summary>
        public static double LeftMargin
        {
            get { return iApp.Factory.PlatformDefaults.LeftMargin; }
        }

        /// <summary>
        /// Gets a value that is appropriate for spacing between an element and the right edge of its parent container.
        /// This value may differ between platforms.
        /// </summary>
        public static double RightMargin
        {
            get { return iApp.Factory.PlatformDefaults.RightMargin; }
        }

        /// <summary>
        /// Gets a value that is appropriate for spacing between an element and the top edge of its parent container.
        /// This value may differ between platforms.
        /// </summary>
        public static double TopMargin
        {
            get { return iApp.Factory.PlatformDefaults.TopMargin; }
        }

        /// <summary>
        /// Gets a value that is appropriate for a small amount of horizontal spacing between two elements.
        /// This value may differ between platforms.
        /// </summary>
        public static double SmallHorizontalSpacing
        {
            get { return iApp.Factory.PlatformDefaults.SmallHorizontalSpacing; }
        }

        /// <summary>
        /// Gets a value that is appropriate for a small amount of vertical spacing between two elements.
        /// This value may differ between platforms.
        /// </summary>
        public static double SmallVerticalSpacing
        {
            get { return iApp.Factory.PlatformDefaults.SmallVerticalSpacing; }
        }

        /// <summary>
        /// Gets a value that is appropriate for a large amount of horizontal spacing between two elements.
        /// This value may differ between platforms.
        /// </summary>
        public static double LargeHorizontalSpacing
        {
            get { return iApp.Factory.PlatformDefaults.LargeHorizontalSpacing; }
        }

        /// <summary>
        /// Gets a value that is appropriate for a large amount of vertical spacing between two elements.
        /// This value may differ between platforms.
        /// </summary>
        public static double LargeVerticalSpacing
        {
            get { return iApp.Factory.PlatformDefaults.LargeVerticalSpacing; }
        }
        #endregion

        /// <summary>
        /// The thickness of the lower edge of the rectangle.
        /// </summary>
        public double Bottom;

        /// <summary>
        /// The thickness of the left edge of the rectangle.
        /// </summary>
        public double Left;

        /// <summary>
        /// The thickness of the right edge of the rectangle.
        /// </summary>
        public double Right;

        /// <summary>
        /// The thickness of the upper edge of the rectangle.
        /// </summary>
        public double Top;

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure.
        /// </summary>
        /// <param name="uniformLength">The length of all four edges of the rectangle.</param>
        public Thickness(double uniformLength)
        {
            Left = Top = Right = Bottom = uniformLength;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure.
        /// </summary>
        /// <param name="horizontalLength">The length of the left and right edges of the rectangle.</param>
        /// <param name="verticalLength">The length of the upper and lower edges of the rectangle.</param>
        public Thickness(double horizontalLength, double verticalLength)
        {
            Left = Right = horizontalLength;
            Top = Bottom = verticalLength;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure.
        /// </summary>
        /// <param name="left">The thickness of the left edge of the rectangle.</param>
        /// <param name="top">The thickness of the upper edge of the rectangle.</param>
        /// <param name="right">The thickness of the right edge of the rectangle.</param>
        /// <param name="bottom">The thickness of the lower edge of the rectangle.</param>
        public Thickness(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Thickness"/> is equal to the current <see cref="Thickness"/>.
        /// </summary>
        /// <param name="other">The <see cref="Thickness"/> to compare with the current <see cref="Thickness"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Thickness"/> is equal to the current
        /// <see cref="Thickness"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Thickness other)
        {
            return Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Thickness"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Thickness"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Thickness"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Thickness)
            {
                return Equals((Thickness)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Thickness"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return Left.GetHashCode() ^ Top.GetHashCode() ^ Right.GetHashCode() ^ Bottom.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Thickness"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Thickness"/>.</returns>
        public override string ToString()
        {
            return string.Format("Left: {0}, Top: {1}, Right: {2}, Bottom: {3}", Left, Top, Right, Bottom);
        }

        /// <summary>
        /// Determines whether two <see cref="Thickness"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Thickness value1, Thickness value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="Thickness"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Thickness value1, Thickness value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Adds the specified amount to each edge of a <see cref="Thickness"/> object.
        /// </summary>
        /// <param name="value">The object whose edges are to be incremented.</param>
        /// <param name="amount">The Thickness to add.</param>
        /// <returns>A <see cref="Thickness"/> object containing the sum of each edge and the amount.</returns>
        public static Thickness operator +(Thickness value, Thickness amount)
        {
            return new Thickness(value.Left + amount.Left, value.Top + amount.Top, value.Right + amount.Right, value.Bottom + amount.Bottom);
        }

        /// <summary>
        /// Subtracts each edge of a <see cref="Thickness"/> object by the specified amount.
        /// </summary>
        /// <param name="value">The object whose edges are to be decremented.</param>
        /// <param name="amount">The amount to subtract from each edge.</param>
        /// <returns>A <see cref="Thickness"/> object containing the difference of each edge and the amount.</returns>
        public static Thickness operator -(Thickness value, Thickness amount)
        {
            return new Thickness(value.Left - amount.Left, value.Top - amount.Top, value.Right - amount.Right, value.Bottom - amount.Bottom);
        }

        /// <summary>
        /// Adds the specified amount to each edge of a <see cref="Thickness"/> object.
        /// </summary>
        /// <param name="value">The object whose edges are to be incremented.</param>
        /// <param name="amount">The amount to add to each edge.</param>
        /// <returns>A <see cref="Thickness"/> object containing the sum of each edge and the amount.</returns>
        public static Thickness operator +(Thickness value, double amount)
        {
            return new Thickness(value.Left + amount, value.Top + amount, value.Right + amount, value.Bottom + amount);
        }

        /// <summary>
        /// Subtracts each edge of a <see cref="Thickness"/> object by the specified amount.
        /// </summary>
        /// <param name="value">The object whose edges are to be decremented.</param>
        /// <param name="amount">The amount to subtract from each edge.</param>
        /// <returns>A <see cref="Thickness"/> object containing the difference of each edge and the amount.</returns>
        public static Thickness operator -(Thickness value, double amount)
        {
            return new Thickness(value.Left - amount, value.Top - amount, value.Right - amount, value.Bottom - amount);
        }

        /// <summary>
        /// Multiplies each edge of a <see cref="Thickness"/> object by a scalar value.
        /// </summary>
        /// <param name="value">The object whose edges are to be scaled.</param>
        /// <param name="scalar">The amount to multiply each edge.</param>
        /// <returns>A <see cref="Thickness"/> object containing the product of each edge and the scalar value.</returns>
        public static Thickness operator *(Thickness value, double scalar)
        {
            return new Thickness(value.Left * scalar, value.Top * scalar, value.Right * scalar, value.Bottom * scalar);
        }

        /// <summary>
        /// Divides each edge of a <see cref="Thickness"/> object by a scalar value.
        /// </summary>
        /// <param name="value">The object whose edges are to be scaled.</param>
        /// <param name="scalar">The amount to divide each edge.</param>
        /// <returns>A <see cref="Thickness"/> object containing the quotient of each edge and the scalar value.</returns>
        public static Thickness operator /(Thickness value, double scalar)
        {
            return new Thickness(value.Left / scalar, value.Top / scalar, value.Right / scalar, value.Bottom / scalar);
        }
    }
}