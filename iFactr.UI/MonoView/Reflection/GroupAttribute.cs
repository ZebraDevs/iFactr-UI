using System;

namespace iFactr.UI.Reflection
{
    /// <summary>
    /// Represents a collection of members that occupy the same cell.  When applied to a type, all members of that type will be grouped together.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class GroupAttribute : OptInAttribute
    {
        internal string Id { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupAttribute"/> class.
        /// </summary>
        /// <param name="id">A string to identify the group.  Members in groups with the same identifier with be placed together.</param>
        public GroupAttribute(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            Id = id;
        }
    }
}
