using System;
using System.Globalization;

/*
This namespace contains all of the classes that represent abstract objects
relating the application style, including color palette and general style or
theme.
*/
namespace iFactr.Core.Styles
{
    /// <summary>
    /// Represents a display style.
    /// </summary>
    public class Style
    {
        /// <summary>
        /// Gets or sets the color for error labels.
        /// </summary>
        /// <value>The color of the error labels.</value>
        public UI.Color ErrorTextColor { get; set; }
        /// <summary>
        /// Gets or sets the color of the header.
        /// </summary>
        /// <value>The color of the header.</value>
        public UI.Color HeaderColor { get; set; }
        /// <summary>
        /// Gets or sets the color of the text on the header.
        /// </summary>
        /// <value>The color of the header text.</value>
        public UI.Color HeaderTextColor { get; set; }
        /// <summary>
        /// Gets or sets the color of the section header.
        /// </summary>
        public UI.Color SectionHeaderColor { get; set; }
        /// <summary>
        /// Gets or sets the color of the text on the section header.
        /// </summary>
        public UI.Color SectionHeaderTextColor { get; set; }
        /// <summary>
        /// Gets or sets the item selection color.
        /// </summary>
        /// <value>The item selection color.</value>
        public UI.Color SelectionColor { get; set; }
        /// <summary>
        /// Gets or sets the color of the layer background.
        /// </summary>
        /// <value>The color of the layer background.</value>
        public UI.Color LayerBackgroundColor { get; set; }
        /// <summary>
        /// Gets or sets the color of the background of each item on the layer.
        /// </summary>
        /// <value>The color of each item's background.</value>
        public UI.Color LayerItemBackgroundColor { get; set; }
        /// <summary>
        /// Gets or sets the color of the separator between each item on the layer.
        /// </summary>
        /// <value>The color of the separator.</value>
        public UI.Color SeparatorColor { get; set; }
        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public UI.Color TextColor { get; set; }
        /// <summary>
        /// Gets or sets the color of the subtext.
        /// </summary>
        /// <value>The color of the subtext.</value>
        public UI.Color SubTextColor { get; set; }
        /// <summary>
        /// Gets or sets the color of the secondary subtext.
        /// </summary>
        /// <value>The color of the secondary subtext.</value>
        public UI.Color SecondarySubTextColor { get; set; }
        /// <summary>
        /// Gets or sets the background transparency of each item in the layer.
        /// </summary>
        /// <value> The item transparency as a <see cref="Byte"/>.</value>
        [Obsolete("Use LayerItemBackgroundColor instead.")]
        public byte LayerItemAlpha
        {
            get { return LayerItemBackgroundColor.A; }
            set { LayerItemBackgroundColor = new Color(value, LayerItemBackgroundColor.R, LayerItemBackgroundColor.G, LayerItemBackgroundColor.B); }
        }
        /// <summary>
        /// Gets or sets the file path to the layer background image.
        /// </summary>
        /// <value>The layer background image as a <see cref="String"/>.</value>
        public string LayerBackgroundImage { get; set; }
        /// <summary>
        /// Gets or sets the dynamic layer background image for when the layer is on the master pane.
        /// If the app is in small form factor, use this property.
        /// </summary>
        public DynamicImage LayerDynamicMasterImage { get; set; }
        /// <summary>
        /// Gets or sets the dynamic layer background image for when the layer is on the detail pane.
        /// </summary>
        public DynamicImage LayerDynamicDetailImage { get; set; }
        /// <summary>
        /// Gets or sets the dynamic layer background image for when the layer is on the popover pane.
        /// </summary>
        public DynamicImage LayerDynamicPopoverImage { get; set; }
        /// <summary>
        /// Gets or sets the default label style.
        /// </summary>
        /// <value>The default label style as a <see cref="LabelStyle"/>.</value>
        public LabelStyle DefaultLabelStyle { get; set; }
        /// <summary>
        /// Gets or sets the animation to use when navigating between layers on the master pane.
        /// Not all animations are supported on every platform.
        /// </summary>
        public Transition MasterTransitionAnimation { get; set; }
        /// <summary>
        /// Gets or sets the animation to use when navigating between layers on the detail pane.
        /// Not all animations are supported on every platform.
        /// </summary>
        public Transition DetailTransitionAnimation { get; set; }
        /// <summary>
        /// Gets or sets the animation to use when navigating between layers on the popover pane.
        /// Not all animations are supported on every platform.
        /// </summary>
        public Transition PopoverTransitionAnimation { get; set; }
        /// <summary>
        /// Gets or sets the direction of the transition animation on the master pane.
        /// Not all directions will work with all animations.
        /// </summary>
        public TransitionDirection MasterTransitionDirection { get; set; }
        /// <summary>
        /// Gets or sets the direction of the transition animation on the detail pane.
        /// Not all directions will work with all animations.
        /// </summary>
        public TransitionDirection DetailTransitionDirection { get; set; }
        /// <summary>
        /// Gets or sets the direction of the transition animation on the popover pane.
        /// Not all directions will work with all animations.
        /// </summary>
        public TransitionDirection PopoverTransitionDirection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Style"/> class.
        /// </summary>
        public Style()
        {
            DefaultLabelStyle = new LabelStyle();

            MasterTransitionAnimation = Transition.Default;
            MasterTransitionDirection = TransitionDirection.Left;
            DetailTransitionAnimation = Transition.Default;
            DetailTransitionDirection = TransitionDirection.Left;
            PopoverTransitionAnimation = Transition.Default;
            PopoverTransitionDirection = TransitionDirection.Left;
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public Style Clone()
        {
            Style style = (Style)MemberwiseClone();
            style.DefaultLabelStyle = DefaultLabelStyle.Clone();
            return style;
        }

        /// <summary>
        /// Represents a color value.
        /// </summary>
        [Obsolete("Use iFactr.UI.Color instead.")]
        public struct Color
        {
            /// <summary>
            /// The alpha component.  This field is readonly.
            /// </summary>
            public readonly byte A;
            /// <summary>
            /// The red component.  This field is readonly.
            /// </summary>
            public readonly byte R;
            /// <summary>
            /// The green component.  This field is readonly.
            /// </summary>
            public readonly byte G;
            /// <summary>
            /// The blue component.  This field is readonly.
            /// </summary>
            public readonly byte B;
            /// <summary>
            /// The hexidecimal value that this instance equates to.  This field is readonly.
            /// </summary>
            public readonly string HexCode;

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
                get { return new Color(255, 0, 255, 0); }
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
                get { return new Color(255, 255, 0, 255); }
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
                get { return new Color(0, 1, 1, 1); }
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
            /// Initializes a new instance of the <see cref="Color"/> struct using the aRGB values provided.
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
                HexCode = String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", A, R, G, B);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Color"/> class using the RGB values provided.
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
                HexCode = String.Format("#{0:X2}{1:X2}{2:X2}", R, G, B);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Color"/> class using the hexidecimal code provided.
            /// </summary>
            /// <param name="hexCode">A <see cref="String"/> representing the Hex code value.</param>
            public Color(string hexCode)
            {
                HexCode = hexCode;
                hexCode = hexCode.Replace("#", string.Empty);

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
                        A = byte.Parse(hexCode[0].ToString() + hexCode[0].ToString(), NumberStyles.AllowHexSpecifier);
                        R = byte.Parse(hexCode[1].ToString() + hexCode[1].ToString(), NumberStyles.AllowHexSpecifier);
                        G = byte.Parse(hexCode[2].ToString() + hexCode[2].ToString(), NumberStyles.AllowHexSpecifier);
                        B = byte.Parse(hexCode[3].ToString() + hexCode[3].ToString(), NumberStyles.AllowHexSpecifier);
                        break;
                    case 3: //#RGB
                        A = byte.MaxValue;
                        R = byte.Parse(hexCode[0].ToString() + hexCode[0].ToString(), NumberStyles.AllowHexSpecifier);
                        G = byte.Parse(hexCode[1].ToString() + hexCode[1].ToString(), NumberStyles.AllowHexSpecifier);
                        B = byte.Parse(hexCode[2].ToString() + hexCode[2].ToString(), NumberStyles.AllowHexSpecifier);
                        break;
                    default:
                        A = byte.MaxValue;
                        R = 0;
                        G = 0;
                        B = 0;
                        break;
                }
            }

            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            /// <param name="obj">Another object to compare to.</param>
            /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
            public override bool Equals(object obj)
            {
                if (obj is Color)
                {
                    return Equals((Color)obj);
                }
                return false;
            }

            /// <summary>
            /// Indicates whether this instance and a specified <see cref="Color"/> object are equal.
            /// </summary>
            /// <param name="color">Another object to compare to.</param>
            /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
            public bool Equals(Color color)
            {
                return A == color.A && R == color.R && G == color.G && B == color.B;
            }

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
            public override int GetHashCode()
            {
                return A << 24 | R << 16 | G << 8 | B;
            }

            /// <summary>
            /// Determines whether two <see cref="Color"/> objects are considered equal.
            /// </summary>
            /// <param name="left">The first object to compare.</param>
            /// <param name="right">The second object to compare.</param>
            /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
            public static bool operator ==(Color left, Color right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Determines whether two <see cref="Color"/> objects are not considered equal.
            /// </summary>
            /// <param name="left">The first object to compare.</param>
            /// <param name="right">The second object to compare.</param>
            /// <returns><c>true</c> if the objects are not considered equal; otherwise <c>false</c>.</returns>
            public static bool operator !=(Color left, Color right)
            {
                return !left.Equals(right);
            }
        }
    }

    /// <summary>
    /// The transition animation type.
    /// </summary>
    public enum Transition
    {
        /// <summary>
        /// The default transition animation.
        /// </summary>
        Default,
        /// <summary>
        /// A curling animation.
        /// </summary>
        Curl,
        /// <summary>
        /// A fading animation.
        /// </summary>
        Fade,
        /// <summary>
        /// A flipping animation.
        /// </summary>
        Flip,
        /// <summary>
        /// A move-in animation.
        /// </summary>
        MoveIn,
        /// <summary>
        /// No animation.
        /// </summary>
        None,
        /// <summary>
        /// A pushing animation.
        /// </summary>
        Push,
        /// <summary>
        /// A revealing animation.
        /// </summary>
        Reveal,
        /// <summary>
        /// An uncurling animation.
        /// </summary>
        Uncurl,
    }

    /// <summary>
    /// The direction of the transition animation.
    /// </summary>
    public enum TransitionDirection
    {
        /// <summary>
        /// The animation is directed downward.
        /// </summary>
        Down,
        /// <summary>
        /// The animation is directed to the left.
        /// </summary>
        Left,
        /// <summary>
        /// The animation is directed to the right.
        /// </summary>
        Right,
        /// <summary>
        /// The animation is directed upward.
        /// </summary>
        Up,
    }
}