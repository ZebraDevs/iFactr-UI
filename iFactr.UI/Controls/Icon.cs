using System;
using iFactr.Core.Layers;

namespace iFactr.Core.Controls
{
    /// <summary>
    /// Represents a control for displaying images.
    /// </summary>
    public class Icon : PanelItem
    {
        #region Properties
        /// <summary>
        /// Gets or sets the alignment of the icon.
        /// </summary>
        /// <value>The alignment as a <see cref="String"/> value.</value>
        public string Align { get; set; }
        /// <summary>
        /// Gets or sets the display height of the icon.
        /// </summary>
        /// <value>The height as a <see cref="String"/> value.</value>
        public string Height { get; set; }
        /// <summary>
        /// Gets or sets the URI location of the icon image.
        /// </summary>
        /// <value>The location as a <see cref="String"/> value.</value>
        public string Location { get; set; }
        /// <summary>
        /// Gets or sets the name of the control.
        /// </summary>
        /// <value>The name as a <see cref="String"/> value.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the display width of the icon.
        /// </summary>
        /// <value>The width as a <see cref="String"/> value.</value>
        public string Width { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Icon"/> class.
        /// </summary>
        public Icon()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Icon"/> class.
        /// </summary>
        /// <param name="location">The URI location of the image to display.</param>
        public Icon(string location)
        {
            Location = location;
        }

        /// <summary>
        /// Returns an HTML representation of this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> representing this instance in HTML.</returns>
        public override string GetHtml()
        {
            return "<img "
             + (Name == null ? null : ("title=\"" + Name + "\" "))
             + (Align == null ? null : ("align=\"" + Align + "\" "))
#if NETCF
             + (Width == null ? null : "width=\"" + Width + "\" ")
             + (Height == null ? null : "height=\"" + Height + "\" ")
             + "style=\"padding:3px\" src=\""
#else
             + "style=\"padding:5px;max-height:"
             + (Height ?? "none")
             + ";max-width:"
             + (Width ?? "100%")
             + "\" src=\""
#endif
             + HtmlTextExtensions.VirtualPath(Location)
             + "\"   />";
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new Icon Clone()
        {
            return (Icon)CloneOverride();
        }
    }
}