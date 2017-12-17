using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.WM.Settings;
using Assets.Scripts.WM.UI;
using UnityEngine.XR;

namespace Assets.Scripts.WM.ArchiVR.Application
{
    public class ApplicationStateHome : ApplicationState
    {
        public Text m_textStatus = null;

        public bool m_initialModeForce = false;
        public UIManager.UIMode m_initialModeVR = UIManager.UIMode.VR;
        public string m_initialViewMode = "split";

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

                InitializeApplication();
            }
        }

        // Update is called once per frame
        override protected void Update()
        {
            base.Update();

            // Hack to make sure the camera ends up in the correct location in Application Home state.
            Camera.main.transform.position = new Vector3(0.0f, 1.6f, 0.0f);

            // If user presses 'Esc', quit the application.
            if (Input.GetKey("escape"))
            {
                QuitApplication();
            }
        }

        // OnGUI is called once per frame
        void UpdateDebugText()
        {
            var mainCamera = Camera.main;

            if (!mainCamera)
                return;

            var textDebugVR = GameObject.Find("Text_Debug_VR");

            if (!textDebugVR)
                return;

            var textDebugVRComponent = textDebugVR.GetComponent<Text>();

            if (!textDebugVRComponent)
                return;

            var cameraText =
                "Camera\n" +
                "Pos:" + mainCamera.transform.position.ToString() + "\n" +
                "Fwd:" + mainCamera.transform.forward.ToString() + "\n" +
                "Up:" + mainCamera.transform.up.ToString() + "\n";

            var text =
                "XRDevice.isPresent= " + (XRDevice.isPresent ? "true" : "false") + "\n" +
                "XRDevice.model= " + XRDevice.model + "\n" +
                "XRSettings.loadedDeviceName= " + XRSettings.loadedDeviceName + "\n" +
                cameraText;

            textDebugVRComponent.text = text;
        }

        private void InitializeApplication()
        {
            //ApplicationSettings.GetInstance().Load();

            if (m_initialModeForce)
            {
                UIManager.GetInstance().SetUIMode(m_initialModeVR);
                SetViewMode(m_initialViewMode);
            }
            else
            {
                if (XRDevice.isPresent)
                {
                    // When running on a Mixed Reality device, UI mode is always VR.                
                    UIManager.UIMode initialUIMode = UIManager.UIMode.VR;
                    UIManager.GetInstance().SetUIMode(initialUIMode);

                    if (XRSettings.loadedDeviceName == "oculus")
                    {
                        // ViewMode is covered automatically by GearVR initialization.
                        // Rotation is covered automatically by GearVR rotational tracking, so no navigation mode necessary.
                    }
                    else if (XRSettings.loadedDeviceName == "microsoft XR")
                    {
                        // ViewMode is covered automatically by Microsoft Mixed Reality initialization.
                        // Rotation is covered automatically by Microsoft Mixed Reality rotational tracking, so no navigation mode necessary.
                    }
                }
                else
                {
                    string initialRotationMode = "RotationControlMouse";
                    UIManager.UIMode initialUIMode = UIManager.UIMode.NonVR;

                    string initialViewMode =
                        null;
                    //"split";

                    // If executing system has gyroscope support, startup with:
                    if (SystemInfo.supportsGyroscope)
                    {
                        // ... GYRO rotation active
                        initialRotationMode = "RotationControlGyro";

                        // ... and  VR mode On.
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

                    if (null != initialViewMode)
                    {
                        SetViewMode(initialViewMode);
                    }
                }
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
