using System;

namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a field for accepting numerical text input.
    /// </summary>
    public class NumericField : TextField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericField"/> class with no submission ID.
        /// </summary>
        public NumericField()
        {
            Expression = @"^[-\.\d]*$";
            base.KeyboardType = KeyboardType.Symbolic;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericField"/> class.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        public NumericField(string id)
            : this()
        {
            ID = id;
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new NumericField Clone()
        {
            return (NumericField)CloneOverride();
        }
    }
}