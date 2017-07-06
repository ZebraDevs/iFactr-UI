using System;
using System.Collections.Generic;
using System.Linq;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a combination of keyboard keys that can be pressed simultaneously to invoke an action.
    /// </summary>
    public class Keystroke
    {
        private readonly List<int> _keys = new List<int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Keystroke"/> class.
        /// </summary>
        public Keystroke() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Keystroke"/> class.
        /// </summary>
        /// <param name="key1">An <see cref="Int32"/> representing the keyboard key that must be pressed.</param>
        public Keystroke(int key1)
            : this()
        {
            _keys.Insert(0, key1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Keystroke"/> class.
        /// </summary>
        /// <param name="key1">An <see cref="Int32"/> representing the first keyboard key that must be pressed.</param>
        /// <param name="key2">An <see cref="Int32"/> representing the second keyboard key that must be pressed.</param>
        public Keystroke(int key1, int key2)
            : this(key1)
        {
            _keys.Insert(0, key2);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Keystroke"/> class.
        /// </summary>
        /// <param name="key1">An <see cref="Int32"/> representing the first keyboard key that must be pressed.</param>
        /// <param name="key2">An <see cref="Int32"/> representing the second keyboard key that must be pressed.</param>
        /// <param name="key3">An <see cref="Int32"/> representing the third keyboard key that must be pressed.</param>
        public Keystroke(int key1, int key2, int key3)
            : this(key1, key2)
        {
            _keys.Insert(0, key3);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Keystroke"/> class.
        /// </summary>
        /// <param name="key1">An <see cref="Int32"/> representing the first keyboard key that must be pressed.</param>
        /// <param name="key2">An <see cref="Int32"/> representing the second keyboard key that must be pressed.</param>
        /// <param name="key3">An <see cref="Int32"/> representing the third keyboard key that must be pressed.</param>
        /// <param name="key4">An <see cref="Int32"/> representing the fourth keyboard key that must be pressed.</param>
        public Keystroke(int key1, int key2, int key3, int key4)
            : this(key1, key2, key3)
        {
            _keys.Insert(0, key4);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Keystroke"/> class.
        /// </summary>
        /// <param name="keys">An <see cref="IEnumerable&lt;T&gt;"/> of <see cref="Int32"/> representing the keyboard keys that must be pressed.</param>
        public Keystroke(IEnumerable<int> keys)
        {
            _keys.AddRange(keys);
        }

        /// <summary>
        /// Tests for equality between two <see cref="Keystroke"/> instances.
        /// </summary>
        /// <param name="stroke1">The first keystroke to test.</param>
        /// <param name="stroke2">The second keystroke to test.</param>
        /// <returns><c>true</c> if the keystrokes are equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Keystroke stroke1, Keystroke stroke2)
        {
            if ((object)stroke1 == null)
                return (object)stroke2 == null;
            return stroke1.Equals(stroke2);
        }

        /// <summary>
        /// Tests for inequality between two <see cref="Keystroke"/> instances.
        /// </summary>
        /// <param name="stroke1">The first keystroke to test.</param>
        /// <param name="stroke2">The second keystroke to test.</param>
        /// <returns><c>true</c> if the keystrokes are not equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Keystroke stroke1, Keystroke stroke2)
        {
            if ((object)stroke1 == null)
                return (object)stroke2 != null;
            return !stroke1.Equals(stroke2);
        }

        /// <summary>
        /// Determines if the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to test for equality.</param>
        /// <returns><c>true</c> if the object is equal to this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var comparingStroke = obj as Keystroke;
            return comparingStroke != null && GetHashCode() == comparingStroke.GetHashCode();
        }

        /// <summary>
        /// Returns the hash code of this instance.
        /// </summary>
        public override int GetHashCode()
        {
            //Order independent, unique values
            return _keys.Select(element => EqualityComparer<int>.Default.GetHashCode(element)).Aggregate(0, (current, curHash) => unchecked(current + (curHash | (curHash >> 32)) * 37));
        }
    }
}
