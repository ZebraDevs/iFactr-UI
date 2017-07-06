using System;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Defines a UI element that can be sized and arranged within a view.
    /// </summary>
    public interface IElement : IPairable, IEquatable<IElement>
    {
        /// <summary>
        /// Gets or sets the zero-based index of the column that the element should reside in.
        /// </summary>
        int ColumnIndex { get; set; }

        /// <summary>
        /// Gets or sets how many columns the element should span across.
        /// </summary>
        int ColumnSpan { get; set; }

        /// <summary>
        /// Gets or sets how the element should horizontally align itself in the space that is allotted for it.
        /// </summary>
        HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets an identifier that can be used to easily identify the element within its parent container.
        /// </summary>
        string ID { get; set; }

        /// <summary>
        /// Gets or sets the amount of spacing around the element.
        /// </summary>
        Thickness Margin { get; set; }

        /// <summary>
        /// Gets a collection for storing arbitrary data on the object.
        /// </summary>
        MetadataCollection Metadata { get; }

        /// <summary>
        /// Gets the parent object that contains this instance.
        /// </summary>
        object Parent { get; }

        /// <summary>
        /// Gets or sets the zero-based index of the row that the element should reside in.
        /// </summary>
        int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets how many rows the element should span across.
        /// </summary>
        int RowSpan { get; set; }

        /// <summary>
        /// Gets or sets how the element should vertically align itself in the space that is allotted for it.
        /// </summary>
        VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the visible state of the element.
        /// </summary>
        Visibility Visibility { get; set; }

        /// <summary>
        /// Calculates and returns an appropriate width and height value for the element.
        /// This is called by the underlying layout system and should not be used in application logic.
        /// </summary>
        /// <param name="constraints">The size that the element is limited to.</param>
        Size Measure(Size constraints);

        /// <summary>
        /// Sets the location and size of the element within its parent container.
        /// This is called by the underlying layout system and should not be used in application logic.
        /// </summary>
        /// <param name="location">The X and Y coordinates of the upper left corner of the element.</param>
        /// <param name="size">The width and height of the element.</param>
        void SetLocation(Point location, Size size);
    }
}