// 
// Copyright(c) 2021 Real-Time Weather ASSIST-Software
// https://www.assist.ro
//
// For information about the licensing and copyright of this software please
// contact assist contact@assist.ro
//

using System;
using UnityEngine;

namespace RealTimeWeather.Geocoding.Classes
{
    /// <summary>
    /// <para>
    /// This class manages the administrative BigData API information.
    /// </para>
    /// </summary>
    [Serializable]
    public class Administrative
    {
        #region Constructors
        public Administrative()
        {
            order = 0;
            adminLevel = 0;
            geonameId = 0;
            name = string.Empty;
            description = string.Empty;
            isoName = string.Empty;
            isoCode = string.Empty;
            wikidataId = string.Empty;
        }
        #endregion

        #region Private Variables
        [SerializeField]
        private int order;
        [SerializeField]
        private int adminLevel;
        [SerializeField]
        private int geonameId;
        [SerializeField]
        private string name;
        [SerializeField]
        private string description;
        [SerializeField]
        private string isoName;
        [SerializeField]
        private string isoCode;
        [SerializeField]
        private string wikidataId;
        #endregion

        #region Public Properties
        /// <summary>
        /// Order value consistent across all entities in the Locality Info parent object. Ordered by geographic area.
        /// </summary>
        public int Order
        {
            get
            {
                return order;
            }

            set
            {
                order = value;
            }
        }

        /// <summary>
        /// An administrative level as defined by OpenStreetMaps project.
        /// </summary>
        public int AdminLevel
        {
            get
            {
                return adminLevel;
            }

            set
            {
                adminLevel = value;
            }
        }

        /// <summary>
        /// Unique identifier given by GeoNames.org.
        /// </summary>
        public int GeonameId
        {
            get
            {
                return geonameId;
            }

            set
            {
                geonameId = value;
            }
        }

        /// <summary>
        /// Localised description of the place in the requested language, if available.
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }
        /// <summary>
        /// Localised name of the place in the requested language, if available.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// ISO 3166-2 standard code, if available.
        /// </summary>
        public string IsoCode
        {
            get
            {
                return isoCode;
            }

            set
            {
                isoCode = value;
            }
        }

        /// <summary>
        /// ISO 3166-2 standard name, if available.
        /// </summary>
        public string IsoName
        {
            get
            {
                return isoName;
            }

            set
            {
                isoName = value;
            }
        }

        /// <summary>
        /// Wikidata item identifier, if available.
        /// </summary>
        public string WikidataId
        {
            get
            {
                return wikidataId;
            }

            set
            {
                wikidataId = value;
            }
        }
        #endregion

        #region Public Methods       
        /// <summary>
        /// Concatenates the attributes of the Administrative class into a single string.
        /// </summary>
        public override string ToString()
        {
            return ("\n[Administrative data]\n" +
                   $"Order: {order}\n" +
                   $"Admin level: {adminLevel}\n" +
                   $"Name: {name}\n" +
                   $"Geoname Id: {geonameId}\n" +
                   $"Description: {description}\n" +
                   $"Iso name: {isoName}\n" +
                   $"Iso code: {isoCode}\n" +
                   $"wikidata Id: {wikidataId}\n"
                   );
        }
        #endregion
    }
}