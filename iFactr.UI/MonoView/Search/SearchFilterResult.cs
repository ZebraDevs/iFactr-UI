namespace iFactr.UI
{
    /// <summary>
    /// Describes how a value is filtered by a search query.
    /// </summary>
    public enum SearchFilterResult
    {
        /// <summary>
        /// The value did not match the query.
        /// </summary>
        ImplicitExclusion,
        /// <summary>
        /// The value matches an exclude query.
        /// </summary>
        ExplicitExclusion,
        /// <summary>
        /// The value implicitly matches the query.
        /// </summary>
        ImplicitInclusion,
        /// <summary>
        /// The value matches the query.
        /// </summary>
        ExplicitInclusion,
    }
}