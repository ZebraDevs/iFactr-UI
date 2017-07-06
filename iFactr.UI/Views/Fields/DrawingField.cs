using iFactr.UI;

namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a field with a canvas for drawing on.
    /// </summary>
    public class DrawingField : Field
    {
        /// <summary>
        /// Gets or sets the URL of an optional image to display on the canvas as part of the drawing.
        /// </summary>
        /// <value>The image URL as a <see cref="string"/> value.</value>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL of an optional image to display in the background of the canvas.
        /// </summary>
        /// <value>The image URL as a <see cref="string"/> value.</value>
        public string BackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the signed image identifier. Typically a GUID, but can be any string.
        /// </summary>
        /// <value>The signed image identifier as a <see cref="string"/> value.</value>
        /// <remarks>This property can be used to hydrate a drawing field with an existing drawing, separate from the background image.</remarks>
        public string DrawnImageId { get; set; }

        /// <summary>
        /// Gets or sets the color of the stroke when drawing.
        /// </summary>
        /// <value>The stroke color as a <see cref="UI.Color"/>.</value>
        public Color StrokeColor { get; set; }

        /// <summary>
        /// Gets or sets whether to include the background image as part of the drawing when submitting.
        /// </summary>
        /// <value><c>true</c> to combine the drawing and background; otherwise <c>false</c>.</value>
        public bool CompositeResult { get; set; }


        /// <summary>
        /// Gets or sets the drawing type of this instance.
        /// </summary>
        /// <value>The drawing type as a <see cref="DrawingType"/> value.</value>
        public DrawingType DrawingFieldType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingField"/> class with no submission ID.
        /// </summary>
        public DrawingField() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingField"/> class using the ID provided.
        /// </summary>
        /// <param name="id">A <see cref="string"/> representing the ID.</param>
        /// <param name="label">A <see cref="string"/> representing the label value.</param>
        /// <param name="drawingFieldType">The drawing type as a <see cref="DrawingType"/> value.</param>
        /// <param name="backgroundImage">A <see cref="string"/> representing the URL to an optional background image.</param>
        /// <param name="compositeResult">Whether to include the background image as part of the drawing when submitting.</param>
        public DrawingField(string id, string label, DrawingType drawingFieldType, string backgroundImage, bool compositeResult)
        {
            ID = id;
            Label = label;
            DrawingFieldType = drawingFieldType;
            BackgroundImage = backgroundImage;
            CompositeResult = compositeResult;
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new DrawingField Clone()
        {
            return (DrawingField)CloneOverride();
        }
    }

    /// <summary>
    /// The available layouts for <see cref="DrawingField"/>s.
    /// </summary>
    public enum DrawingType
    {
        /// <summary>
        /// Lays out a <see cref="DrawingField"/> for signature capture.
        /// </summary>
        Signature,

        /// <summary>
        /// Lays out a <see cref="DrawingField"/> for canvas painting.
        /// </summary>
        Canvas,
    }
}