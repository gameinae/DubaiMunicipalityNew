//
// Copyright(c) 2020 Real-Time Weather ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at hello@assist.ro
//

using RealTimeWeather.Classes;
using RealTimeWeather.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RealTimeWeather.UI
{
    /// <summary>
    /// Class used to display weather information in the UI.
    /// </summary>
    public class WeatherUI : MonoBehaviour
    {
        #region Private Const Variables
        private const string kLatStr = "lat ";
        private const string kLongStr = " / long ";
        private const string kSeparatorStr = ", ";
        private const string kCelsiusDegreeStr = "°C";
        private const string kPercentStr = "%";
        private const string kMbarStr = "mbar";
        private const string kMmStr = "mm";
        private const string kKmStr = "km";
        private const string kHumidityStr = "Humidity: ";
        private const string kDewpointStr = "Dewpoint: ";
        private const string kPressureStr = "Pressure: ";
        private const string kPrecipitationStr = "Precipitation: ";
        private const string kVisibilityStr = "Visibility: ";
        private const string kUVIndexStr = "UV index: ";
        private const string kUTCOffsetStr = "UTC offset: ";
        private const string kHourlyForecastStr = "Hourly Forecast: ";
        private const string kDailyForecastStr = "Daily Forecast: ";
        private const string kHourStr = " hour";
        private const string kHoursStr = " hours";
        private const string kDayStr = " day";
        private const string kDaysStr = " days ";

        #endregion

        #region Public Variables
        [Header("Toogles")]
        public Toggle _displayInfo;
        [Header("Info Panel")]
        public GameObject _infoPanel;
        [Header("Separator")]
        public GameObject _separator;
        [Header("Text Properties")]
        public Text _localizationText;
        public Text _geoCoordinatesText;
        public Text _weatherStateText;
        public Text _windText;
        public Text _humidityText;
        public Text _dewpointText;
        public Text _pressureText;
        public Text _precipitationText;
        public Text _visibilityText;
        public Text _indexUVText;
        public Text _offsetUTCText;
        public Text _hourlyForecastText;
        public Text _dailyForecastText;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (RealTimeWeatherManager.instance)
            {
                RealTimeWeatherManager.instance.OnCurrentWeatherUpdate += OnCurrentWeatherUpdate;
                RealTimeWeatherManager.instance.OnHourlyWeatherUpdate += OnHourlyWeatherUpdate;
                RealTimeWeatherManager.instance.OnDailyWeatherUpdate += OnDailyWeatherUpdate;
            }

            _displayInfo.onValueChanged.AddListener(delegate { DisplayInfoToggleChanged(); });
        }

        private void OnDestroy()
        {
            if (RealTimeWeatherManager.instance)
            {
                RealTimeWeatherManager.instance.OnCurrentWeatherUpdate -= OnCurrentWeatherUpdate;
                RealTimeWeatherManager.instance.OnHourlyWeatherUpdate -= OnHourlyWeatherUpdate;
                RealTimeWeatherManager.instance.OnDailyWeatherUpdate -= OnDailyWeatherUpdate;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Handles the current weather data update event.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the received current weather data.</param>
        private void OnCurrentWeatherUpdate(WeatherData weatherData)
        {
            if (weatherData == null)
            {
                return;
            }

            SetLocalizationInfo(weatherData.Localization.City, weatherData.Localization.Country);
            SetGeoCoordinatesInfo(weatherData.Localization);
            SetWeatherStateInfo(weatherData);
            SetWindInfo(weatherData.Wind);
            SetHumidityInfo(weatherData.Humidity);
            SetDewpointInfo(weatherData.Dewpoint);
            SetPressureInfo(weatherData.Pressure);
            SetPrecipitationInfo(weatherData.Precipitation);
            SetVisibilityInfo(weatherData.Visibility);
            SetIndexUVInfo(weatherData.IndexUV);
            SetUTCOffsetInfo(weatherData.UTCOffset);
        }

        /// <summary>
        /// Handles the hourly weather data update event.
        /// </summary>
        /// <param name="weatherDataList">A WeatherData class instance list that represents the received hourly weather data.</param>
        private void OnHourlyWeatherUpdate(List<WeatherData> weatherDataList)
        {
            if (weatherDataList.Count > 0)
            {
                Debug.Log("Hourly Weather Update:");
                foreach (var weatherData in weatherDataList)
                {
                    Debug.Log(weatherData.ToString());
                }
            }

            SetHourlyForecast(weatherDataList.Count);
        }

        /// <summary>
        /// Handles the daily weather data update event.
        /// </summary>
        /// <param name="weatherDataList">A WeatherData class instance list that represents the received daily weather data.</param>
        private void OnDailyWeatherUpdate(List<WeatherData> weatherDataList)
        {
            if (weatherDataList.Count > 0)
            {
                Debug.Log("Daily Weather Update:");
                foreach (var weatherData in weatherDataList)
                {
                    Debug.Log(weatherData.ToString());
                }
            }

            SetDailyForecast(weatherDataList.Count);
        }

        /// <summary>
        /// Function shows/hides info panel, triggered at toggle change value.
        /// </summary>
        public void DisplayInfoToggleChanged()
        {
            _infoPanel.SetActive(_displayInfo.isOn);
            _separator.SetActive(_displayInfo.isOn);
        }

        /// <summary>
        /// Set the localization information, city, and country.
        /// </summary>
        /// <param name="city">A string value that represents the city.</param>
        /// <param name="country">A string value that represents the country.</param>
        public void SetLocalizationInfo(string city, string country)
        {
            _localizationText.text = city.ToUpper() + kSeparatorStr + country.ToUpper();
        }

        /// <summary>
        /// Set geographic coordinates information, latitude and longitude.
        /// </summary>
        /// <param name="localization">A Localization class instance that represents the localization data.</param>
        public void SetGeoCoordinatesInfo(Localization localization)
        {
            _geoCoordinatesText.text = kLatStr
                + localization.Latitude.ToString()
                + kLongStr
                + localization.Longitude.ToString();
        }

        /// <summary>
        /// Set the weather state information.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the received weather data.</param>
        private void SetWeatherStateInfo(WeatherData weatherData)
        {
            _weatherStateText.text = weatherData.WeatherState.ToString()
                + kSeparatorStr
                + weatherData.Temperature.ToString()
                + kCelsiusDegreeStr
                + kSeparatorStr
                + weatherData.TimeZone;
        }

        /// <summary>
        /// Set the wind information.
        /// </summary>
        /// <param name="wind">A Wind class instance that represents wind data.</param>
        private void SetWindInfo(Wind wind)
        {
            _windText.text = wind.ToString();
        }

        /// <summary>
        /// Set the humidity information.
        /// </summary>
        /// <param name="humidity">A float value that represents the humidity in percent.</param>
        private void SetHumidityInfo(float humidity)
        {
            _humidityText.text = kHumidityStr + humidity + kPercentStr;
        }

        /// <summary>
        /// Set the dewpoint information.
        /// </summary>
        /// <param name="dewpoint">A float value that represents the dewpoint in °C.</param>
        private void SetDewpointInfo(float dewpoint)
        {
            _dewpointText.text = kDewpointStr + dewpoint + kCelsiusDegreeStr;
        }

        /// <summary>
        /// Set the pressure information.
        /// </summary>
        /// <param name="pressure">A float value that represents the pressure in mbar.</param>
        private void SetPressureInfo(float pressure)
        {
            _pressureText.text = kPressureStr + pressure + kMbarStr;
        }

        /// <summary>
        /// Set the precipitation information.
        /// </summary>
        /// <param name="precipitation">A float value that represents the precipitation in mm.</param>
        private void SetPrecipitationInfo(float precipitation)
        {
            _precipitationText.text = kPrecipitationStr + precipitation + kMmStr;
        }

        /// <summary>
        /// Set the visibility information.
        /// </summary>
        /// <param name="visibility">A float value that represents the visibility in km.</param>
        private void SetVisibilityInfo(float visibility)
        {
            _visibilityText.text = kVisibilityStr + visibility + kKmStr;
        }

        /// <summary>
        /// Set the UV index information.
        /// </summary>
        /// <param name="indexUV">A float value that represents the UV index.</param>
        private void SetIndexUVInfo(float indexUV)
        {
            _indexUVText.text = kUVIndexStr + indexUV;
        }

        /// <summary>
        /// Set the UTC offset information.
        /// </summary>
        /// <param name="utcOffset">An TimeSpan value that represents the UTC.</param>
        public void SetUTCOffsetInfo(TimeSpan utcOffset)
        {
            _offsetUTCText.text = kUTCOffsetStr + utcOffset.Hours;
        }

        private void SetHourlyForecast(int hours)
        {
            string detailsText = hours > 1 ? kHoursStr : kHourStr;
            _hourlyForecastText.text = kHourlyForecastStr + hours + detailsText;
        }

        private void SetDailyForecast(int days)
        {
            string detailsText = days > 1 ? kDaysStr : kDayStr;
            _dailyForecastText.text = kDailyForecastStr + days + detailsText;
        }

        #endregion
    }
}