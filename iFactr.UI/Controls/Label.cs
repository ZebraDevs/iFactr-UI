using System;
using iFactr.Core.Styles;

namespace iFactr.Core.Controls
{
    /// <summary>
    /// Represents a text label within an <see cref="iFactr.Core.Layers.iBlock"/> or <see cref="iFactr.Core.Layers.iPanel"/> control.
    /// </summary>
    public class Label : PanelItem
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        public Label()
        {
            Style = new LabelStyle();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="style">The <see cref="Style"/> instance to style this object with.</param>
        public Label(LabelStyle style)
        {
            Style = style.Clone();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="style">The <see cref="Style"/> instance with a label style.</param>
        public Label(Style style) : this(style.DefaultLabelStyle) { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of this instance.
        /// </summary>
        /// <value>The name of this instance as a <see cref="String"/> value.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the label style of this instance.
        /// </summary>
        /// <value>The label style as a <see cref="LabelStyle"/>.</value>
        public LabelStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the text to display.
        /// </summary>
        /// <value>The text to display as a <see cref="String"/> value.</value>
        public string Text { get; set; }

        #endregion

        #region IBlockPanelItem methods

        /// <summary>
        /// Returns an HTML representation of this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> representing this instance in HTML.</returns>
        public override string GetHtml()
        {
            string format = "{0}";//, style;
            if (Style.HeaderLevel > 0)
                format = string.Format("<h{1}>{0}</h{1}>", format, Style.HeaderLevel);

            switch (Style.TextFormat)
            {
                case LabelStyle.Format.Bold:
                    format = string.Format("<b>{0}</b> ", format);
                    break;
                case LabelStyle.Format.Italic:
                    format = string.Format("<i>{0}</i> ", format);
                    break;
                case LabelStyle.Format.BoldItalic:
                    format = string.Format("<b><i>{0}</i></b> ", format);
                    break;
            }

            //switch (Style.TextAlign)
            //{
            //    case LabelStyle.Align.Center:
            //        style = "center";
            //        break;
            //    case LabelStyle.Align.Right:
            //        style = "right";
            //        break;
            //    default:
            //        style = string.Empty;
            //        break;
            //}
            //if (!string.IsNullOrWhiteSpace(style))
            //    format = string.Format("<div style=\"text-align:{1}\">{0}</div>", format, style);

            return string.Format(format, Text);
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new Label Clone()
        {
            return (Label)CloneOverride();
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        protected override object CloneOverride()
        {
            var clone = (Label)base.CloneOverride();
            clone.Style = Style.Clone();
            return clone;
        }

        #endregion
    }
}