using System;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a navigation element with a fixed height and multiple lines of text and subtext.
    /// </summary>
    public class ContentItem : iItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentItem"/> class.
        /// </summary>
        public ContentItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        public ContentItem(string linkAddress, string text) : base(linkAddress, text) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="async">If <c>true</c>, sets the link rev value to Async.</param>
        public ContentItem(string linkAddress, string text, bool async) : base(linkAddress, text, async) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        public ContentItem(string linkAddress, string text, string subText) : base(linkAddress, text, subText) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        /// <param name="async">If <c>true</c>, sets the link rev value to Async.</param>
        public ContentItem(string linkAddress, string text, string subText, bool async) : base(linkAddress, text, subText, async) { }
    }
}