using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.WM.CameraNavigation
{
    abstract public class CameraNavigationModeBase : MonoBehaviour
    {
        public string m_spritePath;

        public FirstPersonController m_firstPersonController;

        abstract public void PositionCamera(Vector3 translation, Quaternion rotation);

        abstract public void OnEnable();

        abstract public void OnDisable();

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
