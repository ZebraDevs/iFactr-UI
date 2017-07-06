using System;
using System.Collections.Generic;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a geographical map on an <see cref="iLayer"/>.
    /// </summary>
    public class Map : iLayerItem
    {
        /// <summary>
        /// Gets or sets the initial start location.
        /// </summary>
        /// <value>The location as a <see cref="GeoCoords"/> instance.</value>
        public GeoCoords Location { get; set; }
        /// <summary>
        /// Gets or sets the annotations of multiple points to be marked on the map.
        /// </summary>
        /// <value>The annotations as a <see cref="List&lt;T&gt;"/> of <see cref="GeoCoords"/>.</value>
        public List<GeoCoords> Annotations { get; set; }
        /// <summary>
        /// Gets or sets the type of map.
        /// </summary>
        /// <value>The type of the map as a <see cref="Types"/> value.</value>
        public Types Type { get; set; }

        /// <summary>
        /// The available types of map.
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// A map with only basic components such as roads and landmarks.
            /// </summary>
            Map,
            /// <summary>
            /// A map that is laid out with satellite imagery.
            /// </summary>
            Satellite,
            /// <summary>
            /// A map with basic components overlaid on top of satellite imagery.
            /// </summary>
            Hybrid
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map()
        {
            Location = new GeoCoords(44.8655776, -93.1475079);
            Annotations = new List<GeoCoords>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="lat">The latitude coordinates.</param>
        /// <param name="lng">The longitude coordinates.</param>
        public Map(double lat, double lng)
        {
            Location = new GeoCoords(lat, lng);
            Annotations = new List<GeoCoords>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="lat">The latitude coordinates.</param>
        /// <param name="lng">The longitude coordinates.</param>
        public Map(string lat, string lng)
        {
            Location = new GeoCoords(lat, lng);
            Annotations = new List<GeoCoords>();
        }

        /// <summary>
        /// Represents a point with latitudinal and longitudinal coordinates.
        /// </summary>
        public struct GeoCoords
        {
            /// <summary>
            /// The latitude coordinates.
            /// </summary>
            public double Lat;

            /// <summary>
            /// The longitude coordinates.
            /// </summary>
            public double Long;

            /// <summary>
            /// Initializes a new instance of the <see cref="GeoCoords"/> struct.
            /// </summary>
            /// <param name="lat">The latitude coordinates.</param>
            /// <param name="lng">The longitude coordinates.</param>
            public GeoCoords(double lat, double lng)
            {
                Lat = lat;
                Long = lng;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="GeoCoords"/> struct.
            /// </summary>
            /// <param name="lat">The latitude coordinates.</param>
            /// <param name="lng">The longitude coordinates.</param>
            public GeoCoords(string lat, string lng)
            {
                try
                {
                    Lat = Convert.ToDouble(lat);
                    Long = Convert.ToDouble(lng);
                }
                catch
                {
                    Lat = 44.8655776;
                    Long = -93.1475079;
                }
            }
        }
    }
}