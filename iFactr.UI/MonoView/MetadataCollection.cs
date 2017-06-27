using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a collection of key/value pairs for storing arbitrary data.
    /// </summary>
#if !NETCF
    [DebuggerDisplay("Count = {Count}")]
#endif
    public class MetadataCollection : IEnumerable<KeyValuePair<object, object>>, IEnumerable
    {
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
#endif
        private Dictionary<object, object> data;

        /// <summary>
        /// Gets the number of key/value pairs contained within the collection.
        /// </summary>
        public int Count
        {
            get { return data.Count; }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the <paramref name="key"/>, or <c>null</c> if no key was found.</returns>
        public object this[object key]
        {
            get { return data.GetValueOrDefault(key); }
            set
            {
                if (key != null)
                {
                    data[key] = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataCollection"/> class.
        /// </summary>
        public MetadataCollection()
        {
            data = new Dictionary<object, object>();
        }

        /// <summary>
        /// Adds the specified <paramref name="key"/> and <paramref name="value"/> to the collection.
        /// </summary>
        /// <param name="key">The key of the object to add.</param>
        /// <param name="value">The value of the object to add.</param>
        /// <returns><c>true</c> if the object was successfully added to the collection; otherwise, <c>false</c>.</returns>
        public bool Add(object key, object value)
        {
            if (key == null)
            {
                return false;
            }

            try
            {
                data.Add(key, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Removes all keys and values from the collection.
        /// </summary>
        public void Clear()
        {
            data.Clear();
        }

        /// <summary>
        /// Takes every key/value pair contained within the specified <paramref name="collection"/> and copies them to this instance.
        /// </summary>
        /// <param name="collection">The collection whose key/value pairs are to be copied.</param>
        /// <param name="replaceExisting">Whether to replace existing entries with entries in the specified collection if they have identical keys.</param>
        public void Copy(MetadataCollection collection, bool replaceExisting)
        {
            if (collection == null)
            {
                return;
            }

            foreach (var kvp in collection)
            {
                if (replaceExisting || !data.ContainsKey(kvp.Key))
                {
                    data[kvp.Key] = kvp.Value;
                }
            }
        }

        /// <summary>
        /// Returns the value associated with the specified <paramref name="key"/> as an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to return the value as.</typeparam>
        /// <param name="key">The key associated with the value to be returned.</param>
        public T Get<T>(object key)
        {
            var value = data.GetValueOrDefault(key);
            if (value is T)
            {
                return (T)value;
            }

            return default(T);
        }

        /// <summary>
        /// Removes the value with the specified <paramref name="key"/> from the collection.
        /// </summary>
        /// <param name="key">The key of the object to remove.</param>
        /// <returns><c>true</c> if the object was successfully removed to the collection; otherwise, <c>false</c>.</returns>
        public bool Remove(object key)
        {
            if (key == null)
            {
                return false;
            }

            return data.Remove(key);
        }

        IEnumerator<KeyValuePair<object, object>> IEnumerable<KeyValuePair<object, object>>.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }
}
