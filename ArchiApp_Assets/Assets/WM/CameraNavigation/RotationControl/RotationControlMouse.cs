using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.WM.CameraNavigation.RotationControl
{
    public class RotationControlMouse : RotationControlBase
    {
        public float m_xSpeed = 100;
        public float m_ySpeed = 100;

        public float m_xRotMin = -89;
        public float m_xRotMax = 89;

        public FirstPersonController m_firstPersonController;        

        void OnDisable()
        {
            Debug.Log("RotationControlMouse.OnDisable()");
        }

        void OnEnable()
        {
            Debug.Log("RotationControlMouse.OnEnable()");

            if (null != m_firstPersonController)
            {
                m_firstPersonController.m_UseGyro = false;
            }
        }

        public override void UpdateRotation(GameObject gameObject)
        {
        }
    }
}
