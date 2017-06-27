using System;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a navigation element with a fixed height and several labels ideal for displaying messages.
    /// </summary>
    public class MessageItem : iItem
    {
        /// <summary>
        /// Gets or sets a two-line message text that is displayed underneath the subtext.
        /// </summary>
        /// <value>The message text as a <see cref="String"/> value.</value>
        public string MessageText { get; set; }

        /// <summary>
        /// Gets or sets the text displayed next to the primary text.  If the primary text is too long, this text may be truncated.
        /// This text is ideal for displaying date information.
        /// </summary>
        /// <value>The date text as a <see cref="String"/> value.</value>
        public string DateText { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageItem"/> class.
        /// </summary>
        public MessageItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        public MessageItem(string linkAddress, string text) : base(linkAddress, text) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="async">If <c>true</c>, sets the link rev value to Async.</param>
        public MessageItem(string linkAddress, string text, bool async) : base(linkAddress, text, async) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        public MessageItem(string linkAddress, string text, string subText) : base(linkAddress, text, subText) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        /// <param name="async">If <c>true</c>, sets the link rev value to Async.</param>
        public MessageItem(string linkAddress, string text, string subText, bool async) : base(linkAddress, text, subText, async) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        /// <param name="dateText">A <see cref="String"/> representing the Date text value.</param>
        /// <param name="messageText">A <see cref="String"/> representing the Message text value.</param>
        public MessageItem(string linkAddress, string text, string subText, string dateText, string messageText)
            : base(linkAddress, text, subText)
        {
            DateText = dateText;
            MessageText = messageText;
        }
    }
}