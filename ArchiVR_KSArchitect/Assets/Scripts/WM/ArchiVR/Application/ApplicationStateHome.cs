using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.WM.Settings;
using Assets.Scripts.WM.UI;

namespace Assets.Scripts.WM.ArchiVR.Application
{
    public class ApplicationStateHome : ApplicationState
    {
        public Text m_textStatus = null;

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

                string initialRotationMode = "RotationControlMouse";
                UIManager.UIMode initialUIMode = UIManager.UIMode.NonVR;
                int viewMode = 1;

                if (SystemInfo.supportsGyroscope)
                {
                    initialRotationMode = "RotationControlGyro";
                    initialUIMode = UIManager.UIMode.VR;
                }

                var cameraNavigation = GameObject.Find("CameraNavigation");

                if (null == cameraNavigation)
                {
                    Debug.LogWarning("GameObject 'CameraNavigation' not found in scene!");
                }

                var cameraNavigationComponent = cameraNavigation.GetComponent<CameraNavigation.CameraNavigation>();

                if (null == cameraNavigationComponent)
                {
                    Debug.LogWarning("GameObject 'CameraNavigation' has no 'CameraNavigation' component!");
                }

                cameraNavigationComponent.SetActiveRotationControlModeByName(initialRotationMode);

                UIManager.GetInstance().SetUIMode(initialUIMode);
                SetViewMode(viewMode);
            }            
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

        public void GoButton_OnClick()
        {
            ApplicationState.OpenProject();
        }
    }
}
