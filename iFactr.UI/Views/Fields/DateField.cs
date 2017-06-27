using System;

namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a field with a date and time picker.
    /// </summary>
    public class DateField : Field
    {
        #region Enumerations
        /// <summary>
        /// The available types of date picker.
        /// </summary>
        public enum DateType
        {
            /// <summary>
            /// A picker with a date value only (month, day, year).
            /// </summary>
            Date,
            /// <summary>
            /// A picker with a time value only (hour, minute).
            /// </summary>
            Time,
            /// <summary>
            /// A picker with both date and time values.
            /// </summary>
            DateTime
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the date type of this instance.
        /// </summary>
        /// <value>The date type as a <see cref="DateType"/> value.</value>
        public DateType Type { get; set; }

        /// <summary>
        /// Gets or sets the date and time values of this instance.
        /// </summary>
        /// <value>The date and time values as a <see cref="DateTime"/>.</value>
        public DateTime? Value { get; set; }

        /// <summary>
        /// Gets or sets the minute interval of this instance.  The value must be a factor of 60.
        /// </summary>
        public int MinuteInterval { get; set; }
        #endregion

        #region Ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateField"/> class with no submission ID.
        /// </summary>
        public DateField()
        {
            Value = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateField"/> class using the ID provided.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        public DateField(string id)
        {
            ID = id;
            Value = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateField"/> class using the ID and date type provided.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        /// <param name="type">A <see cref="DateType"/> representing the type of picker.</param>
        public DateField(string id, DateType type)
        {
            ID = id;
            Type = type;
            Value = DateTime.Now;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DateField"/> class using the ID, date type, and field value provided.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        /// <param name="type">A <see cref="DateType"/> representing the type of picker.</param>
        /// <param name="dateTime">A <see cref="DateTime"/> representing the date and time.</param>
        public DateField(string id, DateType type, DateTime? dateTime)
        {
            ID = id;
            Type = type;
            Value = dateTime;
        }
        #endregion

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new DateField Clone()
        {
            return (DateField)CloneOverride();
        }
    }
}
