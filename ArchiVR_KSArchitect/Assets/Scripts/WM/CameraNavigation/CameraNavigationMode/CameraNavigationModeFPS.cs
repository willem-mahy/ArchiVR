
using UnityEngine;

namespace Assets.Scripts.WM.CameraNavigation
{
    public class CameraNavigationModeFPS : CameraNavigationModeBase
    {
        public abstract class ICameraNavigationFPSTranslationInput
        {
            abstract public Vector3 GetTranslationVector();
            abstract public bool GetCrouch();
            abstract public bool GetJump();
            abstract public bool GetFastMovement();
        }

        // Use this for initialization
        void Start()
        {
        }

        override public void OnEnable()
        {
            Debug.Log("CameraNavigationModeFPS.OnEnable()");

            m_firstPersonController.m_MouseLook.lockCursor = true;

            m_firstPersonController.m_WalkSpeed = 5;
            m_firstPersonController.m_RunSpeed = 10;
            m_firstPersonController.m_GravityMultiplier = 2;
            m_firstPersonController.m_enableJump = true;

            m_firstPersonController.m_MouseLook.lockCursor = true;

            m_firstPersonController.m_UseGyro = SystemInfo.supportsGyroscope;

            EnableCharacterController();
        }

        override public void OnDisable()
        {
            Debug.Log("CameraNavigationModeFPS.OnDisable()");
        }

        public override void PositionCamera(Vector3 translation, Quaternion rotation)
        {
            m_firstPersonController.transform.position = translation;

            m_firstPersonController.transform.rotation = Quaternion.Euler(new Vector3(0, rotation.eulerAngles.y, 0));

            var firstPersonCharacter = m_firstPersonController.transform.Find("FirstPersonCharacter");
            
            if (null != m_firstPersonController.m_MouseLook)
            {
                m_firstPersonController.m_MouseLook.m_CharacterTargetRot.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
                m_firstPersonController.m_MouseLook.m_CameraTargetRot.eulerAngles = new Vector3(rotation.eulerAngles.x, 0, 0);
            }
            else
            {
                m_firstPersonController.transform.rotation = Quaternion.Euler(new Vector3(0, rotation.eulerAngles.y, 0));
                firstPersonCharacter.transform.rotation = Quaternion.Euler(new Vector3(rotation.eulerAngles.x, 0, 0));
            }
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
