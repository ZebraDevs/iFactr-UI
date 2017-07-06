using System;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a navigation element with a fixed height and subtext that is right-aligned.
    /// </summary>
    public class RightSubtextItem : iItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightSubtextItem"/> class.
        /// </summary>
        public RightSubtextItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RightSubtextItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        public RightSubtextItem(string linkAddress, string text, string subText) : base(linkAddress, text, subText) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RightSubtextItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        /// <param name="async">If <c>true</c>, sets the link rev value to Async.</param>
        public RightSubtextItem(string linkAddress, string text, string subText, bool async) : base(linkAddress, text, subText, async) { }
    }
}