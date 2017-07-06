using System;
using System.Xml.Serialization;
using iFactr.Core.Controls;
using MonoCross.Utilities;
using iFactr.UI;
using Link = iFactr.Core.Controls.Link;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a base layer for retrieving barcode values from a scanning device.
    /// </summary>
    public class ScanLayer : iLayer
    {
        /// <summary>
        /// Gets or sets the link to the callback URI.
        /// </summary>
        /// <value>The callback <see cref="Controls.Link"/> instance.</value>
        public Link Callback { get; set; }

        /// <summary>
        /// Gets or sets the action button to display.
        /// </summary>
        /// <value>The action button as a <see cref="Link"/> instance.</value>
        [Obsolete("Use the Callback property")]
        public Link ActionButton { get { return Callback; } set { Callback = value; } }

        /// <summary>
        /// Gets or sets the text that is displayed to the user while the layer is on the screen
        /// </summary>
        [Obsolete("Use the HID scan layer to use DisplayText")]
        public string DisplayText { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanLayer"/> class.
        /// </summary>
        public ScanLayer()
        {
            DisplayText = Device.Resources.GetString("ScanNow") ?? "Scan barcode now";
            LayerStyle.LayerBackgroundColor = Color.White;
            LayerStyle.LayerItemBackgroundColor = Color.White;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanLayer"/> class.
        /// </summary>
        /// <param name="uri">The URI that is navigated to when the action button is hit.</param>
        public ScanLayer(string uri)
            : this()
        {
            Callback = new Link(uri) { Text = Device.Resources.GetString("Done") ?? "Done" };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanLayer"/> class.
        /// </summary>
        /// <param name="scanningText">The text that is displayed to the user while the layer is on the screen.</param>
        /// <param name="actionButtonText">The text that is displayed on the layer's single action button.</param>
        /// <param name="uri">The URI that is navigated to when the action button is hit.</param>
        public ScanLayer(string scanningText, string actionButtonText, string uri)
        {
            DisplayText = scanningText;
            Callback = new Link(uri) { Text = actionButtonText, };
            LayerStyle.LayerBackgroundColor = Color.White;
            LayerStyle.LayerItemBackgroundColor = Color.White;
        }

        /// <summary>
        /// Formats a scanned barcode.
        /// </summary>
        /// <param name="barcode">The raw barcode string.</param>
        /// <returns>A <see cref="string"/> containing formatted barcode data.</returns>
        public virtual string ScannedBarcode(string barcode) { return barcode; }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public new ScanLayer Clone()
        {
            return (ScanLayer)CloneOverride();
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        protected override object CloneOverride()
        {
            var layer = (ScanLayer)base.CloneOverride();
            layer.Callback = Callback == null ? null : Callback.Clone();
            return layer;
        }

        /// <summary>
        /// The key that is used in the parameters collection when passing barcode values that have been scanned.
        /// </summary>
        public static string BarcodeKey = "BarcodeValues";

        /// <summary>
        /// The string that separates the barcode values in the parameters collection.
        /// </summary>
        public static string BarcodeSeparatorChar = Environment.NewLine;
    }

    /// <summary>
    /// Represents a layer for retrieving barcode values from a scanning device.
    /// This layer is only designed for scanners that emulate keyboard input.
    /// </summary>
    public class HIDScanLayer : ScanLayer
    {
        /// <summary>
        /// Gets or sets the text that is displayed to the user while the layer is on the screen
        /// </summary>
        [XmlIgnore]
        public new string DisplayText
        {
            get { return base.DisplayText; }
            set { base.DisplayText = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HIDScanLayer"/> class.
        /// </summary>
        public HIDScanLayer() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HIDScanLayer"/> class.
        /// </summary>
        /// <param name="uri">The URI that is navigated to when the action button is hit.</param>
        public HIDScanLayer(string uri)
            : base(uri) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HIDScanLayer"/> class.
        /// </summary>
        /// <param name="scanningText">The text that is displayed to the user while the layer is on the screen.</param>
        /// <param name="actionButtonText">The text that is displayed on the layer's single action button.</param>
        /// <param name="uri">The URI that is navigated to when the action button is hit.</param>
        public HIDScanLayer(string scanningText, string actionButtonText, string uri)
            : base(scanningText, actionButtonText, uri) { }
    }

    /// <summary>
    /// Represents a layer for retrieving barcode values from a scanning device.
    /// This layer is designed for scanners that use the camera.
    /// </summary>
    public class CameraScanLayer : ScanLayer
    {
        /// <summary>
        /// Gets or set the delay before the last barcode scanned will be accepted again as a new scan.
        /// </summary>
        public int DuplicateTimeout { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraScanLayer"/> class.
        /// </summary>
        public CameraScanLayer()
        {
            DuplicateTimeout = 1500;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraScanLayer"/> class.
        /// </summary>
        /// <param name="uri">The URI that is navigated to when the action button is hit.</param>
        public CameraScanLayer(string uri)
            : base(uri)
        {
            DuplicateTimeout = 1500;
        }
    }
}