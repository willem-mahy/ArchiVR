using Assets.WM.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.WM.Script.UI.Menu
{
    public class MenuSettings : WMMenu
    {
        //! The button to open the 'Graphics Settings' submenu.
        public Button m_graphicsSettingsButton = null;

        //! The button to open the 'Controls Settings' submenu.
        public Button m_controlsSettingsButton = null;

        public new void Start()
        {
            base.Start();

            m_controlsSettingsButton.onClick.AddListener(ControlsSettingsButton_OnClick);
            m_graphicsSettingsButton.onClick.AddListener(GraphicsSettingsButton_OnClick);
        }

        void GraphicsSettingsButton_OnClick()
        {
            Debug.Log("MenuSettings.GraphicsSettingsButton_OnClick()");

            //UIManager.GetInstance().OpenMenu(GameObject.Find("MenuGraphicsSettings").GetComponent<Assets.WM.UI.Menu>());
            // //UIManager.GetInstance().OpenMenu("MenuGraphicsSettings");
        }

        void ControlsSettingsButton_OnClick()
        {
            Debug.Log("MenuSettings.ControlsSettingsButton_OnClick()");

            //UIManager.GetInstance().OpenMenu(GameObject.Find("MenuControlsSettings").GetComponent<Assets.WM.UI.Menu>());
            // //UIManager.GetInstance().OpenMenu("MenuControlsSettings");
        }
    }
}