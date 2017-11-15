using Assets.Scripts.WM.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WM.ArchiVR.Menu
{
    public class MenuSettings : MonoBehaviour
    {
        //! The button to close this menu.
        public Button m_exitButton = null;

        //! The button to open the 'Graphics Settings' submenu.
        public Button m_graphicsSettingsButton = null;

        //! The button to open the 'Controls Settings' submenu.
        public Button m_controlsSettingsButton = null;

        // Use this for initialization
        void Start()
        {            
            m_exitButton.onClick.AddListener(ExitButton_OnClick);
            m_graphicsSettingsButton.onClick.AddListener(GraphicsSettingsButton_OnClick);
            m_controlsSettingsButton.onClick.AddListener(ControlsSettingsButton_OnClick);
        }

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