using System;
using System.Collections;
using System.Collections.Generic;
using iFactr.Core.Forms;
using iFactr.UI;
using MonoCross.Utilities;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a base class for layer items that serve as collections, including <see cref="Fieldset"/>s, <see cref="iList"/>s, and <see cref="iMenu"/>s.  This class is abstract.
    /// </summary>
    public abstract class iCollection<T> : iLayerItem, IList<T>, IList
    {
        /// <summary>
        /// Gets or sets the name of this instance.
        /// </summary>
        /// <value>The name as a <see cref="String"/> value.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the header text of this instance.
        /// </summary>
        /// <value>The text as a <see cref="String"/> value.</value>
        [Obsolete("Use Header instead.")]
        public string Text
        {
            get { return Header; }
            set { Header = value; }
        }

        /// <summary>
        /// Gets or sets a collection of <typeparamref name="T"/> to render as a group on the layer.
        /// </summary>
        /// <value>
        /// The items as an <see cref="ItemsCollection&lt;T&gt;"/> instance.
        /// </value>
        public ItemsCollection<T> Items { get; set; }

        /// <summary>
        /// Gets the number of items in the Items collection.
        /// </summary>
        public int Count { get { return Items.Count; } }

        /// <summary>
        /// Gets whether the Items collection is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get { return Items.IsReadOnly; }
        }

        /// <summary>
        /// Gets or sets the value at the specified index of the Items collection.
        /// </summary>
        /// <param name="index">The index of the Items collection.</param>
        public T this[int index]
        {
            get { return Items[index]; }
            set { Items[index] = value; }
        }

        /// <summary>
        /// Represents the method that will handle any selection event.
        /// </summary>
        public delegate void SelectEventHandler(T item);

        /// <summary>
        /// Occurs when an item is selected.
        /// </summary>
        public event SelectEventHandler OnItemSelection;

        /// <summary>
        /// Gets a value indicating whether this instance has a selection event handler.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has a selection event handler; otherwise <c>false</c>.
        /// </value>
        public bool HasSelectEventHandler
        {
            get { return OnItemSelection != null; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iCollection"/> class.
        /// </summary>
        public iCollection()
        {
            Items = new List<T>();
        }

        /// <summary>
        /// Adds an element of type <typeparamref name="T"/> to the Items collection.
        /// </summary>
        /// <param name="item">The element instance to add.</param>
        public void Add(T item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Adds a collection of <typeparamref name="T"/> to the Items collection.
        /// </summary>
        /// <param name="items">The items to add.</param>
        public void AddRange(IEnumerable<T> items)
        {
            Items.AddRange(items);
        }

        /// <summary>
        /// Removes all items from the Items collection.
        /// </summary>
        public void Clear()
        {
            Items.Clear();
        }

        /// <summary>
        /// Returns whether the Items collection contains the given item.
        /// </summary>
        /// <param name="item">The item to check for.</param>
        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        /// <summary>
        /// Copies the items of the Items collection to the given array.
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <param name="arrayIndex">The index of the Items collection at which to begin copying.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns the index of the first occurrence of the given item.
        /// </summary>
        /// <param name="item">The item to get the index of.</param>
        public int IndexOf(T item)
        {
            return Items.IndexOf(item);
        }

        /// <summary>
        /// Inserts an element of type <typeparamref name="T"/> into the Items collection at the specified index.
        /// </summary>
        /// <param name="index">The index at which the element should be inserted.</param>
        /// <param name="item">The element to insert.</param>
        public void Insert(int index, T item)
        {
            Items.Insert(index, item);
        }

        /// <summary>
        /// Inserts a collection of elements into the Items collection at the specified index.
        /// </summary>
        /// <param name="index">The index at which the elements in the collection should be inserted.</param>
        /// <param name="items">The elements to insert.</param>
        public void InsertRange(int index, IEnumerable<T> items)
        {
            Items.InsertRange(index, items);
        }

        /// <summary>
        /// Removes an element from the Items collection.
        /// </summary>
        /// <param name="item">The element to remove.</param>
        public bool Remove(T item)
        {
            return Items.Remove(item);
        }

        /// <summary>
        /// Removes the element at the specified index of the Items collection.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        /// <summary>
        /// Removes a range of items from the Items collection.
        /// </summary>
        /// <param name="index">The starting index of the range of items to remove.</param>
        /// <param name="count">The number of items to remove.</param>
        public void RemoveRange(int index, int count)
        {
            Items.RemoveRange(index, count);
        }

        /// <summary>
        /// Returns an enumerator for the Items collections.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator for the Items collections.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Selects the specified item.
        /// </summary>
        /// <param name="item">The item to select.</param>
        public void ItemSelected(T item)
        {
            if (HasSelectEventHandler)
            {
                OnItemSelection(item);
            }
        }

        int IList.Add(object value)
        {
            return Items.Add(value);
        }

        bool IList.Contains(object value)
        {
            return Items.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return Items.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            Items.Insert(index, value);
        }

        bool IList.IsFixedSize
        {
            get { return Items.IsFixedSize; }
        }

        void IList.Remove(object value)
        {
            Items.Remove(value);
        }

        object IList.this[int index]
        {
            get
            {
                return Items[index];
            }
            set
            {
                Items[index] = (T)value;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            Items.CopyTo(array, index);
        }

        bool ICollection.IsSynchronized
        {
            get { return Items.IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return Items.SyncRoot; }
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public new iCollection<T> Clone()
        {
            return (iCollection<T>)CloneOverride();
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        protected override object CloneOverride()
        {
            // First, make a shallow-clone
            var collection = (iCollection<T>)base.CloneOverride();
            // Then allocate new Items (do NOT collection.Clear() as that would wipe the source too)
            collection.Items = new ItemsCollection<T>();  

            foreach (var item in this)
            {
                // Generic type T does not have a Clone method at compile-time,
                // but we can look at the actual object at run-time and find its
                // Clone method, if it exists, using iFactr's Device Reflection.
                var method = Device.Reflector.GetMethod(typeof(T), "Clone");
                if (method != null && method.ReturnType != null)
                {
                    collection.Add((T)method.Invoke(item, null));
                }
                else
                {
                    collection.Add(item);
                }
            }

            return collection;
        }
    }

    /// <summary>
    /// Represents a base class for layer items that serve as <see cref="iItem" /> collections, including <see cref="iList"/>s and <see cref="iMenu"/>s. This class is abstract.
    /// </summary>
    public abstract class iCollection : iCollection<iItem> { }
}