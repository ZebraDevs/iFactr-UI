namespace iFactr.UI
{
    /// <summary>
    /// Describes how content is stretched to fit the space that is allotted for it.
    /// </summary>
	public enum ContentStretch : byte
	{
        /// <summary>
        /// The content is not stretched.
        /// </summary>
		None = 0,
        /// <summary>
        /// The content resizes itself to match the dimensions of the space.  Aspect ratio is not preserved.
        /// </summary>
		Fill = 1,
        /// <summary>
        /// The content stretches itself to fill as much of the space as possible without spilling over.
        /// Aspect ratio is preserved.
        /// </summary>
		Uniform = 2,
        /// <summary>
        /// The content stretches itself to fill the entire space while preserving aspect ratio.
        /// Any portion of the content that spills over will be clipped.
        /// </summary>
		UniformToFill = 3
	}
}

