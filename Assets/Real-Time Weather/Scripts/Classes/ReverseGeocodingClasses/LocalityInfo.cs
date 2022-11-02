// 
// Copyright(c) 2021 Real-Time Weather ASSIST-Software
// https://www.assist.ro
//
// For information about the licensing and copyright of this software please
// contact assist contact@assist.ro
//

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RealTimeWeather.Geocoding.Classes
{
    /// <summary>
    /// <para>
    /// This class manages the locality information obtained from the Big Data API.
    /// </para>
    /// </summary>
    [Serializable]
    public class LocalityInfo
    {
        #region Constructors
        public LocalityInfo()
        {
            informative = new List<Informative>();
            administrative = new List<Administrative>();
        }
        #endregion

        #region Private Variables
        [SerializeField]
        private List<Administrative> administrative;
        [SerializeField]
        private List<Informative> informative;
        #endregion

        #region Public Properties
        /// <summary>
        /// A list of non-administrative information.
        /// </summary>
        public List<Informative> Informative
        {
            get
            {
                return informative;
            }

            set
            {
                informative = value;
            }
        }

        /// <summary>
        /// A list of administrative information.
        /// </summary>
        public List<Administrative> Administrative
        {
            get
            {
                return administrative;
            }

            set
            {
                administrative = value;
            }
        }
        #endregion

        #region Public Methods       
        /// <summary>
        /// Concatenates the attributes of the LocalityInfo class into a single string.
        /// </summary>
        public override string ToString()
        {
            return ("" +
                   $"{String.Join<Administrative>("\n", administrative)}" +
                   $"{String.Join<Informative>("\n", informative)}");
        }
        #endregion
    }
}