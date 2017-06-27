using System;

namespace iFactr.UI.Reflection
{
    /// <summary>
    /// Represents the text to display when presenting the user control that is representing the member.
    /// Without this attribute, the text that is displayed is the member's name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class LabelAttribute : OptInAttribute
    {
        internal string Text { get; private set; }

        /// <summary>
        /// Gets or sets the color of the label text.
        /// </summary>
        public string ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the name of the font family to be used when rendering the text.
        /// </summary>
        public string FontName { get; set; }


        /// <summary>
        /// Gets or sets the size of the font that is used when rendering the text.
        /// </summary>
        public string FontSize { get; set; }

        /// <summary>
        /// Gets or sets the formatting to apply to the font that is used when rendering the text.
        /// </summary>
        public FontFormatting FontFormatting { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of lines of text that are allowed to be displayed.
        /// A value of 0 means that there is no limit.
        /// </summary>
        public string Lines { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelAttribute"/> class.
        /// </summary>
        /// <param name="text">The text that is to be rendered.</param>
        public LabelAttribute(string text)
        {
            Text = text;
            FontName = Font.PreferredLabelFont.Name;
            FontSize = Font.PreferredLabelFont.Size.ToString();
            FontFormatting = Font.PreferredLabelFont.Formatting;
            Lines = "1";
        }
    }
}
