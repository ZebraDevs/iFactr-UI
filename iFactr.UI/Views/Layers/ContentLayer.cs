using System;
using System.Collections.Generic;
using iFactr.Core.Controls;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a layer that is specialized in displaying multimedia content.
    /// </summary>
    public abstract class ContentLayer : iLayer
    {
        /// <summary>Gets or sets the <see cref="Uri"/> of the content to display.</summary>
        /// <value>The <see cref="Uri"/> of the content to display.</value>
        /// <remarks>The <see cref="ContentType"/> property comes from this value.</remarks>
        public Uri ContentUri
        {
            get { return _contentUri; }
            set
            {
                _contentUri = value;
                string extension = _contentUri.ToString();
                extension = extension.Substring(extension.LastIndexOf('.'));
                switch (extension.ToLower())
                {
                    case ".mov":
                    case ".mpg":
                    case ".mpeg":
                    case ".mp4":
                        ContentType = Content.Video;
                        break;

                    case ".pdf":
                        ContentType = Content.PDF;
                        break;

                    default:
                        ContentType = Content.Unknown;
                        break;
                }
            }
        }
        Uri _contentUri;

        /// <summary>
        /// Available type of multimedia content.
        /// </summary>
        public enum Content
        {
            /// <summary>
            /// The content is of an unknown type.
            /// </summary>
            Unknown,
            /// <summary>
            /// The content is in the Portable Document Format.
            /// </summary>
            PDF,
            /// <summary>
            /// The content is a video.
            /// </summary>
            Video,
        };

        /// <summary>
        /// Gets the <see cref="Content"/> type of this instance.
        /// </summary>
        /// <value>The content type as a <see cref="Content"/> value.</value>
        public Content ContentType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentLayer"/> class.
        /// </summary>
        public ContentLayer()
            : base()
        {
            ContentType = Content.Unknown;
            //ContentUri = null; for now
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new ContentLayer Clone()
        {
            return (ContentLayer)CloneOverride();
        }
    }
}