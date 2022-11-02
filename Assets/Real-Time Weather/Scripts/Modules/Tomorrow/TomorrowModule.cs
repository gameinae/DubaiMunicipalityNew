//
// Copyright(c) 2021 Real-Time Weather ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at hello@assist.ro
//

using RealTimeWeather.Geocoding.Classes;
using RealTimeWeather.Geocoding;
using RealTimeWeather.Tomorrow.Classes;
using System.Collections;
using System;
using System.Globalization;
using UnityEngine;

namespace RealTimeWeather.Tomorrow
{
    /// <summary>
    /// This module is responsible for Tomorrow API communication.
    /// </summary>
    public class TomorrowModule : MonoBehaviour
    {
        #region Public Delegates
        public delegate void OnTomorrowExceptionRaised(string exceptionMessage);
        public delegate void OnTomorrowDataSent(TomorrowData tomorrowData);

        public OnTomorrowExceptionRaised onTomorrowExceptionRaised;
        public OnTomorrowDataSent onTomorrowDataSent;
        #endregion

        #region Private Variables
        private const string kTomorrowURL = "https://api.tomorrow.io/v4/timelines";
        private const string kApiKeyUrlStr = "?apikey=";
        private const string kLocationUrlStr = "&location=";
        private const string kFieldsUrlStr = "&fields=";
        private const string kSeparatorStr = ",";
        private const string kTemperatureStr = "temperature";
        private const string kDewPointStr = "dewPoint";
        private const string kHumidityStr = "humidity";
        private const string kWindSpeedStr = "windSpeed";
        private const string kWindDirectionStr = "windDirection";
        private const string kWindGustStr = "windGust";
        private const string kPressureSurfaceLevelStr = "pressureSurfaceLevel";
        private const string kPressureSeaLevelStr = "pressureSeaLevel";
        private const string kVisibilityStr = "visibility";
        private const string kPrecipitationIntensityStr = "precipitationIntensity";
        private const string kPrecipitationProbabilityStr = "precipitationProbability";
        private const string kWeatherCodeStr = "weatherCode";
        private const string kCloudCoverStr = "cloudCover";
        private const string kTimestepsStr = "&timesteps=";
        private const string kTimestepCurrentStr = "current";
        private const string kTimestepHourlyStr = "1h";
        private const string kTimestepDailyStr = "1d";
        private const string kEndTimeStr = "&endTime=";
        private const string kTomorrowStr = "Tomorrow service exception: ";
        private const string kTemperatureApparentStr = "temperatureApparent";
        private const string kParticulateMatter25Str = "particulateMatter25";
        private const string kPollutantO3Str = "pollutantO3";
        private const string kPollutantNO2Str = "pollutantNO2";
        private const string kPollutantCOStr = "pollutantCO";
        private const string kPollutantSO2Str = "pollutantSO2";
        private const string kTreeIndexStr = "treeIndex";
        private const string kGrassIndexStr = "grassIndex";
        private const string kWeedIndexStr = "weedIndex";
        private const int kHoursPerDay = 24;

        //Warnings
        private const string kApiWarningMessageStr = "Invalid API key.";
        private const string kLocationWarningMessageStr = "Invalid location(latitude, longitude).";
        #endregion

        #region Private Variables
        [Header("Tomorrow API key")]
        [Tooltip("Tomorrow API key is a string that can be abtained from https://app.tomorrow.io/. \n\nAccess to the Tomorrow API requires a valid access key with the right permissions, allowing it to be used to make requests to specific endpoints.")]
        [SerializeField]
        private string _apiKey;

        [Header("Location")]
        [SerializeField]
        [Tooltip("Is a float value that represents a geographic coordinate that specifies the north–south position of a point on the Earth's surface.\nLatitude must be set according to ISO 6709.")]
        [Range(-90.0f, 90.0f)]
        private float _latitude;
        [SerializeField]
        [Tooltip("Is a float value that represents a geographic coordinate that specifies the east-west position of a point on the Earth's surface.\nLongitude must be set according to ISO 6709.")]
        [Range(-180.0f, 180.0f)]
        private float _longitude;
        private TomorrowAPIRequest _tomorrowWebRequest;
        private ReverseGeocoding _reverseGeocoding;

        [SerializeField]
        [Tooltip("Get the hourly weather forecast.")]
        public bool _hourlyForecast = false;
        [SerializeField]
        [Tooltip("Get weather forecast for next selected hours.")]
        [Range(1, 108)]
        public int _hourlyForecastLength = 1;
        [SerializeField]
        [Tooltip("Get the daily weather forecast.")]
        public bool _dailyForecast = false;
        [SerializeField]
        [Tooltip("Get weather forecast for next selected days.")]
        [Range(1, 14)]
        public int _dailyForecastLength = 1;
        [SerializeField]
        [Tooltip("The force exerted against a surface by the weight of the air above the surface (at the mean sea level).")]
        private bool _pressureSeaLevel = false;
        [SerializeField]
        [Tooltip("The fraction of the sky obscured by clouds when observed from a particular location.")]
        private bool _cloudCover = false;
        [SerializeField]
        [Tooltip("The chance of precipitation that at least some minimum quantity of precipitation will occur within a specified forecast period and location.")]
        private bool _precipitationProbability = false;
        [SerializeField]
        [Tooltip("The maximum brief increase in the speed of the wind, usually less than 20 seconds (at 10m).")]
        private bool _windGust = false;
        [SerializeField]
        [Tooltip("The temperature equivalent perceived by humans, caused by the combined effects of air temperature, relative humidity, and wind speed.")]
        private bool _temperatureApparent = false;
        [SerializeField]
        [Tooltip("The concentration of atmospheric particulate matter (PM) that have a diameter of fewer than 2.5 micrometers.")]
        private bool _particulateMatter25 = false;
        [SerializeField]
        [Tooltip("The concentration of surface Ozone (O3).")]
        private bool _pollutantO3 = false;
        [SerializeField]
        [Tooltip("The concentration of surface Nitrogen Dioxide (NO2).")]
        private bool _pollutantNO2 = false;
        [SerializeField]
        [Tooltip("The concentration of surface Carbon Monoxide (CO2).")]
        private bool _pollutantCO = false;
        [SerializeField]
        [Tooltip("The concentration of surface Sulfur Dioxide (SO2).")]
        private bool _pollutantSO2 = false;
        [SerializeField]
        [Tooltip("The Tomorrow index representing the extent of grains of overall tree pollen or mold spores in a cubic meter of the air.")]
        private bool _treeIndex = false;
        [SerializeField]
        [Tooltip("The Tomorrow index representing the extent of grains of overall grass pollen or mold spores in a cubic meter of the air.")]
        private bool _grassIndex = false;
        [SerializeField]
        [Tooltip("The Tomorrow index representing the extent of grains of overall weed pollen or mold spores in a cubic meter of the air.")]
        private bool _weedIndex = false;
        #endregion

        #region Public Variables
        public bool _showApiKeySettings;
        public bool _showLocationSettings;
        public bool _showWeatherDataSettings;
        #endregion

        #region Public Proprieties
        /// <summary>
        /// <para>
        /// ApiKey is a string value that represents the Tomorrow API key that can be generated on the  https://app.tomorrow.io/
        /// </para>
        /// </summary>
        public string ApiKey
        {
            get { return _apiKey; }
            set { _apiKey = value; }
        }

        /// <summary>
        /// <para>
        /// Is a float value that represents a geographic coordinate that specifies the north–south position of a point on the Earth's surface.
        /// </para>
        /// <para>
        /// Latitude must be set according to ISO 6709.
        /// </para>
        /// </summary>
        public float Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        /// <summary>
        /// <para>
        /// Is a float value that represents a geographic coordinate that specifies the east-west position of a point on the Earth's surface.
        /// </para>
        /// <para>
        ///  Longitude must be set according to ISO 6709.
        /// </para>
        /// </summary>
        public float Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            _reverseGeocoding = new ReverseGeocoding();
            _tomorrowWebRequest = new TomorrowAPIRequest();
            _tomorrowWebRequest.onSendResponse += OnReceiveServerData;
            _tomorrowWebRequest.onErrorRaised += OnServerError;
        }

        private void OnDestroy()
        {
            _tomorrowWebRequest.onSendResponse -= OnReceiveServerData;
            _tomorrowWebRequest.onErrorRaised -= OnServerError;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Main functionality which starts the Tomorrow API web request co-routine.
        /// </summary>
        /// <param name="url">url is a string value that represents the URL of the resource to retrieve via HTTP GET.</param>
        public void RequestTomorrowDataByCustomUrl(string url)
        {
            StartCoroutine(_tomorrowWebRequest.GetRequest(url));
        }

        /// <summary>
        /// Main functionality which starts the Tomorrow API web request co-routine.
        /// </summary>
        public void RequestTomorrowData()
        {
            string url = GenerateTheUrl();
            StartCoroutine(_tomorrowWebRequest.GetRequest(url));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Generates the URL for the Tomorrow API call.
        /// </summary>
        /// <returns>
        /// A string value that represents the URL of the resource to retrieve via HTTP GET.
        /// </returns>
        private string GenerateTheUrl()
        {
            string url = kTomorrowURL;
            url += AddApiKeyStr();
            url += AddLocationStr();
            url += AddFieldsStr();
            url += AddTimestepsStr();
            url += AddEndTimeStr();

            return url;
        }

        /// <summary>
        /// Validates and returns the Tomorrow API key needed for the URL.
        /// </summary>
        private string AddApiKeyStr()
        {
            string apiKeyStr = string.Empty;

            if (_apiKey.Equals(string.Empty))
            {
                onTomorrowExceptionRaised?.Invoke(kTomorrowStr + kApiWarningMessageStr);
            }
            else
            {
                apiKeyStr = kApiKeyUrlStr + _apiKey;
            }

            return apiKeyStr;
        }

        /// <summary>
        /// Validates and returns the location information needed for the URL.
        /// </summary>
        private string AddLocationStr()
        {
            string location = string.Empty;

            if (_latitude.ToString(CultureInfo.InvariantCulture).Equals(string.Empty) || _longitude.ToString(CultureInfo.InvariantCulture).Equals(string.Empty))
            {
                onTomorrowExceptionRaised?.Invoke(kTomorrowStr + kLocationWarningMessageStr);
            }
            else
            {
                string latitude = _latitude.ToString("0.00", CultureInfo.InvariantCulture);
                string longitude = _longitude.ToString("0.00", CultureInfo.InvariantCulture);
                location = kLocationUrlStr + latitude + kSeparatorStr + longitude;
            }

            return location;
        }

        /// <summary>
        /// Returns the weather data information that must be requested from the API.
        /// </summary>
        private string AddFieldsStr()
        {
            string fields = kFieldsUrlStr;
            // Core data
            fields += kTemperatureStr + kSeparatorStr;
            fields += kDewPointStr + kSeparatorStr;
            fields += kHumidityStr + kSeparatorStr;
            fields += kWindSpeedStr + kSeparatorStr;
            fields += kWindDirectionStr + kSeparatorStr;
            fields += kPressureSurfaceLevelStr + kSeparatorStr;
            fields += kVisibilityStr + kSeparatorStr;
            fields += kPrecipitationIntensityStr + kSeparatorStr;
            fields += kWeatherCodeStr + kSeparatorStr;

            //Extra data
            fields += _pressureSeaLevel ? kPressureSeaLevelStr + kSeparatorStr : string.Empty;
            fields += _precipitationProbability ? kPrecipitationProbabilityStr + kSeparatorStr : string.Empty;
            fields += _temperatureApparent ? kTemperatureApparentStr + kSeparatorStr : string.Empty;
            fields += _cloudCover ? kCloudCoverStr + kSeparatorStr : string.Empty;
            fields += _particulateMatter25 ? kParticulateMatter25Str + kSeparatorStr : string.Empty;
            fields += _pollutantCO ? kPollutantCOStr + kSeparatorStr : string.Empty;
            fields += _pollutantNO2 ? kPollutantNO2Str + kSeparatorStr : string.Empty;
            fields += _pollutantO3 ? kPollutantO3Str + kSeparatorStr : string.Empty;
            fields += _pollutantSO2 ? kPollutantSO2Str + kSeparatorStr : string.Empty;
            fields += _grassIndex ? kGrassIndexStr + kSeparatorStr : string.Empty;
            fields += _treeIndex ? kTreeIndexStr + kSeparatorStr : string.Empty;
            fields += _weedIndex ? kWeedIndexStr + kSeparatorStr : string.Empty;
            fields += _windGust ? kWindGustStr : string.Empty;
            return fields;
        }

        /// <summary>
        /// Returns the timesteps information that must be requested from the API.
        /// </summary>
        private string AddTimestepsStr()
        {
            string fields = kTimestepsStr;
            fields += kTimestepCurrentStr + kSeparatorStr;
            fields += _hourlyForecast ? kTimestepHourlyStr + kSeparatorStr : string.Empty;
            fields += _dailyForecast ? kTimestepDailyStr : string.Empty;

            return fields;
        }

        /// <summary>
        /// Returns the endTime information that must be requested from the API.
        /// </summary>
        private string AddEndTimeStr()
        {
            string fields = string.Empty;
            fields += (_hourlyForecast || _dailyForecast) ? kEndTimeStr : string.Empty;
            int dailyToHourlyForecastLength = 0;
            if (_dailyForecast)
            {
                dailyToHourlyForecastLength = _dailyForecastLength * kHoursPerDay;
            }
            int hourlyForecastLength = 0;
            if (_hourlyForecast)
            {
                hourlyForecastLength = _hourlyForecastLength;
            }
            var maxHourlyForecastLength = Math.Max(dailyToHourlyForecastLength, hourlyForecastLength);
            fields += (_dailyForecast || _hourlyForecast) ? DateTime.UtcNow.AddHours(maxHourlyForecastLength).AddMinutes(1).ToString("yyyy-MM-ddTHH:mm:ssK") : string.Empty;
            return fields;
        }

        /// <summary>
        /// Handles the data receive event from TomorrowWebRequest.
        /// </summary>
        /// <param name="jsonData">A string value that represents the requested data in a JSON format.</param>
        private void OnReceiveServerData(string jsonData)
        {
            TomorrowData tomorrowData = JsonUtility.FromJson<TomorrowData>(jsonData);
            tomorrowData.Latitude = _latitude;
            tomorrowData.Longitude = _longitude;
            StartCoroutine(GetGeocodingInformation(tomorrowData));
        }

        /// <summary>
        /// The main functionality that calls RequestGeocodingInformation to get reverse geocoding data
        /// and then sends the complete Tomorrow information by invoking the onTomorrowDataSent delegate.
        /// </summary>
        /// <param name="tomorrowData">A TomorrowData class instance that represents the data received from the Tomorrow.</param>
        private IEnumerator GetGeocodingInformation(TomorrowData tomorrowData)
        {
            CoroutineWithData geoCoroutine = new CoroutineWithData(this, _reverseGeocoding.RequestGeocodingInformation(tomorrowData.Latitude, tomorrowData.Longitude));
            yield return geoCoroutine.Coroutine;

            GeocodingData reverseGeoData = (GeocodingData)geoCoroutine.Result;

            if (reverseGeoData != null)
            {
                tomorrowData.City = reverseGeoData.City;
                tomorrowData.Country = reverseGeoData.CountryName;
            }

            onTomorrowDataSent?.Invoke(tomorrowData);
        }

        /// <summary>
        /// Handles the network and HTTP error events.
        /// </summary>
        /// <param name="errorMessage">A string value that represents the error message.</param>
        private void OnServerError(string errorMessage)
        {
            onTomorrowExceptionRaised?.Invoke(kTomorrowStr + errorMessage);
        }
        #endregion
    }
}