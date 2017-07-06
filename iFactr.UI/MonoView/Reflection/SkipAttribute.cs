using System;

namespace iFactr.UI.Reflection
{
    /// <summary>
    /// Represents a type or member that should be excluded during construction of a reflective UI.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SkipAttribute : Attribute
    {
    }
}

