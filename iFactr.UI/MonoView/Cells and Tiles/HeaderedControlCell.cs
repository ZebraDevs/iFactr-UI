using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using iFactr.Core;
using iFactr.UI.Controls;
using iFactr.UI.Instructions;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a cell with a predefined header label coupled with one or more <see cref="IControl"/> objects.
    /// The header label and the controls are automatically laid out in a manner appropriate to the target platform.
    /// </summary>
    public sealed class HeaderedControlCell : Cell, IElementHost, ILayoutInstruction
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
        /// Gets a collection of the UI elements that currently reside within the cell.
        /// </summary>
        public IEnumerable<IElement> Children
        {
            get { return NativeCell.Children; }
        }

        /// <summary>
        /// Gets the header label for the cell.
        /// </summary>
        public Label Header { get; private set; }

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
        /// Initializes a new instance of the <see cref="HeaderedControlCell"/> class.
        /// </summary>
        /// <param name="headerText">The text to apply to the header label.</param>
        /// <param name="controls">The <see cref="IControl"/> objects to include with the header label.</param>
        public HeaderedControlCell(string headerText, params IControl[] controls)
        {
            base.Pair = MXContainer.Resolve<IGridCell>();

            NativeCell.Padding = new Thickness(Thickness.LeftMargin, Thickness.TopMargin, Thickness.RightMargin, Thickness.BottomMargin);
            NativeCell.SelectionColor = iApp.Instance.Style.SelectionColor;
            NativeCell.SelectionStyle = SelectionStyle.Default;

            Header = new Label();
            Header.Text = headerText;
            NativeCell.AddChild(Header);

            if (controls != null)
            {
                foreach (var control in controls.Where(c => c != null))
                {
                    NativeCell.AddChild(control);
                }
            }

            iApp.Factory.Instructor.Layout(this);
        }

        /// <summary>
        /// Adds the specified <see cref="iFactr.UI.Controls.IControl"/> instance to the cell.
        /// </summary>
        /// <param name="control">The control to be added to the cell.</param>
        public void AddControl(IControl control)
        {
            NativeCell.AddChild(control);
        }

        /// <summary>
        /// Returns the first control found that is of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the control.</typeparam>
        /// <returns>The first control found of type <typeparamref name="T"/> -or-
        /// <c>null</c> if no control of the specified type was found.</returns>
        public T GetControl<T>()
            where T : IControl
        {
            return (T)NativeCell.Children.FirstOrDefault(c => c is T);
        }

        /// <summary>
        /// Returns the first control found with the specified ID that is of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the control.</typeparam>
        /// <param name="id">The identifier of the control.</param>
        /// <returns>The first control found with the specified ID that is of type <typeparamref name="T"/> -or-
        /// <c>null</c> if no control was found.</returns>
        public T GetControl<T>(string id)
            where T : IControl
        {
            return (T)NativeCell.Children.FirstOrDefault(c => c is T && c.ID == id);
        }

        /// <summary>
        /// Resets the invocation list of all events within the class.
        /// </summary>
        public void NullifyEvents()
        {
            NativeCell.NullifyEvents();
        }

        /// <summary>
        /// Removes the specified <see cref="iFactr.UI.Controls.IControl"/> instance from the cell.
        /// </summary>
        /// <param name="control">The control to be removed from the cell.</param>
        public void RemoveControl(IControl control)
        {
            NativeCell.RemoveChild(control);
        }

        /// <summary>
        /// Programmatically selects the cell.
        /// </summary>
        public void Select()
        {
            NativeCell.Select();
        }

        void ILayoutInstruction.Layout()
        {
            NativeCell.Columns.Clear();
            NativeCell.Rows.Clear();

            NativeCell.Columns.Add(Column.OneStar);
            NativeCell.Rows.Add(Row.AutoSized);

            Header.Font = Font.PreferredHeaderFont;
            Header.Lines = 1;
            Header.VerticalAlignment = VerticalAlignment.Top;
            Header.HorizontalAlignment = HorizontalAlignment.Stretch;
            Header.RowIndex = 0;
            Header.ColumnIndex = 0;

            foreach (var control in NativeCell.Children.Where(c => c != Header))
            {
                NativeCell.Rows.Add(Row.AutoSized);

                control.RowIndex = NativeCell.Rows.Count - 1;
                control.ColumnIndex = 0;

                if (control is ITextArea)
                {
                    control.VerticalAlignment = VerticalAlignment.Stretch;
                    NativeCell.Rows[control.RowIndex] = Row.OneStar;
                }
                else
                {
                    control.VerticalAlignment = VerticalAlignment.Top;
                }

                if (string.IsNullOrEmpty(Header.Text) && control.RowIndex == 1)
                {
                    control.Margin = new Thickness(0, 0, 0, 0);
                }
                else
                {
                    control.Margin = new Thickness(0, Thickness.SmallVerticalSpacing, 0, 0);
                }
            }
        }

        void IElementHost.AddChild(IElement element)
        {
            NativeCell.AddChild(element);
        }

        void IElementHost.RemoveChild(IElement element)
        {
            NativeCell.RemoveChild(element);
        }
    }
}