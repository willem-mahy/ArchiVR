using UnityEngine;
using VRStandardAssets.Utils;

namespace Assets.Scripts.WM.UI.VR
{
    /* Updates the position of the player-attached menu in VR-mode,
     * to be always on the ground beneath the player, and facing the player viewing direction.
     */
    public class PlayerGazeMenuBehavior : MonoBehaviour
    {
        public Camera m_camera = null;

        public GameObject m_fixationArea = null;

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
            if (alwaysUpdateRotation || !IsOverFixationArea())
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

            gameObject.transform.rotation = Quaternion.Euler(0, cameraRotationEuler.y, 0);
        }

        public bool IsOverFixationArea()
        {
            if (null == m_fixationArea)
            {
                return false;
            }

            var collider = m_fixationArea.GetComponent<Collider>();

            if (null == collider)
            {
                return false;
            }

            //if (null == collider.bounds)
            //{
            //    return false;
            //}

            if (null == m_camera)
            {
                return false;
            }

            Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);

            bool isOver = collider.bounds.IntersectRay(ray);

            return isOver;
        }
    }
}