using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Assets.ArchiApp.Application.Managers;

namespace Assets.WM.CameraNavigation
{
    abstract public class CameraNavigationModeBase : MonoBehaviour
    {
        public string m_spritePath;

        public FirstPersonController m_firstPersonController;

        public GameObject m_virtualGamepad = null;

        abstract public void PositionCamera(Vector3 translation, Quaternion rotation);

        // Obsolete: to remove.
        // we've come to the conclusion that:
        // ALL camera navigation modes have use for SOME form of Gamepad Input.
        abstract public bool SupportsDPadInput();

        abstract public bool SupportsNavigationViaPOI();

        virtual public void OnEnable()
        {
            Debug.Log("CameraNavigationModeBase.OnEnable()");

            if (m_virtualGamepad)
            {
                m_virtualGamepad.SetActive(true);
            }
        }

        virtual public void OnDisable()
        {
            Debug.Log("CameraNavigationModeBase.OnDisable()");

            if (m_virtualGamepad)
            {
                m_virtualGamepad.SetActive(false);
            }
        }

        protected void DisableCharacterController()
        {
            m_firstPersonController.enabled = false;
            m_firstPersonController.gameObject.GetComponent<CharacterController>().enabled = false;

            // And reset the camera to its default position/orientation.
            m_firstPersonController.gameObject.transform.localPosition = Vector3.zero;
            m_firstPersonController.gameObject.transform.localRotation = Quaternion.identity;

            var firstPersonCharacter = m_firstPersonController.transform.Find("FirstPersonCharacter");
            firstPersonCharacter.transform.localPosition = Vector3.zero;
            firstPersonCharacter.transform.localRotation = Quaternion.identity;

            SetMouseCursorLocked(false);
        }

        protected void EnableCharacterController()
        {
            m_firstPersonController.enabled = true;
            m_firstPersonController.GetComponent<CharacterController>().enabled = true;
        }

        protected void PositionCameraToActivePOI()
        {
            var poiManager = POIManager.GetInstance();
            var activePOI = poiManager.GetActivePOI();

            if (!activePOI)
            {
                return;
            }

            var t = activePOI.transform;

            PositionCamera(
                t.position,
                t.rotation);
        }

        protected Camera GetCameraFromFirstPersonCharacter()
        {
            var firstPersonCharacter = m_firstPersonController.transform.Find("FirstPersonCharacter");

            return firstPersonCharacter.GetComponent<Camera>();
        }

        protected static void SetMouseCursorLocked(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }
    }
}
