
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.Scripts.WM.CameraNavigation
{
    public class CameraNavigationModeFly : CameraNavigationModeBase
    {
        public float m_translateSpeedNormal = 5;
        public float m_translateSpeedFast = 10;

        public float m_rotateSpeedX = 100;
        public float m_rotateSpeedY = 100;

        public float m_rotationMinX = -89;
        public float m_rotationMaxX =  89;

        private Camera m_camera = null;

        public void Awake()
        {
            m_camera = GetCameraFromFirstPersonCharacter();
        }

        override public void OnEnable()
        {
            Debug.Log("TranslationControlFly.OnEnable()");

            DisableCharacterController();            
        }

        override public void OnDisable()
        {
            Debug.Log("TranslationControlFly.OnDisable()");
        }

        public void Update()
        {
            UpdateTranslation();
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            var eulerOffset = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

            Rotate(eulerOffset);
        }

        private void UpdateTranslation()
        {
            float leftRight = CrossPlatformInputManager.GetAxis("Horizontal");
            float forwardBackward = CrossPlatformInputManager.GetAxis("Vertical");
            float upDown = CrossPlatformInputManager.GetAxis("UpDown");

            if (upDown == 0)
            {
                if (Input.GetKey(KeyCode.PageUp))
                    upDown += 1;

                if (Input.GetKey(KeyCode.PageDown))
                    upDown -= 1;
            }

            Vector3 translationVector = new Vector3(leftRight, upDown, forwardBackward);

            bool fast = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            float speed = (fast ? m_translateSpeedFast : m_translateSpeedNormal);

            float offset = speed * Time.deltaTime;

            translationVector *= offset;

            Vector2 translationVectorXZ = new Vector2(translationVector.x, translationVector.z);
            TranslateXZ(translationVectorXZ, offset, true);

            TranslateY(translationVector.y);
        }

        /*
         * cameraMovementDirXZ_World_Vector2    The translation direction, expressed in the Camera local frame, and encoded in a 2d vector (X=X, Y=Z).
         * offset                               The magnitude of the translation.
         * bool fixY                            Whether to constrain movement to the world XZ plane.
         */
        private void TranslateXZ(
            Vector2 cameraMovementDirXZ_Camera_Vector2,
            float offset,
            bool fixY)
        {
            if (offset == 0)
            {
                return;
            }

            // Put the movement direction into a 3D Vector.
            Vector3 cameraMovementDirection_Camera = new Vector3(cameraMovementDirXZ_Camera_Vector2.x, 0, cameraMovementDirXZ_Camera_Vector2.y);

            // Express the movement direction in the Camera's local frame.
            Vector3 cameraMovementDirection_World = m_camera.transform.localToWorldMatrix.MultiplyVector(cameraMovementDirection_Camera);

            if (fixY)
            {
                // Project movement vector into Camera local frame's XZ plane.
                cameraMovementDirection_World.y = 0;

                if (cameraMovementDirection_World.sqrMagnitude < 0.000001)
                {
                    return; // no movement
                }
            }

            // Normalize the movement dir.
            cameraMovementDirection_World.Normalize();

            // Compute the 3D translation offset with magnitude.
            Vector3 cameraMovement_World = cameraMovementDirection_World * offset;

            m_camera.transform.position = m_camera.transform.position + cameraMovement_World;
        }

        private void TranslateY(
            float offset)
        {
            if (offset == 0)
            {
                return;
            }

            var p = Camera.main.transform.position;
            p.y += offset;
            Camera.main.transform.position = p;
        }
                
        protected void Rotate(Vector3 eulerOffset)
        {
            // Mouse drag over X axis = camera rotation around Y axis.
            eulerOffset.x *= m_rotateSpeedX;
            // Mouse drag over Y axis = camera rotation around X axis.
            eulerOffset.y *= m_rotateSpeedY;

            eulerOffset *= Time.deltaTime;

            if (Input.GetMouseButton(0))
            {
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
        }

        public override void PositionCamera(Vector3 translation, Quaternion rotation)
        {
            m_camera.transform.position = translation;
            m_camera.transform.rotation = rotation;

            // TODO? Offset Gyro orientation so that the 
            /*
            if (gyroEnabled)
            {
                Quaternion cameraRotationFromGyro = RotationControlGyro.GetRotationFromGyro();

                GameObject temp = new GameObject();
                temp.transform.Rotate(cameraRotationFromGyro.eulerAngles);
                Vector3 forwardCameraFromGyro = temp.transform.forward;
                forwardCameraFromGyro.y = 0;

                Vector3 forwardPOI = activePOI.transform.forward;
                forwardPOI.y = 0;

                if ((forwardCameraFromGyro.sqrMagnitude != 0) &&
                        (forwardPOI.sqrMagnitude != 0))
                {
                    forwardCameraFromGyro.Normalize();
                    forwardPOI.Normalize();

                    //m_POINameText.text = "fCam:" + forwardCameraFromGyro.ToString() + " fPOI:" + forwardPOI.ToString();

                    CameraRotateByGyro.m_offsetRotY = (180.0f / Mathf.PI) * Mathf.Acos(Vector3.Dot(forwardPOI, forwardCameraFromGyro));

                    if (Vector3.Cross(forwardPOI, forwardCameraFromGyro).y > 0)
                    {
                        CameraRotateByGyro.m_offsetRotY = -CameraRotateByGyro.m_offsetRotY;
                    }              
                }
            }
            */
        }

        public override bool SupportsDPadInput()
        {
            return true;
        }
    }
}
