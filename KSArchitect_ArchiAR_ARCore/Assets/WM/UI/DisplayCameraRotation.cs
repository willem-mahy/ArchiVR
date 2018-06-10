using UnityEngine;

namespace WM.UI
{
    public class DisplayCameraRotation : MonoBehaviour
    {
        //! The camera to show info about.  If not set, info for Main camera is shown.
        public Camera m_camera = null;

        // Update is called once per frame
        void Update()
        {
            var camera = m_camera ? m_camera : Camera.main;

            var goText = this.gameObject.GetComponent<UnityEngine.UI.Text>();
            goText.text = camera.transform.rotation.eulerAngles.ToString();
        }
    }
}