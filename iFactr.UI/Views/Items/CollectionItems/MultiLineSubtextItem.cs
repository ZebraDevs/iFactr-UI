using System;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a navigation element with variable height, a single line of text, and multiple lines of subtext.
    /// </summary>
    public class MultiLineSubtextItem : iItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineSubtextItem"/> class.
        /// </summary>
        public MultiLineSubtextItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineSubtextItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        public MultiLineSubtextItem(string linkAddress, string text, string subText) : base(linkAddress, text, subText) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineSubtextItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        /// <param name="async">If <c>true</c>, sets the link rev value to Async.</param>
        public MultiLineSubtextItem(string linkAddress, string text, string subText, bool async) : base(linkAddress, text, subText, async) { }
    }
}