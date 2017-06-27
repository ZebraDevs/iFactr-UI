using System;

namespace iFactr.UI
{
	/// <summary>
	/// Indicates that the API is not officially supported on web-based targets and may not function.
	/// </summary>
	public sealed class NativeOnlyAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets a web URL that points to further information about the API that is decorated with this attribute.
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="iFactr.UI.NativeOnlyAttribute"/> class.
		/// </summary>
		/// <param name="url">A web URL that points to further information about the API that is decorated with this attribute.</param>
		public NativeOnlyAttribute(string url)
		{
			Url = url;
		}
	}
}

