using System;

namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a selectable item within a <see cref="SelectListField"/>.
    /// </summary>
    public class SelectListFieldItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the key for this instance.  This key can be used as an alternative to the displayed
        /// text when reading the value of a <see cref="SelectListField"/>.  To retrieve it from a parameters
        /// dictionary, use the field ID with ".Key" appended to the end.
        /// </summary>
        /// <value>The key as a <see cref="String"/> value.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the displayed text value for this instance.
        /// </summary>
        /// <value>The value as a <see cref="String"/> value.</value>
        public string Value { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectListFieldItem"/> class.
        /// </summary>
        public SelectListFieldItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectListFieldItem"/> class.
        /// </summary>
        /// <param name="value">A <see cref="String"/> representing the displayed text.</param>
        public SelectListFieldItem(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectListFieldItem"/> class.
        /// </summary>
        /// <param name="key">A <see cref="String"/> representing the key.</param>
        /// <param name="value">A <see cref="String"/> representing the displayed text.</param>
        public SelectListFieldItem(string key, string value)
        {
            Key = key;
            Value = value;
        }

        #endregion

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public SelectListFieldItem Clone()
        {
            return (SelectListFieldItem)MemberwiseClone();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return Value;
        }
    }
}