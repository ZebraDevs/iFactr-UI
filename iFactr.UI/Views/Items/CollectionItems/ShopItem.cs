using System;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a navigation element with a fixed height and
    /// labels for displaying information about a retail product.
    /// </summary>
    [Obsolete("This concept has been deprecated.  Use a different item type or create a custom item.")]
    public class ShopItem : iItem
    {
        /// <summary>
        /// Gets or sets the text above the primary text.  This is ideal for displaying information such as the product owner.
        /// </summary>
        /// <value>The top text as a <see cref="String"/> value.</value>
        public string Toptext { get; set; }

        /// <summary>
        /// Gets or sets an optional value between -1 and 5, rounded to the nearest half,
        /// that determines how many stars will be filled in.
        /// A value below 0 means the star rating will not be displayed.
        /// </summary>
        public float StarRating
        {
            get { return _starRating; }
            set
            {
                if (value < 0)
                {
                    _starRating = -1f;
                }
                else if (value > 5)
                {
                    _starRating = 5f;
                }
                else
                {
                    _starRating = ((int)(value * 2 + 0.5f)) / 2.0f;
                }
            }
        }
        private float _starRating = -1f;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItem"/> class.
        /// </summary>
        public ShopItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        public ShopItem(string linkAddress, string text) : base(linkAddress, text) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="async">If <c>true</c>, sets the link rev value to Async.</param>
        public ShopItem(string linkAddress, string text, bool async) : base(linkAddress, text, async) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        public ShopItem(string linkAddress, string text, string subText) : base(linkAddress, text, subText) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        /// <param name="async">If <c>true</c>, sets the link rev value to Async.</param>
        public ShopItem(string linkAddress, string text, string subText, bool async) : base(linkAddress, text, subText, async) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        /// <param name="topText">A <see cref="String"/> representing the Top text value.</param>
        public ShopItem(string linkAddress, string text, string subText, string topText)
            : base(linkAddress, text, subText)
        {
            Toptext = topText;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopItem"/> class.
        /// </summary>
        /// <param name="linkAddress">A <see cref="String"/> representing the Link address to navigate to when selected.</param>
        /// <param name="text">A <see cref="String"/> representing the Text value.</param>
        /// <param name="subText">A <see cref="String"/> representing the Subtext value.</param>
        /// <param name="topText">A <see cref="String"/> representing the Top text value.</param>
        /// <param name="async">If <c>true</c>, sets the link rev value to Async.</param>
        public ShopItem(string linkAddress, string text, string subText, string topText, bool async)
            : base(linkAddress, text, subText, async)
        {
            Toptext = topText;
        }
    }
}