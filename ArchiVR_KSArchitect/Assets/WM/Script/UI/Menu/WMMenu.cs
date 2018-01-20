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
            bool e =
                Input.GetKey(KeyCode.RightControl)
                || Input.GetKey(KeyCode.LeftControl);
            EnableCameraNavigationInputMouseKB(e);
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

            m_firstPersonController.enabled = state;
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
