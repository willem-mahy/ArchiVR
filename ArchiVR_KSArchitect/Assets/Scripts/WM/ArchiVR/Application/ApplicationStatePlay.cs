using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        // List of references to the buttons to open the 'Home' menu.
        public List<Button> m_homeButtons = new List<Button>();

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
            
            // Attach 'OnClick' handler to the buttons to open the 'Home' mode.
            foreach (var button in m_homeButtons)
            {
                if (null == button)
                {
                    continue;
                }

                button.onClick.AddListener(HomeButton_OnClick);
            }
        }

        // Update is called once per frame
        override protected void Update()
        {
            base.Update();
        }

        void HomeButton_OnClick()
        {
            Debug.Log("Home Button clicked.");
            OpenHomeMenu();
        }

        public void OpenHomeMenu()
        {
            if (ApplicationStatePlay.IsActiveViewModeVR())
            {
                SceneManager.LoadScene("MainMenu_VR");
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
