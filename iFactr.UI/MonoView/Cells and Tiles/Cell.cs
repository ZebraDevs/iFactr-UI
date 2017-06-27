using System;
using System.Diagnostics;

using iFactr.Core;

namespace iFactr.UI
{
    /// <summary>
    /// Represents an entry in an <see cref="iFactr.UI.IListView"/> instance.  This class is abstract and is the base
    /// class for the <see cref="iFactr.UI.GridCell"/> and <see cref="iFactr.UI.RichContentCell"/> classes.
    /// </summary>
	public abstract class Cell : ICell
	{
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:BackgroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string BackgroundColorProperty = "BackgroundColor";

        /// <summary>
        /// The name of the <see cref="P:MaxHeight"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string MaxHeightProperty = "MaxHeight";

        /// <summary>
        /// The name of the <see cref="P:MinHeight"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string MinHeightProperty = "MinHeight";
        #endregion

        /// <summary>
        /// Gets the height value for a typical cell on the target platform.
        /// </summary>
        public static double StandardCellHeight
        {
            get { return iApp.Factory.PlatformDefaults.CellHeight; }
        }

        /// <summary>
        /// Gets or sets the background color of the cell.
        /// </summary>
		public Color BackgroundColor
		{
			get { return Pair.BackgroundColor; }
			set { Pair.BackgroundColor = value; }
		}

        /// <summary>
        /// Gets or sets the maximum amount of vertical space that the cell is allowed to consume.
        /// </summary>
        public double MaxHeight
        {
            get { return Pair.MaxHeight; }
            set { Pair.MaxHeight = value; }
        }

        /// <summary>
        /// Gets a collection for storing arbitrary data on the object.
        /// </summary>
        public MetadataCollection Metadata
        {
            get { return Pair.Metadata; }
        }

        /// <summary>
        /// Gets or sets the minimum amount of vertical space that the cell is allowed to consume.
        /// </summary>
        public double MinHeight
        {
            get { return Pair.MinHeight; }
            set { Pair.MinHeight = value; }
        }

        /// <summary>
        /// Gets or sets the native object that is paired with the cell.  This can be set only once.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		protected ICell Pair
        {
            get
            {
                if (pair == null)
                {
                    throw new InvalidOperationException("No native cell was found for the current instance.");
                }
                return pair;
            }
            set
            {
                if (pair == null && value != null)
                {
                    pair = value;
                    pair.Pair = this;

                    pair.BackgroundColor = iApp.Instance.Style.LayerItemBackgroundColor;
                    pair.MaxHeight = double.PositiveInfinity;
                    pair.MinHeight = 0;
                }
            }
        }
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private ICell pair;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        IPairable IPairable.Pair
        {
            get { return Pair; }
            set { Pair = value as ICell; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
		public override bool Equals (object obj)
		{
			Cell cell = obj as Cell;
			if (cell != null)
			{
				return Pair == cell.Pair;
			}
			
			ICell icell = obj as ICell;
			if (icell != null)
			{
				return Pair == icell;
			}
			
			return false;
		}

        /// <summary>
        /// Determines whether the specified <see cref="iFactr.UI.ICell"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="iFactr.UI.ICell"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="iFactr.UI.ICell"/> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
		public bool Equals (ICell other)
		{
			Cell cell = other as Cell;
			if (cell != null)
			{
				return Pair == cell.Pair;
			}
			
			return Pair == other;
		}

        /// <summary>
        /// Serves as a hash function for a <see cref="iFactr.UI.Cell"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in
        /// hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode ()
		{
			return Pair.GetHashCode();
		}
	}
}

