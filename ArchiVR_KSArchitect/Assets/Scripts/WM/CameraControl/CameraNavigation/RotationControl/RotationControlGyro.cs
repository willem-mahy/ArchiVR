using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WM.CameraControl.CameraNavigation.RotationControl
{
    public class RotationCOntrolGyro : IRotationControl
    {
        // TODO: comment
        public float m_offsetRotY = 0;

        // Use this for initialization
        public void Start()
        {
            Debug.Log("RotationCOntrolGyro.Start()");

            if (!SystemInfo.supportsGyroscope)
                return;

            Input.gyro.enabled = true;
        }

        public void UpdateRotation(GameObject gameObject)
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
