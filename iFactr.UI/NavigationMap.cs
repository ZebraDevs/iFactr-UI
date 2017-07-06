using System.Collections.Generic;
using iFactr.Core.Layers;
using iFactr.Core.Targets;
using MonoCross.Navigation;

namespace iFactr.Core
{
    /// <summary>
    /// A <see cref="NavigationList"/> implementation that provides a browser controller.
    /// </summary>
    public class NavigationMap : NavigationList
    {
        /// <summary>
        /// Returns a <see cref="MXNavigation"/> from the NavigationMap that matches the specified URL.
        /// </summary>
        /// <param name="url">A <see cref="string"/> representing the URL to match.</param>
        /// <returns>A <see cref="MXNavigation"/> that matches the URL.</returns>
        public override MXNavigation MatchUrl(string url)
        {
            // If there is no result, assume the URL is external and create a new Browser Layer
            return base.MatchUrl(url) ?? (url.Contains(":") ? new MXNavigation(url, new Browser(url), null) : null);
        }
    }
}