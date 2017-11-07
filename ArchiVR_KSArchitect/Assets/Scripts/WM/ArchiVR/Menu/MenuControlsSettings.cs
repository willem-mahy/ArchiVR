using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WM.ArchiVR.Menu
{
    public class MenuControlsSettings : MonoBehaviour
    {
        //! The 'Settings' parent menu.
        public GameObject m_menuSettings = null;

        //! The button to close this menu.
        public Button m_exitButton = null;

        //! The 'Rotate Mode' button.
        public Button m_rotateModeButton = null;

        //! The 'Translate Mode' button.
        public Button m_translateModeButton = null;

        // Use this for initialization
        void Start()
        {
            m_exitButton.onClick.AddListener(ExitButton_OnClick);
            m_rotateModeButton.onClick.AddListener(RotateModeButton_OnClick);
            m_translateModeButton.onClick.AddListener(TranslateModeButton_OnClick);
        }

        void ExitButton_OnClick()
        {
            Debug.Log("MenuControlsSettings.ExitButton_OnClick()");
            gameObject.SetActive(false);
            m_menuSettings.SetActive(true);
        }

        void RotateModeButton_OnClick()
        {
            Debug.Log("MenuControlsSettings.RotateModeButton_OnClick()");
        }

        void TranslateModeButton_OnClick()
        {
            Debug.Log("MenuControlsSettings.TranslateModeButton_OnClick()");
        }
    }
}
