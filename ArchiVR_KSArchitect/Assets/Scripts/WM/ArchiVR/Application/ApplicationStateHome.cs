using UnityEngine;

using Assets.Scripts.WM.Settings;

namespace Assets.Scripts.WM.ArchiVR.Application
{
    public class ApplicationStateHome : ApplicationState
    {
        // Used to omit some actions upon re-entry from Play mode (eg Loading the application settings.
        static bool s_firstTime = true;

        override protected string GetName()
        {
            return "Home";
        }

        // Use this for initialization
        override protected void Start()
        {
            Debug.Log("ApplicationStateHome.Start()");
            base.Start();

            if (s_firstTime)
            {
                s_firstTime = false;
                ApplicationSettings.GetInstance().Load();
            }

            SetViewMode(1);
        }

        // Update is called once per frame
        override protected void Update()
        {
            base.Update();

            // If user presses 'Esc', quit the application.
            if (Input.GetKey("escape"))
            {
                QuitApplication();
            }
        }

        public void QuitButton_OnClick()
        {
            Debug.Log("ApplicationStateHome.QuitButton_OnClick()");

            QuitApplication();
        }
    }
}
