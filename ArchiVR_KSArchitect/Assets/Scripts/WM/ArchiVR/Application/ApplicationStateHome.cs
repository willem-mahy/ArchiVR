using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

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
