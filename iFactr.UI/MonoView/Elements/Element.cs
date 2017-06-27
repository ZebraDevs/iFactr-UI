using System;
using System.Diagnostics;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Represents a UI element that can be sized and arranged within a view.  This class is abstract.
    /// </summary>
    public abstract class Element : IElement
    {
        /// <summary>
        /// A column or row index that signifies that the element should use the automatic layout system to determine its position.
        /// This field is constant.
        /// </summary>
        public const int AutoLayoutIndex = -1;

        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:ColumnIndex"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ColumnIndexProperty = "ColumnIndex";

        /// <summary>
        /// The name of the <see cref="P:ColumnSpan"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ColumnSpanProperty = "ColumnSpan";

        /// <summary>
        /// The name of the <see cref="P:HorizontalAlignment"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string HorizontalAlignmentProperty = "HorizontalAlignment";

        /// <summary>
        /// The name of the <see cref="P:ID"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string IDProperty = "ID";

        /// <summary>
        /// The name of the <see cref="P:Margin"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string MarginProperty = "Margin";

        /// <summary>
        /// The name of the <see cref="P:RowIndex"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string RowIndexProperty = "RowIndex";

        /// <summary>
        /// The name of the <see cref="P:RowSpan"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string RowSpanProperty = "RowSpan";

        /// <summary>
        /// The name of the <see cref="P:VerticalAlignment"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string VerticalAlignmentProperty = "VerticalAlignment";

        /// <summary>
        /// The name of the <see cref="P:Visibility"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string VisibilityProperty = "Visibility";
        #endregion

        /// <summary>
        /// Gets or sets the zero-based index of the column that the element should reside in.
        /// </summary>
        public int ColumnIndex
        {
            get { return Pair.ColumnIndex; }
            set { Pair.ColumnIndex = value; }
        }

        /// <summary>
        /// Gets or sets how many columns the element should span across.
        /// </summary>
        public int ColumnSpan
        {
            get { return Pair.ColumnSpan; }
            set { Pair.ColumnSpan = value; }
        }

        /// <summary>
        /// Gets or sets how the element should horizontally align itself in the space that is allotted for it.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment
        {
            get { return Pair.HorizontalAlignment; }
            set { Pair.HorizontalAlignment = value; }
        }

        /// <summary>
        /// Gets or sets an identifier that can be used to easily identify the element within its parent container.
        /// </summary>
        public string ID
        {
            get { return Pair.ID; }
            set { Pair.ID = value; }
        }

        /// <summary>
        /// Gets or sets the amount of spacing around the element.
        /// </summary>
        public Thickness Margin
        {
            get { return Pair.Margin; }
            set { Pair.Margin = value; }
        }

        /// <summary>
        /// Gets a collection for storing arbitrary data on the object.
        /// </summary>
        public MetadataCollection Metadata
        {
            get { return Pair.Metadata; }
        }

        /// <summary>
        /// Gets the parent object that contains this instance.
        /// </summary>
        public object Parent
        {
            get { return Pair.Parent; }
        }

        /// <summary>
        /// Gets or sets the zero-based index of the row that the element should reside in.
        /// </summary>
        public int RowIndex
        {
            get { return Pair.RowIndex; }
            set { Pair.RowIndex = value; }
        }

        /// <summary>
        /// Gets or sets how many rows the element should span across.
        /// </summary>
        public int RowSpan
        {
            get { return Pair.RowSpan; }
            set { Pair.RowSpan = value; }
        }

        /// <summary>
        /// Gets or sets how the element should vertically align itself in the space that is allotted for it.
        /// </summary>
        public VerticalAlignment VerticalAlignment
        {
            get { return Pair.VerticalAlignment; }
            set { Pair.VerticalAlignment = value; }
        }

        /// <summary>
        /// Gets or sets the visible state of the element.
        /// </summary>
        public Visibility Visibility
        {
            get { return Pair.Visibility; }
            set { Pair.Visibility = value; }
        }

        /// <summary>
        /// Gets or sets the native object that is paired with the element.  This can be set only once.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        protected IElement Pair
        {
            get
            {
                if (pair == null)
                {
                    throw new InvalidOperationException("No native object was found for the current instance.");
                }
                return pair;
            }
            set
            {
                if (pair == null && value != null)
                {
                    pair = value;
                    pair.Pair = this;

                    pair.ColumnSpan = 1;
                    pair.RowSpan = 1;
                    pair.ColumnIndex = Element.AutoLayoutIndex;
                    pair.RowIndex = Element.AutoLayoutIndex;
                }
            }
        }
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private IElement pair;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        IPairable IPairable.Pair
        {
            get { return Pair; }
            set { Pair = value as IElement; }
        }

        /// <summary>
        /// Calculates and returns an appropriate width and height value for the contents of the element.
        /// This is called by the underlying grid layout system and should not be used in application logic.
        /// </summary>
        /// <param name="constraints">The size that the element is limited to.</param>
        public Size Measure(Size constraints)
        {
            return Pair.Measure(constraints);
        }

        /// <summary>
        /// Sets the location and size of the element within its parent container.
        /// This is called by the underlying grid layout system and should not be used in application logic.
        /// </summary>
        /// <param name="location">The X and Y coordinates of the upper left corner of the control.</param>
        /// <param name="size">The width and height of the control.</param>
        public void SetLocation(Point location, Size size)
        {
            Pair.SetLocation(location, size);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var element = obj as Element;
            if (element != null)
            {
                return Pair == element.Pair;
            }

            var ielement = obj as IElement;
            if (ielement != null)
            {
                return Pair == ielement;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="IElement"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="IElement"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="IElement"/> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(IElement other)
        {
            var element = other as Element;
            if (element != null)
            {
                return Pair == element.Pair;
            }

            return Pair == other;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="Element"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in
        /// hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return Pair.GetHashCode();
        }
    }
}
