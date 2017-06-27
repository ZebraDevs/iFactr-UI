using System;

namespace iFactr.Core.Styles
{
    /// <summary>
    /// Represents the styling of a label.
    /// </summary>
    public class LabelStyle
    {
        /// <summary>
        /// The alignment of a label.
        /// </summary>
        public enum Align
        {
            /// <summary>
            /// The label is aligned to the center.
            /// </summary>
            Center,
            /// <summary>
            /// The label is aligned to the left.
            /// </summary>
            Left,
            /// <summary>
            /// The label is aligned to the right.
            /// </summary>
            Right,
        }
        /// <summary>
        /// The formatting of a label.
        /// </summary>
        [Flags]
        public enum Format
        {
            /// <summary>
            /// The label is displayed with non-bold and non-italic font.
            /// </summary>
            Normal = 0x0,
            /// <summary>
            /// The label is displayed with a bold font.
            /// </summary>
            Bold = 0x1,
            /// <summary>
            /// The label is displayed with an italic font.
            /// </summary>
            Italic = 0x2,
            /// <summary>
            /// The label is displayed with a bold and italic font.
            /// </summary>
            BoldItalic = 0x3,
        }

        /// <summary>
        /// Gets or sets how many levels deep this instance is in the header.
        /// </summary>
        public ushort HeaderLevel
        {
            get { return _headerLevel; }
            set
            {
                _headerLevel = value > 6 ? (ushort)6 : value;
            }
        }
        private ushort _headerLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelStyle"/> class.
        /// </summary>
        public LabelStyle()
        {
            FontFamily = "Arial";
            FontSize = 10;
            TextAlign = Align.Left;
            TextFormat = Format.Normal;
            WordWrap = false;
        }

        /// <summary>
        /// Gets or sets whether to word-wrap the label text.
        /// </summary>
        /// <value><c>true</c> if the label text is to be word-wrapped; otherwise, <c>false</c>.</value>
        public bool WordWrap { get; set; }

        private int _maxLines;
        /// <summary>
        /// Gets or sets the maximum number of lines of text allowed to display.
        /// </summary>
        /// <value>The maximum number of lines of text as an <see cref="Int32"/>.</value>
        public int MaxLines
        {
            get { return WordWrap ? _maxLines : 1; }
            set { _maxLines = value; }
        }

        /// <summary>
        /// Gets or sets the font family of the text.
        /// </summary>
        /// <value>The font family of the text as a <see cref="String"/>.</value>
        public string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>The size of the font.</value>
        public double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        /// <value>The text alignment as an <see cref="Align"/> value.</value>
        public Align TextAlign { get; set; }

        /// <summary>
        /// Gets or sets the formatting of the text.
        /// </summary>
        /// <value>The formatting of the text as a <see cref="Format"/> value.</value>
        public Format TextFormat { get; set; }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public LabelStyle Clone()
        {
            return (LabelStyle)MemberwiseClone();
        }
    }
}