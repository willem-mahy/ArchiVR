using UnityEngine;
using UnityEngine.UI;

namespace Assets.WM.UI
{
    public class CheckBox : MonoBehaviour
    {
        public Button m_button = null;

        private bool m_checked = false;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public bool ToggleCheckedState(bool state)
        {
            m_checked = !m_checked;
            UpdateCaption();
            return m_checked;
        }

        public void SetCheckedState(bool state)
        {
            m_checked = state;
            UpdateCaption();
        }

        public bool IsChecked()
        {
            return m_checked;
        }

        private void UpdateCaption()
        {
            m_button.GetComponentInChildren<Text>().text = m_checked ? "V" : "";
        }
    }
}