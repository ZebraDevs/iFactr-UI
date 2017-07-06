using System;

namespace iFactr.UI
{
    /// <summary>
    /// Defines an item in an <see cref="iFactr.UI.IToolbar"/> object that serves as a separator between other items.
    /// </summary>
    public interface IToolbarSeparator : IToolbarItem, IEquatable<IToolbarSeparator>
    {
    }
}
