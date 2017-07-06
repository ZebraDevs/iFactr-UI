using System;
using iFactr.Core;
using iFactr.Core.Targets;

namespace iFactr.UI
{
    /// <summary>
    /// Represents the visual characteristics for a string of text.
    /// </summary>
    public struct Font
    {
        #region Default Fonts
        /// <summary>
        /// Gets the font that is preferred for label text on the target platform.
        /// </summary>
        public static Font PreferredLabelFont
        {
            get { return iApp.Factory.PlatformDefaults.LabelFont; }
        }

        /// <summary>
        /// Gets the font that is preferred for labels that act as headers on the target platform.
        /// </summary>
        public static Font PreferredHeaderFont
        {
            get { return iApp.Factory.PlatformDefaults.HeaderFont; }
        }

        /// <summary>
        /// Gets the font that is preferred for labels that act as the body of messages on the target platform.
        /// </summary>
        public static Font PreferredMessageBodyFont
        {
            get { return iApp.Factory.PlatformDefaults.MessageBodyFont; }
        }

        /// <summary>
        /// Gets the font that is preferred for labels that act as the title for messages on the target platform.
        /// </summary>
        public static Font PreferredMessageTitleFont
        {
            get { return iApp.Factory.PlatformDefaults.MessageTitleFont; }
        }

        /// <summary>
        /// Gets the font that is preferred for small label text on the target platform.
        /// </summary>
        public static Font PreferredSmallFont
        {
            get { return iApp.Factory.PlatformDefaults.SmallFont; }
        }

        /// <summary>
        /// Gets the font that is preferred for labels showing data values on the target platform.
        /// </summary>
        public static Font PreferredValueFont
        {
            get { return iApp.Factory.PlatformDefaults.ValueFont; }
        }

        /// <summary>
        /// Gets the font that is preferred for button controls on the target platform.
        /// </summary>
        public static Font PreferredButtonFont
        {
            get { return iApp.Factory.PlatformDefaults.ButtonFont; }
        }

        /// <summary>
        /// Gets the font that is preferred for date and time pickers on the target platform.
        /// </summary>
        public static Font PreferredDateTimePickerFont
        {
            get { return iApp.Factory.PlatformDefaults.DateTimePickerFont; }
        }

        /// <summary>
        /// Gets the font that is preferred for select lists on the target platform.
        /// </summary>
        public static Font PreferredSelectListFont
        {
            get { return iApp.Factory.PlatformDefaults.SelectListFont; }
        }

        /// <summary>
        /// Gets the font that is preferred for text boxes and password boxes on the target platform.
        /// </summary>
        public static Font PreferredTextBoxFont
        {
            get { return iApp.Factory.PlatformDefaults.TextBoxFont; }
        }

        /// <summary>
        /// Gets the font that is preferred for section header text on the target platform.
        /// </summary>
        public static Font PreferredSectionHeaderFont
        {
            get { return iApp.Factory.PlatformDefaults.SectionHeaderFont; }
        }

        /// <summary>
        /// Gets the font that is preferred for section footer text on the target platform.
        /// </summary>
        public static Font PreferredSectionFooterFont
        {
            get { return iApp.Factory.PlatformDefaults.SectionFooterFont; }
        }

        /// <summary>
        /// Gets the font that is preferred for tab items on the target platform.
        /// </summary>
        public static Font PreferredTabFont
        {
            get { return iApp.Factory.PlatformDefaults.TabFont; }
        }
        #endregion

        /// <summary>
        /// Any special formatting applied to the font.
        /// </summary>
        public FontFormatting Formatting;

        /// <summary>
        /// The name of the font family.
        /// </summary>
        public string Name;

        /// <summary>
        /// The size of the text characters.
        /// </summary>
        public double Size;

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Font"/> structure.
        /// </summary>
        /// <param name="name">The name of the font family.</param>
        public Font(string name)
            : this(name, PreferredLabelFont.Size, FontFormatting.Normal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Font"/> structure.
        /// </summary>
        /// <param name="name">The name of the font family.</param>
        /// <param name="size">The size of the text characters.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="size"/> is less than 0.</exception>
        public Font(string name, double size)
            : this(name, size, FontFormatting.Normal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Font"/> structure.
        /// </summary>
        /// <param name="name">The name of the font family.</param>
        /// <param name="size">The size of the text characters.</param>
        /// <param name="formatting">Any special formatting applied to the font.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="size"/> is less than 0.</exception>
        public Font(string name, double size, FontFormatting formatting)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size", "Size cannot be less than 0.");
            }

            Name = string.IsNullOrEmpty(name) ? PreferredLabelFont.Name : name;
            Size = size;
            Formatting = formatting;
        }

        /// <summary>
        /// Measures the amount of vertical space that a single line of text will take up when it is rendered.
        /// </summary>
        /// <returns>The render height of a single line of text.</returns>
        public double GetLineHeight()
        {
            return iApp.Factory.GetLineHeight(this);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Font"/> is equal to the current <see cref="Font"/>.
        /// </summary>
        /// <param name="other">The <see cref="Font"/> to compare with the current <see cref="Font"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Font"/> is equal to the current
        /// <see cref="Font"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Font other)
        {
            return Name == other.Name && Size == other.Size && Formatting == other.Formatting;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Font"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Font"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Font"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Font)
            {
                return Equals((Font)obj);
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Font"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Size.GetHashCode() ^ Formatting.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Name: {0}, Size: {1}, Formatting: {2}", Name, Size, Formatting);
        }

        /// <summary>
        /// Determines whether two <see cref="Font"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Font value1, Font value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="Font"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Font value1, Font value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Adds the specified <paramref name="amount"/> to the size of a <see cref="Font"/>.
        /// </summary>
        /// <param name="font">The font whose size is to be added to.</param>
        /// <param name="amount">The amount to add to the size of the font.</param>
        /// <returns>The result of adding the <paramref name="amount"/> to the font's size.</returns>
        public static Font operator +(Font font, double amount)
        {
            return new Font(font.Name, font.Size + amount, font.Formatting);
        }

        /// <summary>
        /// Subtracts the specified <paramref name="amount"/> from the size of a <see cref="Font"/>.
        /// </summary>
        /// <param name="font">The font whose size is to be subtracted from.</param>
        /// <param name="amount">The amount to subtract from the size of the font.</param>
        /// <returns>The result of subtracting the <paramref name="amount"/> from the font's size.</returns>
        public static Font operator -(Font font, double amount)
        {
            return new Font(font.Name, font.Size - amount, font.Formatting);
        }

        /// <summary>
        /// Multiplies the size of a <see cref="Font"/> by the specified <paramref name="amount"/>.
        /// </summary>
        /// <param name="font">The font whose size is to be multiplied.</param>
        /// <param name="amount">The amount to multiply the size of the font.</param>
        /// <returns>The result of multiplying the font's size by the <paramref name="amount"/>.</returns>
        public static Font operator *(Font font, double amount)
        {
            return new Font(font.Name, font.Size * amount, font.Formatting);
        }

        /// <summary>
        /// Divides the size of a <see cref="Font"/> by the specified <paramref name="amount"/>.
        /// </summary>
        /// <param name="font">The font whose size is to be divided.</param>
        /// <param name="amount">The amount to divide the size of the font.</param>
        /// <returns>The result of dividing the font's size by the <paramref name="amount"/>.</returns>
        public static Font operator /(Font font, double amount)
        {
            return new Font(font.Name, font.Size / amount, font.Formatting);
        }
    }

    /// <summary>
    /// Describes special formatting characteristics to be applied to a font.
    /// </summary>
    [Flags]
    public enum FontFormatting : byte
    {
        /// <summary>
        /// No special formatting is applied.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// The text characters are bolded.
        /// </summary>
        Bold = 1,
        /// <summary>
        /// The text characters are italicized.
        /// </summary>
        Italic = 2,
        /// <summary>
        /// The text characters are bolded and italicized.
        /// </summary>
        BoldItalic = 3
    }
}

