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

using System;
using UnityEngine;

namespace RealTimeWeather.Managers
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public class ScriptingDefineSymbolsManager
    {
    #region Private Const Variables
        private const string kCommaStr = ";";
        // Enviro module constants
        private const string kEnviroAssetStr = "Enviro - Sky and Weather";
        private const string kEnviroSymbolStr = "ENVIRO_PRESENT";
        private const string kEnviroHDSymbolStr = "ENVIRO_HD";
        private const string kEnviroLWSymbolStr = "ENVIRO_LW";
        // Tenkoku module constants
        private const string kTenkokuAssetStr = "TENKOKU - DYNAMIC SKY";
        private const string kTenkokuSymbolStr = "TENKOKU_PRESENT";
        // Atmos module constants
        private const string kAtmosAssetStr = "MassiveCloudsAtmos";
        private const string kAtmosSymbolStr = "ATMOS_PRESENT";
        private const string kAtmosPadPackagePath = "/Real-Time Weather/Packages/AtmosPadPackage.unitypackage";
        private const string kAtmosPadAssetPath = "/Real-Time Weather/Prefabs/Atmos Prefabs/Atmos Pad Package Prefabs";
        // Expanse module constants
        private const string kExpanseAssetStr = "ExpanseAsmDefs";
        private const string kExpanseSymbolStr = "EXPANSE_PRESENT";
        //HDRP constants
        private const string kHDRenderPipelineSymbolStr = "UNITY_PIPELINE_HDRP";
        private const string kHDRenderPipelineAssetNameStr = "HDRenderPipelineAsset";
    #endregion

    #region Private Variables
        private static BuildTargetGroup[] _buildTargetPlatforms = { BuildTargetGroup.Standalone, BuildTargetGroup.Android, BuildTargetGroup.iOS };
    #endregion

    #region Constructor
        static ScriptingDefineSymbolsManager()
        {
#if UNITY_2018_1_OR_NEWER
            EditorApplication.projectChanged += UpdateDefines;
#else
            EditorApplication.update += UpdateDefines;
#endif
        }
#endregion

            #region Private Methods
        /// <summary>
        /// Updates the scripting define symbols.
        /// </summary>
        private static void UpdateDefines()
        {
            ValidateEnviroDefines();
            ValidateTenkokuDefines();
            ValidateAtmosDefines();
            ValidateHDRenderPipelineDefines();
#if UNITY_PIPELINE_HDRP
            ValidateExpanseDefines();
#endif
        }

        /// <summary>
        /// Attempts to add a new #define constant to the Player Settings
        /// </summary>
        /// <param name="newDefineSymbol">A string value that represents the symbol to define.</param>
        /// <param name="targetPlatforms">A BuildTargetGroup array that specifies the target platforms.</param>
        private static void AddDefine(string newDefineSymbol, BuildTargetGroup[] targetPlatforms = null)
        {
            if (targetPlatforms == null)
            {
                return;
            }

            foreach (BuildTargetGroup target in targetPlatforms)
            {
                if (target == BuildTargetGroup.Unknown)
                {
                    continue;
                }

                string targetDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);

                if (!targetDefines.Contains(newDefineSymbol))
                {
                    if (targetDefines.Length > 0)
                    {
                        targetDefines += kCommaStr;
                    }

                    targetDefines += newDefineSymbol;
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(target, targetDefines);
                }
            }
        }

        /// <summary>
        /// Attempts to remove a #define constant from the Player Settings
        /// </summary>
        /// <param name="defineSymbol">A string value that represents the symbol to remove.</param>
        /// <param name="targetPlatforms">A BuildTargetGroup array that specifies the target platforms.</param>
        private static void RemoveDefine(string defineSymbol, BuildTargetGroup[] targetPlatforms = null)
        {
            if (targetPlatforms == null)
            {
                return;
            }

            foreach (BuildTargetGroup target in targetPlatforms)
            {
                string targetDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
                int symbolIndex = targetDefines.IndexOf(defineSymbol);

                if (symbolIndex < 0)
                {
                    continue;
                }
                else if (symbolIndex > 0)
                {
                    symbolIndex -= 1;
                }

                int length = Math.Min(defineSymbol.Length + 1, targetDefines.Length - symbolIndex);
                targetDefines = targetDefines.Remove(symbolIndex, length);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(target, targetDefines);
            }
        }

        /// <summary>
        /// Validates the presence of Enviro in the project and adds/removes the #define constant "ENVIRO_PRESENT".
        /// </summary>
        private static void ValidateEnviroDefines()
        {
            string[] enviroAssets = AssetDatabase.FindAssets(kEnviroAssetStr, null);

            if (enviroAssets.Length != 0)
            {
#if !ENVIRO_PRESENT
                AddDefine(kEnviroSymbolStr, _buildTargetPlatforms);
#endif
#if UNITY_ANDROID || UNITY_IOS
#if !ENVIRO_HD
                AddDefine(kEnviroHDSymbolStr, _buildTargetPlatforms);
#endif

#if !ENVIRO_LW
                AddDefine(kEnviroLWSymbolStr, _buildTargetPlatforms);
#endif
#endif

            }
            else
            {
#if ENVIRO_PRESENT
                RemoveDefine(kEnviroSymbolStr, _buildTargetPlatforms);
#endif
#if UNITY_ANDROID || UNITY_IOS
#if ENVIRO_HD
                RemoveDefine(kEnviroHDSymbolStr, _buildTargetPlatforms);
#endif

#if ENVIRO_LW
                RemoveDefine(kEnviroLWSymbolStr, _buildTargetPlatforms);
#endif
#endif
            }
        }

        /// <summary>
        /// Validates the presence of Tenkoku in the project and adds/removes the #define constant "TENKOKU_PRESENT".
        /// </summary>
        private static void ValidateTenkokuDefines()
        {
            string[] tentokuAssets = AssetDatabase.FindAssets(kTenkokuAssetStr, null);

            if (tentokuAssets.Length != 0)
            {
#if !TENKOKU_PRESENT
                AddDefine(kTenkokuSymbolStr, _buildTargetPlatforms);
#endif
            }
            else
            {
#if TENKOKU_PRESENT
                RemoveDefine(kTenkokuSymbolStr, _buildTargetPlatforms);
#endif
            }
        }

        /// <summary>
        /// Validates the presence of Massive Clouds Atmos in the project and adds/removes the #define constant "ATMOS_PRESENT".
        /// </summary>
        private static void ValidateAtmosDefines()
        {
            string[] atmosAssets = AssetDatabase.FindAssets(kAtmosAssetStr, null);

            if (atmosAssets.Length != 0)
            {
#if !ATMOS_PRESENT
                AddDefine(kAtmosSymbolStr, _buildTargetPlatforms);
                PackageManager.ImportPackage(Application.dataPath + kAtmosPadPackagePath, false);
#endif
            }
            else
            {
#if ATMOS_PRESENT
                RemoveDefine(kAtmosSymbolStr, _buildTargetPlatforms);
                PackageManager.DeleteDirectory(Application.dataPath + kAtmosPadAssetPath);
#endif
            }
        }

        /// <summary>
        /// Validates the presence of HD Render Pipeline setup in the project and adds/removes the #define constant "UNITY_PIPELINE_HDRP".
        /// </summary>
        private static void ValidateHDRenderPipelineDefines()
        {
            var renderPipelineAsset = UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset;
            if (renderPipelineAsset != null && renderPipelineAsset.GetType().ToString().Contains(kHDRenderPipelineAssetNameStr))
            {
                AddDefine(kHDRenderPipelineSymbolStr, _buildTargetPlatforms);
            }
            else
            {
                RemoveDefine(kHDRenderPipelineSymbolStr, _buildTargetPlatforms);
            }
        }

        /// <summary>
        /// Validates the presence of Expanse in the project and adds/removes the #define constant "EXPANSE_PRESENT".
        /// </summary>
        private static void ValidateExpanseDefines()
        {
            string[] expanseAssets = AssetDatabase.FindAssets(kExpanseAssetStr, null);
            if (expanseAssets.Length != 0)
            {
#if !EXPANSE_PRESENT
                AddDefine(kExpanseSymbolStr, _buildTargetPlatforms);
#endif
            }
            else
            {
#if EXPANSE_PRESENT
                RemoveDefine(kExpanseSymbolStr, _buildTargetPlatforms);
#endif
            }
        }

            #endregion
    }
#endif
        }