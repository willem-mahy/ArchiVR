using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WM.CameraControl.CameraNavigation.RotationControl
{
    public class RotationControlMouse : IRotationControl
    {
        public float m_xSpeed = 100;
        public float m_ySpeed = 100;

        public float m_xRotMin = -89;
        public float m_xRotMax = 89;

        private Vector3 m_lastMousePosition = Vector3.zero;
        private bool m_lastMousePositionSet = false;
        
        public void UpdateRotation(GameObject gameObject)
        {
            if (Input.mousePresent)
            {
                if (!m_lastMousePositionSet)
                {
                    m_lastMousePosition = Input.mousePosition;
                    m_lastMousePositionSet = true;
                }

                Vector3 delta = Input.mousePosition - m_lastMousePosition;

                Vector3 rotation = 3.0f * delta;

                Rotate(gameObject, rotation);

                m_lastMousePosition = Input.mousePosition;
            }
        }
        
        protected void Rotate(
            GameObject gameObject,
            Vector3 eulerOffset)
        {
            // Mouse drag over X axis = camera rotation around Y axis.
            eulerOffset.x *= m_xSpeed * Time.deltaTime;
            // Mouse drag over Y axis = camera rotation around X axis.
            eulerOffset.y *= m_ySpeed * Time.deltaTime;

            if (Input.GetMouseButton(0))
            {
                var cameraEulerAngles = gameObject.transform.eulerAngles;

                // Mouse drag over X axis = camera rotation around Y axis.
                cameraEulerAngles.x -= Input.GetAxis("Mouse Y") * m_xSpeed * Time.deltaTime;
                // Mouse drag over Y axis = camera rotation around X axis.
                cameraEulerAngles.y += Input.GetAxis("Mouse X") * m_ySpeed * Time.deltaTime;

                cameraEulerAngles.x = Assets.Scripts.WM.Util.Math.FormatAngle180(cameraEulerAngles.x);
                cameraEulerAngles.x = Mathf.Clamp(cameraEulerAngles.x, m_xRotMin, m_xRotMax);

                var rotation = Quaternion.Euler(cameraEulerAngles.x, cameraEulerAngles.y, 0);
                gameObject.transform.rotation = rotation;
            }
        }
    }
}
