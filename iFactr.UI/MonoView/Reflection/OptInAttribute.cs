using System;

namespace iFactr.UI.Reflection
{
    /// <summary>
    /// Represents the base class for attributes that ensure a member is included when a type's participation is set to <see cref="Participation.OptIn"/>.
    /// </summary>
    public abstract class OptInAttribute : Attribute
    {
    }
}

