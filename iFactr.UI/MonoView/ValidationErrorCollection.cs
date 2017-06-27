using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a collection of control submission keys and any errors that have occurred while validating the control.
    /// </summary>
#if !NETCF
    [DebuggerDisplay("Count = {Count}")]
#endif
    public sealed class ValidationErrorCollection : IDictionary<string, string[]>
    {
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private IDictionary<string, string[]> items;

        /// <summary>
        /// Gets the number of entries contained in the collection.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        bool ICollection<KeyValuePair<string, string[]>>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a collection containing the keys for this instance.
        /// These keys are the submission keys of the controls that have encountered validation errors.
        /// </summary>
        public ICollection<string> Keys
        {
            get { return items.Keys; }
        }

        /// <summary>
        /// Gets a collection containing the values for this instance.
        /// These values are the validation errors that have occurred for each control.
        /// </summary>
        public ICollection<string[]> Values
        {
            get { return items.Values; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.ValidationErrorCollection"/> class.
        /// </summary>
        public ValidationErrorCollection()
        {
            items = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.ValidationErrorCollection"/> class.
        /// </summary>
        /// <param name="initialValues">An <see cref="T:IDictionary&lt;string, string[]&gt;"/> containing elements to copy over to the new instance.</param>
        public ValidationErrorCollection(IDictionary<string, string[]> initialValues)
        {
            items = new Dictionary<string, string[]>(initialValues);
        }

        /// <summary>
        /// Adds an entry with the specified submission key and validation errors to the collection.
        /// </summary>
        public void Add(string submitKey, params string[] errors)
        {
            items.Add(submitKey, errors);
        }

        /// <summary>
        /// Determines whether the collection contains an entry with the specified submission key.
        /// </summary>
        /// <returns><c>true</c> if the key was located in the collection; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(string submitKey)
        {
            return items.ContainsKey(submitKey);
        }

        /// <summary>
        /// Removes the entry with the specified submission key from the collection.
        /// </summary>
        /// <returns><c>true</c> if the entry was successfully removed; otherwise, <c>false</c>.</returns>
        public bool Remove(string submitKey)
        {
            return items.Remove(submitKey);
        }

        /// <summary>
        /// Gets the validation errors associated with the specified submission key.
        /// </summary>
        /// <param name="submitKey">The submission key whose associated validation errors to get.</param>
        /// <param name="errors">When the method returns, the errors associated with the specified submission key, if the key was found; otherwise, <c>null</c>."/></param>
        /// <returns><c>true</c> if the validation errors were successfully retrieved; otherwise, <c>false</c>.</returns>
        public bool TryGetValue(string submitKey, out string[] errors)
        {
            return items.TryGetValue(submitKey, out errors);
        }

        /// <summary>
        /// Gets or sets the validation errors associated with the specified submission key.
        /// </summary>
        /// <param name="submitKey">The submission key associated with the validation errors to get or set.</param>
        /// <returns>The validation errors associated with the <paramref name="submitKey"/>.</returns>
        public string[] this[string submitKey]
        {
            get { return items[submitKey]; }
            set { items[submitKey] = value; }
        }

        /// <summary>
        /// Removes all entries from the collection.
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        void ICollection<KeyValuePair<string, string[]>>.Add(KeyValuePair<string, string[]> item)
        {
            Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<string, string[]>>.Contains(KeyValuePair<string, string[]> item)
        {
            return items.Contains(item);
        }

        void ICollection<KeyValuePair<string, string[]>>.CopyTo(KeyValuePair<string, string[]>[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, string[]>>.Remove(KeyValuePair<string, string[]> item)
        {
            return items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}