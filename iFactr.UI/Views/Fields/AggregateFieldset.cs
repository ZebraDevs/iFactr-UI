namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a column of fieldsets to be used in a table of <see cref="Field"/>s.
    /// <remarks>The Header property of an <see cref="AggregateFieldset"/> refers to each column's header.</remarks>
    /// </summary>
    public class AggregateFieldset : Fieldset
    {
        /// <summary>
        /// Gets or sets the text above the aggregated fieldsets.
        /// <remarks>Specifiying this property on an <see cref="AggregateFieldset"/> marks it as the first fieldset in a new table.</remarks>
        /// </summary>
        public string AggregateHeader { get; set; }
    }
}