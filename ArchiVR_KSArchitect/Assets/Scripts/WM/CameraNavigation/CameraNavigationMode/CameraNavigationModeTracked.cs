using System;
using UnityEngine;

namespace Assets.Scripts.WM.CameraNavigation
{
    public class CameraNavigationModeTracked : CameraNavigationModeBase
    {
        private Camera m_camera = null;

        public void Awake()
        {
            m_camera = GetCameraFromFirstPersonCharacter();
        }

        override public void OnEnable()
        {
            Debug.Log("CameraNavigationModeTracked.OnEnable()");

            DisableCharacterController();
        }

        override public void OnDisable()
        {
            Debug.Log("CameraNavigationModeTracked.OnDisable()");
        }

        public override void PositionCamera(Vector3 translation, Quaternion rotation)
        {
            m_camera.transform.position = translation;
            m_camera.transform.rotation = rotation;
        }
    }
}
