using System.Xml.Serialization;
namespace iFactr.Core.Controls
{
    /// <summary>
    /// The available types of actions to perform.
    /// </summary>
    [XmlType("CoreActionType", TypeName = "CoreActionType", Namespace = "Core")]
    public enum ActionType
    {
        /// <summary>
        /// An undefined or default action.
        /// </summary>
        Undefined,
        /// <summary>
        /// The action performs an addition or creation operation.
        /// </summary>
        Add,
        /// <summary>
        /// The action performs a cancellation operation.
        /// </summary>
        Cancel,
        /// <summary>
        /// The action performs an edit operation.
        /// </summary>
        Edit,
        /// <summary>
        /// The action performs a deletion operation.
        /// </summary>
        Delete,
        /// <summary>
        /// The action performs an operation for displaying additional information.
        /// </summary>
        More,
        /// <summary>
        /// The action performs a form submission operation.
        /// </summary>
        Submit,
        /// <summary>
        /// No action is performed.
        /// </summary>
        None,
    }
}