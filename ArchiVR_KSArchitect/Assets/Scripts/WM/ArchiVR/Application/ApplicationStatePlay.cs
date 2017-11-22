using Assets.Scripts.WM.UI;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Assets.Scripts.WM.CameraNavigation.TranslationControl;

namespace Assets.Scripts.WM.ArchiVR.Application
{
    public class ApplicationStatePlay : ApplicationState
    {
        // For debugging purposes: allows to start up in 'Play' state,
        // with the project designated in 'm_initialProjectSceneName'.
        private static bool s_firstTime = true;

        // For debugging purposes: allows to start up in 'Play' state,
        // with the project designated in 'm_initialProjectSceneName'.
        public string m_initialProjectSceneName = "";

        override protected string GetName()
        {
            return "Play";
        }

        // Use this for initialization
        override protected void Start()
        {
            base.Start();

            // For debugging purposes: enables to open an initial project (defined by 'm_initialProjectSceneName') when starting the application in 'Play' mode.
            if (s_firstTime)
            {
                s_firstTime = false;

                if (m_initialProjectSceneName.Length > 0)
                {
                    OpenProject(m_initialProjectSceneName);
                }
            }
        }

        // Update is called once per frame
        override protected void Update()
        {
            base.Update();
        }

        public void HomeButton_OnClick()
        {
            Debug.Log("ApplicationStatePlay.HomeButton_OnClick()");
            OpenHomeMenu();
        }

        public void MenuTimeButton_OnClick()
        {
            Debug.Log("ApplicationStatePlay.MenuTimeButton_OnClick()");

            //UIManager.GetInstance().OpenMenu("MenuTime");
            UIManager.GetInstance().OpenMenu(GameObject.Find("MenuTime").GetComponent<Assets.Scripts.WM.UI.Menu>());
        }

        public void OpenHomeMenu()
        {
            SceneManager.LoadScene("Home");
        }        
    }
}
