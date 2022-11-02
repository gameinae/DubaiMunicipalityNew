//
// Copyright(c) 2021 Real-Time Weather ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at hello@assist.ro
//

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using RealTimeWeather.OpenWeatherMap.Classes;
using System.ComponentModel;
using System.Reflection;
using RealTimeWeather.Enums;

namespace RealTimeWeather.OpenWeatherMap
{
    /// <summary>
    /// This module is responsible for requesting weather data from OpenWeatherMap server and parsing them.
    /// </summary>
    public class OpenWeatherMapModule : MonoBehaviour
    {
        #region Const Members
        private const string kServer = "https://api.openweathermap.org/data/2.5/weather?";
        private const string kOneCallServer = "https://api.openweathermap.org/data/2.5/onecall?";
        private const string kInputCityNameIdentifier = "q=";
        private const string kInputCityIDIdentifier = "id=";
        private const string kInputLatitudeIdentifier = "lat=";
        private const string kInputLongitudeIdentifier = "&lon=";
        private const string kInputExcludeIdentifier = "&exclude=current,minutely,alerts";
        private const string kInputZipCodeIdentifier = "zip=";
        private const string kInputAppIDIdentifier = "&appid=";
        private const string kInputUnitsIdentifier = "&units=";
        private const string kInputLanguageIdentifier = "&lang=";
        #endregion

        #region Private Variables
        [Header("Open Weather Map API key")]
        [SerializeField] private string _apiKey;

        [Header("Parameters")]
        [SerializeField]
        private Language _language;
        [SerializeField]
        private Units _units = Units.Metric;
        [SerializeField]
        private RequestMode _requestMode;

        [Header("By City Name")]
        [SerializeField]
        private string _cityName;
        [SerializeField]
        private string _stateCode;
        [SerializeField]
        private string _countryCode;

        [Header("By City ID")]
        [Tooltip("city.list.json.gz => http://bulk.openweathermap.org/sample/")]
        [SerializeField]
        private int _cityID;

        [Header("By Geographic Coordinates")]
        [Range(-90.0f, 90.0f)]
        [SerializeField]
        private float _latitude;

        [Range(-180.0f, 180.0f)]
        [SerializeField]
        private float _longitude;

        [Header("By Zip Code")]
        [SerializeField]
        private string _zipCode;
        [SerializeField]
        private string _countryCodeZ;

        private string requestLink = string.Empty;
        private string requestOneCallAPILink = string.Empty;

        [SerializeField]
        private bool _hourlyWeather;
        [SerializeField]
        private bool _dailyWeather;
        #endregion

        #region Public Variables
        public delegate void OnExceptionRaised(ExceptionType exceptionType, string message);
        public OnExceptionRaised onExceptionRaised;

        public delegate void OnServerResponse(OpenWeatherMapData weatherData);
        public delegate void OnServerOneCallAPIResponse(OpenWeatherOneCallAPIMapData weatherData);
        public OnServerResponse onServerResponse;
        public OnServerOneCallAPIResponse onServerOneCallAPIResponse;

        public bool _showApiKeySettings;
        public bool _showParameterSettings;
        public bool _showWeatherByPeriodSettings;
        #endregion

        #region Public Properties
        /// <summary>
        /// ApiKey is a string value that represents the Open Weather API key that can be generated on the https://openweathermap.org/  
        /// </summary>
        public string ApiKey
        {
            get
            {
                return _apiKey;
            }

            set
            {
                _apiKey = value;
            }
        }

        /// <summary>
        /// Represents an Language enum that specify in what language you get data about the weather description
        /// </summary>
        public Language Language
        {
            get
            {
                return _language;
            }

            set
            {
                _language = value;
            }
        }

        /// <summary>
        /// Represents the type of units for the properties of the weather data
        /// </summary>
        public Units Units
        {
            get
            {
                return _units;
            }

            set
            {
                _units = value;
            }
        }

        /// <summary>
        /// Represents the request mode of obtaining data
        /// </summary>
        public RequestMode RequestMode
        {
            get
            {
                return _requestMode;
            }

            set
            {
                _requestMode = value;
            }
        }

        /// <summary>
        /// Represents a city name (London, Paris, Oslo)
        /// </summary>
        public string CityName
        {
            get
            {
                return _cityName;
            }

            set
            {
                _cityName = value;
            }
        }

        /// <summary>
        /// Represents a state code. Applies only for US (NY => New York)
        /// </summary>
        public string StateCode
        {
            get
            {
                return _stateCode;
            }

            set
            {
                _stateCode = value;
            }
        }

        /// <summary>
        /// Represents a country code. (RO => Romania)
        /// </summary>
        public string CountryCode
        {
            get
            {
                return _countryCode;
            }

            set
            {
                _countryCode = value;
            }
        }

        /// <summary>
        /// Represents a city ID.
        /// Check list of all cities IDs from OpenWeatherMap website. ("city.list.json.gz" => http://bulk.openweathermap.org/sample/)
        /// </summary>
        public int CityID
        {
            get
            {
                return _cityID;
            }

            set
            {
                _cityID = value;
            }
        }

        /// <summary>
        /// Is a float value that represets a geographic coordinate that specifies the north–south position of a point on the Earth's surface.
        /// Latitude must be set according to ISO 6709.
        /// </summary>
        public float Latitude
        {
            get
            {
                return _latitude;
            }

            set
            {
                _latitude = value;
            }
        }

        /// <summary>
        /// Is a float value that represents a geographic coordinate that specifies the east-west position of a point on the Earth's surface.
        /// Longitude must be set according to ISO 6709.
        /// </summary>
        public float Longitude
        {
            get
            {
                return _longitude;
            }

            set
            {
                _longitude = value;
            }
        }

        /// <summary>
        /// Represents a zip code. (Versailles (78000), Yvelines, France)
        /// </summary>
        public string ZipCode
        {
            get
            {
                return _zipCode;
            }

            set
            {
                _zipCode = value;
            }
        }

        /// <summary>
        /// Represents a country code. (RO => Romania)
        /// This is duplicated because is using on "By zip code" request mode
        /// </summary>
        public string CountryCodeZ
        {
            get
            {
                return _countryCodeZ;
            }

            set
            {
                _countryCodeZ = value;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Main functionality which will start the download and parse weather data from OpenWeatherMap
        /// Request is made based on the parameters of the request mode
        /// </summary>
        public void RequestOpenWeatherMapData()
        {
            StartCoroutine(RequestOpenWeatherData());
            if (_hourlyWeather || _dailyWeather)
            {
                StartCoroutine(RequestOpenWeatherPeriodData());
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// This method will create the request link based on the available 4 methods
        /// Methods: by city name, by city ID, by geographic coordinates, by zip code
        /// </summary>
        private void ConstructRequestLink()
        {
            switch (_requestMode)
            {
                case RequestMode.CityName:
                    requestLink = CityNameRequest();
                    break;
                case RequestMode.CityID:
                    requestLink = CityIDRequest();
                    break;
                case RequestMode.GeographicCoordinates:
                    requestLink = GeographicCoordinatesRequest();
                    break;
                case RequestMode.ZipCode:
                    requestLink = ZipCodeRequest();
                    break;
            }

            CompleteRequestLinkWithParameters();
        }

        /// <summary>
        /// This method creates the request link based on the city name, state code and country code
        /// </summary>
        /// <returns>The request link composed using city name, state code and country code details</returns>
        private string CityNameRequest()
        {
            string cityNamePart = _cityName != string.Empty ? _cityName : string.Empty;
            string stateCodePart = _stateCode != string.Empty ? "," + _stateCode : string.Empty;
            string countryCodePart = _countryCode != string.Empty ? "," + _countryCode : string.Empty;

            return kServer + kInputCityNameIdentifier + cityNamePart + stateCodePart + countryCodePart;
        }

        /// <summary>
        /// This method creates the request link based on the city ID
        /// Check list of all cities IDs from OpenWeatherMap website. ("city.list.json.gz" => http://bulk.openweathermap.org/sample/)
        /// </summary>
        /// <returns>The request link composed using city ID details</returns>
        private string CityIDRequest()
        {
            return kServer + kInputCityIDIdentifier + _cityID;
        }

        /// <summary>
        /// This method creates the request link based on the geographic coordinates
        /// </summary>
        /// <returns>The request link composed using geographic coordinates details</returns>
        private string GeographicCoordinatesRequest()
        {
            return kServer + kInputLatitudeIdentifier + _latitude + kInputLongitudeIdentifier + _longitude;
        }

        /// <summary>
        /// This method creates the request link based on the zip code and country code
        /// </summary>
        /// <returns>The request link composed using zip data details</returns>
        private string ZipCodeRequest()
        {
            string countryCodePart = _countryCodeZ != string.Empty ? "," + _countryCodeZ : string.Empty;

            return kServer + kInputZipCodeIdentifier + _zipCode + countryCodePart;
        }

        /// <summary>
        /// This method alteres the request link, for OneCall API, by adding to parameter "exclude" unwanted items: hourly and daily
        /// </summary>
        private void ConstructRequestLinkWithWeatherPeriodParameters()
        {
            requestOneCallAPILink = kOneCallServer + kInputLatitudeIdentifier + _latitude + kInputLongitudeIdentifier + _longitude + kInputExcludeIdentifier;
            if (!_hourlyWeather)
            {
                requestOneCallAPILink += ",hourly";
            }
            if (!_dailyWeather)
            {
                requestOneCallAPILink += ",daily";
            }

            requestOneCallAPILink += kInputAppIDIdentifier + _apiKey;
            requestOneCallAPILink += kInputUnitsIdentifier + _units.ToString().ToLower();
            requestOneCallAPILink += kInputLanguageIdentifier + GetDescriptionEnum(_language);
        }

        /// <summary>
        /// This method represents the last step in creating the request link
        /// We add: api code, type of units (standard, metric or imperial) and the language selected for results
        /// </summary>
        private void CompleteRequestLinkWithParameters()
        {
            requestLink += kInputAppIDIdentifier + _apiKey;
            requestLink += kInputUnitsIdentifier + _units.ToString().ToLower();
            requestLink += kInputLanguageIdentifier + GetDescriptionEnum(_language);
        }

        /// <summary>
        /// This method contains the functionality to parse the results from the server
        /// </summary>
        private IEnumerator RequestOpenWeatherData()
        {
            ConstructRequestLink();

            UnityWebRequest webRequest = UnityWebRequest.Get(requestLink);
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.responseCode != (long)System.Net.HttpStatusCode.OK || webRequest.isHttpError)
            {
                ExceptionType exception = webRequest.isHttpError ? ExceptionType.HTTPException : ExceptionType.SystemException;
                onExceptionRaised?.Invoke(exception, webRequest.downloadHandler.text);
            }
            else
            {
                string resultData = webRequest.downloadHandler.text;
                OpenWeatherMapData resultedData = JsonUtility.FromJson<OpenWeatherMapData>(resultData);
                resultedData.Units = _units;

                onServerResponse?.Invoke(resultedData);
            }
        }

        /// <summary>
        /// This method contains the functionality to request weather from the server
        /// </summary>
        private IEnumerator RequestOpenWeatherPeriodData()
        {
            ConstructRequestLinkWithWeatherPeriodParameters();

            UnityWebRequest webRequest = UnityWebRequest.Get(requestOneCallAPILink);
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.responseCode != (long)System.Net.HttpStatusCode.OK || webRequest.isHttpError)
            {
                ExceptionType exception = webRequest.isHttpError ? ExceptionType.HTTPException : ExceptionType.SystemException;
                onExceptionRaised?.Invoke(exception, webRequest.downloadHandler.text);
            }
            else
            {
                string resultData = webRequest.downloadHandler.text;
                OpenWeatherOneCallAPIMapData resultedData = JsonUtility.FromJson<OpenWeatherOneCallAPIMapData>(resultData);
                resultedData.Units = _units;
                onServerOneCallAPIResponse?.Invoke(resultedData);
            }
        }

        /// <summary>
        /// Gets the description of an enum value of type T
        /// </summary>
        /// <typeparam name="T">The enum type of the value</typeparam>
        /// <param name="source">The value from where the description is obtained</param>
        /// <returns>The description string of the enum value</returns>
        private static string GetDescriptionEnum<T>(T source)
        {
            FieldInfo field = source.GetType().GetField(source.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes != null && attributes.Length > 0 ? attributes[0].Description : source.ToString();
        }
        #endregion
    }
}
