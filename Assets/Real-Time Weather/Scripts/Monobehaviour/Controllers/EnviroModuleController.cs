//
// Copyright(c) 2020 Real-Time Weather ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at hello@assist.ro
//

using RealTimeWeather.Classes;
using RealTimeWeather.Enums;
using RealTimeWeather.Managers;
using System;
using UnityEngine;

namespace RealTimeWeather.Simulation.Enviro
{
    /// <summary>
    /// Class used to simulate weather using Enviro plug-in.
    /// </summary>
    public class EnviroModuleController : MonoBehaviour
    {
        #region Private Const Variables
        private const string kEnviroManagerName = "EnviroSkyManager";

        private const float kZeroValue = 0f;
        private const float kCloudsUpwardsIntensity = 0.015f;
        private const float kOneValue = 1f;
        private const float kHundredValue = 100f;
        private const float kThousand = 1000f;
        private const float kPrecipitationMean = 50f;

        private const int kStateClearId = 0;
        private const int kStatePartlyClearId = 1;
        private const int kStateSunnyId = 0;
        private const int kStatePartlySunnyId = 1;
        private const int kStateCloudyId = 2;
        private const int kStatePartlyCloudyId = 4;
        private const int kStateThunderstormsId = 10;
        private const int kStateHeavyPrecipitationId = 7;
        private const int kStateRainPrecipitationId = 6;
        private const int kStateRainSnowPrecipitationId = 11;
        private const int kStateHeavySnowPrecipitationId = 8;
        private const int kStateLightSnowPrecipitationId = 9;
        private const int kStateFairId = 1;
        private const int kStateMistId = 5;
        private const int kStateWindyId = 3;
        #endregion

        #region Public Variables
        [Header("Enviro Weather Presets Manager")]
        public EnviroModulePresetsManager _enviroPresetManager;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (RealTimeWeatherManager.instance)
            {
                RealTimeWeatherManager.instance.OnCurrentWeatherUpdate += OnWeatherUpdate;
            }
        }

        private void Start()
        {
#if ENVIRO_PRESENT
            if (EnviroSkyMgr.instance == null || !EnviroSkyMgr.instance.IsAvailable())
            {
                this.enabled = false;
                return;
            }
#endif
        }

        private void OnDestroy()
        {
            if (RealTimeWeatherManager.instance)
            {
                RealTimeWeatherManager.instance.OnCurrentWeatherUpdate -= OnWeatherUpdate;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This function creates the "EnviroSkyManager" manager and adds it as a child to the "RealTimeWeatherManager" manager.
        /// </summary>
        public void CreateEnviroManagerInstance()
        {
#if ENVIRO_PRESENT && UNITY_EDITOR
            if (EnviroSkyMgr.instance == null)
            {
                GameObject enviroManagerObj = new GameObject();
                enviroManagerObj.name = kEnviroManagerName;
                EnviroSkyMgr enviroManager = enviroManagerObj.AddComponent<EnviroSkyMgr>();
                enviroManagerObj.AddComponent<EnviroEvents>();
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            }
#endif
        }

        /// <summary>
        /// Initializes Enviro components.
        /// </summary>
        public void InitializeEnviro()
        {
#if ENVIRO_PRESENT
            if (EnviroSkyMgr.instance == null)
            {
                return;
            }

#if UNITY_STANDALONE
            if (!EnviroSkyMgr.instance.enviroHDInstance && !EnviroSkyMgr.instance.enviroLWInstance)
            {
                EnviroSkyMgr.instance.CreateEnviroHDInstance();
                EnviroSkyMgr.instance.ActivateHDInstance();
                EnviroSkyMgr.instance.AssignAndStart(Camera.main.gameObject, Camera.main);
                EnviroSkyMgr.instance.ReInit();
            }
#endif

#if (UNITY_ANDROID || UNITY_IOS) && (ENVIRO_HD || ENVIRO_LW)
            if (!EnviroSkyMgr.instance.enviroLWInstance && !EnviroSkyMgr.instance.enviroHDInstance)
            {
                EnviroSkyMgr.instance.CreateEnviroLWMobileInstance();
                EnviroSkyMgr.instance.ActivateLWInstance();
                EnviroSkyMgr.instance.AssignAndStart(Camera.main.gameObject, Camera.main);
                EnviroSkyMgr.instance.ReInit();
            }
#endif
            EnviroSkyMgr.instance.SetTimeProgress(EnviroTime.TimeProgressMode.None);
            EnviroSkyMgr.instance.SetAutoWeatherUpdates(false);

#if ENVIRO_HD || ENVIRO_LW
            EnviroSkyMgr.instance.Seasons.calcSeasons = false;
            EnviroSkyMgr.instance.WeatherSettings.wetnessDryingSpeed = kZeroValue;
            EnviroSkyMgr.instance.WeatherSettings.wetnessAccumulationSpeed = kZeroValue;
            EnviroSkyMgr.instance.CloudSettings.cloudsUpwardsWindIntensity = kCloudsUpwardsIntensity;
#endif

            _enviroPresetManager.IntiliazileEnviroPresets();

            SetUseWindZoneDirection(false);
            SetVolumeClouds(true);
            SetFlatClouds(false);
            SetParticleClouds(false);
            SetFog(true);
            SetVolumeLighting(true);
            SetSunShafts(true);
            SetMoonShafts(true);
#endif
        }
        #endregion

        #region Private Methods

        #region Events
        /// <summary>
        /// Handles the weather data update event.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the received weather data.</param>
        private void OnWeatherUpdate(WeatherData weatherData)
        {
#if ENVIRO_PRESENT
            if (weatherData == null)
            {
                return;
            }

            SetLocalization(weatherData.Localization);
            SetTime(weatherData.DateTime);
            SetWeatherState(weatherData);
            SetSeason(weatherData.DateTime.Month);
            SetTemperature(weatherData.Temperature);
            SetHumidity(weatherData.Humidity);
            SetWind(weatherData.Wind);
            SetIndexUV(weatherData.IndexUV);
            SetVisibility(weatherData.Visibility);
            SetPrecipitation(weatherData.Precipitation);
            SetUTCOffset(weatherData.UTCOffset);
#endif
        }
        #endregion

        #region Localization Methods
        /// <summary>
        /// Set the localization, latitude and longitude values.
        /// </summary>
        /// <param name="localization">A Localization class instance that represents the localization data.</param>
        public void SetLocalization(Localization localization)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.Time.Latitude = localization.Latitude;
            EnviroSkyMgr.instance.Time.Longitude = localization.Longitude;
#endif
        }
        #endregion

        #region Time Methods
        /// <summary>
        /// Set the exact date, by DateTime.
        /// </summary>
        /// <param name="dateTime">An DateTime value that represents the date.</param>
        public void SetTime(DateTime dateTime)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.SetTime(dateTime);
#endif
        }

        /// <summary>
        /// Set the UTC offset, by TimeSpan.
        /// </summary>
        /// <param name="utcOffset">An TimeSpan value that represents the UTC.</param>
        public void SetUTCOffset(TimeSpan utcOffset)
        {
#if ENVIRO_PRESENT
#if ENVIRO_HD
            if (EnviroSkyMgr.instance.enviroHDInstance)
            {
                EnviroSkyMgr.instance.enviroHDInstance.GameTime.utcOffset = utcOffset.Hours;
            }
#endif
#if ENVIRO_LW
            if (EnviroSkyMgr.instance.enviroLWInstance)
            {
                EnviroSkyMgr.instance.enviroLWInstance.GameTime.utcOffset = utcOffset.Hours;
            }
#endif
#endif
        }

        /// <summary>
        /// Set the season.
        /// </summary>
        /// <param name="month">An int value that represents the month.</param>
        public void SetSeason(int month)
        {
#if ENVIRO_PRESENT
            switch (month)
            {
                case 1:
                case 2:
                case 12:
                    EnviroSkyMgr.instance.ChangeSeason(EnviroSeasons.Seasons.Winter);
                    break;
                case 3:
                case 4:
                case 5:
                    EnviroSkyMgr.instance.ChangeSeason(EnviroSeasons.Seasons.Spring);
                    break;
                case 6:
                case 7:
                case 8:
                    EnviroSkyMgr.instance.ChangeSeason(EnviroSeasons.Seasons.Summer);
                    break;
                case 9:
                case 10:
                case 11:
                    EnviroSkyMgr.instance.ChangeSeason(EnviroSeasons.Seasons.Autumn);
                    break;
            }
#endif
        }
        #endregion

        #region Weather Methods
        /// <summary>
        /// Set weather over id with smooth transtion.
        /// </summary>
        /// <param name="id">An int value that represents the weather profile Id.</param>
        private void SetWeatherID(int id)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.ChangeWeather(id);
#endif
        }

        /// <summary>
        /// Set the current temperature value.
        /// </summary>
        /// <param name="temperature">A float value that represents the temperature in °C.</param>
        private void SetTemperature(float temperature)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.Weather.currentTemperature = temperature;
#endif
        }

        /// <summary>
        /// Set the current humidity value.
        /// </summary>
        /// <param name="humidity">A float value that represents the humidity in percent.</param>
        private void SetHumidity(float humidity)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.Weather.curWetness = Mathf.Clamp((humidity / kHundredValue), kZeroValue, kOneValue);
#endif
        }

        /// <summary>
        /// Set the precipitation value.
        /// </summary>
        /// <param name="precipitation">A float value that represents the precipitation in mm.</param>
        private void SetPrecipitation(float precipitation)
        {
#if ENVIRO_PRESENT
            //From weather services, the value for precipitation comes 0. 
            //Thus, it is no longer applied because it is not correct.
            //This function is kept for a later update.
            //EnviroSkyMgr.instance.Weather.curSnowStrength = precipitation;
#endif
        }

        /// <summary>
        /// Set the current weather state.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the received weather data.</param>
        private void SetWeatherState(WeatherData weatherData)
        {
#if ENVIRO_PRESENT

            switch (weatherData.WeatherState)
            {
                case WeatherState.Clear:
                    SetSunShafts(false);
                    SetWeatherID(kStateClearId);
                    break;
                case WeatherState.PartlyClear:
                    SetSunShafts(false);
                    SetWeatherID(kStatePartlyClearId);
                    break;
                case WeatherState.Sunny:
                    SetSunShafts(true);
                    SetWeatherID(kStateSunnyId);
                    break;
                case WeatherState.PartlySunny:
                    SetSunShafts(true);
                    SetWeatherID(kStatePartlySunnyId);
                    break;
                case WeatherState.Cloudy:
                    SetSunShafts(false);
                    SetWeatherID(kStateCloudyId);
                    break;
                case WeatherState.PartlyCloudy:
                    SetSunShafts(false);
                    SetWeatherID(kStatePartlyCloudyId);
                    break;
                case WeatherState.Thunderstorms:
                    SetSunShafts(false);
                    SetWeatherID(kStateThunderstormsId);
                    break;
                case WeatherState.RainPrecipitation:
                    SetSunShafts(false);
                    if (weatherData.Humidity < kPrecipitationMean)
                    {
                        SetWeatherID(kStateRainPrecipitationId);
                    }
                    else
                    {
                        SetWeatherID(kStateHeavyPrecipitationId);
                    }
                    break;
                case WeatherState.RainSnowPrecipitation:
                    SetSunShafts(false);
                    SetWeatherID(kStateRainSnowPrecipitationId);
                    break;
                case WeatherState.SnowPrecipitation:
                    SetSunShafts(false);
                    if (weatherData.Humidity < kPrecipitationMean)
                    {
                        SetWeatherID(kStateLightSnowPrecipitationId);
                    }
                    else
                    {
                        SetWeatherID(kStateHeavySnowPrecipitationId);
                    }
                    break;
                case WeatherState.Fair:
                    SetSunShafts(true);
                    SetWeatherID(kStateFairId);
                    break;
                case WeatherState.Mist:
                    SetSunShafts(false);
                    SetWeatherID(kStateMistId);
                    break;
                case WeatherState.Windy:
                    SetSunShafts(false);
                    SetWeatherID(kStateWindyId);
                    break;
                default:
                    SetSunShafts(true);
                    SetWeatherID(kStateClearId);
                    break;
            }
#endif
        }
        #endregion

        #region Wind Methods
        /// <summary>
        /// Enable/Disable wind zone direction.
        /// </summary>
        /// <param name="enable">A bool value that represents the enabled state.</param>
        public void SetUseWindZoneDirection(bool enable)
        {
#if ENVIRO_PRESENT && (ENVIRO_HD || ENVIRO_LW)
            EnviroSkyMgr.instance.CloudSettings.useWindZoneDirection = enable;
#endif
        }

        /// <summary>
        /// Set the wind speed and direction.
        /// </summary>
        /// <param name="wind">A Wind class instance that represents wind data.</param>
        private void SetWind(Wind wind)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.CloudSettings.cloudsWindIntensity = Mathf.Clamp((wind.Speed / kThousand), kZeroValue, kOneValue);
            EnviroSkyMgr.instance.CloudSettings.cloudsWindDirectionX = wind.Direction.x;
            EnviroSkyMgr.instance.CloudSettings.cloudsWindDirectionY = wind.Direction.y;
#endif
        }
        #endregion

        #region Fog Methods
        /// <summary>
        /// Enable/Disable fog.
        /// </summary>
        /// <param name="enable">A bool value that represents the enabled state.</param>
        private void SetFog(bool enable)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.useFog = enable;
            EnviroSkyMgr.instance.UpdateFogIntensity = enable;
#endif
        }

        /// <summary>
        /// Set the visibility value.
        /// </summary>
        /// <param name="visibility">A float value that represents the visibility in km.</param>
        private void SetVisibility(float visibility)
        {
#if ENVIRO_PRESENT

            if (EnviroSkyMgr.instance.Weather.currentActiveWeatherPreset)
            {
                EnviroSkyMgr.instance.Weather.currentActiveWeatherPreset.fogDensity = kOneValue / (visibility * kThousand);
            }
#endif
        }
        #endregion

        #region Clouds Methods
        /// <summary>
        /// Enable/Disable volumn clouds.
        /// </summary>
        /// <param name="enable">A bool value that represents the enabled state.</param>
        private void SetVolumeClouds(bool enable)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.useVolumeClouds = enable;
#endif
        }

        /// <summary>
        /// Enable/Disable flat clouds.
        /// </summary>
        /// <param name="enable">A bool value that represents the enabled state.</param>
        private void SetFlatClouds(bool enable)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.useFlatClouds = enable;
#endif
        }

        /// <summary>
        /// Enable/Disable particle clouds
        /// </summary>
        /// <param name="enable">A bool value that represents the enabled state.</param>
        private void SetParticleClouds(bool enable)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.useParticleClouds = enable;
#endif
        }

        /// <summary>
        /// Enable/Disable Aurora effect
        /// </summary>
        /// <param name="enable">A bool value that represents the enabled state.</param>
        private void SetAurora(bool enable)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.useAurora = enable;
#endif
        }
        #endregion

        #region Lights Methods
        /// <summary>
        /// Enable/Disable Volumn Lighting.
        /// </summary>
        /// <param name="enable">A bool value that represents the enabled state.</param>
        private void SetVolumeLighting(bool enable)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.useVolumeLighting = enable;
#endif
        }

        /// <summary>
        /// Enable/Disable sun light shafts clouds.
        /// </summary>
        /// <param name="enable">A bool value that represents the enabled state.</param>
        private void SetSunShafts(bool enable)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.useSunShafts = enable;
#endif
        }

        /// <summary>
        /// Enable/Disable moon light shafts clouds.
        /// </summary>
        /// <param name="enable">A bool value that represents the enabled state.</param>
        private void SetMoonShafts(bool enable)
        {
#if ENVIRO_PRESENT
            EnviroSkyMgr.instance.useMoonShafts = enable;
#endif
        }

        /// <summary>
        /// Set the UV index value.
        /// </summary>
        /// <param name="indexUV">A float value that represents the UV index.</param>
        private void SetIndexUV(float indexUV)
        {
#if ENVIRO_PRESENT
            //This line is commented because Enviro always calculates the light intensity and the value cannot be directly and realistically overwritten.
            //This function is kept for a later update.
            //EnviroSkyMgr.instance.enviroHDInstance.MainLight.intensity = indexUV;
#endif
        }
        #endregion

        #endregion
    }
}