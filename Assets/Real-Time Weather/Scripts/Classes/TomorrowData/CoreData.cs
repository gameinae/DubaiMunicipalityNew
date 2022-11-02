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

namespace RealTimeWeather.Tomorrow.Classes
{
    /// <summary>
    /// <para>
    /// This class manages Tomorrow.io API core data.
    /// </para>
    /// </summary>
    [Serializable]
    public class CoreData
    {
        #region Constructors
        public CoreData()
        {
            timelines = new List<TimeData>();
        }

        public CoreData(List<TimeData> timelines)
        {
            this.timelines = timelines;
        }
        #endregion

        #region Private Variables
        [SerializeField]
        private List<TimeData> timelines;
        #endregion

        #region Public Properties
        /// <summary>
        /// <para>
        /// A list of TimeData instances.
        /// </para>
        /// </summary>
        public List<TimeData> TimelinesList
        {
            get
            {
                return timelines;
            }

            set
            {
                timelines = value;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Concatenates the attributes of the CoreData class into a single string.
        /// </summary>
        public override string ToString()
        {
            return ($"{String.Join<TimeData>("\n", timelines)}\n");
        }
        #endregion
    }
}