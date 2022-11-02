//
// Copyright(c) 2020 Real-Time Weather ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at hello@assist.ro
//

using RealTimeWeather.Managers;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static RealTimeWeather.Managers.RealTimeWeatherManager;

namespace RealTimeWeather.Editors
{
    /// <summary>
    /// This class create a custom editor for RealTimeWeatherManager component.
    /// </summary>
    [CustomEditor(typeof(RealTimeWeatherManager))]
    public class RealTimeWeatherEditor : Editor
    {
        #region Private Const Variables
        // Title constants
        private const string kTitleStr = "Real-Time Weather Manager";
        // Info
        private const string kInfoVersionStr = "Version";
        private const string kInfoCurrentVersion = "1.5.0";
        private const string kHelp = "HELP";
        private const string kHelpUrl = "https://assist-software.net/contact-us";
        private const string kAssist = "ASSIST";
        private const string kSeparator = "|";
        private const string kAssistUrl = "https://assist-software.net/";
        private const string kWelcomeMessageStr = "Welcome to the Real-Time Weather!";
        private const string kInfoMessageStr = "Real-Time Weather is an efficient tool to obtain real-time weather data. Use Real-Time Weather Manager to get weather data or to simulate weather by adding one of the third-party support components: Enviro, Tenkoku or Massive Clouds Atmos.";
        // General settings constants
        private const string kGeneralSettingsStr = "General Settings";
        private const string kDontDestroyStr = "Don't Destroy On Load";
        private const string kRequestModeStr = "Request mode";
        // Simulation settings constants
        private const string kSimulationSettings = "Simulation Settings";
        private const string kWeatherSimulationStr = "Weather Simulation";
        private const string kInfoSimulationStr = "To simulate the weather, you must activate one of the third party support components: Enviro, Tenkoku or Massive Clouds Atmos.";
        // Localization constants
        private const string kLocalizationStr = "Localization";
        private const string kInfoLocalizationStr = "Weather data can be obtained by specifying the city and the country or geographical coordinates.";
        private const string kCityStr = "City";
        private const string kCountryStr = "Country";
        private const string kRequestByGeoCoordinatesStr = "Request weather by geographic coordinates";
        // Automatic weather update constants
        private const string kAutoWeatherUpdateStr = "Enable auto weather update";
        private const string kAutoUpdateRateStr = "Update frequency";
        private const string kAutoWeatherStr = "Weather Update";
        private const string kInfoAutoUpdateStr = "Weather data will be updated with the set frequency (in minutes).";
        // Weather simulation constants
        private const string kWarningStr = "One simulation plugin can be active at a time.";
        // Enviro constants
        private const string kEnviroStr = "Enviro";
        private const string kEnviroNotFoundStr = "Enviro not found in your project!";
        private const string kEnviroDeactivatedStr = "Enviro simulation is deactivated!";
        private const string kEnviroDeactivateBtnStr = "Deactivate Enviro Simulation";
        private const string kEnviroActivateBtnStr = "Activate Enviro Simulation";
        private const string kEnviroActivatedStr = "Enviro simulation is activated!";
        // Tenkoku constants
        private const string kTenkokuStr = "Tenkoku";
        private const string kTenkokuNotFoundStr = "Tenkoku not found in your project!";
        private const string kTenkokuDeactivatedStr = "Tenkoku simulation is deactivated!";
        private const string kTenkokuDeactivateBtnStr = "Deactivate Tenkoku Simulation";
        private const string kTenkokuActivateBtnStr = "Activate Tenkoku Simulation";
        private const string kTenkokuActivatedStr = "Tenkoku simulation is activated!";
        // Massive Clouds Atmos constants
        private const string kAtmosStr = "Atmos";
        private const string kAtmosNotFoundStr = "Atmos not found in your project!";
        private const string kAtmosDeactivatedStr = "Atmos simulation is deactivated!";
        private const string kAtmosDeactivateBtnStr = "Deactivate Atmos Simulation";
        private const string kAtmosActivateBtnStr = "Activate Atmos Simulation";
        private const string kAtmosActivatedStr = "Atmos simulation is activated!";
        // Expanse constants
        private const string kExpanseStr = "Expanse";
        private const string kExpanseNotFoundStr = "Expanse not found in your project!";
        private const string kExpanseDeactivatedStr = "Expanse simulation is deactivated!";
        private const string kExpanseDeactivateBtnStr = "Deactivate Expanse Simulation";
        private const string kExpanseActivateBtnStr = "Activate Expanse Simulation";
        private const string kExpanseActivatedStr = "Expanse simulation is activated!";
        // Properties constants
        private const string kRequestedCityPropStr = "_requestedCity";
        private const string kRequestedCountryPropStr = "_requestedCountry";
        private const string kAutoUpdateRatePropStr = "_autoUpdateRate";
        private const string kIsAutoWeatherUpdateEnabledPropStr = "_isAutoWeatherUpdateEnabled";
        private const string kIsWeatherSimulationEnabledPropStr = "_isWeatherSimulationEnabled";
        private const string kDontDestroyPropStr = "_dontDestroy";
        private const string kEnviroEnabledPropStr = "_isEnviroEnabled";
        private const string kTenkokuEnabledPropStr = "_isTenkokuEnabled";
        private const string kAtmosEnabledPropStr = "_isAtmosEnabled";
        private const string kExpanseEnabledPropStr = "_isExpanseEnabled";
        private const string kRealtimeModePropStr = "_isRealtimeWeatherMode";
        private const string kLatitudePropStr = "_latitude";
        private const string kLongitudePropStr = "_longitude";
        private const string kRequestByGeoCoordinatesPropStr = "_requestByGeoCoordinates";
        //Module tag constants
        private const string kTomorrowModuleObjectName = "TomorrowModule";
        private const string kOpenWeatherMapObjectName = "OpenWeatherMapModule";

        private const int kMarginSpace = 5;
        private const int kElementsSpace = 5;
        private const int kHeaderSpace = 20;
        private const int kLogoBottomSpace = 60;
        private const int kInfoBottomSpace = 40;
        private const int kInfoMargin = 15;
        private const int kInfoSpace = 110;

        private const float kMinFrequencyValue = 1f;
        private const float kMaxFrequencyValue = 120.0f;
        #endregion

        #region Private Variables
        private RealTimeWeatherManager _scriptTarget;

        private GUIStyle _headerStyle;
        private GUIStyle _outerBoxStyle;
        private GUIStyle _innerBoxStyle;
        private GUIStyle _labelInfoStyle;
        private GUIStyle _labelWarningStyle;

        private Color _labelTextColor;
        private Color _backgroundColor1;
        private Color _backgroundColor2;

        private Rect _logoRect;
        private Rect _infoRect;

        private Texture2D _logoTexture;

        private string[] _dataRequestMode = new[] { "Real-Time Weather mode", "Tomorrow.io mode", "OpenWeatherMap mode" };

        private ReorderableList list;
        private int currentIndex;
        #endregion

        #region Private Properties
        private SerializedProperty AutoWeatherDataUpdateProperty;
        private SerializedProperty RequestedCityProperty;
        private SerializedProperty RequestedCountryProperty;
        private SerializedProperty AutoUpdateRateProperty;
        private SerializedProperty IsWeatherSimulationEnabledProperty;
        private SerializedProperty DontDestroyProperty;
        private SerializedProperty IsEnviroEnabledProperty;
        private SerializedProperty IsTenkokuEnabledProperty;
        private SerializedProperty IsAtmosEnabledProperty;
        private SerializedProperty IsExpanseEnabledProperty;
        private SerializedProperty LatitudeProperty;
        private SerializedProperty LongitudeProperty;
        private SerializedProperty RequestByGeoCoordinates;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            _scriptTarget = (RealTimeWeatherManager)target;
            AutoWeatherDataUpdateProperty = serializedObject.FindProperty(kIsAutoWeatherUpdateEnabledPropStr);
            RequestedCityProperty = serializedObject.FindProperty(kRequestedCityPropStr);
            RequestedCountryProperty = serializedObject.FindProperty(kRequestedCountryPropStr);
            AutoUpdateRateProperty = serializedObject.FindProperty(kAutoUpdateRatePropStr);
            IsWeatherSimulationEnabledProperty = serializedObject.FindProperty(kIsWeatherSimulationEnabledPropStr);
            DontDestroyProperty = serializedObject.FindProperty(kDontDestroyPropStr);
            IsEnviroEnabledProperty = serializedObject.FindProperty(kEnviroEnabledPropStr);
            IsTenkokuEnabledProperty = serializedObject.FindProperty(kTenkokuEnabledPropStr);
            IsAtmosEnabledProperty = serializedObject.FindProperty(kAtmosEnabledPropStr);
            IsExpanseEnabledProperty = serializedObject.FindProperty(kExpanseEnabledPropStr);
            LatitudeProperty = serializedObject.FindProperty(kLatitudePropStr);
            LongitudeProperty = serializedObject.FindProperty(kLongitudePropStr);
            RequestByGeoCoordinates = serializedObject.FindProperty(kRequestByGeoCoordinatesPropStr);

            list = new ReorderableList(_scriptTarget.DataWeatherProviders, typeof(List<string>), true, false, false, false);
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent(_scriptTarget.DataWeatherProviders[index], _scriptTarget.DataWeatherProvidersAddress[_scriptTarget.RTWDataWeatherProvidersIndexes[index]]));
            };

            list.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, "Weather Providers");

#if UNITY_2019_1_OR_NEWER
            list.onReorderCallbackWithDetails = (ReorderableList list, int oldIndex, int newIndex) =>
            {
                var listTemp = _scriptTarget.RTWDataWeatherProvidersIndexes;
                var moveItem = listTemp[oldIndex];
                listTemp.RemoveAt(oldIndex);
                listTemp.Insert(newIndex, moveItem);
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            };
#else
            list.onSelectCallback = (ReorderableList list) =>
            {
                currentIndex = list.index;
            };

            list.onReorderCallback = (ReorderableList list) =>
            {
                var listTemp = _scriptTarget.RTWDataWeatherProvidersIndexes;
                var moveItem = listTemp[currentIndex];
                listTemp.RemoveAt(currentIndex);
                listTemp.Insert(list.index, moveItem);
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            };
#endif
            list.footerHeight = 0;
        }

        public override void OnInspectorGUI()
        {
            SetEditorElementsStyle();
            // Title
            GUI.backgroundColor = _backgroundColor1;
            GUILayout.BeginVertical(string.Empty, _outerBoxStyle);
            GUILayout.Space(kHeaderSpace);
            // Logo
            GUI.DrawTexture(new Rect(_logoRect.x - 30, _logoRect.y + 10, 362.5f, 150), _logoTexture, ScaleMode.ScaleToFit, true);
            // Info
            GUILayout.Space(kLogoBottomSpace);
            GUIContent buttonText = new GUIContent(string.Empty);
            GUIStyle buttonStyle = GUIStyle.none;
            _infoRect = GUILayoutUtility.GetRect(buttonText, buttonStyle);
            // Help & Assist
            Rect helpRect = new Rect(_infoRect.x + kInfoMargin + kInfoSpace, _infoRect.y + 15, 38, 18);
            Rect assistRect = new Rect(_infoRect.x + kInfoMargin + kInfoSpace + 50, _infoRect.y + 15, 75, 18);

            if (Event.current.type == EventType.MouseUp && helpRect.Contains(Event.current.mousePosition))
            {
                Application.OpenURL(kHelpUrl);
            }

            if (Event.current.type == EventType.MouseUp && assistRect.Contains(Event.current.mousePosition))
            {
                Application.OpenURL(kAssistUrl);
            }

            EditorGUI.LabelField(new Rect(_infoRect.x + kInfoMargin + kInfoSpace + 38, _infoRect.y + 15, 220, 18), kSeparator, _labelInfoStyle);
            EditorGUI.LabelField(helpRect, kHelp, _labelInfoStyle);
            EditorGUI.LabelField(assistRect, kAssist, _labelInfoStyle);
            GUILayout.Space(kInfoBottomSpace);
            // Welcome
            _labelInfoStyle.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField(kWelcomeMessageStr, _labelInfoStyle);
            _labelInfoStyle.fontStyle = FontStyle.Normal;
            EditorGUILayout.LabelField(kInfoMessageStr, _labelInfoStyle);
            GUILayout.Space(kHeaderSpace);
            GUI.backgroundColor = _backgroundColor2;
            // General Settings
            AddGeneralSettingsControlsToInspector();
            // Simulation Settings
            GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
            _scriptTarget._showSimulationSettings = GUILayout.Toggle(_scriptTarget._showSimulationSettings, kSimulationSettings, _headerStyle);

            if (_scriptTarget._showSimulationSettings)
            {
                GUILayout.Space(kMarginSpace);
                // Localization;
                AddLocalizationControlsToInspector();
                // Automatic Weather Data Update
                AddAutoWeatherUpdateControlsToInspector();
                // Wheather simulation
                AddWeatherSimulationSettingsControlsToInspector();
                GUILayout.Space(kMarginSpace);
            }

            GUILayout.EndVertical();

            GUILayout.Space(20);
            EditorGUILayout.LabelField(kInfoVersionStr + " " + kInfoCurrentVersion, _labelInfoStyle);

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
#endregion

#region Private Methods
        /// <summary>
        /// Defines custom styles for the editor elements.
        /// </summary>
        private void SetEditorElementsStyle()
        {
            _labelTextColor = Color.white;
            _backgroundColor1 = new Color(0.50f, 0.50f, 0.50f, 1f);
            _backgroundColor2 = Color.white;

            if (_logoTexture == null)
            {
                _logoTexture = Resources.Load("textures/logo") as Texture2D;

                if (EditorGUIUtility.isProSkin == true)
                {
                    _logoTexture = Resources.Load("textures/logo") as Texture2D;
                }
            }

            GUIContent buttonText = new GUIContent(string.Empty);
            GUIStyle buttonStyle = GUIStyle.none;
            _logoRect = GUILayoutUtility.GetRect(buttonText, buttonStyle);

            if (_headerStyle == null)
            {
                _headerStyle = new GUIStyle(EditorStyles.foldout);
                _headerStyle.fontStyle = FontStyle.Bold;
            }

            if (_outerBoxStyle == null)
            {
                _outerBoxStyle = new GUIStyle(GUI.skin.box);
                _outerBoxStyle.normal.textColor = GUI.skin.label.normal.textColor;
                _outerBoxStyle.alignment = TextAnchor.UpperLeft;
                _outerBoxStyle.fontStyle = FontStyle.Bold;
            }

            if (_innerBoxStyle == null)
            {
                _innerBoxStyle = new GUIStyle(EditorStyles.helpBox);
                _innerBoxStyle.normal.textColor = GUI.skin.label.normal.textColor;
                _innerBoxStyle.alignment = TextAnchor.UpperLeft;
                _innerBoxStyle.fontStyle = FontStyle.Bold;
                _innerBoxStyle.fontSize = 11;
            }

            if (_labelInfoStyle == null)
            {
                _labelInfoStyle = new GUIStyle(GUI.skin.label);
                _labelInfoStyle.normal.textColor = _labelTextColor;
                _labelInfoStyle.fontStyle = FontStyle.Normal;
                _labelInfoStyle.alignment = TextAnchor.UpperLeft;
                _labelInfoStyle.wordWrap = true;
            }

            if (_labelWarningStyle == null)
            {
                _labelWarningStyle = new GUIStyle(GUI.skin.label);
                _labelWarningStyle.normal.textColor = new Color(204, 156, 0, 0.7f);
                _labelWarningStyle.fontStyle = FontStyle.Normal;
                _labelWarningStyle.alignment = TextAnchor.UpperLeft;
                _labelWarningStyle.wordWrap = true;
            }
        }

        /// <summary>
        /// This function adds elements to the inspector based on the presence of the Enviro plugin in the project.
        /// </summary>
        private void AddEnviroControlsToInspector()
        {
            GUILayout.BeginVertical(kEnviroStr, _innerBoxStyle);
            GUILayout.Space(kHeaderSpace);

#if ENVIRO_PRESENT
            if (!IsEnviroEnabledProperty.boolValue)
            {
                EditorGUILayout.LabelField(kEnviroDeactivatedStr, _labelInfoStyle);

                if (!IsTenkokuEnabledProperty.boolValue && !IsAtmosEnabledProperty.boolValue && !IsExpanseEnabledProperty.boolValue)
                {
                    if (GUILayout.Button(kEnviroActivateBtnStr))
                    {
                        IsEnviroEnabledProperty.boolValue = true;
                        _scriptTarget.ActivateEnviroSimulation();
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(kWarningStr, _labelWarningStyle);
                }
            }
            else
            {
                EditorGUILayout.LabelField(kEnviroActivatedStr, _labelInfoStyle);

                if (GUILayout.Button(kEnviroDeactivateBtnStr))
                {
                    IsEnviroEnabledProperty.boolValue = false;
                    _scriptTarget.DeactivateEnviroSimulation();
                }
            }
#else
            EditorGUILayout.LabelField(kEnviroNotFoundStr, _labelWarningStyle);

            if (IsEnviroEnabledProperty.boolValue)
            {
                _scriptTarget.DeactivateEnviroSimulation();
                IsEnviroEnabledProperty.boolValue = false;
            }
#endif
            GUILayout.Space(kMarginSpace);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// This function adds elements to the inspector based on the presence of the Tenkoku plugin in the project.
        /// </summary>
        private void AddTenkokuControlsToInspector()
        {
            GUILayout.BeginVertical(kTenkokuStr, _innerBoxStyle);
            GUILayout.Space(kHeaderSpace);

#if TENKOKU_PRESENT
            if (!IsTenkokuEnabledProperty.boolValue)
            {
                EditorGUILayout.LabelField(kTenkokuDeactivatedStr, _labelInfoStyle);

                if (!IsEnviroEnabledProperty.boolValue && !IsAtmosEnabledProperty.boolValue && !IsExpanseEnabledProperty.boolValue)
                {
                    if (GUILayout.Button(kTenkokuActivateBtnStr))
                    {
                        IsTenkokuEnabledProperty.boolValue = true;
                        _scriptTarget.ActivateTenkokuSimulation();
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(kWarningStr, _labelWarningStyle);
                }
            }
            else
            {
                EditorGUILayout.LabelField(kTenkokuActivatedStr, _labelInfoStyle);

                if (GUILayout.Button(kTenkokuDeactivateBtnStr))
                {
                    IsTenkokuEnabledProperty.boolValue = false;
                    _scriptTarget.DeactivateTenkokuSimulation();
                }
            }
#else
            EditorGUILayout.LabelField(kTenkokuNotFoundStr, _labelWarningStyle);

            if (IsTenkokuEnabledProperty.boolValue)
            {
                _scriptTarget.DeactivateTenkokuSimulation();
                IsTenkokuEnabledProperty.boolValue = false;
            }
#endif
            GUILayout.Space(kMarginSpace);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// This function adds elements to the inspector based on the presence of the Massive Clouds Atmos plugin in the project.
        /// </summary>
        private void AddAtmosControlsToInspector()
        {
            GUILayout.BeginVertical(kAtmosStr, _innerBoxStyle);
            GUILayout.Space(kHeaderSpace);

#if ATMOS_PRESENT
            if (!IsAtmosEnabledProperty.boolValue)
            {
                EditorGUILayout.LabelField(kAtmosDeactivatedStr, _labelInfoStyle);

                if (!IsTenkokuEnabledProperty.boolValue && !IsEnviroEnabledProperty.boolValue && !IsExpanseEnabledProperty.boolValue)
                {
                    if (GUILayout.Button(kAtmosActivateBtnStr))
                    {
                        IsAtmosEnabledProperty.boolValue = true;
                        _scriptTarget.ActivateAtmosSimulation();
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(kWarningStr, _labelWarningStyle);
                }
            }
            else
            {
                EditorGUILayout.LabelField(kAtmosActivatedStr, _labelInfoStyle);

                if (GUILayout.Button(kAtmosDeactivateBtnStr))
                {
                    IsAtmosEnabledProperty.boolValue = false;
                    _scriptTarget.DeactivateAtmosSimulation();
                }
            }
#else
            EditorGUILayout.LabelField(kAtmosNotFoundStr, _labelWarningStyle);

            if (IsAtmosEnabledProperty.boolValue)
            {
                _scriptTarget.DeactivateAtmosSimulation();
                IsAtmosEnabledProperty.boolValue = false;
            }
#endif
            GUILayout.Space(kMarginSpace);
            GUILayout.EndVertical();
        }

         /// <summary>
        /// This function adds elements to the inspector based on the presence of the Expanse plugin in the project.
        /// </summary>
        private void AddExpanseControlsToInspector()
        {
            GUILayout.BeginVertical(kExpanseStr, _innerBoxStyle);
            GUILayout.Space(kHeaderSpace);

#if EXPANSE_PRESENT
            if (!IsExpanseEnabledProperty.boolValue)
            {
                EditorGUILayout.LabelField(kExpanseDeactivatedStr, _labelInfoStyle);

                if (!IsTenkokuEnabledProperty.boolValue && !IsEnviroEnabledProperty.boolValue && !IsAtmosEnabledProperty.boolValue)
                {
                    if (GUILayout.Button(kExpanseActivateBtnStr))
                    {
                        IsExpanseEnabledProperty.boolValue = true;
                        _scriptTarget.ActivateExpanseSimulation();
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(kWarningStr, _labelWarningStyle);
                }
            }
            else
            {
                EditorGUILayout.LabelField(kExpanseActivatedStr, _labelInfoStyle);

                if (GUILayout.Button(kExpanseDeactivateBtnStr))
                {
                    IsExpanseEnabledProperty.boolValue = false;
                    _scriptTarget.DeactivateExpanseSimulation();
                }
            }
#else
            EditorGUILayout.LabelField(kExpanseNotFoundStr, _labelWarningStyle);

            if (IsExpanseEnabledProperty.boolValue)
            {
                _scriptTarget.DeactivateExpanseSimulation();
                IsExpanseEnabledProperty.boolValue = false;
            }
#endif
            GUILayout.Space(kMarginSpace);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// This function adds the general settings controls to the inspector.
        /// </summary>
        private void AddGeneralSettingsControlsToInspector()
        {
            GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
            _scriptTarget._showGeneralSettings = GUILayout.Toggle(_scriptTarget._showGeneralSettings, kGeneralSettingsStr, _headerStyle);

            if (_scriptTarget._showGeneralSettings)
            {
                GUILayout.Space(kMarginSpace);
                GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
                GUILayout.Space(kMarginSpace);
                DontDestroyProperty.boolValue = EditorGUILayout.ToggleLeft(kDontDestroyStr, DontDestroyProperty.boolValue);

                GUILayout.Space(kElementsSpace);
                EditorGUILayout.LabelField(kRequestModeStr);
                int requestModeIndex = (int)_scriptTarget.DataRequestMode;
                _scriptTarget.DataRequestMode = (RequestMode)EditorGUILayout.Popup(string.Empty, requestModeIndex, _dataRequestMode);

                if (requestModeIndex != (int)_scriptTarget.DataRequestMode)
                {
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
                }
                if (_scriptTarget.DataRequestMode == RequestMode.RtwMode)
                {
                    GUILayout.Space(kElementsSpace);
                    list.DoLayoutList();
                }

                if (_scriptTarget.DataRequestMode != _scriptTarget.LastChoosedRequestMode)
                {
                    _scriptTarget.LastChoosedRequestMode = _scriptTarget.DataRequestMode;

                    SwitchModule();
                }

                GUILayout.Space(kMarginSpace);
                GUILayout.EndVertical();
                GUILayout.Space(kMarginSpace);
            }

            GUILayout.EndVertical();
            GUILayout.Space(7);
        }

        /// <summary>
        /// This function adds the localization settings controls to the inspector.
        /// </summary>
        private void AddLocalizationControlsToInspector()
        {
            GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
            if (_scriptTarget.DataRequestMode == RequestMode.RtwMode)
            {
                _scriptTarget._showLocalizationSettings = GUILayout.Toggle(_scriptTarget._showLocalizationSettings, kLocalizationStr, _headerStyle);
                if (_scriptTarget._showLocalizationSettings)
                {
                    GUILayout.Space(kMarginSpace);
                    EditorGUILayout.LabelField(kInfoLocalizationStr, _labelInfoStyle);
                    GUILayout.Space(kElementsSpace);
                    RequestByGeoCoordinates.boolValue = EditorGUILayout.ToggleLeft(kRequestByGeoCoordinatesStr, RequestByGeoCoordinates.boolValue);

                    if (!RequestByGeoCoordinates.boolValue)
                    {
                        // City
                        GUILayout.Space(kElementsSpace);
                        EditorGUILayout.LabelField(kCityStr);
                        RequestedCityProperty.stringValue = GUILayout.TextField(RequestedCityProperty.stringValue);
                        // Country
                        GUILayout.Space(kElementsSpace);
                        EditorGUILayout.LabelField(kCountryStr);
                        RequestedCountryProperty.stringValue = GUILayout.TextField(RequestedCountryProperty.stringValue);
                    }
                    else
                    {
                        // Longitude
                        GUILayout.Space(kElementsSpace);
                        EditorGUILayout.PropertyField(LatitudeProperty);
                        // Latitude
                        GUILayout.Space(kElementsSpace);
                        EditorGUILayout.PropertyField(LongitudeProperty);
                    }

                    GUILayout.Space(kMarginSpace);
                }
            }
            else
            {
                String redirectBtnContent = "Go to ";
                if (_scriptTarget.DataRequestMode == RequestMode.TomorrowMode)
                    redirectBtnContent += kTomorrowModuleObjectName;
                else if (_scriptTarget.DataRequestMode == RequestMode.OpenWeatherMapMode)
                    redirectBtnContent += kOpenWeatherMapObjectName;

                if (GUILayout.Button(new GUIContent(redirectBtnContent, EditorGUIUtility.IconContent("PlayButton").image)))
                {
                    SwitchModule();
                }
            }
            GUILayout.EndVertical();
        }

        /// <summary>
        /// This function switches to selected module in Request mode
        /// </summary>
        private void SwitchModule()
        {
            if (_scriptTarget.DataRequestMode == RequestMode.TomorrowMode)
            {
                for (int i = 0; i < _scriptTarget.gameObject.transform.childCount; i++)
                {
                    if (_scriptTarget.transform.GetChild(i).name.Equals(kTomorrowModuleObjectName))
                    {
                        Selection.activeObject = _scriptTarget.transform.GetChild(i);
                    }
                }
            }

            if (_scriptTarget.DataRequestMode == RequestMode.OpenWeatherMapMode)
            {
                for (int i = 0; i < _scriptTarget.gameObject.transform.childCount; i++)
                {
                    if (_scriptTarget.transform.GetChild(i).name.Equals(kOpenWeatherMapObjectName))
                    {
                        Selection.activeObject = _scriptTarget.transform.GetChild(i);
                    }
                }
            }
        }

        /// <summary>
        /// This function adds the auto weather update controls to the inspector.
        /// </summary>
        private void AddAutoWeatherUpdateControlsToInspector()
        {
            GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
            _scriptTarget._showAutoUpdateSettings = GUILayout.Toggle(_scriptTarget._showAutoUpdateSettings, kAutoWeatherStr, _headerStyle);

            if (_scriptTarget._showAutoUpdateSettings)
            {
                GUILayout.Space(kMarginSpace);
                EditorGUILayout.LabelField(kInfoAutoUpdateStr, _labelInfoStyle);
                // Auto weather checkbox
                GUILayout.Space(kElementsSpace);
                AutoWeatherDataUpdateProperty.boolValue = EditorGUILayout.ToggleLeft(kAutoWeatherUpdateStr, AutoWeatherDataUpdateProperty.boolValue);
                _scriptTarget.OnAutoWeatherStateChanged();
                // Auto weather update rate
                GUILayout.Space(kElementsSpace);
                AutoUpdateRateProperty.floatValue = EditorGUILayout.Slider(kAutoUpdateRateStr, AutoUpdateRateProperty.floatValue, kMinFrequencyValue, kMaxFrequencyValue);
                GUILayout.Space(kMarginSpace);
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        /// This function adds the weather simulation settings controls to the inspector.
        /// </summary>
        private void AddWeatherSimulationSettingsControlsToInspector()
        {
            GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
            _scriptTarget._showWeatherSimulationSettings = GUILayout.Toggle(_scriptTarget._showWeatherSimulationSettings, kWeatherSimulationStr, _headerStyle);

            if (_scriptTarget._showWeatherSimulationSettings)
            {
                GUILayout.Space(kMarginSpace);
                EditorGUILayout.LabelField(kInfoSimulationStr, _labelInfoStyle);
                GUILayout.Space(kElementsSpace);
                // Envrio
                AddEnviroControlsToInspector();
                // Tenkoku
                AddTenkokuControlsToInspector();
                // Atmos
                AddAtmosControlsToInspector();
#if UNITY_PIPELINE_HDRP
                // Expanse
                AddExpanseControlsToInspector();
#endif
                GUILayout.Space(kMarginSpace);
            }

            GUILayout.EndVertical();
        }
#endregion
    }
}