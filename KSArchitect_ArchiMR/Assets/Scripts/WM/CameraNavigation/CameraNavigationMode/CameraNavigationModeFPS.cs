
using Assets.Scripts.WM.Settings;
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

        // Use this for initialization
        void Update()
        {
            var cn = CameraNavigation.GetInstance();

            if (cn)
            {
                if (m_firstPersonController)
                {
                    m_firstPersonController.m_enableTranslation = cn.m_enableTranslation;
                }
            }
        }

        override public void OnEnable()
        {
            base.OnEnable();

            Debug.Log("CameraNavigationModeFPS.OnEnable()");

            m_firstPersonController.m_MouseLook.lockCursor = true;

            m_firstPersonController.m_WalkSpeed = 1;
            m_firstPersonController.m_RunSpeed = 10;
            m_firstPersonController.m_GravityMultiplier = 2;
            m_firstPersonController.m_enableJump = true;

            // When entering first-person Camera navigation,
            // Do not lock the mouse cursor when Virtual Gamepad is shown.
            // Locking the mouse cursor makes Virtual gamepad behavior act jerky.
            var enableVirtualGamepad = false;

            var appset = ApplicationSettings.GetInstance();
            if (appset)
            {
                enableVirtualGamepad = appset.m_data.m_controlSettings.m_enableVirtualGamepad;
            }

            m_firstPersonController.m_MouseLook.lockCursor = !enableVirtualGamepad;

            m_firstPersonController.m_UseGyro = SystemInfo.supportsGyroscope;

            EnableCharacterController();

            var poiManager = POIManager.GetInstance();

            if (poiManager)
            {
                var poiCollection = GameObject.Find("/World/Construction/Phases/Final/POI/FPS");
                poiManager.SetPOICollection(poiCollection);
            }
        }

        override public void OnDisable()
        {
            base.OnDisable();

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
