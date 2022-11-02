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
using UnityEngine;
using RealTimeWeather.AlertSystem;
#if TENKOKU_PRESENT
using Tenkoku.Core;
#endif

namespace RealTimeWeather.Simulation
{
    /// <summary>
    /// Class used to simulate weather using Tenkoku plug-in.
    /// </summary>
    ///
    public class TenkokuModuleController : MonoBehaviour
    {
        #region Private Const Variables
        private const string kTenkokuManagerName = "Tenkoku DynamicSky";
        #endregion

        #region private variable
        private const int kHumidityDivide = 50;
        private const float kMinPrecipitation = 0.02f;

#if TENKOKU_PRESENT
        private bool _testPresets = false;
        private TenkokuModule _tenkokuModule;
#endif
        [SerializeField] private WeatherDataProfile _weatherDataProfile;
        [SerializeField] private WeatherDataProfile _clearWeatherDataProfile;
        [SerializeField] private WeatherDataProfile _fairWeatherDataProfile;
        [SerializeField] private WeatherDataProfile _cloudyWeatherDataProfile;
        [SerializeField] private WeatherDataProfile _partlyCloudyWeatherDataProfile;
        [SerializeField] private WeatherDataProfile _rainWeatherDataProfile;
        [SerializeField] private WeatherDataProfile _stormWeatherDataProfile;
        [SerializeField] private WeatherDataProfile _sunnyWeatherDataProfile;
        #endregion

        /// <summary>
        /// This function creates the TenkokuManager" manager and adds it as a child to the "RealTimeWeatherManager" manager.
        /// </summary>
        public void CreateTenkokuManagerInstance()
        {
#if TENKOKU_PRESENT && UNITY_EDITOR
            _tenkokuModule = FindObjectOfType<TenkokuModule>();
            if (_tenkokuModule == null)
            {
                GameObject tenkokuModulePrefab = RealTimeWeatherManager.GetPrefab(kTenkokuManagerName);
                GameObject tenkokuModuleInstance = Instantiate(tenkokuModulePrefab, Vector3.zero, Quaternion.identity);
                _tenkokuModule = tenkokuModuleInstance.GetComponent<TenkokuModule>();
                _tenkokuModule.enabled = true;
                tenkokuModuleInstance.transform.SetParent(transform);
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            }
#endif
        }
#if TENKOKU_PRESENT

        #region Unity Methods
        private void Awake()
        {
            if (RealTimeWeatherManager.instance)
            {
                RealTimeWeatherManager.instance.OnCurrentWeatherUpdate += OnWeatherUpdate;
            }
        }

        private void OnDestroy()
        {
            if (RealTimeWeatherManager.instance)
            {
                RealTimeWeatherManager.instance.OnCurrentWeatherUpdate -= OnWeatherUpdate;
            }
        }
        private void Update()
        {
            if (_testPresets)
            {
                SetCurrentProfile();
            }
        }
        private void Start()
        {
            var tenkokuModules = FindObjectsOfType<TenkokuModule>();
            if (tenkokuModules.Length > 1)
            {
                LogFile.Write("Multiple Tenkoku Module: " + tenkokuModules.Length);
                Debug.LogError("Multiple Tenkoku Module: " + tenkokuModules.Length);
            }
            _tenkokuModule = FindObjectOfType<TenkokuModule>();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Handles the weather data update event.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the received weather data.</param>
        private void OnWeatherUpdate(WeatherData weatherData)
        {
            SetWeatherState(weatherData);
            SetTimeZone(weatherData);
            SetDate(weatherData);
            SetVisibility(weatherData);
            SetWindsData(weatherData);
            SetTemperature(weatherData);
            SetPrecipitation(weatherData);
        }

        /// <summary>
        /// Sets precipitation in tenkoku.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the received weather data.</param>
        private void SetPrecipitation(WeatherData weatherData)
        {
            float precipitationFromHumidity = (weatherData.Humidity > kHumidityDivide) ? (weatherData.Humidity - kHumidityDivide) / kHumidityDivide : kMinPrecipitation;
            float precipitation = (weatherData.Precipitation > 0) ? weatherData.Precipitation : precipitationFromHumidity;
            switch (weatherData.WeatherState)
            {
                case Enums.WeatherState.RainPrecipitation:
                    _tenkokuModule.weather_RainAmt = precipitation;
                    break;
                case Enums.WeatherState.SnowPrecipitation:
                    _tenkokuModule.weather_SnowAmt = precipitation;
                    break;
                case Enums.WeatherState.RainSnowPrecipitation:
                    _tenkokuModule.weather_RainAmt = precipitation;
                    _tenkokuModule.weather_SnowAmt = precipitation;
                    break;
                default:
                    _tenkokuModule.weather_RainAmt = weatherData.Precipitation;
                    break;
            }
        }

        /// <summary>
        /// Sets the temperature in tenkoku.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the received weather data.</param>
        private void SetTemperature(WeatherData weatherData)
        {
            _tenkokuModule.weather_temperature = weatherData.Temperature;
        }

        /// <summary>
        /// Sets the weather state in tenkoku.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the received weather data.</param>
        private void SetWeatherState(WeatherData weatherData)
        {
            switch (weatherData.WeatherState)
            {
                case Enums.WeatherState.Clear:
                case Enums.WeatherState.PartlyClear:
                    _weatherDataProfile = _clearWeatherDataProfile;
                    break;
                case Enums.WeatherState.Fair:
                    _weatherDataProfile = _fairWeatherDataProfile;
                    break;
                case Enums.WeatherState.Cloudy:
                    _weatherDataProfile = _cloudyWeatherDataProfile;
                    break;
                case Enums.WeatherState.PartlyCloudy:
                case Enums.WeatherState.Mist:
                case Enums.WeatherState.Windy:
                    _weatherDataProfile = _partlyCloudyWeatherDataProfile;
                    break;
                case Enums.WeatherState.Thunderstorms:
                    _weatherDataProfile = _stormWeatherDataProfile;
                    break;
                case Enums.WeatherState.RainSnowPrecipitation:
                case Enums.WeatherState.RainPrecipitation:
                case Enums.WeatherState.SnowPrecipitation:
                    _weatherDataProfile = _rainWeatherDataProfile;
                    break;
                case Enums.WeatherState.PartlySunny:
                case Enums.WeatherState.Sunny:
                    _weatherDataProfile = _sunnyWeatherDataProfile;
                    break;
            }

            SetCurrentProfile();
        }


        /// <summary>
        /// Sets the current weather profile in tenkoku.
        /// </summary>
        private void SetCurrentProfile()
        {
            _tenkokuModule.weather_setAuto = _weatherDataProfile.Parameter.setAuto;
            _tenkokuModule.weather_qualityCloud = _weatherDataProfile.Parameter.qualityCloud;
            _tenkokuModule.weather_cloudAltAmt = _weatherDataProfile.Parameter.cloudAltAmt;
            _tenkokuModule.weather_cloudAltoStratusAmt = _weatherDataProfile.Parameter.cloudAltoStratusAmt;
            _tenkokuModule.weather_cloudCirrusAmt = _weatherDataProfile.Parameter.cloudCirrusAmt;
            _tenkokuModule.weather_cloudCumulusAmt = _weatherDataProfile.Parameter.cloudCumulusAmt;
            _tenkokuModule.weather_cloudScale = _weatherDataProfile.Parameter.cloudScale;
            _tenkokuModule.weather_cloudSpeed = _weatherDataProfile.Parameter.cloudSpeed;
            _tenkokuModule.weather_OvercastAmt = _weatherDataProfile.Parameter.overcastAmt;
            _tenkokuModule.weather_OvercastDarkeningAmt = _weatherDataProfile.Parameter.overcastDarkeningAmt;
            _tenkokuModule.weather_lightning = _weatherDataProfile.Parameter.lightning;
            _tenkokuModule.weather_rainbow = _weatherDataProfile.Parameter.rainbow;
            _tenkokuModule.weather_lightningDir = _weatherDataProfile.Parameter.lightningDir;
            _tenkokuModule.weather_lightningRange = _weatherDataProfile.Parameter.lightningRange;
            _tenkokuModule.autoFog = _weatherDataProfile.Parameter.setFogAuto;
        }

        /// <summary>
        /// Sets the date in tenkoku.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the received weather data.</param>
        private void SetDate(WeatherData weatherData)
        {
            _tenkokuModule.autoDateSync = false;
            _tenkokuModule.currentYear = weatherData.DateTime.Year;
            _tenkokuModule.currentMonth = weatherData.DateTime.Month;
            _tenkokuModule.currentDay = weatherData.DateTime.Day;
            _tenkokuModule.currentHour = weatherData.DateTime.Hour;
            _tenkokuModule.currentMinute = weatherData.DateTime.Minute;
            _tenkokuModule.currentSecond = weatherData.DateTime.Second;
            _tenkokuModule.setTZOffset = weatherData.UTCOffset.Hours;
        }

        /// <summary>
        /// Sets time zone in tenkoku.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the received weather data.</param>
        private void SetTimeZone(WeatherData weatherData)
        {
            _tenkokuModule.setLatitude = weatherData.Localization.Latitude;
            _tenkokuModule.setLongitude = weatherData.Localization.Longitude;
        }

        /// <summary>
        /// Sets humidity in tenkoku.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the received weather data.</param>
        private void SetVisibility(WeatherData weatherData)
        {
            _tenkokuModule.weather_humidity = weatherData.Humidity / kHumidityDivide;
        }

        /// <summary>
        /// Sets wind in tenkoku.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the received weather data.</param>
        private void SetWindsData(WeatherData weatherData)
        {
            float radians = (float)Math.Atan2(weatherData.Wind.Direction.x, weatherData.Wind.Direction.y);
            float windDir = GetWindDirectionInDegress(radians);
            if (windDir < 0)
            {
                windDir += 360;
            }
            _tenkokuModule.weather_WindDir = windDir;
            _tenkokuModule.weather_WindAmt = weatherData.Wind.Speed / _weatherDataProfile.Parameter.windScaleFactor;
        }

        /// <summary>
        /// Transform wind direction from radians to degrees.
        /// </summary>
        /// <param name="radians">Current wind direction</param>
        /// <returns>Wind direction in degree </returns>
        private static float GetWindDirectionInDegress(float radians)
        {
            return radians * (float)(180 / Math.PI);
        }
        #endregion
#endif
    }
}