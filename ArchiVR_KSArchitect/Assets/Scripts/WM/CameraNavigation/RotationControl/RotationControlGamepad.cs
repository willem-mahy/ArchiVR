using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.WM.CameraNavigation.RotationControl
{
    public class RotationControlGamepad : RotationControlBase
    {
        public float m_xSpeed = 100;
        public float m_ySpeed = 100;

        public float m_xRotMin = -89;
        public float m_xRotMax = 89;

        public FirstPersonController m_firstPersonController;
        public Camera m_camera;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnDisable()
        {
            Debug.Log("RotationControlGamepad.OnDisable()");
        }

        void OnEnable()
        {
            Debug.Log("RotationControlGamepad.OnEnable()");

            if (null != m_firstPersonController)
            {
                m_firstPersonController.m_UseGyro = false;
            }
        }

        public override void UpdateRotation(GameObject gameObject)
        {
            if (null != m_camera)
            {
                if (Input.GetMouseButton(0))
                {
                    var cameraEulerAngles = m_camera.transform.eulerAngles;

                    // Mouse drag over X axis = camera rotation around Y axis.
                    cameraEulerAngles.x -= Input.GetAxis("Vertical") * m_xSpeed * Time.deltaTime;
                    // Mouse drag over Y axis = camera rotation around X axis.
                    cameraEulerAngles.y += Input.GetAxis("Horizontal") * m_ySpeed * Time.deltaTime;

                    cameraEulerAngles.x = Assets.Scripts.WM.Util.Math.FormatAngle180(cameraEulerAngles.x);
                    cameraEulerAngles.x = Mathf.Clamp(cameraEulerAngles.x, m_xRotMin, m_xRotMax);

                    var rotation = Quaternion.Euler(cameraEulerAngles.x, cameraEulerAngles.y, 0);
                    m_camera.transform.rotation = rotation;
                }
            }
        }
    }
}
