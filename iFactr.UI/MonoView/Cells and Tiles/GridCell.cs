using System;
using System.Collections.Generic;
using System.Diagnostics;

using iFactr.Core;
using iFactr.UI.Controls;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a cell that acts as a grid for laying out various UI elements.
    /// This is the most common type of cell.
    /// </summary>
    public class GridCell : Cell, IGridCell
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:AccessoryLink"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string AccessoryLinkProperty = "AccessoryLink";

        /// <summary>
        /// The name of the <see cref="P:NavigationLink"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string NavigationLinkProperty = "NavigationLink";

        /// <summary>
        /// The name of the <see cref="P:Padding"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string PaddingProperty = "Padding";

        /// <summary>
        /// The name of the <see cref="P:SelectionColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string SelectionColorProperty = "SelectionColor";

        /// <summary>
        /// The name of the <see cref="P:SelectionStyle"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string SelectionStyleProperty = "SelectionStyle";
        #endregion

        /// <summary>
        /// Occurs when the user selects the cell.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        public event EventHandler Selected
        {
            add { NativeCell.Selected += value; }
            remove { NativeCell.Selected -= value; }
        }

        /// <summary>
        /// Occurs when the user selects the accessory for the cell.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        public event EventHandler AccessorySelected
        {
            add { NativeCell.AccessorySelected += value; }
            remove { NativeCell.AccessorySelected -= value; }
        }

        /// <summary>
        /// Gets or sets the link to navigate to when the cell's accessory has been selected by the user.
        /// The navigation only occurs if there is no handler for the <see cref="AccessorySelected"/> event.
        /// </summary>
        public Link AccessoryLink
        {
            get { return NativeCell.AccessoryLink; }
            set { NativeCell.AccessoryLink = value; }
        }

        /// <summary>
        /// Gets a collection of the columns that currently make up the cell.
        /// </summary>
        public ColumnCollection Columns
        {
            get { return NativeCell.Columns; }
        }

        /// <summary>
        /// Gets a collection of the UI elements that currently reside within the cell.
        /// </summary>
        public IEnumerable<IElement> Children
        {
            get { return NativeCell.Children; }
        }

        /// <summary>
        /// Gets or sets the link to navigate to when the cell has been selected by the user.
        /// The navigation only occurs if there is no handler for the <see cref="Selected"/> event.
        /// </summary>
        public Link NavigationLink
        {
            get { return NativeCell.NavigationLink; }
            set { NativeCell.NavigationLink = value; }
        }

        /// <summary>
        /// Gets or sets the amount of spacing between the edges of the cell and the children within it.
        /// </summary>
        public Thickness Padding
        {
            get { return NativeCell.Padding; }
            set { NativeCell.Padding = value; }
        }

        /// <summary>
        /// Gets a collection of the rows that currently make up the cell.
        /// </summary>
        public RowCollection Rows
        {
            get { return NativeCell.Rows; }
        }

        /// <summary>
        /// Gets or sets the color with which to highlight the cell when it is selected.
        /// This may not appear on all platforms.
        /// </summary>
        public Color SelectionColor
        {
            get { return NativeCell.SelectionColor; }
            set { NativeCell.SelectionColor = value; }
        }

        /// <summary>
        /// Gets or sets which visual elements to use to indicate that the cell is selectable or has been selected.
        /// </summary>
        public SelectionStyle SelectionStyle
        {
            get { return NativeCell.SelectionStyle; }
            set { NativeCell.SelectionStyle = value; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private IGridCell NativeCell
        {
            get { return (IGridCell)base.Pair; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.GridCell"/> class.
        /// </summary>
        public GridCell()
        {
            base.Pair = MXContainer.Resolve<IGridCell>();

            NativeCell.Padding = new Thickness(Thickness.LeftMargin, Thickness.TopMargin, Thickness.RightMargin, Thickness.BottomMargin);
            NativeCell.SelectionColor = iApp.Instance.Style.SelectionColor;
            NativeCell.SelectionStyle = SelectionStyle.Default;
        }

        /// <summary>
        /// Adds the specified <see cref="IElement"/> instance to the cell.
        /// </summary>
        /// <param name="element">The element to be added to the cell.</param>
        public void AddChild(IElement element)
        {
            NativeCell.AddChild(element);
        }

        /// <summary>
        /// Resets the invocation list of all events within the class.
        /// </summary>
        public void NullifyEvents()
        {
            NativeCell.NullifyEvents();
        }

        /// <summary>
        /// Removes the specified <see cref="IElement"/> instance from the cell.
        /// </summary>
        /// <param name="element">The element to be removed from the cell.</param>
        public void RemoveChild(IElement element)
        {
            NativeCell.RemoveChild(element);
        }

        /// <summary>
        /// Programmatically selects the cell.
        /// </summary>
        public void Select()
        {
            NativeCell.Select();
        }
    }
}

