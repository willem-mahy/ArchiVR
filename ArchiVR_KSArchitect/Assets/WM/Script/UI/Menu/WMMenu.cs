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
            var focusedGameObject = GameObject.Find(GUI.GetNameOfFocusedControl());

            var focusedSelectable = focusedGameObject ? focusedGameObject.GetComponent<Selectable>() : null;
            var focusedButton = focusedGameObject ? focusedGameObject.GetComponent<Button>() : null;
            
            // If user starts a tap on GearVR (down event) ...
            if (Input.GetButton("Tap"))
            {
                // ... , perform a click.                
                if (null != focusedGameObject)
                {
                    if (null != focusedSelectable)
                    {
                        focusedSelectable.Select();
                    }

                    if (null != focusedButton)
                    {
                        focusedButton.onClick.Invoke();
                    }
                }

                //Debug.Log("GearVR Tap Down");
                //GameObject myEventSystem = GameObject.Find("EventSystem");
                //myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().;
            }

            // If user stops a tap on GearVR (up) ...
            if (Input.GetButtonUp("Tap"))
            {
                //Debug.Log("GearVR Tap Up");

                // ... NOOP
            }

            if (Input.GetKeyDown(GamepadXBox.X))
            {
                
            }

            if (Input.GetKeyDown(GamepadXBox.Y))
            {
                
            }

            if (Input.GetKeyDown(GamepadXBox.A))
            {
                // ... , perform a click.                
                if (null != focusedGameObject)
                {
                    if (null != focusedSelectable)
                    {
                        focusedSelectable.Select();
                    }

                    if (null != focusedButton)
                    {
                        focusedButton.onClick.Invoke();
                    }
                }
            }

            if (Input.GetKeyDown(GamepadXBox.B))
            {
                
            }

            // If user presses 'gamepad button Y' or 'escape' key, close menu.
            if (Input.GetKeyDown(GamepadXBox.Y) || Input.GetKeyDown(KeyCode.Escape))
            {
                CloseMenu();
            }

            var joystick0Hor = Input.GetAxis("Horizontal");
            var joystick0Vert = Input.GetAxis("Vertical");

            if (joystick0Vert > 0)
            {
                if (null != focusedSelectable)
                {
                    var nextSelectable = focusedSelectable.FindSelectableOnUp();

                    if (null != nextSelectable)
                    {
                        nextSelectable.Select();
                    }
                }
            }

            if (joystick0Vert < 0)
            {
                if (null != focusedSelectable)
                {
                    var nextSelectable = focusedSelectable.FindSelectableOnDown();

                    if (null != nextSelectable)
                    {
                        nextSelectable.Select();
                    }
                }
            }

            if (joystick0Hor > 0)
            {
                if (null != focusedSelectable)
                {
                    var nextSelectable = focusedSelectable.FindSelectableOnRight();

                    if (null != nextSelectable)
                    {
                        nextSelectable.Select();
                    }
                }
            }

            if (joystick0Hor < 0)
            {
                if (null != focusedSelectable)
                {
                    var nextSelectable = focusedSelectable.FindSelectableOnLeft();

                    if (null != nextSelectable)
                    {
                        nextSelectable.Select();
                    }
                }
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

            if (null != cameraNavigation)
            {
                cameraNavigation.EnableTranslation(false);
            }
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
