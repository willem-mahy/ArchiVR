using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.WM.CameraNavigation
{
    public class CameraNavigationModeTeleport : CameraNavigationModeBase
    {
        // Need to press LMB in order to rotate(true), or rotate always(false).
        public bool m_dragToRotate = false;

        public float m_rotateSpeedX = 100;
        public float m_rotateSpeedY = 100;

        public float m_rotationMinX = -89;
        public float m_rotationMaxX = 89;
        
        private Camera m_camera = null;

        public void Awake()
        {
            m_camera = GetCameraFromFirstPersonCharacter();
        }

        override public void OnEnable()
        {
            base.OnEnable();

            Debug.Log("CameraNavigationModeTeleport.OnEnable()");

            DisableCharacterController();            
        }

        override public void OnDisable()
        {
            base.OnDisable();

            Debug.Log("CameraNavigationModeTeleport.OnDisable()");
        }

        public void Update()
        {
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            // Decide if we need to update the rotation, or not.
            var rotate = (m_dragToRotate ? Input.GetMouseButton(0) : true);
            
            if (!rotate)
            {
                return;
            }

            var eulerOffset = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

            Rotate(eulerOffset);
        }

        protected void Rotate(Vector3 eulerOffset)
        {
            // Mouse drag over X axis = camera rotation around Y axis.
            eulerOffset.x *= m_rotateSpeedX;
            // Mouse drag over Y axis = camera rotation around X axis.
            eulerOffset.y *= m_rotateSpeedY;

            eulerOffset *= Time.deltaTime;

            
            var cameraEulerAngles = m_camera.transform.eulerAngles;

            // Mouse drag over X axis = camera rotation around Y axis.
            cameraEulerAngles.x -= eulerOffset.x;
            // Mouse drag over Y axis = camera rotation around X axis.
            cameraEulerAngles.y += eulerOffset.y;

            cameraEulerAngles.x = WM.Util.Math.FormatAngle180(cameraEulerAngles.x);
            cameraEulerAngles.x = Mathf.Clamp(cameraEulerAngles.x, m_rotationMinX, m_rotationMaxX);

            var rotation = Quaternion.Euler(cameraEulerAngles.x, cameraEulerAngles.y, 0);
            m_camera.transform.rotation = rotation;
        }

        public override void PositionCamera(Vector3 translation, Quaternion rotation)
        {
            m_camera.transform.position = translation;
            m_camera.transform.rotation = rotation;
        }

        public override bool SupportsDPadInput()
        {
            return true;
        }

        public override bool SupportsNavigationViaPOI()
        {
            return true;
        }
    }
}
