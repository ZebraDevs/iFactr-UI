using System;
using System.Globalization;
using iFactr.Core.Styles;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a color value with red, green, blue, and alpha components.
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// The alpha component.
        /// </summary>
        public byte A;

        /// <summary>
        /// The red component.
        /// </summary>
        public byte R;

        /// <summary>
        /// The green component.
        /// </summary>
        public byte G;

        /// <summary>
        /// The blue component.
        /// </summary>
        public byte B;

        /// <summary>
        /// Gets or sets the hexidecimal value that this instance equates to.
        /// </summary>
        public string HexCode
        {
            get
            {
                return String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", A, R, G, B);
            }
            set
            {
                string hexCode = value.Replace("#", string.Empty);
                
                switch (hexCode.Length)
                {
                    case 8: //#AARRGGBB
                        A = byte.Parse(hexCode.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                        R = byte.Parse(hexCode.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                        G = byte.Parse(hexCode.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                        B = byte.Parse(hexCode.Substring(6, 2), NumberStyles.AllowHexSpecifier);
                        break;
                    case 6: //#RRGGBB
                        A = byte.MaxValue;
                        R = byte.Parse(hexCode.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                        G = byte.Parse(hexCode.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                        B = byte.Parse(hexCode.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                        break;
                    case 4: //#ARGB
                        A = byte.Parse(hexCode[0].ToString() + hexCode[0], NumberStyles.AllowHexSpecifier);
                        R = byte.Parse(hexCode[1].ToString() + hexCode[1], NumberStyles.AllowHexSpecifier);
                        G = byte.Parse(hexCode[2].ToString() + hexCode[2], NumberStyles.AllowHexSpecifier);
                        B = byte.Parse(hexCode[3].ToString() + hexCode[3], NumberStyles.AllowHexSpecifier);
                        break;
                    case 3: //#RGB
                        A = byte.MaxValue;
                        R = byte.Parse(hexCode[0].ToString() + hexCode[0], NumberStyles.AllowHexSpecifier);
                        G = byte.Parse(hexCode[1].ToString() + hexCode[1], NumberStyles.AllowHexSpecifier);
                        B = byte.Parse(hexCode[2].ToString() + hexCode[2], NumberStyles.AllowHexSpecifier);
                        break;
                    default:
                        A = byte.MaxValue;
                        R = 0;
                        G = 0;
                        B = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance represents a null color value (#00000000).
        /// Using a default color value to set a property on a UI element will reset that property to
        /// the current platform's default value.
        /// </summary>
        public bool IsDefaultColor
        {
            get { return A + R + G + B == 0; }
        }
        
        #region Static Colors
        /// <summary>
        /// Gets a black color.
        /// </summary>
        public static Color Black
        {
            get { return new Color(255, 0, 0, 0); }
        }
        
        /// <summary>
        /// Gets a blue color.
        /// </summary>
        public static Color Blue
        {
            get { return new Color(255, 0, 0, 255); }
        }
        
        /// <summary>
        /// Gets a brown color.
        /// </summary>
        public static Color Brown
        {
            get { return new Color(255, 160, 82, 45); }
        }
        
        /// <summary>
        /// Gets a cyan color.
        /// </summary>
        public static Color Cyan
        {
            get { return new Color(255, 0, 255, 255); }
        }
        
        /// <summary>
        /// Gets a gray color.
        /// </summary>
        public static Color Gray
        {
            get { return new Color(255, 128, 128, 128); }
        }
        
        /// <summary>
        /// Gets a green color.
        /// </summary>
        public static Color Green
        {
            get { return new Color(255, 0, 128, 0); }
        }
        
        /// <summary>
        /// Gets an orange color.
        /// </summary>
        public static Color Orange
        {
            get { return new Color(255, 255, 140, 0); }
        }
        
        /// <summary>
        /// Gets a purple color.
        /// </summary>
        public static Color Purple
        {
            get { return new Color(255, 128, 0, 192); }
        }
        
        /// <summary>
        /// Gets a red color.
        /// </summary>
        public static Color Red
        {
            get { return new Color(255, 255, 0, 0); }
        }
        
        /// <summary>
        /// Gets a transparent color.
        /// </summary>
        public static Color Transparent
        {
            get { return new Color(0, 255, 255, 255); }
        }
        
        /// <summary>
        /// Gets a white color.
        /// </summary>
        public static Color White
        {
            get { return new Color(255, 255, 255, 255); }
        }
        
        /// <summary>
        /// Gets a yellow color.
        /// </summary>
        public static Color Yellow
        {
            get { return new Color(255, 255, 255, 0); }
        }
#endregion
        
        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Color"/> structure using the ARGB values provided.
        /// </summary>
        /// <param name="alpha">The alpha component value.</param>
        /// <param name="red">The red component value.</param>
        /// <param name="green">The green component value.</param>
        /// <param name="blue">The blue component value.</param>
        public Color(byte alpha, byte red, byte green, byte blue)
        {
            A = alpha;
            R = red;
            G = green;
            B = blue;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Color"/> structure using the RGB values provided.
        /// </summary>
        /// <param name="red">The red component value.</param>
        /// <param name="green">The green component value.</param>
        /// <param name="blue">The blue component value.</param>
        public Color(byte red, byte green, byte blue)
        {
            A = byte.MaxValue;
            R = red;
            G = green;
            B = blue;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Color"/> structure using the hexidecimal code provided.
        /// </summary>
        /// <param name="hexCode">A <see cref="String"/> representing the Hex code value.</param>
        public Color(string hexCode)
        {
            A = R = B = G = 0;
            HexCode = hexCode;
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="iFactr.UI.Color"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="iFactr.UI.Color"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="iFactr.UI.Color"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Color)
            {
                return Equals((Color)obj);
            }
            return false;
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="iFactr.UI.Color"/> is equal to the current <see cref="iFactr.UI.Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="iFactr.UI.Color"/> to compare with the current <see cref="iFactr.UI.Color"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="iFactr.UI.Color"/> is equal to the current
        /// <see cref="iFactr.UI.Color"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Color color)
        {
            return A == color.A && R == color.R && G == color.G && B == color.B;
        }
        
        /// <summary>
        /// Serves as a hash function for a <see cref="iFactr.UI.Color"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return A << 24 | R << 16 | G << 8 | B;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="iFactr.UI.Color"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="iFactr.UI.Color"/>.</returns>
        public override string ToString()
        {
            return HexCode;
        }
        
        /// <summary>
        /// Determines whether two <see cref="iFactr.UI.Color"/> objects are considered equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }
        
        /// <summary>
        /// Determines whether two <see cref="iFactr.UI.Color"/> objects are not considered equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are not considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }

#pragma warning disable 618

        /// <summary>
        /// Implicitly converts the specified <see cref="iFactr.Core.Styles.Style.Color"/> object into a <see cref="iFactr.UI.Color"/> object.
        /// </summary>
        /// <param name="color">The object to be converted.</param>
        /// <returns>The converted object.</returns>
        public static implicit operator Color(Style.Color color)
        {
            return new Color(color.HexCode);
        }

        /// <summary>
        /// Implicitly converts the specified <see cref="iFactr.UI.Color"/> object into a <see cref="iFactr.Core.Styles.Style.Color"/> object.
        /// </summary>
        /// <param name="color">The object to be converted.</param>
        /// <returns>The converted object.</returns>
        public static implicit operator Style.Color(Color color)
        {
            return new Style.Color(color.HexCode);
        }

#pragma warning restore 618
    }
}