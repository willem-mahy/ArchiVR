using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.WM.CameraControl.CameraNavigation.RotationControl;
using Assets.Scripts.WM.CameraControl.CameraNavigation.TranslationControl;

namespace Assets.Scripts.WM.CameraControl.CameraNavigation
{
    public class CameraNavigation : MonoBehaviour
    {
        public Camera m_camera = null;

        public IRotationControl m_rotationControl = null;
        public ITranslationControl m_translationControl = null;

        // Use this for initialization
        void Awake()
        {
            if (null == m_camera)
            {
                Debug.LogWarning("m_camera == null!");
            }
        }

        public void Update()
        {
            if (null != m_rotationControl)
            {
                m_rotationControl.UpdateRotation(m_camera.gameObject);
            }

            if (null != m_translationControl)
            {
                m_translationControl.UpdateTranslation(m_camera.gameObject);
            }

        }
    }
}
