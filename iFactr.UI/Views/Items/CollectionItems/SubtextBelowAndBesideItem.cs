using System;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a navigation element with a fixed height and two sets of subtext, one below the primary text and one beside it.
    /// </summary>
    public class SubtextBelowAndBesideItem : iItem
    {
        /// <summary>
        /// Gets or sets the text to display beside the primary text.
        /// </summary>
        /// <value>The text beside the primary text as a <see cref="String"/> value.</value>
        public string BesideText { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubtextBelowAndBesideItem"/> class.
        /// </summary>
        public SubtextBelowAndBesideItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubtextBelowAndBesideItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        public SubtextBelowAndBesideItem(string linkAddress, string text, string subText) : base(linkAddress, text, subText) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubtextBelowAndBesideItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        /// <param name="async">If <c>true</c>, sets the link rev value to Async.</param>
        public SubtextBelowAndBesideItem(string linkAddress, string text, string subText, bool async) : base(linkAddress, text, subText, async) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubtextBelowAndBesideItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        /// <param name="besideText">A <see cref="String"/> representing the Beside text value.</param>
        public SubtextBelowAndBesideItem(string linkAddress, string text, string subText, string besideText)
            : base(linkAddress, text, subText)
        {
            BesideText = besideText;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubtextBelowAndBesideItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        /// <param name="besideText">A <see cref="String"/> representing the Beside text value.</param>
        /// <param name="async">If <c>true</c>, sets the link rev value to Async.</param>
        public SubtextBelowAndBesideItem(string linkAddress, string text, string subText, string besideText, bool async)
            : base(linkAddress, text, subText, async)
        {
            BesideText = besideText;
        }
    }
}