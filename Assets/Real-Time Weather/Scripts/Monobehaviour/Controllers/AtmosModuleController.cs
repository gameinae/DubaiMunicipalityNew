//
// Copyright(c) 2020 Real-Time Weather ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at hello@assist.ro
//

#if UNITY_EDITOR
using UnityEditor;
#endif

using RealTimeWeather.Classes;
using RealTimeWeather.Managers;
using UnityEngine;
using System;
using RealTimeWeather.Enums;

#if ATMOS_PRESENT
using Mewlist.MassiveClouds;
#endif

namespace RealTimeWeather.Simulation.Atmos
{
    /// <summary>
    /// Class used to simulate weather using Massive Clouds Atmos plug-in.
    /// </summary>
    public class AtmosModuleController : MonoBehaviour
    {
        #region Private Const Variables
        private const string kAtmosPadHighStr = "AtmosPadHigh";
        private const string kAtmosPadMiddleStr = "AtmosPadMiddle";
        private const string kSunStr = "Sun";

        private const float kStateClearId = 1f;
        private const float kStatePartlyClearId = 0.8f;
        private const float kStateSunnyId = 0.5f;
        private const float kStatePartlySunnyId = 1f;
        private const float kStateCloudyId = 0.2f;
        private const float kStatePartlyCloudyId = 0.4f;
        private const float kStateThunderstormsId = 0.2f;
        private const float kStateRainPrecipitationId = 0.2f;
        private const float kStateRainSnowPrecipitationId = 0.2f;
        private const float kStateLightSnowPrecipitationId = 0.2f;
        private const float kStateFairId = 0.5f;
        private const float kStateMistId = 0.6f;
        private const float kStateWindyId = 0.4f;
        private const float kSunXrotation = 21.691f;
        private const float kSunYrotation = 64.48f;
        private const float kSunZrotation = -144.611f;

        private const int kSunIntensity = 2;
        private const int kCameraMoveVelocity = 500;
        #endregion

        #region Private Variables
#if ATMOS_PRESENT
        [SerializeField] private AtmosPad _atmosPad;
        [SerializeField] private Light _sun;
        [SerializeField] private Vector3 _sunPosition = new Vector3(0, 3, -100);
        [SerializeField] private Color _sunColor = new Color(181f, 104f, 29f);
        [SerializeField] private MassiveCloudsCameraEffect _cameraEffect;
#endif
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (RealTimeWeatherManager.instance)
            {
                RealTimeWeatherManager.instance.OnCurrentWeatherUpdate += OnWeatherUpdate;
            }
        }

        void Update()
        {

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
        /// This function creates the "AtmosPad" instance and adds it as a child to the "RealTimeWeatherManager" manager.
        /// </summary>
        public void CreateAtmosManagerInstance()
        {
#if ATMOS_PRESENT 
            _atmosPad = FindObjectOfType<AtmosPad>();

            if (_atmosPad == null)
            {
                GameObject atmosPadPrefab = null;

#if UNITY_STANDALONE
                atmosPadPrefab = RealTimeWeatherManager.GetPrefab(kAtmosPadHighStr);
#endif

#if UNITY_ANDROID || UNITY_IOS
                atmosPadPrefab = RealTimeWeatherManager.GetPrefab(kAtmosPadMiddleStr);
#endif
                if (atmosPadPrefab)
                {
                    _atmosPad = Instantiate(atmosPadPrefab).GetComponent<AtmosPad>();
                    _atmosPad.transform.SetParent(transform);
#if UNITY_EDITOR
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
#endif
                }
            }
#endif
        }

        /// <summary>
        /// Initializes Massive Clouds Atmos components.
        /// </summary>
        public void InitializeAtmos()
        {
#if ATMOS_PRESENT
            if (_atmosPad == null)
            {
                return;
            }

            SetSun();
            SetCamera();
#endif
        }

        /// <summary>
        /// This function destroys the Atmos components.
        /// </summary>
        public void DestroyAtmosComponents()
        {
#if ATMOS_PRESENT && UNITY_EDITOR
            DestroyImmediate(_cameraEffect);
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
#if ATMOS_PRESENT
            if (weatherData != null)
            {
                SetHour(weatherData.DateTime);
                SetWeatherState(weatherData);
            }
#endif
        }
        #endregion

        #region Time Methods
        /// <summary>
        /// Set the hour.
        /// </summary>
        /// <param name="dateTime">An DateTime value that represents the date.</param>
        public void SetHour(DateTime dateTime)
        {
#if ATMOS_PRESENT
            float hour = ((float)dateTime.Hour + ((float)dateTime.Minute * 0.01f));
            _atmosPad.SetHour(hour);
#endif
        }
        #endregion

        #region Weather Methods
        /// <summary>
        /// Set the current weather state.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the received weather data.</param>
        private void SetWeatherState(WeatherData weatherData)
        {
#if ATMOS_PRESENT

            switch (weatherData.WeatherState)
            {
                case WeatherState.Clear:
                    _atmosPad.SetVariation(kStateClearId);
                    break;
                case WeatherState.PartlyClear:
                    _atmosPad.SetVariation(kStatePartlyClearId);
                    break;
                case WeatherState.Sunny:
                    _atmosPad.SetVariation(kStateSunnyId);
                    break;
                case WeatherState.PartlySunny:
                    _atmosPad.SetVariation(kStatePartlySunnyId);
                    break;
                case WeatherState.Cloudy:
                    _atmosPad.SetVariation(kStateCloudyId);
                    break;
                case WeatherState.PartlyCloudy:
                    _atmosPad.SetVariation(kStatePartlyCloudyId);
                    break;
                case WeatherState.Thunderstorms:
                    _atmosPad.SetVariation(kStateThunderstormsId);
                    break;
                case WeatherState.RainPrecipitation:
                    _atmosPad.SetVariation(kStateRainPrecipitationId);
                    break;
                case WeatherState.RainSnowPrecipitation:
                    _atmosPad.SetVariation(kStateRainSnowPrecipitationId);
                    break;
                case WeatherState.SnowPrecipitation:
                    _atmosPad.SetVariation(kStateLightSnowPrecipitationId);
                    break;
                case WeatherState.Fair:
                    _atmosPad.SetVariation(kStateFairId);
                    break;
                case WeatherState.Mist:
                    _atmosPad.SetVariation(kStateMistId);
                    break;
                case WeatherState.Windy:
                    _atmosPad.SetVariation(kStateWindyId);
                    break;
                default:
                    break;
            }
#endif
        }
        #endregion

        #region Setup Methods
        /// <summary>
        /// This function sets the sun. If the Light component is not found in the scene, a new one is instantiated.
        /// </summary>
        private void SetSun()
        {
#if ATMOS_PRESENT
            _sun = FindObjectOfType<Light>();

            if (_sun == null)
            {
                GameObject sunObject = new GameObject(kSunStr);
                sunObject.transform.SetParent(transform);
                sunObject.transform.position = _sunPosition;
                sunObject.transform.rotation = Quaternion.Euler(kSunXrotation, kSunYrotation, kSunZrotation);
                _sun = sunObject.AddComponent<Light>();
                _sun.type = LightType.Directional;
                _sun.color = _sunColor;
#if UNITY_EDITOR
                _sun.lightmapBakeType = LightmapBakeType.Realtime;
#endif
                _sun.intensity = kSunIntensity;
                _sun.shadows = LightShadows.Soft;
#if UNITY_EDITOR
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
#endif
            }
#endif
        }

        /// <summary>
        /// This function adds the MassiveCloudsCameraEffect component to the camera.
        /// </summary>
        private void SetCamera()
        {
#if ATMOS_PRESENT
            Camera camera = FindObjectOfType<Camera>();

            if (camera != null)
            {
                _cameraEffect = camera.gameObject.AddComponent<MassiveCloudsCameraEffect>();
            }
#endif
        }
        #endregion

        #endregion
    }
}
