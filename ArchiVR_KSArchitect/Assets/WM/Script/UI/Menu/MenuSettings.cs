using Assets.Scripts.WM.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.WM.Script.UI.Menu
{
    public class MenuSettings : WMMenu
    {
        //! The button to close this menu.
        public Button m_exitButton = null;

        //! The button to open the 'Graphics Settings' submenu.
        public Button m_graphicsSettingsButton = null;

        //! The button to open the 'Controls Settings' submenu.
        public Button m_controlsSettingsButton = null;

        void ExitButton_OnClick()
        {
            Debug.Log("MenuSettings.ExitButton_OnClick()");
            UIManager.GetInstance().CloseMenu();
        }

        void GraphicsSettingsButton_OnClick()
        {
            Debug.Log("MenuSettings.GraphicsSettingsButton_OnClick()");
            UIManager.GetInstance().OpenMenu("MenuGraphicsSettings");
        }

        void ControlsSettingsButton_OnClick()
        {
            Debug.Log("MenuSettings.ControlsSettingsButton_OnClick()");
            UIManager.GetInstance().OpenMenu("MenuControlsSettings");
        }
    }
}