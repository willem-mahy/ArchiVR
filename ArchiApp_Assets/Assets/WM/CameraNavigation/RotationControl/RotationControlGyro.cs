using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.WM.CameraNavigation.RotationControl
{
    public class RotationControlGyro : RotationControlBase
    {
        public FirstPersonController m_firstPersonController;

        // TODO: comment
        public float m_offsetRotY = 0;

        // Use this for initialization
        public void Start()
        {
            Debug.Log("RotationControlGyro.Start()");
        }

        void OnDisable()
        {
            Debug.Log("RotationControlGyro.OnDisable()");

            Input.gyro.enabled = false;
        }

        void OnEnable()
        {
            Debug.Log("RotationControlGyro.OnEnable()");

            if (!SystemInfo.supportsGyroscope)
            {
                Debug.LogWarning("System does not support Gyroscope!");
                return;
            }

            Input.gyro.enabled = true;

            m_firstPersonController.m_UseGyro = true;
        }
        
        override public void UpdateRotation(GameObject gameObject)
        {
            //Debug.Log("RotationControlGyro.UpdateRotation()");

            /*
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
            */
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
