//
// Copyright(c) 2021 Real-Time Weather ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at hello@assist.ro
//

using UnityEngine;

namespace RealTimeWeather.Controllers
{
    /// <summary>
    /// This class controls the camera movements: rotation and position.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        #region Private Const Variables
        private const string kMouseXstr = "Mouse X";
        private const string kMouseYstr = "Mouse Y";

        private const int kMinAngle = -30;
        private const int kMaxAngle = 50;
        private const int kZeroValue = 0;
        #endregion

        #region Public Variables
        public float _cameraRotationSpeed = 5f;
        #endregion

        #region Private Variables
        private float _mouseX;
        private float _mouseY;
        #endregion

        #region Unity Methods
        private void Update()
        {
            RotateCamera();
        }

        #endregion

        #region Private Methods

        private void RotateCamera()
        {
            _mouseX += Input.GetAxis(kMouseXstr) * _cameraRotationSpeed;
            _mouseY -= Input.GetAxis(kMouseYstr) * _cameraRotationSpeed;
            _mouseY = Mathf.Clamp(_mouseY, kMinAngle, kMaxAngle);
            transform.rotation = Quaternion.Euler(_mouseY, _mouseX, kZeroValue);
        }

        #endregion
    }
}