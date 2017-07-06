using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using iFactr.Core;

namespace iFactr.UI
{
    /// <summary>
    /// Represents the method that will retrieve an <see cref="ICell"/> instance when it is ready to be rendered on screen.
    /// </summary>
    /// <param name="index">The index at which the cell will be placed within the section.</param>
    /// <param name="recycledCell">An already instantiated cell that is ready for reuse, or <c>null</c> if no cell has been recycled.</param>
    /// <returns>The cell that will be rendered on screen.</returns>
    public delegate ICell SectionCellDelegate(int index, ICell recycledCell);

    /// <summary>
    /// Represents the method that will retrieve an identifier for determining which, if any, <see cref="ICell"/>
    /// instances can be reused for the item at the given position.
    /// </summary>
    /// <param name="index">The index at which the item will be placed in the section.</param>
    /// <returns>An identifier for determining which cells can be reused for the item at the given position.</returns>
    public delegate int SectionItemIdDelegate(int index);

    /// <summary>
    /// Represents a group of <see cref="ICell"/> objects within an <see cref="IListView"/> instance.
    /// </summary>
#if !NETCF
    [DebuggerDisplay("Item Count = {ItemCount}")]
#endif
    public class Section
    {
        /// <summary>
        /// Gets or sets the header that displays on top of the section.
        /// </summary>
        public SectionHeader Header { get; set; }

        /// <summary>
        /// Gets or sets the footer that displays on the bottom of the section.
        /// </summary>
        public SectionFooter Footer { get; set; }

        /// <summary>
        /// Gets or sets the total number of <see cref="ICell"/> objects that will go into the section.
        /// </summary>
        public int ItemCount
        {
            get { return _itemCount; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Value cannot be less than zero.");
                }

                _itemCount = value;
            }
        }
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private int _itemCount;

        /// <summary>
        /// Invoked when a cell is ready to be rendered on the screen.  Return the <see cref="ICell"/>
        /// instance that should be placed at the given index within the section.
        /// </summary>
        public SectionCellDelegate CellRequested { get; set; }

        /// <summary>
        /// Invoked when a reuse identifier is needed for a cell.  Return the identifier that should be used
        /// to determine which cells may be reused for the item at the given index within the section.
        /// This is only invoked on platforms that support recycling.
        /// </summary>
        public SectionItemIdDelegate ItemIdRequested { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        public Section()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        /// <param name="itemCount">The total number of <see cref="ICell"/> objects that will go into the section.</param>
        public Section(int itemCount)
        {
            ItemCount = itemCount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        /// <param name="itemCount">The total number of <see cref="ICell"/> objects that will go into the section.</param>
        /// <param name="headerText">The text for the section header.</param>
        public Section(int itemCount, string headerText)
            : this(itemCount)
        {
            if (headerText != null)
            {
                Header = new SectionHeader(headerText);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        /// <param name="itemCount">The total number of <see cref="ICell"/> objects that will go into the section.</param>
        /// <param name="headerText">The text for the section header.</param>
        /// <param name="footerText">The text for the section footer.</param>
        public Section(int itemCount, string headerText, string footerText)
            : this(itemCount, headerText)
        {
            if (footerText != null)
            {
                Footer = new SectionFooter(footerText);
            }
        }
    }

    /// <summary>
    /// Represents a collection of <see cref="Section"/> objects.
    /// </summary>
#if !NETCF
    [DebuggerDisplay("Count = {Count}")]
#endif
    public sealed class SectionCollection : IList<Section>
    {
        /// <summary>
        /// Gets the number of sections contained in the collection.
        /// </summary>
        public int Count
        {
            get { return _items.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        bool ICollection<Section>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets the section at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the section to get or set.</param>
        public Section this[int index]
        {
            get
            {
                CheckIndex(index);
                return _items[index];
            }
            set
            {
                CheckIndex(index);
                _items[index] = value;
            }
        }

        private readonly List<Section> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionCollection"/> class.
        /// </summary>
        public SectionCollection()
        {
            _items = new List<Section>();
        }

        /// <summary>
        /// Adds a <see cref="Section"/> object to the end of the collection.  See Remarks.
        /// </summary>
        /// <param name="itemCount">The total number of <see cref="ICell"/> objects that will go into the section.</param>
        /// <param name="headerText">The text for the section header.</param>
        /// <remarks>It isn't necessary to explicitly add new sections to the collection.
        /// New sections will automatically be added when indexing the collection to ensure that the index is in range.</remarks>
        public void Add(int itemCount, string headerText)
        {
            Add(new Section(itemCount, headerText));
        }

        /// <summary>
        /// Adds a <see cref="Section"/> object to the end of the collection.  See Remarks.
        /// </summary>
        /// <param name="itemCount">The total number of <see cref="ICell"/> objects that will go into the section.</param>
        /// <param name="headerText">The text for the section header.</param>
        /// <param name="footerText">The text for the section footer.</param>
        /// <remarks>It isn't necessary to explicitly add new sections to the collection.
        /// New sections will automatically be added when indexing the collection to ensure that the index is in range.</remarks>
        public void Add(int itemCount, string headerText, string footerText)
        {
            Add(new Section(itemCount, headerText, footerText));
        }

        /// <summary>
        /// Adds a <see cref="Section"/> object to the end of the collection.  See Remarks.
        /// </summary>
        /// <param name="section">The section to add to the collection.</param>
        /// <remarks>It isn't necessary to explicitly add new sections to the collection.
        /// New sections will automatically be added when indexing the collection to ensure that the index is in range.</remarks>
        public void Add(Section section)
        {
            _items.Add(section);
        }

        /// <summary>
        /// Adds a <see cref="Section"/> range to the end of the collection.  See Remarks.
        /// </summary>
        /// <param name="sections">The range of sections to add to the collection.</param>
        /// <remarks>It isn't necessary to explicitly add new sections to the collection.
        /// New sections will automatically be added when indexing the collection to ensure that the index is in range.</remarks>
        public void AddRange(IEnumerable<Section> sections)
        {
            _items.AddRange(sections);
        }

        /// <summary>
        /// Inserts a <see cref="iFactr.UI.Section"/> object into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the item should be inserted.</param>
        /// <param name="itemCount">The total number of <see cref="ICell"/> objects that will go into the section.</param>
        /// <param name="headerText">The text for the section header.</param>
        public void Insert(int index, int itemCount, string headerText)
        {
            Insert(index, new Section(itemCount, headerText));
        }

        /// <summary>
        /// Inserts a <see cref="iFactr.UI.Section"/> object into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the item should be inserted.</param>
        /// <param name="itemCount">The total number of <see cref="ICell"/> objects that will go into the section.</param>
        /// <param name="headerText">The text for the section header.</param>
        /// <param name="footerText">The text for the section footer.</param>
        public void Insert(int index, int itemCount, string headerText, string footerText)
        {
            Insert(index, new Section(itemCount, headerText, footerText));
        }

        /// <summary>
        /// Inserts a <see cref="iFactr.UI.Section"/> object into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the item should be inserted.</param>
        /// <param name="section">The section to be inserted into the collection.</param>
        public void Insert(int index, Section section)
        {
            CheckIndex(index);
            _items.Insert(index, section);
        }

        /// <summary>
        /// Determines the index of a specific section in the collection.
        /// </summary>
        /// <param name="section">The section to locate in the collection.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(Section section)
        {
            return _items.IndexOf(section);
        }

        /// <summary>
        /// Removes the section at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            if (index < Count)
            {
                _items.RemoveAt(index);
            }
        }

        /// <summary>
        /// Removes all sections from the collection.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains a specific section.
        /// </summary>
        /// <param name="section">The section to locate in the collection.</param>
        /// <returns><c>true</c> if the section is found in the collection; otherwise, <c>false</c>.</returns>
        public bool Contains(Section section)
        {
            return _items.Contains(section);
        }

        /// <summary>
        /// Copies the elements of the collection to an <see cref="System.Array"/>, starting at a particular index.</summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements
        /// copied from the collection.  The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="ArgumentException">Thrown when the number of elements in the source collection is
        /// greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
        public void CopyTo(Section[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific section from the collection.
        /// </summary>
        /// <param name="section">The section to remove from the collection.</param>
        /// <returns><c>true</c> if the section was successfully removed from the collection; otherwise, <c>false</c>.
        /// This method also returns <c>false</c> if the section was not found in the collection.</returns>
        public bool Remove(Section section)
        {
            return _items.Remove(section);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<Section> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        private void CheckIndex(int index)
        {
            if (index >= _items.Count)
            {
                for (int i = _items.Count; i <= index; i++)
                {
                    _items.Add(new Section());
                }
            }
        }
    }
}