using System;

namespace iFactr.UI.Reflection
{
    /// <summary>
    /// Describes how a type should handle members that do not have attributes relevant to reflective UI.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
    public sealed class MemberParticipationAttribute : Attribute
    {
        internal Participation Participation { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberParticipationAttribute"/> class.
        /// </summary>
        /// <param name="participation">The default behavior to use when determining which members to build the UI with.</param>
        public MemberParticipationAttribute(Participation participation)
        {
            Participation = participation;
        }
    }

    /// <summary>
    /// Describes how member eligibility should be determined when constructing a reflective UI.
    /// </summary>
    public enum Participation : byte
    {
        /// <summary>
        /// Members must have an appropriate attribute in order to be included during construction of the UI; otherwise, they are ignored.
        /// </summary>
        OptIn,
        /// <summary>
        /// Members are included in the construction of the UI unless explicitly excluded via the <see cref="SkipAttribute"/>.  This is the default.
        /// </summary>
        OptOut
    }
}
