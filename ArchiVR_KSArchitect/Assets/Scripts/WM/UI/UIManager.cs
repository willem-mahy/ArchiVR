using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.WM.UI.VR;

namespace Assets.Scripts.WM.UI
{
    public class UIManager : MonoBehaviour
    {
        public enum UIMode
        {
            NonVR = 0,
            VR
        }

        //! The list of references to UI controls to be enabled in Non-VR modes.
        public List<GameObject> m_uiControlsNonVR = new List<GameObject>();

        //! The list of references to UI controls to be enabled in VR modes.
        public List<GameObject> m_uiControlsVR = new List<GameObject>();

        //! The list of buttons to toggle UI visible/invisible.
        public List<Button> m_buttonToggleUIArray = new List<Button>();

        // The active UI mode.
        private UIMode m_uiMode = UIMode.NonVR;

        // The 'ui visible' state.
        private bool m_uiVisible = true;

        // Use this for initialization
        void Start()
        {
            foreach (var button in m_buttonToggleUIArray)
            {
                var buttonComponent = button.GetComponent<Button>();
                buttonComponent.onClick.AddListener(ButtonToggleActiveUI_OnClick);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // 'u' key: Toggle UI show/hide.
            if (Input.GetKeyUp("u"))
            {
                ToggleUIVisible();
            }
        }

        private List<GameObject> GetControlsForActiveMode()
        {
            switch (m_uiMode)
            {
                case UIMode.NonVR:
                    return m_uiControlsNonVR;
                case UIMode.VR:
                    return m_uiControlsVR;
                default:
                    return null;
            }
        }

        private List<GameObject> GetControlsForNonActiveMode()
        {
            switch (m_uiMode)
            {
                case UIMode.NonVR:
                    return m_uiControlsVR;
                case UIMode.VR:
                    return m_uiControlsNonVR;
                default:
                    return null;
            }
        }

        public void ButtonToggleActiveUI_OnClick()
        {
            ToggleUIVisible();
        }

        private bool ToggleUIVisible()
        {
            m_uiVisible = !m_uiVisible;

            UpdateUiControlsActiveState();

            return m_uiVisible;
        }

        public void SetUIMode(UIMode uiMode)
        {
            m_uiMode = uiMode;

            UpdateUiControlsActiveState();
        }

        private void UpdateUiControlsActiveState()
        {
            var uiControlsForActiveMode = GetControlsForActiveMode();

            // Set controls for active UI mode to the current 'ui visible' state.
            foreach (var gameObject in uiControlsForActiveMode)
            {
                gameObject.SetActive(m_uiVisible);                
            }

            // HACK: TODO: implement in a cleaner way...
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

            // Set controls for non-active UI mode(s) to 'invisible'.
            var uiControlsForNonActiveMode = GetControlsForNonActiveMode();

            foreach (var gameObject in uiControlsForNonActiveMode)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
 