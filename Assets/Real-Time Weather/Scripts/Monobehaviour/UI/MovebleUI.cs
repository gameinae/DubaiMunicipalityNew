//
// Copyright(c) 2020 Real-Time Weather ASSIST-Software
// https://www.assist.ro
//
// For information about the licensing and copyright of this software please
// contact assist contact@assist.ro
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RealTimeWeather.UI
{
    /// <summary>
    /// THis class allow to drag UI Elements
    /// </summary>
    public class MovebleUI : MonoBehaviour
    {
        private float offsetX, offsetY;

        /// <summary>
        /// This method store screen postions.
        /// </summary>
        public void BeginDrag()
        {
            offsetX = transform.position.x - Input.mousePosition.x;
            offsetY = transform.position.y - Input.mousePosition.y;
        }

        /// <summary>
        /// This function move current element depending on how much mouse has move.
        /// </summary>
        public void OnDrag()
        {
            transform.position = new Vector3(offsetX + Input.mousePosition.x, offsetY + Input.mousePosition.y);
        }

    }
}
