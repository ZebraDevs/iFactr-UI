using System.Collections.Generic;
using System.Linq;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a mechanism for accepting keyboard input through keystrokes.
    /// </summary>
    public class Gesture
    {
        private readonly List<Keystroke> _keystrokes = new List<Keystroke>(2);

        /// <summary>
        /// Initializes a new instance of the <see cref="Gesture"/> class.
        /// </summary>
        public Gesture() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Gesture"/> class.
        /// </summary>
        /// <param name="stroke1">The first keystroke to add.</param>
        public Gesture(Keystroke stroke1)
            : this()
        {
            _keystrokes.Insert(0, stroke1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Gesture"/> class.
        /// </summary>
        /// <param name="stroke1">The first keystroke to add.</param>
        /// <param name="stroke2">The second keystroke to add.</param>
        public Gesture(Keystroke stroke1, Keystroke stroke2)
            : this(stroke1)
        {
            _keystrokes.Insert(0, stroke2);
        }

        /// <summary>
        /// Tests for equality between two <see cref="Gesture"/> instances.
        /// </summary>
        /// <param name="gesture1">The first gesture to test.</param>
        /// <param name="gesture2">The second gesture to test.</param>
        /// <returns><c>true</c> if the gestures are equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Gesture gesture1, Gesture gesture2)
        {
            if ((object)gesture1 == null)
                return (object)gesture2 == null;
            return gesture1.Equals(gesture2);
        }

        /// <summary>
        /// Tests for inequality between two <see cref="Gesture"/> instances.
        /// </summary>
        /// <param name="gesture1">The first gesture to test.</param>
        /// <param name="gesture2">The second gesture to test.</param>
        /// <returns><c>true</c> if the gestures are not equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Gesture gesture1, Gesture gesture2)
        {
            if ((object)gesture1 == null)
                return (object)gesture2 != null;
            return !gesture1.Equals(gesture2);
        }

        /// <summary>
        /// Determines if the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to test for equality.</param>
        /// <returns><c>true</c> if the object is equal to this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var comparingGesture = obj as Gesture;
            return comparingGesture != null && GetHashCode() == comparingGesture.GetHashCode();
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return _keystrokes.Aggregate(1, (current, keystroke) => current * 31 + keystroke.GetHashCode());
        }
    }
}