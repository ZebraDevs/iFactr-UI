using System.Collections.Generic;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Defines an object that acts as a container for <see cref="IElement"/> objects.
    /// </summary>
    public interface IElementHost
    {
        /// <summary>
        /// Gets a collection of the UI elements that are currently contained within this instance.
        /// </summary>
        IEnumerable<IElement> Children { get; }

        /// <summary>
        /// Adds the specified <see cref="IElement"/> object to this instance.
        /// </summary>
        /// <param name="element">The element to be added to this instance.</param>
        void AddChild(IElement element);

        /// <summary>
        /// Removes the specified <see cref="IElement"/> object from this instance.
        /// </summary>
        /// <param name="element">The element to be removed from this instance.</param>
        void RemoveChild(IElement element);
    }
}
