using System;
using iFactr.UI;

namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a field for text input that allows multiple lines of text to be entered.
    /// </summary>
    public class MultiLineTextField : TextField
    {
        /// <summary>
        /// Gets or sets the number of rows of text to display at once.
        /// When the rows of text exceed this number, this instance will be scrollable.
        /// </summary>
        /// <value>The number of rows as an <see cref="Int32"/> value.</value>
        public int Rows
        {
            get { return rows; }
            set { rows = value < 1 ? 1 : value; }
        }
        private int rows;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineTextField"/> class with no submission ID.
        /// </summary>
        public MultiLineTextField()
        {
            Rows = 4;
            base.TextCompletion = TextCompletion.AutoCapitalize | TextCompletion.OfferSuggestions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineTextField"/> class using the ID provided.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        public MultiLineTextField(string id)
            : base(id)
        {
            Rows = 4;
            base.TextCompletion = TextCompletion.AutoCapitalize | TextCompletion.OfferSuggestions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineTextField"/> class using the ID provided.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        /// <param name="rows">An <see cref="Int32"/> representing the number of rows of text.</param>
        public MultiLineTextField(string id, int rows)
            : base(id)
        {
            Rows = rows;
            base.TextCompletion = TextCompletion.AutoCapitalize | TextCompletion.OfferSuggestions;
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new MultiLineTextField Clone()
        {
            return (MultiLineTextField)CloneOverride();
        }
    }
}