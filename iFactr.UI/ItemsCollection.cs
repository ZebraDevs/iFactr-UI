using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MonoCross.Utilities;

#if NETCF
using System.Collections.ObjectModel;
#else
using System.Diagnostics;
#endif


namespace iFactr.UI
{
    /// <summary>
    /// Represents a thread-safe collection of elements.
    /// </summary>
#if !NETCF
    [DebuggerDisplay("Count = {Count}")]
#endif
#if !PCL
    [Serializable]
#endif
    public class ItemsCollection<T> : IList<T>, IList, IXmlSerializable
    {
        /// <summary>
        /// Gets or sets the number of elements this instance can contain.
        /// </summary>
        public int Capacity
        {
            get { return internalList.Capacity; }
            set { internalList.Capacity = value; }
        }

        /// <summary>
        /// Gets the number of elements contain in this instance.
        /// </summary>
        public int Count
        {
            get { return internalList.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the collection can be modified.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the collection can be dynamically resized.
        /// </summary>
        public bool IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is synchronized.
        /// </summary>
        public bool IsSynchronized
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the sync root object used to maintain synchronization.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        public object SyncRoot
        {
            get { return internalList; }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        public T this[int index]
        {
            get
            {
                lock (SyncRoot)
                {
                    return internalList[index];
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    internalList[index] = value;
                }
            }
        }

        object IList.this[int index]
        {
            get
            {
                lock (SyncRoot)
                {
                    return internalList[index];
                }
            }
            set
            {
                if (value is T)
                {
                    lock (SyncRoot)
                    {
                        internalList[index] = (T)value;
                    }
                }
            }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
#endif
        private List<T> internalList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsCollection&lt;T&gt;"/> class.
        /// </summary>
        public ItemsCollection()
        {
            internalList = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The initial capacity of the collection.</param>
        public ItemsCollection(int capacity)
        {
            internalList = new List<T>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="collection">The collection containing the elements that will be copied into this collection.</param>
        public ItemsCollection(IEnumerable<T> collection)
        {
            internalList = new List<T>(collection);
        }

#if !PCL
        /// <summary>
        /// Converts the items in this list to a different type
        /// </summary>
        /// <param name="converter">A <see cref="Converter&lt;T,TOutput&gt;"/> that contains logic to convert the items.</param>
        /// <typeparam name="TOutput">The type that the items are converted to</typeparam>
        /// <returns>A <see cref="List&lt;T&gt;"/> populated with converted items.</returns>
        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            return internalList.ConvertAll(converter);
        }

        /// <summary>
        /// Determines whether the collection contains elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The predicate delegate that specifies the elements to search for.</param>
        public bool Exists(Predicate<T> match)
        {
            return internalList.Exists(match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate and returns the first occurrence.
        /// </summary>
        /// <param name="match">The predicate delegate that specifies the element to search for.</param>
        public T Find(Predicate<T> match)
        {
            return internalList.Find(match);
        }

        /// <summary>
        /// Searches for elements that match the conditions defined by the specified predicate and returns them.
        /// </summary>
        /// <param name="match">The predicate delegate that specifies the elements to search for.</param>
        public List<T> FindAll(Predicate<T> match)
        {
            return internalList.FindAll(match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate and returns the index of the first occurrence.
        /// </summary>
        /// <param name="match">The predicate delegate that specifies the element to search for.</param>
        public int FindIndex(Predicate<T> match)
        {
            return internalList.FindIndex(match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate and returns the last occurrence.
        /// </summary>
        /// <param name="match">The predicate delegate that specifies the element to search for.</param>
        public T FindLast(Predicate<T> match)
        {
            return internalList.FindLast(match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate and returns the index of the last occurrence.
        /// </summary>
        /// <param name="match">The predicate delegate that specifies the element to search for.</param>
        public int FindLastIndex(Predicate<T> match)
        {
            return internalList.FindLastIndex(match);
        }

        /// <summary>
        /// Determines whether every element in the collection matches the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The predicate delegate that specifies the elements to search for.</param>
        public bool TrueForAll(Predicate<T> match)
        {
            return internalList.TrueForAll(match);
        }
#endif

        /// <summary>
        /// Adds the specified item to the end of the collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {
            lock (SyncRoot)
            {
                internalList.Add(item);
            }
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of this collection.
        /// </summary>
        /// <param name="collection">The collection containing the elements to add.</param>
        public void AddRange(IEnumerable<T> collection)
        {
            lock (SyncRoot)
            {
                internalList.AddRange(collection);
            }
        }

#if !PCL
        /// <summary>
        /// Returns a read-only wrapper to this collection.
        /// </summary>
        public ReadOnlyCollection<T> AsReadOnly()
        {
            return internalList.AsReadOnly();
        }

        /// <summary>
        /// Performs the specified action on each element of the collection.
        /// </summary>
        /// <param name="action">The action delegate to perform on each element of the collection.</param>
        public void ForEach(Action<T> action)
        {
            internalList.ForEach(action);
        }
#endif

        /// <summary>
        /// Searches the sorted collection for an element and returns the zero-based index of that element.
        /// </summary>
        /// <param name="item">The element for which to search.</param>
        public int BinarySearch(T item)
        {
            return internalList.BinarySearch(item);
        }

        /// <summary>
        /// Searches the sorted collection for an element using the specified comparer and returns the zero-based index of that element.
        /// </summary>
        /// <param name="item">The element for which to search.</param>
        /// <param name="comparer">The comparer to use when comparing elements.</param>
        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return internalList.BinarySearch(item, comparer);
        }

        /// <summary>
        /// Searches a range of elements in the sorted collection for an element using the specified comparer and returns the zero-based index of that element.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="item">The element for which to search.</param>
        /// <param name="comparer">The comparer to use when comparing elements.</param>
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            return internalList.BinarySearch(index, count, item, comparer);
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        public void Clear()
        {
            lock (SyncRoot)
            {
                internalList.Clear();
            }
        }

        /// <summary>
        /// Determines whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">The item to check for.</param>
        public bool Contains(T item)
        {
            return internalList.Contains(item);
        }

        /// <summary>
        /// Copies the entire collection to the specified array.
        /// </summary>
        /// <param name="array">The array to copy the collection to.</param>
        public void CopyTo(T[] array)
        {
            internalList.CopyTo(array);
        }

        /// <summary>
        /// Copies the elements of this collection to an array, starting at the specified index.
        /// </summary>
        /// <param name="array">A one-dimensional, zero-based array that is the destination of the elements being copied.</param>
        /// <param name="arrayIndex">The zero-based index of <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            internalList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Copies a range of elements of this collection to an array, starting at a particular index of the target array.
        /// </summary>
        /// <param name="index">The zero-based index in the source collection at which copying begins.</param>
        /// <param name="array">A one-dimensional, zero-based array that is the destination of the elements being copied.</param>
        /// <param name="arrayIndex">The zero-based index of <paramref name="array"/> at which copying begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            internalList.CopyTo(index, array, arrayIndex, count);
        }

        /// <summary>
        /// Returns an enumerator that can be used to iterate over the collection.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return new ItemsEnumerator(this);
        }

        /// <summary>
        /// Creates a shallow copy of a range of elements in the current collection.
        /// </summary>
        /// <param name="index">The zero-based index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        public List<T> GetRange(int index, int count)
        {
            return internalList.GetRange(index, count);
        }

        /// <summary>
        /// Returns the index of the first occurrence of the specified item.
        /// </summary>
        /// <param name="item">The item to return the index of.</param>
        public int IndexOf(T item)
        {
            return internalList.IndexOf(item);
        }

        /// <summary>
        /// Returns the index of the first occurrence of the specified item.
        /// </summary>
        /// <param name="item">The item to return the index of.</param>
        /// <param name="index">The index of where to begin searching for the item.</param>
        public int IndexOf(T item, int index)
        {
            return internalList.IndexOf(item, index);
        }

        /// <summary>
        /// Returns the index of the first occurrence of the specified item within the specified range.
        /// </summary>
        /// <param name="item">The item to return the index of.</param>
        /// <param name="index">The index of where to begin searching for the item.</param>
        /// <param name="count">The number of elements to search.</param>
        public int IndexOf(T item, int index, int count)
        {
            return internalList.IndexOf(item, index, count);
        }

        /// <summary>
        /// Inserts the specified item into the collection at the specified position.
        /// </summary>
        /// <param name="index">The position to insert the item at.</param>
        /// <param name="item">The item to insert.</param>
        public void Insert(int index, T item)
        {
            lock (SyncRoot)
            {
                internalList.Insert(index, item);
            }
        }

        /// <summary>
        /// Inserts the items of a collection into this collection starting at the specified position.
        /// </summary>
        /// <param name="index">The  zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">The collection of items to insert.</param>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            lock (SyncRoot)
            {
                internalList.InsertRange(index, collection);
            }
        }

        /// <summary>
        /// Returns the index of the last occurrence of the specified item.
        /// </summary>
        /// <param name="item">The item to return the index of.</param>
        public int LastIndexOf(T item)
        {
            return internalList.LastIndexOf(item);
        }

        /// <summary>
        /// Returns the index of the last occurrence of the specified item.
        /// </summary>
        /// <param name="item">The item to return the index of.</param>
        /// <param name="index">The index of where to begin searching for the item.</param>
        public int LastIndexOf(T item, int index)
        {
            return internalList.LastIndexOf(item, index);
        }

        /// <summary>
        /// Returns the index of the last occurrence of the specified item within the specified range.
        /// </summary>
        /// <param name="item">The item to return the index of.</param>
        /// <param name="index">The index of where to begin searching for the item.</param>
        /// <param name="count">The number of elements to search.</param>
        public int LastIndexOf(T item, int index, int count)
        {
            return internalList.LastIndexOf(item, index, count);
        }

        /// <summary>
        /// Removes the first occurrence of the specified item.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public bool Remove(T item)
        {
            lock (SyncRoot)
            {
                return internalList.Remove(item);
            }
        }

        /// <summary>
        /// Removes all elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The predicate delegate that specifies the element to search for.</param>
        public int RemoveAll(Predicate<T> match)
        {
            lock (SyncRoot)
            {
                return internalList.RemoveAll(match);
            }
        }

        /// <summary>
        /// Removes the element at the specified zero-based index.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            lock (SyncRoot)
            {
                internalList.RemoveAt(index);
            }
        }

        /// <summary>
        /// Removes all elements within the specified range.
        /// </summary>
        /// <param name="index">The index at which to begin removing elements.</param>
        /// <param name="count">The number of elements to remove.</param>
        public void RemoveRange(int index, int count)
        {
            lock (SyncRoot)
            {
                internalList.RemoveRange(index, count);
            }
        }

        /// <summary>
        /// Reverses the order of the elements in the collection.
        /// </summary>
        public void Reverse()
        {
            lock (SyncRoot)
            {
                internalList.Reverse();
            }
        }

        /// <summary>
        /// Reverses the order of the elements within the specified range.
        /// </summary>
        /// <param name="index">The starting index of the range of elements to reverse.</param>
        /// <param name="count">The number of elements to reverse.</param>
        public void Reverse(int index, int count)
        {
            lock (SyncRoot)
            {
                internalList.Reverse(index, count);
            }
        }

        /// <summary>
        /// Sorts the elements in the collection using the default comparer.
        /// </summary>
        public void Sort()
        {
            lock (SyncRoot)
            {
                internalList.Sort();
            }
        }

        /// <summary>
        /// Sorts the elements in the collection using the specified comparison.
        /// </summary>
        /// <param name="comparison">The comparison to use when comparing elements.</param>
        public void Sort(Comparison<T> comparison)
        {
            lock (SyncRoot)
            {
                internalList.Sort(comparison);
            }
        }

        /// <summary>
        /// Sorts the elements in the collection using the specified comparer.
        /// </summary>
        /// <param name="comparer">The comparer to use when comparing elements.</param>
        public void Sort(IComparer<T> comparer)
        {
            lock (SyncRoot)
            {
                internalList.Sort(comparer);
            }
        }

        /// <summary>
        /// Sorts the elements in the collection using the specified comparer.
        /// </summary>
        /// <param name="index">The starting index of the range of elements to sort.</param>
        /// <param name="count">The number of elements to sort.</param>
        /// <param name="comparer">The comparer to use when comparing elements.</param>
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            lock (SyncRoot)
            {
                internalList.Sort(index, count, comparer);
            }
        }

        /// <summary>
        /// Copies the elements in the collection to a new array.
        /// </summary>
        /// <returns>The array.</returns>
        public T[] ToArray()
        {
            return internalList.ToArray();
        }

        /// <summary>
        /// Suggests that the capacity be reduced to the actual number of elements in the collection.
        /// </summary>
        public void TrimExcess()
        {
            lock (SyncRoot)
            {
                internalList.TrimExcess();
            }
        }

        /// <summary>
        /// Adds the specified item to the end of the collection.
        /// </summary>
        /// <param name="value">The item to add.</param>
        public int Add(object value)
        {
            if (value is T)
            {
                lock (SyncRoot)
                {
                    internalList.Add((T)value);
                    return internalList.Count - 1;
                }
            }
            return -1;
        }

        /// <summary>
        /// Determines whether the collection contains the specified item.
        /// </summary>
        /// <param name="value">The item to check for.</param>
        public bool Contains(object value)
        {
            if (value is T)
            {
                return internalList.Contains((T)value);
            }
            return false;
        }

        /// <summary>
        /// Returns the index of the first occurrence of the specified item.
        /// </summary>
        /// <param name="value">The item to return the index of.</param>
        public int IndexOf(object value)
        {
            if (value is T)
            {
                return internalList.IndexOf((T)value);
            }
            return -1;
        }

        /// <summary>
        /// Inserts the specified item into the collection at the specified position.
        /// </summary>
        /// <param name="index">The position to insert the item at.</param>
        /// <param name="value">The item to insert.</param>
        public void Insert(int index, object value)
        {
            if (value is T)
            {
                lock (SyncRoot)
                {
                    internalList.Insert(index, (T)value);
                }
            }
        }

        /// <summary>
        /// Removes the first occurrence of the specified item.
        /// </summary>
        /// <param name="value">The item to remove.</param>
        public void Remove(object value)
        {
            if (value is T)
            {
                lock (SyncRoot)
                {
                    internalList.Remove((T)value);
                }
            }
        }

        /// <summary>
        /// Copies the elements of this collection to an array, starting at the specified index.
        /// </summary>
        /// <param name="array">A one-dimensional, zero-based array that is the destination of the elements being copied.</param>
        /// <param name="index">The zero-based index of <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            for (int i = index; i < array.Length; i++)
            {
                array.SetValue(internalList[i - index], i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ItemsEnumerator(this);
        }

        /// <summary>
        /// Implicitly converts an <see cref="ItemsCollection&lt;T&gt;"/> to a <see cref="List&lt;T&gt;"/>.
        /// </summary>
        /// <param name="collection">The collection to convert to a <see cref="List&lt;T&gt;"/>.</param>
        /// <returns>A <see cref="List&lt;T&gt;"/> containing the items from the given collection.</returns>
        public static implicit operator List<T>(ItemsCollection<T> collection)
        {
            return collection.internalList;
        }

        /// <summary>
        /// Implicitly converts a <see cref="List&lt;T&gt;"/> to an <see cref="ItemsCollection&lt;T&gt;"/>.
        /// </summary>
        /// <param name="collection">The collection to convert to an <see cref="ItemsCollection&lt;T&gt;"/>.</param>
        /// <returns>A <see cref="ItemsCollection&lt;T&gt;"/> containing the items of the given collection.</returns>
        public static implicit operator ItemsCollection<T>(List<T> collection)
        {
            return new ItemsCollection<T>(collection);
        }

        private class ItemsEnumerator : IEnumerator<T>
        {
            public T Current
            {
                get { return currentItem; }
            }

            object IEnumerator.Current
            {
                get { return currentItem; }
            }

            private readonly ItemsCollection<T> collection;
            private int currentIndex;
            private T currentItem;

            public ItemsEnumerator(ItemsCollection<T> collection)
            {
                this.collection = collection;
                currentIndex = -1;
                currentItem = default(T);
            }

            public bool MoveNext()
            {
                if (++currentIndex >= collection.Count)
                {
                    return false;
                }

                currentItem = collection[currentIndex];
                return true;
            }

            public void Reset()
            {
                currentIndex = -1;
            }

            public void Dispose()
            {
            }
        }

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema() { return null; }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            lock (SyncRoot)
            {
                internalList.Clear();
                if (reader.IsEmptyElement) { return; }
                reader.Read();
                while (reader.Depth > 2 || reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.Depth > 2)
                    {
                        reader.Read();
                        continue;
                    }
                    var typeName = reader.Name;
                    var type = Type.GetType(typeName);
                    var x = new XmlSerializer(type, new XmlRootAttribute(type.FullName));
                    var s = reader.ReadOuterXml();
                    internalList.Add((T)x.Deserialize(new StringReader(s)));
                }
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    reader.ReadEndElement();
                }
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            lock (SyncRoot)
            {
                foreach (var item in internalList)
                {
                    var type = item.GetType();
                    var s = new MemoryStream();
                    try
                    {
                        var x = new XmlSerializer(type, new XmlRootAttribute(type.FullName));
                        x.Serialize(s, item);
                    }
                    catch (Exception e)
                    {
                        Device.Log.Error("Could not serialize item: " + item, e);
                        continue;
                    }
                    s.Position = 0;
                    var xml = new StreamReader(s).ReadToEnd();
                    xml = xml.Replace("<?xml version=\"1.0\"?>", string.Empty);
                    xml = xml.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
                    writer.WriteRaw(Environment.NewLine + xml.Trim() + Environment.NewLine);
                }
            }
        }
    }
}