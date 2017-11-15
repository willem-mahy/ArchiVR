using UnityEngine;

using Assets.Scripts.WM.Settings;

namespace Assets.Scripts.WM.ArchiVR.Application
{
    public class ApplicationStateHome : ApplicationState
    {
        override protected string GetName()
        {
            return "Home";
        }

        // Use this for initialization
        override protected void Start()
        {
            Debug.Log("ApplicationStateHome.Start()");
            base.Start();

            ApplicationSettings.GetInstance().Load();                     
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
    }
}
