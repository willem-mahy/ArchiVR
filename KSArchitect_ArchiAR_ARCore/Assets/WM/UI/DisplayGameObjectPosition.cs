using UnityEngine;

namespace WM.UI
{
    public class DisplayGameObjectPosition : MonoBehaviour
    {
        //! The GameObject to show info about.
        public GameObject m_gameObject = null;

        // Update is called once per frame
        void Update()
        {
            var goText = this.gameObject.GetComponent<UnityEngine.UI.Text>();
            goText.text = m_gameObject ? m_gameObject.transform.position.ToString() : "-";
        }
    }
}