using System;

namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a text field for inputting email addresses.
    /// </summary>
    public class EmailField : TextField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailField"/> class with no submission ID.
        /// </summary>
        public EmailField() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailField"/> class using the ID provided.
        /// </summary>
        /// <param name="id">A <see cref="string"/> representing the ID value.</param>
        public EmailField(string id) : base(id) { }

        /// <summary>
        /// Gets the virtual keyboard type to use for this instance.
        /// </summary>
        public override KeyboardType KeyboardType
        {
            get
            {
                return KeyboardType.Email;
            }
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new EmailField Clone()
        {
            return (EmailField)CloneOverride();
        }
    }
}
