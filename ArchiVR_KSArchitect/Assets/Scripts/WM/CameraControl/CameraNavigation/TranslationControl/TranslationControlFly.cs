using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.WM.Util;

namespace Assets.Scripts.WM.CameraControl.CameraNavigation.TranslationControl
{
    public interface ITranslationControlFlyInput
    {
        Vector3 GetTranslationVector();
        bool IsFast();
    }

    public class TranslationControlFlyInputKB : ITranslationControlFlyInput
    {
        public Vector3 GetTranslationVector()
        {
            var translationVector = Vector3.zero;

            if (Input.GetKey("up")) // Forward
            {
                translationVector += Vector3.forward;
            }

            if (Input.GetKey("down")) // Backward
            {
                translationVector += Vector3.back;
            }

            if (Input.GetKey("left")) // Left
            {
                translationVector += Vector3.left;
            }

            if (Input.GetKey("right")) // Right
            {
                translationVector += Vector3.right;
            }
            
            if (Input.GetKey("u"))
            {
                translationVector += Vector3.up;
            }

            if (Input.GetKey("d"))
            {
                translationVector += Vector3.down;
            }

            return translationVector;
        }

        public bool IsFast()
        {
            return Input.GetKey("right shift");
        }
    }

    public class TranslationControlFlyInputGamepad : ITranslationControlFlyInput
    {
        // Physical gamepad buttons are mapped as follows:
        // A = 0
        // B = 1
        // X = 2
        // Y = 3
        private static string Fast = "joystick button 4";
        private static string Down = "joystick button 0";
        private static string Up = "joystick button 1";

        public Vector3 GetTranslationVector()
        {
            var translationVector = Vector3.zero;

            translationVector+= Vector3.forward * Input.GetAxis("Horizontal");
            translationVector+= Vector3.right * Input.GetAxis("Vertical");

            if (Input.GetKey(Down))
            {
                translationVector += Vector3.down;
            }

            if (Input.GetKey(Up))
            {
                translationVector += Vector3.up;
            }

            return translationVector;
        }

        public bool IsFast()
        {
            return Input.GetKey(Fast);
        }
    }

    public class TranslationControlGearVRTrackpad : ITranslationControlFlyInput
    {
        public Vector3 GetTranslationVector()
        {
            var translationVector = Vector3.zero;

            if (Input.GetMouseButton(0))
            {
                translationVector += Vector3.forward;
            }

            return translationVector;
        }

        public bool IsFast()
        {
            return false;
        }
    }

    public class TranslationControlFly : ITranslationControl
    {
        ITranslationControlFlyInput m_input = null;

        static float xSpeed = 100.0f;
        static float ySpeed = 100.0f;

        static float xRotMin = -90.0f;
        static float xRotMax = 90;

        public bool m_doDebug = false;

        public Text m_textDebug = null;

        //! The camera to be controlled.
        public Camera m_camera = null;

        //! The normal translation speed, in units/sec.
        public float m_translateSpeedNormal = 20.0f;

        //! The normal translation speed, in units/sec.
        public float m_translateSpeedFast = 50.0f;

        public void UpdateTranslation(GameObject gameObject)
        {
            if (null == m_input)
            {
                Debug.LogWarning("m_input == null!");
                m_input = new TranslationControlFlyInputKB();
                return;
            }

            Vector3 translationVector = m_input.GetTranslationVector();

            Vector2 translationVectorHorizontal = new Vector2(translationVector.x, translationVector.y);

            float speed = (m_input.IsFast() ? m_translateSpeedNormal : m_translateSpeedFast);

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

                if (cameraMovementDirection_World.sqrMagnitude < 0.01)
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

            var p = m_camera.transform.position;
            p.y += offset;
            m_camera.transform.position = p;
        }

        protected void Rotate(Vector3 eulerOffset)
        {
            // Mouse drag over X axis = camera rotation around Y axis.
            eulerOffset.x *= xSpeed * Time.deltaTime;
            // Mouse drag over Y axis = camera rotation around X axis.
            eulerOffset.y *= ySpeed * Time.deltaTime;

            if (Input.GetMouseButton(0))
            {
                var cameraEulerAngles = m_camera.transform.eulerAngles;

                // Mouse drag over X axis = camera rotation around Y axis.
                cameraEulerAngles.x -= Input.GetAxis("Mouse Y") * xSpeed * Time.deltaTime;
                // Mouse drag over Y axis = camera rotation around X axis.
                cameraEulerAngles.y += Input.GetAxis("Mouse X") * ySpeed * Time.deltaTime;

                cameraEulerAngles.x = Math.FormatAngle180(cameraEulerAngles.x);
                cameraEulerAngles.x = Mathf.Clamp(cameraEulerAngles.x, xRotMin, xRotMax);

                var rotation = Quaternion.Euler(cameraEulerAngles.x, cameraEulerAngles.y, 0);
                m_camera.transform.rotation = rotation;
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
