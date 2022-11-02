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

using RealTimeWeather.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace RealTimeWeather.Simulation.Enviro
{
    /// <summary>
    /// This class manages Enviro's weather presets.
    /// </summary>
    public class EnviroModulePresetsManager : MonoBehaviour
    {
        #region Private Const Variables
        private const string kRainAndSnowPresetName = "Rain And Snow";
        private const string kEnviroLightRainPresetName = "Light Rain";
        private const string kEnviroLightSnowPrefabName = "Light Snow HQ";
        #endregion

        #region Private Variables
#if ENVIRO_PRESENT
        private List<EnviroWeatherPreset> _presets;
        private EnviroWeatherPreset _rainAndSnowPreset;
#endif
        #endregion

        #region Public Properties
#if ENVIRO_PRESENT
        public List<EnviroWeatherPreset> EnviroPresetsList
        {
            get { return _presets; }
            set { _presets = value; }
        }
#endif
        #endregion

        #region Unity Methods
        #endregion

        #region Public Methods
        /// <summary>
        /// Initializes Enviro weather presets.
        /// </summary>
        public void IntiliazileEnviroPresets()
        {
#if ENVIRO_PRESENT
            if (EnviroSkyMgr.instance == null)
            {
                return;
            }

            EnviroZone activeZone = EnviroSkyMgr.instance.GetCurrentActiveZone();
            _presets = activeZone.zoneWeatherPresets;

            EnviroWeatherEffects snowEffect = new EnviroWeatherEffects();
            snowEffect.prefab = RealTimeWeatherManager.GetPrefab(kEnviroLightSnowPrefabName);
            EnviroWeatherPreset rainPreset = GetEnviroPreset(kEnviroLightRainPresetName);

            if (rainPreset)
            {
                _rainAndSnowPreset = Instantiate(rainPreset);
                _rainAndSnowPreset.Name = kRainAndSnowPresetName;
                _rainAndSnowPreset.name = kRainAndSnowPresetName;
                _rainAndSnowPreset.winter = true;
                _rainAndSnowPreset.effectSystems.Add(snowEffect);
                activeZone.zoneWeatherPresets.Add(_rainAndSnowPreset);
            }
#endif
        }
        #endregion

        #region Private Methods
#if ENVIRO_PRESENT
        /// <summary>
        /// Finds and returns an Enviro weather preset by name.
        /// </summary>
        /// <param name="presetName">A string value that represents the preset name.</param>
        private EnviroWeatherPreset GetEnviroPreset(string presetName)
        {
            EnviroWeatherPreset preset = ScriptableObject.CreateInstance<EnviroWeatherPreset>();

            if (_presets.Count > 0)
            {
                preset = _presets.Find(i => i.Name == presetName);
            }

            return preset;
        }
#endif
        #endregion
    }
}