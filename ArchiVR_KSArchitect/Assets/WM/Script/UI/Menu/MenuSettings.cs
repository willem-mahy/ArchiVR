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

        // Use this for initialization
        void Start()
        {
            //m_exitButton.onClick.AddListener(ExitButton_OnClick);
            //m_graphicsSettingsButton.onClick.AddListener(GraphicsSettingsButton_OnClick);
            //m_controlsSettingsButton.onClick.AddListener(ControlsSettingsButton_OnClick);

            /*
            var exit = m_exitButton.GetComponent<FocusableItem>();
            var controls = m_controlsSettingsButton.GetComponent<FocusableItem>();
            var graphics = m_graphicsSettingsButton.GetComponent<FocusableItem>();
           
            // Link up focusable menu item graph.
            exit.SetNeighbourFocusable(FocusableItem.NeighbourDirection.Bottom, controls);
            controls.SetNeighbourFocusable(FocusableItem.NeighbourDirection.Bottom, graphics);
            graphics.SetNeighbourFocusable(FocusableItem.NeighbourDirection.Bottom, exit);

            SetFocusedItem(m_exitButton.GetComponent<FocusableItem>());
            */

            // Set UI Focus to 'Graphics Settings' button when opening the menu.
            SetSelected(m_graphicsSettingsButton);
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