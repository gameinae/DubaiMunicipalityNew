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
    /// This class manages the non-administrative BigData API information.
    /// </para>
    /// </summary>
    [Serializable]
    public class Informative
    {
        #region Constructors
        public Informative()
        {
            order = 0;
            geonameId = 0;
            name = string.Empty;
            description = string.Empty;
            isoCode = string.Empty;
            wikidataId = string.Empty;
        }
        #endregion

        #region Private Variables
        [SerializeField]
        private int order;
        [SerializeField]
        private int geonameId;
        [SerializeField]
        private string name;
        [SerializeField]
        private string description;
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
        /// Wikidata item identifier,if available.
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
        /// Concatenates the attributes of the Informative class into a single string.
        /// </summary>
        public override string ToString()
        {
            return ("\n[Informative data]\n" +
                   $"Order: {order}\n" +
                   $"Geoname Id: {geonameId}\n" +
                   $"Name: {name}\n" +
                   $"Description: {description}\n" +
                   $"Iso code: {isoCode}\n" +
                   $"wikidata Id: {wikidataId}\n"
                   );
        }
        #endregion
    }
}