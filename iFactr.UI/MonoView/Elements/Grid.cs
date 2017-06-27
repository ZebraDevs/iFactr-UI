using System.Collections.Generic;
using iFactr.Core;
using MonoCross.Navigation;
#if !NETCF
using System.Diagnostics;
#endif

namespace iFactr.UI.Controls
{
//    /// <summary>
//    /// Represents an object that acts as a grid made up of columns and rows for laying out various UI elements.
//    /// </summary>
//    public class Grid : Element, IGrid
//    {
//        #region Property Names
//        /// <summary>
//        /// The name of the <see cref="P:Padding"/> property.  Use this when binding to the property.  This field is read-only.
//        /// </summary>
//        public static readonly string PaddingProperty = "Padding";
//        #endregion
//
//        /// <summary>
//        /// Gets a collection of the columns that currently make up the grid.
//        /// </summary>
//        public ColumnCollection Columns
//        {
//            get { return NativeElement.Columns; }
//        }
//
//        /// <summary>
//        /// Gets or sets the amount of spacing between the edges of the grid and the children within it.
//        /// </summary>
//        public Thickness Padding
//        {
//            get { return NativeElement.Padding; }
//            set { NativeElement.Padding = value; }
//        }
//
//        /// <summary>
//        /// Gets a collection of the rows that currently make up the grid.
//        /// </summary>
//        public RowCollection Rows
//        {
//            get { return NativeElement.Rows; }
//        }
//
//        /// <summary>
//        /// Gets a collection of the UI elements that are currently contained within the grid.
//        /// </summary>
//        public IEnumerable<IElement> Children
//        {
//            get { return NativeElement.Children; }
//        }
//
//#if !NETCF
//        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//#endif
//        private IGrid NativeElement
//        {
//            get { return (IGrid)Pair; }
//        }
//
//        /// <summary>
//        /// Initializes a new instance of the <see cref="Grid"/> class.
//        /// </summary>
//        public Grid()
//        {
//            Pair = MXContainer.Resolve<IGrid>();
//
//            NativeElement.HorizontalAlignment = HorizontalAlignment.Stretch;
//            NativeElement.VerticalAlignment = VerticalAlignment.Stretch;
//        }
//
//        /// <summary>
//        /// Adds the specified <see cref="IElement"/> object to the grid.
//        /// </summary>
//        /// <param name="element">The element to be added to the grid.</param>
//        public void AddChild(IElement element)
//        {
//            NativeElement.AddChild(element);
//        }
//
//        /// <summary>
//        /// Removes the specified <see cref="IElement"/> object from the grid.
//        /// </summary>
//        /// <param name="element">The element to be removed from the grid.</param>
//        public void RemoveChild(IElement element)
//        {
//            NativeElement.RemoveChild(element);
//        }
//    }
}
