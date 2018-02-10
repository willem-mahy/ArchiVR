using Assets.Scripts.WM;
using Assets.Scripts.WM.CameraNavigation;
using Assets.Scripts.WM.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.WM.Script.UI.Menu
{
    public class WMMenu : MonoBehaviour
    {
        public FirstPersonController m_firstPersonController = null;

        // The UI control that will receive UI focus when opening the menu.
        public Selectable m_defaultSelectedItem = null;

        //! The button to close this menu.
        public Button m_exitButton = null;

        protected bool m_enableTranslation = false;

        public void Start()
        {
            EnableCameraNavigationInputMouseKB(false);

            if (null != m_exitButton)
            {
                m_exitButton.onClick.AddListener(ExitButton_OnClick);
            }

            // Set UI focus to the 'default selected item' when opening the menu.
            if (null != m_defaultSelectedItem)
            {
                SetSelected(m_defaultSelectedItem);
            }
        }

        public void OnEnable()
        {
            EnableCameraNavigationInputMouseKB(false);

            // Set UI focus to the 'default selected item' when opening the menu.
            if (null != m_defaultSelectedItem)
            {
                SetSelected(m_defaultSelectedItem);
            }
        }

        public void OnDisable()
        {
            EnableCameraNavigationInputMouseKB(true);
        }

        public void Update()
        {
            ProcessInput();
        }

        virtual protected void ProcessInput()
        {
            if (Input.GetKeyDown(GamepadXBox.X))
            {
                
            }

            if (Input.GetKeyDown(GamepadXBox.B))
            {
                
            }

            // If user presses 'gamepad button Y' or 'escape' key, close menu.
            if (Input.GetKeyDown(GamepadXBox.Y) || Input.GetKeyDown(KeyCode.Escape))
            {
                CloseMenu();
            }

            bool e =
                Input.GetKey(KeyCode.RightControl)
                || Input.GetKey(KeyCode.LeftControl);

            if (m_enableTranslation)
            {
                e = true;
            }

            EnableCameraNavigationInputMouseKB(e);
        }

        public void CloseMenu()
        {
            Debug.Log("WMMenu.CloseMenu()");

            UIManager.GetInstance().CloseMenu();
        }

        void ExitButton_OnClick()
        {
            Debug.Log("WMMenu(" + gameObject.name + ").ExitButton_OnClick()");
            UIManager.GetInstance().CloseMenu();
        }

        //! Enable or disable the FPS controller.
        // (TODO: disable amera navigation as a whole!)
        // Used to prevent player from moving around
        // while navigating through menus using the arrow keys.
        private void EnableCameraNavigationInputMouseKB(bool state)
        {
            if (null == m_firstPersonController)
            {
                return;
            }

            //m_firstPersonController.enabled = state;

            var cameraNavigation = CameraNavigation.GetInstance();
            cameraNavigation.EnableTranslation(false);
        }

        //! Set UI focus to the given selectable.
        protected void SetSelected(Selectable s)
        {
            if (null == s)
            {
                return;
            }

            s.interactable = true;
            s.Select();
            EventSystem.current.SetSelectedGameObject(s.gameObject, null);
        }
    }
}
