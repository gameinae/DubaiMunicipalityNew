//
// Copyright(c) 2021 Real-Time Weather ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at hello@assist.ro
//

using RealTimeWeather.Tomorrow;
using UnityEditor;
using UnityEngine;

namespace RealTimeWeather.Editors.Tomorrow
{
    /// <summary>
    /// This class create a custom editor for TomorrowModule component.
    /// </summary>
    [CustomEditor(typeof(TomorrowModule))]
    public class TomorrowModuleEditor : Editor
    {
        #region Private Const Variables
        // Properties constants
        private const string kApiKeyStr = "_apiKey";
        private const string kLatitudeStr = "_latitude";
        private const string kLongitudeStr = "_longitude";
        private const string kHourlyWeatherStr = "_hourlyForecast";
        private const string kHourlyWeatherLengthStr = "_hourlyForecastLength";
        private const string kDailyWeatherLengthStr = "_dailyForecastLength";
        private const string kDailyWeatherStr = "_dailyForecast";
        private const string kPressureSeaLeveStr = "_pressureSeaLevel";
        private const string kPrecipitationProbabilityStr = "_precipitationProbability";
        private const string kCloudCoverStr = "_cloudCover";
        private const string kWindGustStr = "_windGust";
        private const string kTemperatureApparentStr = "_temperatureApparent";
        private const string kParticulateMatter25Str = "_particulateMatter25";
        private const string kPollutantO3Str = "_pollutantO3";
        private const string kPollutantNO2Str = "_pollutantNO2";
        private const string kPollutantCOStr = "_pollutantCO";
        private const string kPollutantSO2Str = "_pollutantSO2";
        private const string kTreeIndexStr = "_treeIndex";
        private const string kGrassIndexStr = "_grassIndex";
        private const string kWeedIndexStr = "_weedIndex";

        // Labels
        private const string kApiSettingStr = "API Settings";
        private const string kLocationSettingStr = "Location Settings";
        private const string kWeatherDataSettingStr = "Weather Data Settings";
        private const string kWeatherDataLabelStr = "Check what extra weather data you want to be requested from the API:";
        private const string kWeatherDataForecastStr = "Check what forecast to be requested from the API:";
        private const string kGoBackButtonStr = "Go back";

        private const int kMarginSpace = 5;
        private const int kInnerSpace = 7;
        private const int kGoBackBtnWidth = 100;
        private const int kGoBackBtnHeight = 20;
        private const int kHoursForOneDay = 24;
        private const int kMaxHoursForecast = 108;
        #endregion

        #region Private Variables
        private TomorrowModule _scriptTarget;

        private GUIStyle _headerStyle;
        private GUIStyle _outerBoxStyle;
        private GUIStyle _innerBoxStyle;
        private GUIStyle _labelInfoStyle;
        private GUIStyle _goBackButtonStyle;

        private Color _labelTextColor;
        private Color _backgroundColor1;
        private Color _backgroundColor2;
        #endregion

        #region Private Properties
        private SerializedProperty ApiKeyProperty;
        private SerializedProperty LatitudeProperty;
        private SerializedProperty LongitudeProperty;
        private SerializedProperty HourlyForecastProperty;
        private SerializedProperty ForecastRateHoursProperty;
        private SerializedProperty DailyForecastProperty;
        private SerializedProperty ForecastRateDaysProperty;
        private SerializedProperty PressureSeaLeveProperty;
        private SerializedProperty PrecipitationProbabilityProperty;
        private SerializedProperty CloudCoverProperty;
        private SerializedProperty WindGustProperty;
        private SerializedProperty TemperatureApparentProperty;
        private SerializedProperty ParticulateMatter25Property;
        private SerializedProperty PollutantO3Property;
        private SerializedProperty PollutantNO2Property;
        private SerializedProperty PollutantCOProperty;
        private SerializedProperty PollutantSO2Property;
        private SerializedProperty TreeIndexProperty;
        private SerializedProperty GrassIndexProperty;
        private SerializedProperty WeedIndexProperty;
        #endregion

        #region Public Properties
        public TomorrowModule TomorrowModule
        {
            get { return _scriptTarget; }
        }
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            _scriptTarget = (TomorrowModule)target;
            ApiKeyProperty = serializedObject.FindProperty(kApiKeyStr);
            LatitudeProperty = serializedObject.FindProperty(kLatitudeStr);
            LongitudeProperty = serializedObject.FindProperty(kLongitudeStr);
            HourlyForecastProperty = serializedObject.FindProperty(kHourlyWeatherStr);
            ForecastRateHoursProperty = serializedObject.FindProperty(kHourlyWeatherLengthStr);
            DailyForecastProperty = serializedObject.FindProperty(kDailyWeatherStr);
            ForecastRateDaysProperty = serializedObject.FindProperty(kDailyWeatherLengthStr);
            PressureSeaLeveProperty = serializedObject.FindProperty(kPressureSeaLeveStr);
            PrecipitationProbabilityProperty = serializedObject.FindProperty(kPrecipitationProbabilityStr);
            CloudCoverProperty = serializedObject.FindProperty(kCloudCoverStr);
            WindGustProperty = serializedObject.FindProperty(kWindGustStr);
            TemperatureApparentProperty = serializedObject.FindProperty(kTemperatureApparentStr);
            ParticulateMatter25Property = serializedObject.FindProperty(kParticulateMatter25Str);
            PollutantO3Property = serializedObject.FindProperty(kPollutantO3Str);
            PollutantSO2Property = serializedObject.FindProperty(kPollutantSO2Str);
            PollutantCOProperty = serializedObject.FindProperty(kPollutantCOStr);
            PollutantNO2Property = serializedObject.FindProperty(kPollutantNO2Str);
            TreeIndexProperty = serializedObject.FindProperty(kTreeIndexStr);
            GrassIndexProperty = serializedObject.FindProperty(kGrassIndexStr);
            WeedIndexProperty = serializedObject.FindProperty(kWeedIndexStr);
            
        }

        public override void OnInspectorGUI()
        {
            SetTomorrowEditorElementsStyle();
            // General Settings
            serializedObject.Update();

            GUI.backgroundColor = _backgroundColor1;
            GUILayout.BeginVertical(string.Empty, _outerBoxStyle);
            GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
            GUI.backgroundColor = _backgroundColor2;
            GUILayout.Space(kMarginSpace);
            // Api key;
            AddApiSettingsControlsToInspector();
            // Localization
            AddLocationControlsToInspector();
            // Weather data;
            AddWeatherDataControlsToInspector();
            GUILayout.Space(kMarginSpace);
            GUILayout.EndVertical();

            //Back Button
            GUI.backgroundColor = Color.gray;
            GUILayout.Space(kInnerSpace);
            if(GUILayout.Button(kGoBackButtonStr, _goBackButtonStyle, GUILayout.Width(kGoBackBtnWidth), GUILayout.Height(kGoBackBtnHeight)))
            {
                Selection.activeObject = _scriptTarget.transform.root.gameObject;
            }

            GUI.backgroundColor = _backgroundColor1;
            GUILayout.Space(kInnerSpace);
            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Defines custom styles for the Tomorrow editor elements.
        /// </summary>
        private void SetTomorrowEditorElementsStyle()
        {
            _labelTextColor = Color.white;
            _backgroundColor1 = new Color(0.50f, 0.50f, 0.50f, 1f);
            _backgroundColor2 = Color.white;

            if (_headerStyle == null)
            {
                _headerStyle = new GUIStyle(EditorStyles.foldout);
                _headerStyle.fontStyle = FontStyle.Bold;
                _headerStyle.normal.textColor = _labelTextColor;
                _headerStyle.active.textColor = _labelTextColor;
                _headerStyle.hover.textColor = _labelTextColor;
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

            if(_goBackButtonStyle == null)
            {
                _goBackButtonStyle = new GUIStyle(GUI.skin.button);
                _goBackButtonStyle.normal.textColor = _labelTextColor;
                _goBackButtonStyle.hover.textColor = _labelTextColor;
            }
        }

        /// <summary>
        /// This function adds the API settings controls to the inspector.
        /// </summary>
        private void AddApiSettingsControlsToInspector()
        {
            GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
            _scriptTarget._showApiKeySettings = GUILayout.Toggle(_scriptTarget._showApiKeySettings, kApiSettingStr, _headerStyle);

            if (_scriptTarget._showApiKeySettings)
            {
                GUILayout.Space(kMarginSpace);
                EditorGUILayout.PropertyField(ApiKeyProperty);
                GUILayout.Space(kMarginSpace);
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        /// This function adds the localization settings controls to the inspector.
        /// </summary>
        private void AddLocationControlsToInspector()
        {
            GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
            _scriptTarget._showLocationSettings = GUILayout.Toggle(_scriptTarget._showLocationSettings, kLocationSettingStr, _headerStyle);

            if (_scriptTarget._showLocationSettings)
            {
                GUILayout.Space(kMarginSpace);
                EditorGUILayout.PropertyField(LatitudeProperty);
                EditorGUILayout.PropertyField(LongitudeProperty);
                GUILayout.Space(kMarginSpace);
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        /// This function adds the weather data settings controls to the inspector.
        /// </summary>
        private void AddWeatherDataControlsToInspector()
        {
            GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
            _scriptTarget._showWeatherDataSettings = GUILayout.Toggle(_scriptTarget._showWeatherDataSettings, kWeatherDataSettingStr, _headerStyle);

            if (_scriptTarget._showWeatherDataSettings)
            {
                GUILayout.Space(kMarginSpace);
                EditorGUILayout.LabelField(kWeatherDataForecastStr, _labelInfoStyle);
                EditorGUILayout.PropertyField(HourlyForecastProperty);
                if (HourlyForecastProperty.boolValue)
                {
                    EditorGUILayout.PropertyField(ForecastRateHoursProperty);
                }
                EditorGUILayout.PropertyField(DailyForecastProperty);
                if (DailyForecastProperty.boolValue)
                {
                    EditorGUILayout.PropertyField(ForecastRateDaysProperty);
                }

                GUILayout.Space(kInnerSpace);
                EditorGUILayout.LabelField(kWeatherDataLabelStr, _labelInfoStyle);
                EditorGUILayout.PropertyField(TemperatureApparentProperty);
                EditorGUILayout.PropertyField(PressureSeaLeveProperty);
                EditorGUILayout.PropertyField(PrecipitationProbabilityProperty);
                EditorGUILayout.PropertyField(CloudCoverProperty);
                EditorGUILayout.PropertyField(WindGustProperty);
                EditorGUILayout.PropertyField(ParticulateMatter25Property);
                EditorGUILayout.PropertyField(PollutantO3Property);
                EditorGUILayout.PropertyField(PollutantCOProperty);
                EditorGUILayout.PropertyField(PollutantNO2Property);
                EditorGUILayout.PropertyField(PollutantSO2Property);
                EditorGUILayout.PropertyField(TreeIndexProperty);
                EditorGUILayout.PropertyField(GrassIndexProperty);
                EditorGUILayout.PropertyField(WeedIndexProperty);
                GUILayout.Space(kMarginSpace);
            }

            GUILayout.EndVertical();
        }
        #endregion
    }
}