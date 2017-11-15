
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.WM.UI
{
    public class UIManager : MonoBehaviour
    {
        public enum UIMode
        {
            NonVR = 0,
            VR
        }

        //! Reference to the singleton instance.
        private static UIManager s_instance = null;

        //! Get reference to the singleton instance.         
        public static UIManager GetInstance()
        {
            if (null == s_instance)
            {
                Debug.LogWarning("UIManager.GetInstance() = null!");
            }

            return s_instance;
        }

        public Menu m_initialMenu = null;

        //! Reference to the currently shown menu.
        private Menu m_menu;

        public Widget m_widgetDebug = null;

        public Widget m_widgetFPS = null;

        public Widget m_widgetDPad = null;

        // The active UI mode.
        private UIMode m_uiMode = UIMode.NonVR;

        // The 'ui visible' state.
        private bool m_uiVisible = true;

        public string m_toggleUIVisibleKey = "";

        // Flags whether a touch is in progress that can be further considered as a valid trigger tap.
        private bool m_considerTouch = false;
        private float m_touchStartTime;

        // Use this for initialization
        void Awake()
        {
            Debug.Log("UIManager.Awake()");
            s_instance = this;
        }

        // Use this for initialization
        void Start()
        {
            if (m_initialMenu)
            {
                OpenMenu(m_initialMenu);
            }

            UpdateUIState();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_toggleUIVisibleKey != "")
            {
                if (Input.GetKeyUp(m_toggleUIVisibleKey))
                {
                    ToggleUIVisible();
                }
            }

            HandleTouches();
        }

        /// <summary>
        /// Cast a ray to test if Input.mousePosition is over any UI object in EventSystem.current.
        /// This is a replacement for IsPointerOverGameObject(),
        /// which does not work on Android in 4.6.0f3
        /// </summary>
        private bool IsPointerOverUIObject(Touch t)
        {
            // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
            // the ray cast appears to require only eventData.position.
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(t.position.x, t.position.y);

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        void HandleTouches()
        {
            switch (Input.touchCount)
            {
                case 1:
                    {
                        var touch = Input.GetTouch(0);

                        switch (touch.phase)
                        {
                            case TouchPhase.Began:
                                // Only consider touches that are not on UI elements.
                                m_considerTouch = !IsPointerOverUIObject(touch);
                                m_touchStartTime = Time.time;
                                break;
                            case TouchPhase.Ended:
                                {
                                    if (m_considerTouch)
                                    {
                                        var touchEndTime = Time.time;
                                        var touchDuration = touchEndTime - m_touchStartTime;
                                        if (touchDuration < 0.5)
                                        {
                                            // Upon each timely ended single-touch as a 'tap' where there are no UI elements,
                                            // each tap toggles visibility of the targeted UI elements.
                                            ToggleUIVisible();
                                        }
                                        m_considerTouch = false;
                                        m_touchStartTime = 0;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    m_considerTouch = false;
                    m_touchStartTime = 0;
                    break;
            }
        }

        private void OnDestroy()
        {
            Debug.Log("UIManager.OnDestroy()");
            s_instance = null;
        }

        public bool ToggleUIVisible()
        {
            Debug.Log("UIManager.ToggleUIVisible(): -> " + !m_uiVisible);

            m_uiVisible = !m_uiVisible;

            UpdateUIState();

            return m_uiVisible;
        }

        private void UpdateUIState()
        {
            if (m_menu)
            {
                m_menu.UpdateUiControlsActiveState();
            }

            if (m_widgetDebug)
            {
                m_widgetDebug.UpdateUiControlsActiveState();
            }

            if (m_widgetFPS)
            {
                m_widgetFPS.UpdateUiControlsActiveState();
            }

            if (m_widgetDPad)
            {
                m_widgetDPad.UpdateUiControlsActiveState();
            }
        }

        public bool IsUIVisible()
        {
            //Debug.Log("UIManager.IsUIVisible() = " + m_uiVisible);

            return m_uiVisible;
        }

        public void SetUIMode(UIMode uiMode)
        {
            Debug.Log("UIManager.SetUIMode(" + uiMode + ")");

            m_uiMode = uiMode;

            UpdateUIState();
        }

        public UIMode GetUIMode()
        {
            //Debug.Log("UIManager.GetUIMode() = " + m_uiMode);

            return m_uiMode;
        }

        public void OpenMenu(string menuName)
        {
            var menus = gameObject.transform.Find("Menu");

            var menu = menus.transform.Find(menuName).GetComponent<Menu>();

            OpenMenu(menu);
        }

        public void OpenMenu(Menu menu)
        {
            if (null != m_menu)
            {
                m_menu.Close();
            }

            if (menu)
            {
                menu.Open(m_menu);
            }

            m_menu = menu;
        }

        public void CloseMenu()
        {
            if (null == m_menu)
            {
                return;
            }

            var parentMenu = m_menu.Close();
            
            m_menu = parentMenu;

            if (m_menu)
            {
                m_menu.Open();
            }
        }
    }
}
 