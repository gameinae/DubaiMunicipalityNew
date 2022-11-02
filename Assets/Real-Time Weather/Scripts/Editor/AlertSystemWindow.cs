//
// Copyright(c) 2020 Real-Time Weather ASSIST-Software
// https://www.assist.ro
//
// For information about the licensing and copyright of this software please
// contact assist contact@assist.ro
//

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace RealTimeWeather.AlertSystem
{
    /// <summary>
    /// This class it use for alerts that are send from weather system.
    /// </summary>
    public class AlertSystemWindow : EditorWindow
    {
        #region Private Const
        private const string ksendButton = "Send";
        public string[] _options = new string[3] { EditorConstants.kBug, EditorConstants.kServicesDown, EditorConstants.kAskQuestion };
        #endregion

        #region private Varibles
        private string _emailFrom = "";
        private string _details = "Enter text...";
        private Vector2 _scroll;
        private int _selected = 1;
        #endregion

        #region Public Methods
        /// <summary>
        /// This method Activate the Weather System Bug Reporter window.
        /// </summary>
        [MenuItem(EditorConstants.kWeatherTabName)]
        public static void Init()
        {
            AlertSystemWindow window = (AlertSystemWindow)GetWindow(typeof(AlertSystemWindow), false);
            window.titleContent.text = EditorConstants.kWeatherDescription;
        }

        /// <summary>
        /// This method open Display Dialog when email format is invalid.
        /// </summary>
        public static void InvalidEmailDialog()
        {
            EditorUtility.DisplayDialog("Invalid Email", "Please use a correct email address format!", "OK!");
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            _selected = EditorGUILayout.Popup("Select the problem", _selected, _options);
            EditorGUILayout.LabelField("Details");
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            _details = EditorGUILayout.TextArea(_details, GUILayout.Height(position.height));
            EditorGUILayout.EndScrollView();
            EditorGUILayout.TextField("Attached files:", LogFile.Path);
            _emailFrom = EditorGUILayout.TextField("Your Email Adress:", _emailFrom);
            //Check if the send button is press
            if (GUILayout.Button(ksendButton))
            {
                if (AlertSystemModuleHelper.IsValidEmail(_emailFrom))
                {
                    AlertSystemModuleHelper.SendEmail(_emailFrom, EditorConstants.kEmailTo, _options[_selected], _details + "\n\n" + LogFile.GetLogText() + "\n\n");
                }
                else
                {
                    InvalidEmailDialog();
                }
            }

        }

        #endregion
    }
}