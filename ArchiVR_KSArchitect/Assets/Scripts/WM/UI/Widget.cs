using Assets.Scripts.WM.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WM.UI
{
    public class Widget : MonoBehaviour
    {
        public bool m_visible = true;

        public GameObject m_uiControlNonVR = null;
        public GameObject m_uiControlVR = null;

        // Use this for initialization
        public bool ToggleVisible()
        {
            m_visible = !m_visible;
            UpdateUiControlsActiveState();
            return m_visible;
        }

        // Use this for initialization
        public void SetVisible(bool visible)
        {
            Debug.Log("Widget(" + name + ").SetVisible(" + visible + ")");
            m_visible = visible;
            UpdateUiControlsActiveState();
        }

        public void UpdateUiControlsActiveState()
        {
            var uiManager = UIManager.GetInstance();

            // Always hide the control for non-active UI mode.
            var uiControlForNonActiveMode = GetControlForNonActiveMode();

            if (uiControlForNonActiveMode)
            {
                uiControlForNonActiveMode.SetActive(false);
            }

            // Show/hide the control for active UI mode depending on whether the UI is set visible in UIManager.
            var uiControlForActiveMode = GetControlForActiveMode();

            if (uiControlForActiveMode)
            {
                uiControlForActiveMode.SetActive(m_visible && uiManager.IsUIVisible());
            }
        }

        private GameObject GetControlForActiveMode()
        {
            var uiMode = UIManager.GetInstance().GetUIMode();

            switch (uiMode)
            {
                case UIManager.UIMode.NonVR:
                    return m_uiControlNonVR;
                case UIManager.UIMode.VR:
                    return m_uiControlVR;
                default:
                    Debug.LogWarning("Unsupported UI Mode! (" + uiMode + ")");
                    return null;
            }
        }

        private GameObject GetControlForNonActiveMode()
        {
            var uiMode = UIManager.GetInstance().GetUIMode();

            switch (uiMode)
            {
                case UIManager.UIMode.NonVR:
                    return m_uiControlVR;
                case UIManager.UIMode.VR:
                    return m_uiControlNonVR;
                default:
                    Debug.LogWarning("Unsupported UI Mode! (" + uiMode + ")");
                    return null;
            }
        }
    }
}