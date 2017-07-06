using System;

namespace iFactr.UI.Reflection
{
    /// <summary>
    /// Describes the order in which the member should be presented on screen relative to other members of its declaring type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class OrderAttribute : OptInAttribute
    {
        internal int Index { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderAttribute"/> class.
        /// </summary>
        /// <param name="index">The position of the member on screen.</param>
        public OrderAttribute(int index)
        {
            Index = index;
        }
    }
}
