namespace iFactr.UI
{
    /// <summary>
    /// Describes the direction in which values are passed for a <see cref="Binding"/> object.
    /// </summary>
    public enum BindingMode : byte
    {
        /// <summary>
        /// The value of the target property is updated when the value of the source property is changed,
        /// and the value of the source property is updated when the value of the target property is changed.
        /// </summary>
        TwoWay,
        /// <summary>
        /// The value of the target property is updated when the value of the source property is changed.
        /// This is the default.
        /// </summary>
        OneWayToTarget,
        /// <summary>
        /// The value of the source property is updated when the value of the target property is changed.
        /// </summary>
        OneWayToSource,
    }
}
