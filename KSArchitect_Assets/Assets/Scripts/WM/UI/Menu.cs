using Assets.Scripts.WM.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WM.UI
{
    public class Menu : MonoBehaviour
    {
        private Menu m_parentMenu;

        public GameObject m_uiControlNonVR = null;
        public GameObject m_uiControlVR = null;

        // Use this for initialization
        public Menu Close()
        {
            SetControlsActiveState(false);
            return m_parentMenu;
        }

        //! Called when opening the menu from a parent menu.
        public void Open(Menu parentMenu)
        {
            m_parentMenu = parentMenu;
            Open();
        }

        //! Called when reopening the menu, after closing a child menu.
        public void Open()
        {
            UpdateUiControlsActiveState();
        }

        void SetControlsActiveState(bool state)
        {
            if (m_uiControlNonVR)
                m_uiControlNonVR.SetActive(state);

            if (m_uiControlVR)
                m_uiControlVR.SetActive(state);
        }

        public Menu GetParentMenu()
        {
            return m_parentMenu;
        }

        public void UpdateUiControlsActiveState()
        {
            var uiManager = UIManager.GetInstance();

            if (null == uiManager)
            {
                return;
            }

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
                uiControlForActiveMode.SetActive(uiManager.IsUIVisible());
            }

            // HACK: TODO: implement in a cleaner way...
            /*
            if (m_uiVisible)
            {
                var menu_ground_VR = GameObject.Find("CanvasMainMenu_VR");

                if (null != menu_ground_VR)
                {
                    // While it was disabled, the VR ground menu did not update its position.
                    // So update it once now just before setting it visible, in order to put it conveniently in front of the player.
                    var c = menu_ground_VR.GetComponent<PlayerGazeMenuBehavior>();

                    if (null != c)
                    {
                        c.UpdateLocationFromCamera(true);
                    }
                }
            }
            */
        }

        private GameObject GetControlForActiveMode()
        {
            var uiMode = UIManager.GetInstance().GetUIMode();

            switch (uiMode)
            {
                case UIMode.ScreenSpace:
                    return m_uiControlNonVR;
                case UIMode.WorldSpace:
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
                case UIMode.ScreenSpace:
                    return m_uiControlVR;
                case UIMode.WorldSpace:
                    return m_uiControlNonVR;
                default:
                    Debug.LogWarning("Unsupported UI Mode! (" + uiMode + ")");
                    return null;
            }
        }
    }
}