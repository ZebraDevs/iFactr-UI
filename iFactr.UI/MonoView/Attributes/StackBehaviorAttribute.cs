using System;

namespace iFactr.UI
{
    /// <summary>
    /// Indicates that a view has special behavioral properties to consider when it is pushed onto a history stack.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class StackBehaviorAttribute : Attribute
    {
        /// <summary>
        /// Gets the behavioral properties that will be applied to the view.
        /// </summary>
        public StackBehaviorOptions Options { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StackBehaviorAttribute"/> class.
        /// </summary>
        /// <param name="options">The behavioral properties to apply to the view.</param>
        public StackBehaviorAttribute(StackBehaviorOptions options)
        {
            Options = options;
        }
    }

    /// <summary>
    /// Describes the available behavioral properties that can be applied to a view being pushed onto a history stack.
    /// </summary>
    [Flags]
    public enum StackBehaviorOptions : byte
    {
        /// <summary>
        /// The view has no special behaviors to consider.
        /// </summary>
        Default = 0,
        /// <summary>
        /// The view should only appear in the root of a stack, and it should only be replaced by other
        /// views that also have this option.  Views without this option should always be pushed above.
        /// </summary>
        ForceRoot = 1,
        /// <summary>
        /// The view should not be considered a part of the stack's history; no back button should be provided to it.
        /// </summary>
        HistoryShy = 2
    }
}
