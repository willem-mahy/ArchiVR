using UnityEngine;

namespace Assets.Scripts.WM.UI.VR
{
    /* Updates the position of the player-attached menu in VR-mode,
     * to be always on the ground beneath the player, and facing the player viewing direction.
     */
    public class PlayerGazeMenuBehavior : MonoBehaviour
    {
        public Camera m_camera = null;

        public Vector3 m_offsetFromCamera = new Vector3(0, -1.6f, 0);

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            UpdateLocationFromCamera(false);           
        }

        public bool IsCameraLookingDown(float angleDegrees)
        {
            if (null == m_camera)
            {
                return false;
            }
            
            return (m_camera.transform.rotation.eulerAngles.x > angleDegrees);
        }

        public void UpdateLocationFromCamera(bool alwaysUpdateRotation)
        {
            if (null == m_camera)
            {
                return;
            }

            var cameraPosition = m_camera.transform.position;

            gameObject.transform.position = cameraPosition + m_offsetFromCamera;

            // Only adjust the rotation of player gaze menu to follow camera orientation,
            // when not looking down at the menu.
            // This enables the player to gaze at different buttons in the menu.
            if (alwaysUpdateRotation || !IsCameraLookingDown(40.0f))
            {
                UpdateRotationFromCamera();
            }
        }

        private void UpdateRotationFromCamera()
        {
            if (null == m_camera)
            {
                return;
            }

            var cameraRotationEuler = m_camera.transform.rotation.eulerAngles;

            gameObject.transform.rotation = Quaternion.Euler(90, cameraRotationEuler.y, 0);
        }
    }
}