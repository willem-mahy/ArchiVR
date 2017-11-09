using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WM.ArchiVR.Application
{
    public class ApplicationStateHome : ApplicationState
    {
        //! The button to open the 'Settings' menu.
        public Button m_settingsButton = null;

        //! The button to open the 'Main' menu.
        public GameObject m_mainMenu = null;

        //! The button to open the 'Settings' menu.
        public GameObject m_settingsMenu = null;

        // Use this for initialization
        override protected void Start()
        {
            Debug.Log("ApplicationStateHome.Start()");
            base.Start();

            ApplicationSettings.GetInstance().Load();           

            m_settingsButton.onClick.AddListener(MenuSettingsButton_OnClick);
        }

        // Update is called once per frame
        override protected void Update()
        {
            base.Update();
        }

        void MenuSettingsButton_OnClick()
        {
            Debug.Log("ApplicationStateHome.MenuSettingsButton_OnClick()");
            m_settingsMenu.SetActive(true);
            m_mainMenu.SetActive(false);
        }
    }
}
