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
using System.Collections;
using UnityEngine;

using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using RealTimeWeather.AlertSystem;
using RealTimeWeather.Classes;
using RealTimeWeather.Tomorrow;
using RealTimeWeather.Tomorrow.Classes;
using RealTimeWeather.Geocoding;
using RealTimeWeather.Enums;
using RealTimeWeather.Simulation;
using RealTimeWeather.Simulation.Atmos;
using RealTimeWeather.Simulation.Enviro;
using RealTimeWeather.WeatherAtlas;
using RealTimeWeather.WeatherForYou;
using RealTimeWeather.WeatherUnderground;
using RealTimeWeather.OpenWeatherMap;
using RealTimeWeather.OpenWeatherMap.Classes;
using RealTimeWeather.Geocoding.Classes;

namespace RealTimeWeather.Managers
{
    /// <summary>
    /// Class that represents the main Real-Time Weather manager, this allows the weather data request from WeatherAtlasModule, WeatherUndergroundModule, WeatherForYouModule
    /// also it manages the automatic weather data update and whether simulation using third-party support components: Enviro or Tenkoku.
    /// </summary>
    public class RealTimeWeatherManager : MonoBehaviour
    {
        #region Enums
        /// <summary>
        /// <para>
        /// RequestMode is an enumeration which specifies what service will be requested to get weather data.
        /// </para>
        /// <para>
        /// RequestMode enum values are:
        /// <br>0: RTW mod</br>
        /// <br>1: Tomorrow mode</br>
        /// <br>2: OpenWeatherMap mode</br>
        /// </para>
        /// </summary>
        public enum RequestMode
        {
            [Description("1: RTW mode")] RtwMode,
            [Description("2: Tomorrow mode")] TomorrowMode,
            [Description("3: OpenWeatherMap mode")] OpenWeatherMapMode
        }
        #endregion

        #region Real-Time Weather Manager Instance
        private static RealTimeWeatherManager _instance;
        public static RealTimeWeatherManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<RealTimeWeatherManager>();
                }

                return _instance;
            }
        }
        #endregion

        #region Public Delegates
        public delegate void CurrentWeatherUpdate(WeatherData weatherData);
        public delegate void HourlyWeatherUpdate(List<WeatherData> weatherData);
        public delegate void DailyWeatherUpdate(List<WeatherData> weatherData);
        public delegate void RequestWeatherFromService(string city, string country);
        public delegate void RequestWeatherFromTomorrowService();

        public event CurrentWeatherUpdate OnCurrentWeatherUpdate;
        public event HourlyWeatherUpdate OnHourlyWeatherUpdate;
        public event DailyWeatherUpdate OnDailyWeatherUpdate;
        public event RequestWeatherFromService RequestAtlasWeather;
        public event RequestWeatherFromService RequestUndergroundWeather;
        public event RequestWeatherFromService RequestWeatherForYouWeather;
        public event RequestWeatherFromTomorrowService RequestWeatherFromTomorrow;
        #endregion

        #region Private Const Variables
        private const string kUpdateWeatherDataCoroutine = "UpdateWeatherData";
        private const string kMenuItemPath = "Real-time Weather/Real-Time Weather Manager";
        private const string kManagerObjectName = "RealTimeWeatherManager";
        private const string kPrefabExtension = ".prefab";
        // Atlas module constants
        private const string kAtlasServiceException = "Atlas service exception: ";
        private const string kAtlasObjectName = "WeatherAtlasModule";
        // Underground module constants
        private const string kUndergroundServiceException = "Underground service exception:";
        private const string kUndergroundObjectName = "WeatherUndergroundModule";
        // WeatherForYou module constants
        private const string kWeatherForYouServiceException = "Weather For You service exception:";
        private const string kWeatherForYouObjectName = "WeatherForYouModule";
        //OpenWeatherMap module constants
        private const string kOpenWeatherMapServiceException = "Open Weather Map service exception:";
        private const string kOpenWeatherMapObjectName = "OpenWeatherMapModule";
        // Enviro module constants
        private const string kEnviroModulePrefabName = "EnviroModulePrefab";
        private const string kEnviroModuleObjName = "EnviroModule";
        // Tenkoku module constants
        private const string kTenkokuModulePrefabName = "TenkokuModulePrefab";
        private const string kTenkokuModuleObjectName = "TenkokuModule";
        // Atmos module constants
        private const string kAtmosModulePrefabName = "AtmosModulePrefab";
        private const string kAtmosModuleObjName = "AtmosModule";
        // Tomorrow module constants
        private const string kTomorrowModuleObjectName = "TomorrowModule";
        // Alert module constants
        private const string kAlertModulePrefabName = "AlertModule_Prefab";
        private const string kAlertModuleObjectName = "Alert Module";
        // Geocoding
        private const string kIncorrectGeocodingData = "Latitude and longitude incorrect: ";
        private const string kUSStr = "US";
        private const string kCommaStr = ",";
        // UI
        private const string kWeatherUIModulePrefabName = "WeatherUI_Prefab";
        private const string kWeatherUIModuleObjectName = "Weather UI";
        #endregion

        #region Private Variables
        private WaitForSeconds _weatherUpdateWait;
        private WeatherAtlasModule _atlasModule;
        private WeatherUndergroundModule _undergroundModule;
        private WeatherForYouModule _weatherForYouModule;
        private TomorrowModule _tomorrowModule;
        private OpenWeatherMapModule _openWeatherMapModule;
        private AlertSystemModule _alertSystemModule;
        private ReverseGeocoding _reverseGeocoding;
        private EnviroModuleController _enviroModuleController;
        private TenkokuModuleController _tenkokuModule;
        private AtmosModuleController _atmosModuleController;
        private List<RequestWeatherFromService> _RTWWeatherProviders;
        private List<string> _RTWServiceException;

        [SerializeField] private string _requestedCity;
        [SerializeField] private string _requestedCountry;
        [SerializeField] private bool _requestByGeoCoordinates = false;
        [SerializeField] private bool _isCoroutineActive;
        [SerializeField] private bool _isAutoWeatherUpdateEnabled = true;
        [SerializeField] private bool _isWeatherSimulationEnabled;
        [SerializeField] private bool _isEnviroEnabled;
        [SerializeField] private bool _isTenkokuEnabled;
        [SerializeField] private bool _isAtmosEnabled;
        [SerializeField] private bool _isExpanseEnabled;
        [SerializeField] private bool _dontDestroy = false;
        [SerializeField] private float _autoUpdateRate = 1f;
        [SerializeField] private RequestMode _requestMode = RequestMode.RtwMode;
        [SerializeField] private RequestMode _lastChoosedRequestMode = RequestMode.RtwMode;
        [SerializeField] private List<string> _weatherProviders = new List<string> { "Weather Atlas", "Weather Underground", "Weather For You" };
        [SerializeField] private List<string> _weatherProvidersAddress = new List<string> { "https://www.weather-atlas.com", "https://www.wunderground.com", "https://www.weatherforyou.com" };
        [SerializeField] private List<int> _RTWWeatherProvidersIndexes = new List<int> { 0, 1, 2 };
        [SerializeField]
        [Tooltip("Is a float value that represets a geographic coordinate that specifies the north–south position of a point on the Earth's surface.\nLatitude must be set according to ISO 6709.")]
        [Range(-90.0f, 90.0f)]
        private float _latitude;
        [SerializeField]
        [Tooltip("Is a float value that represents a geographic coordinate that specifies the east-west position of a point on the Earth's surface.\nLongitude must be set according to ISO 6709.")]
        [Range(-180.0f, 180.0f)]
        private float _longitude;
        [Range(0, 2)]
        private int _indexRTWWeatherProviders = 0;
        #endregion

        #region Public Variables
        public bool _showGeneralSettings;
        public bool _showSimulationSettings;
        public bool _showLocalizationSettings;
        public bool _showAutoUpdateSettings;
        public bool _showWeatherSimulationSettings;
        #endregion

        #region Public Properties
        public string RequestedCity
        {
            get { return _requestedCity; }
            set { _requestedCity = value; }
        }

        public string RequestedCountry
        {
            get { return _requestedCountry; }
            set { _requestedCountry = value; }
        }

        public bool IsAutoWeatherEnabled
        {
            get { return _isAutoWeatherUpdateEnabled; }
            set
            {
                _isAutoWeatherUpdateEnabled = value;
                OnAutoWeatherStateChanged();
            }
        }

        public bool IsWeatherSimulationEnabled
        {
            get { return _isWeatherSimulationEnabled; }
            set { _isWeatherSimulationEnabled = value; }
        }

        public bool IsEnviroEnabled
        {
            get { return _isEnviroEnabled; }
            set { _isEnviroEnabled = value; }
        }

        public bool IsTenkokuEnabled
        {
            get { return _isTenkokuEnabled; }
            set { _isTenkokuEnabled = value; }
        }

        public bool IsAtmosEnabled
        {
            get { return _isAtmosEnabled; }
            set { _isAtmosEnabled = value; }
        }

        public float AutoWeatherUpdateRate
        {
            get { return _autoUpdateRate; }
            set { _autoUpdateRate = value; }
        }

        public RequestMode DataRequestMode
        {
            get { return _requestMode; }
            set { _requestMode = value; }
        }

        public List<string> DataWeatherProviders
        {
            get { return _weatherProviders; }
            set { _weatherProviders = value; }
        }

        public List<string> DataWeatherProvidersAddress
        {
            get { return _weatherProvidersAddress; }
            set { _weatherProvidersAddress = value; }
        }

        public List<int> RTWDataWeatherProvidersIndexes
        {
            get { return _RTWWeatherProvidersIndexes; }
            set { _RTWWeatherProvidersIndexes = value; }
        }

        public RequestMode LastChoosedRequestMode
        {
            get { return _lastChoosedRequestMode; }
            set { _lastChoosedRequestMode = value; }
        }

        public float Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

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
            _enviroModuleController = transform.GetComponentInChildren<EnviroModuleController>();
            _tenkokuModule = transform.GetComponentInChildren<TenkokuModuleController>();
            _atmosModuleController = transform.GetComponentInChildren<AtmosModuleController>();
            _tomorrowModule = transform.GetComponentInChildren<TomorrowModule>();
            _alertSystemModule = GetComponentInChildren<AlertSystemModule>();
            _atlasModule = transform.GetComponentInChildren<WeatherAtlasModule>();
            _undergroundModule = transform.GetComponentInChildren<WeatherUndergroundModule>();
            _weatherForYouModule = transform.GetComponentInChildren<WeatherForYouModule>();
            _openWeatherMapModule = transform.GetComponentInChildren<OpenWeatherMapModule>();

            RequestAtlasWeather += _atlasModule.StartWeatherServiceParser;
            RequestUndergroundWeather += _undergroundModule.StartWeatherServiceParser;
            RequestWeatherForYouWeather += _weatherForYouModule.StartWeatherServiceParser;
            RequestWeatherFromTomorrow += _tomorrowModule.RequestTomorrowData;

            _atlasModule.onWebPageParsed += OnReceivingAtlasWeatherData;
            _atlasModule.onExceptionRaised += OnRequestWeatherServiceExceptionRaised;
            _undergroundModule.onWebPageParsed += OnReceivingUndergroundWeatherData;
            _undergroundModule.onExceptionRaised += OnRequestWeatherServiceExceptionRaised;
            _weatherForYouModule.onWebPageParsed += OnReceivingWeatherForYouWeatherData;
            _weatherForYouModule.onExceptionRaised += OnRequestWeatherServiceExceptionRaised;
            _tomorrowModule.onTomorrowDataSent += OnReceivingTomorrowWeatherData;
            _tomorrowModule.onTomorrowExceptionRaised += OnTomorrowServiceExceptionRaised;
            _openWeatherMapModule.onServerResponse += OnReceivingOpenWeatherMapData;
            _openWeatherMapModule.onServerOneCallAPIResponse += OnReceivingOpenWeatherMapOneCallAPIData;
            _openWeatherMapModule.onExceptionRaised += OnOpenWeatherMapServiceExceptionRaised;
            _RTWWeatherProviders = new List<RequestWeatherFromService> { RequestWeatherFromAtlasService, RequestWeatherFromUndergroundService, RequestWeatherFromWeatherForYouService, };
            _RTWServiceException = new List<string> { kAtlasServiceException, kUndergroundServiceException, kWeatherForYouServiceException };
        }

        private void Start()
        {
            if (Application.isPlaying && _dontDestroy)
            {
                DontDestroyOnLoad(gameObject);
            }

            if (Application.isPlaying)
            {
                StartCoroutine(UpdateWeatherData());
            }
        }

        private void OnDestroy()
        {
            RequestAtlasWeather -= _atlasModule.StartWeatherServiceParser;
            RequestUndergroundWeather -= _undergroundModule.StartWeatherServiceParser;
            RequestWeatherForYouWeather -= _weatherForYouModule.StartWeatherServiceParser;
            RequestWeatherFromTomorrow -= _tomorrowModule.RequestTomorrowData;

            _atlasModule.onWebPageParsed -= OnReceivingAtlasWeatherData;
            _atlasModule.onExceptionRaised -= OnRequestWeatherServiceExceptionRaised;
            _undergroundModule.onWebPageParsed -= OnReceivingUndergroundWeatherData;
            _undergroundModule.onExceptionRaised -= OnRequestWeatherServiceExceptionRaised;
            _weatherForYouModule.onWebPageParsed -= OnReceivingWeatherForYouWeatherData;
            _weatherForYouModule.onExceptionRaised -= OnRequestWeatherServiceExceptionRaised;
            _tomorrowModule.onTomorrowDataSent -= OnReceivingTomorrowWeatherData;
            _tomorrowModule.onTomorrowExceptionRaised -= OnTomorrowServiceExceptionRaised;
            _openWeatherMapModule.onServerResponse -= OnReceivingOpenWeatherMapData;
            _openWeatherMapModule.onServerOneCallAPIResponse -= OnReceivingOpenWeatherMapOneCallAPIData;
            _openWeatherMapModule.onExceptionRaised -= OnOpenWeatherMapServiceExceptionRaised;
        }
        #endregion

        #region Public Methods

        #region General Settings Methods
#if UNITY_EDITOR
        /// <summary>
        /// This function adds the "Real-Time Weather Manager Instance" option in the "Assets / Create / ..." menu.
        /// When the option is activated, a new GameObject will be instantiated with the RealTimeWeatherManager component on it.
        /// </summary>
        [MenuItem(kMenuItemPath)]
        public static void CreateManagerInstance()
        {
            if (RealTimeWeatherManager.instance == null)
            {
                GameObject weatherManagerObj = new GameObject();
                weatherManagerObj.name = kManagerObjectName;
                weatherManagerObj.AddComponent<RealTimeWeatherManager>();

                GameObject atlasModuleObj = new GameObject();
                atlasModuleObj.name = kAtlasObjectName;
                atlasModuleObj.AddComponent<WeatherAtlasModule>();
                atlasModuleObj.transform.SetParent(weatherManagerObj.transform);

                GameObject undergroundModule = new GameObject();
                undergroundModule.name = kUndergroundObjectName;
                undergroundModule.AddComponent<WeatherUndergroundModule>();
                undergroundModule.transform.SetParent(weatherManagerObj.transform);

                GameObject weatherForYouModuleObj = new GameObject();
                weatherForYouModuleObj.name = kWeatherForYouObjectName;
                weatherForYouModuleObj.AddComponent<WeatherForYouModule>();
                weatherForYouModuleObj.transform.SetParent(weatherManagerObj.transform);

                GameObject tomorrowModuleObj = new GameObject();

                tomorrowModuleObj.name = kTomorrowModuleObjectName;
                tomorrowModuleObj.AddComponent<TomorrowModule>();
                tomorrowModuleObj.transform.SetParent(weatherManagerObj.transform);

                GameObject openWeatherMapModuleObj = new GameObject();
                openWeatherMapModuleObj.name = kOpenWeatherMapObjectName;
                openWeatherMapModuleObj.AddComponent<OpenWeatherMapModule>();
                openWeatherMapModuleObj.transform.SetParent(weatherManagerObj.transform);

                Undo.RegisterCreatedObjectUndo(weatherManagerObj, weatherManagerObj.name);
                Selection.activeObject = weatherManagerObj;
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

                GameObject alertModulePrefab = GetPrefab(kAlertModulePrefabName);

                if (alertModulePrefab != null)
                {
                    GameObject alertModuleInstance = Instantiate(alertModulePrefab, Vector3.zero, Quaternion.identity);
                    alertModuleInstance.transform.SetParent(weatherManagerObj.transform);
                    alertModuleInstance.name = kAlertModuleObjectName;
                    alertModuleInstance.SetActive(true);
                }

                GameObject weatherUIModulePrefab = GetPrefab(kWeatherUIModulePrefabName);

                if (weatherUIModulePrefab != null)
                {
                    GameObject alertModuleInstance = Instantiate(weatherUIModulePrefab, Vector3.zero, Quaternion.identity);
                    alertModuleInstance.name = kWeatherUIModuleObjectName;
                    alertModuleInstance.SetActive(true);
                }
            }
        }
#endif
        #endregion

        #region Simulation Settings Methods
        /// <summary>
        /// Handles the simulation activation event using Enviro plug-in. An instance of EnviroModulePrefab is created.
        /// </summary>
        public void ActivateEnviroSimulation()
        {
            GameObject enviroPrefab = GetPrefab(kEnviroModulePrefabName);

            if (enviroPrefab != null)
            {
                GameObject envirModuleInstance = Instantiate(enviroPrefab, Vector3.zero, Quaternion.identity);
                envirModuleInstance.name = kEnviroModuleObjName;
                envirModuleInstance.transform.SetParent(transform);
                envirModuleInstance.SetActive(true);
                _enviroModuleController = envirModuleInstance.GetComponent<EnviroModuleController>();
                _enviroModuleController.CreateEnviroManagerInstance();
                _enviroModuleController.InitializeEnviro();
            }
        }

        /// <summary>
        /// Handles the simulation activation event using Tenkoku plug-in. An instance of TenkokuModulePrefab is created.
        /// </summary>
        public void ActivateTenkokuSimulation()
        {
            GameObject tenkokuModulePrefab = GetPrefab(kTenkokuModulePrefabName);

            if (tenkokuModulePrefab != null)
            {
                GameObject tenkokuModuleInstance = Instantiate(tenkokuModulePrefab, Vector3.zero, Quaternion.identity);
                tenkokuModuleInstance.name = kTenkokuModuleObjectName;
                tenkokuModuleInstance.transform.SetParent(transform);
                tenkokuModuleInstance.SetActive(true);
                _tenkokuModule = tenkokuModuleInstance.GetComponent<TenkokuModuleController>();
                _tenkokuModule.CreateTenkokuManagerInstance();
            }
        }

        /// <summary>
        /// Handles the simulation activation event using Massive Clouds Atmos plug-in. An instance of AtmosModulePrefab is created.
        /// </summary>
        public void ActivateAtmosSimulation()
        {
            GameObject atmosPrefab = GetPrefab(kAtmosModulePrefabName);

            if (atmosPrefab != null)
            {
                GameObject atmosModuleInstance = Instantiate(atmosPrefab, Vector3.zero, Quaternion.identity);
                atmosModuleInstance.name = kAtmosModuleObjName;
                atmosModuleInstance.transform.SetParent(transform);
                atmosModuleInstance.SetActive(true);

                _atmosModuleController = atmosModuleInstance.GetComponent<AtmosModuleController>();
                _atmosModuleController.CreateAtmosManagerInstance();
                _atmosModuleController.InitializeAtmos();
            }
        }

        /// <summary>
        /// Handles the simulation activation event using Expanse plug-in. An instance of ExpanseModulePrefab is created.
        /// </summary>
        public void ActivateExpanseSimulation()
        {
            // The functionality will be impelemented in the next task.
            // Debug.Log() method is used for testing only.
            Debug.Log("Activate Expanse Simulation");
        }

        /// <summary>
        /// Handles the simulation disable event. The created instance of EnviroModulePrefab is destroyed.
        /// </summary>
        public void DeactivateEnviroSimulation()
        {
            _enviroModuleController = transform.GetComponentInChildren<EnviroModuleController>();

            if (_enviroModuleController != null)
            {
                DestroyImmediate(_enviroModuleController.gameObject);
            }
        }

        /// <summary>
        /// Handles the simulation disable event. The created instance of TenkokuModulePrefab is destroyed.
        /// </summary>
        public void DeactivateTenkokuSimulation()
        {
            _tenkokuModule = transform.GetComponentInChildren<TenkokuModuleController>();

            if (_tenkokuModule != null)
            {
                DestroyImmediate(_tenkokuModule.gameObject);
            }
        }

        /// <summary>
        /// Handles the simulation disable event. The created instance of AtmosModulePrefab is destroyed.
        /// </summary>
        public void DeactivateAtmosSimulation()
        {
            _atmosModuleController = transform.GetComponentInChildren<AtmosModuleController>();

            if (_atmosModuleController != null)
            {
                _atmosModuleController.DestroyAtmosComponents();
                DestroyImmediate(_atmosModuleController.gameObject);
            }
        }

        /// <summary>
        /// Handles the simulation disable event. The created instance of ExpanseModulePrefab is destroyed.
        /// </summary>
        public void DeactivateExpanseSimulation()
        {
            // The functionality will be impelemented in the next task.
            // Debug.Log() method is used for testing only.
            Debug.Log("Deactivate Expanse Simulation");
        }

        /// <summary>
        /// Finds and returns prefab specified by name.
        /// </summary>
        /// <param name="prefabName">A string value that represents the prefab name.</param>
        public static GameObject GetPrefab(string prefabName)
        {
#if UNITY_EDITOR
            string[] assets = AssetDatabase.FindAssets(prefabName);
            for (int idx = 0; idx < assets.Length; idx++)
            {
                string path = AssetDatabase.GUIDToAssetPath(assets[idx]);
                if (path.Contains(kPrefabExtension))
                {
                    return AssetDatabase.LoadAssetAtPath<GameObject>(path);
                }
            }
#endif
            return null;
        }
        #endregion

        #region Public Events Methods
        /// <summary>
        /// Request weather data using city and country names.
        /// </summary>
        /// <param name="city">A string value that represents the city for which the weather is requested.</param>
        /// <param name="country">A string value that represents the country.</param>
        public void RequestWeatherByCityAndCountry(string city, string country)
        {
            if (!city.Equals(string.Empty) && !country.Equals(string.Empty))
            {
                if (_RTWWeatherProviders.Count() > 0)
                {
                    _RTWWeatherProviders[_RTWWeatherProvidersIndexes[_indexRTWWeatherProviders]](city, country);
                }
            }
        }

        /// <summary>
        /// Requests weather data using geographical coordinates.
        /// </summary>
        /// <param name="latitude">Is a float value that represets a geographic coordinate that specifies the north–south position of a point on the Earth's surface.</param>
        /// <param name="longitude">Is a float value that represents a geographic coordinate that specifies the east-west position of a point on the Earth's surface.</param>
        public IEnumerator RequestWeatherByGeoCoordinates(float latitude, float longitude)
        {
            // Atlas, WeatherForYou, and Weather Underground services use city and country names to get weather data.
            // For this reason, reverse geocoding must be applied.
            // Reverse geocoding is a process that converts latitude and longitude to readable locality properties.
            CoroutineWithData reverseGeoCoroutine = new CoroutineWithData(this, _reverseGeocoding.RequestGeocodingInformation(latitude, longitude));
            yield return reverseGeoCoroutine.Coroutine;

            GeocodingData reverseGeoData = (GeocodingData)reverseGeoCoroutine.Result;

            if (reverseGeoData != null && reverseGeoData.City != string.Empty && reverseGeoData.CountryName != string.Empty)
            {
                _requestedCity = reverseGeoData.City;
                _requestedCountry = reverseGeoData.CountryName;

                if (reverseGeoData.CountryCode == kUSStr)
                {
                    _requestedCity = reverseGeoData.PrincipalSubdivision + kCommaStr + reverseGeoData.City;
                }

                RequestWeatherByCityAndCountry(_requestedCity, _requestedCountry);
            }
            else
            {
                LogFile.ClearText();
                LogFile.Write(kIncorrectGeocodingData + latitude + " lat , " + longitude + " long");
                _alertSystemModule.OpenView();
            }
        }

        /// <summary>
        /// Handles the automatic weather state changed event and triggers the Start/Stop for the UpdateWheatherData coroutine.
        /// </summary>
        public void OnAutoWeatherStateChanged()
        {
            if (_isAutoWeatherUpdateEnabled)
            {
                if (!_isCoroutineActive && Application.isPlaying)
                {
                    StartCoroutine(UpdateWeatherData());
                }
            }
            else if (_isCoroutineActive)
            {
                StopCoroutine(kUpdateWeatherDataCoroutine);
                _isCoroutineActive = false;
            }
        }
        #endregion

        #endregion

        #region Private Methods

        #region Private Events Methods
        /// <summary>
        /// Requests the weather data.
        /// </summary>
        private void RequestWeather()
        {
            LogFile.ClearText();
            _indexRTWWeatherProviders = 0;
            switch (_requestMode)
            {
                case RequestMode.RtwMode:
                    if (_requestByGeoCoordinates)
                    {
                        StartCoroutine(RequestWeatherByGeoCoordinates(_latitude, _longitude));
                    }
                    else
                    {
                        RequestWeatherByCityAndCountry(_requestedCity, _requestedCountry);
                    }
                    break;
                case RequestMode.TomorrowMode:
                    RequestWeatherDataFromTomorrowService();
                    break;
                case RequestMode.OpenWeatherMapMode:
                    _openWeatherMapModule.RequestOpenWeatherMapData();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Send notification with updated current weather data to the components that listen to the OnCurrentWeatherUpdate event.
        /// </summary>
        /// <param name="currentWeatherData">A WeatherData class instance that represents the current weather data received from services.</param>
        private void NotifyCurrentWeatherChanged(WeatherData currentWeatherData)
        {
            OnCurrentWeatherUpdate?.Invoke(currentWeatherData);
        }

        /// <summary>
        /// If hourly weather data is present, send notification with updated hourly weather data to the components that listen to the OnHourlyWeatherUpdate event.
        /// </summary>
        /// <param name="hourlyWeatherData">A WeatherData class instance list that represents the hourly weather data received from services.</param>
        private void NotifyHourlyWeatherChanged(List<WeatherData> hourlyWeatherData)
        {
            OnHourlyWeatherUpdate?.Invoke(hourlyWeatherData);
        }

        /// <summary>
        /// If daily weather data is present, send notification with updated daily weather data to the components that listen to the OnDailyWeatherUpdate event.
        /// </summary>
        /// <param name="dailyWeatherData">A WeatherData class instance list that represents the daily weather data received from services.</param>
        private void NotifyDailyWeatherChanged(List<WeatherData> dailyWeatherData)
        {
            OnDailyWeatherUpdate?.Invoke(dailyWeatherData);
        }

        /// <summary>
        /// Requests the weather data from the Atlas service.
        /// </summary>
        /// <param name="city">A string value that represents the city for which the weather is requested.</param>
        /// <param name="country">A string value that represents the country.</param>
        private void RequestWeatherFromAtlasService(string city, string country)
        {
            RequestAtlasWeather?.Invoke(city, country);
        }

        /// <summary>
        /// Requests the weather data from the Underground service.
        /// </summary>
        /// <param name="city">A string value that represents the city for which the weather is requested.</param>
        /// <param name="country">A string value that represents the country.</param>
        private void RequestWeatherFromUndergroundService(string city, string country)
        {
            RequestUndergroundWeather?.Invoke(city, country);
        }

        /// <summary>
        /// Requests the weather data from the Weather For You service.
        /// </summary>
        /// <param name="city">A string value that represents the city for which the weather is requested.</param>
        /// <param name="country">A string value that represents the country.</param>
        private void RequestWeatherFromWeatherForYouService(string city, string country)
        {
            RequestWeatherForYouWeather?.Invoke(city, country);
        }

        /// <summary>
        /// Requests the weather data from the Tomorrow service.
        /// </summary>
        private void RequestWeatherDataFromTomorrowService()
        {
            RequestWeatherFromTomorrow?.Invoke();
        }

        /// <summary>
        /// Handles the data receive event from Atlas service.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the weather data received from the Atlas service.</param>
        private void OnReceivingAtlasWeatherData(WeatherData weatherData)
        {
            if (weatherData != null)
            {
                NotifyCurrentWeatherChanged(weatherData);
            }
        }

        /// <summary>
        /// Handles the data receive event from Underground service.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the weather data received from the Underground service.</param>
        private void OnReceivingUndergroundWeatherData(WeatherData weatherData)
        {
            if (weatherData != null)
            {
                NotifyCurrentWeatherChanged(weatherData);
            }
        }

        /// <summary>
        /// Handles the data receive event from Weather For You service.
        /// </summary>
        /// <param name="weatherData">A WeatherData class instance that represents the weather data received from the Weather For You service.</param>
        private void OnReceivingWeatherForYouWeatherData(WeatherData weatherData)
        {
            if (weatherData != null)
            {
                NotifyCurrentWeatherChanged(weatherData);
            }
        }

        /// <summary>
        /// Handles the data receive event from Tomorrow service.
        /// </summary>
        /// <param name="tomorrowData">A TomorrowData class instance that represents the weather data received from the Tomorrow.</param>
        private void OnReceivingTomorrowWeatherData(TomorrowData tomorrowData)
        {
            if (tomorrowData != null)
            {
                TomorrowDataConverter tomorrowDataConverter = new TomorrowDataConverter(tomorrowData);
                NotifyCurrentWeatherChanged(tomorrowDataConverter.ConvertCurrentTomorrowDataToRtwData());
                List<WeatherData> hourlyRtwData = new List<WeatherData>();
                if (_tomorrowModule._hourlyForecast)
                {
                    hourlyRtwData = tomorrowDataConverter.ConvertHourlyTomorrowDataToRtwData();
                    hourlyRtwData.RemoveRange(_tomorrowModule._hourlyForecastLength + 1,
                        hourlyRtwData.Count - _tomorrowModule._hourlyForecastLength - 1);
                }
                NotifyHourlyWeatherChanged(hourlyRtwData);

                List<WeatherData> dailyRtwData = new List<WeatherData>();
                if (_tomorrowModule._dailyForecast)
                {
                    dailyRtwData = tomorrowDataConverter.ConvertDailyTomorrowDataToRtwData();
                    dailyRtwData.RemoveRange(_tomorrowModule._dailyForecastLength + 1,
                        dailyRtwData.Count - _tomorrowModule._dailyForecastLength - 1);
                }
                NotifyDailyWeatherChanged(dailyRtwData);
            }
        }

        /// <summary>
        /// Handles the data receive event from OpenWeatherMap service.
        /// </summary>
        /// <param name="weatherData">A OpenWeatherMapData class instance that represents the weather data received from the OpenWeatherMap service.</param>
        private void OnReceivingOpenWeatherMapData(OpenWeatherMapData weatherData)
        {
            if (weatherData != null)
            {
                OpenWeatherMapConverter converter = new OpenWeatherMapConverter(weatherData);
                NotifyCurrentWeatherChanged(converter.ConvertToRealTimeManagerWeatherData());
            }
        }
        /// <summary>
        /// Handles the data receive event from OpenWeatherMap service for OneCallAPI.
        /// </summary>
        /// <param name="weatherData">A OpenWeatherOneCallAPIMapData class instance that represents the weather data received from the Open Weather Map One Call API service.</param>
        private void OnReceivingOpenWeatherMapOneCallAPIData(OpenWeatherOneCallAPIMapData weatherData)
        {
            if (weatherData != null)
            {
                var converter = new OpenWeatherOneCallAPIMapConverter(weatherData);
                NotifyHourlyWeatherChanged(converter.ConvertHourlyWeatherDataToRealTimeManagerWeatherListData());
                NotifyDailyWeatherChanged(converter.ConvertDailyWeatherDataToRealTimeManagerWeatherListData());
            }
        }

        /// <summary>
        /// Handles the Request Weather service exception event.
        /// </summary>
        /// <param name="exception">An ExceptionType value that represents the exception type.</param>
        /// <param name="message">A string value that represents the exception message.</param>
        private void OnRequestWeatherServiceExceptionRaised(ExceptionType exception, string message)
        {
            LogFile.Write(_RTWServiceException[_RTWWeatherProvidersIndexes[_indexRTWWeatherProviders]] + message);
            _indexRTWWeatherProviders++;
            if (_indexRTWWeatherProviders < _RTWWeatherProviders.Count())
            {
                _RTWWeatherProviders[_RTWWeatherProvidersIndexes[_indexRTWWeatherProviders]](_requestedCity, _requestedCountry);
            }
            else
            {
                _alertSystemModule.OpenView();
            }
        }

        /// <summary>
        /// Handles the Tomorrow service exception event.
        /// </summary>
        /// <param name="errorMessage">A string value that represents the exception message.</param>
        private void OnTomorrowServiceExceptionRaised(string errorMessage)
        {
            LogFile.Write(errorMessage);
            _alertSystemModule.OpenView();
        }

        /// <summary>
        /// Handles the OpenWeatherMap service exception event.
        /// </summary>
        /// <param name="exception">An ExceptionType value that represents the exception type.</param>
        /// <param name="message">A string value that represents the exception message.</param>
        private void OnOpenWeatherMapServiceExceptionRaised(ExceptionType exception, string message)
        {
            LogFile.Write(kOpenWeatherMapServiceException + exception.ToString() + " => " + message);
            _alertSystemModule.OpenView();
        }
        #endregion

        #region Auto Weather Data Update Methods
        /// <summary>
        /// This is the main functionality for the automatic weather data update co-routine.
        /// It requests the weather data from the services with the established frequency.
        /// </summary>
        private IEnumerator UpdateWeatherData()
        {
            _isCoroutineActive = true;

            if (_isAutoWeatherUpdateEnabled)
            {
                while (_isAutoWeatherUpdateEnabled)
                {
                    RequestWeather();
                    _weatherUpdateWait = new WaitForSeconds(Utils.ConvertMinutesToSeconds(_autoUpdateRate));
                    yield return _weatherUpdateWait;
                }
            }
            else
            {
                RequestWeather();
            }

            _isCoroutineActive = false;
            yield return null;
        }
        #endregion

        #endregion
    }
}