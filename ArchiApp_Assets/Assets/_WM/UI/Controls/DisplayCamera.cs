using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WM.UI
{
    // Add to a UnityEngine.UI.Text gameobject,
    // in order to show the camera position and orientation.
    public class DisplayCamera : MonoBehaviour
    {
        public Camera m_camera = null;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (!m_camera)
            {
                return;
            }

            gameObject.GetComponent<Text>().text =
                "Debug Camera" +
                "\nPosition: " + m_camera.transform.position +
                "\nRotation: " + m_camera.transform.rotation.eulerAngles;
        }
    }
}