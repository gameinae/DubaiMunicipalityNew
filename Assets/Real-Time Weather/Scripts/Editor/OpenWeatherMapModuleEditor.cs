//
// Copyright(c) 2021 Real-Time Weather ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at hello@assist.ro
//

using RealTimeWeather.OpenWeatherMap;
using RealTimeWeather.OpenWeatherMap.Classes;
using UnityEditor;
using UnityEngine;

namespace RealTimeWeather.Editors.OpenWeatherMap
{
    /// <summary>
    /// This class create a custom editor for OpenWeatherMapModule component. 
    /// </summary>
    [CustomEditor(typeof(OpenWeatherMapModule))]
    public class OpenWeatherMapModuleEditor : Editor
    {
        #region Private Const Variables
        // Properties constants
        private const string kApiKeyStr = "_apiKey";
        private const string kLanguageParameterStr = "_language";
        private const string kUnitsParameterStr = "_units";
        private const string kRequestModeStr = "_requestMode";
        private const string kCityNameStr = "_cityName";
        private const string kStateCodeStr = "_stateCode";
        private const string kCountryCodeStr = "_countryCode";
        private const string kCityIDStr = "_cityID";
        private const string kLatitudeStr = "_latitude";
        private const string kLongitudeStr = "_longitude";
        private const string kZipCodeStr = "_zipCode";
        private const string kCountryCodeZStr = "_countryCodeZ";
        private const string kHourlyWeather = "_hourlyWeather";
        private const string kDailyWeather = "_dailyWeather";

        // Labels
        private const string kApiSettingStr = "API Settings";
        private const string kParametersSettingStr = "Parameters Settings";
        private const string kWeatherPeriodSettingStr = "Weather Period Settings";

        private const string kHourlyWeatherStr = "Hourly - next 48 hours";
        private const string kDailyWeatherStr = "Daily - current & next 7 days";

        private const string kGoBackButtonStr = "Go back";

        private const int kMarginSpace = 5;
        private const int kElementsSpace = 5;
        private const int kInnerSpace = 7;
        private const int kGoBackBtnWidth = 100;
        private const int kGoBackBtnHeight = 20;
        #endregion

        #region Private Variables
        private OpenWeatherMapModule _scriptTarget;

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
        private SerializedProperty LanguageParamaterProperty;
        private SerializedProperty UnitsParamaterProperty;
        private SerializedProperty RequestModeParamaterProperty;
        private SerializedProperty CityNameProperty;
        private SerializedProperty StateCodeProperty;
        private SerializedProperty CountryCodeProperty;
        private SerializedProperty CityIDProperty;
        private SerializedProperty LatitudeProperty;
        private SerializedProperty LongitudeProperty;
        private SerializedProperty ZipCodeProperty;
        private SerializedProperty CountryCodeZProperty;
        private SerializedProperty HourlyWeatherProperty;
        private SerializedProperty DailyWeatherProperty;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            _scriptTarget = (OpenWeatherMapModule)target;
            ApiKeyProperty = serializedObject.FindProperty(kApiKeyStr);
            LanguageParamaterProperty = serializedObject.FindProperty(kLanguageParameterStr);
            UnitsParamaterProperty = serializedObject.FindProperty(kUnitsParameterStr);
            RequestModeParamaterProperty = serializedObject.FindProperty(kRequestModeStr);
            CityNameProperty = serializedObject.FindProperty(kCityNameStr);
            StateCodeProperty = serializedObject.FindProperty(kStateCodeStr);
            CountryCodeProperty = serializedObject.FindProperty(kCountryCodeStr);
            CityIDProperty = serializedObject.FindProperty(kCityIDStr);
            LatitudeProperty = serializedObject.FindProperty(kLatitudeStr);
            LongitudeProperty = serializedObject.FindProperty(kLongitudeStr);
            ZipCodeProperty = serializedObject.FindProperty(kZipCodeStr);
            CountryCodeZProperty = serializedObject.FindProperty(kCountryCodeZStr);
            HourlyWeatherProperty = serializedObject.FindProperty(kHourlyWeather);
            DailyWeatherProperty = serializedObject.FindProperty(kDailyWeather);
        }

        public override void OnInspectorGUI()
        {
            SetOpenWeatherMapEditorElementsStyle();
            // General Settings
            serializedObject.Update();

            GUI.backgroundColor = _backgroundColor1;
            GUILayout.BeginVertical(string.Empty, _outerBoxStyle);
            GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
            GUI.backgroundColor = _backgroundColor2;
            GUILayout.Space(kMarginSpace);
            // Api key;
            AddApiSettingsControlsToInspector();
            // Parameters
            AddParametersControlsToInspector();
            // Input data;
            AddInputDataControlsToInspector();
            GUILayout.Space(kMarginSpace);
            GUILayout.EndVertical();

            //Request wheather for a period
            AddParametersForPeriodControlsToInspector();

            //Back Button
            GUI.backgroundColor = Color.gray;
            GUILayout.Space(kInnerSpace);
            if (GUILayout.Button(kGoBackButtonStr, _goBackButtonStyle, GUILayout.Width(kGoBackBtnWidth), GUILayout.Height(kGoBackBtnHeight)))
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
        /// Defines custom styles for the OpenWeatherMap editor elements.
        /// </summary>
        private void SetOpenWeatherMapEditorElementsStyle()
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

            if (_goBackButtonStyle == null)
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
        /// This function adds the parameters settings controls to the inspector.
        /// </summary>
        private void AddParametersControlsToInspector()
        {
            GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
            _scriptTarget._showParameterSettings = GUILayout.Toggle(_scriptTarget._showParameterSettings, kParametersSettingStr, _headerStyle);

            if (_scriptTarget._showParameterSettings)
            {
                GUILayout.Space(kMarginSpace);
                EditorGUILayout.PropertyField(LanguageParamaterProperty);
                //EditorGUILayout.PropertyField(UnitsParamaterProperty);
                EditorGUILayout.PropertyField(RequestModeParamaterProperty);
                GUILayout.Space(kMarginSpace);
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        /// This function adds the input data settings controls to the inspector.
        /// </summary>
        private void AddInputDataControlsToInspector()
        {
            GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
            switch (_scriptTarget.RequestMode)
            {
                case RequestMode.CityName:
                    EditorGUILayout.PropertyField(CityNameProperty);
                    EditorGUILayout.PropertyField(StateCodeProperty);
                    EditorGUILayout.PropertyField(CountryCodeProperty);
                    break;
                case RequestMode.CityID:
                    EditorGUILayout.PropertyField(CityIDProperty);
                    break;
                case RequestMode.GeographicCoordinates:
                    EditorGUILayout.PropertyField(LatitudeProperty);
                    EditorGUILayout.PropertyField(LongitudeProperty);
                    break;
                case RequestMode.ZipCode:
                    EditorGUILayout.PropertyField(ZipCodeProperty);
                    EditorGUILayout.PropertyField(CountryCodeZProperty);
                    break;
            }
            GUILayout.Space(kMarginSpace);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// This function adds checkboxes for data settings controls to the inspector.
        /// </summary>
        private void AddParametersForPeriodControlsToInspector()
        {
            if (_scriptTarget.RequestMode == RequestMode.GeographicCoordinates)
            { 
                GUILayout.BeginVertical(string.Empty, _innerBoxStyle);
                _scriptTarget._showWeatherByPeriodSettings = GUILayout.Toggle(_scriptTarget._showWeatherByPeriodSettings, kWeatherPeriodSettingStr, _headerStyle);
                if (_scriptTarget._showWeatherByPeriodSettings)
                {
                    GUILayout.Space(kMarginSpace);
                    EditorGUILayout.PropertyField(HourlyWeatherProperty, new GUIContent(kHourlyWeatherStr));
                    EditorGUILayout.PropertyField(DailyWeatherProperty, new GUIContent(kDailyWeatherStr, "Visibility is not available/always 100km"));
                    GUILayout.Space(kInnerSpace);
                }
                GUILayout.EndVertical();
            }
        }

        #endregion
    }
}