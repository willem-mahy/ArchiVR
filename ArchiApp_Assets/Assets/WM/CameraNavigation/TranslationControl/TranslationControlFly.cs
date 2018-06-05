
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.WM.CameraNavigation.TranslationControl
{
    /*
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
    */

    public class TranslationControlFly : TranslationControlBase
    {
        //ITranslationControlFlyInput m_input = null;

        public bool m_doDebug = false;

        public Text m_textDebug = null;

        public float m_translateSpeedNormal = 1;

        public float m_translateSpeedFast = 2;

        public FirstPersonController m_firstPersonController;

        private void Awake()
        {
        }
                
        void OnDisable()
        {
            Debug.Log("TranslationControlFly.OnDisable()");
        }

        void OnEnable()
        {
            Debug.Log("TranslationControlFly.OnEnable()");
            m_firstPersonController.gameObject.transform.localPosition = Vector3.zero;
            m_firstPersonController.gameObject.transform.localRotation = Quaternion.identity;

            var c = m_firstPersonController.transform.Find("FirstPersonCharacter");
            c.transform.localPosition = Vector3.zero;
            c.transform.localRotation = Quaternion.identity;
        }

        public override void UpdateTranslation(GameObject gameObject)
        {
            float leftRight = CrossPlatformInputManager.GetAxis("Horizontal");
            float forwardBackward = CrossPlatformInputManager.GetAxis("Vertical");
            float upDown = 0;//TODO: CrossPlatformInputManager.GetAxis("UpDown");
            
            Vector3 translationVector = new Vector3(leftRight, upDown, forwardBackward);

            float speed = (
                //m_input.IsFast()
                Input.GetKey(KeyCode.LeftShift) 
                ? m_translateSpeedFast : m_translateSpeedNormal);

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
            Vector3 cameraMovementDirection_World = Camera.main.transform.localToWorldMatrix.MultiplyVector(cameraMovementDirection_Camera);

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

            m_firstPersonController.gameObject.transform.position = m_firstPersonController.gameObject.transform.position + cameraMovement_World;
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
    }
}
