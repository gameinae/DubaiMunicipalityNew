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
    /// This class manages BigData API geocoding data.
    /// </para>
    /// </summary>
    [Serializable]
    public class GeocodingData
    {
        #region Constructors
        public GeocodingData()
        {
            latitude = 0f;
            longitude = 0f;
            plusCode = string.Empty;
            localityLanguageRequested = "en";
            continent = string.Empty;
            continentCode = string.Empty;
            countryName = string.Empty;
            countryCode = string.Empty;
            principalSubdivision = string.Empty;
            principalSubdivisionCode = string.Empty;
            city = string.Empty;
            locality = string.Empty;
            postcode = string.Empty;
            localityInfo = new LocalityInfo();
        }
        #endregion

        #region Private Variables
        [Range(-90.0f, 90.0f)]
        [SerializeField]
        private float latitude;
        [SerializeField]
        [Range(-180.0f, 180.0f)]
        private float longitude;
        [SerializeField]
        private string plusCode;
        [SerializeField]
        private string localityLanguageRequested;
        [SerializeField]
        private string continent;
        [SerializeField]
        private string continentCode;
        [SerializeField]
        private string countryName;
        [SerializeField]
        private string countryCode;
        [SerializeField]
        private string principalSubdivision;
        [SerializeField]
        private string principalSubdivisionCode;
        [SerializeField]
        private string city;
        [SerializeField]
        private string locality;
        [SerializeField]
        private string postcode;
        [SerializeField]
        private LocalityInfo localityInfo;
        #endregion

        #region Public Properties
        /// <summary>
        /// <para>
        /// Is a float value that represets a geographic coordinate that specifies the north–south position of a point on the Earth's surface.
        /// </para> 
        /// </summary>
        public float Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                latitude = value;
            }
        }

        /// <summary>
        /// <para>
        /// Is a float value that represents a geographic coordinate that specifies the east-west position of a point on the Earth's surface.
        /// </para> 
        /// </summary>
        public float Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                longitude = value;
            }
        }

        /// <summary>
        /// <para>
        /// Is a string value that represents the Open Location Code.
        /// </para> 
        /// <para>
        /// https://maps.google.com/pluscodes/
        /// </para>
        /// </summary>
        public string PlusCode
        {
            get
            {
                return plusCode;
            }

            set
            {
                plusCode = value;
            }
        }

        /// <summary>
        /// <para>
        /// Is a string value that represents the language input parameter.
        /// </para> 
        /// <para>
        /// The default value is English "en".
        /// </para>
        /// </summary>
        public string LocalityLanguageRequested
        {
            get
            {
                return localityLanguageRequested;
            }

            set
            {
                localityLanguageRequested = value;
            }
        }

        /// <summary>
        /// <para>
        /// Localised Continent name in the requested language.
        /// </para> 
        /// </summary>
        public string Continent
        {
            get
            {
                return continent;
            }

            set
            {
                continent = value;
            }
        }

        /// <summary>
        /// <para>
        /// Is a string value that represents the continent code.
        /// </para> 
        /// </summary>
        public string ContinentCode
        {
            get
            {
                return continentCode;
            }

            set
            {
                continentCode = value;
            }
        }

        /// <summary>
        /// <para>
        /// Is a string value that represents the localised country name in the requested language, if available.
        /// </para> 
        /// </summary>
        public string CountryName
        {
            get
            {
                return countryName;
            }

            set
            {
                countryName = value;
            }
        }

        /// <summary>
        /// <para>
        /// Is a string value that represents the country code as defined by ISO 3166-1 standard.
        /// </para> 
        /// </summary>
        public string CountryCode
        {
            get
            {
                return countryCode;
            }

            set
            {
                countryCode = value;
            }
        }

        /// <summary>
        /// <para>
        /// Is a string value that represents the localised principal subdivision name in the requested language, if available.
        /// </para> 
        /// </summary>
        public string PrincipalSubdivision
        {
            get
            {
                return principalSubdivision;
            }

            set
            {
                principalSubdivision = value;
            }
        }

        /// <summary>
        /// <para>
        /// Is a string value that represents the principal subdivision code as defined by ISO 3166-2 standard.
        /// </para> 
        /// </summary>
        public string PrincipalSubdivisionCode
        {
            get
            {
                return principalSubdivisionCode;
            }

            set
            {
                principalSubdivisionCode = value;
            }
        }

        /// <summary>
        /// <para>
        /// Is a string value that represents the localised city name in the requested language, if available.
        /// </para> 
        /// </summary>
        public string City
        {
            get
            {
                return city;
            }

            set
            {
                city = value;
            }
        }

        /// <summary>
        /// <para>
        /// Is a string value that represents the smallest geographic area recognised to which the target belongs.
        /// </para> 
        /// </summary>
        public string Locality
        {
            get
            {
                return locality;
            }

            set
            {
                locality = value;
            }
        }

        /// <summary>
        /// <para>
        /// Is a string value that represents the postcode.
        /// </para> 
        /// </summary>
        public string Postcode
        {
            get
            {
                return postcode;
            }

            set
            {
                postcode = value;
            }
        }

        /// <summary>
        /// <para>
        /// Detailed reverse geocoded locality information localised to the language as defined by ‘localityLanguage’ request parameter.
        /// </para> 
        /// </summary>
        public LocalityInfo LocalityInfo
        {
            get
            {
                return localityInfo;
            }

            set
            {
                localityInfo = value;
            }
        }
        #endregion

        #region Public Methods       
        /// <summary>
        /// Concatenates the attributes of the GeocodingData class into a single string.
        /// </summary>
        public override string ToString()
        {
            return ("\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> GeocodingData Data <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n" +
                   $"Latitude: {latitude}\n" +
                   $"Longitude: {longitude}\n" +
                   $"Continent: {continent}\n" +
                   $"Continent code: {continentCode}\n" +
                   $"Country name: {countryName}\n" +
                   $"Country code: {CountryCode}\n" +
                   $"Subdivision: {principalSubdivision}\n" +
                   $"Subdivision code: {principalSubdivisionCode}\n" +
                   $"City name: {city}\n" +
                   $"Locality: {locality}\n" +
                   $"Open location code: {plusCode}\n" +
                   $"Postcode: {postcode}\n" +
                   $"Language: {localityLanguageRequested}\n" +
                   $"Locality info: {localityInfo.ToString()}\n"
                   );
        }
        #endregion
    }
}