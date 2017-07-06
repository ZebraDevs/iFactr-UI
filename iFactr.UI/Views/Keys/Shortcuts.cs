using iFactr.Core.Controls;
using MonoCross.Navigation;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a collection of keyboard shortcuts that initiate navigations when the appropriate keys are pressed.
    /// </summary>
    public class Shortcuts : SerializableDictionary<Gesture, Link>
    {
        /// <summary>
        /// Adds a keystroke and a link to the collection of shortcuts.
        /// </summary>
        /// <param name="key">The keys to press to initiate the navigation.</param>
        /// <param name="value">The link to navigate to when the keys are pressed.</param>
        public void Add(Keystroke key, Link value)
        {
            Add(new Gesture(key), value);
        }
    }
}