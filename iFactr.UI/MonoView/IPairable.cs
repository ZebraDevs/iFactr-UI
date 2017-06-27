namespace iFactr.UI
{
    /// <summary>
    /// Defines an abstract or native object that can be paired with a native or abstract counterpart.
    /// </summary>
    public interface IPairable
    {
        /// <summary>
        /// Gets the abstract or native object that is paired with this instance.
        /// This property is used internally by the framework and user-defined controls, and it should not be used in application logic.
        /// </summary>
        IPairable Pair { get; set; }
    }
}
