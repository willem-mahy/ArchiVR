using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.WM.CameraNavigation.RotationControl
{
    public class RotationControlGyro : RotationControlBase
    {
        public FirstPersonController m_firstPersonController;

        public Camera m_camera = null;

        // TODO: comment
        public float m_offsetRotY = 0;

        // Use this for initialization
        public void Start()
        {
            Debug.Log("RotationControlGyro.Start()");

            if (!SystemInfo.supportsGyroscope)
            {
                Debug.LogWarning("System does not support Gyroscope!");
                return;
            }

            Input.gyro.enabled = true;            
        }

        void OnDisable()
        {
            Debug.Log("RotationControlGyro.OnDisable()");
        }

        void OnEnable()
        {
            Debug.Log("RotationControlGyro.OnEnable()");
            m_firstPersonController.m_UseGyro = true;
        }
        
        override public void UpdateRotation(GameObject gameObject)
        {
            //Debug.Log("WMCameraRotateByGyro.UpdateCameraRotation()");

            Start(); // TODO: call once somewhere else...

            if (!SystemInfo.supportsGyroscope)
            {
                return;
            }

            Quaternion rotation = GetRotationFromGyro();

            if (m_offsetRotY != 0)
            {
                Quaternion r = Quaternion.Euler(0, m_offsetRotY, 0);
                rotation = r * rotation;
            }

            gameObject.transform.rotation = rotation;
        }

        public static Quaternion GetRotationFromGyro()
        {
            Quaternion att = Input.gyro.attitude;
            att.z = -att.z;
            att.w = -att.w;

            Quaternion rot = Quaternion.Euler(new Vector3(90, 0, 0));

            Quaternion rotation = rot * att;

            return rotation;
        }
    }
}
