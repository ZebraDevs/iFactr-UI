using System;

namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a non-editable label.
    /// </summary>
    public class LabelField : Field
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelField"/> class.
        /// </summary>
        public LabelField() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelField"/> class using the ID provided.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID value.</param>
        public LabelField(string id)
        {
            ID = id;
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new LabelField Clone()
        {
            return (LabelField)CloneOverride();
        }
    }
}