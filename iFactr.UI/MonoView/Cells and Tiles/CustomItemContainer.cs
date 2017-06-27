using System;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a container for passing <see cref="iFactr.Core.Layers.ICustomItem"/> objects to the platform bindings for handling.
    /// This class is used by the framework and should not be used in application code.
    /// </summary>
    public sealed class CustomItemContainer : ICell
    {
        /// <summary>
        /// Gets the custom object that is being passed to the platform bindings.
        /// </summary>
        public object CustomItem { get; internal set; }

        /// <summary>
        /// Gets the type of entry the item will represent when inserted into an <see cref="iFactr.UI.IListView"/>.
        /// </summary>
        public Type ItemType { get; internal set; }

        Color ICell.BackgroundColor
        {
            get { return new Color(); }
            set { }
        }

        double ICell.MaxHeight
        {
            get { return double.MaxValue; }
            set { }
        }

        MetadataCollection ICell.Metadata
        {
            get { return null; }
        }

        double ICell.MinHeight
        {
            get { return 0; }
            set { }
        }

        IPairable IPairable.Pair
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomItemContainer"/> class.
        /// </summary>
        /// <param name="item">The custom item to be wrapped.</param>
        public CustomItemContainer(object item)
        {
            CustomItem = item;
            ItemType = typeof(ICell);
        }

        bool IEquatable<ICell>.Equals(ICell other)
        {
            return other == this;
        }
    }
}